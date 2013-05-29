using System;

namespace fastCSharp
{
    /// <summary>
    /// 用于HASH的字节数组
    /// </summary>
    public struct hashBytes : IEquatable<hashBytes>
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        private byte[] data;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int Length
        {
            get { return data.length(); }
        }
        /// <summary>
        /// HASH值
        /// </summary>
        private int hashCode;

        public static implicit operator hashBytes(byte[] data) { return new hashBytes(data); }
        public static implicit operator byte[](hashBytes value) { return value.data; }

        /// <summary>
        /// 字节数组HASH
        /// </summary>
        /// <param name="data">字节数组</param>
        public hashBytes(byte[] data)
        {
            this.data = data;
            hashCode = int.MinValue;
        }
        /// <summary>
        /// 获取HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            if (hashCode == int.MinValue)
            {
                hashCode = algorithm.hashCode.GetHashCode(data);
                if (hashCode == int.MinValue) hashCode = int.MaxValue;
                return hashCode;
            }
            return hashCode;
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">字节数组HASH</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object other)
        {
            return other != null && other.GetType() == typeof(hashBytes) && Equals((hashBytes)other);
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">用于HASH的字节数组</param>
        /// <returns>是否相等</returns>
        public bool Equals(hashBytes other)
        {
            return (hashCode == other.hashCode || hashCode == int.MinValue || other.hashCode == int.MinValue)
                && data.equal(other.data);
        }
    }

}
