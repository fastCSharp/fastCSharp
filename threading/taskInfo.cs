using System;
using System.Diagnostics;

namespace fastCSharp.threading
{
    /// <summary>
    /// 任务信息
    /// </summary>
    internal struct taskInfo
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        public action Action;
        /// <summary>
        /// 任务执行出错委托,停止任务参数null
        /// </summary>
        public action<Exception> OnError;
        /// <summary>
        /// 执行任务
        /// </summary>
        public void Run()
        {
            if (Action != null)
            {
                Exception exception = null;
                try
                {
                    Action();
                }
                catch (Exception error)
                {
                    if (OnError == null) fastCSharp.log.Default.Add(error, null, false);
                    else exception = error;
                }
                if (exception != null)
                {
                    try
                    {
                        OnError(exception);
                    }
                    catch (Exception error)
                    {
                        fastCSharp.log.Default.Add(error, null, false);
                    }
                }
            }
        }
    }
}
