using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 计数缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class counterCache<valueType, memberType, keyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 缓存计数器
        /// </summary>
        protected counter<valueType, memberType, keyType> counter;
        /// <summary>
        /// 计数缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        protected counterCache(counter<valueType, memberType, keyType> counter)
        {
            if (counter == null) log.Default.Throw(log.exceptionType.Null);
            this.counter = counter;
        }
    }
}
