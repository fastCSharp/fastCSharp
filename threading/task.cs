using System;
using System.Threading;

namespace fastCSharp.threading
{
    /// <summary>
    /// 任务处理类(适用于短小任务，因为处理阻塞)
    /// </summary>
    public class task : IDisposable
    {
        /// <summary>
        /// 带返回值的泛型任务信息
        /// </summary>
        public abstract class runReturn
        {
            /// <summary>
            /// 异常信息
            /// </summary>
            protected Exception error;
            /// <summary>
            /// 任务委托是否执行过
            /// </summary>
            protected int isRun;
            /// <summary>
            /// 执行任务
            /// </summary>
            protected void tryRun()
            {
                try
                {
                    interlocked.WaitRunOnce(ref isRun, run);
                }
                catch (Exception error)
                {
                    this.error = error;
                }
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            protected abstract void run();
            /// <summary>
            /// 任务信息
            /// </summary>
            internal threading.taskInfo TaskInfo
            {
                get
                {
                    return new threading.taskInfo { Action = tryRun };
                }
            }
        }
        /// <summary>
        /// 带返回值的泛型任务信息
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        public abstract class runReturnValue<returnType> : runReturn
        {
            /// <summary>
            /// 返回值
            /// </summary>
            protected returnType returnValue;
            /// <summary>
            /// 返回值
            /// </summary>
            public returnType Return
            {
                get
                {
                    tryRun();
                    if (error == null) return returnValue;
                    throw error;
                }
            }
        }
        /// <summary>
        /// 带返回值的泛型任务信息
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        public class runReturn<returnType> : runReturnValue<returnType>
        {
            /// <summary>
            /// 任务执行委托
            /// </summary>
            private func<returnType> func;
            /// <summary>
            /// 执行任务
            /// </summary>
            protected override void run()
            {
                returnValue = func();
            }
            /// <summary>
            /// 带返回值的泛型任务信息
            /// </summary>
            /// <param name="func">任务执行委托</param>
            public runReturn(func<returnType> func)
            {
                this.func = func;
            }
        }
        /// <summary>
        /// 带返回值的泛型任务信息
        /// </summary>
        /// <typeparam name="parameterType">任务执行参数类型</typeparam>
        /// <typeparam name="returnType">返回值类型</typeparam>
        public class runReturn<parameterType, returnType> : runReturnValue<returnType>
        {
            /// <summary>
            /// 任务执行委托
            /// </summary>
            private func<parameterType, returnType> func;
            /// <summary>
            /// 任务执行参数
            /// </summary>
            private parameterType parameter;
            /// <summary>
            /// 执行任务
            /// </summary>
            protected override void run()
            {
                returnValue = func(parameter);
            }
            /// <summary>
            /// 带返回值的泛型任务信息
            /// </summary>
            /// <param name="func">任务执行委托</param>
            /// <param name="parameter">任务执行参数</param>
            public runReturn(func<parameterType, returnType> func, parameterType parameter)
            {
                this.func = func;
                this.parameter = parameter;
            }
        }
        /// <summary>
        /// 线程池
        /// </summary>
        private threadPool threadPool;
        /// <summary>
        /// 新任务集合
        /// </summary>
        private list<taskInfo> tasks = new list<taskInfo>();
        /// <summary>
        /// 任务访问锁
        /// </summary>
        private readonly object taskLock = new object();
        /// <summary>
        /// 等待结束访问锁
        /// </summary>
        private readonly object waitLock = new object();
        /// <summary>
        /// 等待空闲访问锁
        /// </summary>
        private readonly object waitFreeLock = new object();
        /// <summary>
        /// 最大线程数量
        /// </summary>
        private int maxThreadCount;
        /// <summary>
        /// 线程数量
        /// </summary>
        private int threadCount;
        /// <summary>
        /// 空闲线程数量
        /// </summary>
        private int freeThreadCount;
        /// <summary>
        /// 已结束线程数量
        /// </summary>
        private int finallyThreadCount;
        /// <summary>
        /// 默认释放资源是否等待线程结束
        /// </summary>
        private bool isDisposeWait;
        /// <summary>
        /// 是否停止任务
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 任务处理
        /// </summary>
        /// <param name="count">线程数</param>
        /// <param name="isDisposeWait">默认释放资源是否等待线程结束</param>
        /// <param name="threadPool">线程池</param>
        public task(int count, bool isDisposeWait = true, threadPool threadPool = null)
        {
            if (count <= 0 || count > config.pub.Default.TaskMaxThreadCount) fastCSharp.log.Default.Throw(log.exceptionType.IndexOutOfRange);
            maxThreadCount = count;
            this.isDisposeWait = isDisposeWait;
            this.threadPool = threadPool ?? fastCSharp.threading.threadPool.Default;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(isDisposeWait);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isWait">是否等待线程结束</param>
        public void Dispose(bool isWait)
        {
            Monitor.Enter(taskLock);
            try
            {
                if (!isStop)
                {
                    isStop = isWait = true;
                    Monitor.PulseAll(taskLock);
                }
            }
            finally { Monitor.Exit(taskLock); }
            if (isWait)
            {
                Monitor.Enter(waitLock);
                try
                {
                    if (threadCount != finallyThreadCount) Monitor.Wait(waitLock);
                }
                finally { Monitor.Exit(waitLock); }
            }
        }
        /// <summary>
        /// 单线程添加任务后，等待所有线程空闲
        /// </summary>
        public void WaitFree()
        {
            bool isExit = false;
            Monitor.Enter(taskLock);
            try
            {
                Monitor.Enter(waitFreeLock);
                try
                {
                    if (threadCount != freeThreadCount || tasks.Count != 0)
                    {
                        isExit = true;
                        Monitor.Exit(taskLock);
                        Monitor.Wait(waitFreeLock);
                    }
                }
                finally { Monitor.Exit(waitFreeLock); }
            }
            finally
            {
                if (!isExit) Monitor.Exit(taskLock);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns>任务添加是否成功</returns>
        internal bool add(taskInfo task)
        {
            bool isTask = false;
            Monitor.Enter(taskLock);
            try
            {
                if (!isStop)
                {
                    if (threadCount != maxThreadCount && freeThreadCount == 0)
                    {
                        ++threadCount;
                        try
                        {
                            if (maxThreadCount == 1) threadPool.Start(runTask, Dispose);
                            else if (threadCount != 1) threadPool.Start(runManyTask);
                            else threadPool.Start(runManyTask, Dispose);
                        }
                        catch (Exception error)
                        {
                            log.Default.Add(error, null, true);
                            Monitor.Enter(waitFreeLock);
                            try
                            {
                                Monitor.Enter(waitLock);
                                try
                                {
                                    if (--threadCount == finallyThreadCount) Monitor.PulseAll(waitLock);
                                }
                                finally { Monitor.Exit(waitLock); }
                                if (threadCount == freeThreadCount) Monitor.PulseAll(waitFreeLock);
                            }
                            finally { Monitor.Exit(waitFreeLock); }
                        }
                    }
                    tasks.Add(task);
                    Monitor.Pulse(taskLock);
                    isTask = true;
                }
            }
            finally { Monitor.Exit(taskLock); }
            return isTask;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="run">任务执行委托</param>
        /// <param name="onError">任务执行出错委托,停止任务参数null</param>
        /// <returns>任务添加是否成功</returns>
        public bool Add(action run, action<Exception> onError = null)
        {
            return run != null && add(new taskInfo { Action = run, OnError = onError });
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <typeparam name="parameterType">执行参数类型</typeparam>
        /// <param name="run">任务执行委托</param>
        /// <param name="parameter">执行参数</param>
        /// <param name="onError">任务执行出错委托,停止任务参数null</param>
        /// <returns>任务添加是否成功</returns>
        public bool Add<parameterType>(action<parameterType> run, parameterType parameter, action<Exception> onError = null)
        {
            return run != null && add(new taskInfo
            {
                Action = run<parameterType>.Create(run, parameter),
                OnError = onError
            });
        }
        /// <summary>
        /// 添加带返回值的任务
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="func">任务执行委托</param>
        /// <returns>任务添加是否成功</returns>
        public runReturn<returnType> Add<returnType>(func<returnType> func)
        {
            if (func != null)
            {
                runReturn<returnType> value = new runReturn<returnType>(func);
                if (add(value.TaskInfo)) return value;
            }
            return null;
        }
        /// <summary>
        /// 添加带返回值的任务
        /// </summary>
        /// <typeparam name="parameterType">执行参数类型</typeparam>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="func">任务执行委托</param>
        /// <param name="parameter">执行参数</param>
        /// <returns>任务添加是否成功</returns>
        public runReturn<parameterType, returnType> Add<parameterType, returnType>(func<parameterType, returnType> func, parameterType parameter)
        {
            if (func != null)
            {
                runReturn<parameterType, returnType> value = new runReturn<parameterType, returnType>(func, parameter);
                if (add(value.TaskInfo)) return value;
            }
            return null;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        private void runTask()
        {
            try
            {
                list<taskInfo> runTasks = new list<taskInfo>(), oldTasks;
                while (true)
                {
                    Monitor.Enter(taskLock);
                    try
                    {
                        if (tasks.Count == 0)
                        {
                            if (isStop) break;
                            onThreadFree();
                            Monitor.Wait(taskLock);
                            --freeThreadCount;
                        }
                        oldTasks = runTasks;
                        runTasks = tasks;
                        tasks = oldTasks;
                    }
                    finally { Monitor.Exit(taskLock); }
                    foreach (taskInfo task in runTasks) task.Run();
                    runTasks.Empty();
                }
            }
            catch (Exception error)
            {
                log.Default.Real(error, null, false);
            }
            finally { onThreadFinally(); }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        private void runManyTask()
        {
            try
            {
                taskInfo popTask;
                while (true)
                {
                    Monitor.Enter(taskLock);
                    try
                    {
                        if (tasks.Count == 0)
                        {
                            if (isStop) break;
                            onThreadFree();
                            Monitor.Wait(taskLock);
                            --freeThreadCount;
                        }
                        popTask = tasks.Count != 0 ? tasks.Pop() : default(taskInfo);
                    }
                    finally { Monitor.Exit(taskLock); }
                    popTask.Run();
                }
            }
            catch (Exception error)
            {
                log.Default.Real(error, null, false);
            }
            finally { onThreadFinally(); }
        }
        /// <summary>
        /// 线程结束
        /// </summary>
        private void onThreadFinally()
        {
            Monitor.Enter(waitLock);
            try
            {
                if (++finallyThreadCount == threadCount) Monitor.PulseAll(waitLock);
            }
            finally { Monitor.Exit(waitLock); }
        }
        /// <summary>
        /// 线程空闲
        /// </summary>
        private void onThreadFree()
        {
            Monitor.Enter(waitFreeLock);
            try
            {
                if (threadCount == ++freeThreadCount) Monitor.PulseAll(waitFreeLock);
            }
            finally { Monitor.Exit(waitFreeLock); }
        }
        /// <summary>
        /// 默认任务
        /// </summary>
        public static readonly task Default = new task(config.pub.Default.TaskThreadCount);
        /// <summary>
        /// 微型线程任务
        /// </summary>
        public static readonly task TinyTask = new task(config.pub.Default.TinyThreadCount, true, threadPool.TinyPool);
    }
}
