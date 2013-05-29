using System;
using System.Collections.Generic;
using System.Text;

namespace fastCSharp
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    public unsafe static class String
    {
        #region ASCII位图
        /// <summary>
        /// ASCII位图
        /// </summary>
        public unsafe struct asciiMap
        {
            /// <summary>
            /// 位图字节长度
            /// </summary>
            public const int mapBytes = 128 >> 3;
            /// <summary>
            /// 非安全访问ASCII位图
            /// </summary>
            public fixedMap.unsafer Unsafer
            {
                get { return new fixedMap.unsafer(map); }
            }
            /// <summary>
            /// 位图
            /// </summary>
            private byte* map;
            /// <summary>
            /// 位图
            /// </summary>
            public byte* Map { get { return map; } }
            /// <summary>
            /// 指针
            /// </summary>
            public pointer Pointer { get { return new pointer { Data = map }; } }
            /// <summary>
            /// 初始化ASCII位图
            /// </summary>
            /// <param name="map">位图指针</param>
            /// <param name="value">初始值集合</param>
            /// <param name="isUnsafe">初始值是否安全</param>
            public asciiMap(pointer map, string value, bool isUnsafe = true) : this(map.Byte, value, isUnsafe) { }
            /// <summary>
            /// 初始化ASCII位图
            /// </summary>
            /// <param name="map">位图指针</param>
            /// <param name="value">初始值集合</param>
            /// <param name="isUnsafe">初始值是否安全</param>
            public asciiMap(byte* map, string value, bool isUnsafe = true)
            {
                this.map = map;
                if (isUnsafe && value.Length != 0)
                {
                    fixed (char* valueFixed = value)
                    {
                        for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                        {
                            map[*start >> 3] |= (byte)(1 << (*start & 7));
                        }
                    }
                }
                else Set(value);
            }
            /// <summary>
            /// 设置占位
            /// </summary>
            /// <param name="value">位值</param>
            public void Set(char value)
            {
                if ((value & 0xff80) == 0) map[value >> 3] |= (byte)(1 << (value & 7));
            }
            /// <summary>
            /// 设置占位
            /// </summary>
            /// <param name="value">位值</param>
            public void Set(byte value)
            {
                if ((value & 0x80) == 0) map[value >> 3] |= (byte)(1 << (value & 7));
            }
            /// <summary>
            /// 获取占位状态
            /// </summary>
            /// <param name="value">位值</param>
            /// <returns>是否占位</returns>
            public bool Get(char value)
            {
                return (value & 0xff80) == 0 && (map[value >> 3] & (byte)(1 << (value & 7))) != 0;
            }
            /// <summary>
            /// 获取占位状态
            /// </summary>
            /// <param name="value">位值</param>
            /// <returns>是否占位</returns>
            public bool Get(int value)
            {
                return ((uint)value & 0xffffff80) == 0 && (map[value >> 3] & (byte)(1 << (value & 7))) != 0;
            }
            /// <summary>
            /// 设置占位
            /// </summary>
            /// <param name="value">值集合</param>
            public unsafe void Set(string value)
            {
                if (value != null)
                {
                    fixed (char* valueFixed = value)
                    {
                        for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                        {
                            if ((*start & 0xff80) == 0) map[*start >> 3] |= (byte)(1 << (*start & 7));
                        }
                    }
                }
            }
        }
        #endregion

        #region 汉字编码检测
        /// <summary>
        /// 汉字编码检测
        /// </summary>
        private unsafe struct chineseEncoder
        {
            /// <summary>
            /// 当前位置
            /// </summary>
            private byte* start;
            /// <summary>
            /// 结束位置
            /// </summary>
            private byte* end;
            /// <summary>
            /// 第一个非ASCII码位置
            /// </summary>
            private byte* noAscii;
            /// <summary>
            /// GB2312命中数量
            /// </summary>
            private int gb2312Count;
            /// <summary>
            /// GBK命中数量
            /// </summary>
            private int gbkCount;
            /// <summary>
            /// GB18030命中数量
            /// </summary>
            private int gb18030Count;
            /// <summary>
            /// UTF8命中数量
            /// </summary>
            private int utf8Count;
            /// <summary>
            /// BIG5命中数量
            /// </summary>
            private int big5Count;
            /// <summary>
            /// UTF16命中数量
            /// </summary>
            private int utf16Count;
            /// <summary>
            /// 大端UTF16命中数量
            /// </summary>
            private int bigUtf16Count;
            /// <summary>
            /// UTF32命中数量
            /// </summary>
            private int utf32Count;
            /// <summary>
            /// 大端UTF32命中数量
            /// </summary>
            private int bigUtf32Count;
            /// <summary>
            /// 最后一个字符
            /// </summary>
            private int endValue;
            /// <summary>
            /// GB18030匹配表
            /// </summary>
            private char* gb18030Char;
            /// <summary>
            /// UTF16匹配表
            /// </summary>
            private char* utf16Char;
            /// <summary>
            /// 大端TF16匹配表
            /// </summary>
            private char* bigUtf16Char;
            //的是不了个，。
            //的是不了個，。
            /// <summary>
            /// 获取编码
            /// </summary>
            /// <param name="data">字节数组</param>
            /// <returns>编码,失败为null</returns>
            public Encoding Get(byte[] data)
            {
                fixed (byte* dataFixed = data)
                {
                    uint bom = *(uint*)dataFixed;
                    if (bom == 0xfeff && (data.Length & 3) == 0) return Encoding.UTF32;
                    //else if (bom == 0xfffe0000U && (data.Length & 3) == 0) return Encoding.UTF32;//大端
                    else if ((bom & 0xffffff) == 0xbfbbef) return Encoding.UTF8;
                    else if ((bom &= 0xffff) == 0xfeff && (data.Length & 1) == 0) return Encoding.Unicode;
                    else if (bom == 0xfffe && (data.Length & 1) == 0) return Encoding.BigEndianUnicode;

                    #region ASCII
                    start = dataFixed;
                    end = dataFixed + data.Length;
                    endValue = *--end;
                    if ((data.Length & 1) == 0)
                    {
                        if (isAscii())
                        {
                            int* counts = stackalloc int[2];
                            for (start = dataFixed, *end = 0, counts[1] = *counts = 0; *start != 0; ++start) ;
                            while (start != end)
                            {
                                ++counts[(uint)start & 1];
                                while (*++start != 0) ;
                            }
                            *end = (byte)endValue;
                            return counts[1] >= *counts ? (counts[1] == 0 ? Encoding.ASCII : Encoding.Unicode) : Encoding.BigEndianUnicode;
                        }
                        utf16Count = bigUtf16Count = 0;
                        utf32Count = bigUtf32Count = (data.Length & 3) != 0 ? int.MinValue : 0;
                    }
                    else
                    {
                        if (isAscii())
                        {
                            *end = (byte)endValue;
                            return Encoding.ASCII;
                        }
                        utf16Count = bigUtf16Count = utf32Count = bigUtf32Count = int.MinValue;
                    }
                    noAscii = start;
                    #endregion

                    #region GB2312
                    char* gb18030Chars = stackalloc char[0x40];
                    unsafer.memory.Fill((byte*)(gb18030Char = gb18030Chars), 0U, 0x40 * sizeof(char) / sizeof(uint));
                    gb18030Char[0] = char.MaxValue;
                    gb18030Char[0xc4b5 & 0x3f] = (char)0xc4b5;
                    gb18030Char[0xc7ca & 0x3f] = (char)0xc7ca;
                    gb18030Char[0xbbb2 & 0x3f] = (char)0xbbb2;
                    gb18030Char[0xcbc1 & 0x3f] = (char)0xcbc1;
                    gb18030Char[0xf6b8 & 0x3f] = (char)0xf6b8;
                    gb18030Char[0xaca3 & 0x3f] = (char)0xaca3;
                    gb18030Char[0xa3a1 & 0x3f] = (char)0xa3a1;
                    gb18030Char[0x8082 & 0x3f] = (char)0x8082;
                    gb2312();
                    if (gb2312Count >= 0) gb18030Count = gbkCount = int.MinValue;
                    else
                    {
                        *end++ = (byte)endValue;
                        gb18030();
                        gbk();
                        --end;
                    }
                    #endregion

                    utf8();

                    *end++ = (byte)endValue;
                    big5();

                    if (utf16Count >= 0)
                    {
                        #region UTF16
                        char* utf16Chars = stackalloc char[0x10];
                        unsafer.memory.Fill((byte*)(utf16Char = utf16Chars), 0U, 0x10 * sizeof(char) / sizeof(uint));
                        utf16Char[0] = char.MaxValue;
                        utf16Char[0x7684 & 0xf] = (char)0x7684;
                        utf16Char[0x662f & 0xf] = (char)0x662f;
                        utf16Char[0x4e0d & 0xf] = (char)0x4e0d;
                        utf16Char[0x4e86 & 0xf] = (char)0x4e86;
                        utf16Char[0x4e2a & 0xf] = (char)0x4e2a;
                        utf16Char[0xff0c & 0xf] = (char)0xff0c;
                        utf16Char[0x3002 & 0xf] = (char)0x3002;
                        utf16Char[0x500b & 0xf] = (char)0x500b;
                        char* bigUtf16Chars = stackalloc char[0x10];
                        unsafer.memory.Fill((byte*)(bigUtf16Char = bigUtf16Chars), 0U, 0x10 * sizeof(char) / sizeof(uint));
                        bigUtf16Char[0] = char.MaxValue;
                        bigUtf16Char[(0x8476 >> 8) & 0xf] = (char)0x8476;
                        bigUtf16Char[(0x2f66 >> 8) & 0xf] = (char)0x2f66;
                        bigUtf16Char[(0x0d4e >> 8) & 0xf] = (char)0x0d4e;
                        bigUtf16Char[(0x864e >> 8) & 0xf] = (char)0x864e;
                        bigUtf16Char[(0x2a4e >> 8) & 0xf] = (char)0x2a4e;
                        bigUtf16Char[(0x0cff >> 8) & 0xf] = (char)0x0cff;
                        bigUtf16Char[(0x0230 >> 8) & 0xf] = (char)0x0230;
                        bigUtf16Char[(0x0b50 >> 8) & 0xf] = (char)0x0b50;
                        #endregion
                        start = dataFixed;
                        if (utf32Count >= 0)
                        {
                            utf16();
                            start = dataFixed;
                            utf32();
                        }
                        else
                        {
                            end -= 2;
                            utf16();
                            bom = *(ushort*)end;
                            if (utf16Char[bom & 0xf] == bom) ++utf16Count;
                            else if (bigUtf16Char[(bom >> 8) & 0xf] == bom) ++bigUtf16Count;
                        }
                    }

                    Encoding value = null;
                    endValue = -1;
                    if (gb2312Count > endValue)
                    {
                        value = pub.Gb2312;
                        endValue = gb2312Count;
                    }
                    if (gb18030Count > endValue)
                    {
                        value = pub.Gb18030;
                        endValue = gb18030Count;
                    }
                    if (gbkCount > endValue)
                    {
                        value = pub.Gbk;
                        endValue = gbkCount;
                    }
                    if (utf8Count > endValue)
                    {
                        value = Encoding.UTF8;
                        endValue = utf8Count;
                    }
                    if (big5Count > endValue)
                    {
                        value = pub.Big5;
                        endValue = big5Count;
                    }
                    if (utf32Count > endValue)
                    {
                        value = Encoding.UTF32;
                        endValue = utf32Count;
                    }
                    //if (bigUtf32 > endValue)
                    //{
                    //    value = Encoding.UTF32;//大端
                    //    endValue = bigUtf32;
                    //}
                    if (utf16Count > endValue)
                    {
                        value = Encoding.Unicode;
                        endValue = utf16Count;
                    }
                    if (bigUtf16Count > endValue)
                    {
                        value = Encoding.BigEndianUnicode;
                        endValue = bigUtf16Count;
                    }
                    return value;
                }
            }
            /// <summary>
            /// 判断是否ASCII(包括结束字符)
            /// </summary>
            /// <returns></returns>
            private bool isAscii()
            {
                *end = 0xff;
                uint code = *(uint*)start;
                while ((code & 0x80808080) == 0) code = *(uint*)(start += 4);
                if (start == end) return (endValue & 0x80) == 0;
                else if ((code & 0x80) == 0)
                {
                    if (++start == end) return (endValue & 0x80) == 0;
                    else if ((code & 0x8000) == 0)
                    {
                        if (++start == end) return (endValue & 0x80) == 0;
                        else if ((code & 0x800000) == 0)
                        {
                            if (++start == end) return (endValue & 0x80) == 0;
                        }
                    }
                }
                return false;
            }
            /// <summary>
            /// 扫描ASCII(不包括结束字符)
            /// </summary>
            private void ascii()
            {
                *end = 0x80;
                uint code = *(uint*)start;
                while ((code & 0x80808080) == 0) code = *(uint*)(start += 4);
                if (start != end && (code & 0x80) == 0
                    && ++start != end && (code & 0x8000) == 0
                    && ++start != end && (code & 0x800000) == 0)
                {
                    ++start;
                }
            }
            /// <summary>
            /// 匹配GB2312
            /// </summary>
            private void gb2312()
            {
                uint code;
                gb2312Count = 0;
                do
                {
                    *end = 0;
                    for (code = *(uint*)start; (code & 0x80808080) == 0x80808080; code = *(uint*)(start += 4))
                    {
                        if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                        {
                            if (gb18030Char[(ushort)code & 0x3f] == (ushort)code) ++gb2312Count;
                            else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                                || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                            {
                                gb2312Count = int.MinValue;
                                return;
                            }
                        }
                        code >>= 16;
                        if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                        {
                            if (gb18030Char[code & 0x3f] == code) ++gb2312Count;
                            else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                                || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                            {
                                gb2312Count = int.MinValue;
                                return;
                            }
                        }
                    }
                    if ((code & 0x8080) == 0x8080)
                    {
                        if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                        {
                            if (gb18030Char[(ushort)code & 0x3f] == (ushort)code) ++gb2312Count;
                            else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                                || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                            {
                                gb2312Count = int.MinValue;
                                return;
                            }
                        }
                        code >>= 16;
                        start += 2;
                    }
                    if (start != end)
                    {
                        if ((code & 0x80) == 0)
                        {
                            if (++start != end)
                            {
                                if ((code & 0x8000) == 0)
                                {
                                    ++start;
                                    ascii();
                                    if (start == end)
                                    {
                                        if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                                return;
                            }
                        }
                        else
                        {
                            gb2312Count = int.MinValue;
                            return;
                        }
                    }
                    else
                    {
                        if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                        return;
                    }
                }
                while (true);
            }
            /// <summary>
            /// 匹配GB18030
            /// </summary>
            private void gb18030()
            {
                uint code;
                start = noAscii;
                gb18030Count = 0;
                while (start != end)
                {
                    code = *(ushort*)start;
                    if ((code & 0x80) != 0)
                    {
                        if ((byte)(code - 0x81) <= (0xfe - 0x81) && (ushort)(code - 0x3000) < (0xfe00 - 0x3000 + 0x100))
                        {
                            if (code >= 0x4000)
                            {
                                if ((code & 0xff00) != 0x7f00 && ++start != end)
                                {
                                    ++start;
                                    if (gb18030Char[code & 0x3f] == code) ++gb18030Count;
                                    continue;
                                }
                            }
                            else if (code < 0x3a00 && ++start != end && ++start != end)
                            {
                                code = *(ushort*)start;
                                if ((byte)(code - 0x81) <= (0xfe - 0x81)
                                    && (ushort)(code - 0x3000) < (0x3900 - 0x3000 + 0x100) && ++start != end)
                                {
                                    ++start;
                                    continue;
                                }
                            }
                        }
                        gb18030Count = int.MinValue;
                        return;
                    }
                    else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                    {
                        --end;
                        ascii();
                        if (start == end)
                        {
                            ++end;
                            if ((endValue & 0x80) != 0) gb18030Count = int.MinValue;
                            return;
                        }
                        else *end++ = (byte)endValue;
                    }
                }
            }
            /// <summary>
            /// 匹配GBK
            /// </summary>
            private void gbk()
            {
                uint code;
                start = noAscii;
                gbkCount = 0;
                while (start != end)
                {
                    code = *(ushort*)start;
                    if ((code & 0x80) != 0)
                    {
                        if (code >= 0xa100)
                        {
                            if (code <= 0xfe00)
                            {
                                if ((byte)code >= 0xb0 ? (byte)code <= 0xf7 : (byte)(code - 0x81) <= (0xa9 - 0x81))
                                {
                                    if (++start != end)
                                    {
                                        ++start;
                                        if (gb18030Char[code & 0x3f] == code) ++gbkCount;
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (code >= 0x4000 && (code & 0xff00) != 0x7f00)
                        {
                            if ((byte)code >= 0xa8 ? (byte)code <= 0xfe : (byte)(code - 0x81) <= (0xa0 - 0x81))
                            {
                                if (++start != end)
                                {
                                    ++start;
                                    continue;
                                }
                            }
                        }
                        gbkCount = int.MinValue;
                        return;
                    }
                    else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                    {
                        --end;
                        ascii();
                        if (start == end)
                        {
                            ++end;
                            if ((endValue & 0x80) != 0) gbkCount = int.MinValue;
                            return;
                        }
                        else *end++ = (byte)endValue;
                    }
                }
            }
            /// <summary>
            /// 匹配UTF8
            /// </summary>
            private void utf8()
            {
                uint* utf8Char = stackalloc uint[0x10];
                unsafer.memory.Fill((byte*)utf8Char, 0U, 0x10);
                utf8Char[0] = uint.MaxValue;
                utf8Char[(0x849ae7 >> 16) & 0xf] = 0x849ae7;
                utf8Char[(0xaf98e6 >> 16) & 0xf] = 0xaf98e6;
                utf8Char[(0x8db8e4 >> 16) & 0xf] = 0x8db8e4;
                utf8Char[(0x86bae4 >> 16) & 0xf] = 0x86bae4;
                utf8Char[(0xaab8e4 >> 16) & 0xf] = 0xaab8e4;
                utf8Char[(0x8cbcef >> 16) & 0xf] = 0x8cbcef;
                utf8Char[(0x8280e3 >> 16) & 0xf] = 0x8280e3;
                utf8Char[(0x8b80e5 >> 16) & 0xf] = 0x8b80e5;
                uint code, code1;
                start = noAscii;
                utf8Count = 0;
                do
                {
                    *end = 0;
                    for (code = *(uint*)start; (code & 0x80808080) == 0x80808080; )
                    {
                        code1 = code;
                        if ((code &= 0xe0) >= 0xe0)
                        {
                            if (code == 0xe0)
                            {
                                if ((code1 & 0xc0c000) == 0x808000)
                                {
                                    code = *(uint*)(start += 3);
                                    if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                    continue;
                                }
                            }
                            else if ((code1 & 0xc0c0c000U) == 0x80808000U)
                            {
                                code = *(uint*)(start += 4);
                                continue;
                            }
                        }
                        else if (code == 0xc0 && (code1 & 0xc000) == 0x8000)
                        {
                            if ((code1 & 0xe00000) == 0xc00000)
                            {
                                if ((code1 & 0xc0000000U) == 0x80000000U)
                                {
                                    code = *(uint*)(start += 4);
                                    continue;
                                }
                            }
                            else
                            {
                                code = *(uint*)(start += 2);
                                continue;
                            }
                        }
                        utf8Count = int.MinValue;
                        return;
                    }
                    while ((code & 0x80) != 0)
                    {
                        code1 = code;
                        if ((code &= 0xe0) >= 0xe0)
                        {
                            if (code == 0xe0)
                            {
                                if ((code1 & 0xc0c000) == 0x808000)
                                {
                                    start += 3;
                                    if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                    break;
                                }
                                else if ((code1 & 0xffc000) == 0x8000 && (start + 2) == end && (endValue & 0xc0) == 0x80)
                                {
                                    code1 |= (uint)endValue << 16;
                                    if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                    return;
                                }
                            }
                            else if ((code1 & 0xc0c0c000U) == 0x80808000U)
                            {
                                start += 4;
                                break;
                            }
                            else if ((code1 & 0xffc0c000U) == 0x808000 && (start + 3) == end && (endValue & 0xc0) == 0x80)
                            {
                                return;
                            }
                        }
                        else if (code == 0xc0)
                        {
                            if ((code1 & 0xc000) == 0x8000)
                            {
                                start += 2;
                                if ((code1 & 0x800000) == 0) break;
                                if ((code1 & 0xe00000) == 0xc00000 && ++start == end && (endValue & 0xc0) == 0x80)
                                {
                                    return;
                                }
                            }
                            else if (++start == end && (endValue & 0xc0) == 0x80) return;
                        }
                        utf8Count = int.MinValue;
                        return;
                    }
                    ascii();
                    if (start == end)
                    {
                        if ((endValue & 0x80) != 0) utf8Count = int.MinValue;
                        return;
                    }
                }
                while (true);
            }
            /// <summary>
            /// 匹配BIG5
            /// </summary>
            private void big5()
            {
                char* big5Char = stackalloc char[0x40];
                unsafer.memory.Fill((byte*)big5Char, 0U, 0x40 * sizeof(char) / sizeof(uint));
                big5Char[0] = char.MaxValue;
                big5Char[(0xbaaa >> 8) & 0x3f] = (char)0xbaaa;
                big5Char[(0x4fac >> 8) & 0x3f] = (char)0x4fac;
                big5Char[(0xa3a4 >> 8) & 0x3f] = (char)0xa3a4;
                big5Char[(0x46a4 >> 8) & 0x3f] = (char)0x46a4;
                big5Char[(0xd3ad >> 8) & 0x3f] = (char)0xd3ad;
                big5Char[(0x41a1 >> 8) & 0x3f] = (char)0x41a1;
                big5Char[(0x43a1 >> 8) & 0x3f] = (char)0x43a1;
                uint code;
                start = noAscii;
                big5Count = 0;
                while (start != end)
                {
                    code = *(ushort*)start;
                    if ((code & 0x80) != 0)
                    {
                        if ((byte)(code - 0x81) <= (0xfe - 0x81)
                            && (ushort)(code - 0x4000) < (0xfe00 - 0x4000 + 0x100) && (code & 0xff00) != 0x7f00)
                        {
                            if ((byte)code >= 0xa4)
                            {
                                if ((byte)code <= 0xc6)
                                {
                                    if (((byte)code != 0xc6 || (code & 0x8000) == 0) && ++start != end)
                                    {
                                        ++start;
                                        if (big5Char[(code >> 8) & 0x3f] == code) ++big5Count;
                                        continue;
                                    }
                                }
                                else if ((byte)(code - 0xc9) <= (0xf9 - 0xc9) && ++start != end)
                                {
                                    ++start;
                                    continue;
                                }
                            }
                            else if ((byte)code >= 0xa1)
                            {
                                if (((byte)code != 0xa3 || (code & 0xc000) == 0) && ++start != end)
                                {
                                    ++start;
                                    if (big5Char[(code >> 8) & 0x3f] == code) ++big5Count;
                                    continue;
                                }
                            }
                        }
                        big5Count = int.MinValue;
                        return;
                    }
                    else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                    {
                        --end;
                        ascii();
                        if (start == end)
                        {
                            ++end;
                            if ((endValue & 0x80) != 0) big5Count = int.MinValue;
                            return;
                        }
                        else *end++ = (byte)endValue;
                    }
                }
            }
            /// <summary>
            /// 匹配UTF16
            /// </summary>
            private void utf16()
            {
                while (start != end)
                {
                    uint code = *(uint*)start;
                    if (utf16Char[code & 0xf] == (char)code) ++utf16Count;
                    else if (bigUtf16Char[(code >> 8) & 0xf] == (char)code) ++bigUtf16Count;
                    code >>= 16;
                    if (utf16Char[code & 0xf] == code) ++utf16Count;
                    else if (bigUtf16Char[(code >> 8) & 0xf] == code) ++bigUtf16Count;
                    start += 4;
                }
            }
            /// <summary>
            /// 匹配UTF32
            /// </summary>
            private void utf32()
            {
                uint* utf32Char = stackalloc uint[0x10];
                unsafer.memory.Fill((byte*)utf32Char, 0U, 0x10);
                utf32Char[0] = uint.MaxValue;
                utf32Char[0x7684 & 0xf] = 0x7684;
                utf32Char[0x662f & 0xf] = 0x662f;
                utf32Char[0x4e0d & 0xf] = 0x4e0d;
                utf32Char[0x4e86 & 0xf] = 0x4e86;
                utf32Char[0x4e2a & 0xf] = 0x4e2a;
                utf32Char[0xff0c & 0xf] = 0xff0c;
                utf32Char[0x3002 & 0xf] = 0x3002;
                utf32Char[0x500b & 0xf] = 0x500b;
                //uint* bigUtf32Char = stackalloc uint[0x10];
                //showjim.sys.memory.set((byte*)bigUtf32Char, 0U, 0x10);
                //bigUtf32Char[0] = uint.MaxValue;
                //bigUtf32Char[(0x84760000 >> 24) & 0xf] = 0x84760000;
                //bigUtf32Char[(0x2f660000 >> 24) & 0xf] = 0x2f660000;
                //bigUtf32Char[(0x0d4e0000 >> 24) & 0xf] = 0x0d4e0000;
                //bigUtf32Char[(0x864e0000 >> 24) & 0xf] = 0x864e0000;
                //bigUtf32Char[(0x2a4e0000 >> 24) & 0xf] = 0x2a4e0000;
                //bigUtf32Char[(0x0cff0000 >> 24) & 0xf] = 0x0cff0000;
                //bigUtf32Char[(0x02300000 >> 24) & 0xf] = 0x02300000;
                //bigUtf32Char[(0x0b500000 >> 24) & 0xf] = 0x0b500000;
                while (start != end)
                {
                    uint code = *(uint*)start;
                    if (utf32Char[code & 0xf] == code) ++utf32Count;
                    //else if (bigUtf32Char[(code >> 24) & 0xf] == code) ++bigUtf32;
                    start += 4;
                }
            }
        }
        /// <summary>
        /// 汉字编码检测
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>编码,失败为null</returns>
        public static Encoding chineseEncoding(this byte[] data)
        {
            return data != null ? (data.Length != 0 ? new chineseEncoder().Get(data) : Encoding.ASCII) : null;
        }
        /// <summary>
        /// 字节流转字符串
        /// </summary>
        /// <param name="data">字节流</param>
        /// <param name="encoding">编码,检测失败为本地编码</param>
        /// <returns>字符串</returns>
        public static string toString(this byte[] data, Encoding encoding)
        {
            if (data.length() != 0)
            {
                if (encoding == null) encoding = chineseEncoding(data) ?? Encoding.Default;
                if (encoding == Encoding.ASCII) return data.deSerialize();
                else return encoding.GetString(data);
            }
            return data != null ? string.Empty : null;
        }
        #endregion

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>null为0</returns>
        public static int length(this string value)
        {
            return value != null ? value.Length : 0;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* Find(char* start, char* end, char value)
        {
            return start != null && end > start ? unsafer.String.Find(start, end, value) : null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="valueMap">字符集合</param>
        /// <param name="value">一个合法字符</param>
        /// <returns>字符位置,失败为null</returns>
        public unsafe static char* Find(char* start, char* end, asciiMap valueMap, char value)
        {
            return start != null && end > start ? unsafer.String.Find(start, end, valueMap, value) : null;
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        public unsafe static string joinString(this IEnumerable<string> values, char join)
        {
            if (values != null)
            {
                int length = 0, count = 0;
                foreach (string value in values)
                {
                    ++count;
                    if (value != null) length += value.Length;
                }
                if (count > 1)
                {
                    string newValue = new string(join, length + count - 1);
                    fixed (char* valueFixed = newValue)
                    {
                        char* write = valueFixed;
                        foreach (string value in values)
                        {
                            if (value != null)
                            {
                                unsafer.String.Copy(value, write);
                                write += value.Length;
                            }
                            ++write;
                        }
                    }
                    return newValue;
                }
                else if (count == 1)
                {
                    foreach (string value in values) return value ?? string.Empty;
                }
                return string.Empty;
            }
            return null;
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        public static string joinString(this IEnumerable<string> values, string join)
        {
            return values != null ? string.Join(join, values.getArray()) : null;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        public static list<subString> split(this string value, char split)
        {
            return value.split(0, value.length(), split);
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">分割字符串长度</param>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        public unsafe static list<subString> split(this string value, int startIndex, int length, char split)
        {
            array.range range = new array.range(value.length(), startIndex, length);
            if (range.GetCount != length) fastCSharp.log.Default.Throw(log.exceptionType.IndexOutOfRange);
            list<subString> values = new list<subString>();
            if (value != null)
            {
                fixed (char* valueFixed = value)
                {
                    char* last = valueFixed + range.SkipCount, end = last + range.GetCount;
                    for (char* start = last; start != end; )
                    {
                        if (*start == split)
                        {
                            values.Add(new subString(value, (int)(last - valueFixed), (int)(start - last)));
                            last = ++start;
                        }
                        else ++start;
                    }
                    values.Add(new subString(value, (int)(last - valueFixed), (int)(end - last)));
                }
            }
            return values;
        }
        /// <summary>
        /// 分割字符串并返回数值集合(不检查数字合法性)
        /// </summary>
        /// <param name="ints">原字符串</param>
        /// <param name="split">分割符</param>
        /// <returns>数值集合</returns>
        public unsafe static list<int> splitIntNoCheck(this string ints, char split)
        {
            list<int> values = new list<int>();
            if (ints != null && ints.Length != 0)
            {
                fixed (char* intPoint = ints)
                {
                    int intValue = 0;
                    for (char* next = intPoint, end = intPoint + ints.Length; next != end; ++next)
                    {
                        if (*next == split)
                        {
                            values.Add(intValue);
                            intValue = 0;
                        }
                        else
                        {
                            intValue *= 10;
                            intValue += *next;
                            intValue -= '0';
                        }
                    }
                    values.Add(intValue);
                }
            }
            return values;
        }
        /// <summary>
        /// 获取字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <returns>原始字节流</returns>
        public unsafe static byte[] serializeNotNull(this string value)
        {
            return value.Length != 0 ? unsafer.String.Serialize(value) : nullValue<byte>.Array;
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字节流</returns>
        public unsafe static byte[] getBytes(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value) return unsafer.String.GetBytes(valueFixed, value.Length);
            }
            return null;
        }
        /// <summary>
        /// 获取字符串原始字节流
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>字节流</returns>
        public static byte[] getBytes(this string value, Encoding encoding)
        {
            return encoding != Encoding.ASCII ? encoding.GetBytes(value) : value.getBytes();
        }
        /// <summary>
        /// 根据原始字节流生成字符串
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <returns>字符串</returns>
        public unsafe static string deSerialize(this byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    fixed (byte* dataFixed = data) return DeSerialize(dataFixed, -data.Length);
                }
                return string.Empty;
            }
            return null;
        }
        /// <summary>
        /// 根据原始字节流生成字符串
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">字符串长度</param>
        /// <returns>字符串</returns>
        public unsafe static string DeSerialize(byte* data, int length)
        {
            if (length >= 0)
            {
                return length != 0 ? new string((char*)data, 0, length >> 1) : string.Empty;
            }
            else
            {
                string value = new string((char)0, length = -length);
                fixed (char* valueFixed = value)
                {
                    char* start = valueFixed;
                    for (byte* end = data + length; data != end; *start++ = (char)*data++) ;
                }
                return value;
            }
        }
        /// <summary>
        /// 字节流转字符串
        /// </summary>
        /// <param name="data">字节流</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">字节数量</param>
        /// <returns>字符串</returns>
        public unsafe static string deSerialize(this byte[] data, int index, int length)
        {
            return toString(data, index, length, Encoding.ASCII);
        }
        /// <summary>
        /// 字节流转字符串
        /// </summary>
        /// <param name="data">字节流</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">字节数量</param>
        /// <param name="encoding">编码,检测失败为本地编码</param>
        /// <returns>字符串</returns>
        public unsafe static string toString(this byte[] data, int index, int length, Encoding encoding)
        {
            if (encoding != Encoding.ASCII) return encoding.GetString(data, index, length);
            array.range range = new array.range(data.length(), index, length);
            if (range.GetCount == length)
            {
                if (range.GetCount != 0)
                {
                    fixed (byte* dataFixed = data) return DeSerialize(dataFixed + range.SkipCount, -range.GetCount);
                }
                return string.Empty;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        public unsafe static string toLower(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value) unsafer.String.ToLower(valueFixed, valueFixed + value.Length);
            }
            return value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        public unsafe static void ToLower(char* start, char* end)
        {
            if (start != null && end > start) unsafer.String.ToLower(start, end);
        }
        /// <summary>
        /// 小写转大写
        /// </summary>
        public unsafe static string toUpper(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value) unsafer.String.ToUpper(valueFixed, valueFixed + value.Length);
            }
            return value;
        }
        /// <summary>
        /// 小写转大写
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        public unsafe static void ToUpper(char* start, char* end)
        {
            if (start != null && end > start) unsafer.String.ToUpper(start, end);
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        /// <returns>是否相等</returns>
        public unsafe static bool equalCase(this string left, string right, int count)
        {
            if (left != null)
            {
                if (right != null)
                {
                    int leftLength = left.Length, rightLength = right.Length;
                    if (leftLength > count) leftLength = count;
                    if (rightLength > count) rightLength = count;
                    if (leftLength == rightLength && count >= 0)
                    {
                        fixed (char* leftFixed = left, rightFixed = right) return unsafer.String.EqualCase(leftFixed, rightFixed, count);
                    }
                }
                return false;
            }
            return right == null;
        }
        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>比较结果,负数left小于right,正数left大于right</returns>
        public static unsafe int cmp(this string left, string right)
        {
            if (left != null && right != null)
            {
                int length = left.Length <= right.Length ? left.Length : right.Length;
                for (int index = 0, endIndex = Math.Min(length, 4); index != endIndex; ++index)
                {
                    int value = left[index] - right[index];
                    if (value != 0) return value;
                }
                if ((length -= 4) > 0)
                {
                    fixed (char* leftFixed = left, rightFixed = right)
                    {
                        int value = Cmp(leftFixed + 4, rightFixed + 4, length);
                        if (value != 0) return value;
                    }
                }
                return left.Length - right.Length;
            }
            if (left == right) return 0;
            return left != null ? 1 : -1;
        }
        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>比较结果,负数left小于right,正数left大于right</returns>
        private static unsafe int Cmp(char* left, char* right, int length)
        {
            while (length >= 8)
            {
                if (((*(uint*)left ^ *(uint*)right) | (*(uint*)(left + 4) ^ *(uint*)(right + 4))
                    | (*(uint*)(left + 8) ^ *(uint*)(right + 8)) | (*(uint*)(left + 12) ^ *(uint*)(right + 12))) != 0)
                {
                    if (((*(uint*)left ^ *(uint*)right) | (*(uint*)(left + 4) ^ *(uint*)(right + 4))) == 0)
                    {
                        left += 8;
                        right += 8;
                    }
                    if (*(uint*)left == *(uint*)right)
                    {
                        left += 4;
                        right += 4;
                    }
                    int value = (int)*(ushort*)left - *(ushort*)right;
                    return value == 0 ? (int)*(ushort*)(left += 2) - *(ushort*)(right += 2) : value;
                }
                length -= 8;
                left += 16;
                right += 16;
            }
            if ((length & 4) != 0)
            {
                if (((*(uint*)left ^ *(uint*)right) | (*(uint*)(left + 4) ^ *(uint*)(right + 4))) != 0)
                {
                    if ((*(uint*)left ^ *(uint*)right) == 0)
                    {
                        left += 4;
                        right += 4;
                    }
                    int value = (int)*(ushort*)left - *(ushort*)right;
                    return value == 0 ? (int)*(ushort*)(left += 2) - *(ushort*)(right += 2) : value;
                }
                left += 8;
                right += 8;
            }
            if ((length & 2) != 0)
            {
                int code = (int)*(ushort*)left - *(ushort*)right;
                if (code != 0) return code;
                code = (int)*(ushort*)(left + 2) - *(ushort*)(right + 2);
                if (code != 0) return code;
                left += 4;
                right += 4;
            }
            return (length & 1) == 0 ? 0 : ((int)*(ushort*)left - *(ushort*)right);
        }
        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="oldChar">原字符</param>
        /// <param name="newChar">目标字符</param>
        /// <returns>字符串</returns>
        public static string replace(this string value, char oldChar, char newChar)
        {
            return value != null && value.Length != 0 ? unsafer.String.Replace(value, oldChar, newChar) : null;
        }
        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="oldChar">原字符</param>
        /// <param name="newChar">目标字符</param>
        /// <returns>字符串</returns>
        public unsafe static string replaceNotNull(this string value, char oldChar, char newChar)
        {
            return value.Length != 0 ? unsafer.String.Replace(value, oldChar, newChar) : value;
        }
        /// <summary>
        /// 去除前后空格
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>是否有效数据</returns>
        public static bool Trim(ref string value)
        {
            return value != null && (value = value.Trim()).Length != 0;
        }
        /// <summary>
        /// 取左侧显示字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="count">显示字符串数量</param>
        /// <returns>左侧显示字符串</returns>
        public unsafe static string getLeft(this string value, int count)
        {
            if (count <= 0) return string.Empty;
            if (value.length() << 1 > count)
            {
                fixed (char* valueFixed = value)
                {
                    char* start = valueFixed;
                    for (char* end = valueFixed + value.Length; start != end; ++start)
                    {
                        if (*start > 0xff)
                        {
                            if ((count -= 2) <= 0)
                            {
                                if (count == 0) ++start;
                                break;
                            }
                        }
                        else if (--count == 0)
                        {
                            ++start;
                            break;
                        }
                    }
                    count = (int)(start - valueFixed);
                }
                return count != value.Length ? value.Substring(0, count) : value;
            }
            return value;
        }
        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>转换后的字符串</returns>
        public unsafe static string toHalf(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value)
                {
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        int code = *start;
                        if ((uint)(code - 0xff01) <= 0xff5e - 0xff01) *start = (char)(code - 0xfee0);
                        else
                        {
                            switch (code)
                            {
                                case 0x2019:
                                case 0x2018:
                                    *start = '\''; break;
                                case 0x201c:
                                case 0x201d:
                                    *start = '"'; break;
                                case 0x3002:
                                    *start = '.'; break;
                                case 0xb7:
                                    *start = '@'; break;
                                //default:
                                //    if ((uint)(code - 0x2160) < 9) *start = (char)(code - 0x212f);
                                //    break;
                            }
                        }
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 正则转义字符集合
        /// </summary>
        public const string RegexEscape = @"^[-]{}()\|/?*+.$";
        /// <summary>
        /// 正则转义字符位图
        /// </summary>
        private static readonly pointer regexEscapeMap = new asciiMap(unmanaged.Get(String.asciiMap.mapBytes), RegexEscape, true).Pointer;
        /// <summary>
        /// 将字符串转换成正则代达式
        /// </summary>
        /// <param name="value">待转换成与正则表达式的字符串</param>
        /// <returns>转换后的正则表达式</returns>
        public unsafe static string toRegex(this string value)
        {
            if (value == null || value.Length == 0)
            {
                fixed (char* valueFixed = value)
                {
                    char* end = valueFixed + value.Length;
                    int count = unsafer.String.asciiCount(valueFixed, end, regexEscapeMap.Byte, RegexEscape[0]);
                    if (count != 0)
                    {
                        fixedMap map = new fixedMap(regexEscapeMap);
                        value = new string((char)0, count += value.Length);
                        fixed (char* writeFixed = value)
                        {
                            for (char* start = valueFixed, write = writeFixed; start != end; *write++ = *start++)
                            {
                                if ((*start & 0xff80) == 0 && map.Get(*start)) *write++ = '\\';
                            }
                        }
                    }
                }
            }
            return value;
        }
        ///// <summary>
        ///// 格式化逗号分隔值
        ///// </summary>
        ///// <param name="value">原始值</param>
        ///// <returns>格式化后的逗号分隔值</returns>
        //public static string toCsv(this string value)
        //{
        //    return value == null ? string.Empty
        //        : (value.IndexOf(',') == -1 ? value : (@"""" + value.Replace(@"""", @"""""") + @""""));
        //}
        /// <summary>
        /// 汉语拼音转英文字母
        /// </summary>
        /// <param name="value">汉语拼音</param>
        /// <returns>英文字母</returns>
        public static unsafe string pinyinToLetter(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value)
                {
                    for (char* start = valueFixed, end = valueFixed + value.Length, pinyinFixed = pinyins.Char; start != end; ++start)
                    {
                        if ((uint)(*start - 224) <= (476 - 224)) *start = pinyinFixed[*start - 224];
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 汉语拼音
        /// </summary>
        private static readonly pointer pinyins;
        static String()
        {
            pinyins = unmanaged.Get((476 - 224 + 1) * sizeof(char));
            char* pinyinData = pinyins.Char;
            pinyinData['ā' - 224] = 'a';
            pinyinData['á' - 224] = 'a';
            pinyinData['ǎ' - 224] = 'a';
            pinyinData['à' - 224] = 'a';
            pinyinData['ē' - 224] = 'e';
            pinyinData['é' - 224] = 'e';
            pinyinData['ě' - 224] = 'e';
            pinyinData['è' - 224] = 'e';
            pinyinData['ī' - 224] = 'i';
            pinyinData['í' - 224] = 'i';
            pinyinData['ǐ' - 224] = 'i';
            pinyinData['ì' - 224] = 'i';
            pinyinData['ō' - 224] = 'o';
            pinyinData['ó' - 224] = 'o';
            pinyinData['ǒ' - 224] = 'o';
            pinyinData['ò' - 224] = 'o';
            pinyinData['ū' - 224] = 'u';
            pinyinData['ú' - 224] = 'u';
            pinyinData['ǔ' - 224] = 'u';
            pinyinData['ù' - 224] = 'u';
            pinyinData['ǘ' - 224] = 'v';
            pinyinData['ǚ' - 224] = 'v';
            pinyinData['ǜ' - 224] = 'v';
        }
    }
}
