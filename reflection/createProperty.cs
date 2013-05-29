using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 属性创建器
    /// </summary>
    /// <typeparam name="valueType">创建目标类型</typeparam>
    public abstract class createProperty<valueType>
    {
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>目标,失败返回null</returns>
        public abstract valueType Create(PropertyInfo property, bool nonPublic = true);
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <param name="name">属性名称</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>目标,失败返回null</returns>
        public valueType Create(Type type, string name, bool nonPublic = true)
        {
            if (type != null && name != null)
            {
                return Create(type.GetProperty(name, BindingFlags.Instance | (nonPublic ? BindingFlags.NonPublic : BindingFlags.Public)), nonPublic);
            }
            return default(valueType);
        }
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="typeName">类型全名</param>
        /// <param name="name">属性名称</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>目标,失败返回null</returns>
        public valueType Create(Assembly assembly, string typeName, string name, bool nonPublic = true)
        {
            return assembly != null && typeName != null ? Create(assembly.GetType(typeName), name, nonPublic) : default(valueType);
        }
    }
}
