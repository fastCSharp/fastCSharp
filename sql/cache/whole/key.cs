using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class key<valueType, memberType, keyType> : eventCache<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 键值获取器
        /// </summary>
        private func<valueType, keyType> getKey;
        /// <summary>
        /// 字典缓存数据
        /// </summary>
        protected Dictionary<keyType, valueType> dictionary;
        /// <summary>
        /// 数据集合,请使用GetArray
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                return GetArray();
            }
        }
        /// <summary>
        /// 关键字版本
        /// </summary>
        protected int keyVersion;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据</returns>
        public valueType this[keyType key]
        {
            get
            {
                valueType value;
                int version = keyVersion;
                if (dictionary.TryGetValue(key, out value) && getKey(value).Equals(key)) return value;
                else if (version != keyVersion)
                {
                    Monitor.Enter(SqlTool.Lock);
                    try
                    {
                        if (dictionary.TryGetValue(key, out value)) return value;
                    }
                    finally { Monitor.Exit(SqlTool.Lock); }
                }
                return null;
            }
        }
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        public key
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, func<valueType, keyType> getKey, memberType memberMap = default(memberType))
            : base(sqlTool, memberMap)
        {
            if (getKey == null) log.Default.Throw(log.exceptionType.Null);
            this.getKey = getKey;

            sqlTool.OnInsertedLock += onInserted;
            sqlTool.OnUpdatedLock += onUpdated;
            sqlTool.OnDeletedLock += onDeleted;

            reset();
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        protected void reset()
        {
            dictionary = SqlTool.Where(null, memberMap).getDictionary(getKey);
            ++keyVersion;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        protected void onInserted(valueType value)
        {
            valueType newValue = fastCSharp.setup.constructor<valueType>.New;
            copyFrom(newValue, value, memberMap);
            dictionary.Add(getKey(value), newValue);
            ++keyVersion;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        protected void onUpdated(valueType value, valueType oldValue, memberType memberMap)
        {
            valueType cacheValue;
            if (dictionary.TryGetValue(getKey(value), out cacheValue))
            {
                update(cacheValue, value, oldValue, updateMemberMap(memberMap));
                callOnUpdated(cacheValue, oldValue);
            }
            else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            valueType cacheValue;
            if (dictionary.TryGetValue(getKey(value), out cacheValue))
            {
                dictionary.Remove(getKey(value));
                callOnDeleted(cacheValue);
            }
            else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
        /// <summary>
        /// 获取数据数组集合
        /// </summary>
        /// <returns>数据数组集合</returns>
        public valueType[] GetArray()
        {
            Monitor.Enter(SqlTool.Lock);
            try
            {
                return dictionary.Values.getArray();
            }
            finally { Monitor.Exit(SqlTool.Lock); }
        }
    }

}
