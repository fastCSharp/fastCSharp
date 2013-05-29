using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 程序集扩展操作
    /// </summary>
    public static class assembly
    {
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="fullName">程序集名称</param>
        /// <returns>程序集,失败返回null</returns>
        public static Assembly Get(string fullName)
        {
            Assembly value;
            return nameCache.TryGetValue(fullName, out value) ? value : null;
        }
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="assembly">程序集信息</param>
        /// <param name="fullName">类型全名</param>
        /// <returns>类型信息</returns>
        public static Type getType(this Assembly assembly, string fullName)
        {
            return assembly != null ? assembly.GetType(fullName) : null;
        }
        /// <summary>
        /// 程序集名称缓存
        /// </summary>
        private static readonly Dictionary<string, Assembly> nameCache = new Dictionary<string, Assembly>();
        /// <summary>
        /// 程序集名称缓存访问锁
        /// </summary>
        private static int nameCacheLock;
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        private static void loadAssembly(Assembly assembly)
        {
            while (Interlocked.CompareExchange(ref nameCacheLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                nameCache[assembly.FullName] = assembly;
            }
            finally { nameCacheLock = 0; }
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void loadAssembly(object sender, AssemblyLoadEventArgs args)
        {
            loadAssembly(args.LoadedAssembly);
        }
        static assembly()
        {
            AppDomain.CurrentDomain.AssemblyLoad += loadAssembly;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) loadAssembly(assembly);
        }
    }
}
