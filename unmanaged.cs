using System;
using System.Runtime.InteropServices;

namespace fastCSharp
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    public unsafe static class unmanaged
    {
        ///// <summary>
        ///// 指针
        ///// </summary>
        //private byte* data;
        ///// <summary>
        ///// 指针
        ///// </summary>
        //public byte* Data
        //{
        //    get { return data; }
        //}
        ///// <summary>
        ///// 字节长度
        ///// </summary>
        //private int size;
        ///// <summary>
        ///// 非托管内存
        ///// </summary>
        ///// <param name="size">内存字节数</param>
        ///// <param name="isClear">是否需要清除</param>
        //public unmanaged(int size, bool isClear = true)
        //{
        //    data = Get(size, isClear);
        //    this.size = size;
        //}
        ///// <summary>
        ///// 释放内存
        ///// </summary>
        //public void Free()
        //{
        //    Free(data);
        //    data = null;
        //    size = 0;
        //}
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        public static pointer Get(int size, bool isClear = true)
        {
            if (size < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            if (size != 0)
            {
                byte* data = (byte*)Marshal.AllocHGlobal(size);
                if (isClear) fastCSharp.unsafer.memory.Fill(data, (byte)0, size);
                return new pointer { Data = data };
            }
            return default(pointer);
        }
        /// <summary>
        /// 批量申请非托管内存
        /// </summary>
        /// <param name="isClear">是否需要清除</param>
        /// <param name="getters">内存字节数集合</param>
        /// <returns>非托管内存起始指针</returns>
        public static pointer[] Get(bool isClear, params int[] sizes)
        {
            if (sizes.length() != 0)
            {
                int sum = 0;
                foreach (int size in sizes)
                {
                    if (size < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
                    checked { sum += size; }
                }
                pointer pointer = Get(sum, isClear);
                byte* data = pointer.Byte;
                if (data != null)
                {
                    int index = 0;
                    pointer[] datas = new pointer[sizes.Length];
                    foreach (int size in sizes)
                    {
                        datas[index++] = new pointer { Data = data };
                        data += size;
                    }
                    return datas;
                }
            }
            return null;
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        public static void Free(void* data)
        {
            if (data != null) Marshal.FreeHGlobal((IntPtr)data);
        }
    }
}
