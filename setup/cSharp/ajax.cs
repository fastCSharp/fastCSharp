using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// AJAX代码生成自定义属性
    /// </summary>
    public partial class ajax : memberFilter.publicInstance
    {
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly ajax ToJsonAttribute = new ajax { IsToJson = true };
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly ajax ParseJsonAutoSetup = new ajax { IsParseJson = true, IsParseAll = true };
        /// <summary>
        /// JSON接口实现[showjim.setup]
        /// </summary>
        public class json : fastCSharp.setup.cSharp.ajax.IToJson, fastCSharp.setup.cSharp.ajax.IParseJson
        {
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <returns>JSON字符串</returns>
            public string ToJson() { return null; }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="memberMap">成员位图接口</param>
            /// <returns>JSON字符串</returns>
            public string ToJson<memberType>(memberType memberMap) where memberType : IMemberMap { return null; }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">对象转换JSON字符串</param>
            public void ToJson(toJsoner toJsoner) { }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="toJsoner">对象转换JSON字符串</param>
            /// <param name="value">对象集合</param>
            public void ToJson<valueType>(toJsoner toJsoner, IList<valueType> value) where valueType : IToJson { }
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <param name="json">JSON字符串</param>
            public void FromJson(string json) { }
            /// <summary>
            /// JSON字符串解析节点换成对象
            /// </summary>
            /// <param name="node">JSON字符串解析节点</param>
            public void FromJson(jsonNode node) { }
        }
        /// <summary>
        /// 对象转换成JSON字符串接口
        /// </summary>
        public interface IToJson
        {
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <returns>JSON字符串</returns>
            string ToJson();
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">对象转换JSON字符串</param>
            void ToJson(toJsoner toJsoner);
        }
        /// <summary>
        /// 对象转换成JSON字符串接口
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface IToJson<memberType> : IToJson where memberType : fastCSharp.setup.cSharp.IMemberMap<memberType>
        {
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="memberMap">成员位图接口</param>
            /// <returns>JSON字符串</returns>
            string ToJson(memberType memberMap);
        }
        /// <summary>
        /// JSON字符串转换成对象接口
        /// </summary>
        public interface IParseJson
        {
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <param name="json">JSON字符串</param>
            void FromJson(string json);
            /// <summary>
            /// JSON字符串解析节点换成对象
            /// </summary>
            /// <param name="node">JSON字符串解析节点</param>
            void FromJson(jsonNode node);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        public abstract class toJsoner : history
        {
            /// <summary>
            /// 十进制前缀"--0x"
            /// </summary>
            internal static readonly ulong HexPrefix;
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="isPushParent">是否需要循环检测</param>
            protected toJsoner(bool isPushParent)
            {
                jsonStream = new charStream();
                if (isPushParent) parents = new list<object>();
            }
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="parent">对象转换JSON字符串</param>
            protected toJsoner(fastCSharp.setup.cSharp.ajax.toJsoner parent)
            {
                jsonStream = parent.jsonStream;
                parents = parent.parents;
            }
            /// <summary>
            /// JSON字符串缓存
            /// </summary>
            protected internal charStream jsonStream;
            /// <summary>
            /// 当前成员数量
            /// </summary>
            protected int memberCount;
            /// <summary>
            /// 是否第一个成员
            /// </summary>
            protected internal bool isFirstMember;
            /// <summary>
            /// 反射模式成员选择
            /// </summary>
            internal setup.memberFilter memberFilter = setup.memberFilter.PublicInstance;
            /// <summary>
            /// 输出成员名称
            /// </summary>
            /// <param name="name">成员名称</param>
            protected internal unsafe void name(string name)
            {
                int length = name.Length + 4;
                jsonStream.PrepLength(length);
                fixed (char* jsonFixed = jsonStream.Array)
                {
                    char* write = jsonFixed + jsonStream.Length;
                    if (isFirstMember)
                    {
                        --length;
                        isFirstMember = false;
                    }
                    else *write++ = ',';
                    *write = fastCSharp.web.ajax.Quote;
                    fastCSharp.unsafer.String.Copy(name, ++write);
                    *(write += name.Length) = fastCSharp.web.ajax.Quote;
                    *++write = ':';
                }
                jsonStream.Unsafer.AddLength(length);
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(long value)
            {
                jsonStream.Write(fastCSharp.web.ajax.Quote);
                if (value < 0)
                {
                    jsonStream.Write('-');
                    value = -value;
                }
                valueNoQuote((ulong)value);
                jsonStream.Write(fastCSharp.web.ajax.Quote);
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(ulong value)
            {
                jsonStream.Write(fastCSharp.web.ajax.Quote);
                valueNoQuote(value);
                jsonStream.Write(fastCSharp.web.ajax.Quote);
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            private unsafe void valueNoQuote(ulong value)
            {
                char* chars = stackalloc char[20];
                if (value < 10000)
                {
                    int value32 = (int)value;
                    char* nextChar = chars + 4;
                    if (value32 < 10) *nextChar = (char)(value32 + '0');
                    else
                    {
                        int div = (value32 * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                        *nextChar = (char)((value32 - div * 10) + '0');
                        if (div < 10) *--nextChar = (char)(div + '0');
                        else
                        {
                            value32 = (div * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                            *--nextChar = (char)((div - value32 * 10) + '0');
                            if (value32 < 10) *--nextChar = (char)(value32 + '0');
                            else
                            {
                                div = (value32 * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                                *--nextChar = (char)((value32 - div * 10) + '0');
                                *--nextChar = (char)(div + '0');
                            }
                        }
                    }
                    jsonStream.Write(nextChar, 5 - (int)(nextChar - chars));
                }
                else
                {
                    char* nextChar = chars + 19;
                    ulong nextValue = value & 15;
                    *nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    if (value >= 0x1000000000L)
                    {
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    }
                    uint value32 = (uint)(value >> 4), nextValue32;
                    if (value32 >= 0x10000)
                    {
                        nextValue32 = value32 & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        nextValue32 = (value32 >>= 4) & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        nextValue32 = (value32 >>= 4) & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        nextValue32 = (value32 >>= 4) & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        value32 >>= 4;
                    }
                    if (value32 >= 0x100)
                    {
                        nextValue32 = value32 & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        nextValue32 = (value32 >>= 4) & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        value32 >>= 4;
                    }
                    if (value32 >= 0x10)
                    {
                        nextValue32 = value32 & 15;
                        *--nextChar = (char)(nextValue32 < 10 ? nextValue32 + '0' : (nextValue32 + ('0' + 'A' - '9' - 1)));
                        value32 >>= 4;
                    }
                    *--nextChar = (char)(value32 < 10 ? value32 + '0' : (value32 + ('0' + 'A' - '9' - 1)));
                    *(uint*)(nextChar -= 2) = (uint)(HexPrefix >> 32);
                    jsonStream.Write(nextChar, 20 - (int)(nextChar - chars));
                }
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(int value)
            {
                if (value < 0)
                {
                    jsonStream.Write('-');
                    value = -value;
                }
                this.value((uint)value);
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(uint value)
            {
                char* chars = stackalloc char[12];
                if (value < 10000)
                {
                    char* nextChar = chars + 4;
                    if (value < 10) *nextChar = (char)(value + '0');
                    else
                    {
                        uint div = (value * fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                        *nextChar = (char)((value - div * 10) + '0');
                        if (div < 10) *--nextChar = (char)(div + '0');
                        else
                        {
                            value = (div * fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                            *--nextChar = (char)((div - value * 10) + '0');
                            if (value < 10) *--nextChar = (char)(value + '0');
                            else
                            {
                                div = (value * fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                                *--nextChar = (char)((value - div * 10) + '0');
                                *--nextChar = (char)(div + '0');
                            }
                        }
                    }
                    jsonStream.Write(nextChar, 5 - (int)(nextChar - chars));
                }
                else
                {
                    char* nextChar = chars + 11;
                    uint nextValue = value & 15;
                    *nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    if (value >= 0x1000)
                    {
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    }
                    if (value >= 0x100)
                    {
                        nextValue = (value >>= 4) & 15;
                        *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    }
                    *--nextChar = (char)((value >>= 4) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
                    *(uint*)(nextChar -= 2) = (uint)(HexPrefix >> 32);
                    jsonStream.Write(nextChar, 12 - (int)(nextChar - chars));
                }
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(short value)
            {
                if (value < 0)
                {
                    jsonStream.Write('-');
                    value = (short)-value;
                }
                this.value((ushort)value);
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(ushort value16)
            {
                int value = value16;
                char* chars = stackalloc char[8];
                if (value < 10000)
                {
                    char* nextChar = chars + 4;
                    if (value < 10) *nextChar = (char)(value + '0');
                    else
                    {
                        int div = (value * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                        *nextChar = (char)((value - div * 10) + '0');
                        if (div < 10) *--nextChar = (char)(div + '0');
                        else
                        {
                            value = (div * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                            *--nextChar = (char)((div - value * 10) + '0');
                            if (value < 10) *--nextChar = (char)(value + '0');
                            else
                            {
                                div = (value * (int)fastCSharp.number.Div10_16Mul) >> fastCSharp.number.Div10_16Shift;
                                *--nextChar = (char)((value - div * 10) + '0');
                                *--nextChar = (char)(div + '0');
                            }
                        }
                    }
                    jsonStream.Write(nextChar, 5 - (int)(nextChar - chars));
                }
                else
                {
                    char* nextChar = chars + 7;
                    int nextValue = value & 15;
                    *nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = (value >>= 4) & 15;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    nextValue = value >> 4;
                    *--nextChar = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    *(uint*)(nextChar -= 2) = (uint)(HexPrefix >> 32);
                    jsonStream.Write(nextChar, 8 - (int)(nextChar - chars));
                }
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(sbyte value)
            {
                if (value != 0)
                {
                    char* chars = stackalloc char[6];
                    *(ulong*)chars = HexPrefix;
                    *(chars + 4) = (char)((value >> 4) + '0');
                    *(chars + 5) = (char)((value &= 15) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
                    if (value < 0) jsonStream.Write(chars + 1, 5);
                    else jsonStream.Write(chars + 2, 4);
                }
                else jsonStream.Write('0');
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="value">数字值</param>
            protected unsafe void value(byte value)
            {
                if (value != 0)
                {
                    jsonStream.Write('0');
                    int nextValue = value >> 4;
                    jsonStream.Write('x');
                    jsonStream.Write((char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1))));
                    jsonStream.Write((char)((value &= 15) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1))));
                }
                else jsonStream.Write('0');
            }
            /// <summary>
            /// 逻辑值转换成字符串
            /// </summary>
            /// <param name="value">逻辑值</param>
            protected internal void value(bool value)
            {
                jsonStream.Write(value ? '1' : '0');
            }
            /// <summary>
            /// 单精度浮点数转字符串
            /// </summary>
            /// <param name="value">单精度浮点数</param>
            protected void value(float value)
            {
                jsonStream.Write(value.ToString());
            }
            /// <summary>
            /// 双精度浮点数转字符串
            /// </summary>
            /// <param name="value">双精度浮点数</param>
            protected void value(double value)
            {
                jsonStream.Write(value.ToString());
            }
            /// <summary>
            /// 十进制数转字符串
            /// </summary>
            /// <param name="value">十进制数</param>
            protected void value(decimal value)
            {
                jsonStream.Write(value.ToString());
            }
            /// <summary>
            /// 时间转字符串
            /// </summary>
            /// <param name="value">时间</param>
            protected internal void value(DateTime value)
            {
                if (value != DateTime.MinValue)
                {
                    jsonStream.Write(fastCSharp.web.ajax.DateStart);
                    valueNoQuote(fastCSharp.web.ajax.toJavascriptMilliseconds(value));
                    jsonStream.Write(fastCSharp.web.ajax.DateEnd);
                }
                else jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 对象转字符串
            /// </summary>
            /// <param name="value">字符串</param>
            protected internal unsafe void value(string value)
            {
                int length = value.Length + 2;
                jsonStream.PrepLength(length);
                fixed (char* jsonFixed = jsonStream.Array)
                {
                    char* write = jsonFixed + jsonStream.Length;
                    *write = fastCSharp.web.ajax.Quote;
                    fastCSharp.unsafer.String.Copy(value, ++write);
                    *(write += value.Length) = fastCSharp.web.ajax.Quote;
                }
                jsonStream.Unsafer.AddLength(length);
            }
            /// <summary>
            /// 对象转字符串
            /// </summary>
            /// <param name="value">字符串</param>
            protected internal unsafe void value(char value)
            {
                charStream.unsafer unsafeStream = jsonStream.Unsafer;
                jsonStream.PrepLength(3);
                unsafeStream.Write(fastCSharp.web.ajax.Quote);
                unsafeStream.Write(value);
                unsafeStream.Write(fastCSharp.web.ajax.Quote);
            }
            /// <summary>
            /// 枚举转字符串
            /// </summary>
            /// <typeparam name="valueType">枚举类型</typeparam>
            /// <param name="value">枚举</param>
            protected void enumValue<valueType>(valueType value)
            {
                this.value(value.ToString());
            }
            /// <summary>
            /// 键值对转字符串
            /// </summary>
            /// <typeparam name="keyType">键类型</typeparam>
            /// <typeparam name="valueType">值类型</typeparam>
            /// <param name="value">键值对</param>
            protected void keyValue<keyType, valueType>(KeyValuePair<keyType, valueType> value)
            {
                jsonStream.Write('{');
                if (value.Key == null)
                {
                    jsonStream.Write(fastCSharp.web.ajax.Quote);
                    jsonStream.Write(fastCSharp.web.ajax.Quote);
                }
                else this.value(value.Key.ToString());
                jsonStream.Write(':');
                if (value.Value == null) jsonStream.Write(fastCSharp.web.ajax.Null);
                else toJson<valueType>.Get(this, value.Value);
                jsonStream.Write('}');
            }
            /// <summary>
            /// 键值对转字符串
            /// </summary>
            /// <typeparam name="keyType">键类型</typeparam>
            /// <typeparam name="valueType">值类型</typeparam>
            /// <param name="value">键值对</param>
            protected void keyValue<keyType, valueType>(keyValue<keyType, valueType> value)
            {
                keyValue(new KeyValuePair<keyType, valueType>(value.Key, value.Value));
            }
            /// <summary>
            /// 对象转字符串
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">对象</param>
            protected internal void value<valueType>(valueType value)
            {
                toJson<valueType>.Get(this, value);
            }
            /// <summary>
            /// 生成JSON字符串
            /// </summary>
            /// <returns>JSON字符串</returns>
            public override string ToString()
            {
                return fastCSharp.web.ajax.formatJavascript(jsonStream.ToString());
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <typeparam name="jsonType">JSON换成类型</typeparam>
            /// <typeparam name="memberMapType">成员位图类型</typeparam>
            /// <typeparam name="valueType">对象集合类型</typeparam>
            /// <param name="values">对象集合</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="toJson">对象转换成JSON字符串处理委托</param>
            protected void ToJson<valueType, memberMapType>(IEnumerable<valueType> values
                , memberMapType memberMap, action<valueType, memberMapType> toJson)
            {
                if (values != null)
                {
                    bool isNext = false;
                    jsonStream.Write('[');
                    foreach (valueType value in values)
                    {
                        if (isNext) jsonStream.Write(',');
                        toJson(value, memberMap);
                        isNext = true;
                    }
                    jsonStream.Write(']');
                }
                else jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <typeparam name="jsonType">JSON换成类型</typeparam>
            /// <typeparam name="memberMapType">成员位图类型</typeparam>
            /// <typeparam name="valueType">对象集合类型</typeparam>
            /// <param name="values">对象集合</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="toJson">对象转换成JSON字符串处理委托</param>
            protected void ToJsonPush<valueType, memberMapType>(IEnumerable<valueType> values
                , memberMapType memberMap, action<valueType, memberMapType> toJson)
            {
                if (values != null)
                {
                    Push(values);
                    bool isNext = false;
                    jsonStream.Write('[');
                    foreach (valueType value in values)
                    {
                        if (isNext) jsonStream.Write(',');
                        toJson(value, memberMap);
                        isNext = true;
                    }
                    jsonStream.Write(']');
                    parents.Unsafer.AddLength(-1);
                }
                else jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <typeparam name="jsonType">JSON换成类型</typeparam>
            /// <typeparam name="memberMapType">成员位图类型</typeparam>
            /// <typeparam name="valueType">对象集合类型</typeparam>
            /// <param name="values">对象集合</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="toJson">对象转换成JSON字符串处理委托</param>
            protected void ToJson<jsonType, memberMapType, valueType>(IEnumerable<valueType> values
                , memberMapType memberMap, action<jsonType, memberMapType> toJson)
                where valueType : jsonType
            {
                if (values != null)
                {
                    bool isNext = false;
                    jsonStream.Write('[');
                    foreach (jsonType value in values)
                    {
                        if (isNext) jsonStream.Write(',');
                        toJson(value, memberMap);
                        isNext = true;
                    }
                    jsonStream.Write(']');
                }
                else jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <typeparam name="jsonType">JSON换成类型</typeparam>
            /// <typeparam name="memberMapType">成员位图类型</typeparam>
            /// <typeparam name="valueType">对象集合类型</typeparam>
            /// <param name="values">对象集合</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="toJson">对象转换成JSON字符串处理委托</param>
            protected void ToJsonPush<jsonType, memberMapType, valueType>(IEnumerable<valueType> values
                , memberMapType memberMap, action<jsonType, memberMapType> toJson)
                where valueType : jsonType
            {
                if (values != null)
                {
                    Push(values);
                    bool isNext = false;
                    jsonStream.Write('[');
                    foreach (jsonType value in values)
                    {
                        if (isNext) jsonStream.Write(',');
                        toJson(value, memberMap);
                        isNext = true;
                    }
                    jsonStream.Write(']');
                    parents.Unsafer.AddLength(-1);
                }
                else jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            unsafe static toJsoner()
            {
                string hex = "--0x";
                fixed (char* hexFixed = hex) HexPrefix = *(ulong*)hexFixed;
            }
        }
        /// <summary>
        /// 字符串反序列化节点
        /// </summary>
        public struct jsonNode
        {
            /// <summary>
            /// 节点类型
            /// </summary>
            public enum nodeType
            {
                /// <summary>
                /// 空值
                /// </summary>
                Null,
                /// <summary>
                /// 字符串
                /// </summary>
                String,
                /// <summary>
                /// 整数值
                /// </summary>
                Int,
                /// <summary>
                /// 数值
                /// </summary>
                Number,
                /// <summary>
                /// 双引号子串
                /// </summary>
                DoubleSubString,
                /// <summary>
                /// 单引号子串
                /// </summary>
                SingleSubString,
                /// <summary>
                /// 毫秒时间
                /// </summary>
                DateTime,
                /// <summary>
                /// 逻辑真值
                /// </summary>
                True,
                /// <summary>
                /// 逻辑假值
                /// </summary>
                False,
                /// <summary>
                /// 列表
                /// </summary>
                List,
                /// <summary>
                /// 字典
                /// </summary>
                Dictionary,
            }
            /// <summary>
            /// 字符串解析
            /// </summary>
            internal unsafe struct unescape
            {
                /// <summary>
                /// 原字符串
                /// </summary>
                private string value;
                /// <summary>
                /// 起始位置
                /// </summary>
                private char* start;
                /// <summary>
                /// 结束位置
                /// </summary>
                private char* end;
                /// <summary>
                /// 十六进制解析位置
                /// </summary>
                private char* hex;
                /// <summary>
                /// 转义字符集合
                /// </summary>
                private char* escapeCharDataFixed;
                /// <summary>
                /// 字符串反转义
                /// </summary>
                /// <param name="value">字符串</param>
                /// <param name="start">其实位置</param>
                /// <param name="end">结束位置</param>
                /// <returns>反转义后的字符串</returns>
                public string Get(string value, char* start, char* end)
                {
                    this.value = value;
                    this.start = start;
                    this.end = end;
                    return get();
                }
                /// <summary>
                /// 字符串反转义
                /// </summary>
                /// <param name="value">字符串</param>
                /// <returns>反转义后的字符串</returns>
                public string Get(subString value)
                {
                    this.value = value.Value;
                    fixed (char* valueFixed = this.value)
                    {
                        start = valueFixed + value.StartIndex;
                        end = start + value.Length;
                        return get();
                    }
                }
                /// <summary>
                /// 字符串反转义
                /// </summary>
                /// <returns>反转义后的字符串</returns>
                private string get()
                {
                    escapeCharDataFixed = escapeCharData.Char;
                    char* start = this.start, end = this.end;
                    char quote = *--end;
                    hex = ++start;
                    *end = '\\';
                    int length = (int)(end - start);
                    while (*hex != '\\') ++hex;
                    if (hex == end)
                    {
                        *end = quote;
                        return new string(start, 0, length);
                    }
                    do
                    {
                        --length;
                        if (*++hex == 'u')
                        {
                            length = -4;
                            hex += 4;
                        }
                        else if (*hex == 'x')
                        {
                            length = -2;
                            hex += 2;
                        }
                        while (*++hex != '\\') ;
                    }
                    while (hex != end);
                    string value = new string((char)0, length);
                    fixed (char* valueFixed = value)
                    {
                        char* write = valueFixed;
                        for (hex = start; hex != end; ++hex)
                        {
                            if (*hex != '\\') *write++ = *hex;
                            else if (*++hex == 'u')
                            {
                                *write++ = (char)((parseHex() << 12) | (parseHex() << 8) | (parseHex() << 4) | parseHex());
                            }
                            else if (*hex == 'x')
                            {
                                *write++ = (char)((parseHex() << 4) | parseHex());
                            }
                            else if (*hex < escapeCharSize) *write++ = escapeCharDataFixed[*hex];
                            else *write++ = *hex;
                        }
                    }
                    *end = quote;
                    return value;
                }
                /// <summary>
                /// 十六进制字符解析
                /// </summary>
                /// <returns>十六进制数字</returns>
                private int parseHex()
                {
                    int value = *++hex - '0';
                    if ((uint)value < 10) return value;
                    value += '0';
                    value &= 0xffdf;
                    value -= 'A';
                    if ((uint)value < 6) return value += 10;
                    log.Default.Throw("未能识别的十六进制数 [" + ((uint)(hex - start)).toString() + "] = " + *hex + @"
" + new string(start, 0, (int)(end - start)), true, false);
                    return 0;
                }
                /// <summary>
                /// 转义字符集合尺寸
                /// </summary>
                private const int escapeCharSize = 128;
                /// <summary>
                /// 转义字符集合
                /// </summary>
                private static readonly pointer escapeCharData;
                static unescape()
                {
                    escapeCharData = unmanaged.Get(escapeCharSize * sizeof(char));
                    char* escapeCharDataChar = escapeCharData.Char;
                    for (int value = 0; value != escapeCharSize; ++value) escapeCharDataChar[value] = (char)value;
                    escapeCharDataChar['0'] = (char)0;
                    escapeCharDataChar['B'] = escapeCharDataChar['b'] = '\b';
                    escapeCharDataChar['F'] = escapeCharDataChar['f'] = '\f';
                    escapeCharDataChar['N'] = escapeCharDataChar['n'] = '\n';
                    escapeCharDataChar['R'] = escapeCharDataChar['r'] = '\r';
                    escapeCharDataChar['T'] = escapeCharDataChar['t'] = '\t';
                    escapeCharDataChar['V'] = escapeCharDataChar['v'] = '\v';
                }
            }
            /// <summary>
            /// 整数值
            /// </summary>
            public ulong Int
            {
                get { return (ulong)Value; }
            }
            /// <summary>
            /// 数值
            /// </summary>
            public string Number
            {
                get { return type == nodeType.String ? (string)Value : (string)(subString)Value; }
            }
            /// <summary>
            /// 字符串
            /// </summary>
            public string String
            {
                get { return type == nodeType.String ? (string)Value : new unescape().Get((subString)Value); }
            }
            /// <summary>
            /// 数值或者字符串
            /// </summary>
            public string NumberString
            {
                get
                {
                    if (type == nodeType.Number) return (subString)Value;
                    if (type == nodeType.String) return (string)Value;
                    return new unescape().Get((subString)Value);
                }
            }
            /// <summary>
            /// 字符
            /// </summary>
            public char Char
            {
                get
                {
                    string value = String;
                    if (value.Length == 1) return value[0];
                    fastCSharp.log.Default.Throw("字符长度不能为 " + value.Length.toString(), true, false);
                    return default(char);
                }
            }
            /// <summary>
            /// 毫秒时间
            /// </summary>
            public DateTime DateTime
            {
                get
                {
                    if (type == jsonNode.nodeType.DateTime)
                    {
                        long millisecond;
                        if (long.TryParse((subString)Value, out millisecond))
                        {
                            return config.web.Default.ParseJavascriptMinTime.AddTicks(millisecond * date.MillisecondTicks);
                        }
                    }
                    else if (type != nodeType.Null)
                    {
                        DateTime value;
                        string stringValue = String;
                        if (DateTime.TryParse(stringValue, out value)) return value;
                        fastCSharp.log.Default.Throw("时间转换失败 " + stringValue, true, false);
                    }
                    return default(DateTime);
                }
            }
            /// <summary>
            /// 逻辑值
            /// </summary>
            public bool Bool
            {
                get
                {
                    if (type == jsonNode.nodeType.Int) return Int != 0;
                    if (type == nodeType.True) return true;
                    if (type == nodeType.False) return false;
                    string stringValue = NumberString;
                    int intValue;
                    if (int.TryParse(stringValue, out intValue)) return intValue != 0;
                    bool value;
                    if (bool.TryParse(stringValue, out value)) return value;
                    fastCSharp.log.Default.Throw("非逻辑值", true, false);
                    return false;
                }
            }
            /// <summary>
            /// 键值集合
            /// </summary>
            public Dictionary<string, jsonNode> Dictionary
            {
                get { return (Dictionary<string, jsonNode>)Value; }
            }
            /// <summary>
            /// 数组集合
            /// </summary>
            public list<jsonNode> List
            {
                get { return (list<jsonNode>)Value; }
            }
            /// <summary>
            /// 节点数据
            /// </summary>
            internal object Value;
            /// <summary>
            /// 节点类型
            /// </summary>
            private nodeType type;
            /// <summary>
            /// 节点类型
            /// </summary>
            public nodeType Type
            {
                get { return type; }
            }
            /// <summary>
            /// 字符串值节点
            /// </summary>
            /// <param name="value">整数值</param>
            internal jsonNode(ulong value)
            {
                this.type = nodeType.Int;
                Value = value;
            }
            /// <summary>
            /// 字符串值节点
            /// </summary>
            /// <param name="value">字符串值</param>
            public jsonNode(string value)
            {
                if (value == null)
                {
                    this.type = nodeType.Null;
                    Value = null;
                }
                else
                {
                    this.type = nodeType.String;
                    Value = value;
                }
            }
            /// <summary>
            /// 字符串值节点
            /// </summary>
            /// <param name="value">字符串值</param>
            /// <param name="type">节点类型</param>
            internal jsonNode(subString value, nodeType type)
            {
                this.type = type;
                Value = value;
            }
            /// <summary>
            /// 字符串值节点
            /// </summary>
            /// <param name="value">字符串值</param>
            internal jsonNode(bool value)
            {
                this.type = value ? nodeType.True : nodeType.False;
                Value = null;
            }
            /// <summary>
            /// 键值集合节点
            /// </summary>
            /// <param name="value">键值集合</param>
            public jsonNode(Dictionary<string, jsonNode> value)
            {
                if (value == null)
                {
                    this.type = nodeType.Null;
                    Value = null;
                }
                else
                {
                    this.type = nodeType.Dictionary;
                    Value = value;
                }
            }
            /// <summary>
            /// 数组集合节点
            /// </summary>
            /// <param name="value">数组集合</param>
            internal jsonNode(list<jsonNode> value)
            {
                this.type = nodeType.List;
                Value = value;
            }
            /// <summary>
            /// 设置节点类型
            /// </summary>
            /// <param name="type">节点类型</param>
            internal void Set(nodeType type)
            {
                this.type = type;
            }
        }
        /// <summary>
        /// JSON字符串解析器
        /// </summary>
        public unsafe struct jsonParser
        {
            /// <summary>
            /// JSON字符串
            /// </summary>
            private string json;
            /// <summary>
            /// JSON字符串起始位置
            /// </summary>
            private char* start;
            /// <summary>
            /// 当前解析位置
            /// </summary>
            private char* current;
            /// <summary>
            /// JSON字符串结束位置
            /// </summary>
            private char* end;
            ///// <summary>
            ///// 十六进制解析位置
            ///// </summary>
            //private char* hex;
            /// <summary>
            /// JSON字符串结束字符
            /// </summary>
            private char endValue;
            /// <summary>
            /// 空格字符位图
            /// </summary>
            private fixedMap spaceFixedMap;
            /// <summary>
            /// 数字字符位图
            /// </summary>
            private fixedMap numberFixedMap;
            /// <summary>
            /// 键值字符位图
            /// </summary>
            private fixedMap nameFixedMap;
            /// <summary>
            /// 当前解析键值集合
            /// </summary>
            private Dictionary<string, jsonNode> currentDictionary;
            /// <summary>
            /// 解析JSON字符串
            /// </summary>
            /// <param name="json">JSON字符串</param>
            /// <returns>解析节点</returns>
            public jsonNode Parse(string json)
            {
                if (json.length() != 0)
                {
                    fixed (char* jsonFixed = json)
                    {
                        spaceFixedMap = spaceMap;
                        numberFixedMap = numberMap;
                        nameFixedMap = nameMap;
                        this.json = json;
                        current = start = jsonFixed;
                        end = jsonFixed + json.Length;
                        endValue = *--end;
                        try
                        {
                            jsonNode value = spaceValue();
                            space();
                            if (current > end) return value;
                            unknownValue();
                        }
                        finally
                        {
                            *end = endValue;
                        }
                    }
                }
                return default(jsonNode);
            }
            /// <summary>
            /// 扫描空格字符后解析JSON值
            /// </summary>
            /// <returns>解析节点</returns>
            private jsonNode spaceValue()
            {
                space();
                if (current > end) noValue();
                if (current == end)
                {
                    int number = endValue - '0';
                    if ((uint)number < 10)
                    {
                        ++current;
                        return new jsonNode((ulong)number);
                    }
                    unknownValue();
                }
                return value();
            }
            /// <summary>
            /// 解析JSON值
            /// </summary>
            /// <returns>解析节点</returns>
            private jsonNode value()
            {
                switch (*current)
                {
                    case '"':
                        return new jsonNode(getSubString(), jsonNode.nodeType.DoubleSubString);
                    case '\'':
                        return new jsonNode(getSubString(), jsonNode.nodeType.SingleSubString);
                    case '{':
                        return dictionary();
                    case '[':
                        return list();
                    case 'n':
                        *end = endValue;
                        ++current;
                        if (*current == 'u')
                        {
                            if (*++current == 'l' && *++current == 'l' && current <= end)
                            {
                                ++current;
                                return default(jsonNode);
                            }
                        }
                        else if (*current == 'e')
                        {
                            if (*++current == 'w' && *++current == ' ' && *++current == 'D' && *++current == 'a' && *++current == 't' && *++current == 'e' && *++current == '(' && current < end)
                            {
                                ++current;
                                jsonNode node = number();
                                if ((current == end ? endValue : *current) == ')')
                                {
                                    ++current;
                                    node.Set(jsonNode.nodeType.DateTime);
                                    return node;
                                }
                            }
                        }
                        break;
                    case 't':
                        *end = endValue;
                        if (*++current == 'r' && *++current == 'u' && *++current == 'e' && current <= end)
                        {
                            ++current;
                            return new jsonNode(true);
                        }
                        break;
                    case 'f':
                        *end = endValue;
                        if (*++current == 'a' && *++current == 'l' && *++current == 's' && *++current == 'e' && current <= end)
                        {
                            ++current;
                            return new jsonNode(false);
                        }
                        break;
                    default:
                        if (*current < NumberMapSize && numberFixedMap.Get(*current)) return number();
                        break;
                }
                unknownValue();
                return default(jsonNode);
            }
            /// <summary>
            /// 未能识别的值
            /// </summary>
            private void unknownValue()
            {
                *end = endValue;
                log.Default.Throw("未能识别的值 [" + ((uint)(current - start)).toString() + "] = " + *current, true, false);
            }
            /// <summary>
            /// 未找到值已结束
            /// </summary>
            private void noValue()
            {
                log.Default.Throw("未找到值已结束", true, false);
            }
            /// <summary>
            /// 扫描空格字符
            /// </summary>
            private void space()
            {
                *end = '.';
                while (*current < SpaceMapSize && spaceFixedMap.Get(*current)) ++current;
                if (current == end && endValue < SpaceMapSize && spaceFixedMap.Get(endValue)) ++current;
            }
            /// <summary>
            /// 解析字符串
            /// </summary>
            /// <returns>字符串</returns>
            private subString getSubString()
            {
                char* quoteStart = current;
                return new subString(json, (int)(quoteStart - start), (int)(searchString() - quoteStart));
            }
            /// <summary>
            /// 字符串结束位置搜索
            /// </summary>
            /// <returns>字符串结束位置</returns>
            private char* searchString()
            {
                char quote = *end = *current;
                do
                {
                    while (*++current == '\\')
                    {
                        if (*++current == 'u') current += 4;
                        else if (*current == 'x') current += 2;
                        if (current >= end) unknownValue();
                    }
                    if (*current == quote)
                    {
                        if (current == end)
                        {
                            if (endValue != quote) unknownValue();
                            *end = endValue;
                        }
                        return ++current;
                    }
                    if (*current == '\n') unknownValue();
                }
                while (true);
            }
            /// <summary>
            /// 数值解析
            /// </summary>
            /// <returns>数值字符串节点</returns>
            private jsonNode number()
            {
                char* numberStart = current;
                char sign = *current;
                if (sign == '-' && ++current == end)
                {
                    int value = endValue - '0';
                    if ((uint)value < 10) new jsonNode((ulong)-value);
                    unknownValue();
                }
                if (*current == '0')
                {
                    if ((*++current | 0x20) == 'x')
                    {
                        numberStart = ++current;
                        *end = '\n';
                        ulong value = 0;
                        for (int number = getHex(); number >= 0; number = getHex())
                        {
                            value <<= 4;
                            ++current;
                            value += (uint)number;
                        }
                        if (current == end)
                        {
                            *end = endValue;
                            int number = getHex();
                            if (number >= 0)
                            {
                                value <<= 4;
                                ++current;
                                value += (uint)number;
                            }
                        }
                        if (current == numberStart) unknownValue();
                        return sign == '0' ? new jsonNode(value) : new jsonNode((ulong)-(long)value);
                    }
                    else --current;
                }
                *end = '\n';
                while (*++current < NumberMapSize && numberFixedMap.Get(*current)) ;
                if (current == end && endValue < NumberMapSize && numberFixedMap.Get(endValue))
                {
                    ++current;
                    *end = endValue;
                }
                return new jsonNode(new subString(json, (int)(numberStart - start), (int)(current - numberStart)), jsonNode.nodeType.Number);
            }
            /// <summary>
            /// 十六进制字符解析
            /// </summary>
            /// <returns>十六进制数字,负数表示失败</returns>
            private int getHex()
            {
                int value = *current - '0';
                if ((uint)value < 10) return value;
                value += '0';
                value &= 0xffdf;
                value -= 'A';
                if ((uint)value < 6) return value += 10;
                return -1;
            }
            /// <summary>
            /// 数组值解析
            /// </summary>
            /// <param name="value">数组值</param>
            /// <returns>是否解析成功,失败表示结束</returns>
            private bool listValue(ref jsonNode value)
            {
                space();
                if (current > end) noValue();
                if (current == end)
                {
                    if (endValue == ']')
                    {
                        ++current;
                        return false;
                    }
                    unknownValue();
                }
                if (*current == ']')
                {
                    ++current;
                    return false;
                }
                value = this.value();
                return true;
            }
            /// <summary>
            /// 数组解析
            /// </summary>
            /// <returns>数组节点</returns>
            private jsonNode list()
            {
                ++current;
                list<jsonNode> values = new list<jsonNode>();
                jsonNode value = default(jsonNode);
                if (listValue(ref value))
                {
                    do
                    {
                        if (current > end) noValue();
                        space();
                        if (current > end) noValue();
                        values.Add(value);
                        if (current == end)
                        {
                            if (endValue == ']')
                            {
                                ++current;
                                return new jsonNode(values);
                            }
                            unknownValue();
                        }
                        if (*current == ',')
                        {
                            ++current;
                            if (listValue(ref value)) continue;
                            return new jsonNode(values);
                        }
                        if (*current == ']')
                        {
                            ++current;
                            return new jsonNode(values);
                        }
                        unknownValue();
                    }
                    while (true);
                }
                return new jsonNode(values);
            }
            /// <summary>
            /// 键值解析
            /// </summary>
            private void name()
            {
                *end = '\n';
                while (*current < NameMapSize && nameFixedMap.Get(*current)) ++current;
                if (current == end && endValue < NameMapSize && nameFixedMap.Get(endValue)) ++current;
            }
            /// <summary>
            /// 键值解析
            /// </summary>
            /// <param name="name">键值</param>
            /// <returns>是否解析成功,失败表示结束</returns>
            private bool getName(ref string name)
            {
                space();
                if (current > end) noValue();
                if (current == end)
                {
                    if (endValue == '}')
                    {
                        ++current;
                        return false;
                    }
                    unknownValue();
                }
                if (*current == '}')
                {
                    ++current;
                    return false;
                }
                if (*current == '\'' || *current == '"')
                {
                    char* nameStart = current, nameEnd = searchString();
                    if (current >= end) noValue();
                    name = new jsonNode.unescape().Get(json, nameStart, nameEnd);
                }
                else
                {
                    char* nameStart = current;
                    this.name();
                    if (current == nameStart) unknownValue();
                    if (current >= end) noValue();
                    name = new string(nameStart, 0, (int)(current - nameStart));
                }
                return true;
            }
            /// <summary>
            /// 键值对解析
            /// </summary>
            /// <returns>是否解析成功,失败表示结束</returns>
            private bool nameValue()
            {
                string name = null;
                if (getName(ref name))
                {
                    space();
                    if (current >= end) noValue();
                    if (*current == ':')
                    {
                        ++current;
                        space();
                        if (current >= end) noValue();
                        currentDictionary[name] = value();
                        return true;
                    }
                    unknownValue();
                }
                return false;
            }
            /// <summary>
            /// 键值集合解析
            /// </summary>
            /// <returns>键值集合节点</returns>
            private jsonNode dictionary()
            {
                ++current;
                Dictionary<string, jsonNode> save = currentDictionary, dictionary = currentDictionary = new Dictionary<string, jsonNode>();
                if (nameValue())
                {
                    do
                    {
                        if (current > end) noValue();
                        space();
                        if (current > end) noValue();
                        if (current == end)
                        {
                            if (endValue == '}')
                            {
                                ++current;
                                currentDictionary = save;
                                return new jsonNode(dictionary);
                            }
                            unknownValue();
                        }
                        if (*current == ',')
                        {
                            ++current;
                            if (nameValue()) continue;
                            currentDictionary = save;
                            return new jsonNode(dictionary);
                        }
                        if (*current == '}')
                        {
                            ++current;
                            currentDictionary = save;
                            return new jsonNode(dictionary);
                        }
                        unknownValue();
                    }
                    while (true);
                }
                currentDictionary = save;
                return new jsonNode(dictionary);
            }
            /// <summary>
            /// 空格字符位图尺寸
            /// </summary>
            internal const int SpaceMapSize = 64;
            /// <summary>
            /// 空格字符位图
            /// </summary>
            private static readonly fixedMap spaceMap;
            /// <summary>
            /// 数字字符位图尺寸
            /// </summary>
            internal const int NumberMapSize = 128;
            /// <summary>
            /// 数字字符位图
            /// </summary>
            private static readonly fixedMap numberMap;
            /// <summary>
            /// 键值字符位图尺寸
            /// </summary>
            internal const int NameMapSize = 128;
            /// <summary>
            /// 键值字符位图
            /// </summary>
            private static readonly fixedMap nameMap;
            static jsonParser()
            {
                int dataIndex = 0;
                pointer[] datas = unmanaged.Get(true, SpaceMapSize >> 3, NumberMapSize >> 3, NameMapSize >> 3);
                spaceMap = new fixedMap(datas[dataIndex++]);
                numberMap = new fixedMap(datas[dataIndex++]);
                nameMap = new fixedMap(datas[dataIndex++]);

                foreach (char value in " \r\n\t") spaceMap.Set(value);

                numberMap.Set('0', 10);
                numberMap.Set('a');
                numberMap.Set('e');
                numberMap.Set('E');
                numberMap.Set('N');
                numberMap.Set('+');
                numberMap.Set('-');
                numberMap.Set('.');

                nameMap.Set('0', 10);
                nameMap.Set('A', 26);
                nameMap.Set('a', 26);
                nameMap.Set('_');
            }
        }
        /// <summary>
        /// JSON解析节点转换对象
        /// </summary>
        public abstract class nodeParser
        {
            /// <summary>
            /// JSON解析节点
            /// </summary>
            private jsonNode node;
            /// <summary>
            /// 键值节点集合
            /// </summary>
            protected Dictionary<string, jsonNode> dictionary;
            /// <summary>
            /// JSON解析节点
            /// </summary>
            public jsonNode Node
            {
                get { return node; }
                set
                {
                    if (value.Type != jsonNode.nodeType.Dictionary) fastCSharp.log.Default.Throw(currentMemberName + " 非字典类型节点", true, false);
                    node = value;
                    dictionary = node.Dictionary;
                }
            }
            /// <summary>
            /// 当前成员名称
            /// </summary>
            protected string currentMemberName;
            /// <summary>
            /// 未知转换类型
            /// </summary>
            /// <typeparam name="valueType">类型</typeparam>
            /// <param name="node">转换节点</param>
            /// <returns>转换后的值</returns>
            protected valueType unknown<valueType>(jsonNode node)
            {
                return parseJson<valueType>.Get(node);
            }
            /// <summary>
            /// 没有可识别的字符串
            /// </summary>
            private void noString()
            {
                log.Default.Throw(currentMemberName + " 没有可识别的字符串", true, false);
            }
        }
        /// <summary>
        /// 对象转换JSON字符串(反射模式)
        /// </summary>
        internal class reflectionToJsoner : toJsoner
        {
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="isPushParent">是否需要循环检测</param>
            /// <param name="filter">成员选择</param>
            public reflectionToJsoner(bool isPushParent, setup.memberFilter filter) : base(isPushParent) 
            {
                if (filter != setup.memberFilter.Unknown) memberFilter = filter;
            }
            /// <summary>
            /// 字符串换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            public static void GetString(toJsoner toJsoner, string value)
            {
                if (value != null) toJsoner.value(value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 逻辑值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(toJsoner toJsoner, bool value)
            {
                toJsoner.value(value);
            }
            /// <summary>
            /// 逻辑值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(toJsoner toJsoner, bool? value)
            {
                if (value != null) toJsoner.value((bool)value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 时间值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">时间值</param>
            public static void GetDateTime(toJsoner toJsoner, DateTime value)
            {
                toJsoner.value(value);
            }
            /// <summary>
            /// 时间值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">时间值</param>
            public static void GetDateTime(toJsoner toJsoner, DateTime? value)
            {
                if (value != null) toJsoner.value((DateTime)value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 字符换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符</param>
            public static void GetChar(toJsoner toJsoner, char value)
            {
                toJsoner.value(value);
            }
            /// <summary>
            /// 字符换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符</param>
            public static void GetChar(toJsoner toJsoner, char? value)
            {
                if (value != null) toJsoner.value((char)value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 获取 成员对象转换成JSON字符串
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象转换成JSON字符串</returns>
            private static action<toJsoner, object> getMemberGetter(Type type)
            {
                if (type.isStruct())
                {
                    if (ToStringType.Contains(type)) return toString;
                    Type nullType = type.nullableType();
                    if (nullType != null)
                    {
                        if (ToStringType.Contains(nullType)) return toString;
                        if (nullType == typeof(bool)) return getBoolNull;
                        if (nullType == typeof(DateTime)) return getDateTimeNull;
                        if (nullType == typeof(char)) return getCharNull;
                        return (action<toJsoner, object>)Delegate.CreateDelegate(typeof(action<toJsoner, object>), typeof(reflectionToJsoner).GetMethod("getNullTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                    }
                    if (type == typeof(bool)) return getBool;
                    if (type == typeof(DateTime)) return getDateTime;
                    if (type == typeof(char)) return getChar;
                    return getObjectByType(type);
                }
                memberType memberType = type;
                if (memberType.Type.isInterface(typeof(IEnumerable)))
                {
                    memberType enumerableArgumentType = memberType.EnumerableArgumentType;
                    if (enumerableArgumentType != null)
                    {
                        return (action<toJsoner, object>)Delegate.CreateDelegate(typeof(action<toJsoner, object>), typeof(reflectionToJsoner).GetMethod("getEnumerableGenericObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                    }
                    if (memberType.IsString) return getString;
                    return getEnumerableObject;
                }
                if (memberType.Type.IsEnum) return getString;
                return getObjectByType(type);
            }
            /// <summary>
            /// 获取 成员对象转换成JSON字符串
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象转换成JSON字符串</returns>
            public static action<toJsoner, object> GetMemberGetter(Type type)
            {
                action<toJsoner, object> getMember;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getMemberLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getMemberTypes.TryGetValue(hashType, out getMember)) getMemberTypes.Add(hashType, getMember = getMemberGetter(type));
                }
                finally { getMemberLock = 0; }
                return getMember;
            }
            /// <summary>
            /// 成员对象JSON转换器访问锁
            /// </summary>
            private static int getMemberLock;
            /// <summary>
            /// 成员对象JSON转换器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<toJsoner, object>> getMemberTypes = new Dictionary<hashCode<Type>, action<toJsoner, object>>();
            /// <summary>
            /// ToJson调用换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            private static void toString(toJsoner toJsoner, object value)
            {
                toJsoner.jsonStream.Write(value.ToString());
            }
            /// <summary>
            /// 字符串换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            private static void getString(toJsoner toJsoner, object value)
            {
                toJsoner.value(value.ToString());
            }
            /// <summary>
            /// 逻辑值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">逻辑值</param>
            private static void getBool(toJsoner toJsoner, object value)
            {
                toJsoner.value((bool)value);
            }
            /// <summary>
            /// 逻辑值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">逻辑值</param>
            private static void getBoolNull(toJsoner toJsoner, object value)
            {
                toJsoner.value((bool)(bool?)value);
            }
            /// <summary>
            /// 时间值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">时间值</param>
            private static void getDateTime(toJsoner toJsoner, object value)
            {
                toJsoner.value((DateTime)value);
            }
            /// <summary>
            /// 时间值换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">时间值</param>
            private static void getDateTimeNull(toJsoner toJsoner, object value)
            {
                toJsoner.value((DateTime)(DateTime?)value);
            }
            /// <summary>
            /// 字符串换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            private static void getChar(toJsoner toJsoner, object value)
            {
                toJsoner.value((char)value);
            }
            /// <summary>
            /// 字符串换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            private static void getCharNull(toJsoner toJsoner, object value)
            {
                toJsoner.value((char)(char?)value);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerableNotNull(toJsoner toJsoner, IEnumerable value)
            {
                toJsoner.Push(value);
                charStream jsonStream = toJsoner.jsonStream;
                jsonStream.Write('[');
                bool isNext = false;
                Type type = null;
                action<toJsoner, object> getObject = null;
                foreach (object nextValue in value)
                {
                    if (isNext) jsonStream.Write(',');
                    if (nextValue != null)
                    {
                        Type nextType = nextValue.GetType();
                        if (nextType != type) getObject = getObjectByType(type = nextType);
                        getObject(toJsoner, nextValue);
                    }
                    else jsonStream.Write(fastCSharp.web.ajax.Null);
                    isNext = true;
                }
                jsonStream.Write(']');
                toJsoner.parents.Unsafer.AddLength(-1);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerable<valueType>(toJsoner toJsoner, valueType value) where valueType : IEnumerable
            {
                if (value != null) getEnumerableNotNull(toJsoner, (IEnumerable)value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerableObject(toJsoner toJsoner, object value)
            {
                getEnumerableNotNull(toJsoner, (IEnumerable)value);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullType<valueType>(toJsoner toJsoner, Nullable<valueType> value) where valueType : struct
            {
                if (value != null) toJson<valueType>.GetNotNull(toJsoner, value.Value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullTypeObject<valueType>(toJsoner toJsoner, object value) where valueType : struct
            {
                toJson<valueType>.GetNotNull(toJsoner, ((Nullable<valueType>)value).Value);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerableGenericNotNull<valueType, enumerableArgumentType>(toJsoner toJsoner, valueType value)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                toJsoner.Push(value);
                charStream jsonStream = toJsoner.jsonStream;
                jsonStream.Write('[');
                bool isNext = false;
                foreach (enumerableArgumentType nextValue in value)
                {
                    if (isNext) jsonStream.Write(',');
                    toJson<enumerableArgumentType>.Get(toJsoner, nextValue);
                    isNext = true;
                }
                jsonStream.Write(']');
                toJsoner.parents.Unsafer.AddLength(-1);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerableGeneric<valueType, enumerableArgumentType>(toJsoner toJsoner, valueType value)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                if (value != null) getEnumerableGenericNotNull<valueType, enumerableArgumentType>(toJsoner, value);
                else toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 可枚举数据换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">可枚举数据</param>
            private static void getEnumerableGenericObject<valueType, enumerableArgumentType>(toJsoner toJsoner, object value)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                getEnumerableGenericNotNull<valueType, enumerableArgumentType>(toJsoner, (valueType)value);
            }
            /// <summary>
            /// 未知类型数据换成JSON字符串
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            private static void getObject<valueType>(toJsoner toJsoner, object value)
            {
                toJson<valueType>.GetNotNull(toJsoner, (valueType)value);
            }
            /// <summary>
            /// 获取 未知类型JSON转换器
            /// </summary>
            /// <param name="type">未知类型</param>
            /// <returns>未知类型JSON转换器</returns>
            private static action<toJsoner, object> getObjectByType(Type type)
            {
                action<toJsoner, object> getObject;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getObjectLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getObjectTypes.TryGetValue(hashType, out getObject))
                    {
                        getObjectTypes.Add(hashType, getObject = (action<toJsoner, object>)Delegate.CreateDelegate(typeof(action<toJsoner, object>), typeof(reflectionToJsoner).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type)));
                    }
                }
                finally { getObjectLock = 0; }
                return getObject;
            }
            /// <summary>
            /// 未知类型JSON转换器访问锁
            /// </summary>
            private static int getObjectLock;
            /// <summary>
            /// 未知类型JSON转换器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<toJsoner, object>> getObjectTypes = new Dictionary<hashCode<Type>, action<toJsoner, object>>();
            /// <summary>
            /// ToJson调用ToString类型
            /// </summary>
            public static readonly staticHashSet<hashCode<Type>> ToStringType = new staticHashSet<hashCode<Type>>(new hashCode<Type>[]
            {
                typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte), typeof(float),
                typeof(double), typeof(decimal)
            });
            /// <summary>
            /// 不需要做循环检测的类型
            /// </summary>
            public static readonly staticHashSet<hashCode<Type>> NotPushParentType = new staticHashSet<hashCode<Type>>(new hashCode<Type>[]
            {
                typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte), typeof(float),
                typeof(double), typeof(decimal), typeof(string), typeof(char), typeof(bool), typeof(DateTime)
            });
        }
        /// <summary>
        /// 对象转换JSON字符串(反射模式)
        /// </summary>
        public static class toJson
        {
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>JSON字符串</returns>
            public static string Get<valueType>(valueType value
                , setup.memberFilter filter = setup.memberFilter.Unknown, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return toJson<valueType>.Get(value, filter, memberMap);
            }
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共成员</param>
            /// <returns>JSON字符串</returns>
            public static string Get(object value, setup.memberFilter filter = setup.memberFilter.Unknown)
            {
                return value != null ? getter(value.GetType())(value, filter) : fastCSharp.web.ajax.Null;
            }
            /// <summary>
            /// 获取 对象转换JSON字符串委托
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns>对象转换JSON字符串委托</returns>
            private static func<object, setup.memberFilter, string> getter(Type type)
            {
                func<object, setup.memberFilter, string> value;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getterLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getters.TryGetValue(hashType, out value))
                    {
                        getters.Add(hashType, value = (func<object, setup.memberFilter, string>)Delegate.CreateDelegate(typeof(func<object, setup.memberFilter, string>), typeof(toJson<>).MakeGenericType(type).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic)));
                    }
                }
                finally { getterLock = 0; }
                return value;
            }
            /// <summary>
            /// 对象转换JSON字符串委托 访问锁
            /// </summary>
            private static int getterLock;
            /// <summary>
            /// 对象转换JSON字符串委托 集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<object, setup.memberFilter, string>> getters = new Dictionary<hashCode<Type>, func<object, setup.memberFilter, string>>();
        }
        /// <summary>
        /// 对象转换JSON字符串(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal static class toJson<valueType>
        {
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <returns>JSON字符串</returns>
            private static string getObject(object value, setup.memberFilter filter)
            {
                reflectionToJsoner toJsoner = new reflectionToJsoner(isPushParent, filter);
                GetNotNull(toJsoner, (valueType)value);
                return toJsoner.ToString();
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>JSON字符串</returns>
            public static string Get(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                reflectionToJsoner toJsoner;
                if (getter != null)
                {
                    getter(toJsoner = new reflectionToJsoner(isPushParent, filter), value);
                    return toJsoner.ToString();
                }
                if (!isStruct && value == null) return fastCSharp.web.ajax.Null;
                toJsoner = new reflectionToJsoner(isPushParent, filter);
                getMember(toJsoner, value, memberMap);
                return toJsoner.ToString();
            }
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            public static void Get(toJsoner toJsoner, valueType value)
            {
                if (getter != null)
                {
                    getter(toJsoner, value);
                    return;
                }
                if (!isStruct && value == null)
                {
                    toJsoner.jsonStream.Write(fastCSharp.web.ajax.Null);
                    return;
                }
                getMember(toJsoner, value, default(memberMap<valueType>));
            }
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            public static void GetNotNull(toJsoner toJsoner, valueType value)
            {
                if (getter != null)
                {
                    getter(toJsoner, value);
                    return;
                }
                getMember(toJsoner, value, default(memberMap<valueType>));
            }
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">数据对象</param>
            /// <param name="memberMap">成员位图</param>
            private static void getMember(toJsoner toJsoner, valueType value, memberMap<valueType> memberMap)
            {
                if (isPushParent) toJsoner.Push(value);
                charStream jsonStream = toJsoner.jsonStream;
                jsonStream.Write('{');
                toJsoner.isFirstMember = true;
                foreach (keyValue<setup.memberInfo, object> memberValue in memberGroup.GetMemberValue(value, toJsoner.memberFilter, memberMap))
                {
                    toJsoner.name(memberValue.Key.MemberName);
                    if (memberValue.Value != null) memberGetters[memberValue.Key.MemberIndex](toJsoner, memberValue.Value);
                    else jsonStream.Write(fastCSharp.web.ajax.Null);
                }
                jsonStream.Write('}');
                toJsoner.isFirstMember = false;
                if (isPushParent) toJsoner.parents.Unsafer.AddLength(-1);
            }
            /// <summary>
            /// ToString调用
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            private static void toString(toJsoner toJsoner, valueType value)
            {
                toJsoner.jsonStream.Write(value.ToString());
            }
            /// <summary>
            /// 字符串调用
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            private static void getString(toJsoner toJsoner, valueType value)
            {
                toJsoner.value(value.ToString());
            }
            /// <summary>
            /// ToString调用
            /// </summary>
            /// <param name="toJsoner">JSON字符串转换器</param>
            /// <param name="value">字符串</param>
            private static void toStringNull(toJsoner toJsoner, valueType value)
            {
                toJsoner.jsonStream.Write(value != null ? value.ToString() : fastCSharp.web.ajax.Null);
            }
            /// <summary>
            /// 初始化 动态成员分组
            /// </summary>
            private static void setMemberGroup()
            {
                memberGroup = memberGroup<valueType>.Create<ajax>(ajax.ToJsonAttribute.IsAttribute, ajax.ToJsonAttribute.IsBaseTypeAttribute, ajax.ToJsonAttribute.IsInheritAttribute, value => value.CanGet);
                memberGetters = new action<toJsoner, object>[memberGroup<valueType>.MemberCount];
                setup.memberInfo[] members = memberGroup.Members;
                foreach (setup.memberInfo member in members)
                {
                    memberGetters[member.MemberIndex] = reflectionToJsoner.GetMemberGetter(member.MemberType);
                }
                isPushParent = members.any(value => value.MemberType.IsToJsonPushParent);
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            private static readonly action<toJsoner, valueType> getter;
            /// <summary>
            /// 可空类型的泛型参数类型
            /// </summary>
            private static readonly Type nullType;
            /// <summary>
            /// 是否值类型
            /// </summary>
            private static readonly bool isStruct;
            /// <summary>
            /// 是否需要循环检测
            /// </summary>
            private static bool isPushParent;
            /// <summary>
            /// 成员对象转换成JSON字符串 集合
            /// </summary>
            private static action<toJsoner, object>[] memberGetters;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            private static memberGroup<valueType> memberGroup;
            static toJson()
            {
                Type type = typeof(valueType);
                if (type.isStruct())
                {
                    isStruct = true;
                    if (reflectionToJsoner.ToStringType.Contains(type))
                    {
                        getter = toString;
                        return;
                    }
                    if ((nullType = type.nullableType()) != null)
                    {
                        if (reflectionToJsoner.ToStringType.Contains(nullType))
                        {
                            getter = toStringNull;
                            return;
                        }
                        if (nullType == typeof(bool))
                        {
                            getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, bool?>)reflectionToJsoner.GetBool;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, DateTime?>)reflectionToJsoner.GetDateTime;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, char?>)reflectionToJsoner.GetChar;
                            return;
                        }
                        isPushParent = true;
                        getter = (action<toJsoner, valueType>)Delegate.CreateDelegate(typeof(action<toJsoner, valueType>), typeof(reflectionToJsoner).GetMethod("getNullType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        return;
                    }
                    if (type == typeof(bool))
                    {
                        getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, bool>)reflectionToJsoner.GetBool;
                        return;
                    }
                    if (type == typeof(DateTime))
                    {
                        getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, DateTime>)reflectionToJsoner.GetDateTime;
                        return;
                    }
                    if (type == typeof(char))
                    {
                        getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, char>)reflectionToJsoner.GetChar;
                        return;
                    }
                    setMemberGroup();
                    return;
                }
                memberType memberType = type;
                if (memberType.Type.isInterface(typeof(IEnumerable)))
                {
                    memberType enumerableArgumentType = memberType.EnumerableArgumentType;
                    if (enumerableArgumentType != null)
                    {
                        if (!reflectionToJsoner.NotPushParentType.Contains(enumerableArgumentType.NotNullType.Type)
                            && !enumerableArgumentType.Type.IsEnum)
                        {
                            isPushParent = true;
                        }
                        getter = (action<toJsoner, valueType>)Delegate.CreateDelegate(typeof(action<toJsoner, valueType>), typeof(reflectionToJsoner).GetMethod("getEnumerableGeneric", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                        return;
                    }
                    if (memberType.IsString)
                    {
                        getter = (action<toJsoner, valueType>)(Delegate)(action<toJsoner, string>)reflectionToJsoner.GetString;
                        return;
                    }
                    isPushParent = true;
                    getter = (action<toJsoner, valueType>)Delegate.CreateDelegate(typeof(action<toJsoner, valueType>), typeof(reflectionToJsoner).GetMethod("getEnumerable", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    return;
                }
                if (memberType.Type.IsEnum)
                {
                    getter = getString;
                    return;
                }
                setMemberGroup();
            }
        }
        /// <summary>
        /// JSON解析节点转换对象
        /// </summary>
        internal class reflectionNodeParser : nodeParser
        {
            /// <summary>
            /// 获取逻辑值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>逻辑值</returns>
            public static bool GetBool(jsonNode node)
            {
                return node.Bool;
            }
            /// <summary>
            /// 获取逻辑值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>逻辑值</returns>
            public static bool? GetBoolNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (bool?)node.Bool;
            }
            /// <summary>
            /// 获取时间值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>时间值</returns>
            public static DateTime GetDateTime(jsonNode node)
            {
                return node.DateTime;
            }
            /// <summary>
            /// 获取时间值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>时间值</returns>
            public static DateTime? GetDateTimeNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (DateTime?)node.DateTime;
            }
            /// <summary>
            /// 获取字符值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符值</returns>
            public static char GetChar(jsonNode node)
            {
                return node.Char;
            }
            /// <summary>
            /// 获取字符值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符值</returns>
            public static char? GetCharNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (char?)node.Char;
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static ulong GetULong(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return node.Int;
                return parseJson<ulong>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static ulong? GetULongNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (ulong?)GetULong(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static long GetLong(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (long)node.Int;
                return parseJson<long>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static long? GetLongNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (long?)GetLong(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static uint GetUInt(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (uint)node.Int;
                return parseJson<uint>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static uint? GetUIntNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (uint?)GetUInt(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static int GetInt(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (int)node.Int;
                return parseJson<int>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static int? GetIntNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (int?)GetInt(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static short GetShort(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (short)node.Int;
                return parseJson<short>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static short? GetShortNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (short?)GetShort(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static ushort GetUShort(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (ushort)node.Int;
                return parseJson<ushort>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static ushort? GetUShortNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (ushort?)GetUShort(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static byte GetByte(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (byte)node.Int;
                return parseJson<byte>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static byte? GetByteNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (byte?)GetByte(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static sbyte GetSByte(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Int) return (sbyte)node.Int;
                return parseJson<sbyte>.GetTryParse(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            public static sbyte? GetSByteNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (sbyte?)GetSByte(node);
            }
            /// <summary>
            /// 获取字符串
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符串</returns>
            public static string GetString(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : node.String;
            }
            /// <summary>
            /// 未知类型成员换成
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>数据对象</returns>
            public static object ToObject(jsonNode node)
            {
                return node;
            }
            /// <summary>
            /// 获取TryParse值
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>TryParse值</returns>
            private static Nullable<valueType> getTryParseNull<valueType>(jsonNode node) where valueType : struct
            {
                return node.Type == jsonNode.nodeType.Null ? null : (Nullable<valueType>)parseJson<valueType>.GetTryParse(node);
            }
            /// <summary>
            /// 获取可空类型数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>可空类型数据</returns>
            private static Nullable<valueType> getNullType<valueType>(jsonNode node) where valueType : struct
            {
                return node.Type == jsonNode.nodeType.Null ? null : (Nullable<valueType>)parseJson<valueType>.GetNotNull(node);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>数组</returns>
            private static valueType[] getArrayNotNull<valueType>(jsonNode node)
            {
                list<jsonNode> nodes = node.List;
                if (nodes.Count == 0) return nullValue<valueType>.Array;
                valueType[] values = new valueType[nodes.Count];
                int index = 0;
                foreach (jsonNode nextNode in nodes) values[index++] = parseJson<valueType>.Get(nextNode);
                return values;
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>数组</returns>
            private static valueType getArray<valueType, enumerableArgumentType>(jsonNode node)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return (valueType)(IEnumerable<enumerableArgumentType>)getArrayNotNull<enumerableArgumentType>(node);
            }
            /// <summary>
            /// 获取可枚举集合
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>可枚举集合</returns>
            private static valueType getEnumerableConstructor<valueType, enumerableArgumentType>(jsonNode node)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return (valueType)parseJson<valueType>.EnumerableConstructor.Invoke(new object[] { getArrayNotNull<enumerableArgumentType>(node) });
            }
            /// <summary>
            /// 获取逻辑值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>逻辑值</returns>
            private static object getBool(jsonNode node)
            {
                return node.Bool;
            }
            /// <summary>
            /// 获取逻辑值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>逻辑值</returns>
            private static object getBoolNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (bool?)node.Bool;
            }
            /// <summary>
            /// 获取时间值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>时间值</returns>
            private static object getDateTime(jsonNode node)
            {
                return node.DateTime;
            }
            /// <summary>
            /// 获取时间值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>时间值</returns>
            private static object getDateTimeNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (DateTime?)node.DateTime;
            }
            /// <summary>
            /// 获取字符值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符值</returns>
            private static object getChar(jsonNode node)
            {
                return node.Char;
            }
            /// <summary>
            /// 获取字符值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符值</returns>
            private static object getCharNull(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : (char?)node.Char;
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getULong(jsonNode node)
            {
                return GetULong(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getULongNull(jsonNode node)
            {
                return GetULongNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getLong(jsonNode node)
            {
                return GetLong(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getLongNull(jsonNode node)
            {
                return GetLongNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getUInt(jsonNode node)
            {
                return GetUInt(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getUIntNull(jsonNode node)
            {
                return GetUIntNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getInt(jsonNode node)
            {
                return GetInt(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getIntNull(jsonNode node)
            {
                return GetIntNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getUShort(jsonNode node)
            {
                return GetUShort(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getUShortNull(jsonNode node)
            {
                return GetUShortNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getShort(jsonNode node)
            {
                return GetShort(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getShortNull(jsonNode node)
            {
                return GetShortNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getSByte(jsonNode node)
            {
                return GetSByte(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getSByteNull(jsonNode node)
            {
                return GetSByteNull(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getByte(jsonNode node)
            {
                return GetByte(node);
            }
            /// <summary>
            /// 获取整数值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>整数值</returns>
            private static object getByteNull(jsonNode node)
            {
                return GetByteNull(node);
            }
            /// <summary>
            /// 获取TryParse值
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>TryParse值</returns>
            private static object getTryParseNullObject<valueType>(jsonNode node) where valueType : struct
            {
                return node.Type == jsonNode.nodeType.Null ? null : (Nullable<valueType>)parseJson<valueType>.GetTryParse(node);
            }
            /// <summary>
            /// 获取可空类型数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>可空类型数据</returns>
            private static object getNullTypeObject<valueType>(jsonNode node) where valueType : struct
            {
                return node.Type == jsonNode.nodeType.Null ? null : (Nullable<valueType>)parseJson<valueType>.GetNotNull(node);
            }
            /// <summary>
            /// 获取TryParse值
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>TryParse值</returns>
            private static object getTryParseObject<valueType>(jsonNode node) where valueType : struct
            {
                return parseJson<valueType>.GetTryParse(node);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>数组</returns>
            private static object getArrayObject<valueType, enumerableArgumentType>(jsonNode node)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return getArrayNotNull<enumerableArgumentType>(node);
            }
            /// <summary>
            /// 获取可枚举集合
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <typeparam name="enumerableArgumentType">泛型参数类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>可枚举集合</returns>
            private static object getEnumerableConstructorObject<valueType, enumerableArgumentType>(jsonNode node)
                where valueType : IEnumerable<enumerableArgumentType>
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return parseJson<valueType>.EnumerableConstructor.Invoke(new object[] { getArrayNotNull<enumerableArgumentType>(node) });
            }
            /// <summary>
            /// 获取枚举值
            /// </summary>
            /// <typeparam name="valueType">枚举类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>枚举值</returns>
            private static object getEnum<valueType>(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return fastCSharp.Enum.parseEnum<valueType>(node.String, default(valueType));
            }
            /// <summary>
            /// 获取字符串
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>字符串</returns>
            private static object getString(jsonNode node)
            {
                return node.Type == jsonNode.nodeType.Null ? null : node.String;
            }
            /// <summary>
            /// 未知类型成员换成
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>数据对象</returns>
            private static object getObject<valueType>(jsonNode node)
            {
                return parseJson<valueType>.Get(node);
            }
            /// <summary>
            /// 获取 未知类型成员对象转换器
            /// </summary>
            /// <param name="type">未知类型</param>
            /// <returns>未知类型成员对象转换器</returns>
            private static func<jsonNode, object> getObjectByType(Type type)
            {
                func<jsonNode, object> getObject;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getObjectLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getObjectTypes.TryGetValue(hashType, out getObject))
                    {
                        getObjectTypes.Add(hashType, getObject = (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type)));
                    }
                }
                finally { getObjectLock = 0; }
                return getObject;
            }
            /// <summary>
            /// 未知类型成员对象转换器访问锁
            /// </summary>
            private static int getObjectLock;
            /// <summary>
            /// 未知类型成员对象转换器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<jsonNode, object>> getObjectTypes = new Dictionary<hashCode<Type>, func<jsonNode, object>>();
            /// 获取 JSON节点转换成员对象
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>JSON节点转换成员对象</returns>
            private static func<jsonNode, object> getMemberGetter(Type type)
            {
                if (type.isStruct())
                {
                    Type nullType = type.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNull;
                        if (nullType == typeof(DateTime)) return getDateTimeNull;
                        if (nullType == typeof(char)) return getCharNull;
                        if (nullType == typeof(int)) return getIntNull;
                        if (nullType == typeof(uint)) return getUIntNull;
                        if (nullType == typeof(long)) return getLongNull;
                        if (nullType == typeof(ulong)) return getULongNull;
                        if (nullType == typeof(byte)) return getByteNull;
                        if (nullType == typeof(sbyte)) return getSByteNull;
                        if (nullType == typeof(short)) return getShortNull;
                        if (nullType == typeof(ushort)) return getUShortNull;
                        MethodInfo tryParseMethodNull = nullType.getTryParse();
                        if (tryParseMethodNull != null)
                        {
                            return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getTryParseNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        }
                        return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getNullTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                    }
                    if (type == typeof(bool)) return getBool;
                    if (type == typeof(DateTime)) return getDateTime;
                    if (type == typeof(char)) return getChar;
                    if (type == typeof(int)) return getInt;
                    if (type == typeof(uint)) return getUInt;
                    if (type == typeof(long)) return getLong;
                    if (type == typeof(ulong)) return getULong;
                    if (type == typeof(byte)) return getByte;
                    if (type == typeof(sbyte)) return getSByte;
                    if (type == typeof(short)) return getShort;
                    if (type == typeof(ushort)) return getUShort;
                    MethodInfo tryParseMethod = type.getTryParse();
                    if (tryParseMethod != null)
                    {
                        return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getTryParseObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    }
                    return getObjectByType(type);
                }
                memberType memberType = type;
                memberType enumerableArgumentType = memberType.EnumerableArgumentType;
                if (enumerableArgumentType != null)
                {
                    ConstructorInfo enumerableConstructor = memberType.EnumerableConstructor;
                    if (enumerableConstructor == null)
                    {
                        return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                    }
                    return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getEnumerableConstructorObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                }
                if (memberType.Type.IsEnum)
                {
                    return (func<jsonNode, object>)Delegate.CreateDelegate(typeof(func<jsonNode, object>), typeof(reflectionNodeParser).GetMethod("getEnum", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                }
                if (memberType.IsString) return getString;
                if (type == typeof(object)) return ToObject;
                return getObjectByType(type);
            }
            /// <summary>
            /// 获取 JSON节点转换成员对象
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>JSON节点转换成员对象</returns>
            public static func<jsonNode, object> GetMemberGetter(Type type)
            {
                func<jsonNode, object> getMember;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getMemberLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getMemberTypes.TryGetValue(hashType, out getMember)) getMemberTypes.Add(hashType, getMember = getMemberGetter(type));
                }
                finally { getMemberLock = 0; }
                return getMember;
            }
            /// <summary>
            /// 成员对象JSON转换器访问锁
            /// </summary>
            private static int getMemberLock;
            /// <summary>
            /// 成员对象JSON转换器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<jsonNode, object>> getMemberTypes = new Dictionary<hashCode<Type>, func<jsonNode, object>>();
        }
        /// <summary>
        /// JSON字符串转换对象(反射模式)
        /// </summary>
        public static class parseJson
        {
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="json">JSON字符串</param>
            /// <returns>目标对象</returns>
            public static valueType Get<valueType>(string json)
            {
                return Get<valueType>(new jsonParser().Parse(json));
            }
            /// <summary>
            /// JSON解析节点转换成对象
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>目标对象</returns>
            public static valueType Get<valueType>(jsonNode node)
            {
                return parseJson<valueType>.Get(node);
            }
            /// <summary>
            /// JSON解析节点转换成对象(用于配置类)
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">目标对象,不能为null</param>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>目标对象</returns>
            internal static valueType GetConfig<valueType>(valueType value, jsonNode node)
            {
                if (node.Type != jsonNode.nodeType.Null) parseJson<valueType>.GetConfig(value, node);
                return value;
            }
            /// <summary>
            /// JSON解析节点转换成对象(用于配置类)
            /// </summary>
            /// <param name="value">目标对象,不能为null</param>
            /// <param name="node">字符串反序列化节点</param>
            internal static void GetConfig(object value, jsonNode node)
            {
                ((action<object, jsonNode>)Delegate.CreateDelegate(typeof(action<object, jsonNode>), getConfigMethod.MakeGenericMethod(value.GetType())))(value, node);
            }
            /// <summary>
            ///  JSON解析节点转换成对象(用于配置类)
            /// </summary>
            /// <param name="value">目标对象,不能为null</param>
            /// <param name="node">字符串反序列化节点</param>
            private static void getConfig<valueType>(object value, jsonNode node)
            {
                parseJson<valueType>.GetConfig((valueType)value, node);
            }
            /// <summary>
            /// JSON解析节点转换成对象 函数信息
            /// </summary>
            private static readonly MethodInfo getConfigMethod = typeof(parseJson).GetMethod("getConfig", BindingFlags.Static | BindingFlags.NonPublic);
        }
        /// <summary>
        /// JSON字符串转换对象(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal static class parseJson<valueType>
        {
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>目标对象</returns>
            public static valueType Get(jsonNode node)
            {
                if (getter != null) return getter(node);
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                if (node.Type != jsonNode.nodeType.Dictionary) log.Default.Throw("非字典类型节点", true, false);
                return isStruct ? GetNotNull(node) : getNotNull(node);
            }
            /// <summary>
            /// JSON字符串转换成引用类型对象
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>目标对象</returns>
            private unsafe static valueType getNotNull(jsonNode node)
            {
                valueType value = constructor<valueType>.New;
                GetConfig(value, node);
                return value;
            }
            /// <summary>
            /// JSON字符串转换成值类型对象
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>目标对象</returns>
            public unsafe static valueType GetNotNull(jsonNode node)
            {
                if (node.Type != jsonNode.nodeType.Dictionary) log.Default.Throw("非字典类型节点", true, false);
                byte* isValueFixed = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValueFixed, memberMapSize);
                object[] values = getValue(node, isValueMap);
                return values != null ? memberGroup.SetMemberValue(default(valueType), values, isValueMap) : default(valueType);
            }
            /// <summary>
            /// JSON解析节点转换成对象(用于配置类)
            /// </summary>
            /// <param name="value">目标对象,不能为null</param>
            /// <param name="node">字符串反序列化节点</param>
            public unsafe static void GetConfig(valueType value, jsonNode node)
            {
                byte* isValueFixed = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValueFixed, memberMapSize);
                object[] values = getValue(node, isValueMap);
                if (values != null) memberGroup.SetMember(value, values, isValueMap);
            }
            /// <summary>
            /// 获取成员值集合
            /// </summary>
            /// <param name="node">json解析节点</param>
            /// <param name="isValueMap">成员值位图</param>
            /// <returns>成员值集合</returns>
            private static object[] getValue(jsonNode node, fixedMap isValueMap)
            {
                bool isValue = false;
                object[] values = new object[memberCount];
                Dictionary<string, jsonNode> dictionary = node.Dictionary;
                foreach (setup.memberInfo member in members)
                {
                    jsonNode nextNode;
                    if (dictionary.TryGetValue(member.MemberName, out nextNode))
                    {
                        values[member.MemberIndex] = memberGetters[member.MemberIndex](nextNode);
                        isValueMap.Set(member.MemberIndex);
                        isValue = true;
                    }
                }
                return isValue ? values : null;
            }
            /// <summary>
            /// 获取TryParse值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>TryParse值</returns>
            public static valueType GetTryParse(jsonNode node)
            {
                valueType value;
                string stringValue = node.NumberString;
                if (tryParse(stringValue, out value)) return value;
                log.Default.Throw(typeof(valueType).fullName() + " 转换失败 " + stringValue, true, false);
                return default(valueType);
            }
            /// <summary>
            /// 获取枚举值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>枚举值</returns>
            private static valueType getEnum(jsonNode node)
            {
                if (node.Type == jsonNode.nodeType.Null) return default(valueType);
                return fastCSharp.Enum.parseEnum<valueType>(node.String, default(valueType));
            }
            /// <summary>
            /// 获取默认空值
            /// </summary>
            /// <param name="node">字符串反序列化节点</param>
            /// <returns>默认空值</returns>
            private static valueType getDefault(jsonNode node)
            {
                return default(valueType);
            }
            /// <summary>
            /// 初始化 动态成员分组
            /// </summary>
            private static void setMemberGroup()
            {
                memberGroup = memberGroup<valueType>.Create<ajax>(ajax.ToJsonAttribute.IsAttribute, ajax.ToJsonAttribute.IsBaseTypeAttribute, ajax.ToJsonAttribute.IsInheritAttribute, value => value.CanSet);
                memberCount = memberGroup<valueType>.MemberCount;
                memberMapSize = (memberCount + 7) >> 3;
                if (memberMapSize == 0) memberMapSize = 1;
                memberGetters = new func<jsonNode, object>[memberCount];
                members = memberGroup.Members;
                foreach (setup.memberInfo member in members)
                {
                    memberGetters[member.MemberIndex] = reflectionNodeParser.GetMemberGetter(member.MemberType);
                }
            }
            /// <summary>
            /// JSON解析节点转换成对象
            /// </summary>
            private static readonly func<jsonNode, valueType> getter;
            /// <summary>
            /// 可枚举构造函数
            /// </summary>
            public static readonly ConstructorInfo EnumerableConstructor;
            /// <summary>
            /// TryParse函数
            /// </summary>
            private static readonly tryParse<valueType> tryParse;
            /// <summary>
            /// 可空类型的泛型参数类型
            /// </summary>
            private static readonly Type nullType;
            /// <summary>
            /// 是否值类型
            /// </summary>
            private static readonly bool isStruct;
            /// <summary>
            /// 成员获取器集合
            /// </summary>
            private static func<jsonNode, object>[] memberGetters;
            /// <summary>
            /// 成员集合
            /// </summary>
            private static setup.memberInfo[] members;
            /// <summary>
            /// 成员值位图字节大小
            /// </summary>
            private static int memberMapSize;
            /// <summary>
            /// 成员数量
            /// </summary>
            private static int memberCount;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            private static memberGroup<valueType> memberGroup;
            static parseJson()
            {
                Type type = typeof(valueType);
                if (type.isStruct())
                {
                    isStruct = true;
                    if ((nullType = type.nullableType()) != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, bool?>)reflectionNodeParser.GetBoolNull;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, DateTime?>)reflectionNodeParser.GetDateTimeNull;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, char?>)reflectionNodeParser.GetCharNull;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, int?>)reflectionNodeParser.GetIntNull;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, uint?>)reflectionNodeParser.GetUIntNull;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, long?>)reflectionNodeParser.GetLongNull;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, ulong?>)reflectionNodeParser.GetULongNull;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, byte?>)reflectionNodeParser.GetByteNull;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, sbyte?>)reflectionNodeParser.GetSByteNull;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, short?>)reflectionNodeParser.GetShortNull;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, ushort?>)reflectionNodeParser.GetUShortNull;
                            return;
                        }
                        MethodInfo tryParseMethodNull = nullType.getTryParse();
                        if (tryParseMethodNull != null)
                        {
                            getter = (func<jsonNode, valueType>)Delegate.CreateDelegate(typeof(func<jsonNode, valueType>), typeof(reflectionNodeParser).GetMethod("getTryParseNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                            return;
                        }
                        getter = (func<jsonNode, valueType>)Delegate.CreateDelegate(typeof(func<jsonNode, valueType>), typeof(reflectionNodeParser).GetMethod("getNullType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        return;
                    }
                    if (type == typeof(bool))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, bool>)reflectionNodeParser.GetBool;
                        return;
                    }
                    if (type == typeof(DateTime))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, DateTime>)reflectionNodeParser.GetDateTime;
                        return;
                    }
                    if (type == typeof(char))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, char>)reflectionNodeParser.GetChar;
                        return;
                    }
                    MethodInfo tryParseMethod = type.getTryParse();
                    if (tryParseMethod != null) tryParse = (tryParse<valueType>)Delegate.CreateDelegate(typeof(tryParse<valueType>), tryParseMethod);
                    if (type == typeof(int))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, int>)reflectionNodeParser.GetInt;
                        return;
                    }
                    if (type == typeof(uint))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, uint>)reflectionNodeParser.GetUInt;
                        return;
                    }
                    if (type == typeof(long))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, long>)reflectionNodeParser.GetLong;
                        return;
                    }
                    if (type == typeof(ulong))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, ulong>)reflectionNodeParser.GetULong;
                        return;
                    }
                    if (type == typeof(byte))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, byte>)reflectionNodeParser.GetByte;
                        return;
                    }
                    if (type == typeof(sbyte))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, sbyte>)reflectionNodeParser.GetSByte;
                        return;
                    }
                    if (type == typeof(short))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, short>)reflectionNodeParser.GetShort;
                        return;
                    }
                    if (type == typeof(ushort))
                    {
                        getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, ushort>)reflectionNodeParser.GetUShort;
                        return;
                    }
                    if (tryParseMethod != null)
                    {
                        getter = GetTryParse;
                        return;
                    }
                    setMemberGroup();
                    return;
                }
                if (type.IsInterface || type.IsAbstract)
                {
                    getter = getDefault;
                    return;
                }
                memberType memberType = type;
                memberType enumerableArgumentType = memberType.EnumerableArgumentType;
                if (enumerableArgumentType != null)
                {
                    EnumerableConstructor = memberType.EnumerableConstructor;
                    if (EnumerableConstructor == null)
                    {
                        getter = (func<jsonNode, valueType>)Delegate.CreateDelegate(typeof(func<jsonNode, valueType>), typeof(reflectionNodeParser).GetMethod("getArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                        return;
                    }
                    getter = (func<jsonNode, valueType>)Delegate.CreateDelegate(typeof(func<jsonNode, valueType>), typeof(reflectionNodeParser).GetMethod("getEnumerableConstructor", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type, enumerableArgumentType));
                    return;
                }
                if (memberType.Type.IsEnum)
                {
                    getter = getEnum;
                    return;
                }
                if (memberType.IsString)
                {
                    getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, string>)reflectionNodeParser.GetString;
                    return;
                }
                if (type == typeof(object))
                {
                    getter = (func<jsonNode, valueType>)(Delegate)(func<jsonNode, object>)reflectionNodeParser.ToObject;
                    return;
                }
                setMemberGroup();
            }
        }
        /// <summary>
        /// 对象是否需要转换成JSON字符串
        /// </summary>
        public bool IsToJson;
        /// <summary>
        /// 是否需要从JSON字符串转换成对象
        /// </summary>
        public bool IsParseJson;
        /// <summary>
        /// 是否转换所有成员当,false表示字段需要显示指定IsParseJson=true
        /// </summary>
        public bool IsParseAll;
        /// <summary>
        /// 是否使用名称解析子节点,否则与父节点公用解析节点[用于成员描述]
        /// </summary>
        public bool IsParseNode = true;
        /// <summary>
        /// 生成部分代码[showjim.setup]
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="partName">部分名称</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="members">成员集合</param>
        /// <param name="attribute">自定义属性</param>
        /// <param name="isMemberMap">是否生成成员位图</param>
        /// <returns>部分代码</returns>
        public static string PartCode(Assembly assembly, string partName, string typeName, setup.cSharp.memberInfo[] members, ajax attribute, bool isMemberMap)
        {
            return cSharp.PartCode(assembly, partName, typeName, members, attribute, isMemberMap);
        }

        /// <summary>
        /// AJAX代码生成
        /// </summary
        [auto(Name = "AJAX", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : member<ajax>
        {
            /// <summary>
            /// 未知的转换JSON类型
            /// </summary>
            private readonly static HashSet<Type> unknownToJsonTypes = new HashSet<Type>();
            /// <summary>
            /// 未知的JSON转换类型
            /// </summary>
            private readonly static HashSet<Type> unknownParseJsonTypes = new HashSet<Type>();
            /// <summary>
            /// 是否转换所有成员
            /// </summary>
            public bool IsParseJson
            {
                get
                {
                    return Members[_loopIndex_].CanSet && (Attribute.IsParseAll || Members[_loopIndex_].ToJsonAttribute.IsParseJson);
                }
            }
            /// <summary>
            /// 是否添加历史对象
            /// </summary>
            public bool IsPushParent
            {
                get
                {
                    return Members.any(member => member.MemberType.IsToJsonPushParent);
                }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                if (Attribute.IsParseJson && !isConstructor()) Attribute.IsParseJson = false;
                Members = getMembers();
                if (Attribute.IsToJson) memberMap.create(type);
                if (Attribute.IsParseJson && !type.Type.isNull()) copy.create(type);
                create(true);
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                if (unknownToJsonTypes.Count != 0)
                {
                    error.Message("没有生成ToJson代码 : " + unknownToJsonTypes.joinString(',', value => value.fullName()));
                    unknownToJsonTypes.Clear();
                }
                if (unknownParseJsonTypes.Count != 0)
                {
                    error.Message("没有生成ParseJson代码 : " + unknownParseJsonTypes.joinString(',', value => value.fullName()));
                    unknownParseJsonTypes.Clear();
                }
            }
            /// <summary>
            /// 生成部分代码
            /// </summary>
            /// <param name="assembly">程序集</param>
            /// <param name="partName">部分名称</param>
            /// <param name="typeName">类型名称</param>
            /// <param name="members">成员集合</param>
            /// <param name="attribute">自定义属性</param>
            /// <param name="isMemberMap">是否生成成员位图</param>
            /// <returns>部分代码</returns>
            public static string PartCode(Assembly assembly, string partName, string typeName, memberInfo[] members, ajax attribute, bool isMemberMap)
            {
                cSharp coder = new cSharp();
                coder.assembly = assembly;
                coder.type = new memberType(typeName);
                coder.Attribute = attribute;
                coder.Members = members.getArray(member => member.create());
                coder.create(false);
                string code = coder._partCodes_[partName];
                if (isMemberMap) code += setup.cSharp.memberMap.cSharp.PartCode("BODY", coder.type, coder.Members);
                return code;
            }
            /// <summary>
            /// 未知的转换JSON类型
            /// </summary>
            /// <param name="type">类型</param>
            public static void UnknownToJson(Type type)
            {
                if (type != null && type.FullName != null) unknownToJsonTypes.Add(type);
            }
            /// <summary>
            /// 未知的JSON转换类型
            /// </summary>
            /// <param name="type">类型</param>
            public static void UnknownParseJson(Type type)
            {
                if (type != null && type.FullName != null) unknownParseJsonTypes.Add(type);
            }
        }
    }
}
namespace fastCSharp.setup
{
    /// <summary>
    /// 成员信息
    /// </summary>
    internal partial class memberInfo
    {
        /// <summary>
        /// AJAX代码生成自定义属性
        /// </summary>
        private cSharp.ajax toJsonAttribute;
        /// <summary>
        /// AJAX代码生成自定义属性
        /// </summary>
        public cSharp.ajax ToJsonAttribute
        {
            get
            {
                if (toJsonAttribute == null)
                {
                    toJsonAttribute = GetAttribute<cSharp.ajax>(true, true) ?? cSharp.ajax.ToJsonAttribute;
                }
                return toJsonAttribute;
            }
        }
    }
    /// <summary>
    /// 成员类型
    /// </summary>
    internal partial class memberType
    {
        /// <summary>
        /// 硬编码ToJson类型
        /// </summary>
        private static readonly staticHashSet<hashCode<Type>> toJsonTypes = new staticHashSet<hashCode<Type>>(new hashCode<Type>[]
        {
            typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte), typeof(float),
            typeof(double), typeof(decimal), typeof(DateTime), typeof(bool), typeof(string), typeof(char)
        });
        /// <summary>
        /// 未知类型转换
        /// </summary>
        private bool? isToJsonUnknown;
        /// <summary>
        /// 未知类型转换
        /// </summary>
        public bool IsToJsonUnknown
        {
            get
            {
                if (isToJsonUnknown == null)
                {
                    if (Type.IsEnum || IsToJson || PairKeyType != null || KeyValueType != null || IsEnumerable) isToJsonUnknown = false;
                    else
                    {
                        if (!toJsonTypes.Contains(NotNullType.Type)) cSharp.ajax.cSharp.UnknownToJson(Type.IsArray ? EnumerableArgumentType.Type : Type);
                        isToJsonUnknown = true;
                    }
                }
                return (bool)isToJsonUnknown;

            }
        }
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.ajax.IsToJson
        /// </summary>
        private bool? isToJson;
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.ajax.IsToJson
        /// </summary>
        public bool IsToJson
        {
            get
            {
                if (isToJson == null) isToJson = Type.getGenericInterface(typeof(fastCSharp.setup.cSharp.ajax.IToJson<>)) != null;
                return (bool)isToJson;
            }
        }
        /// <summary>
        /// 是否添加历史对象
        /// </summary>
        private bool? notToJsonPushParent;
        /// <summary>
        /// 是否添加历史对象
        /// </summary>
        public bool NotToJsonPushParent
        {
            get
            {
                if (notToJsonPushParent == null)
                {
                    notToJsonPushParent = fastCSharp.setup.cSharp.ajax.reflectionToJsoner.NotPushParentType.Contains(NotNullType.Type) || Type.IsEnum;
                }
                return (bool)notToJsonPushParent;
            }
        }
        /// <summary>
        /// 是否添加历史对象
        /// </summary>
        public bool IsToJsonPushParent
        {
            get
            {
                if (NotToJsonPushParent) return false;
                if (IsEnumerable && EnumerableArgumentType.NotToJsonPushParent) return false;
                return true;
            }
        }
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.ajax.IParseJson
        /// </summary>
        private bool? isParseJson;
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.ajax.IParseJson
        /// </summary>
        public bool IsParseJson
        {
            get
            {
                if (isParseJson == null) isParseJson = Type.isInterface(typeof(fastCSharp.setup.cSharp.ajax.IParseJson));
                return (bool)isParseJson;
            }
        }
        /// <summary>
        /// 是否可从字符串转JSON
        /// </summary>
        public bool CanParseJsonEnumerable
        {
            get
            {
                return IsString || IsChar || Type.IsEnum || IsTryParse || IsParseJson || IsObject;
            }
        }
        /// <summary>
        /// 是否可从字符串转JSON
        /// </summary>
        private bool? canParseJson;
        /// <summary>
        /// 是否可从字符串转JSON
        /// </summary>
        public bool CanParseJson
        {
            get
            {
                if (canParseJson == null)
                {
                    if (CanParseJsonEnumerable || (IsEnumerable && EnumerableArgumentType.CanParseJsonEnumerable)) canParseJson = true;
                    else
                    {
                        cSharp.ajax.cSharp.UnknownParseJson(Type.IsArray ? EnumerableArgumentType.Type : Type);
                        canParseJson = false;
                    }
                }
                return (bool)canParseJson;
            }
        }
        /// <summary>
        /// 是否拥有静态转换函数
        /// </summary>
        private bool? isTryParseJson;
        /// <summary>
        /// 是否拥有静态转换函数
        /// </summary>
        public bool IsTryParseJson
        {
            get
            {
                if (isTryParseJson == null) isTryParseJson = IsTryParse && !IsBool && !IsDateTime && !IsChar;
                return (bool)isTryParseJson;
            }
        }
        /// <summary>
        /// 整数类型
        /// </summary>
        private static readonly staticHashSet<hashCode<Type>> parseIntTypes = new staticHashSet<hashCode<Type>>(new hashCode<Type>[]
        {
            typeof(long), typeof(ulong), typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte)
        });
        /// <summary>
        /// 是否整数类型
        /// </summary>
        private bool? isTryParseInt;
        /// <summary>
        /// 是否整数类型
        /// </summary>
        public bool IsTryParseInt
        {
            get
            {
                if (isTryParseInt == null) isTryParseInt = parseIntTypes.Contains(NotNullType.Type);
                return (bool)isTryParseInt;
            }
        }
    }
}
