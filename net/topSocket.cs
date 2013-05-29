using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace fastCSharp.net
{
    /// <summary>
    /// TCP调用套接字
    /// </summary>
    public unsafe abstract class topSocket : IDisposable
    {
        /// <summary>
        /// 数据长度异常
        /// </summary>
        protected static readonly Exception lengthError = new Exception("数据长度错误");
        /// <summary>
        /// 操作套接字
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        private IPAddress ipAddress;
        /// <summary>
        /// 客户端IP地址,失败返回null
        /// </summary>
        public IPAddress IpAddress
        {
            get
            {
                if (ipAddress == null)
                {
                    if (socket != null)
                    {
                        IPEndPoint ip = socket.RemoteEndPoint as IPEndPoint;
                        if (ip != null) ipAddress = ip.Address;
                    }
                    if (ipAddress == null) ipAddress = IPAddress.Any;
                }
                return ipAddress != IPAddress.Any ? ipAddress : null;
            }
        }
        /// <summary>
        /// 数据缓存
        /// </summary>
        protected internal byte[] data;
        /// <summary>
        /// 数据缓存起始位置
        /// </summary>
        protected internal byte* dataPoint;
        /// <summary>
        /// 操作套接字事件
        /// </summary>
        private action<topSocket> onClose;
        /// <summary>
        /// 当前处理数据
        /// </summary>
        protected byte[] currentData;
        /// <summary>
        /// 当前处理数据
        /// </summary>
        public byte[] CurrentData { get { return currentData; } }
        /// <summary>
        /// 最后一次接收数据异常错误
        /// </summary>
        protected internal Exception receiveError;
        /// <summary>
        /// 最后一次发送数据异常错误
        /// </summary>
        protected internal Exception sendError;
        /// <summary>
        /// 套接字是否可用
        /// </summary>
        public bool IsLive
        {
            get { return socket != null && socket.Connected; }
        }
        /// <summary>
        /// 初始化同步套接字
        /// </summary>
        /// <param name="socket">操作套接字</param>
        /// <param name="data">数据缓存</param>
        /// <param name="onClose">关闭套接字处理</param>
        /// <param name="dataPoint">数据缓存起始位置</param>
        protected topSocket(Socket socket, byte[] data, action<topSocket> onClose, byte* dataPoint = null)
        {
            if (socket == null) log.Default.Throw(null, "缺少套接字", true);
            if (data == null) log.Default.Throw(null, "缺少数据缓存", true);
            if (dataPoint != null)
            {
                fixed (byte* dataFixed = data)
                {
                    if (dataFixed != dataPoint) log.Default.Throw(null, "数据缓存起始位置不正确", true);
                }
                this.dataPoint = dataPoint;
            }
            this.socket = socket;
            this.data = data;
            this.onClose = onClose;
        }
        /// <summary>
        /// 关闭套接字连接
        /// </summary>
        public void Dispose()
        {
            currentData = data = null;
            if (this.socket != null)
            {
                Socket socket = this.socket;
                this.socket = null;
                try
                {
                    onClose(this);
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally
                {
                    socket.Close();
                }
            }
        }
        /// <summary>
        /// 接收整数值
        /// </summary>
        /// <returns>是否成功</returns>
        protected bool receiveInt()
        {
            if (socket != null)
            {
                try
                {
                    int index = socket.Receive(data, 0, sizeof(int), SocketFlags.None);
                    if (index > 0)
                    {
                        while (index != sizeof(int))
                        {
                            Thread.Sleep(1);
                            int length = socket.Receive(data, index, sizeof(int) - index, SocketFlags.None);
                            if (length >= 0) index += length;
                            else break;
                        }
                        if (index == sizeof(int)) return true;
                    }
                }
                catch (Exception error)
                {
                    receiveError = error;
                }
                Dispose();
            }
            return false;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="data">数据缓存</param>
        /// <param name="length">数据长度</param>
        /// <returns>是否成功</returns>
        protected bool receive(byte[] data, int length)
        {
            receiveError = null;
            if (socket != null)
            {
                try
                {
                    int index = socket.Receive(data, 0, length, SocketFlags.None);
                    if (index > 0)
                    {
                        while (index != length)
                        {
                            Thread.Sleep(1);
                            int receiveLength = socket.Receive(data, index, length - index, SocketFlags.None);
                            if (receiveLength >= 0) index += receiveLength;
                            else break;
                        }
                        if (index == length) return true;
                    }
                }
                catch (Exception error)
                {
                    receiveError = error;
                }
                Dispose();
            }
            return false;
        }
        /// <summary>
        /// 接收指定长度的数据
        /// </summary>
        /// <param name="length">数据长度</param>
        /// <returns>接收的数据,失败返回null</returns>
        public byte[] Receive(int length)
        {
            byte[] data = new byte[length];
            return receive(data, length) ? data : null;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">发送长度</param>
        /// <returns>是否成功</returns>
        protected bool send(byte[] data, int startIndex, int length)
        {
            if (socket != null)
            {
                try
                {
                    if (socket.Send(data, startIndex, length, SocketFlags.None) == length) return true;
                }
                catch (Exception error)
                {
                    sendError = error;
                }
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否成功</returns>
        public bool Send(byte[] data)
        {
            return data != null && (data.Length == 0 || send(data, 0, data.Length));
        }
    }
}
