using System;
using fastCSharp.setup.cSharp;
using System.Threading;
using System.Collections.Generic;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 分组列表 延时排序缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryListOrderLadyWhere<valueType, memberType, keyType>
        : dictionaryListOrderLady<valueType, memberType, keyType>
        where valueType : class
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 缓存值判定
        /// </summary>
        private func<valueType, bool> isValue;
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isValue">缓存值判定</param>
        public dictionaryListOrderLadyWhere
            (eventCache<valueType, memberType> cache, func<valueType, keyType> getKey, func<list<valueType>, valueType[]> sorter
            , func<valueType, bool> isValue)
            : base(cache, getKey, sorter, false)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
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
            insert insert = new insert { groups = new Dictionary<keyType, group>(), getKey = getKey };
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                foreach (valueType value in cache.Values)
                {
                    if (isValue(value)) insert.onInserted(value);
                }
                groups = insert.groups;
                ++keyVersion;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
        }
         /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        private new void onInserted(valueType value)
        {
            if (isValue(value)) base.onInserted(value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        private new void onUpdated(valueType value, valueType oldValue)
        {
            if (isValue(value))
            {
                if (isValue(oldValue)) base.onUpdated(value, oldValue);
                else base.onInserted(value);
            }
            else if (isValue(oldValue)) onDeleted(value, getKey(oldValue));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private new void onDeleted(valueType value)
        {
            if (isValue(value)) base.onDeleted(value);
        }
    }
    /// <summary>
    /// 分组列表 延时排序缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public class dictionaryListOrderLadyWhere<valueType, keyType> : dictionaryListOrderLadyWhere<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isValue">缓存值判定</param>
        public dictionaryListOrderLadyWhere
            (eventCache<valueType, memberMap<valueType>> cache, func<valueType, keyType> getKey, func<list<valueType>, valueType[]> sorter
            , func<valueType, bool> isValue)
            : base(cache, getKey, sorter, isValue)
        {
        }
    }
}
