using System;

namespace fastCSharp
{
    /// <summary>
    /// 指针(因为指针无法静态初始化)
    /// </summary>
    public unsafe struct pointer
    {
        /// <summary>
        /// 指针
        /// </summary>
        public void* Data;
        /// <summary>
        /// 字节指针
        /// </summary>
        public byte* Byte
        {
            get { return (byte*)Data; }
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public sbyte* SByte
        {
            get { return (sbyte*)Data; }
        }
        /// <summary>
        /// 字符指针
        /// </summary>
        public char* Char
        {
            get { return (char*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public short* Short
        {
            get { return (short*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ushort* UShort
        {
            get { return (ushort*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public int* Int
        {
            get { return (int*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public uint* UInt
        {
            get { return (uint*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public long* Long
        {
            get { return (long*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ulong* ULong
        {
            get { return (ulong*)Data; }
        }
        /// <summary>
        /// 浮点指针
        /// </summary>
        public float* Float
        {
            get { return (float*)Data; }
        }
        /// <summary>
        /// 双精度浮点指针
        /// </summary>
        public double* Double
        {
            get { return (double*)Data; }
        }
        /// <summary>
        /// 日期指针
        /// </summary>
        public DateTime* DateTime
        {
            get { return (DateTime*)Data; }
        }
    }
}
