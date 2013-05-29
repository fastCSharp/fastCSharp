using System;
/*Type:ulong,ulongSortIndex;long,longSortIndex;uint,uintSortIndex;int,intSortIndex;ushort,ushortSortIndex;short,shortSortIndex;byte,byteSortIndex;sbyte,sbyteSortIndex;double,doubleSortIndex;float,floatSortIndex;DateTime,dateTimeSortIndex*/

namespace fastCSharp.algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static partial class quickSort
    {
        /// <summary>
        /// 排序索引
        /// </summary>
        public struct /*Type[1]*/ulongSortIndex/*Type[1]*/
        {
            /// <summary>
            /// 数值
            /// </summary>
            public /*Type[0]*/ulong/*Type[0]*/ Value;
            /// <summary>
            /// 位置索引
            /// </summary>
            public int Index;

            /// <summary>
            /// 根据数组获取排序索引
            /// </summary>
            /// <typeparam name="valueType">数组类型</typeparam>
            /// <param name="values">数组</param>
            /// <param name="getValue">数据排序值获取器</param>
            /// <returns>排序索引</returns>
            public unsafe static /*Type[1]*/ulongSortIndex/*Type[1]*/[] Get<valueType>(valueType[] values, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
            {
                if (values.length() != 0)
                {
                    if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                    /*Type[1]*/
                    ulongSortIndex/*Type[1]*/[] indexs = new /*Type[1]*/ulongSortIndex/*Type[1]*/[values.Length];
                    fixed (/*Type[1]*/ulongSortIndex/*Type[1]*/* indexFixed = indexs)
                    {
                        /*Type[1]*/
                        ulongSortIndex/*Type[1]*/* write = indexFixed;
                        for (int index = 0; index != values.Length; ++index)
                        {
                            (*write).Value = getValue(values[index]);
                            (*write).Index = index;
                            ++write;
                        }
                    }
                    return indexs;
                }
                return nullValue</*Type[1]*/ulongSortIndex/*Type[1]*/>.Array;
            }
        }
    }
}