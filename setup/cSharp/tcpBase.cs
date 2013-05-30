using System;
using System.Reflection;
using fastCSharp.reflection;
using System.Net;
using fastCSharp.threading;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// TCP调用配置基类
    /// </summary>
    public abstract class tcpBase : ignore
    {
        /// <summary>
        /// 返回值参数名称
        /// </summary>
        protected const string ReturnParameterName = "_Return_";
        /// <summary>
        /// 泛型返回值类型名称
        /// </summary>
        protected const string ReturnParameterTypeName = "_ReturnType_";
        /// <summary>
        /// 泛型类型服务器端调用类型名称
        /// </summary>
        protected const string genericTypeServerName = "tcpServer";
        /// <summary>
        /// TCP客户端验证接口
        /// </summary>
        public interface ITcpVerifyClient
        {
            /// <summary>
            /// TCP客户端验证
            /// </summary>
            /// <param name="socket">TCP调用客户端</param>
            /// <returns>是否通过验证</returns>
            bool Verify(fastCSharp.net.tcpClient client);
        }
        /// <summary>
        /// TCP服务器端同步验证客户端接口
        /// </summary>
        public interface ITcpVerify : ITcpVerifyClient
        {
            /// <summary>
            /// TCP客户端同步验证
            /// </summary>
            /// <param name="socket">同步套接字</param>
            /// <returns>是否通过验证</returns>
            bool Verify(fastCSharp.net.tcpServer.socket socket);
        }
        /// <summary>
        /// TCP服务器端异步验证客户端接口
        /// </summary>
        public interface ITcpVerifyAsynchronous : ITcpVerifyClient
        {
            /// <summary>
            /// TCP客户端异步验证
            /// </summary>
            /// <param name="socket">同步套接字</param>
            /// <param name="onVerify">验证后的处理</param>
            void Verify(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify);
        }
        /// <summary>
        /// 泛型函数信息
        /// </summary>
        public struct genericMethod : IEquatable<genericMethod>
        {
            /// <summary>
            /// 泛型参数数量
            /// </summary>
            public int ArgumentCount;
            /// <summary>
            /// 函数名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 参数名称集合
            /// </summary>
            public string[] ParameterTypeNames;
            /// <summary>
            /// 哈希值
            /// </summary>
            public int HashCode;
            /// <summary>
            /// 泛型函数信息
            /// </summary>
            /// <param name="method">泛型函数信息</param>
            public genericMethod(MethodInfo method)
            {
                Name = method.Name;
                ArgumentCount = method.GetGenericArguments().Length;
                ParameterTypeNames = parameterInfo.Get(method).getArray(value => value.ParameterRef + value.ParameterType.FullName);
                HashCode = Name.GetHashCode() ^ ArgumentCount;
                setHashCode();
            }
            /// <summary>
            /// 泛型函数信息
            /// </summary>
            /// <param name="name">函数名称</param>
            /// <param name="argumentCount">泛型参数数量</param>
            /// <param name="typeNames">参数名称集合</param>
            public genericMethod(string name, int argumentCount, params string[] typeNames)
            {
                Name = name;
                ArgumentCount = argumentCount;
                ParameterTypeNames = typeNames;
                HashCode = Name.GetHashCode() ^ ArgumentCount;
                setHashCode();
            }
            /// <summary>
            /// 计算哈希值
            /// </summary>
            private void setHashCode()
            {
                foreach (string name in ParameterTypeNames) HashCode ^= name.GetHashCode();
            }
            /// <summary>
            /// 哈希值
            /// </summary>
            /// <returns>哈希值</returns>
            public override int GetHashCode()
            {
                return HashCode;
            }
            /// <summary>
            /// 比较是否相等
            /// </summary>
            /// <param name="other">比较对象</param>
            /// <returns>是否相等</returns>
            public override bool Equals(object other)
            {
                return other != null && other.GetType() == typeof(genericMethod) && Equals((genericMethod)other);
            }
            /// <summary>
            /// 比较是否相等
            /// </summary>
            /// <param name="other">比较对象</param>
            /// <returns>是否相等</returns>
            public bool Equals(genericMethod other)
            {
                if (HashCode == other.HashCode && Name == other.Name
                    && ParameterTypeNames.Length == other.ParameterTypeNames.Length)
                {
                    int index = 0;
                    foreach (string name in other.ParameterTypeNames)
                    {
                        if (ParameterTypeNames[index++] != name) return false;
                    }
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 异步返回值
        /// </summary>
        public struct asynchronousReturn
        {
            /// <summary>
            /// 是否调用成功
            /// </summary>
            public bool IsReturn;
        }
        /// <summary>
        /// 异步返回值
        /// </summary>
        /// <typeparam name="valueType">返回值类型</typeparam>
        public struct asynchronousReturn<valueType>
        {
            /// <summary>
            /// 是否调用成功
            /// </summary>
            public bool IsReturn;
            /// <summary>
            /// 返回值
            /// </summary>
            public valueType Value;
            /// <summary>
            /// 获取返回值
            /// </summary>
            /// <param name="value">异步返回值</param>
            /// <returns>返回值</returns>
            public static implicit operator asynchronousReturn<valueType>(valueType value)
            {
                return new asynchronousReturn<valueType> { IsReturn = true, Value = value };
            }
        }
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <typeparam name="valueType">返回参数类型</typeparam>
        public class returnParameter<valueType>
        {
            /// <summary>
            /// 返回值
            /// </summary>
            public valueType _Return_;
        }
        /// <summary>
        /// 异步回调泛型返回值
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        public struct asyncReturnGeneric<returnType, outputParameterType> where outputParameterType : returnParameter<object>
        {
            /// <summary>
            /// 异步回调返回值
            /// </summary>
            public action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType>> OnReturn;
            /// <summary>
            /// 异步回调返回值
            /// </summary>
            /// <param name="outputParameter">输出参数</param>
            public void CallOnReturn(outputParameterType outputParameter)
            {
                if (outputParameter != null)
                {
                    OnReturn(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType> { IsReturn = true, Value = (returnType)outputParameter._Return_ });
                }
                else OnReturn(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType> { IsReturn = false });
            }
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        public struct asyncReturn<returnType, outputParameterType> where outputParameterType : returnParameter<returnType>
        {
            /// <summary>
            /// 异步回调返回值
            /// </summary>
            public action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType>> OnReturn;
            /// <summary>
            /// 异步回调返回值
            /// </summary>
            /// <param name="outputParameter">输出参数</param>
            public void CallOnReturn(outputParameterType outputParameter)
            {
                if (outputParameter != null)
                {
                    OnReturn(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType> { IsReturn = true, Value = outputParameter._Return_ });
                }
                else OnReturn(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<returnType> { IsReturn = false });
            }
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        public struct asyncReturn
        {
            /// <summary>
            /// 异步回调
            /// </summary>
            public action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> OnReturn;
            /// <summary>
            /// 异步回调
            /// </summary>
            /// <param name="isReturn">调用使用成功</param>
            public void CallOnReturn(bool isReturn)
            {
                OnReturn(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn { IsReturn = isReturn });
            }
        }
        /// <summary>
        /// 本地异步回调
        /// </summary>
        public struct asyncCall
        {
            /// <summary>
            /// 本地调用委托
            /// </summary>
            public action Call;
            /// <summary>
            /// 异步返回值
            /// </summary>
            public action<asynchronousReturn> OnCall;
            /// <summary>
            /// 启动调用线程
            /// </summary>
            public void Start()
            {
                task.TinyTask.Add(call);
            }
            /// <summary>
            /// 调用线程
            /// </summary>
            private void call()
            {
                bool isReturn = true;
                try
                {
                    Call();
                }
                catch (Exception error)
                {
                    isReturn = false;
                    log.Default.Add(error, null, false);
                }
                finally
                {
                    OnCall(new asynchronousReturn { IsReturn = isReturn });
                }
            }
        }
        /// <summary>
        /// 本地异步回调
        /// </summary>
        /// <typeparam name="valueType">返回值类型</typeparam>
        public struct asyncCall<valueType>
        {
            /// <summary>
            /// 本地调用委托
            /// </summary>
            public func<valueType> Call;
            /// <summary>
            /// 异步返回值
            /// </summary>
            public action<asynchronousReturn<valueType>> OnCall;
            /// <summary>
            /// 启动调用线程
            /// </summary>
            public void Start()
            {
                task.TinyTask.Add(call);
            }
            /// <summary>
            /// 调用线程
            /// </summary>
            private void call()
            {
                bool isReturn = true;
                valueType value = default(valueType);
                try
                {
                    value = Call();
                }
                catch (Exception error)
                {
                    isReturn = false;
                    log.Default.Add(error, null, false);
                }
                finally
                {
                    OnCall(new asynchronousReturn<valueType> { IsReturn = isReturn, Value = value });
                }
            }
        }
        /// <summary>
        /// 成员是否匹配自定义属性类型
        /// </summary>
        public bool IsAttribute = true;
        /// <summary>
        /// 是否搜索父类自定义属性
        /// </summary>
        public bool IsBaseTypeAttribute;
        /// <summary>
        /// 成员匹配自定义属性是否可继承
        /// </summary>
        public bool IsInheritAttribute = true;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Service;
        /// <summary>
        /// 验证类,必须继承接口fastCSharp.setup.cSharp.tcpBase.ITcpVerify或fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous
        /// </summary>
        public Type VerifyType;
        /// <summary>
        /// 服务器端函数是否显示异步回调,tcpServer.IsAsynchronous为true时才有效(返回值必须为void，最后一个参数必须为回调委托action(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn))
        /// </summary>
        public bool IsAsynchronousCallback;
        /// <summary>
        /// 客户端是否提供同步调用
        /// </summary>
        public bool IsClientSynchronous = true;
        /// <summary>
        /// 客户端是否提供异步调用
        /// </summary>
        public bool IsClientAsynchronous;
        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string ServiceName
        {
            get { return Service; }
        }
        /// <summary>
        /// 方法标识名称转TCP调用命令
        /// </summary>
        /// <param name="name">方法标识名称</param>
        /// <returns>TCP调用命令</returns>
        public unsafe static byte[] GetMethodKeyNameCommand(string name)
        {
            int length = name.Length, commandLength = (length + 3) & (int.MaxValue - 3);
            byte[] data = new byte[commandLength + sizeof(int)];
            fixed (byte* dataFixed = data)
            {
                *(int*)dataFixed = commandLength;
                if ((length & 3) != 0) *(int*)(dataFixed + sizeof(int) + (length & (int.MaxValue - 3))) = 0x20202020;
                net.tcpServer.formatMethodKeyName(name, dataFixed + sizeof(int));
            }
            return data;
        }
        /// <summary>
        /// 获取泛型参数集合
        /// </summary>
        /// <param name="_"></param>
        /// <param name="types">泛型参数集合</param>
        /// <returns>泛型参数集合</returns>
        public static fastCSharp.setup.remoteType[] GetGenericParameters(int _, params Type[] types)
        {
            return types.getArray(type => new fastCSharp.setup.remoteType(type));
        }
        /// <summary>
        /// 泛型回调委托
        /// </summary>
        /// <typeparam name="valueType">返回值类型</typeparam>
        private struct genericParameterCallback<valueType>
        {
            /// <summary>
            /// 回调委托
            /// </summary>
            public action<asynchronousReturn<object>> OnCallback;
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="value">返回值</param>
            public void Callback(asynchronousReturn<valueType> value)
            {
                OnCallback(new asynchronousReturn<object> { IsReturn = value.IsReturn, Value = value.IsReturn ? (object)value.Value : null });
            }
        }
        /// <summary>
        /// 获取泛型回调委托
        /// </summary>
        /// <typeparam name="valueType">返回值类型</typeparam>
        /// <param name="callback">回调委托</param>
        /// <returns>泛型回调委托</returns>
        private static object getGenericParameterCallback<valueType>(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>> callback)
        {
            return (action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<valueType>>)new genericParameterCallback<valueType> { OnCallback = callback }.Callback;
        }
        /// <summary>
        /// 获取泛型回调委托函数信息
        /// </summary>
        private static readonly MethodInfo getGenericParameterCallbackMethod = typeof(tcpBase).GetMethod("getGenericParameterCallback", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型回调委托
        /// </summary>
        /// <param name="type">返回值类型</param>
        /// <param name="callback">回调委托</param>
        /// <returns>泛型回调委托</returns>
        public static object GetGenericParameterCallback(fastCSharp.setup.remoteType type, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>> callback)
        {
            return ((func<action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>>, object>)Delegate.CreateDelegate(typeof(func<action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>>, object>), getGenericParameterCallbackMethod.MakeGenericMethod(type.Type)))(callback);
        }
        /// <summary>
        /// 主机名称转换成IP地址
        /// </summary>
        /// <param name="host">主机名称</param>
        /// <returns>IP地址</returns>
        internal static IPAddress HostToIpAddress(string host)
        {
            if (host.length() != 0)
            {
                IPAddress ipAddress;
                if (!IPAddress.TryParse(host, out ipAddress))
                {
                    try
                    {
                        ipAddress = Dns.GetHostEntry(host).AddressList[0];
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, host, true);
                    }
                }
                return ipAddress;
            }
            return null;
        }
        /// <summary>
        /// TCP调用代码生成
        /// </summary>
        internal abstract class cSharp<attributeType> : cSharper<attributeType> where attributeType : tcpBase
        {
            /// <summary>
            /// 方法泛型参数类型名称
            /// </summary>
            private const string genericParameterTypeName = "_GenericParameterTypes_";
            /// <summary>
            /// 类型泛型参数类型名称
            /// </summary>
            protected const string typeGenericParameterName = "_TypeGenericParameter_";
            /// <summary>
            /// 服务器端位置
            /// </summary>
            protected const string serverPart = "tcpServer";
            /// <summary>
            /// 客户端位置
            /// </summary>
            protected const string clientPart = "tcpClient";
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public class methodIndex
            {
                /// <summary>
                /// 获取改方法的类型
                /// </summary>
                public memberType MethodType;
                /// <summary>
                /// 方法信息
                /// </summary>
                public setup.methodInfo Method;
                /// <summary>
                /// 方法索引
                /// </summary>
                public int MethodIndex;
                /// <summary>
                /// 是否处理类型泛型参数类型名称
                /// </summary>
                public bool IsTypeGenericParameterName;
                /// <summary>
                /// TCP服务器端配置
                /// </summary>
                public tcpServer ServiceAttribute;
                /// <summary>
                /// 类型TCP调用配置
                /// </summary>
                public attributeType TypeAttribute;
                /// <summary>
                /// TCP调用配置
                /// </summary>
                private attributeType attribute;
                /// <summary>
                /// TCP调用配置
                /// </summary>
                public attributeType Attribute
                {
                    get
                    {
                        if (attribute == null)
                        {
                            attribute = Method.Method.customAttribute<attributeType>(false, false) ?? TypeAttribute;
                        }
                        return attribute;
                    }
                }
                /// <summary>
                /// 方法索引名称
                /// </summary>
                public string MethodIndexName
                {
                    get
                    {
                        return "_m" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// 方法索引泛型名称
                /// </summary>
                public string MethodIndexGenericName
                {
                    get
                    {
                        return MethodIndexName + Method.GenericParameterName;
                    }
                }
                /// <summary>
                /// 是否异步回调方法
                /// </summary>
                private bool isAsynchronousCallback;
                /// <summary>
                /// 是否异步回调方法
                /// </summary>
                public bool IsAsynchronousCallback
                {
                    get
                    {
                        if (ServiceAttribute.IsAsynchronous)
                        {
                            checkAsynchronousReturn();
                            return isAsynchronousCallback;
                        }
                        return false;
                    }
                }
                /// <summary>
                /// 异步回调方返回值是否泛型
                /// </summary>
                public bool IsGenericParameterCallback
                {
                    get
                    {
                        return IsAsynchronousCallback && MethodReturnType.Type.IsGenericParameter;
                    }
                }
                /// <summary>
                /// 返回值类型
                /// </summary>
                private memberType methodReturnType;
                /// <summary>
                /// 返回值类型
                /// </summary>
                public memberType MethodReturnType
                {
                    get
                    {
                        checkAsynchronousReturn();
                        return methodReturnType;
                    }
                }
                /// <summary>
                /// 方法参数
                /// </summary>
                private parameterInfo[] methodParameters;
                /// <summary>
                /// 方法参数
                /// </summary>
                public parameterInfo[] MethodParameters
                {
                    get
                    {
                        checkAsynchronousReturn();
                        return methodParameters;
                    }
                }
                /// <summary>
                /// 是否有返回值
                /// </summary>
                public bool MethodIsReturn
                {
                    get
                    {
                        checkAsynchronousReturn();
                        return methodReturnType.Type != typeof(void);
                    }
                }
                /// <summary>
                /// 检测异步回调方法
                /// </summary>
                private void checkAsynchronousReturn()
                {
                    if (methodParameters == null)
                    {
                        methodParameters = Method.Parameters;
                        methodReturnType = Method.ReturnType;
                        if (ServiceAttribute.IsAsynchronous && Attribute.IsAsynchronousCallback && Method.ReturnType.Type == typeof(void) && methodParameters.Length != 0)
                        {
                            Type type = methodParameters[methodParameters.Length - 1].ParameterType.Type;
                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(action<>))
                            {
                                Type parameterType = type.GetGenericArguments()[0];
                                if (parameterType == typeof(asynchronousReturn))
                                {
                                    isAsynchronousCallback = true;
                                }
                                else if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(asynchronousReturn<>))
                                {
                                    methodReturnType = parameterType.GetGenericArguments()[0];
                                    isAsynchronousCallback = true;
                                }
                                if (isAsynchronousCallback)
                                {
                                    methodParameters = parameterInfo.Get(methodParameters.getSub(0, methodParameters.Length - 1));
                                }
                            }
                        }
                    }
                }
                /// <summary>
                /// 异步回调
                /// </summary>
                public string AsynchronousCallbackIndexName
                {
                    get
                    {
                        return "_a" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// 是否存在输入参数
                /// </summary>
                public bool IsInputParameter
                {
                    get
                    {
                        return MethodParameters.Length != 0 || Method.GenericParameters.Length != 0 || (IsAsynchronousCallback && MethodReturnType.Type.IsGenericParameter) || IsTypeGenericParameterName;
                    }
                }
                /// <summary>
                /// 是否存在输出参数
                /// </summary>
                public bool IsOutputParameter
                {
                    get
                    {
                        return MethodReturnType.Type != typeof(void) || Method.OutputParameters.Length != 0;
                    }
                }
                /// <summary>
                /// 是否有输入输出参数
                /// </summary>
                public bool IsAnyParameter
                {
                    get
                    {
                        return IsInputParameter || IsOutputParameter;
                    }
                }
                /// <summary>
                /// 输入参数名称
                /// </summary>
                public string InputParameterTypeName
                {
                    get
                    {
                        return "_i" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// 输入参数序列化代码
                /// </summary>
                public string InputParameterSerialize
                {
                    get
                    {
                        int index = 0;
                        parameterInfo[] parameters = MethodParameters;
                        list<fastCSharp.setup.cSharp.memberInfo>.unsafer members = new list<memberInfo>(parameters.length() + (IsTypeGenericParameterName ? 1 : 0) + (Method.GenericParameters.Length == 0 ? 0 : 1) + (IsAsynchronousCallback && MethodReturnType.Type.IsGenericParameter ? 1 : 0)).Unsafer;
                        if (IsTypeGenericParameterName) members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = typeGenericParameterName, MemberType = typeof(fastCSharp.setup.remoteType), MemberIndex = index++ });
                        if (Method.GenericParameters.Length != 0) members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = genericParameterTypeName, MemberType = typeof(fastCSharp.setup.remoteType[]), MemberIndex = index++ });
                        if (IsAsynchronousCallback && MethodReturnType.Type.IsGenericParameter) members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = ReturnTypeName, MemberType = typeof(fastCSharp.setup.remoteType), MemberIndex = index++ });
                        foreach (parameterInfo parameter in parameters)
                        {
                            members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = parameter.ParameterName, MemberType = parameter.ParameterType.GenericParameterType, MemberIndex = index++ });
                        }
                        if (members.List.Count == 0) return null;
                        return fastCSharp.setup.cSharp.serialize.cSharp.PartCode(Method.Method.ReflectedType.Assembly, "BODY"
                            , InputParameterTypeName, members.List.ToArray(), fastCSharp.setup.cSharp.serialize.SerializeAttribute, true);
                    }
                }
                /// <summary>
                /// 返回值参数名称
                /// </summary>
                public string ReturnName
                {
                    get { return ReturnParameterName; }
                }
                /// <summary>
                /// 泛型返回值参数类型名称
                /// </summary>
                public string ReturnTypeName
                {
                    get { return ReturnParameterTypeName; }
                }
                /// <summary>
                /// 输出参数名称
                /// </summary>
                public string OutputParameterTypeName
                {
                    get
                    {
                        return "_o" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// 输出参数名称
                /// </summary>
                public string OutputParameterGenericTypeName
                {
                    get
                    {
                        return OutputParameterTypeName;
                    }
                }
                /// <summary>
                /// 输出参数序列化代码
                /// </summary>
                public string OutputParameterSerialize
                {
                    get
                    {
                        int index = 0;
                        parameterInfo[] parameters = Method.OutputParameters;
                        list<fastCSharp.setup.cSharp.memberInfo>.unsafer members = new list<memberInfo>(parameters.length() + (MethodReturnType.Type == typeof(void) ? 0 : 1)).Unsafer;
                        if (MethodReturnType.Type != typeof(void)) members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = ReturnName, MemberType = methodReturnType.GenericParameterType, MemberIndex = index++ });
                        foreach (parameterInfo parameter in parameters)
                        {
                            members.Add(new fastCSharp.setup.cSharp.memberInfo { MemberName = parameter.ParameterName, MemberType = parameter.ParameterType.GenericParameterType, MemberIndex = index++ });
                        }
                        if (members.List.Count == 0) return null;
                        return fastCSharp.setup.cSharp.serialize.cSharp.PartCode(Method.Method.ReflectedType.Assembly, "BODY"
                            , OutputParameterTypeName, members.List.ToArray(), fastCSharp.setup.cSharp.serialize.SerializeAttribute, true);
                    }
                }
                /// <summary>
                /// 泛型函数信息名称
                /// </summary>
                public string GenericMethodInfoName
                {
                    get
                    {
                        return "_g" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// TCP调用命令名称
                /// </summary>
                public string MethodCommandName
                {
                    get
                    {
                        return "_c" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// 客户端调用是否支持异步
                /// </summary>
                public bool IsClientAsynchronous
                {
                    get
                    {
                        return Attribute.IsClientAsynchronous && Method.OutputParameters.Length == 0;
                    }
                }
                /// <summary>
                /// 异步处理类索引名称
                /// </summary>
                public string AsyncIndexName
                {
                    get
                    {
                        return "_s" + MethodIndex.toString() + Method.GenericParameterName;
                    }
                }
                /// <summary>
                /// 是否泛型类型 或者 函数是否拥有输入参数
                /// </summary>
                public bool IsGenericTypeOrParameter
                {
                    get
                    {
                        return MethodType.Type.IsGenericType && Method.Parameters.Length != 0;
                    }
                }
                /// <summary>
                /// 引用参数是否需要反射泛型调用
                /// </summary>
                public bool IsInvokeGenericMethod
                {
                    get
                    {
                        return MethodType.Type.IsGenericType || Method.Method.IsGenericMethod;
                    }
                }
            }
            /// <summary>
            /// 方法索引集合
            /// </summary>
            public methodIndex[] MethodIndexs;
            /// <summary>
            /// 方法泛型参数类型名称
            /// </summary>
            public string GenericParameterTypeName
            {
                get { return genericParameterTypeName; }
            }
            /// <summary>
            /// 泛型类型服务器端调用类型名称
            /// </summary>
            public string GenericTypeServerName
            {
                get { return genericTypeServerName; }
            }
            /// <summary>
            /// TCP验证类型
            /// </summary>
            public virtual string TcpVerifyType
            {
                get { return Attribute.VerifyType.fullName(); }
            }
            /// <summary>
            /// 是否存在泛型函数
            /// </summary>
            public bool IsAnyGenericMethod
            {
                get
                {
                    return MethodIndexs.any(value => value.Method.Method.IsGenericMethod);
                }
            }
            /// <summary>
            /// 客户端位置
            /// </summary>
            public string ClientPart
            {
                get { return clientPart; }
            }
        }
    }
}
