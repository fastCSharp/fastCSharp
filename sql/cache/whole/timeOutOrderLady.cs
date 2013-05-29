using System;
using System.Collections.Generic;
using fastCSharp.setup.cSharp;
using System.Threading;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 超时缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class timeOutOrderLady<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected eventCache<valueType, memberType> cache;
        /// <summary>
        /// 超时秒数
        /// </summary>
        protected double timeOutSeconds;
        /// <summary>
        /// 最小有效时间
        /// </summary>
        protected DateTime outTime
        {
            get
            {
                return date.NowTime.AddSeconds(timeOutSeconds);
            }
        }
        /// <summary>
        /// 排序数据最小时间
        /// </summary>
        protected DateTime minTime;
        /// <summary>
        /// 时间获取器
        /// </summary>
        protected func<valueType, DateTime> getTime;
        /// <summary>
        /// 数据集合
        /// </summary>
        protected HashSet<valueType> values;
        /// <summary>
        /// 数据数组
        /// </summary>
        protected valueType[] array;
        /// <summary>
        /// 数据数组是否排序
        /// </summary>
        protected bool isSort;
        /// <summary>
        /// 超时缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="timeOutSeconds">超时秒数</param>
        /// <param name="getKey">关键字获取器</param>
        /// <param name="isReset">是否绑定事件与重置数据</param>
        public timeOutOrderLady(eventCache<valueType, memberType> cache
            , double timeOutSeconds, func<valueType, DateTime> getTime, bool isReset = true)
        {
            if (cache == null || timeOutSeconds < 1 || getTime == null) log.Default.Throw(log.exceptionType.Null);
            this.cache = cache;
            this.timeOutSeconds = -timeOutSeconds;
            this.getTime = getTime;

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
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                HashSet<valueType> newValues = new HashSet<valueType>();
                DateTime minTime = this.outTime;
                foreach (valueType value in cache.Values)
                {
                    if (getTime(value) > minTime) newValues.Add(value);
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
        protected void onInserted(valueType value)
        {
            if (getTime(value) > outTime)
            {
                values.Add(value);
                array = null;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        protected void onUpdated(valueType value, valueType oldValue)
        {
            onInserted(value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            if (values.Remove(value)) array = null;
        }
        /// <summary>
        /// 删除过期数据
        /// </summary>
        private void remove()
        {
            DateTime outTime = this.outTime;
            list<valueType> removeValues = this.values.getFind(value => getTime(value) < outTime);
            if (removeValues != null)
            {
                foreach (valueType value in removeValues) this.values.Remove(value);
            }
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns>数据数量</returns>
        public int GetCount()
        {
            return GetArray().Length;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>数据集合</returns>
        public valueType[] GetArray()
        {
            valueType[] values = array;
            if (values == null || minTime < date.NowTime)
            {
                Monitor.Enter(cache.SqlTool.Lock);
                try
                {
                    if ((values = array) == null || minTime < date.NowTime)
                    {
                        remove();
                        values = array = this.values.getArray();
                        minTime = array.Length != 0 ? array.maxKey(getTime, DateTime.MinValue).AddSeconds(timeOutSeconds) : DateTime.MaxValue;
                        isSort = array.Length < 2;
                    }
                }
                finally { Monitor.Exit(cache.SqlTool.Lock); }
            }
            return values;
        }
        /// <summary>
        /// 获取排序后的数据集合
        /// </summary>
        /// <returns>排序后的数据集合</returns>
        public valueType[] GetSort()
        {
            Monitor.Enter(cache.SqlTool.Lock);
            try
            {
                if (array == null || minTime < date.NowTime)
                {
                    remove();
                    array = this.values.getArray().sortDesc(getTime);
                    minTime = array.Length != 0 ? getTime(array[array.Length - 1]).AddSeconds(timeOutSeconds) : DateTime.MaxValue;
                    isSort = true;
                }
                else if (!isSort) array = array.sortDesc(getTime);
                return array;
            }
            finally { Monitor.Exit(cache.SqlTool.Lock); }
        }
    }
    /// <summary>
    /// 超时缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class timeOutOrderLady<valueType> : timeOutOrderLady<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 超时缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="timeOutSeconds">超时秒数</param>
        /// <param name="getKey">关键字获取器</param>
        /// <param name="isReset">是否绑定事件与重置数据</param>
        public timeOutOrderLady(eventCache<valueType, memberMap<valueType>> cache
            , double timeOutSeconds, func<valueType, DateTime> getTime, bool isReset = true)
            : base(cache, timeOutSeconds, getTime, isReset)
        {
        }
    }
}
