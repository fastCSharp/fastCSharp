using System;

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 数组扩展操作(非安全,请自行确保数据可靠性)
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        public static void Move<valueType>(valueType[] array, int index, int writeIndex, int count)
        {
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                for (int writeEndIndex = writeIndex + count; endIndex != index; array[--writeEndIndex] = array[--endIndex]) ;
            }
            else Array.Copy(array, index, array, writeIndex, count);
        }
	}
}
