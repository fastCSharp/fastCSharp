using System;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// SQL成员信息
    /// </summary>
    public partial class sqlMember : ignore
    {
        /// <summary>
        /// SQL成员信息空值
        /// </summary>
        private static readonly sqlMember defaultSqlMember = new sqlMember();
        /// <summary>
        /// SQL类型
        /// </summary>
        public Type SqlType;
        /// <summary>
        /// SQL成员类型
        /// </summary>
        private memberType sqlMemberType;
        /// <summary>
        /// SQL成员类型
        /// </summary>
        internal memberType SqlMemberType
        {
            get
            {
                if (SqlType == null) return null;
                if (sqlMemberType == null) sqlMemberType = SqlType;
                return sqlMemberType;
            }
        }
        /// <summary>
        /// 外键实体类型，不可用
        /// </summary>
        public Type Link;
        /// <summary>
        /// 外键不存在时是否删除记录，不可用
        /// </summary>
        public bool IsDeleteLink;
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey;
        /// <summary>
        /// 是否允许空值
        /// </summary>
        public bool IsNull;
        /// <summary>
        /// 字符串是否ASCII
        /// </summary>
        public bool IsAscii;
        /// <summary>
        /// 是否固定长度
        /// </summary>
        public bool IsFixedLength;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
        ///// <summary>
        ///// 正则验证,不可用
        ///// </summary>
        //public string RegularVerify;
        ///// <summary>
        ///// 字符串最小长度验证,不可用
        ///// </summary>
        //public int MinLength;
        /// <summary>
        /// 字符串最大长度验证
        /// </summary>
        public int MaxLength;
        ///// <summary>
        ///// 最小数值验证,不可用
        ///// </summary>
        //public double MinValue = double.NaN;
        ///// <summary>
        ///// 是否验证最小数值,不可用
        ///// </summary>
        //public bool IsMinValue
        //{
        //    get { return !double.IsNaN(MinValue); }
        //}
        ///// <summary>
        ///// 最大数值验证,不可用
        ///// </summary>
        //public double MaxValue = double.NaN;
        ///// <summary>
        ///// 是否验证最大数值,不可用
        ///// </summary>
        //public bool IsMaxValue
        //{
        //    get { return !double.IsNaN(MaxValue); }
        //}
        ///// <summary>
        ///// 是否存在验证,不可用
        ///// </summary>
        //public bool IsVerify
        //{
        //    get { return (MinLength | MaxLength) != 0 || IsMinValue || IsMaxValue || RegularVerify != null; }
        //}
        /// <summary>
        /// 是否SQL成员信息空值
        /// </summary>
        internal bool IsDefaultSqlMember
        {
            get
            {
                return this == defaultSqlMember;
            }
        }

        /// <summary>
        /// 获取SQL成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <returns>SQL成员信息</returns>
        internal static sqlMember Get(memberInfo member)
        {
            sqlMember value = member.Member.customAttribute<sqlMember>(true);
            if (value == null || value.SqlType == null)
            {
                if (member.MemberType.Type.IsEnum)
                {
                    if (value == null) value = new sqlMember();
                    value.SqlType = System.Enum.GetUnderlyingType(member.MemberType.Type);
                }
                else
                {
                    sqlMember sqlMember = member.MemberType.Type.customAttribute<sqlMember>(false, false);
                    if (sqlMember != null && sqlMember.SqlType != null)
                    {
                        if (value == null) value = new sqlMember();
                        value.SqlType = sqlMember.SqlType;
                    }
                }
            }
            return value ?? defaultSqlMember;
        }
    }
}
