using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;DateTime*/

namespace fastCSharp
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static partial class iCollection
    {
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns>数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getArray
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (values.count() != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    foreach (/*Type[0]*/ulong/*Type[0]*/ value in values) *write++ = value;
                }
                return newValues;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (values.count() != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    foreach (valueType value in values) *write++ = getValue(value);
                }
                return newValues;
            }
            return nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 根据集合内容返回单向列表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>单向列表</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> getList<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            return values != null ? new list</*Type[0]*/ulong/*Type[0]*/>(getArray(values, getValue), true) : null;
        }
        /// <summary>
        /// 根据集合内容返回单向列表
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns>单向列表</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> getList(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            return values != null ? new list</*Type[0]*/ulong/*Type[0]*/>(getArray(values), true) : null;
        }
        /// <summary>
        /// 根据集合内容返回双向列表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>双向列表</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getCollection<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            return values != null ? new collection</*Type[0]*/ulong/*Type[0]*/>(getArray(values, getValue), true) : null;
        }
        /// <summary>
        /// 根据集合内容返回双向列表
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns>双向列表</returns>
        public static collection</*Type[0]*/ulong/*Type[0]*/> getCollection
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            return values != null ? new collection</*Type[0]*/ulong/*Type[0]*/>(getArray(values), true) : null;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> getFind
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (values.count() != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    foreach (/*Type[0]*/ulong/*Type[0]*/ value in values)
                    {
                        if (isValue(value)) *write++ = value;
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> getFind
            (this System.Collections.ICollection values, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (values != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = newValueFixed;
                    foreach (/*Type[0]*/ulong/*Type[0]*/ value in values)
                    {
                        if (isValue(value)) *write++ = value;
                    }
                    return new list</*Type[0]*/ulong/*Type[0]*/>(newValues, 0, (int)(write - newValueFixed), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return values.getFind(isValue).toArray();
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray
            (this System.Collections.ICollection values, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return values.getFind(isValue).toArray();
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (values.count() != 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values)
                {
                    if (nextValue > value) value = nextValue;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ max
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return max(values, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out valueType value)
        {
            if (values.count() != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                value = default(valueType);
                int count = -1;
                /*Type[0]*/
                ulong/*Type[0]*/ key = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/ nextKey = getKey(nextValue);
                        if (nextKey > key)
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
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , valueType nullValue)
        {
            valueType value;
            return max(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="key">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out /*Type[0]*/ulong/*Type[0]*/ key)
        {
            if (values.count() != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                valueType value = default(valueType);
                int count = -1;
                key = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/ nextKey = getKey(nextValue);
                        if (nextKey > key)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                return true;
            }
            key = default(/*Type[0]*/ulong/*Type[0]*/);
            return false;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ maxKey<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return maxKey(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (values.count() != 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values)
                {
                    if (nextValue < value) value = nextValue;
                }
                return true;
            }
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ min
            (this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return min(values, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out valueType value)
        {
            if (values.count() != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                value = default(valueType);
                int count = -1;
                /*Type[0]*/
                ulong/*Type[0]*/ key = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/ nextKey = getKey(nextValue);
                        if (nextKey < key)
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
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , valueType nullValue)
        {
            valueType value;
            return min(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="key">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , out /*Type[0]*/ulong/*Type[0]*/ key)
        {
            if (values.count() != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                valueType value = default(valueType);
                int count = -1;
                key = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/ nextKey = getKey(nextValue);
                        if (nextKey < key)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                return true;
            }
            key = default(/*Type[0]*/ulong/*Type[0]*/);
            return false;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ minKey<valueType>
            (this System.Collections.Generic.ICollection<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey
            , /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return minKey(values, getKey, out value) ? value : nullValue;
        }
    }
}
