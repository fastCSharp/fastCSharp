using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class dictionary<valueType, memberType, keyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected eventCache<valueType, memberType> cache;
        /// <summary>
        /// 分组字典关键字获取器
        /// </summary>
        protected func<valueType, keyType> getKey;
        /// <summary>
        /// 字典缓存
        /// </summary>
        protected Dictionary<keyType, valueType> values;
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据,失败返回null</returns>
        public valueType this[keyType key]
        {
            get
            {
                valueType value;
                return values.TryGetValue(key, out value) ? value : null;
            }
        }
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        public dictionary(eventCache<valueType, memberType> cache, func<valueType, keyType> getKey)
        {
            if (cache == null || getKey == null) log.Default.Throw(log.exceptionType.Null);
            this.cache = cache;
            this.getKey = getKey;

            cache.OnInserted += onInserted;
            cache.OnUpdated += onUpdated;
            cache.OnDeleted += onDeleted;

            reset();
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            Dictionary<keyType, valueType> newValues = new Dictionary<keyType, valueType>();
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                foreach (valueType value in cache.Values) newValues.Add(getKey(value), value);
                values = newValues;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        protected void onInserted(valueType value)
        {
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key">数据对象的关键字</param>
        protected void onInserted(valueType value, keyType key)
        {
            values.Add(key, value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        protected void onUpdated(valueType value, valueType oldValue)
        {
            keyType key = getKey(value), oldKey = getKey(oldValue);
            if (!key.Equals(oldKey))
            {
                onInserted(value, key);
                onDeleted(oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">被删除数据的关键字</param>
        protected void onDeleted(keyType key)
        {
            if (!values.Remove(key))
            {
                log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            onDeleted(getKey(value));
        }
    }
     /// <summary>
    /// 字典缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class dictionary<valueType, keyType> : dictionary<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        public dictionary(eventCache<valueType, memberMap<valueType>> cache, func<valueType, keyType> getKey)
            : base(cache, getKey)
        {
        }
    }
}
