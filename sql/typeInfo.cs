using System;

namespace fastCSharp.sql
{
    /// <summary>
    /// SQL类型信息
    /// </summary>
    public class typeInfo : Attribute
    {
        /// <summary>
        /// SQL客户端处理类型
        /// </summary>
        public Type ClientType;

        /// <summary>
        /// SQL类型
        /// </summary>
        public enum type
        {
            /// <summary>
            /// SQL Server2000
            /// </summary>
            [typeInfo(ClientType = typeof(msSql.sql2000))]
            sql2000,
            /// <summary>
            /// SQL Server2005
            /// </summary>
            [typeInfo(ClientType = typeof(msSql.sql2005))]
            sql2005,
            /// <summary>
            /// SQL Server2008
            /// </summary>
            [typeInfo(ClientType = typeof(msSql.sql2008))]
            sql2008,
            ///// <summary>
            ///// Excel
            ///// </summary>
            //[typeInfo(ClientType = typeof(excel.sqlClient))]
            //excel,
        }
    }
}
