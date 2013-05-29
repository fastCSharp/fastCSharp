using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 属性设置器
    /// </summary>
    internal static class propertySetter
    {
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <param name="targetType">目标对象类型</param>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性设置器,失败返回null</returns>
        private static object create(PropertyInfo property, bool nonPublic, Type targetType)
        {
            MethodInfo method = property.GetSetMethod(nonPublic);
            return Activator.CreateInstance(typeof(propertySetter<,>).MakeGenericType(targetType, property.PropertyType), method);
        }
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性设置器,失败返回null</returns>
        public static object Create(PropertyInfo property, bool nonPublic)
        {
            return property != null && property.CanWrite ? create(property, nonPublic, property.DeclaringType) : null;
        }
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <typeparam name="targetType">目标对象类型</typeparam>
        /// <param name="property">属性信息</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <returns>属性设置器,失败返回null</returns>
        public static object Create<targetType>(PropertyInfo property, bool nonPublic)
        {
            return property != null && property.CanWrite ? create(property, nonPublic, typeof(targetType)) : null;
        }
    }
    /// <summary>
    /// 属性设置器
    /// </summary>
    /// <typeparam name="valueType">属性值类型</typeparam>
    public static class propertySetter<valueType>
    {
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public sealed class createPropertySetter : createProperty<action<object, valueType>>
        {
            /// <summary>
            /// 创建属性设置器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性设置器,失败返回null</returns>
            public override action<object, valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertySetter.Create(property, nonPublic);
                if (value != null) return ((IPropertySetter<valueType>)value).Set;
                return null;
            }
        }
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public static readonly createPropertySetter Creator = new createPropertySetter();
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public sealed class createPropertyTargetSetter : createProperty<action<valueType, object>>
        {
            /// <summary>
            /// 创建属性设置器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性设置器,失败返回null</returns>
            public override action<valueType, object> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertySetter.Create<valueType>(property, nonPublic);
                if (value != null) return ((IPropertyTargetSetter<valueType>)value).Set;
                return null;
            }
            /// <summary>
            /// 属性设置器 创建器
            /// </summary>
            public static readonly createPropertyTargetSetter Default = new createPropertyTargetSetter();
        }
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public sealed class createPropertySetter<targetType> : createProperty<action<targetType, valueType>>
        {
            /// <summary>
            /// 创建属性设置器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性设置器,失败返回null</returns>
            public override action<targetType, valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                object value = propertySetter.Create<targetType>(property, nonPublic);
                if (value != null) return ((propertySetter<targetType, valueType>)value).Setter;
                return null;
            }
            /// <summary>
            /// 属性设置器 创建器
            /// </summary>
            public static readonly createPropertySetter<targetType> Default = new createPropertySetter<targetType>();
        }
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public sealed class createPropertyStaticSetter : createProperty<action<valueType>>
        {
            /// <summary>
            /// 创建属性设置器
            /// </summary>
            /// <param name="property">属性信息</param>
            /// <param name="nonPublic">是否非公有属性</param>
            /// <returns>属性设置器,失败返回null</returns>
            public override action<valueType> Create(PropertyInfo property, bool nonPublic = true)
            {
                if (property != null && property.CanWrite)
                {
                    return (action<valueType>)Delegate.CreateDelegate(typeof(action<valueType>), property.GetGetMethod(nonPublic));
                }
                return null;
            }
        }
        /// <summary>
        /// 属性设置器 创建器
        /// </summary>
        public static readonly createPropertyStaticSetter StaticCreator = new createPropertyStaticSetter();
    }
    /// <summary>
    /// 属性设置器接口
    /// </summary>
    /// <typeparam name="valueType">属性值类型</typeparam>
    internal interface IPropertySetter<valueType>
    {
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="value">属性值</param>
        /// <returns>属性值</returns>
        void Set(object instance, valueType value);
    }
    /// <summary>
    /// 属性设置器接口
    /// </summary>
    /// <typeparam name="targetType">属性值类型</typeparam>
    internal interface IPropertyTargetSetter<targetType>
    {
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="value">属性值</param>
        /// <returns>属性值</returns>
        void Set(targetType instance, object value);
    }
    /// <summary>
    /// 属性设置器
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    /// <typeparam name="valueType">属性值类型</typeparam>
    internal struct propertySetter<targetType, valueType> : IPropertyTargetSetter<targetType>, IPropertySetter<valueType>
    {
        /// <summary>
        /// 属性获取器
        /// </summary>
        public action<targetType, valueType> Setter;
        /// <summary>
        /// 属性获取器
        /// </summary>
        /// <param name="method">属性方法</param>
        public propertySetter(MethodInfo method)
        {
            Setter = (action<targetType, valueType>)Delegate.CreateDelegate(typeof(action<targetType, valueType>), null, method);
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="value">属性值</param>
        /// <returns>属性值</returns>
        public void Set(object instance, valueType value)
        {
            Setter((targetType)instance, value);
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="value">属性值</param>
        /// <returns>属性值</returns>
        public void Set(targetType instance, object value)
        {
            Setter(instance, (valueType)value);
        }
    }
}
