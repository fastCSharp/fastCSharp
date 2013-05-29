using System;
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
    public abstract class queueExpression<valueType, memberType, counterKeyType, keyType, cacheValueType>
        : queue<valueType, memberType, counterKeyType, keyType, cacheValueType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where counterKeyType : IEquatable<counterKeyType>
        where cacheValueType : class
    {
        /// <summary>
        /// 条件表达式获取器
        /// </summary>
        protected func<keyType, Expression<func<valueType, bool>>> getWhere;
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        /// <param name="where">条件表达式</param>
        protected queueExpression(counter<valueType, memberType, counterKeyType> counter, func<valueType, keyType> getKey, int maxCount
            , func<keyType, Expression<func<valueType, bool>>> getWhere)
            : base(counter, getKey, maxCount)
        {
            if (getWhere == null) log.Default.Throw(log.exceptionType.Null);
            this.getWhere = getWhere;
        }
    }
}
