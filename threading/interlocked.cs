using System;
using System.Threading;

namespace fastCSharp.threading
{
    /// <summary>
    /// 原子操作扩张
    /// </summary>
    public static class interlocked
    {
        /// <summary>
        /// 等待单次运行
        /// </summary>
        /// <param name="runState">执行状态,0表示未运行,1表示运行中,2表示已经运行</param>
        /// <param name="run">执行委托</param>
        public static void WaitRunOnce(ref int runState, action run)
        {
            int isRun = Interlocked.CompareExchange(ref runState, 1, 0);
            if (isRun == 0)
            {
                try
                {
                    run();
                }
                finally { runState = 2; }
            }
            else if (isRun == 1)
            {
                while (runState == 1) Thread.Sleep(1);
            }
        }
    }
}
