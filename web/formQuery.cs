using System;

namespace fastCSharp.web
{
    /// <summary>
    /// Get或Post查询字符串相关操作
    /// </summary>
    public static class formQuery
    {
        /// <summary>
        /// 字符串数字转逻辑值
        /// </summary>
        /// <param name="stringValue">字符串数字</param>
        /// <param name="value">转换成功的目标逻辑值</param>
        /// <returns>是否转换成功</returns>
        public static bool TryParse(string stringValue, out bool value)
        {
            bool isParse = false;
            if (stringValue != null && stringValue.Length != 0)
            {
                byte parseValue;
                if (byte.TryParse(stringValue, out parseValue))
                {
                    value = parseValue != 0;
                    isParse = true;
                }
                else if (bool.TryParse(stringValue, out value)) isParse = true;
            }
            else value = false;
            return isParse;
        }
        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="stringValue">字符串</param>
        /// <param name="value">转换成功的目标时间</param>
        /// <returns>是否转换成功</returns>
        public static bool TryParse(string stringValue, out DateTime value)
        {
            bool isParse = DateTime.TryParse(stringValue, out value);
            if (!isParse)
            {
                string escapeValue = JavascriptUnescape(stringValue);
                if (escapeValue != stringValue) return DateTime.TryParse(escapeValue, out value);
            }
            return isParse;
        }
        /// <summary>
        /// 字符串过滤空字符
        /// </summary>
        /// <param name="stringValue">源字符串</param>
        /// <param name="value">过滤空字符后的字符串</param>
        /// <returns>true</returns>
        public static bool TryParse(string stringValue, out string value)
        {
            value = stringValue.replace(ajax.Quote, ' ');
            return true;
        }
        /// <summary>
        /// 字符串转枚举值
        /// </summary>
        /// <typeparam name="enumType">枚举类型</typeparam>
        /// <param name="stringValue">字符串</param>
        /// <param name="value">转换成功的枚举值</param>
        /// <returns>是否转换成功</returns>
        public static bool TryParse<enumType>(string stringValue, out enumType value)
        {
            bool isParse = false;
            object enumValue = null;
            if (stringValue != null && stringValue.Length != 0)
            {
                Type type = typeof(enumType);
                if (type.IsEnum)
                {
                    int intValue;
                    try
                    {
                        enumValue = int.TryParse(stringValue, out intValue) ? System.Enum.ToObject(type, intValue) : System.Enum.Parse(type, stringValue);
                    }
                    catch { }
                }
            }
            if (enumValue != null)
            {
                value = (enumType)enumValue;
                isParse = true;
            }
            else value = default(enumType);
            return isParse;
        }
        /// <summary>
        /// 字符串转枚举整数值
        /// </summary>
        /// <typeparam name="enumType">枚举类型</typeparam>
        /// <param name="stringValue">字符串</param>
        /// <param name="value">转换成功的枚举整数值</param>
        /// <returns>是否转换成功</returns>
        public static bool TryParse<enumType>(string stringValue, out byte value) where enumType : IConvertible
        {
            enumType enumValue;
            if (TryParse(stringValue, out enumValue))
            {
                value = enumValue.ToByte(null);
                return true;
            }
            value = 0;
            return false;
        }
        /// <summary>
        /// 模拟javascript编码函数escape
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <returns>escape编码后的字符串</returns>
        public static unsafe string JavascriptEscape(string value)
        {
            if (value.length() != 0)
            {
                char* newValue = stackalloc char[value.Length << 3];
                byte* write = (byte*)newValue;
                fixed (char* valueFixed = value)
                {
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        uint charValue = *start;
                        if ((charValue & 0xff00) == 0)
                        {
                            if ((uint)(charValue - '0') < 10 || (uint)((charValue | 0x20) - 'a') < 26)
                            {
                                *(char*)write = (char)charValue;
                                write += sizeof(char);
                            }
                            else
                            {
                                *(char*)write = '%';
                                write += sizeof(char);
                                uint code = charValue >> 4;
                                code += code < 10 ? (uint)'0' : (uint)('0' + 'A' - '9' - 1);
                                code += (charValue << 16) & 0xf0000;
                                *(uint*)write = code + (code < 0xa0000 ? (uint)'0' << 16 : ((uint)('0' + 'A' - '9' - 1) << 16));
                                write += sizeof(uint);
                            }
                        }
                        else
                        {
                            *(uint*)write = '%' + ('u' << 16);
                            write += sizeof(uint);
                            uint code = charValue >> 12;
                            code += code < 10 ? (uint)'0' : (uint)('0' + 'A' - '9' - 1);
                            code += (charValue & 0xf00) << 8;
                            *(uint*)write = code + (code < 0xa0000 ? (uint)'0' << 16 : ((uint)('0' + 'A' - '9' - 1) << 16));
                            write += sizeof(uint);
                            code = (charValue >> 4) & 0xf;
                            code += code < 10 ? (uint)'0' : (uint)('0' + 'A' - '9' - 1);
                            code += (charValue << 16) & 0xf0000;
                            *(uint*)write = code + (code < 0xa0000 ? (uint)'0' << 16 : ((uint)('0' + 'A' - '9' - 1) << 16));
                            write += sizeof(uint);
                        }
                    }
                }
                int length = (int)((char*)write - newValue);
                if (length != value.Length) return new string(newValue, 0, length);
            }
            return value;
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <returns>unescape解码后的字符串</returns>
        public static unsafe string JavascriptUnescape(string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value)
                {
                    char* start = valueFixed, end = valueFixed + value.Length;
                    while (start != end && *start != '%')
                    {
                        if (*start == 0) *start = ' ';
                        ++start;
                    }
                    if (start != end)
                    {
                        char* write = start;
                        do
                        {
                            if (*++start == 'u')
                            {
                                uint code = (uint)(*++start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                code <<= 4;
                                code += (uint)(*++start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                code <<= 4;
                                code += (uint)(*++start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                code <<= 4;
                                code += (uint)(*++start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                *write++ = code != 0 ? (char)code : ' ';
                            }
                            else
                            {
                                uint code = (uint)(*start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                code <<= 4;
                                code += (uint)(*++start - '0' < 10 ? *start - '0' : ((*start & 0xdf) - ('0' + 'A' - '9' - 1)));
                                *write++ = code != 0 ? (char)code : ' ';
                            }
                            while (++start < end && *start != '%') *write++ = *start != 0 ? *start : ' ';
                        }
                        while (start < end);
                        return new string(valueFixed, 0, (int)(write - valueFixed));
                    }
                }
            }
            return value;
        }
    }
}
