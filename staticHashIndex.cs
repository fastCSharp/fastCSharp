using System;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希索引
    /// </summary>
    public abstract class staticHashIndex
    {
        /// <summary>
        /// 索引范围
        /// </summary>
        protected struct range
        {
            /// <summary>
            /// 起始位置
            /// </summary>
            public int StartIndex;
            /// <summary>
            /// 结束位置
            /// </summary>
            public int EndIndex;
            /// <summary>
            /// 重置索引范围
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            public void Set(int startIndex, int endIndex)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">数据</param>
        /// <returns>哈希值</returns>
        protected static int getHashCode<valueType>(valueType value)
        {
            if (value != null)
            {
                int code = value.GetHashCode();
                return code ^ (code >> defaultArrayLengthBits);
            }
            return 0;
        }
        /// <summary>
        /// 默认索引集合尺寸二进制位数
        /// </summary>
        protected const int defaultArrayLengthBits = 4;
        /// <summary>
        /// 默认索引集合
        /// </summary>
        protected static range[] defaultIndexs = new range[1 << defaultArrayLengthBits];

        /// <summary>
        /// 索引&值
        /// </summary>
        protected int indexAnd;
        /// <summary>
        /// 索引集合
        /// </summary>
        protected range[] indexs;
        /// <summary>
        /// 获取哈希数据数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="hashs">哈希集合</param>
        /// <returns>哈希数据数组</returns>
        protected unsafe valueType[] getValues<valueType>(valueType[] values, int* hashs)
        {
            int indexBits = ((uint)values.Length).bits();
            if (indexBits < defaultArrayLengthBits) indexBits = defaultArrayLengthBits;
            else if ((1 << (indexBits - 1)) == values.Length) --indexBits;
            indexAnd = 1 << indexBits;
            indexs = new range[indexAnd--];
            fixed (range* indexFixed = indexs)
            {
                for (int* hash = hashs + values.Length; hash != hashs; ++indexFixed[*--hash & indexAnd].StartIndex) ;
                int startIndex = 0;
                for (range* index = indexFixed, endIndex = indexFixed + indexAnd + 1; index != endIndex; ++index)
                {
                    int nextIndex = startIndex + (*index).StartIndex;
                    (*index).StartIndex = (*index).EndIndex = startIndex;
                    startIndex = nextIndex;
                }
                valueType[] newValues = new valueType[values.Length];
                foreach (valueType value in values) newValues[indexFixed[*hashs++ & indexAnd].EndIndex++] = value;
                //for (int index = 0; index != values.Length; newValues[indexFixed[*hashs++ & indexAnd].EndIndex++] = values[index++]) ;
                return newValues;
            }
        }
    }
}
