using System;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace fastCSharp.win32.safeHandles
{
    /// <summary>
    /// 内存句柄
    /// </summary>
    [SuppressUnmanagedCodeSecurity, HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    internal sealed class safeLocalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// 内存句柄
        /// </summary>
        internal safeLocalMemHandle() : base(true) { }
        /// <summary>
        /// 内存句柄
        /// </summary>
        /// <param name="existingHandle">内存句柄</param>
        /// <param name="ownsHandle">是否拥有句柄</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal safeLocalMemHandle(IntPtr existingHandle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(existingHandle);
        }
        /// <summary>
        /// 释放句柄
        /// </summary>
        /// <returns>是否成功</returns>
        protected override bool ReleaseHandle()
        {
            return kernel32.LocalFree(base.handle) == IntPtr.Zero;
        }
    }
}
