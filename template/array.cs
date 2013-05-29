using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;DateTime*/

namespace fastCSharp
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="values">数据指针</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为null</returns>
        private unsafe static /*Type[0]*/ulong/*Type[0]*/* IndexOf
            (/*Type[0]*/ulong/*Type[0]*/* valueFixed, int length, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + length; start != end; ++start)
            {
                if (*start == value) return start;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array != null)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, value);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int length, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            array.range range = new array.range(array.length(), index, length);
            if ((length = range.GetCount) != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed + range.SkipCount, length, value);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getReverse(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            return array.length() != 0 ? GetReverse(array, 0, array.Length) : array.notNull();
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        /// <returns>翻转后的新数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getReverse(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int length)
        {
            array.range range = new array.range(array.length(), index, length);
            if (((range.SkipCount ^ index) | (range.GetCount ^ length)) == 0)
            {
                return GetReverse(array, index, length);
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        /// <returns>翻转后的新数组</returns>
        private unsafe static /*Type[0]*/ulong/*Type[0]*/[] GetReverse(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int length)
        {
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array, newValueFixed = newValues)
            {
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + length, wirte = newValueFixed + length;
                    start != end; 
                    *--wirte = *start++) ;
            }
            return newValues;
        }
        /// <summary>
        /// 移除第一个匹配数据数据
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配值</param>
        /// <returns>移除数据后的数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] removeFirst
            (this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array != null)
            {
                int index = -1;
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, value);
                    if (valueIndex != null) index = (int)(valueIndex - valueFixed);
                }
                if (index != -1) return GetRemoveAt(array, index);
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        public unsafe static void move(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int writeIndex, int count)
        {
            if (count > 0)
            {
                int writeEndIndex = writeIndex + count;
                if (index >= 0 && writeEndIndex <= array.Length)
                {
                    int endIndex = index + count;
                    if (index < writeIndex && endIndex > writeIndex)
                    {
                        fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                        {
                            for (/*Type[0]*/ulong/*Type[0]*/* write = valueFixed + writeEndIndex, end = valueFixed + endIndex, start = valueFixed + index;
                                end != start;
                                *--write = *--end) ;
                        }
                    }
                    else if (writeIndex >= 0 && endIndex <= array.Length) Array.Copy(array, index, array, writeIndex, count);
                    else log.Default.Throw(log.exceptionType.IndexOutOfRange);
                }
                else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
            else if (count != 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="values">数组数据</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为null</returns>
        private unsafe static /*Type[0]*/ulong/*Type[0]*/* IndexOf
            (/*Type[0]*/ulong/*Type[0]*/* valueFixed, int length, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + length; start != end; ++start)
            {
                if (isValue(*start)) return start;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf(this /*Type[0]*/ulong/*Type[0]*/[] array, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, isValue);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int length, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            array.range range = new array.range(array.length(), index, length);
            if ((length = range.GetCount) != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed + range.SkipCount, length, isValue);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="index">起始位置</param>
        /// <returns>第一个匹配值,失败为default(/*Type[0]*/ulong/*Type[0]*/)</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ firstOrDefault
            (this /*Type[0]*/ulong/*Type[0]*/[] array, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue, int index)
        {
            if (array != null && (uint)index < (uint)array.Length)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed + index, array.Length - index, isValue);
                    if (valueIndex != null) return *valueIndex;
                }
            }
            return default(/*Type[0]*/ulong/*Type[0]*/);
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public unsafe static int count
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                int value = 0;
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + count; start != end; ++start)
                    {
                        if (isValue(*start)) ++value;
                    }
                }
                return value;
            }
            return 0;
        }
        /// <summary>
        /// 移除第一个匹配数据数据
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>移除数据后的数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] removeFirst
            (this /*Type[0]*/ulong/*Type[0]*/[] array, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                int index = -1;
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, isValue);
                    if (valueIndex != null) index = (int)(valueIndex - valueFixed);
                }
                if (index != -1) return GetRemoveAt(array, index);
                return array;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">新值</param>
        /// <param name="isValue">数据匹配器</param>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] replaceFirst
            (this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ value, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, isValue);
                    if (valueIndex != null) *valueIndex = value;
                }
                return array;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 转换键值对集合
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="array">键值数组</param>
        /// <param name="values">数据数组</param>
        /// <returns>键值对数组</returns>
        public unsafe static keyValue</*Type[0]*/ulong/*Type[0]*/, valueType>[] getKeyValue<valueType>
            (this /*Type[0]*/ulong/*Type[0]*/[] array, valueType[] values)
        {
            int length = array.length();
            if (length != values.length()) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            if (length != 0)
            {
                keyValue</*Type[0]*/ulong/*Type[0]*/, valueType>[] newValues = new keyValue</*Type[0]*/ulong/*Type[0]*/, valueType>[values.Length];
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* readValue = valueFixed;
                    int index = 0;
                    foreach (valueType value in values)
                    {
                        newValues[index].Set(*readValue++, value);
                        ++index;
                    }
                }
                return newValues;
            }
            return nullValue<keyValue</*Type[0]*/ulong/*Type[0]*/, valueType>>.Array;
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="valueType">数组类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (array.length() != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[array.Length];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* writeValue = newValueFixed;
                    foreach (valueType value in array) *writeValue++ = getValue(value);
                }
                return newValues;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> getFind
            (this /*Type[0]*/ulong/*Type[0]*/[] array, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.length() != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[array.Length < sizeof(int) ? sizeof(int) : array.Length];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length; start != end; ++start)
                    {
                        if (isValue(*start)) *write++ = *start;
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> getFind
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count < sizeof(int) ? sizeof(int) : count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + count; start != end; ++start)
                    {
                        if (isValue(*start)) *write++ = *start;
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getFindArray
            (this /*Type[0]*/ulong/*Type[0]*/[] array, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int length = array.length();
            if (length != 0)
            {
                length = (length + 7) >> 3;
                if (length <= config.pub.Default.StreamBufferLength)
                {
                    byte* data = stackalloc byte[length];
                    return GetFindArray(array, 0, array.Length, isValue, new fixedMap(data, length));
                }
                else
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataFixed = data) return GetFindArray(array, 0, array.Length, isValue, new fixedMap(dataFixed));
                }
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getFindArray
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                int length = (count + 7) >> 3;
                if (length <= config.pub.Default.StreamBufferLength)
                {
                    byte* data = stackalloc byte[length];
                    return GetFindArray(array, range.SkipCount, count, isValue, new fixedMap(data, length));
                }
                else
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataFixed = data) return GetFindArray(array, range.SkipCount, count, isValue, new fixedMap(dataFixed));
                }
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private unsafe static /*Type[0]*/ulong/*Type[0]*/[] GetFindArray
            (/*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue, fixedMap map)
        {
            int length = 0, mapIndex = 0;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* startFixed = valueFixed + index, end = startFixed + count;
                for (/*Type[0]*/ulong/*Type[0]*/* start = startFixed; start != end; ++start, ++mapIndex)
                {
                    if (isValue(*start))
                    {
                        ++length;
                        map.Set(mapIndex);
                    }
                }
                if (length != 0)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length];
                    fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/* write = newValueFixed + length;
                        while (mapIndex != 0)
                        {
                            if (map.Get(--mapIndex)) *--write = startFixed[mapIndex];
                        }
                    }
                    return newValues;
                }
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <param name="getVlaue">目标数据获取器</param>
        /// <returns>子集合</returns>
        public unsafe static valueType[] getSub<valueType>
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, valueType> getVlaue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (getVlaue == null) log.Default.Throw(log.exceptionType.Null);
                valueType[] newValues = new valueType[count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* read = valueFixed + range.SkipCount;
                    for (index = 0; index != count; ++index) newValues[index] = getVlaue(*read++);
                }
                return newValues;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <param name="getVlaue">数据获取器</param>
        /// <returns>子集合</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getSub<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    for (index = range.SkipCount, count = range.EndIndex; index != count; ++index) *write++ = getValue(array[index]);
                }
                return newValues;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public unsafe static list<valueType> distinct<valueType>
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, func</*Type[0]*/ulong/*Type[0]*/, valueType> getValue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                list<valueType>.unsafer newValues = new list<valueType>(count).Unsafer;
                System.Collections.Generic.HashSet</*Type[0]*/ulong/*Type[0]*/> hash
                    = new System.Collections.Generic.HashSet</*Type[0]*/ulong/*Type[0]*/>();
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + range.SkipCount, end = start + count; start != end; ++start)
                    {
                        if (!hash.Contains(*start))
                        {
                            newValues.Add(getValue(*start));
                            hash.Add(*start);
                        }
                    }
                }
                return newValues.List;
            }
            return null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> distinct<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (array != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[array.Length];
                System.Collections.Generic.HashSet<valueType> hash = new System.Collections.Generic.HashSet<valueType>();
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    foreach (valueType value in array)
                    {
                        if (!hash.Contains(value))
                        {
                            *write++ = getValue(value);
                            hash.Add(value);
                        }
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> distinct<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
                System.Collections.Generic.HashSet<valueType> hash = new System.Collections.Generic.HashSet<valueType>();
                valueType value;
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    for (index = range.SkipCount, count = range.EndIndex; index != count; ++index)
                    {
                        if (!hash.Contains(value = array[index]))
                        {
                            *write++ = getValue(value);
                            hash.Add(value);
                        }
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 求交集
        /// </summary>
        /// <param name="left">左侧数据</param>
        /// <param name="right">右侧数据</param>
        /// <returns>数据交集</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> intersect(this /*Type[0]*/ulong/*Type[0]*/[] left, /*Type[0]*/ulong/*Type[0]*/[] right)
        {
            int leftLength = left.length();
            if (leftLength != 0)
            {
                int rightLength = right.length();
                if (rightLength != 0)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/[] min = leftLength <= rightLength ? left : right, values = new /*Type[0]*/ulong/*Type[0]*/[min.Length];
                    fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = values)
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/* write = valueFixed;
                        staticHashSet</*Type[0]*/ulong/*Type[0]*/> hash = new staticHashSet</*Type[0]*/ulong/*Type[0]*/>(min);
                        foreach (/*Type[0]*/ulong/*Type[0]*/ value in leftLength <= rightLength ? right : left)
                        {
                            if (hash.Contains(value)) *write++ = value;
                        }
                        return new list</*Type[0]*/ulong/*Type[0]*/>(values, 0, (int)(write - valueFixed), true);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">遍历数据数量</param>
        /// <param name="method">调用函数</param>
        /// <returns>数据数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] each
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, action</*Type[0]*/ulong/*Type[0]*/> method)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                if (method == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + count; start != end; method(*start++)) ;
                }
                return array;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public unsafe static bool max(this /*Type[0]*/ulong/*Type[0]*/[] array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.length() != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    value = *valueFixed;
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
                    {
                        if (*start > value) value = *start;
                    }
                    return true;
                }
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ max(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return max(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public unsafe static bool max(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* start = valueFixed + range.SkipCount;
                    value = *start;
                    for (/*Type[0]*/ulong/*Type[0]*/* end = start + count; ++start != end; )
                    {
                        if (*start > value) value = *start;
                    }
                    return true;
                }
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ max
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return max(array, index, count, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.length() != 0)
            {
                value = getKey(array[0]);
                foreach (valueType nextValue in array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextKey = getKey(nextValue);
                    if (nextKey > value) value = nextKey;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ maxKey<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return maxKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                value = getKey(array[index = range.SkipCount]);
                for (int endIndex = index + count; ++index != endIndex; )
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextValue = getKey(array[index]);
                    if (nextValue > value) value = nextValue;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ maxKey<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return maxKey(array, index, count, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (array.length() != 0)
            {
                value = array[0];
                /*Type[0]*/
                ulong/*Type[0]*/ maxKey = getKey(value);
                foreach (valueType nextValue in array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextKey = getKey(nextValue);
                    if (nextKey > maxKey)
                    {
                        maxKey = nextKey;
                        value = nextValue;
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return max(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                value = array[index = range.SkipCount];
                /*Type[0]*/
                ulong/*Type[0]*/ maxValue = getKey(value);
                for (int endIndex = index + count; ++index != endIndex; )
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextValue = getKey(array[index]);
                    if (nextValue > maxValue)
                    {
                        maxValue = nextValue;
                        value = array[index];
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return max(array, index, count, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public unsafe static bool min(this /*Type[0]*/ulong/*Type[0]*/[] array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.length() != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    value = *valueFixed;
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
                    {
                        if (*start < value) value = *start;
                    }
                    return true;
                }
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ min(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return min(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public unsafe static bool min(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* start = valueFixed + range.SkipCount;
                    value = *start;
                    for (/*Type[0]*/ulong/*Type[0]*/* end = start + count; ++start != end; )
                    {
                        if (*start < value) value = *start;
                    }
                    return true;
                }
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ min
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return min(array, index, count, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.length() != 0)
            {
                value = getKey(array[0]);
                foreach (valueType nextValue in array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextKey = getKey(nextValue);
                    if (nextKey < value) value = nextKey;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ minKey<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return minKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                value = getKey(array[index = range.SkipCount]);
                for (int endIndex = index + count; ++index != endIndex; )
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextValue = getKey(array[index]);
                    if (nextValue < value) value = nextValue;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ minKey<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return minKey(array, index, count, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (array.length() != 0)
            {
                value = array[0];
                /*Type[0]*/
                ulong/*Type[0]*/ minKey = getKey(value);
                foreach (valueType nextValue in array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextKey = getKey(nextValue);
                    if (nextKey < minKey)
                    {
                        minKey = nextKey;
                        value = nextValue;
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return min(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                value = array[index = range.SkipCount];
                /*Type[0]*/
                ulong/*Type[0]*/ minValue = getKey(value);
                for (int endIndex = index + count; ++index != endIndex; )
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ nextValue = getKey(array[index]);
                    if (nextValue < minValue)
                    {
                        minValue = nextValue;
                        value = array[index];
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>
            (this valueType[] array, int index, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return min(array, index, count, getKey, out value) ? value : nullValue;
        }
    }
}

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 数组非安全扩展操作(请自行确保数据可靠性)
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        public unsafe static void Move(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int writeIndex, int count)
        {
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* write = valueFixed + writeIndex + count, start = valueFixed + index, end = valueFixed + endIndex;
                        end != start;
                        *--write = *--end) ;
                }
            }
            else Array.Copy(array, index, array, writeIndex, count);
        }
    }
}