using System;
/*Type:ulong,ulongSortIndex;long,longSortIndex;uint,uintSortIndex;int,intSortIndex;ushort,ushortSortIndex;short,shortSortIndex;byte,byteSortIndex;sbyte,sbyteSortIndex;double,doubleSortIndex;float,floatSortIndex;DateTime,dateTimeSortIndex*/
/*Compare:,>,<;Desc,<,>*/

namespace fastCSharp.algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static partial class quickSort
    {
        /// <summary>
        /// 快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        private unsafe static void sort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/* startIndex, /*Type[0]*/ulong/*Type[0]*/* endIndex)
        {
            do
            {
                /*Type*/
                ulong/*Type*/ leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                /*Type*/
                ulong/*Type*/* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                /*Type*/
                ulong/*Type*/ value = *averageIndex;
                if (leftValue /*Compare[1]*/>/*Compare[1]*/ value)
                {
                    if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
                    {
                        *rightIndex = leftValue;
                        if (value /*Compare[1]*/>/*Compare[1]*/ rightValue) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = value;
                            *averageIndex = value = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = value;
                        *averageIndex = value = leftValue;
                    }
                }
                else
                {
                    if (value /*Compare[1]*/>/*Compare[1]*/ rightValue)
                    {
                        *rightIndex = value;
                        if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = value = leftValue;
                        }
                        else *averageIndex = value = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                do
                {
                    while (*leftIndex /*Compare[2]*/</*Compare[2]*/ value) ++leftIndex;
                    while (value /*Compare[2]*/</*Compare[2]*/ *rightIndex) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = *leftIndex;
                        *leftIndex = *rightIndex;
                        *rightIndex = leftValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            ++leftIndex;
                            --rightIndex;
                        }
                        break;
                    }
                }
                while (++leftIndex <= --rightIndex);
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) sort/*Compare[0]*//*Compare[0]*/(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) sort/*Compare[0]*//*Compare[0]*/(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        public unsafe static void Sort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values)
        {
            if (values != null && values.Length > 1)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = values)
                {
                    sort/*Compare[0]*//*Compare[0]*/(valueFixed, valueFixed + values.Length - 1);
                }
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] GetSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values)
        {
            if (values.length() != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValue = new /*Type[0]*/ulong/*Type[0]*/[values.Length];
                Buffer.BlockCopy(values, 0, newValue, 0, values.Length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValue, valueFixed = values)
                {
                    sort/*Compare[0]*//*Compare[0]*/(newValueFixed, newValueFixed + values.Length - 1);
                }
                return newValue;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        public unsafe static void Sort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values, int startIndex, int count)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if (range.GetCount > 1)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = values)
                {
                    sort/*Compare[0]*//*Compare[0]*/(valueFixed + range.SkipCount, valueFixed + range.EndIndex - 1);
                }
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] GetSort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/[] values, int startIndex, int count)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
                Buffer.BlockCopy(values, range.SkipCount * sizeof(/*Type[0]*/ulong/*Type[0]*/), newValues, 0, count * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = values)
                {
                    if (--count > 0) sort/*Compare[0]*//*Compare[0]*/(newValueFixed, newValueFixed + count);
                }
                return newValues;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        private unsafe static void sort/*Compare[0]*//*Compare[0]*/
            (/*Type[1]*/ulongSortIndex/*Type[1]*/* startIndex, /*Type[1]*/ulongSortIndex/*Type[1]*/* endIndex)
        {
            do
            {
                /*Type[1]*/
                ulongSortIndex/*Type[1]*/ leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                /*Type[1]*/
                ulongSortIndex/*Type[1]*/* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                /*Type[1]*/
                ulongSortIndex/*Type[1]*/ indexValue = *averageIndex;
                if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ indexValue.Value)
                {
                    if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = indexValue;
                            *averageIndex = indexValue = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = indexValue;
                        *averageIndex = indexValue = leftValue;
                    }
                }
                else
                {
                    if (indexValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                /*Type[0]*/
                ulong/*Type[0]*/ value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value /*Compare[2]*/</*Compare[2]*/ value) ++leftIndex;
                    while (value /*Compare[2]*/</*Compare[2]*/ (*rightIndex).Value) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = *leftIndex;
                        *leftIndex = *rightIndex;
                        *rightIndex = leftValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            ++leftIndex;
                            --rightIndex;
                        }
                        break;
                    }
                }
                while (++leftIndex <= --rightIndex);
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) sort/*Compare[0]*//*Compare[0]*/(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) sort/*Compare[0]*//*Compare[0]*/(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <returns>排序后的数组</returns>
        public unsafe static valueType[] GetSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            if (values.length() != 0)
            {
                if (values.Length > (config.pub.Default.StreamBufferLength >> 4))
                {
                    /*Type[1]*/
                    ulongSortIndex/*Type[1]*/[] indexs = new /*Type[1]*/ulongSortIndex/*Type[1]*/[values.Length];
                    fixed (/*Type[1]*/ulongSortIndex/*Type[1]*/* fixedIndex = indexs)
                    {
                        return getSort/*Compare[0]*//*Compare[0]*/(values, getKey, fixedIndex);
                    }
                }
                else
                {
                    /*Type[1]*/
                    ulongSortIndex/*Type[1]*/* fixedIndex = stackalloc /*Type[1]*/ulongSortIndex/*Type[1]*/[values.Length];
                    return getSort/*Compare[0]*//*Compare[0]*/(values, getKey, fixedIndex);
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private unsafe static valueType[] getSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[1]*/ulongSortIndex/*Type[1]*/* fixedIndex)
        {
            /*Type[1]*/
            ulongSortIndex/*Type[1]*/* writeIndex = fixedIndex;
            if (values.Length > 1)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                for (int index = 0; index != values.Length; ++index)
                {
                    *writeIndex++ = new /*Type[1]*/ulongSortIndex/*Type[1]*/ { Value = getKey(values[index]), Index = index };
                }
                sort/*Compare[0]*//*Compare[0]*/(writeIndex = fixedIndex, fixedIndex + values.Length - 1);
            }
            else *fixedIndex = new /*Type[1]*/ulongSortIndex/*Type[1]*/ { Index = 0 };
            valueType[] newValues = new valueType[values.Length];
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = values[(*writeIndex++).Index];
            return newValues;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public unsafe static valueType[] GetSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int startIndex, int count)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                if (count > (config.pub.Default.StreamBufferLength >> 4))
                {
                    /*Type[1]*/
                    ulongSortIndex/*Type[1]*/[] indexs = new /*Type[1]*/ulongSortIndex/*Type[1]*/[count];
                    fixed (/*Type[1]*/ulongSortIndex/*Type[1]*/* fixedIndex = indexs)
                    {
                        return getSort/*Compare[0]*//*Compare[0]*/(values, getKey, range.SkipCount, count, fixedIndex);
                    }
                }
                else
                {
                    /*Type[1]*/
                    ulongSortIndex/*Type[1]*/* fixedIndex = stackalloc /*Type[1]*/ulongSortIndex/*Type[1]*/[count];
                    return getSort/*Compare[0]*//*Compare[0]*/(values, getKey, range.SkipCount, count, fixedIndex);
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private unsafe static valueType[] getSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int startIndex, int count
            , /*Type[1]*/ulongSortIndex/*Type[1]*/* fixedIndex)
        {
            /*Type[1]*/
            ulongSortIndex/*Type[1]*/* writeIndex = fixedIndex;
            if (count > 1)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                for (int index = startIndex, endIndex = startIndex + count; index != endIndex; ++index)
                {
                    *writeIndex++ = new /*Type[1]*/ulongSortIndex/*Type[1]*/ { Value = getKey(values[index]), Index = index };
                }
                sort/*Compare[0]*//*Compare[0]*/(writeIndex = fixedIndex, fixedIndex + count - 1);
            }
            else *fixedIndex = new /*Type[1]*/ulongSortIndex/*Type[1]*/ { Index = startIndex };
            valueType[] newValues = new valueType[count];
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = values[(*writeIndex++).Index];
            return newValues;
        }
    }
}

