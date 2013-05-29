using System;

namespace fastCSharp.algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static partial class quickSort
    {
        /// <summary>
        /// 排序器
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        private struct sorter<valueType>
        {
            /// <summary>
            /// 待排序数组
            /// </summary>
            public valueType[] Array;
            /// <summary>
            /// 排序比较器
            /// </summary>
            public func<valueType, valueType, int> Comparer;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置-1</param>
            public void Sort(int startIndex, int endIndex)
            {
                do
                {
                    valueType leftValue = Array[startIndex], rightValue = Array[endIndex];
                    int average = (endIndex - startIndex) >> 1;
                    if (average == 0)
                    {
                        if (Comparer(leftValue, rightValue) > 0)
                        {
                            Array[startIndex] = rightValue;
                            Array[endIndex] = leftValue;
                        }
                        break;
                    }
                    int leftIndex = startIndex, rightIndex = endIndex;
                    valueType value = Array[average += startIndex];
                    if (Comparer(leftValue, value) <= 0)
                    {
                        if (Comparer(value, rightValue) > 0)
                        {
                            Array[rightIndex] = value;
                            if (Comparer(leftValue, rightValue) <= 0) Array[average] = value = rightValue;
                            else
                            {
                                Array[leftIndex] = rightValue;
                                Array[average] = value = leftValue;
                            }
                        }
                    }
                    else if (Comparer(leftValue, rightValue) <= 0)
                    {
                        Array[leftIndex] = value;
                        Array[average] = value = leftValue;
                    }
                    else
                    {
                        Array[rightIndex] = leftValue;
                        if (Comparer(value, rightValue) <= 0)
                        {
                            Array[leftIndex] = value;
                            Array[average] = value = rightValue;
                        }
                        else Array[leftIndex] = rightValue;
                    }
                    ++leftIndex;
                    --rightIndex;
                    do
                    {
                        while (Comparer(Array[leftIndex], value) < 0) ++leftIndex;
                        while (Comparer(value, Array[rightIndex]) < 0) --rightIndex;
                        if (leftIndex < rightIndex)
                        {
                            leftValue = Array[leftIndex];
                            Array[leftIndex] = Array[rightIndex];
                            Array[rightIndex] = leftValue;
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
                        if (startIndex < rightIndex) Sort(startIndex, rightIndex);
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex) Sort(leftIndex, endIndex);
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        public static void Sort<valueType>(valueType[] values, func<valueType, valueType, int> comparer)
        {
            if (values != null && values.Length > 1)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                new sorter<valueType> { Array = values, Comparer = comparer }.Sort(0, values.Length - 1);
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] GetSort<valueType>(valueType[] values, func<valueType, valueType, int> comparer)
        {
            if (values.length() != 0)
            {
                sorter<valueType> sorter = new sorter<valueType> { Array = values.copy(), Comparer = comparer };
                if (values.Length > 1)
                {
                    if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                    sorter.Sort(0, values.Length - 1);
                }
                return sorter.Array;
            }
            return values.notNull();
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        public static void Sort<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int startIndex, int count)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if (range.GetCount > 1)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                new sorter<valueType> { Array = values, Comparer = comparer }.Sort(range.SkipCount, range.EndIndex - 1);
            }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] GetSort<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int startIndex, int count)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                valueType[] newValues = new valueType[count];
                Array.Copy(values, range.SkipCount, newValues, 0, count);
                if (--count > 0)
                {
                    if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                    new sorter<valueType> { Array = newValues, Comparer = comparer }.Sort(0, count);
                }
                return newValues;
            }
            return values.notNull();
        }

        /// <summary>
        /// 范围排序器(一般用于获取分页)
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        private struct rangeSorter<valueType>
        {
            /// <summary>
            /// 待排序数组
            /// </summary>
            public valueType[] Array;
            /// <summary>
            /// 排序比较器
            /// </summary>
            public func<valueType, valueType, int> Comparer;
            /// <summary>
            /// 跳过数据数量
            /// </summary>
            public int SkipCount;
            /// <summary>
            /// 最后一条记录位置-1
            /// </summary>
            public int GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置-1</param>
            public void Sort(int startIndex, int endIndex)
            {
                do
                {
                    valueType leftValue = Array[startIndex], rightValue = Array[endIndex];
                    int average = (endIndex - startIndex) >> 1;
                    if (average == 0)
                    {
                        if (Comparer(leftValue, rightValue) > 0)
                        {
                            Array[startIndex] = rightValue;
                            Array[endIndex] = leftValue;
                        }
                        break;
                    }
                    average += startIndex;
                    //if (average > getEndIndex) average = getEndIndex;
                    //else if (average < skipCount) average = skipCount;
                    int leftIndex = startIndex, rightIndex = endIndex;
                    valueType value = Array[average];
                    if (Comparer(leftValue, value) <= 0)
                    {
                        if (Comparer(value, rightValue) > 0)
                        {
                            Array[rightIndex] = value;
                            if (Comparer(leftValue, rightValue) <= 0) Array[average] = value = rightValue;
                            else
                            {
                                Array[leftIndex] = rightValue;
                                Array[average] = value = leftValue;
                            }
                        }
                    }
                    else if (Comparer(leftValue, rightValue) <= 0)
                    {
                        Array[leftIndex] = value;
                        Array[average] = value = leftValue;
                    }
                    else
                    {
                        Array[rightIndex] = leftValue;
                        if (Comparer(value, rightValue) <= 0)
                        {
                            Array[leftIndex] = value;
                            Array[average] = value = rightValue;
                        }
                        else Array[leftIndex] = rightValue;
                    }
                    ++leftIndex;
                    --rightIndex;
                    do
                    {
                        while (Comparer(Array[leftIndex], value) < 0) ++leftIndex;
                        while (Comparer(value, Array[rightIndex]) < 0) --rightIndex;
                        if (leftIndex < rightIndex)
                        {
                            leftValue = Array[leftIndex];
                            Array[leftIndex] = Array[rightIndex];
                            Array[rightIndex] = leftValue;
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
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static collection<valueType> RangeSort<valueType>
            (valueType[] values, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                new rangeSorter<valueType>
                {
                    Array = values,
                    Comparer = comparer,
                    SkipCount = range.SkipCount,
                    GetEndIndex = range.EndIndex - 1
                }.Sort(0, values.Length - 1);
                return new collection<valueType>(values, range.SkipCount, getCount, true);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        public static collection<valueType> GetRangeSort<valueType>
            (valueType[] values, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                rangeSorter<valueType> sorter = new rangeSorter<valueType>
                {
                    Array = values.copy(),
                    Comparer = comparer,
                    SkipCount = range.SkipCount,
                    GetEndIndex = range.EndIndex - 1
                };
                sorter.Sort(0, values.Length - 1);
                return new collection<valueType>(sorter.Array, range.SkipCount, getCount, true);
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static collection<valueType> RangeSort<valueType>
            (valueType[] values, int startIndex, int count, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                array.range getRange = new array.range(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                    skipCount = range.SkipCount + getRange.SkipCount;
                    new rangeSorter<valueType>
                    {
                        Array = values,
                        Comparer = comparer,
                        SkipCount = skipCount,
                        GetEndIndex = skipCount + getCount - 1
                    }.Sort(range.SkipCount, range.SkipCount + --count);
                    return new collection<valueType>(values, skipCount, getCount, true);
                }
            }
            return null;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static collection<valueType> GetRangeSort<valueType>
            (valueType[] values, int startIndex, int count, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            array.range range = new array.range(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                array.range getRange = new array.range(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    valueType[] newValues = new valueType[count];
                    Array.Copy(values, range.SkipCount, newValues, 0, count);
                    return RangeSort(newValues, comparer, getRange.SkipCount, getCount);
                }
            }
            return null;
        }

        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static list<valueType> GetTop<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int count)
        {
            if (values == null) return null;
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (count > 0)
            {
                if (count < values.Length)
                {
                    if (count <= values.Length >> 1) return getTop(values, comparer, count);
                    else return getRemoveTop(values, comparer, count);
                }
                valueType[] newValues = new valueType[values.Length];
                Array.Copy(values, 0, newValues, 0, values.Length);
                return new list<valueType>(newValues, true);
            }
            return null;
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static list<valueType> Top<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int count)
        {
            if (values == null) return null;
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (count > 0)
            {
                if (count < values.Length)
                {
                    if (count <= values.Length >> 1) return getTop(values, comparer, count);
                    else return getRemoveTop(values, comparer, count);
                }
                return new list<valueType>(values, true);
            }
            return null;
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static list<valueType> getTop<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            valueType[] newValues = new valueType[length];
            int readIndex = values.Length - length, writeIndex = count;
            Array.Copy(values, readIndex, newValues, 0, length);
            rangeSorter<valueType> sort
                = new rangeSorter<valueType> { Array = newValues, Comparer = comparer, SkipCount = count - 1, GetEndIndex = count - 1 };
            sort.Sort(0, --length);
            for (valueType maxValue = newValues[sort.GetEndIndex]; readIndex != 0; )
            {
                if (comparer(values[--readIndex], maxValue) < 0)
                {
                    newValues[writeIndex] = values[readIndex];
                    if (writeIndex == length)
                    {
                        sort.Sort(0, length);
                        writeIndex = count;
                        maxValue = newValues[sort.GetEndIndex];
                    }
                    else ++writeIndex;
                }
            }
            if (writeIndex != count) sort.Sort(0, writeIndex - 1);
            Array.Clear(newValues, count, newValues.Length - count);
            return newValues.left(count);
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static list<valueType> getRemoveTop<valueType>(valueType[] values, func<valueType, valueType, int> comparer, int count)
        {
            valueType[] newValues = new valueType[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            valueType[] removeValues = new valueType[length];
            int readIndex = values.Length - length, copyCount = length - count, removeIndex = copyCount, writeIndex = copyCount;
            Array.Copy(values, readIndex, removeValues, 0, length);
            rangeSorter<valueType> sort 
                = new rangeSorter<valueType> { Array = removeValues, Comparer = comparer, SkipCount = copyCount, GetEndIndex = copyCount };
            sort.Sort(0, --length);
            Array.Copy(removeValues, 0, newValues, 0, copyCount);
            for (valueType maxValue = removeValues[copyCount]; readIndex != 0; )
            {
                if (comparer(values[--readIndex], maxValue) <= 0) newValues[writeIndex++] = values[readIndex];
                else
                {
                    removeValues[--removeIndex] = values[readIndex];
                    if (removeIndex == 0)
                    {
                        sort.Sort(0, length);
                        removeIndex = copyCount;
                        maxValue = removeValues[copyCount];
                        Array.Copy(removeValues, 0, newValues, writeIndex, copyCount);
                        writeIndex += copyCount;
                    }
                }
            }
            if (removeIndex != copyCount)
            {
                sort.Sort(removeIndex, length);
                Array.Copy(removeValues, removeIndex, newValues, writeIndex, copyCount - removeIndex);
            }
            return new list<valueType>(newValues, true);
        }
    }
}
