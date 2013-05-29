using System;

namespace fastCSharp.threading
{
    /// <summary>
    /// 局部唯一标识产生器
    /// </summary>
    public class identity
    {
        /// <summary>
        /// 当前值
        /// </summary>
        private int value;
        /// <summary>
        /// 获取下一个唯一标识值
        /// </summary>
        /// <returns>唯一标识值</returns>
        public int Next()
        {
            int newValue = value++;
            while (++newValue != value) newValue = value++;
            return newValue;
        }
        /// <summary>
        /// 默认唯一标识产生器
        /// </summary>
        public static readonly identity Default = new identity();
    }
}
