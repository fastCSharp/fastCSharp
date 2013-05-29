using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 字段获取器
    /// </summary>
    public static class fieldGetValue
    {
        /// <summary>
        /// 域初始化属性获取器
        /// </summary>
        internal static readonly func<object, bool> getDomainInitialized = propertyGetter<bool>.Creator.Create(typeof(RuntimeFieldHandle).Assembly, "System.RuntimeType", "DomainInitialized");
        /// <summary>
        /// 字段值获取方法信息
        /// </summary>
        private static readonly MethodInfo runtimeFieldHandleGetValue = typeof(RuntimeFieldHandle).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// .NET4.0字段值获取方法信息
        /// </summary>
        private static readonly MethodInfo runtimeFieldHandleGetValue4 = typeof(RuntimeFieldHandle).GetMethod("GetValue", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 字段值获取委托
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="fieldType">字段值类型</param>
        /// <param name="declaringType">定义字段的类型</param>
        /// <param name="domainInitialized">域初始化状态</param>
        /// <returns>字段值</returns>
        private delegate object getValueDelegate(object instance, RuntimeTypeHandle fieldType, RuntimeTypeHandle declaringType, ref bool domainInitialized);
        /// <summary>
        /// 字段获取器接口
        /// </summary>
        private interface IFieldGetValue
        {
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            object GetObject(object instance);
        }
        /// <summary>
        /// .NET4.0字段获取器接口
        /// </summary>
        /// <typeparam name="valueType">字段值类型</typeparam>
        private interface IFieldGetValue<valueType> : IFieldGetValue
        {
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            valueType GetValue(object instance);
        }
        /// <summary>
        /// 字段获取器
        /// </summary>
        /// <typeparam name="valueType">字段值类型</typeparam>
        private struct getter<valueType> : IFieldGetValue
        {
            /// <summary>
            /// 字段值获取委托
            /// </summary>
            private getValueDelegate getValueDelegate;
            /// <summary>
            /// 字段值类型
            /// </summary>
            private RuntimeTypeHandle fieldType;
            /// <summary>
            /// 定义字段的类型
            /// </summary>
            private RuntimeTypeHandle declaringType;
            /// <summary>
            /// 域初始化状态
            /// </summary>
            private bool domainInitialized;
            /// <summary>
            /// 字段获取器
            /// </summary>
            /// <param name="field">字段信息</param>
            public getter(FieldInfo field)
            {
                getValueDelegate = (fieldGetValue.getValueDelegate)Delegate.CreateDelegate(typeof(fieldGetValue.getValueDelegate), field.FieldHandle, fieldGetValue.runtimeFieldHandleGetValue);
                fieldType = field.FieldType.TypeHandle;
                declaringType = field.DeclaringType.TypeHandle;
                domainInitialized = fieldGetValue.getDomainInitialized(field.DeclaringType);
            }
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            public valueType GetValue(object instance)
            {
                return (valueType)GetObject(instance);
            }
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            public object GetObject(object instance)
            {
                return getValueDelegate(instance, fieldType, declaringType, ref domainInitialized);
            }
        }
        /// <summary>
        /// .NET4.0字段获取器
        /// </summary>
        /// <typeparam name="valueType">字段值类型</typeparam>
        /// <typeparam name="RtFieldInfoType">字段类型</typeparam>
        /// <typeparam name="RuntimeType">运行时类型</typeparam>
        private struct getter<valueType, RtFieldInfoType, RuntimeType> : IFieldGetValue<valueType>
            where RtFieldInfoType : FieldInfo
            where RuntimeType : Type
        {
            /// <summary>
            /// 字段值获取委托
            /// </summary>
            /// <param name="field">字段信息</param>
            /// <param name="instance">目标对象</param>
            /// <param name="fieldType">字段值类型</param>
            /// <param name="declaringType">定义字段的类型</param>
            /// <param name="domainInitialized">域初始化状态</param>
            /// <returns>字段值</returns>
            private delegate object getValue(RtFieldInfoType field, object instance, RuntimeType fieldType, RuntimeType declaringType, ref bool domainInitialized);
            /// <summary>
            /// 字段值获取委托
            /// </summary>
            private getValue getValueDelegate;
            /// <summary>
            /// 字段信息
            /// </summary>
            private RtFieldInfoType field;
            /// <summary>
            /// 字段值类型
            /// </summary>
            private RuntimeType fieldType;
            /// <summary>
            /// 定义字段的类型
            /// </summary>
            private RuntimeType declaringType;
            /// <summary>
            /// 域初始化状态
            /// </summary>
            private bool domainInitialized;
            /// <summary>
            /// 字段获取器
            /// </summary>
            /// <param name="field">字段信息</param>
            public getter(FieldInfo field)
            {
                getValueDelegate = (getValue)Delegate.CreateDelegate(typeof(getValue), fieldGetValue.runtimeFieldHandleGetValue4);
                this.field = (RtFieldInfoType)field;
                fieldType = (RuntimeType)field.FieldType;
                declaringType = (RuntimeType)field.DeclaringType;
                domainInitialized = fieldGetValue.getDomainInitialized(field.DeclaringType);
            }
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            public valueType GetValue(object instance)
            {
                return (valueType)GetObject(instance);
            }
            /// <summary>
            /// 获取字段值
            /// </summary>
            /// <param name="instance">目标对象</param>
            /// <returns>字段值</returns>
            public object GetObject(object instance)
            {
                return getValueDelegate(field, instance, fieldType, declaringType, ref domainInitialized);
            }
        }
        /// <summary>
        /// 字段获取器 创建器
        /// </summary>
        public sealed class createFieldGetValue : createField<func<object, object>>
        {
            /// <summary>
            /// 创建字段获取器
            /// </summary>
            /// <typeparam name="valueType">返回值类型</typeparam>
            /// <param name="field">字段信息</param>
            /// <returns>字段获取器</returns>
            public override func<object, object> Create(FieldInfo field)
            {
                if (field != null)
                {
                    if (runtimeFieldHandleGetValue != null) return ((IFieldGetValue)Activator.CreateInstance(typeof(getter<>).MakeGenericType(field.FieldType), field)).GetObject;
                    return ((IFieldGetValue)Activator.CreateInstance(typeof(getter<,,>).MakeGenericType(field.FieldType, fieldInfo.RtFieldInfoType, type.RuntimeType), field)).GetObject;
                }
                return null;
            }
        }
        /// <summary>
        /// 字段获取器 创建器
        /// </summary>
        public static readonly createFieldGetValue Creator = new createFieldGetValue();
        /// <summary>
        /// 字段获取器 创建器
        /// </summary>
        public sealed class createFieldGetValue<valueType> : createField<func<object, valueType>>
        {
            /// <summary>
            /// 创建字段获取器
            /// </summary>
            /// <typeparam name="valueType">返回值类型</typeparam>
            /// <param name="field">字段信息</param>
            /// <returns>字段获取器</returns>
            public override func<object, valueType> Create(FieldInfo field)
            {
                if (field != null)
                {
                    if (runtimeFieldHandleGetValue != null) return new getter<valueType>(field).GetValue;
                    return ((IFieldGetValue<valueType>)Activator.CreateInstance(typeof(getter<,,>).MakeGenericType(field.FieldType, fieldInfo.RtFieldInfoType, type.RuntimeType), field)).GetValue;
                }
                return null;
            }
            /// <summary>
            /// 字段获取器 创建器
            /// </summary>
            public static readonly createFieldGetValue<valueType> Default = new createFieldGetValue<valueType>();
        }
    }
}
