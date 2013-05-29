using System;
using System.Collections;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 单向动态数组
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    [fastCSharp.setup.cSharp.serialize(SerializeType = typeof(fastCSharp.setup.cSharp.serialize.ISerializeGeneric[]))]
    public class list<valueType> : dynamicArray<valueType>, IList<valueType>
    {
        /// <summary>
        /// 非安全访问单向动态数组(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 单向动态数组
            /// </summary>
            public list<valueType> List;
            /// <summary>
            /// 数据数组
            /// </summary>
            public valueType[] Array
            {
                get { return List.array; }
            }
            /// <summary>
            /// 设置或获取值
            /// </summary>
            /// <param name="index">位置</param>
            /// <returns>数据值</returns>
            public valueType this[int index]
            {
                get
                {
                    return Array[index];
                }
                set
                {
                    Array[index] = value;
                }
            }
            /// <summary>
            /// 清除数据
            /// </summary>
            public void Clear()
            {
                System.Array.Clear(List.array, 0, List.length);
                List.Empty();
            }
            /// <summary>
            /// 增加数据长度
            /// </summary>
            /// <param name="length">增加数据长度</param>
            public void AddLength(int length)
            {
                List.length += length;
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            public void Add(valueType value)
            {
                List.array[List.length++] = value;
            }
            /// <summary>
            /// 添加数据集合
            /// </summary>
            /// <param name="values">数据集合</param>
            /// <param name="index">起始位置</param>
            /// <param name="count">数量</param>
            public void Add(valueType[] values, int index, int count)
            {
                System.Array.Copy(values, index, Array, List.length, count);
                List.length += count;
            }
            /// <summary>
            /// 移除数据范围
            /// </summary>
            /// <param name="index">起始位置</param>
            public void RemoveRange(int index)
            {
                List.length = index;
            }
            /// <summary>
            /// 弹出最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            public valueType Pop()
            {
                return List.array[--List.length];
            }
            /// <summary>
            /// 转换成前端子段集合
            /// </summary>
            /// <param name="count">数量</param>
            public void Left(int count)
            {
                List.length = count;
            }
            /// <summary>
            /// 转换成子集合
            /// </summary>
            /// <param name="index">起始位置</param>
            /// <returns>子集合</returns>
            public collection<valueType> Sub(int index)
            {
                return new collection<valueType>(Array, index, List.length - index, true);
            }
            /// <summary>
            /// 转换成子集合()
            /// </summary>
            /// <param name="index">起始位置</param>
            /// <param name="count">数量,小于0表示所有</param>
            /// <returns>子集合</returns>
            public collection<valueType> Sub(int index, int count)
            {
                return new collection<valueType>(Array, index, count, true);
            }
        }
        /// <summary>
        /// 非安全访问单向动态数组
        /// </summary>
        /// <returns>非安全访问单向动态数组</returns>
        public unsafer Unsafer
        {
            get
            {
                return new unsafer { List = this };
            }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        private int length;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return length; }
        }
        /// 数据数量
        /// </summary>
        protected override int ValueCount { get { return length; } }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public valueType this[int index]
        {
            get
            {
                if ((uint)index < (uint)length) return array[index];
                log.Default.Throw(log.exceptionType.IndexOutOfRange);
                return default(valueType);
            }
            set
            {
                if ((uint)index < (uint)length) array[index] = value;
                else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
        }
        /// <summary>
        /// 根据位置设置数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <param name="value">数据</param>
        public void Set(int index, valueType value)
        {
            if (index < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            int count = index - length;
            if (count >= 0) addLength(++count);
            array[index] = value;
        }

        /// <summary>
        /// 单向动态数据
        /// </summary>
        public list() { }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="count">数组容器尺寸</param>
        public list(int count)
        {
            if (count > 0) array = new valueType[count];
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="values">数据集合</param>
        public list(ICollection<valueType> values) : this(values == null ? null : values.getArray(), true) { }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="values">数据数组</param>
        /// <param name="isUnsafe">true表示使用原数组,false表示需要复制数组</param>
        public list(valueType[] values, bool isUnsafe = false)
        {
            if ((length = values.length()) != 0)
            {
                if (isUnsafe) array = values;
                else Array.Copy(values, 0, array = getNewArray(length), 0, length);
            }
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="values">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <param name="isUnsafe">true表示可以使用原数组,false表示需要复制数组</param>
        public list(valueType[] values, int index, int count, bool isUnsafe = false)
        {
            int length = values.length();
            array.range range = new array.range(length, index, count);
            this.length = range.GetCount;
            if (isUnsafe)
            {
                if (this.length == 0)
                {
                    if (length != 0) array = values;
                }
                else
                {
                    if ((index = range.SkipCount) == 0) array = values;
                    else Array.Copy(values, index, array = getNewArray(count), 0, this.length);
                }
            }
            else if (this.length != 0)
            {
                Array.Copy(values, range.SkipCount, array = getNewArray(count), 0, this.length);
            }
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value">单向动态数组</param>
        /// <returns>数组数据</returns>
        public static explicit operator valueType[](list<valueType> value)
        {
            return value != null ? value.ToArray() : null;
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value">数组数据</param>
        /// <returns>单向动态数组</returns>
        public static explicit operator list<valueType>(valueType[] value)
        {
            return value != null ? new list<valueType>(value, true) : null;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (length != 0) return new arrayEnumerator(this);
            return defaultNullEnumerator;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<valueType>)this).GetEnumerator();
        }
        /// <summary>
        /// 长度设为0
        /// </summary>
        public void Empty()
        {
            length = 0;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            if (length != 0)
            {
                if (isClearArray) Array.Clear(array, 0, array.Length);
                Empty();
            }
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="count">数据长度</param>
        private void setLength(int count)
        {
            valueType[] newArray = getNewArray(count);
            Array.Copy(array, 0, newArray, 0, length);
            array = newArray;
        }
        /// <summary>
        /// 增加数据长度
        /// </summary>
        /// <param name="count">数据长度</param>
        private void addLength(int count)
        {
            count += length;
            if (array != null)
            {
                if (count > array.Length) setLength(count);
            }
            else array = getNewArray(count > sizeof(int) ? count : sizeof(int));
            length = count;
        }
        /// <summary>
        /// 增加数据长度
        /// </summary>
        /// <param name="count">数据长度</param>
        public void AddLength(int count)
        {
            if (count > 0) addLength(count);
            else
            {
                count += length;
                if (count >= 0) length = count;
                else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(valueType value)
        {
            if (length != 0)
            {
                if (length == array.Length) setLength(array.Length << 1);
            }
            else if (array == null) array = new valueType[sizeof(int)];
            array[length++] = value;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ICollection<valueType> values)
        {
            int count = values.count();
            if (count != 0)
            {
                int index = length, oldLength = length;
                addLength(count);
                foreach (valueType value in values) array[index++] = value;
                length += index - oldLength - count;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        public override void Add(valueType[] values, int index, int count)
        {
            array.range range = new array.range(values.length(), index, count);
            if ((count = range.GetCount) != 0)
            {
                index = this.length;
                addLength(count);
                Array.Copy(values, range.SkipCount, array, index, count);
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        public void Insert(int index, valueType value)
        {
            if ((uint)index <= (uint)length)
            {
                if (index != length)
                {
                    if (length != array.Length)
                    {
                        fastCSharp.unsafer.array.Move(array, index, index + 1, length - index);
                        array[index] = value;
                        ++length;
                    }
                    else
                    {
                        valueType[] values = getNewArray(array.Length << 1);
                        Array.Copy(array, 0, values, 0, index);
                        values[index] = value;
                        Array.Copy(array, index, values, ++index, ++length - index);
                        array = values;
                    }
                }
                else Add(value);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        public void Insert(int index, IEnumerable<valueType> values)
        {
            if (values != null)
            {
                list<valueType> newValues = values.getList();
                Insert(index, newValues.array, 0, newValues.length);
            }
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        public void Insert(int index, ICollection<valueType> values)
        {
            if ((uint)index <= (uint)this.length)
            {
                int count = values.count();
                if (count != 0)
                {
                    int length = this.length + count;
                    if (array != null)
                    {
                        if (length <= array.Length)
                        {
                            fastCSharp.unsafer.array.Move(array, index, index + count, this.length - index);
                            foreach (valueType value in values) array[index++] = value;
                        }
                        else
                        {
                            valueType[] newValues = getNewArray(length);
                            Array.Copy(array, 0, newValues, 0, index);
                            foreach (valueType value in values) array[index++] = value;
                            Array.Copy(array, index -= count, newValues, index + count, this.length - index);
                            array = newValues;
                        }
                        this.length = length;
                    }
                    else
                    {
                        array = getNewArray(length);
                        foreach (valueType value in values) array[this.length++] = value;
                    }
                }
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        public void Insert(int index, valueType[] values)
        {
            if (values != null) Insert(index, values, 0, values.Length);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">插入数量</param>
        public void Insert(int index, valueType[] values, int startIndex, int count)
        {
            if ((uint)index <= (uint)this.length)
            {
                array.range range = new array.range(values.length(), startIndex, count);
                if ((count = range.GetCount) != 0)
                {
                    int length = this.length + count;
                    if (array != null)
                    {
                        if (length <= array.Length)
                        {
                            fastCSharp.unsafer.array.Move(array, index, index + count, this.length - index);
                            Array.Copy(values, range.SkipCount, array, index, count);
                        }
                        else
                        {
                            valueType[] newValues = getNewArray(length);
                            Array.Copy(array, 0, newValues, 0, index);
                            Array.Copy(values, startIndex, newValues, index, count);
                            Array.Copy(array, index, newValues, index + count, this.length - index);
                            array = newValues;
                        }
                    }
                    else Array.Copy(values, range.SkipCount, array = getNewArray(length), 0, length);
                    this.length = length;
                }
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据集合</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">插入数量</param>
        public void Insert(int index, list<valueType> value, int startIndex, int count)
        {
            if (value != null) Insert(index, value.array, startIndex, count);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        public void CopyTo(valueType[] values, int index)
        {
            if (values != null && index >= 0 && length + index <= values.Length)
            {
                Array.Copy(array, 0, values, index, length);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public override void RemoveAt(int index)
        {
            if ((uint)index < (uint)length)
            {
                fastCSharp.unsafer.array.Move(array, index + 1, index, --length - index);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public override valueType GetRemoveAt(int index)
        {
            if ((uint)index < (uint)length)
            {
                valueType value = array[index];
                fastCSharp.unsafer.array.Move(array, index + 1, index, --length - index);
                return value;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public override int IndexOf(valueType value)
        {
            return length != 0 ? Array.IndexOf<valueType>(array, value, 0, length) : -1;
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public override int IndexOf(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            return indexOf(isValue);
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        protected override int indexOf(func<valueType, bool> isValue)
        {
            if (length != 0)
            {
                int index = 0;
                foreach (valueType value in array)
                {
                    if (isValue(value)) return index;
                    if (++index == length) break;
                }
            }
            return -1;
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        public void RemoveRange(int index)
        {
            if ((uint)index <= (uint)length) length = index;
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        public void RemoveRange(int index, int count)
        {
            if (index + count <= length && index >= 0 && count >= 0)
            {
                fastCSharp.unsafer.array.Move(array, index + count, index, (length -= count) - index);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns>最后一个数据</returns>
        public valueType Pop()
        {
            if (length != 0) return array[--length];
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最后一个数据,失败返回默认空值</returns>
        public valueType Pop(valueType nullValue)
        {
            return length != 0 ? array[--length] : nullValue;
        }
        /// <summary>
        /// 移除所有后端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeEnd(func<valueType, bool> isValue)
        {
            while (--length >= 0 && isValue(array[length])) ;
            ++length;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        public void Remove(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            removeEnd(isValue);
            int index = IndexOf(isValue);
            if (index != -1)
            {
                for (int start = index; ++start != length; )
                {
                    if (!isValue(array[start])) array[index++] = array[start];
                }
                length = index;
            }
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        public void Reverse()
        {
            if (length != 0) Array.Reverse(array, 0, length);
        }
        /// <summary>
        /// 转换成前端子段集合
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public list<valueType> Left(int count)
        {
            if (count >= 0)
            {
                if (count < length) length = count;
                return this;
            }
            return null;
        }
        /// <summary>
        /// 转换成子集合(不清除数组)
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public collection<valueType> Sub(int index, int count)
        {
            array.range range = new array.range(length, index, count < 0 ? length - index : count);
            if (range.GetCount > 0)
            {
                return new collection<valueType>(array, range.SkipCount, range.GetCount, true);
            }
            return null;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public valueType[] GetSub(int index, int count)
        {
            array.range range = new array.range(length, index, count);
            if (range.GetCount > 0)
            {
                valueType[] values = new valueType[range.GetCount];
                Array.Copy(array, range.SkipCount, values, 0, range.GetCount);
                return values;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public int GetCount(func<valueType, bool> isValue)
        {
            int count = 0;
            if (length != 0)
            {
                if (isValue == null) log.Default.Throw(log.exceptionType.Null);
                int index = length;
                foreach (valueType value in array)
                {
                    if (isValue(value)) ++count;
                    if (--index == 0) break;
                }
            }
            return count;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public list<valueType> GetFind(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                int count = 0, index = length;
                valueType[] values = new valueType[length < sizeof(int) ? sizeof(int) : length];
                foreach (valueType value in array)
                {
                    if (isValue(value)) values[count++] = value;
                    if (--index == 0) break;
                }
                return new list<valueType>(values, 0, count, true);
            }
            return null;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配值集合</returns>
        protected override valueType[] getFindArray(func<valueType, bool> isValue, fixedMap map)
        {
            int count = 0, index = 0;
            foreach (valueType value in array)
            {
                if (isValue(value))
                {
                    ++count;
                    map.Set(index);
                }
                if (++index == length) break;
            }
            if (count != 0)
            {
                valueType[] values = new valueType[count];
                for (index = length; count != 0; values[--count] = array[index])
                {
                    while (!map.Get(--index)) ;
                }
                return values;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        public void Sort(func<valueType, valueType, int> comparer)
        {
            algorithm.quickSort.Sort(array, comparer, 0, length);
        }
        /// <summary>
        /// 获取数据范围
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>目标数据</returns>
        protected override arrayType[] getRange<arrayType>(int index, int count, func<valueType, arrayType> getValue)
        {
            arrayType[] values = new arrayType[count];
            for (int length = 0, endIndex = index + count; index != endIndex; ++index) values[length++] = getValue(array[index]);
            return values;
        }
        /// <summary>
        /// 获取数据排序范围(不清除数组)
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序数据</returns>
        public collection<valueType> RangeSort(func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            array.range range = new array.range(length, skipCount, getCount);
            return algorithm.quickSort.RangeSort(array, 0, length, comparer, range.SkipCount, range.GetCount);
        }
        /// <summary>
        /// 获取排序数据分页(不清除数组)
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>排序数据</returns>
        public collection<valueType> PageSort(func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            array.page page = new array.page(length, pageSize, currentPage);
            return algorithm.quickSort.RangeSort(array, 0, length, comparer, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public override bool Max(func<valueType, valueType, int> comparer, out valueType value)
        {
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                value = array[0];
                for (int index = 1; index != length; ++index)
                {
                    valueType nextValue = array[index];
                    if (comparer(nextValue, value) > 0) value = nextValue;
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public override bool Max<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                value = array[0];
                if (length != 1)
                {
                    keyType key = getKey(value);
                    for (int index = 1; index != length; ++index)
                    {
                        valueType nextValue = array[index];
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) > 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public override bool Min(func<valueType, valueType, int> comparer, out valueType value)
        {
            if (comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                value = array[0];
                for (int index = 1; index != length; ++index)
                {
                    valueType nextValue = array[index];
                    if (comparer(nextValue, value) < 0) value = nextValue;
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public override bool Min<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                value = array[0];
                if (length != 1)
                {
                    keyType key = getKey(value);
                    for (int index = 1; index != length; ++index)
                    {
                        valueType nextValue = array[index];
                        keyType nextKey = getKey(nextValue);
                        if (comparer(nextKey, key) < 0)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组数据</returns>
        public Dictionary<keyType, list<valueType>> Group<keyType>(func<valueType, keyType> getKey)
        {
            if (getKey == null) log.Default.Throw(log.exceptionType.Null);
            if (length != 0)
            {
                Dictionary<keyType, list<valueType>> newValues = new Dictionary<keyType, list<valueType>>(length);
                list<valueType> list;
                int count = length;
                foreach (valueType value in array)
                {
                    keyType key = getKey(value);
                    if (!newValues.TryGetValue(key, out list)) newValues[key] = list = new list<valueType>();
                    list.Add(value);
                    if (--count == 0) break;
                }
                return newValues;
            }
            return new Dictionary<keyType, list<valueType>>();
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标集合</returns>
        public list<arrayType> Distinct<arrayType>(func<valueType, arrayType> getValue)
        {
            if (length != 0)
            {
                list<arrayType>.unsafer newValues = new list<arrayType>(length).Unsafer;
                HashSet<valueType> hash = new HashSet<valueType>();
                int count = length;
                foreach (valueType value in array)
                {
                    if (!hash.Contains(value))
                    {
                        newValues.Add(getValue(value));
                        hash.Add(value);
                    }
                    if (--count == 0) break;
                }
                return newValues.List;
            }
            return null;
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public override arrayType[] GetArray<arrayType>(func<valueType, arrayType> getValue)
        {
            if (length != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                arrayType[] values = new arrayType[length];
                int index = 0;
                foreach (valueType value in array)
                {
                    values[index] = getValue(value);
                    if (++index == length) break;
                }
                return values;
            }
            return nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 转换键值对数组
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <param name="getKey">键值获取器</param>
        /// <returns>键值对数组</returns>
        public override keyValue<keyType, valueType>[] GetKeyValueArray<keyType>(func<valueType, keyType> getKey)
        {
            if (length != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                keyValue<keyType, valueType>[] values = new keyValue<keyType, valueType>[length];
                int index = 0;
                foreach (valueType value in array)
                {
                    values[index].Set(getKey(value), value);
                    if (++index == length) break;
                }
                return values;
            }
            return nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 转换双向动态数组
        /// </summary>
        /// <returns>双向动态数组</returns>
        public collection<valueType> ToCollection()
        {
            return new collection<valueType>(this);
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        private valueType[] getArray()
        {
            valueType[] values = new valueType[length];
            Array.Copy(array, 0, values, 0, length);
            return values;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        public valueType[] GetArray()
        {
            return length != 0 ? getArray() : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        public valueType[] ToArray()
        {
            if (length != 0)
            {
                return length != array.Length ? getArray() : array;
            }
            return nullValue<valueType>.Array;
        }
    }
    /// <summary>
    /// 单向动态数组扩展
    /// </summary>
    public static partial class list
    {
        /// <summary>
        /// 长度设为0
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        public static list<valueType> empty<valueType>(this list<valueType> list)
        {
            if (list != null) list.Empty();
            return list;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        public static list<valueType> clear<valueType>(this list<valueType> list)
        {
            if (list != null) list.Clear();
            return list;
        }
        /// <summary>
        /// 获取第一个值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>第一个值,失败为default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this list<valueType> list)
        {
            return list.count() != 0 ? list.Unsafer[0] : default(valueType);
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        public static valueType lastOrDefault<valueType>(this list<valueType> list)
        {
            return list.count() != 0 ? list.Unsafer[list.Count - 1] : default(valueType);
        }
        /// <summary>
        /// 根据位置设置数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">数据位置</param>
        /// <param name="value">数据</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> Set<valueType>(this list<valueType> list, int index, valueType value)
        {
            if (list == null) list = new list<valueType>(index + 1);
            list.Set(index, value);
            return list;
        }
        /// <summary>
        /// 增加数据长度
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="count">数据长度</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> addLength<valueType>(this list<valueType> list, int count)
        {
            if (list != null)
            {
                list.AddLength(count);
                return list;
            }
            return new list<valueType>(count);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">数据</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, valueType value)
        {
            if (list == null) list = new list<valueType>();
            list.Add(value);
            return list;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, IEnumerable<valueType> values)
        {
            if (list != null)
            {
                list.Add(values.getList());
                return list;
            }
            return values.getList();
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, ICollection<valueType> values)
        {
            if (list != null)
            {
                list.Add(values);
                return list;
            }
            return values.getList();
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, valueType[] values)
        {
            if (list != null)
            {
                list.Add(values);
                return list;
            }
            return new list<valueType>(values);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, valueType[] values, int index, int count)
        {
            if (list != null)
            {
                list.Add(values, index, count);
                return list;
            }
            return new list<valueType>(values, index, count, false);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, list<valueType> value)
        {
            if (list != null)
            {
                list.Add(value);
                return list;
            }
            return new list<valueType>(value);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> add<valueType>(this list<valueType> list, list<valueType> value, int index, int count)
        {
            if (list != null)
            {
                list.Add(value, index, count);
                return list;
            }
            return value.count() != 0 ? new list<valueType>(value.Unsafer.Array, index, count, false) : null;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, valueType value)
        {
            if (list != null)
            {
                list.Insert(index, value);
                return list;
            }
            if (index == 0) return list.add(value);
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, IEnumerable<valueType> values)
        {
            if (list != null)
            {
                list.Insert(index, values);
                return list;
            }
            if (index == 0) return values.getList();
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, ICollection<valueType> values)
        {
            if (list != null)
            {
                list.Insert(index, values);
                return list;
            }
            if (index == 0) return values.getList();
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, valueType[] values)
        {
            return list.insert(index, values, 0, values.Length);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">插入数量</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, valueType[] values, int startIndex, int count)
        {
            if (list != null)
            {
                list.Insert(index, values, startIndex, count);
                return list;
            }
            if (index == 0) return new list<valueType>(values, startIndex, count, false);
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据集合</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">插入数量</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> insert<valueType>(this list<valueType> list, int index, list<valueType> value, int startIndex, int count)
        {
            return value.count() != 0 ? list.insert(index, value.Unsafer.Array, startIndex, count) : list;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        /// <returns>目标数组</returns>
        public static valueType[] copyTo<valueType>(this list<valueType> list, valueType[] values, int index)
        {
            if (list != null) list.CopyTo(values, index);
            return values;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        /// <returns>单向动态数组</returns>
        public static list<valueType> removeAt<valueType>(this list<valueType> list, int index)
        {
            if (list != null)
            {
                list.RemoveAt(index);
                return list;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public static bool removeFirst<valueType>(this list<valueType> list, valueType value)
        {
            return list != null ? list.Remove(value) : false;
        }
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">新数据</param>
        /// <param name="isValue">数据匹配器</param>
        public static list<valueType> replaceFirst<valueType>(this list<valueType> list, valueType value, func<valueType, bool> isValue)
        {
            if (list != null) list.ReplaceFirst(value, isValue);
            return list;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this list<valueType> list, valueType value)
        {
            return list != null ? list.IndexOf(value) : -1;
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null ? list.IndexOf(isValue) : -1;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null ? list.FirstOrDefault(isValue) : default(valueType);
        }
        /// <summary>
        /// 判断是否存在匹配
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配</returns>
        public static bool any<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null && list.Any(isValue);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public static bool contains<valueType>(this list<valueType> list, valueType value)
        {
            return list.indexOf(value) != -1;
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> removeRange<valueType>(this list<valueType> list, int index)
        {
            if (list != null)
            {
                list.RemoveRange(index);
                return list;
            }
            if (index == 0) return null;
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> removeRange<valueType>(this list<valueType> list, int index, int count)
        {
            if (list != null)
            {
                list.RemoveRange(index, count);
                return list;
            }
            if ((index | count) == 0) return null;
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> remove<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            if (list != null) list.Remove(isValue);
            return list;
        }
        /// <summary>
        /// 逆转单向动态数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> reverse<valueType>(this list<valueType> list)
        {
            if (list != null) list.Reverse();
            return list;
        }
        /// <summary>
        /// 转换成前端子段集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public static list<valueType> left<valueType>(this list<valueType> list, int count)
        {
            return list != null ? list.Left(count) : null;
        }
        /// <summary>
        /// 转换成子集合(不清除数组)
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <returns>子集合</returns>
        public static collection<valueType> sub<valueType>(this list<valueType> list, int index)
        {
            return list.sub(index, -1);
        }
        /// <summary>
        /// 转换成子集合(不清除数组)
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public static collection<valueType> sub<valueType>(this list<valueType> list, int index, int count)
        {
            return list != null ? list.Sub(index, count) : null;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        public static valueType[] getSub<valueType>(this list<valueType> list, int index, int count)
        {
            return list != null ? list.GetSub(index, count) : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null ? list.GetCount(isValue) : 0;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public static list<valueType> getFind<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null ? list.GetFind(isValue) : null;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public static valueType[] getFindArray<valueType>(this list<valueType> list, func<valueType, bool> isValue)
        {
            return list != null ? list.GetFindArray(isValue) : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="method">调用函数</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> each<valueType>(this list<valueType> list, action<valueType> method)
        {
            if (list.count() != 0)
            {
                if (method == null) log.Default.Throw(log.exceptionType.Null);
                int count = list.Count;
                foreach (valueType value in list.Unsafer.Array)
                {
                    method(value);
                    if (--count == 0) break;
                }
            }
            return list;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> sort<valueType>(this list<valueType> list, func<valueType, valueType, int> comparer)
        {
            if (list != null) list.Sort(comparer);
            return list;
        }
        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>目标数据</returns>
        public static arrayType[] getPage<valueType, arrayType>
            (this list<valueType> list, int pageSize, int currentPage, func<valueType, arrayType> getValue)
        {
            return list != null ? list.GetPage(pageSize, currentPage, getValue) : nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 获取数据排序范围(不清除数组)
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序数据</returns>
        public static collection<valueType> rangeSort<valueType>
            (this list<valueType> list, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return list != null ? list.RangeSort(comparer, skipCount, getCount) : null;
        }
        /// <summary>
        /// 获取排序数据分页(不清除数组)
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>排序数据</returns>
        public static collection<valueType> pageSort<valueType>
            (this list<valueType> list, func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            return list != null ? list.PageSort(comparer, pageSize, currentPage) : null;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this list<valueType> list, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (list != null) return list.Max(comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (list != null) list.Max(getKey, comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this list<valueType> list, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            if (list != null)
            {
                valueType value;
                if (list.Max((left, right) => left.CompareTo(right), out value)) return value;
            }
            return nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>(this list<valueType> list, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            return list != null ? list.Max(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            return list != null ? list.Max(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>(this list<valueType> list, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            return list != null ? list.MaxKey(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            return list != null ? list.MaxKey(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this list<valueType> list, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (list != null) return list.Min(comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (list != null) list.Min(getKey, comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this list<valueType> list, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            if (list != null)
            {
                valueType value;
                if (list.Min((left, right) => left.CompareTo(right), out value)) return value;
            }
            return nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>(this list<valueType> list, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            return list != null ? list.Min(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            return list != null ? list.Min(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>(this list<valueType> list, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            return list != null ? list.MinKey(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>
            (this list<valueType> list, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            return list != null ? list.MinKey(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组数据</returns>
        public static Dictionary<keyType, list<valueType>> group<valueType, keyType>(this list<valueType> list, func<valueType, keyType> getKey)
        {
            return list != null ? list.Group(getKey) : null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标集合</returns>
        public static list<arrayType> distinct<valueType, arrayType>(this list<valueType> list, func<valueType, arrayType> getValue)
        {
            return list != null ? list.Distinct(getValue) : null;
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public static arrayType[] getArray<valueType, arrayType>(this list<valueType> list, func<valueType, arrayType> getValue)
        {
            return list != null ? list.GetArray(getValue) : nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 转换键值对数组
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>键值对数组</returns>
        public static keyValue<keyType, valueType>[] getKeyValueArray<keyType, valueType>(this list<valueType> list, func<valueType, keyType> getKey)
        {
            return list != null ? list.GetKeyValueArray(getKey) : nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 转换双向动态数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> toCollection<valueType>(this list<valueType> list)
        {
            return list != null ? list.ToCollection() : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this list<valueType> list, func<valueType, string> toString)
        {
            return list != null ? list.JoinString(toString) : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this list<valueType> list, string join, func<valueType, string> toString)
        {
            return list != null ? list.JoinString(join, toString) : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this list<valueType> list, char join, func<valueType, string> toString)
        {
            return list != null ? list.JoinString(join, toString) : null;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>数组</returns>
        public static valueType[] getArray<valueType>(this list<valueType> list)
        {
            return list != null ? list.GetArray() : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="list">单向动态数组</param>
        /// <returns>数组</returns>
        public static valueType[] toArray<valueType>(this list<valueType> list)
        {
            return list != null ? list.ToArray() : nullValue<valueType>.Array;
        }
    }
}
