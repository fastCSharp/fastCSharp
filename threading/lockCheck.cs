using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace fastCSharp.threading
{
    /// <summary>
    /// 死锁检测
    /// </summary>
    public static class lockCheck
    {
        /// <summary>
        /// 未释放锁信息
        /// </summary>
        private struct lockInfo
        {
            /// <summary>
            /// 申请次数
            /// </summary>
            public int Count;
            /// <summary>
            /// 第一次申请时间
            /// </summary>
            public DateTime LockTime;
            /// <summary>
            /// 最后一次申请堆栈
            /// </summary>
            public StackTrace StackTrace;
            /// <summary>
            /// 未释放锁信息
            /// </summary>
            /// <returns>未释放锁信息</returns>
            public override string ToString()
            {
                return LockTime.toString() + "[" + Count.toString() + @"]
" + StackTrace.ToString();
            }
        }
        /// <summary>
        /// 当前未释放的锁
        /// </summary>
        private static readonly Dictionary<object, lockInfo> lockInfos;
        /// <summary>
        /// 访问锁
        /// </summary>
        private static int interlock;
        /// <summary>
        /// 是否输出日志
        /// </summary>
        private static bool isOutput;
        /// <summary>
        /// 输出日志休眠时间
        /// </summary>
        private static int sleepTime;
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="value">锁对象, 必须是引用类型且不能为null</param>
        public static void Enter(object value)
        {
            Monitor.Enter(value);
            if (fastCSharp.config.pub.Default.LockCheckMinutes != 0)
            {
                while (Interlocked.CompareExchange(ref interlock, 1, 0) != 0) Thread.Sleep(1);
                lockInfo lockInfo;
                if (lockInfos.TryGetValue(value, out lockInfo))
                {
                    ++lockInfo.Count;
                    lockInfo.StackTrace = new StackTrace();
                    lockInfos[value] = lockInfo;
                }
                else lockInfos.Add(value, new lockInfo { Count = 1, LockTime = fastCSharp.date.NowTime, StackTrace = new StackTrace() });
                interlock = 0;
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="value">必须是当前线程已经申请的锁对象, 必须是引用类型且不能为null</param>
        public static void Exit(object value)
        {
            if (fastCSharp.config.pub.Default.LockCheckMinutes != 0)
            {
                while (Interlocked.CompareExchange(ref interlock, 1, 0) != 0) Thread.Sleep(1);
                lockInfo lockInfo;
                if (lockInfos.TryGetValue(value, out lockInfo))
                {
                    if (--lockInfo.Count == 0) lockInfos.Remove(value);
                    else
                    {
                        lockInfo.StackTrace = new StackTrace();
                        lockInfos[value] = lockInfo;
                    }
                }
                else log.Default.Add("没有找到需要释放的锁", true, true);
                interlock = 0;
            }
            Monitor.Exit(value);
        }
        /// <summary>
        /// 释放锁并等待唤醒
        /// </summary>
        /// <param name="value">必须是当前线程已经申请的锁对象, 是引用类型且不能为null</param>
        public static void Wait(object value)
        {
            object newValue = null;
            if (fastCSharp.config.pub.Default.LockCheckMinutes != 0)
            {
                while (Interlocked.CompareExchange(ref interlock, 1, 0) != 0) Thread.Sleep(1);
                lockInfo lockInfo;
                if (lockInfos.TryGetValue(value, out lockInfo))
                {
                    lockInfo.StackTrace = new StackTrace();
                    lockInfos.Add(newValue = new object(), lockInfo);
                    lockInfos.Remove(value);
                }
                else log.Default.Add("没有找到需要释放的锁", true, true);
                interlock = 0;
            }
            Monitor.Wait(value);
            if (newValue != null)
            {
                while (Interlocked.CompareExchange(ref interlock, 1, 0) != 0) Thread.Sleep(1);
                lockInfo lockInfo = lockInfos[newValue];
                lockInfos.Add(value, lockInfo);
                lockInfos.Remove(newValue);
                interlock = 0;
            }
        }
        /// <summary>
        /// 输出未释放的锁信息
        /// </summary>
        private static void output()
        {
            while (isOutput)
            {
                try
                {
                    DateTime time = fastCSharp.date.NowTime.AddMinutes(-fastCSharp.config.pub.Default.LockCheckMinutes);
                    lockInfo[] values;
                    while (Interlocked.CompareExchange(ref interlock, 1, 0) != 0) Thread.Sleep(1);
                    values = lockInfos.Values.getArray();
                    interlock = 0;
                    list<lockInfo> list = values.toList().remove(value => value.LockTime > time);
                    if (list.count() != 0)
                    {
                        log.Default.Add("未释放锁数量 " + list.Count.toString() + @"
" + list.JoinString(@"
", value => value.ToString()));
                    }
                }
                catch (Exception error)
                {
                    log.Default.Add(error);
                }
                Thread.Sleep(sleepTime);
            }
        }
        /// <summary>
        /// 停止输出日志
        /// </summary>
        private static void dispose()
        {
            isOutput = false;
        }
        static lockCheck()
        {
            if (fastCSharp.config.pub.Default.LockCheckMinutes != 0)
            {
                lockInfos = new Dictionary<object, lockInfo>();
                isOutput = true;
                sleepTime = fastCSharp.config.pub.Default.LockCheckMinutes * 60 * 1000;
                threading.threadPool.TinyPool.Start(output, dispose);
            }
        }
    }
}
