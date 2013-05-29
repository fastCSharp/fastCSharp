using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float*/

namespace fastCSharp
{
    /// <summary>
    /// 可枚举相关扩展
    /// </summary>
    public static partial class iEnumerable
    {
        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getValue">数值获取器</param>
        /// <returns>数值求和</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ sum<valueType>
            (this System.Collections.Generic.IEnumerable<valueType> values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (getValue == null) log.Default.Throw(log.exceptionType.Null);
            /*Type[0]*/
            ulong/*Type[0]*/ value = 0;
            if (values != null)
            {
                foreach (valueType nextValue in values) value += getValue(nextValue);
            }
            return value;
        }
        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="values">数值集合</param>
        /// <returns>数值求和</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ sum(this System.Collections.Generic.IEnumerable</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (values != null)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ value = 0;
                foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values) value += nextValue;
                return value;
            }
            return 0;
        }
        /// <summary>
        /// 分组求和
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <returns>分组求和</returns>
        public static System.Collections.Generic.Dictionary<valueType, /*Type[0]*/ulong/*Type[0]*/> groupSum<valueType>
            (this System.Collections.Generic.IEnumerable<keyValue<valueType, /*Type[0]*/ulong/*Type[0]*/>> values)
        {
            if (values != null)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ sum;
                System.Collections.Generic.Dictionary<valueType, /*Type[0]*/ulong/*Type[0]*/> sums
                    = new System.Collections.Generic.Dictionary<valueType, /*Type[0]*/ulong/*Type[0]*/>();
                foreach (keyValue<valueType,/*Type[0]*/ulong/*Type[0]*/> value in values)
                {
                    if (sums.TryGetValue(value.Key, out sum)) sums[value.Key] = (/*Type[0]*/ulong/*Type[0]*/)(sum + value.Value);
                    else sums.Add(value.Key, value.Value);
                }
                return sums;
            }
            return null;
        }
        /// <summary>
        /// 分组求和
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键值类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>分组求和</returns>
        public static System.Collections.Generic.Dictionary<keyType, /*Type[0]*/ulong/*Type[0]*/> groupSum<valueType, keyType>
            (this System.Collections.Generic.IEnumerable<valueType> values, func<valueType, keyType> getKey
            , func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (values != null)
            {
                if (getKey == null || getValue == null) log.Default.Throw(log.exceptionType.Null);
                /*Type[0]*/
                ulong/*Type[0]*/ sum;
                System.Collections.Generic.Dictionary<keyType, /*Type[0]*/ulong/*Type[0]*/> sums
                    = new System.Collections.Generic.Dictionary<keyType, /*Type[0]*/ulong/*Type[0]*/>();
                foreach (valueType value in values)
                {
                    keyType key = getKey(value);
                    if (sums.TryGetValue(key, out sum)) sums[key] = (/*Type[0]*/ulong/*Type[0]*/)(sum + getValue(value));
                    else sums.Add(key, getValue(value));
                }
                return sums;
            }
            return null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <returns>字符串</returns>
        public static list<string> getString(this System.Collections.Generic.IEnumerable</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (values != null)
            {
                list<string> newValues = new list<string>();
                foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values) newValues.Add(number.toString(nextValue));
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.IEnumerable</*Type[0]*/ulong/*Type[0]*/> values)
        {
            return string.Concat(getString(values).toArray());
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.IEnumerable</*Type[0]*/ulong/*Type[0]*/> values, string join)
        {
            return string.Join(join, getString(values).toArray());
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.IEnumerable</*Type[0]*/ulong/*Type[0]*/> values, char join)
        {
            return getString(values).toArray().joinString(join);
        }
    }
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static partial class iCollection
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <returns>字符串</returns>
        public static string[] getString(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (values.count() != 0)
            {
                string[] newValues = new string[values.Count];
                int index = 0;
                foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values) newValues[index++] = number.toString(nextValue);
                return newValues;
            }
            return nullValue<string>.Array;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            return string.Concat(getString(values));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, string join)
        {
            return string.Join(join, getString(values));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, char join)
        {
            return getString(values).joinString(join);
        }
    }
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="getValue">数值获取器</param>
        /// <returns>数值求和</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ sum<valueType>(this valueType[] array, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ sum = 0;
            if (array != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in array) sum += getValue(value);
            }
            return sum;
        }
        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <returns>数值求和</returns>
        public unsafe static /*Type[0]*/ulong/*Type[0]*/ sum(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array != null)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ value = 0;
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length; start != end; value += *start++) ;
                }
                return value;
            }
            return 0;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <returns>字符串</returns>
        public unsafe static string[] getString(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array.length() != 0)
            {
                string[] newValues = new string[array.Length];
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* readValue = valueFixed;
                    for (int index = 0; index != newValues.Length; ++index)
                    {
                        newValues[index] = number.toString(*readValue++);
                    }
                }
                return newValues;
            }
            return nullValue<string>.Array;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <returns>字符串</returns>
        public unsafe static string[] getString(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count)
        {
            array.range range = new array.range(array.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                string[] newValues = new string[count];
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* readValue = valueFixed + range.SkipCount;
                    for (index = 0; index != count; newValues[index++] = number.toString(*readValue++)) ;
                }
                return newValues;
            }
            return nullValue<string>.Array;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <returns>字符串</returns>
        public static string joinString(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            return string.Concat(getString(array));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this /*Type[0]*/ulong/*Type[0]*/[] array, string join)
        {
            return string.Join(join, getString(array));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this /*Type[0]*/ulong/*Type[0]*/[] array, char join)
        {
            return getString(array).joinString(join);
        }
    }
    /// <summary>
    /// 单向动态数组扩展
    /// </summary>
    public static partial class list
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <returns>字符串</returns>
        public static string[] getString(this list</*Type[0]*/ulong/*Type[0]*/> list)
        {
            return list.count() != 0 ? list.Unsafer.Array.getString(0, list.Count) : nullValue<string>.Array;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <returns>字符串</returns>
        public static string joinString(this list</*Type[0]*/ulong/*Type[0]*/> list)
        {
            return string.Concat(getString(list));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this list</*Type[0]*/ulong/*Type[0]*/> list, string join)
        {
            return string.Join(join, getString(list));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString(this list</*Type[0]*/ulong/*Type[0]*/> list, char join)
        {
            return getString(list).joinString(join);
        }
    }
}
