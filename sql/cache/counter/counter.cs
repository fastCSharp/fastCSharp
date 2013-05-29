using System;
using System.Collections.Generic;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class counter<valueType, memberType, keyType> : copy<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        internal func<valueType, keyType> GetKey { get; private set; }
        /// <summary>
        /// 缓存数据
        /// </summary>
        private Dictionary<keyType, keyValue<valueType, int>> values;
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存值</returns>
        public valueType this[keyType key]
        {
            get
            {
                return Get(key);
            }
        }
        /// <summary>
        /// 缓存计数
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        protected counter(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, keyType> getKey)
            : base(sqlTool, memberMap)
        {
            if (getKey == null) log.Default.Throw(log.exceptionType.Null);
            GetKey = getKey;
            values = new Dictionary<keyType, keyValue<valueType, int>>();

            //sqlTool.OnUpdatedByIdentityLock += onUpdated;
            sqlTool.OnUpdatedLock += onUpdated;
            sqlTool.OnDeletedLock += onDeleted;
        }
        /// <summary>
        /// 更新记录事件
        /// </summary>
        public event action<valueType, valueType, valueType> OnUpdated;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, memberType memberMap)
        {
            keyValue<valueType, int> cacheValue;
            keyType key = GetKey(value);
            if (values.TryGetValue(key, out cacheValue)) update(cacheValue.Key, value, memberMap);
            if (OnUpdated != null) OnUpdated(cacheValue.Key, value, oldValue);
        }
        /// <summary>
        /// 删除记录事件
        /// </summary>
        public event action<valueType> OnDeleted;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            keyValue<valueType, int> cacheValue;
            keyType key = GetKey(value);
            if (values.TryGetValue(key, out cacheValue))
            {
                values.Remove(GetKey(value));
                if (OnDeleted != null) OnDeleted(cacheValue.Key);
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        internal valueType Get(keyType key)
        {
            keyValue<valueType, int> valueCount;
            return values.TryGetValue(key, out valueCount) ? valueCount.Key : null;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="value">查询数据</param>
        /// <returns>缓存数据</returns>
        internal valueType Get(valueType value)
        {
            keyValue<valueType, int> valueCount;
            return values.TryGetValue(GetKey(value), out valueCount) ? valueCount.Key : null;
        }
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>缓存数据</returns>
        internal valueType Add(valueType value)
        {
            keyValue<valueType, int> valueCount;
            keyType key = GetKey(value);
            if (values.TryGetValue(key, out valueCount))
            {
                ++valueCount.Value;
                values[key] = valueCount;
                return valueCount.Key;
            }
            valueType copyValue = fastCSharp.setup.constructor<valueType>.New;
            copyFrom(copyValue, value, memberMap);
            values.Add(key, new keyValue<valueType, int>(copyValue, 0));
            return copyValue;
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        internal void Remove(valueType value)
        {
            keyValue<valueType, int> valueCount;
            keyType key = GetKey(value);
            if (values.TryGetValue(key, out valueCount))
            {
                if (valueCount.Value == 0) values.Remove(key);
                else
                {
                    --valueCount.Value;
                    values[key] = valueCount;
                }
            }
            else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
    }
}
