using System;

namespace fastCSharp.sql
{
    /// <summary>
    /// 列集合
    /// </summary>
    public class columnCollection
    {
        /// <summary>
        /// 列集合类型
        /// </summary>
        public enum type
        {
            /// <summary>
            /// 普通集合
            /// </summary>
            None,
            /// <summary>
            /// 主键
            /// </summary>
            PrimaryKey,
            /// <summary>
            /// 普通索引
            /// </summary>
            Index,
            /// <summary>
            /// 唯一索引
            /// </summary>
            UniqueIndex,
        }
        /// <summary>
        /// 列集合名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 列集合类型
        /// </summary>
        public type Type;
        /// <summary>
        /// 列集合
        /// </summary>
        public column[] Columns;
    }
}
