using System;
/*Type:ulong,ulongRangeSorter,ulongSortIndex,ulongRangeIndexSorter;long,longRangeSorter,longSortIndex,longRangeIndexSorter;uint,uintRangeSorter,uintSortIndex,uintRangeIndexSorter;int,intRangeSorter,intSortIndex,intRangeIndexSorter;ushort,ushortRangeSorter,ushortSortIndex,ushortRangeIndexSorter;short,shortRangeSorter,shortSortIndex,shortRangeIndexSorter;byte,byteRangeSorter,byteSortIndex,byteRangeIndexSorter;sbyte,sbyteRangeSorter,sbyteSortIndex,sbyteRangeIndexSorter;double,doubleRangeSorter,doubleSortIndex,doubleRangeIndexSorter;float,floatRangeSorter,floatSortIndex,floatRangeIndexSorter;DateTime,dateTimeRangeSorter,dateTimeSortIndex,dateTimeRangeIndexSorter*/
/*Compare:,>,<;Desc,<,>*/

namespace fastCSharp.algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static partial class quickSort
    {
        /// <summary>
        /// 范围排序器(一般用于获取分页)
        /// </summary>
        private unsafe struct /*Type[1]*/ulongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public /*Type[0]*/ulong/*Type[0]*/* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public /*Type[0]*/ulong/*Type[0]*/* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(/*Type[0]*/ulong/*Type[0]*/* startIndex, /*Type[0]*/ulong/*Type[0]*/* endIndex)
            {
                do
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ leftValue = *startIndex, rightValue = *endIndex;
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
                    /*Type[0]*/
                    ulong/*Type[0]*/* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                    /*Type[0]*/
                    ulong/*Type[0]*/ value = *averageIndex;
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
                        if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                        if (leftIndex > GetEndIndex) break;
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                        if (rightIndex < SkipCount) break;
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public unsafe static collection</*Type[0]*/ulong/*Type[0]*/> RangeSort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/[] values, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = values)
                {
                    new /*Type[1]*/ulongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                    {
                        SkipCount = valueFixed + range.SkipCount,
                        GetEndIndex = valueFixed + range.EndIndex - 1
                    }.Sort(valueFixed, valueFixed + values.Length - 1);
                }
                return new collection</*Type[0]*/ulong/*Type[0]*/>(values, range.SkipCount, getCount, true);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        public unsafe static collection</*Type[0]*/ulong/*Type[0]*/> GetRangeSort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/[] values, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Length];
                Buffer.BlockCopy(values, 0, newValues, 0, values.Length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = values)
                {
                    new /*Type[1]*/ulongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                    {
                        SkipCount = newValueFixed + range.SkipCount,
                        GetEndIndex = newValueFixed + range.EndIndex - 1
                    }.Sort(newValueFixed, newValueFixed + values.Length - 1);
                }
                return new collection</*Type[0]*/ulong/*Type[0]*/>(newValues, range.SkipCount, getCount, true);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public unsafe static collection</*Type[0]*/ulong/*Type[0]*/> RangeSort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/[] values, int startIndex, int count, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                array.range getRange = new array.range(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    skipCount = range.SkipCount + getRange.SkipCount;
                    fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = values)
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/* skip = valueFixed + skipCount, start = valueFixed + range.SkipCount;
                        new /*Type[1]*/ulongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                        {
                            SkipCount = skip,
                            GetEndIndex = skip + getCount - 1
                        }.Sort(start, start + --count);
                    }
                    return new collection</*Type[0]*/ulong/*Type[0]*/>(values, skipCount, getCount, true);
                }
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        public unsafe static collection</*Type[0]*/ulong/*Type[0]*/> GetRangeSort/*Compare[0]*//*Compare[0]*/
            (/*Type[0]*/ulong/*Type[0]*/[] values, int startIndex, int count, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                array.range getRange = new array.range(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
                    Buffer.BlockCopy(values, range.SkipCount * sizeof(/*Type[0]*/ulong/*Type[0]*/), newValues, 0, count * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                    fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = values)
                    {
                        new /*Type[1]*/ulongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                        {
                            SkipCount = newValueFixed + getRange.SkipCount,
                            GetEndIndex = newValueFixed + getRange.SkipCount + getCount - 1
                        }.Sort(newValueFixed, newValueFixed + count - 1);
                    }
                    return new collection</*Type[0]*/ulong/*Type[0]*/>(newValues, getRange.SkipCount, getCount, true);
                }
            }
            return null;
        }

        /// <summary>
        /// 索引范围排序器
        /// </summary>
        private unsafe struct /*Type[3]*/ulongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public /*Type[2]*/ulongSortIndex/*Type[2]*/* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public /*Type[2]*/ulongSortIndex/*Type[2]*/* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(/*Type[2]*/ulongSortIndex/*Type[2]*/* startIndex, /*Type[2]*/ulongSortIndex/*Type[2]*/* endIndex)
            {
                do
                {
                    /*Type[2]*/
                    ulongSortIndex/*Type[2]*/ leftValue = *startIndex, rightValue = *endIndex;
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
                    /*Type[2]*/
                    ulongSortIndex/*Type[2]*/* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                    /*Type[2]*/
                    ulongSortIndex/*Type[2]*/ indexValue = *averageIndex;
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
                        if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                        if (leftIndex > GetEndIndex) break;
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                        if (rightIndex < SkipCount) break;
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public unsafe static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                if (values.Length > (config.pub.Default.StreamBufferLength >> 4))
                {
                    /*Type[2]*/
                    ulongSortIndex/*Type[2]*/[] indexs = new /*Type[2]*/ulongSortIndex/*Type[2]*/[values.Length];
                    fixed (/*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex = indexs)
                    {
                        return getRangeSort/*Compare[0]*//*Compare[0]*/(values, getKey, range.SkipCount, getCount, fixedIndex);
                    }
                }
                else
                {
                    /*Type[2]*/
                    ulongSortIndex/*Type[2]*/* fixedIndex = stackalloc /*Type[2]*/ulongSortIndex/*Type[2]*/[values.Length];
                    return getRangeSort/*Compare[0]*//*Compare[0]*/(values, getKey, range.SkipCount, getCount, fixedIndex);
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private unsafe static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount
            , /*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ulongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = 0; index != values.Length; ++index)
            {
                *writeIndex++ = new /*Type[2]*/ulongSortIndex/*Type[2]*/ { Value = getKey(values[index]), Index = index };
            }
            return getRangeSort/*Compare[0]*//*Compare[0]*/(values, skipCount, getCount, fixedIndex);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private unsafe static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, int skipCount, int getCount, /*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex)
        {
            new /*Type[3]*/ulongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + values.Length - 1);
            valueType[] newValues = new valueType[getCount];
            /*Type[2]*/
            ulongSortIndex/*Type[2]*/* writeIndex = fixedIndex + skipCount;
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = values[(*writeIndex++).Index];
            return newValues;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="indexs">排序索引</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public unsafe static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, /*Type[2]*/ulongSortIndex/*Type[2]*/[] indexs, int skipCount, int getCount)
        {
            if (values.length() != indexs.length()) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                fixed (/*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex = indexs) return getRangeSort/*Compare[0]*//*Compare[0]*/(values, skipCount, getCount, fixedIndex);
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public unsafe static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, int startIndex, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                array.range getRange = new array.range(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                    if (values.Length > (config.pub.Default.StreamBufferLength >> 4))
                    {
                        /*Type[2]*/
                        ulongSortIndex/*Type[2]*/[] indexs = new /*Type[2]*/ulongSortIndex/*Type[2]*/[count];
                        fixed (/*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex = indexs)
                        {
                            return getRangeSort/*Compare[0]*//*Compare[0]*/
                                (values, range.SkipCount, count, getKey, getRange.SkipCount, getCount, fixedIndex);
                        }
                    }
                    else
                    {
                        /*Type[2]*/
                        ulongSortIndex/*Type[2]*/* fixedIndex = stackalloc /*Type[2]*/ulongSortIndex/*Type[2]*/[count];
                        return getRangeSort/*Compare[0]*//*Compare[0]*/
                            (values, range.SkipCount, count, getKey, getRange.SkipCount, getCount, fixedIndex);
                    }
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private unsafe static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (valueType[] values, int startIndex, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount
            , /*Type[2]*/ulongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ulongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = startIndex, endIndex = startIndex + count; index != endIndex; ++index)
            {
                *writeIndex++ = new /*Type[2]*/ulongSortIndex/*Type[2]*/ { Value = getKey(values[index]), Index = index };
            }
            new /*Type[3]*/ulongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + count - 1);
            valueType[] newValues = new valueType[getCount];
            writeIndex = fixedIndex + skipCount;
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
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> rangeSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getRangeSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> rangeSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getRangeSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (this valueType[] array, int startIndex, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> pageSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/(array, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static valueType[] getPageSort/*Compare[0]*//*Compare[0]*/<valueType>
            (this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getPageSort/*Compare[0]*//*Compare[0]*/
            (this /*Type[0]*/ulong/*Type[0]*/[] array, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array, page.SkipCount, page.CurrentPageSize);
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
        /// 范围排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> rangeSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int skipCount, int getCount)
        {
            if (list.count() != 0)
            {
                return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, 0, list.Count, skipCount, getCount);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getRangeSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int skipCount, int getCount)
        {
            if (list.count() != 0)
            {
                return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, 0, list.Count, skipCount, getCount);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            if (list.count() != 0)
            {
                return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/
                    (list.Unsafer.Array, 0, list.Count, getKey, skipCount, getCount);
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> rangeSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int startIndex, int count, int skipCount, int getCount)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/
                    (list.Unsafer.Array, range.SkipCount, count, skipCount, getCount);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getRangeSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int startIndex, int count, int skipCount, int getCount)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/
                    (list.Unsafer.Array, range.SkipCount, count, skipCount, getCount);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>
            (this list<valueType> list, int startIndex, int count, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , int skipCount, int getCount)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/
                    (list.Unsafer.Array, range.SkipCount, count, getKey, skipCount, getCount);
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> pageSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int pageSize, int currentPage)
        {
            array.page page = new array.page(list.count(), pageSize, currentPage);
            int count = page.CurrentPageSize;
            if (count != 0)
            {
                return algorithm.quickSort.RangeSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, page.SkipCount, count);
            }
            return null;
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="list">待排序单向动态数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getPageSort/*Compare[0]*//*Compare[0]*/
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int pageSize, int currentPage)
        {
            array.page page = new array.page(list.count(), pageSize, currentPage);
            int count = page.CurrentPageSize;
            if (count != 0)
            {
                return algorithm.quickSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(list.Unsafer.Array, page.SkipCount, count);
            }
            return null;
        }
    }
}