///本文件由程序自动生成,请不要自行修改
using System;
using fastCSharp;

namespace fastCSharp.testCase.tcpClient
{

        public class tcpServerSegmentation: IDisposable
        {
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            private fastCSharp.net.tcpClient _tcpClient_;
            /// <summary>
            /// TCP调用客户端是否已启动
            /// </summary>
            public bool _IsClientStart_
            {
                get
                {
                    return _tcpClient_ != null && _tcpClient_.IsStart;
                }
            }
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            /// <param name="isStart">是否启动连接</param>
            public tcpServerSegmentation(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify = null, bool isStart = true)
            {
                _tcpClient_ = new fastCSharp.net.tcpClient(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("tcpServerSegmentation", typeof(fastCSharp.testCase.tcpServerSegmentation)), verify, isStart);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                if (_tcpClient_ != null)
                {
                    _tcpClient_.Dispose();
                    _tcpClient_ = null;
                }
            }


            private static readonly byte[] _c0 = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("()action");
            public void action()
            {
                if (_tcpClient_.Call(_c0)) return;
                throw new Exception();
            }
            private static readonly byte[] _c1 = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("()actionAsynchronous");
            public void actionAsynchronous()
            {
                if (_tcpClient_.Call(_c1)) return;
                throw new Exception();
            }
            public void actionAsynchronous(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
            {
                bool _isCall_ = false;
                try
                {
                    fastCSharp.net.tcpClient _client_ = _tcpClient_;
                    if (_client_ != null)
                    {
                        _isCall_ = true;
                        _client_.Call(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = _onReturn_ }.CallOnReturn, _c1);
                    }
                }
                catch (Exception _error_)
                {
                    fastCSharp.log.Default.Add(_error_, null, false);
                }
                finally
                {
                    if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn{ IsReturn = false });
                }
            }
        }
}