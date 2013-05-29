using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace fastCSharp
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public class log : IDisposable
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public enum exceptionType
        {
            None,
            /// <summary>
            /// 关键值为空
            /// </summary>
            Null,
            /// <summary>
            /// 索引超出范围
            /// </summary>
            IndexOutOfRange,
            /// <summary>
            /// 操作不可用
            /// </summary>
            ErrorOperation,
        }
        /// <summary>
        /// 日志信息
        /// </summary>
        private class debug
        {
            /// <summary>
            /// 调用堆栈
            /// </summary>
            public StackTrace StackTrace;
            /// <summary>
            /// 调用堆栈帧
            /// </summary>
            public StackFrame StackFrame;
            /// <summary>
            /// 提示信息
            /// </summary>
            public string Message;
            /// <summary>
            /// 错误异常
            /// </summary>
            public Exception Exception;
            /// <summary>
            /// 异常类型
            /// </summary>
            public exceptionType Type;
            /// <summary>
            /// 字符串
            /// </summary>
            public string toString;
            /// <summary>
            /// 字符串
            /// </summary>
            /// <returns>字符串</returns>
            public override string ToString()
            {
                if (toString == null)
                {
                    list<string>.unsafer errorString = new list<string>(2).Unsafer;
                    if (Message != null) errorString.Add("附加信息 : " + Message);
                    if (StackFrame != null) errorString.Add("堆栈帧信息 : " + StackFrame.toString());
                    if (StackTrace != null) errorString.Add("堆栈信息 : " + StackTrace.ToString());
                    if (Exception != null) errorString.Add("异常信息 : " + Exception.ToString());
                    if (Type != exceptionType.None) errorString.Add("异常类型 : " + Type.ToString());
                    toString = errorString[0] + @"
" + errorString[1];
                }
                return toString;
            }
        }
        /// <summary>
        /// 异常错误信息前缀
        /// </summary>
        public const string ExceptionPrefix = pub.fastCSharp + " Exception : ";
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public const string DefaultFilePrefix = "log_";

        /// <summary>
        /// 日志文件
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 日志缓存
        /// </summary>
        private readonly Dictionary<string, debug> cache = new Dictionary<string, debug>();
        /// <summary>
        /// 日志缓存访问锁
        /// </summary>
        private int cacheLock;
        /// <summary>
        /// 日志缓存访问锁
        /// </summary>
        private readonly object logLock = new object();
        /// <summary>
        /// 文件输出访问锁
        /// </summary>
        private int outputLock;
        /// <summary>
        /// 等待处理的日志队列
        /// </summary>
        private list<debug> newDebugs = new list<debug>();
        /// <summary>
        /// 当前输出的日志队列
        /// </summary>
        private list<debug> currentDebugs = new list<debug>();
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="fileName">日志文件</param>
        public log(string fileName)
        {
            FileName = fileName;
            threading.threadPool.TinyPool.Start(output, Dispose);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(logLock);
            try
            {
                isStop = true;
                Monitor.Pulse(logLock);
            }
            finally { Monitor.Exit(logLock); }
        }
        /// <summary>
        /// 日志信息写文件
        /// </summary>
        private void output()
        {
            while (!isStop)
            {
                Monitor.Enter(logLock);
                try
                {
                    if (newDebugs.Count == 0) Monitor.Wait(logLock);
                    list<debug> oldDebugs = currentDebugs;
                    currentDebugs = newDebugs;
                    newDebugs = oldDebugs;
                }
                finally { Monitor.Exit(logLock); }
                output(string.Join(@"
- - - - - - - - - -
", currentDebugs.GetArray(error => error.ToString())));
                currentDebugs.Empty();
            }
            newDebugs = null;
        }
        /// <summary>
        /// 日志信息写文件
        /// </summary>
        /// <param name="value">日志信息</param>
        private void output(string value)
        {
            string outString = @"
" + date.NowTime.toString() + " : " + value + @"
";
            while (Interlocked.CompareExchange(ref outputLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                File.AppendAllText(FileName, outString, config.pub.Default.Encoding);
            }
            catch { }
            finally { outputLock = 0; }
        }
        /// <summary>
        /// 添加新的日志
        /// </summary>
        /// <param name="value">日志信息</param>
        private void add(debug value)
        {
            if (!isStop)
            {
                Monitor.Enter(logLock);
                try
                {
                    newDebugs.Add(value);
                    Monitor.Pulse(logLock);
                }
                finally { Monitor.Exit(logLock); }
            }
        }
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="value">日志信息</param>
        /// <returns>缓存是否存在</returns>
        private bool checkCache(debug value)
        {
            string key = value.ToString();
            while (Interlocked.CompareExchange(ref cacheLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (cache.ContainsKey(key)) return true;
                cache.Add(key, value);
            }
            finally { cacheLock = 0; }
            return false;
        }
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <param name="fileName">新文件名</param>
        /// <returns>是否成功</returns>
        public bool MoveFile(string fileName)
        {
            while (Interlocked.CompareExchange(ref outputLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (File.Exists(FileName))
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                    File.Move(FileName, fileName);
                    return true;
                }
            }
            catch (Exception error)
            {
                if (Default != this) Default.Add(error, null, true);
            }
            finally { outputLock = 0; }
            return false;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        public void Add(Exception error, string message = null, bool isCache = true)
        {
            if (error != null && error.Message.StartsWith(ExceptionPrefix, StringComparison.Ordinal)) error = null;
            if (error == null)
            {
                if (message != null) Add(message, true, isCache);
            }
            else
            {
                debug value = new debug
                {
                    Exception = error,
                    Message = message
                };
                if (!isCache || !checkCache(value)) add(value);
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="isStackTrace">是否包含调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        public void Add(string message, bool isStackTrace = false, bool isCache = false)
        {
            debug value = new debug
            {
                StackTrace = isStackTrace ? new StackTrace() : null,
                StackFrame = isStackTrace ? null : new StackFrame(1),
                Message = message
            };
            if (!isCache || !checkCache(value)) add(value);
        }
        /// <summary>
        /// 实时添加日志
        /// </summary>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        public void Real(Exception error, string message = null, bool isCache = true)
        {
            if (error != null && error.Message.StartsWith(ExceptionPrefix, StringComparison.Ordinal)) error = null;
            if (error == null)
            {
                if (message != null) Real(message, true, isCache);
            }
            else
            {
                debug value = new debug
                {
                    Exception = error,
                    Message = message
                };
                if (!isCache || !checkCache(value)) output(value.ToString());
            }
        }
        /// <summary>
        /// 实时添加日志
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="isStackTrace">是否包含调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        public void Real(string message, bool isStackTrace = false, bool isCache = false)
        {
            debug value = new debug
            {
                StackTrace = isStackTrace ? new StackTrace() : null,
                StackFrame = isStackTrace ? null : new StackFrame(1),
                Message = message
            };
            if (!isCache || !checkCache(value)) output(value.ToString());
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="error">异常类型</param>
        public void Throw(exceptionType error)
        {
            debug value = new debug
            {
                StackTrace = new StackTrace(),
                Type = error
            };
            if (!checkCache(value)) add(value);
            throw new Exception(ExceptionPrefix + value.ToString());
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        public void Throw(Exception error, string message = null, bool isCache = true)
        {
            if (error != null && error.Message.StartsWith(ExceptionPrefix, StringComparison.Ordinal)) error = null;
            if (error == null)
            {
                if (message != null) Throw(message, true, isCache);
            }
            else
            {
                debug value = new debug
                {
                    Exception = error,
                    Message = message
                };
                if (!isCache || !checkCache(value)) add(value);
                throw error != null ? new Exception(ExceptionPrefix + message, error) : new Exception(ExceptionPrefix + message);
            }
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="isStackTrace">是否包含调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        public void Throw(string message, bool isStackTrace = false, bool isCache = false)
        {
            debug value = new debug
            {
                StackTrace = isStackTrace ? new StackTrace() : null,
                StackFrame = isStackTrace ? null : new StackFrame(1),
                Message = message
            };
            if (!isCache || !checkCache(value)) add(value);
            throw new Exception(ExceptionPrefix + message);
        }
        /// <summary>
        /// 实时添加日志并抛出异常
        /// </summary>
        /// <param name="error">异常类型</param>
        public void ThrowReal(exceptionType error)
        {
            debug value = new debug
            {
                StackTrace = new StackTrace(),
                Type = error
            };
            if (!checkCache(value)) output(value.ToString());
            throw new Exception(ExceptionPrefix + value.ToString());
        }
        /// <summary>
        /// 实时添加日志并抛出异常
        /// </summary>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        public void ThrowReal(Exception error, string message = null, bool isCache = true)
        {
            if (error != null && error.Message.StartsWith(ExceptionPrefix, StringComparison.Ordinal)) error = null;
            if (error == null)
            {
                if (message != null) ThrowReal(message, true, isCache);
            }
            else
            {
                debug value = new debug
                {
                    Exception = error,
                    Message = message
                };
                if (!isCache || !checkCache(value)) output(value.ToString());
                throw error != null ? new Exception(ExceptionPrefix + message, error) : new Exception(ExceptionPrefix + message);
            }
        }
        /// <summary>
        /// 实时添加日志并抛出异常
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="isStackTrace">是否包含调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        public void ThrowReal(string message, bool isStackTrace = false, bool isCache = false)
        {
            debug value = new debug
            {
                StackTrace = isStackTrace ? new StackTrace() : null,
                StackFrame = isStackTrace ? null : new StackFrame(1),
                Message = message
            };
            if (!isCache || !checkCache(value)) output(value.ToString());
            throw new Exception(ExceptionPrefix + message);
        }
        /// <summary>
        /// 过滤Socket不可用异常
        /// </summary>
        /// <param name="error">异常</param>
        /// <returns>过滤后的异常,匹配返回null</returns>
        public static Exception FilterSocketDisposed(Exception error)
        {
            if (error != null)
            {
                IOException ioError = error as IOException;
                return filterSocketDisposed(ioError != null ? ioError.InnerException : error);
            }
            return null;
        }
        /// <summary>
        /// 过滤Socket不可用异常
        /// </summary>
        /// <param name="error">异常</param>
        /// <returns>过滤后的异常,匹配返回null</returns>
        private static Exception filterSocketDisposed(Exception error)
        {
            if (error != null)
            {
                ObjectDisposedException disposedError = error.InnerException as ObjectDisposedException;
                if (disposedError == null)
                {
                    System.Net.Sockets.SocketException socketException = error as System.Net.Sockets.SocketException;
                    if (socketException != null && socketException.ErrorCode == 10053 && socketException.ErrorCode == 10054) return null;
                }
            }
            return error;
        }
        /// <summary>
        /// 默认日志
        /// </summary>
        public static readonly log Default;
        static log()
        {
            Default = new log(config.pub.Default.LogPath + DefaultFilePrefix + "default.txt");
        }
    }
}
