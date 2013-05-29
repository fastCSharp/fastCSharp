using System;
using System.Collections;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 双向动态数组
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    [fastCSharp.setup.cSharp.serialize(SerializeType = typeof(fastCSharp.setup.cSharp.serialize.ISerializeGeneric[]))]
    public class collection<valueType> : dynamicArray<valueType>, IList<valueType>
    {
        /// <summary>
        /// 非安全访问双向动态数组(请自行确保数据可靠性)
        /// </summary>
        public struct unsafer
        {
            /// <summary>
            /// 单向动态数组
            /// </summary>
            public collection<valueType> Collection;
            /// <summary>
            /// 数据数组
            /// </summary>
            public valueType[] Array
            {
                get { return Collection.array; }
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
                    valueType[] array = Array;
                    index += Collection.StartIndex;
                    return array[index < array.Length ? index : (index - array.Length)];
                }
                set
                {
                    valueType[] array = Array;
                    index += Collection.StartIndex;
                    array[index < array.Length ? index : (index - array.Length)] = value;

                }
            }
            /// <summary>
            /// 获取第一个值
            /// </summary>
            /// <returns>第一个值</returns>
            public valueType FirstOrDefault()
            {
                return Array[Collection.StartIndex];
            }
            /// <summary>
            /// 获取最后一个值
            /// </summary>
            /// <returns>最后一个值</returns>
            public valueType LastOrDefault()
            {
                return Array[(Collection.EndIndex != 0 ? Collection.EndIndex : Array.Length) - 1];
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            public void Add(valueType value)
            {
                Array[Collection.EndIndex] = value;
                if (++Collection.EndIndex == Array.Length) Collection.EndIndex = 0;
                if (Collection.StartIndex == Collection.EndIndex) Collection.IsFull = true;
            }
            /// <summary>
            /// 向前端添加一个数据
            /// </summary>
            /// <param name="value">数据</param>
            public void AddExpand(valueType value)
            {
                if (Collection.StartIndex == 0) Collection.StartIndex = Array.Length;
                Array[--Collection.StartIndex] = value;
                if (Collection.StartIndex == Collection.EndIndex) Collection.IsFull = true;
            }
            /// <summary>
            /// 弹出最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            public valueType Pop()
            {
                if (Collection.EndIndex == 0) Collection.EndIndex = Array.Length;
                valueType value = Array[--Collection.EndIndex];
                Collection.IsFull = false;
                return value;
            }
            /// <summary>
            /// 弹出第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            public valueType PopExpand()
            {
                valueType value = Array[Collection.StartIndex];
                if (++Collection.StartIndex == Array.Length) Collection.StartIndex = 0;
                Collection.IsFull = false;
                return value;
            }
            /// <summary>
            /// 取子集合
            /// </summary>
            /// <param name="index">起始位置</param>
            /// <returns>子集合</returns>
            public collection<valueType> Sub(int index)
            {
                if (index != 0)
                {
                    if ((Collection.StartIndex += index) >= Array.Length) Collection.StartIndex -= Array.Length;
                    Collection.IsFull = false;
                }
                return Collection;
            }
            /// <summary>
            /// 取子集合
            /// </summary>
            /// <param name="index">起始位置</param>
            /// <param name="count">数量,小于0表示所有</param>
            /// <returns>子集合</returns>
            public collection<valueType> Sub(int index, int count)
            {
                valueType[] array = Array;
                if ((Collection.StartIndex += index) >= array.Length) Collection.StartIndex -= array.Length;
                if ((Collection.EndIndex = Collection.StartIndex + count) >= array.Length) Collection.EndIndex -= array.Length;
                if (count != array.Length) Collection.IsFull = false;
                return Collection;
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
                return new unsafer { Collection = this };
            }
        }
        /// <summary>
        /// 起始位置
        /// </summary>
        public int StartIndex { get; protected set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public int EndIndex { get; protected set; }
        /// <summary>
        /// 数据是否全有效
        /// </summary>
        public bool IsFull { get; protected set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get
            {
                return IsFull ? array.Length : ((EndIndex >= StartIndex ? EndIndex : (array.Length + EndIndex)) - StartIndex);
            }
        }
        /// 数据数量
        /// </summary>
        protected override int ValueCount { get { return Count; } }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public valueType this[int index]
        {
            get
            {
                if (index >= 0)
                {
                    index += StartIndex;
                    if (EndIndex > StartIndex ? index < EndIndex : ((IsFull || EndIndex < StartIndex) && (index < array.Length || (index -= array.Length) < EndIndex)))
                    {
                        return array[index];
                    }
                }
                log.Default.Throw(log.exceptionType.IndexOutOfRange);
                return default(valueType);
            }
            set
            {
                if (index >= 0)
                {
                    index += StartIndex;
                    if (EndIndex > StartIndex ? index < EndIndex : ((IsFull || EndIndex < StartIndex) && (index < array.Length || (index -= array.Length) < EndIndex)))
                    {
                        array[index] = value;
                        return;
                    }
                }
                log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
        }
        /// <summary>
        /// 获取第一个值
        /// </summary>
        /// <returns>第一个值,失败为default(valueType)</returns>
        public valueType FirstOrDefault()
        {
            return EndIndex != StartIndex || IsFull ? array[StartIndex] : default(valueType);
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        public valueType LastOrDefault()
        {
            return StartIndex != EndIndex || IsFull ? array[(EndIndex != 0 ? EndIndex : array.Length) - 1] : default(valueType);
        }

        /// <summary>
        /// 双向动态数组
        /// </summary>
        public collection() { }
        /// <summary>
        /// 双向动态数组
        /// </summary>
        /// <param name="count">数组容器尺寸</param>
        public collection(int count)
        {
            if (count > 0) array = new valueType[count];
        }
        /// <summary>
        /// 双向动态数组
        /// </summary>
        /// <param name="values">数据集合</param>
        public collection(ICollection<valueType> values) : this(values == null ? null : values.getArray(), true) { }
        /// <summary>
        /// 双向动态数组
        /// </summary>
        /// <param name="value">单向动态数组</param>
        public collection(list<valueType> value)
        {
            EndIndex = value.count();
            if (EndIndex != 0)
            {
                array = value.Unsafer.Array;
                if (EndIndex == array.Length)
                {
                    EndIndex = 0;
                    IsFull = true;
                }
            }
        }
        /// <summary>
        /// 双向动态数组
        /// </summary>
        /// <param name="values">数据数组</param>
        /// <param name="isUnsafe">true表示使用原数组,false表示需要复制数组</param>
        public collection(valueType[] values, bool isUnsafe = false)
        {
            if (values.length() != 0)
            {
                if (isUnsafe)
                {
                    array = values;
                    IsFull = true;
                }
                else
                {
                    setLength(values.Length);
                    Array.Copy(values, array, EndIndex = values.Length);
                    if (values.Length == array.Length) IsFull = true;
                }
            }
        }
        /// <summary>
        /// 双向动态数组
        /// </summary>
        /// <param name="values">数据数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <param name="isUnsafe">true表示可以使用原数组,false表示需要复制数组</param>
        public collection(valueType[] values, int index, int count, bool isUnsafe = false)
        {
            array.range range = new array.range(values.length(), index, count);
            count = range.GetCount;
            if (isUnsafe)
            {
                if (count != 0)
                {
                    array = values;
                    StartIndex = EndIndex = range.SkipCount;
                    if (count == values.Length) IsFull = true;
                    else
                    {
                        IsFull = false;
                        if ((EndIndex += count) >= values.Length) EndIndex -= values.Length;
                    }
                }
                else if (values.Length != 0) array = values;
            }
            else if (count != 0)
            {
                Array.Copy(values, range.SkipCount, array = getNewArray(count), 0, count);
                IsFull = true;
            }
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value">单向动态数组</param>
        /// <returns>数组数据</returns>
        public static explicit operator valueType[](collection<valueType> value)
        {
            return value != null ? value.ToArray() : null;
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value">数组数据</param>
        /// <returns>单向动态数组</returns>
        public static explicit operator collection<valueType>(valueType[] value)
        {
            return value != null ? new collection<valueType>(value, true) : null;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        private class enumerator : IEnumerator<valueType>
        {
            /// <summary>
            /// 被枚举数组
            /// </summary>
            private valueType[] array;
            /// <summary>
            /// 当前位置
            /// </summary>
            private int currentIndex;
            /// <summary>
            /// 当前结束位置
            /// </summary>
            private int currentEndIndex;
            /// <summary>
            /// 起始位置
            /// </summary>
            private int startIndex;
            /// <summary>
            /// 结束位置
            /// </summary>
            private int endIndex;
            /// <summary>
            /// 枚举器
            /// </summary>
            /// <param name="value">双向动态数组</param>
            public enumerator(collection<valueType> value)
            {
                array = value.array;
                startIndex = value.StartIndex;
                endIndex = value.EndIndex;
                Reset();
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            valueType IEnumerator<valueType>.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            object IEnumerator.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 转到下一个数据元素
            /// </summary>
            /// <returns>是否存在下一个数据元素</returns>
            public bool MoveNext()
            {
                if (++currentIndex == currentEndIndex)
                {
                    if (currentEndIndex == endIndex)
                    {
                        --currentIndex;
                        return false;
                    }
                    else
                    {
                        currentIndex = 0;
                        currentEndIndex = endIndex;
                    }
                }
                return true;
            }
            /// <summary>
            /// 重置枚举器状态
            /// </summary>
            public void Reset()
            {
                currentEndIndex = array.Length;
                currentIndex = startIndex - 1;
            }
            /// <summary>
            /// 释放枚举器
            /// </summary>
            public void Dispose()
            {
                array = null;
            }
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (EndIndex == 0)
            {
                if (StartIndex != 0 || IsFull) return new arrayEnumerator(this);
            }
            else if (EndIndex > StartIndex) return new arrayEnumerator(this);
            else if (IsFull || EndIndex < StartIndex) return new enumerator(this);
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
        /// 设置数据长度
        /// </summary>
        /// <param name="length">数据长度</param>
        private void setLength(int length)
        {
            valueType[] values = getNewArray(length);
            copyTo(values, 0);
            EndIndex = Count;
            StartIndex = 0;
            IsFull = false;
            array = values;
        }
        /// <summary>
        /// 长度设为0
        /// </summary>
        public void Empty()
        {
            IsFull = false;
            StartIndex = EndIndex = 0;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            if (isClearArray) Array.Clear(array, 0, array.Length);
            Empty();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(valueType value)
        {
            if (IsFull) setLength(array.Length << 1);
            else if (array == null) setLength(sizeof(int));
            array[EndIndex] = value;
            if (++EndIndex == array.Length) EndIndex = 0;
            if (StartIndex == EndIndex) IsFull = true;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ICollection<valueType> values)
        {
            Add(values.getArray());
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        private void add(valueType[] values, int index, int count)
        {
            if (EndIndex >= StartIndex)
            {
                int copyCount = array.Length - EndIndex;
                if (copyCount < count)
                {
                    Array.Copy(values, index, array, EndIndex, copyCount);
                    index += copyCount;
                    count -= copyCount;
                    EndIndex = 0;
                }
            }
            Array.Copy(values, index, array, EndIndex, count);
            if ((EndIndex += count) == array.Length) EndIndex = 0;
            if (EndIndex == StartIndex) IsFull = true;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public new void Add(valueType[] values)
        {
            if (values.length() != 0)
            {
                int length = Count + values.Length;
                if (array == null || length > array.Length) setLength(length);
                add(values, 0, values.Length);
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
            if (values != null && values.Length != 0 && count >= 0)
            {
                int endIndex = index + count;
                if (endIndex <= values.Length && index >= 0)
                {
                    int length = this.Count + count;
                    if (array == null || length > array.Length) setLength(length);
                    add(values, index, count);
                }
                else log.Default.Throw(log.exceptionType.IndexOutOfRange);
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="value">数据集合</param>
        public void Add(collection<valueType> value)
        {
            if (value != null) Add(value, 0, value.Count);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        public void Add(collection<valueType> value, int index, int count)
        {
            int endIndex = index + count, valueCount = value != null ? value.Count : 0;
            if (endIndex > valueCount || index < 0 || count < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            if (count != 0)
            {
                int length = this.Count + count;
                if (array == null || length > array.Length) setLength(length);
                valueType[] values = value.array;
                if ((index += value.StartIndex) >= values.Length) add(values, index - values.Length, count);
                else if ((endIndex += value.StartIndex) <= values.Length) add(values, index, count);
                else
                {
                    add(values, index, length = values.Length - index);
                    add(values, 0, count - length);
                }
            }
        }
        /// <summary>
        /// 向前端添加一个数据
        /// </summary>
        /// <param name="value">数据</param>
        public void AddExpand(valueType value)
        {
            if (IsFull) setLength(array.Length << 1);
            else if (array == null) setLength(sizeof(int));
            if (StartIndex == 0) StartIndex = array.Length;
            array[--StartIndex] = value;
            if (StartIndex == EndIndex) IsFull = true;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        public void Insert(int index, valueType value)
        {
            if (index != 0)
            {
                int count = this.Count;
                if (index != count)
                {
                    if (index > 0 && index < count)
                    {
                        int length = count + 1;
                        if (length <= array.Length)
                        {
                            if (index < (count >> 1))
                            {
                                copyLeft(StartIndex, index, 1);
                                if (StartIndex != 0) --StartIndex;
                                else StartIndex = array.Length - 1;
                            }
                            else
                            {
                                copyRight(StartIndex + index, count - index, 1);
                                if (++EndIndex == array.Length) EndIndex = 0;
                            }
                            if ((index += StartIndex) >= array.Length) index -= array.Length;
                            array[index] = value;
                            if (StartIndex == EndIndex) IsFull = true;
                        }
                        else
                        {
                            valueType[] values = getNewArray(length);
                            copyTo(values, 0, index);
                            values[index] = value;
                            if ((StartIndex += index) >= array.Length) StartIndex -= array.Length;
                            IsFull = false;
                            copyTo(values, index + 1, count - index);
                            StartIndex = 0;
                            if (length == values.Length)
                            {
                                IsFull = true;
                                EndIndex = 0;
                            }
                            else EndIndex = length;
                            array = values;
                        }
                    }
                    else log.Default.Throw(log.exceptionType.IndexOutOfRange);
                }
                else Add(value);
            }
            else AddExpand(value);
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据集合</param>
        public void Insert(int index, collection<valueType> value)
        {
            int count = this.Count, valueCount = value.count();
            if (index >= 0 && index <= count)
            {
                if (valueCount != 0)
                {
                    int length = count + valueCount;
                    if (length <= array.Length)
                    {
                        if (index == 0)
                        {
                            count = EndIndex;
                            if ((EndIndex += array.Length - length) > array.Length) EndIndex -= array.Length;
                            value.copyTo(this);
                            if ((StartIndex -= valueCount) < 0) StartIndex += array.Length;
                            if (StartIndex == (EndIndex = count)) IsFull = true;
                        }
                        else if (index == count) value.copyTo(this);
                        else if (index < (count >> 1))
                        {
                            copyLeft(StartIndex, index, valueCount);
                            if ((StartIndex -= valueCount) < 0) StartIndex += array.Length;
                            length = EndIndex;
                            if ((EndIndex -= valueCount + (count - index)) < 0) EndIndex += array.Length;
                            value.copyTo(this);
                            if (StartIndex == (EndIndex = length)) IsFull = true;
                        }
                        else
                        {
                            copyRight(StartIndex + index, count - index, valueCount);
                            length = EndIndex;
                            if ((EndIndex -= count - index) < 0) EndIndex += array.Length;
                            value.copyTo(this);
                            if ((EndIndex = length + valueCount) >= array.Length) EndIndex -= array.Length;
                            if (StartIndex == EndIndex) IsFull = true;
                        }
                    }
                    else
                    {
                        valueType[] values = getNewArray(length);
                        copyTo(values, 0, index);
                        value.copyTo(values, index);
                        if ((StartIndex += index) >= array.Length) StartIndex -= array.Length;
                        IsFull = false;
                        copyTo(values, index + valueCount, count - index);
                        StartIndex = 0;
                        if (length == values.Length)
                        {
                            IsFull = true;
                            EndIndex = 0;
                        }
                        else EndIndex = length;
                        array = values;
                    }
                }
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        public override void RemoveAt(int index)
        {
            GetRemoveAt(index);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public override valueType GetRemoveAt(int index)
        {
            if (index != 0)
            {
                int count = this.Count;
                if (index != count - 1)
                {
                    if (index < count && index > 0)
                    {
                        int valueIndex = index + StartIndex;
                        valueType value = array[valueIndex < array.Length ? valueIndex : valueIndex - array.Length];
                        if (index < (count >> 1))
                        {
                            copyRight(StartIndex, index, 1);
                            if (++StartIndex == array.Length) StartIndex = 0;
                        }
                        else
                        {
                            copyLeft(StartIndex + ++index, count - index, 1);
                            if (EndIndex == 0) EndIndex = array.Length;
                            --EndIndex;
                        }
                        IsFull = false;
                        return value;
                    }
                }
                else return Pop();
            }
            else return PopExpand();
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        public void RemoveRange(int index)
        {
            RemoveRange(index, this.Count - index);
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        public void RemoveRange(int index, int count)
        {
            int endCount = this.Count - index - count;
            if (endCount >= 0 && index >= 0 && count >= 0)
            {
                if (count != 0)
                {
                    if (index != 0)
                    {
                        if (endCount == 0)
                        {
                            if ((EndIndex -= count) < 0) EndIndex += array.Length;
                        }
                        else if (index < endCount)
                        {
                            copyRight(StartIndex, index, count);
                            if ((StartIndex += count) >= array.Length) StartIndex -= array.Length;
                        }
                        else
                        {
                            copyLeft(StartIndex + index + count, endCount, count);
                            if ((EndIndex -= count) < 0) EndIndex += array.Length;
                        }
                    }
                    else if ((StartIndex += count) >= array.Length) StartIndex -= array.Length;
                    IsFull = false;
                }
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns>最后一个数据</returns>
        public valueType Pop()
        {
            if (IsFull || StartIndex != EndIndex)
            {
                if (EndIndex == 0) EndIndex = array.Length;
                valueType value = array[--EndIndex];
                IsFull = false;
                return value;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns>第一个数据</returns>
        public valueType PopExpand()
        {
            if (IsFull || StartIndex != EndIndex)
            {
                valueType value = array[StartIndex];
                if (++StartIndex == array.Length) StartIndex = 0;
                IsFull = false;
                return value;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 元素右移
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移动元素数量</param>
        /// <param name="shiftCount">移动位置数量</param>
        private void copyRight(int index, int count, int shiftCount)
        {
            int endIndex = index + count, copyIndex = endIndex + shiftCount;
            if (index >= array.Length)
            {
                index -= array.Length;
                endIndex -= array.Length;
                copyIndex -= array.Length;
            }
            else
            {
                if (endIndex > array.Length)
                {
                    for (endIndex -= array.Length, copyIndex -= array.Length; --endIndex >= 0; array[--copyIndex] = array[endIndex]) ;
                    if ((endIndex = array.Length) - index > copyIndex)
                    {
                        while (--copyIndex >= 0) array[copyIndex] = array[--endIndex];
                        copyIndex = array.Length;
                    }
                }
                else if (copyIndex > array.Length)
                {
                    if (count > (copyIndex -= array.Length))
                    {
                        while (--copyIndex >= 0) array[copyIndex] = array[--endIndex];
                        copyIndex = array.Length;
                    }
                }
            }
            while (--endIndex >= index) array[--copyIndex] = array[endIndex];
        }
        /// <summary>
        /// 元素左移
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移动元素数量</param>
        /// <param name="shiftCount">移动位置数量</param>
        private void copyLeft(int index, int count, int shiftCount)
        {
            int endIndex = index + count, copyIndex = index - shiftCount;
            if (index >= array.Length)
            {
                index -= array.Length;
                endIndex -= array.Length;
                if (copyIndex < array.Length)
                {
                    if ((array.Length - copyIndex) < count)
                    {
                        while (copyIndex != array.Length) array[copyIndex++] = array[index++];
                        copyIndex = 0;
                    }
                }
                else copyIndex -= array.Length;
            }
            else if (endIndex > array.Length)
            {
                while (index != array.Length) array[copyIndex++] = array[index++];
                index = 0;
                endIndex -= array.Length;
            }
            else if (copyIndex < 0)
            {
                if (-copyIndex >= count) copyIndex += array.Length;
                else
                {
                    for (copyIndex += array.Length; copyIndex != array.Length; array[copyIndex++] = array[index++]) ;
                    copyIndex = 0;
                }
            }
            while (index != endIndex) array[copyIndex++] = array[index++];
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        private void copyTo(valueType[] values, int index, int count)
        {
            int endIndex = StartIndex + count;
            if (endIndex <= array.Length) Array.Copy(array, StartIndex, values, index, count);
            else
            {
                Array.Copy(array, StartIndex, values, index, count = array.Length - StartIndex);
                Array.Copy(array, 0, values, index + count, endIndex - array.Length);
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数组</param>
        /// <param name="index">起始位置</param>
        private void copyTo(valueType[] values, int index)
        {
            if (EndIndex > StartIndex) Array.Copy(array, StartIndex, values, index, EndIndex - StartIndex);
            else if (IsFull || EndIndex < StartIndex)
            {
                int count = array.Length - StartIndex;
                Array.Copy(array, StartIndex, values, index, count);
                Array.Copy(array, 0, values, index + count, EndIndex);
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="value">目标列表</param>
        private void copyTo(collection<valueType> value)
        {
            if (EndIndex > StartIndex) value.add(array, StartIndex, EndIndex - StartIndex);
            else if (IsFull || EndIndex < StartIndex)
            {
                value.add(array, StartIndex, array.Length - StartIndex);
                value.add(array, 0, EndIndex);
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="values">目标数组</param>
        /// <param name="count">数量</param>
        private void copyTo(int index, valueType[] values, int count)
        {
            if (EndIndex > StartIndex) Array.Copy(array, StartIndex + index, values, 0, count);
            else if ((index += StartIndex) >= array.Length) Array.Copy(array, index - array.Length, values, 0, count);
            else
            {
                int endCount = array.Length - index;
                if (endCount >= count) Array.Copy(array, index, values, 0, count);
                else
                {
                    Array.Copy(array, index, values, 0, endCount);
                    Array.Copy(array, 0, values, endCount, count - endCount);
                }
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        public void CopyTo(valueType[] values, int index)
        {
            int length = Count;
            if (values != null && index >= 0 && length + index <= values.Length)
            {
                if (length != 0) copyTo(values, index);
            }
            else log.Default.Throw(log.exceptionType.IndexOutOfRange);
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        public valueType[] GetArray()
        {
            valueType[] values = new valueType[Count];
            copyTo(values, 0);
            return values;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        public valueType[] ToArray()
        {
            return IsFull && StartIndex == 0 ? array : GetArray();
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public override arrayType[] GetArray<arrayType>(func<valueType, arrayType> getValue)
        {
            int length = Count;
            if (length != 0)
            {
                if (getValue == null) log.Default.Throw(log.exceptionType.Null);
                arrayType[] values = new arrayType[length];
                length = 0;
                if (EndIndex > StartIndex)
                {
                    for (int index = StartIndex; index != EndIndex; ++index) values[length++] = getValue(array[index]);
                }
                else
                {
                    for (int index = StartIndex; index != array.Length; ++index) values[length++] = getValue(array[index]);
                    for (int index = 0; index != EndIndex; ++index) values[length++] = getValue(array[index]);
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
            int length = Count;
            if (length != 0)
            {
                if (getKey == null) log.Default.Throw(log.exceptionType.Null);
                keyValue<keyType, valueType>[] values = new keyValue<keyType, valueType>[length];
                length = 0;
                if (EndIndex > StartIndex)
                {
                    for (int index = StartIndex; index != EndIndex; ++index)
                    {
                        valueType value = array[index];
                        values[length++].Set(getKey(value), value);
                    }
                }
                else
                {
                    for (int index = StartIndex; index != array.Length; ++index)
                    {
                        valueType value = array[index];
                        values[length++].Set(getKey(value), value);
                    }
                    for (int index = 0; index != EndIndex; ++index)
                    {
                        valueType value = array[index];
                        values[length++].Set(getKey(value), value);
                    }
                }
                return values;
            }
            return nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 转换单向列表
        /// </summary>
        /// <returns>单向列表</returns>
        public list<valueType> ToList()
        {
            return StartIndex == 0 ? new list<valueType>(array, 0, Count, true) : new list<valueType>(this);
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        public void Reverse()
        {
            if (EndIndex > StartIndex)
            {
                for (int leftIndex = StartIndex, endIndex = (StartIndex + EndIndex) >> 1, rightIndex = EndIndex;
                    leftIndex != endIndex;
                    ++leftIndex)
                {
                    valueType value = array[leftIndex];
                    array[leftIndex] = array[--rightIndex];
                    array[rightIndex] = value;
                }
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                int leftCount = array.Length - StartIndex, leftIndex = StartIndex, rightIndex = EndIndex, endIndex;
                if (leftCount > EndIndex)
                {
                    for (; --rightIndex >= 0; ++leftIndex)
                    {
                        valueType value = array[leftIndex];
                        array[leftIndex] = array[rightIndex];
                        array[rightIndex] = value;
                    }
                    endIndex = (leftIndex + array.Length) >> 1;
                    rightIndex = array.Length;
                }
                else
                {
                    while (leftIndex != array.Length)
                    {
                        valueType value = array[leftIndex];
                        array[leftIndex++] = array[--rightIndex];
                        array[rightIndex] = value;
                    }
                    leftIndex = 0;
                    endIndex = rightIndex >> 1;
                }
                while (leftIndex != endIndex)
                {
                    valueType value = array[leftIndex];
                    array[leftIndex++] = array[--rightIndex];
                    array[rightIndex] = value;
                }
            }
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public collection<valueType> Sub(int index, int count = -1)
        {
            array.range range = new array.range(Count, index, count);
            if ((count = range.GetCount) > 0)
            {
                if ((StartIndex += range.SkipCount) >= array.Length) StartIndex -= array.Length;
                if ((EndIndex = StartIndex + count) >= array.Length) EndIndex -= array.Length;
                if (count != array.Length) IsFull = false;
            }
            else
            {
                StartIndex = EndIndex = 0;
                IsFull = false;
            }
            return this;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public valueType[] GetSub(int index, int count)
        {
            array.range range = new array.range(Count, index, count);
            if ((count = range.GetCount) > 0)
            {
                valueType[] values = new valueType[count];
                copyTo(index, values, count);
                return values;
            }
            return nullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public override int IndexOf(valueType value)
        {
            int index = -1;
            if (EndIndex > StartIndex) index = Array.IndexOf<valueType>(array, value, StartIndex, EndIndex - StartIndex);
            else if (IsFull || EndIndex < StartIndex)
            {
                index = Array.IndexOf<valueType>(array, value, StartIndex, array.Length - StartIndex);
                if (index < 0)
                {
                    index = Array.IndexOf<valueType>(array, value, 0, EndIndex);
                    if (index >= 0) index += array.Length;
                }
            }
            return index - StartIndex;
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        protected override int indexOf(func<valueType, bool> isValue)
        {
            if (EndIndex > StartIndex)
            {
                for (int index = StartIndex; index != EndIndex; ++index)
                {
                    if (isValue(array[index])) return index;
                }
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                for (int index = StartIndex; index != array.Length; ++index)
                {
                    if (isValue(array[index])) return index;
                }
                for (int index = 0; index != EndIndex; ++index)
                {
                    if (isValue(array[index])) return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public override int IndexOf(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int index = indexOf(isValue);
            if (index != -1)
            {
                if (index < StartIndex) index += array.Length;
                return index - StartIndex;
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public list<valueType> GetFind(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int length = Count;
            if (length > 0)
            {
                valueType[] values = new valueType[length < sizeof(int) ? sizeof(int) : length];
                length = 0;
                if (EndIndex > StartIndex)
                {
                    for (int index = StartIndex; index != EndIndex; ++index)
                    {
                        if (isValue(array[index])) values[length++] = array[index];
                    }
                }
                else
                {
                    for (int index = StartIndex; index != array.Length; ++index)
                    {
                        if (isValue(array[index])) values[length++] = array[index];
                    }
                    for (int index = 0; index != EndIndex; ++index)
                    {
                        if (isValue(array[index])) values[length++] = array[index];
                    }
                }
                return new list<valueType>(values, 0, length, true);
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
            int length = 0;
            if (EndIndex > StartIndex)
            {
                for (int index = StartIndex; index != EndIndex; ++index)
                {
                    if (isValue(array[index]))
                    {
                        ++length;
                        map.Set(index);
                    }
                }
            }
            else
            {
                for (int index = StartIndex; index != array.Length; ++index)
                {
                    if (isValue(array[index]))
                    {
                        ++length;
                        map.Set(index);
                    }
                }
                for (int index = 0; index != EndIndex; ++index)
                {
                    if (isValue(array[index]))
                    {
                        ++length;
                        map.Set(index);
                    }
                }
            }
            if (length != 0)
            {
                valueType[] values = new valueType[length];
                length = 0;
                if (EndIndex > StartIndex)
                {
                    for (int index = StartIndex; index != EndIndex; ++index)
                    {
                        if (map.Get(index)) values[length++] = array[index];
                    }
                }
                else
                {
                    for (int index = StartIndex; index != array.Length; ++index)
                    {
                        if (map.Get(index)) values[length++] = array[index];
                    }
                    for (int index = 0; index != EndIndex; ++index)
                    {
                        if (map.Get(index)) values[length++] = array[index];
                    }
                }
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
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            int value = 0;
            if (EndIndex > StartIndex)
            {
                for (int index = StartIndex; index != EndIndex; ++index)
                {
                    if (isValue(array[index])) ++value;
                }
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                for (int index = StartIndex; index != array.Length; ++index)
                {
                    if (isValue(array[index])) ++value;
                }
                for (int index = 0; index != EndIndex; ++index)
                {
                    if (isValue(array[index])) ++value;
                }
            }
            return value;
        }
        /// <summary>
        /// 移除所有前端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeStart(func<valueType, bool> isValue)
        {
            if (EndIndex > StartIndex)
            {
                while (StartIndex != EndIndex && isValue(array[StartIndex])) StartIndex++;
                if (StartIndex == EndIndex) StartIndex = EndIndex = 0;
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                while (StartIndex != array.Length && isValue(array[StartIndex])) StartIndex++;
                if (StartIndex == array.Length)
                {
                    for (IsFull = false, StartIndex = 0; StartIndex != EndIndex && isValue(array[StartIndex]); StartIndex++) ;
                    if (StartIndex == EndIndex) StartIndex = EndIndex = 0;
                }
                else if (StartIndex != EndIndex) IsFull = false;
            }
        }
        /// <summary>
        /// 移除所有后端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeEnd(func<valueType, bool> isValue)
        {
            if (EndIndex > StartIndex)
            {
                while (StartIndex != EndIndex && isValue(array[EndIndex - 1])) --EndIndex;
                if (StartIndex == EndIndex) StartIndex = EndIndex = 0;
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                int endIndex = EndIndex;
                while (endIndex != 0 && isValue(array[--endIndex])) --EndIndex;
                if (EndIndex == 0)
                {
                    for (endIndex = array.Length; StartIndex != endIndex && isValue(array[--endIndex]); --EndIndex) ;
                    if (EndIndex != 0)
                    {
                        IsFull = false;
                        if (StartIndex == (EndIndex += array.Length)) StartIndex = EndIndex = 0;
                    }
                }
                if (StartIndex != EndIndex) IsFull = false;
            }
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        public void Remove(func<valueType, bool> isValue)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            removeStart(isValue);
            removeEnd(isValue);
            int index = indexOf(isValue);
            if (index != -1)
            {
                int count = 1, writeIndex = index;
                if (EndIndex > StartIndex || index < StartIndex)
                {
                    while (++index != EndIndex)
                    {
                        if (isValue(array[index])) ++count;
                        else array[writeIndex++] = array[index];
                    }
                }
                else if (index >= StartIndex)
                {
                    while (++index != array.Length)
                    {
                        if (isValue(array[index])) ++count;
                        else array[writeIndex++] = array[index];
                    }
                    for (index = 0; index != EndIndex; ++index)
                    {
                        if (isValue(array[index])) ++count;
                        else
                        {
                            array[writeIndex++] = array[index];
                            if (writeIndex == array.Length) writeIndex = 0;
                        }
                    }
                }
                if ((EndIndex -= count) < 0) EndIndex += array.Length;
                IsFull = false;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        public void Sort(func<valueType, valueType, int> comparer)
        {
            if (StartIndex != EndIndex)
            {
                if (EndIndex < StartIndex) setLength(array.Length);
                array.sort(comparer, StartIndex, EndIndex - StartIndex);
            }
            else if (IsFull)
            {
                if (StartIndex != 0) array = GetArray();
                StartIndex = EndIndex = 0;
                array.sort(comparer);
            }
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
            int length = 0;
            if (EndIndex > StartIndex)
            {
                for (int endIndex = (index += StartIndex) + count; index != endIndex; ++index) values[length++] = getValue(array[index]);
            }
            else if ((index += StartIndex) >= array.Length)
            {
                for (int endIndex = (index -= array.Length) + count; index != endIndex; ++index) values[length++] = getValue(array[index]);
            }
            else
            {
                int endCount = array.Length - index;
                if (endCount >= count)
                {
                    for (int endIndex = index + count; index != endIndex; ++index) values[length++] = getValue(array[index]);
                }
                else
                {
                    while (index != array.Length) values[length++] = getValue(array[index++]);
                    for (index = 0, count -= endCount; index != count; ++index) values[length++] = getValue(array[index]);
                }
            }
            return values;
        }
        /// <summary>
        /// 获取数据排序范围
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序数据</returns>
        public collection<valueType> RangeSort(func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            if (StartIndex != EndIndex)
            {
                if (EndIndex < StartIndex) setLength(array.Length);
                return array.rangeSort(StartIndex, EndIndex - StartIndex, comparer, skipCount, getCount);
            }
            else if (IsFull)
            {
                if (StartIndex != 0) array = GetArray();
                StartIndex = EndIndex = 0;
                return array.rangeSort(comparer, skipCount, getCount);
            }
            return null;
        }
        /// <summary>
        /// 获取排序数据分页
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>排序数据</returns>
        public collection<valueType> PageSort(func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            if (StartIndex != EndIndex)
            {
                array.page page = new array.page(Count, pageSize, currentPage);
                if (EndIndex < StartIndex) setLength(array.Length);
                return array.rangeSort(StartIndex, EndIndex - StartIndex, comparer, page.SkipCount, page.CurrentPageSize);
            }
            else if (IsFull)
            {
                array.page page = new array.page(array.Length, pageSize, currentPage);
                if (StartIndex != 0) array = GetArray();
                StartIndex = EndIndex = 0;
                return array.rangeSort(comparer, page.SkipCount, page.CurrentPageSize);
            }
            return null;
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
            if (Count != 0)
            {
                value = array[StartIndex];
                if (Count != 1)
                {
                    if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                    if (EndIndex > StartIndex)
                    {
                        for (int index = StartIndex; ++index != EndIndex; )
                        {
                            if (comparer(array[index], value) > 0) value = array[index];
                        }
                    }
                    else
                    {
                        for (int index = StartIndex; ++index != array.Length; )
                        {
                            if (comparer(array[index], value) > 0) value = array[index];
                        }
                        for (int index = 0; index != EndIndex; ++index)
                        {
                            if (comparer(array[index], value) > 0) value = array[index];
                        }
                    }
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
            if (Count != 0)
            {
                value = array[StartIndex];
                if (Count != 1)
                {
                    if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
                    keyType key = getKey(value);
                    if (EndIndex > StartIndex)
                    {
                        for (int index = StartIndex; ++index != EndIndex; )
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) > 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
                        }
                    }
                    else
                    {
                        for (int index = StartIndex; ++index != array.Length; )
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) > 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
                        }
                        for (int index = 0; index != EndIndex; ++index)
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) > 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
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
            if (Count != 0)
            {
                value = array[StartIndex];
                if (Count != 1)
                {
                    if (comparer == null) log.Default.Throw(log.exceptionType.Null);
                    if (EndIndex > StartIndex)
                    {
                        for (int index = StartIndex; ++index != EndIndex; )
                        {
                            if (comparer(array[index], value) < 0) value = array[index];
                        }
                    }
                    else
                    {
                        for (int index = StartIndex; ++index != array.Length; )
                        {
                            if (comparer(array[index], value) < 0) value = array[index];
                        }
                        for (int index = 0; index != EndIndex; ++index)
                        {
                            if (comparer(array[index], value) < 0) value = array[index];
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
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public override bool Min<keyType>
            (func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (Count != 0)
            {
                value = array[StartIndex];
                if (Count != 1)
                {
                    if (getKey == null || comparer == null) log.Default.Throw(log.exceptionType.Null);
                    keyType key = getKey(value);
                    if (EndIndex > StartIndex)
                    {
                        for (int index = StartIndex; ++index != EndIndex; )
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) < 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
                        }
                    }
                    else
                    {
                        for (int index = StartIndex; ++index != array.Length; )
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) < 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
                        }
                        for (int index = 0; index != EndIndex; ++index)
                        {
                            keyType nextKey = getKey(array[index]);
                            if (comparer(nextKey, key) < 0)
                            {
                                value = array[index];
                                key = nextKey;
                            }
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
            if (Count != 0)
            {
                Dictionary<keyType, list<valueType>> newValues = new Dictionary<keyType, list<valueType>>(Count);
                list<valueType> value;
                if (EndIndex > StartIndex)
                {
                    for (int index = StartIndex; index != EndIndex; ++index)
                    {
                        keyType key = getKey(array[index]);
                        if (!newValues.TryGetValue(key, out value)) newValues[key] = value = new list<valueType>();
                        value.Add(array[index]);
                    }
                }
                else if (IsFull || EndIndex < StartIndex)
                {
                    for (int index = StartIndex; index != array.Length; ++index)
                    {
                        keyType key = getKey(array[index]);
                        if (!newValues.TryGetValue(key, out value)) newValues[key] = value = new list<valueType>();
                        value.Add(array[index]);
                    }
                    for (int index = 0; index != EndIndex; ++index)
                    {
                        keyType key = getKey(array[index]);
                        if (!newValues.TryGetValue(key, out value)) newValues[key] = value = new list<valueType>();
                        value.Add(array[index]);
                    }
                }
                return newValues;
            }
            return new Dictionary<keyType, list<valueType>>();
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="getValue">目标数据获取器</param>
        /// <returns>目标集合</returns>
        public list<arrayType> Distinct<arrayType>(func<valueType, arrayType> getValue)
        {
            valueType value;
            list<arrayType>.unsafer newValues = new list<arrayType>(Count).Unsafer;
            if (EndIndex > StartIndex)
            {
                HashSet<valueType> hash = new HashSet<valueType>();
                for (int index = StartIndex; index != EndIndex; )
                {
                    if (!hash.Contains(value = array[index++]))
                    {
                        newValues.Add(getValue(value));
                        hash.Add(value);
                    }
                }
            }
            else if (IsFull || EndIndex < StartIndex)
            {
                HashSet<valueType> hash = new HashSet<valueType>();
                for (int index = StartIndex; index != array.Length; )
                {
                    if (!hash.Contains(value = array[index++]))
                    {
                        newValues.Add(getValue(value));
                        hash.Add(value);
                    }
                }
                for (int index = 0; index != EndIndex; )
                {
                    if (!hash.Contains(value = array[index++]))
                    {
                        newValues.Add(getValue(value));
                        hash.Add(value);
                    }
                }
            }
            return newValues.List;
        }
    }
    /// <summary>
    /// 双向动态数组扩展
    /// </summary>
    public static class collection
    {
        /// <summary>
        /// 长度设为0
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> empty<valueType>(this collection<valueType> collection)
        {
            if (collection != null) collection.Empty();
            return collection;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> clear<valueType>(this collection<valueType> collection)
        {
            if (collection != null) collection.Clear();
            return collection;
        }
        /// <summary>
        /// 获取第一个值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>第一个值,失败为default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this collection<valueType> collection)
        {
            return collection != null ? collection.FirstOrDefault() : default(valueType);
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        public static valueType lastOrDefault<valueType>(this collection<valueType> collection)
        {
            return collection != null ? collection.LastOrDefault() : default(valueType);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, valueType value)
        {
            if (collection == null) collection = new collection<valueType>();
            collection.Add(value);
            return collection;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, IEnumerable<valueType> values)
        {
            if (collection != null)
            {
                collection.Add(values.getList());
                return collection;
            }
            return values.getCollection();
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, ICollection<valueType> values)
        {
            if (collection != null)
            {
                collection.Add(values.getList());
                return collection;
            }
            return values.getCollection();
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, valueType[] values)
        {
            if (collection != null)
            {
                collection.Add(values);
                return collection;
            }
            return new collection<valueType>(values, false);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="values">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, valueType[] values, int index, int count)
        {
            if (collection != null)
            {
                collection.Add(values, index, count);
                return collection;
            }
            return new collection<valueType>(values, index, count, false);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, collection<valueType> value)
        {
            if (collection != null)
            {
                collection.Add(value);
                return collection;
            }
            return new collection<valueType>(value);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>
            (this collection<valueType> collection, collection<valueType> value, int index, int count)
        {
            if (collection != null)
            {
                collection.Add(value, index, count);
                return collection;
            }
            return new collection<valueType>(value.getArray(), index, count, true);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, list<valueType> value)
        {
            if (collection != null)
            {
                collection.Add(value);
                return collection;
            }
            return new collection<valueType>(value.getArray(), true);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据集合</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> add<valueType>(this collection<valueType> collection, list<valueType> value, int index, int count)
        {
            if (collection != null)
            {
                collection.Add(value, index, count);
                return collection;
            }
            return new collection<valueType>(value.getArray(), index, count, true);
        }
        /// <summary>
        /// 向前端添加一个数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> addExpand<valueType>(this collection<valueType> collection, valueType value)
        {
            if (collection == null) collection = new collection<valueType>();
            collection.AddExpand(value);
            return collection;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> insert<valueType>(this collection<valueType> collection, int index, valueType value)
        {
            if (collection != null)
            {
                collection.Insert(index, value);
                return collection;
            }
            if (index == 0) return collection.add(value);
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 插入数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">插入位置</param>
        /// <param name="values">数据集合</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> insert<valueType>(this collection<valueType> collection, int index, collection<valueType> values)
        {
            if (collection != null)
            {
                collection.Insert(index, values);
                return collection;
            }
            if (index == 0) return new collection<valueType>(values);
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">数据</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> removeFirst<valueType>(this collection<valueType> collection, valueType value)
        {
            if (collection != null) collection.Remove(value);
            return collection;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public static valueType removeAt<valueType>(this collection<valueType> collection, int index)
        {
            if (collection != null) return collection.GetRemoveAt(index);
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> removeRange<valueType>(this collection<valueType> collection, int index)
        {
            if (collection != null)
            {
                collection.RemoveRange(index);
                return collection;
            }
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> removeRange<valueType>(this collection<valueType> collection, int index, int count)
        {
            if (collection != null)
            {
                collection.RemoveRange(index, count);
                return collection;
            }
            if (count != 0) return null;
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return null;
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>最后一个数据</returns>
        public static valueType pop<valueType>(this collection<valueType> collection)
        {
            if (collection != null) return collection.Pop();
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>第一个数据</returns>
        public static valueType popExpand<valueType>(this collection<valueType> collection)
        {
            if (collection != null) return collection.PopExpand();
            log.Default.Throw(log.exceptionType.IndexOutOfRange);
            return default(valueType);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        /// <returns>目标数组</returns>
        public static valueType[] copyTo<valueType>(this collection<valueType> collection, valueType[] values, int index)
        {
            if (collection != null) collection.CopyTo(values, index);
            return values;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>数组</returns>
        public static valueType[] getArray<valueType>(this collection<valueType> collection)
        {
            return collection != null ? collection.GetArray() : nullValue<valueType>.Array;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>数组</returns>
        public static valueType[] toArray<valueType>(this collection<valueType> collection)
        {
            return collection != null ? collection.ToArray() : null;
        }
        /// <summary>
        /// 转换数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>数据集合</returns>
        public static arrayType[] getArray<valueType, arrayType>(this collection<valueType> collection, func<valueType, arrayType> getValue)
        {
            return collection != null ? collection.GetArray(getValue) : nullValue<arrayType>.Array;
        }
        /// <summary>
        /// 转换键值对数组
        /// </summary>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>键值对数组</returns>
        public static keyValue<keyType, valueType>[] getKeyValueArray<keyType, valueType>(this collection<valueType> collection, func<valueType, keyType> getKey)
        {
            return collection != null ? collection.GetKeyValueArray(getKey) : nullValue<keyValue<keyType, valueType>>.Array;
        }
        /// <summary>
        /// 转换单向动态数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <returns>单向动态数组</returns>
        public static list<valueType> toList<valueType>(this collection<valueType> collection)
        {
            return collection != null ? collection.ToList() : null;
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> reverse<valueType>(this collection<valueType> collection)
        {
            if (collection != null) collection.Reverse();
            return collection;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <returns>子集合</returns>
        public static collection<valueType> sub<valueType>(this collection<valueType> collection, int index)
        {
            return collection != null ? collection.Sub(index) : null;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public static collection<valueType> sub<valueType>(this collection<valueType> collection, int index, int count)
        {
            return collection != null ? collection.Sub(index, count) : null;
        }
        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量,小于0表示所有</param>
        /// <returns>子集合</returns>
        public static valueType[] getSub<valueType>(this collection<valueType> collection, int index, int count)
        {
            return collection != null ? collection.GetSub(index, count) : null;

        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this collection<valueType> collection, valueType value)
        {
            return collection != null ? collection.IndexOf(value) : -1;
        }
        /// <summary>
        /// 判断是否存在匹配
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配</returns>
        public static bool any<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null && collection.Any(isValue);   
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public static bool contains<valueType>(this collection<valueType> collection, valueType value)
        {
            return collection != null && collection.Contains(value);
        }
        /// <summary>
        /// 获取匹配位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.IndexOf(isValue) : -1;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.FirstOrDefault(isValue) : default(valueType);
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public static list<valueType> getFind<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.GetFind(isValue) : null;
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public static valueType[] getFindArray<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.GetFindArray(isValue) : null;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.GetCount(isValue) : 0;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="method">调用函数</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> each<valueType>(this collection<valueType> collection, action<valueType> method)
        {
            if (collection.count() != 0)
            {
                if (method == null) log.Default.Throw(log.exceptionType.Null);
                valueType[] array = collection.Unsafer.Array;
                int endIndex = collection.EndIndex, startIndex = collection.StartIndex;
                if (endIndex > startIndex)
                {
                    for (int index = startIndex; index != endIndex; method(array[index++])) ;
                }
                else if (collection.IsFull || endIndex < startIndex)
                {
                    for (int index = startIndex; index != array.Length; method(array[index++])) ;
                    for (int index = 0; index != endIndex; method(array[index++])) ;
                }
            }
            return collection;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> removeFirst<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            if (collection != null) collection.RemoveFirst(isValue);
            return null;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>被移除的数据元素,失败返回default(valueType)</returns>
        public static valueType getRemoveFirst<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            return collection != null ? collection.GetRemoveFirst(isValue) : default(valueType);
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> remove<valueType>(this collection<valueType> collection, func<valueType, bool> isValue)
        {
            if (collection != null) collection.Remove(isValue);
            return collection;
        }
        /// <summary>
        /// 替换第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="value">新数据元素</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> replaceFirst<valueType>
            (this collection<valueType> collection, valueType value, func<valueType, bool> isValue)
        {
            if (collection != null) collection.ReplaceFirst(value, isValue);
            return collection;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>双向动态数组</returns>
        public static collection<valueType> sort<valueType>(this collection<valueType> collection, func<valueType, valueType, int> comparer)
        {
            if (collection != null) collection.Sort(comparer);
            return collection;
        }
        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue">数据转换器</param>
        /// <returns>目标数据</returns>
        public static arrayType[] getPage<valueType, arrayType>
            (this collection<valueType> collection, int pageSize, int currentPage, func<valueType, arrayType> getValue)
        {
            return collection != null ? collection.GetPage(pageSize, currentPage, getValue) : null;
        }
        /// <summary>
        /// 获取数据排序范围
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序数据</returns>
        public static collection<valueType> rangeSort<valueType>
            (this collection<valueType> collection, func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return collection != null ? collection.RangeSort(comparer, skipCount, getCount) : null;
        }
        /// <summary>
        /// 获取排序数据分页
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="comparer">比较器</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>排序数据</returns>
        public static collection<valueType> pageSort<valueType>
            (this collection<valueType> collection, func<valueType, valueType, int> comparer, int pageSize, int currentPage)
        {
            return collection != null ? collection.PageSort(comparer, pageSize, currentPage) : null;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this collection<valueType> collection, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (collection != null) return collection.Max(comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (collection != null) collection.Max(getKey, comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType>(this collection<valueType> collection, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            if (collection != null)
            {
                valueType value;
                if (collection.Max((left, right) => left.CompareTo(right), out value)) return value;
            }
            return nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>(this collection<valueType> collection, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            return collection != null ? collection.Max(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        public static valueType max<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            return collection != null ? collection.Max(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>(this collection<valueType> collection, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            return collection != null ? collection.MaxKey(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最大键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大键值,失败返回默认空值</returns>
        public static keyType maxKey<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            return collection != null ? collection.MaxKey(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this collection<valueType> collection, func<valueType, valueType, int> comparer, out valueType value)
        {
            if (collection != null) return collection.Min(comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, out valueType value)
        {
            if (collection != null) collection.Min(getKey, comparer, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType>(this collection<valueType> collection, valueType nullValue)
            where valueType : IComparable<valueType>
        {
            if (collection != null)
            {
                valueType value;
                if (collection.Min((left, right) => left.CompareTo(right), out value)) return value;
            }
            return nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>(this collection<valueType> collection, func<valueType, keyType> getKey, valueType nullValue)
            where keyType : IComparable<keyType>
        {
            return collection != null ? collection.Min(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        public static valueType min<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, valueType nullValue)
        {
            return collection != null ? collection.Min(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>(this collection<valueType> collection, func<valueType, keyType> getKey, keyType nullValue)
            where keyType : IComparable<keyType>
        {
            return collection != null ? collection.MinKey(getKey, (left, right) => left.CompareTo(right), nullValue) : nullValue;
        }
        /// <summary>
        /// 获取最小键值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">比较键类型</typeparam>
        /// <param name="collection">数组数据</param>
        /// <param name="getKey">获取键值</param>
        /// <param name="comparer">比较器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小键值,失败返回默认空值</returns>
        public static keyType minKey<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey, func<keyType, keyType, int> comparer, keyType nullValue)
        {
            return collection != null ? collection.MinKey(getKey, comparer, nullValue) : nullValue;
        }
        /// <summary>
        /// 数据分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">键类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组数据</returns>
        public static Dictionary<keyType, list<valueType>> group<valueType, keyType>
            (this collection<valueType> collection, func<valueType, keyType> getKey)
        {
            return collection != null ? collection.Group(getKey) : null;
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="getValue">目标数据获取器</param>
        /// <returns>目标集合</returns>
        public static list<arrayType> distinct<valueType, arrayType>(this collection<valueType> collection, func<valueType, arrayType> getValue)
        {
            return collection != null ? collection.Distinct(getValue) : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this collection<valueType> collection, func<valueType, string> toString)
        {
            return collection != null ? collection.JoinString(toString) : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this collection<valueType> collection, string join, func<valueType, string> toString)
        {
            return collection != null ? collection.JoinString(join, toString) : null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="collection">双向动态数组</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        public static string joinString<valueType>(this collection<valueType> collection, char join, func<valueType, string> toString)
        {
            return collection != null ? collection.JoinString(join, toString) : null;
        }
    }
}
