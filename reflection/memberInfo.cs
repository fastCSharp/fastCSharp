using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 成员属性相关操作
    /// </summary>
    public static class memberInfo
    {
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="member">成员属性</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <returns>自定义属性</returns>
        public static attributeType customAttribute<attributeType>
            (this MemberInfo member, bool isBaseType = false, bool isInheritAttribute = false)
            where attributeType : Attribute
        {
            attributeType value = null;
            if (member != null)
            {
                int minDepth = int.MaxValue;
                foreach (object attribute in member.GetCustomAttributes(typeof(attributeType), isBaseType))
                {
                    if (isInheritAttribute)
                    {
                        if (attribute is attributeType)
                        {
                            int depth = 0;
                            for (Type type = attribute.GetType(); type != typeof(attributeType); type = type.BaseType) ++depth;
                            if (depth < minDepth)
                            {
                                minDepth = depth;
                                value = attribute as attributeType;
                            }
                        }
                        if (minDepth == 0) break;
                    }
                    else if (attribute.GetType() == typeof(attributeType)) return attribute as attributeType;
                }
            }
            return value;
        }
        /// <summary>
        /// 根据成员属性获取自定义属性集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="member">成员属性</param>
        /// <param name="isInherit">是否包含继承属性</param>
        /// <returns>自定义属性集合</returns>
        public static list<attributeType> customAttributes<attributeType>
            (this MemberInfo member, bool isInherit) where attributeType : Attribute
        {
            if (member != null)
            {
                attributeType value;
                list<attributeType> values = new list<attributeType>();
                foreach (object attribute in member.GetCustomAttributes(typeof(attributeType), isInherit))
                {
                    value = attribute as attributeType;
                    if (value != null) values.Add(value);
                }
                return values;
            }
            return null;
        }
    }
}
