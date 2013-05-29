using System;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表排序树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract unsafe class identityTreeBase<valueType, memberType> : identityCache<valueType, memberType>, IDisposable
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 排序树节点数量集合
        /// </summary>
        private int* counts;
        /// <summary>
        /// 排序树容器数量
        /// </summary>
        private int size;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityTreeBase
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, int> getIdentity)
            : base(sqlTool, memberMap, getIdentity, true)
        {
            sqlTool.OnInsertedLock += onInserted;
            //sqlTool.OnDeletedByIdentityLock += onDelete;
            sqlTool.OnDeletedLock += onDelete;

            reset();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            unmanaged.Free(counts);
            counts = null;
            size = 0;
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            valueType[] values = SqlTool.Where(null, memberMap).getArray();
            int maxIdentity = values.maxKey(value => getIdentity(value), 0);
            int length = maxIdentity >= 1024 ? 1 << ((uint)maxIdentity).bits() : 1024;
            valueType[] newValues = new valueType[length];
            int* newCounts = unmanaged.Get(length * sizeof(int)).Int;
            bool isFree = false;
            try
            {
                foreach (valueType value in values)
                {
                    int identity = getIdentity(value);
                    newValues[identity] = value;
                    newCounts[identity] = 1;
                }
                for (int step = 2; step != length; step <<= 1)
                {
                    for (int index = step, countStep = step >> 1; index != length; index += step)
                    {
                        newCounts[index] += newCounts[index - countStep];
                    }
                }
                unmanaged.Free(counts);
                isFree = true;
                this.values = newValues;
                counts = newCounts;
                size = length;
                Count = values.Length;
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, true);
            }
            finally
            {
                if (!isFree) unmanaged.Free(newCounts);
            }
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            int identity = getIdentity(value);
            if (identity >= values.Length)
            {
                int newLength = int.MaxValue, oldLength = values.Length;
                if ((identity & 0x40000000) == 0 && oldLength != 0x40000000)
                {
                    for (newLength = oldLength << 1; newLength <= identity; newLength <<= 1) ;
                }
                valueType[] newValues = new valueType[newLength];
                values.CopyTo(newValues, 0);
                bool isFree = false;
                int* newCounts = unmanaged.Get(newLength * sizeof(int)).Int;
                try
                {
                    unsafer.memory.Copy(counts, newCounts, newLength * sizeof(int));
                    unmanaged.Free(counts);
                    isFree = true;
                    values = newValues;
                    counts = newCounts;
                    size = newLength;

                    int index = oldLength, count = counts[--index];
                    for (int step = 1; (index -= step) != 0; step <<= 1) count += counts[index];
                    counts[oldLength] = count;
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, true);
                }
                finally
                {
                    if (!isFree) unmanaged.Free(newCounts);
                }
            }
            valueType newValue = fastCSharp.setup.constructor<valueType>.New;
            copyFrom(newValue, value, memberMap);
            values[identity] = newValue;
            for (uint index = (uint)identity, countStep = 1, length = (uint)size; index <= length; countStep <<= 1)
            {
                ++counts[index];
                while ((index & countStep) == 0) countStep <<= 1;
                index += countStep;
            }
            ++Count;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDelete(valueType value)
        {
            int identity = getIdentity(value);
            valueType cacheValue = values[identity];
            --Count;
            for (uint index = (uint)identity, countStep = 1, length = (uint)size; index <= length; countStep <<= 1)
            {
                --counts[index];
                while ((index & countStep) == 0) countStep <<= 1;
                index += countStep;
            }
            values[identity] = null;
            callOnDeleted(cacheValue);
        }
        /// <summary>
        /// 获取记录起始位置
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <returns>起始位置</returns>
        private int getIndex(int skipCount)
        {
            int index = values.Length != int.MaxValue ? values.Length >> 1 : 0x40000000, step = index;
            while (counts[index] != skipCount)
            {
                step >>= 1;
                if (counts[index] < skipCount)
                {
                    skipCount -= counts[index];
                    index += step;
                }
                else index -= step;
            }
            return index + 1;
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">记录总数</param>
        /// <returns>分页记录集合</returns>
        public valueType[] GetPageDesc(int pageSize, int currentPage, out int count)
        {
            Monitor.Enter(SqlTool.Lock);
            try
            {
                array.page page = new array.page(count = Count, pageSize, currentPage);
                valueType[] values = new valueType[page.CurrentPageSize];
                for (int writeIndex = values.Length, index = getIndex(Count - page.SkipCount - page.CurrentPageSize); writeIndex != 0; values[--writeIndex] = this.values[index++])
                {
                    while (this.values[index] == null) ++index;
                }
                return values;
            }
            finally { Monitor.Exit(SqlTool.Lock); }
        }
    }
    /// <summary>
    /// 自增ID整表排序树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class identityTree<valueType, memberType> : identityTreeBase<valueType, memberType>
        where valueType : class, sqlTable.ISqlIdentity<valueType, memberType, int>
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityTree
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap = default(memberType))
            : base(sqlTool, memberMap, value => value.SqlIdentity32)
        {
        }
    }
    /// <summary>
    /// 自增ID整表排序树缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class identityTree<valueType> : identityTreeBase<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityTree(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, int> getIdentity
            , memberMap<valueType> memberMap = default(memberMap<valueType>))
            : base(sqlTool, memberMap, getIdentity)
        {
        }
    }
}
