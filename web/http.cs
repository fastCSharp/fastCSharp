using System;
using System.Collections.Generic;

namespace fastCSharp.web
{
    /// <summary>
    /// HTTP参数及其相关操作
    /// </summary>
    public static class http
    {
        /// <summary>
        /// 查询模式字典
        /// </summary>
        private static readonly staticDictionary<string, methodType> methodTypes;
        /// <summary>
        /// 查询模式字典
        /// </summary>
        private static readonly staticDictionary<hashBytes, methodType> methodBytes;
        /// <summary>
        /// 查询模式字符串最大长度
        /// </summary>
        public static readonly int MaxMethodLength;
        /// <summary>
        /// 查询模式类别
        /// </summary>
        public enum methodType
        {
            None = 0,
            /// <summary>
            /// 请求获取Request-URI所标识的资源
            /// </summary>
            GET,
            /// <summary>
            /// 在Request-URI所标识的资源后附加新的数据
            /// </summary>
            POST,
            /// <summary>
            /// 请求获取由Request-URI所标识的资源的响应消息报头
            /// </summary>
            HEAD,
            /// <summary>
            /// 请求服务器存储一个资源，并用Request-URI作为其标识
            /// </summary>
            PUT,
            /// <summary>
            /// 请求服务器删除Request-URI所标识的资源
            /// </summary>
            DELETE,
            /// <summary>
            /// 请求服务器回送收到的请求信息，主要用于测试或诊断
            /// </summary>
            TRACE,
            /// <summary>
            /// 保留将来使用
            /// </summary>
            CONNECT,
            /// <summary>
            /// 请求查询服务器的性能，或者查询与资源相关的选项和需求
            /// </summary>
            OPTIONS
        }
        /// <summary>
        /// 获取当前查询模式
        /// </summary>
        //public static methodType currentMethod
        //{
        //    get { return MethodTypes.get(HttpContext.Current.Request.HttpMethod.toUpper(), methodType.None); }
        //}
        /// <summary>
        /// 查询模式字节转枚举
        /// </summary>
        /// <param name="method">查询模式</param>
        /// <returns>查询模式枚举</returns>
        public static methodType getMethod(byte[] method)
        {
            return methodBytes.Get(method, methodType.None);
        }

        static http()
        {
            #region 初始化查询模式集合
            keyValue<string, methodType>[] types = System.Enum.GetValues(typeof(methodType))
                .toArray<methodType>().getFind(value => value != methodType.None)
                .GetArray(value => new keyValue<string, methodType>(value.ToString(), value));
            methodTypes = new staticDictionary<string, methodType>(types);
            methodBytes = new staticDictionary<hashBytes, methodType>(types.getArray(value => new keyValue<hashBytes, methodType>(value.Key.getBytes(), value.Value)));
            MaxMethodLength = methodTypes.Keys.maxKey(value => value.Length, 0);
            #endregion
        }
    }
}
