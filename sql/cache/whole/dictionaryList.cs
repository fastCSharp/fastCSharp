using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 分组列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryList<valueType, memberType, keyType>
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
        /// 分组数据
        /// </summary>
        protected Dictionary<keyType, keyValue<keyType, list<valueType>>> groups;
        /// <summary>
        /// 关键字版本号
        /// </summary>
        protected int keyVersion;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public dictionaryList(eventCache<valueType, memberType> cache, func<valueType, keyType> getKey, bool isReset = true)
        {
            if (cache == null || getKey == null) log.Default.Throw(log.exceptionType.Null);
            this.cache = cache;
            this.getKey = getKey;

            if (isReset)
            {
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
                cache.OnDeleted += onDeleted;

                reset();
            }
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            insert insert = new insert { groups = new Dictionary<keyType, keyValue<keyType, list<valueType>>>(), getKey = getKey };
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                foreach (valueType value in cache.Values) insert.onInserted(value);
                this.groups = insert.groups;
                ++keyVersion;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
        }
        /// <summary>
        /// 数据添加器
        /// </summary>
        protected struct insert
        {
            /// <summary>
            /// 分组数据
            /// </summary>
            public Dictionary<keyType, keyValue<keyType, list<valueType>>> groups;
            /// <summary>
            /// 分组字典关键字获取器
            /// </summary>
            public func<valueType, keyType> getKey;
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据对象</param>
            public void onInserted(valueType value)
            {
                keyValue<keyType, list<valueType>> list;
                keyType key = getKey(value);
                if (!groups.TryGetValue(key, out list)) groups.Add(key, list = new keyValue<keyType, list<valueType>>(key, new list<valueType>()));
                list.Value.Add(value);
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="cache">缓存对象</param>
            public void onInserted(valueType value, dictionaryList<valueType, memberType, keyType> cache)
            {
                keyValue<keyType, list<valueType>> list;
                keyType key = getKey(value);
                if (!groups.TryGetValue(key, out list))
                {
                    groups.Add(key, list = new keyValue<keyType, list<valueType>>(key, new list<valueType>()));
                    ++cache.keyVersion;
                }
                list.Value.Add(value);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        protected void onInserted(valueType value)
        {
            new insert { groups = groups, getKey = getKey }.onInserted(value, this);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        protected void onUpdated(valueType value, valueType oldValue)
        {
            keyType oldKey = getKey(oldValue);
            if (!getKey(value).Equals(oldKey))
            {
                onInserted(value);
                onDeleted(value, oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除的数据关键字</param>
        protected void onDeleted(valueType value, keyType key)
        {
            keyValue<keyType, list<valueType>> keyList;
            if (groups.TryGetValue(key, out keyList))
            {
                list<valueType> list = keyList.Value;
                int index = Array.LastIndexOf(list.Unsafer.Array, value, list.Count - 1);
                if (index != -1)
                {
                    if (list.Count != 1) list.RemoveAt(index);
                    else groups.Remove(key);
                    return;
                }
            }
            log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", false, true);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getKey(value));
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>匹配数据数量</returns>
        public int Count(keyType key)
        {
            keyValue<keyType, list<valueType>> list;
            return groups.TryGetValue(key, out list) ? list.Value.Count : 0;
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据数量</returns>
        public int CountLock(keyType key, func<valueType, bool> isValue)
        {
            keyValue<keyType, list<valueType>> list;
            int version = keyVersion;
            if (groups.TryGetValue(key, out list) && list.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return list.Value.GetCount(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out list)) return list.Value.GetCount(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return 0;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        public valueType[] GetArray(keyType key)
        {
            keyValue<keyType, list<valueType>> list;
            int version = keyVersion;
            if (groups.TryGetValue(key, out list) && list.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return list.Value.GetArray();
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out list)) return list.Value.GetArray();
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据集合</returns>
        public valueType[] GetFindArrayLock(keyType key, func<valueType, bool> isValue)
        {
            keyValue<keyType, list<valueType>> list;
            int version = keyVersion;
            if (groups.TryGetValue(key, out list) && list.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return list.Value.GetFindArray(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out list)) return list.Value.GetFindArray(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数据集合</returns>
        public list<valueType> GetFindLock(keyType key, func<valueType, bool> isValue)
        {
            keyValue<keyType, list<valueType>> list;
            int version = keyVersion;
            if (groups.TryGetValue(key, out list) && list.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return list.Value.GetFind(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out list)) return list.Value.GetFind(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return null;
        }
    }
    /// <summary>
    /// 分组列表缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryList<valueType, keyType> : dictionaryList<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public dictionaryList(eventCache<valueType, memberMap<valueType>> cache, func<valueType, keyType> getKey, bool isReset = true)
            :base(cache, getKey, isReset)
        {
        }
    }
}
