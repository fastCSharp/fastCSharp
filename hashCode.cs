using System;

namespace fastCSharp
{
    /// <summary>
    /// 引用哈希值
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public struct hashCode<valueType> : IEquatable<hashCode<valueType>> where valueType : class
    {
        /// <summary>
        /// 数据
        /// </summary>
        private valueType value;
        /// <summary>
        /// 哈希值
        /// </summary>
        private int hashValue;
        /// <summary>
        /// 引用哈希值
        /// </summary>
        /// <param name="value">数据,不能为null</param>
        public hashCode(valueType value)
        {
            this.value = value;
            hashValue = value.GetHashCode();
            hashValue ^= hashValue >> 3;
        }
        /// <summary>
        /// 数据隐式转换为哈希值
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>哈希值</returns>
        public static implicit operator hashCode<valueType>(valueType value) { return new hashCode<valueType>(value); }
        /// <summary>
        /// 哈希值隐式转换为数据
        /// </summary>
        /// <param name="value">哈希值</param>
        /// <returns>数据</returns>
        public static implicit operator valueType(hashCode<valueType> value) { return value.value; }
        /// <summary>
        /// 哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return hashValue;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object other)
        {
            return other != null && other.GetType() == typeof(hashCode<valueType>) && Equals((hashCode<valueType>)other);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">比较对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(hashCode<valueType> other)
        {
            return hashValue == other.hashValue && value == other.value;
        }
    }
}
