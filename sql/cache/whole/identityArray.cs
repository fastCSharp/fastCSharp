using System;
using System.Collections.Generic;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class identityArrayBase<valueType, memberType> : identityCache<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 数组长度递增
        /// </summary>
        protected int addLength;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isReset">是否初始化事件与数据</param>
        /// <param name="addLength">数组长度递增</param>
        public identityArrayBase(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, int> getIdentity
            , bool isReset, int addLength)
            : base(sqlTool, memberMap, getIdentity, isReset)
        {
            this.addLength = addLength == 0 ? (1 << 16) : (addLength < 1024 ? 1024 : addLength);

            if (isReset)
            {
                sqlTool.OnInsertedLock += onInserted;
                //sqlTool.OnDeletedByIdentityLock += onDelete;
                sqlTool.OnDeletedLock += onDeleted;

                reset();
            }
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="values">数据集合</param>
        protected void reset(valueType[] values)
        {
            valueType[] newValues = new valueType[(values.maxKey(value => getIdentity(value), 0) / addLength + 1) * addLength];
            foreach (valueType value in values) newValues[getIdentity(value)] = value;
            this.values = newValues;
            Count = values.Length;
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            reset(SqlTool.Where(null, memberMap).getArray());
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        protected void onInserted(valueType value)
        {
            int identity = getIdentity(value);
            if (identity >= values.Length)
            {
                int newLength = values.Length + addLength;
                while (newLength <= identity) newLength += addLength;
                valueType[] newValues = new valueType[newLength];
                values.CopyTo(newValues, 0);
                values = newValues;
            }
            valueType newValue = fastCSharp.setup.constructor<valueType>.New;
            copyFrom(newValue, value, memberMap);
            values[identity] = newValue;
            ++Count;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            int identity = getIdentity(value);
            valueType cacheValue = values[identity];
            --Count;
            values[identity] = null;
            callOnDeleted(cacheValue);
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class identityArray<valueType, memberType> : identityArrayBase<valueType, memberType>
        where valueType : class, sqlTable.ISqlIdentity<valueType, memberType, int>
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="isReset">是否初始化事件与数据</param>
        public identityArray(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , memberType memberMap = default(memberType), int addLength = 0, bool isReset = true)
            : base(sqlTool, memberMap, value => value.SqlIdentity32, isReset, addLength)
        {
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class identityArray<valueType> : identityArrayBase<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="isReset">是否初始化事件与数据</param>
        public identityArray(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, int> getIdentity
            , memberMap<valueType> memberMap = default(memberMap<valueType>), int addLength = 0, bool isReset = true)
            : base(sqlTool, memberMap, getIdentity, isReset, addLength)
        {
        }
    }
}
