using System;
using fastCSharp.threading;
using System.Threading;
using System.Net.Sockets;

namespace fastCSharp.net
{
    /// <summary>
    /// TCP调用服务端(异步模式)
    /// </summary>
    public class tcpServerAsynchronous : tcpServer<tcpServerAsynchronous.socket, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous>
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        public unsafe class socket : topSocket
        {
            /// <summary>
            /// 是否通过验证
            /// </summary>
            private bool? isVerify;
            /// <summary>
            /// 当前数据索引位置
            /// </summary>
            private int startIndex;
            /// <summary>
            /// 当前待处理数据长度
            /// </summary>
            private int nextLength;
            /// <summary>
            /// 数据发送完成后的回调委托
            /// </summary>
            private action<socket> onSendFinally;
            /// <summary>
            /// 数据接收完成后的回调委托
            /// </summary>
            private action<socket> onReceivedFinally;
            /// <summary>
            /// 初始化套接字异步操作
            /// </summary>
            /// <param name="socket">异步操作套接字</param>
            /// <param name="data">数据缓存</param>
            /// <param name="onClose">关闭套接字处理</param>
            public socket(Socket socket, byte[] data, action<topSocket> onClose)
                : base(socket, data, onClose) { }
            /// <summary>
            /// 设置验证状态
            /// </summary>
            /// <param name="isVerify">是否通过验证</param>
            protected internal void setVerify(bool isVerify)
            {
                if (this.isVerify == null)
                {
                    if (isVerify) this.isVerify = isVerify;
                    else Dispose();
                }
                else
                {
                    Dispose();
                    log.Default.Add("重复验证调用", true, true);
                }
            }
            /// <summary>
            /// 发送缓存数据
            /// </summary>
            /// <param name="onSend">数据发送完成后的回调委托</param>
            /// <param name="length">数据长度</param>
            protected internal void send(action<socket> onSend, int length)
            {
                if (onSend == null) log.Default.Throw(null, "缺少回调委托", true);
                onSendFinally = onSend;
                currentData = data;
                startIndex = 0;
                nextLength = length;
                send();
            }
            /// <summary>
            /// 发送缓存数据
            /// </summary>
            /// <param name="onSend">数据发送完成后的回调委托</param>
            /// <param name="data">待发送数据</param>
            protected internal void send(action<socket> onSend, byte[] data)
            {
                if (onSend == null) log.Default.Throw(null, "缺少回调委托", true);
                onSendFinally = onSend;
                currentData = data;
                startIndex = 0;
                nextLength = data.Length;
                send();
            }
            /// <summary>
            /// 数据发送完成
            /// </summary>
            private void callOnSend()
            {
                action<socket> onSend = onSendFinally;
                onSendFinally = null;
                onSend(this);
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            private void send()
            {
                try
                {
                    socket.BeginSend(currentData, startIndex, nextLength, SocketFlags.None, onSend, this);
                }
                catch (Exception error)
                {
                    sendError = error;
                    callOnSend();
                }
            }
            /// <summary>
            /// 发送数据并判断数据是否发送完整
            /// </summary>
            /// <param name="result">发送数据结果</param>
            private void onSend(IAsyncResult result)
            {
                int length = 0;
                try
                {
                    if ((length = socket.EndSend(result)) <= 0) sendError = lengthError;
                }
                catch (Exception error)
                {
                    sendError = error;
                }
                if (sendError == null)
                {
                    if ((nextLength -= length) == 0) callOnSend();
                    else
                    {
                        startIndex += length;
                        send();
                    }
                }
                else callOnSend();
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否发送成功</returns>
            public bool SendInt(int value)
            {
                fixed (byte* dataFixed = data) *(int*)dataFixed = value;
                return send(data, 0, sizeof(int));
            }
            /// <summary>
            /// 数据发送器
            /// </summary>
            private struct sender
            {
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public action<bool> OnSend;
                /// <summary>
                /// 异步套接字
                /// </summary>
                public socket Socket;
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                /// <param name="socket">异步套接字</param>
                public void CallOnSend(socket socket)
                {
                    if (socket.receiveError == null) OnSend(true);
                    else
                    {
                        Socket.Dispose();
                        OnSend(false);
                    }
                }
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="onSend">数据接收完成后的回调委托</param>
            /// <param name="value">数据</param>
            public void SendInt(action<bool> onSend, int value)
            {
                if (onSend == null) log.Default.Throw(null, "缺少回调委托", true);
                fixed (byte* dataFixed = data) *(int*)dataFixed = value;
                send(new sender { Socket = this, OnSend = onSend }.CallOnSend, sizeof(int));
            }
            /// <summary>
            /// 发送数据
            /// </summary>
            /// <param name="onSend">数据接收完成后的回调委托</param>
            /// <param name="data">数据</param>
            public void Send(action<bool> onSend, byte[] data)
            {
                if (onSend == null) log.Default.Throw(null, "缺少回调委托", true);
                send(new sender { Socket = this, OnSend = onSend }.CallOnSend, data);
            }
            /// <summary>
            /// 数据接收器
            /// </summary>
            private struct receiver
            {
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public action OnReceived;
                /// <summary>
                /// 异步套接字
                /// </summary>
                public socket Socket;
                /// <summary>
                /// 接收数据缓存
                /// </summary>
                public byte[] Data;
                /// <summary>
                /// 当前数据索引位置
                /// </summary>
                public int StartIndex;
                /// <summary>
                /// 当前待处理数据长度
                /// </summary>
                public int NextLength;
                /// <summary>
                /// 接收数据
                /// </summary>
                public void Receive()
                {
                    try
                    {
                        Socket.socket.BeginReceive(Data, StartIndex, NextLength, SocketFlags.None, onReceive, this);
                    }
                    catch (Exception error)
                    {
                        Socket.receiveError = error;
                        OnReceived();
                    }
                }
                /// <summary>
                /// 异步接收数据
                /// </summary>
                /// <param name="result">接收数据状态</param>
                private void onReceive(IAsyncResult result)
                {
                    int length = 0;
                    try
                    {
                        if ((length = Socket.socket.EndReceive(result)) <= 0) Socket.receiveError = lengthError;
                    }
                    catch (Exception error)
                    {
                        Socket.receiveError = error;
                    }
                    if (Socket.receiveError == null)
                    {
                        if ((NextLength -= length) == 0) OnReceived();
                        else
                        {
                            StartIndex += length;
                            Receive();
                        }
                    }
                    else OnReceived();
                }
            }
            /// <summary>
            /// 异步接收数据长度+数据
            /// </summary>
            /// <param name="onReceived">数据接收完成后的回调委托</param>
            protected internal void receive(action<socket> onReceived)
            {
                if (onReceived == null) log.Default.Throw(null, "缺少回调委托", true);
                onReceivedFinally = onReceived;
                new receiver { Socket = this, Data = data, NextLength = sizeof(int), OnReceived = onReceivedLength }.Receive();
            }
            /// <summary>
            /// 数据发送完成
            /// </summary>
            private void callOnReceived()
            {
                action<socket> onReceived = onReceivedFinally;
                onReceivedFinally = null;
                onReceived(this);
            }
            /// <summary>
            /// 数据长度接收后的处理
            /// </summary>
            private void onReceivedLength()
            {
                if (receiveError == null)
                {
                    nextLength = fastCSharp.unsafer.memory.GetInt(this.data, 0);
                    byte[] data = this.data;
                    int dataLength = nextLength >= 0 ? nextLength : -nextLength;
                    if (data.Length < dataLength)
                    {
                        if (currentData.length() < dataLength) currentData = new byte[dataLength];
                        data = currentData;
                    }
                    new receiver { Socket = this, Data = data, NextLength = dataLength, OnReceived = onReceivedData }.Receive();
                }
                else callOnReceived();
            }
            /// <summary>
            /// 数据数据接收后的处理
            /// </summary>
            private void onReceivedData()
            {
                try
                {
                    if (receiveError == null)
                    {
                        if (nextLength >= 0)
                        {
                            if (data.Length != nextLength) Buffer.BlockCopy(data, 0, currentData = new byte[nextLength], 0, nextLength);
                            else currentData = data;
                        }
                        else currentData = data.getDeCompress(fastCSharp.stream.compression.Deflate, 0, -nextLength);
                    }
                }
                catch (Exception error)
                {
                    receiveError = error;
                }
                finally { callOnReceived(); }
            }
            /// <summary>
            /// 接收整数数据
            /// </summary>
            /// <param name="onReceived">数据接收完成后的回调委托</param>
            protected internal void receiveInt(action<socket> onReceived)
            {
                if (onReceived == null) log.Default.Throw(null, "缺少回调委托", true);
                onReceivedFinally = onReceived;
                new receiver { Socket = this, Data = data, NextLength = sizeof(int), OnReceived = callOnReceived }.Receive();
            }
            /// <summary>
            /// 整数数据接收器
            /// </summary>
            private struct intReceiver
            {
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public action<int?> OnReceived;
                /// <summary>
                /// 异步套接字
                /// </summary>
                public socket Socket;
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public void CallOnReceived()
                {
                    if (Socket.receiveError == null)
                    {
                        fixed (byte* dataFixed = Socket.data) OnReceived(*(int*)dataFixed);
                    }
                    else
                    {
                        Socket.Dispose();
                        OnReceived(null);
                    }
                }
            }
            /// <summary>
            /// 接收整数数据
            /// </summary>
            /// <param name="onReceived">数据接收完成后的回调委托</param>
            public void ReceiveInt(action<int?> onReceived)
            {
                if (onReceived == null) log.Default.Throw(null, "缺少回调委托", true);
                new receiver { Socket = this, Data = data, NextLength = sizeof(int), OnReceived = new intReceiver { Socket = this, OnReceived = onReceived }.CallOnReceived }.Receive();
            }
            /// <summary>
            /// 数据接收器
            /// </summary>
            private struct dataReceiver
            {
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public action<byte[]> OnReceived;
                /// <summary>
                /// 异步套接字
                /// </summary>
                public socket Socket;
                /// <summary>
                /// 接收的数据
                /// </summary>
                public byte[] Data;
                /// <summary>
                /// 数据接收完成后的回调委托
                /// </summary>
                public void CallOnReceived()
                {
                    if (Socket.receiveError == null) OnReceived(Data);
                    else
                    {
                        Socket.Dispose();
                        OnReceived(null);
                    }
                }
            }
            /// <summary>
            /// 接收数据
            /// </summary>
            /// <param name="onReceived">数据接收完成后的回调委托</param>
            /// <param name="length">数据长度</param>
            public void Receive(action<byte[]> onReceived, int length)
            {
                if (onReceived == null) log.Default.Throw(null, "缺少回调委托", true);
                if (length > 0)
                {
                    byte[] data = null;
                    try
                    {
                        data = new byte[length];
                    }
                    catch (Exception error)
                    {
                        receiveError = error;
                        Dispose();
                    }
                    if (data != null) new receiver { Socket = this, Data = data, NextLength = length, OnReceived = new dataReceiver { Socket = this, OnReceived = onReceived, Data = data }.CallOnReceived }.Receive();
                }
                else onReceived(length == 0 ? nullValue<byte>.Array : null);
            }
            /// <summary>
            /// 接收整数值
            /// </summary>
            /// <param name="value">整数值</param>
            /// <returns>是否成功</returns>
            public unsafe bool ReceiveInt(out int value)
            {
                if (receiveInt())
                {
                    fixed (byte* dataFixed = data) value = *(int*)dataFixed;
                    return true;
                }
                value = 0;
                return false;
            }
            /// <summary>
            /// 关闭异步套接字
            /// </summary>
            /// <param name="socket">异步套接字</param>
            public static void Close(socket socket)
            {
                socket.Dispose();
            }
        }
        /// <summary>
        /// 关闭链接命令
        /// </summary>
        protected static byte[] closeCommandData
        {
            get { return tcpServer.closeCommandData; }
        }
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        /// <param name="attribute">配置信息</param>
        /// <param name="verify">TCP客户端验证接口</param>
        public tcpServerAsynchronous(fastCSharp.setup.cSharp.tcpServer attribute, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous verify = null)
            : base(attribute, verify)
        {
            onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>[] { new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)) });
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        /// <param name="identity">回话标识</param>
        protected void close(socket socket, int identity)
        {
            send(socket, socket.Close, tcpServer.status.Success, identity);
        }
        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="socket">异步套接字</param>
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
                    if (listener.Pending()) newSocket(listener.AcceptSocket());
                    else Thread.Sleep(1);
                }
            }
            catch (Exception error)
            {
                if (isStart != 0) log.Default.Add(error, null, false);
            }
        }
        /// <summary>
        /// 异步套接字验证
        /// </summary>
        private struct verifySocket
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            public tcpServerAsynchronous TcpServer;
            /// <summary>
            /// 异步套接字
            /// </summary>
            public socket Socket;
            /// <summary>
            /// 验证后的处理
            /// </summary>
            /// <param name="isVerify">是否通过验证</param>
            public void OnVerify(bool isVerify)
            {
                Socket.setVerify(isVerify);
                if (Socket.IsLive) TcpServer.send(Socket, TcpServer.receiveCommand, tcpServer.status.Success, int.MinValue);
            }
        }
        /// <summary>
        /// 获取客户端套接字并准备验证
        /// </summary>
        /// <param name="socket">客户端套接字</param>
        private void newSocket(Socket socket)
        {
            socket client = new socket(socket, new byte[fastCSharp.config.pub.Default.StreamBufferLength], new action<topSocket>(close));
            if (attribute.ReceiveTimeout > 0) socket.ReceiveTimeout = attribute.ReceiveTimeout;
            if (verify == null) send(client, receiveCommand, tcpServer.status.Success, int.MinValue);
            else verify.Verify(client, new verifySocket { TcpServer = this, Socket = client }.OnVerify);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        /// <param name="onSend">数据发送完后的处理委托</param>
        /// <param name="identity">回话标志</param>
        /// <param name="status">数据发送状态</param>
        protected unsafe void send(socket socket, action<socket> onSend, tcpServer.status status, int identity)
        {
            fixed (byte* dataFixed = socket.data)
            {
                *dataFixed = (byte)status;
                *(int*)(dataFixed + 1) = identity;
            }
            socket.send(onSend, sizeof(int) + 1);
        }
        /// <summary>
        /// 数据发送器
        /// </summary>
        private struct sender
        {
            /// <summary>
            /// 待发送数据
            /// </summary>
            public byte[] Data;
            /// <summary>
            /// 数据发送完后的处理委托
            /// </summary>
            public action<socket> OnSend;
            /// <summary>
            /// 数据长度发送完成处理
            /// </summary>
            /// <param name="socket">异步套接字</param>
            public void OnSendLength(socket socket)
            {
                if (socket.sendError == null) socket.send(OnSend, Data);
                else OnSend(socket);
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        /// <param name="onSend">数据发送完后的处理委托</param>
        /// <param name="status">数据发送状态</param>
        /// <param name="identity">回话标志</param>
        /// <param name="data">发送数据</param>
        protected unsafe void send(socket socket, action<socket> onSend, tcpServer.status status, int identity, byte[] data)
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
            fixed (byte* dataFixed = socket.data)
            {
                *dataFixed = (byte)status;
                *(int*)(dataFixed + 1) = identity;
                *(int*)(dataFixed + sizeof(int) + 1) = compressData == null ? data.Length : -compressData.Length;
            }
            socket.send(new sender { Data = compressData ?? data, OnSend = onSend }.OnSendLength, sizeof(int) + sizeof(int) + 1);
        }
        /// <summary>
        /// 接收命令
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        protected void receiveCommand(socket socket)
        {
            if (socket.sendError == null) socket.receive(onReceiveCommand);
            else
            {
                socket.Dispose();
                log.Default.Add(socket.sendError, null, true);
            }
        }
        /// <summary>
        /// 会话标识接收器
        /// </summary>
        private struct identityReceiver
        {
            /// <summary>
            /// 命令处理委托
            /// </summary>
            public keyValue<action<socket, int>, bool> OnCommand;
            /// <summary>
            /// 异步套接字
            /// </summary>
            public socket Socket;
            /// <summary>
            /// 会话标识
            /// </summary>
            private int identity;
            /// <summary>
            /// 会话标识接收完后的处理
            /// </summary>
            /// <param name="socket">异步套接字</param>
            public void OnReceive(socket socket)
            {
                if (socket.receiveError == null)
                {
                    identity = fastCSharp.unsafer.memory.GetInt(Socket.data, 0);
                    if (OnCommand.Value) socket.receive(onReceive);
                    else task.TinyTask.Add(doCommand);
                }
                else
                {
                    socket.Dispose();
                    log.Default.Add(socket.receiveError, "通讯标识接收失败", false);
                }
            }
            /// <summary>
            /// 数据接收完后的处理
            /// </summary>
            /// <param name="socket">异步套接字</param>
            private void onReceive(socket socket)
            {
                if (socket.receiveError == null) task.TinyTask.Add(doCommand);
                else
                {
                    socket.Dispose();
                    log.Default.Add(socket.receiveError, "数据接收失败", false);
                }
            }
            /// <summary>
            /// 命令处理
            /// </summary>
            private void doCommand()
            {
                OnCommand.Key(Socket, identity);
            }
        }
        /// <summary>
        /// 接收命令
        /// </summary>
        /// <param name="socket">套接字异步操作</param>
        private void onReceiveCommand(socket socket)
        {
            if (socket.receiveError == null)
            {
                byte[] command = socket.CurrentData;
                keyValue<action<socket, int>, bool> onCommand = new keyValue<action<socket, int>, bool>();
                if (onCommands.Get(command, ref onCommand))
                {
                    socket.receiveInt(new identityReceiver { Socket = socket, OnCommand = onCommand }.OnReceive);
                }
                else
                {
                    socket.Dispose();
                    log.Default.Add(GetType().FullName + " 缺少命令处理委托 " + command.getReverse().deSerialize(), false, false);
                }
            }
            else
            {
                socket.Dispose();
                log.Default.Add(socket.receiveError, "命令数据错误", false);
            }
        }
        /// <summary>
        /// 格式化方法标识名称
        /// </summary>
        /// <param name="name">方法标识名称</param>
        /// <returns>方法标识名称</returns>
        protected static byte[] formatMethodKeyName(string name)
        {
            return tcpServer.formatMethodKeyName(name);
        }
    }
}
