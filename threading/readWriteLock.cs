using System;
using System.Threading;

namespace fastCSharp.threading
{
    /// <summary>
    /// 读写锁(写优先)，用于高并发的IO读取操作
    /// </summary>
    public class readWriteLock
    {
        /// <summary>
        /// 锁状态访问锁
        /// </summary>
        private int stateLock;
        /// <summary>
        /// 写操作等待数量(等待读取完毕)
        /// </summary>
        private int waitWriteCount;
        /// <summary>
        /// 当前写操作数量
        /// </summary>
        private int writeCount;
        /// <summary>
        /// 读操作等待数量(等待写入完毕)
        /// </summary>
        private int waitReadCount;
        /// <summary>
        /// 当前读操作数量
        /// </summary>
        private int readCount;
        /// <summary>
        /// 等待读操作锁
        /// </summary>
        private readonly object waitReadLock = new object();
        /// <summary>
        /// 等待写操作锁
        /// </summary>
        private readonly object waitWriteLock = new object();
        /// <summary>
        /// 写操作锁
        /// </summary>
        private readonly object writeLock = new object();
        /// <summary>
        /// 是否正在唤醒 读操作等待
        /// </summary>
        private bool isPulseRead;
        /// <summary>
        /// 是否正在唤醒 写操作等待
        /// </summary>
        private bool isPulseWrite;
        /// <summary>
        /// 添加一个读取状态
        /// </summary>
        public void EnterRead()
        {
            int isWait = 1;
            while (true)
            {
                while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
                if (writeCount == 0)
                {
                    waitReadCount -= isWait ^ 1;
                    ++readCount;
                    stateLock = 0;
                    break;
                }
                waitReadCount += isWait;
                stateLock = 0;
                isWait = 0;
                Monitor.Enter(waitReadLock);
                try
                {
                    Monitor.Wait(waitReadLock);
                }
                finally { Monitor.Exit(waitReadLock); }
            }
        }
        /// <summary>
        /// 结束一个读取状态
        /// </summary>
        public void ExitRead()
        {
            while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
            if (--readCount == 0 && waitWriteCount != 0 && !isPulseWrite)
            {
                isPulseWrite = true;
                stateLock = 0;
                do
                {
                    Monitor.Enter(waitWriteLock);
                    try
                    {
                        Monitor.PulseAll(waitWriteLock);
                    }
                    finally { Monitor.Exit(waitWriteLock); }
                    Thread.Sleep(1);
                    while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
                    if (readCount != 0 || waitWriteCount == 0)
                    {
                        isPulseWrite = false;
                        stateLock = 0;
                        break;
                    }
                    stateLock = 0;
                }
                while (true);
            }
            else stateLock = 0;
        }
        /// <summary>
        /// 添加一个写入状态
        /// </summary>
        public void EnterWrite()
        {
            while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
            ++writeCount;
            if (readCount == 0) stateLock = 0;
            else
            {
                ++waitWriteCount;
                stateLock = 0;
                Monitor.Enter(waitWriteLock);
                try
                {
                    Monitor.Wait(waitWriteLock);
                }
                finally { Monitor.Exit(waitWriteLock); }
                while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
                --waitWriteCount;
                stateLock = 0;
            }
            Monitor.Enter(writeLock);
        }
        /// <summary>
        /// 结束一个写入状态
        /// </summary>
        public void ExitWrite()
        {
            Monitor.Exit(writeLock);
            while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
            if (--writeCount == 0 && waitReadCount != 0 && !isPulseRead)
            {
                isPulseRead = true;
                stateLock = 0;
                do
                {
                    Monitor.Enter(waitReadLock);
                    try
                    {
                        Monitor.PulseAll(waitReadLock);
                    }
                    finally { Monitor.Exit(waitReadLock); }
                    Thread.Sleep(1);
                    while (Interlocked.CompareExchange(ref stateLock, 1, 0) != 0) Thread.Sleep(1);
                    if (writeCount != 0 || waitReadCount == 0)
                    {
                        isPulseRead = false;
                        stateLock = 0;
                        break;
                    }
                    stateLock = 0;
                }
                while (true);
            }
            else stateLock = 0;
        }
    }
}
