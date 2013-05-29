using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 字段信息扩展
    /// </summary>
    public static class fieldInfo
    {
        /// <summary>
        /// 字段信息类型
        /// </summary>
        public static readonly Type RtFieldInfoType = typeof(FieldInfo).Assembly.GetType("System.Reflection.RtFieldInfo");
    }
}
