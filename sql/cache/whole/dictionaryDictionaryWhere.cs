using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 分组字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="groupKeyType">分组关键字类型</typeparam>
    /// <typeparam name="keyType">字典关键字类型</typeparam>
    public class dictionaryDictionaryWhere<valueType, memberType, groupKeyType, keyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where groupKeyType : IEquatable<groupKeyType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected eventCache<valueType, memberType> cache;
        /// <summary>
        /// 分组关键字获取器
        /// </summary>
        protected func<valueType, groupKeyType> getGroupKey;
        /// <summary>
        /// 字典关键字获取器
        /// </summary>
        protected func<valueType, keyType> getKey;
        /// <summary>
        /// 数据匹配器
        /// </summary>
        protected func<valueType, bool> isValue;
        /// <summary>
        /// 分组数据
        /// </summary>
        protected Dictionary<groupKeyType, Dictionary<keyType, valueType>> groups;
        /// <summary>
        /// 分组字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getGroupKey">分组关键字获取器</param>
        /// <param name="getKey">字典关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        public dictionaryDictionaryWhere(eventCache<valueType, memberType> cache
            , func<valueType, groupKeyType> getGroupKey, func<valueType, keyType> getKey, func<valueType, bool> isValue)
        {
            if (cache == null || getGroupKey == null || getKey == null || isValue == null)
            {
                log.Default.Throw(log.exceptionType.Null);
            }
            this.cache = cache;
            this.getGroupKey = getGroupKey;
            this.getKey = getKey;
            this.isValue = isValue;

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
            insert insert = new insert { groups = new Dictionary<groupKeyType, Dictionary<keyType, valueType>>(), getGroupKey = getGroupKey, getKey = getKey };
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                foreach (valueType value in cache.Values)
                {
                    if (isValue(value)) insert.onInserted(value);
                }
                this.groups = insert.groups;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
        }
        /// <summary>
        /// 数据添加器
        /// </summary>
        private struct insert
        {
            /// <summary>
            /// 分组数据
            /// </summary>
            public Dictionary<groupKeyType, Dictionary<keyType, valueType>> groups;
            /// <summary>
            /// 分组关键字获取器
            /// </summary>
            public func<valueType, groupKeyType> getGroupKey;
            /// <summary>
            /// 字典关键字获取器
            /// </summary>
            public func<valueType, keyType> getKey;
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据对象</param>
            public void onInserted(valueType value)
            {
                onInserted(value, getGroupKey(value));
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="groupKey">分组关键字</param>
            public void onInserted(valueType value, groupKeyType groupKey)
            {
                Dictionary<keyType, valueType> dictionary;
                if (!groups.TryGetValue(groupKey, out dictionary))
                {
                    groups.Add(groupKey, dictionary = new Dictionary<keyType, valueType>());
                }
                dictionary.Add(getKey(value), value);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        protected void onInserted(valueType value)
        {
            if (isValue(value))
            {
                new insert { groups = groups, getGroupKey = getGroupKey, getKey = getKey }.onInserted(value);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        protected void onUpdated(valueType value, valueType oldValue)
        {
            if (isValue(value))
            {
                if (isValue(oldValue))
                {
                    groupKeyType groupKey = getGroupKey(value), oldGroupKey = getGroupKey(oldValue);
                    if (groupKey.Equals(oldGroupKey))
                    {
                        keyType key = getKey(value), oldKey = getKey(oldValue);
                        if (!key.Equals(oldKey))
                        {
                            Dictionary<keyType, valueType> dictionary;
                            if (groups.TryGetValue(groupKey, out dictionary) && dictionary.Remove(oldKey))
                            {
                                dictionary.Add(key, value);
                            }
                            else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
                        }
                    }
                    else
                    {
                        new insert { groups = groups, getKey = getKey }.onInserted(value, groupKey);
                        onDeleted(oldValue, oldGroupKey);
                    }
                }
                else new insert { groups = groups, getKey = getKey }.onInserted(value, getGroupKey(value));
            }
            else if (isValue(oldValue)) onDeleted(oldValue, getGroupKey(oldValue));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="groupKey">分组关键字</param>
        protected void onDeleted(valueType value, groupKeyType groupKey)
        {
            Dictionary<keyType, valueType> dictionary;
            if (groups.TryGetValue(groupKey, out dictionary) && dictionary.Remove(getKey(value)))
            {
                if (dictionary.Count == 0) groups.Remove(groupKey);
            }
            else log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            if (isValue(value)) onDeleted(value, getGroupKey(value));
        }
        /// <summary>
        /// 获取匹配分组数据集合
        /// </summary>
        /// <param name="groupKeys">分组关键字集合</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据集合</returns>
        public list<valueType> GetFindLock(groupKeyType[] groupKeys, func<valueType, bool> isValue)
        {
            if (groupKeys.length() != 0)
            {
                list<valueType> values = new list<valueType>();
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    Dictionary<keyType, valueType> dictionary;
                    foreach (groupKeyType groupKey in groupKeys)
                    {
                        if (groups.TryGetValue(groupKey, out dictionary)) values.Add(dictionary.Values.getFind(isValue));
                    }
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
                return values;
            }
            return null;
        }
    }
    /// <summary>
    /// 分组字典缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="groupKeyType">分组关键字类型</typeparam>
    /// <typeparam name="keyType">字典关键字类型</typeparam>
    public class dictionaryDictionaryWhere<valueType, groupKeyType, keyType>
        : dictionaryDictionaryWhere<valueType, memberMap<valueType>, groupKeyType, keyType>
        where valueType : class
        where groupKeyType : IEquatable<groupKeyType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getGroupKey">分组关键字获取器</param>
        /// <param name="getKey">字典关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        public dictionaryDictionaryWhere(eventCache<valueType, memberMap<valueType>> cache
            , func<valueType, groupKeyType> getGroupKey, func<valueType, keyType> getKey, func<valueType, bool> isValue)
            : base(cache, getGroupKey, getKey, isValue)
        {
        }
    }
}