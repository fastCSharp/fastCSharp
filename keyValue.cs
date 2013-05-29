using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 键值对
    /// </summary>
    /// <typeparam name="keyType">键类型</typeparam>
    /// <typeparam name="valueType">值类型</typeparam>
    public struct keyValue<keyType, valueType>
    {
        /// <summary>
        /// 键
        /// </summary>
        public keyType Key;
        /// <summary>
        /// 值
        /// </summary>
        public valueType Value;
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public keyValue(keyType key, valueType value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 重置键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(keyType key,valueType value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 键值对隐式转换
        /// </summary>
        /// <param name="value">键值对</param>
        /// <returns>键值对</returns>
        public static implicit operator keyValue<keyType, valueType>(KeyValuePair<keyType, valueType> value)
        {
            return new keyValue<keyType, valueType>(value.Key, value.Value);
        }
        /// <summary>
        /// 键值对隐式转换
        /// </summary>
        /// <param name="value">键值对</param>
        /// <returns>键值对</returns>
        public static implicit operator KeyValuePair<keyType, valueType>(keyValue<keyType, valueType> value)
        {
            return new KeyValuePair<keyType, valueType>(value.Key, value.Value);
        }
    }
}
