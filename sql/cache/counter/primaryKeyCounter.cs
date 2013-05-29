using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.counter
{
    /// <summary>
    /// 关键字缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class primaryKeyCounter<valueType, memberType, keyType> : counter<valueType, memberType, keyType>
        where valueType : class, sqlTable.ISqlKey<valueType, memberType, keyType>
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
         /// <summary>
        /// 关键字缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        public primaryKeyCounter
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap = default(memberType))
            : base(sqlTool, memberMap, value => value.SqlPrimaryKey)
        {
        }
    }
    /// <summary>
    /// 关键字缓存计数器(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class primaryKeyCounter<valueType, keyType> : counter<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        public primaryKeyCounter
            (sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, keyType> getKey
            , memberMap<valueType> memberMap = default(memberMap<valueType>))
            : base(sqlTool, memberMap, getKey)
        {
        }
    }
}
