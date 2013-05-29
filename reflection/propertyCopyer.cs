using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 属性复制器
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    public sealed class propertyCopyer<targetType> : createProperty<action<targetType, targetType>>
    {
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性设置器,失败返回null</returns>
        public override action<targetType, targetType> Create(PropertyInfo property, bool nonPublic = true)
        {
            if (property != null && property.CanRead && property.CanWrite)
            {
                return ((IPropertyCopyer<targetType>)Activator.CreateInstance(typeof(propertyCopyer<,>).MakeGenericType(typeof(targetType), property.PropertyType), new object[] { property, nonPublic })).Copy;
            }
            return null;
        }
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public static readonly propertyCopyer<targetType> Creator = new propertyCopyer<targetType>();
    }
    /// <summary>
    /// 属性复制器
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    internal interface IPropertyCopyer<targetType>
    {
        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="copyValue">被复制对象</param>
        void Copy(targetType value, targetType copyValue);
    }
    /// <summary>
    /// 属性复制器
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    /// <typeparam name="valueType">属性值类型</typeparam>
    internal struct propertyCopyer<targetType, valueType> : IPropertyCopyer<targetType>
    {
        /// <summary>
        /// 属性获取器
        /// </summary>
        public func<targetType, valueType> Getter;
        /// <summary>
        /// 属性设置器
        /// </summary>
        public action<targetType, valueType> Setter;
        /// <summary>
        /// 属性获取器
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        public propertyCopyer(PropertyInfo property, bool nonPublic)
        {
            Getter = (func<targetType, valueType>)Delegate.CreateDelegate(typeof(func<targetType, valueType>), null, property.GetGetMethod(nonPublic));
            Setter = (action<targetType, valueType>)Delegate.CreateDelegate(typeof(action<targetType, valueType>), null, property.GetSetMethod(nonPublic));
        }
        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="copyValue">被复制对象</param>
        public void Copy(targetType value, targetType copyValue)
        {
            Setter(value, Getter(copyValue));
        }
    }
}
