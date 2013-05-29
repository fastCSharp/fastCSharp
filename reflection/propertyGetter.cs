using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 属性获取器
    /// </summary>
    internal static class propertyGetter
    {
        /// <summary>
        /// 创建属性获取器
        /// </summary>
        /// <param name="targetType">目标对象类型</param>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性获取器,失败返回null</returns>
        private static object create(PropertyInfo property, bool nonPublic, Type targetType)
        {
            MethodInfo method = property.GetGetMethod(nonPublic);
            return Activator.CreateInstance(typeof(propertyGetter<,>).MakeGenericType(targetType, property.PropertyType), method);
        }
        /// <summary>
        /// 创建属性获取器
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性获取器,失败返回null</returns>
        public static object Create(PropertyInfo property, bool nonPublic)
        {
            return property != null && property.CanRead ? create(property, nonPublic, property.DeclaringType) : null;
        }
        /// <summary>
        /// 创建属性获取器
        /// </summary>
        /// <typeparam name="targetType">目标对象类型</typeparam>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性获取器,失败返回null</returns>
        public static object Create<targetType>(PropertyInfo property, bool nonPublic)
        {
            return property != null && property.CanRead ? create(property, nonPublic, typeof(targetType)) : null;
        }
    }
    /// <summary>
    /// 属性获取器
    /// </summary>
    /// <typeparam name="valueType">属性值类型</typeparam>
    public static class propertyGetter<valueType>
    {
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        public sealed class createPropertyGetter : createProperty<func<object, valueType>>
        {
            /// <summary>
            /// 创建属性获取器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性获取器,失败返回null</returns>
            public override func<object, valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertyGetter.Create(property, nonPublic);
                if (value != null) return ((IPropertyGetter<valueType>)value).Get;
                return null;
            }
        }
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        public static readonly createPropertyGetter Creator = new createPropertyGetter();
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        public sealed class createPropertyTargetGetter : createProperty<func<valueType, object>>
        {
            /// <summary>
            /// 创建属性获取器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性获取器,失败返回null</returns>
            public override func<valueType, object> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertyGetter.Create<valueType>(property, nonPublic);
                if (value != null) return ((IPropertyTargetGetter<valueType>)value).Get;
                return null;
            }
            /// <summary>
            /// 属性获取器 创建器
            /// </summary>
            public static readonly createPropertyTargetGetter Default = new createPropertyTargetGetter();
        }
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        /// <typeparam name="targetType">目标对象类型</typeparam>
        public sealed class createPropertyGetter<targetType> : createProperty<func<targetType, valueType>>
        {
            /// <summary>
            /// 创建属性获取器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性获取器,失败返回null</returns>
            public override func<targetType, valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertyGetter.Create<targetType>(property, nonPublic);
                if (value != null) return ((propertyGetter<targetType, valueType>)value).Getter;
                return null;
            }
            /// <summary>
            /// 属性获取器 创建器
            /// </summary>
            public static readonly createPropertyGetter<targetType> Default = new createPropertyGetter<targetType>();
        }
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        public sealed class createPropertyStaticGetter : createProperty<func<valueType>>
        {
            /// <summary>
            /// 创建属性获取器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性获取器,失败返回null</returns>
            public override func<valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                if (property != null && property.CanRead)
                {
                    return (func<valueType>)Delegate.CreateDelegate(typeof(func<valueType>), property.GetGetMethod(nonPublic));
                }
                return null;
            }
        }
        /// <summary>
        /// 属性获取器 创建器
        /// </summary>
        public static readonly createPropertyStaticGetter StaticCreator = new createPropertyStaticGetter();
    }
    /// <summary>
    /// 属性获取器接口
    /// </summary>
    /// <typeparam name="valueType">属性值类型</typeparam>
    internal interface IPropertyGetter<valueType>
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <returns>属性值</returns>
        valueType Get(object value);
    }
    /// <summary>
    /// 属性获取器接口
    /// </summary>
    /// <typeparam name="targetType">属性值类型</typeparam>
    internal interface IPropertyTargetGetter<targetType>
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <returns>属性值</returns>
        object Get(targetType value);
    }
    /// <summary>
    /// 属性获取器
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    /// <typeparam name="valueType">属性值类型</typeparam>
    internal struct propertyGetter<targetType, valueType> : IPropertyTargetGetter<targetType>, IPropertyGetter<valueType>
    {
        /// <summary>
        /// 属性获取器
        /// </summary>
        public func<targetType, valueType> Getter;
        /// <summary>
        /// 属性获取器
        /// </summary>
        /// <param name="method">属性方法</param>
        public propertyGetter(MethodInfo method)
        {
            Getter = (func<targetType, valueType>)Delegate.CreateDelegate(typeof(func<targetType, valueType>), null, method);
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <returns>属性值</returns>
        public valueType Get(object value)
        {
            return Getter((targetType)value);
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <returns>属性值</returns>
        public object Get(targetType value)
        {
            return Getter(value);
        }
    }
}
