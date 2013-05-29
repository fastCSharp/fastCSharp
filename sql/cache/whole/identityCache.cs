using System;
using fastCSharp.setup.cSharp;
using System.Collections.Generic;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class identityCache<valueType, memberType> : identityMemberMap<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 缓存数据集合
        /// </summary>
        protected valueType[] values;
        /// <summary>
        /// 数据集合
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                int count = Count;
                foreach (valueType value in values)
                {
                    if (value != null)
                    {
                        yield return value;
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">数据自增ID</param>
        /// <returns>数据</returns>
        public valueType this[int identity]
        {
            get
            {
                valueType[] values = this.values;
                return (uint)identity < (uint)values.Length ? values[identity] : null;
            }
        }
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isEvent">是否绑定更新事件</param>
        protected identityCache
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, int> getIdentity, bool isEvent)
            : base(sqlTool, memberMap, getIdentity)
        {
            if (isEvent)
            {
                //sqlTool.OnUpdatedByIdentityLock += onUpdated;
                sqlTool.OnUpdatedLock += onUpdated;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        protected void onUpdated(valueType value, valueType oldValue, memberType memberMap)
        {
            valueType cacheValue = values[getIdentity(value)];
            update(cacheValue, value, oldValue, updateMemberMap(memberMap));
            callOnUpdated(cacheValue, oldValue);
        }
    }
}
