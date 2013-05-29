using System;
using System.IO;
using System.IO.Compression;

namespace fastCSharp
{
    /// <summary>
    /// 流扩展操作
    /// </summary>
    public static class stream
    {
        /// <summary>
        /// 压缩类型
        /// </summary>
        public enum compression
        {
            None,
            Deflate,
            GZip,
        }
        /// <summary>
        /// 根据压缩类型获取压缩数据流
        /// </summary>
        /// <param name="dataStream">原始数据流</param>
        /// <param name="mode">压缩模式</param>
        /// <param name="leaveOpen">是否流保留为打开状态</param>
        /// <param name="type">压缩类型</param>
        /// <returns>数据流</returns>
        private static Stream toCompressStream(this Stream dataStream, CompressionMode mode, bool leaveOpen, compression type)
        {
            Stream stream = dataStream;
            if (type == compression.GZip) stream = (Stream)new GZipStream(dataStream, mode, leaveOpen);
            else stream = (Stream)new DeflateStream(dataStream, mode, leaveOpen);
            return stream;
        }
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="oldData">原始数据</param>
        /// <param name="type">压缩类型</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">压缩字节数</param>
        /// <returns>压缩后的数据,失败返回null</returns>
        public static byte[] getCompress(this byte[] oldData, compression type, int startIndex, int count)
        {
            array.range range = new array.range(oldData.length(), startIndex, count);
            if (count == range.GetCount)
            {
                using (MemoryStream dataStream = new MemoryStream())
                {
                    using (Stream compressStream = dataStream.toCompressStream(CompressionMode.Compress, true, type))
                    {
                        compressStream.Write(oldData, startIndex, count);
                    }
                    if (dataStream.Position < oldData.Length)
                    {
                        if (dataStream.Length != dataStream.Position) dataStream.SetLength(dataStream.Position);
                        return dataStream.ToArray();
                    }
                }
                return null;
            }
            else if (count == 0) return null;
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="compressData">压缩数据</param>
        /// <param name="type">压缩类型</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">解压缩字节数</param>
        /// <returns>解压缩后的数据</returns>
        public static memoryStream getDeCompressStream(this byte[] compressData, compression type, int startIndex, int count)
        {
            array.range range = new array.range(compressData.length(), startIndex, count);
            if (count == range.GetCount)
            {
                using (Stream memoryStream = new MemoryStream(compressData, 0, compressData.Length))
                using (Stream compressStream = memoryStream.toCompressStream(CompressionMode.Decompress, false, type))
                {
                    memoryStream dataStream = new memoryStream(compressData.Length);
                    byte[] buffer = new byte[config.pub.Default.StreamBufferLength];
                    for (int readLength = compressStream.Read(buffer, 0, config.pub.Default.StreamBufferLength);
                        readLength != 0;
                        readLength = compressStream.Read(buffer, 0, config.pub.Default.StreamBufferLength))
                    {
                        dataStream.Write(buffer, 0, readLength);
                    }
                    return dataStream;
                }
            }
            else if (count == 0) return null;
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="compressData">压缩数据</param>
        /// <param name="type">压缩类型</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">解压缩字节数</param>
        /// <returns>解压缩后的数据</returns>
        public static byte[] getDeCompress(this byte[] compressData, compression type, int startIndex, int count)
        {
            using (memoryStream stream = compressData.getDeCompressStream(type, startIndex, count))
            {
                return stream != null ? stream.ToArray() : nullValue<byte>.Array;
            }
        }
    }
}
