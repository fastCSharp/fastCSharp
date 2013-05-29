using System;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// 延时排序数据
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public struct listOrderLadyValue<valueType>
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        internal list<valueType> List;
        /// <summary>
        /// 数据数组
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 数据数组是否排序
        /// </summary>
        internal bool IsSort;
        /// <summary>
        /// 获取匹配分页集合
        /// </summary>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="count">匹配数据总数</param>
        /// <returns>匹配分页集合</returns>
        internal list<valueType> GetArrayPage(int pageSize, int currentPage, func<valueType, bool> isValue, out int count)
        {
            int valueCount = 0;
            array.page page = new array.page(Array.Length, pageSize, currentPage);
            int currentPageSize = page.CurrentPageSize;
            list<valueType>.unsafer values = new list<valueType>(page.CurrentPageSize).Unsafer;
            int skipCount = page.SkipCount;
            currentPageSize += skipCount;
            foreach (valueType value in Array)
            {
                if (isValue(value))
                {
                    if (valueCount >= skipCount && valueCount < currentPageSize) values.Add(value);
                    ++valueCount;
                }
            }
            count = valueCount;
            return values.List;
        }
    }
}
