using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 先进先出优先队列自定义缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">缓存数据类型</typeparam>
    public class queueExpressionCustom<valueType, memberType, counterKeyType, keyType, cacheValueType>
        : queueExpression<valueType, memberType, counterKeyType, keyType, cacheValueType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where counterKeyType : IEquatable<counterKeyType>
        where cacheValueType : class, ICustom<valueType>
    {
        /// <summary>
        /// 自定义缓存获取器
        /// </summary>
        private func<IEnumerable<valueType>, cacheValueType> getValue;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public cacheValueType this[keyType key]
        {
            get
            {
                Monitor.Enter(counter.SqlTool.Lock);
                try
                {
                    cacheValueType values = queueCache.Get(key, null);
                    if (values != null) return values;
                    queueCache[key] = values = getValue(counter.SqlTool.Where(getWhere(key), counter.MemberMap).getArray(value => counter.Add(value)));
                    if (queueCache.Count > maxCount)
                    {
                        foreach (valueType value in queueCache.Pop().Value.Values) counter.Remove(value);
                    }
                    return values;
                }
                finally { Monitor.Exit(counter.SqlTool.Lock); }
            }
        }
        /// <summary>
        /// 先进先出优先队列自定义缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="where">条件表达式</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueExpressionCustom(counter<valueType, memberType, counterKeyType> counter, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, func<IEnumerable<valueType>, cacheValueType> getValue, int maxCount = 0)
            : base(counter, getKey, maxCount, getWhere)
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
            keyType key = getKey(value);
            cacheValueType values = queueCache.Get(key, null);
            if (values != null) values.Add(counter.Add(value));
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
                cacheValueType values;
                if (queueCache.Remove(key, out values))
                {
                    foreach (valueType removeValue in values.Values) counter.Remove(removeValue);
                }
            }
            else
            {
                keyType oldKey = getKey(oldValue);
                if (!key.Equals(oldKey))
                {
                    cacheValueType values = queueCache.Get(key, null), oldValues = queueCache.Get(oldKey, null);
                    if (values != null)
                    {
                        if (oldValues != null)
                        {
                            values.Add(cacheValue);
                            if (!oldValues.Remove(cacheValue)) log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
                        }
                        else values.Add(counter.Add(cacheValue));
                    }
                    else if (oldValues != null)
                    {
                        if (oldValues.Remove(cacheValue)) counter.Remove(cacheValue);
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
            cacheValueType values = queueCache.Get(getKey(value), null);
            if (values != null && !values.Remove(value)) log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
    }
    /// <summary>
    /// 先进先出优先队列自定义缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">缓存数据类型</typeparam>
    public class queueExpressionCustom<valueType, counterKeyType, keyType, cacheValueType>
        : queueExpressionCustom<valueType, memberMap<valueType>, counterKeyType, keyType, cacheValueType>
        where valueType : class
        where counterKeyType : IEquatable<counterKeyType>
        where cacheValueType : class, ICustom<valueType>
    {
        /// <summary>
        /// 先进先出优先队列自定义缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="where">条件表达式</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueExpressionCustom(counter<valueType, memberMap<valueType>, counterKeyType> counter, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, func<IEnumerable<valueType>, cacheValueType> getValue, int maxCount = 0)
            : base(counter, getKey, getWhere, getValue, maxCount)
        {
        }
    }
}
