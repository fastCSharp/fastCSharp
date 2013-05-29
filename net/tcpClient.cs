using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace fastCSharp.net
{
    /// <summary>
    /// TCP调用客户端
    /// </summary>
    public class tcpClient
    {
        /// <summary>
        /// 最后一次未找到注册的服务名称
        /// </summary>
        private static string errorServiceName;
        /// <summary>
        /// 最后一次连接是否没有指定IP地址
        /// </summary>
        private static bool isAnyIpAddress;
        /// <summary>
        /// 配置信息
        /// </summary>
        protected fastCSharp.setup.cSharp.tcpServer attribute;
        /// <summary>
        /// 验证接口
        /// </summary>
        private fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify;
        /// <summary>
        /// TCP客户端连接
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// 网络数据流
        /// </summary>
        private NetworkStream stream;
        /// <summary>
        /// 数据缓存
        /// </summary>
        protected byte[] data;
        /// <summary>
        /// 是否已启动服务
        /// </summary>
        protected int isStart;
        /// <summary>
        /// 是否已启动服务
        /// </summary>
        public bool IsStart
        {
            get { return isStart != 0; }
        }
        /// <summary>
        /// 是否正在释放资源
        /// </summary>
        private bool isDispose;
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        /// <param name="attribute">配置信息</param>
        /// <param name="verify">验证接口</param>
        /// <param name="isStart">是否启动连接</param>
        public tcpClient(fastCSharp.setup.cSharp.tcpServer attribute, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify = null, bool isStart = true)
        {
            this.attribute = attribute;
            if (attribute.TcpRegisterName != null)
            {
                if (fastCSharp.setup.tcpRegister.client.Get(attribute.TcpRegisterName).GetRegister(attribute)) errorServiceName = null;
                else
                {
                    if (errorServiceName != attribute.ServiceName) log.Default.Add(attribute.ServiceName + " 未找到注册服务信息", false, false);
                    errorServiceName = attribute.ServiceName;
                }
            }
            this.verify = verify;
            data = new byte[sizeof(long)];
            if (isStart) start();
        }
        /// <summary>
        /// 停止客户端链接
        /// </summary>
        public void Dispose()
        {
            if (!isDispose)
            {
                isDispose = true;
                try
                {
                    if (stream != null && SendInt(tcpServer.closeCommandData.Length) && send(tcpServer.closeCommandData) && SendInt(int.MaxValue))
                    {
                        IsSuccess(int.MaxValue);
                    }
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                }
            }
            if (Interlocked.Exchange(ref isStart, 0) == 1)
            {
                using (client)
                using (stream)
                {
                    stream = null;
                    client = null;
                }
            }
        }
        /// <summary>
        /// 启动客户端链接
        /// </summary>
        /// <returns>启动是否成功</returns>
        private bool start()
        {
            if (attribute.IpAddress == IPAddress.Any)
            {
                if (!isAnyIpAddress) log.Default.Add("客户端TCP连接失败(" + attribute.Host + ":" + attribute.Port.toString() + ")", false, false);
                isAnyIpAddress = true;
                return false;
            }
            isAnyIpAddress = false;
            if (Interlocked.Exchange(ref isStart, 1) == 0)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(attribute.IpAddress, attribute.Port);
                    stream = client.GetStream();
                    if (attribute.ReceiveTimeout > 0) client.ReceiveTimeout = stream.ReadTimeout = attribute.ReceiveTimeout;
                    if (verify != null ? !verify.Verify(this) : !IsSuccess(int.MinValue))
                    {
                        if (isStart != 0) Dispose();
                        log.Default.Add(null, "TCP客户端验证失败", false);
                    }
                }
                catch (Exception error)
                {
                    if (isStart != 0) Dispose();
                    log.Default.Add(error, "客户端TCP连接失败(" + attribute.Host + ":" + attribute.Port.toString() + ")", false);
                }
                return isStart != 0;
            }
            return false;
        }
        /// <summary>
        /// 发送字节数组到服务器
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>是否成功</returns>
        protected bool send(byte[] data)
        {
            if (isStart != 0)
            {
                try
                {
                    stream.Write(data, 0, data.Length);
                    return true;
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, true);
                    Dispose();
                }
            }
            return false;
        }
        /// <summary>
        /// 发送数组到服务器
        /// </summary>
        /// <param name="value">待发送数据</param>
        /// <returns>是否成功</returns>
        public unsafe bool SendInt(int value)
        {
            if (isStart != 0)
            {
                try
                {
                    fixed (byte* dataFixed = data) *(int*)dataFixed = value;
                    stream.Write(data, 0, sizeof(int));
                    return true;
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, true);
                    Dispose();
                }
            }
            return false;
        }
        /// <summary>
        /// 发送数组到服务器
        /// </summary>
        /// <param name="value">待发送数据</param>
        /// <returns>是否成功</returns>
        public unsafe bool SendLong(long value)
        {
            if (isStart != 0)
            {
                try
                {
                    fixed (byte* dataFixed = data) *(long*)dataFixed = value;
                    stream.Write(data, 0, sizeof(long));
                    return true;
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, true);
                    Dispose();
                }
            }
            return false;
        }
        /// <summary>
        /// 发送字节数组到服务器
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>是否成功</returns>
        public bool Send(byte[] data)
        {
            return data != null && send(data);
        }
        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="onSend">发送数据以后的处理</param>
        /// <param name="command">TCP调用命令</param>
        /// <param name="identity">回话标志</param>
        /// <param name="data">待发送数据</param>
        protected unsafe void send(action<bool> onSend, byte[] command, int identity, byte[] data)
        {
            try
            {
                byte[] compressData = this.getCompress(data);
                if (compressData != null) data = compressData;
                byte[] sendData = new byte[command.Length + sizeof(int) + sizeof(int) + data.Length];
                Buffer.BlockCopy(command, 0, sendData, 0, command.Length);
                fixed (byte* dataFixed = sendData)
                {
                    byte* write = dataFixed + command.Length;
                    *(int*)write = identity;
                    *(int*)(write + sizeof(int)) = compressData == null ? data.Length : -data.Length;
                }
                Buffer.BlockCopy(data, 0, sendData, command.Length + sizeof(int) + sizeof(int), data.Length);
                stream.BeginWrite(sendData, 0, sendData.Length, this.onSend, onSend);
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, false);
                if (isStart != 0) Dispose();
                onSend(false);
            }
        }
        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="onSend">发送数据以后的处理</param>
        /// <param name="command">TCP调用命令</param>
        /// <param name="identity">回话标志</param>
        protected unsafe void send(action<bool> onSend, byte[] command, int identity)
        {
            try
            {
                byte[] sendData = new byte[command.Length + sizeof(int)];
                Buffer.BlockCopy(command, 0, sendData, 0, command.Length);
                fixed (byte* dataFixed = sendData) *(int*)(dataFixed + command.Length) = identity;
                stream.BeginWrite(sendData, 0, sendData.Length, this.onSend, onSend);
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, false);
                if (isStart != 0) Dispose();
                onSend(false);
            }
        }
        /// <summary>
        /// 异步流写入完成
        /// </summary>
        /// <param name="result">异步操作状态</param>
        private void onSend(IAsyncResult result)
        {
            action<bool> onSend = (action<bool>)result.AsyncState;
            bool isError = false;
            try
            {
                stream.EndWrite(result);
            }
            catch (Exception error)
            {
                log.Default.Add(error, null, false);
                if (isStart != 0) Dispose();
                isError = true;
                onSend(false);
            }
            if (!isError) onSend(true);
        }
        /// <summary>
        /// 从数据流读取数据
        /// </summary>
        /// <returns>读取数据是否成功</returns>
        private bool readByte()
        {
            if (isStart != 0)
            {
                try
                {
                    int length = stream.Read(data, 0, 1);
                    while (length == 0)
                    {
                        Thread.Sleep(1);
                        length = stream.Read(data, 0, 1);
                    }
                    if (length == 1) return true;

                }
                catch (Exception error)
                {
                    if (isStart != 0) log.Default.Add(error, null, false);
                }
                Dispose();
            }
            return false;
        }
        /// <summary>
        /// 从数据流读取数据
        /// </summary>
        /// <returns>读取数据是否成功</returns>
        private bool readInt()
        {
            if (isStart != 0)
            {
                try
                {
                    int index = stream.Read(data, 0, sizeof(int));
                    if (index >= 0)
                    {
                        while (index != sizeof(int))
                        {
                            Thread.Sleep(1);
                            int length = stream.Read(data, index, sizeof(int) - index);
                            if (length >= 0) index += length;
                            else break;
                        }
                        if (index == sizeof(int)) return true;
                    }
                }
                catch (Exception error)
                {
                    if (isStart != 0) log.Default.Add(error, null, false);
                }
                Dispose();
            }
            return false;
        }
        /// <summary>
        /// 从数据流读取数据
        /// </summary>
        /// <param name="value">读取的数据</param>
        /// <returns>是否成功</returns>
        public unsafe bool ReadInt(out int value)
        {
            if (readInt())
            {
                fixed (byte* dataFixed = data) value = *(int*)dataFixed;
                return true;
            }
            value = 0;
            return false;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="length">读取数据长度</param>
        /// <returns>读取的数据,失败返回null</returns>
        public byte[] Read(int length)
        {
            byte[] data = new byte[length];
            return read(data) ? data : null;
        }
        /// <summary>
        /// 从数据流读取数据
        /// </summary>
        /// <param name="data">数据缓存</param>
        /// <returns>读取数据是否成功</returns>
        private bool read(byte[] data)
        {
            if (isStart != 0)
            {
                try
                {
                    int index = stream.Read(data, 0, data.Length);
                    if (index >= 0)
                    {
                        while (index != data.Length)
                        {
                            Thread.Sleep(1);
                            int length = stream.Read(data, index, data.Length - index);
                            if (length >= 0) index += length;
                            else break;
                        }
                        if (index == data.Length) return true;
                    }
                }
                catch (Exception error)
                {
                    if (isStart != 0) log.Default.Add(error, null, false);
                }
                Dispose();
            }
            return false;
        }
        /// <summary>
        /// 数据读取器
        /// </summary>
        private class reader
        {
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            public tcpClient Client;
            /// <summary>
            /// 读取数据后的处理
            /// </summary>
            public action<byte[]> OnRead;
            /// <summary>
            /// 回话标志
            /// </summary>
            public int Identity;
            /// <summary>
            /// 是否接受数据
            /// </summary>
            public bool IsData;
            /// <summary>
            /// 当前读取位置
            /// </summary>
            private int currentIndex;
            /// <summary>
            /// 接收数据缓存
            /// </summary>
            private byte[] data;
            /// <summary>
            /// 数据是否压缩
            /// </summary>
            private bool isCompress;
            /// <summary>
            /// 开始读取数据
            /// </summary>
            public void Start()
            {
                try
                {
                    Client.stream.BeginRead(Client.data, 0, 1, onStatus, this);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    OnRead(null);
                }
            }
            /// <summary>
            /// 读取回话状态处理
            /// </summary>
            /// <param name="result">异步读取状态</param>
            private void onStatus(IAsyncResult result)
            {
                int count = 0;
                bool isError = false;
                try
                {
                    count = Client.stream.EndRead(result);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    isError = true;
                    OnRead(null);
                }
                if (!isError)
                {
                    if (count > 0)
                    {
                        currentIndex = 0;
                        readIdentity();
                    }
                    else OnRead(null);
                }
            }
            /// <summary>
            /// 读取回话标志
            /// </summary>
            private void readIdentity()
            {
                try
                {
                    Client.stream.BeginRead(Client.data, currentIndex, sizeof(int) - currentIndex, onIdentity, this);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    OnRead(null);
                }
            }
            /// <summary>
            /// 读取回话标志处理
            /// </summary>
            /// <param name="result">异步读取状态</param>
            private void onIdentity(IAsyncResult result)
            {
                int count = 0;
                bool isError = false;
                try
                {
                    count = Client.stream.EndRead(result);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    isError = true;
                    OnRead(null);
                }
                if (!isError)
                {
                    if (count > 0)
                    {
                        currentIndex += count;
                        if (currentIndex == sizeof(int))
                        {
                            if (fastCSharp.unsafer.memory.GetInt(Client.data) == Identity)
                            {
                                if (IsData)
                                {
                                    currentIndex = 0;
                                    readLength();
                                }
                                else OnRead(nullValue<byte>.Array);
                            }
                            else OnRead(null);
                        }
                        else readIdentity();
                    }
                    else OnRead(null);
                }
            }
            /// <summary>
            /// 读取数据长度
            /// </summary>
            private void readLength()
            {
                try
                {
                    Client.stream.BeginRead(Client.data, currentIndex, sizeof(int) - currentIndex, onLength, this);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    OnRead(null);
                }
            }
            /// <summary>
            /// 读取数据长度处理
            /// </summary>
            /// <param name="result">异步读取状态</param>
            private void onLength(IAsyncResult result)
            {
                int count = 0;
                bool isError = false;
                try
                {
                    count = Client.stream.EndRead(result);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    isError = true;
                    OnRead(null);
                }
                if (!isError)
                {
                    if (count > 0)
                    {
                        currentIndex += count;
                        if (currentIndex == sizeof(int))
                        {
                            isError = false;
                            try
                            {
                                int length = fastCSharp.unsafer.memory.GetInt(Client.data);
                                if (length < 0)
                                {
                                    isCompress = true;
                                    length = -length;
                                }
                                data = new byte[length];
                            }
                            catch (Exception error)
                            {
                                log.Default.Add(error, null, false);
                                Client.Dispose();
                                isError = true;
                                OnRead(null);
                            }
                            if (!isError)
                            {
                                currentIndex = 0;
                                readData();
                            }
                        }
                        else readLength();
                    }
                    else OnRead(null);
                }
            }
            /// <summary>
            /// 读取数据长度
            /// </summary>
            private void readData()
            {
                try
                {
                    Client.stream.BeginRead(data, currentIndex, data.Length - currentIndex, onData, this);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    OnRead(null);
                }
            }
            /// <summary>
            /// 读取数据处理
            /// </summary>
            /// <param name="result">异步读取状态</param>
            private void onData(IAsyncResult result)
            {
                int count = 0;
                bool isError = false;
                try
                {
                    count = Client.stream.EndRead(result);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                    Client.Dispose();
                    isError = true;
                    OnRead(null);
                }
                if (!isError)
                {
                    if (count > 0)
                    {
                        currentIndex += count;
                        if (currentIndex == data.Length)
                        {
                            isError = false;
                            if (isCompress)
                            {
                                try
                                {
                                    data = data.getDeCompress(fastCSharp.stream.compression.Deflate, 0, data.Length);
                                }
                                catch (Exception error)
                                {
                                    log.Default.Add(error, null, false);
                                    Client.Dispose();
                                    isError = true;
                                    OnRead(null);
                                }
                            }
                            if (!isError) OnRead(data);
                        }
                        else readData();
                    }
                    else OnRead(null);
                }
            }
        }
        /// <summary>
        /// 异步读取数据
        /// </summary>
        /// <param name="onRead">读取数据后的处理</param>
        /// <param name="identity">回话标志</param>
        /// <param name="isData">是否接受数据</param>
        protected void read(action<byte[]> onRead, int identity, bool isData)
        {
            new reader { Client = this, OnRead = onRead, Identity = identity, IsData = isData }.Start();
        }
        /// <summary>
        /// 判断操作状态是否成功
        /// </summary>
        /// <param name="identity">回话标志</param>
        /// <returns>操作状态是否成功</returns>
        public bool IsSuccess(int identity = int.MinValue)
        {
            return readByte() && data[0] == (byte)tcpServer.status.Success && readInt() && fastCSharp.unsafer.memory.GetInt(data) == identity;
        }
        /// <summary>
        /// 判断操作状态是否成功并获取反馈数据
        /// </summary>
        /// <returns>反馈数据,失败为null</returns>
        protected byte[] getData(int identity)
        {
            if (IsSuccess(identity) && readInt())
            {
                int length = fastCSharp.unsafer.memory.GetInt(this.data);
                if (length != 0)
                {
                    bool isCompression = false;
                    if (length < 0)
                    {
                        isCompression = true;
                        length = -length;
                    }
                    byte[] data = new byte[length];
                    if (read(data))
                    {
                        if (isCompression)
                        {
                            try
                            {
                                data = data.getDeCompress(fastCSharp.stream.compression.Deflate, 0, data.Length);
                            }
                            catch (Exception error)
                            {
                                log.Default.Add(error, "数据解压缩失败", false);
                                Dispose();
                                data = null;
                            }
                        }
                        return data;
                    }
                }
                else return nullValue<byte>.Array;
            }
            return null;
        }
        /// <summary>
        /// 获取压缩数据
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns>压缩数据,失败返回null</returns>
        protected byte[] getCompress(byte[] data)
        {
            return attribute.IsCompress ? data.getCompress(fastCSharp.stream.compression.Deflate, 0, data.Length) : null;
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
            if (isStart != 0)
            {
                try
                {
                    int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                    if (send(command) && SendInt(identity))
                    {
                        byte[] data = getData(identity);
                        return outputParameter.DeSerialize(data);
                    }
                }
                catch (Exception error)
                {
                    Dispose();
                    throw error;
                }
            }
            return false;
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
            if (isStart != 0)
            {
                try
                {
                    byte[] data = inputParameter.Serialize(), compressData = getCompress(data);
                    int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                    if (send(command) && SendInt(identity)
                        && SendInt(compressData == null ? data.Length : -compressData.Length) && send(compressData ?? data)
                        && (data = getData(identity)) != null && outputParameter.DeSerialize(data))
                    {
                        return true;
                    }
                }
                catch (Exception error)
                {
                    Dispose();
                    throw error;
                }
            }
            return false;
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
            if (isStart != 0)
            {
                try
                {
                    byte[] data = inputParameter.Serialize(), compressData = getCompress(data);
                    int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                    if (send(command) && SendInt(identity)
                        && SendInt(compressData == null ? data.Length : -compressData.Length) && send(compressData ?? data)
                        && IsSuccess(identity))
                    {
                        return true;
                    }
                }
                catch (Exception error)
                {
                    Dispose();
                    throw error;
                }
            }
            return false;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="command">TCP调用命令</param>
        /// <returns>TCP调用是否成功</returns>
        public bool Call(byte[] command)
        {
            if (isStart != 0)
            {
                try
                {
                    int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                    return send(command) && SendInt(identity) && IsSuccess(identity);
                }
                catch (Exception error)
                {
                    Dispose();
                    throw error;
                }
            }
            return false;
        }
        /// <summary>
        /// 异步TCP调用
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        public struct async<returnType> where returnType : fastCSharp.setup.cSharp.serialize.ISerialize
        {
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            public tcpClient Client;
            /// <summary>
            /// 回调委托
            /// </summary>
            public action<returnType> OnReturn;
            /// <summary>
            /// 返回值
            /// </summary>
            public returnType OutputParameter;
            /// <summary>
            /// 回话标识
            /// </summary>
            public int Identity;
            /// <summary>
            /// 发送数据回调
            /// </summary>
            /// <param name="isSend">数据是否发送成功</param>
            public void OnSend(bool isSend)
            {
                if (isSend) Client.read(onRead, Identity, true);
                else OnReturn(default(returnType));
            }
            /// <summary>
            /// 读取数据回调
            /// </summary>
            /// <param name="data">读取的数据,失败返回null</param>
            private void onRead(byte[] data)
            {
                if (data != null)
                {
                    bool isSerialize = false;
                    try
                    {
                        if (OutputParameter.DeSerialize(data)) isSerialize = true;
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                    finally
                    {
                        OnReturn(isSerialize ? OutputParameter : default(returnType));
                    }
                }
                else OnReturn(default(returnType));
            }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="onGet">回调委托,返回null表示失败</param>
        /// <param name="command">TCP调用命令</param>
        /// <param name="outputParameter">输出参数</param>
        public void Get<outputParameterType>(action<outputParameterType> onGet, byte[] command, outputParameterType outputParameter)
            where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
        {
            if (isStart != 0)
            {
                int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                send(new async<outputParameterType> { Client = this, OnReturn = onGet, OutputParameter = outputParameter, Identity = identity }.OnSend, command, identity);
            }
            else onGet(default(outputParameterType));
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
        public void Get<inputParameterType, outputParameterType>(action<outputParameterType> onGet, byte[] command, inputParameterType inputParameter, outputParameterType outputParameter)
            where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
            where outputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
        {
            if (isStart != 0)
            {
                byte[] data = null;
                try
                {
                    data = inputParameter.Serialize();
                }
                catch (Exception error)
                {
                    Dispose();
                    log.Default.Add(error, null, false);
                }
                finally
                {
                    if (data != null)
                    {
                        int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                        send(new async<outputParameterType> { Client = this, OnReturn = onGet, OutputParameter = outputParameter, Identity = identity }.OnSend, command, identity, data);
                    }
                    else onGet(default(outputParameterType));
                }
            }
            else onGet(default(outputParameterType));
        }
        /// <summary>
        /// 异步TCP调用
        /// </summary>
        public struct async
        {
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            public tcpClient Client;
            /// <summary>
            /// 回调委托
            /// </summary>
            public action<bool> OnReturn;
            /// <summary>
            /// 回话标识
            /// </summary>
            public int Identity;
            /// <summary>
            /// 发送数据回调
            /// </summary>
            /// <param name="isSend">数据是否发送成功</param>
            public void OnSend(bool isSend)
            {
                if (isSend) Client.read(onRead, Identity, false);
                else OnReturn(false);
            }
            /// <summary>
            /// 读取数据回调
            /// </summary>
            /// <param name="data">读取的数据,失败返回null</param>
            private void onRead(byte[] data)
            {
                OnReturn(data != null);
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="onCall">回调委托,返回false表示失败</param>
        /// <param name="command">TCP调用命令</param>
        /// <param name="inputParameter">输入参数</param>
        public void Call<inputParameterType>(action<bool> onCall, byte[] command, inputParameterType inputParameter)
            where inputParameterType : fastCSharp.setup.cSharp.serialize.ISerialize
        {
            if (isStart != 0)
            {
                byte[] data = null;
                try
                {
                    data = inputParameter.Serialize();
                }
                catch (Exception error)
                {
                    Dispose();
                    log.Default.Add(error, null, false);
                }
                finally
                {
                    if (data != null)
                    {
                        int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                        send(new async { Client = this, OnReturn = onCall, Identity = identity }.OnSend, command, identity, data);
                    }
                    else onCall(false);
                }
            }
            else onCall(false);
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="onCall">回调委托,返回false表示失败</param>
        /// <param name="command">TCP调用命令</param>
        public void Call(action<bool> onCall, byte[] command)
        {
            if (isStart != 0)
            {
                int identity = fastCSharp.threading.identity.Default.Next() ^ (int)fastCSharp.pub.StartTime.Ticks;
                send(new async { Client = this, OnReturn = onCall, Identity = identity }.OnSend, command, identity);
            }
            else onCall(false);
        }
    }
}
