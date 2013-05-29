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
    public class queueList<valueType, memberType, keyType>
        : queue<valueType, memberType, keyType, list<valueType>>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 条件表达式获取器
        /// </summary>
        protected func<keyType, Expression<func<valueType, bool>>> getWhere;
        /// <summary>
        /// 先进先出优先队列 列表缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueList(sqlTable.sqlToolBase<valueType, memberType> sqlTool, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, memberType memberMap = default(memberType), int maxCount = 0)
            : base(sqlTool, memberMap, getKey, maxCount)
        {
            if (getWhere == null) log.Default.Throw(log.exceptionType.Null);
            this.getWhere = getWhere;

            sqlTool.OnInsertedLock += onInserted;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            keyType key = getKey(value);
            list<valueType> values = queueCache.Get(key, null);
            if (values != null)
            {
                valueType newValue = fastCSharp.setup.constructor<valueType>.New;
                copyFrom(newValue, value, memberMap);
                values.Add(newValue);
            }
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        protected list<valueType> getList(keyType key)
        {
            list<valueType> list = queueCache.Get(key, null);
            if (list == null)
            {
                list = SqlTool.Where(getWhere(key), memberMap).getList() ?? new list<valueType>();
                queueCache[key] = list;
                if (queueCache.Count > maxCount) queueCache.Pop();
            }
            return list;
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据数量</returns>
        public int GetCountLock(keyType key, func<valueType, bool> isValue)
        {
            Monitor.Enter(SqlTool.Lock);
            try
            {
                return getList(key).count(isValue);
            }
            finally { Monitor.Exit(SqlTool.Lock); }
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
                return getList(key).getArray();
            }
            finally { Monitor.Exit(SqlTool.Lock); }
        }
    }
    /// <summary>
    /// 先进先出优先队列 列表缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class queueList<valueType, keyType>
        : queueList<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 先进先出优先队列 列表缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public queueList(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, keyType> getKey
            , func<keyType, Expression<func<valueType, bool>>> getWhere, memberMap<valueType> memberMap = default(memberMap<valueType>)
            , int maxCount = 0)
            : base(sqlTool, getKey, getWhere, memberMap, maxCount)
        {
        }
    }
}
