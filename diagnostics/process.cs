using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using Microsoft.Win32.SafeHandles;
using fastCSharp.reflection;
using fastCSharp.win32;
using fastCSharp.win32.safeHandles;
#if CPU64
using cpuUint = System.UInt64;
#else
using cpuUint = System.UInt32;
#endif

namespace fastCSharp.diagnostics
{
    /// <summary>
    /// 进程扩展
    /// </summary>
    public class process : Process
    {
        /// <summary>
        /// 进程类型所在程序集
        /// </summary>
        private static readonly Assembly assembly = typeof(Process).Assembly;
        /// <summary>
        /// Microsoft.Win32.SafeHandles.SafeProcessHandle类信息
        /// </summary>
        private static readonly Type safeProcessHandle = assembly.GetType("Microsoft.Win32.SafeHandles.SafeProcessHandle");
        /// <summary>
        /// Microsoft.Win32.SafeHandles.SafeProcessHandle类名全称
        /// </summary>
        private static readonly string safeProcessHandleFullName = safeProcessHandle.FullName;
        /// <summary>
        /// 设置进程句柄
        /// </summary>
        private static readonly action<object, IntPtr> processInitialSetHandle = methodDelegate.Create<IntPtr>(assembly.GetType("Microsoft.Win32.SafeHandles.SafeProcessHandle").GetMethod("InitialSetHandle", BindingFlags.Instance | BindingFlags.NonPublic));
        /// <summary>
        /// 句柄是否有效
        /// </summary>
        private static readonly func<object, bool> handleIsInvalid = propertyGetter<bool>.Creator.Create(typeof(System.Runtime.InteropServices.SafeHandle).Assembly, "Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid", "IsInvalid", false);
        /// <summary>
        /// 环境变量
        /// </summary>
        private static readonly func<object, StringDictionary> startInfoEnvironmentVariables = fieldGetValue.createFieldGetValue<StringDictionary>.Default.Create(typeof(ProcessStartInfo), "environmentVariables");
        /// <summary>
        /// 环境变量转字节
        /// </summary>
        private static readonly func<StringDictionary, bool, byte[]> environmentBlockToByteArray = staticMethodDelegate.Create<StringDictionary, bool, byte[]>(assembly.GetType("System.Diagnostics.EnvironmentBlock").GetMethod("ToByteArray", BindingFlags.Static | BindingFlags.Public));
        /// <summary>
        /// 是否WIN NT系统
        /// </summary>
        private static readonly func<bool> processManagerIsNt = propertyGetter<bool>.StaticCreator.Create(assembly, "System.Diagnostics.ProcessManager", "IsNt", false);
        /// <summary>
        /// 进程是否已经释放
        /// </summary>
        private static readonly func<object, bool> disposed = fieldGetValue.createFieldGetValue<bool>.Default.Create(typeof(Process), "disposed");
        /// <summary>
        /// 进程标准输入
        /// </summary>
        private static readonly action<object, StreamWriter> standardInput = fieldSetValue.createFieldSetValue<StreamWriter>.Default.Create(typeof(Process), "standardInput");
        /// <summary>
        /// 进程标准输出
        /// </summary>
        private static readonly action<object, StreamReader> standardOutput = fieldSetValue.createFieldSetValue<StreamReader>.Default.Create(typeof(Process), "standardOutput");
        /// <summary>
        /// 进程标准错误
        /// </summary>
        private static readonly action<object, StreamReader> standardError = fieldSetValue.createFieldSetValue<StreamReader>.Default.Create(typeof(Process), "standardError");
        /// <summary>
        /// 设置进程句柄
        /// </summary>
        private static readonly action<Process, object> setProcessHandle = methodTargetDelegate<Process>.Create(typeof(Process).GetMethod("SetProcessHandle", BindingFlags.Instance | BindingFlags.NonPublic), safeProcessHandle);
        /// <summary>
        /// 设置进程ID
        /// </summary>
        private static readonly action<Process, int> setProcessId = methodTargetDelegate<Process>.Create<int>(typeof(Process).GetMethod("SetProcessId", BindingFlags.Instance | BindingFlags.NonPublic));

