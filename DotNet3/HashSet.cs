#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE

#if DOTNET35
#else
using System;
using System.Collections.Generic;
using fastCSharp;

namespace System.Collections.Generic
{
    /// <summary>
    /// 使用Dictionary模拟HashSet
    /// </summary>
    /// <typeparam name="valueType">值类型</typeparam>
    public class HashSet<valueType> : ICollection<valueType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private Dictionary<valueType, bool> dictionary;
        /// <summary>
        /// 哈希集合
        /// </summary>
        public HashSet()
        {
            dictionary = new Dictionary<valueType, bool>();
        }
        /// <summary>
        /// 哈希集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public HashSet(IEnumerable<valueType> values)
        {
            dictionary = new Dictionary<valueType, bool>();
            if (values != null)
            {
                foreach (valueType value in values) dictionary[value] = false;
            }
        }
        /// <summary>
        /// 哈希集合
        /// </summary>
        /// <param name="comparer">比较器</param>
        public HashSet(IEqualityComparer<valueType> comparer)
        {
            dictionary = new Dictionary<valueType, bool>(comparer);
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear() { dictionary.Clear(); }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public bool Add(valueType value)
        {
            if (dictionary.ContainsKey(value)) return false;
            dictionary.Add(value, false);
            return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        void ICollection<valueType>.Add(valueType value)
        {
            Add(value);
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否包含数据</returns>
        public bool Contains(valueType value)
        {
            return dictionary.ContainsKey(value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在被删除数据</returns>
        public bool Remove(valueType value)
        {
            return dictionary.Remove(value);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="array">目标数组</param>
        public void CopyTo(valueType[] array)
        {
            dictionary.Keys.copyTo(array, 0);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="index">目标数组起始位置</param>
        public void CopyTo(valueType[] array, int index)
        {
            dictionary.Keys.copyTo(array, index);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="index">目标数组起始位置</param>
        /// <param name="count">复制数据数量</param>
        public void CopyTo(valueType[] array, int index, int count)
        {
            dictionary.Keys.copyTo(array, index, count);
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            foreach (KeyValuePair<valueType, bool> value in dictionary)
            {
                yield return value.Key;
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
}
#endif