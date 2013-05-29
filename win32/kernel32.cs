using System;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;
using fastCSharp.win32.safeHandles;

namespace fastCSharp.win32
{
    /// <summary>
    /// kernel32.dll API
    /// </summary>
    internal static class kernel32
    {
        /// <summary>
        /// 错误句柄值
        /// </summary>
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <summary>
        /// 获取指定磁盘的信息，包括磁盘的可用空间。
        /// </summary>
        /// <param name="bootPath">磁盘根目录。如：@"C:\"</param>
        /// <param name="sectorsPerCluster">每个簇所包含的扇区个数</param>
        /// <param name="bytesPerSector">每个扇区所占的字节数</param>
        /// <param name="numberOfFreeClusters">空闲的簇的个数</param>
        /// <param name="totalNumberOfClusters">簇的总个数</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool GetDiskFreeSpace(string bootPath, out uint sectorsPerCluster, out uint bytesPerSector, out uint numberOfFreeClusters, out uint totalNumberOfClusters);
        /// <summary>
        /// 创建进程
        /// </summary>
        /// <param name="lpApplicationName">应用名称,可以null</param>
        /// <param name="lpCommandLine">命令行</param>
        /// <param name="lpProcessAttributes">进程安全属性</param>
        /// <param name="lpThreadAttributes">线程安全属性</param>
        /// <param name="bInheritHandles">是否继承句柄</param>
        /// <param name="dwCreationFlags">创建状态标识</param>
        /// <param name="lpEnvironment">环境变量</param>
        /// <param name="lpCurrentDirectory">工作目录</param>
        /// <param name="lpStartupInfo">启动信息</param>
        /// <param name="lpProcessInformation">进程信息</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool CreateProcess([MarshalAs(UnmanagedType.LPTStr)] string lpApplicationName, string lpCommandLine, securityAttributes lpProcessAttributes, securityAttributes lpThreadAttributes, bool bInheritHandles, diagnostics.process.createFlags dwCreationFlags, IntPtr lpEnvironment, [MarshalAs(UnmanagedType.LPTStr)] string lpCurrentDirectory, diagnostics.process.startupInfo lpStartupInfo, diagnostics.process.processInformation lpProcessInformation);
        /// <summary>
        /// 唤醒线程
        /// </summary>
        /// <param name="hThread">线程句柄</param>
        /// <returns>负数表示失败</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int ResumeThread(IntPtr hThread);
        /// <summary>
        /// 创建通道
        /// </summary>
        /// <param name="hReadPipe">读取通道</param>
        /// <param name="hWritePipe">写入通道</param>
        /// <param name="lpPipeAttributes">安全属性</param>
        /// <param name="nSize">缓存区字节数,0表示默认</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, securityAttributes lpPipeAttributes, int nSize);
        /// <summary>
        /// 复制句柄
        /// </summary>
        /// <param name="hSourceProcessHandle">被复制进程句柄,必须有PROCESS_DUP_HANDLE权限</param>
        /// <param name="hSourceHandle">被复制句柄</param>
        /// <param name="hTargetProcess">目标进程句柄，必须有PROCESS_DUP_HANDLE权限</param>
        /// <param name="targetHandle">目标句柄</param>
        /// <param name="dwDesiredAccess">访问权限标识</param>
        /// <param name="bInheritHandle">是否继承</param>
        /// <param name="dwOptions">可选参数</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeFileHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, duplicateHandleOptions dwOptions);
        /// <summary>
        /// 获取当前进程句柄
        /// </summary>
        /// <returns>当前进程句柄</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetCurrentProcess();
        /// <summary>
        /// 获取控制台输入编码
        /// </summary>
        /// <returns>Encoding编码</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetConsoleCP();
        /// <summary>
        /// 获取控制台输出编码
        /// </summary>
        /// <returns>Encoding编码</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetConsoleOutputCP();
        /// <summary>
        /// 获取标准输入输出句柄
        /// </summary>
        /// <param name="whichHandle"></param>
        /// <returns>句柄</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetStdHandle(standardHandle whichHandle);
        /// <summary>
        /// 进程附加到作业
        /// </summary>
        /// <param name="job">作业句柄</param>
        /// <param name="process">进程句柄</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);
        /// <summary>
        /// 设置作业信息
        /// </summary>
        /// <param name="hJob">作业句柄</param>
        /// <param name="infoType">信息类型</param>
        /// <param name="lpJobObjectInfo">信息句柄</param>
        /// <param name="cbJobObjectInfoLength">信息字节长度</param>
        /// <returns>是否成功</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetInformationJobObject(IntPtr hJob, diagnostics.job.informationType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

        /// <summary>
        /// 释放句柄
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <returns>是否成功</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        internal static extern bool CloseHandle(IntPtr handle);
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="hMem">内存句柄</param>
        /// <returns>IntPtr.Zero表示成功</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("kernel32.dll")]
        internal static extern IntPtr LocalFree(IntPtr hMem);
        /// <summary>
        /// 创建作业
        /// </summary>
        /// <param name="lpJobAttributes">安全属性SECURITY_ATTRIBUTES</param>
        /// <param name="lpName">作业名称</param>
        /// <returns>作业句柄</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        /// <summary>
        /// 复制句柄可选参数
        /// </summary>
        [Flags]
        internal enum duplicateHandleOptions : int
        {
            NONE = 0,
            /// <summary>
            /// 关闭源句柄
            /// </summary>
            DUPLICATE_CLOSE_SOURCE = 1,
            /// <summary>
            /// 忽略dwDesiredAccess，设置同样的访问权限
            /// </summary>
            DUPLICATE_SAME_ACCESS = 2
        }
        /// <summary>
        /// 获取标准输入输出句柄类型
        /// </summary>
        internal enum standardHandle : int
        {
            /// <summary>
            /// 标准输入
            /// </summary>
            StandardInput = -10,
            /// <summary>
            /// 标准输出
            /// </summary>
            StandardOutput = -11,
            /// <summary>
            /// 标准错误
            /// </summary>
            StandardError = -12,
        }

        /// <summary>
        /// 安全属性SECURITY_ATTRIBUTES
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class securityAttributes
        {
            /// <summary>
            /// securityAttributes所占内存大小
            /// </summary>
            public int Size;
            /// <summary>
            /// 安全描述
            /// </summary>
            public safeLocalMemHandle SecurityDescriptor = new safeLocalMemHandle(IntPtr.Zero, false);
            /// <summary>
            /// 是否继承句柄
            /// </summary>
            public bool IsInheritHandle;
            /// <summary>
            /// 安全属性
            /// </summary>
            public securityAttributes()
            {
                Size = Marshal.SizeOf(typeof(securityAttributes));
            }
        }
    }
}
