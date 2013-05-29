#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE

#if DOTNET35
#else
using System;
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// 编译器扩展方法支持
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute { }
}
#endif