using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class array
    {
        #region 数据记录范围
        /// <summary>
        /// 数据记录范围
        /// </summary>
        public struct range
        {
            /// <summary>
            /// 数据总量
            /// </summary>
            private int count;
            /// <summary>
            /// 起始位置
            /// </summary>
            private int startIndex;
            /// <summary>
            /// 跳过记录数
            /// </summary>
            public int SkipCount
            {
                get { return startIndex; }
            }
            /// <summary>
            /// 结束位置
            /// </summary>
            private int endIndex;
            /// <summary>
            /// 结束位置
            /// </summary>
            public int EndIndex
            {
                get { return endIndex; }
            }
            /// <summary>
            /// 获取记录数
            /// </summary>
            public int GetCount
            {
                get { return endIndex - startIndex; }
            }

            /// <summary>
            /// 数据记录范围
            /// </summary>
            /// <param name="count">数据总量</param>
            /// <param name="skipCount">跳过记录数</param>
            /// <param name="getCount">获取记录数</param>
            public range(int count, int skipCount, int getCount)
            {
                this.count = count < 0 ? 0 : count;
                if (skipCount < count && getCount != 0)
                {
                    if (getCount > 0)
                    {
                        if (skipCount >= 0)
                        {
                            startIndex = skipCount;
                            if ((endIndex = skipCount + getCount) > count) endIndex = count;
                        }
                        else
                        {
                            startIndex = 0;
                            if ((endIndex = skipCount + getCount) > count) endIndex = count;
                            else if (endIndex < 0) endIndex = 0;
                        }
                    }
                    else
                    {
                        startIndex = skipCount >= 0 ? skipCount : 0;
                        endIndex = count;
                    }
                }
                else startIndex = endIndex = 0;
            }
        }
        #endregion

        #region 分页记录范围
        /// <summary>
        /// 分页记录范围
        /// </summary>
        public struct page
        {
            /// <summary>
            /// 数据总量
            /// </summary>
            private int count;
            /// <summary>
            /// 数据总量
            /// </summary>
            public int Count
            {
                get { return count; }
            }
            /// <summary>
            /// 分页总数
            /// </summary>
            private int pageCount;
            /// <summary>
            /// 分页总数
            /// </summary>
            public int PageCount
            {
                get { return pageCount; }
            }
            /// <summary>
            /// 当前页号
            /// </summary>
            private int currentPage;
            /// <summary>
            /// 当前页号
            /// </summary>
            public int CurrentPage
            {
                get { return currentPage; }
            }
            /// <summary>
            /// 分页尺寸
            /// </summary>
            private int pageSize;
            /// <summary>
            /// 分页尺寸
            /// </summary>
            public int PageSize
            {
                get { return pageSize; }
            }
            /// <summary>
            /// 跳过记录数
            /// </summary>
            public int SkipCount
            {
                get { return (CurrentPage - 1) * PageSize; }
            }
            /// <summary>
            /// 当前页记录数
            /// </summary>
            public int CurrentPageSize
            {
                get
                {
                    int value = SkipCount;
                    return Math.Min(value + PageSize, Count) - value;
                }
            }
            /// <summary>
            /// 分页记录范围
            /// </summary>
            /// <param name="count">数据总量</param>
            /// <param name="pageSize">分页尺寸</param>
            /// <param name="currentPage">页号</param>
            public page(int count, int pageSize, int currentPage)
            {
                this.pageSize = pageSize > 0 ? pageSize : config.pub.Default.PageSize;
                this.count = count < 0 ? 0 : count;
                pageCount = (this.count + this.pageSize - 1) / this.pageSize;
                if (pageCount < 0) pageCount = 0;
                this.currentPage = currentPage > 0 ? (currentPage <= pageCount ? currentPage : (pageCount == 0 ? 1 : pageCount)) : 1;
            }
        }
        #endregion

        /// <summary>
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>null为0</returns>
        public static int length<valueType>(this valueType[] array)
        {
            return array != null ? array.Length : 0;
        }
        /// <summary>
        /// 空值转0长度数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>非空数组</returns>
        public static valueType[] notNull<valueType>(this valueType[] array)
        {
            return array != null ? array : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 根据索引位置获取数组元素值
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <param name="index">索引位置</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>数组元素值</returns>
        public static valueType get<valueType>(this valueType[] array, int index, valueType nullValue)
        {
            return array != null && (uint)index < (uint)array.Length ? array[index] : nullValue;
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        public static valueType lastOrDefault<valueType>(this valueType[] array)
        {
            return array != null && array.Length != 0 ? array[array.Length - 1] : default(valueType);
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>第一个匹配值,失败为default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in array)
                {
                    if (isValue(value)) return value;
                }
            }
            return default(valueType);
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="index">起始位置</param>
        /// <returns>第一个匹配值,失败为default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this valueType[] array, func<valueType, bool> isValue, int index)
        {
            if (array != null && (uint)index < (uint)array.Length)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                while (index != array.Length)
                {
                    if (isValue(array[index])) return array[index];
                    ++index;
                }
            }
            return default(valueType);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this valueType[] array, valueType value)
        {
            return array != null ? Array.IndexOf(array, value) : -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (array.length() != 0)
            {
                for (int index = 0; index != array.Length; ++index)
                {
                    if (isValue(array[index])) return index;
                }
            }
            return -1;
        }
        #region 二分查找匹配值位置
        /// <summary>
        /// 二分查找匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryIndexOf<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryIndexOf(values, value, (left, right) => left.CompareTo(right));
        }
        /// <summary>
        /// 二分查找匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryIndexOf<valueType>(this valueType[] values, valueType value
            , func<valueType, valueType, int> comparer)
        {
            if (values != null && values.Length != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp < 0) length = average;
                        else if (cmp == 0) return average;
                        else start = average + 1;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp > 0) length = average;
                        else if (cmp == 0) return average;
                        else start = average + 1;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 二分查找匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer, bool isAscending)
        {
            return binaryIndexOf(values, value, comparer, (left, right) => isAscending ? -1 : 1);
        }
        /// <summary>
        /// 二分查找匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer, func<valueType, valueType, int> orderComparer)
        {
            if (values != null && values.Length != 0)
            {
                if (comparer == null || orderComparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp < 0) length = average;
                        else if (cmp == 0) return average;
                        else start = average + 1;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp > 0) length = average;
                        else if (cmp == 0) return average;
                        else start = average + 1;
                    }
                }
            }
            return -1;
        }
        #endregion

        #region 二分查找第一个匹配值位置
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryFirstIndexOf<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryFirstIndexOf(values, value, (left, right) => left.CompareTo(right));
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryFirstIndexOf<valueType>(this valueType[] values, valueType value, func<valueType, valueType, int> comparer)
        {
            int index = -1;
            if (values != null && values.Length != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp <= 0)
                        {
                            length = average;
                            if (cmp == 0) index = average;
                        }
                        else start = average + 1;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp >= 0)
                        {
                            length = average;
                            if (cmp == 0) index = average;
                        }
                        else start = average + 1;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryFirstIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryFirstIndexOf(values, value, comparer, (left, right) => isAscending ? -1 : 1);
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryFirstIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , func<valueType, valueType, int> orderComparer)
        {
            int index = -1;
            if (values != null && values.Length != 0)
            {
                if (comparer == null || orderComparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp <= 0)
                        {
                            length = average;
                            if (cmp == 0) index = average;
                        }
                        else start = average + 1;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp >= 0)
                        {
                            length = average;
                            if (cmp == 0) index = average;
                        }
                        else start = average + 1;
                    }
                }
            }
            return index;
        }
        #endregion

        #region 二分查找最后一个匹配值位置
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryLastIndexOf<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryLastIndexOf(values, value, (left, right) => left.CompareTo(right));
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryLastIndexOf<valueType>(this valueType[] values, valueType value, func<valueType, valueType, int> comparer)
        {
            int index = -1;
            if (values != null && values.Length != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp >= 0)
                        {
                            start = average + 1;
                            if (cmp == 0) index = average;
                        }
                        else length = average;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp <= 0)
                        {
                            start = average + 1;
                            if (cmp == 0) index = average;
                        }
                        else length = average;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryLastIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryLastIndexOf(values, value, comparer, (left, right) => isAscending ? -1 : 1);
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        public static int binaryLastIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , func<valueType, valueType, int> orderComparer)
        {
            int index = -1;
            if (values != null && values.Length != 0)
            {
                if (comparer == null || orderComparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, start = 0, length = values.Length, cmp;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp >= 0)
                        {
                            start = average + 1;
                            if (cmp == 0) index = average;
                        }
                        else length = average;
                    }
                }
                else
                {
                    while (start < length)
                    {
                        average = start + ((length - start) >> 1);
                        cmp = comparer(value, values[average]);
                        if (cmp <= 0)
                        {
                            start = average + 1;
                            if (cmp == 0) index = average;
                        }
                        else length = average;
                    }
                }
            }
            return index;
        }
        #endregion

        #region 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryIndexOfThan(values, value, (left, right) => left.CompareTo(right));
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<valueType>(this valueType[] values, valueType value, func<valueType, valueType, int> comparer)
        {
            int index = 0;
            if (values == null) index = -1;
            else if (values.Length != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, length = values.Length;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) >= 0) index = average + 1;
                        else length = average;
                    }
                }
                else
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) <= 0) index = average + 1;
                        else length = average;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryIndexOfThan(values, value, comparer, (left, right) => isAscending ? -1 : 1);
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , func<valueType, valueType, int> orderComparer)
        {
            int index = 0;
            if (values == null) index = -1;
            else if (values.Length != 0)
            {
                if (comparer == null || orderComparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, length = values.Length;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) >= 0) index = average + 1;
                        else length = average;
                    }
                }
                else
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) <= 0) index = average + 1;
                        else length = average;
                    }
                }
            }
            return index;
        }
        #endregion

        #region 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryIndexOfLess(values, value, (left, right) => left.CompareTo(right));
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<valueType>(this valueType[] values, valueType value, func<valueType, valueType, int> comparer)
        {
            int index = 0;
            if (values == null) index = -1;
            else if (values.Length != 0)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, length = values.Length;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) > 0) index = average + 1;
                        else length = average;
                    }
                }
                else
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) < 0) index = average + 1;
                        else length = average;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryIndexOfLess(values, value, comparer, (left, right) => isAscending ? -1 : 1);
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<keyType, valueType>(this valueType[] values, keyType value
            , func<keyType, valueType, int> comparer
            , func<valueType, valueType, int> orderComparer)
        {
            int index = 0;
            if (values == null) index = -1;
            else if (values.Length != 0)
            {
                if (comparer == null || orderComparer == null) log.Default.Throw(log.exceptionType.Null);

                int average, length = values.Length;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) > 0) index = average + 1;
                        else length = average;
                    }
                }
                else
                {
                    while (index < length)
                    {
                        average = index + ((length - index) >> 1);
                        if (comparer(value, values[average]) < 0) index = average + 1;
                        else length = average;
                    }
                }
            }
            return index;
        }
        #endregion
        /// <summary>
        /// 判断是否存在匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配值</param>
        /// <returns>是否存在匹配值</returns>
        public static bool any<valueType>(this valueType[] array, valueType value)
        {
            return array != null && Array.IndexOf(array, value) != -1;
        }
        /// <summary>
        /// 判断是否存在匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配值</returns>
        public static bool any<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in array)
                {
                    if (isValue(value)) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            int count = 0;
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in array)
                {
                    if (isValue(value)) ++count;
                }
            }
            return count;
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <returns>复制后的新数组</returns>
        public static valueType[] copy<valueType>(this valueType[] array)
        {
            if (array.length() != 0)
            {
                valueType[] newValues = new valueType[array.Length];
                Array.Copy(array, 0, newValues, 0, array.Length);
                return newValues;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 翻转数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的数组</returns>
        public static valueType[] reverse<valueType>(this valueType[] array)
        {
            if (array.length() != 0)
            {
                Array.Reverse(array);
                return array;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">添加的数据</param>
        /// <returns>添加数据的数组</returns>
        public static valueType[] getAdd<valueType>(this valueType[] array, valueType value)
        {
            if (array != null)
            {
                valueType[] newValues = new valueType[array.Length + 1];
                Array.Copy(array, 0, newValues, 0, array.Length);
                newValues[array.Length] = value;
                return newValues;
            }
            return new valueType[] { value };
        }
        /// <summary>
        /// 获取前端子段集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">原数组</param>
        /// <param name="count">数量</param>
        /// <returns>子段集合</returns>
        public static list<valueType> left<valueType>(this valueType[] array, int count)
        {
            return array != null ? new list<valueType>(array, 0, count <= array.Length ? count : array.Length, true) : null;
        }
        /// <summary>
        /// 获取子段集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <returns>子段集合</returns>
        public static collection<valueType> sub<valueType>(this valueType[] array, int index)
        {
            return array != null ? sub(array, index, -1) : null;
        }
        /// <summary>
        /// 获取子段集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子段集合</returns>
        public static collection<valueType> sub<valueType>(this valueType[] array, int index, int count)
        {
            if (array != null)
            {
                range range = new range(array.Length, index, count);
                return new collection<valueType>(array, range.SkipCount, range.GetCount, true);
            }
            return null;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public static valueType[] getSub<valueType>(this valueType[] array, int index, int count)
        {
            return array.sub(index, count).getArray();
        }
        /// <summary>
        /// 获取分页字段数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>分页字段数组</returns>
        public static arrayType[] getPage<valueType, arrayType>
            (this valueType[] array, int pageSize, int currentPage, func<valueType, arrayType> getValue)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return array.sub(page.SkipCount, page.CurrentPageSize).getArray(getValue);
        }
        /// <summary>
        /// 获取分页字段数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array"></param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue"></param>
        /// <returns>分页字段数组</returns>
        public static valueType[] getPage<valueType>(this valueType[] array, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return array.getSub(page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="index">移除数据位置</param>
        /// <returns>移除数据后的数组</returns>
        private static valueType[] GetRemoveAt<valueType>(valueType[] array, int index)
        {
            valueType[] newValues = new valueType[array.Length - 1];
            Array.Copy(array, 0, newValues, 0, index);
            Array.Copy(array, index + 1, newValues, index, array.Length - index - 1);
            return newValues;
        }
        /// <summary>
        /// 移除第一个匹配数据数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">待移除的数据</param>
        /// <returns>移除数据后的数组</returns>
        public static valueType[] removeFirst<valueType>(this valueType[] array, valueType value) where valueType : IComparable<valueType>
        {
            if (array != null)
            {
                int index = Array.IndexOf(array, value);
                if (index != -1) return GetRemoveAt(array, index);
                return array;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 移除第一个匹配数据数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>移除数据后的数组</returns>
        public static valueType[] removeFirst<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            if (array != null)
            {
                int index = 0;
                while (index != array.Length && !isValue(array[index])) ++index;
                if (index != array.Length) return GetRemoveAt(array, index);
            }
            return array.notNull();
        }
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">新值</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>替换数据后的数组</returns>
        public static valueType[] replaceFirst<valueType>(this valueType[] array, valueType value, func<valueType, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                for (int index = 0; index != array.Length; ++index)
                {
                    if (isValue(array[index])) array[index] = value;
                }
            }
            return array.notNull();
        }
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        public static void move<valueType>(this valueType[] array, int index, int writeIndex, int count)
        {
            if (count > 0)
            {
                int writeEndIndex = writeIndex + count;
                if (index >= 0 && writeEndIndex <= array.Length)
                {
                    int endIndex = index + count;
                    if (index < writeIndex && endIndex > writeIndex)
                    {
                        while (endIndex != index) array[--writeEndIndex] = array[--endIndex];
                    }
                    else if (writeIndex >= 0 && endIndex <= array.Length) Array.Copy(array, index, array, writeIndex, count);
                    else log.Default.Throw(log.exceptionType.IndexOutOfRange);
                }
                else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
            else if (count != 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 根据集合内容返回单向列表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <returns>单向列表</returns>
        public static list<valueType> toList<valueType>(this valueType[] array)
        {
            return new list<valueType>(array, true);
        }
        /// <summary>
        /// 根据集合内容返回双向列表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <returns>双向列表</returns>
        public static collection<valueType> toCollection<valueType>(this valueType[] array)
        {
            return new collection<valueType>(array, true);
        }
        /// <summary>
        /// 转换HASH
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <returns>HASH</returns>
        public static HashSet<valueType> getHash<valueType>(this valueType[] array)
        {
            if (array != null)
            {
                HashSet<valueType> hash = new HashSet<valueType>();
                foreach (valueType value in array) hash.Add(value);
                return hash;
            }
            return null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public static list<arrayType> distinct<valueType, arrayType>(this valueType[] array, func<valueType, arrayType> getValue)
        {
            if (array != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                list<arrayType>.unsafer newValues = new list<arrayType>(array.Length).Unsafer;
                HashSet<valueType> hash = new HashSet<valueType>();
                foreach(valueType value in array)
                {
                    if (!hash.Contains(value))
                    {
                        newValues.Add(getValue(value));
                        hash.Add(value);
                    }
                }
                return newValues.List;
            }
            return null;
        }
        /// <summary>
        /// 转换HASH
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>HASH</returns>
        public static HashSet<valueType> getHash<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (array != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                HashSet<valueType> hash = new HashSet<valueType>();
                foreach(valueType value in array)
                {
                    if (isValue(value)) hash.Add(value);
                }
                return hash;
            }
            return null;
        }
        /// <summary>
        /// 转换HASH
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="hashType">目标数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>HASH</returns>
        public static HashSet<hashType> getHash<valueType, hashType>(this valueType[] array, func<valueType, hashType> getValue)
        {
            if (array != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                HashSet<hashType> hash = new HashSet<hashType>();
                foreach (valueType value in array) hash.Add(getValue(value));
                return hash;
            }
            return null;
        }
        /// <summary>
        /// 将数组转化为字典
        /// </summary>
        /// <typeparam name="keyType">关键字类型</typeparam>
        /// <typeparam name="valueType">默认值类型</typeparam>
        /// <param name="array">待转化的数组</param>
        /// <param name="defaultValue">默认值数组</param>
        /// <returns>数组转化后字典</returns>
        public static Dictionary<keyType, valueType> getDictionary<keyType, valueType>(this keyType[] array, valueType[] defaultValue)
        {
            if (array != null)
            {
                Dictionary<keyType, valueType> dictionary = new Dictionary<keyType, valueType>(array.Length << 1);
                if (defaultValue.length() == array.Length)
                {
                    int index = 0;
                    foreach (keyType key in array) dictionary.Add(key, defaultValue[index++]);
                }
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 将数组转化为字典
        /// </summary>
        /// <typeparam name="keyType">关键字类型</typeparam>
        /// <typeparam name="valueType">默认值类型</typeparam>
        /// <param name="array">待转化的数组</param>
        /// <param name="defaultValue">默认值数组</param>
        /// <returns>数组转化后字典</returns>
        public static Dictionary<keyType, valueType> getDictionary<keyType, valueType>(this keyType[] array, valueType defaultValue)
        {
            if (array != null)
            {
                Dictionary<keyType, valueType> dictionary = new Dictionary<keyType, valueType>(array.Length << 1);
                foreach (keyType key in array) dictionary.Add(key, defaultValue);
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 将键值数组转化为字典
        /// </summary>
        /// <typeparam name="keyValues">关键字类型</typeparam>
        /// <typeparam name="valueType">默认值类型</typeparam>
        /// <param name="array">键值数组</param>
        /// <returns>数组转化后字典</returns>
        public static Dictionary<keyType, valueType> getDictionary<keyType, valueType>(this keyValue<keyType, valueType>[] array)
        {
            if (array != null)
            {
                Dictionary<keyType, valueType> dictionary = new Dictionary<keyType, valueType>(array.Length << 1);
                foreach (keyValue<keyType, valueType> value in array) dictionary.Add(value.Key, value.Value);
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <returns>分组数据</returns>
        public static Dictionary<keyType, list<valueType>> group<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey)
        {
            if (array != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, list<valueType>> newValues = new Dictionary<keyType, list<valueType>>(array.Length << 1);
                list<valueType> list;
                foreach (valueType value in array)
                {
                    keyType key = getKey(value);
                    if (!newValues.TryGetValue(key, out list)) newValues[key] = list = new list<valueType>();
                    list.Add(value);
                }
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 分组计数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组计数</returns>
        public static Dictionary<keyType, int> groupCount<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey)
        {
            if (array != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                int count;
                Dictionary<keyType, int> dictionary = new Dictionary<keyType, int>(array.Length);
                foreach (valueType value in array)
                {
                    keyType key = getKey(value);
                    if (dictionary.TryGetValue(key, out count)) dictionary[key] = count + 1;
                    else dictionary.Add(key, 1);
                }
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// HASH统计
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>HASH统计</returns>
        public static Dictionary<valueType, int> valueCount<valueType>(this valueType[] array)
        {
            if (array != null)
            {
                int count;
                Dictionary<valueType, int> dictionary = new Dictionary<valueType, int>(array.Length << 1);
                foreach (valueType value in array)
                {
                    if (dictionary.TryGetValue(value, out count)) dictionary[value] = ++count;
                    else dictionary.Add(value, 1);
                }
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 求交集
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="left">左侧数据</param>
        /// <param name="right">右侧数据</param>
        /// <returns>数据交集</returns>
        public static list<valueType> intersect<valueType>(this valueType[] left, valueType[] right)
        {
            int leftLength = left.length();
            if (leftLength != 0)
            {
                int rightLength = right.length();
                if (rightLength != 0)
                {
                    valueType[] min = leftLength <= rightLength ? left : right;
                    list<valueType>.unsafer values = new list<valueType>(min.Length).Unsafer;
                    staticHashSet<valueType> hash = new staticHashSet<valueType>(min);
                    foreach (valueType value in leftLength <= rightLength ? right : left)
                    {
                        if (hash.Contains(value)) values.Add(value);
                    }
                    return values.List;
                }
            }
            return null;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="method">调用函数</param>
        /// <returns>数组数据</returns>
        public static valueType[] each<valueType>(this valueType[] array, action<valueType> method)
        {
            if (array != null)
            {
                if (method == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in array) method(value);
            }
            return array.notNull();
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static arrayType[] getArray<valueType, arrayType>(this valueType[] array, func<valueType, arrayType> getValue)
        {
            if (array.length() != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                arrayType[] newValues = new arrayType[array.Length];
                int index = 0;
                foreach (valueType value in array) newValues[index++] = getValue(value);
                return newValues;
            }
            return nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 获取键值对数组
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">键值对数组</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>键值对数组</returns>
        public static keyValue<keyType, valueType>[] getKeyValueArray<keyType, valueType>(this valueType[] array, func<valueType, keyType> getKey)
        {
            if (array.length() != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                keyValue<keyType, valueType>[] newValues = new keyValue<keyType, valueType>[array.Length];
                int index = 0;
                foreach (valueType value in array) newValues[index++].Set(getKey(value), value);
                return newValues;
            }
            return nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <typeparam name="valueType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>目标数组</returns>
        public static valueType[] toArray<valueType>(this Array array)
        {
            return array != null ? array as valueType[] : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="valueType">目标数组类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">一次处理两个数据</param>
        /// <returns>目标数组</returns>
        public static arrayType[] getArray<valueType, arrayType>(this valueType[] array, func<valueType, valueType, arrayType> getValue)
        {
            if (getValue == null) log.Default.Throw(log.exceptionType.Null);
            if (array.length() != 0)
            {
                int length = array.Length, index = (length + 1) >> 1;
                arrayType[] newValues = new arrayType[index];
                if ((length & 1) != 0) newValues[--index] = getValue(array[--length], default(valueType));
                while (--index >= 0)
                {
                    valueType right = array[--length];
                    newValues[index] = getValue(array[--length], right);
                }
                return newValues;
            }
            return nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        private static valueType[] GetArray<valueType>(valueType[][] array)
        {
            int length = 0;
            foreach(valueType[] value in array)
            {
                if (value != null) length += value.Length;
            }
            if (length != 0)
            {
                valueType[] newValues = new valueType[length];
                length = 0;
                foreach (valueType[] value in array)
                {
                    if (value != null)
                    {
                        value.CopyTo(newValues, length);
                        length += value.Length;
                    }
                }
                return newValues;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static valueType[] toArray<valueType>(this valueType[][] array)
        {
            if (array.length() != 0)
            {
                return array.Length == 1 ? array[0].notNull() : GetArray(array);
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static valueType[] getArray<valueType>(this valueType[][] array)
        {
            if (array.length() != 0)
            {
                if (array.Length != 1) return GetArray(array);
                return array[0].copy();
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <param name="addValues">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static valueType[] concat<valueType>(this valueType[] array, valueType[] addValues)
        {
            return getArray(new valueType[][] { array, addValues });
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static valueType[] concat<valueType>(params valueType[][] array)
        {
            return array.getArray();
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static arrayType[] concat<valueType, arrayType>(this valueType[] array, func<valueType, arrayType[]> getValue)
        {
            if (getValue == null) log.Default.Throw(log.exceptionType.Null);
            return array.getArray(value => getValue(value)).toArray();
        }
        /// <summary>
        /// 分割数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="count">子数组长度</param>
        /// <returns>分割后的数组集合</returns>
        public static collection<valueType>[] split<valueType>(this valueType[] array, int count)
        {
            if (array != null && count > 0)
            {
                int length = (array.Length + count - 1) / count;
                collection<valueType>[] newValues = new collection<valueType>[length];
                for (int index = length * count; 
                    length != 0;
                    newValues[--length] = new collection<valueType>(array, index -= count, count, true)) ;
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 分割数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="count">子数组长度</param>
        /// <returns>分割后的数组集合</returns>
        public static valueType[][] getSplit<valueType>(this valueType[] array, int count)
        {
            if (array != null && count > 0)
            {
                if (count < array.Length)
                {
                    int length = (array.Length + count - 1) / count, copyIndex = 0;
                    valueType[][] newValues = new valueType[length--][];
                    for (int index = 0; index != length; ++index, copyIndex += count)
                    {
                        Array.Copy(array, copyIndex, newValues[index] = new valueType[count], 0, count);
                    }
                    Array.Copy(array, copyIndex, newValues[length] = new valueType[count = array.Length - copyIndex], 0, count);
                    return newValues;
                }
                if (array.Length != 0) return new valueType[][] { array };
            }
            return nullValue<valueType[]>.Array;
        }
        /// <summary>
        /// 转换键值对集合
        /// </summary>
        /// <typeparam name="keyType">键值类型</typeparam>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">键值数组</param>
        /// <param name="values">数组数据</param>
        /// <returns>键值对数组</returns>
        public static keyValue<keyType, valueType>[] getKeyValue<keyType, valueType>(this keyType[] array, valueType[] values)
        {
            int length = array.length();
            if (length != values.length()) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            if (length != 0)
            {
                keyValue<keyType, valueType>[] newValues = new keyValue<keyType, valueType>[array.Length];
                int index = 0;
                foreach (keyType key in array)
                {
                    newValues[index].Set(key, values[index]);
                    ++index;
                }
                return newValues;
            }
            return nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public static list<valueType> getFind<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            if (array != null)
            {
                int length = array.Length;
                if (length != 0)
                {
                    valueType[] newValues = new valueType[array.Length < sizeof(int) ? sizeof(int) : length];
                    length = 0;
                    foreach(valueType value in array)
                    {
                        if (isValue(value)) newValues[length++] = value;
                    }
                    return new list<valueType>(newValues, 0, length, true);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public unsafe static valueType[] getFindArray<valueType>(this valueType[] array, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int length = array.length();
            if (length != 0)
            {
                length = (length + 7) >> 3;
                if (length <= config.pub.Default.StreamBufferLength)
                {
                    byte* data = stackalloc byte[length];
                    return GetFindArray(array, isValue, new fixedMap(data, length));
                }
                else
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataFixed = data) return GetFindArray(array, isValue, new fixedMap(dataFixed));
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static valueType[] GetFindArray<valueType>(valueType[] array, func<valueType, bool> isValue, fixedMap map)
        {
            int length = 0;
            for (int index = 0; index != array.Length; ++index)
            {
                if (isValue(array[index]))
                {
                    ++length;
                    map.Set(index);
                }
            }
            if (length != 0)
            {
                valueType[] newValues = new valueType[length];
                for (int index = array.Length; length != 0; )
                {
                    if (map.Get(--index)) newValues[--length] = array[index];
                }
                return newValues;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>匹配数组</returns>
        public unsafe static arrayType[] getFindArray<valueType, arrayType>
            (this valueType[] array, func<valueType, bool> isValue, func<valueType, arrayType> getValue)
        {
            if (isValue == null || getValue == null) log.Default.Throw(log.exceptionType.Null);
            int length = array.length();
            if (length != 0)
            {
                length = (length + 7) >> 3;
                if (length <= config.pub.Default.StreamBufferLength)
                {
                    byte* data = stackalloc byte[length];
                    return GetFindArray(array, isValue, getValue, new fixedMap(data, length));
                }
                else
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataFixed = data) return GetFindArray(array, isValue, getValue, new fixedMap(dataFixed)); 
                }
            }
            return nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static arrayType[] GetFindArray<valueType, arrayType>
            (valueType[] array, func<valueType, bool> isValue, func<valueType, arrayType> getValue, fixedMap map)
        {
            int length = 0;
            for (int index = 0; index != array.Length; ++index)
            {
                if (isValue(array[index]))
                {
                    ++length;
                    map.Set(index);
                }
            }
            if (length != 0)
            {
                arrayType[] newValues = new arrayType[length];
                for (int index = array.Length; length != 0; )
                {
                    if (map.Get(--index)) newValues[--length] = getValue(array[index]);
                }
                return newValues;
            }
            return nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] sort<valueType>(this valueType[] array, func<valueType, valueType, int> comparer)
        {
            algorithm.quickSort.Sort(array, comparer);
            return array.notNull();
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] getSort<valueType>(this valueType[] array, func<valueType, valueType, int> comparer)
        {
            return algorithm.quickSort.GetSort(array, comparer);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        public static valueType[] sort<valueType>(this valueType[] array, func<valueType, valueType, int> comparer, int startIndex, int count)
        {
            algorithm.quickSort.Sort(array, comparer, startIndex, count);
            return array.notNull();
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的新数组</returns>
        public static valueType[] getSort<valueType>(this valueType[] array, func<valueType, valueType, int> comparer, int startIndex, int count)
        {
            return algorithm.quickSort.GetSort(array, comparer, startIndex, count);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection<valueType> rangeSort<valueType>
            (this valueType[] array, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return algorithm.quickSort.RangeSort(array, comparer, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        public static collection<valueType> getRangeSort<valueType>
            (this valueType[] array, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort(array, comparer, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection<valueType> rangeSort<valueType>
            (this valueType[] array, int startIndex, int count, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return algorithm.quickSort.RangeSort(array, startIndex, count, comparer, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        public static collection<valueType> getRangeSort<valueType>
            (this valueType[] array, int startIndex, int count, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return algorithm.quickSort.GetRangeSort(array, startIndex, count, comparer, skipCount, getCount);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection<valueType> pageSort<valueType>
            (this valueType[] array, func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return algorithm.quickSort.RangeSort(array, comparer, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        public static collection<valueType> getPageSort<valueType>
            (this valueType[] array, func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            array.page page = new array.page(array.length(), pageSize, currentPage);
            return algorithm.quickSort.GetRangeSort(array, comparer, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this valueType[] array, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (array.length() != 0)
            {
                value = array[0];
                foreach (valueType nextValue in array)
                {
                    if (comparer(nextValue, value) > 0) value = nextValue;
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
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (array.length() != 0)
            {
                value = array[0];
                if (array.Length != 1)
                {
                    keyType key = getKey(value);
                    foreach (valueType nextValue in array)
                    {
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) > 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
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
        /// <param name="array">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this valueType[] array, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            valueType value;
            return max(array, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return max(array, getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return max(array, getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return max(array, getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return max(array, getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this valueType[] array, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (array.length() != 0)
            {
                value = array[0];
                foreach (valueType nextValue in array)
                {
                    if (comparer(nextValue, value) < 0) value = nextValue;
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
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (array.length() != 0)
            {
                value = array[0];
                if (array.Length != 1)
                {
                    keyType key = getKey(value);
                    foreach (valueType nextValue in array)
                    {
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) < 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
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
        /// <param name="array">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this valueType[] array, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            valueType value;
            return min(array, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return min(array, getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return min(array, getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>(this valueType[] array, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return min(array, getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>
            (this valueType[] array, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return min(array, getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this valueType[] array, func<valueType, string> toString)
        {
            return string.Concat(array.getArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this valueType[] array, string join, func<valueType, string> toString)
        {
            return string.Join(join, array.getArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this valueType[] array, char join, func<valueType, string> toString)
        {
            return array.getArray(toString).joinString(join);
        }
    }
}
