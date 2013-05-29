using System;

namespace fastCSharp.setup.cSharp.template
{
    class TcpCall : pub
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
        #region NOT IsAllType
        #region PART SERVERCALL
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            #region IF type.Type.IsGenericType
            [fastCSharp.setup.cSharp.tcpCall(IsGenericTypeServerMethod = true, IsIgnore = true)]
            #endregion IF type.Type.IsGenericType
            internal static class @GenericTypeServerName
            {
                #region LOOP MethodIndexs
                public static /*PUSH:Method*/@ReturnType.FullName/*PUSH:Method*/ @MethodIndexGenericName(/*LOOP:Method.Parameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:Method.Parameters*/)
                {
                    #region PUSH Method
                    /*IF:IsReturn*/
                    return /*NOTE*/(FullName)/*NOTE*//*IF:IsReturn*/@type.FullName/**/.@StaticMethodGenericName(/*LOOP:Parameters*/@ParameterJoinRefName/*LOOP:Parameters*/);
                    #endregion PUSH Method
                }
                #region NOT MethodType.Type.IsGenericType
                #region IF Method.Method.IsGenericMethod
                public static readonly System.Reflection.MethodInfo @GenericMethodInfoName;
                #endregion IF Method.Method.IsGenericMethod
                #endregion NOT MethodType.Type.IsGenericType
                #endregion LOOP MethodIndexs
                #region NOT type.Type.IsGenericType
                #region IF IsAnyGenericMethod
                static @GenericTypeServerName()
                {
                    System.Collections.Generic.Dictionary<fastCSharp.setup.cSharp.tcpBase.genericMethod, System.Reflection.MethodInfo> genericMethods = fastCSharp.setup.cSharp.tcpCall.GetGenericMethods(typeof(@type.FullName));
                    #region LOOP MethodIndexs
                    #region IF Method.Method.IsGenericMethod
                    @GenericMethodInfoName = /*PUSH:Method*/genericMethods[new fastCSharp.setup.cSharp.tcpBase.genericMethod("@MethodName", @GenericParameters.Length/*LOOP:Parameters*/, "@ParameterRef@ParameterType.FullName"/*LOOP:Parameters*/)]/*PUSH:Method*/;
                    #endregion IF Method.Method.IsGenericMethod
                    #endregion LOOP MethodIndexs
                }
                #endregion IF IsAnyGenericMethod
                #endregion NOT type.Type.IsGenericType
            }
        }
        #endregion PART SERVERCALL
        #region PART CLIENTCALL
        public static partial class tcpCall
        {
            /*NOTE*/
            public partial class /*NOTE*/@TypeNameDefinition
            {
                #region LOOP MethodIndexs
                private static readonly byte[] @MethodCommandName = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand("@Method.MethodKeyFullName");
                #region IF Attribute.IsClientSynchronous
                public static @MethodReturnType.FullName /*PUSH:Method*/@MethodGenericName/*PUSH:Method*/(/*LOOP:MethodParameters*/@ParameterTypeRefName @ParameterJoinName/*LOOP:MethodParameters*/)
                {
                    #region NOT IsAsynchronousCallback
                    #region NOT Attribute.IsSegmentation
                    if (/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServiceName/**/.defaultTcpServer.IsServer)
                    {
                        /*IF:MethodIsReturn*/
                        return /*IF:MethodIsReturn*//*NOTE*/(FullName)/*NOTE*/@type.FullName/**/.@GenericTypeServerName/*PUSH:Method*/.@MethodIndexGenericName/*PUSH:Method*/(/*LOOP:MethodParameters*/@ParameterJoinRefName/*LOOP:MethodParameters*/);
                    }
                    #endregion NOT Attribute.IsSegmentation
                    #endregion NOT IsAsynchronousCallback
                    #region IF IsInputParameter
                    /*PUSH:AutoParameter*/
                    @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName
                    {
                        #region IF MethodType.Type.IsGenericType
                        @TypeGenericParameterName = typeof(@type.FullName/**/.@GenericTypeServerName),
                        #endregion IF MethodType.Type.IsGenericType
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
                    if (/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServiceName/**/.DefaultPool.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName>/*IF:IsAnyParameter*/(@MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, _outputParameter_))
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
                    if (/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServiceName/**/.DefaultPool.Call/*IF:IsInputParameter*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName>/*IF:IsInputParameter*/(@MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/)) return/*NOTE*/default(MethodReturnType.FullName)/*NOTE*/;
                    #endregion NOT IsOutputParameter
                    throw new Exception();
                }
                #endregion IF Attribute.IsClientSynchronous
                #region IF Attribute.IsClientAsynchronous
                public static fastCSharp.net.tcpClient /*PUSH:Method*/@MethodGenericName/*PUSH:Method*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/> _onReturn_/*LOOP:MethodParameters*/, @ParameterType.FullName @ParameterName/*LOOP:MethodParameters*/)
                {
                    bool _isCall_ = false;
                    try
                    {
                        fastCSharp.setup.cSharp.tcpCall.clientPool _client_ = /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ClientPart/**/.@ServiceName/**/.DefaultPool;
                        #region IF IsInputParameter
                        /*PUSH:AutoParameter*/
                        @DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName _inputParameter_ = new /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName
                        {
                            #region IF MethodType.Type.IsGenericType
                            @TypeGenericParameterName = typeof(@type.FullName/**/.@GenericTypeServerName),
                            #endregion IF MethodType.Type.IsGenericType
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
                        return _client_.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName>/*IF:IsAnyParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturnGeneric<@MethodReturnType.FullName, /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName> { OnReturn = _onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, (/*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterGenericTypeName)(object)_outputParameter_);
                        #endregion IF MethodReturnType.Type.IsGenericParameter
                        #region NOT MethodReturnType.Type.IsGenericParameter
                        return _client_.Get/*IF:IsAnyParameter*/</*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName, /*IF:IsInputParameter*//*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName>/*IF:IsAnyParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturn<@MethodReturnType.FullName, /*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@OutputParameterTypeName> { OnReturn = _onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/, _outputParameter_);
                        #endregion NOT MethodReturnType.Type.IsGenericParameter
                        #endregion IF IsOutputParameter
                        #region NOT IsOutputParameter
                        _isCall_ = true;
                        return _client_.Call/*IF:IsInputParameter*/</*PUSH:AutoParameter*/@DefaultNamespace/*PUSH:AutoParameter*/.@ParameterPart/**/.@ServiceName/**/.@InputParameterTypeName>/*IF:IsInputParameter*/(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = /*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn>)(object)/*NOTE*/_onReturn_ }.CallOnReturn, @MethodCommandName/*IF:IsInputParameter*/, _inputParameter_/*IF:IsInputParameter*/);
                        #endregion NOT IsOutputParameter
                    }
                    catch (Exception _error_)
                    {
                        fastCSharp.log.Default.Add(_error_, null, false);
                    }
                    finally
                    {
                        if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.FullName>/*IF:MethodIsReturn*/{ IsReturn = false });
                    }
                    return null;
                }
                #endregion IF Attribute.IsClientAsynchronous
                #endregion LOOP MethodIndexs
            }
        }
        #endregion PART CLIENTCALL
        #endregion NOT IsAllType

        #region IF IsAllType
        #region PART SERVER
        #region NOT ServiceAttribute.IsAsynchronous
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public/*NOTE*/ partial/*NOTE*/ class @ServiceName : fastCSharp.net.tcpServer
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            public @ServiceName(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerify verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("@ServiceName"), verify/*IF:ServiceAttribute.VerifyType*/ ?? new @TcpVerifyType()/*IF:ServiceAttribute.VerifyType*/)
            {
                #region NAME OnCommands
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(@MethodIndexs.Length + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));
                #region LOOP MethodIndexs
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName("@Method.MethodKeyFullName"), new keyValue<action<socket, int>, bool>(@MethodIndexName, @IsInputParameter)));
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
                #region IF MethodType.Type.IsGenericType
                public fastCSharp.setup.remoteType @TypeGenericParameterName;
                #endregion IF MethodType.Type.IsGenericType
                #region IF Method.Method.IsGenericMethod
                public fastCSharp.setup.remoteType[] @GenericParameterTypeName;
                #endregion IF Method.Method.IsGenericMethod
                #region IF IsGenericParameterCallback
                public fastCSharp.setup.remoteType @ReturnTypeName;
                #endregion IF IsGenericParameterCallback
                #region LOOP MethodParameters
                public @ParameterType.GenericParameterType.FullName @ParameterName;
                #endregion LOOP MethodParameters
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
                    #region IF IsInvokeGenericMethod
                    object[] _invokeParameter_ = new object[] { /*LOOP:MethodParameters*/_inputParameter_.@ParameterJoinName/*LOOP:MethodParameters*/};
                    #endregion IF IsInvokeGenericMethod
                    #endregion IF IsInputParameter
                    /*IF:MethodIsReturn*/
                    @MethodReturnType.GenericParameterType.FullName _return_ = /*IF:MethodIsReturn*/
                    #region IF MethodType.Type.IsGenericType
                        /*IF:MethodIsReturn*/(@MethodReturnType.GenericParameterType.FullName)/*IF:MethodIsReturn*/fastCSharp.setup.cSharp.tcpCall.InvokeGenericTypeMethod(_inputParameter_.@TypeGenericParameterName, "@MethodIndexName"/*IF:Method.Method.IsGenericMethod*/, _inputParameter_.@GenericParameterTypeName/*IF:Method.Method.IsGenericMethod*//*IF:IsInputParameter*/, _invokeParameter_/*IF:IsInputParameter*/);
                    #endregion IF MethodType.Type.IsGenericType
                    #region NOT MethodType.Type.IsGenericType
                    #region NOTE
                    object _ =
                    #endregion NOTE
                    #region IF Method.Method.IsGenericMethod
                        /*IF:MethodIsReturn*/(@MethodReturnType.GenericParameterType.FullName)/*IF:MethodIsReturn*/fastCSharp.setup.cSharp.tcpCall.InvokeGenericMethod(@MethodType.FullName/**/.@GenericTypeServerName/**/.@GenericMethodInfoName, _inputParameter_.@GenericParameterTypeName/*IF:IsInputParameter*/, _invokeParameter_/*IF:IsInputParameter*/);
                    #endregion IF Method.Method.IsGenericMethod
                    #region NOT Method.Method.IsGenericMethod
                    @MethodType.FullName/**/.@GenericTypeServerName/**/.@MethodIndexGenericName(/*LOOP:MethodParameters*//*AT:ParameterRef*//*AT:ParameterRef*/_inputParameter_.@ParameterJoinName/*LOOP:MethodParameters*/);
                    #endregion NOT Method.Method.IsGenericMethod
                    #endregion NOT MethodType.Type.IsGenericType
                    #region IF IsOutputParameter
                    #region IF IsInvokeGenericMethod
                    #region LOOP MethodParameters
                    #region IF IsRefOrOut
                    _inputParameter_.@ParameterName = (@ParameterType.GenericParameterType.FullName)_invokeParameter_[@ParameterIndex];
                    #endregion IF IsRefOrOut
                    #endregion LOOP MethodParameters
                    #endregion IF IsInvokeGenericMethod
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
        /// TCP调用服务端
        /// </summary>
        public class @ServiceNameAsynchronous : fastCSharp.net.tcpServerAsynchronous
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name="attribute">TCP调用服务器端配置信息</param>
            /// <param name="verify">TCP验证实例</param>
            public @ServiceNameAsynchronous(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig("@ServiceName"), verify/*IF:ServiceAttribute.VerifyType*/ ?? new @TcpVerifyType()/*IF:ServiceAttribute.VerifyType*/)
            {
                #region FROMNAME OnCommands
                #endregion FROMNAME OnCommands
            }
            #region LOOP MethodIndexs
            #region FROMNAME Parameter
            #endregion FROMNAME Parameter
            #region IF IsAsynchronousCallback
            private struct @AsynchronousCallbackIndexName
            {
                public @ServiceNameAsynchronous Server;
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
                            data = (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/new /*NOTE*/ServiceName./*NOTE*/@OutputParameterTypeName { @ReturnName = returnValue.Value }).Serialize();
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
                    ServiceName./*NOTE*/@InputParameterTypeName _inputParameter_ = new /*NOTE*/ServiceName./*NOTE*/@InputParameterTypeName();
                    (/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)/*NOTE*/_inputParameter_).DeSerialize(_socket_.CurrentData);
                    #endregion IF IsInputParameter
                    #region IF IsInvokeGenericMethod
                    object[] _invokeParameter_ = new object[] { /*LOOP:MethodParameters*/_inputParameter_.@ParameterName, /*LOOP:MethodParameters*//*IF:IsGenericParameterCallback*/fastCSharp.setup.cSharp.tcpBase.GetGenericParameterCallback(_inputParameter_.@ReturnTypeName, /*IF:IsGenericParameterCallback*//*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<object>>)(object)/*NOTE*//*NOT:IsGenericParameterCallback*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn/*IF:MethodIsReturn*/<@MethodReturnType.GenericParameterType.FullName>/*IF:MethodIsReturn*/>)/*NOT:IsGenericParameterCallback*/new @AsynchronousCallbackIndexName { Server = this, Socket = _socket_, Identity = _identity_ }.Callback/*IF:IsGenericParameterCallback*/)/*IF:IsGenericParameterCallback*/ };
                    #endregion IF IsInvokeGenericMethod
                    _isAsync_ = true;
                    #region IF MethodType.Type.IsGenericType
                    fastCSharp.setup.cSharp.tcpCall.InvokeGenericTypeMethod(_inputParameter_.@TypeGenericParameterName, "@MethodIndexName"/*IF:Method.Method.IsGenericMethod*/, _inputParameter_.@GenericParameterTypeName/*IF:Method.Method.IsGenericMethod*/, _invokeParameter_);
                    #endregion IF MethodType.Type.IsGenericType
                    #region NOT MethodType.Type.IsGenericType
                    #region IF Method.Method.IsGenericMethod
                    fastCSharp.setup.cSharp.tcpCall.InvokeGenericMethod(@MethodType.FullName/**/.@GenericTypeServerName/**/.@GenericMethodInfoName, _inputParameter_.@GenericParameterTypeName, _invokeParameter_);
                    #endregion IF Method.Method.IsGenericMethod
                    #region NOT Method.Method.IsGenericMethod
                    @MethodType.FullName/**/.@GenericTypeServerName/**/.@MethodIndexGenericName(/*LOOP:MethodParameters*//*AT:ParameterRef*//*AT:ParameterRef*/_inputParameter_.@ParameterName, /*LOOP:MethodParameters*//*NOTE*/(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<MethodReturnType.FullName>>)/*NOTE*/new @AsynchronousCallbackIndexName { Server = this, Socket = _socket_, Identity = _identity_ }.Callback);
                    #endregion NOT Method.Method.IsGenericMethod
                    #endregion NOT MethodType.Type.IsGenericType
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
        public/*NOTE*/ partial/*NOTE*/ class @ServiceName
        {
            #region IF Attribute.IsSegmentation
            #region LOOP MethodIndexs
            #region FROMNAME Parameter
            #endregion FROMNAME Parameter
            #endregion LOOP MethodIndexs
            #endregion IF Attribute.IsSegmentation
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
                    #region NOT Attribute.IsSegmentation
                    if (defaultTcpServer.IsServer) fastCSharp.log.Default.Add("请确认 @ServiceName 服务器端是否本地调用");
                    #endregion NOT Attribute.IsSegmentation
                    ClientPool = new fastCSharp.setup.cSharp.tcpCall.clientPool(defaultTcpServer, true, "@ServiceName"/*IF:ServiceAttribute.VerifyType*/, new @TcpVerifyType()/*IF:ServiceAttribute.VerifyType*/);
                }
            }
            /// <summary>
            /// 默认客户端TCP调用池
            /// </summary>
            public static fastCSharp.setup.cSharp.tcpCall.clientPool DefaultPool
            {
                get { return clientPool.ClientPool; }
            }
            static @ServiceName()
            {
                defaultTcpServer = fastCSharp.setup.cSharp.tcpServer.GetConfig("@ServiceName");
                #region IF Attribute.IsSegmentation
                defaultTcpServer.IsServer = false;
                #endregion IF Attribute.IsSegmentation
            }
        }
        #endregion PART CLIENT
        #endregion IF IsAllType
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 默认命名空间
        /// </summary>
        public partial class DefaultNamespace
        {
            /// <summary>
            /// 调用参数代码
            /// </summary>
            public class ParameterPart : fastCSharp.setup.cSharp.template.TcpCall { }
            /// <summary>
            /// 客服端代码
            /// </summary>
            public class ClientPart : fastCSharp.setup.cSharp.template.TcpCall { }
        }
        /// <summary>
        /// 获取该函数的类型
        /// </summary>
        public class MethodType : pub { }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public class ReturnType : pub { }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public class MethodReturnType : pub { }
        /// <summary>
        /// 参数类型
        /// </summary>
        public class ParameterType : pub { }
        /// <summary>
        /// 带引用修饰的参数名称
        /// </summary>
        public class ParameterTypeRefName : pub { }
        /// <summary>
        /// 泛型类型
        /// </summary>
        public class GenericParameterType : pub { }
        /// <summary>
        /// TCP调用
        /// </summary>
        public class GenericTypeServerName
        {
            /// <summary>
            /// TCP函数调用
            /// </summary>
            /// <param name="value">调用参数</param>
            /// <returns>返回值</returns>
            public static object MethodIndexGenericName(params object[] value)
            {
                return null;
            }
            /// <summary>
            /// TCP函数调用
            /// </summary>
            /// <param name="value">调用参数</param>
            /// <returns>返回值</returns>
            public static object MethodGenericName(params object[] value)
            {
                return null;
            }
            /// <summary>
            /// 设置TCP服务调用配置
            /// </summary>
            /// <param name="value">TCP服务调用配置</param>
            public static void _setTcpServerAttribute_(params object[] value)
            {
            }
            /// <summary>
            /// 泛型函数信息
            /// </summary>
            public static readonly System.Reflection.MethodInfo GenericMethodInfoName = null;
        }
        /// <summary>
        /// TCP验证类型
        /// </summary>
        public class TcpVerifyType : fastCSharp.setup.cSharp.tcpBase.ITcpVerify, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous
        {
            /// <summary>
            /// TCP客户端同步验证
            /// </summary>
            /// <param name="socket">同步套接字</param>
            /// <returns>是否通过验证</returns>
            public bool Verify(fastCSharp.net.tcpServer.socket socket) { return false; }
            /// <summary>
            /// TCP客户端异步验证
            /// </summary>
            /// <param name="socket">同步套接字</param>
            /// <param name="onVerify">验证后的处理</param>
            public void Verify(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify) { }
            /// <summary>
            /// TCP客户端验证
            /// </summary>
            /// <param name="socket">TCP调用客户端</param>
            /// <returns>是否通过验证</returns>
            public bool Verify(fastCSharp.net.tcpClient client) { return false; }
        }
    }
    #endregion NOTE
}
