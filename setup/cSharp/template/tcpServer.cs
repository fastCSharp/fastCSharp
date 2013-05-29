using System;

namespace fastCSharp.setup.cSharp.template
{
    class tcpServer : pub
    {
        #region NOTE
        private static FullName[] MethodIndexs = null;
        private static FullName[] GenericParameters = null;
        private static FullName ParameterName = null;
        private static FullName ParameterJoinRefName = null;
        private const bool IsInputParameter = false;
        private const int ParameterIndex = 0;
        #endregion NOTE

        #region PART CLASS
        #region IF IsServerCall
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : fastCSharp.setup.cSharp.tcpServer.ITcpServer
        {
            internal static class @GenericTypeServerName
            {
                #region LOOP MethodIndexs
                public static /*PUSH:Method*/@ReturnType.FullName @MethodGenericName/*PUSH:Method*/(@type.FullName _value_/*LOOP:Method.Parameters*/, @ParameterTypeRefName @ParameterName/*LOOP:Method.Parameters*/)
                {
                    #region PUSH Method
                    /*IF:IsReturn*/
                    return /*NOTE*/(FullName)/*NOTE*//*IF:IsReturn*/_value_.@MethodGenericName(/*LOOP:Parameters*/@ParameterJoinRefName/*LOOP:Parameters*/);
                    #endregion PUSH Method
                }
                #region IF Method.Method.IsGenericMethod
                public static readonly System.Reflection.MethodInfo @GenericMethodInfoName;
                #endregion IF Method.Method.IsGenericMethod
                #endregion LOOP MethodIndexs
                #region IF IsAnyGenericMethod
                static @GenericTypeServerName()
                {
                    System.Collections.Generic.Dictionary<fastCSharp.setup.cSharp.tcpBase.genericMethod, System.Reflection.MethodInfo> genericMethods = fastCSharp.setup.cSharp.tcpServer.GetGenericMethods(typeof(@type.FullName));
                    #region LOOP MethodIndexs
                    #region IF Method.Method.IsGenericMethod
                    @GenericMethodInfoName = /*PUSH:Method*/genericMethods[new fastCSharp.setup.cSharp.tcpBase.genericMethod("@MethodName", @GenericParameters.Length/*LOOP:Parameters*/, "@ParameterRef@ParameterType.FullName"/*LOOP:Parameters*/)]/*PUSH:Method*/;
                    #endregion IF Method.Method.IsGenericMethod
                    #endregion LOOP MethodIndexs
                }
                #endregion IF IsAnyGenericMethod
            }
            #region NOTE
            public void SetTcpServer(fastCSharp.net.tcpServerBase tcpServer) { }
            #endregion NOTE
        }
        #endregion IF IsServerCall
        #region NOT IsServerCall
        #region PART SERVER
        #region NOT ServiceAttribute.IsAsynchronous
        /// <summary>
        /// TCP服务
        /// </summary>
        public/*NOTE*/ partial/*NOTE*/ class /*PUSH:type*/@TypeOnlyName/*PUSH:type*//*IF:type.Type.IsGenericType*/</*PUSH:type*/@GenericParameterNames/*PUSH:type*/>/*IF:type.Type.IsGenericType*/ : fastCSharp.net.tcpServer
        {
            /// <summary>
            /// TCP服务目标对象
            /// </summary>
            private readonly @type.FullName _value_ = new @type.FullName();
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            public /*PUSH:type*/@TypeOnlyName/*PUSH:type*/(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerify verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("@Attribute.ServiceName", typeof(@type.FullName)), verify/*IF:Attribute.VerifyType*/ ?? new @TcpVerifyType()/*IF:Attribute.VerifyType*/)
            {
                _value_.SetTcpServer(this);
                #region NAME OnCommands
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(@MethodIndexs.Length + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));
                #region LOOP MethodIndexs
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName("@Method.MethodKeyName"), new keyValue<action<socket, int>, bool>(@MethodIndexName, @IsInputParameter)));
                #endregion LOOP MethodIndexs
                this.onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(onCommands.List);
                #endregion NAME OnCommands
            }
            #region LOOP MethodIndexs
            #region NAME Parameter
            #region IF IsInputParameter
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class @InputParameterTypeName : fastCSharp.setup.cSharp.serialize.ISerialize
            {
                #region IF Method.Method.IsGenericMethod
                public fastCSharp.setup.remoteType[] @GenericParameterTypeName;
                #endregion IF Method.Method.IsGenericMethod
                #region LOOP MethodParameters
                public @ParameterType.GenericParameterType.FullName @ParameterName;
                #endregion LOOP MethodParameters
                #region IF IsGenericParameterCallback
                public fastCSharp.setup.remoteType @ReturnTypeName;
                #endregion IF IsGenericParameterCallback
                #region AT InputParameterSerialize
                #endregion AT InputParameterSerialize
                #region NOTE
                public object ParameterJoinName = null;
                public byte[] Serialize() { return null; }
                public void Serialize(memoryStream stream) { }
                public void Serialize(fastCSharp.setup.cSharp.serialize.dataSerializer serializer) { }
                public bool DeSerialize(byte[] data) { return false; }
                public bool DeSerialize(byte[] data, int startIndex, out int endIndex) { endIndex = 0; return false; }
                public void DeSerialize(fastCSharp.setup.cSharp.serialize.deSerializer deSerializer) { }
                #endregion NOTE
            }
            #endregion IF IsInputParameter
            #region IF IsOutputParameter
            internal class @OutputParameterTypeName : /*IF:MethodIsReturn*/fastCSharp.setup.cSharp.tcpBase.returnParameter<@MethodReturnType.GenericParameterType.FullName>, /*IF:MethodIsReturn*/fastCSharp.setup.cSharp.serialize.ISerialize
            {
                #region LOOP Method.OutputParameters
                public @ParameterType.GenericParameterType.FullName @ParameterName;
                #endregion LOOP Method.OutputParameters
                #region AT OutputParameterSerialize
                #endregion AT OutputParameterSerialize
                #region NOTE
                public MethodReturnType.GenericParameterType.FullName ReturnName;
                public byte[] Serialize() { return null; }
                public void Serialize(memoryStream stream) { }
                public void Serialize(fastCSharp.setup.cSharp.serialize.dataSerializer serializer) { }
                public bool DeSerialize(byte[] data) { return false; }
                public bool DeSerialize(byte[] data, int startIndex, out int endIndex) { endIndex = 0; return false; }
                public void DeSerialize(fastCSharp.setup.cSharp.serialize.deSerializer deSerializer) { }
                #endregion NOTE
            }
            #endregion IF IsOutputParameter
            #endregion NAME Parameter
            #region NOTE
            internal class @OutputParameterGenericTypeName : fastCSharp.setup.cSharp.tcpBase.returnParameter<object>, fastCSharp.setup.cSharp.serialize.ISerialize
            {
                public byte[] Serialize() { return null; }
                public void Serialize(memoryStream stream) { }
                public void Serialize(fastCSharp.setup.cSharp.serialize.dataSerializer serializer) { }
                public bool DeSerialize(byte[] data) { return false; }
                public bool DeSerialize(byte[] data, int startIndex, out int endIndex) { endIndex = 0; return false; }
                public void DeSerialize(fastCSharp.setup.cSharp.serialize.deSerializer deSerializer) { }
            }
            #endregion NOTE
            private void @MethodIndexName(socket _socket_, int _identity_)
            {
                bool _isError_ = false;
                #region IF IsOutputParameter
                byte[] _data_ = null;
                #endregion IF IsOutputParameter
                try
                {
                    #region NAME ServerMethod
                    #region IF IsInputParameter
                    @InputParameterTypeName _inputParameter_ = new @InputParameterTypeName();
                    (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/_inputParameter_).DeSerialize(_socket_.CurrentData);
                    #region IF Method.Method.IsGenericMethod
                    object[] _invokeParameter_ = new object[] { /*LOOP:MethodParameters*/_inputParameter_.@ParameterJoinName/*LOOP:MethodParameters*/};
                    #endregion IF Method.Method.IsGenericMethod
                    #endregion IF IsInputParameter
                    /*IF:MethodIsReturn*/
                    @MethodReturnType.GenericParameterType.FullName _return_ = /*IF:MethodIsReturn*/
                    #region IF Method.Method.IsGenericMethod
                        /*IF:MethodIsReturn*/(@MethodReturnType.GenericParameterType.FullName)/*IF:MethodIsReturn*/fastCSharp.setup.cSharp.tcpServer.InvokeGenericMethod(_value_, @MethodType.FullName/**/.@GenericTypeServerName/**/.@GenericMethodInfoName, _inputParameter_.@GenericParameterTypeName/*IF:IsInputParameter*/, _invokeParameter_/*IF:IsInputParameter*/);
                    #endregion IF Method.Method.IsGenericMethod
                    #region NOT Method.Method.IsGenericMethod
                    @MethodType.FullName/**/.@GenericTypeServerName/*PUSH:Method*/.@MethodGenericName/*PUSH:Method*/(_value_/*LOOP:MethodParameters*/, /*AT:ParameterRef*//*AT:ParameterRef*//*IF:ParameterType.Type.IsGenericParameter*/(@ParameterType.FullName)/*IF:ParameterType.Type.IsGenericParameter*/_inputParameter_.@ParameterName/*LOOP:MethodParameters*/);
                    #endregion NOT Method.Method.IsGenericMethod
                    #region IF IsOutputParameter
                    #region IF Method.Method.IsGenericMethod
                    #region LOOP MethodParameters
                    #region IF IsRefOrOut
                    _inputParameter_.@ParameterName = (@ParameterType.GenericParameterType.FullName)_invokeParameter_[@ParameterIndex];
                    #endregion IF IsRefOrOut
                    #endregion LOOP MethodParameters
                    #endregion IF Method.Method.IsGenericMethod
                    _data_ = (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/new @OutputParameterTypeName
                    {
                        #region LOOP Method.OutputParameters
                        @ParameterName = _inputParameter_.@ParameterName,
                        #endregion LOOP Method.OutputParameters
                        #region IF MethodIsReturn
                        @ReturnName = _return_
                        #endregion IF MethodIsReturn
                    }).Serialize();
                    #endregion IF IsOutputParameter
                    #endregion NAME ServerMethod
                }
                catch (Exception error)
                {
                    _isError_ = true;
                    fastCSharp.log.Default.Add(error, null, true);
                    if (send(_socket_, fastCSharp.net.tcpServer.status.Error, _identity_)) close(_socket_, _identity_);
                    _socket_.Dispose();
                }
                if (!_isError_)
                {
                    #region IF IsOutputParameter
                    if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_, _data_))
                    #endregion IF IsOutputParameter
                        #region NOT IsOutputParameter
                        if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_))
                        #endregion NOT IsOutputParameter
                            _socket_.Dispose();
                }
            }
            #endregion LOOP MethodIndexs
        }
        #endregion NOT ServiceAttribute.IsAsynchronous
        #region IF ServiceAttribute.IsAsynchronous
        /// <summary>
        /// TCP服务
        /// </summary>
        public class @TypeNameAsynchronous/*IF:type.Type.IsGenericType*/</*PUSH:type*/@GenericParameterNames/*PUSH:type*/>/*IF:type.Type.IsGenericType*/ : fastCSharp.net.tcpServerAsynchronous
        {
            /// <summary>
            /// TCP服务目标对象
            /// </summary>
            private readonly @type.FullName _value_ = new @type.FullName();
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            public @TypeNameAsynchronous(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("@Attribute.ServiceName", typeof(@type.FullName)), verify/*IF:Attribute.VerifyType*/ ?? new @TcpVerifyType()/*IF:Attribute.VerifyType*/)
            {
                _value_.SetTcpServer(this);
                #region FROMNAME OnCommands
                #endregion FROMNAME OnCommands
            }
            #region LOOP MethodIndexs
            #region FROMNAME Parameter
            #endregion FROMNAME Parameter
            #region IF IsAsynchronousCallback
            private struct @AsynchronousCallbackIndexName
            {
                public @TypeNameAsynchronous/*IF:type.Type.IsGenericType*/</*PUSH:type*/@GenericParameterNames/*PUSH:type*/>/*IF:type.Type.IsGenericType*/ Server;
                public socket Socket;
                public int Identity;
                public void Callback(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.GenericParameterType.FullName>/*IF:MethodIsReturn*/ returnValue)
                {
                    #region IF MethodIsReturn
                    byte[] data = null;
                    if (returnValue.IsReturn)
                    {
                        try
                        {
                            data = (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/new /*NOTE*/TypeOnlyName<int>./*NOTE*/@OutputParameterTypeName { @ReturnName = returnValue.Value }).Serialize();
                        }
                        catch (Exception error)
                        {
                            returnValue.IsReturn = false;
                            fastCSharp.log.Default.Add(error, null, true);
                        }
                    }
                    #endregion IF MethodIsReturn
                    if (returnValue.IsReturn) Server.send(Socket, Server.receiveCommand, fastCSharp.net.tcpServer.status.Success, Identity/*IF:MethodIsReturn*/, data/*IF:MethodIsReturn*/);
                    else Server.send(Socket, socket.Close, fastCSharp.net.tcpServer.status.Error, Identity);
                }
            }
            #endregion IF IsAsynchronousCallback
            private void @MethodIndexName(socket _socket_, int _identity_)
            {
                #region IF IsAsynchronousCallback
                bool _isAsync_ = false;
                #endregion IF IsAsynchronousCallback
                #region NOT IsAsynchronousCallback
                bool _isError_ = false;
                #region IF IsOutputParameter
                byte[] _data_ = null;
                #endregion IF IsOutputParameter
                #endregion NOT IsAsynchronousCallback
                try
                {
                    #region IF IsAsynchronousCallback
                    #region IF IsInputParameter
                    /*NOTE*/
                    TypeOnlyName<int>./*NOTE*/@InputParameterTypeName _inputParameter_ = new /*NOTE*/TypeOnlyName<int>./*NOTE*/@InputParameterTypeName();
                    (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/_inputParameter_).DeSerialize(_socket_.CurrentData);
                    #endregion IF IsInputParameter
                    #region IF IsInvokeGenericMethod
                    object[] _invokeParameter_ = new object[] { /*LOOP:MethodParameters*/_inputParameter_.@ParameterName, /*LOOP:MethodParameters*//*IF:IsGenericParameterCallback*/fastCSharp.setup.cSharp.tcpBase.GetGenericParameterCallback(_inputParameter_.@ReturnTypeName, /*IF:IsGenericParameterCallback*//*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>>)(object)/*NOTE*//*NOT:IsGenericParameterCallback*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.GenericParameterType.FullName>/*IF:MethodIsReturn*/>)/*NOT:IsGenericParameterCallback*/new @AsynchronousCallbackIndexName { Server = this, Socket = _socket_, Identity = _identity_ }.Callback/*IF:IsGenericParameterCallback*/)/*IF:IsGenericParameterCallback*/ };
                    #endregion IF IsInvokeGenericMethod
                    _isAsync_ = true;
                    #region IF Method.Method.IsGenericMethod
                    fastCSharp.setup.cSharp.tcpServer.InvokeGenericMethod(_value_, @MethodType.FullName/**/.@GenericTypeServerName/**/.@GenericMethodInfoName, _inputParameter_.@GenericParameterTypeName, _invokeParameter_);
                    #endregion IF Method.Method.IsGenericMethod
                    #region NOT Method.Method.IsGenericMethod
                    @MethodType.FullName/**/.@GenericTypeServerName/*PUSH:Method*/.@MethodGenericName/*PUSH:Method*/(_value_/*LOOP:MethodParameters*/, /*AT:ParameterRef*//*AT:ParameterRef*//*IF:ParameterType.Type.IsGenericParameter*/(@ParameterType.FullName)/*IF:ParameterType.Type.IsGenericParameter*/_inputParameter_.@ParameterName/*LOOP:MethodParameters*/, /*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<MethodReturnType.FullName>>)/*NOTE*/new @AsynchronousCallbackIndexName { Server = this, Socket = _socket_, Identity = _identity_ }.Callback);
                    #endregion NOT Method.Method.IsGenericMethod
                    #endregion IF IsAsynchronousCallback

                    #region NOT IsAsynchronousCallback
                    #region FROMNAME ServerMethod
                    #endregion FROMNAME ServerMethod
                    #endregion NOT IsAsynchronousCallback
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, null, true);
                    #region IF IsAsynchronousCallback
                    if (!_isAsync_)
                    #endregion IF IsAsynchronousCallback
                    {
                        #region NOT IsAsynchronousCallback
                        _isError_ = true;
                        #endregion NOT IsAsynchronousCallback
                        send(_socket_, socket.Close, fastCSharp.net.tcpServer.status.Error, _identity_);
                    }
                }
                #region NOT IsAsynchronousCallback
                if (!_isError_)
                {
                    #region IF IsOutputParameter
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_, _data_);
                    #endregion IF IsOutputParameter
                    #region NOT IsOutputParameter
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_);
                    #endregion NOT IsOutputParameter
                }
                #endregion NOT IsAsynchronousCallback
            }
            #endregion LOOP MethodIndexs
        }
        #endregion IF ServiceAttribute.IsAsynchronous
        #endregion PART SERVER
        #region PART CLIENT
        public/*NOTE*/ partial/*NOTE*/ class /*PUSH:type*/@TypeOnlyName/*PUSH:type*//*IF:type.Type.IsGenericType*/</*PUSH:type*/@GenericParameterNames/*PUSH:type*/>/*IF:type.Type.IsGenericType*/: IDisposable
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
            public /*PUSH:type*/@TypeOnlyName/*PUSH:type*/(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify = null, bool isStart = true)
                #region NOTE
                : base(null, null)
                #endregion NOTE
            {
                _tcpClient_ = new fastCSharp.net.tcpClient(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("@Attribute.ServiceName", typeof(@type.FullName)), verify/*IF:Attribute.VerifyType*/ ?? new @TcpVerifyType()/*IF:Attribute.VerifyType*/, isStart);
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

            #region IF Attribute.IsSegmentation
            #region LOOP MethodIndexs
            #region FROMNAME Parameter
            #endregion FROMNAME Parameter
            #endregion LOOP MethodIndexs
            #endregion IF Attribute.IsSegmentation

            #region LOOP MethodIndexs
            private static readonly byte[] @MethodCommandName = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("@Method.MethodKeyName");
            #region IF Attribute.IsClientSynchronous
            public @MethodReturnType.FullName /*PUSH:Method*/@MethodGenericName/*PUSH:Method*/(/*LOOP:MethodParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:MethodParameters*/)
            {
                #region IF IsInputParameter
                /*PUSH:AutoParameter*/
                @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName
                {
                    #region IF Method.Method.IsGenericMethod
                    @GenericParameterTypeName = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0/*LOOP:Method.GenericParameters*/, typeof(@FullName)/*LOOP:Method.GenericParameters*/),
                    #endregion IF Method.Method.IsGenericMethod
                    #region IF IsGenericParameterCallback
                    @ReturnTypeName = typeof(@MethodReturnType.FullName),
                    #endregion IF IsGenericParameterCallback
                    #region LOOP MethodParameters
                    #region NOT IsOut
                    @ParameterName = @ParameterName,
                    #endregion NOT IsOut
                    #endregion LOOP MethodParameters
                };
                #endregion IF IsInputParameter
                #region IF IsOutputParameter
                /*PUSH:AutoParameter*/
                @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName _outputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName();
                if (_tcpClient_.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName>/*IF:IsAnyParameter*/(@MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, _outputParameter_))
                {
                    #region LOOP MethodParameters
                    #region IF IsRefOrOut
                    @ParameterName = /*IF:Method.Method.IsGenericMethod*/(@ParameterType.FullName)/*IF:Method.Method.IsGenericMethod*/_outputParameter_.@ParameterName;
                    #endregion IF IsRefOrOut
                    #endregion LOOP MethodParameters
                    return /*IF:MethodIsReturn*//*IF:MethodReturnType.Type.IsGenericParameter*/(@MethodReturnType.FullName)/*IF:MethodReturnType.Type.IsGenericParameter*/_outputParameter_.@ReturnName/*IF:MethodIsReturn*/;
                }
                #endregion IF IsOutputParameter
                #region NOT IsOutputParameter
                if (_tcpClient_.Call/*IF:IsInputParameter*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName>/*IF:IsInputParameter*/(@MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/)) return/*NOTE*/default(/*PUSH:Method*/@ReturnType.FullName/*PUSH:Method*/)/*NOTE*/;
                #endregion NOT IsOutputParameter
                throw new Exception();
            }
            #endregion IF Attribute.IsClientSynchronous
            #region IF Attribute.IsClientAsynchronous
            public void /*PUSH:Method*/@MethodGenericName/*PUSH:Method*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_/*LOOP:MethodParameters*/, @ParameterType.FullName @ParameterName/*LOOP:MethodParameters*/)
            {
                bool _isCall_ = false;
                try
                {
                    fastCSharp.net.tcpClient _client_ = _tcpClient_;
                    if (_client_ != null)
                    {
                        #region IF IsInputParameter
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName
                        {
                            #region IF Method.Method.IsGenericMethod
                            @GenericParameterTypeName = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0/*LOOP:Method.GenericParameters*/, typeof(@FullName)/*LOOP:Method.GenericParameters*/),
                            #endregion IF Method.Method.IsGenericMethod
                            #region IF IsGenericParameterCallback
                            @ReturnTypeName = typeof(@MethodReturnType.FullName),
                            #endregion IF IsGenericParameterCallback
                            #region LOOP MethodParameters
                            #region NOT IsOut
                            @ParameterName = @ParameterName,
                            #endregion NOT IsOut
                            #endregion LOOP MethodParameters
                        };
                        #endregion IF IsInputParameter
                        #region IF IsOutputParameter
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName _outputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName();
                        _isCall_ = true;
                        #region IF MethodReturnType.Type.IsGenericParameter
                        _client_.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName>/*IF:IsAnyParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturnGeneric<@MethodReturnType.FullName, /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName> { OnReturn = _onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, (/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName)(object)_outputParameter_);
                        #endregion IF MethodReturnType.Type.IsGenericParameter
                        #region NOT MethodReturnType.Type.IsGenericParameter
                        _client_.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName>/*IF:IsAnyParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturn<@MethodReturnType.FullName, /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName> { OnReturn = _onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, _outputParameter_);
                        #endregion NOT MethodReturnType.Type.IsGenericParameter
                        #endregion IF IsOutputParameter
                        #region NOT IsOutputParameter
                        _isCall_ = true;
                        _client_.Call/*IF:IsInputParameter*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName>/*IF:IsInputParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = /*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn>)(object)/*NOTE*/_onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/);
                        #endregion NOT IsOutputParameter
                    }
                }
                catch (Exception _error_)
                {
                    fastCSharp.log.Default.Add(_error_, null, false);
                }
                finally
                {
                    if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ IsReturn = false });
                }
            }
            #endregion IF Attribute.IsClientAsynchronous
            #endregion LOOP MethodIndexs
        }
        #endregion PART CLIENT
        #endregion NOT IsServerCall
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : fastCSharp.setup.cSharp.tcpServer.ITcpServer
        {
            /// <summary>
            /// 设置TCP服务端
            /// </summary>
            /// <param name="tcpServer">TCP服务端</param>
            public void SetTcpServer(fastCSharp.net.tcpServerBase tcpServer)
            {
            }
        }
    }
    #endregion NOTE
}
