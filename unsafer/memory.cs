using System;
#if CPU64
using cpuUint = System.UInt64;
#else
using cpuUint = System.UInt32;
#endif

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 内存或字节数组(非安全,请自行确保数据可靠性)
    /// </summary>
    public static class memory
    {
        /// <summary>
        /// 字节流转16位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>整数值</returns>
        public static short GetShort(byte[] values)
        {
            return (short)(values[0] + (values[1] << 8));
        }
        /// <summary>
        /// 字节流转16位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>整数值</returns>
        public static short GetShort(byte[] values, int startIndex)
        {
            return (short)(values[startIndex] + (values[startIndex + 1] << 8));
        }
        /// <summary>
        /// 字节流转16位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>无符号整数值</returns>
        public static ushort GetUShort(byte[] values)
        {
            return (ushort)(values[0] + (values[1] << 8));
        }
        /// <summary>
        /// 字节流转16位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>无符号整数值</returns>
        public static ushort GetUShort(byte[] values, int startIndex)
        {
            return (ushort)(values[startIndex] + (values[startIndex + 1] << 8));
        }
        /// <summary>
        /// 字节流转32位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>整数值</returns>
        public unsafe static int GetInt(byte[] values)
        {
            fixed (byte* value = values) return *((int*)value);
        }
        /// <summary>
        /// 字节流转32位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>整数值</returns>
        public unsafe static int GetInt(byte[] values, int startIndex)
        {
            fixed (byte* value = values) return *((int*)(value + startIndex));
        }
        /// <summary>
        /// 字节流转32位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>无符号整数值</returns>
        public unsafe static uint GetUInt(byte[] values)
        {
            fixed (byte* value = values) return *((uint*)value);
        }
        /// <summary>
        /// 字节流转32位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>无符号整数值</returns>
        public unsafe static uint GetUInt(byte[] values, int startIndex)
        {
            fixed (byte* value = values) return *((uint*)(value + startIndex));
        }
        /// <summary>
        /// 字节流转64位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>整数值</returns>
        public unsafe static long GetLong(byte[] values)
        {
            fixed (byte* value = values) return *((long*)value);
        }
        /// <summary>
        /// 字节流转64位整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>整数值</returns>
        public unsafe static long GetLong(byte[] values, int startIndex)
        {
            fixed (byte* value = values) return *((long*)(value + startIndex));
        }
        /// <summary>
        /// 字节流转64位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <returns>无符号整数值</returns>
        public unsafe static ulong GetULong(byte[] values)
        {
            fixed (byte* value = values) return *((ulong*)value);
        }
        /// <summary>
        /// 字节流转64位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>无符号整数值</returns>
        public unsafe static ulong GetULong(byte[] values, int startIndex)
        {
            fixed (byte* value = values) return *((ulong*)(value + startIndex));
        }
        /// <summary>
        /// 填充整数
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="value">整数值</param>
        /// <param name="length">整数数量,大于等于0</param>
        public unsafe static void Fill(void* src, uint value, int count)
        {
            byte* data = (byte*)src;
#if CPU64
            if ((count & 1) != 0)
            {
                *(uint*)data = value;
                --count;
                data += 4;
            }
#endif
            int shift = (int)data & (sizeof(cpuUint) - 1);
#if CPU64
            if (shift == 0) fill((byte*)data, (cpuUint)value | ((cpuUint)value << 32), count >> 1);
#else
            if (shift == 0) fill((byte*)data, value, count);
#endif
            else if (count > 0)
            {
#if CPU64
                cpuUint cpuValue = value | (value << 32);
                count >>= 1;
#else
                cpuUint cpuValue = value;
#endif
                *(cpuUint*)data = cpuValue;
                fill(data + (sizeof(cpuUint) - shift), (cpuValue >> (shift <<= 3)) | (cpuValue << (fastCSharp.pub.CpuBits - shift)), --count);
                *((cpuUint*)data + count) = cpuValue;
            }
        }
        /// <summary>
        /// 填充字节
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="value">字节值</param>
        /// <param name="length">字节数量,大于等于0</param>
        public unsafe static void Fill(void* src, byte value, int length)
        {
            byte* data = (byte*)src;
            cpuUint cpuValue = (uint)((value << 8) | value);
            int shift = (int)data & (sizeof(cpuUint) - 1);
#if CPU64
            cpuValue |= cpuValue << 16;
#endif
            if (shift != 0)
            {
                shift = sizeof(cpuUint) - shift;
                if (shift > length) shift = length;
                if ((shift & 1) != 0) *data++ = value;
                if ((shift & 2) != 0)
                {
                    *((short*)data) = (short)cpuValue;
                    data += 2;
                }
#if CPU64
                if ((shift & 4) != 0)
                {
                    *((uint*)data) = (uint)cpuValue;
                    data += 4;
                }
#endif
                length -= shift;
            }
            fill(data, cpuValue | (cpuValue << (fastCSharp.pub.CpuBits / 2)), length >> fastCSharp.pub.Byte2CpuShift);
            data += length & (int.MaxValue - (sizeof(cpuUint) - 1));
#if CPU64
            if ((length & 4) != 0)
            {
                *((uint*)data) = (uint)cpuValue;
                data += 4;
            }
#endif
            if ((length & 2) != 0)
            {
                *((short*)data) = (short)cpuValue;
                data += 2;
            }
            if ((length & 1) != 0) *data = value;
        }
        /// <summary>
        /// 填充整数(用Buffer.BlockCopy可能比指针快)
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="value">整数值</param>
        /// <param name="length">整数数量,大于等于0</param>
        unsafe static void fill(byte* src, cpuUint value, int count)
        {
            for (int index = count >> fastCSharp.pub.Byte2CpuShift; index != 0; --index)
            {
                *((cpuUint*)src) = *((cpuUint*)(src + sizeof(cpuUint)))
                    = *((cpuUint*)(src + sizeof(cpuUint) * 2)) = *((cpuUint*)(src + sizeof(cpuUint) * 3)) = value;
                src += sizeof(cpuUint) * 4;
            }
            if ((count & 2) != 0)
            {
                *((cpuUint*)src) = *((cpuUint*)(src + sizeof(cpuUint))) = value;
                src += sizeof(cpuUint) * 2;
            }
            if ((count & 1) != 0) *((cpuUint*)src) = value;
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原字节数组,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        public unsafe static void Copy(byte[] source, void* destination, int length)
        {
            fixed (byte* data = source) Copy((void*)data, destination, length);
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原字节起始地址,不能为null</param>
        /// <param name="destination">目标串数组,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        public unsafe static void Copy(void* source, byte[] destination, int length)
        {
            fixed (byte* data = destination) Copy(source, (void*)data, length);
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        public unsafe static void Copy(void* source, void* destination, int length)
        {
            int shift = (int)destination & (sizeof(cpuUint) - 1);
            if (shift == 0) copy((byte*)source, (byte*)destination, length);
            else
            {
                shift = sizeof(cpuUint) - shift;
                if (shift > length) shift = length;
                if ((shift & 1) != 0)
                {
                    *(byte*)destination = *(byte*)source;
                    if ((shift & 2) != 0)
                    {
                        *(ushort*)((byte*)destination + 1) = *(ushort*)((byte*)source + 1);
#if CPU64
                        if ((shift & 4) != 0) *(uint*)((byte*)destination + 3) = *(uint*)((byte*)source + 3);
#endif
                    }
#if CPU64
                    else if ((shift & 4) != 0) *(uint*)((byte*)destination + 1) = *(uint*)((byte*)source + 1);
#endif
                }
#if CPU64
                else if ((shift & 2) != 0)
                {
                    *(ushort*)destination = *(ushort*)source;
                    if ((shift & 4) != 0) *(uint*)((byte*)destination + 2) = *(uint*)((byte*)source + 2);
                }
                else *(uint*)destination = *(uint*)source;
#else
                else *(ushort*)destination = *(ushort*)source;
#endif
                copy((byte*)source + shift, (byte*)destination + shift, length -= (int)shift);
            }
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="destination">目标字节数组,不能为null</param>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        public unsafe static byte[] Copy(void* source, int length)
        {
            byte[] data = new byte[length];
            fixed (byte* dataFixed = data) copy((byte*)source, dataFixed, length);
            return data;
        }
        /// <summary>
        /// 复制字符数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        public unsafe static void Copy(char* source, char* destination, int count)
        {
            Copy((void*)source, (void*)destination, count << 1);
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        unsafe static void copy(byte* source, byte* destination, int length)
        {
            if (length >= sizeof(cpuUint) * 4)
            {
                do
                {
                    *((cpuUint*)destination) = *((cpuUint*)source);
                    *((cpuUint*)(destination + sizeof(cpuUint))) = *((cpuUint*)(source + sizeof(cpuUint)));
                    *((cpuUint*)(destination + sizeof(cpuUint) * 2)) = *((cpuUint*)(source + sizeof(cpuUint) * 2));
                    *((cpuUint*)(destination + sizeof(cpuUint) * 3)) = *((cpuUint*)(source + sizeof(cpuUint) * 3));
                    destination += sizeof(cpuUint) * 4;
                    source += sizeof(cpuUint) * 4;
                }
                while ((length -= sizeof(cpuUint) * 4) >= sizeof(cpuUint) * 4);
            }
            if ((length & (sizeof(cpuUint) * 2)) != 0)
            {
                *((cpuUint*)destination) = *((cpuUint*)source);
                *((cpuUint*)(destination + sizeof(cpuUint))) = *((cpuUint*)(source + sizeof(cpuUint)));
                destination += sizeof(cpuUint) * 2;
                source += sizeof(cpuUint) * 2;
            }
            if ((length & sizeof(cpuUint)) != 0)
            {
                *((cpuUint*)destination) = *((cpuUint*)source);
                destination += sizeof(cpuUint);
                source += sizeof(cpuUint);
            }
#if CPU64
            if ((length & sizeof(uint)) != 0)
            {
                *((uint*)destination) = *((uint*)source);
                destination += sizeof(uint);
                source += sizeof(uint);
            }
#endif
            if ((length & 2) != 0)
            {
                *((ushort*)destination) = *((ushort*)source);
                destination += 2;
                source += 2;
            }
            if ((length & 1) != 0) *destination = *source;
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        public unsafe static void CopyDesc(void* source, void* destination, int length)
        {
            source = (byte*)source + length;
            destination = (byte*)destination + length;
            int shift = (int)destination & (sizeof(cpuUint) - 1);
            if (shift == 0) copyDesc((byte*)source, (byte*)destination, length);
            else
            {
                if (shift > length) shift = length;
                if ((shift & 1) != 0)
                {
                    source = (byte*)source - 1;
                    destination = (byte*)destination - 1;
                    *(byte*)destination = *(byte*)source;
                }
                if ((shift & 2) != 0)
                {
                    source = (byte*)source - sizeof(ushort);
                    destination = (byte*)destination - sizeof(ushort);
                    *(ushort*)destination = *(ushort*)source;
                }
#if CPU64
                if ((shift & 4) != 0)
                {
                    source = (byte*)source - sizeof(uint);
                    destination = (byte*)destination - sizeof(uint);
                    *(uint*)destination = *(uint*)source;
                }
#endif
                copyDesc((byte*)source, (byte*)destination, length -= (int)shift);
            }
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        unsafe static void copyDesc(byte* source, byte* destination, int length)
        {
            if (length >= sizeof(cpuUint) * 4)
            {
                do
                {
                    destination -= sizeof(cpuUint) * 4;
                    source -= sizeof(cpuUint) * 4;
                    *((cpuUint*)(destination + sizeof(cpuUint) * 3)) = *((cpuUint*)(source + sizeof(cpuUint) * 3));
                    *((cpuUint*)(destination + sizeof(cpuUint) * 2)) = *((cpuUint*)(source + sizeof(cpuUint) * 2));
                    *((cpuUint*)(destination + sizeof(cpuUint))) = *((cpuUint*)(source + sizeof(cpuUint)));
                    *((cpuUint*)destination) = *((cpuUint*)source);
                }
                while ((length -= sizeof(cpuUint) * 4) >= sizeof(cpuUint) * 4);
            }
            if ((length & (sizeof(cpuUint) * 2)) != 0)
            {
                destination -= sizeof(cpuUint) * 2;
                source -= sizeof(cpuUint) * 2;
                *((cpuUint*)(destination + sizeof(cpuUint))) = *((cpuUint*)(source + sizeof(cpuUint)));
                *((cpuUint*)destination) = *((cpuUint*)source);
            }
            if ((length & sizeof(cpuUint)) != 0)
            {
                destination -= sizeof(cpuUint);
                source -= sizeof(cpuUint);
                *((cpuUint*)destination) = *((cpuUint*)source);
            }
#if CPU64
            if ((length & 4) != 0)
            {
                destination -= 4;
                source -= 4;
                *((uint*)destination) = *((uint*)source);
            }
#endif
            if ((length & 2) != 0)
            {
                destination -= 2;
                source -= 2;
                *((ushort*)destination) = *((ushort*)source);
            }
            if ((length & 1) != 0) *--destination = *--source;
        }
        /// <summary>
        /// 填充二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        public unsafe static void FillBits(byte[] data, int start, int count)
        {
            fixed (byte* dataFixed = data) FillBits(dataFixed, start, count);
        }
        /// <summary>
        /// 填充二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        public unsafe static void FillBits(byte* data, int start, int count)
        {
            data += (start >> 5) << 2;
            if ((start &= ((sizeof(uint) << 3) - 1)) != 0)
            {
                int high = (sizeof(uint) << 3) - start;
                if ((count -= high) >= 0)
                {
                    *(uint*)data |= uint.MaxValue << start;
                    data += sizeof(uint);
                }
                else
                {
                    *(uint*)data |= (uint.MaxValue >> (start - count)) << start;
                    return;
                }
            }
            Fill(data, uint.MaxValue, start = count >> 5);
            if ((count = -count & 31) != 0) *(uint*)(data + (start << 2)) |= uint.MaxValue >> count;
        }
        /// <summary>
        /// 清除二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        public unsafe static void ClearBits(byte[] data, int start, int count)
        {
            fixed (byte* dataFixed = data) ClearBits(dataFixed, start, count);
        }
        /// <summary>
        /// 清除二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        public unsafe static void ClearBits(byte* data, int start, int count)
        {
            data += (start >> 5) << 2;
            if ((start &= ((sizeof(uint) << 3) - 1)) != 0)
            {
                int high = (sizeof(uint) << 3) - start;
                if ((count -= high) >= 0)
                {
                    *(uint*)data &= uint.MaxValue >> high;
                    data += sizeof(uint);
                }
                else
                {
                    *(uint*)data &= ((uint.MaxValue >> (start - count)) << start) ^ uint.MaxValue;
                    return;
                }
            }
            Fill(data, (uint)0, start = count >> 5);
            if ((count &= 31) != 0) *(uint*)(data + (start << 2)) &= uint.MaxValue << count;

        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <returns>是否相等</returns>
        public static bool Equal(byte[] left, byte[] right)
        {
            return left.Length == right.Length && Equal(left, right, left.Length);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        public static unsafe bool Equal(byte[] left, byte[] right, int count)
        {
            fixed (byte* leftFixed = left, rightFixed = right) return equal(leftFixed, rightFixed, count);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        public static unsafe bool Equal(byte[] left, void* right, int count)
        {
            fixed (byte* leftFixed = left) return equal(leftFixed, (byte*)right, count);
        }
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        public static unsafe bool Equal(void* left, void* right, int count)
        {
            int shift = (int)left & (sizeof(cpuUint) - 1);
            if (count > shift)
            {
                return equal((byte*)left, (byte*)right, shift) && equal((byte*)left + shift, (byte*)right + shift, count - shift);
            }
            else return equal((byte*)left, (byte*)right, count);
        }
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数</param>
        /// <returns>是否相等</returns>
        static unsafe bool equal(byte* left, byte* right, int count)
        {
            while (count >= sizeof(cpuUint) * 4)
            {
                if (((*((cpuUint*)left) ^ *((cpuUint*)right)) |
                    (*((cpuUint*)(left + sizeof(cpuUint))) ^ *((cpuUint*)(right + sizeof(cpuUint)))) |
                    (*((cpuUint*)(left + sizeof(cpuUint) * 2)) ^ *((cpuUint*)(right + sizeof(cpuUint) * 2))) |
                    (*((cpuUint*)(left + sizeof(cpuUint) * 3)) ^ *((cpuUint*)(right + sizeof(cpuUint) * 3)))) != 0) return false;
                left += sizeof(cpuUint) * 4;
                right += sizeof(cpuUint) * 4;
                count -= sizeof(cpuUint) * 4;
            }
            if (count < sizeof(cpuUint) * 4)
            {
                cpuUint isEqual = 0;
                if ((count & (sizeof(cpuUint) * 2)) != 0)
                {
                    isEqual |= (*((cpuUint*)left) ^ *((cpuUint*)right))
                        | (*((cpuUint*)(left + sizeof(cpuUint))) ^ *((cpuUint*)(right + sizeof(cpuUint))));
                    left += sizeof(cpuUint) * 2;
                    right += sizeof(cpuUint) * 2;
                }
                if ((count & sizeof(cpuUint)) != 0)
                {
                    isEqual |= *((cpuUint*)left) ^ *((cpuUint*)right);
                    left += sizeof(cpuUint);
                    right += sizeof(cpuUint);
                }
                if ((count &= (sizeof(cpuUint) - 1)) != 0)
                {
                    count <<= 3;
                    isEqual |= (cpuUint)((*((cpuUint*)left) ^ *((cpuUint*)right)) << (fastCSharp.pub.CpuBits - count));
                }
                return isEqual == 0;
            }
            return true;
        }
        /// <summary>
        /// 查找字节
        /// </summary>
        /// <param name="data">字节数组,不能为null</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为-1</returns>
        public unsafe static int IndexOf(byte[] data, byte value)
        {
            fixed (byte* dataFixed = data)
            {
                byte* valueData = Find(dataFixed, dataFixed + data.Length, value);
                return valueData != null ? (int)(valueData - dataFixed) : -1;
            }
        }
        /// <summary>
        /// 查找字节
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为null</returns>
        public unsafe static byte* Find(byte* start, byte* end, byte value)
        {
            byte oldValue = *--end;
            *end = value;
            while (*start != value) ++start;
            *end = oldValue;
            return start != end || oldValue == value ? start : null;
        }
        /// <summary>
        /// 字节替换
        /// </summary>
        /// <param name="value">字节数组,长度不能为0</param>
        /// <param name="oldData">原字节</param>
        /// <param name="newChar">目标字节</param>
        /// <returns>字节数组</returns>
        public unsafe static void Replace(byte[] value, byte oldData, byte newData)
        {
            fixed (byte* valueFixed = value)
            {
                byte* start = valueFixed, end = valueFixed + value.Length;
                byte endValue = *--end;
                *end = oldData;
                do
                {
                    while (*start != oldData) ++start;
                    *start = newData;
                }
                while (start++ != end);
                if (endValue != oldData) *end = endValue;
            }
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        public unsafe static void ToLower(byte* start, byte* end)
        {
            while (start != end)
            {
                if ((uint)(*start - 'A') < 26) *start |= 0x20;
                ++start;
            }
        }
    }
}
