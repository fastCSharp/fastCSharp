using System;

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 字符串相关操作(非安全,请自行确保数据可靠性)
    /// </summary>
    public static class String
    {
        /// <summary>
        /// 获取最后一个字符
        /// </summary>
        /// <param name="value">长度大于0</param>
        /// <returns>最后一个字符</returns>
        public static char Last(string value)
        {
            return value[value.Length - 1];
        }
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="destination">目标字符串地址,不能为null</param>
        public unsafe static void Copy(string source, void* destination)
        {
            fixed (char* sourceFixed = source) memory.Copy(sourceFixed, destination, source.Length << 1);
        }
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="destination">目标字符串,不能为null</param>
        /// <param name="destinationIndex">目标字符串位置,大于等于0</param>
        public unsafe static void Copy(string source, string destination, int destinationIndex)
        {
            fixed (char* sourceFixed = source, destinationFixed = destination)
            {
                memory.Copy((void*)sourceFixed, destinationFixed + destinationIndex, source.Length << 1);
            }
        }
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="sourceIndex">原字符串位置,大于等于0</param>
        /// <param name="destination">目标字符串,不能为null</param>
        /// <param name="destinationIndex">目标字符串位置,大于等于0</param>
        /// <param name="count">复制字符数量,大于等于0</param>
        public unsafe static void Copy(string source, int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            fixed (char* sourceFixed = source, destinationFixed = destination)
            {
                memory.Copy((void*)(sourceFixed + sourceIndex), destinationFixed + destinationIndex, count << 1);
            }
        }
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="destination">目标字符串,不能为null</param>
        /// <param name="destinationIndex">目标字符串位置,大于等于0</param>
        public unsafe static void Copy(string source, char[] destination, int destinationIndex)
        {
            fixed (char* sourceFixed = source, destinationFixed = destination)
            {
                memory.Copy((void*)sourceFixed, destinationFixed + destinationIndex, source.Length << 1);
            }
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* Find(char* start, char* end, char value)
        {
            char endValue = *--end;
            for (*end = value; *start != value; ++start) ;
            *end = endValue;
            return start != end || endValue == value ? start : null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <param name="value">一个合法值</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* Find(char* start, char* end, fastCSharp.String.asciiMap valueMap, char value)
        {
            return findAscii(start, end, valueMap.Map, value);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <param name="value">一个合法值</param>
        /// <returns>字符位置,失败为null</returns>
        internal unsafe static char* findAscii(char* start, char* end, byte* valueMap, char value)
        {
            char endValue = *--end;
            for (*end = value; (*start & 0xff80) != 0 || (valueMap[*start >> 3] & (1 << (*start & 7))) == 0; ++start) ;
            *end = endValue;
            if (start != end || ((endValue & 0xff80) == 0 && (valueMap[endValue >> 3] & (1 << (endValue & 7))) != 0)) return start;
            return null;
        }
        /// <summary>
        /// 字符查找数量
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <param name="value">一个合法值</param>
        /// <returns>字符数量</returns>
        public unsafe static int AsciiCount(char* start, char* end, fastCSharp.String.asciiMap valueMap, char value)
        {
            return asciiCount(start, end, valueMap.Map, value);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <param name="value">一个合法值</param>
        /// <returns>字符数量</returns>
        internal unsafe static int asciiCount(char* start, char* end, byte* valueMap, char value)
        {
            int count = 0;
            char endValue = *--end;
            *end = value;
            do
            {
                while ((*start & 0xff80) != 0 || (valueMap[*start >> 3] & (1 << (*start & 7))) == 0) ++start;
                ++count;
            }
            while (start++ != end);
            *end = endValue;
            if ((endValue & 0xff80) != 0 || (valueMap[endValue >> 3] & (1 << (endValue & 7))) == 0) --count;
            return count;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* FindNot(char* start, char* end, fastCSharp.String.asciiMap valueMap)
        {
            return findNotAscii(start, end, valueMap.Map);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <returns>字符位置,失败为null</returns>
        internal unsafe static char* findNotAscii(char* start, char* end, byte* valueMap)
        {
            while (start < end && (*start & 0xff80) == 0 && (valueMap[*start >> 3] & (1 << (*start & 7))) != 0) ++start;
            return start != end ? start : null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* FindLastNot(char* start, char* end, fastCSharp.String.asciiMap valueMap)
        {
            return findLastNotAscii(start, end, valueMap.Map);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="valueMap">字符集合</param>
        /// <returns>字符位置,失败为null</returns>
        internal unsafe static char* findLastNotAscii(char* start, char* end, byte* valueMap)
        {
            while (--end >= start && (*end & 0xff80) == 0 && (valueMap[*end >> 3] & (1 << (*end & 7))) != 0) ;
            return ++end != start ? end : null;
        }
        /// <summary>
        /// 获取字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <returns>原始字节流</returns>
        public unsafe static byte[] Serialize(string value)
        {
            fixed (char* valueFixed = value)
            {
                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                {
                    if ((*start & 0xff00) != 0) return memory.Copy(valueFixed, value.Length << 1);
                }
                return GetBytes(valueFixed, value.Length);
            }
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="length">字符串长度</param>
        /// <returns>字节流</returns>
        internal unsafe static byte[] GetBytes(char* value, int length)
        {
            byte[] data = new byte[length];
            fixed (byte* dataFixed = data)
            {
                byte* write = dataFixed;
                for (char* start = value, end = value + length; start != end; *write++ = (byte)*start++) ;
            }
            return data;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        public unsafe static void ToLower(char* start, char* end)
        {
            while (start != end)
            {
                if ((uint)(*start - 'A') < 26) *start |= (char)0x20;
                ++start;
            }
        }
        /// <summary>
        /// 小写转大写
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        public unsafe static void ToUpper(char* start, char* end)
        {
            while (start != end)
            {
                if ((uint)(*start - 'a') < 26) *start -= (char)0x20;
                ++start;
            }
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        /// <returns>是否相等</returns>
        public static unsafe bool EqualCase(string left, char* right, int count)
        {
            fixed (char* leftFixed = left) return EqualCase(leftFixed, right, count);
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        /// <returns>是否相等</returns>
        public static unsafe bool EqualCase(char* left, char* right, int count)
        {
            for (char* end = left + count; left != end; ++left, ++right)
            {
                if (*left != *right)
                {
                    if ((*left | (char)0x20) != (*right | (char)0x20) || (uint)(*left - 'a') >= 26) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="value">字符串,长度不能为0</param>
        /// <param name="oldChar">原字符</param>
        /// <param name="newChar">目标字符</param>
        /// <returns>字符串</returns>
        public unsafe static string Replace(string value, char oldChar, char newChar)
        {
            fixed (char* valueFixed = value)
            {
                char* start = valueFixed, end = valueFixed + value.Length;
                char endValue = *--end;
                *end = oldChar;
                do
                {
                    while (*start != oldChar) ++start;
                    *start = newChar;
                }
                while (start++ != end);
                if (endValue != oldChar) *end = endValue;
            }
            return value;
        }
    }
}
