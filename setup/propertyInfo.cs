using System;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 属性信息
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal class propertyInfo<valueType> : memberInfo
    {
        /// <summary>
        /// 引用类型成员获取器
        /// </summary>
        public func<valueType, object> Getter;
        /// <summary>
        /// 引用类型成员设置器
        /// </summary>
        public action<valueType, object> Setter;
        /// <summary>
        /// 引用类型成员复制器
        /// </summary>
        public action<valueType, valueType> Copyer;
        /// <summary>
        /// 值类型获取属性函数
        /// </summary>
        private MethodInfo getMethod;
        /// <summary>
        /// 值类型获取属性
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <returns>属性值</returns>
        public object GetValue(object value)
        {
            return getMethod.Invoke(value, null);
        }
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>成员值</returns>
        public object GetValue(valueType value)
        {
            return getMethod.Invoke(value, null);
        }
        /// <summary>
        /// 值类型设置属性函数
        /// </summary>
        private MethodInfo setMethod;
        /// <summary>
        /// 值类型设置属性
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="values">属性值数组</param>
        public void SetValue(object value, object[] values)
        {
            setMethod.Invoke(value, values);
        }
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>成员值</returns>
        public void SetValue(valueType value, object memberValue)
        {
            setMethod.Invoke(value, new object[] { memberValue });
        }
        /// <summary>
        /// 属性信息
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        /// <param name="nonPublic">是否非公有属性</param>
        /// <param name="isStruct">目标类型是否结构体</param>
        public propertyInfo(PropertyInfo property, memberFilter filter, int index, bool nonPublic, bool isStruct)
            : base((MemberInfo)property, filter)
        {
            CanSet = property.CanWrite;
            CanGet = property.CanRead;
            MemberType = property.PropertyType;
            MemberIndex = index;
            if (isStruct)
            {
                getMethod = property.GetGetMethod(nonPublic);
                setMethod = property.GetSetMethod(nonPublic);
            }
            else
            {
                Getter = propertyGetter<valueType>.createPropertyTargetGetter.Default.Create(property, nonPublic);
                Setter = propertySetter<valueType>.createPropertyTargetSetter.Default.Create(property, nonPublic);
                Copyer = propertyCopyer<valueType>.Creator.Create(property, nonPublic);
            }
        }
    }
}
