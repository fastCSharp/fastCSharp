using System;
using fastCSharp.setup.cSharp;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 先进先出优先队列 字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="dictionaryKeyType">字典关键字类型</typeparam>
    public class queueDictionary<valueType, memberType, counterKeyType, keyType, dictionaryKeyType>
        : queueExpression<valueType, memberType, counterKeyType, keyType, Dictionary<dictionaryKeyType, valueType>>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where counterKeyType : IEquatable<counterKeyType>
        where keyType : IEquatable<keyType>
        where dictionaryKeyType : IEquatable<dictionaryKeyType>
    {
        /// <summary>
        /// 缓存字典关键字获取器
        /// </summary>
        protected func<valueType, dictionaryKeyType> getDictionaryKey;
        /// <summary>
        /// 先进先出优先队列 字典缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="getDictionaryKey">缓存字典关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueDictionary(counter<valueType, memberType, counterKeyType> counter
            , func<valueType, keyType> getKey, func<keyType, Expression<func<valueType, bool>>> getWhere
            , func<valueType, dictionaryKeyType> getDictionaryKey
            , int maxCount = 0)
            : base(counter, getKey, maxCount, getWhere)
        {
            if (getDictionaryKey == null) log.Default.Throw(log.exceptionType.Null);
            this.getDictionaryKey = getDictionaryKey;

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
            keyType key = getKey(value);
            Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(key, null);
            if (values != null) values.Add(getDictionaryKey(value), counter.Add(value));
        }     
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        private void onUpdated(valueType cacheValue, valueType value, valueType oldValue)
        {
            keyType key = getKey(value);
            if (cacheValue == null)
            {
                Dictionary<dictionaryKeyType, valueType> values;
                if (queueCache.Remove(key, out values))
                {
                    foreach (valueType removeValue in values.Values) counter.Remove(removeValue);
                }
            }
            else
            {
                keyType oldKey = getKey(oldValue);
                if (key.Equals(oldKey))
                {
                    Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(key, null);
                    if (values != null)
                    {
                        dictionaryKeyType dictionaryKey = getDictionaryKey(cacheValue), oldDictionaryKey = getDictionaryKey(oldValue);
                        if (!dictionaryKey.Equals(oldDictionaryKey))
                        {
                            values.Add(dictionaryKey, cacheValue);
                            values.Remove(oldDictionaryKey);
                        }
                    }
                }
                else
                {
                    Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(key, null);
                    Dictionary<dictionaryKeyType, valueType> oldValues = queueCache.Get(oldKey, null);
                    if (values != null)
                    {
                            if (oldValues != null)
                            {
                                values.Add(getDictionaryKey(cacheValue), cacheValue);
                                if (!oldValues.Remove(getDictionaryKey(oldValue)))
                                {
                                    log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
                                }
                            }
                            else values.Add(getDictionaryKey(cacheValue), counter.Add(cacheValue));
                    }
                    else if (oldValues != null)
                    {
                        if (oldValues.Remove(getDictionaryKey(value))) counter.Remove(cacheValue);
                        else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
                    }
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(getKey(value), null);
            if (values != null && !values.Remove(getDictionaryKey(value)))
            {
                log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
            }
        }
        /// <summary>
        /// 判断是否存在关键字匹配的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <param name="isKey">是否存在关键字</param>
        /// <returns>是否存在关键字匹配的缓存</returns>
        public bool TryIsKey(keyType key, dictionaryKeyType dictionaryKey, out bool isKey)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(key, null);
                isKey = values != null;
                if (isKey) return values.ContainsKey(dictionaryKey);
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
            return false;
        }
        /// <summary>
        /// 获取字典缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>字典缓存</returns>
        private Dictionary<dictionaryKeyType, valueType> getDictionary(keyType key)
        {
            Dictionary<dictionaryKeyType, valueType> values = queueCache.Get(key, null);
            if (values == null)
            {
                values = new Dictionary<dictionaryKeyType, valueType>();
                foreach (valueType value in counter.SqlTool.Where(getWhere(key), counter.MemberMap))
                {
                    values.Add(getDictionaryKey(value), counter.Add(value));
                }
                queueCache[key] = values;
            }
            return values;
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <param name="errorValue">错误返回值</param>
        /// <returns>匹配数据</returns>
        public valueType Get(keyType key, dictionaryKeyType dictionaryKey, valueType errorValue = null)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                if(values != null)
                {
                    valueType value;
                    if (values.TryGetValue(dictionaryKey, out value)) return value;
                }
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
            return errorValue;
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <returns>关键字是否存在</returns>
        public bool IsKey(keyType key, dictionaryKeyType dictionaryKey)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                return values != null && values.ContainsKey(dictionaryKey);
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
        }
        /// <summary>
        /// 获取缓存数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据数量</returns>
        public int GetCount(keyType key)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                if (values != null) return values.Count;
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
            return 0;
        }
        /// <summary>
        /// 获取缓存数组
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数组</returns>
        public valueType[] GetArray(keyType key)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                return values != null ? values.Values.getArray() : nullValue<valueType>.Array;
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
        }
        /// <summary>
        /// 获取缓存关键字数组
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>关键字数组</returns>
        public dictionaryKeyType[] GetKeyArray(keyType key)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                return values != null ? values.Keys.getArray() : nullValue<dictionaryKeyType>.Array;
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
        }
        /// <summary>
        /// 获取匹配缓存数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配缓存数据集合</returns>
        public list<valueType> GetFindLock(keyType key, func<valueType, bool> isValue)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                if (values != null) return values.Values.getFind(isValue);
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
            return null;
        }
        /// <summary>
        /// 获取一个匹配数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据,失败返回null</returns>
        public valueType FirstOrDefaultLock(keyType key, func<valueType, bool> isValue)
        {
            Monitor.Enter(counter.SqlTool.Lock);
            try
            {
                Dictionary<dictionaryKeyType, valueType> values = getDictionary(key);
                if (values != null) return values.Values.firstOrDefault(isValue);
            }
            finally { Monitor.Exit(counter.SqlTool.Lock); }
            return null;
        }
    }
    /// <summary>
    /// 先进先出优先队列 字典缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="dictionaryKeyType">字典关键字类型</typeparam>
    public class queueDictionary<valueType, counterKeyType, keyType, dictionaryKeyType>
        : queueDictionary<valueType, memberMap<valueType>, counterKeyType, keyType, dictionaryKeyType>
        where valueType : class
        where counterKeyType : IEquatable<counterKeyType>
        where keyType : IEquatable<keyType>
        where dictionaryKeyType : IEquatable<dictionaryKeyType>
    {
        /// <summary>
        /// 先进先出优先队列 字典缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="getDictionaryKey">缓存字典关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueDictionary(counter<valueType, memberMap<valueType>, counterKeyType> counter
            , func<valueType, keyType> getKey, func<keyType, Expression<func<valueType, bool>>> getWhere
            , func<valueType, dictionaryKeyType> getDictionaryKey
            , int maxCount = 0)
            : base(counter, getKey, getWhere, getDictionaryKey, maxCount)
        {
        }
    }
}