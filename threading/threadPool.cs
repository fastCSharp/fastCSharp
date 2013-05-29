using System;
using System.Threading;

namespace fastCSharp.threading
{
    /// <summary>
    /// 线程池
    /// </summary>
    public class threadPool
    {
        /// <summary>
        /// 最低线程堆栈大小
        /// </summary>
        private const int minStackSize = 128 << 10;
        /// <summary>
        /// 线程堆栈大小
        /// </summary>
        private int stackSize;
        /// <summary>
        /// 线程集合
        /// </summary>
        private readonly list<thread> threads = new list<thread>();
        /// <summary>
        /// 线程获取访问锁
        /// </summary>
        private int threadLock;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 线程池
        /// </summary>
        /// <param name="stackSize">线程堆栈大小</param>
        private threadPool(int stackSize = 1 << 20)
        {
            this.stackSize = stackSize < minStackSize ? minStackSize : stackSize;
            fastCSharp.domainUnload.Add(dispose);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void dispose()
        {
            while (Interlocked.CompareExchange(ref threadLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    foreach (thread thread in threads) thread.Stop();
                    threads.Clear();
                }
            }
            finally { threadLock = 0; }
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="domainUnload">应用程序退出处理</param>
        /// <param name="onError">应用程序退出处理</param>
        /// <returns>线程池线程</returns>
        private thread start(action task, action domainUnload, action<Exception> onError)
        {
            if (task == null) log.Default.Throw(null, "缺少 线程委托", false);
            bool isDisposed;
            thread thread = null;
            while (Interlocked.CompareExchange(ref threadLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (!(isDisposed = this.isDisposed) && threads.Count != 0) thread = threads.Pop();
            }
            finally { threadLock = 0; }
            if (isDisposed) log.Default.Add("线程池已经被释放", true, false);
            else
            {
                if (thread == null) thread = new thread(this, stackSize);
                thread.RunTask(task, domainUnload, onError);
            }
            return thread;
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="domainUnload">应用程序退出处理</param>
        /// <param name="onError">应用程序退出处理</param>
        /// <returns>线程池线程</returns>
        public thread Start(action task, action domainUnload = null, action<Exception> onError = null)
        {
            if (task == null) log.Default.Throw(null, "缺少 线程委托", false);
            return start(task, domainUnload, onError);
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <typeparam name="parameterType">参数类型</typeparam>
        /// <param name="task">任务委托</param>
        /// <param name="parameter">线程参数</param>
        /// <param name="domainUnload">应用程序退出处理</param>
        /// <param name="onError">应用程序退出处理</param>
        /// <returns>线程池线程</returns>
        public thread Start<parameterType>
            (action<parameterType> task, parameterType parameter, action domainUnload = null, action<Exception> onError = null)
        {
            if (task == null) log.Default.Throw(null, "缺少 线程委托", false);
            return start(run<parameterType>.Create(task, parameter), domainUnload, onError);
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <typeparam name="parameterType">参数类型</typeparam>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="task">任务委托</param>
        /// <param name="parameter">线程参数</param>
        /// <param name="onReturn">返回值执行委托</param>
        /// <param name="domainUnload">应用程序退出处理</param>
        /// <param name="onError">应用程序退出处理</param>
        /// <returns>线程池线程</returns>
        public thread Start<parameterType, returnType>(func<parameterType, returnType> task, parameterType parameter,
            action<returnType> onReturn, action domainUnload = null, action<Exception> onError = null)
        {
            if (task == null) log.Default.Throw(null, "缺少 线程委托", false);
            return start(run<parameterType, returnType>.Create(task, parameter,onReturn), domainUnload, onError);
        }
        /// <summary>
        /// 线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        internal void Push(thread thread)
        {
            while (Interlocked.CompareExchange(ref threadLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (isDisposed) thread.Stop();
                else threads.Add(thread);
            }
            finally { threadLock = 0; }
        }
        /// <summary>
        /// 默认线程池
        /// </summary>
        public static readonly threadPool Default = new threadPool(fastCSharp.config.pub.Default.DefaultThreadPoolStackSize);
        /// <summary>
        /// 微型线程池,堆栈大小可能只有128K
        /// </summary>
        public static readonly threadPool TinyPool = fastCSharp.config.pub.Default.DefaultThreadPoolStackSize != fastCSharp.config.pub.Default.TinyThreadPoolStackSize ? new threadPool(fastCSharp.config.pub.Default.TinyThreadPoolStackSize) : Default;
    }
}
