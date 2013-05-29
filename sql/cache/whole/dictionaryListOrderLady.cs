using System;
using System.Collections.Generic;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 分组列表 延时排序缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryListOrderLady<valueType, memberType, keyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组数据
        /// </summary>
        protected struct group
        {
            /// <summary>
            /// 分组字典关键字
            /// </summary>
            public keyType Key;
            /// <summary>
            /// 分组数据
            /// </summary>
            public list<valueType> List;
            /// <summary>
            /// 分组有序数据索引
            /// </summary>
            public int Index;
        }
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected eventCache<valueType, memberType> cache;
        /// <summary>
        /// 分组字典关键字获取器
        /// </summary>
        protected func<valueType, keyType> getKey;
        /// <summary>
        /// 排序器
        /// </summary>
        private func<list<valueType>, valueType[]> sorter;
        /// <summary>
        /// 分组数据
        /// </summary>
        protected Dictionary<keyType, group> groups;
        /// <summary>
        /// 关键字版本号
        /// </summary>
        protected int keyVersion;
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        public dictionaryListOrderLady(eventCache<valueType, memberType> cache
            , func<valueType, keyType> getKey, func<list<valueType>, valueType[]> sorter, bool isReset = true)
        {
            if (cache == null || getKey == null || sorter == null) log.Default.Throw(log.exceptionType.Null);
            this.cache = cache;
            this.getKey = getKey;
            this.sorter = sorter;

            if (isReset)
            {
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
                cache.OnDeleted += onDeleted;

                reset();
            }
        }
        /// <summary>
        /// 数据添加器
        /// </summary>
        protected struct insert
        {
            /// <summary>
            /// 分组数据
            /// </summary>
            public Dictionary<keyType, group> groups;
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
                keyType key = getKey(value);
                group values;
                if (!groups.TryGetValue(key, out values))
                {
                    groups.Add(key, values = new group { Key = key, List = new list<valueType>(), Index = 0 });
                }
                values.List.Add(value);
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="cache">缓存对象</param>
            public void onInserted(valueType value, dictionaryListOrderLady<valueType, memberType, keyType> cache)
            {
                keyType key = getKey(value);
                group values;
                if (!groups.TryGetValue(key, out values))
                {
                    groups.Add(key, values = new group { Key = key, List = new list<valueType>(), Index = 0 });
                    ++cache.keyVersion;
                }
                values.List.Add(value);
            }
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            insert insert = new insert { groups = new Dictionary<keyType, group>(), getKey = getKey };
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                foreach (valueType value in cache.Values) insert.onInserted(value);
                groups = insert.groups;
                ++keyVersion;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
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
            keyType key = getKey(value), oldKey = getKey(oldValue);
            if (key.Equals(oldKey))
            {
                group values;
                if (indexOf(value, key, out values) == -1)
                {
                    log.Default.Add("ERROR " + key.ToString(), false, false);
                }
            }
            else
            {
                onInserted(value);
                onDeleted(value, oldKey);
            }
        }
        /// <summary>
        /// 查找历史位置
        /// </summary>
        /// <param name="value">待查找对象</param>
        /// <param name="key">查找关键字</param>
        /// <param name="values">列表数据集合</param>
        /// <returns>历史位置,失败为-1</returns>
        private int indexOf(valueType value, keyType key, out group values)
        {
            if (groups.TryGetValue(key, out values))
            {
                valueType[] array = values.List.Unsafer.Array;
                int index = Array.LastIndexOf(array, value, values.List.Count - 1);
                if (index != -1)
                {
                    if (values.Index > index)
                    {
                        values.Index = index;
                        groups[key] = values;
                    }
                    return index;
                }
            }
            log.Default.Add(typeof(valueType).FullName + " 缓存同步错误", true, true);
            return -1;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除数据的关键字</param>
        protected void onDeleted(valueType value, keyType key)
        {
            group values;
            int index = indexOf(value, key, out values);
            if (index != -1)
            {
                values.List.Unsafer.AddLength(-1);
                int count = values.List.Count;
                if (count != 0)
                {
                    valueType[] array = values.List.Unsafer.Array;
                    array[index] = array[count];
                    array[count] = null;
                }
                else groups.Remove(key);
            }
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
        /// 获取匹配数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>匹配数量</returns>
        public int Count(keyType key)
        {
            group values;
            return groups.TryGetValue(key, out values) ? values.List.Count : 0;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数量</returns>
        public int CountLock(keyType key, func<valueType, bool> isValue)
        {
            group values;
            int version = keyVersion;
            if (groups.TryGetValue(key, out values) && values.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return values.List.GetCount(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out values)) return values.List.GetCount(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return 0;
        }
        /// <summary>
        /// 查找第一个匹配的数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>第一个匹配的数据,失败返回null</returns>
        public valueType FirstOrDefaultLock(keyType key, func<valueType, bool> isValue)
        {
            group values;
            int version = keyVersion;
            if (groups.TryGetValue(key, out values) && values.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return values.List.FirstOrDefault(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out values)) return values.List.FirstOrDefault(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return null;
        }
        /// <summary>
        /// 获取匹配的数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>数据集合</returns>
        public valueType[] GetFindArrayNoOrderLock(keyType key, func<valueType, bool> isValue)
        {
            group values;
            int version = keyVersion;
            if (groups.TryGetValue(key, out values) && values.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return values.List.GetFindArray(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out values)) return values.List.GetFindArray(isValue);
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取不排序的数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        public valueType[] GetArrayNoOrder(keyType key)
        {
            group values;
            int version = keyVersion;
            if (groups.TryGetValue(key, out values) && values.Key.Equals(key))
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    return values.List.GetArray();
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            else if (version != keyVersion)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if (groups.TryGetValue(key, out values)) return values.List.GetArray();
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据列表</returns>
        private list<valueType> get(keyType key)
        {
            group values;
            if (groups.TryGetValue(key, out values) && values.Key.Equals(key))
            {
                list<valueType> list = values.List;
                if (values.Index != list.Count)
                {
                    sorter(list).CopyTo(list.Unsafer.Array, 0);
                    values.Index = list.Count;
                    groups[key] = values;
                }
                return values.List;
            }
            return null;
        }
        /// <summary>
        /// 获取逆序分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <returns>逆序分页数据集合</returns>
        public valueType[] GetPageDesc(keyType key, int pageSize, int currentPage, out int count)
        {
            valueType[] values = null;
            count = 0;
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                list<valueType> list = get(key);
                if (list != null)
                {
                    array.page page = new array.page(count = list.Count, pageSize, currentPage);
                    values = list.GetSub(count - page.SkipCount - page.CurrentPageSize, page.CurrentPageSize);
                }
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
            return values.reverse();
        }
        /// <summary>
        /// 获取逆序分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <param name="count">数据总数</param>
        /// <returns>逆序分页数据集合</returns>
        public list<valueType> GetPageDescLock
            (keyType key, int pageSize, int currentPage, func<valueType, bool> isValue, out int count)
        {
            list<valueType> values = null;
            count = 0;
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                list<valueType> list = get(key);
                if (list != null)
                {
                    valueType[] array = list.Unsafer.Array;
                    array.page page = new array.page(count = list.Count, pageSize, currentPage);
                    list<valueType>.unsafer valueList = new list<valueType>(pageSize).Unsafer;
                    for (int index = list.Count, skipCount = page.SkipCount, getCount = page.CurrentPageSize; --index >= 0; )
                    {
                        valueType value = array[index];
                        if (isValue(value))
                        {
                            if (skipCount == 0)
                            {
                                valueList.Add(value);
                                if (--getCount == 0) break;
                            }
                            else --skipCount;
                        }
                    }
                    values = valueList.List;
                }
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
            return values;
        }
        /// <summary>
        /// 获取排序数据范围集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序数据范围集合</returns>
        public valueType[] GetSort(keyType key, int skipCount, int getCount)
        {
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                list<valueType> list = get(key);
                if (list != null)
                {
                    array.range range = new array.range(list.Count, skipCount, getCount);
                    if ((getCount = range.GetCount) != 0) return list.GetSub(range.SkipCount, range.GetCount);
                }
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取逆序数据范围集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>逆序数据范围集合</returns>
        public valueType[] GetSortDesc(keyType key, int skipCount, int getCount)
        {
            valueType[] values = null;
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                list<valueType> list = get(key);
                if (list != null)
                {
                    array.range range = new array.range(list.Count, skipCount, getCount);
                    if ((getCount = range.GetCount) != 0)
                    {
                        values = list.GetSub(list.Count - range.SkipCount - range.GetCount, range.GetCount);
                    }
                }
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
            return values.reverse();
        }
    }
    /// <summary>
    /// 分组列表 延时排序缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryListOrderLady<valueType, keyType> : dictionaryListOrderLady<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        public dictionaryListOrderLady(eventCache<valueType, memberMap<valueType>> cache
            , func<valueType, keyType> getKey, func<list<valueType>, valueType[]> sorter, bool isReset = true)
            : base(cache, getKey, sorter, isReset)
        {
        }
    }
}
