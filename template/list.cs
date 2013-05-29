using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;DateTime*/

namespace fastCSharp
{
    /// <summary>
    /// 单向动态数组扩展
    /// </summary>
    public static partial class list
    {
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> add
            (this list</*Type[0]*/ulong/*Type[0]*/> list, System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (list != null)
            {
                int count = values.count();
                if (count != 0)
                {
                    int index = list.Count;
                    list.AddLength(count);
                    fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = list.Unsafer.Array)
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/* write = valueFixed + index;
                        foreach (/*Type[0]*/ulong/*Type[0]*/ nextValue in values) *write++ = nextValue;
                    }
                }
                return list;
            }
            return values.getList();
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf(this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            return list.count() != 0 ? list.Unsafer.Array.indexOf(0, list.Count, value) : -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public unsafe static int indexOf(this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return list.count() != 0 ? list.Unsafer.Array.indexOf(0, list.Count, isValue) : -1;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">数据位置</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> removeAt(this list</*Type[0]*/ulong/*Type[0]*/> list, int index)
        {
            int count = list.count();
            if ((uint)index < (uint)count)
            {
                unsafer.array.Move(list.Unsafer.Array, index + 1, index, --count - index);
                list.Unsafer.AddLength(-1);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return list;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">数据位置</param>
        /// <returns>被移除的数据</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ getRemoveAt(this list</*Type[0]*/ulong/*Type[0]*/> list, int index)
        {
            int count = list.count();
            if ((uint)index < (uint)count)
            {
                list</*Type[0]*/ulong/*Type[0]*/>.unsafer values = new list</*Type[0]*/ulong/*Type[0]*/>.unsafer { List = list };
                /*Type[0]*/
                ulong/*Type[0]*/ value = values.Array[index];
                unsafer.array.Move(values.Array, index + 1, index, --count - index);
                values.AddLength(-1);
                return value;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(/*Type[0]*/ulong/*Type[0]*/);
        }
        /// <summary>
        /// 移除第一个数据
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">被移除的数据</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> removeFirst
            (this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (list != null)
            {
                int index = list.indexOf(value);
                if (index >= 0) list.removeAt(index);
            }
            return list;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>单向动态数组</returns>
        public unsafe static list</*Type[0]*/ulong/*Type[0]*/> remove
            (this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            int count = list.count();
            if (count != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = list.Unsafer.Array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* start = valueFixed, end = valueFixed + count;
                    while (start != end && !isValue(*start)) ++start;
                    /*Type[0]*/
                    ulong/*Type[0]*/* write = start;
                    while (start != end)
                    {
                        if (!isValue(*start)) *write++ = *start;
                        ++start;
                    }
                    list.Unsafer.AddLength((int)(write - valueFixed) - count);
                }
            }
            return list;
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> removeRange(this list</*Type[0]*/ulong/*Type[0]*/> list, int index, int count)
        {
            int valueCount = list.count();
            if (index + count <= valueCount && index >= 0 && count >= 0 && list != null)
            {
                list.Unsafer.AddLength(-count);
                unsafer.array.Move(list.Unsafer.Array, index + count, index, list.Count - index);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return list;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">子集合数据数量</param>
        /// <param name="getVlaue">数据获取器</param>
        /// <returns>子集合</returns>
        public unsafe static valueType[] getSub<valueType>
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int startIndex, int count, func</*Type[0]*/ulong/*Type[0]*/, valueType> getVlaue)
        {
            array.range range = new array.range(list.count(), startIndex, count);
            return (count = range.GetCount) != 0 ? list.Unsafer.Array.getSub(range.SkipCount, count, getVlaue) : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public static bool contains(this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            return list.indexOf(value) != -1;
        }
        /// <summary>
        /// 判断是否存在匹配
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配</returns>
        public static bool any(this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return list.indexOf(isValue) != -1;
        }
        /// <summary>
        /// 替换第一个匹配值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">新的数据值</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> replaceFirst
            (this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ value, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (list != null)
            {
                int index = list.indexOf(isValue);
                if (index != -1) list.Unsafer.Array[index] = value;
            }
            return list;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(/*Type[0]*/ulong/*Type[0]*/)</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ firstOrDefault
            (this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            int index = list.indexOf(isValue);
            return index != -1 ? list.Unsafer.Array[index] : default(/*Type[0]*/ulong/*Type[0]*/);
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> getFind
            (this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return list.count() != 0 ? list.Unsafer.Array.getFind(0, list.Count, isValue) : null;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray
            (this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return list.count() != 0 ? list.Unsafer.Array.getFindArray(0, list.Count, isValue) : nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count(this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return list.count() != 0 ? list.Unsafer.Array.count(0, list.Count, isValue) : 0;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> clear(this list</*Type[0]*/ulong/*Type[0]*/> list)
        {
            if (list != null) list.Unsafer.AddLength(-list.Count);
            return list;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="method">调用函数</param>
        /// <returns>单向动态数组</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> each
            (this list</*Type[0]*/ulong/*Type[0]*/> list, action</*Type[0]*/ulong/*Type[0]*/> method)
        {
            if (list.count() != 0) list.Unsafer.Array.each(0, list.Count, method);
            return list;
        }
        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号,从1开始</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>分页数据数组</returns>
        public static valueType[] getPage<valueType>
            (this list</*Type[0]*/ulong/*Type[0]*/> list, int pageSize, int currentPage, func</*Type[0]*/ulong/*Type[0]*/, valueType> getValue)
        {
            array.page page = new array.page(list.count(), pageSize, currentPage);
            return list.getSub(page.SkipCount, page.CurrentPageSize, getValue);
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            return list.count() != 0 ? list.Unsafer.Array.getSub(0, list.Count, getValue) : nullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max(this list</*Type[0]*/ulong/*Type[0]*/> list, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (list.count() != 0) return list.Unsafer.Array.max(0, list.Count, out value);
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (list.count() != 0) return list.Unsafer.Array.maxKey(0, list.Count, getKey, out value);
            value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (list.count() != 0) return list.Unsafer.Array.max(0, list.Count, getKey, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ max(this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return max(list, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ maxKey<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return maxKey(list, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return max(list, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min(this list</*Type[0]*/ulong/*Type[0]*/> list, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (list.count() != 0) return list.Unsafer.Array.min(0, list.Count, out value);
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (list.count() != 0) return list.Unsafer.Array.minKey(0, list.Count, getKey, out value);
            value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (list.count() != 0) return list.Unsafer.Array.min(0, list.Count, getKey, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="list">单向动态数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ min(this list</*Type[0]*/ulong/*Type[0]*/> list, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return min(list, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ minKey<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
            return minKey(list, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">比较值获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return min(list, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public static list<valueType> distinct<valueType>
            (this list</*Type[0]*/ulong/*Type[0]*/> list, func</*Type[0]*/ulong/*Type[0]*/, valueType> getValue)
        {
            if (list.count() != 0) return list.Unsafer.Array.distinct(0, list.Count, getValue);
            return null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        public static list</*Type[0]*/ulong/*Type[0]*/> distinct<valueType>
            (this list<valueType> list, func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (list.count() != 0) return list.Unsafer.Array.distinct(0, list.Count, getValue);
            return null;
        }
    }
}
