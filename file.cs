using System;
using System.IO;
using fastCSharp.win32;

namespace fastCSharp
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class file
    {
        /// <summary>
        /// 完全限定文件名必须少于 260 个字符
        /// </summary>
        public const int MaxFullNameLength = 260;
        /// <summary>
        /// 默认簇字节大小
        /// </summary>
        private static readonly uint defaultBytesPerCluster = 1 << 12;
        /// <summary>
        /// 根据磁盘根目录获取簇字节大小
        /// </summary>
        /// <param name="bootPath">磁盘根目录，如@"C:\"</param>
        /// <returns>簇字节大小</returns>
        public static uint BytesPerCluster(string bootPath)
        {
            uint value = defaultBytesPerCluster;
            if (bootPath != null)
            {
                uint sectorsPerCluster, bytesPerSector, numberOfFreeClusters, totalNumberOfClusters;
                if (kernel32.GetDiskFreeSpace(bootPath, out sectorsPerCluster, out bytesPerSector, out numberOfFreeClusters, out totalNumberOfClusters))
                {
                    value = sectorsPerCluster * bytesPerSector;
                }
            }
            return value;
        }
    }
}
