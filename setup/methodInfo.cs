using System;
using System.Collections.Generic;
using System.Reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 成员方法
    /// </summary>
    internal class methodInfo : memberInfo
    {
        /// <summary>
        /// 成员方法信息
        /// </summary>
        public MethodInfo Method;
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName
        {
            get { return Method.Name; }
        }
        /// <summary>
        /// 方法泛型名称
        /// </summary>
        public string MethodGenericName
        {
            get
            {
                return MethodName + GenericParameterName;
            }
        }
        /// <summary>
        /// 方法泛型名称
        /// </summary>
        public string StaticMethodGenericName
        {
            get { return MethodGenericName; }
        }
        /// <summary>
        /// 方法全称标识
        /// </summary>
        public string MethodKeyFullName
        {
            get
            {
                return Method.DeclaringType.FullName + MethodKeyName;
            }
        }
        /// <summary>
        /// 方法标识
        /// </summary>
        public string MethodKeyName
        {
            get
            {
                return "(" + Parameters.joinString(',', value => value.ParameterRef + value.ParameterType.FullName) + ")" + GenericParameterName + MethodName;
            }
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public memberType ReturnType;
        /// <summary>
        /// 是否有返回值
        /// </summary>
        public bool IsReturn
        {
            get
            {
                return ReturnType.Type != typeof(void);
            }
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        public parameterInfo[] Parameters;
        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        public memberType[] GenericParameters;
        /// <summary>
        /// 泛型参数拼写
        /// </summary>
        private string genericParameterName;
        /// <summary>
        /// 泛型参数拼写
        /// </summary>
        public string GenericParameterName
        {
            get
            {
                if (genericParameterName == null)
                {
                    memberType[] genericParameters = GenericParameters;
                    genericParameterName = genericParameters.Length == 0 ? string.Empty : ("<" + genericParameters.joinString(',', value => value.FullName) + ">");
                }
                return genericParameterName;
            }
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        public parameterInfo[] OutputParameters;
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        public methodInfo(MethodInfo method, memberFilter filter)
            : base(method, filter)
        {
            Method = method;
            ReturnType = Method.ReturnType;
            Parameters = parameterInfo.Get(method);
            OutputParameters = Parameters.getFindArray(value => value.Parameter.IsOut || value.Parameter.ParameterType.IsByRef);
            GenericParameters = method.GetGenericArguments().getArray(value => (memberType)value);
        }

        /// <summary>
        /// 类型成员方法缓存
        /// </summary>
        private static readonly Dictionary<hashCode<Type>, methodInfo[]> methodCache = new Dictionary<hashCode<Type>, methodInfo[]>();
        /// <summary>
        /// 获取类型的成员方法集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员方法集合</returns>
        private static methodInfo[] getMethods(Type type)
        {
            methodInfo[] methods;
            hashCode<Type> hashType = type;
            if (!methodCache.TryGetValue(hashType, out methods))
            {
                int index = 0;
                methodCache[hashType] = methods = array.concat(
                    type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).getArray(value => new methodInfo(value, memberFilter.PublicStatic)),
                    type.GetMethods(BindingFlags.Public | BindingFlags.Instance).getArray(value => new methodInfo(value, memberFilter.PublicInstance)),
                    type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).getArray(value => new methodInfo(value, memberFilter.NonPublicStatic)),
                    type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).getArray(value => new methodInfo(value, memberFilter.NonPublicInstance)))
                    .each(value => value.MemberIndex = index++);
            }
            return methods;
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <returns>匹配的成员方法集合</returns>
        public static list<methodInfo> GetMethods(Type type, memberFilter filter, bool isFilter)
        {
            return getMethods(type).getFind(value => isFilter ? (value.Filter & filter) == filter : ((value.Filter & filter) != 0));
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="methods">成员方法集合</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类自定义属性</param>
        /// <param name="isInheritAttribute">自定义属性类型是否可继承</param>
        /// <returns>匹配成员方法集合</returns>
        private static methodInfo[] getMethods<attributeType>
            (Type type, list<methodInfo> methods, bool isAttribute, bool isBaseType, bool isInheritAttribute)
            where attributeType : ignore
        {
            if (isAttribute)
            {
                return methods.getFindArray(value => value.IsAttribute<attributeType>(isBaseType, isInheritAttribute));
            }
            else
            {
                return methods.getFindArray(value => value.Method.DeclaringType == type && !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute));
            }
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类自定义属性</param>
        /// <param name="isInheritAttribute">自定义属性类型是否可继承</param>
        /// <returns>匹配成员方法集合</returns>
        public static methodInfo[] GetMethods<attributeType>(Type type, memberFilter filter, bool isFilter, bool isAttribute, bool isBaseType, bool isInheritAttribute)
            where attributeType : ignore
        {
            return getMethods<attributeType>(type, GetMethods(type, filter, isFilter), isAttribute, isBaseType, isInheritAttribute);
        }
    }
}
