using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 字符串连接(适应于较长的字符串链接,一个一个字符添加的请用memoryStream)
    /// </summary>
    public class stringBuilder : list<string>
    {
        /// <summary>
        /// 初始化字符串连接
        /// </summary>
        public stringBuilder() : base() { }
        /// <summary>
        /// 初始化字符串连接
        /// </summary>
        /// <param name="length">字符串集合初始大小</param>
        public stringBuilder(int length) : base(length) { }
        /// <summary>
        /// 初始化字符串连接
        /// </summary>
        /// <param name="value">字符串值</param>
        public stringBuilder(string value)
            : base()
        {
            Add(value);
        }
        /// <summary>
        /// 初始化字符串连接
        /// </summary>
        /// <param name="value">字符串集合</param>
        public stringBuilder(params string[] value) : base(value, false) { }

        #region 添加字符串
        /// <summary>
        /// 添加字符
        /// </summary>
        /// <param name="value">字符值</param>
        public void Append(char value)
        {
            Add(new string(value, 1));
        }
        /// <summary>
        /// 添加字符
        /// </summary>
        /// <param name="value">字符值</param>
        /// <param name="count">字符数</param>
        public void Append(char value, int count)
        {
            if (count > 0) Add(new string(value, count));
        }
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <returns>当前字符串连接</returns>
        public void Append(params string[] values)
        {
            Add(values);
        }
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <returns>当前字符串连接</returns>
        public void AppendConcat(params string[] values)
        {
            Add(string.Concat(values));
        }
        #endregion

        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="joinValue">字符串连接</param>
        /// <returns>连接后的字符串</returns>
        public string Join(string joinValue)
        {
            return joinValue == null ? ToString() : string.Join(joinValue, this.ToArray());
        }
        #endregion

        #region 生成字符串
        /// <summary>
        /// 生成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return string.Concat(this.ToArray());
        }
        #endregion
    }
}
