using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 自增id标识缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class identityCounter<valueType, memberType> : counter<valueType, memberType, int>
        where valueType : class, sqlTable.ISqlIdentity<valueType, memberType, int>
        where memberType : IMemberMap<memberType>
    {
         /// <summary>
        /// 自增id标识缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityCounter
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap = default(memberType))
            : base(sqlTool, memberMap, value => value.SqlIdentity32)
        {
        }
    }
    /// <summary>
    /// 自增id标识缓存计数器(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class identityCounter<valueType> : counter<valueType, memberMap<valueType>, int>
        where valueType : class
    {
        /// <summary>
        /// 自增id标识缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getIdentity">自增id获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityCounter
            (sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, int> getIdentity
            , memberMap<valueType> memberMap = default(memberMap<valueType>))
            : base(sqlTool, memberMap, getIdentity)
        {
        }
    }
}