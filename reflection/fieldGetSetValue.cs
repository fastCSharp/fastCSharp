using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 字段获取&设置器
    /// </summary>
    public struct fieldGetSetValue
    {
        /// <summary>
        /// 字段获取器
        /// </summary>
        public func<object, object> Getter;
        /// <summary>
        /// 字段获取器
        /// </summary>
        public action<object, object> Setter;
        /// <summary>
        /// 字段获取&设置器 创建器
        /// </summary>
        public sealed class createFieldGetSetValue : createField<fieldGetSetValue>
        {
            /// <summary>
            /// 创建字段获取&设置器
            /// </summary>
            /// <param name="field">字段信息</param>
            /// <returns>字段获取&设置器</returns>
            public override fieldGetSetValue Create(FieldInfo field)
            {
                if (field != null)
                {
                    return new fieldGetSetValue { Getter = fieldGetValue.Creator.Create(field), Setter = fieldSetValue.Creator.Create(field) };
                }
                return default(fieldGetSetValue);
            }
        }
        /// <summary>
        /// 字段获取&设置器 创建器
        /// </summary>
        public static readonly createFieldGetSetValue Creator = new createFieldGetSetValue();
    }
}
