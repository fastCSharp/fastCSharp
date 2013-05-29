using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 字段创建器
    /// </summary>
    /// <typeparam name="valueType">创建目标类型</typeparam>
    public abstract class createField<valueType>
    {
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <returns>目标</returns>
        public abstract valueType Create(FieldInfo field);
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <param name="name">字段名称</param>
        /// <param name="nonPublic">是否非公共字段</param>
        /// <returns>目标</returns>
        public valueType Create(Type type, string name, bool nonPublic = true)
        {
            if (type != null && name != null)
            {
                return Create(type.GetField(name, BindingFlags.Instance | (nonPublic ? BindingFlags.NonPublic : BindingFlags.Public)));
            }
            return default(valueType);
        }
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="assembly">程序及信息</param>
        /// <param name="typeName">类型全名</param>
        /// <param name="name">字段名称</param>
        /// <param name="nonPublic">是否非公共字段</param>
        /// <returns>目标</returns>
        public valueType Create(Assembly assembly, string typeName, string name, bool nonPublic = true)
        {
            return assembly != null && typeName != null ? Create(assembly.GetType(typeName), name, nonPublic) : default(valueType);
        }
    }
    /// <summary>
    /// 字段创建器
    /// </summary>
    /// <typeparam name="valueType">创建目标类型</typeparam>
    /// <typeparam name="parameterType">参数类型</typeparam>
    public abstract class createField<valueType, parameterType>
    {
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <returns>目标</returns>
        public abstract valueType Create(FieldInfo field);
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <param name="name">字段名称</param>
        /// <param name="nonPublic">是否非公共字段</param>
        /// <returns>目标</returns>
        public valueType Create(Type type, string name, bool nonPublic = true)
        {
            if (type != null && name != null)
            {
                return Create(type.GetField(name, BindingFlags.Instance | (nonPublic ? BindingFlags.NonPublic : BindingFlags.Public)));
            }
            return default(valueType);
        }
        /// <summary>
        /// 创建目标
        /// </summary>
        /// <param name="assembly">程序及信息</param>
        /// <param name="typeName">类型全名</param>
        /// <param name="name">字段名称</param>
        /// <param name="nonPublic">是否非公共字段</param>
        /// <returns>目标</returns>
        public valueType Create(Assembly assembly, string typeName, string name, bool nonPublic = true)
        {
            return assembly != null && typeName != null ? Create(assembly.GetType(typeName), name, nonPublic) : default(valueType);
        }
    }
}
