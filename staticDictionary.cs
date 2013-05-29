using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希字典
    /// </summary>
    /// <typeparam name="keyType">键值类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public class staticDictionary<keyType, valueType> : staticHash<keyValue<keyType, valueType>>
    {
        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <returns>数据值</returns>
        public valueType this[keyType key]
        {
            get
            {
                int index = indexOf(key);
                if (index == -1) log.Default.Throw(log.exceptionType.IndexOutOfRange);
                return array[index].Value;
            }
            set
            {
                int index = indexOf(key);
                if (index == -1) log.Default.Throw(log.exceptionType.IndexOutOfRange);
                array[index].Set(key, value);
            }
        }
        /// <summary>
        /// 匹配键值重置数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>是否存在匹配键值</returns>
        public bool Set(keyType key, func<valueType, valueType> getValue)
        {
            int index = indexOf(key);
            if (index != -1)
            {
                array[index].Set(key, getValue(array[index].Value));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>匹配数据,失败返回默认空值</returns>
        public valueType Get(keyType key, valueType nullValue)
        {
            int index = indexOf(key);
            return index != -1 ? array[index].Value : nullValue;
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在匹配数据</returns>
        public bool Get(keyType key, ref valueType value)
        {
            int index = indexOf(key);
            if (index != -1)
            {
                value = array[index].Value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 键值集合
        /// </summary>
        public list<keyType> Keys
        {
            get
            {
                return getList(value => value.Key);
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public list<valueType> Values
        {
            get
            {
                return getList(value => value.Value);
            }
        }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public unsafe staticDictionary(keyValue<keyType, valueType>[] values)
        {
            if (values.length() != 0)
            {
                if (values.Length <= (config.pub.Default.StreamBufferLength >> 2))
                {
                    int* hashFixed = stackalloc int[values.Length];
                    getValues(values, hashFixed);
                }
                else
                {
                    int[] hashs = new int[values.Length];
                    fixed (int* hashFixed = hashs) getValues(values, hashFixed);
                }
            }
            else
            {
                indexs = defaultIndexs;
                array = nullValue<keyValue<keyType, valueType>>.Array;
            }
        }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public staticDictionary(IEnumerable<keyValue<keyType, valueType>> values)
            : this(values.getArray()) { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public staticDictionary(ICollection<keyValue<keyType, valueType>> values)
            : this(values.getArray()) { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public staticDictionary(ICollection<KeyValuePair<keyType, valueType>> values)
            : this(values.getKeyValueArray()) { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public staticDictionary(list<keyValue<keyType, valueType>> values)
            : this(values.toArray()) { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public staticDictionary(collection<keyValue<keyType, valueType>> values)
            : this(values.toArray()) { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">哈希键值获取器</param>
        public staticDictionary(IEnumerable<valueType> values, func<valueType, keyType> getKey)
            : this(values.getArray(value => new keyValue<keyType, valueType>(getKey(value), value)))
        { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">哈希键值获取器</param>
        public staticDictionary(ICollection<valueType> values, func<valueType, keyType> getKey)
            : this(values.getKeyValueArray(getKey))
        { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">哈希键值获取器</param>
        public staticDictionary(list<valueType> values, func<valueType, keyType> getKey)
            : this(values.getKeyValueArray(getKey))
        { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">哈希键值获取器</param>
        public staticDictionary(collection<valueType> values, func<valueType, keyType> getKey)
            : this(values.getKeyValueArray(getKey))
        { }
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="getKey">哈希键值获取器</param>
        public staticDictionary(valueType[] values, func<valueType, keyType> getKey)
            : this(values.getKeyValueArray(getKey))
        { }
        /// <summary>
        /// 获取哈希数据数组
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="hashs">哈希集合</param>
        private unsafe void getValues(keyValue<keyType, valueType>[] values, int* hashs)
        {
            int* hash = hashs;
            foreach (keyValue<keyType, valueType> value in values) *hash++ = getHashCode(value.Key);
            array = base.getValues(values, hashs);
        }

        /// <summary>
        /// 获取键值匹配数组位置
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <returns>数组位置</returns>
        protected int indexOf(keyType key)
        {
            for (range range = indexs[getHashCode(key) & indexAnd]; range.StartIndex != range.EndIndex; ++range.StartIndex)
            {
                if (array[range.StartIndex].Key.Equals(key)) return range.StartIndex;
            }
            return -1;
        }
        /// <summary>
        /// 判断是否存在键值
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <returns>是否存在键值</returns>
        public bool ContainsKey(keyType key)
        {
            return indexOf(key) != -1;
        }
        /// <summary>
        /// 是否存在匹配数据
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配数据</returns>
        public bool ContainsValue(func<valueType, bool> isValue)
        {
            return IsExist(value => isValue(value.Value));
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="value">被移除数据</param>
        /// <returns>是否存在被移除数据</returns>
        public bool Remove(keyType key, out valueType value)
        {
            keyValue<keyType, valueType> keyValue;
            if (remove(key, nextValue => nextValue.Key, out keyValue))
            {
                value = keyValue.Value;
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取匹配的数据集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配的数据集合</returns>
        public list<keyValue<keyType, valueType>> GetList(func<valueType, bool> isValue)
        {
            return getList(value => isValue(value.Value));
        }
    }
}
