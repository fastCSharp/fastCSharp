using System;
using System.Runtime.InteropServices;

namespace fastCSharp.win32
{
    /// <summary>
    /// psapi.dll API
    /// </summary>
    internal static class psapi
    {
        /// <summary>
        /// 获取进程内存统计信息
        /// </summary>
        /// <param name="hProcess">进程句柄</param>
        /// <param name="counters">进程内存统计信息</param>
        /// <param name="size">进程内存统计信息字节数</param>
        /// <returns>是否获取成功</returns>
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern bool GetProcessMemoryInfo(IntPtr hProcess, out diagnostics.process.memoryCounters counters, uint size);
    }
}
