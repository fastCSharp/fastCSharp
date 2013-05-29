using System;
using System.Threading;
using System.Linq.Expressions;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">缓存数据类型</typeparam>
    public abstract class queue<valueType, memberType, counterKeyType, keyType, cacheValueType>
        : counterCache<valueType, memberType, counterKeyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where counterKeyType : IEquatable<counterKeyType>
        where cacheValueType : class
    {
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        protected func<valueType, keyType> getKey;
        /// <summary>
        /// 缓存默认最大容器大小
        /// </summary>
        protected int maxCount;
        /// <summary>
        /// 数据集合
        /// </summary>
        protected fifoPriorityQueue<keyType, cacheValueType> queueCache = new fifoPriorityQueue<keyType, cacheValueType>();
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected queue(counter<valueType, memberType, counterKeyType> counter, func<valueType, keyType> getKey, int maxCount)
            : base(counter)
        {
            if (getKey == null) log.Default.Throw(log.exceptionType.Null);
            this.getKey = getKey;
            this.maxCount = maxCount <= 0 ? config.sql.Default.CacheMaxCount : maxCount;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据,失败返回null</returns>
        public cacheValueType TryGet(keyType key)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                return queueCache.Get(key, null);
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
        }
    }
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class queue<valueType, memberType, keyType>
        : queue<valueType, memberType, keyType, keyType, valueType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数据获取器
        /// </summary>
        protected func<keyType, memberType, valueType> getValue;
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public valueType this[keyType key]
        {
            get
            {
                Monitor.Enter(counter.SqlTool.Lock);
                try
                {
                    valueType value = queueCache.Get(key, null);
                    if (value != null) return value;
                    if (getKey == counter.GetKey)
                    {
                        value = counter.Get(key);
                        if (value != null) return value;
                    }
                    if ((value = getValue(key ,counter.MemberMap)) != null) onInserted(value);
                    return value;
                }
                finally { Monitor.Exit(counter.SqlTool.Lock); }
            }
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getValue">数据获取器,禁止锁操作</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queue(counter<valueType, memberType, keyType> counter
            , func<keyType, memberType, valueType> getValue, int maxCount = 0)
            : base(counter, counter.GetKey, maxCount)
        {
            if (getValue == null) log.Default.Throw(log.exceptionType.Null);
            this.getValue = getValue;

            counter.SqlTool.OnInsertedLock += onInserted;
            counter.OnUpdated += onUpdated;
            counter.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        /// <param name="key">关键字</param>
        private void onInserted(valueType value, keyType key)
        {
            queueCache[key] = counter.Add(value);
            if (queueCache.Count > maxCount) counter.Remove(queueCache.Pop().Value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        private void onUpdated(valueType cacheValue, valueType value, valueType oldValue)
        {
            if (cacheValue != null)
            {
                keyType key = getKey(value), oldKey = getKey(oldValue);
                if (!key.Equals(oldKey))
                {
                    valueType removeValue;
                    if (queueCache.Remove(oldKey, out removeValue)) queueCache.Set(key, cacheValue);
                    else onInserted(cacheValue, key);
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            queueCache.Remove(getKey(value), out value);
        }
    }
    /// <summary>
    /// 先进先出优先队列缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class queue<valueType, keyType> : queue<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {

        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getValue">数据获取器,禁止锁操作</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queue(counter<valueType, memberMap<valueType>, keyType> counter
            , func<keyType, memberMap<valueType>, valueType> getValue, int maxCount = 0)
            : base(counter, getValue, maxCount)
        {
        }
    }
}
