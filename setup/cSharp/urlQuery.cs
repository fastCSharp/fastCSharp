using System;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using fastCSharp.reflection;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 查询字符串代码生成自定义属性
    /// </summary>
    public partial class urlQuery : memberFilter.publicInstance
    {
        /// <summary>
        /// 默认空查询字符串代码生成自定义属性
        /// </summary>
        internal static readonly urlQuery NullUrlQuery = new urlQuery();
        /// <summary>
        /// 逻辑值转字符串
        /// </summary>
        /// <param name="value">逻辑值</param>
        /// <returns>字符串</returns>
        private static string boolToString(bool value)
        {
            return value ? "true" : "false";
        }
        /// <summary>
        /// 转换GET查询字符串
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface IQuery<memberType> where memberType : fastCSharp.setup.cSharp.IMemberMap<memberType>
        {
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="encoding">编码格式</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>查询字符串</returns>
            string GetQuery(Encoding encoding, memberType memberMap = default(memberType));
        }
        /// <summary>
        /// 转换POST表单
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface IForm<memberType> where memberType : fastCSharp.setup.cSharp.IMemberMap<memberType>
        {
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>POST表单</returns>
            NameValueCollection GetForm(memberType memberMap = default(memberType));
        }
        /// <summary>
        /// 查询字符串获取器
        /// </summary>
        public interface IGetter
        {
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, string value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, byte value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, byte? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, sbyte value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, sbyte? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, short value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, short? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, ushort value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, ushort? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, int value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, int? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, uint value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, uint? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, long value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, long? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, ulong value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, ulong? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, bool value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, bool? value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, decimal value);
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get(string name, decimal? value);
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void Get<valueType>(string name, valueType value) where valueType : class;
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            void GetValue<valueType>(string name, valueType value) where valueType : struct;
        }
        /// <summary>
        /// GET查询字符串获取器
        /// </summary>
        public abstract class queryGetter : IGetter
        {
            /// <summary>
            /// 编码格式
            /// </summary>
            protected Encoding encoding;
            /// <summary>
            /// 查询字符串集合
            /// </summary>
            protected list<string>.unsafer querys;
            /// <summary>
            /// 查询字符串
            /// </summary>
            public string Query
            {
                get
                {
                    return querys.List.joinString('&');
                }
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, string value)
            {
                if (value != null) querys.Add(name + "=" + HttpUtility.UrlEncode(value, encoding));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, byte value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, byte? value)
            {
                if (value != null) querys.Add(name + "=" + ((byte)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, sbyte value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, sbyte? value)
            {
                if (value != null) querys.Add(name + "=" + ((sbyte)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, short value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, short? value)
            {
                if (value != null) querys.Add(name + "=" + ((short)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ushort value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ushort? value)
            {
                if (value != null) querys.Add(name + "=" + ((ushort)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, int value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, int? value)
            {
                if (value != null) querys.Add(name + "=" + ((int)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, uint value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, uint? value)
            {
                if (value != null) querys.Add(name + "=" + ((uint)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, long value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, long? value)
            {
                if (value != null) querys.Add(name + "=" + ((long)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ulong value)
            {
                querys.Add(name + "=" + value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ulong? value)
            {
                if (value != null) querys.Add(name + "=" + ((ulong)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, bool value)
            {
                querys.Add(name + "=" + boolToString(value));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, bool? value)
            {
                if (value != null) querys.Add(name + "=" + boolToString((bool)value));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, decimal value)
            {
                querys.Add(name + "=" + value.ToString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, decimal? value)
            {
                if (value != null) querys.Add(name + "=" + ((decimal)value).ToString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get<valueType>(string name, valueType value) where valueType : class
            {
                if (value != null) querys.Add(name + "=" + HttpUtility.UrlEncode(value.ToString(), encoding));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void GetValue<valueType>(string name, valueType value) where valueType : struct
            {
                querys.Add(name + "=" + HttpUtility.UrlEncode(value.ToString(), encoding));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            internal void Get(string name, object value)
            {
                if (value != null) querys.Add(name + "=" + HttpUtility.UrlEncode(value.ToString(), encoding));
            }
        }
        /// <summary>
        /// POST表单获取器
        /// </summary>
        public abstract class formGetter : IGetter
        {
            /// <summary>
            /// 查询字符串集合
            /// </summary>
            public NameValueCollection Form { get; protected set; }
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, string value)
            {
                if (value != null) Form.Add(name, value);
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, byte value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, byte? value)
            {
                if (value != null) Form.Add(name, ((byte)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, sbyte value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, sbyte? value)
            {
                if (value != null) Form.Add(name, ((sbyte)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, short value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, short? value)
            {
                if (value != null) Form.Add(name, ((short)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ushort value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ushort? value)
            {
                if (value != null) Form.Add(name, ((ushort)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, int value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, int? value)
            {
                if (value != null) Form.Add(name, ((int)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, uint value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, uint? value)
            {
                if (value != null) Form.Add(name, ((uint)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, long value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, long? value)
            {
                if (value != null) Form.Add(name, ((long)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ulong value)
            {
                Form.Add(name, value.toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, ulong? value)
            {
                if (value != null) Form.Add(name, ((ulong)value).toString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, bool value)
            {
                Form.Add(name, boolToString(value));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, bool? value)
            {
                if (value != null) Form.Add(name, boolToString((bool)value));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, decimal value)
            {
                Form.Add(name, value.ToString());
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get(string name, decimal? value)
            {
                if (value != null) Form.Add(name, ((decimal)value).ToString());
            }
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void Get<valueType>(string name, valueType value) where valueType : class
            {
                if (value != null) Form.Add(name, value.ToString());
            }
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <typeparam name="valueType">数据值类型</typeparam>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            public void GetValue<valueType>(string name, valueType value) where valueType : struct
            {
                Form.Add(name, value.ToString());
            }
            /// <summary>
            /// 获取POST表单项
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">数据值</param>
            internal void Get(string name, object value)
            {
                if (value != null) Form.Add(name, value.ToString());
            }
        }
        /// <summary>
        /// GET查询字符串获取器
        /// </summary>
        private class reflectionQueryGetter : queryGetter
        {
            /// <summary>
            /// GET查询字符串获取器
            /// </summary>
            /// <param name="encoding">编码格式</param>
            /// <param name="memberCount">成员数量</param>
            public reflectionQueryGetter(Encoding encoding, int memberCount)
            {
                this.encoding = encoding;
                querys = new list<string>(memberCount).Unsafer;
            }
        }
        /// <summary>
        /// 查询字符串获取器(反射模式)
        /// </summary>
        public static class query
        {
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">查询对象</param>
            /// <param name="encoding">编码格式</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>查询字符串</returns>
            public static string Get<valueType>(valueType value, Encoding encoding
                , setup.memberFilter filter = setup.memberFilter.PublicInstance, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return query<valueType>.Get(value, encoding, filter, memberMap);
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="encoding">编码格式</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <returns>查询字符串</returns>
            public static string Get(object value, Encoding encoding, setup.memberFilter filter = setup.memberFilter.PublicInstance)
            {
                return value != null ? getter(value.GetType())(value, encoding, filter) : null;
            }
            /// <summary>
            /// 获取 查询字符串获取器
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns>查询字符串获取器</returns>
            private static func<object, Encoding, setup.memberFilter, string> getter(Type type)
            {
                func<object, Encoding, setup.memberFilter, string> value;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getterLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getters.TryGetValue(hashType, out value))
                    {
                        getters.Add(hashType, value = (func<object, Encoding, setup.memberFilter, string>)Delegate.CreateDelegate(typeof(func<object, Encoding, setup.memberFilter, string>), typeof(query<>).MakeGenericType(type).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic)));
                    }
                }
                finally { getterLock = 0; }
                return value;
            }
            /// <summary>
            /// 查询字符串获取器 访问锁
            /// </summary>
            private static int getterLock;
            /// <summary>
            /// 查询字符串获取器 集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<object, Encoding, setup.memberFilter, string>> getters = new Dictionary<hashCode<Type>, func<object, Encoding, setup.memberFilter, string>>();
        }
        /// <summary>
        /// 查询字符串获取器(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal static class query<valueType>
        {
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="encoding">编码格式</param>
            /// <param name="filter">成员选择</param>
            /// <returns>查询字符串</returns>
            private static string getObject(object value, Encoding encoding, setup.memberFilter filter)
            {
                return getNotNull((valueType)value, encoding, filter, default(memberMap<valueType>));
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="encoding">编码格式</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>查询字符串</returns>
            public static string Get(valueType value, Encoding encoding, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                if (!isStruct && value == null) return null;
                return getNotNull(value, encoding, filter, memberMap);
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="encoding">编码格式</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>查询字符串</returns>
            private static string getNotNull(valueType value, Encoding encoding, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                reflectionQueryGetter queryGetter = new reflectionQueryGetter(encoding, memberGroup.Count);
                foreach (keyValue<setup.memberInfo, object> query in memberGroup.GetMemberValue(value, filter, memberMap))
                {
                    queryGetter.Get(query.Key.MemberName, query.Value);
                }
                return queryGetter.Query;
            }
            /// <summary>
            /// 是否值类型
            /// </summary>
            private static readonly bool isStruct = typeof(valueType).isStruct() || typeof(valueType).IsEnum;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            private static readonly memberGroup<valueType> memberGroup = memberGroup<valueType>.Create<urlQuery>(urlQuery.NullUrlQuery.IsAttribute, urlQuery.NullUrlQuery.IsBaseTypeAttribute, urlQuery.NullUrlQuery.IsInheritAttribute, value => value.CanGet);
        }
        /// <summary>
        /// POST表单获取器(反射模式)
        /// </summary>
        private class reflectionFormGetter : formGetter
        {
            /// <summary>
            /// POST表单获取器
            /// </summary>
            /// <param name="memberCount">成员数量</param>
            public reflectionFormGetter(int memberCount)
            {
                Form = new NameValueCollection(memberCount);
            }
        }
        /// <summary>
        /// POST表单获取器(反射模式)
        /// </summary>
        public static class form
        {
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">查询对象</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>POST表单</returns>
            public static NameValueCollection Get<valueType>(valueType value
                , setup.memberFilter filter = setup.memberFilter.PublicInstance, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return form<valueType>.Get(value, filter, memberMap);
            }
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <returns>POST表单</returns>
            public static NameValueCollection Get(object value, setup.memberFilter filter = setup.memberFilter.PublicInstance)
            {
                return value != null ? getter(value.GetType())(value, filter) : null;
            }
            /// <summary>
            /// 获取 POST表单获取器
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns>POST表单获取器</returns>
            private static func<object, setup.memberFilter, NameValueCollection> getter(Type type)
            {
                func<object, setup.memberFilter, NameValueCollection> value;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getterLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getters.TryGetValue(hashType, out value))
                    {
                        getters.Add(hashType, value = (func<object, setup.memberFilter, NameValueCollection>)Delegate.CreateDelegate(typeof(func<object, setup.memberFilter, NameValueCollection>), typeof(form<>).MakeGenericType(type).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic)));
                    }
                }
                finally { getterLock = 0; }
                return value;
            }
            /// <summary>
            /// POST表单获取器 访问锁
            /// </summary>
            private static int getterLock;
            /// <summary>
            /// POST表单获取器 集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<object, setup.memberFilter, NameValueCollection>> getters = new Dictionary<hashCode<Type>, func<object, setup.memberFilter, NameValueCollection>>();
        }
        /// <summary>
        /// POST表单获取器(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal static class form<valueType>
        {
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="filter">成员选择</param>
            /// <returns>POST表单</returns>
            private static NameValueCollection getObject(object value, setup.memberFilter filter)
            {
                return getNotNull((valueType)value, filter, default(memberMap<valueType>));
            }
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>POST表单</returns>
            public static NameValueCollection Get(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                if (!isStruct && value == null) return null;
                return getNotNull(value, filter, memberMap);
            }
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="value">查询对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>POST表单</returns>
            private static NameValueCollection getNotNull(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                reflectionFormGetter formGetter = new reflectionFormGetter(memberGroup.Count);
                foreach (keyValue<setup.memberInfo, object> query in memberGroup.GetMemberValue(value, filter, memberMap))
                {
                    formGetter.Get(query.Key.MemberName, query.Value);
                }
                return formGetter.Form;
            }
            /// <summary>
            /// 是否值类型
            /// </summary>
            private static readonly bool isStruct = typeof(valueType).isStruct() || typeof(valueType).IsEnum;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            private static readonly memberGroup<valueType> memberGroup = memberGroup<valueType>.Create<urlQuery>(urlQuery.NullUrlQuery.IsAttribute, urlQuery.NullUrlQuery.IsBaseTypeAttribute, urlQuery.NullUrlQuery.IsInheritAttribute, value => value.CanGet);
        }
        /// <summary>
        /// 是否转换为GET查询字符串
        /// </summary>
        public bool IsQuery = true;
        /// <summary>
        /// 是否转换为POST表单
        /// </summary>
        public bool IsForm;
        /// <summary>
        /// 查询字符串代码生成
        /// </summary
        [auto(Name = "查询字符串", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : member<urlQuery>
        {
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                Members = getMembers().getFindArray(value => value.CanGet);
                memberMap.create(type);
                create(true);
            }
        }
    }
}
namespace fastCSharp.setup
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal partial class memberType
    {
        /// <summary>
        /// 硬编码查询类型
        /// </summary>
        private static readonly staticHashSet<hashCode<Type>> urlQueryTypes = new staticHashSet<hashCode<Type>>(new hashCode<Type>[]
        {
            typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte), typeof(decimal), typeof(bool), 
        });
        /// <summary>
        /// 是否硬编码查询
        /// </summary>
        private bool? isUrlQueryType;
        /// <summary>
        /// 是否硬编码查询
        /// </summary>
        public bool IsUrlQueryType
        {
            get
            {
                if (isUrlQueryType == null) isUrlQueryType = urlQueryTypes.Contains(Type.nullableType() ?? Type);
                return (bool)isUrlQueryType;
            }
        }
    }
}
