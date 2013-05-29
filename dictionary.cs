using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 字典扩展操作
    /// </summary>
    public static class dictionary
    {
        /// <summary>
        /// 根据键值获取数据
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">字典</param>
        /// <param name="key">键值</param>
        /// <param name="nullValue">失败值</param>
        /// <returns>数据</returns>
        public static valueType get<keyType, valueType>
            (this Dictionary<keyType, valueType> values, keyType key, valueType nullValue = default(valueType))
        {
            valueType value;
            return values.TryGetValue(key, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取键值集合
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">字典</param>
        /// <returns>键值集合</returns>
        public static ICollection<keyType> keys<keyType, valueType>(this Dictionary<keyType, valueType> values)
        {
            return values != null ? values.Keys : null;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">字典</param>
        /// <returns>数据集合</returns>
        public static ICollection<valueType> values<keyType, valueType>(this Dictionary<keyType, valueType> values)
        {
            return values != null ? values.Values : null;
        }
    }
}
