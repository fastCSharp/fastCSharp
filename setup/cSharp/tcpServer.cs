using System;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;
using System.Net;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// TCP服务调用配置,定义类必须实现fastCSharp.setup.cSharp.tcpServer.ITcpServer接口
    /// </summary>
    public partial class tcpServer : tcpBase
    {
        /// <summary>
        /// TCP服务接口
        /// </summary>
        public interface ITcpServer
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            void SetTcpServer(fastCSharp.net.tcpServerBase tcpServer);
        }
        /// <summary>
        /// 获取TCP调用泛型函数集合
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns>TCP调用泛型函数集合</returns>
        public static Dictionary<genericMethod, MethodInfo> GetGenericMethods(Type type)
        {
            if (type != null)
            {
                tcpServer tcpServer = type.customAttribute<tcpServer>(false, cSharp.Default.IsInheritAttribute);
                if (tcpServer != null && tcpServer.IsSetup)
                {
                    Dictionary<genericMethod, MethodInfo> values = new Dictionary<genericMethod, MethodInfo>();
                    methodInfo[] methods = methodInfo.GetMethods<tcpServer>(type, tcpServer.filter, false, tcpServer.IsAttribute, tcpServer.IsBaseTypeAttribute, tcpServer.IsInheritAttribute);
                    if (type.IsGenericType)
                    {
                        methodInfo[] definitionMethods = methodInfo.GetMethods<tcpServer>(type.GetGenericTypeDefinition(), tcpServer.filter, false, tcpServer.IsAttribute, tcpServer.IsBaseTypeAttribute, tcpServer.IsInheritAttribute);
                        int index = 0;
                        foreach (methodInfo method in methods)
                        {
                            if (method.Method.IsGenericMethod) values.Add(new genericMethod(definitionMethods[index].Method), method.Method);
                            ++index;
                        }
                    }
                    else
                    {
                        foreach (methodInfo method in methods)
                        {
                            if (method.Method.IsGenericMethod) values.Add(new genericMethod(method.Method), method.Method);
                        }
                    }
                    return values;
                }
            }
            return null;
        }
        /// <summary>
        /// 泛型方法调用
        /// </summary>
        /// <param name="value">服务器端目标对象</param>
        /// <param name="method">泛型方法信息</param>
        /// <param name="types">泛型参数类型集合</param>
        /// <param name="parameters">调用参数</param>
        /// <returns>返回值</returns>
        public static object InvokeGenericMethod(object value, MethodInfo method, fastCSharp.setup.remoteType[] types, params object[] parameters)
        {
            if (method == null) fastCSharp.log.Default.Throw(fastCSharp.log.exceptionType.Null);
            return method.MakeGenericMethod(types.getArray(type => type.Type)).Invoke(value, parameters);
        }
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public setup.memberFilter Filter = setup.memberFilter.NonPublicInstance;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public setup.memberFilter filter
        {
            get { return Filter & setup.memberFilter.Instance; }
        }
        /// <summary>
        /// 服务器端是否异步模式(一般来说内网同步,外网异步)
        /// </summary>
        public bool IsAsynchronous;
        /// <summary>
        /// 主机名称或者IP地址
        /// </summary>
        public string Host;
        /// <summary>
        /// 主机名称或者IP地址
        /// </summary>
        private string host;
        /// <summary>
        /// IP地址
        /// </summary>
        private IPAddress ipAddress;
        /// <summary>
        /// IP地址
        /// </summary>
        internal IPAddress IpAddress
        {
            get
            {
                if (ipAddress == null || host != Host)
                {
                    ipAddress = HostToIpAddress(host = Host) ?? IPAddress.Any;
                }
                return ipAddress;
            }
        }
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port;
        /// <summary>
        /// 接收数据超时的毫秒数
        /// </summary>
        public int ReceiveTimeout;
        /// <summary>
        /// 最大客户端连接数
        /// </summary>
        public int MaxClientCount;
        /// <summary>
        /// 是否压缩数据
        /// </summary>
        public bool IsCompress;
        /// <summary>
        /// 是否服务器端
        /// </summary>
        public bool IsServer;
        /// <summary>
        /// TCP注册服务名称
        /// </summary>
        public string TcpRegister;
        /// <summary>
        /// TCP注册服务名称
        /// </summary>
        public virtual string TcpRegisterName
        {
            get { return TcpRegister; }
        }
        /// <summary>
        /// 是否只允许一个TCP服务实例
        /// </summary>
        public bool IsSingleRegister = true;
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP调用服务名称</param>
        /// <param name="type">TCP服务器类型</param>
        /// <returns>TCP调用服务器端配置信息</returns>
        public static tcpServer GetConfig(string serviceName, Type type = null)
        {
            tcpServer attribute = fastCSharp.config.pub.Default.LoadConfig(type != null ? type.customAttribute<tcpServer>(false, true) ?? new tcpServer() : new tcpServer(), serviceName);
            if (attribute.Service == null) attribute.Service = serviceName;
            return attribute;
        }

        /// <summary>
        /// TCP服务调用代码生成
        /// </summary
        [auto(Name = "TCP服务", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : tcpBase.cSharp<tcpServer>, IAuto
        {
            /// <summary>
            /// 默认TCP调用代码生成
            /// </summary>
            public static readonly cSharp Default = new cSharp();
            /// <summary>
            /// 是否服务器端调用代理
            /// </summary>
            public bool IsServerCall;
            /// <summary>
            /// 类名称
            /// </summary>
            public string TypeNameAsynchronous
            {
                get { return type.TypeOnlyName; }
            }
            /// <summary>
            /// 服务类名称
            /// </summary>
            public string ServiceName
            {
                get
                {
                    return TypeName;
                }
            }
            /// <summary>
            /// TCP服务器端配置
            /// </summary>
            public tcpServer ServiceAttribute
            {
                get { return Attribute; }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                Attribute = fastCSharp.config.pub.Default.LoadConfig(Attribute, Attribute.ServiceName);
                int methodIndex = 0;
                MethodIndexs = methodInfo.GetMethods<tcpServer>(type, Attribute.filter, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute, Attribute.IsInheritAttribute)
                    .getArray(value => new methodIndex
                    {
                        Method = value,
                        MethodIndex = methodIndex++,
                        MethodType = type,
                        ServiceAttribute = Attribute,
                        TypeAttribute = Attribute,
                        IsTypeGenericParameterName = false
                    });
                create(IsServerCall = true);
                _code_.Empty();
                create(IsServerCall = false);
                fastCSharp.setup.cSharp.coder.Add(@"
namespace " + AutoParameter.DefaultNamespace + "." + serverPart + @"
{
" + _partCodes_["SERVER"] + @"
}
namespace " + AutoParameter.DefaultNamespace + "." + clientPart + @"
{
" + _partCodes_["CLIENT"] + @"
}");
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
            }
        }
    }
}
