using System;
using System.Threading;

namespace fastCSharp.threading
{
    /// <summary>
    /// 线程池线程
    /// </summary>
    public class thread
    {
        /// <summary>
        /// 线程池
        /// </summary>
        private readonly threadPool threadPool;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly Thread threadHandle;
        /// <summary>
        /// 线程ID
        /// </summary>
        public int ManagedThreadId
        {
            get { return threadHandle.ManagedThreadId; }
        }
        /// <summary>
        /// 线程访问锁
        /// </summary>
        private readonly object threadLock = new object();
        /// <summary>
        /// 任务委托
        /// </summary>
        private action task;
        /// <summary>
        /// 应用程序退出处理
        /// </summary>
        private action domainUnload;
        /// <summary>
        /// 应用程序退出处理
        /// </summary>
        private action<Exception> onError;
        /// <summary>
        /// 是否结束线程
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        /// <param name="stackSize">堆栈大小</param>
        internal thread(threadPool threadPool, int stackSize)
        {
            this.threadPool = threadPool;
            threadHandle = new Thread(run, stackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 等待任务委托
        /// </summary>
        private void wait()
        {
            Monitor.Enter(threadLock);
            try
            {
                if (task == null) Monitor.Wait(threadLock);
            }
            finally { Monitor.Exit(threadLock); }
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void run()
        {
            action task, domainUnload;
            action<Exception> onError;
            Monitor.Enter(threadLock);
            try
            {
                if (this.task == null) Monitor.Wait(threadLock);
                task = this.task;
                domainUnload = this.domainUnload;
                onError = this.onError;
                this.task = null;
                this.domainUnload = null;
                this.onError = null;
            }
            finally { Monitor.Exit(threadLock); }
            while (!isStop)
            {
                if (domainUnload != null) fastCSharp.domainUnload.Add(domainUnload);
                try
                {
                    task();
                }
                catch (Exception error)
                {
                    if (onError != null)
                    {
                        try
                        {
                            onError(error);
                        }
                        catch (Exception error1)
                        {
                            log.Default.Add(error1, null, false);
                        }
                    }
                    else log.Default.Add(error, null, false);
                }
                finally
                {
                    if (domainUnload != null) fastCSharp.domainUnload.Remove(domainUnload, false);
                }
                threadPool.Push(this);
                Monitor.Enter(threadLock);
                try
                {
                    if (this.task == null) Monitor.Wait(threadLock);
                    task = this.task;
                    domainUnload = this.domainUnload;
                    onError = this.onError;
                    this.task = null;
                    this.domainUnload = null;
                    this.onError = null;
                }
                finally { Monitor.Exit(threadLock); }
            }
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        internal void Stop()
        {
            isStop = true;
            RunTask(null, null, null);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="handle">任务委托</param>
        /// <param name="domainUnload">应用程序退出处理</param>
        /// <param name="onError">应用程序退出处理</param>
        internal void RunTask(action task, action domainUnload, action<Exception> onError)
        {
            Monitor.Enter(threadLock);
            try
            {
                this.domainUnload = domainUnload;
                this.onError = onError;
                this.task = task;
                Monitor.Pulse(threadLock);
            }
            finally { Monitor.Exit(threadLock); }
        }
    }
}
