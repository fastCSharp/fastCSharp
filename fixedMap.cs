using System;

namespace fastCSharp
{
    /// <summary>
    /// 指针位图
    /// </summary>
    public unsafe struct fixedMap
    {
        /// <summary>
        /// 非安全访问指针位图(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 位图指针
            /// </summary>
            private byte* map;
            /// <summary>
            /// 非安全访问指针位图
            /// </summary>
            /// <param name="map">位图指针</param>
            public unsafer(byte* map)
            {
                this.map = map;
            }
            /// <summary>
            /// 设置占位
            /// </summary>
            /// <param name="bit">位值</param>
            public void Set(int bit)
            {
                map[bit >> 3] |= (byte)(1 << (int)(bit &= 7));
            }
            /// <summary>
            /// 获取占位状态
            /// </summary>
            /// <param name="bit">位值</param>
            /// <returns>是否已占位</returns>
            public bool Get(int bit)
            {
                return (map[bit >> 3] & (byte)(1 << (int)(bit &= 7))) != 0;
            }
            /// <summary>
            /// 设置占位段
            /// </summary>
            /// <param name="start">位值,大于等于0</param>
            /// <param name="count">段长,大于等于0</param>
            public void Set(int start, int count)
            {
                fastCSharp.unsafer.memory.FillBits(map, start, count);
            }
            /// <summary>
            /// 清除占位段
            /// </summary>
            /// <param name="start">位值,大于等于0</param>
            /// <param name="count">段长,大于等于0</param>
            public void Clear(int start, int count)
            {
                fastCSharp.unsafer.memory.ClearBits(map, start, count);
            }
        }
        /// <summary>
        /// 非安全访问指针位图
        /// </summary>
        public unsafer Unsafer
        {
            get { return new unsafer(map); }
        }
        /// <summary>
        /// 位图指针
        /// </summary>
        private byte* map;
        /// <summary>
        /// 位图指针
        /// </summary>
        public byte* Map
        {
            get { return map; }
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        public fixedMap(void* map)
        {
            this.map = (byte*)map;
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        public fixedMap(pointer map)
        {
            this.map = map.Byte;
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        /// <param name="length">字节数,大于等于0</param>
        /// <param name="value">初始值</param>
        public fixedMap(void* map, int length, byte value = 0)
        {
            this.map = (byte*)map;
            memory.Fill(map, value, length);
        }

        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        public void Set(int bit)
        {
            map[bit >> 3] |= (byte)(1 << (bit & 7));
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        public bool Get(int bit)
        {
            return (map[bit >> 3] & (1 << (bit & 7))) != 0;
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        public bool Get(char bit)
        {
            return (map[bit >> 3] & (1 << (bit & 7))) != 0;
        }
        /// <summary>
        /// 设置占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        public void Set(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if (count > 0) fastCSharp.unsafer.memory.FillBits(map, start, count);
        }
        /// <summary>
        /// 清除占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        public void Clear(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if (count > 0) fastCSharp.unsafer.memory.ClearBits(map, start, count);

        }
    }
    /// <summary>
    /// 指针位图
    /// </summary>
    /// <typeparam name="enumType">枚举类型</typeparam>
    public unsafe struct fixedMap<enumType> where enumType : IConvertible
    {
        /// <summary>
        /// 非安全访问指针位图
        /// </summary>
        public fixedMap.unsafer Unsafer
        {
            get { return new fixedMap.unsafer(map); }
        }
        /// <summary>
        /// 位图指针
        /// </summary>
        private byte* map;
        /// <summary>
        /// 位图指针
        /// </summary>
        public byte* Map
        {
            get { return map; }
        }
        /// <summary>
        /// 最大值
        /// </summary>
        private uint size;
        ///// <summary>
        ///// 指针位图(非托管内存，必须自行释放)
        ///// </summary>
        //public fixedMap()
        //{
        //    size = (uint)fastCSharp.Enum.GetMaxValue<enumType>(-1) + 1;
        //    map = size != 0 ? unmanaged.Get(((int)size + 7) >> 3, true) : null;
        //}
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">枚举位图</param>
        /// <param name="size">最大值</param>
        public fixedMap(byte* map, uint size)
        {
            if (map == null) log.Default.Throw(log.exceptionType.Null);
            this.map = map;
            this.size = size;
        }

        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        public void Set(int bit)
        {
            if ((uint)bit < size) map[bit >> 3] |= (byte)(1 << (int)(bit &= 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="value">位值</param>
        public void Set(enumType value)
        {
            Set(value.ToInt32(null));
        }
        /// <summary>
        /// 清除占位
        /// </summary>
        /// <param name="bit">位值</param>
        public void Clear(int bit)
        {
            if ((uint)bit < size) map[bit >> 3] &= (byte)(0xff - (1 << (int)(bit &= 7)));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="value">位值</param>
        public void Clear(enumType value)
        {
            Clear(value.ToInt32(null));
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        public bool Get(int bit)
        {
            return (uint)bit < size && (map[bit >> 3] & (byte)(1 << (int)(bit &= 7))) != 0;
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        public bool Get(enumType value)
        {
            return Get(value.ToInt32(null));
        }
    }
}
