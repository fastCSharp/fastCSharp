using System;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 成员选择
    /// </summary>
    public abstract class memberFilter : ignore
    {
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public abstract setup.memberFilter filter { get; }
        /// <summary>
        /// 成员是否匹配自定义属性类型
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// 是否搜索父类自定义属性
        /// </summary>
        public bool IsBaseTypeAttribute;
        /// <summary>
        /// 成员匹配自定义属性是否可继承
        /// </summary>
        public bool IsInheritAttribute = true;
        /// <summary>
        /// 默认公有动态成员
        /// </summary>
        public abstract class publicInstance : memberFilter
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public setup.memberFilter Filter = setup.memberFilter.PublicInstance;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override setup.memberFilter filter
            {
                get { return Filter & setup.memberFilter.Instance; }
            }
        }
        /// <summary>
        /// 默认公有动态字段成员
        /// </summary>
        public abstract class publicInstanceField : memberFilter
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public setup.memberFilter Filter = setup.memberFilter.PublicInstanceField;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override setup.memberFilter filter
            {
                get { return Filter & setup.memberFilter.Instance; }
            }
        }
    }
}
