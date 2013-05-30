///本文件由程序自动生成,请不要自行修改
using System;
using fastCSharp;
namespace fastCSharp.testCase
{
        public static partial class tcpCall
        {
            public partial class tcpCallSegmentation
            {
                private static readonly byte[] _c73 = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("fastCSharp.testCase.tcpCallSegmentation()action");
                public static void action()
                {
                    if (fastCSharp.testCase.tcpClient/**/.tcpCallSegmentation/**/.DefaultPool.Call(_c73)) return;
                    throw new Exception();
                }
                private static readonly byte[] _c74 = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("fastCSharp.testCase.tcpCallSegmentation()actionAsynchronous");
                public static void actionAsynchronous()
                {
                    if (fastCSharp.testCase.tcpClient/**/.tcpCallSegmentation/**/.DefaultPool.Call(_c74)) return;
                    throw new Exception();
                }
                public static fastCSharp.net.tcpClient actionAsynchronous(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
                {
                    bool _isCall_ = false;
                    try
                    {
                        fastCSharp.setup.cSharp.tcpCall.clientPool _client_ = fastCSharp.testCase.tcpClient/**/.tcpCallSegmentation/**/.DefaultPool;
                        _isCall_ = true;
                        return _client_.Call(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = _onReturn_ }.CallOnReturn, _c74);
                    }
                    catch (Exception _error_)
                    {
                        fastCSharp.log.Default.Add(_error_, null, false);
                    }
                    finally
                    {
                        if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn{ IsReturn = false });
                    }
                    return null;
                }
            }
        }
}
namespace fastCSharp.testCase.tcpClient
{

        public class tcpCallSegmentation
        {
            /// <summary>
            /// 默认TCP调用服务器端配置信息
            /// </summary>
            protected internal static readonly fastCSharp.setup.cSharp.tcpServer defaultTcpServer;
            /// <summary>
            /// 客户端TCP调用池
            /// </summary>
            public class clientPool
            {
                /// <summary>
                /// 客户端TCP调用池
                /// </summary>
                public static readonly fastCSharp.setup.cSharp.tcpCall.clientPool ClientPool;
                static clientPool()
                {
                    ClientPool = new fastCSharp.setup.cSharp.tcpCall.clientPool(defaultTcpServer, true, "tcpCallSegmentation");
                }
            }
            /// <summary>
            /// 默认客户端TCP调用池
            /// </summary>
            public static fastCSharp.setup.cSharp.tcpCall.clientPool DefaultPool
            {
                get { return clientPool.ClientPool; }
            }
            static tcpCallSegmentation()
            {
                defaultTcpServer = fastCSharp.setup.cSharp.tcpServer.GetConfig("tcpCallSegmentation");
                defaultTcpServer.IsServer = false;
            }
        }
}