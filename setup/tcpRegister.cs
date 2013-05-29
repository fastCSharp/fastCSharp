using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;

namespace fastCSharp.setup
{
    /// <summary>
    /// TCP注册服务
    /// </summary>
    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpRegister", IsAsynchronous = true, VerifyType = typeof(tcpRegister.verify))]
    public partial class tcpRegister
    {
        /// <summary>
        /// TCP注册服务验证接口示例
        /// </summary>
        public class verify : fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous
        {
            /// <summary>
            /// TCP注册服务验证数据
            /// </summary>
            private static readonly byte[] verifyData = fastCSharp.config.setup.Default.TcpRegisterVerify != null ? fastCSharp.config.setup.Default.TcpRegisterVerify.serializeNotNull() : null;
            /// <summary>
            /// TCP注册服务验证连续失败次数
            /// </summary>
            private Dictionary<string, int> errorCounts = new Dictionary<string, int>();
            /// <summary>
            /// TCP注册服务验证连续失败次数 访问锁
            /// </summary>
            private int errorLock;
            /// <summary>
            /// TCP注册服务端异步验证
            /// </summary>
            private struct asynchronous
            {
                /// <summary>
                /// TCP注册服务验证实例
                /// </summary>
                public verify Verify;
                /// <summary>
                /// 验证IP地址
                /// </summary>
                public string Ip;
                /// <summary>
                /// TCP服务端异步套接字
                /// </summary>
                public fastCSharp.net.tcpServerAsynchronous.socket Socket;
                /// <summary>
                /// TCP注册服务验证后的处理
                /// </summary>
                public action<bool> OnVerify;
                /// <summary>
                /// 开始验证
                /// </summary>
                public void Start()
                {
                    bool isVerify = true;
                    try
                    {
                        if (verifyData == null)
                        {
                            isVerify = false;
                            log.Default.Add("TCP服务注册验证数据不能为空", false, true);
                        }
                        else
                        {
                            IPAddress ipAddress = Socket.IpAddress;
                            if (ipAddress == null)
                            {
                                isVerify = false;
                                log.Default.Add("客户端IP地址不可识别", false, false);
                            }
                            else if (!Verify.checkError(Ip = ipAddress.ToString())) isVerify = false;
                        }
                    }
                    catch (Exception error)
                    {
                        isVerify = false;
                        log.Default.Add(error, null, false);
                    }
                    finally
                    {
                        if (isVerify) Socket.ReceiveInt(onReceiveLength);
                        else OnVerify(false);
                    }
                }
                /// <summary>
                /// 接受验证数据长度
                /// </summary>
                /// <param name="length">验证数据长度</param>
                private void onReceiveLength(int? length)
                {
                    if (length != null && (int)length == verifyData.Length) Socket.Receive(onReceiveData, (int)length);
                    else OnVerify(false);
                }
                /// <summary>
                /// 接受验证数据
                /// </summary>
                /// <param name="data">验证数据</param>
                private void onReceiveData(byte[] data)
                {
                    if (data != null && fastCSharp.unsafer.memory.Equal(data, verifyData))
                    {
                        OnVerify(true);
                        fastCSharp.threading.task.Default.Add(Verify.removeError, Ip);
                    }
                    else OnVerify(false);
                }
            }
            /// <summary>
            /// TCP注册服务端验证
            /// </summary>
            /// <param name="socket">客户端连接套接字</param>
            /// <param name="onVerify">验证后的处理</param>
            public void Verify(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify)
            {
                new asynchronous { Verify = this, Socket = socket, OnVerify = onVerify }.Start();
            }
            /// <summary>
            /// 检测IP验证失败次数
            /// </summary>
            /// <param name="ip">IP地址</param>
            /// <returns>是否验证成功</returns>
            private bool checkError(string ip)
            {
                bool isCheck = true;
                while (Interlocked.CompareExchange(ref errorLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    int errorCount;
                    if (errorCounts.TryGetValue(ip, out errorCount))
                    {
                        if (errorCount >= fastCSharp.config.setup.Default.TcpRegisterMaxVerifyError) isCheck = false;
                        else errorCounts[ip] = ++errorCount;
                    }
                    else errorCounts.Add(ip, 1);
                }
                finally { errorLock = 0; }
                return isCheck;
            }
            /// <summary>
            /// 清除验证失败信息
            /// </summary>
            /// <param name="ip">TCP服务端IP地址</param>
            private void removeError(string ip)
            {
                while (Interlocked.CompareExchange(ref errorLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    errorCounts.Remove(ip);
                }
                finally { errorLock = 0; }
            }
            /// <summary>
            /// TCP注册服务验证客户端
            /// </summary>
            /// <param name="client">TCP注册服务客户端</param>
            /// <returns>是否验证成功</returns>
            public bool Verify(fastCSharp.net.tcpClient client)
            {
                if (verifyData == null)
                {
                    log.Default.Add("TCP服务注册验证数据不能为空", false, true);
                    return false;
                }
                return client.SendInt(verifyData.Length) && client.Send(verifyData) && client.IsSuccess();
            }
        }
        /// <summary>
        /// TCP注册服务 客户端
        /// </summary>
        public class client : IDisposable
        {
            /// <summary>
            /// TCP注册服务名称
            /// </summary>
            private string serviceName;
            /// <summary>
            /// TCP服务配置信息
            /// </summary>
            private fastCSharp.setup.cSharp.tcpServer attribute;
            /// <summary>
            /// TCP服务端标识
            /// </summary>
            private clientId clientId;
            /// <summary>
            /// TCP注册服务客户端
            /// </summary>
            private tcpClient.tcpRegister registerClient;
            /// <summary>
            /// TCP注册服务轮询客户端
            /// </summary>
            private tcpClient.tcpRegister pollClient;
            /// <summary>
            /// TCP服务信息
            /// </summary>
            private Dictionary<string, services> services;
            /// <summary>
            /// TCP注册服务访问锁
            /// </summary>
            private readonly object clientLock = new object();
            /// <summary>
            /// 创建TCP注册服务客户端失败是否输出日志
            /// </summary>
            private bool isNewClientErrorLog;
            /// <summary>
            /// TCP注册服务客户端
            /// </summary>
            /// <param name="serviceName">TCP注册服务服务名称</param>
            public client(string serviceName)
            {
                this.serviceName = serviceName;
                attribute = fastCSharp.config.pub.Default.LoadConfig(new fastCSharp.setup.cSharp.tcpServer(), serviceName);
                isNewClientErrorLog = true;
                start();
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                Monitor.Enter(clientLock);
                try
                {
                    dispose();
                }
                finally { Monitor.Exit(clientLock); }
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            private void dispose()
            {
                if (pollClient != null)
                {
                    pollClient.Dispose();
                    pollClient = null;
                }
                if (registerClient != null)
                {
                    if (clientId.Tick != 0)
                    {
                        try
                        {
                            registerClient.removeRegister(clientId);
                        }
                        catch (Exception error)
                        {
                            log.Default.Add(error, null, false);
                        }
                        clientId.Tick = 0;
                    }
                    registerClient.Dispose();
                    registerClient = null;
                }
                services = null;
            }
            /// <summary>
            /// 启动TCP注册服务客户端
            /// </summary>
            private void start()
            {
                Monitor.Enter(clientLock);
                try
                {
                    newRegisterClient();
                }
                finally { Monitor.Exit(clientLock); }
            }
            /// <summary>
            /// 创建TCP注册服务客户端
            /// </summary>
            /// <returns>TCP注册服务客户端,失败返回null</returns>
            private tcpClient.tcpRegister newClient()
            {
                try
                {
                    tcpClient.tcpRegister client = new tcpClient.tcpRegister(attribute);
                    if (client._IsClientStart_)
                    {
                        isNewClientErrorLog = true;
                        return client;
                    }
                }
                catch (Exception error)
                {
                    if (isNewClientErrorLog)
                    {
                        isNewClientErrorLog = false;
                        log.Default.Add(error, null, false);
                    }
                }
                return null;
            }
            /// <summary>
            /// 创建TCP注册服务客户端
            /// </summary>
            private void newRegisterClient()
            {
                bool isClient = false;
                registerClient = newClient();
                if (registerClient != null)
                {
                    try
                    {
                        clientId = registerClient.register();
                        services = registerClient.getServices(out clientId.Version).getDictionary(value => value.Name);
                        isClient = true;
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                        Dispose();
                    }
                }
                if (isClient) fastCSharp.threading.task.Default.Add(newPollClient);
                else fastCSharp.threading.timerTask.Default.Add(start, fastCSharp.date.NowTime.AddSeconds(1));
            }
            /// <summary>
            /// 创建TCP注册服务轮询客户端
            /// </summary>
            private void newPollClient()
            {
                tcpClient.tcpRegister client = newClient();
                if (client != null)
                {
                    Monitor.Enter(clientLock);
                    pollClient = client;
                    Monitor.Exit(clientLock);
                    pollClient.poll(poll, clientId);
                }
                else fastCSharp.threading.timerTask.Default.Add(newPollClient, fastCSharp.date.NowTime.AddSeconds(1));
            }
            /// <summary>
            /// 客户端轮询
            /// </summary>
            /// <param name="result">轮询结果</param>
            private void poll(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<fastCSharp.setup.tcpRegister.pollResult> result)
            {
                if (result.IsReturn)
                {
                    switch (result.Value.State)
                    {
                        case pollState.Check:
                            pollClient.poll(poll, clientId);
                            break;
                        case pollState.RegisterChange:
                            Monitor.Enter(clientLock);
                            try
                            {
                                if (clientId.Version < result.Value.Version)
                                {
                                    clientId.Version = result.Value.Version;
                                    foreach (services service in result.Value.Services) services[service.Name] = service;
                                }
                            }
                            finally
                            {
                                Monitor.Exit(clientLock);
                                pollClient.poll(poll, clientId);
                            }
                            break;
                        case pollState.VersionError:
                            Monitor.Enter(clientLock);
                            try
                            {
                                services = registerClient.getServices(out clientId.Version).getDictionary(value => value.Name);
                            }
                            finally
                            {
                                Monitor.Exit(clientLock);
                                pollClient.poll(poll, clientId);
                            }
                            break;
                        case pollState.ClientError:
                            Dispose();
                            start();
                            break;
                        case pollState.NewClient:
                            log.Default.Add(serviceName + " 轮询客户端冲突", false, false);
                            break;
                        default:
                            log.Default.Add("不可识别的轮询状态 " + result.Value.State.ToString(), false, false);
                            break;
                    }
                }
                else if (pollClient._IsClientStart_) pollClient.poll(poll, clientId);
                else
                {
                    Monitor.Enter(clientLock);
                    try
                    {
                        if (pollClient != null)
                        {
                            pollClient.Dispose();
                            pollClient = null;
                        }
                    }
                    finally { Monitor.Exit(clientLock); }
                    fastCSharp.threading.timerTask.Default.Add(newPollClient, fastCSharp.date.NowTime.AddSeconds(1));
                }
            }
            /// <summary>
            /// 注册TCP服务端
            /// </summary>
            /// <param name="attribute">TCP服务配置</param>
            /// <returns>是否注册成功</returns>
            public registerState Register(fastCSharp.setup.cSharp.tcpServer attribute)
            {
                registerResult result = new registerResult { State = registerState.NoClient };
                bool isNewClient = false;
                Monitor.Enter(clientLock);
                try
                {
                    if (registerClient == null)
                    {
                        isNewClient = true;
                        newRegisterClient();
                    }
                    if (registerClient != null)
                    {
                        result = register(attribute);
                        if (result.State == registerState.NoClient && !isNewClient)
                        {
                            newRegisterClient();
                            result = register(attribute);
                        }
                    }
                }
                finally { Monitor.Exit(clientLock); }
                if (result.State == registerState.Success)
                {
                    attribute.Host = result.Service.Host.Host;
                    attribute.Port = result.Service.Host.Port;
                }
                return result.State;
            }
            /// <summary>
            /// 注册TCP服务端
            /// </summary>
            /// <param name="attribute">TCP服务配置</param>
            /// <returns>是否注册成功</returns>
            private registerResult register(fastCSharp.setup.cSharp.tcpServer attribute)
            {
                try
                {
                    return registerClient.register(clientId, new service { Host = new host { Host = attribute.Host, Port = attribute.Port }, Name = attribute.ServiceName, IsSingle = attribute.IsSingleRegister });
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                }
                return new registerResult { State = registerState.NoClient };
            }
            /// <summary>
            /// 获取TCP服务端注册信息
            /// </summary>
            /// <param name="attribute">TCP服务配置</param>
            /// <returns>是否获取成功</returns>
            public bool GetRegister(fastCSharp.setup.cSharp.tcpServer attribute)
            {
                Dictionary<string, services> services =this.services;
                services service;
                if (services != null && services.TryGetValue(attribute.ServiceName, out service))
                {
                    host host = service.Hosts[0];
                    attribute.Host = host.Host;
                    attribute.Port = host.Port;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// TCP注册服务客户端缓存
            /// </summary>
            private static Dictionary<string, client> clients = new Dictionary<string, client>();
            /// <summary>
            /// TCP注册服务客户端 访问锁
            /// </summary>
            private static readonly object clientsLock = new object();
            /// <summary>
            /// 关闭TCP注册服务客户端
            /// </summary>
            private static void disposeClients()
            {
                client[] clientArray = null;
                Monitor.Enter(clientsLock);
                try
                {
                    clientArray = clients.Values.getArray();
                    clients = null;
                }
                finally { Monitor.Exit(clientsLock); }
                foreach (client client in clientArray) client.Dispose();
            }
            /// <summary>
            /// 获取TCP注册服务客户端
            /// </summary>
            /// <param name="serviceName">服务名称</param>
            /// <returns>TCP注册服务客户端,失败返回null</returns>
            public static client Get(string serviceName)
            {
                if (serviceName.length() != 0)
                {
                    int count = int.MinValue;
                    client client = null;
                    Monitor.Enter(clientsLock);
                    try
                    {
                        if (clients != null && !clients.TryGetValue(serviceName, out client))
                        {
                            try
                            {
                                client = new client(serviceName);
                            }
                            catch (Exception error)
                            {
                                log.Default.Add(error, null, false);
                            }
                            if (client != null)
                            {
                                count = clients.Count;
                                clients.Add(serviceName, client);
                            }
                        }
                    }
                    finally { Monitor.Exit(clientsLock); }
                    if (count == 0) fastCSharp.domainUnload.Add(disposeClients);
                    return client;
                }
                return null;
            }
        }
        /// <summary>
        /// TCP服务端标识
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial struct clientId : IEquatable<clientId>
        {
            /// <summary>
            /// TCP注册服务进程时间
            /// </summary>
            public long Tick;
            /// <summary>
            /// 进程级唯一编号
            /// </summary>
            public int Identity;
            /// <summary>
            /// 注册信息版本
            /// </summary>
            public int Version;
            /// <summary>
            /// 获取哈希值
            /// </summary>
            /// <returns>哈希值</returns>
            public override int GetHashCode()
            {
                return (int)(Tick >> 32) ^ (int)Tick ^ Identity;
            }
            /// <summary>
            /// 判断是否同一TCP服务端
            /// </summary>
            /// <param name="other">TCP服务端</param>
            /// <returns>是否同一TCP服务端</returns>
            public override bool Equals(object other)
            {
                return Equals((clientId)other);
            }
            /// <summary>
            /// 判断是否同一TCP服务端
            /// </summary>
            /// <param name="other">TCP服务端</param>
            /// <returns>是否同一TCP服务端</returns>
            public bool Equals(clientId other)
            {
                return Tick == other.Tick && Identity == other.Identity;
            }
        }
        /// <summary>
        /// TCP服务端口信息
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial struct host : IEquatable<host>
        {
            /// <summary>
            /// 主机名称或者IP地址
            /// </summary>
            public string Host;
            /// <summary>
            /// 端口号
            /// </summary>
            public int Port;
            /// <summary>
            /// 主机名称转换成IP地址
            /// </summary>
            /// <returns>是否转换成功</returns>
            public bool HostToIpAddress()
            {
                IPAddress ipAddress = fastCSharp.setup.cSharp.tcpBase.HostToIpAddress(Host);
                if (ipAddress == null) return false;
                Host = ipAddress.ToString();
                return true;
            }
            /// <summary>
            /// 获取哈希值
            /// </summary>
            /// <returns>哈希值</returns>
            public override int GetHashCode()
            {
                return Host.GetHashCode() ^ Port;
            }
            /// <summary>
            /// 判断是否TCP服务端口信息
            /// </summary>
            /// <param name="other">TCP服务端口信息</param>
            /// <returns>是否同一TCP服务端口信息</returns>
            public override bool Equals(object other)
            {
                return Equals((host)other);
            }
            /// <summary>
            /// 判断是否TCP服务端口信息
            /// </summary>
            /// <param name="other">TCP服务端口信息</param>
            /// <returns>是否同一TCP服务端口信息</returns>
            public bool Equals(host other)
            {
                return Host == other.Host && Port == other.Port;
            }
        }
        /// <summary>
        /// TCP服务信息
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial struct service
        {
            /// <summary>
            /// TCP服务名称标识
            /// </summary>
            public string Name;
            /// <summary>
            /// 端口信息集合
            /// </summary>
            public host Host;
            /// <summary>
            /// 是否只允许一个TCP服务实例
            /// </summary>
            public bool IsSingle;
        }
        /// <summary>
        /// TCP服务信息集合
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial struct services
        {
            /// <summary>
            /// TCP服务名称标识
            /// </summary>
            public string Name;
            /// <summary>
            /// 端口信息集合
            /// </summary>
            public host[] Hosts;
            /// <summary>
            /// 是否只允许一个TCP服务实例
            /// </summary>
            public bool IsSingle;
        }
        /// <summary>
        /// 注册状态
        /// </summary>
        public enum registerState
        {
            /// <summary>
            /// 客户端不可用
            /// </summary>
            NoClient,
            /// <summary>
            /// 客户端标识错误
            /// </summary>
            ClientError,
            /// <summary>
            /// 单例服务冲突
            /// </summary>
            SingleError,
            /// <summary>
            /// TCP服务端口信息不合法
            /// </summary>
            HostError,
            /// <summary>
            /// TCP服务端口信息已存在
            /// </summary>
            HostExists,
            /// <summary>
            /// 没有可用的端口号
            /// </summary>
            PortError,
            /// <summary>
            /// TCP服务信息检测被更新,需要重试
            /// </summary>
            ServiceChange,
            /// <summary>
            /// 注册成功
            /// </summary>
            Success,
        }
        /// <summary>
        /// 注册结果
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial struct registerResult
        {
            /// <summary>
            /// 注册状态
            /// </summary>
            public registerState State;
            /// <summary>
            /// 注册成功的TCP服务信息
            /// </summary>
            public service Service;
        }
        /// <summary>
        /// 轮询状态
        /// </summary>
        public enum pollState
        {
            /// <summary>
            /// 客户端标识错误
            /// </summary>
            ClientError,
            /// <summary>
            /// 检测是否在线
            /// </summary>
            Check,
            /// <summary>
            /// TCP服务信息版本号不匹配
            /// </summary>
            VersionError,
            /// <summary>
            /// 轮询TCP客户端冲突
            /// </summary>
            NewClient,
            /// <summary>
            /// TCP服务端注册更新
            /// </summary>
            RegisterChange,
        }
        /// <summary>
        /// 轮询结果
        /// </summary>
        [fastCSharp.setup.cSharp.serialize]
        public partial class pollResult
        {
            /// <summary>
            /// 轮询状态
            /// </summary>
            public pollState State;
            /// <summary>
            /// TCP服务端注册信息
            /// </summary>
            public services[] Services;
            /// <summary>
            /// TCP服务端注册版本号
            /// </summary>
            public int Version;
        }
        /// <summary>
        /// TCP服务端在线检测
        /// </summary>
        private struct checkClient
        {
            /// <summary>
            /// TCP服务端标识
            /// </summary>
            public clientId Client;
            /// <summary>
            /// 轮询委托
            /// </summary>
            public action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> Action;
            /// <summary>
            /// 最后检测时间
            /// </summary>
            public DateTime CheckTime;
            /// <summary>
            /// 是否已检测
            /// </summary>
            public bool IsCheck;
            /// <summary>
            /// TCP服务端在线检测
            /// </summary>
            /// <param name="client">TCP服务端标识</param>
            /// <param name="action">轮询委托</param>
            /// <param name="checkTime">最后检测时间</param>
            public void Set(clientId client
                , action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> action, DateTime checkTime)
            {
                Client = client;
                Action = action;
                CheckTime = checkTime;
                IsCheck = false;
            }
            /// <summary>
            /// 设置检测状态
            /// </summary>
            public void Check()
            {
                Action = null;
                IsCheck = true;
            }
        }
        /// <summary>
        /// TCP服务端轮询检测时间(秒数)
        /// </summary>
        private const int pollCheckSeconds = 60;
        /// <summary>
        /// TCP服务信息集合
        /// </summary>
        private Dictionary<string, services> serviceCache;
        /// <summary>
        /// TCP服务信息 访问锁
        /// </summary>
        private readonly object serviceLock = new object();
        /// <summary>
        /// TCP服务信息 版本号
        /// </summary>
        private int serviceVersion;
        /// <summary>
        /// 轮询TCP服务端集合
        /// </summary>
        private readonly Dictionary<clientId, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>>> pollClients = new Dictionary<clientId, action<cSharp.tcpBase.asynchronousReturn<pollResult>>>();
        /// <summary>
        /// TCP服务端最后一次响应时间
        /// </summary>
        private readonly Dictionary<clientId, DateTime> clientCheckTimes = new Dictionary<clientId, DateTime>();
        /// <summary>
        /// TCP服务端信息集合
        /// </summary>
        private readonly Dictionary<host, clientId> hostClients = new Dictionary<host, clientId>();
        /// <summary>
        /// TCP服务端口信息集合
        /// </summary>
        private readonly Dictionary<string, int> hostPorts = new Dictionary<string, int>();
        /// <summary>
        /// 下一次轮询检测时间
        /// </summary>
        private DateTime nextCheckTime;
        /// <summary>
        /// TCP服务信息集合保存文件访问锁
        /// </summary>
        private int saveLock;
        /// <summary>
        /// 缓存文件名称
        /// </summary>
        private string cacheFile;
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public void SetTcpServer(fastCSharp.net.tcpServerBase tcpServer)
        {
            cacheFile = fastCSharp.config.pub.Default.CachePath + tcpServer.ServiceName + @".cache";
            serviceCache = fromCacheFile() ?? new Dictionary<string, services>();
            fastCSharp.threading.timerTask.Default.Add(checkPoll, nextCheckTime = fastCSharp.date.NowTime.AddSeconds(pollCheckSeconds));
        }
        /// <summary>
        /// TCP服务端注册
        /// </summary>
        /// <returns>TCP服务端标识</returns>
        [fastCSharp.setup.cSharp.tcpServer]
        private clientId register()
        {
            return new clientId { Tick = fastCSharp.pub.StartTime.Ticks, Identity = fastCSharp.threading.identity.Default.Next(), Version = int.MinValue };
        }
        /// <summary>
        /// 获取TCP服务信息集合
        /// </summary>
        /// <param name="version">TCP服务信息 版本号</param>
        /// <returns>TCP服务信息集合</returns>
        [fastCSharp.setup.cSharp.tcpServer]
        private services[] getServices(out int version)
        {
            services[] services = null;
            Monitor.Enter(serviceLock);
            try
            {
                version = serviceVersion;
                services = serviceCache.Values.getArray();
            }
            finally { Monitor.Exit(serviceLock); }
            return services;
        }
        /// <summary>
        /// 注册TCP服务信息
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        /// <param name="service">TCP服务信息</param>
        /// <returns>注册状态</returns>
        [fastCSharp.setup.cSharp.tcpServer]
        private registerResult register(clientId client, service service)
        {
            if (client.Tick != fastCSharp.pub.StartTime.Ticks) return new registerResult { State = registerState.ClientError };
            if (!service.Host.HostToIpAddress()) return new registerResult { State = registerState.HostError };
            services oldService, newService = new services { Name = service.Name };
            checkClient[] checkClients = null;
            int version = int.MinValue;
            Monitor.Enter(serviceLock);
            try
            {
                if (service.Host.Port != 0 && hostClients.ContainsKey(service.Host)) return new registerResult { State = registerState.HostExists };
                if (serviceCache.TryGetValue(service.Name, out oldService))
                {
                    if (oldService.IsSingle || service.IsSingle)
                    {
                        int hostIndex = 0;
                        action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> action;
                        DateTime checkTime;
                        checkClients = new checkClient[oldService.Hosts.Length];
                        foreach (host host in oldService.Hosts)
                        {
                            clientId checkClient = hostClients[host];
                            if (!clientCheckTimes.TryGetValue(checkClient, out checkTime)) checkTime = DateTime.MinValue;
                            if (pollClients.TryGetValue(checkClient, out action))
                            {
                                checkClients[hostIndex++].Set(checkClient, action, checkTime);
                                pollClients.Remove(checkClient);
                            }
                            else checkClients[hostIndex++].Set(checkClient, null, checkTime);
                        }
                    }
                    else
                    {
                        if (service.Host.Port == 0)
                        {
                            service.Host.Port = getPort(client, service.Host.Host);
                            if (service.Host.Port == 0) return new registerResult { State = registerState.PortError };
                        }
                        newService.Hosts = new host[oldService.Hosts.Length + 1];
                        Array.Copy(oldService.Hosts, 0, newService.Hosts, 0, oldService.Hosts.Length);
                        newService.Hosts[oldService.Hosts.Length] = service.Host;
                        serviceCache[service.Name] = newService;
                        version = ++serviceVersion;
                    }
                }
                else
                {
                    if (service.Host.Port == 0)
                    {
                        service.Host.Port = getPort(client, service.Host.Host);
                        if (service.Host.Port == 0) return new registerResult { State = registerState.PortError };
                    }
                    newService.Hosts = new host[] { service.Host };
                    newService.IsSingle = service.IsSingle;
                    serviceCache.Add(service.Name, newService);
                    version = ++serviceVersion;
                }
            }
            finally { Monitor.Exit(serviceLock); }
            registerState state = registerState.Success;
            if (checkClients != null)
            {
                int actionCount = 0;
                fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult> returnValue = new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult> { IsReturn = true, Value = new pollResult { State = pollState.Check } };
                foreach (checkClient checkClient in checkClients)
                {
                    try
                    {
                        if (checkClient.Action != null)
                        {
                            ++actionCount;
                            checkClient.Action(returnValue);
                        }
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                }
                int checkActionCount = 0, checkTimeCount = 0;
                DateTime checkTime, timeout = DateTime.Now.AddSeconds(fastCSharp.config.setup.Default.TcpRegisterCheckTimeoutSeconds);
                do
                {
                    Thread.Sleep(1);
                    try
                    {
                        if (actionCount != checkActionCount)
                        {
                            int checkIndex = 0;
                            foreach (checkClient checkClient in checkClients)
                            {
                                if (checkClient.Action != null
                                    && clientCheckTimes.TryGetValue(checkClient.Client, out checkTime)
                                    && checkClient.CheckTime != checkTime)
                                {
                                    ++checkActionCount;
                                    ++checkTimeCount;
                                    checkClients[checkIndex].Check();
                                }
                                ++checkIndex;
                            }
                        }
                        if (actionCount == checkActionCount)
                        {
                            int checkIndex = 0;
                            foreach (checkClient checkClient in checkClients)
                            {
                                if (!checkClient.IsCheck
                                    && clientCheckTimes.TryGetValue(checkClient.Client, out checkTime)
                                    && checkClient.CheckTime != checkTime)
                                {
                                    ++checkTimeCount;
                                    checkClients[checkIndex].IsCheck = true;
                                }
                            }
                            if (checkTimeCount == checkClients.Length) break;
                        }
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                }
                while (DateTime.Now < timeout);
                Monitor.Enter(serviceLock);
                try
                {
                    services currentService;
                    if (serviceCache.TryGetValue(service.Name, out currentService) && currentService.Hosts == oldService.Hosts)
                    {
                        if (checkTimeCount == 0)
                        {
                            if (service.Host.Port == 0)
                            {
                                service.Host.Port = getPort(client, service.Host.Host);
                                if (service.Host.Port == 0) return new registerResult { State = registerState.PortError };
                            }
                            newService.Hosts = new host[] { service.Host };
                            newService.IsSingle = service.IsSingle;
                            serviceCache[service.Name] = newService;
                            foreach (host host in oldService.Hosts) hostClients.Remove(host);
                            version = ++serviceVersion;
                        }
                        else
                        {
                            if (checkTimeCount != checkClients.Length)
                            {
                                host[] hosts = new host[checkTimeCount];
                                int checkIndex = 0, hostIndex = 0;
                                foreach (checkClient checkClient in checkClients)
                                {
                                    if (checkClient.IsCheck) hosts[hostIndex++] = oldService.Hosts[checkIndex];
                                    else hostClients.Remove(oldService.Hosts[checkIndex]);
                                    ++checkIndex;
                                }
                                newService.Hosts = hosts;
                                serviceCache[service.Name] = newService;
                                version = ++serviceVersion;
                            }
                            state = registerState.SingleError;
                        }
                    }
                    else return new registerResult { State = registerState.ServiceChange };
                }
                finally { Monitor.Exit(serviceLock); }
            }
            if (version != int.MinValue) fastCSharp.threading.task.Default.Add(onRegister, new keyValue<services, int>(newService, version));
            return new registerResult { State = state, Service = state == registerState.Success ? service : new service() };
        }
        /// <summary>
        /// 获取TCP服务端口号
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        /// <param name="host">主机IP地址</param>
        /// <returns>TCP服务端口号</returns>
        private int getPort(clientId client, string ipAddress)
        {
            host host = new host { Host = ipAddress };
            if (!hostPorts.TryGetValue(ipAddress, out host.Port)) host.Port = fastCSharp.config.setup.Default.TcpRegisterPortStart;
            int startPort = host.Port;
            while (hostClients.ContainsKey(host)) ++host.Port;
            if (host.Port >= 65536)
            {
                host.Port = fastCSharp.config.setup.Default.TcpRegisterPortStart;
                while (host.Port != startPort && hostClients.ContainsKey(host)) ++host.Port;
                if (host.Port == startPort) return 0;
            }
            hostPorts[ipAddress] = host.Port + 1;
            hostClients.Add(host, client);
            return host.Port;
        }
        /// <summary>
        /// TCP服务注册通知轮询TCP服务端
        /// </summary>
        /// <param name="serviceVersion">TCP服务信息</param>
        private void onRegister(keyValue<services, int> serviceVersion)
        {
            callPoll(new pollResult { State = pollState.RegisterChange, Services = new services[] { serviceVersion.Key }, Version = serviceVersion.Value });
        }
        /// <summary>
        /// TCP服务注册通知轮询TCP服务端
        /// </summary>
        /// <param name="result">TCP服务信息</param>
        /// <param name="isCheckTime">是否检测轮询时间</param>
        private void callPoll(pollResult result, bool isCheckTime = false)
        {
            action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>>[] clients = null;
            Monitor.Enter(serviceLock);
            try
            {
                if (!isCheckTime || fastCSharp.date.NowTime >= nextCheckTime)
                {
                    clients = pollClients.Values.getArray();
                    pollClients.Clear();
                }
            }
            finally { Monitor.Exit(serviceLock); }
            if (clients != null)
            {
                fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult> returnValue = new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>
                {
                    IsReturn = true,
                    Value = result
                };
                foreach (action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> client in clients)
                {
                    try
                    {
                        client(returnValue);
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                }
                Monitor.Enter(serviceLock);
                nextCheckTime = fastCSharp.date.NowTime.AddSeconds(pollCheckSeconds);
                Monitor.Exit(serviceLock);
            }
            if (result.State == pollState.RegisterChange) fastCSharp.threading.task.TinyTask.Add(saveCacheFile);
        }
        /// <summary>
        /// TCP服务端轮询
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        /// <param name="onRegisterChanged">TCP服务注册通知委托</param>
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true, IsClientAsynchronous = true, IsClientSynchronous = false)]
        private void poll(clientId client, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> onRegisterChanged)
        {
            if (onRegisterChanged != null)
            {
                pollResult value = null;
                if (client.Tick != fastCSharp.pub.StartTime.Ticks) value = new pollResult { State = pollState.ClientError };
                else
                {
                    Monitor.Enter(serviceLock);
                    try
                    {
                        if (client.Version != serviceVersion) value = new pollResult { State = pollState.VersionError };
                        else
                        {
                            action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult>> pollAction;
                            if (pollClients.TryGetValue(client, out pollAction))
                            {
                                pollClients[client] = onRegisterChanged;
                                onRegisterChanged = pollAction;
                                value = new pollResult { State = pollState.NewClient };
                            }
                            else pollClients.Add(client, onRegisterChanged);

                            DateTime pollTime;
                            if (clientCheckTimes.TryGetValue(client, out pollTime))
                            {
                                if (pollTime == fastCSharp.date.NowTime) clientCheckTimes[client] = pollTime.AddMilliseconds(1);
                            }
                            else clientCheckTimes.Add(client, fastCSharp.date.NowTime);

                        }
                    }
                    finally { Monitor.Exit(serviceLock); }
                }
                if (value != null) onRegisterChanged(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<pollResult> { IsReturn = true, Value = value });
            }
        }
        /// <summary>
        /// 轮询检测
        /// </summary>
        private void checkPoll()
        {
            try
            {
                callPoll(new pollResult { State = pollState.Check }, true);
            }
            finally { fastCSharp.threading.timerTask.Default.Add(checkPoll, nextCheckTime); }
        }
        /// <summary>
        /// 注销TCP服务信息
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        /// <param name="serviceName">TCP服务名称</param>
        [fastCSharp.setup.cSharp.tcpServer]
        private void removeRegister(clientId client, string serviceName)
        {
            if (client.Tick == fastCSharp.pub.StartTime.Ticks)
            {
                services service;
                int version = int.MinValue;
                Monitor.Enter(serviceLock);
                try
                {
                    if (serviceCache.TryGetValue(serviceName, out service))
                    {
                        host[] hosts = removeRegister(client, service);
                        if (hosts != service.Hosts)
                        {
                            version = ++serviceVersion;
                            service.Hosts = hosts;
                        }
                    }
                }
                finally { Monitor.Exit(serviceLock); }
                if (version != int.MinValue) fastCSharp.threading.task.Default.Add(onRegister, new keyValue<services, int>(service, version));
            }
        }
        /// <summary>
        /// 注销TCP服务信息
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        /// <param name="serviceName">TCP服务信息</param>
        /// <returns>注销操作之后的TCP服务端口信息集合</returns>
        private unsafe host[] removeRegister(clientId client, services service)
        {
            int count = (service.Hosts.Length + 7) >> 3, index = 0;
            byte* isRemove = stackalloc byte[count];
            fixedMap removeMap = new fixedMap(isRemove, count);
            count = 0;
            foreach (host host in service.Hosts)
            {
                if (client.Equals(hostClients[host])) removeMap.Set(index);
                else ++count;
                ++index;
            }
            if (count != service.Hosts.Length)
            {
                if (count == 0)
                {
                    serviceCache.Remove(service.Name);
                    foreach (host host in service.Hosts) hostClients.Remove(host);
                    return null;
                }
                host[] hosts = new host[count];
                count = index = 0;
                foreach (host host in service.Hosts)
                {
                    if (removeMap.Get(index++)) hostClients.Remove(host);
                    else hosts[count++] = host;
                }
                service.Hosts = hosts;
                serviceCache[service.Name] = service;
            }
            return service.Hosts;
        }
        /// <summary>
        /// 注销TCP服务信息
        /// </summary>
        /// <param name="client">TCP服务端标识</param>
        [fastCSharp.setup.cSharp.tcpServer]
        private void removeRegister(clientId client)
        {
            if (client.Tick == fastCSharp.pub.StartTime.Ticks)
            {
                list<services>.unsafer removeServices;
                int version = int.MinValue;
                Monitor.Enter(serviceLock);
                try
                {
                    removeServices = new list<services>(serviceCache.Count).Unsafer;
                    foreach (services service in serviceCache.Values.getArray())
                    {
                        host[] hosts = removeRegister(client, service);
                        if (hosts != service.Hosts) removeServices.Add(new services { Name = service.Name, Hosts = hosts, IsSingle = service.IsSingle });
                    }
                    if (removeServices.List.Count != 0) version = ++serviceVersion;
                }
                finally { Monitor.Exit(serviceLock); }
                if (version != int.MinValue) fastCSharp.threading.task.Default.Add(onRegister, new keyValue<list<services>, int>(removeServices.List, version));
            }
        }
        /// <summary>
        /// TCP服务注册通知轮询TCP服务端
        /// </summary>
        /// <param name="serviceVersion">TCP服务信息</param>
        private void onRegister(keyValue<list<services>, int> serviceVersion)
        {
            callPoll(new pollResult { State = pollState.RegisterChange, Services = serviceVersion.Key.ToArray(), Version = serviceVersion.Value });
        }
        /// <summary>
        /// 保存TCP服务信息集合到缓存文件
        /// </summary>
        private void saveCacheFile()
        {
            while (Interlocked.CompareExchange(ref saveLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                services[] services = null;
                Monitor.Enter(serviceLock);
                try
                {
                    services = serviceCache.Values.getArray();
                }
                finally { Monitor.Exit(serviceLock); }
                if (services != null) File.WriteAllBytes(cacheFile, fastCSharp.setup.cSharp.serialize.dataSerialize.Get<services[]>(services));
            }
            finally { saveLock = 0; }
        }
        /// <summary>
        /// 从缓存文件恢复TCP服务信息集合
        /// </summary>
        /// <returns>TCP服务信息集合</returns>
        private Dictionary<string, services> fromCacheFile()
        {
            Dictionary<string, services> services = null;
            if (File.Exists(cacheFile))
            {
                try
                {
                    services = fastCSharp.setup.cSharp.serialize.deSerialize.Get<services[]>(File.ReadAllBytes(cacheFile))
                        .getDictionary(value => value.Name);
                    //log.Default.Add(cacheFile + " " + services.Count.toString(), false, false);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    try
                    {
                        File.Delete(cacheFile);
                    }
                    catch (Exception error1)
                    {
                        log.Default.Add(error1, null, false);
                    }
                }
            }
            return services;
        }
    }
}
