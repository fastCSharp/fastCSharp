using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using fastCSharp.reflection;

namespace fastCSharp
{
    /// <summary>
    /// 动态数组信息
    /// </summary>
    internal static class dynamicArray
    {
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>需要清除数组</returns>
        public static bool IsClearArray(Type type)
        {
            if (type.IsPointer) return false;
            if (type.IsClass || type.IsInterface) return true;
            if (type.IsEnum) return false;
            if (type.IsValueType)
            {
                bool isClear;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref isClearArrayLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (isClearArrayCache.TryGetValue(hashType, out isClear)) return isClear;
                    isClearArrayCache.Add(hashType, isClear = isClearArray(type));
                }
                finally { isClearArrayLock = 0; }
                return isClear;
            }
            else
            {
                log.Default.Add(type.fullName(), false, false);
                return true;
            }
        }
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>需要清除数组</returns>
        private static bool isClearArray(Type type)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Type fieldType = field.FieldType;
                if (fieldType != type && !fieldType.IsPointer)
                {
                    if (fieldType.IsClass || fieldType.IsInterface) return true;
                    if (!fieldType.IsEnum)
                    {
                        if (fieldType.IsValueType)
                        {
                            bool isClear;
                            hashCode<Type> hashType = fieldType;
                            if (!isClearArrayCache.TryGetValue(hashType, out isClear))
                            {
                                isClearArrayCache.Add(hashType, isClear = isClearArray(fieldType));
                            }
                            if (isClear) return true;
                        }
                        else
                        {
                            log.Default.Add(fieldType.fullName(), false, false);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 是否需要清除数组缓存 访问锁
        /// </summary>
        private static int isClearArrayLock;
        /// <summary>
        /// 是否需要清除数组缓存信息
        /// </summary>
        private static readonly Dictionary<hashCode<Type>, bool> isClearArrayCache = new Dictionary<hashCode<Type>, bool>();
    }
    /// <summary>
    /// 动态数组基类
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class dynamicArray<valueType>
    {
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        protected static readonly bool isClearArray = dynamicArray.IsClearArray(typeof(valueType));
        /// <summary>
        /// 空枚举器
        /// </summary>
        protected class nullEnumerator : IEnumerator<valueType>
        {
            /// <summary>
            /// 当前数据元素
            /// </summary>
            valueType IEnumerator<valueType>.Current
            {
                get
                {
                    log.Default.Throw(log.exceptionType.IndexOutOfRange);
                    return default(valueType);
                }
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    log.Default.Throw(log.exceptionType.IndexOutOfRange);
                    return default(valueType);
                }
            }
            /// <summary>
            /// 转到下一个数据元素
            /// </summary>
            /// <returns>是否存在下一个数据元素</returns>
            public bool MoveNext()
            {
                return false;
            }
            /// <summary>
            /// 重置枚举器状态
            /// </summary>
            public void Reset() { }
            /// <summary>
            /// 释放枚举器
            /// </summary>
            public void Dispose() { }
        }
        /// <summary>
        /// 空枚举实例
        /// </summary>
        protected static readonly nullEnumerator defaultNullEnumerator = new nullEnumerator();
        /// <summary>
        /// 数组枚举器
        /// </summary>
        protected class arrayEnumerator : IEnumerator<valueType>
        {
            /// <summary>
            /// 被枚举数组
            /// </summary>
            private valueType[] array;
            /// <summary>
            /// 当前位置
            /// </summary>
            private int currentIndex;
            /// <summary>
            /// 结束位置
            /// </summary>
            private int endIndex;
            /// <summary>
            /// 起始位置
            /// </summary>
            private int startIndex;
            /// <summary>
            /// 数组枚举器
            /// </summary>
            /// <param name="value">双向动态数据</param>
            public arrayEnumerator(collection<valueType> value)
            {
                array = value.array;
                startIndex = value.StartIndex;
                endIndex = value.EndIndex;
                if (endIndex == 0) endIndex = array.Length;
                Reset();
            }
            /// <summary>
            /// 数组枚举器
            /// </summary>
            /// <param name="value">单向动态数据</param>
            public arrayEnumerator(list<valueType> value)
            {
                array = value.array;
                startIndex = 0;
                endIndex = value.Count;
                Reset();
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            valueType IEnumerator<valueType>.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            object IEnumerator.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 转到下一个数据元素
            /// </summary>
            /// <returns>是否存在下一个数据元素</returns>
            public bool MoveNext()
            {
                if (++currentIndex != endIndex) return true;
                --currentIndex;
                return false;
            }
            /// <summary>
            /// 重置枚举器状态
            /// </summary>
            public void Reset()
            {
                currentIndex = startIndex - 1;
            }
            /// <summary>
            /// 释放枚举器
            /// </summary>
            public void Dispose()
            {
                array = null;
            }
        }
        /// <summary>
        /// 创建新数组
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <returns>数组</returns>
        protected static valueType[] getNewArray(int length)
        {
            if (length > 0 && (length & (length - 1)) != 0) length = 1 << ((uint)length).bits();
            if (length <= 0) length = int.MaxValue;
            return new valueType[length];
        }

        /// <summary>
        /// 数据数组
        /// </summary>
        protected valueType[] array;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// 数据数量
        /// </summary>
        protected abstract int ValueCount { get; }
        
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        public abstract void Add(valueType[] values, int index, int count);
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(IEnumerable<valueType> values)
        {
            if (values != null) Add(values.getList());
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(valueType[] values)
        {
            if (values != null) Add(values, 0, values.Length);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="value">数据集合</param>
        public void Add(list<valueType> value)
        {
            if (value.count() != 0) Add(value.array, 0, value.Count);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        public void Add(list<valueType> value, int index, int count)
        {
            if (value.count() != 0) Add(value.array, index, count);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(valueType value)
        {
            int index = IndexOf(value);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否存在匹配
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配</returns>
        public bool Any(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            return indexOf(isValue) != -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public abstract int IndexOf(valueType value);
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public bool Contains(valueType value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(valueType)</returns>
        public valueType FirstOrDefault(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int index = indexOf(isValue);
            return index != -1 ? array[index] : default(valueType);
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配值集合</returns>
        protected abstract valueType[] getFindArray(func<valueType, bool> isValue, fixedMap map);
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public unsafe valueType[] GetFindArray(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int length = ValueCount;
            if (length != 0)
            {
                length = (length + 7) >> 3;
                if (length <= config.pub.Default.StreamBufferLength)
                {
                    byte* data = stackalloc byte[length];
                    return getFindArray(isValue, new fixedMap(data, length));
                }
                else
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataFixed = data) return getFindArray(isValue, new fixedMap(dataFixed));
                }
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public abstract int IndexOf(func<valueType, bool> isValue);
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public abstract valueType GetRemoveAt(int index);
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public abstract void RemoveAt(int index);
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        public void RemoveFirst(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int index = IndexOf(isValue);
            if (index != -1) RemoveAt(index);
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>被移除的数据元素,失败返回default(valueType)</returns>
        public valueType GetRemoveFirst(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int index = IndexOf(isValue);
            return index != -1 ? GetRemoveAt(index) : default(valueType);
        }
        /// <summary>
        /// 获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        protected abstract int indexOf(func<valueType, bool> isValue);
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="value">新数据</param>
        /// <param name="isValue">数据匹配器</param>
        public void ReplaceFirst(valueType value, func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int index = indexOf(isValue);
            if (index != -1) array[index] = value;
        }
        /// <summary>
        /// 获取数据范围
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>目标数据</returns>
        protected abstract arrayType[] getRange<arrayType>(int index, int count, func<valueType, arrayType> getValue);
        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>目标数据</returns>
        public arrayType[] GetPage<arrayType>(int pageSize, int currentPage, func<valueType, arrayType> getValue)
        {
            if (getValue == null) log.Default.Throw(log.exceptionType.Null);
            array.page page = new array.page(ValueCount, pageSize, currentPage);
            return page.SkipCount < page.Count ? getRange(page.SkipCount, page.CurrentPageSize, getValue) : nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public abstract bool Max(func<valueType, valueType, int> comparer, out valueType value);
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public abstract bool Max<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value);
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public valueType Max<keyType>(func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return Max(getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public valueType Max<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return Max(getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public keyType MaxKey<keyType>(func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return Max(getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public keyType MaxKey<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return Max(getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public abstract bool Min(func<valueType, valueType, int> comparer, out valueType value);
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public abstract bool Min<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value);
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public valueType Min<keyType>(func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return Min(getKey, (left, right) => left.CompareTo(right), out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public valueType Min<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            valueType value;
            return Min(getKey, comparer, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public keyType MinKey<keyType>(func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            valueType value;
            return Min(getKey, (left, right) => left.CompareTo(right), out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public keyType MinKey<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            valueType value;
            return Min(getKey, comparer, out value) ? getKey(value) : nullValue;
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public abstract arrayType[] GetArray<arrayType>(func<valueType, arrayType> getValue);
        /// <summary>
        /// 转换键值对集合
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <param name="getKey">键值获取器</param>
        /// <returns>键值对数组</returns>
        public abstract keyValue<keyType, valueType>[] GetKeyValueArray<keyType>(func<valueType, keyType> getKey);
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <returns>字符串</returns>
        public string JoinString(func<valueType, string> toString)
        {
            return string.Concat(GetArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public string JoinString(string join, func<valueType, string> toString)
        {
            return string.Join(join, GetArray(toString));
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        public string JoinString(char join, func<valueType, string> toString)
        {
            return GetArray(toString).joinString(join);
        }
    }
}
