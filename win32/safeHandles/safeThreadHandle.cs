using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace fastCSharp.win32.safeHandles
{
    /// <summary>
    /// 线程句柄
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal sealed class safeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// 线程句柄
        /// </summary>
        internal safeThreadHandle() : base(true) { }
        /// <summary>
        /// 设置句柄
        /// </summary>
        /// <param name="handle">线程句柄</param>
        internal void InitialSetHandle(IntPtr handle)
        {
            base.SetHandle(handle);
        }
        /// <summary>
        /// 释放句柄
        /// </summary>
        /// <returns>是否成功</returns>
        protected override bool ReleaseHandle()
        {
            return kernel32.CloseHandle(base.handle);
        }
    }
}
