using System;
using System.Threading;
using System.Timers;

namespace fastCSharp.threading
{
    /// <summary>
    /// 定时任务信息
    /// </summary>
    public class timerTask : IDisposable
    {
        /// <summary>
        /// 定时任务信息
        /// </summary>
        private struct taskInfo
        {
            /// <summary>
            /// 任务信息
            /// </summary>
            public threading.taskInfo Info;
            /// <summary>
            /// 执行时间
            /// </summary>
            public DateTime RunTime;
        }
        /// <summary>
        /// 任务处理
        /// </summary>
        private task task;
        /// <summary>
        /// 已排序任务
        /// </summary>
        private collection<taskInfo> tasks = new collection<taskInfo>();
        /// <summary>
        /// 未排序任务
        /// </summary>
        private list<taskInfo> newTasks = new list<taskInfo>();
        /// <summary>
        /// 定时器
        /// </summary>
        private System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// 最近时间
        /// </summary>
        private DateTime nearTime = DateTime.MaxValue;
        /// <summary>
        /// 任务访问锁
        /// </summary>
        private readonly object taskLock = new object();
        /// <summary>
        /// 定时任务信息
        /// </summary>
        /// <param name="task">任务处理</param>
        public timerTask(task task)
        {
            this.task = task ?? task.Default;
            timer.Elapsed += onTimer;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
            tasks = null;
            newTasks = null;
        }
        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="task">任务信息</param>
        private void add(taskInfo task)
        {
            if (task.RunTime != DateTime.MaxValue)
            {
                DateTime now = DateTime.Now;
                if (now < task.RunTime)
                {
                    Monitor.Enter(taskLock);
                    try
                    {
                        if (task.RunTime < nearTime)
                        {
                            timer.Stop();
                            if (nearTime == DateTime.MaxValue) tasks.Add(task);
                            else tasks.AddExpand(task);
                            nearTime = task.RunTime;
                            timer.Interval = (task.RunTime - now).TotalMilliseconds;
                            timer.Start();
                        }
                        else if (newTasks.Count == 0 && task.RunTime >= tasks[tasks.Count - 1].RunTime) tasks.Add(task);
                        else newTasks.Add(task);
                    }
                    finally { Monitor.Exit(taskLock); }
                }
                else this.task.add(task.Info);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="run">任务执行委托</param>
        /// <param name="runTime">执行时间</param>
        /// <param name="onError">任务执行出错委托,停止任务参数null</param>
        public void Add(action run, DateTime runTime, action<Exception> onError = null)
        {
            if (run != null) add(new taskInfo { Info = new threading.taskInfo { Action = run, OnError = onError }, RunTime = runTime });
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <typeparam name="parameterType">执行参数类型</typeparam>
        /// <param name="run">任务执行委托</param>
        /// <param name="parameter">执行参数</param>
        /// <param name="runTime">执行时间</param>
        /// <param name="onError">任务执行出错委托,停止任务参数null</param>
        public void Add<parameterType>
            (action<parameterType> run, parameterType parameter, DateTime runTime, action<Exception> onError = null)
        {
            if (run != null)
            {
                add(new taskInfo
                {
                    Info = new threading.taskInfo
                    {
                        Action = run<parameterType>.Create(run, parameter),
                        OnError = onError
                    },
                    RunTime = runTime
                });
            }
        }
        /// <summary>
        /// 触发定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTimer(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            list<taskInfo> runs = null;
            Monitor.Enter(taskLock);
            try
            {
                timer.Stop();
                int count = 0;
                foreach (taskInfo task in tasks)
                {
                    if (task.RunTime <= now) ++count;
                    else break;
                }
                runs = new list<taskInfo>(tasks.GetSub(0, count), true);
                tasks.Sub(count, tasks.Count - count);
                if (newTasks.Count != 0)
                {
                    taskInfo[] newArray = newTasks.Unsafer.Array;
                    int left = 0;
                    while (left != newTasks.Count && newArray[left].RunTime > now) ++left;
                    if (left != newTasks.Count)
                    {
                        int right = newTasks.Count;
                        while (right != 0 && newArray[--right].RunTime <= now) ;
                        if (left < right)
                        {
                            do
                            {
                                taskInfo task = newArray[left];
                                newArray[left] = newArray[right];
                                newArray[right] = task;
                                while (newArray[++left].RunTime > now) ;
                                while (newArray[--right].RunTime <= now) ;
                            }
                            while (left < right);
                        }
                        runs.Add(newArray, left, newTasks.Count - left);
                        newTasks.Unsafer.AddLength(left - newTasks.Count);
                    }
                    if (newTasks.Count != 0)
                    {
                        newArray = newTasks.sort(value => value.RunTime);
                        if (tasks.Count == 0) tasks = new collection<taskInfo>(newArray, true);
                        else
                        {
                            tasks = new collection<taskInfo>(unsafer.array.GetMergeSort(tasks.ToArray(), newArray, value => value.RunTime), true);
                        }
                        newTasks.Empty();
                    }
                }
                if (tasks.Count != 0)
                {
                    taskInfo task = tasks[0];
                    nearTime = task.RunTime;
                    timer.Interval = (task.RunTime - now).TotalMilliseconds;
                    timer.Start();
                }
                else nearTime = DateTime.MaxValue;
            }
            catch (Exception error)
            {
                fastCSharp.log.Default.Add(error.ToString());
                timer.Interval = 1;
                timer.Start();
            }
            finally
            {
                Monitor.Exit(taskLock);
                foreach (taskInfo task in runs) task.Info.Run();
            }
        }
        /// <summary>
        /// 默认定时任务
        /// </summary>
        public static readonly timerTask Default = new timerTask(null);
    }
}
