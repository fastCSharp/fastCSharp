using System;
using System.Text;
using System.IO;
using System.Management;

namespace fastCSharp
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static class pub
    {
#if CPU64
        /// <summary>
        /// 字节数转CPU字长度的移位
        /// </summary>
        public const int Byte2CpuShift = 3;
#else
        /// <summary>
        /// 字节数转CPU字长度的移位
        /// </summary>
        public const int Byte2CpuShift = 2;
#endif
        /// <summary>
        /// CPU位长
        /// </summary>
        public const int CpuBits = 1 << (Byte2CpuShift + 3);
        /// <summary>
        /// 项目常量，不可修改
        /// </summary>
        public const string fastCSharp = "fastCSharp";
        /// <summary>
        /// 程序执行主目录(小写字母)
        /// </summary>
        public static readonly string ApplicationPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).fullName().toLower();
        /// <summary>
        /// CPU核心数量
        /// </summary>
        public static readonly int CpuCount = 1;
        /// <summary>
        /// 默认空字符
        /// </summary>
        public const char NullChar = (char)0;
        /// <summary>
        /// 默认随机生成器
        /// </summary>
        public static readonly Random Random = new Random();
        /// <summary>
        /// 程序启用时间
        /// </summary>
        public static readonly DateTime StartTime = date.NowTime;
        /// <summary>
        /// 最小时间值
        /// </summary>
        public static readonly DateTime MinTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// gb2312编码
        /// </summary>
        public static readonly Encoding Gb2312 = Encoding.GetEncoding("gb2312");
        /// <summary>
        /// gb18030编码
        /// </summary>
        public static readonly Encoding Gb18030 = Encoding.GetEncoding("gb18030");
        /// <summary>
        /// gbk编码
        /// </summary>
        public static readonly Encoding Gbk = Encoding.GetEncoding("gbk");
        /// <summary>
        /// big5编码
        /// </summary>
        public static readonly Encoding Big5 = Encoding.GetEncoding("big5");
        /// <summary>
        /// 函数调用,用于链式编程
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">数据</param>
        /// <param name="method">函数调用</param>
        /// <returns>数据</returns>
        public static valueType action<valueType>(this valueType value, action<valueType> method) where valueType : class
        {
            if (method == null) log.Default.Throw(log.exceptionType.Null);
            if (value != null) method(value);
            return value;
        }

        static pub()
        {
            try
            {
                int count = 0, coreCount;
                foreach (ManagementObject managementObject in (new ManagementClass("Win32_Processor")).GetInstances())
                {
                    object cores = managementObject["NumberOfCores"];
                    if (cores == null) break;
                    if (int.TryParse(cores.ToString(), out coreCount)) count += coreCount;
                }
                CpuCount = count;
            }
            catch { }
        }
    }
}
