using System;
using System.Collections.Generic;
using System.Threading;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class primaryKey<valueType, memberType, keyType> : key<valueType, memberType, keyType>
        where valueType : class, sqlTable.ISqlKey<valueType, memberType, keyType>
        where memberType : IMemberMap<memberType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        public primaryKey(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap = default(memberType))
            : base(sqlTool, value => value.SqlPrimaryKey, memberMap)
        { }
    }
    /// <summary>
    /// 关键字整表缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class primaryKey<valueType, keyType> : key<valueType, memberMap<valueType>, keyType>
        where valueType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="memberMap">数据成员位图</param>
        public primaryKey(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, keyType> getKey
            , memberMap<valueType> memberMap = default(memberMap<valueType>))
            : base(sqlTool, getKey, memberMap)
        { }
    }
}
