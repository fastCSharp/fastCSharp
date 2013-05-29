using System;
using System.IO;

namespace fastCSharp
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    public class charStream : IDisposable
    {
        /// <summary>
        /// 内存字符流(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 内存字符流
            /// </summary>
            public charStream Stream;
            /// <summary>
            /// 增加数据流长度
            /// </summary>
            /// <param name="length">增加长度</param>
            public void AddLength(int length)
            {
                Stream.Length += length;
            }
            /// <summary>
            /// 设置数据流长度
            /// </summary>
            /// <param name="length">数据流长度</param>
            public void SetLength(int length)
            {
                Stream.Length = length;
            }
            /// <summary>
            /// 写数据
            /// </summary>
            /// <param name="value">数据</param>
            public void Write(char value)
            {
                Stream.array[Stream.Length++] = value;
            }
            /// <summary>
            /// 写数据
            /// </summary>
            /// <param name="stream">数据,不能为null</param>
            public void Write(charStream stream)
            {
                Buffer.BlockCopy(stream.array, 0, Stream.array, Stream.Length << 1, stream.Length << 1);
                Stream.Length += stream.Length;
            }
        }
        /// <summary>
        /// 非安全访问内存字符流
        /// </summary>
        /// <returns>非安全访问内存字符流</returns>
        public unsafer Unsafer
        {
            get
            {
                return new unsafer { Stream = this };
            }
        }
        /// <summary>
        /// 默认容器初始尺寸
        /// </summary>
        public const int DefaultLength = 256;
        /// <summary>
        /// 数据
        /// </summary>
        private char[] array;
        /// <summary>
        /// 数据
        /// </summary>
        public char[] Array
        {
            get { return array; }
        }
        /// <summary>
        /// 当前数据长度
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// 内存数据流
        /// </summary>
        public charStream() : this(DefaultLength) { }
        /// <summary>
        /// 内存数据流
        /// </summary>
        /// <param name="length">容器初始尺寸</param>
        public charStream(int length)
        {
            array = new char[length > 0 ? length : DefaultLength];
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public void Close()
        {
            array = null;
        }
        /// <summary>
        /// 设置容器尺寸
        /// </summary>
        /// <param name="length">容器尺寸</param>
        private void setArrayLength(int length)
        {
            char[] data = new char[length];
            Buffer.BlockCopy(array, 0, data, 0, Length << 1);
            array = data;
        }
        /// <summary>
        /// 设置容器尺寸
        /// </summary>
        /// <param name="length">容器尺寸</param>
        public void SetArrayLength(int length)
        {
            if ((length += Length) > array.Length) setArrayLength(length);
        }
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        /// <returns>是否需要增加容器尺寸</returns>
        public bool PrepLength(int length)
        {
            int newLength = length + Length;
            if (newLength > array.Length)
            {
                setArrayLength(length > Length ? newLength : (Length << 1));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        private void prepLength(int length)
        {
            int newLength = length + Length;
            if (newLength > array.Length) setArrayLength(length > Length ? newLength : (Length << 1));
        }
        /// <summary>
        /// 检测是否需要预增数据容器尺寸
        /// </summary>
        /// <param name="length">增加数据流长度</param>
        /// <returns>是否需要预增数据容器尺寸</returns>
        public bool Check(int length)
        {
            return Length + length <= array.Length;
        }
        /// <summary>
        /// 重置当前数据长度
        /// </summary>
        /// <param name="length">当前数据长度</param>
        public void SetLength(int length)
        {
            if (length > 0)
            {
                if (length > array.Length) setArrayLength(length);
                Length = length;
            }
            else if (length == 0) Length = 0;
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Write(char value)
        {
            if (Length == array.Length) setArrayLength(Length << 1);
            array[Length++] = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="stream">数据</param>
        public void Write(charStream stream)
        {
            if (stream != null)
            {
                prepLength(stream.Length);
                Buffer.BlockCopy(stream.array, 0, array, Length << 1, stream.Length << 1);
                Length += stream.Length;
            }
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        public unsafe void Write(string value)
        {
            if (value != null)
            {
                prepLength(value.Length);
                fixed (char* dataFixed = array) fastCSharp.unsafer.String.Copy(value, dataFixed + Length);
                Length += value.Length;
            }
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="start">字符串起始位置</param>
        /// <param name="count">写入字符数</param>
        public unsafe void Write(char* start, int count)
        {
            if (start != null)
            {
                prepLength(count);
                fixed (char* dataFixed = array) memory.Copy(start, dataFixed + Length, count);
                Length += count;
            }
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">写入字符数</param>
        public unsafe void Write(string value, int index, int count)
        {
            array.range range = new array.range(value.length(), index, count);
            if (range.GetCount == count)
            {
                prepLength(count);
                fixed (char* dataFixed = array, valueFixed = value)
                {
                    fastCSharp.unsafer.memory.Copy(valueFixed + index, dataFixed + Length, count);
                }
                Length += count;
            }
            else if (count != 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 写字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        public unsafe void Write(params string[] values)
        {
            if (values != null)
            {
                int length = 0;
                foreach (string value in values)
                {
                    if (value != null) length += value.Length;
                }
                prepLength(length);
                fixed (char* dataFixed = array)
                {
                    char* write = dataFixed + Length;
                    foreach (string value in values)
                    {
                        if (value != null)
                        {
                            fastCSharp.unsafer.String.Copy(value, write);
                            write += value.Length;
                        }
                    }
                }
                Length += length;
            }
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override unsafe string ToString()
        {
            fixed (char* dataFixed = array) return new string(dataFixed, 0, Length);
        }
    }
}
