using System;
using fastCSharp.setup.cSharp;
using System.Threading;
using System.Collections.Generic;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 超时缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class timeOutOrderLadyWhere<valueType, memberType> : timeOutOrderLady<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private func<valueType, bool> isValue;
        /// <summary>
        /// 超时缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="timeOutSeconds">超时秒数</param>
        /// <param name="getKey">关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        public timeOutOrderLadyWhere(eventCache<valueType, memberType> cache
            , double timeOutSeconds, func<valueType, DateTime> getTime, func<valueType, bool> isValue)
            : base(cache, timeOutSeconds, getTime, false)
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
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                HashSet<valueType> newValues = new HashSet<valueType>();
                DateTime minTime = this.outTime;
                foreach (valueType value in cache.Values)
                {
                    if (getTime(value) > minTime && isValue(value)) newValues.Add(value);
                }
                values = newValues;
                array = null;
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
            onInserted(value);
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
    /// 超时缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class timeOutOrderLadyWhere<valueType> : timeOutOrderLadyWhere<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 超时缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="timeOutSeconds">超时秒数</param>
        /// <param name="getKey">关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        public timeOutOrderLadyWhere(eventCache<valueType, memberMap<valueType>> cache
            , double timeOutSeconds, func<valueType, DateTime> getTime, func<valueType, bool> isValue)
            : base(cache, timeOutSeconds, getTime, isValue)
        {
        }
    }
}