namespace fastCSharp
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] sort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            algorithm.quickSort.Sort/*Compare[0]*//*Compare[0]*/(array);
            return array.notNull();
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(array);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] sort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count)
        {
            algorithm.quickSort.Sort/*Compare[0]*//*Compare[0]*/(array, startIndex, count);
            return array.notNull();
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的新数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count)
        {
            return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count);
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] sort/*Compare[0]*//*Compare[0]*/<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, getKey);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] sort/*Compare[0]*//*Compare[0]*/<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int startIndex, int count)
        {
            return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, getKey, startIndex, count);
        }
    }
}

namespace fastCSharp
{
    /// <summary>
    /// 单向动态数组扩展
    /// </summary>
    public static partial class list
    {
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <returns>排序后的单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> sort/*Compare[0]*//*Compare[0]*/(this list</*Type[0]*/ulong/*Type[0]*/> list)
        {
            if (list.count() != 0) algorithm.quickSort.Sort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, 0, list.Count);
            return list;
        }
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <returns>排序后新的单向动态数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/(this list</*Type[0]*/ulong/*Type[0]*/> list)
        {
            if (list.count() != 0)
            {
                return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, 0, list.Count);
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> sort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int startIndex, int count)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                algorithm.quickSort.Sort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, range.SkipCount, count);
            }
            return list;
        }
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后新的单向动态数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int startIndex, int count)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, range.SkipCount, count);
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] sort/*Compare[0]*//*Compare[0]*/<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            if (list.count() != 0)
            {
                return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, getKey, 0, list.Count);
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 单向动态数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] sort/*Compare[0]*//*Compare[0]*/<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int startIndex, int count)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                return algorithm.quickSort.GetSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, getKey, range.SkipCount, count);
            }
            return nullValue<valueType>.Array;
        }
    }
}

