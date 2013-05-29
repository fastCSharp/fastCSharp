using System;
using System.Collections;
using System.Collections.Generic;

namespace fastCSharp.setup.cSharp.template
{
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub : IEnumerable
    {
        /// <summary>
        /// 成员索引
        /// </summary>
        public const int MemberIndex = 0;
        /// <summary>
        /// 最大成员索引
        /// </summary>
        public const int MaxMemberIndex = 0;
        /// <summary>
        /// 可赋值成员位图
        /// </summary>
        public const ulong CanSetMember = 0;
        /// <summary>
        /// 可赋值成员位图
        /// </summary>
        public const byte CanSetMemberMap = 0;
        /// <summary>
        /// 可赋值成员位图
        /// </summary>
        public static readonly IMemberMap CanSet = null;
        /// <summary>
        /// 成员集合
        /// </summary>
        public static setup.memberInfo[] Members = null;
        /// <summary>
        /// 成员名称
        /// </summary>
        public pub MemberName = null;
        /// <summary>
        /// 枚举糊类型
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator pub(FullName[] value) { return null; }
        /// <summary>
        /// 成员类型
        /// </summary>
        public class MemberType : pub
        {
        }
        /// <summary>
        /// 非空类型
        /// </summary>
        public class NotNullType : pub
        {
        }
        /// <summary>
        /// 枚举参数类型
        /// </summary>
        public class EnumerableArgumentType : pub
        {
        }
        /// <summary>
        /// 类型
        /// </summary>
        public partial class type : pub
        {
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : pub
        {
            public FullName() { }
            public FullName(object value) { }
        }
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public class KeyValueType : pub
        {
        }
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public class PairKeyType : KeyValueType
        {
        }
        /// <summary>
        /// 键值对值类型
        /// </summary>
        public class PairValueType : KeyValueType
        {
        }
        /// <summary>
        /// 非可空类型
        /// </summary>
        public struct StructNotNullType
        {
            /// <summary>
            /// 隐式转换
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static implicit operator FullName(StructNotNullType value) { return null; }
        }
        /// <summary>
        /// 字符串转解析值
        /// </summary>
        /// <typeparam name="valueType">解析类型</typeparam>
        /// <param name="stringValue">字符串</param>
        /// <param name="value">解析值</param>
        /// <returns>转换是否成功</returns>
        public static bool TryParse<valueType>(string stringValue, out valueType value)
        {
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public object MethodGenericName(params object[] value)
        {
            return null;
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public static object StaticMethodGenericName(params object[] value)
        {
            return null;
        }
    }
}
