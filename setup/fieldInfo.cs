using System;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 字段信息
    /// </summary>
    internal class fieldInfo : memberInfo
    {
        /// <summary>
        /// 成员获取器
        /// </summary>
        public func<object, object> Getter;
        /// <summary>
        /// 成员设置器
        /// </summary>
        public action<object, object> Setter;
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        public fieldInfo(FieldInfo field, memberFilter filter, int index)
            : base((MemberInfo)field, filter)
        {
            IsField = CanGet = true;
            CanSet = !field.IsInitOnly;
            MemberIndex = index;
            MemberType = field.FieldType;
            Getter = fieldGetValue.Creator.Create(field);
            Setter = fieldSetValue.Creator.Create(field);
        }
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>成员值</returns>
        public object GetValue<valueType>(valueType value)
        {
            return Getter(value);
        }
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="memberValue">成员值</param>
        public void SetValue<valueType>(valueType value, object memberValue)
        {
            Setter(value, memberValue);
        }
    }
}
