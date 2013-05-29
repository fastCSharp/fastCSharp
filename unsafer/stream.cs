using System;
using System.IO;

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 流扩展操作(非安全,请自行确保数据可靠性)
    /// </summary>
    public static class stream
    {
        /// <summary>
        /// 32位整数0的字节数组
        /// </summary>
        static readonly byte[] IntZore = BitConverter.GetBytes((int)0);
        /// <summary>
        /// 数据流写入32位整数0
        /// </summary>
        /// <param name="stream">数据流</param>
        public static void WirteIntZore(Stream stream)
        {
            stream.Write(IntZore, 0, sizeof(int));
        }
    }
}
