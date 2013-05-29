using System;

namespace fastCSharp
{
    /// <summary>
    /// 位图
    /// </summary>
    public struct bitMap
    {
        /// <summary>
        /// 非安全访问位图(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 位图
            /// </summary>
            private byte[] map;
            /// <summary>
            /// 位图字节数组
            /// </summary>
            public byte[] Map
            {
                get { return map; }
            }
            /// <summary>
            /// 非安全访问位图
            /// </summary>
            /// <param name="map">位图</param>
            public unsafer(bitMap map)
            {
                this.map = map.map;
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
            /// 清除占位
            /// </summary>
            /// <param name="bit">位值</param>
            public void Clear(int bit)
            {
                map[bit >> 3] &= (byte)(0xff - (1 << (int)(bit &= 7)));
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
            /// <param name="start">位值</param>
            /// <param name="count">段长</param>
            public unsafe void Set(int start, int count)
            {
                fastCSharp.unsafer.memory.FillBits(map, start, count);
            }
            /// <summary>
            /// 清除占位段
            /// </summary>
            /// <param name="start">位值</param>
            /// <param name="count">段长</param>
            public unsafe void Clear(int start, int count)
            {
                fastCSharp.unsafer.memory.ClearBits(map, start, count);
            }
        }
        /// <summary>
        /// 非安全访问位图
        /// </summary>
        public unsafer Unsafer
        {
            get
            {
                return new unsafer(this);
            }
        }
        /// <summary>
        /// 位图字节数组
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 最大值
        /// </summary>
        private uint size;
        /// <summary>
        /// 位图
        /// </summary>
        /// <param name="size">位图尺寸</param>
        public bitMap(int size)
        {
            this.size = size <= 0 ? 0 : ((uint)size + 7) >> 3;
            map = this.size > 0 ? new byte[this.size] : nullValue<byte>.Array;
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
        /// 清除占位
        /// </summary>
        /// <param name="bit">位值</param>
        public void Clear(int bit)
        {
            if ((uint)bit < size) map[bit >> 3] &= (byte)(0xff - (1 << (int)(bit &= 7)));
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
        /// 设置占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        public unsafe void Set(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if ((uint)start < size && count > 0)
            {
                if ((uint)start + count > size) count = (int)size - start;
                fastCSharp.unsafer.memory.FillBits(map, start, count);
            }
        }
        /// <summary>
        /// 清除占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        public unsafe void Clear(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if ((uint)start < size && count > 0)
            {
                if ((uint)start + count > size) count = (int)size - start;
                fastCSharp.unsafer.memory.ClearBits(map, start, count);
            }
        }
    }
    /// <summary>
    /// 枚举位图
    /// </summary>
    /// <typeparam name="enumType">枚举类型</typeparam>
    public struct bitMap<enumType> where enumType : IConvertible
    {
        /// <summary>
        /// 非安全访问枚举位图(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 枚举位图
            /// </summary>
            private bitMap<enumType> map;
            /// <summary>
            /// 位图字节数组
            /// </summary>
            public byte[] Map
            {
                get { return map.map; }
            }
            /// <summary>
            /// 非安全访问枚举位图
            /// </summary>
            /// <param name="map">枚举位图</param>
            public unsafer(bitMap<enumType> map)
            {
                this.map = map;
            }
            /// <summary>
            /// 设置占位
            /// </summary>
            /// <param name="bit">位值</param>
            public void Set(int bit)
            {
                map.map[bit >> 3] |= (byte)(1 << (int)(bit &= 7));
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
                map.map[bit >> 3] &= (byte)(0xff - (1 << (int)(bit &= 7)));
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
                return (map.map[bit >> 3] & (byte)(1 << (int)(bit &= 7))) != 0;
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
        /// <summary>
        /// 非安全访问枚举位图
        /// </summary>
        public unsafer Unsafer
        {
            get { return new unsafer(this); }
        }
        /// <summary>
        /// 位图字节数组
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 最大值
        /// </summary>
        private uint size;
        /// <summary>
        /// 枚举位图
        /// </summary>
        /// <param name="map">位图字节数组</param>
        public bitMap(byte[] map)
        {
            size = (uint)fastCSharp.Enum.GetMaxValue<enumType>(-1) + 1;
            this.map = map ?? new byte[(size + 7) >> 3];
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
