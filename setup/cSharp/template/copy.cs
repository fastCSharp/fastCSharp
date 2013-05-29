using System;

namespace fastCSharp.setup.cSharp.template
{
    class copy : pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : fastCSharp.setup.cSharp.ICopy<@type.FullName, @type.FullName/**/.memberMap>
        {
            /*NOTE*/
            object MemberName;/*NOTE*/
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="value">被复制对象</param>
            /// <param name="memberMap">复制成员位图</param>
            public void CopyFrom(@type.FullName value, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
            {
                if (memberMap.IsDefault)
                {
                    #region LOOP Members
                    this.@MemberName = value.@MemberName;
                    #endregion LOOP Members
                }
                else
                {
                    #region LOOP Members
                    if (memberMap.IsMember(@MemberIndex)) this.@MemberName = value.@MemberName;
                    #endregion LOOP Members
                }
            }
            /// <summary>
            /// 浅复制对象
            /// </summary>
            /// <returns>复制的对象</returns>
            public @type.FullName CopyMember()
            {
                return (@type.FullName)MemberwiseClone();
            }
            #region IF IsConstructor
            /// <summary>
            /// 无参数构造函数调用
            /// </summary>
            private static @type.FullName @ConstructorName
            {
                get
                {
                    return new @type.FullName();
                }
            }
            #endregion IF IsConstructor
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : ICopy<FullName, memberMap>
        {
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="value">被复制对象</param>
            public void CopyFrom(FullName value, memberMap memberMap = null) { }
            /// <summary>
            /// 浅复制对象
            /// </summary>
            /// <returns>复制的对象</returns>
            public FullName CopyMember() { return null; }
        }
    }
    #endregion NOTE
}
