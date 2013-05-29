using System;
using System.Collections.Generic;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class identityDictionaryWhereBase<valueType, memberType> : identityMemberMap<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private func<valueType, bool> isValue;
        /// <summary>
        /// 数据缓存集合
        /// </summary>
        private Dictionary<int, valueType> values;
        /// <summary>
        /// 数据集合,请使用GetArray()
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                return GetArray();
            }
        }
        /// <summary>
        /// 自增ID版本号
        /// </summary>
        protected int identityVersion;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        public identityDictionaryWhereBase(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap
            , func<valueType, int> getIdentity, func<valueType, bool> isValue)
            : base(sqlTool, memberMap, getIdentity)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            this.isValue = isValue;

            sqlTool.OnInserted += onInserted;
            sqlTool.OnUpdated += onUpdated;
            sqlTool.OnDeleted += onDeleted;

            reset();
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            valueType[] values = SqlTool.Where(null, memberMap).getFindArray(isValue);
            Dictionary<int, valueType> newValues = new Dictionary<int, valueType>(values.Length);
            foreach (valueType value in values) newValues.Add(getIdentity(value), value);
            this.values = newValues;
            ++identityVersion;
            Count = values.Length;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            if (isValue(value)) add(value);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void add(valueType value)
        {
            valueType newValue = fastCSharp.setup.constructor<valueType>.New;
            copyFrom(newValue, value, memberMap);
            values.Add(getIdentity(value), newValue);
            ++identityVersion;
            ++Count;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, memberType memberMap)
        {
            if (isValue(value))
            {
                valueType cacheValue;
                if (values.TryGetValue(getIdentity(value), out cacheValue))
                {
                    update(cacheValue, value, oldValue, updateMemberMap(memberMap));
                    callOnUpdated(cacheValue, oldValue);
                }
                else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            if (isValue(value))
            {
                valueType cacheValue;
                int identity = getIdentity(value);
                if (values.TryGetValue(identity, out cacheValue))
                {
                    values.Remove(identity);
                    --Count;
                    callOnDeleted(cacheValue);
                }
                else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id">数据自增ID</param>
        /// <param name="errorValue">错误返回值</param>
        /// <returns>数据</returns>
        public valueType Get(int id, valueType errorValue)
        {
            valueType value;
            int version = identityVersion;
            if (values.TryGetValue(id, out value) && getIdentity(value) == id) return value;
            else if (version != identityVersion)
            {
                Monitor.Enter(SqlTool.Lock);
                try
                {
                    if (values.TryGetValue(id, out value)) return value;
                }
                finally { Monitor.Exit(SqlTool.Lock); }
            }
            return errorValue;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>数据集合</returns>
        public valueType[] GetArray()
        {
            Monitor.Enter(SqlTool.Lock);
            try
            {
                return values.Values.getArray();
            }
            finally { Monitor.Exit(SqlTool.Lock); }
        }
    }
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class identityDictionaryWhere<valueType, memberType> : identityDictionaryWhereBase<valueType, memberType>
        where valueType : class, sqlTable.ISqlIdentity<valueType, memberType, int>
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityDictionaryWhere(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , func<valueType, bool> isValue, memberType memberMap = default(memberType))
            : base(sqlTool, memberMap, value => value.SqlIdentity32, isValue)
        {
        }
    }
    /// <summary>
    /// 自增ID整表缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class identityDictionaryWhere<valueType> : identityDictionaryWhereBase<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityDictionaryWhere(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, int> getIdentity
            , func<valueType, bool> isValue, memberMap<valueType> memberMap = default(memberMap<valueType>))
            : base(sqlTool, memberMap, getIdentity, isValue)
        {
        }
    }
}
