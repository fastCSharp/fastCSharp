using System;
using System.Threading;
using System.Net.Sockets;
using fastCSharp.threading;
using System.Reflection;

namespace fastCSharp.net
{
    /// <summary>
    /// TCP调用服务端基类
    /// </summary>
    public abstract class tcpServerBase : IDisposable
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected fastCSharp.setup.cSharp.tcpServer attribute;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return attribute.ServiceName; }
        }
        /// <summary>
        /// TCP监听服务器端连接
        /// </summary>
        protected TcpListener listener;
        /// <summary>
        /// 是否已启动服务
        /// </summary>
        protected int isStart;
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        /// <param name="attribute">配置信息</param>
        protected tcpServerBase(fastCSharp.setup.cSharp.tcpServer attribute)
        {
            if (attribute == null) log.Default.Throw(log.exceptionType.Null);
            if (attribute.TcpRegisterName != null)
            {
                fastCSharp.setup.tcpRegister.client client = fastCSharp.setup.tcpRegister.client.Get(attribute.TcpRegisterName);
                if (client == null) log.Default.Throw("TCP注册服务 " + attribute.TcpRegisterName + " 链接失败", true, false);
                fastCSharp.setup.tcpRegister.registerState state = client.Register(attribute);
                if (state != fastCSharp.setup.tcpRegister.registerState.Success) log.Default.Throw("TCP服务注册 " + attribute.ServiceName + " 失败 " + state.ToString(), true, false);
                log.Default.Add(attribute.ServiceName + " 注册 " + attribute.Host + ":" + attribute.Port.toString());
            }
            if (!attribute.IsServer) log.Default.Add("配置未指明的TCP服务端", true, false);
            this.attribute = attribute;
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref isStart, 0) == 1)
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        protected bool start()
        {
            if (Interlocked.Exchange(ref isStart, 1) == 0)
            {
                try
                {
                    listener = new TcpListener(attribute.IpAddress, attribute.Port);
                    listener.Start();
                }
                catch (Exception error)
                {
                    Dispose();
                    log.Default.ThrowReal(error, GetType().FullName + "服务器端口 " + attribute.Host + ":" + attribute.Port.toString() + " TCP连接失败)", false);
                }
                return isStart != 0;
            }
            return false;
        }
    }
    /// <summary>
    /// TCP调用服务端
    /// </summary>
    /// <typeparam name="socketType">套接字类型</typeparam>
    /// <typeparam name="verifyType">验证接口类型</typeparam>
    public abstract class tcpServer<socketType, verifyType> : tcpServerBase
    {
        /// <summary>
        /// TCP客户端验证接口
        /// </summary>
        protected verifyType verify;
        /// <summary>
        /// 命令处理委托集合
        /// </summary>
        protected staticDictionary<hashBytes, keyValue<action<socketType, int>, bool>> onCommands;
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        /// <param name="attribute">配置信息</param>
        /// <param name="verify">TCP客户端验证接口</param>
        public tcpServer(fastCSharp.setup.cSharp.tcpServer attribute, verifyType verify)
            : base(attribute)
        {
            if (verify == null) log.Default.Add("缺少TCP客户端验证接口", true, false);
            this.verify = verify;
        }
    }
    /// <summary>
    /// TCP调用服务端
    /// </summary>
    public abstract class tcpServer : tcpServer<tcpServer.socket, fastCSharp.setup.cSharp.tcpBase.ITcpVerify>
    {
        /// <summary>
        /// 关闭链接命令
        /// </summary>
        protected internal static readonly hashBytes closeCommandData = new hashBytes(new byte[] { 0xff, 0xff, 0xff, 0xff });
        /// <summary>
        /// 服务状态
        /// </summary>
        public enum status : byte
        {
            /// <summary>
            /// 未知状态
            /// </summary>
            None,
            /// <summary>
            /// 错误状态
            /// </summary>
            Error,
            /// <summary>
            /// 成功状态
            /// </summary>
            Success,
        }
        /// <summary>
        /// 同步套接字
        /// </summary>
        public unsafe class socket : topSocket
        {
            /// <summary>
            /// 初始化套接字异步操作
            /// </summary>
            /// <param name="socket">异步操作套接字</param>
            /// <param name="data">数据缓存</param>
            /// <param name="onClose">关闭套接字处理</param>
            /// <param name="dataPoint">数据缓存起始位置</param>
            public socket(Socket socket, byte[] data, action<topSocket> onClose, byte* dataPoint)
                : base(socket, data, onClose, dataPoint) { }
            /// <summary>
            /// 发送缓存数据
            /// </summary>
            /// <param name="length">发送长度</param>
            /// <returns>是否成功</returns>
            protected internal bool send(int length)
            {
                return send(data, 0, length);
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="data">待发送数据</param>
            /// <returns>是否成功</returns>
            protected internal bool send(byte[] data)
            {
                return send(data, 0, data.Length);
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否发送成功</returns>
            public bool SendInt(int value)
            {
                *(int*)dataPoint = value;
                return send(data, 0, sizeof(int));
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="data">数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="length">发送长度</param>
            /// <returns>是否成功</returns>
            public bool Send(byte[] data, int startIndex, int length)
            {
                if (length != 0 && startIndex >= 0)
                {
                    array.range range = new array.range(data.length(), startIndex, length);
                    return range.GetCount == length && send(data, range.SkipCount, length);
                }
                return false;
            }
            /// <summary>
            /// 接收整数值
            /// </summary>
            /// <param name="value">整数值</param>
            /// <returns>是否成功</returns>
            public bool ReceiveInt(out int value)
            {
                if (receiveInt())
                {
                    value = *(int*)dataPoint;
                    return true;
                }
                value = 0;
                return false;
            }
            /// <summary>
            /// 接收长度+数据
            /// </summary>
            /// <returns>接收数据,失败返回null</returns>
            protected internal byte[] receive()
            {
                int length;
                if (ReceiveInt(out length))
                {
                    int dataLength = length < 0 ? -length : length;
                    byte[] data = this.data;
                    if (data.Length < dataLength)
                    {
                        if (currentData.length() < dataLength) currentData = new byte[dataLength];
                        data = currentData;
                    }
                    if (receive(data, dataLength))
                    {
                        if (length >= 0)
                        {
                            if (data.Length != length) Buffer.BlockCopy(data, 0, currentData = new byte[length], 0, length);
                            else currentData = data;
                        }
                        else currentData = data.getDeCompress(fastCSharp.stream.compression.Deflate, 0, dataLength);
                        return currentData;
                    }
                }
                else if (receiveError == null) receiveError = lengthError;
                return null;
            }
        }
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        /// <param name="attribute">配置信息</param>
        /// <param name="verify">TCP客户端验证接口</param>
        public tcpServer(fastCSharp.setup.cSharp.tcpServer attribute, fastCSharp.setup.cSharp.tcpBase.ITcpVerify verify = null)
            : base(attribute, verify)
        {
            onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>[] { new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)) });
        }
        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="socket">同步套接字</param>
        /// <param name="identity">回话标识</param>
        protected unsafe void close(socket socket, int identity)
        {
            if (socket.IsLive)
            {
                *socket.dataPoint = (byte)status.Error;
                *(int*)(socket.dataPoint + 1) = identity;
                socket.send(1 + sizeof(int));
                socket.Dispose();
            }
        }
        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="socket">同步套接字</param>
        private void close(topSocket socket)
        {
            close((socket)socket, int.MinValue);
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Start()
        {
            if (start())
            {
                threadPool.TinyPool.Start(getSocket, Dispose);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocket()
        {
            try
            {
                while (isStart != 0)
                {
                    if (listener.Pending()) threadPool.TinyPool.Start(newSocket, listener.AcceptSocket());
                    else Thread.Sleep(1);
                }
            }
            catch (Exception error)
            {
                if (isStart != 0) log.Default.Add(error, null, false);
            }
        }
        /// <summary>
        /// 新的请求处理
        /// </summary>
        /// <param name="socket">套接字</param>
        private unsafe void newSocket(Socket socket)
        {
            byte[] data = new byte[fastCSharp.config.pub.Default.StreamBufferLength];
            fixed (byte* dataFixed = data)
            {
                socket client = new socket(socket, data, new action<topSocket>(close), dataFixed);
                try
                {
                    if (attribute.ReceiveTimeout > 0) socket.ReceiveTimeout = attribute.ReceiveTimeout;
                    if (verify == null || verify.Verify(client))
                    {
                        *dataFixed = (byte)status.Success;
                        *(int*)(dataFixed + 1) = int.MinValue;
                        if (client.send(1 + sizeof(int)))
                        {
                            byte[] command;
                            int identity;
                            keyValue<action<socket, int>, bool> onCommand = new keyValue<action<socket, int>, bool>();
                            while ((command = client.receive()) != null)
                            {
                                if (onCommands.Get(command, ref onCommand))
                                {
                                    if (client.ReceiveInt(out identity) && (!onCommand.Value || (client.receive() != null))) onCommand.Key(client, identity);
                                }
                                else
                                {
                                    log.Default.Add(GetType().FullName + " 缺少命令处理委托 " + command.getReverse().deSerialize(), false, false);
                                    break;
                                }
                            }
                            log.Default.Add("通讯结束 : " + GetType().FullName, false, false);
                        }
                    }
                }
                finally
                {
                    close(client);
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        /// <param name="status">数据发送状态</param>
        /// <param name="identity">回话标志</param>
        /// <returns>是否发送成功</returns>
        protected unsafe bool send(socket socket, tcpServer.status status, int identity)
        {
            try
            {
                *socket.dataPoint = (byte)status;
                *(int*)(socket.dataPoint + 1) = identity;
                return socket.send(1 + sizeof(int));
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, true);
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        /// <param name="status">数据发送状态</param>
        /// <param name="identity">回话标志</param>
        /// <param name="data">待发送数据</param>
        /// <returns>是否发送成功</returns>
        protected unsafe bool send(socket socket, status status, int identity, byte[] data)
        {
            try
            {
                byte[] compressData = null;
                if (attribute.IsCompress)
                {
                    try
                    {
                        compressData = data.getCompress(fastCSharp.stream.compression.Deflate, 0, data.Length);
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                }
                *socket.dataPoint = (byte)status;
                *(int*)(socket.dataPoint + 1) = identity;
                *(int*)(socket.dataPoint + sizeof(int) + 1) = compressData == null ? data.Length : -compressData.Length;
                return socket.send(1 + sizeof(int) + sizeof(int)) && socket.send(compressData ?? data);
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, true);
            }
            return false;
        }
        /// <summary>
        /// 格式化方法标识名称
        /// </summary>
        /// <param name="name">方法标识名称</param>
        /// <returns>方法标识名称</returns>
        protected internal unsafe static byte[] formatMethodKeyName(string name)
        {
            int length = name.Length;
            byte[] data = new byte[(length + 3) & (int.MaxValue - 3)];
            fixed (byte* dataFixed = data)
            {
                if ((length & 3) != 0) *(int*)(dataFixed + (length & (int.MaxValue - 3))) = 0x20202020;
                formatMethodKeyName(name, dataFixed);
            }
            return data;
        }
        /// <summary>
        /// 格式化方法标识名称
        /// </summary>
        /// <param name="name">方法标识名称</param>
        /// <param name="write">写入数据起始位置</param>
        protected internal unsafe static void formatMethodKeyName(string name, byte* write)
        {
            fixed (char* commandFixed = name)
            {
                for (char* start = commandFixed + name.Length, end = commandFixed; start != end; *write++ = (byte)*--start) ;
            }
        }
    }
}
