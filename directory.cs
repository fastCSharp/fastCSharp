using System;
using System.IO;

namespace fastCSharp
{
    /// <summary>
    /// 目录相关操作
    /// </summary>
    public static class directory
    {
        /// <summary>
        /// 取以\结尾的路径全名
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>\结尾的路径全名</returns>
        public static string fullName(this DirectoryInfo path)
        {
            return path != null ? path.FullName.pathSuffix() : null;
        }
        /// <summary>
        /// 路径补全结尾的\
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>路径</returns>
        public static string pathSuffix(this string path)
        {
            if (path.length() != 0)
            {
                return unsafer.String.Last(path) == '\\' ? path : (path + "\\");
            }
            return "\\";
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>是否创建成功</returns>
        public static bool Create(string path)
        {
            if (path != null)
            {
                if (Directory.Exists(path)) return true;
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, "目录创建失败 : " + path, false);
                }
            }
            return false;
        }
    }
}
