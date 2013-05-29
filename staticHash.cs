using System;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希基类
    /// </summary>
    /// <typeparam name="hashType">数据类型</typeparam>
    public abstract class staticHash<valueType> : staticHashIndex
    {
        /// <summary>
        /// 哈希数据数组
        /// </summary>
        protected valueType[] array;
        /// <summary>
        /// 是否空集合
        /// </summary>
        /// <returns>是否空集合</returns>
        public unsafe bool IsEmpty()
        {
            fixed (range* indexFixed = indexs)
            {
                for (range* index = indexFixed + indexs.Length; index != indexFixed; )
                {
                    --index;
                    if ((*index).StartIndex != (*index).EndIndex) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否存在匹配数据
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配数据</returns>
        public unsafe bool IsExist(func<valueType, bool> isValue)
        {
            fixed (range* indexFixed = indexs)
            {
                for (range* index = indexFixed + indexs.Length; index != indexFixed; )
                {
                    for (range range = *--index; range.StartIndex != range.EndIndex; ++range.StartIndex)
                    {
                        if (isValue(array[range.StartIndex])) return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="keyType">哈希键类型</typeparam>
        /// <param name="key">哈希键值</param>
        /// <param name="value">被移除数据</param>
        /// <returns>是否存在被移除数据</returns>
        protected bool remove<keyType>(keyType key, out valueType value)
        {
            int index = (getHashCode(key) & indexAnd);
            for (range range = indexs[index]; range.StartIndex != range.EndIndex; ++range.StartIndex)
            {
                value = array[range.StartIndex];
                if (value.Equals(key))
                {
                    indexs[index].EndIndex = --range.EndIndex;
                    array[range.StartIndex] = array[range.EndIndex];
                    return true;
                }
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="keyType">哈希键类型</typeparam>
        /// <param name="key">哈希键值</param>
        /// <param name="getKey">哈希键值获取器</param>
        /// <param name="value">被移除数据</param>
        /// <returns>是否存在被移除数据</returns>
        protected bool remove<keyType>(keyType key, func<valueType, keyType> getKey, out valueType value)
        {
            int index = (getHashCode(key) & indexAnd);
            for (range range = indexs[index]; range.StartIndex != range.EndIndex; ++range.StartIndex)
            {
                value = array[range.StartIndex];
                if (getKey(value).Equals(key))
                {
                    indexs[index].EndIndex = --range.EndIndex;
                    array[range.StartIndex] = array[range.EndIndex];
                    return true;
                }
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 数据转换成单向动态数组
        /// </summary>
        /// <returns>单向动态数组</returns>
        public unsafe list<valueType> GetList()
        {
            list<valueType>.unsafer values = new list<valueType>(array.Length).Unsafer;
            fixed (range* indexFixed = indexs)
            {
                for (range* index = indexFixed + indexs.Length; index != indexFixed; )
                {
                    for (range range = *--index; range.StartIndex != range.EndIndex; ++range.StartIndex)
                    {
                        values.Add(array[range.StartIndex]);
                    }
                }
            }
            return values.List;
        }
        /// <summary>
        /// 获取匹配的数据集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配的数据集合</returns>
        protected unsafe list<valueType> getList(func<valueType, bool> isValue)
        {
            list<valueType>.unsafer values = new list<valueType>(array.Length).Unsafer;
            fixed (range* indexFixed = indexs)
            {
                for (range* index = indexFixed + indexs.Length; index != indexFixed; )
                {
                    for (range range = *--index; range.StartIndex != range.EndIndex; ++range.StartIndex)
                    {
                        valueType value = array[range.StartIndex];
                        if (isValue(value)) values.Add(value);
                    }
                }
            }
            return values.List;
        }
        /// <summary>
        /// 获取匹配的数据集合
        /// </summary>
        /// <typeparam name="listType">目标数据类型</typeparam>
        /// <param name="getValue">数据转换器</param>
        /// <returns>匹配的数据集合</returns>
        protected unsafe list<listType> getList<listType>(func<valueType, listType> getValue)
        {
            list<listType>.unsafer values = new list<listType>(array.Length).Unsafer;
            fixed (range* indexFixed = indexs)
            {
                for (range* index = indexFixed + indexs.Length; index != indexFixed; )
                {
                    for (range range = *--index; range.StartIndex != range.EndIndex; ++range.StartIndex)
                    {
                        values.Add(getValue(array[range.StartIndex]));
                    }
                }
            }
            return values.List;
        }
    }
}
