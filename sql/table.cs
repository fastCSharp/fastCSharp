using System;

namespace fastCSharp.sql
{
    /// <summary>
    /// 表格信息
    /// </summary>
    public class table
    {
        /// <summary>
        /// 列集合
        /// </summary>
        public columnCollection Columns;
        /// <summary>
        /// 主键
        /// </summary>
        public columnCollection PrimaryKey;
        /// <summary>
        /// 自增列
        /// </summary>
        public column Identity;
        /// <summary>
        /// 索引集合
        /// </summary>
        public columnCollection[] Indexs;
    }
}
