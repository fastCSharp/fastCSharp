using System;
using System.Data;
using fastCSharp.setup;

namespace fastCSharp.sql
{
    /// <summary>
    /// 数据列
    /// </summary>
    public class column
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public SqlDbType DbType;
        /// <summary>
        /// 列长
        /// </summary>
        public int Size;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNull;

        /// <summary>
        /// 判断是否匹配数据列
        /// </summary>
        /// <param name="value">数据列</param>
        /// <returns>是否匹配</returns>
        internal bool IsMatch(column value)
        {
            return value != null && Name == value.Name && DbType == value.DbType && Size == value.Size && IsNull == value.IsNull;
        }
    }
}
