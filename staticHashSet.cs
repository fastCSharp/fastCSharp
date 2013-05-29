using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希集合
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public class staticHashSet<valueType> : staticHash<valueType>
    {
        /// <summary>
        /// 静态哈希集合
        /// </summary>
        /// <param name="values">哈希数据集合</param>
        public unsafe staticHashSet(valueType[] values)
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
                array = nullValue<valueType>.Array;
            }
        }
        /// <summary>
        /// 静态哈希集合
        /// </summary>
        /// <param name="values">哈希数据集合</param>
        public staticHashSet(IEnumerable<valueType> values) : this(values.getArray()) { }
        /// <summary>
        /// 静态哈希集合
        /// </summary>
        /// <param name="values">哈希数据集合</param>
        public staticHashSet(ICollection<valueType> values) : this(values.getArray()) { }
        /// <summary>
        /// 静态哈希集合
        /// </summary>
        /// <param name="values">哈希数据集合</param>
        public staticHashSet(list<valueType> values) : this(values.toArray()) { }
        /// <summary>
        /// 静态哈希集合
        /// </summary>
        /// <param name="values">哈希数据集合</param>
        public staticHashSet(collection<valueType> values) : this(values.toArray()) { }
        /// <summary>
        /// 获取哈希数据数组
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="hashs">哈希集合</param>
        private unsafe void getValues(valueType[] values, int* hashs)
        {
            int* hash = hashs;
            foreach (valueType value in values) *hash++ = getHashCode(value);
            array = base.getValues(values, hashs);
        }
        /// <summary>
        /// 是否存在匹配数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在匹配数据</returns>
        public bool Contains(valueType value)
        {
            for (range range = indexs[getHashCode(value) & indexAnd]; range.StartIndex != range.EndIndex; ++range.StartIndex)
            {
                if (array[range.StartIndex].Equals(value)) return true;
            }
            return false;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">移除数据匹配值</param>
        /// <returns>是否存在被移除数据</returns>
        public bool Remove(valueType value)
        {
            return remove(value, out value);
        }
    }
}
