using System;
using fastCSharp.setup.cSharp;
using System.Threading;
using System.Linq.Expressions;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// 先进先出优先队列 列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class queueListOrderLady<valueType, memberType, keyType>
        : queue<valueType, memberType, keyType, listOrderLadyValue<valueType>>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 条件表达式获取器
        /// </summary>
        protected func<keyType, Expression<func<valueType, bool>>> getWhere;
        /// <summary>
        /// 排序器
        /// </summary>
        protected func<list<valueType>, valueType[]> sorter;
        /// <summary>
        /// 先进先出优先队列 列表缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueListOrderLady(sqlTable.sqlToolBase<valueType, memberType> sqlTool, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, func<list<valueType>, valueType[]> sorter
            , memberType memberMap = default(memberType), int maxCount = 0)
            : base(sqlTool, memberMap, getKey, maxCount)
        {
            if (getWhere == null || sorter == null) log.Default.Throw(log.exceptionType.Null);
            this.getWhere = getWhere;
            this.sorter = sorter;

            sqlTool.OnInsertedLock += onInserted;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        protected void onInserted(valueType value)
        {
            keyType key = getKey(value);
            listOrderLadyValue<valueType> cache = queueCache.Get(key, default(listOrderLadyValue<valueType>));
            if (cache.List != null)
            {
                valueType newValue = fastCSharp.setup.constructor<valueType>.New;
                copyFrom(newValue, value, memberMap);
                cache.List.Add(newValue);
                cache.Array = null;
                cache.IsSort = false;
                queueCache[key] = cache;
            }
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        protected listOrderLadyValue<valueType> getCache(keyType key)
        {
            listOrderLadyValue<valueType> cache = queueCache.Get(key, default(listOrderLadyValue<valueType>));
            if (cache.List == null)
            {
                cache.List = SqlTool.Where(getWhere(key), memberMap).getList() ?? new list<valueType>();
                queueCache[key] = cache;
                if (queueCache.Count > maxCount) queueCache.Pop();
            }
            return cache;
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>匹配数据数量</returns>
        public int GetCount(keyType key)
        {
            listOrderLadyValue<valueType> cache;
            Monitor.Enter(SqlTool.Lock);
            try
            {
                cache = getCache(key);
                if (cache.Array == null) return cache.List.Count;
            }
            finally { Monitor.Exit(SqlTool.Lock); }
            return cache.Array.Length;
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据数量</returns>
        public int GetCountLock(keyType key, func<valueType, bool> isValue)
        {
            listOrderLadyValue<valueType> cache;
            Monitor.Enter(SqlTool.Lock);
            try
            {
                cache = getCache(key);
                if (cache.Array == null) return cache.List.count(isValue);
            }
            finally { Monitor.Exit(SqlTool.Lock); }
            return cache.Array.count(isValue);
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        public valueType[] GetArray(keyType key)
        {
            Monitor.Enter(SqlTool.Lock);
            try
            {
                listOrderLadyValue<valueType> cache = getCache(key);
                if (cache.Array == null)
                {
                    cache.Array = cache.List.getArray();
                    cache.IsSort = false;
                    queueCache[key] = cache;
                }
                return cache.Array;
            }
            finally { Monitor.Exit(SqlTool.Lock); }
        }
        /// <summary>
        /// 获取数据范围集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据范围集合</returns>
        public collection<valueType> GetSort(keyType key, int skipCount, int getCount)
        {
            listOrderLadyValue<valueType> cache;
            array.range range;
            Monitor.Enter(SqlTool.Lock);
            try
            {
                cache = getCache(key);
                range = new array.range(cache.List.count(), skipCount, getCount);
                if (range.GetCount != 0)
                {
                    if (!cache.IsSort)
                    {
                        cache.Array = sorter(cache.List);
                        cache.IsSort = true;
                        queueCache[key] = cache;
                    }
                }
            }
            finally { Monitor.Exit(SqlTool.Lock); }
            return range.GetCount != 0 ? cache.Array.sub(range.SkipCount, range.GetCount) : null;
        }
        /// <summary>
        /// 获取匹配分页集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="count">匹配数据总数</param>
        /// <returns>匹配分页集合</returns>
        public list<valueType> GetPage(keyType key, int pageSize, int currentPage, func<valueType, bool> isValue, out int count)
        {
            listOrderLadyValue<valueType> cache;
            Monitor.Enter(SqlTool.Lock);
            try
            {
                cache = getCache(key);
                if (!cache.IsSort)
                {
                    cache.Array = sorter(cache.List);
                    cache.IsSort = true;
                    queueCache[key] = cache;
                }
            }
            finally { Monitor.Exit(SqlTool.Lock); }
            return cache.GetArrayPage(pageSize, currentPage, isValue, out count);
        }
    }
    /// <summary>
    /// 先进先出优先队列 列表缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class queueListOrderLady<valueType, keyType>
        : queueListOrderLady<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 先进先出优先队列 列表缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueListOrderLady(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, func<list<valueType>, valueType[]> sorter
            , memberMap<valueType> memberMap = default( memberMap<valueType>), int maxCount = 0)
            : base(sqlTool, getKey, getWhere, sorter, memberMap, maxCount)
        {
        }
    }
}
