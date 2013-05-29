using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 事件缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class eventCache<valueType, memberType> : copy<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 添加记录事件
        /// </summary>
        public event action<valueType> OnInserted;
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="value">新添加的对象</param>
        protected void callOnInserted(valueType value)
        {
            if (OnInserted != null) OnInserted(value);
        }
        /// <summary>
        /// 更新记录事件
        /// </summary>
        public event action<valueType, valueType> OnUpdated;
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="value">更新后的对象</param>
        /// <param name="oldValue">更新前的对象</param>
        protected void callOnUpdated(valueType value, valueType oldValue)
        {
            if (OnUpdated != null) OnUpdated(value, oldValue);
        }
        /// <summary>
        /// 删除记录事件
        /// </summary>
        public event action<valueType> OnDeleted;
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="value">被删除的对象</param>
        protected void callOnDeleted(valueType value)
        {
            if (OnDeleted != null) OnDeleted(value);
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public abstract IEnumerable<valueType> Values { get; }
        /// <summary>
        /// 事件缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        protected eventCache(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap)
            : base(sqlTool, memberMap) { }
    }
}
