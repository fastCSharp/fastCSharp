using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static class number
    {
        #region 转字符串(用于代码生成)
        /// <summary>
        /// 16位除以10转乘法的乘数
        /// </summary>
        public const uint Div10_16Mul = ((1 << 19) + 9) / 10;
        /// <summary>
        /// 16位除以10转乘法的位移
        /// </summary>
        public const int Div10_16Shift = 19;
        //public const int Div100_16Mul = ((1 << 22) + 99) / 100;
        //public const int Div100_16Shift = 22;
        /// <summary>
        /// 32位除以10000转乘法的乘数
        /// </summary>
        public const ulong Div10000Mul = ((1L << 45) + 9999) / 10000;
        /// <summary>
        /// 32位除以10000转乘法的位移
        /// </summary>
        public const int Div10000Shift = 45;
        /// <summary>
        /// 32位除以100000000转乘法的乘数
        /// </summary>
        public const ulong Div100000000Mul = ((1L << 58) + 99999999) / 100000000;
        /// <summary>
        /// 32位除以100000000转乘法的位移
        /// </summary>
        public const int Div100000000Shift = 58;
        /// <summary>
        /// 获取除法转乘法的乘数与位移
        /// </summary>
        /// <param name="value">除数</param>
        /// <returns>乘数与位移</returns>
        public static keyValue<uint, int> divShift(ushort value)
        {
            uint divMul = uint.MaxValue / value;
            int shift = divMul.bits() - 16;
            divMul >>= shift;
            shift ^= 31;
            return new keyValue<uint, int>(++divMul, ++shift);
        }
        /// <summary>
        /// 获取除法转乘法的乘数与位移
        /// </summary>
        /// <param name="value">除数</param>
        /// <returns>乘数与位移</returns>
        public static keyValue<ulong, int> divShift(uint value)
        {
            ulong divMul = ulong.MaxValue / value;
            int shift = ((uint)(divMul >> 32)).bits();
            divMul >>= shift;
            shift ^= 63;
            return new keyValue<ulong, int>(++divMul, ++shift);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this byte value)
        {
            if (value >= 100)
            {
                char* chars = stackalloc char[4];
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                chars[2] = (char)((value - value10 * 10) + '0');
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                return new string(chars, 0, 3);
            }
            else if (value >= 10)
            {
                char* chars = stackalloc char[2];
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return new string(chars, 0, 2);
            }
            return new string((char)(value + '0'), 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this sbyte value)
        {
            if (value >= 0)
            {
                if (value >= 100)
                {
                    char* chars = stackalloc char[3];
                    value -= 100;
                    *chars = '1';
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 3);
                }
                else if (value >= 10)
                {
                    char* chars = stackalloc char[2];
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 2);
                }
                return new string((char)(value + '0'), 1);
            }
            else
            {
                int value32 = -value;
                if (value32 >= 100)
                {
                    char* chars = stackalloc char[4];
                    value32 -= 100;
                    *(int*)chars = '-' + ('1' << 16);
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 4);
                }
                else if (value32 >= 10)
                {
                    char* chars = stackalloc char[3];
                    *chars = '-';
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 3);
                }
                else
                {
                    char* chars = stackalloc char[2];
                    *(int*)chars = '-' + ((value32 + '0') << 16);
                    return new string(chars, 0, 2);
                }
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this ushort value)
        {
            if (value >= 10000)
            {
                char* chars = stackalloc char[5];
                int value10 = (int)((uint)(value * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 3) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value = (ushort)((value10 * Div10_16Mul) >> Div10_16Shift);
                *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030;
                *chars = (char)(value + '0');
                return new string(chars, 0, 5);
            }
            else if (value >= 100)
            {
                char* chars = stackalloc char[4];
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 4);
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                    return new string(chars, 0, 3);
                }
            }
            else if (value >= 10)
            {
                char* chars = stackalloc char[2];
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return new string(chars, 0, 2);
            }
            return new string((char)(value + '0'), 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this short value)
        {
            if (value >= 0) return toString((ushort)value);
            int value32 = -value;
            if (value32 >= 10000)
            {
                char* chars = stackalloc char[6];
                int value10 = (int)((uint)(value32 * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 4) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value32 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 2) = ((value100 - value10 * 10) << 16) | (value10 - value32 * 10) | 0x300030;
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return new string(chars, 0, 6);
            }
            else if (value32 >= 100)
            {
                if (value32 >= 1000)
                {
                    char* chars = stackalloc char[5];
                    *chars = '-';
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 3) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return new string(chars, 0, 5);
                }
                else
                {
                    char* chars = stackalloc char[4];
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    *(int*)chars = '-' + ((value100 + '0') << 16);
                    return new string(chars, 0, 4);
                }
            }
            else if (value32 >= 10)
            {
                char* chars = stackalloc char[3];
                *chars = '-';
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return new string(chars, 0, 3);
            }
            else
            {
                char* chars = stackalloc char[2];
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return new string(chars, 0, 2);
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this uint value)
        {
            if (value >= 100000000)
            {
                char* chars = stackalloc char[10];
                uint value100000000 = (uint)((value * (ulong)Div100000000Mul) >> Div100000000Shift);
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 2);
                ToString(value - value10000 * 10000, chars + 6);
                if (value100000000 >= 10)
                {
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new string(chars, 0, 10);
                }
                *++chars = (char)(value100000000 + '0');
                return new string(chars, 0, 9);
            }
            return ToString99999999(value);
        }
        /// <summary>
        /// 小于100000000的正整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        private unsafe static string ToString99999999(uint value)
        {
            if (value >= 10000)
            {
                char* chars = stackalloc char[8];
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        ToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        return new string(chars, 0, 8);
                    }
                    else
                    {
                        ToString(value - value10000 * 10000, chars + 3);
                        chars[2] = (char)((value10000 - value10 * 10) + '0');
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10 - value * 10) << 16) | value | 0x300030U;
                        return new string(chars, 0, 7);
                    }
                }
                else if (value10000 >= 10)
                {
                    ToString(value - value10000 * 10000, chars + 2);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    return new string(chars, 0, 6);
                }
                ToString(value - value10000 * 10000, chars + 1);
                chars[0] = (char)(value10000 + '0');
                return new string(chars, 0, 5);
            }
            else if (value >= 100)
            {
                char* chars = stackalloc char[4];
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    return new string(chars, 0, 4);
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030U;
                    return new string(chars, 0, 3);
                }
            }
            else if (value >= 10)
            {
                char* chars = stackalloc char[2];
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return new string(chars, 0, 2);
            }
            return new string((char)(value + '0'), 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this int value)
        {
            if (value >= 0) return toString((uint)value);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                char* chars = stackalloc char[12];
                uint value100000000 = (uint)((value32 * (ulong)Div100000000Mul) >> Div100000000Shift);
                value32 -= value100000000 * 100000000;
                uint value10000 = (uint)((value32 * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 4);
                ToString(value32 - value10000 * 10000, chars + 8);
                if (value100000000 >= 10)
                {
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *++chars = '-';
                    return new string(chars, 0, 11);
                }
                *(uint*)(chars += 2) = '-' + ((value100000000 + '0') << 16);
                return new string(chars, 0, 10);
            }
            return ToString_99999999(value32);
        }
        /// <summary>
        /// 绝对值小于100000000的负整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        private unsafe static string ToString_99999999(uint value)
        {
            if (value >= 10000)
            {
                char* chars = stackalloc char[10];
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        ToString(value - value10000 * 10000, chars + 6);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        *++chars = '-';
                        return new string(chars, 0, 9);
                    }
                    else
                    {
                        ToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        *(uint*)chars = '-' + ((value + '0') << 16);
                        return new string(chars, 0, 8);
                    }
                }
                else if (value10000 >= 10)
                {
                    ToString(value - value10000 * 10000, chars + 4);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    *++chars = '-';
                    return new string(chars, 0, 7);
                }
                ToString(value - value10000 * 10000, chars + 2);
                *(uint*)chars = '-' + ((value10000 + '0') << 16);
                return new string(chars, 0, 6);
            }
            else if (value >= 100)
            {
                if (value >= 1000)
                {
                    char* chars = stackalloc char[6];
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    *++chars = '-';
                    return new string(chars, 0, 5);
                }
                else
                {
                    char* chars = stackalloc char[4];
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    *(uint*)chars = '-' + ((value100 + '0') << 16);
                    return new string(chars, 0, 4);
                }
            }
            else if (value >= 10)
            {
                char* chars = stackalloc char[3];
                *chars = '-';
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return new string(chars, 0, 3);
            }
            else
            {
                char* chars = stackalloc char[2];
                *(uint*)chars = '-' + ((value + '0') << 16);
                return new string(chars, 0, 2);
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        private unsafe static void ToString(uint value, char* chars)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
            uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this ulong value)
        {
            if (value >= 10000000000000000L)
            {
                char* chars = stackalloc char[20];
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 12);
                ToString((uint)value - value10000 * 10000U, chars + 16);
                value = value100000000 / 100000000;
                value100000000 -= value * 100000000;
                value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 4);
                ToString((uint)value100000000 - value10000 * 10000U, chars + 8);
                uint value32 = (uint)value;
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        return new string(chars, 0, 20);
                    }
                    else
                    {
                        *++chars = (char)(value100 + '0');
                        return new string(chars, 0, 19);
                    }
                }
                else if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars += 2) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new string(chars, 0, 18);
                }
                *(chars += 3) = (char)(value32 + '0');
                return new string(chars, 0, 17);
            }
            else if (value >= 100000000)
            {
                char* chars = stackalloc char[16];
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 8);
                ToString((uint)value - value10000 * 10000U, chars + 12);
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                    ToString(value32 - value10000 * 10000, chars + 4);
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        if (value10000 >= 1000)
                        {
                            uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)(chars + 2) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)chars = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            return new string(chars, 0, 16);
                        }
                        else
                        {
                            chars[3] = (char)((value10000 - value32 * 10) + '0');
                            uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)(chars += 1) = ((value32 - value100 * 10) << 16) | value100 | 0x300030U;
                            return new string(chars, 0, 15);
                        }
                    }
                    else if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars += 2) = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        return new string(chars, 0, 14);
                    }
                    *(chars += 3) = (char)(value10000 + '0');
                    return new string(chars, 0, 13);
                }
                else if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 6) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars += 4) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        return new string(chars, 0, 12);
                    }
                    else
                    {
                        chars[7] = (char)((value32 - value10000 * 10) + '0');
                        uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars += 5) = ((value10000 - value100 * 10) << 16) | value100 | 0x300030U;
                        return new string(chars, 0, 11);
                    }
                }
                else if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars += 6) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new string(chars, 0, 10);
                }
                *(chars += 7) = (char)(value32 + '0');
                return new string(chars, 0, 9);
            }
            return ToString99999999((uint)value);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this long value)
        {
            if (value >= 0) return toString((ulong)value);
            ulong value64 = (ulong)-value;
            if (value64 >= 10000000000000000L)
            {
                char* chars = stackalloc char[22];
                ulong value100000000 = value64 / 100000000;
                value64 -= value100000000 * 100000000;
                uint value10000 = (uint)((value64 * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 14);
                ToString((uint)value64 - value10000 * 10000U, chars + 18);
                value64 = value100000000 / 100000000;
                value100000000 -= value64 * 100000000;
                value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 6);
                ToString((uint)value100000000 - value10000 * 10000U, chars + 10);
                uint value32 = (uint)value64;
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        *++chars = '-';
                        return new string(chars, 0, 21);
                    }
                    else
                    {
                        *(uint*)(chars += 2) = '-' + ((value100 + '0') << 16);
                        return new string(chars, 0, 20);
                    }
                }
                else if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *(chars += 3) = '-';
                    return new string(chars, 0, 19);
                }
                *(uint*)(chars += 4) = '-' + ((value32 + '0') << 16);
                return new string(chars, 0, 18);
            }
            else if (value64 >= 100000000)
            {
                char* chars = stackalloc char[18];
                ulong value100000000 = value64 / 100000000;
                value64 -= value100000000 * 100000000;
                uint value10000 = (uint)((value64 * Div10000Mul) >> Div10000Shift);
                ToString(value10000, chars + 10);
                ToString((uint)value64 - value10000 * 10000U, chars + 14);
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                    ToString(value32 - value10000 * 10000, chars + 6);
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                        if (value10000 >= 1000)
                        {
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)(chars + 2) = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            *++chars = '-';
                            return new string(chars, 0, 17);
                        }
                        else
                        {
                            *(uint*)(chars += 2) = '-' + ((value100 + '0') << 16);
                            return new string(chars, 0, 16);
                        }
                    }
                    else if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        *(chars += 3) = '-';
                        return new string(chars, 0, 15);
                    }
                    *(uint*)(chars += 4) = '-' + ((value10000 + '0') << 16);
                    return new string(chars, 0, 14);
                }
                else if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 8) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 6) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        *(chars += 5) = '-';
                        return new string(chars, 0, 13);
                    }
                    else
                    {
                        *(uint*)(chars += 6) = '-' + ((value100 + '0') << 16);
                        return new string(chars, 0, 12);
                    }
                }
                else if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 8) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *(chars += 7) = '-';
                    return new string(chars, 0, 11);
                }
                *(uint*)(chars += 8) = '-' + ((value32 + '0') << 16);
                return new string(chars, 0, 10);
            }
            return ToString_99999999((uint)value64);
        }
        /// <summary>
        /// 双精度浮点数转字符串(用于代码生成)
        /// </summary>
        /// <param name="value">浮点数</param>
        /// <returns>字符串</returns>
        public static string toString(this double value)
        {
            return value.ToString();
        }
        /// <summary>
        /// 双精度浮点数转字符串(用于代码生成)
        /// </summary>
        /// <param name="value">浮点数</param>
        /// <returns>字符串</returns>
        public static string toString(this float value)
        {
            return value.ToString();
        }
        #endregion

        /// <summary>
        /// 2^n相关32位deBruijn序列集合
        /// </summary>
        private static pointer deBruijn32;
        /// <summary>
        /// 2^n相关32位deBruijn序列
        /// </summary>
        private const uint deBruijn32Number = 0x04653adfU;
        ///// <summary>
        ///// 2^n相关64位deBruijn序列集合
        ///// </summary>
        //private static byte[] deBruijn64;
        ///// <summary>
        ///// 2^n相关64位deBruijn序列
        ///// </summary>
        //private const ulong deBruijn64Number = 0x0218a392cd3d5dbfUL;
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        public static int bits(this uint value)
        {
            if ((value & 0x80000000U) == 0)
            {
                uint code = value;
                code |= code >> 16;
                code |= code >> 8;
                code |= code >> 4;
                code |= code >> 2;
                code |= code >> 1;
                return deBruijn32.Byte[((++code) * deBruijn32Number) >> 27];
            }
            return 32;
        }
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        public static int bits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0 ? bits((uint)value) : (bits((uint)(value >> 32)) + 32);
            //if ((value & 0x8000000000000000UL) == 0)
            //{
            //    ulong code = value;
            //    code |= code >> 32;
            //    code |= code >> 16;
            //    code |= code >> 8;
            //    code |= code >> 4;
            //    code |= code >> 2;
            //    code |= code >> 1;
            //    return DeBruijn64[((++code) * DeBruijn64Number) >> 58];
            //}
            //else return 32;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        public static int endBits(this uint value)
        {
            return value != 0 ? deBruijn32.Byte[((value & (0U - value)) * deBruijn32Number) >> 27] : 0;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        public static int endBits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0
                ? (value != 0 ? endBits((uint)(value >> 32)) + 32 : 0)
                : endBits((uint)value);
            //return value != 0 ? DeBruijn64[((value & (0UL - value)) * DeBruijn64Number) >> 58] : 0;
        }
        /// <summary>
        /// 获取二进制1位的个数
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>二进制1位的个数</returns>
        public static int bitCount(this uint value)
        {
            //return bitCounts[(byte)value] + bitCounts[(byte)(value >> 8)] + bitCounts[(byte)(value >> 16)] + bitCounts[value >> 24];

            //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
            //value = (value & 0xc71c71c7) + ((value >> 3) & 0xc71c71c7);
            //uint div = (uint)(((ulong)value * (((1UL << 37) + 62) / 63)) >> 37);
            //return (int)(value - (div << 6) + div);

            //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
            //value = (value & 0x71c71c7) + ((value >> 3) & 0x71c71c7) + (value >> 30);
            //uint nextValue = (uint)((value * 0x41041042UL) >> 36);
            //return (int)(value - (nextValue << 6) + nextValue);

            value -= ((value >> 1) & 0x55555555U);//2:2
            value = (value & 0x33333333U) + ((value >> 2) & 0x33333333U);//4:4
            value += value >> 4;
            value &= 0x0f0f0f0f;//8:8

            //uint div = (uint)(((ulong)value * (((1UL << 39) + 254) / 255)) >> 39);
            //return (int)(value - (div << 8) + div);
            value += (value >> 8);
            return (byte)(value + (value >> 16));
        }
        /// <summary>
        /// 获取二进制1位的个数
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>二进制1位的个数</returns>
        public static int bitCount(this ulong value)
        {
            value -= ((value >> 1) & 0x5555555555555555UL);//2:2
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);//4:4
            value += value >> 4;
            value &= 0x0f0f0f0f0f0f0f0fUL;//8:8
            //return (int)(value % 255);
            value += (value >> 8);
            value += (value >> 16);
            return (byte)(value + (value >> 32));
        }
        /// <summary>
        /// 判断一个数是否2的幂次方
        /// </summary>
        /// <returns>是否2的幂次方</returns>
        public static bool isPower2(this ulong value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        /// <summary>
        /// 判断一个数是否2的幂次方
        /// </summary>
        /// <returns>是否2的幂次方</returns>
        public static bool isPower2(this uint value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        /// <summary>
        /// 获取2的幂次方的位数
        /// </summary>
        /// <param name="value">2的幂次方值</param>
        /// <returns>位数,非幂次方返回-1</returns>
        public static int power2Bits(this ulong value)
        {
            int bits = (value & 0xffffffff00000000UL) == 0
                ? deBruijn32.Byte[((uint)value * deBruijn32Number) >> 27]
                : (deBruijn32.Byte[((uint)(value >> 32) * deBruijn32Number) >> 27] + 32);
            return 1UL << bits == value ? bits : -1;
        }
        /// <summary>
        /// 获取2的幂次方的位数
        /// </summary>
        /// <param name="value">2的幂次方值</param>
        /// <returns>位数,非幂次方返回-1</returns>
        public static int power2Bits(this uint value)
        {
            int bits = deBruijn32.Byte[(value * deBruijn32Number) >> 27];
            return 1U << bits == value ? bits : -1;
        }
        /// <summary>
        /// 二进制反转
        /// </summary>
        /// <param name="value">整数</param>
        /// <returns>反转后的数</returns>
        public static uint reverse(this uint value)
        {
            value = ((value << 1) & 0xaaaaaaaaU) | ((value >> 1) & 0x55555555U);
            value = ((value << 2) & 0xccccccccU) | ((value >> 2) & 0x33333333U);
            value = ((value << 4) & 0xf0f0f0f0U) | ((value >> 4) & 0xf0f0f0fU);
            value = ((value << 8) & 0xff00ff00U) | ((value >> 8) & 0xff00ffU);
            return (value << 16) | (value >> 16);
        }
        /// <summary>
        /// 求最大公约数
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns>最大公约数</returns>
        public static ulong gcd(this ulong leftValue, ulong rightValue)
        {
            if (leftValue == 0 || rightValue == 0) log.Default.Throw(log.exceptionType.Null);
            for (rightValue %= leftValue; rightValue != 0; rightValue %= leftValue)
            {
                if ((leftValue %= rightValue) == 0) return rightValue;
            }
            return leftValue;
            //int leftShift = leftValue == 0 ? 0 : power2_64bits[(leftValue ^ (leftValue & (leftValue - 1))) % 67], rightShift = rightValue == 0 ? 0 : power2_64bits[(rightValue ^ (rightValue & (rightValue - 1))) % 67];
            //if (leftShift == 0)
            //{
            //    if (rightShift != 0) rightValue >>= rightShift;
            //}
            //else if (rightShift == 0)
            //{
            //    leftValue >>= leftShift;
            //    leftShift = 0;
            //}
            //else
            //{
            //    leftValue >>= leftShift;
            //    rightValue >>= rightShift;
            //    if (leftShift > rightShift) leftShift = rightShift;
            //}
            //if (leftValue < rightValue) rightValue -= leftValue;
            //else leftValue -= (rightValue = leftValue - rightValue);
            //while (rightValue != 0)
            //{
            //    if (leftValue < (rightValue >>= power2_64bits[(rightValue ^ (rightValue & (rightValue - 1))) % 67])) rightValue -= leftValue;
            //    else leftValue -= (rightValue = leftValue - rightValue);
            //}
            //leftValue <<= leftShift;
        }
        /// <summary>
        /// 求最大公约数
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns>最大公约数</returns>
        public static uint gcd(this uint leftValue, uint rightValue)
        {
            if (leftValue == 0 || rightValue == 0) log.Default.Throw(log.exceptionType.Null);
            for (rightValue %= leftValue; rightValue != 0; rightValue %= leftValue)
            {
                if ((leftValue %= rightValue) == 0) return rightValue;
            }
            return leftValue;
        }
        /// <summary>
        /// 乘方取余
        /// </summary>
        /// <param name="value">基数</param>
        /// <param name="power">幂</param>
        /// <param name="mod">模</param>
        /// <returns>余数</returns>
        public static uint powerMod(uint value, ulong power, uint mod)
        {
            if (mod == 0) log.Default.Throw(log.exceptionType.Null);
            ulong value64 = (ulong)value, mod64 = (ulong)mod, modValue = 1;
            if ((power & 1) != 0) modValue = value % mod;
            for (power >>= 1; power != 0; power >>= 1)
            {
                value64 = (value64 * value64) % mod64;
                if ((power & 1) != 0) modValue = (modValue * value64) % mod64;
            }
            return (uint)modValue;
        }
        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>十六进制字符串</returns>
        public unsafe static string toHex(this ushort value)
        {
            char* chars = stackalloc char[4], nextChar = chars + 3;
            int next = value & 15;
            *nextChar = (char)(next < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            next = (value >>= 4) & 15;
            *--nextChar = (char)(next < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            next = (value >>= 4) & 15;
            *--nextChar = (char)(next < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((value >>= 4) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
            return new string(chars, 0, 4);
        }
        /// <summary>
        /// 数字值转换为十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        unsafe static void ToHex(this uint value, char* hexs)
        {
            char* nextChar = hexs + 8;
            uint next = value & 15;
            *--nextChar = (char)(next < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((next = (value >>= 4) & 15) < 10 ? next + '0' : (next + ('0' + 'A' - '9' - 1)));
            *--nextChar = (char)((value >>= 4) < 10 ? value + '0' : (value + ('0' + 'A' - '9' - 1)));
        }
        /// <summary>
        /// 转换8位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>8位十六进制字符串</returns>
        public unsafe static string toHex8(this uint value)
        {
            char* hexs = stackalloc char[8];
            value.ToHex(hexs);
            return new string(hexs, 0, 8);
        }
        /// <summary>
        /// 转换16位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>16位十六进制字符串</returns>
        public unsafe static string toHex16(this ulong value)
        {
            char* hexs = stackalloc char[16];
            ((uint)value).ToHex(hexs + 8);
            value >>= 32;
            ((uint)value).ToHex(hexs);
            return new string(hexs, 0, 16);
        }
        /// <summary>
        /// 16位十六进制转64二进制位整数(无格式检测)
        /// </summary>
        /// <param name="value">十六进制字符串</param>
        /// <returns>64二进制位整数</returns>
        public unsafe static ulong parseHex16NoCheck(this string value)
        {
            ulong low = 0;
            if (value != null && value.Length == 16)
            {
                fixed (char* hexs = value)
                {
                    char* hex = hexs;
                    uint bits = (uint)(*hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    low = (ulong)bits << 32;
                    bits = (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    bits <<= 4;
                    bits += (uint)(*++hex - '0' < 10 ? *hex - '0' : ((*hex & 0xdf) - ('0' + 'A' - '9' - 1)));
                    low |= bits;
                }
            }
            return low;
        }
        /// <summary>
        /// 返回随机长整数
        /// </summary>
        /// <returns>随机数</returns>
        public static ulong random()
        {
            return ((ulong)pub.Random.Next() << 32) + (ulong)pub.Random.Next();
        }
        /// <summary>
        /// 返回随机长整数
        /// </summary>
        /// <param name="maxNumber">最大数</param>
        /// <returns>随机数</returns>
        public static ulong random(ulong maxNumber)
        {
            return maxNumber > 1 ? random() % maxNumber : 0;
        }

        #region 求平方根
        /// <summary>
        /// 求平方根
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="mod">余数</param>
        /// <returns>平方根</returns>
        public static uint sqrt(this uint value, out uint mod)
        {
            uint sqrtValue = 0;
            if ((mod = value) >= 0x40000000)
            {
                sqrtValue = 0x8000;
                mod -= 0x40000000;
            }
            value = (sqrtValue << 15) + 0x10000000;
            if (mod >= value)
            {
                sqrtValue |= 0x4000;
                mod -= value;
            }
            value = (sqrtValue << 14) + 0x4000000;
            if (mod >= value)
            {
                sqrtValue |= 0x2000;
                mod -= value;
            }
            value = (sqrtValue << 13) + 0x1000000;
            if (mod >= value)
            {
                sqrtValue |= 0x1000;
                mod -= value;
            }
            value = (sqrtValue << 12) + 0x400000;
            if (mod >= value)
            {
                sqrtValue |= 0x800;
                mod -= value;
            }
            value = (sqrtValue << 11) + 0x100000;
            if (mod >= value)
            {
                sqrtValue |= 0x400;
                mod -= value;
            }
            value = (sqrtValue << 10) + 0x40000;
            if (mod >= value)
            {
                sqrtValue |= 0x200;
                mod -= value;
            }
            value = (sqrtValue << 9) + 0x10000;
            if (mod >= value)
            {
                sqrtValue |= 0x100;
                mod -= value;
            }
            value = (sqrtValue << 8) + 0x4000;
            if (mod >= value)
            {
                sqrtValue |= 0x80;
                mod -= value;
            }
            value = (sqrtValue << 7) + 0x1000;
            if (mod >= value)
            {
                sqrtValue |= 0x40;
                mod -= value;
            }
            value = (sqrtValue << 6) + 0x400;
            if (mod >= value)
            {
                sqrtValue |= 0x20;
                mod -= value;
            }
            value = (sqrtValue << 5) + 0x100;
            if (mod >= value)
            {
                sqrtValue |= 0x10;
                mod -= value;
            }
            value = (sqrtValue << 4) + 0x40;
            if (mod >= value)
            {
                sqrtValue |= 0x8;
                mod -= value;
            }
            value = (sqrtValue << 3) + 0x10;
            if (mod >= value)
            {
                sqrtValue |= 0x4;
                mod -= value;
            }
            value = (sqrtValue << 2) + 0x4;
            if (mod >= value)
            {
                sqrtValue |= 0x2;
                mod -= value;
            }
            value = (sqrtValue << 1) + 0x1;
            if (mod >= value)
            {
                sqrtValue++;
                mod -= value;
            }
            return sqrtValue;
        }
        /// <summary>
        /// 求平方根
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="mod">余数</param>
        /// <returns>平方根</returns>
        public static uint sqrt32(this ulong value, out ulong mod)
        {
            uint sqrtValue = 0, highValue = (uint)(value >> 32);
            if (highValue >= 0x40000000)
            {
                sqrtValue = 0x8000;
                highValue -= 0x40000000;
            }
            uint modHigh = (sqrtValue << 15) + 0x10000000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x4000;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 14) + 0x4000000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x2000;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 13) + 0x1000000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x1000;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 12) + 0x400000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x800;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 11) + 0x100000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x400;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 10) + 0x40000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x200;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 9) + 0x10000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x100;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 8) + 0x4000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x80;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 7) + 0x1000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x40;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 6) + 0x400;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x20;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 5) + 0x100;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x10;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 4) + 0x40;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x8;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 3) + 0x10;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x4;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 2) + 0x4;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x2;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 1) + 0x1;
            if (highValue >= modHigh)
            {
                sqrtValue++;
                highValue -= modHigh;
            }
            sqrtValue <<= 8;
            if ((highValue & 0x10000) == 0)
            {
                highValue = (highValue << 16) | (((uint)value) >> 16);
                modHigh = (sqrtValue << 8) + 0x4000;
                if (highValue >= modHigh)
                {
                    sqrtValue |= 0x80;
                    highValue -= modHigh;
                }
            }
            else
            {
                highValue = ((highValue << 16) | (((uint)value) >> 16)) - ((sqrtValue << 8) + 0x4000);
                sqrtValue |= 0x80;
            }
            modHigh = (sqrtValue << 7) + 0x1000;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x40;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 6) + 0x400;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x20;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 5) + 0x100;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x10;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 4) + 0x40;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x8;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 3) + 0x10;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x4;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 2) + 0x4;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x2;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 1) + 0x1;
            if (highValue >= modHigh)
            {
                sqrtValue++;
                highValue -= modHigh;
            }
            sqrtValue <<= 4;
            if ((highValue & 0x1000000) == 0)
            {
                highValue = (highValue << 8) | ((byte)(value >> 8));
                modHigh = (sqrtValue << 4) + 0x40;
                if (highValue >= modHigh)
                {
                    sqrtValue |= 0x8;
                    highValue -= modHigh;
                }
            }
            else
            {
                highValue = ((highValue << 8) | ((byte)(value >> 8))) - ((sqrtValue << 4) + 0x40);
                sqrtValue |= 0x8;
            }
            modHigh = (sqrtValue << 3) + 0x10;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x4;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 2) + 0x4;
            if (highValue >= modHigh)
            {
                sqrtValue |= 0x2;
                highValue -= modHigh;
            }
            modHigh = (sqrtValue << 1) + 0x1;
            if (highValue >= modHigh)
            {
                sqrtValue++;
                highValue -= modHigh;
            }
            sqrtValue <<= 2;
            if ((highValue & 0x10000000) == 0)
            {
                highValue = (highValue << 4) | (uint)(((byte)value) >> 4);
                modHigh = (sqrtValue << 2) + 0x4;
                if (highValue >= modHigh)
                {
                    sqrtValue |= 0x2;
                    highValue -= modHigh;
                }
            }
            else
            {
                highValue = ((highValue << 4) | (uint)(((byte)value) >> 4)) - ((sqrtValue << 2) + 0x4);
                sqrtValue |= 0x2;
            }
            modHigh = (sqrtValue << 1) + 0x1;
            if (highValue >= modHigh)
            {
                sqrtValue++;
                highValue -= modHigh;
            }
            //
            sqrtValue <<= 1;
            if ((highValue & 0x40000000) == 0)
            {
                highValue = (highValue << 2) | (uint)((((byte)value) >> 2) & 0x3);
                modHigh = (sqrtValue << 1) + 0x1;
                if (highValue >= modHigh)
                {
                    sqrtValue++;
                    highValue -= modHigh;
                }
            }
            else
            {
                highValue = ((highValue << 2) | (uint)((((byte)value) >> 2) & 0x3)) - ((sqrtValue << 1) + 0x1);
                sqrtValue++;
            }
            sqrtValue <<= 1;
            mod = ((ulong)highValue << 2) + (value & 0x3);
            value = (((ulong)sqrtValue) << 1) + 1;
            if (mod > value)
            {
                sqrtValue++;
                mod -= value;
            }
            //sqrtValue <<= 2;
            //mod = ((ulong)highValue << 4) + (value & 0xf);
            //value = ((ulong)sqrtValue << 2) + 0x4;
            //if (mod >= value)
            //{
            //    sqrtValue |= 0x2;
            //    mod -= value;
            //}
            //value = (((ulong)sqrtValue) << 1) + 1;
            //if (mod >= value)
            //{
            //    sqrtValue++;
            //    mod -= value;
            //}
            return sqrtValue;
        }
        //public static uint sqrt(this uint value)
        //{
        //    uint nextValue = value | (value >> 16);
        //    nextValue |= (nextValue >> 8);
        //    nextValue |= (nextValue >> 4);
        //    nextValue |= (nextValue >> 2);
        //    nextValue |= (nextValue >> 1);
        //    int bits = nextValue == uint.MaxValue ? 15 : (((power2_64bits[(nextValue + 1) % 67] + 1) >> 1) - 1);
        //    uint rightValue = value >> bits;
        //    nextValue = 1U << bits;//1:2
        //    nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//4:5
        //    if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//40:41
        //    if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//2380:2381
        //    if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//11333560:11333561
        //    return nextValue < rightValue ? nextValue : rightValue;
        //}
        //public static uint sqrt(ulong value)
        //{
        //    ulong nextValue = value | (value >> 32);
        //    nextValue |= (nextValue >> 16);
        //    nextValue |= (nextValue >> 8);
        //    nextValue |= (nextValue >> 4);
        //    nextValue |= (nextValue >> 2);
        //    nextValue |= (nextValue >> 1);
        //    int bits = nextValue == uint.MaxValue ? 31 : (((power2_64bits[(nextValue + 1) % 67] + 1) >> 1) - 1);
        //    if (bits >= 30)
        //    {
        //        ulong rightValue = value >> bits;
        //        nextValue = 1UL << bits;//1:2
        //        nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//4:5
        //        if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//40:41
        //        if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//2380:2381
        //        if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);//11333560:11333561
        //        if (nextValue < rightValue) nextValue = value / (rightValue = (rightValue + nextValue) >> 1);
        //        if (nextValue > rightValue) nextValue = rightValue;
        //    }
        //    else
        //    {
        //        uint rightValue = (uint)(value >> bits), leftValue = 1U << bits;
        //        leftValue = (uint)(value / (rightValue = (rightValue + leftValue) >> 1));//4:5
        //        if (leftValue < rightValue) leftValue = (uint)(value / (rightValue = (rightValue + leftValue) >> 1));//40:41
        //        if (leftValue < rightValue) leftValue = (uint)(value / (rightValue = (rightValue + leftValue) >> 1));//2380:2381
        //        if (leftValue < rightValue) leftValue = (uint)(value / (rightValue = (rightValue + leftValue) >> 1));//11333560:11333561
        //        if (leftValue < rightValue) leftValue = (uint)(value / (rightValue = (rightValue + leftValue) >> 1));
        //        nextValue = leftValue < rightValue ? leftValue : rightValue;
        //    }
        //    return (uint)nextValue;
        //}
        /// <summary>
        /// 求平方根余数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>余数(32bits)+平方根(32bits)</returns>
        public static ulong mSqrt(this ulong value)
        {
            if (value >= 2)
            {
                int bits = (value.bits() - 1) & 62;
                ulong result = 1, maxResult = 1UL << ((bits >> 1) + 1), subValue;
                for (value -= 1UL << bits, bits -= 2; bits >= 0 && value > maxResult; bits -= 2)
                {
                    result = ((result << 2) + 1) << bits;
                    subValue = (((value >> 1) - (result >> 1)) | ((((subValue = result - value - 1) | (subValue >> 1)) & long.MaxValue) - 1)) >> 63;
                    value -= (subValue - 1) & result;
                    result = (((result >> bits) - 1) >> 1) + (subValue ^ 1);
                }
                value = (value << 32) + (result << ((bits >> 1) + 1));
            }
            return value;
        }
        #endregion

        static number()
        {
            deBruijn32 = unmanaged.Get(32);
            byte* deBruijn32Data = deBruijn32.Byte;
            for (byte bit = 0; bit != 32; ++bit) deBruijn32Data[((1U << bit) * deBruijn32Number) >> 27] = bit;
            //deBruijn64 = new byte[64];
            //for (byte bit = 0; bit != 64; ++bit) deBruijn64[((1UL << bit) * deBruijn64Number) >> 58] = bit;
        }
    }
}
