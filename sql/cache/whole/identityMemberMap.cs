using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class identityMemberMap<valueType, memberType> : eventCache<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 自增ID获取器
        /// </summary>
        protected func<valueType, int> getIdentity;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get; protected set; }
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getIdentity">自增ID获取器</param>
        protected identityMemberMap(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, int> getIdentity)
            : base(sqlTool, memberMap)
        {
            if (getIdentity == null) log.Default.Throw(log.exceptionType.Null);
            this.getIdentity = getIdentity;
        }
    }
}
