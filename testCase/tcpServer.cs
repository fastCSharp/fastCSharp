using System;
using System.Threading;

namespace fastCSharp.testCase
{
    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerNoVerify", VerifyType = null)]
    public partial class tcpServerNoVerify : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(ref int intValue, out string stringValue)
        {
            isServer = intValue == inputInt;
            intValue = outputInt;
            stringValue = outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected int func()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected int funcAsynchronous()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected int func(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected int funcAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected string func(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected string funcAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }

        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType1, valueType2>(ref valueType1 value1, out valueType2 value2)
        {
            isServer = value1.Equals(genericInputOutput[typeof(valueType1)].Key);
            value1 = (valueType1)genericInputOutput[typeof(valueType1)].Value;
            value2 = (valueType2)genericInputOutput[typeof(valueType2)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected valueType func<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected valueType funcAsynchronous<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected valueType func<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected valueType funcAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected string func<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected string funcAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerNoVerify server = new fastCSharp.testCase.tcpServer.tcpServerNoVerify())
            {
                if (!server.Start()) throw new Exception("tcpServerNoVerify");
                using (fastCSharp.testCase.tcpClient.tcpServerNoVerify client = new fastCSharp.testCase.tcpClient.tcpServerNoVerify())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");

                    isServer = false;
                    client.action(inputInt);
                    checkServer("action(int)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn, inputInt);
                    checkIsReturn("actionAsynchronous(int)");

                    isServer = false;
                    client.action(inputInt, inputString);
                    checkServer("action(int,string)");

                    isServer = false;
                    int refInt = inputInt;
                    string refString;
                    client.action(ref refInt, out refString);
                    if (refInt != outputInt || refString != outputString) throw new Exception("action(ref int, out string)");
                    checkServer("action(ref int, out string)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn, inputInt, inputString);
                    checkIsReturn("actionAsynchronous(int,string)");

                    isServer = false;
                    if (client.func() != outputInt) throw new Exception("func");
                    checkServer("func");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousInt);
                    checkIsReturn("funcAsynchronous");

                    isServer = false;
                    if (client.func(inputInt) != outputInt) throw new Exception("func(int)");
                    checkServer("func(int)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousInt, inputInt);
                    checkIsReturn("funcAsynchronous(int)");

                    isServer = false;
                    if (client.func(inputInt, inputString) != outputString) throw new Exception("func(int,string)");
                    checkServer("func(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousString, inputInt, inputString);
                    checkIsReturn("funcAsynchronous(int,string)");

                    isServer = false;
                    client.action<int>();
                    checkServer("action<int>");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous<int>");

                    isServer = false;
                    client.action<int>(inputInt);
                    checkServer("action<int>(int)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn, inputInt);
                    checkIsReturn("actionAsynchronous<int>(int)");

                    isServer = false;
                    client.action<int>(inputInt, inputString);
                    checkServer("action<int>(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn, inputInt, inputString);
                    checkIsReturn("actionAsynchronous<int>(int,string)");

                    isServer = false;
                    refInt = inputInt;
                    refString = null;
                    client.action<int, string>(ref refInt, out refString);
                    if (refInt != outputInt || refString != outputString) throw new Exception("action<int, string>(ref int, out string)");
                    checkServer("action<int, string>(ref int, out string)");

                    isServer = false;
                    if (client.func<int>() != outputInt) throw new Exception("func<int>");
                    checkServer("func<int>");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousInt);
                    checkIsReturn("funcAsynchronous<int>");

                    isServer = false;
                    if (client.func<int>(inputInt) != outputInt) throw new Exception("func<int>(int)");
                    checkServer("func<int>(int)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousInt, inputInt);
                    checkIsReturn("funcAsynchronous<int>(int)");

                    isServer = false;
                    if (client.func<int>(inputInt, inputString) != outputString) throw new Exception("func<int>(int,string)");
                    checkServer("func<int>(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousString, inputInt, inputString);
                    checkIsReturn("funcAsynchronous<int>(int,string)");
                }
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerNoVerifyAsynchronous", VerifyType = null)]
    public partial class tcpServerNoVerifyAsynchronous : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action(ref int intValue, out string stringValue)
        {
            isServer = intValue == inputInt;
            intValue = outputInt;
            stringValue = outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected int func()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected int funcAsynchronous()
        {
            isServer = true;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected int func(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected int funcAsynchronous(int intValue)
        {
            isServer = intValue == inputInt;
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected string func(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected string funcAsynchronous(int intValue, string stringValue)
        {
            isServer = intValue == inputInt && stringValue == inputString;
            return outputString;
        }

        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType1, valueType2>(ref valueType1 value1, out valueType2 value2)
        {
            isServer = value1.Equals(genericInputOutput[typeof(valueType1)].Key);
            value1 = (valueType1)genericInputOutput[typeof(valueType1)].Value;
            value2 = (valueType2)genericInputOutput[typeof(valueType2)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected valueType func<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected valueType funcAsynchronous<valueType>()
        {
            isServer = true;
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected valueType func<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected valueType funcAsynchronous<valueType>(valueType value)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected string func<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected string funcAsynchronous<valueType>(valueType value, string stringValue)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key) && stringValue == inputString;
            return outputString;
        }

        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void actionCallback(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void actionCallback(int intValue, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = intValue == inputInt;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void funcCallback(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void funcCallback(int intValue, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = intValue == inputInt;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }

        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void actionCallback<valueType>(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = genericInputOutput.ContainsKey(typeof(valueType));
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void actionCallback<valueType>(valueType value, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn> _onReturn_)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn { IsReturn = true });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void funcCallback<valueType>(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<valueType>> _onReturn_)
        {
            isServer = true;
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<valueType> { IsReturn = true, Value = (valueType)genericInputOutput[typeof(valueType)].Value });
        }
        [fastCSharp.setup.cSharp.tcpServer(IsAsynchronousCallback = true)]
        protected void funcCallback<valueType>(valueType value, action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn<int>> _onReturn_)
        {
            isServer = value.Equals(genericInputOutput[typeof(valueType)].Key);
            _onReturn_(new setup.cSharp.tcpBase.asynchronousReturn<int> { IsReturn = true, Value = outputInt });
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerNoVerifyAsynchronous server = new fastCSharp.testCase.tcpServer.tcpServerNoVerifyAsynchronous())
            {
                if (!server.Start()) throw new Exception("tcpServerNoVerifyAsynchronous");
                using (fastCSharp.testCase.tcpClient.tcpServerNoVerifyAsynchronous client = new fastCSharp.testCase.tcpClient.tcpServerNoVerifyAsynchronous())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");

                    isServer = false;
                    client.action(inputInt);
                    checkServer("action(int)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn, inputInt);
                    checkIsReturn("actionAsynchronous(int)");

                    isServer = false;
                    client.action(inputInt, inputString);
                    checkServer("action(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn, inputInt, inputString);
                    checkIsReturn("actionAsynchronous(int,string)");

                    isServer = false;
                    if (client.func() != outputInt) throw new Exception("func");
                    checkServer("func");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousInt);
                    checkIsReturn("funcAsynchronous");

                    isServer = false;
                    if (client.func(inputInt) != outputInt) throw new Exception("func(int)");
                    checkServer("func(int)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousInt, inputInt);
                    checkIsReturn("funcAsynchronous(int)");

                    isServer = false;
                    if (client.func(inputInt, inputString) != outputString) throw new Exception("func(int,string)");
                    checkServer("func(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousString, inputInt, inputString);
                    checkIsReturn("funcAsynchronous(int,string)");

                    isServer = false;
                    client.action<int>();
                    checkServer("action<int>");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous<int>");

                    isServer = false;
                    client.action<int>(inputInt);
                    checkServer("action<int>(int)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn, inputInt);
                    checkIsReturn("actionAsynchronous<int>(int)");

                    isServer = false;
                    client.action<int>(inputInt, inputString);
                    checkServer("action<int>(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<int>(actionAsynchronousReturn, inputInt, inputString);
                    checkIsReturn("actionAsynchronous<int>(int,string)");

                    isServer = false;
                    if (client.func<int>() != outputInt) throw new Exception("func<int>");
                    checkServer("func<int>");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousInt);
                    checkIsReturn("funcAsynchronous<int>");

                    isServer = false;
                    if (client.func<int>(inputInt) != outputInt) throw new Exception("func<int>(int)");
                    checkServer("func<int>(int)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousInt, inputInt);
                    checkIsReturn("funcAsynchronous<int>(int)");

                    isServer = false;
                    if (client.func<int>(inputInt, inputString) != outputString) throw new Exception("func<int>(int,string)");
                    checkServer("func<int>(int,string)");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousString, inputInt, inputString);
                    checkIsReturn("funcAsynchronous<int>(int,string)");

                    isCallback = true;

                    isServer = false;
                    client.actionCallback();
                    checkCallback("actionCallback");

                    isServer = false;
                    client.actionCallback(inputInt);
                    checkCallback("actionCallback(int)");

                    isServer = false;
                    if (client.funcCallback() != outputInt) throw new Exception("funcCallback");
                    checkCallback("funcCallback");

                    isServer = false;
                    if (client.funcCallback(inputInt) != outputInt) throw new Exception("funcCallback(int)");
                    checkCallback("funcCallback(int)");

                    isServer = false;
                    client.actionCallback<int>();
                    checkCallback("actionCallback<int>");

                    isServer = false;
                    client.actionCallback<int>(inputInt);
                    checkCallback("actionCallback<int>(int)");

                    isServer = false;
                    if (client.funcCallback<int>() != outputInt) throw new Exception("funcCallback<int>");
                    checkCallback("funcCallback<int>");

                    isServer = false;
                    if (client.funcCallback<int>(inputInt) != outputInt) throw new Exception("funcCallback<int>(int)");
                    checkCallback("funcCallback<int>(int)");
                }
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerVerify", VerifyType = typeof(tcpVerify))]
    public partial class tcpServerVerify : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = true;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerVerify server = new fastCSharp.testCase.tcpServer.tcpServerVerify())
            {
                if (!server.Start()) throw new Exception("tcpServerVerify");
                using (fastCSharp.testCase.tcpClient.tcpServerVerify client = new fastCSharp.testCase.tcpClient.tcpServerVerify())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");
                }
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerVerifyAsynchronous", VerifyType = typeof(tcpVerifyAsynchronous))]
    public partial class tcpServerVerifyAsynchronous : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = true;
        }

        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerVerifyAsynchronous server = new fastCSharp.testCase.tcpServer.tcpServerVerifyAsynchronous())
            {
                if (!server.Start()) throw new Exception("tcpServerVerifyAsynchronous");
                using (fastCSharp.testCase.tcpClient.tcpServerVerifyAsynchronous client = new fastCSharp.testCase.tcpClient.tcpServerVerifyAsynchronous())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");
                }
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerGenericType", VerifyType = null)]
    public partial class tcpServerGenericType<genericType> : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = genericInputOutput.ContainsKey(typeof(genericType));
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = genericInputOutput.ContainsKey(typeof(genericType));
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected genericType func()
        {
            isServer = true;
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected int func(genericType genericValue)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key);
            return outputInt;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected genericType funcAsynchronous()
        {
            isServer = true;
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected genericType func<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (genericType)genericInputOutput[typeof(genericType)].Value;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected valueType funcAsynchronous<valueType>(genericType genericValue, valueType value)
        {
            isServer = genericValue.Equals(genericInputOutput[typeof(genericType)].Key) && value.Equals(genericInputOutput[typeof(valueType)].Key);
            return (valueType)genericInputOutput[typeof(valueType)].Value;
        }
    }
    public class tcpServerGenericType : tcpCallCheck
    {
        //[fastCSharp.setup.testCase]
        internal static bool Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerGenericType<int> server = new fastCSharp.testCase.tcpServer.tcpServerGenericType<int>())
            {
                if (!server.Start()) throw new Exception("tcpServerGenericType<int>");
                using (fastCSharp.testCase.tcpClient.tcpServerGenericType<int> client = new fastCSharp.testCase.tcpClient.tcpServerGenericType<int>())
                {
                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");

                    isServer = false;
                    if (client.func() != outputInt) throw new Exception("func");
                    checkServer("func");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous(funcAsynchronousInt);
                    checkIsReturn("funcAsynchronous");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous<string>(actionAsynchronousReturn, inputInt, inputString);
                    checkIsReturn("actionAsynchronous<string>");

                    isServer = false;
                    if (client.func<string>(inputInt, inputString) != outputInt) throw new Exception("func<string>");
                    checkServer("func<string>");
                }
            }
            using (fastCSharp.testCase.tcpServer.tcpServerGenericType<string> server = new fastCSharp.testCase.tcpServer.tcpServerGenericType<string>())
            {
                if (!server.Start()) throw new Exception("tcpServerGenericType<string>");
                using (fastCSharp.testCase.tcpClient.tcpServerGenericType<string> client = new fastCSharp.testCase.tcpClient.tcpServerGenericType<string>())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    if (client.func(inputString) != outputInt) throw new Exception("func(string)");
                    checkServer("func(string)");

                    isServer = false;
                    client.action<int>(inputString, inputInt);
                    checkServer("action<int>");

                    isServer = false;
                    isReturn = null;
                    client.funcAsynchronous<int>(funcAsynchronousInt, inputString, inputInt);
                    checkIsReturn("funcAsynchronous<int>");
                }
            }
            return true;
        }
    }

    [fastCSharp.setup.cSharp.tcpServer(Service = "tcpServerSegmentation", IsSegmentation = true)]
    public partial class tcpServerSegmentation : tcpCallCheck
    {
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = false)]
        protected void action()
        {
            isServer = true;
        }
        [fastCSharp.setup.cSharp.tcpServer(IsClientAsynchronous = true)]
        protected void actionAsynchronous()
        {
            isServer = true;
        }

        internal static void Test()
        {
            using (fastCSharp.testCase.tcpServer.tcpServerSegmentation server = new fastCSharp.testCase.tcpServer.tcpServerSegmentation())
            {
                if (!server.Start()) throw new Exception("tcpServerSegmentation");

                using (fastCSharp.testCase.tcpClient.tcpServerSegmentation client = new fastCSharp.testCase.tcpClient.tcpServerSegmentation())
                {
                    isServer = false;
                    client.action();
                    checkServer("action");

                    isServer = false;
                    isReturn = null;
                    client.actionAsynchronous(actionAsynchronousReturn);
                    checkIsReturn("actionAsynchronous");
                }
            }
        }
    }
}
