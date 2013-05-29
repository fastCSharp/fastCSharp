using System;
using System.Collections;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// IEnumerable泛型转换
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public struct iEnumerable<valueType> : IEnumerable<valueType>
    {
        /// <summary>
        /// IEnumerable数据集合
        /// </summary>
        private IEnumerable enumerable;
        /// <summary>
        /// IEnumerable泛型转换
        /// </summary>
        /// <param name="collection">ICollection数据集合</param>
        public iEnumerable(IEnumerable enumerable)
        {
            this.enumerable = enumerable;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (enumerable != null)
            {
                foreach (valueType value in enumerable) yield return value;
            }
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<valueType>)this).GetEnumerator();
        }
    }
    /// <summary>
    /// 可枚举相关扩展
    /// </summary>
    public static partial class iEnumerable
    {
        /// <summary>
        /// IEnumerable泛型转换
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>泛型数据集合</returns>
        public static iEnumerable<valueType> toGeneric<valueType>(this IEnumerable value)
        {
            return new iEnumerable<valueType>(value);
        }
        /// <summary>
        /// 单个数据转枚举器
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">数据值</param>
        /// <returns>枚举器</returns>
        public static IEnumerable<valueType> toEnumerable<valueType>(this valueType value)
        {
            yield return value;
        }
        /// <summary>
        /// 空值转数组
        /// </summary>
        /// <typeparam name="valueType">数据集合类型</typeparam>
        /// <param name="value">数据集合</param>
        /// <returns>非空数组</returns>
        public static IEnumerable<valueType> notNull<valueType>(this IEnumerable<valueType> value)
        {
            return value != null ? value : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        public static int count<valueType>(this IEnumerable<valueType> value)
        {
            if (value != null)
            {
                int count = 0;
                for (IEnumerator<valueType> enumerator = value.GetEnumerator(); enumerator.MoveNext(); ++count) ;
                return count;
            }
            return 0;
        }
        /// <summary>
        /// 转数组
        /// </summary>
        /// <typeparam name="valueType">数据集合类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <returns>数组</returns>
        public static valueType[] getArray<valueType>(this IEnumerable<valueType> values)
        {
            return values.getList().toArray();
        }
        /// <summary>
        /// 转单向动态数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据枚举集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> getList<valueType>(this IEnumerable<valueType> values)
        {
            if (values != null)
            {
                list<valueType> newValues = new list<valueType>();
                foreach (valueType value in values) newValues.Add(value);
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 转双向动态数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据枚举集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> getCollection<valueType>(this IEnumerable<valueType> values)
        {
            return values != null ? new collection<valueType>(getList(values)) : null;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="collectionType">集合类型</typeparam>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public static arrayType[] getArray<valueType, arrayType>
            (this IEnumerable<valueType> values, func<valueType, arrayType> getValue)
        {
            return values.getList(getValue).toArray();
        }
        /// <summary>
        /// 根据集合内容返回单向列表
        /// </summary>
        /// <typeparam name="collectionType">集合类型</typeparam>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>单向列表</returns>
        public static list<arrayType> getList<valueType, arrayType>
            (this IEnumerable<valueType> values, func<valueType, arrayType> getValue)
        {
            if (values != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                list<arrayType> value = new list<arrayType>();
                foreach (valueType nextValue in values) value.Add(getValue(nextValue));
                return value;
            }
            return null;
        }
        /// <summary>
        /// 根据集合内容返回双向列表
        /// </summary>
        /// <typeparam name="collectionType">集合类型</typeparam>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>双向列表</returns>
        public static collection<arrayType> getCollection<valueType, arrayType>
            (this IEnumerable<valueType> values, func<valueType, arrayType> getValue)
        {
            return values != null ? new collection<arrayType>(values.getList(getValue)) : null;
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="keyType">哈希键值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<keyType, valueType> getDictionary<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey)
        {
            if (values != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, valueType> dictionary = new Dictionary<keyType, valueType>();
                foreach (valueType value in values) dictionary[getKey(value)] = value;
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="keyType">哈希键值类型</typeparam>
        /// <typeparam name="dictionaryValueType">哈希值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="getValue">哈希值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<keyType, dictionaryValueType> getDictionary<valueType, keyType, dictionaryValueType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<valueType, dictionaryValueType> getValue)
        {
            if (values != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, dictionaryValueType> dictionary = new Dictionary<keyType, dictionaryValueType>();
                foreach (valueType value in values) dictionary[getKey(value)] = getValue(value);
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="keyType">哈希键值类型</typeparam>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="keys">键值集合</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<keyType, valueType> getDictionaryByKey<keyType, valueType>
            (this IEnumerable<keyType> keys, func<keyType, valueType> getValue)
        {
            if (keys != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, valueType> dictionary = new Dictionary<keyType, valueType>();
                foreach (keyType key in keys) dictionary[key] = getValue(key);
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>null为0</returns>
        public static int count<valueType>(this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            if (values != null)
            {
                int count = 0;
                foreach (valueType value in values)
                {
                    if (isValue(value)) ++count;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>第一个匹配值,失败返回default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            foreach (valueType value in values)
            {
                if (isValue(value)) return value;
            }
            return default(valueType);
        }
        /// <summary>
        /// 判断是否存在匹配值
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配值</returns>
        public static bool any<valueType>(this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            foreach (valueType value in values)
            {
                if (isValue(value)) return true;
            }
            return false;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static list<valueType> getFind<valueType>
            (this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            if (values != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                list<valueType> value = new list<valueType>();
                foreach (valueType nextValue in values)
                {
                    if (isValue(nextValue)) value.Add(nextValue);
                }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static IEnumerable<valueType> where<valueType>
            (this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            if (values != null)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                foreach (valueType value in values)
                {
                    if (isValue(value)) yield return value;
                }
            }
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static valueType[] getFindArray<valueType>
            (this IEnumerable<valueType> values, func<valueType, bool> isValue)
        {
            return values.getFind<valueType>(isValue).toArray();
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public static list<valueType> getSub<valueType>(this IEnumerable<valueType> values, int index, int count)
        {
            if (values != null)
            {
                if (index < 0 || count < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
                if (count != 0)
                {
                    list<valueType>.unsafer newValues = new list<valueType>(count).Unsafer;
                    foreach (valueType value in values)
                    {
                        if (index != 0) --index;
                        else
                        {
                            newValues.Add(value);
                            if (--count == 0) break;
                        }
                    }
                    return newValues.List;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this IEnumerable<valueType> values, func<valueType, valueType, int> comparer, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                int index = -1;
                foreach (valueType nextValue in values)
                {
                    if (++index == 0) value = nextValue;
                    else if (comparer(nextValue, value) > 0) value = nextValue;
                }
                if (index != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
                int index = -1;
                keyType key = default(keyType);
                foreach (valueType nextValue in values)
                {
                    if (++index == 0) key = getKey(value = nextValue);
                    else
                    {
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) > 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                if (index != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this IEnumerable<valueType> values, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            valueType value;
            return max(values, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>(this IEnumerable<valueType> values, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return max(values, getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return max(values, getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>(this IEnumerable<valueType> values, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return max(values, getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return max(values, getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this IEnumerable<valueType> values, func<valueType, valueType, int> comparer, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                int index = -1;
                foreach (valueType nextValue in values)
                {
                    if (++index == 0) value = nextValue;
                    else if (comparer(nextValue, value) < 0) value = nextValue;
                }
                if (index != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
                int index = -1;
                keyType key = default(keyType);
                foreach (valueType nextValue in values)
                {
                    if (++index == 0) key = getKey(value = nextValue);
                    else
                    {
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) < 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                if (index != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this IEnumerable<valueType> values, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            valueType value;
            return min(values, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>(this IEnumerable<valueType> values, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return min(values, getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return min(values, getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>(this IEnumerable<valueType> values, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return min(values, getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="values">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return min(values, getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 根据集合内容返回哈希表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>哈希表</returns>
        public static HashSet<valueType> getHash<valueType>(this IEnumerable<valueType> values)
        {
            if (values != null)
            {
                HashSet<valueType> newValues = new HashSet<valueType>();
                foreach (valueType value in values) newValues.Add(value);
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 根据集合内容返回哈希表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="hashType">返回哈希表类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>哈希表</returns>
        public static HashSet<hashType> getHash<valueType, hashType>
            (this IEnumerable<valueType> values, func<valueType, hashType> getValue)
        {
            if (values != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                HashSet<hashType> newValues = new HashSet<hashType>();
                foreach (valueType value in values) newValues.Add(getValue(value));
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 根据集合内容返回哈希表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="hashType">返回哈希表类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="hash">哈希表</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>哈希表</returns>
        public static HashSet<hashType> fillHash<valueType, hashType>
            (this IEnumerable<valueType> values, HashSet<hashType> hash, func<valueType, hashType> getValue)
        {
            if (values != null)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                if (hash == null) hash = new HashSet<hashType>();
                foreach (valueType value in values) hash.Add(getValue(value));
            }
            return hash;
        }
        /// <summary>
        /// 根据集合内容返回哈希表
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="hash">哈希表</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>哈希表</returns>
        public static HashSet<valueType> fillHash<valueType>(this IEnumerable<valueType> values, HashSet<valueType> hash)
        {
            if (values != null)
            {
                if (hash == null) hash = new HashSet<valueType>();
                foreach (valueType value in values) hash.Add(value);
            }
            return hash;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键值类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组数据</returns>
        public static Dictionary<keyType, list<valueType>> group<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey)
        {
            if (values != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, list<valueType>> newValues = new Dictionary<keyType, list<valueType>>();
                list<valueType> value;
                foreach (valueType nextValue in values)
                {
                    keyType key = getKey(nextValue);
                    if (!newValues.TryGetValue(key, out value)) newValues[key] = value = new list<valueType>();
                    value.Add(nextValue);
                }
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键值类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="isLeftValue">唯一值判定器</param>
        /// <returns>分组数据</returns>
        public static Dictionary<keyType, valueType> groupDistinct<valueType, keyType>
           (this IEnumerable<valueType> values, func<valueType, keyType> getKey, func<valueType, valueType, bool> isLeftValue)
        {
            if (values != null)
            {
                if (getKey == null || isLeftValue == null) log.Default.Throw(log.exceptionType.Null);
                Dictionary<keyType, valueType> newValues = new Dictionary<keyType, valueType>();
                valueType value;
                foreach (valueType nextValue in values)
                {
                    keyType key = getKey(nextValue);
                    if (newValues.TryGetValue(key, out value))
                    {
                        if (isLeftValue(nextValue, value)) newValues[key] = nextValue;
                    }
                    else newValues.Add(key, nextValue);
                }
                return newValues;
            }
            return null;
        }
        /// <summary>
        /// 分组计数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键值类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组计数</returns>
        public static Dictionary<keyType, int> groupCount<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey)
        {
            return values.groupCount(getKey, 0);
        }
        /// <summary>
        /// 分组计数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键值类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="capacity">集合容器大小</param>
        /// <returns>分组计数</returns>
        public static Dictionary<keyType, int> groupCount<valueType, keyType>
            (this IEnumerable<valueType> values, func<valueType, keyType> getKey, int capacity)
        {
            if (values != null)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                int count;
                Dictionary<keyType, int> dictionary = new Dictionary<keyType, int>(capacity > 0 ? capacity : 0);
                foreach (valueType value in values)
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
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this IEnumerable<valueType> values, func<valueType, string> toString)
        {
            return string.Concat(values.getArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this IEnumerable<valueType> values, string join, func<valueType, string> toString)
        {
            return string.Join(join, values.getArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this IEnumerable<valueType> values, char join, func<valueType, string> toString)
        {
            return values.getList(toString).joinString(join);
        }
    }
}
