using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 成员方法相关操作
    /// </summary>
    public static class methodInfo
    {
        /// <summary>
        /// 获取匹配的泛型定义方法
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="methods">待匹配方法集合</param>
        /// <param name="isMethod">方法匹配器</param>
        /// <returns>匹配的泛型定义方法,失败返回null</returns>
        public static MethodInfo getGenericDefinition<attributeType>(this MethodInfo[] methods, func<MethodInfo, bool> isMethod)
            where attributeType : Attribute
        {
            if (methods != null)
            {
                foreach (MethodInfo method in methods)
                {
                    if (method.IsGenericMethod && isMethod(method) && method.customAttribute<attributeType>() != null)
                    {
                        return method.IsGenericMethodDefinition ? method : method.GetGenericMethodDefinition();
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 成员方法全名
        /// </summary>
        /// <param name="method">成员方法</param>
        /// <returns>成员方法全名</returns>
        public static string fullName(this MethodInfo method)
        {
            return method != null ? method.DeclaringType.fullName() + "." + method.Name : null;
        }
        /// <summary>
        /// 判断成员方法参数是否匹配
        /// </summary>
        /// <param name="method">成员方法</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="parameters">参数类型集合</param>
        /// <returns>参数是否匹配</returns>
        public static bool isParameter(this MethodInfo method, Type returnType, params Type[] parameters)
        {
            if (method != null && method.ReturnType == returnType)
            {
                ParameterInfo[] methodParameters = method.GetParameters();
                if (methodParameters.Length == parameters.length())
                {
                    int index = 0;
                    foreach(ParameterInfo parameter in methodParameters)
                    {
                        if (parameter.ParameterType != parameters[index]) return false;
                        ++index;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
