using System;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;
using System.Runtime.InteropServices;

namespace fastCSharp.setup
{
    /// <summary>
    /// 成员信息
    /// </summary>
    internal partial class memberInfo
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public MemberInfo Member;
        /// <summary>
        /// 成员类型
        /// </summary>
        public memberType MemberType;
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName;
        /// <summary>
        /// 选择类型
        /// </summary>
        public memberFilter Filter;
        /// <summary>
        /// 是否字段
        /// </summary>
        public bool IsField;
        /// <summary>
        /// 成员编号
        /// </summary>
        public int MemberIndex;
        /// <summary>
        /// 是否忽略该成员
        /// </summary>
        public bool IsIgnore;
        /// <summary>
        /// 是否可赋值
        /// </summary>
        public bool CanSet;
        /// <summary>
        /// 是否可读取
        /// </summary>
        public bool CanGet;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员名称</param>
        /// <param name="index">成员编号</param>
        public memberInfo(memberType type, string name, int index)
        {
            MemberType = type;
            MemberName = name;
            MemberIndex = index;
            IsField = CanSet = CanSet = true;
            Filter = memberFilter.PublicInstance;
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="filter">选择类型</param>
        protected memberInfo(MemberInfo member, memberFilter filter)
        {
            Member = member;
            MemberName = member.Name;
            Filter = filter;
            IsIgnore = member.customAttributes<ignore>(true).Any(value => value.IsIgnore);
            if (CanGet && CanSet)
            {
                sqlMember sqlMember = member.customAttribute<sqlMember>(true);
                if (sqlMember != null && sqlMember.SqlType != null) MemberType = new memberType(MemberType, sqlMember.SqlType);
            }
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">字段</param>
        /// <param name="filter">选择类型</param>
        private memberInfo(FieldInfo member, memberFilter filter)
            : this((MemberInfo)member, filter)
        {
            IsField = CanGet = true;
            CanSet = !member.IsInitOnly;
            sqlMember sqlMember = member.customAttribute<sqlMember>(true);
            if (sqlMember == null || sqlMember.SqlType == null) MemberType = member.FieldType;
            else MemberType = new memberType(member.FieldType, sqlMember.SqlType);
        }
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="member">属性</param>
        /// <param name="filter">选择类型</param>
        private memberInfo(PropertyInfo member, memberFilter filter)
            : this((MemberInfo)member, filter)
        {
            MemberType = member.PropertyType;
            CanSet = member.CanWrite;
            CanGet = member.CanRead;
        }
        /// <summary>
        /// 判断是否存在自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>是否存在自定义属性</returns>
        public bool IsAttribute<attributeType>(bool isBaseType, bool isInheritAttribute) where attributeType : fastCSharp.setup.ignore
        {
            return GetAttribute<attributeType>(isBaseType, isInheritAttribute) != null;
        }
        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>自定义属性,失败返回null</returns>
        public attributeType GetAttribute<attributeType>(bool isBaseType, bool isInheritAttribute) where attributeType : fastCSharp.setup.ignore
        {
            if (!IsIgnore)
            {
                attributeType value = Member.customAttribute<attributeType>(isBaseType, isInheritAttribute);
                if (value != null && value.IsSetup) return value;
            }
            return null;
        }
        /// <summary>
        /// 判断是否忽略自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>是否忽略自定义属性</returns>
        public bool IsIgnoreAttribute<attributeType>(bool isBaseType, bool isInheritAttribute) where attributeType : fastCSharp.setup.ignore
        {
            if (IsIgnore) return true;
            attributeType value = Member.customAttribute<attributeType>(isBaseType, isInheritAttribute);
            return value != null && !value.IsSetup;
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="members">待匹配的成员信息集合</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>成员信息集合</returns>
        private static memberInfo[] getAttribute<attributeType>(memberInfo[] members, bool isAttribute, bool isBaseType, bool isInheritAttribute)
            where attributeType : fastCSharp.setup.ignore
        {
            return members.getFindArray(value => isAttribute ? value.IsAttribute<attributeType>(isBaseType, isInheritAttribute) : !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute));
        }
        /// <summary>
        /// 类型成员集合缓存
        /// </summary>
        private static readonly Dictionary<hashCode<Type>, memberInfo[]> memberCache = new Dictionary<hashCode<Type>, memberInfo[]>();
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员信息集合</returns>
        private static memberInfo[] getMembers(Type type)
        {
            memberInfo[] members;
            hashCode<Type> hashType = type;
            if (!memberCache.TryGetValue(hashType, out members))
            {
                if (type.IsEnum)
                {
                    memberCache[hashType] = members = type.GetFields(BindingFlags.Public | BindingFlags.Static).getArray(value => new memberInfo(value, memberFilter.PublicStaticField));
                }
                else
                {
                    memberGroup memberGroup = new memberGroup(type);
                    memberCache[hashType] = members =
                        array.concat(memberGroup.PublicFields.getArray(value => new memberInfo(value, memberFilter.PublicInstanceField)),
                            memberGroup.NonPublicFields.getArray(value => new memberInfo(value, memberFilter.NonPublicInstanceField)),
                            memberGroup.PublicProperties.getArray(value => new memberInfo(value, memberFilter.PublicInstanceProperty)),
                            memberGroup.NonPublicProperties.getArray(value => new memberInfo(value, memberFilter.NonPublicInstanceProperty)));
                    int memberIndex = 0;
                    foreach (memberInfo member in members) member.MemberIndex = memberIndex++;
                }
            }
            return members;
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="filter">选择类型</param>
        /// <returns>成员信息集合</returns>
        public static memberInfo[] GetMembers(Type type, memberFilter filter)
        {
            return getMembers(type).getFindArray(value => (value.Filter & filter) != 0);
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="filter">选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>成员信息集合</returns>
        public static memberInfo[] GetMembers<attributeType>(Type type, memberFilter filter, bool isAttribute, bool isBaseType, bool isInheritAttribute)
            where attributeType : fastCSharp.setup.ignore
        {
            return getAttribute<attributeType>(GetMembers(type, filter), isAttribute, isBaseType, isInheritAttribute);
        }
    }
}
