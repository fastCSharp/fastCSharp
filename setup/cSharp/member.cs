using System;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 自定义属性字段模板生成基类
    /// </summary>
    /// <typeparam name="attributeType">自定义属性类型</typeparam>
    internal abstract class member<attributeType> : cSharper<attributeType>, IAuto
        where attributeType : memberFilter
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        protected memberMap.cSharp memberMap = new memberMap.cSharp();
        /// <summary>
        /// 成员复制
        /// </summary>
        protected copy copy = new copy();
        /// <summary>
        /// 成员集合
        /// </summary>
        internal setup.memberInfo[] Members;
        ///// <summary>
        ///// 成员集合数量
        ///// </summary>
        //public int MemberCount
        //{
        //    get { return Members.Length; }
        //}
        /// <summary>
        /// 获取SQL成员信息集合
        /// </summary>
        /// <returns>SQL成员信息集合</returns>
        protected setup.memberInfo[] getMembers()
        {
            return getMembers<attributeType>();
        }
        /// <summary>
        /// 获取SQL成员信息集合
        /// </summary>
        /// <typeparam name="memberAttributeType">成员自定义属性类型</typeparam>
        /// <returns>SQL成员信息集合</returns>
        protected setup.memberInfo[] getMembers<memberAttributeType>() where memberAttributeType : ignore
        {
            return setup.memberInfo.GetMembers<memberAttributeType>(type, Attribute.filter, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute, Attribute.IsInheritAttribute);
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected override void onCreated() { }
    }
    /// <summary>
    /// 成员自定义属性
    /// </summary>
    public class member : Attribute
    {
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type Type;
    }
}
