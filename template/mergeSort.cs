using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;DateTime*/
/*Compare:,<=,<;Desc,>=,>*/

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 数组扩展操作(非安全,请自行确保数据可靠性)
    /// </summary>
    public static partial class array
    {
        /// <summary>
        /// 归并排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="left">左侧数组</param>
        /// <param name="right">右侧数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <returns>归并排序结果</returns>
        public static valueType[] GetMergeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] left, valueType[] right, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            int leftLength = left.length();
            if (leftLength == 0) return right.copy();
            int rightLenght = right.length();
            if (rightLenght == 0) return left.copy();
            int newIndex = 0, cmpIndex = 1;
            valueType[] newValues = new valueType[leftLength + rightLenght];
            if (getKey(left[leftLength - 1]) /*Compare[1]*/<=/*Compare[1]*/ getKey(right[rightLenght - 1]))
            {
                valueType cmpValue = right[0];
                /*Type[0]*/
                ulong/*Type[0]*/ cmpKey = getKey(cmpValue);
                foreach (valueType value in left)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ key = getKey(value);
                    while (cmpKey /*Compare[2]*/</*Compare[2]*/ key)
                    {
                        newValues[newIndex++] = cmpValue;
                        cmpKey = getKey(cmpValue = right[cmpIndex++]);
                    }
                    newValues[newIndex++] = value;
                }
                newValues[newIndex++] = cmpValue;
                Array.Copy(right, cmpIndex, newValues, newIndex, newValues.Length - newIndex);
            }
            else
            {
                valueType cmpValue = left[0];
                /*Type[0]*/
                ulong/*Type[0]*/ cmpKey = getKey(cmpValue);
                foreach (valueType value in right)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ key = getKey(value);
                    while (cmpKey /*Compare[1]*/<=/*Compare[1]*/ key)
                    {
                        newValues[newIndex++] = cmpValue;
                        cmpKey = getKey(cmpValue = left[cmpIndex++]);
                    }
                    newValues[newIndex++] = value;
                }
                newValues[newIndex++] = cmpValue;
                Array.Copy(left, cmpIndex, newValues, newIndex, newValues.Length - newIndex);
            }
            return newValues;
        }
    }
}
