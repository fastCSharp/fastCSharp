using System;
using System.Collections.Generic;
using fastCSharp.net;
using fastCSharp.reflection;
using System.Reflection;
using System.Threading;
using fastCSharp.threading;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// TCP调用配置
    /// </summary>
    public partial class tcpCall : tcpBase
    {
        /// <summary>
        /// 获取TCP调用泛型函数集合
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns>TCP调用泛型函数集合</returns>
        public static Dictionary<genericMethod, MethodInfo> GetGenericMethods(Type type)
        {
            if (type != null)
            {
                tcpCall tcpCall = type.customAttribute<tcpCall>(false, cSharp.Default.IsInheritAttribute);
                if (tcpCall != null && tcpCall.IsSetup)
                {
                    Dictionary<genericMethod, MethodInfo> values = new Dictionary<genericMethod, MethodInfo>();
                    foreach (methodInfo method in methodInfo.GetMethods<tcpCall>(type, tcpCall.filter, false, tcpCall.IsAttribute, tcpCall.IsBaseTypeAttribute, tcpCall.IsInheritAttribute))
                    {
                        if (method.Method.IsGenericMethod) values.Add(new genericMethod(method.Method), method.Method);
                    }
                    return values;
                }
            }
            return null;
        }
        /// <summary>
        /// 泛型类型函数调用缓存
        /// </summary>
        private static readonly Dictionary<hashCode<Type>, keyValue<Type, staticDictionary<string, MethodInfo>>> genericTypeMethods = new Dictionary<hashCode<Type>, keyValue<Type, staticDictionary<string, MethodInfo>>>();
        /// <summary>
        /// 泛型类型函数调用缓存 访问锁
        /// </summary>
        private static int genericTypeMethodLock;
        /// <summary>
        /// 泛型类型函数调用缓存 版本
        /// </summary>
        private static int genericTypeMethodVersion;
        /// <summary>
        /// 泛型类型函数调用
        /// </summary>
        /// <param name="remoteType">调用代理类型</param>
        /// <param name="methodName">调用函数名称</param>
        /// <param name="methodGenericTypes">方法泛型参数集合</param>
        /// <param name="parameters">调用参数</param>
        /// <returns>函数返回值</returns>
        public static object InvokeGenericTypeMethod(fastCSharp.setup.remoteType remoteType, string methodName, fastCSharp.setup.remoteType[] methodGenericTypes, params object[] parameters)
        {
            return getGenericTypeMethod(remoteType, methodName).MakeGenericMethod(methodGenericTypes.getArray(value => value.Type)).Invoke(null, parameters);
        }
        /// <summary>
        /// 泛型类型函数调用
        /// </summary>
        /// <param name="remoteType">调用代理类型</param>
        /// <param name="methodName">调用函数名称</param>
        /// <param name="parameters">调用参数</param>
        /// <returns>函数返回值</returns>
        public static object InvokeGenericTypeMethod(fastCSharp.setup.remoteType remoteType, string methodName, params object[] parameters)
        {
            return getGenericTypeMethod(remoteType, methodName).Invoke(null, parameters);
        }
        /// <summary>
        /// 获取泛型类型函数信息
        /// </summary>
        /// <param name="remoteType">调用代理类型</param>
        /// <param name="methodName">调用函数名称</param>
        /// <returns>泛型类型函数信息</returns>
        public static MethodInfo getGenericTypeMethod(fastCSharp.setup.remoteType remoteType, string methodName)
        {
            Type type = remoteType.Type;
            if (type.Name == genericTypeServerName && type.DeclaringType.IsGenericType)
            {
                tcpCall tcpCall = type.customAttribute<tcpCall>();
                if (tcpCall != null && tcpCall.IsGenericTypeServerMethod && tcpCall.IsIgnore)
                {
                    tcpCall = type.DeclaringType.customAttribute<tcpCall>(false, cSharp.Default.IsInheritAttribute);
                    if (tcpCall != null && tcpCall.IsSetup)
                    {
                        keyValue<Type, staticDictionary<string, MethodInfo>> methods;
                        int version = genericTypeMethodVersion;
                        hashCode<Type> hashType = type;
                        if (!genericTypeMethods.TryGetValue(hashType, out methods) || methods.Key != type)
                        {
                            while (Interlocked.CompareExchange(ref genericTypeMethodLock, 1, 0) != 0) Thread.Sleep(1);
                            try
                            {
                                if (version == genericTypeMethodVersion || !genericTypeMethods.TryGetValue(hashType, out methods))
                                {
                                    methods = new keyValue<Type, staticDictionary<string, MethodInfo>>(type, new staticDictionary<string, MethodInfo>(type.GetMethods(BindingFlags.Static | BindingFlags.Public), value => value.Name));
                                    genericTypeMethods.Add(hashType, methods);
                                    ++genericTypeMethodVersion;
                                }
                            }
                            finally { genericTypeMethodLock = 0; }
                        }
                        return methods.Value.Get(methodName, null);
                    }
                }
            }
            log.Default.Throw(type.fullName() + " 不符合泛型类型服务器端调用", false, false);
            return null;
        }
        /// <summary>
        /// 泛型方法调用
        /// </summary>
        /// <param name="method">泛型方法信息</param>
        /// <param name="types">泛型参数类型集合</param>
        /// <param name="parameters">调用参数</param>
        /// <returns>返回值</returns>
        public static object InvokeGenericMethod(MethodInfo method, fastCSharp.setup.remoteType[] types, params object[] parameters)
        {
            if (method == null) fastCSharp.log.Default.Throw(fastCSharp.log.exceptionType.Null);
            return method.MakeGenericMethod(types.getArray(value => value.Type)).Invoke(null, parameters);
        }
        /// <summary>
        /// 客户端TCP调用池
        /// </summary>
        /// <typeparam name="clientType">TCP调用类型</typeparam>
        public class clientPool
        {
            /// <summary>
            /// TCP调用服务器端配置信息
            /// </summary>
            protected fastCSharp.setup.cSharp.tcpServer attribute;
            /// <summary>
            /// 服务名称
            /// </summary>
            private string serviceName;
            /// <summary>
            /// TCP验证实例
            /// </summary>
            protected fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify;
            /// <summary>
            /// 是否启动连接
            /// </summary>
            protected bool isStart;
            /// <summary>
            /// 最大连接数
            /// </summary>
            private int maxCount;
            /// <summary>
            /// 当前可用客户端
            /// </summary>
            private readonly list<fastCSharp.net.tcpClient> clients = new list<fastCSharp.net.tcpClient>();
            /// <summary>
            /// 客户端访问锁
            /// </summary>
            private readonly object clientLock = new object();
            /// <summary>
            /// 当前并发数
            /// </summary>
            private int currentCount;
            /// <summary>
            /// 创建中的客户端数量
            /// </summary>
            private int createCount = 1;
            /// <summary>
            /// 当前等待客户端数量
            /// </summary>
            private int waitCount;
            /// <summary>
            /// 等待客户端访问锁
            /// </summary>
            private readonly object waitLock = new object();
            /// <summary>
            /// 是否已停止
            /// </summary>
            protected bool isStop;
            /// <summary>
            /// 客户端TCP调用池
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="isStart">是否启动连接</param>
            /// <param name="serviceName">服务名称</param>
            /// <param name="verify">TCP验证实例</param>
            public clientPool(fastCSharp.setup.cSharp.tcpServer attribute, bool isStart, string serviceName = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify = null)
            {
                this.attribute = attribute;
                this.serviceName = serviceName;
                this.verify = verify;
                this.isStart = isStart;
                maxCount = attribute.MaxClientCount < 2 ? 2 : attribute.MaxClientCount;

                fastCSharp.threading.task.Default.Add(createClient, onClientError);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                Monitor.Enter(clientLock);
                isStop = true;
                Monitor.Exit(clientLock);
                Monitor.Enter(waitLock);
                try
                {
                    Monitor.PulseAll(waitLock);
                }
                finally { Monitor.Exit(waitLock); }
                foreach (fastCSharp.net.tcpClient value in clients)
                {
                    try
                    {
                        value.Dispose();
                    }
                    catch { }
                }
                clients.Clear();
            }
            /// <summary>
            /// 创建客户端失败
            /// </summary>
            /// <param name="error"></param>
            private void onClientError(Exception error)
            {
                Monitor.Enter(clientLock);
                --createCount;
                Monitor.Exit(clientLock);
            }
            /// <summary>
            /// 创建TCP调用客户端
            /// </summary>
            /// <returns>TCP调用客户端</returns>
            private fastCSharp.net.tcpClient newClient()
            {
                return new fastCSharp.net.tcpClient(attribute, verify, isStart);
            }
            /// <summary>
            /// 创建TCP调用客户端
            /// </summary>
            private void createClient()
            {
                fastCSharp.net.tcpClient value = newClient();
                if (value.IsStart)
                {
                    Monitor.Enter(clientLock);
                    try
                    {
                        clients.Add(value);
                        --createCount;
                    }
                    finally { Monitor.Exit(clientLock); }
                    pulseWait();
                }
                else onClientError(null);
            }
            /// <summary>
            /// 唤醒等待客户端
            /// </summary>
            private void pulseWait()
            {
                if (waitCount != 0)
                {
                    Monitor.Enter(waitLock);
                    try
                    {
                        Monitor.Pulse(waitLock);
                    }
                    finally { Monitor.Exit(waitLock); }
                }
            }
            /// <summary>
            /// 获取TCP调用客户端
            /// </summary>
            /// <returns>TCP调用客户端</returns>
            protected fastCSharp.net.tcpClient getClient()
            {
                fastCSharp.net.tcpClient value = null;
                bool isStop = false, isFull = false, isCreate = false, isNew = false;
                Monitor.Enter(clientLock);
                try
                {
                    if (this.isStop) isStop = true;
                    else if (clients.Count == 0)
                    {
                        if (currentCount >= maxCount) isFull = true;
                        else
                        {
                            ++currentCount;
                            isNew = true;
                        }
                    }
                    else
                    {
                        value = clients.Pop();
                        ++currentCount;
                        if (clients.Count == 0 && currentCount + createCount < maxCount)
                        {
                            ++createCount;
                            isCreate = true;
                        }
                    }
                }
                finally { Monitor.Exit(clientLock); }
                if (value != null)
                {
                    if (isCreate) fastCSharp.threading.task.Default.Add(createClient, onClientError);
                    return value;
                }
                if (isNew)
                {
                    try
                    {
                        value = newClient();
                        isNew = false;
                        return value;
                    }
                    finally
                    {
                        if (isNew)
                        {
                            Monitor.Enter(clientLock);
                            --currentCount;
                            Monitor.Exit(clientLock);
                        }
                    }
                }
                if (isFull)
                {
                    Interlocked.Increment(ref waitCount);
                    fastCSharp.log.Default.Add(null, (serviceName ?? "未知服务") + " 最大并发 " + maxCount.toString() + ", 等待客户端 " + waitCount.toString(), true);
                    while (!isStop && value == null)
                    {
                        Monitor.Enter(waitLock);
                        try
                        {
                            Monitor.Wait(waitLock);
                        }
                        finally { Monitor.Exit(waitLock); }
                        Monitor.Enter(clientLock);
                        try
                        {
                            if (this.isStop) isStop = true;
                            else if (clients.Count != 0) value = clients.Pop();
                        }
                        finally { Monitor.Exit(clientLock); }
                    }
                    Interlocked.Decrement(ref waitCount);
                }
                if (!isFull) fastCSharp.log.Default.Throw(null, "已关闭", false);
                return value;
            }
            /// <summary>
            /// 释放TCP调用客户端
            /// </summary>
            /// <param name="client">TCP调用客户端</param>
            protected void saveClient(fastCSharp.net.tcpClient client)
            {
                if (client != null)
                {
                    bool isSave = false;
                    Monitor.Enter(clientLock);
                    try
                    {
                        --currentCount;
                        if (!isStop && client.IsStart)
                        {
                            isSave = true;
                            clients.Add(client);
                        }
                    }
                    finally { Monitor.Exit(clientLock); }
                    if (isSave) pulseWait();
                    else if (client.IsStart) client.Dispose();
                }
            }
            /// <summary>
            /// 异步回调
            /// </summary>
            /// <typeparam name="returnType">返回值类型</typeparam>
            protected struct asyncMethod<returnType>
            {
                /// <summary>
                /// 回调函数
                /// </summary>
                public action<returnType> OnReturn;
                /// <summary>
                /// 释放TCP调用客户端
                /// </summary>
                public action<fastCSharp.net.tcpClient> SaveClient;
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                public fastCSharp.net.tcpClient Client;
                /// <summary>
                /// 异步回调
                /// </summary>
                /// <param name="_return_">返回值</param>
                public void Async(returnType _return_)
                {
                    try
                    {
                        SaveClient(Client);
                    }
                    finally
                    {
                        OnReturn(_return_);
                    }
                }
            }
            /// <summary>
            /// TCP调用并返回参数值
            /// </summary>
            /// <typeparam name="outputParameterType">输出参数类型</typeparam>
            /// <param name="command">TCP调用命令</param>
            /// <param name="outputParameter">输出参数</param>
            /// <returns>TCP调用是否成功</returns>
            public bool Get<outputParameterType>(byte[] command, outputParameterType outputParameter)
                where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = getClient();
                try
                {
                    return client.Get(command, outputParameter);
                }
                finally { saveClient(client); }
            }
            /// <summary>
            /// TCP调用并返回参数值
            /// </summary>
            /// <typeparam name="inputParameterType">输入参数类型</typeparam>
            /// <typeparam name="outputParameterType">输出参数类型</typeparam>
            /// <param name="command">TCP调用命令</param>
            /// <param name="inputParameter">输入参数</param>
            /// <param name="outputParameter">输出参数</param>
            /// <returns>TCP调用是否成功</returns>
            public bool Get<inputParameterType, outputParameterType>(byte[] command, inputParameterType inputParameter, outputParameterType outputParameter)
                where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
                where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = getClient();
                try
                {
                    return client.Get(command, inputParameter, outputParameter);
                }
                finally { saveClient(client); }
            }
            /// <summary>
            /// TCP调用
            /// </summary>
            /// <typeparam name="inputParameterType">输入参数类型</typeparam>
            /// <param name="command">TCP调用命令</param>
            /// <param name="inputParameter">输入参数</param>
            /// <returns>TCP调用是否成功</returns>
            public bool Call<inputParameterType>(byte[] command, inputParameterType inputParameter)
                where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = getClient();
                try
                {
                    return client.Call(command, inputParameter);
                }
                finally { saveClient(client); }
            }
            /// <summary>
            /// TCP调用
            /// </summary>
            /// <param name="command">TCP调用命令</param>
            /// <returns>TCP调用是否成功</returns>
            public bool Call(byte[] command)
            {
                fastCSharp.net.tcpClient client = getClient();
                try
                {
                    return client.Call(command);
                }
                finally { saveClient(client); }
            }
            /// <summary>
            /// 异步回调释放客户端
            /// </summary>
            /// <typeparam name="returnType">返回值类型</typeparam>
            public struct async<returnType>
            {
                /// <summary>
                /// 异步回调
                /// </summary>
                public action<returnType> OnReturn;
                /// <summary>
                /// 释放TCP调用客户端
                /// </summary>
                public clientPool ClientPool;
                /// <summary>
                /// TCP调用客户端
                /// </summary>
                public fastCSharp.net.tcpClient Client;
                /// <summary>
                /// 异步回调
                /// </summary>
                /// <param name="value">返回值</param>
                public void CallOnReturn(returnType value)
                {
                    try
                    {
                        task.TinyTask.Add(OnReturn, value);
                    }
                    finally { ClientPool.saveClient(Client); }
                }
            }
            /// <summary>
            /// TCP调用并返回参数值
            /// </summary>
            /// <typeparam name="outputParameterType">输出参数类型</typeparam>
            /// <param name="onGet">回调委托,返回null表示失败</param>
            /// <param name="command">TCP调用命令</param>
            /// <param name="outputParameter">输出参数</param>
            /// <returns>TCP调用客户端</returns>
            public fastCSharp.net.tcpClient Get<outputParameterType>(action<outputParameterType> onGet, byte[] command, outputParameterType outputParameter)
                where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = null;
                try
                {
                    client = getClient();
                }
                finally
                {
                    if (client != null) client.Get(new async<outputParameterType> { ClientPool = this, Client = client, OnReturn = onGet }.CallOnReturn, command, outputParameter);
                    else onGet(default(outputParameterType));
                }
                return client;
            }
            /// <summary>
            /// TCP调用并返回参数值
            /// </summary>
            /// <typeparam name="inputParameterType">输入参数类型</typeparam>
            /// <typeparam name="outputParameterType">输出参数类型</typeparam>
            /// <param name="onGet">回调委托,返回null表示失败</param>
            /// <param name="command">TCP调用命令</param>
            /// <param name="inputParameter">输入参数</param>
            /// <param name="outputParameter">输出参数</param>
            /// <returns>TCP调用客户端</returns>
            public fastCSharp.net.tcpClient Get<inputParameterType, outputParameterType>(action<outputParameterType> onGet, byte[] command, inputParameterType inputParameter, outputParameterType outputParameter)
                where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
                where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = null;
                try
                {
                    client = getClient();
                }
                finally
                {
                    if (client != null) client.Get(new async<outputParameterType> { ClientPool = this, Client = client, OnReturn = onGet }.CallOnReturn, command, inputParameter, outputParameter);
                    else onGet(default(outputParameterType));
                }
                return client;
            }
            /// <summary>
            /// TCP调用
            /// </summary>
            /// <typeparam name="inputParameterType">输入参数类型</typeparam>
            /// <param name="onCall">回调委托,返回false表示失败</param>
            /// <param name="command">TCP调用命令</param>
            /// <param name="inputParameter">输入参数</param>
            /// <returns>TCP调用客户端</returns>
            public fastCSharp.net.tcpClient Call<inputParameterType>(action<bool> onCall, byte[] command, inputParameterType inputParameter)
                where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                fastCSharp.net.tcpClient client = null;
                try
                {
                    client = getClient();
                }
                finally
                {
                    if (client != null) client.Call(new async<bool> { ClientPool = this, Client = client, OnReturn = onCall }.CallOnReturn, command, inputParameter);
                    else onCall(false);
                }
                return client;
            }
            /// <summary>
            /// TCP调用
            /// </summary>
            /// <param name="command">TCP调用命令</param>
            /// <param name="onCall">回调委托,返回false表示失败</param>
            /// <returns>TCP调用客户端</returns>
            public fastCSharp.net.tcpClient Call(action<bool> onCall, byte[] command)
            {
                fastCSharp.net.tcpClient client = null;
                try
                {
                    client = getClient();
                }
                finally
                {
                    if (client != null) client.Call(new async<bool> { ClientPool = this, Client = client, OnReturn = onCall }.CallOnReturn, command);
                    else onCall(false);
                }
                return client;
            }
        }
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public setup.memberFilter Filter = setup.memberFilter.NonPublicStatic;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public setup.memberFilter filter
        {
            get { return Filter & setup.memberFilter.Static; }
        }
        /// <summary>
        /// 是否泛型方法服务器端代理,用于代码生成,请不要手动设置此属性,否则可能产生严重的安全问题
        /// </summary>
        public bool IsGenericTypeServerMethod;

        /// <summary>
        /// TCP调用代码生成
        /// </summary
        [auto(Name = "TCP调用", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : tcpBase.cSharp<tcpCall>, IAuto
        {
            /// <summary>
            /// 默认TCP调用代码生成
            /// </summary>
            public static readonly cSharp Default = new cSharp();
            /// <summary>
            /// 是否搜索父类属性
            /// </summary>
            public override bool IsBaseType
            {
                get { return true; }
            }
            /// <summary>
            /// 自定义属性是否可继承
            /// </summary>
            public override bool IsInheritAttribute
            {
                get { return true; }
            }
            /// <summary>
            /// 服务类名称
            /// </summary>
            public string ServiceName
            {
                get { return Attribute.ServiceName; }
            }
            /// <summary>
            /// 服务类名称
            /// </summary>
            public string ServiceNameAsynchronous
            {
                get { return ServiceName; }
            }
            /// <summary>
            /// 类型泛型参数类型名称
            /// </summary>
            public string TypeGenericParameterName
            {
                get { return typeGenericParameterName; }
            }
            /// <summary>
            /// 方法索引集合
            /// </summary>
            private list<methodIndex> methodIndexs = new list<methodIndex>();
            /// <summary>
            /// TCP服务器端配置
            /// </summary>
            public tcpServer ServiceAttribute;
            /// <summary>
            /// TCP验证类型
            /// </summary>
            public override string TcpVerifyType
            {
                get { return ServiceAttribute.VerifyType.fullName(); }
            }
            /// <summary>
            /// 调用参数位置
            /// </summary>
            public string ParameterPart
            {
                get { return ServiceAttribute.IsSegmentation ? clientPart : serverPart; }
            }
            /// <summary>
            /// TCP服务器端配置集合
            /// </summary>
            private static readonly Dictionary<string, tcpServer> serviceAttributes = new Dictionary<string, tcpServer>();
            /// <summary>
            /// TCP客户端调用代码集合
            /// </summary>
            private static readonly Dictionary<string, stringBuilder> clientCallCodes = new Dictionary<string, stringBuilder>();
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                if (!serviceAttributes.TryGetValue(Attribute.ServiceName, out ServiceAttribute))
                {
                    serviceAttributes.Add(Attribute.ServiceName, ServiceAttribute = tcpServer.GetConfig(Attribute.ServiceName));
                }
                if (Attribute.VerifyType != null) ServiceAttribute.VerifyType = Attribute.VerifyType;
                int methodIndex = methodIndexs.Count;
                MethodIndexs = methodInfo.GetMethods<tcpCall>(type, Attribute.filter, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute, Attribute.IsInheritAttribute)
                    .getArray(value => new methodIndex
                    {
                        Method = value,
                        MethodIndex = methodIndex++,
                        MethodType = type,
                        ServiceAttribute = ServiceAttribute,
                        TypeAttribute = Attribute,
                        IsTypeGenericParameterName = type.Type.IsGenericType
                    });
                methodIndexs.Add(MethodIndexs);
                if (ServiceAttribute.IsSegmentation)
                {
                    stringBuilder clientCallCode;
                    if (!clientCallCodes.TryGetValue(Attribute.ServiceName, out clientCallCode))
                    {
                        clientCallCodes.Add(Attribute.ServiceName, clientCallCode = new stringBuilder());
                    }
                    definition definition = new definition(type, true, false);
                    _code_.Empty();
                    create(false);
                    fastCSharp.setup.cSharp.coder.Add(GetType(), CodeType.Type, definition.Start + _partCodes_["SERVERCALL"] + definition.End);
                    clientCallCode.Add(definition.Start + _partCodes_["CLIENTCALL"] + definition.End);
                }
                else create(true);
            }
            /// <summary>
            /// 是否所有类型
            /// </summary>
            public bool IsAllType;
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                if (methodIndexs.Count != 0)
                {
                    IsAllType = true;
                    foreach (list<methodIndex> methods in methodIndexs.group(value => value.TypeAttribute.ServiceName).Values)
                    {
                        MethodIndexs = methods.ToArray();
                        type = MethodIndexs[0].MethodType;
                        Attribute = MethodIndexs[0].TypeAttribute;
                        ServiceAttribute = MethodIndexs[0].ServiceAttribute;
                        _code_.Empty();
                        create(false);
                        fastCSharp.setup.cSharp.coder.Add(@"
namespace " + AutoParameter.DefaultNamespace + "." + serverPart + @"
{
" + _partCodes_["SERVER"] + @"
}");
                        string clientCode = @"
namespace " + AutoParameter.DefaultNamespace + "." + clientPart + @"
{
" + _partCodes_["CLIENT"] + @"
}";
                        if (ServiceAttribute.IsSegmentation)
                        {
                            stringBuilder clientCallCode = clientCallCodes[Attribute.ServiceName];
                            clientCallCode.Add(clientCode);
                            string fileName = AutoParameter.ProjectPath + AutoParameter.DefaultNamespace + ".tcpCall." + Attribute.ServiceName + ".client.cs";
                            clientCode = fastCSharp.setup.cSharp.coder.WarningCode + clientCallCode.ToString();
                            if (fastCSharp.setup.cSharp.coder.WriteFile(fileName, clientCode))
                            {
                                if (ServiceAttribute.ClientSegmentationCopyPath != null)
                                {
                                    string copyFileName = ServiceAttribute.ClientSegmentationCopyPath + AutoParameter.DefaultNamespace + ".tcpCall." + Attribute.ServiceName + ".client.cs";
                                    if (!fastCSharp.setup.cSharp.coder.WriteFile(copyFileName, clientCode)) fastCSharp.setup.error.Add(copyFileName + " 写入失败");
                                }
                                fastCSharp.setup.error.Message(fileName + " 被修改");
                            }
                        }
                        else fastCSharp.setup.cSharp.coder.Add(clientCode);
                    }
                }
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
        /// 泛型参数类型
        /// </summary>
        public memberType GenericParameterType
        {
            get
            {
                return Type.IsGenericParameter ? (memberType)typeof(object) : this;
            }
        }
    }
}