        /// <summary>
        /// 创建状态标识
        /// </summary>
        [Flags]
        public enum createFlags : uint
        {
            /// <summary>
            /// 创建一个调试根进程，可调试其所有子进程(当不设置DEBUG_ONLY_THIS_PROCESS时)，可以使用WaitForDebugEvent接受所有调试事件
            /// </summary>
            DEBUG_PROCESS = 0x1,
            /// <summary>
            /// 创建一个调试进程，可以使用WaitForDebugEvent接受所有调试事件
            /// </summary>
            DEBUG_ONLY_THIS_PROCESS = 0x2,
            /// <summary>
            /// 创建一个暂停的进程，需要调用ResumeThread激活
            /// </summary>
            CREATE_SUSPENDED = 0x4,
            /// <summary>
            /// 创建一个不继承控制台的进程，新进程调用AllocConsole创建控制台，不能和CREATE_NEW_CONSOLE同时使用
            /// </summary>
            DETACHED_PROCESS = 0x8,
            /// <summary>
            /// 创建一个新的控制台(不继承控制台)，不能和DETACHED_PROCESS同时使用
            /// </summary>
            CREATE_NEW_CONSOLE = 0x10,
            /// <summary>
            /// 创建一个新的根进程，Ctrl+C将被禁用，如果设置了CREATE_NEW_CONSOLE将忽略此标识
            /// </summary>
            CREATE_NEW_PROCESS_GROUP = 0x200,
            /// <summary>
            /// 环境变量使用Unicode，否则使用ANSI
            /// </summary>
            CREATE_UNICODE_ENVIRONMENT = 0x400,
            /// <summary>
            /// 创建一个运行于DOS虚拟机的进程，16位windows程序才有效
            /// </summary>
            CREATE_SEPARATE_WOW_VDM = 0x800,
            /// <summary>
            /// 创建一个运行于共享DOS虚拟机的进程，当WIN.INI的DefaultSeparateVDM为TRUE时此标识覆盖此设置，16位windows程序才有效
            /// </summary>
            CREATE_SHARED_WOW_VDM = 0x1000,
            /// <summary>
            /// 继承CPU亲和性，不支持win2008,Vista,win2003,XP
            /// </summary>
            INHERIT_PARENT_AFFINITY = 0x10000,
            /// <summary>
            /// 创建一个受保护的进程，不支持win2003与XP
            /// </summary>
            CREATE_PROTECTED_PROCESS = 0x40000,
            /// <summary>
            /// 创建扩展的启动信息，lpStartupInfo使用STARTUPINFOEX，不支持win2003与XP
            /// </summary>
            EXTENDED_STARTUPINFO_PRESENT = 0x80000,
            /// <summary>
            /// 进程将附加到作业
            /// </summary>
            CREATE_BREAKAWAY_FROM_JOB = 0x1000000,
            /// <summary>
            /// 允许绕过限制调用子进程
            /// </summary>
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x2000000,
            /// <summary>
            /// 不继承错误处理模式
            /// </summary>
            CREATE_DEFAULT_ERROR_MODE = 0x4000000,
            /// <summary>
            /// 创建没有窗口的控制台应用，如果设置了CREATE_NEW_CONSOLE或者DETACHED_PROCESS将忽略此标识
            /// </summary>
            CREATE_NO_WINDOW = 0x8000000
        }
        /// <summary>
        /// 进程启动状态标识
        /// </summary>
        [Flags]
        internal enum startupInfoFlags : uint
        {
            /// <summary>
            /// ShowWindow包含额外信息
            /// </summary>
            STARTF_USESHOWWINDOW = 0x1,
            /// <summary>
            /// 窗口像素宽度设置
            /// </summary>
            STARTF_USESIZE = 0x2,
            /// <summary>
            /// 相对于屏幕左上角横坐标像素设置
            /// </summary>
            STARTF_USEPOSITION = 0x4,
            /// <summary>
            /// 屏幕缓存区字符设置
            /// </summary>
            STARTF_USECOUNTCHARS = 0x8,
            /// <summary>
            /// 文本与背景颜色设置
            /// </summary>
            STARTF_USEFILLATTRIBUTE = 0x10,
            /// <summary>
            /// 全屏模式，仅适用于x86的控制台程序
            /// </summary>
            STARTF_RUNFULLSCREEN = 0x20,
            /// <summary>
            /// CreateProcess两秒后启动光标，调用GetMessage
            /// </summary>
            STARTF_FORCEONFEEDBACK = 0x40,
            /// <summary>
            /// 正常显示光标,关闭STARTF_FORCEONFEEDBACK
            /// </summary>
            STARTF_FORCEOFFFEEDBACK = 0x80,
            /// <summary>
            /// 标准输入输出错误包含额外信息，比如重定向
            /// </summary>
            STARTF_USESTDHANDLES = 0x100,
            /// <summary>
            /// StandardInput包含额外信息，不能与STARTF_USESTDHANDLES同时使用
            /// </summary>
            STARTF_USEHOTKEY = 0x200,
            /// <summary>
            /// Title包含快捷方式文件.lnk，不能与STARTF_TITLEISAPPID同时使用
            /// </summary>
            STARTF_TITLEISLINKNAME = 0x800,
            /// <summary>
            /// Title包含AppUserModelID，不能与STARTF_TITLEISLINKNAME同时使用
            /// </summary>
            STARTF_TITLEISAPPID = 0x1000,
            /// <summary>
            /// 不能固定在任务栏上，需要结合STARTF_TITLEISAPPID
            /// </summary>
            STARTF_PREVENTPINNING = 0x2000
        }
        /// <summary>
        /// 进程启动信息STARTUPINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class startupInfo
        {
            /// <summary>
            /// processStartupInfo所占内存大小
            /// </summary>
            public int Size;
            /// <summary>
            /// 保留，必须为IntPtr.Zero
            /// </summary>
            public IntPtr Reserved = IntPtr.Zero;
            /// <summary>
            /// 桌面窗口名称
            /// </summary>
            public IntPtr Desktop = IntPtr.Zero;
            /// <summary>
            /// 控制台标题，对于不创建新控制台的进程必须为IntPtr.Zero
            /// </summary>
            public IntPtr Title = IntPtr.Zero;
            /// <summary>
            /// 相对于屏幕左上角横坐标像素，需要设置Flags.STARTF_USEPOSITION
            /// </summary>
            public int Left;
            /// <summary>
            /// 相对于屏幕左上角纵坐标像素，需要设置Flags.STARTF_USEPOSITION
            /// </summary>
            public int Top;
            /// <summary>
            /// 窗口像素宽度，需要设置Flags.STARTF_USESIZE
            /// </summary>
            public int Width;
            /// <summary>
            /// 窗口像素高度，需要设置Flags.STARTF_USESIZE
            /// </summary>
            public int Height;
            /// <summary>
            /// 屏幕缓存区字符宽度，需要设置Flags.STARTF_USECOUNTCHARS
            /// </summary>
            public int WidthCountChars;
            /// <summary>
            /// 屏幕缓存区字符高度，需要设置Flags.STARTF_USECOUNTCHARS
            /// </summary>
            public int HeightCountChars;
            /// <summary>
            /// 文本与背景颜色，FOREGROUND_BLUE/FOREGROUND_GREEN/FOREGROUND_RED/FOREGROUND_INTENSITY/BACKGROUND_BLUE/BACKGROUND_GREEN/BACKGROUND_RED/BACKGROUND_INTENSITY
            /// </summary>
            public int FillAttribute;
            /// <summary>
            /// 进程启动状态标识
            /// </summary>
            public startupInfoFlags Flags;
            /// <summary>
            /// ShowWindow的参数nCmdShow值，除了SW_SHOWDEFAULT
            /// </summary>
            public short ShowWindow;
            /// <summary>
            /// C运行时保留，必须是0
            /// </summary>
            public short CRunTimeReserved;
            /// <summary>
            /// C运行时保留，必须是IntPtr.Zero
            /// </summary>
            public IntPtr CRunTimeReservedIntPtr = IntPtr.Zero;
            /// <summary>
            /// 标准输入
            /// </summary>
            public SafeFileHandle StandardInput = new SafeFileHandle(IntPtr.Zero, false);
            /// <summary>
            /// 标准输出
            /// </summary>
            public SafeFileHandle StandardOutput = new SafeFileHandle(IntPtr.Zero, false);
            /// <summary>
            /// 标准错误
            /// </summary>
            public SafeFileHandle StandardError = new SafeFileHandle(IntPtr.Zero, false);
            /// <summary>
            /// 进程启动信息
            /// </summary>
            public startupInfo()
            {
                Size = Marshal.SizeOf(typeof(startupInfo));
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                if ((StandardInput != null) && !StandardInput.IsInvalid)
                {
                    StandardInput.Close();
                    StandardInput = null;
                }
                if ((StandardOutput != null) && !StandardOutput.IsInvalid)
                {
                    StandardOutput.Close();
                    StandardOutput = null;
                }
                if ((StandardError != null) && !StandardError.IsInvalid)
                {
                    StandardError.Close();
                    StandardError = null;
                }
            }
        }
        /// <summary>
        /// 进程信息PROCESS_INFORMATION
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class processInformation
        {
            /// <summary>
            /// 进程句柄
            /// </summary>
            public IntPtr Process = IntPtr.Zero;
            /// <summary>
            /// 主线程句柄
            /// </summary>
            public IntPtr Thread = IntPtr.Zero;
            /// <summary>
            /// 进程ID
            /// </summary>
            public int ProcessId;
            /// <summary>
            /// 主线程ID
            /// </summary>
            public int ThreadId;
        }
        /// <summary>
        /// 进程内存统计信息PROCESS_MEMORY_COUNTERS
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 2 * sizeof(uint) + 8 * sizeof(cpuUint))]
        public struct memoryCounters
        {
            /// <summary>
            /// 进程内存统计信息字节数
            /// </summary>
            public uint Size;
            public uint PageFaultCount;         // The number of page faults (DWORD).
            public cpuUint PeakWorkingSetSize;     // The peak working set size, in bytes (SIZE_T).
            public cpuUint WorkingSetSize;         // The current working set size, in bytes (SIZE_T).
            public cpuUint QuotaPeakPagedPoolUsage;    // The peak paged pool usage, in bytes (SIZE_T).
            public cpuUint QuotaPagedPoolUsage;    // The current paged pool usage, in bytes (SIZE_T).
            public cpuUint QuotaPeakNonPagedPoolUsage; // The peak nonpaged pool usage, in bytes (SIZE_T).
            public cpuUint QuotaNonPagedPoolUsage;     // The current nonpaged pool usage, in bytes (SIZE_T).
            public cpuUint PagefileUsage;          // The Commit Charge value in bytes for this process (SIZE_T). Commit Charge is the total amount of memory that the memory manager has committed for a running process.
            public cpuUint PeakPagefileUsage;      // The peak value in bytes of the Commit Charge during the lifetime of this process (SIZE_T).
        }

        /// <summary>
        /// 挂起的线程句柄
        /// </summary>
        private safeThreadHandle threadHandle;
        /// <summary>
        /// 命令行
        /// </summary>
        private string commandLine
        {
            get
            {
                charStream builder = new charStream();
                string str = StartInfo.FileName.Trim();
                bool flag = str.length() != 0 && str[0] == '\\' && str[str.Length - 1] == '\\';
                if (!flag)
                {
                    builder.Write('"');
                }
                builder.Write(str);
                if (!flag)
                {
                    builder.Write('"');
                }
                if (!string.IsNullOrEmpty(StartInfo.Arguments))
                {
                    builder.Write(' ');
                    builder.Write(StartInfo.Arguments);
                }
                return builder.ToString();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放资源</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed(this))
            {
                if (threadHandle != null)
                {
                    threadHandle.Dispose();
                    threadHandle = null;
                }
                base.Dispose(disposing);
            }
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="flags">创建状态标识</param>
        /// <returns>是否成功</returns>
        public bool Start(createFlags creationFlags)
        {
            Close();

            if ((StartInfo.StandardOutputEncoding != null) && !StartInfo.RedirectStandardOutput)
            {
                throw new InvalidOperationException("StandardOutputEncodingNotAllowed");
            }
            if ((StartInfo.StandardErrorEncoding != null) && !StartInfo.RedirectStandardError)
            {
                throw new InvalidOperationException("StandardErrorEncodingNotAllowed");
            }
            if (disposed(this))
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
            startupInfo lpStartupInfo = new startupInfo();
            processInformation lpProcessInformation = new processInformation();
            object processHandle = assembly.CreateInstance(safeProcessHandleFullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null);
            threadHandle = new safeThreadHandle();
            int error = 0;
            SafeFileHandle parentHandle = null;
            SafeFileHandle outputParentHandle = null;
            SafeFileHandle errorParentHandle = null;
            GCHandle gcHandle = new GCHandle();
            try
            {
                bool flag;
                if ((StartInfo.RedirectStandardInput || StartInfo.RedirectStandardOutput) || StartInfo.RedirectStandardError)
                {
                    if (StartInfo.RedirectStandardInput)
                    {
                        this.createPipe(out parentHandle, out lpStartupInfo.StandardInput, true);
                    }
                    else
                    {
                        lpStartupInfo.StandardInput = new SafeFileHandle(kernel32.GetStdHandle(kernel32.standardHandle.StandardInput), false);
                    }
                    if (StartInfo.RedirectStandardOutput)
                    {
                        this.createPipe(out outputParentHandle, out lpStartupInfo.StandardOutput, false);
                    }
                    else
                    {
                        lpStartupInfo.StandardOutput = new SafeFileHandle(kernel32.GetStdHandle(kernel32.standardHandle.StandardOutput), false);
                    }
                    if (StartInfo.RedirectStandardError)
                    {
                        this.createPipe(out errorParentHandle, out lpStartupInfo.StandardError, false);
                    }
                    else
                    {
                        lpStartupInfo.StandardError = new SafeFileHandle(kernel32.GetStdHandle(kernel32.standardHandle.StandardError), false);
                    }
                    lpStartupInfo.Flags = startupInfoFlags.STARTF_USESTDHANDLES;
                }
                if (StartInfo.CreateNoWindow)
                {
                    creationFlags |= createFlags.CREATE_NO_WINDOW;
                }
                IntPtr zero = IntPtr.Zero;
                StringDictionary environmentVariables = startInfoEnvironmentVariables(StartInfo);
                if (environmentVariables != null)
                {
                    bool unicode = false;
                    if (processManagerIsNt())
                    {
                        creationFlags |= createFlags.CREATE_UNICODE_ENVIRONMENT;
                        unicode = true;
                    }
                    gcHandle = GCHandle.Alloc(environmentBlockToByteArray(environmentVariables, unicode), GCHandleType.Pinned);
                    zero = gcHandle.AddrOfPinnedObject();
                }
                string workingDirectory = StartInfo.WorkingDirectory;
                if (workingDirectory == string.Empty)
                {
                    workingDirectory = Environment.CurrentDirectory;
                }
                RuntimeHelpers.PrepareConstrainedRegions();
                try
                {
                }
                finally
                {
                    flag = kernel32.CreateProcess(null, commandLine, null, null, true, creationFlags, zero, workingDirectory, lpStartupInfo, lpProcessInformation);
                    if (!flag)
                    {
                        error = Marshal.GetLastWin32Error();
                    }
                    if ((lpProcessInformation.Process != IntPtr.Zero) && (lpProcessInformation.Process != kernel32.INVALID_HANDLE_VALUE))
                    {
                        processInitialSetHandle(processHandle, lpProcessInformation.Process);
                    }
                    if ((lpProcessInformation.Thread != IntPtr.Zero) && (lpProcessInformation.Thread != kernel32.INVALID_HANDLE_VALUE))
                    {
                        threadHandle.InitialSetHandle(lpProcessInformation.Thread);
                    }
                }
                if (!flag)
                {
                    if (error == 0xc1)
                    {
                        throw new Win32Exception(error, "InvalidApplication");
                    }
                    throw new Win32Exception(error);
                }
            }
            finally
            {
                if (gcHandle.IsAllocated)
                {
                    gcHandle.Free();
                }
                lpStartupInfo.Dispose();
            }
            if (StartInfo.RedirectStandardInput)
            {
                StreamWriter input = new StreamWriter(new FileStream(parentHandle, FileAccess.Write, 0x1000, false), Encoding.GetEncoding(kernel32.GetConsoleCP()), 0x1000);
                input.AutoFlush = true;
                standardInput(this, input);
            }
            if (StartInfo.RedirectStandardOutput)
            {
                Encoding encoding1 = (StartInfo.StandardOutputEncoding != null) ? StartInfo.StandardOutputEncoding : Encoding.GetEncoding(kernel32.GetConsoleOutputCP());
                standardOutput(this, new StreamReader(new FileStream(outputParentHandle, FileAccess.Read, 0x1000, false), encoding1, true, 0x1000));
            }
            if (StartInfo.RedirectStandardError)
            {
                Encoding encoding2 = (StartInfo.StandardErrorEncoding != null) ? StartInfo.StandardErrorEncoding : Encoding.GetEncoding(kernel32.GetConsoleOutputCP());
                standardError(this, new StreamReader(new FileStream(errorParentHandle, FileAccess.Read, 0x1000, false), encoding2, true, 0x1000));
            }
            if (!handleIsInvalid(processHandle))
            {
                setProcessHandle(this, processHandle);
                setProcessId(this, lpProcessInformation.ProcessId);
                if ((creationFlags & createFlags.CREATE_SUSPENDED) == 0) threadHandle.Close();
                return true;
            }
            return false;

        }
        /// <summary>
        /// 创建输入输入重定向通道
        /// </summary>
        /// <param name="parentHandle">父进程句柄</param>
        /// <param name="childHandle">子进程句柄</param>
        /// <param name="parentInputs">是否输入</param>
        private void createPipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
        {
            kernel32.securityAttributes lpPipeAttributes = new kernel32.securityAttributes();
            lpPipeAttributes.IsInheritHandle = true;
            SafeFileHandle hWritePipe = null;
            try
            {
                if (parentInputs)
                {
                    createPipeWithSecurityAttributes(out childHandle, out hWritePipe, lpPipeAttributes, 0);
                }
                else
                {
                    createPipeWithSecurityAttributes(out hWritePipe, out childHandle, lpPipeAttributes, 0);
                }
                if (!kernel32.DuplicateHandle(new HandleRef(this, kernel32.GetCurrentProcess()), hWritePipe, new HandleRef(this, kernel32.GetCurrentProcess()), out parentHandle, 0, false, kernel32.duplicateHandleOptions.DUPLICATE_SAME_ACCESS))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                if ((hWritePipe != null) && !hWritePipe.IsInvalid)
                {
                    hWritePipe.Close();
                }
            }
        }
        /// <summary>
        /// 创建通道
        /// </summary>
        /// <param name="hReadPipe">读取通道</param>
        /// <param name="hWritePipe">写入通道</param>
        /// <param name="lpPipeAttributes">安全属性</param>
        /// <param name="nSize">缓存区字节数,0表示默认</param>
        private static void createPipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, kernel32.securityAttributes lpPipeAttributes, int nSize)
        {
            if ((!kernel32.CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize) || hReadPipe.IsInvalid) || hWritePipe.IsInvalid)
            {
                throw new Win32Exception();
            }
        }
        /// <summary>
        /// 唤醒线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ResumeThread()
        {
            int flag = kernel32.ResumeThread(threadHandle.DangerousGetHandle());
            if (flag != -1) return true;
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// 获取进程内存统计信息
        /// </summary>
        /// <param name="process">进程</param>
        /// <returns>取进程内存统计信息</returns>
        public static unsafe memoryCounters GetProcessMemoryInfo(Process process)
        {
            memoryCounters memoryCounters;
            memoryCounters.Size = (cpuUint)sizeof(memoryCounters);
            if (psapi.GetProcessMemoryInfo(process.Handle, out memoryCounters, memoryCounters.Size))
            {
                return memoryCounters;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        /// <summary>
        /// 根据进程名称获取进程数量
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns>进程数量</returns>
        public static int Count(string processName)
        {
            if (processName.length() != 0)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes) process.Close();
                return processes.Length;
            }
            return 0;
        }
        /// <summary>
        /// 根据文件名称获取进程名称
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns>进程名称</returns>
        public static string GetProcessNameByFullName(string filename)
        {
            return filename.length() != 0 ? GetProcessName(new FileInfo(filename)) : null;
        }
        /// <summary>
        /// 根据文件信息获取进程名称
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <returns>进程名称</returns>
        public static string GetProcessName(FileInfo file)
        {
            return file != null ? GetProcessName(file.Name) : null;
        }
        /// <summary>
        /// 获取进程名称
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns>进程名称</returns>
        public static string GetProcessName(string processName)
        {
            if (processName.length() != 0)
            {
                return processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? processName.Substring(0, processName.Length - 4) : processName;
            }
            return null;
        }
        /// <summary>
        /// 重启当前进程
        /// </summary>
        public static void ReStart()
        {
            using (Process process = Process.GetCurrentProcess()) StartDirectory(process.MainModule.FileName);
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static bool StartDirectory(string filename, string arguments = null)
        {
            return filename.length() != 0 && StartDirectory(new FileInfo(filename), arguments);
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static bool StartDirectory(FileInfo file, string arguments = null)
        {
            if (file != null && file.Exists)
            {
                ProcessStartInfo info = new ProcessStartInfo(file.FullName, arguments);
                info.UseShellExecute = true;
                info.WorkingDirectory = file.DirectoryName;
                using (Process process = Process.Start(info)) return true;
            }
            return false;
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="filename">文件名称</param>
        public static void StartNew(string filename)
        {
            using (Process process = Process.Start(filename)) process.Close();
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="arguments">参数</param>
        public static void StartNew(string filename, string arguments)
        {
            using (Process process = Process.Start(filename, arguments)) process.Close();
        }
        /// <summary>
        /// 杀死进程并释放进程句柄
        /// </summary>
        /// <param name="processes">进程集合</param>
        public static void Kill(Process[] processes)
        {
            if (processes != null)
            {
                foreach (Process process in processes)
                {
                    try
                    {
                        using (process) process.Kill();
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, null, false);
                    }
                    //using (process)
                    //{
                    //    process.CloseMainWindow();
                    //    //process.WaitForExit(1000);
                    //}
                }
            }
        }
    }
}
