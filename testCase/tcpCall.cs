using System;
using System.Threading;

namespace fastCSharp.testCase
{
    public abstract class tcpCallCheck
    {
        public void SetTcpServer(fastCSharp.net.tcpServerBase tcpServer) { }
        protected static bool isServer;
        protected static void checkServer(string methodName)
        {
            if (!isServer) throw new Exception(methodName + " 服务器端未执行");
        }
        protected static bool isCallback;
        protected static void checkCallback(string methodName)
        {
            DateTime time = DateTime.Now.AddSeconds(1);
            while (isReturn == null && DateTime.Now < time) Thread.Sleep(1);
            if (!isServer) throw new Exception(methodName + " 服务器端未执行");
            if (!isCallback) throw new Exception(methodName + " 客户端未执行");
        }
        protected static bool? isReturn;
        protected static void checkIsReturn(string methodName)
        {
            DateTime time = DateTime.Now.AddSeconds(1);
            while (isReturn == null && DateTime.Now < time) Thread.Sleep(1);
            checkServer(methodName);
            if (isReturn == null) throw new Exception(methodName + " 没有回调");
            if (!(bool)isReturn) throw new Exception(methodName + " 回调失败");
        }
        protected static void actionAsynchronousReturn(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn value)
        {
            isReturn = value.IsReturn;
        }
        protected const int inputInt = 0x789ABCDE;
        protected const int outputInt = 0x7654321F;
        protected static void funcAsynchronousInt(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int> value)
        {
            isReturn = value.IsReturn && value.Value == outputInt;
        }
        protected const string inputString = "0x789ABCDE";
        protected const string outputString = "0x7654321F";
        protected static void funcAsynchronousString(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<string> value)
        {
            isReturn = value.IsReturn && value.Value == outputString;
        }
        protected static readonly staticDictionary<hashCode<Type>, keyValue<object, object>> genericInputOutput = new staticDictionary<hashCode<Type>, keyValue<object, object>>(new keyValue<hashCode<Type>, keyValue<object, object>>[]
            {
                new keyValue<hashCode<Type>, keyValue<object, object>>(typeof(int), new keyValue<object, object>(inputInt, outputInt)),
                new keyValue<hashCode<Type>, keyValue<object, object>>(typeof(string), new keyValue<object, object>(inputString, outputString))
            });
    }

    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallNoVerify", VerifyType = null)]
    public partial class tcpCallNoVerify : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action(ref int intValue, out string stringValue)
        {
            isServer = intValue == inputInt;
            intValue = outputInt;
            stringValue = outputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static int func()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static int funcAsynchronous()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static int func(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static int funcAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static string func(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static string funcAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }

        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action<valueType1, valueType2>(ref valueType1 value1, out valueType2 value2)
        {
            isServer = value1.Equals(genericInputOutput[typeof(valueType1)].Key);
            value1 = (valueType1)genericInputOutput[typeof(valueType1)].Value;
            value2 = (valueType2)genericInputOutput[typeof(valueType2)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static valueType func<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static valueType funcAsynchronous<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static valueType func<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static valueType funcAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static string func<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static string funcAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallNoVerify server = new fastCSharp.testCase.tcpServer.tcpCallNoVerify())
            {
                if (!server.Start()) throw new Exception("tcpCallNoVerify");
                if (fastCSharp.testCase.tcpClient.tcpCallNoVerify.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallNoVerify.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");

                isServer = false;
                tcpCall.tcpCallNoVerify.action(inputInt);
                checkServer("action(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous(actionAsynchronousReturn, inputInt);
                checkIsReturn("actionAsynchronous(int)");

                isServer = false;
                tcpCall.tcpCallNoVerify.action(inputInt, inputString);
                checkServer("action(int,string)");

                isServer = false;
                int refInt = inputInt;
                string refString;
                tcpCall.tcpCallNoVerify.action(ref refInt, out refString);
                if (refInt != outputInt || refString != outputString) throw new Exception("action(ref int, out string)");
                checkServer("action(ref int, out string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous(actionAsynchronousReturn, inputInt, inputString);
                checkIsReturn("actionAsynchronous(int,string)");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func() != outputInt) throw new Exception("func");
                checkServer("func");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous(funcAsynchronousInt);
                checkIsReturn("funcAsynchronous");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func(inputInt) != outputInt) throw new Exception("func(int)");
                checkServer("func(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous(funcAsynchronousInt, inputInt);
                checkIsReturn("funcAsynchronous(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func(inputInt, inputString) != outputString) throw new Exception("func(int,string)");
                checkServer("func(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous(funcAsynchronousString, inputInt, inputString);
                checkIsReturn("funcAsynchronous(int,string)");

                isServer = false;
                tcpCall.tcpCallNoVerify.action<int>();
                checkServer("action<int>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous<int>(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous<int>");

                isServer = false;
                tcpCall.tcpCallNoVerify.action<int>(inputInt);
                checkServer("action<int>(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous<int>(actionAsynchronousReturn, inputInt);
                checkIsReturn("actionAsynchronous<int>(int)");

                isServer = false;
                tcpCall.tcpCallNoVerify.action<int>(inputInt, inputString);
                checkServer("action<int>(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.actionAsynchronous<int>(actionAsynchronousReturn, inputInt, inputString);
                checkIsReturn("actionAsynchronous<int>(int,string)");

                isServer = false;
                refInt = inputInt;
                refString = null;
                tcpCall.tcpCallNoVerify.action<int, string>(ref refInt, out refString);
                if (refInt != outputInt || refString != outputString) throw new Exception("action<int, string>(ref int, out string)");
                checkServer("action<int, string>(ref int, out string)");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func<int>() != outputInt) throw new Exception("func<int>");
                checkServer("func<int>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous<int>(funcAsynchronousInt);
                checkIsReturn("funcAsynchronous<int>");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func<int>(inputInt) != outputInt) throw new Exception("func<int>(int)");
                checkServer("func<int>(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous<int>(funcAsynchronousInt, inputInt);
                checkIsReturn("funcAsynchronous<int>(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerify.func<int>(inputInt, inputString) != outputString) throw new Exception("func<int>(int,string)");
                checkServer("func<int>(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerify.funcAsynchronous<int>(funcAsynchronousString, inputInt, inputString);
                checkIsReturn("funcAsynchronous<int>(int,string)");
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallNoVerifyAsynchronous", VerifyType = null)]
    public partial class tcpCallNoVerifyAsynchronous : tcpCallNoVerify
    {
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void actionCallback(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void actionCallback(int intValue, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = intValue == inputInt;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void funcCallback(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void funcCallback(int intValue, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = intValue == inputInt;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }

        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void actionCallback<valueType>(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = genericInputOutput.ContainsKey(typeof(valueType));
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void actionCallback<valueType>(valueType value, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void funcCallback<valueType>(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<valueType>> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<valueType> { IsReturn = true, Value = (valueType)genericInputOutput[typeof(valueType)].Value });
        }
        [fastCSharp.setup.cSharp.tcpCall(IsAsynchronousCallback = true)]
        protected static void funcCallback<valueType>(valueType value, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallNoVerifyAsynchronous server = new fastCSharp.testCase.tcpServer.tcpCallNoVerifyAsynchronous())
            {
                if (!server.Start()) throw new Exception("tcpCallNoVerifyAsynchronous");
                if (fastCSharp.testCase.tcpClient.tcpCallNoVerifyAsynchronous.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action(inputInt);
                checkServer("action(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous(actionAsynchronousReturn, inputInt);
                checkIsReturn("actionAsynchronous(int)");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action(inputInt, inputString);
                checkServer("action(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous(actionAsynchronousReturn, inputInt, inputString);
                checkIsReturn("actionAsynchronous(int,string)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func() != outputInt) throw new Exception("func");
                checkServer("func");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous(funcAsynchronousInt);
                checkIsReturn("funcAsynchronous");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func(inputInt) != outputInt) throw new Exception("func(int)");
                checkServer("func(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous(funcAsynchronousInt, inputInt);
                checkIsReturn("funcAsynchronous(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func(inputInt, inputString) != outputString) throw new Exception("func(int,string)");
                checkServer("func(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous(funcAsynchronousString, inputInt, inputString);
                checkIsReturn("funcAsynchronous(int,string)");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action<int>();
                checkServer("action<int>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous<int>(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous<int>");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action<int>(inputInt);
                checkServer("action<int>(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous<int>(actionAsynchronousReturn, inputInt);
                checkIsReturn("actionAsynchronous<int>(int)");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.action<int>(inputInt, inputString);
                checkServer("action<int>(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.actionAsynchronous<int>(actionAsynchronousReturn, inputInt, inputString);
                checkIsReturn("actionAsynchronous<int>(int,string)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func<int>() != outputInt) throw new Exception("func<int>");
                checkServer("func<int>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous<int>(funcAsynchronousInt);
                checkIsReturn("funcAsynchronous<int>");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func<int>(inputInt) != outputInt) throw new Exception("func<int>(int)");
                checkServer("func<int>(int)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous<int>(funcAsynchronousInt, inputInt);
                checkIsReturn("funcAsynchronous<int>(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.func<int>(inputInt, inputString) != outputString) throw new Exception("func<int>(int,string)");
                checkServer("func<int>(int,string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallNoVerifyAsynchronous.funcAsynchronous<int>(funcAsynchronousString, inputInt, inputString);
                checkIsReturn("funcAsynchronous<int>(int,string)");

                isCallback = true;

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.actionCallback();
                checkCallback("actionCallBack");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.actionCallback(inputInt);
                checkCallback("actionCallBack(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.funcCallback() != outputInt) throw new Exception("funcCallback");
                checkCallback("funcCallback");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.funcCallback(inputInt) != outputInt) throw new Exception("funcCallback(int)");
                checkCallback("funcCallback(int)");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.actionCallback<int>();
                checkCallback("actionCallBack<int>");

                isServer = false;
                tcpCall.tcpCallNoVerifyAsynchronous.actionCallback<int>(inputInt);
                checkCallback("actionCallBack<int>(int)");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.funcCallback<int>() != outputInt) throw new Exception("funcCallback<int>");
                checkCallback("funcCallback<int>");

                isServer = false;
                if (tcpCall.tcpCallNoVerifyAsynchronous.funcCallback<int>(inputInt) != outputInt) throw new Exception("funcCallback<int>(int)");
                checkCallback("funcCallback<int>(int)");
            }
            return true;
        }
    }

    public abstract class tcpVerifyClient
    {
        protected static byte[] serverData = "if (socket.SendInt(serverData.Length) && socket.Send(serverData))".getBytes();
        protected static byte[] clientData1 = "if (client.SendInt(clientData1.Length) && client.Send(clientData1))".getBytes();
        protected static byte[] clientData2 = "if (client.SendInt(clientData2.Length) && client.Send(clientData2))".getBytes();
        public bool Verify(fastCSharp.net.tcpClient client)
        {
            if (client.SendInt(clientData1.Length) && client.Send(clientData1))
            {
                int length;
                if (client.ReadInt(out length) && length == serverData.Length)
                {
                    byte[] data = client.Read(length);
                    if (data != null && fastCSharp.unsafer.memory.Equal(data, serverData))
                    {
                        if (client.SendInt(clientData2.Length) && client.Send(clientData2))
                        {
                            if (client.IsSuccess()) return true;
                        }
                    }
                }
            }
            throw new Exception("Verify");
        }
    }
    public class tcpVerify : tcpVerifyClient, fastCSharp.setup.cSharp.tcpBase.ITcpVerify
    {
        public bool Verify(fastCSharp.net.tcpServer.socket socket)
        {
            int length = 0;
            if (socket.ReceiveInt(out length) && length == clientData1.Length)
            {
                byte[] data = socket.Receive(length);
                if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData1))
                {
                    if (socket.SendInt(serverData.Length) && socket.Send(serverData))
                    {
                        if (socket.ReceiveInt(out length) && length == clientData2.Length)
                        {
                            data = socket.Receive(length);
                            if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData2)) return true;
                        }
                    }
                }
            }
            throw new Exception("Verify");
        }
    }
    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallVerify", VerifyType = typeof(tcpVerify))]
    public partial class tcpCallVerify : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous()
        {
            isServer = true;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallVerify server = new fastCSharp.testCase.tcpServer.tcpCallVerify())
            {
                if (!server.Start()) throw new Exception("tcpCallVerify");
                if (fastCSharp.testCase.tcpClient.tcpCallVerify.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallVerify.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallVerify.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");
            }
            return true;
        }
    }

    public class tcpVerifyAsynchronous : tcpVerifyClient, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous
    {
        private void verify(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify)
        {
            bool isVerify = false;
            try
            {
                int length = 0;
                if (socket.ReceiveInt(out length) && length == clientData1.Length)
                {
                    byte[] data = socket.Receive(length);
                    if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData1))
                    {
                        if (socket.SendInt(serverData.Length) && socket.Send(serverData))
                        {
                            if (socket.ReceiveInt(out length) && length == clientData2.Length)
                            {
                                data = socket.Receive(length);
                                if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData2)) isVerify = true;
                            }
                        }
                    }
                }
            }
            finally
            {
                onVerify(isVerify);
                if (!isVerify) throw new Exception("Verify");
            }
        }
        private struct verifyAsynchronous
        {
            private fastCSharp.net.tcpServerAsynchronous.socket socket;
            private action<bool> onVerify;
            public verifyAsynchronous(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify)
            {
                this.socket = socket;
                this.onVerify = onVerify;
            }
            public void Start()
            {
                socket.ReceiveInt(onReceiveInt1);
            }
            private void onReceiveInt1(int? length)
            {
                if (length != null && (int)length == clientData1.Length) socket.Receive(onReceiveData1, (int)length);
                else onVerify(false);
            }
            private void onReceiveData1(byte[] data)
            {
                if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData1)) socket.SendInt(onSendInt, serverData.Length);
                else onVerify(false);
            }
            private void onSendInt(bool isSend)
            {
                if (isSend) socket.Send(onSendData, serverData);
                else onVerify(false);
            }
            private void onSendData(bool isSend)
            {
                if (isSend) socket.ReceiveInt(onReceiveInt2);
                else onVerify(false);
            }
            private void onReceiveInt2(int? length)
            {
                if (length != null && (int)length == clientData2.Length) socket.Receive(onReceiveData2, (int)length);
                else onVerify(false);
            }
            private void onReceiveData2(byte[] data)
            {
                if (data != null && fastCSharp.unsafer.memory.Equal(data, clientData2)) onVerify(true);
                else onVerify(false);
            }
        }
        public void Verify(fastCSharp.net.tcpServerAsynchronous.socket socket, action<bool> onVerify)
        {
            //verify(socket, onVerify);
            new verifyAsynchronous(socket, onVerify).Start();
        }
    }
    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallVerifyAsynchronous", VerifyType = typeof(tcpVerifyAsynchronous))]
    public partial class tcpCallVerifyAsynchronous : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous()
        {
            isServer = true;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallVerifyAsynchronous server = new fastCSharp.testCase.tcpServer.tcpCallVerifyAsynchronous())
            {
                if (!server.Start()) throw new Exception("tcpCallVerifyAsynchronous");
                if (fastCSharp.testCase.tcpClient.tcpCallVerifyAsynchronous.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallVerifyAsynchronous.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallVerifyAsynchronous.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallGenericType", VerifyType = null)]
    public partial class tcpCallGenericType<genericType> : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action()
        {
            isServer = genericInputOutput.ContainsKey(typeof(genericType));
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous()
        {
            isServer = genericInputOutput.ContainsKey(typeof(genericType));
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static genericType func()
        {
            isServer = true;
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static int func(genericType genericValue)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key);
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static genericType funcAsynchronous()
        {
            isServer = true;
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static genericType func<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static valueType funcAsynchronous<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
    }
    public class tcpCallGenericType : tcpCallCheck
    {
        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallGenericType server = new fastCSharp.testCase.tcpServer.tcpCallGenericType())
            {
                if (!server.Start()) throw new Exception("tcpCallGenericType");
                if (fastCSharp.testCase.tcpClient.tcpCallGenericType.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallGenericType<string>.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallGenericType<int>.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");

                isServer = false;
                if (tcpCall.tcpCallGenericType<int>.func() != outputInt) throw new Exception("func");
                checkServer("func");

                isServer = false;
                if (tcpCall.tcpCallGenericType<string>.func(inputString) != outputInt) throw new Exception("func(string)");
                checkServer("func(string)");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallGenericType<int>.funcAsynchronous(funcAsynchronousInt);
                checkIsReturn("funcAsynchronous");

                isServer = false;
                tcpCall.tcpCallGenericType<string>.action<int>(inputString, inputInt);
                checkServer("action<int>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallGenericType<int>.actionAsynchronous<string>(actionAsynchronousReturn, inputInt, inputString);
                checkIsReturn("actionAsynchronous<string>");

                isServer = false;
                if (tcpCall.tcpCallGenericType<int>.func<string>(inputInt, inputString) != outputInt) throw new Exception("func<string>"); 
                checkServer("func<string>");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallGenericType<string>.funcAsynchronous<int>(funcAsynchronousInt, inputString, inputInt);
                checkIsReturn("funcAsynchronous<int>");
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpCall(Service = "tcpCallSegmentation")]
    public partial class tcpCallSegmentation : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = false)]
        protected static void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpCall(IsClientAsynchronous = true)]
        protected static void actionAsynchronous()
        {
            isServer = true;
        }

        internal static void Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpCallSegmentation server = new fastCSharp.testCase.tcpServer.tcpCallSegmentation())
            {
                if (!server.Start()) throw new Exception("tcpCallSegmentation");

                if (fastCSharp.testCase.tcpClient.tcpCallSegmentation.defaultTcpServer.IsServer) throw new Exception("IsServer");

                isServer = false;
                tcpCall.tcpCallSegmentation.action();
                checkServer("action");

                isServer = false;
                isReturn = null;
                tcpCall.tcpCallSegmentation.actionAsynchronous(actionAsynchronousReturn);
                checkIsReturn("actionAsynchronous");
            }
        }
    }
}
