using System;

namespace fastCSharp.web
{
    /// <summary>
    /// ajax相关操作
    /// </summary>
    public static class ajax
    {
        /// <summary>
        /// ajax生成串替代字符串,默认输入都必须过滤
        /// </summary>
        public const char Quote = pub.NullChar;
        /// <summary>
        /// ajax时间前缀
        /// </summary>
        public static readonly string DateStart = "new Date(";
        /// <summary>
        /// ajax时间后缀
        /// </summary>
        public const char DateEnd = ')';
        /// <summary>
        /// ajax数组字符串
        /// </summary>
        public static readonly string Array = "[]";
        /// <summary>
        /// ajax空对象
        /// </summary>
        public static readonly string Object = "{}";
        /// <summary>
        /// ajax空值
        /// </summary>
        public static readonly string Null = "null";
        /// <summary>
        /// 客户端格式化ajax串的函数
        /// </summary>
        public static readonly string FormatAjax = ".FormatAjax()";
        /// <summary>
        /// 客户端格式化视图ajax串的函数
        /// </summary>
        public static readonly string FormatView = ".FormatView()";
        /// <summary>
        /// 十进制前缀"--0x"
        /// </summary>
        private static readonly ulong hexPrefix;
        /// <summary>
        /// 格式化ajax字符串
        /// </summary>
        /// <param name="jsString">ajax字符串</param>
        /// <returns>格式化后ajax字符串</returns>
        public unsafe static string formatJavascript(this string jsString)
        {
            int length = jsString.length();
            if (length != 0)
            {
                if (length > (config.pub.Default.StreamBufferLength >> 1))
                {
                    char[] ajaxChars = new char[jsString.Length << 1];
                    fixed (char* formatFixed = ajaxChars) return FormatJavascript(jsString, formatFixed);
                }
                else
                {
                    char* formatFixed = stackalloc char[jsString.Length << 1];
                    return FormatJavascript(jsString, formatFixed);
                }
            }
            return jsString;
        }
        /// <summary>
        /// 格式化ajax字符串
        /// </summary>
        /// <param name="jsString">ajax字符串</param>
        /// <param name="formatFixed">格式化ajax字符串</param>
        /// <returns>格式化后ajax字符串</returns>
        private unsafe static string FormatJavascript(string jsString, char* formatFixed)
        {
            fixed (char* jsFixed = jsString)
            {
                bool isQuote = false;
                char* writeChar = formatFixed;
                for (char* nextChar = jsFixed, endChar = jsFixed + jsString.Length; nextChar != endChar; ++nextChar)
                {
                    if (*nextChar == Quote)
                    {
                        isQuote = true;
                        *writeChar++ = '"';
                    }
                    else if (*nextChar == '\n')
                    {
                        *(int*)writeChar = ('n' << 16) + '\\';
                        writeChar += 2;
                    }
                    else if (*nextChar == '\r')
                    {
                        *(int*)writeChar = ('r' << 16) + '\\';
                        writeChar += 2;
                    }
                    else
                    {
                        if (*nextChar == '"' || *nextChar == '\\') *writeChar++ = '\\';
                        *writeChar++ = *nextChar;
                    }
                }
                int length = (int)(writeChar - formatFixed);
                return isQuote || length != jsString.Length ? new string(formatFixed, 0, length) : jsString;
            }
        }
        /// <summary>
        /// 格式化ajax字符串(无换行)
        /// </summary>
        /// <param name="jsString">ajax字符串</param>
        /// <returns>格式化后ajax字符串</returns>
        public unsafe static string formatJavascriptOnline(this string jsString)
        {
            int length = jsString.length();
            if (length != 0)
            {
                if (length > (config.pub.Default.StreamBufferLength >> 1))
                {
                    char[] ajaxChars = new char[length << 1];
                    fixed (char* formatFixed = ajaxChars) return FormatJavascriptOnline(jsString, formatFixed);
                }
                else
                {
                    char* formatFixed = stackalloc char[length << 1];
                    return FormatJavascriptOnline(jsString, formatFixed);
                }
            }
            return jsString;
        }
        /// <summary>
        /// 格式化ajax字符串(无换行)
        /// </summary>
        /// <param name="jsString">ajax字符串</param>
        /// <param name="formatFixed">格式化ajax字符串</param>
        /// <returns>格式化后ajax字符串</returns>
        private unsafe static string FormatJavascriptOnline(string jsString, char* formatFixed)
        {
            fixed (char* jsFixed = jsString)
            {
                bool isQuote = false;
                char* writeChar = formatFixed;
                for (char* nextChar = jsFixed, endChar = jsFixed + jsString.Length; nextChar != endChar; nextChar++)
                {
                    if (*nextChar == Quote)
                    {
                        isQuote = true;
                        *writeChar++ = '"';
                    }
                    else
                    {
                        if (*nextChar == '"' || *nextChar == '\\') *writeChar++ = '\\';
                        *writeChar++ = *nextChar;
                    }
                }
                int length = (int)(writeChar - formatFixed);
                return isQuote || length != jsString.Length ? new string(formatFixed, 0, length) : jsString;
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this long value)
        {
            int startIndex = 1;
            char* chars = stackalloc char[20];
            if (value < 0)
            {
                value = -value;
                startIndex = 0;
            }
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
                *--nextChar = '-';
                return new string(nextChar += startIndex, 0, 5 - (int)(nextChar - chars));
            }
            else
            {
                char* nextChar = chars + 19;
                long nextValue = value & 15;
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
                *(ulong*)(nextChar -= 4) = hexPrefix;
                return new string(nextChar += ++startIndex, 0, 20 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this ulong value)
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
                return new string(nextChar, 0, 5 - (int)(nextChar - chars));
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
                *(uint*)(nextChar -= 2) = (uint)(hexPrefix >> 32);
                return new string(nextChar, 0, 20 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this int value)
        {
            int startIndex = 1;
            char* chars = stackalloc char[12];
            if (value < 0)
            {
                value = -value;
                startIndex = 0;
            }
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
                *--nextChar = '-';
                return new string(nextChar += startIndex, 0, 5 - (int)(nextChar - chars));
            }
            else
            {
                char* nextChar = chars + 11;
                int nextValue = value & 15;
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
                *(ulong*)(nextChar -= 4) = hexPrefix;
                return new string(nextChar += ++startIndex, 0, 12 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this uint value)
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
                return new string(nextChar, 0, 5 - (int)(nextChar - chars));
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
                *(uint*)(nextChar -= 2) = (uint)(hexPrefix >> 32);
                return new string(nextChar, 0, 12 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this short value16)
        {
            int startIndex = 1, value = value16;
            char* chars = stackalloc char[8];
            if (value < 0)
            {
                value = -value;
                startIndex = 0;
            }
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
                *--nextChar = '-';
                return new string(nextChar += startIndex, 0, 5 - (int)(nextChar - chars));
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
                *--nextChar = (char)((value >> 4) + '0');
                *(ulong*)(nextChar -= 4) = hexPrefix;
                return new string(nextChar += ++startIndex, 0, 8 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this ushort value16)
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
                return new string(nextChar, 0, 5 - (int)(nextChar - chars));
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
                *(uint*)(nextChar -= 2) = (uint)(hexPrefix >> 32);
                return new string(nextChar, 0, 8 - (int)(nextChar - chars));
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this sbyte value)
        {
            string hex = value == 0 ? "0" : null;
            if (value != 0)
            {
                char* chars = stackalloc char[6];
                *(ulong*)chars = hexPrefix;
                *(chars + 4) = (char)((value >> 4) + '0');
                *(chars + 5) = (char)((value &= 15) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
                if (value < 0) hex = new string(chars, 1, 5);
                else hex = new string(chars, 2, 4);
            }
            return hex;
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public unsafe static string toString(this byte value)
        {
            string hex = value == 0 ? "0" : "0x00";
            if (value != 0)
            {
                fixed (char* hexFixed = hex)
                {
                    int nextValue = value >> 4;
                    *(hexFixed + 2) = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                    *(hexFixed + 3) = (char)((value &= 15) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
                }
            }
            return hex;
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>数字字符串</returns>
        public static string toString(this bool value)
        {
            return value ? "1" : "0";
        }
        /// <summary>
        /// 单精度浮点数转字符串
        /// </summary>
        /// <param name="value">单精度浮点数</param>
        /// <returns>字符串</returns>
        public static string toString(this float value)
        {
            return value.ToString();
        }
        /// <summary>
        /// 双精度浮点数转字符串
        /// </summary>
        /// <param name="value">双精度浮点数</param>
        /// <returns>字符串</returns>
        public static string toString(this double value)
        {
            return value.ToString();
        }
        /// <summary>
        /// 十进制数转字符串
        /// </summary>
        /// <param name="value">十进制数</param>
        /// <returns>字符串</returns>
        public static string toString(this decimal value)
        {
            return value.ToString();
        }
        /// <summary>
        ///  Json转换时间差
        /// </summary>
        public static readonly long JavascriptMinTimeTicks = config.web.Default.JavascriptMinTime.Ticks;
        /// <summary>
        /// Json转换时间差毫秒数
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>Json转换时间差毫秒数</returns>
        public static ulong toJavascriptMilliseconds(this DateTime time)
        {
            return (ulong)((time.Ticks - JavascriptMinTimeTicks) / date.MillisecondTicks);
        }

        unsafe static ajax()
        {
            string hex = "--0x";
            fixed (char* hexFixed = hex) hexPrefix = *(ulong*)hexFixed;
        }
    }
}
