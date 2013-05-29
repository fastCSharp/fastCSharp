using System;
using System.Collections.Generic;
using System.IO;
using fastCSharp.diagnostics;

namespace fastCSharp.setup
{
    /// <summary>
    /// 安装错误处理
    /// </summary>
    internal static class error
    {
        /// <summary>
        /// 提示信息集合
        /// </summary>
        private static readonly HashSet<string> messages = new HashSet<string>();
        /// <summary>
        /// 错误信息集合
        /// </summary>
        private static readonly HashSet<string> errors = new HashSet<string>();
        /// <summary>
        /// 异常集合
        /// </summary>
        private static readonly list<Exception> exceptions = new list<Exception>();
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        private static bool isMessage;
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        public static bool IsMessage
        {
            get { return isMessage || messages.Count != 0; }
        }
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        private static bool isError;
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        public static bool IsError
        {
            get { return isError || errors.Count != 0 && exceptions.Count != 0; }
        }
        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="value">提示信息</param>
        public static void Message(string value)
        {
            messages.Add(value);
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public static void Add(string error)
        {
            errors.Add(error);
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="error">异常</param>
        public static void Add(Exception error)
        {
            exceptions.Add(error);
        }
        /// <summary>
        /// 清除所有信息
        /// </summary>
        public static void Clear()
        {
            errors.Clear();
            exceptions.Empty();
            messages.Clear();
            isError = isMessage = false;
        }
        /// <summary>
        /// 信息输出到日志
        /// </summary>
        /// <returns>是否存在错误信息</returns>
        public static bool Output()
        {
            if (messages.Count != 0)
            {
                log.Default.Real(null, messages.joinString(@"
- - - - - - - -
"), false);
                messages.Clear();
                isMessage = true;
            }
            if (errors.Count != 0)
            {
                log.Default.Real(null, errors.joinString(@"
- - - - - - - -
"), false);
                errors.Clear();
                isError = true;
            }
            if (exceptions.Count != 0)
            {
                foreach (Exception error in exceptions) fastCSharp.log.Default.Real(error, null, false);
                exceptions.Empty();
                isError = true;
            }
            return isError;
        }
        /// <summary>
        /// 如果安装出错，抛出异常
        /// </summary>
        public static void ThrowError()
        {
            if (Output()) throw new Exception("安装失败");
        }
        /// <summary>
        /// 输出错误信息到日志并打开日志文件
        /// </summary>
        /// <param name="isClear">是否清除错误信息</param>
        /// <returns>安装是否顺利,没有错误或者提示信息</returns>
        public static bool Open(bool isClear)
        {
            if (Output() || IsMessage)
            {
                FileInfo file = new FileInfo(log.Default.FileName);
                string fileName = file.Directory.fullName() + "@" + DateTime.Now.Ticks.ToString() + "_" + file.Name;
                if (log.Default.MoveFile(fileName)) process.StartNew(fileName);
                if (isClear) Clear();
                return false;
            }
            return true;
        }
    }
}
