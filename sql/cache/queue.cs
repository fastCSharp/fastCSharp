using System;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">缓存数据类型</typeparam>
    public abstract class queue<valueType, memberType, keyType, cacheValueType> : copy<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
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
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected queue(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap
            , func<valueType, keyType> getKey, int maxCount)
            : base(sqlTool, memberMap)
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
            return queueCache.Get(key, default(cacheValueType));
        }
    }
}
