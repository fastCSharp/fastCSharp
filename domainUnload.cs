using System;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp
{
    /// <summary>
    /// 应用程序卸载处理
    /// </summary>
    public static class domainUnload
    {
        /// <summary>
        /// 卸载状态
        /// </summary>
        private enum unloadState
        {
            /// <summary>
            /// 正常运行状态
            /// </summary>
            Run,
            /// <summary>
            /// 卸载中，等待事务结束
            /// </summary>
            WaitTransaction,
            /// <summary>
            /// 卸载事件处理
            /// </summary>
            Event,
            /// <summary>
            /// 已经卸载
            /// </summary>
            Unloaded
        }
        /// <summary>
        /// 是否已关闭
        /// </summary>
        private static unloadState state = unloadState.Run;
        /// <summary>
        /// 卸载处理函数集合
        /// </summary>
        private static readonly HashSet<action> unloaders = new HashSet<action>();
        /// <summary>
        /// 卸载处理函数访问锁
        /// </summary>
        private static readonly object unloaderLock = new object();
        /// <summary>
        /// 事务数量
        /// </summary>
        private static int transactionCount;
        ///// <summary>
        ///// 事务锁
        ///// </summary>
        //private static readonly object transactionLock = new object();
        /// <summary>
        /// 添加应用程序卸载处理
        /// </summary>
        /// <param name="onUnload">卸载处理函数</param>
        /// <returns>是否添加成功</returns>
        public static bool Add(action onUnload)
        {
            bool isAdd = false;
            Monitor.Enter(unloaderLock);
            try
            {
                if (state == unloadState.Run || state == unloadState.WaitTransaction)
                {
                    unloaders.Add(onUnload);
                    isAdd = true;
                }
            }
            finally { Monitor.Exit(unloaderLock); }
            if (!isAdd) log.Default.Real("应用程序正在退出", true, false);
            return isAdd;
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="onUnload">卸载处理函数</param>
        /// <param name="isRun">是否执行删除的函数</param>
        /// <returns>是否删除成功</returns>
        public static bool Remove(action onUnload, bool isRun = true)
        {
            bool isRemove;
            Monitor.Enter(unloaderLock);
            try
            {
                isRemove = (state == unloadState.Run || state == unloadState.WaitTransaction)
                    && unloaders.Count != 0 && unloaders.Remove(onUnload);
            }
            finally { Monitor.Exit(unloaderLock); }
            if (isRemove && isRun) onUnload();
            return isRemove;
        }
        /// <summary>
        /// 新事务开始,请保证唯一调用TransactionEnd,否则将导致卸载事件不被执行
        /// </summary>
        /// <param name="ignoreWait">忽略卸载中的等待事务，用于事务派生的事务</param>
        /// <returns>是否成功</returns>
        public static bool TransactionStart(bool ignoreWait = false)
        {
            bool isTransaction = false;
            Monitor.Enter(unloaderLock);
            if (state == unloadState.Run || (ignoreWait && state == unloadState.WaitTransaction))
            {
                ++transactionCount;
                isTransaction = true;
            }
            Monitor.Exit(unloaderLock);
            if (!isTransaction) log.Default.Real("应用程序正在退出", true, false);
            return isTransaction;
        }
        /// <summary>
        /// 请保证TransactionStart与TransactionEnd一一对应，否则将导致卸载事件不被执行
        /// </summary>
        public static void TransactionEnd()
        {
            Monitor.Enter(unloaderLock);
            try
            {
                if ((state == unloadState.Run || state == unloadState.WaitTransaction) && --transactionCount == 0) Monitor.Pulse(unloaderLock);
            }
            finally { Monitor.Exit(unloaderLock); }
        }
        /// <summary>
        /// 事务结束
        /// </summary>
        private struct transactionEnd
        {
            /// <summary>
            /// 任务执行委托
            /// </summary>
            public action Action;
            /// <summary>
            /// 任务执行
            /// </summary>
            public void Run()
            {
                try
                {
                    Action();
                }
                finally { TransactionEnd(); }
            }
        }
        /// <summary>
        /// 获取事务结束委托
        /// </summary>
        /// <param name="run">任务执行委托</param>
        /// <returns>事务结束委托</returns>
        public static action Transaction(action run)
        {
            if (TransactionStart(true))
            {
                return new transactionEnd { Action = run }.Run;
            }
            log.Default.Throw(log.exceptionType.ErrorOperation);
            return null;
        }
        /// <summary>
        /// 获取事务结束委托
        /// </summary>
        /// <typeparam name="parameterType">参数类型</typeparam>
        /// <param name="run">任务执行委托</param>
        /// <param name="parameter">参数</param>
        /// <returns>事务结束委托</returns>
        public static action Transaction<parameterType>(action<parameterType> run, parameterType parameter)
        {
            if (TransactionStart(true))
            {
                return new transactionEnd { Action = run<parameterType>.Create(run, parameter) }.Run;
            }
            log.Default.Throw(log.exceptionType.ErrorOperation);
            return null;
        }
        /// <summary>
        /// 事务结束检测
        /// </summary>
        private static void transactionCheck()
        {
            if (transactionCount != 0) log.Default.Real("事务未结束 " + transactionCount.toString(), false, false);
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        public static void Exit()
        {
            unload(null, null);
            Environment.Exit(-1);
        }
        /// <summary>
        /// 应用程序卸载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void unload(object sender, EventArgs e)
        {
            unload();
        }
        /// <summary>
        /// 应用程序卸载事件
        /// </summary>
        private static void unload()
        {
            Monitor.Enter(unloaderLock);
            try
            {
                if (state == unloadState.Run)
                {
                    state = unloadState.WaitTransaction;
                    threading.timerTask.Default.Add(transactionCheck, date.NowTime.AddMinutes(1));
                    if (transactionCount != 0) Monitor.Wait(unloaderLock);
                    state = unloadState.Event;
                    foreach (action value in unloaders)
                    {
                        try { value(); }
                        catch (Exception error)
                        {
                            fastCSharp.log.Default.Real(error, null, false);
                        }
                    }
                    state = unloadState.Unloaded;
                }
            }
            finally { Monitor.Exit(unloaderLock); }
        }
        /// <summary>
        /// 线程错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="error"></param>
        private static void onError(object sender, UnhandledExceptionEventArgs error)
        {
            Exception exception = error.ExceptionObject as Exception;
            if (exception != null) log.Default.Real(exception, null, false);
            else log.Default.Real(null, error.ExceptionObject.ToString(), false);
            unload(null, null);
        }
        static domainUnload()
        {
            AppDomain.CurrentDomain.DomainUnload += unload;
            AppDomain.CurrentDomain.ProcessExit += unload;
            AppDomain.CurrentDomain.UnhandledException += onError;
        }
    }

}
