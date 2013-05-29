using System;

namespace fastCSharp
{
    /// <summary>
    /// 字符子串
    /// </summary>
    public unsafe struct subString
    {
        /// <summary>
        /// Trim删除字符位图
        /// </summary>
        private static readonly pointer trimMap = new String.asciiMap(unmanaged.Get(String.asciiMap.mapBytes), " \t\r", true).Pointer;
        /// <summary>
        /// 原字符串
        /// </summary>
        private string value;
        /// <summary>
        /// 原字符串
        /// </summary>
        public string Value
        {
            get { return value; }
        }
        /// <summary>
        /// 原字符串中的起始位置
        /// </summary>
        private int startIndex;
        /// <summary>
        /// 原字符串中的起始位置
        /// </summary>
        internal int StartIndex
        {
            get { return startIndex; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        private int length;
        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return length; }
        }
        /// <summary>
        /// 获取字符
        /// </summary>
        /// <param name="index">字符位置</param>
        /// <returns>字符</returns>
        public char this[int index]
        {
            get
            {
                return value[index + startIndex];
            }
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="value">字符串</param>
        public subString(string value)
        {
            this.value = value;
            startIndex = 0;
            length = value.length();
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="startIndex">起始位置</param>
        public subString(string value, int startIndex)
        {
            if (value == null) fastCSharp.log.Default.Throw(log.exceptionType.Null);
            length = value.Length - (this.startIndex = startIndex);
            if (length < 0 || startIndex < 0) fastCSharp.log.Default.Throw(log.exceptionType.IndexOutOfRange);
            this.value = length != 0 ? value : string.Empty;
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        public subString(string value, int startIndex, int length)
        {
            if (value == null) fastCSharp.log.Default.Throw(log.exceptionType.Null);
            array.range range = new array.range(value.Length, startIndex, length);
            if (range.GetCount != length) fastCSharp.log.Default.Throw(log.exceptionType.IndexOutOfRange);
            if (range.GetCount != 0)
            {
                this.value = value;
                this.startIndex = range.SkipCount;
                this.length = range.GetCount;
            }
            else
            {
                this.value = string.Empty;
                this.startIndex = this.length = 0;
            }
        }
        /// <summary>
        /// 字符串隐式转换为子串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符子串</returns>
        public static implicit operator subString(string value) { return new subString(value); }
        /// <summary>
        /// 字符子串隐式转换为字符串
        /// </summary>
        /// <param name="value">字符子串</param>
        /// <returns>字符串</returns>
        public static implicit operator string(subString value) { return value.ToString(); }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败返回-1</returns>
        public unsafe int IndexOf(char value)
        {
            if (length != 0)
            {
                fixed (char* valueFixed = this.value)
                {
                    char* start = valueFixed + startIndex, find = unsafer.String.Find(start, start + length, value);
                    if (find != null) return (int)(find - start);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <returns>子串</returns>
        public subString Substring(int startIndex)
        {
            return new subString(value, this.startIndex + startIndex, length - startIndex);
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>子串</returns>
        public subString Substring(int startIndex, int length)
        {
            return new subString(value, this.startIndex + startIndex, length);
        }
        /// <summary>
        /// 删除前后空格
        /// </summary>
        /// <returns>删除前后空格</returns>
        public unsafe subString Trim()
        {
            if (length != 0)
            {
                fixed (char* valueFixed = this.value)
                {
                    char* start = valueFixed + startIndex, end = start + length;
                    start = unsafer.String.findNotAscii(start, end, trimMap.Byte);
                    if (start == null) return new subString(string.Empty);
                    end = unsafer.String.findLastNotAscii(start, end, trimMap.Byte);
                    if (end == null) return new subString(string.Empty);
                    return new subString(value, (int)(start - valueFixed), (int)(end - start));
                }
            }
            return this;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        public list<subString> Split(char split)
        {
            return String.split(value, startIndex, length, split);
        }
        /// <summary>
        /// 是否以字符串开始
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>是否以字符串开始</returns>
        public unsafe bool StartsWith(string value)
        {
            if (value == null) fastCSharp.log.Default.Throw(log.exceptionType.Null);
            if (length >= value.Length)
            {
                fixed (char* valueFixed = this.value, cmpFixed = value)
                {
                    return unsafer.memory.Equal(valueFixed + startIndex, cmpFixed, value.Length << 1);
                }
            }
            return false;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public unsafe override string ToString()
        {
            if (value != null)
            {
                fixed (char* valueFixed = value) return new string(valueFixed, startIndex, length);
            }
            return null;
        }
    }
}
