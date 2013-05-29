using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using fastCSharp.win32;

namespace fastCSharp.diagnostics
{
    /// <summary>
    /// 作业
    /// </summary>
    public class job : IDisposable
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        internal enum informationType
        {
            /// <summary>
            /// 基本限制信息 JOBOBJECT_BASIC_LIMIT_INFORMATION
            /// </summary>
            JobObjectBasicLimitInformation = 2,
            /// <summary>
            /// 基本UI限制
            /// </summary>
            JobObjectBasicUIRestrictions = 4,
            /// <summary>
            /// 安全限制信息
            /// </summary>
            JobObjectSecurityLimitInformation = 5,
            /// <summary>
            /// 结束工作的时间信息
            /// </summary>
            JobObjectEndOfJobTimeInformation = 6,
            /// <summary>
            /// 完成端口信息
            /// </summary>
            JobObjectAssociateCompletionPortInformation = 7,
            /// <summary>
            /// 扩展限制信息 JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            /// </summary>
            JobObjectExtendedLimitInformation = 9,
            ///// <summary>
            ///// 组信息
            ///// </summary>
            //JobObjectGroupInformation = 11,
            ///// <summary>
            ///// 通知限制信息
            ///// </summary>
            //JobObjectNotificationLimitInformation = 12,
            ///// <summary>
            ///// 扩展组信息
            ///// </summary>
            //JobObjectGroupInformationEx = 14,
            ///// <summary>
            ///// CPU速率控制信息
            ///// </summary>
            //JobObjectCpuRateControlInformation = 15,
        }
        /// <summary>
        /// 扩展限制信息JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct extendedLimitInformation
        {
            /// <summary>
            /// 基本限制信息
            /// </summary>
            public basicLimitInformation BasicLimitInformation;
            /// <summary>
            /// IO计数限制(保留)
            /// </summary>
            public ioCounters IoInfo;
            /// <summary>
            /// 进程虚拟内存的限制,LimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY
            /// </summary>
            public uint ProcessMemoryLimit;
            /// <summary>
            /// 作业虚拟内存的限制,LimitFlags.JOB_OBJECT_LIMIT_JOB_MEMORY
            /// </summary>
            public uint JobMemoryLimit;
            /// <summary>
            /// 进程内存峰值
            /// </summary>
            public uint PeakProcessMemoryUsed;
            /// <summary>
            /// 作业内存峰值
            /// </summary>
            public uint PeakJobMemoryUsed;
        }
        /// <summary>
        /// 基本限制信息JOBOBJECT_BASIC_LIMIT_INFORMATION
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct basicLimitInformation
        {
            /// <summary>
            /// 进程用时限制(单位100纳秒),LimitFlags.JOB_OBJECT_LIMIT_PROCESS_TIME
            /// </summary>
            public long PerProcessUserTimeLimit;
            /// <summary>
            /// 作业用时限制(单位100纳秒),LimitFlags.JOB_OBJECT_LIMIT_JOB_TIME
            /// </summary>
            public long PerJobUserTimeLimit;
            /// <summary>
            /// 限制状态标识
            /// </summary>
            public limitFlags LimitFlags;
            /// <summary>
            /// 最小工作集,LimitFlags.JOB_OBJECT_LIMIT_WORKINGSET
            /// </summary>
            public UIntPtr MinimumWorkingSetSize;
            /// <summary>
            /// 最大工作集,LimitFlags.JOB_OBJECT_LIMIT_WORKINGSET
            /// </summary>
            public UIntPtr MaximumWorkingSetSize;
            /// <summary>
            /// 活动进程数量限制,LimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
            /// </summary>
            public uint ActiveProcessLimit;
            /// <summary>
            /// CUP亲和限制,必须调用GetProcessAffinityMask,LimitFlags.JOB_OBJECT_LIMIT_AFFINITY
            /// </summary>
            public UIntPtr Affinity;
            /// <summary>
            /// 优先级修改限制,LimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS
            /// </summary>
            public uint PriorityClass;
            /// <summary>
            /// 调度有效值为0到9,LimitFlags.JOB_OBJECT_LIMIT_SCHEDULING_CLASS
            /// </summary>
            public uint SchedulingClass;
        }
        /// <summary>
        /// 限制状态标识
        /// </summary>
        [Flags]
        public enum limitFlags : uint
        {
            /// <summary>
            /// 活动进程数量限制,JOBOBJECT_BASIC_LIMIT_INFORMATION.ActiveProcessLimit
            /// </summary>
            JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
            /// <summary>
            /// CUP亲和限制,JOBOBJECT_BASIC_LIMIT_INFORMATION.Affinity
            /// </summary>
            JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
            /// <summary>
            /// 子进程关联作业限制
            /// </summary>
            JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
            /// <summary>
            /// 异常处理限制
            /// </summary>
            JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
            /// <summary>
            /// 作业虚拟内存的限制,JOBOBJECT_EXTENDED_LIMIT_INFORMATION.JobMemoryLimit
            /// </summary>
            JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
            /// <summary>
            /// 作业用时限制,JOBOBJECT_BASIC_LIMIT_INFORMATION.PerJobUserTimeLimit
            /// </summary>
            JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
            /// <summary>
            /// 作业终止时关闭所有进程
            /// </summary>
            JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,
            /// <summary>
            /// 保留作业用时限制,与JOB_OBJECT_LIMIT_JOB_TIME冲突
            /// </summary>
            JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
            /// <summary>
            /// 优先级修改类型,JOBOBJECT_BASIC_LIMIT_INFORMATION.PriorityClass
            /// </summary>
            JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
            /// <summary>
            /// 进程虚拟内存的限制,JOBOBJECT_EXTENDED_LIMIT_INFORMATION.ProcessMemoryLimit
            /// </summary>
            JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
            /// <summary>
            /// 进程用时限制,JOBOBJECT_BASIC_LIMIT_INFORMATION.PerProcessUserTimeLimit
            /// </summary>
            JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
            /// <summary>
            /// 调度限制,JOBOBJECT_BASIC_LIMIT_INFORMATION.SchedulingClass
            /// </summary>
            JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
            /// <summary>
            /// 允许子进程不关联
            /// </summary>
            JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
            /// <summary>
            /// 最小工作集,最大工作集
            /// </summary>
            JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001,
            ///// <summary>
            ///// 允许CPU亲和进程子集
            ///// </summary>
            //JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 0x00004000
        }
        /// <summary>
        /// IO计数限制IO_COUNTERS
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ioCounters
        {
            /// <summary>
            /// 读取操作次数
            /// </summary>
            public ulong ReadOperationCount;
            /// <summary>
            /// 写入操作次数
            /// </summary>
            public ulong WriteOperationCount;
            /// <summary>
            /// 其它操作次数
            /// </summary>
            public ulong OtherOperationCount;
            /// <summary>
            /// 读取字节数
            /// </summary>
            public ulong ReadTransferCount;
            /// <summary>
            /// 写入字节数
            /// </summary>
            public ulong WriteTransferCount;
            /// <summary>
            /// 其它字节数
            /// </summary>
            public ulong OtherTransferCount;
        }

        /// <summary>
        /// 句柄
        /// </summary>
        private IntPtr handle;
        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool disposed;
        /// <summary>
        /// 作业
        /// </summary>
        public job()
        {
            handle = kernel32.CreateJobObject(IntPtr.Zero, null);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Close();
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// 关闭作业
        /// </summary>
        public void Close()
        {
            kernel32.CloseHandle(handle);
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// 添加进程
        /// </summary>
        /// <param name="process">进程</param>
        public void AddProcess(Process process)
        {
            if (!kernel32.AssignProcessToJobObject(handle, process.Handle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        /// <summary>
        /// 设置扩展限制信息
        /// </summary>
        /// <param name="limit">扩展限制信息</param>
        public void SetInformation(extendedLimitInformation limit)
        {
            int length = Marshal.SizeOf(typeof(extendedLimitInformation));
            IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(limit, extendedInfoPtr, false);
            if (!kernel32.SetInformationJobObject(handle, informationType.JobObjectExtendedLimitInformation, extendedInfoPtr, (uint)length))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
