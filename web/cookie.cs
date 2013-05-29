using System;

namespace fastCSharp.web
{
    /// <summary>
    /// COOKIE处理类
    /// </summary>
    public unsafe static class cookie
    {
        /// <summary>
        /// 需要格式化的cookie名称字符集合
        /// </summary>
        public const string FormatCookieNameChars = ",; -\n\r\t";
        /// <summary>
        /// 需要格式化的cookie名称字符位图
        /// </summary>
        private static readonly String.asciiMap formatCookieNameCharMap = new String.asciiMap(unmanaged.Get(String.asciiMap.mapBytes), FormatCookieNameChars, true);
        /// <summary>
        /// 最大cookie名称长度
        /// </summary>
        public const int MaxCookieNameLength = 256;
        /// <summary>
        /// 格式化Cookie名称
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns>格式化后Cookie名称</returns>
        public unsafe static string FormatCookieName(string name)
        {
            if (name.length() == 0) log.Default.Throw(log.exceptionType.Null);
            if (name.Length > MaxCookieNameLength) log.Default.Throw(null, "cookie名称超过限定 " + ((uint)name.Length).toString(), false);
            fixed (char* nameFixed = name)
            {
                char* endName = nameFixed + name.Length;
                int count = unsafer.String.asciiCount(nameFixed, endName, formatCookieNameCharMap.Map, FormatCookieNameChars[0]);
                if (*nameFixed == '$') ++count;
                if (count != 0)
                {
                    char* nameChars = stackalloc char[count = name.Length + (count << 1)], nextCookieName = nameChars;
                    if (*nameFixed == '$')
                    {
                        *nextCookieName++ = '%';
                        *(uint*)nextCookieName = '2' + ('4' << 16);
                        nextCookieName += 2;
                    }
                    else *nextCookieName = *nameFixed;
                    char* nextName = nameFixed;
                    String.asciiMap formatMap = formatCookieNameCharMap;
                    while (++nextName != endName)
                    {
                        int nextValue = (int)*nextName;
                        if (formatMap.Get(nextValue))
                        {
                            *++nextCookieName = '%';
                            int highValue = nextValue >> 4;
                            *++nextCookieName = (char)(highValue < 10 ? highValue + '0' : (highValue + ('0' + 'A' - '9' - 1)));
                            nextValue &= 15;
                            *++nextCookieName = (char)(nextValue < 10 ? nextValue + '0' : (nextValue + ('0' + 'A' - '9' - 1)));
                        }
                        else *++nextCookieName = (char)nextValue;
                    }
                    return new string(nameChars, 0, count);
                }
            }
            return name;
        }
        /// <summary>
        /// 格式化Cookie值
        /// </summary>
        /// <param name="value">Cookie值</param>
        /// <returns>格式化后的Cookie值</returns>
        public static string FormatCookieValue(string value)
        {
            return value == null ? string.Empty : value.Replace(",", "%2C").Replace(";", "%3B");
        }
    }
}
