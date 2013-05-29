using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using fastCSharp.reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 序列化代码生成自定义属性
    /// </summary>
    public partial class serialize : memberFilter.publicInstanceField
    {
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly serialize SerializeAttribute = new serialize();
        /// <summary>
        /// 序列化转换泛型标识
        /// </summary>
        public interface ISerializeGeneric { }
        /// <summary>
        /// 序列化接口
        /// </summary>
        public interface ISerialize
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <returns>序列化数据</returns>
            byte[] Serialize();
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            void Serialize(memoryStream stream);
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            void Serialize(dataSerializer serializer);
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            bool DeSerialize(byte[] data);
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            bool DeSerialize(byte[] data, int startIndex, out int endIndex);
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="deSerializer">对象反序列化器</param>
            void DeSerialize(deSerializer deSerializer);
        }
        /// <summary>
        /// 序列化接口
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface ISerialize<memberType> : ISerialize where memberType : IMemberMap<memberType>
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            /// <param name="memberMap">成员位图接口</param>
            void Serialize(memoryStream stream, memberType memberMap);
        }
        /// <summary>
        /// 序列化流接口
        /// </summary>
        public interface IStreamSerialize : ISerialize
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            void Serialize(Stream stream);
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            void Serialize(streamSerializer serializer);
        }
        /// <summary>
        /// 序列化流接口
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface IStreamSerialize<memberType> : IStreamSerialize where memberType : IMemberMap<memberType>
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            /// <param name="memberMap">成员位图接口</param>
            void Serialize(Stream stream, memberType memberMap);
        }
        /// <summary>
        /// 未知类型数据
        /// </summary>
        private static class unknownValue
        {
            /// <summary>
            /// 泛型类型缓存
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, keyValue<Type, Type>> genericTypes = new Dictionary<hashCode<Type>, keyValue<Type, Type>>();
            /// <summary>
            /// 泛型类型缓存访问锁
            /// </summary>
            private static int genericTypeLock;
            /// <summary>
            /// 泛型类型缓存版本
            /// </summary>
            private static int genericTypeVersion;
            /// <summary>
            /// 获取 泛型类型
            /// </summary>
            /// <param name="type">数据类型</param>
            /// <returns>泛型类型</returns>
            public static Type GetGenericType(Type type)
            {
                keyValue<Type, Type> genericType;
                int version = genericTypeVersion;
                hashCode<Type> hashType = type;
                if (genericTypes.TryGetValue(hashType, out genericType) && genericType.Key == type) return genericType.Value;
                while (Interlocked.CompareExchange(ref genericTypeLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (version == genericTypeVersion || !genericTypes.TryGetValue(hashType, out genericType))
                    {
                        genericTypes.Add(hashType, genericType = new keyValue<Type, Type>(type, typeof(unknownValue<>).MakeGenericType(type)));
                        ++genericTypeVersion;
                    }
                }
                finally { genericTypeLock = 0; }
                return genericType.Value;
            }
            /// <summary>
            /// 未知类型数据转换
            /// </summary>
            /// <param name="value">未知类型数据</param>
            /// <returns>未知类型数据</returns>
            private static object converter<valueType>(object value)
            {
                return new unknownValue<valueType> { Value = (valueType)value };
            }
            /// <summary>
            /// 未知类型数据转换 函数信息
            /// </summary>
            private static readonly MethodInfo converterMethod = typeof(unknownValue).GetMethod("converter", BindingFlags.Static | BindingFlags.NonPublic);
            /// <summary>
            /// 未知类型数据转换
            /// </summary>
            /// <param name="value">待转换数据</param>
            /// <param name="convertType">目标类型</param>
            /// <returns>转换后的数据</returns>
            public static object Converter(object value, Type convertType)
            {
                return ((func<object, object>)Delegate.CreateDelegate(typeof(func<object, object>), converterMethod.MakeGenericMethod(convertType)))(value);
            }
            /// <summary>
            /// 未知类型数据转换
            /// </summary>
            /// <param name="value">未知类型数据</param>
            /// <returns>未知类型数据</returns>
            private static object getValue<valueType>(object value)
            {
                return ((unknownValue<valueType>)value).Value;
            }
            /// <summary>
            /// 未知类型数据转换 函数信息
            /// </summary>
            private static readonly MethodInfo getValueMethod = typeof(unknownValue).GetMethod("getValue", BindingFlags.Static | BindingFlags.NonPublic);
            /// <summary>
            /// 未知类型数据转换
            /// </summary>
            /// <param name="type">原类型</param>
            /// <param name="convertType">目标类型</param>
            /// <param name="value">待转换数据</param>
            /// <returns>转换后的数据</returns>
            public static object GetValue(Type type, Type convertType, object value)
            {
                return ((func<object, object>)Delegate.CreateDelegate(typeof(func<object, object>), getValueMethod.MakeGenericMethod(convertType)))(value);
            }
        }
        /// <summary>
        /// 未知类型数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        private struct unknownValue<valueType>
        {
            /// <summary>
            /// 数据
            /// </summary>
            public valueType Value;
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        public abstract unsafe class serializer
        {
            /// <summary>
            /// 空对象长度
            /// </summary>
            protected internal static readonly byte[] nullLength = BitConverter.GetBytes(NullValue);
            /// <summary>
            /// 历史对象指针位置
            /// </summary>
            protected Dictionary<object, int> points;
            /// </summary>
            /// 成员位图接口
            /// </summary>
            protected internal IMemberMap memberMap;
            /// </summary>
            /// 数据流起始位置
            /// </summary>
            protected int streamStartIndex;
            /// <summary>
            /// 成员选择(反射模式)
            /// </summary>
            protected internal setup.memberFilter memberFilter;
            /// <summary>
            /// 当前写入位置
            /// </summary>
            protected internal byte* write;
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        public abstract unsafe class dataSerializer : serializer
        {
            /// <summary>
            /// 数据流
            /// </summary>
            protected internal memoryStream dataStream;
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="stream">序列化流</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="memberFilter">成员选择</param>
            protected dataSerializer(memoryStream stream, IMemberMap memberMap, setup.memberFilter memberFilter = setup.memberFilter.InstanceField)
            {
                this.memberMap = memberMap;
                points = new Dictionary<object, int>();
                dataStream = stream;
                streamStartIndex = stream.Length;
                stream.Write((int)fastCSharp.setup.cSharp.serializeVersion.serialize);
                this.memberFilter = memberFilter;
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="parentSerializer">序列化</param>
            /// <param name="memberMap">成员位图接口</param>
            protected dataSerializer(dataSerializer parentSerializer, IMemberMap memberMap)
            {
                points = parentSerializer.points;
                dataStream = parentSerializer.dataStream;
                streamStartIndex = parentSerializer.streamStartIndex;
                this.memberMap = memberMap;
                this.memberFilter = parentSerializer.memberFilter;
            }
            /// <summary>
            /// 序列化版本号与成员位图
            /// </summary>
            /// <param name="version">版本号</param>
            protected internal void versionMemerMap(int version)
            {
                dataStream.Write(version);
                memberMap.Serialize(dataStream);
            }
            /// <summary>
            /// 序列化结束
            /// </summary>
            public void Finally()
            {
                dataStream.Write(dataStream.Length - streamStartIndex);
            }
            /// <summary>
            /// 历史记录检测
            /// </summary>
            /// <param name="value">检测对象</param>
            /// <returns>是否新的对象(非历史对象)</returns>
            protected internal bool checkPoint(object value)
            {
                if (value == null) return false;
                int length;
                if (points.TryGetValue(value, out length))
                {
                    dataStream.Write(-length);
                    return false;
                }
                points[value] = dataStream.Length - streamStartIndex;
                return true;
            }
            /// <summary>
            /// 字符串序列化
            /// </summary>
            /// <param name="value">字符串</param>
            protected void serializeString(string value)
            {
                if (checkPoint(value)) dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerialize<valueType>(valueType value) where valueType : ISerialize
            {
                if (checkPoint(value)) iSerializeNoPoint(value);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerializeNoPoint<valueType>(valueType value) where valueType : ISerialize
            {
                Type type = value.GetType();
                if (type == typeof(valueType)) dataStream.Write(NullValue);
                else (new fastCSharp.setup.remoteType(type)).Serialize(this);
                value.Serialize(this);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerializeNotNull<valueType>(valueType value) where valueType : ISerialize
            {
                value.Serialize(this);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void iSerializeArrayNoPoint<valueType>(valueType[] array) where valueType : ISerialize
            {
                arrayMap(array, dataStream);
                foreach (valueType nextValue in array)
                {
                    if (checkPoint(nextValue)) iSerializeNoPoint(nextValue);
                }
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void iSerializeArrayNotNullNoPoint<valueType>(valueType[] array) where valueType : ISerialize
            {
                dataStream.Write(array.Length);
                foreach (valueType nextValue in array) nextValue.Serialize(this);
            }
            /// <summary>
            /// 字节数组序列化
            /// </summary>
            /// <param name="value">字节数组</param>
            protected void byteArray(byte[] value)
            {
                if (checkPoint(value)) dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(byte?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (byte? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else *write++ = (byte)nextValue;
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(sbyte?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (sbyte? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else *write++ = (byte)(sbyte)nextValue;
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(short?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length * sizeof(short) + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (short? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(short*)write = (short)nextValue;
                            write += sizeof(short);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(ushort?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length * sizeof(ushort) + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (ushort? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(ushort*)write = (ushort)nextValue;
                            write += sizeof(ushort);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(int?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(int) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (int? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(int*)write = (int)nextValue;
                            write += sizeof(int);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(uint?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(uint) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (uint? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(uint*)write = (uint)nextValue;
                            write += sizeof(uint);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(long?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(long) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (long? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(long*)write = (long)nextValue;
                            write += sizeof(long);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(ulong?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(ulong) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (ulong? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(ulong*)write = (ulong)nextValue;
                            write += sizeof(ulong);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(char?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length * sizeof(char) + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (char? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(char*)write = (char)nextValue;
                            write += sizeof(char);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(DateTime?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(DateTime) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (DateTime? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(DateTime*)write = (DateTime)nextValue;
                            write += sizeof(DateTime);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(float?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(float) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (float? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(float*)write = (float)nextValue;
                            write += sizeof(float);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(double?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(double) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (double? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(double*)write = (double)nextValue;
                            write += sizeof(double);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(decimal?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength((array.Length * sizeof(decimal) + sizeof(int) + mapLength + 3) & (int.MaxValue - 3));
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (decimal? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(decimal*)write = (decimal)nextValue;
                            write += sizeof(decimal);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength(((int)(write - start) + 3) & (int.MaxValue - 3));
                }
            }
            /// <summary>
            /// 序列化可空数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void nullArrayNoPoint(Guid?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2;
                dataStream.PrepLength(array.Length * sizeof(Guid) + sizeof(int) + mapLength);
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* start = dataFixed + dataStream.Length, write = start;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    write += mapLength;
                    mapLength = 0;
                    foreach (Guid? nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(mapLength);
                        else
                        {
                            *(Guid*)write = (Guid)nextValue;
                            write += sizeof(Guid);
                        }
                        ++mapLength;
                    }
                    dataStream.Unsafer.AddLength((int)(write - start));
                }
            }
            /// <summary>
            /// 序列化字符串数组
            /// </summary>
            /// <param name="array">字符串数组</param>
            protected void stringArrayNoPoint(string[] array)
            {
                arrayMap(array, dataStream);
                memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                foreach (string nextValue in array)
                {
                    if (checkPoint(nextValue)) unsafeStream.PrepSerialize(nextValue);
                }
            }
            /// <summary>
            /// 未知类型数组序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            private static void arrayMap<valueType>(valueType[] array, memoryStream dataStream)
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                dataStream.PrepLength(length);
                memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* write = dataFixed + dataStream.Length;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    unsafeStream.AddLength(length);
                    length = 0;
                    foreach (valueType nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(length);
                        ++length;
                    }
                }
            }
            /// <summary>
            /// 未知类型数组序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            protected internal void unknownArray<valueType>(valueType[] array)
            {
                arrayMap(array, dataStream);
                foreach (valueType nextValue in array) unknown(nextValue);
            }
            /// <summary>
            /// 未知类型数组序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void unknownArrayNull<valueType>(Nullable<valueType>[] array) where valueType : struct
            {
                unknownArrayNull(new reflectionDataSerializer(this), array);
            }
            /// <summary>
            /// 未知类型数组序列化
            /// </summary>
            /// <typeparam name="serializer">对象序列化器</typeparam>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            internal void unknownArrayNull<valueType>(reflectionDataSerializer serializer, Nullable<valueType>[] array)
                where valueType : struct
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                dataStream.PrepLength(length);
                memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                fixed (byte* dataFixed = dataStream.Array)
                {
                    byte* write = dataFixed + dataStream.Length;
                    *(int*)write = array.Length;
                    fixedMap nullMap = new fixedMap(write += sizeof(int));
                    unsafeStream.AddLength(length);
                    length = 0;
                    foreach (Nullable<valueType> nextValue in array)
                    {
                        if (nextValue == null) nullMap.Set(length);
                        ++length;
                    }
                }
                serializer.memberMap = default(memberMap<valueType>);
                foreach (Nullable<valueType> nextValue in array)
                {
                    if (nextValue != null) dataSerialize<valueType>.GetVersionMemerMap(serializer, nextValue.Value);
                }
            }
            /// <summary>
            /// 未知值类型数组序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void unknownArrayNotNull<valueType>(valueType[] array)
            {
                unknownArrayNotNull(new reflectionDataSerializer(this), array);
            }
            /// <summary>
            /// 未知值类型数组序列化
            /// </summary>
            /// <typeparam name="serializer">对象序列化器</typeparam>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="array">数组数据</param>
            internal void unknownArrayNotNull<valueType>(reflectionDataSerializer serializer, valueType[] array)
            {
                dataStream.Write(array.Length);
                serializer.memberMap = default(memberMap<valueType>);
                foreach (valueType value in array) dataSerialize<valueType>.GetVersionMemerMap(serializer, value);
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknown<valueType>(valueType value)
            {
                if (checkPoint(value)) unknownNoPoint(value);
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknownNoPoint<valueType>(valueType value)
            {
                Type type = value.GetType();
                if (type == typeof(valueType))
                {
                    dataStream.Write(NullValue);
                    unknownNotNull(value);
                }
                else
                {
                    (new fastCSharp.setup.remoteType(type)).Serialize(this);
                    ((action<dataSerializer, object>)Delegate.CreateDelegate(typeof(action<dataSerializer, object>), unknownNotNullMethod.MakeGenericMethod(unknownValue.GetGenericType(type))))(this, unknownValue.Converter(value, type));
                }
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknownNull<valueType>(Nullable<valueType> value) where valueType : struct
            {
                if (value != null) unknownNotNull(value.Value);
            }
            /// <summary>
            /// 未知值类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">值类型数据</param>
            protected void unknownNotNull<valueType>(valueType value)
            {
                reflectionDataSerializer serializer = new reflectionDataSerializer(this, default(memberMap<valueType>));
                dataSerialize<valueType>.GetVersionMemerMap(serializer, value);
            }
            /// <summary>
            /// 未知值类型数据序列化
            /// </summary>
            /// <typeparam name="serializer">对象序列化器</typeparam>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">值类型数据</param>
            private static void unknownNotNullObject<valueType>(dataSerializer serializer, object value)
            {
                serializer.unknownNotNull((valueType)value);
            }
            /// <summary>
            /// 未知值类型数据序列化 函数信息
            /// </summary>
            private static readonly MethodInfo unknownNotNullMethod = typeof(dataSerializer).GetMethod("unknownNotNullObject", BindingFlags.Static | BindingFlags.NonPublic);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        public unsafe abstract class streamSerializer : serializer
        {
            /// <summary>
            /// 序列化格式版本号
            /// </summary>
            private static readonly byte[] serializeVersion = BitConverter.GetBytes((int)fastCSharp.setup.cSharp.serializeVersion.serialize);
            /// <summary>
            /// 数据流
            /// </summary>
            protected internal Stream dataStream;
            /// <summary>
            /// 临时数据
            /// </summary>
            protected internal byte[] valueData;
            /// <summary>
            /// 临时数据
            /// </summary>
            protected internal byte* dataPoint;
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="stream">序列化流</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="serializeSize">序列化基本长度</param>
            /// <param name="memberFilter">成员选择</param>
            protected streamSerializer
                (Stream stream, IMemberMap memberMap, int serializeSize, setup.memberFilter memberFilter = setup.memberFilter.InstanceField)
            {
                dataStream = stream;
                streamStartIndex = (int)stream.Position;
                stream.Write(serializeVersion, 0, sizeof(int));
                points = new System.Collections.Generic.Dictionary<object, int>();
                this.memberMap = memberMap;
                this.memberFilter = memberFilter;
                int length = (((memberMap.MemberInfo.MemberCount + 31) >> 5) << 2) + serializeSize;
                valueData = new byte[length > memberType.MaxSerializeSize ? length : memberType.MaxSerializeSize];
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="parentSerializer">序列化</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="serializeSize">序列化基本长度</param>
            protected streamSerializer(streamSerializer parentSerializer, fastCSharp.setup.cSharp.IMemberMap memberMap, int serializeSize)
            {
                points = parentSerializer.points;
                dataStream = parentSerializer.dataStream;
                streamStartIndex = parentSerializer.streamStartIndex;
                this.memberMap = memberMap;
                this.memberFilter = parentSerializer.memberFilter;
                int length = (((memberMap.MemberInfo.MemberCount + 31) >> 5) << 2) + serializeSize;
                if (length < memberType.MaxSerializeSize) length = memberType.MaxSerializeSize;
                valueData = length > parentSerializer.valueData.Length ? new byte[length] : parentSerializer.valueData;
                dataPoint = parentSerializer.dataPoint;
            }
            /// <summary>
            /// 序列化结束
            /// </summary>
            public void Finally()
            {
                *(int*)dataPoint = (int)(dataStream.Position - streamStartIndex);
                dataStream.Write(valueData, 0, sizeof(int));
            }
            /// <summary>
            /// 序列化版本号与成员位图
            /// </summary>
            /// <param name="version">版本号</param>
            protected internal void versionMemerMap(int version)
            {
                *(int*)dataPoint = version;
                dataStream.Write(valueData, 0, sizeof(int));
                memberMap.Serialize(dataStream);
            }
            /// <summary>
            /// 历史记录检测
            /// </summary>
            /// <param name="value">检测对象</param>
            /// <returns>是否新的对象(非历史对象)</returns>
            protected internal bool checkPoint(object value)
            {
                if (value == null) return false;
                int length;
                if (points.TryGetValue(value, out length))
                {
                    *(int*)dataPoint = -length;
                    dataStream.Write(valueData, 0, sizeof(int));
                    return false;
                }
                points[value] = (int)dataStream.Position - streamStartIndex;
                return true;
            }
            /// <summary>
            /// 序列化字符串(4字节对齐)
            /// </summary>
            /// <param name="value">字符串</param>
            protected void serializeString(string value)
            {
                if (checkPoint(value)) serializeStringNoPoint(value);
            }
            /// <summary>
            /// 序列化字符串(4字节对齐)
            /// </summary>
            /// <param name="value">字符串</param>
            protected void serializeStringNoPoint(string value)
            {
                byte[] data = value.serializeNotNull();
                if (data.Length != 0)
                {
                    *(int*)dataPoint = data.Length == value.Length ? (value.Length << 1) + 1 : (value.Length << 1);
                    dataStream.Write(valueData, 0, sizeof(int));
                    dataStream.Write(data, 0, data.Length);
                    if ((data.Length & 3) != 0) dataStream.Seek(-data.Length & 3, SeekOrigin.Current);
                }
                else
                {
                    *(int*)dataPoint = 0;
                    dataStream.Write(valueData, 0, sizeof(int));
                }
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerialize<valueType>(valueType value) where valueType : IStreamSerialize
            {
                if (checkPoint(value)) iSerializeNoPoint(value);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerializeNoPoint<valueType>(valueType value) where valueType : IStreamSerialize
            {
                Type type = value.GetType();
                if (type == typeof(valueType))
                {
                    *(int*)dataPoint = NullValue;
                    dataStream.Write(valueData, 0, sizeof(int));
                }
                else (new fastCSharp.setup.remoteType(type)).Serialize(this);
                value.Serialize(this);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">序列化接口数据</param>
            protected void iSerializeNotNull<valueType>(valueType value) where valueType : IStreamSerialize
            {
                value.Serialize(this);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void iSerializeArrayNoPoint<valueType>(valueType[] array) where valueType : IStreamSerialize
            {
                unknownArrayMap(array);
                foreach (valueType nextValue in array)
                {
                    if (checkPoint(nextValue)) iSerializeNoPoint(nextValue);
                }
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void iSerializeArrayNotNullNoPoint<valueType>(valueType[] array) where valueType : IStreamSerialize
            {
                *(int*)dataPoint = array.Length;
                dataStream.Write(valueData, 0, sizeof(int));
                foreach (valueType nextValue in array) nextValue.Serialize(this);
            }
            /// <summary>
            /// 写入长度与数据(4字节对齐)
            /// </summary>
            /// <param name="data">数据</param>
            protected void byteArray(byte[] data)
            {
                if (checkPoint(data)) byteArrayNoPoint(data);
            }
            /// <summary>
            /// 写入长度与数据(4字节对齐)
            /// </summary>
            /// <param name="data">数据</param>
            protected internal void byteArrayNoPoint(byte[] data)
            {
                *(int*)dataPoint = data.Length;
                dataStream.Write(valueData, 0, sizeof(int));
                dataStream.Write(data, 0, data.Length);
                if ((data.Length & 3) != 0) dataStream.Seek(-data.Length & 3, SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void serializeNoPoint(bool[] array)
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data)
                    {
                        serialize(array, dataPoint);
                        dataStream.Write(data, 0, length);
                    }
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    serialize(array, dataPoint);
                    dataStream.Write(valueData, 0, length);
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            private static void serialize(bool[] array, byte* dataPoint)
            {
                *(int*)dataPoint = array.Length;
                fixedMap valueMap = new fixedMap(dataPoint + sizeof(int));
                int index = 0;
                foreach (bool value in array)
                {
                    if (value) valueMap.Set(index);
                    ++index;
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void serializeNoPoint(bool?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + mapLength;
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data)
                    {
                        serialize(array, dataPoint, mapLength);
                        dataStream.Write(data, 0, length);
                    }
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    serialize(array, dataPoint, mapLength);
                    dataStream.Write(valueData, 0, length);
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            private static void serialize(bool?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                fixedMap valueMap = new fixedMap(dataPoint += mapLength);
                int index = 0;
                foreach (bool? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else if ((bool)value) valueMap.Set(index);
                    ++index;
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(byte?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length;
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(byte?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (byte? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else *dataPoint++ = (byte)value;
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(sbyte?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length;
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(sbyte?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (sbyte? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else *dataPoint++ = (byte)(sbyte)value;
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(short?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(short);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(short?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (short? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(short*)dataPoint = (short)value;
                        dataPoint += sizeof(short);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(ushort?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(ushort);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(ushort?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (ushort? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(ushort*)dataPoint = (ushort)value;
                        dataPoint += sizeof(ushort);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(int?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(int);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(int?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (int? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(int*)dataPoint = (int)value;
                        dataPoint += sizeof(int);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(uint?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(uint);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(uint?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (uint? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(uint*)dataPoint = (uint)value;
                        dataPoint += sizeof(uint);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(long?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(long);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(long?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (long? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(long*)dataPoint = (long)value;
                        dataPoint += sizeof(long);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(ulong?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(ulong);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(ulong?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (ulong? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(ulong*)dataPoint = (ulong)value;
                        dataPoint += sizeof(ulong);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(char?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(char);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(char?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (char? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(char*)dataPoint = (char)value;
                        dataPoint += sizeof(char);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(DateTime?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(DateTime);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(DateTime?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (DateTime? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(DateTime*)dataPoint = (DateTime)value;
                        dataPoint += sizeof(DateTime);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(float?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(float);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(float?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (float? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(float*)dataPoint = (float)value;
                        dataPoint += sizeof(float);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(double?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(double);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(double?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (double? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(double*)dataPoint = (double)value;
                        dataPoint += sizeof(double);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(decimal?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(decimal);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(decimal?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (decimal? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(decimal*)dataPoint = (decimal)value;
                        dataPoint += sizeof(decimal);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void nullArrayNoPoint(Guid?[] array)
            {
                int mapLength = ((array.Length + 31) >> 5) << 2, length = sizeof(int) + mapLength + array.Length * sizeof(Guid);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data) dataStream.Write(data, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint + sizeof(int), (uint)0, mapLength >> 2);
                    dataStream.Write(valueData, 0, length = (int)(nullArray(array, dataPoint, mapLength) - dataPoint));
                }
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="dataPoint">目标数据</param>
            /// <param name="mapLength">位图字节数</param>
            /// <returns>目标数据结束位置</returns>
            private static byte* nullArray(Guid?[] array, byte* dataPoint, int mapLength)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                dataPoint += mapLength;
                int index = 0;
                foreach (Guid? value in array)
                {
                    if (value == null) nullMap.Set(index);
                    else
                    {
                        *(Guid*)dataPoint = (Guid)value;
                        dataPoint += sizeof(Guid);
                    }
                    ++index;
                }
                return dataPoint;
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <typeparam name="valueType">数组类型</typeparam>
            /// <param name="array">数组</param>
            /// <param name="length">复制字节数</param>
            protected void serializeNoPoint<valueType>(valueType[] array, int length) where valueType : struct
            {
                if (length + sizeof(int) > valueData.Length)
                {
                    *(int*)dataPoint = array.Length;
                    dataStream.Write(valueData, 0, sizeof(int));
                    if (length > valueData.Length)
                    {
                        byte[] data = new byte[length];
                        Buffer.BlockCopy(array, 0, data, 0, length);
                        dataStream.Write(data, 0, length);
                    }
                    else
                    {
                        Buffer.BlockCopy(array, 0, valueData, 0, length);
                        dataStream.Write(valueData, 0, length);
                    }
                }
                else
                {
                    *(int*)dataPoint = array.Length;
                    Buffer.BlockCopy(array, 0, valueData, sizeof(int), length);
                    dataStream.Write(valueData, 0, length + sizeof(int));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void serializeNoPoint(DateTime[] array)
            {
                fixed (DateTime* dataFixed = array) serialize(dataFixed, array.Length, sizeof(DateTime));
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void serializeNoPoint(decimal[] array)
            {
                fixed (decimal* dataFixed = array) serialize(dataFixed, array.Length, sizeof(decimal));
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="array">数组</param>
            protected void serializeNoPoint(Guid[] array)
            {
                fixed (Guid* dataFixed = array) serialize(dataFixed, array.Length, sizeof(Guid));
            }
            /// <summary>
            /// 序列化结构体数组(4字节对齐)
            /// </summary>
            /// <param name="dataFixed">数组</param>
            /// <param name="count">数据数量</param>
            /// <param name="size">数据字节数</param>
            private void serialize(void* dataFixed, int count, int size)
            {
                int length = count * size;
                if (length + sizeof(int) > valueData.Length)
                {
                    *(int*)dataPoint = count;
                    dataStream.Write(valueData, 0, sizeof(int));
                    if (length > valueData.Length)
                    {
                        byte[] data = new byte[length];
                        fastCSharp.unsafer.memory.Copy(dataFixed, data, length);
                        dataStream.Write(data, 0, length);
                    }
                    else
                    {
                        fastCSharp.unsafer.memory.Copy(dataFixed, dataPoint, length);
                        dataStream.Write(valueData, 0, length);
                    }
                }
                else
                {
                    *(int*)dataPoint = count;
                    fastCSharp.unsafer.memory.Copy(dataFixed, dataPoint + sizeof(int), length);
                    dataStream.Write(valueData, 0, length + sizeof(int));
                }
                if ((length & 3) != 0) dataStream.Seek(-length & 3, System.IO.SeekOrigin.Current);
            }
            /// <summary>
            /// 序列化字符串数组(4字节对齐)
            /// </summary>
            /// <param name="array">字符串数组</param>
            protected void stringArrayNoPoint(string[] array)
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data)
                    {
                        serialize(array, dataPoint);
                        dataStream.Write(data, 0, length);
                    }
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    serialize(array, dataPoint);
                    dataStream.Write(valueData, 0, length);
                }
                foreach (string nextValue in array) serializeString(nextValue);
            }
            /// <summary>
            /// 序列化字符串数组(4字节对齐)
            /// </summary>
            /// <param name="array">字符串数组</param>
            /// <param name="dataPoint">目标数据</param>
            private void serialize(string[] array, byte* dataPoint)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                int index = 0;
                foreach (string value in array)
                {
                    if (value == null) nullMap.Set(index);
                    ++index;
                }
            }
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void unknownArray<valueType>(valueType[] array)
            {
                unknownArrayMap(array);
                foreach (valueType nextValue in array) unknown(nextValue);
            }
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected internal void unknownArrayMap<valueType>(valueType[] array)
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data)
                    {
                        unknownArray(array, dataPoint);
                        dataStream.Write(valueData, 0, length);
                    }
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    unknownArray(array, dataPoint);
                    dataStream.Write(valueData, 0, length);
                }
            }
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            /// <param name="dataPoint">目标数据</param>
            private void unknownArray<valueType>(valueType[] array, byte* dataPoint)
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                int index = 0;
                foreach (valueType value in array)
                {
                    if (value == null) nullMap.Set(index);
                    ++index;
                }
            }
            /// <summary>
            /// 未知可空类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">可空数组</param>
            protected void unknownArrayNull<valueType>(Nullable<valueType>[] array) where valueType : struct
            {
                unknownArrayNullMap(array);
                reflectionStreamSerializer serializer = new reflectionStreamSerializer(this, default(memberMap<valueType>), streamSerialize<valueType>.serializeSize);
                foreach (Nullable<valueType> value in array)
                {
                    if (value != null) streamSerialize<valueType>.GetVersionMemerMap(serializer, value.Value);
                }
            }
            /// <summary>
            /// 未知可空类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">可空数组</param>
            protected void unknownArrayNullMap<valueType>(Nullable<valueType>[] array) where valueType : struct
            {
                int length = sizeof(int) + (((array.Length + 31) >> 5) << 2);
                if (length > valueData.Length)
                {
                    byte[] data = new byte[length];
                    fixed (byte* dataPoint = data)
                    {
                        unknownArrayNull(array, dataPoint);
                        dataStream.Write(valueData, 0, length);
                    }
                }
                else
                {
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    unknownArrayNull(array, dataPoint);
                    dataStream.Write(valueData, 0, length);
                }
            }
            /// <summary>
            /// 未知可空类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">可空数组</param>
            /// <param name="dataPoint">目标数据</param>
            private void unknownArrayNull<valueType>(Nullable<valueType>[] array, byte* dataPoint) where valueType : struct
            {
                *(int*)dataPoint = array.Length;
                fixedMap nullMap = new fixedMap(dataPoint += sizeof(int));
                int index = 0;
                foreach (Nullable<valueType> value in array)
                {
                    if (value == null) nullMap.Set(index);
                    ++index;
                }
            }
            /// <summary>
            /// 值类型数组序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void unknownArrayNotNull<valueType>(valueType[] array)
            {
                *(int*)dataPoint = array.Length;
                dataStream.Write(valueData, 0, sizeof(int));
                reflectionStreamSerializer serializer = new reflectionStreamSerializer(this, default(memberMap<valueType>), streamSerialize<valueType>.serializeSize);
                foreach (valueType value in array) streamSerialize<valueType>.GetVersionMemerMap(serializer, value);
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknown<valueType>(valueType value)
            {
                if (checkPoint(value)) unknownNoPoint(value);
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknownNoPoint<valueType>(valueType value)
            {
                Type type = value.GetType();
                if (type == typeof(valueType))
                {
                    *(int*)dataPoint = NullValue;
                    dataStream.Write(valueData, 0, sizeof(int));
                    unknownNotNull(value);
                }
                else
                {
                    (new fastCSharp.setup.remoteType(type)).Serialize(this);
                    ((action<streamSerializer, object>)Delegate.CreateDelegate(typeof(action<streamSerializer, object>), unknownNotNullMethod.MakeGenericMethod(unknownValue.GetGenericType(type))))(this, unknownValue.Converter(value, type));
                }
            }
            /// <summary>
            /// 未知类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">未知类型数据</param>
            protected void unknownNull<valueType>(Nullable<valueType> value) where valueType : struct
            {
                if (value != null) unknownNotNull(value.Value);
            }
            /// <summary>
            /// 未知值类型数据序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">值类型数据</param>
            protected void unknownNotNull<valueType>(valueType value)
            {
                reflectionStreamSerializer serializer = new reflectionStreamSerializer(this, default(memberMap<valueType>), streamSerialize<valueType>.serializeSize);
                streamSerialize<valueType>.GetVersionMemerMap(serializer, value);
            }
            /// <summary>
            /// 未知值类型数据序列化
            /// </summary>
            /// <typeparam name="serializer">对象序列化器</typeparam>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">值类型数据</param>
            private static void unknownNotNullObject<valueType>(streamSerializer serializer, object value)
            {
                serializer.unknownNotNull((valueType)value);
            }
            /// <summary>
            /// 未知值类型数据序列化 函数信息
            /// </summary>
            private static readonly MethodInfo unknownNotNullMethod = typeof(streamSerializer).GetMethod("unknownNotNullObject", BindingFlags.Static | BindingFlags.NonPublic);
        }
        /// <summary>
        /// 对象反序列化
        /// </summary>
        public unsafe abstract class deSerializer
        {
            /// <summary>
            /// 结束位置(公用)
            /// </summary>
            public static int OutEndIndex;
            /// <summary>
            /// 历史对象指针位置
            /// </summary>
            protected internal Dictionary<int, object> points;
            /// <summary>
            /// 数据固定位置
            /// </summary>
            protected byte* dataFixed;
            /// <summary>
            /// 数据起始位置
            /// </summary>
            protected internal byte* dataStart;
            /// <summary>
            /// 字节数组
            /// </summary>
            protected byte[] data;
            /// <summary>
            /// 当前位置
            /// </summary>
            protected internal byte* read;
            /// <summary>
            /// 序列化版本号
            /// </summary>
            protected int dataVersion;
            /// <summary>
            /// 序列化结束位置
            /// </summary>
            public int EndIndex
            {
                get
                {
                    return (int)(read - dataFixed);
                }
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            protected deSerializer(byte[] data)
            {
                this.data = data;
                points = new System.Collections.Generic.Dictionary<int, object>();
            }
            /// <summary>
            /// 对象集合反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="dataFixed">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="version">序列化格式版本号</param>
            protected deSerializer(byte[] data, byte* dataFixed, int startIndex, serializeVersion version)
            {
                this.data = data;
                this.dataFixed = dataFixed;
                read = dataFixed + startIndex;
                if (*(int*)read != (int)version)
                {
                    fastCSharp.log.Default.Throw("序列化格式版本号不匹配 fastCSharp.setup.cSharp.serializeVersion." + version.ToString() + "[" + ((int)version).toString() + "] != " + (*(int*)read).toString(), true, false);
                }
                dataStart = read;
                read += sizeof(int);
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            public deSerializer(deSerializer parentDeSerializer)
            {
                data = parentDeSerializer.data;
                dataFixed = parentDeSerializer.dataFixed;
                dataStart = parentDeSerializer.dataStart;
                read = parentDeSerializer.read;
                points = parentDeSerializer.points;
            }
            /// <summary>
            /// 版本号+成员位图接口
            /// </summary>
            protected abstract void versionMemberMap();
            /// <summary>
            /// 数据结束标识检测
            /// </summary>
            protected internal void checkEnd()
            {
                if (*(int*)read == (int)(read - dataStart)) read += sizeof(int);
                else fastCSharp.log.Default.Throw("数据结束标识不匹配 " + ((int)(read - dataStart)).toString() + " != " + (*(int*)read).toString(), true, false);
            }
            /// <summary>
            /// 设置当前位置
            /// </summary>
            /// <param name="deSerializer">对象反序列化</param>
            public void SetRead(deSerializer deSerializer)
            {
                read = deSerializer.read;
            }
            /// <summary>
            /// 历史记录检测
            /// </summary>
            /// <returns>历史记录对象,失败返回null</returns>
            protected object getPoint()
            {
                object value;
                int point = *(int*)read;
                if (points.TryGetValue(point, out value))
                {
                    read += sizeof(int);
                    return value;
                }
                return null;
            }
            /// <summary>
            /// 反序列化字符串
            /// </summary>
            /// <returns>字符串</returns>
            protected string getStringNoPoint()
            {
                int length = *(int*)read, point = (int)(read - dataStart);
                read += sizeof(int);
                if ((length & 1) == 0)
                {
                    if (length != 0)
                    {
                        string value = new string((char*)read, 0, length >> 1);
                        read += length + (length & 2);
                        points.Add(-point, value);
                        return value;
                    }
                }
                else if (length > 1)
                {
                    string value = new string((char)0, length >>= 1);
                    fixed (char* valueFixed = value)
                    {
                        char* start = valueFixed;
                        for (byte* end = read + length; read != end; *start++ = (char)*read++) ;
                    }
                    if ((length & 3) != 0) read += -length & 3;
                    points.Add(-point, value);
                    return value;
                }
                points.Add(-point, string.Empty);
                return string.Empty;
            }
            /// <summary>
            /// 反序列化字符串
            /// </summary>
            /// <returns>字符串</returns>
            protected string getString()
            {
                object reference = getPoint();
                if (reference != null) return (string)reference;
                return getStringNoPoint();
            }
            /// <summary>
            /// 序列化接口数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="newValue">获取新数据委托</param>
            /// <returns>序列化接口数据</returns>
            protected valueType iSerialize<valueType>(func<valueType> newValue) where valueType : ISerialize
            {
                object reference = getPoint();
                if (reference != null) return (valueType)reference;
                return iSerializeNoPoint(newValue);
            }
            /// <summary>
            /// 序列化接口数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="newValue">获取新数据委托</param>
            /// <returns>序列化接口数据</returns>
            protected valueType iSerializeNoPoint<valueType>(func<valueType> newValue) where valueType : ISerialize
            {
                valueType value;
                int point = -(int)(read - dataStart);
                if (*(int*)read == NullValue)
                {
                    read += sizeof(int);
                    value = newValue != null ? newValue() : constructor<valueType>.New;
                }
                else
                {
                    fastCSharp.setup.remoteType remoteType = new fastCSharp.setup.remoteType();
                    remoteType.DeSerialize(this);
                    Type type = remoteType.Type;
                    if (!type.isInherit(typeof(valueType))) log.Default.Throw(type.fullName() + " 不继承 " + typeof(valueType).fullName() + " ,无法反序列化", false, false);
                    value = (valueType)constructor.Get(type);
                }
                points.Add(point, value);
                value.DeSerialize(this);
                return value;
            }
            /// <summary>
            /// 序列化接口数组(仅用于代码生成)
            /// </summary>
            /// <typeparam name="valueType"></typeparam>
            /// <param name="value"></param>
            /// <returns></returns>
            internal valueType iSerializeNotNull<valueType>(object value)
            {
                return default(valueType);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>序列化接口数据</returns>
            protected valueType iSerializeNotNull<valueType>() where valueType : ISerialize
            {
                valueType value = default(valueType);
                value.DeSerialize(this);
                return value;
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            /// <param name="newValue">获取新数据委托</param>
            protected void iSerializeArrayNoPoint<valueType>(valueType[] array, func<valueType> newValue) where valueType : ISerialize
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = iSerialize<valueType>(newValue);
                }
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>数组数据</returns>
            protected valueType[] iSerializeArrayNoPoint<valueType>() where valueType : ISerialize
            {
                valueType[] array = createArray<valueType>();
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = iSerialize<valueType>();
                }
                return array;
            }
            /// <summary>
            /// 序列化接口数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>序列化接口数据</returns>
            private valueType iSerialize<valueType>() where valueType : ISerialize
            {
                object reference = getPoint();
                if (reference != null) return (valueType)reference;
                return iSerializeNoPoint<valueType>(null);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">数组数据</param>
            protected void iSerializeArrayNotNullNoPoint<valueType>(valueType[] array) where valueType : ISerialize
            {
                for (int index = 0; index != array.Length; ++index) array[index] = iSerializeNotNull<valueType>();
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>数组数据</returns>
            protected valueType[] iSerializeArrayNotNullNoPoint<valueType>() where valueType : ISerialize
            {
                valueType[] array = createArray<valueType>();
                iSerializeArrayNotNullNoPoint(array);
                return array;
            }
            /// <summary>
            /// 创建数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>数组数据</returns>
            private valueType[] createArray<valueType>()
            {
                int length = *(int*)read;
                valueType[] array = new valueType[length];
                points.Add(-(int)(read - dataStart), array);
                read += sizeof(int);
                return array;
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <returns>字节数组</returns>
            protected byte[] byteArrayNoPoint()
            {
                byte[] data = createArray<byte>();
                Buffer.BlockCopy(this.data, (int)(read - dataFixed), data, 0, data.Length);
                read += data.Length;
                if ((data.Length & 3) != 0) read += -data.Length & 3;
                return data;
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <returns>字节数组</returns>
            protected byte[] byteArray()
            {
                object reference = getPoint();
                if (reference != null) return (byte[])reference;
                return byteArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void deSerializeNoPoint(bool[] array)
            {
                fixedMap valueMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                fixed (bool* dataFixed = array)
                {
                    for (int index = array.Length; --index >= 0; dataFixed[index] = valueMap.Get(index)) ;
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void deSerializeNoPoint(bool?[] array)
            {
                int length = ((array.Length + 31) >> 5) << 2;
                fixedMap nullMap = new fixedMap(read);
                fixedMap valueMap = new fixedMap(read += length);
                read += length;
                for (int index = array.Length; --index >= 0; )
                {
                    if (!nullMap.Get(index)) array[index] = valueMap.Get(index);
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected bool[] boolArray()
            {
                object reference = getPoint();
                if (reference != null) return (bool[])reference;
                return boolArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected bool[] boolArrayNoPoint()
            {
                bool[] data = createArray<bool>();
                deSerializeNoPoint(data);
                return data;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected bool?[] boolNullArray()
            {
                object reference = getPoint();
                if (reference != null) return (bool?[])reference;
                return boolNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected bool?[] boolNullArrayNoPoint()
            {
                bool?[] data = createArray<bool?>();
                deSerializeNoPoint(data);
                return data;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(byte?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = *read++;
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected byte?[] byteNullArrayNoPoint()
            {
                byte?[] array = createArray<byte?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(sbyte?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = (sbyte)*read++;
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected sbyte?[] sByteNullArrayNoPoint()
            {
                sbyte?[] array = createArray<sbyte?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(short?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(short*)read;
                        read += sizeof(short);
                    }
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected short?[] shortNullArrayNoPoint()
            {
                short?[] array = createArray<short?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(ushort?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(ushort*)read;
                        read += sizeof(ushort);
                    }
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected ushort?[] uShortNullArrayNoPoint()
            {
                ushort?[] array = createArray<ushort?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(int?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(int*)read;
                        read += sizeof(int);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected int?[] intNullArrayNoPoint()
            {
                int?[] array = createArray<int?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(uint?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(uint*)read;
                        read += sizeof(uint);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected uint?[] uIntNullArrayNoPoint()
            {
                uint?[] array = createArray<uint?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(long?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(long*)read;
                        read += sizeof(long);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected long?[] longNullArrayNoPoint()
            {
                long?[] array = createArray<long?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(ulong?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(ulong*)read;
                        read += sizeof(ulong);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected ulong?[] uLongNullArrayNoPoint()
            {
                ulong?[] array = createArray<ulong?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(char?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(char*)read;
                        read += sizeof(char);
                    }
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected char?[] charNullArrayNoPoint()
            {
                char?[] array = createArray<char?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(DateTime?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(DateTime*)read;
                        read += sizeof(DateTime);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected DateTime?[] dateTimeNullArrayNoPoint()
            {
                DateTime?[] array = createArray<DateTime?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(float?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(float*)read;
                        read += sizeof(float);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected float?[] floatNullArrayNoPoint()
            {
                float?[] array = createArray<float?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(double?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(double*)read;
                        read += sizeof(double);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected double?[] doubleNullArrayNoPoint()
            {
                double?[] array = createArray<double?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(decimal?[] array)
            {
                byte* start = read;
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(decimal*)read;
                        read += sizeof(decimal);
                    }
                }
                int length = (int)(read - start);
                if ((length & 3) != 0) read += -length & 3;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected decimal?[] decimalNullArrayNoPoint()
            {
                decimal?[] array = createArray<decimal?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void nullArrayNoPoint(Guid?[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index))
                    {
                        array[index] = *(Guid*)read;
                        read += sizeof(Guid);
                    }
                }
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected Guid?[] guidNullArrayNoPoint()
            {
                Guid?[] array = createArray<Guid?>();
                nullArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">结构体数组</param>
            /// <param name="size">结构体字节数</param>
            protected void deSerializeNoPoint<valueType>(valueType[] array, int size)
            {
                size *= array.Length;
                Buffer.BlockCopy(data, (int)(read - dataFixed), array, 0, size);
                read += (size + 3) & (int.MaxValue - 3);
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="size">结构体字节数</param>
            /// <returns>结构体数组</returns>
            protected valueType[] getArray<valueType>(int size)
            {
                object reference = getPoint();
                if (reference != null) return (valueType[])reference;
                return getArrayNoPoint<valueType>(size);
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="size">结构体字节数</param>
            /// <returns>结构体数组</returns>
            protected valueType[] getArrayNoPoint<valueType>(int size)
            {
                valueType[] array = createArray<valueType>();
                deSerializeNoPoint(array, size);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void deSerializeNoPoint(DateTime[] array)
            {
                int length = array.Length * sizeof(DateTime);
                fixed (DateTime* dataFixed = array) fastCSharp.unsafer.memory.Copy(read, dataFixed, length);
                read += length;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected DateTime[] dateTimeArray()
            {
                object reference = getPoint();
                if (reference != null) return (DateTime[])reference;
                return dateTimeArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected DateTime[] dateTimeArrayNoPoint()
            {
                DateTime[] array = createArray<DateTime>();
                deSerializeNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void deSerializeNoPoint(decimal[] array)
            {
                int length = array.Length * sizeof(decimal);
                fixed (decimal* dataFixed = array) fastCSharp.unsafer.memory.Copy(read, dataFixed, length);
                read += (length + 3) & (int.MaxValue - 3);
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected decimal[] decimalArray()
            {
                object reference = getPoint();
                if (reference != null) return (decimal[])reference;
                return decimalArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected decimal[] decimalArrayNoPoint()
            {
                decimal[] array = createArray<decimal>();
                deSerializeNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <param name="array">结构体数组</param>
            protected void deSerializeNoPoint(Guid[] array)
            {
                int length = array.Length * sizeof(Guid);
                fixed (Guid* dataFixed = array) fastCSharp.unsafer.memory.Copy(read, dataFixed, length);
                read += (length + 3) & (int.MaxValue - 3);
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected Guid[] guidArray()
            {
                object reference = getPoint();
                if (reference != null) return (Guid[])reference;
                return guidArrayNoPoint();
            }
            /// <summary>
            /// 反序列化结构体数组
            /// </summary>
            /// <returns>结构体数组</returns>
            protected Guid[] guidArrayNoPoint()
            {
                Guid[] array = createArray<Guid>();
                deSerializeNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化字符串数组
            /// </summary>
            /// <param name="array">字符串数组</param>
            protected void stringArrayNoPoint(string[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = getString();
                }
            }
            /// <summary>
            /// 反序列化字符串数组
            /// </summary>
            /// <returns>字符串数组</returns>
            protected string[] stringArray()
            {
                object reference = getPoint();
                if (reference != null) return (string[])reference;
                return stringArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字符串数组
            /// </summary>
            /// <returns>字符串数组</returns>
            protected string[] stringArrayNoPoint()
            {
                string[] array = new string[*(int*)read];
                points.Add(-(int)(read - dataStart), array);
                read += sizeof(int);
                stringArrayNoPoint(array);
                return array;
            }
            /// <summary>
            /// 反序列化未知类型数组
            /// </summary>
            /// <param name="array">数组数据</param>
            protected void unknownArray<valueType>(valueType[] array)
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = unknown<valueType>();
                }
            }
            /// <summary>
            /// 反序列化未知类型数组
            /// </summary>
            /// <returns>数组数据</returns>
            protected valueType[] unknownArray<valueType>()
            {
                valueType[] array = createArray<valueType>();
                unknownArray(array);
                return array;
            }
            /// <summary>
            /// 反序列化值类型数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">值类型数组</param>
            protected void unknownArrayNull<valueType>(Nullable<valueType>[] array) where valueType : struct
            {
                fixedMap nullMap = new fixedMap(read);
                read += ((array.Length + 31) >> 5) << 2;
                for (int index = 0; index != array.Length; ++index)
                {
                    if (!nullMap.Get(index)) array[index] = unknownNotNull<valueType>();
                }
            }
            /// <summary>
            /// 反序列化值类型数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>值类型数组<returns>
            protected Nullable<valueType>[] unknownArrayNull<valueType>() where valueType : struct
            {
                Nullable<valueType>[] array = createArray<Nullable<valueType>>();
                unknownArrayNull(array);
                return array;
            }
            /// <summary>
            /// 反序列化值类型数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">值类型数组</param>
            protected void unknownArrayNotNull<valueType>(valueType[] array)
            {
                for (int index = 0; index != array.Length; ++index) array[index] = unknownNotNull<valueType>();
            }
            /// <summary>
            /// 反序列化值类型数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">值类型数组</param>
            protected valueType[] unknownArrayNotNull<valueType>()
            {
                valueType[] array = createArray<valueType>();
                unknownArrayNotNull(array);
                return array;
            }
            /// <summary>
            /// 反序列化未知类型数据
            /// </summary>
            /// <returns>数组数据</returns>
            protected valueType unknown<valueType>()
            {
                object reference = getPoint();
                if (reference != null) return (valueType)reference;
                return unknownNoPoint<valueType>();
            }
            /// <summary>
            /// 反序列化未知类型数据
            /// </summary>
            /// <returns>数组数据</returns>
            protected valueType unknownNoPoint<valueType>()
            {
                if (*(int*)read == NullValue)
                {
                    int point = (int)(read - dataStart);
                    read += sizeof(int);
                    return serialize.deSerialize<valueType>.GetVersionMemerMap(this, point);
                }
                else
                {
                    fastCSharp.setup.remoteType remoteType = new fastCSharp.setup.remoteType();
                    remoteType.DeSerialize(this);
                    Type type = remoteType.Type;
                    if (typeof(valueType) != typeof(object) && !type.isInherit(typeof(valueType))) log.Default.Throw(type.fullName() + " 不继承 " + typeof(valueType).fullName() + " ,无法反序列化", false, false);
                    return (valueType)unknownValue.GetValue(typeof(valueType), type, ((func<deSerializer, object>)Delegate.CreateDelegate(typeof(func<deSerializer, object>), unknownNotNullMethod.MakeGenericMethod(unknownValue.GetGenericType(type))))(this));
                }
            }
            /// <summary>
            /// 反序列化值类型数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="array">值类型数据</param>
            protected Nullable<valueType> unknownNull<valueType>() where valueType : struct
            {
                return unknownNotNull<valueType>();
            }
            /// <summary>
            /// 反序列化值类型数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            protected valueType unknownNotNull<valueType>()
            {
                return serialize.deSerialize<valueType>.GetVersionMemerMap(this, (int)(read - dataStart));
            }
            /// <summary>
            /// 反序列化值类型数据
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <returns>反序列化数据</returns>
            private static object unknownNotNullObject<valueType>(deSerializer serializer)
            {
                return serializer.unknownNotNull<valueType>();
            }
            /// <summary>
            /// 未知值类型数据序列化 函数信息
            /// </summary>
            private static readonly MethodInfo unknownNotNullMethod = typeof(deSerializer).GetMethod("unknownNotNullObject", BindingFlags.Static | BindingFlags.NonPublic);
        }
        /// <summary>
        /// 对象序列化器(反射模式)
        /// </summary>
        internal class reflectionDataSerializer : dataSerializer
        {
            /// <summary>
            /// 对象序列化器
            /// </summary>
            /// <param name="stream">序列化流</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="memberFilter">成员选择</param>
            public reflectionDataSerializer(memoryStream stream, IMemberMap memberMap, setup.memberFilter memberFilter)
                : base(stream, memberMap, memberFilter)
            {
            }
            /// <summary>
            /// 对象序列化器
            /// </summary>
            /// <param name="parentSerializer">序列化</param>
            /// <param name="memberMap">成员位图接口</param>
            public reflectionDataSerializer(dataSerializer parentSerializer, IMemberMap memberMap = null)
                : base(parentSerializer, memberMap)
            {
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(reflectionDataSerializer serializer, bool value)
            {
                serializer.dataStream.Write(value ? (byte)1 : (byte)0);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(reflectionDataSerializer serializer, bool? value)
            {
                GetBool(serializer, (bool)value);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="array">逻辑值数组</param>
            public unsafe static void GetBoolArray(reflectionDataSerializer serializer, bool[] array)
            {
                serializer.dataStream.Unsafer.PrepSerialize(array);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetByte(reflectionDataSerializer serializer, byte value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetByte(reflectionDataSerializer serializer, byte? value)
            {
                GetByte(serializer, (byte)value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetByteArray(reflectionDataSerializer serializer, byte[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetSByte(reflectionDataSerializer serializer, sbyte value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetSByte(reflectionDataSerializer serializer, sbyte? value)
            {
                GetSByte(serializer, (sbyte)value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetSByteArray(reflectionDataSerializer serializer, sbyte[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(sbyte));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetShort(reflectionDataSerializer serializer, short value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetShort(reflectionDataSerializer serializer, short? value)
            {
                GetShort(serializer, (short)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetShortArray(reflectionDataSerializer serializer, short[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(short));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetUShort(reflectionDataSerializer serializer, ushort value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetUShort(reflectionDataSerializer serializer, ushort? value)
            {
                GetUShort(serializer, (ushort)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUShortArray(reflectionDataSerializer serializer, ushort[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(ushort));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetInt(reflectionDataSerializer serializer, int value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetInt(reflectionDataSerializer serializer, int? value)
            {
                GetInt(serializer, (int)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetIntArray(reflectionDataSerializer serializer, int[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(int));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetUInt(reflectionDataSerializer serializer, uint value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetUInt(reflectionDataSerializer serializer, uint? value)
            {
                GetUInt(serializer, (uint)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUIntArray(reflectionDataSerializer serializer, uint[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(uint));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetLong(reflectionDataSerializer serializer, long value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetLong(reflectionDataSerializer serializer, long? value)
            {
                GetLong(serializer, (long)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetLongArray(reflectionDataSerializer serializer, long[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(long));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetULong(reflectionDataSerializer serializer, ulong value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetULong(reflectionDataSerializer serializer, ulong? value)
            {
                GetULong(serializer, (ulong)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetULongArray(reflectionDataSerializer serializer, ulong[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(ulong));
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            public unsafe static void GetChar(reflectionDataSerializer serializer, char value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            public static void GetChar(reflectionDataSerializer serializer, char? value)
            {
                GetChar(serializer, (char)value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            public static void GetCharArray(reflectionDataSerializer serializer, char[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(char));
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            public static void GetDateTime(reflectionDataSerializer serializer, DateTime value)
            {
                GetLong(serializer, value.Ticks);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            public static void GetDateTime(reflectionDataSerializer serializer, DateTime? value)
            {
                GetLong(serializer, ((DateTime)value).Ticks);
            }
            /// <summary>
            /// 序列化日期值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值数组</param>
            public static void GetDateTimeArray(reflectionDataSerializer serializer, DateTime[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetFloat(reflectionDataSerializer serializer, float value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetFloat(reflectionDataSerializer serializer, float? value)
            {
                GetFloat(serializer, (float)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetFloatArray(reflectionDataSerializer serializer, float[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(float));
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetDouble(reflectionDataSerializer serializer, double value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetDouble(reflectionDataSerializer serializer, double? value)
            {
                GetDouble(serializer, (double)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDoubleArray(reflectionDataSerializer serializer, double[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value, sizeof(double));
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetDecimal(reflectionDataSerializer serializer, decimal value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetDecimal(reflectionDataSerializer serializer, decimal? value)
            {
                GetDecimal(serializer, (decimal)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDecimalArray(reflectionDataSerializer serializer, decimal[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            public unsafe static void GetGuid(reflectionDataSerializer serializer, Guid value)
            {
                serializer.dataStream.Write(value);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            public static void GetGuid(reflectionDataSerializer serializer, Guid? value)
            {
                GetGuid(serializer, (Guid)value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            public unsafe static void GetGuidArray(reflectionDataSerializer serializer, Guid[] value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化字符串
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串</param>
            public unsafe static void GetString(reflectionDataSerializer serializer, string value)
            {
                serializer.dataStream.Unsafer.PrepSerialize(value);
            }
            /// <summary>
            /// 序列化字符串数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="array">字符串数组</param>
            public unsafe static void GetStringArray(reflectionDataSerializer serializer, string[] array)
            {
                serializer.stringArrayNoPoint(array);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullType<valueType>(reflectionDataSerializer serializer, Nullable<valueType> value) where valueType : struct
            {
                serializer.memberMap = default(memberMap<valueType>);
                dataSerialize<valueType>.GetVersionMemerMap(serializer, value.Value);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeType<valueType>(reflectionDataSerializer serializer, valueType value) where valueType : ISerialize
            {
                value.Serialize(serializer);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeTypeObjectNotNull<valueType>(reflectionDataSerializer serializer, object value) where valueType : ISerialize
            {
                ((valueType)value).Serialize(serializer);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeTypeObject<valueType>(reflectionDataSerializer serializer, object value) where valueType : ISerialize
            {
                serializer.iSerializeNoPoint((valueType)value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayNotNull<valueType>(reflectionDataSerializer serializer, valueType[] value) where valueType : ISerialize
            {
                serializer.iSerializeArrayNotNullNoPoint(value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArray<valueType>(reflectionDataSerializer serializer, valueType[] value) where valueType : ISerialize
            {
                serializer.iSerializeArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayNotNullObject<valueType>(reflectionDataSerializer serializer, object value) where valueType : ISerialize
            {
                serializer.iSerializeArrayNotNullNoPoint((valueType[])value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayObject<valueType>(reflectionDataSerializer serializer, object value) where valueType : ISerialize
            {
                serializer.iSerializeArrayNoPoint((valueType[])value);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            public unsafe static void GetBoolNullArray(reflectionDataSerializer serializer, bool?[] array)
            {
                serializer.dataStream.Unsafer.PrepSerialize(array);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            private static void getArrayNull<nullType>(reflectionDataSerializer serializer, Nullable<nullType>[] array)
                where nullType : struct
            {
                serializer.unknownArrayNull<nullType>(serializer, array);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            private unsafe static void getArrayNullObject<nullType>(reflectionDataSerializer serializer, object array)
                where nullType : struct
            {
                serializer.unknownArrayNull<nullType>(serializer, (Nullable<nullType>[])array);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            private unsafe static void getBool(reflectionDataSerializer serializer, object value)
            {
                *(bool*)serializer.write = (bool)value;
                serializer.write += sizeof(bool);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            private unsafe static void getBoolNull(reflectionDataSerializer serializer, object value)
            {
                *(bool*)serializer.write = (bool)(bool?)value;
                serializer.write += sizeof(bool);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getByte(reflectionDataSerializer serializer, object value)
            {
                *(byte*)serializer.write = (byte)value;
                serializer.write += sizeof(byte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getByteNull(reflectionDataSerializer serializer, object value)
            {
                *(byte*)serializer.write = (byte)(byte?)value;
                serializer.write += sizeof(byte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getSByte(reflectionDataSerializer serializer, object value)
            {
                *(sbyte*)serializer.write = (sbyte)value;
                serializer.write += sizeof(sbyte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getSByteNull(reflectionDataSerializer serializer, object value)
            {
                *(sbyte*)serializer.write = (sbyte)(sbyte?)value;
                serializer.write += sizeof(sbyte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getShort(reflectionDataSerializer serializer, object value)
            {
                *(short*)serializer.write = (short)value;
                serializer.write += sizeof(short);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getShortNull(reflectionDataSerializer serializer, object value)
            {
                *(short*)serializer.write = (short)(short?)value;
                serializer.write += sizeof(short);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUShort(reflectionDataSerializer serializer, object value)
            {
                *(ushort*)serializer.write = (ushort)value;
                serializer.write += sizeof(ushort);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUShortNull(reflectionDataSerializer serializer, object value)
            {
                *(ushort*)serializer.write = (ushort)(ushort?)value;
                serializer.write += sizeof(ushort);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getInt(reflectionDataSerializer serializer, object value)
            {
                *(int*)serializer.write = (int)value;
                serializer.write += sizeof(int);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getIntNull(reflectionDataSerializer serializer, object value)
            {
                *(int*)serializer.write = (int)(int?)value;
                serializer.write += sizeof(int);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUInt(reflectionDataSerializer serializer, object value)
            {
                *(uint*)serializer.write = (uint)value;
                serializer.write += sizeof(uint);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUIntNull(reflectionDataSerializer serializer, object value)
            {
                *(uint*)serializer.write = (uint)(uint?)value;
                serializer.write += sizeof(uint);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getLong(reflectionDataSerializer serializer, object value)
            {
                *(long*)serializer.write = (long)value;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getLongNull(reflectionDataSerializer serializer, object value)
            {
                *(long*)serializer.write = (long)(long?)value;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getULong(reflectionDataSerializer serializer, object value)
            {
                *(ulong*)serializer.write = (ulong)value;
                serializer.write += sizeof(ulong);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getULongNull(reflectionDataSerializer serializer, object value)
            {
                *(ulong*)serializer.write = (ulong)(ulong?)value;
                serializer.write += sizeof(ulong);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            private unsafe static void getChar(reflectionDataSerializer serializer, object value)
            {
                *(char*)serializer.write = (char)value;
                serializer.write += sizeof(char);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            private unsafe static void getCharNull(reflectionDataSerializer serializer, object value)
            {
                *(char*)serializer.write = (char)(char?)value;
                serializer.write += sizeof(char);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            private unsafe static void getDateTime(reflectionDataSerializer serializer, object value)
            {
                *(long*)serializer.write = ((DateTime)value).Ticks;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            private unsafe static void getDateTimeNull(reflectionDataSerializer serializer, object value)
            {
                *(long*)serializer.write = ((DateTime)(DateTime?)value).Ticks;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getFloat(reflectionDataSerializer serializer, object value)
            {
                *(float*)serializer.write = (float)value;
                serializer.write += sizeof(float);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getFloatNull(reflectionDataSerializer serializer, object value)
            {
                *(float*)serializer.write = (float)(float?)value;
                serializer.write += sizeof(float);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDouble(reflectionDataSerializer serializer, object value)
            {
                *(double*)serializer.write = (double)value;
                serializer.write += sizeof(double);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDoubleNull(reflectionDataSerializer serializer, object value)
            {
                *(double*)serializer.write = (double)(double?)value;
                serializer.write += sizeof(double);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDecimal(reflectionDataSerializer serializer, object value)
            {
                *(decimal*)serializer.write = (decimal)value;
                serializer.write += sizeof(decimal);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDecimalNull(reflectionDataSerializer serializer, object value)
            {
                *(decimal*)serializer.write = (decimal)(decimal?)value;
                serializer.write += sizeof(decimal);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            private unsafe static void getGuid(reflectionDataSerializer serializer, object value)
            {
                *(Guid*)serializer.write = (Guid)value;
                serializer.write += sizeof(Guid);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            private unsafe static void getGuidNull(reflectionDataSerializer serializer, object value)
            {
                *(Guid*)serializer.write = (Guid)(Guid?)value;
                serializer.write += sizeof(Guid);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullTypeObject<valueType>(reflectionDataSerializer serializer, object value) where valueType : struct
            {
                serializer.memberMap = default(memberMap<valueType>);
                dataSerialize<valueType>.GetVersionMemerMap(serializer, ((Nullable<valueType>)value).Value);
            }
            /// <summary>
            /// 序列化逻辑值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值数组</param>
            private static void getBoolArrayObject(reflectionDataSerializer serializer, object value)
            {
                GetBoolArray(serializer, (bool[])value);
            }
            /// <summary>
            /// 序列化逻辑值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值数组</param>
            private static void getBoolNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                GetBoolNullArray(serializer, (bool?[])value);
            }
            /// <summary>
            /// 字节数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">字节数组</param>
            public static void GetByteNullArray(reflectionDataSerializer serializer, byte?[] array)
            {
                serializer.nullArrayNoPoint(array);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getByteNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((byte?[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getSByteArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((sbyte[])value, sizeof(sbyte));
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetSByteNullArray(reflectionDataSerializer serializer, sbyte?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getSByteNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((sbyte?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getShortArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((short[])value, sizeof(short));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetShortNullArray(reflectionDataSerializer serializer, short?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getShortNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((short?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUShortArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((ushort[])value, sizeof(ushort));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUShortNullArray(reflectionDataSerializer serializer, ushort?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUShortNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((ushort?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getIntArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((int[])value, sizeof(int));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetIntNullArray(reflectionDataSerializer serializer, int?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getIntNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((int?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUIntArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((uint[])value, sizeof(uint));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUIntNullArray(reflectionDataSerializer serializer, uint?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUIntNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((uint?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getLongArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((long[])value, sizeof(long));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetLongNullArray(reflectionDataSerializer serializer, long?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getLongNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((long?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getULongArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((ulong[])value, sizeof(ulong));
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetULongNullArray(reflectionDataSerializer serializer, ulong?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getULongNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((ulong?[])value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            private static void getCharArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((char[])value, sizeof(char));
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            public static void GetCharNullArray(reflectionDataSerializer serializer, char?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            private static void getCharNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((char?[])value);
            }
            /// <summary>
            /// 序列化时间数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">时间数组</param>
            private static void getDateTimeArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((DateTime[])value);
            }
            /// <summary>
            /// 序列化时间数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">时间数组</param>
            public static void GetDateTimeNullArray(reflectionDataSerializer serializer, DateTime?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化时间数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">时间数组</param>
            private static void getDateTimeNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((DateTime?[])value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getFloatArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((float[])value, sizeof(float));
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            public static void GetFloatNullArray(reflectionDataSerializer serializer, float?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getFloatNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((float?[])value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getDoubleArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((double[])value, sizeof(double));
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            public static void GetDoubleNullArray(reflectionDataSerializer serializer, double?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getDoubleNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((double?[])value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getDecimalArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((decimal[])value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            public static void GetDecimalNullArray(reflectionDataSerializer serializer, decimal?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数数组</param>
            private static void getDecimalNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((decimal?[])value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            private unsafe static void getGuidArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.dataStream.Unsafer.PrepSerialize((Guid[])value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            public static void GetGuidNullArray(reflectionDataSerializer serializer, Guid?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            private static void getGuidNullArrayObject(reflectionDataSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((Guid?[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getByteArrayObject(reflectionDataSerializer serializer, object value)
            {
                GetByteArray(serializer, (byte[])value);
            }
            /// <summary>
            /// 序列化字符串
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串</param>
            private static void getStringObject(reflectionDataSerializer serializer, object value)
            {
                GetString(serializer, (string)value);
            }
            /// <summary>
            /// 序列化字符串数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串数组</param>
            private static void getStringArrayObject(reflectionDataSerializer serializer, object value)
            {
                GetStringArray(serializer, (string[])value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArray<valueType>(reflectionDataSerializer serializer, valueType[] value)
            {
                serializer.unknownArray<valueType>(value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayObject<valueType>(reflectionDataSerializer serializer, object value)
            {
                serializer.unknownArray<valueType>((valueType[])value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayNotNull<valueType>(reflectionDataSerializer serializer, valueType[] value)
            {
                serializer.unknownArrayNotNull<valueType>(serializer, value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayNotNullObject<valueType>(reflectionDataSerializer serializer, object value)
            {
                serializer.unknownArrayNotNull<valueType>(serializer, (valueType[])value);
            }
            /// <summary>
            /// 获取 成员对象序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象序列化</returns>
            private static action<reflectionDataSerializer, object> getMemberGetter(Type type)
            {
                memberType memberType = type;
                if (type.isStruct())
                {
                    if (memberType.IsISerialize)
                    {
                        return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeObjectNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    }
                    Type nullType = type.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNull;
                        if (nullType == typeof(byte)) return getByteNull;
                        if (nullType == typeof(sbyte)) return getSByteNull;
                        if (nullType == typeof(short)) return getShortNull;
                        if (nullType == typeof(ushort)) return getUShortNull;
                        if (nullType == typeof(int)) return getIntNull;
                        if (nullType == typeof(uint)) return getUIntNull;
                        if (nullType == typeof(long)) return getLongNull;
                        if (nullType == typeof(ulong)) return getULongNull;
                        if (nullType == typeof(DateTime)) return getDateTimeNull;
                        if (nullType == typeof(char)) return getCharNull;
                        if (nullType == typeof(float)) return getFloatNull;
                        if (nullType == typeof(double)) return getDoubleNull;
                        if (nullType == typeof(decimal)) return getDecimalNull;
                        if (nullType == typeof(Guid)) return getGuidNull;
                        return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getNullTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                    }
                    if (type == typeof(bool)) return getBool;
                    if (type == typeof(byte)) return getByte;
                    if (type == typeof(sbyte)) return getSByte;
                    if (type == typeof(short)) return getShort;
                    if (type == typeof(ushort)) return getUShort;
                    if (type == typeof(int)) return getInt;
                    if (type == typeof(uint)) return getUInt;
                    if (type == typeof(long)) return getLong;
                    if (type == typeof(ulong)) return getULong;
                    if (type == typeof(DateTime)) return getDateTime;
                    if (type == typeof(char)) return getChar;
                    if (type == typeof(float)) return getFloat;
                    if (type == typeof(double)) return getDouble;
                    if (type == typeof(decimal)) return getDecimal;
                    if (type == typeof(Guid)) return getGuid;
                    return getObjectByType(type, true);
                }
                if (memberType.IsISerialize)
                {
                    return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                }
                if (type.IsArray)
                {
                    Type enumerableType = memberType.EnumerableArgumentType;
                    if (((memberType)enumerableType).IsISerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        }
                        return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNullArrayObject;
                        if (nullType == typeof(byte)) return getByteNullArrayObject;
                        if (nullType == typeof(sbyte)) return getSByteNullArrayObject;
                        if (nullType == typeof(short)) return getShortNullArrayObject;
                        if (nullType == typeof(ushort)) return getUShortNullArrayObject;
                        if (nullType == typeof(int)) return getIntNullArrayObject;
                        if (nullType == typeof(uint)) return getUIntNullArrayObject;
                        if (nullType == typeof(long)) return getLongNullArrayObject;
                        if (nullType == typeof(ulong)) return getULongNullArrayObject;
                        if (nullType == typeof(DateTime)) return getDateTimeNullArrayObject;
                        if (nullType == typeof(char)) return getCharNullArrayObject;
                        if (nullType == typeof(float)) return getFloatNullArrayObject;
                        if (nullType == typeof(double)) return getDoubleNullArrayObject;
                        if (nullType == typeof(decimal)) return getDecimalNullArrayObject;
                        if (nullType == typeof(Guid)) return getGuidNullArrayObject;
                        return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getArrayNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    if (enumerableType == typeof(bool)) return getBoolArrayObject;
                    if (enumerableType == typeof(byte)) return getByteArrayObject;
                    if (enumerableType == typeof(sbyte)) return getSByteArrayObject;
                    if (enumerableType == typeof(short)) return getShortArrayObject;
                    if (enumerableType == typeof(ushort)) return getUShortArrayObject;
                    if (enumerableType == typeof(int)) return getIntArrayObject;
                    if (enumerableType == typeof(uint)) return getUIntArrayObject;
                    if (enumerableType == typeof(long)) return getLongArrayObject;
                    if (enumerableType == typeof(ulong)) return getULongArrayObject;
                    if (enumerableType == typeof(DateTime)) return getDateTimeArrayObject;
                    if (enumerableType == typeof(char)) return getCharArrayObject;
                    if (enumerableType == typeof(float)) return getFloatArrayObject;
                    if (enumerableType == typeof(double)) return getDoubleArrayObject;
                    if (enumerableType == typeof(decimal)) return getDecimalArrayObject;
                    if (enumerableType == typeof(Guid)) return getGuidArrayObject;
                    if (enumerableType == typeof(string)) return reflectionDataSerializer.getStringArrayObject;
                    if (enumerableType.isStruct())
                    {
                        return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    return (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod("getArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                }
                if (type == typeof(string)) return reflectionDataSerializer.getStringObject;
                return getObjectByType(type, false);
            }
            /// <summary>
            /// 获取 成员对象序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象序列化</returns>
            public static action<reflectionDataSerializer, object> GetMemberGetter(Type type)
            {
                action<reflectionDataSerializer, object> getMember;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getMemberLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getMemberTypes.TryGetValue(hashType, out getMember)) getMemberTypes.Add(hashType, getMember = getMemberGetter(type));
                }
                finally { getMemberLock = 0; }
                return getMember;
            }
            /// <summary>
            /// 成员对象序列化器访问锁
            /// </summary>
            private static int getMemberLock;
            /// <summary>
            /// 成员对象序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<reflectionDataSerializer, object>> getMemberTypes = new Dictionary<hashCode<Type>, action<reflectionDataSerializer, object>>();
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数据对象</param>
            private static void getObjectNotNull<valueType>(reflectionDataSerializer serializer, object value)
            {
                serializer.memberMap = default(memberMap<valueType>);
                dataSerialize<valueType>.GetVersionMemerMap(serializer, (valueType)value);
            }
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数据对象</param>
            private static void getObject<valueType>(reflectionDataSerializer serializer, object value)
            {
                serializer.unknownNoPoint<valueType>((valueType)value);
            }
            /// <summary>
            /// 获取 未知类型序列化器
            /// </summary>
            /// <param name="type">未知类型</param>
            /// <param name="isStruct">是否值类型</param>
            /// <returns>未知类型序列化器</returns>
            private static action<reflectionDataSerializer, object> getObjectByType(Type type, bool isStruct)
            {
                action<reflectionDataSerializer, object> getObject;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getObjectLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getObjectTypes.TryGetValue(hashType, out getObject))
                    {
                        getObjectTypes.Add(hashType, getObject = (action<reflectionDataSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, object>), typeof(reflectionDataSerializer).GetMethod(isStruct ? "getObjectNotNull" : "getObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type)));
                    }
                }
                finally { getObjectLock = 0; }
                return getObject;
            }
            /// <summary>
            /// 未知类型序列化器访问锁
            /// </summary>
            private static int getObjectLock;
            /// <summary>
            /// 未知类型序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<reflectionDataSerializer, object>> getObjectTypes = new Dictionary<hashCode<Type>, action<reflectionDataSerializer, object>>();
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        public static class dataSerialize
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共字段成员</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get<valueType>(valueType value
                    , setup.memberFilter filter = setup.memberFilter.InstanceField, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return dataSerialize<valueType>.Get(value, filter, memberMap);
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共字段成员</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get(object value, setup.memberFilter filter = setup.memberFilter.InstanceField)
            {
                return value != null ? getter(value.GetType())(value, filter) : BitConverter.GetBytes(NullValue);
            }
            /// <summary>
            /// 获取 对象序列化委托
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns>对象序列化委托</returns>
            private static func<object, setup.memberFilter, byte[]> getter(Type type)
            {
                func<object, setup.memberFilter, byte[]> value;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getterLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getters.TryGetValue(hashType, out value))
                    {
                        getters.Add(hashType, value = (func<object, setup.memberFilter, byte[]>)Delegate.CreateDelegate(typeof(func<object, setup.memberFilter, byte[]>), typeof(dataSerialize<>).MakeGenericType(type).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic)));
                    }
                }
                finally { getterLock = 0; }
                return value;
            }
            /// <summary>
            /// 对象序列化委托 访问锁
            /// </summary>
            private static int getterLock;
            /// <summary>
            /// 对象序列化委托 集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<object, setup.memberFilter, byte[]>> getters = new Dictionary<hashCode<Type>, func<object, setup.memberFilter, byte[]>>();
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal abstract class serializer<valueType>
        {
            /// <summary>
            /// 二次类型转换器
            /// </summary>
            protected struct converter2
            {
                /// <summary>
                /// 第一次类型转换器
                /// </summary>
                public func<object, object> Converter1;
                /// <summary>
                /// 第一次类型转换器
                /// </summary>
                public func<object, object> Converter2;
                /// <summary>
                /// 类型转换
                /// </summary>
                /// <param name="value">原始数据</param>
                /// <returns>目标数据</returns>
                public object Convert(object value)
                {
                    return Converter2(Converter1(value));
                }
            }
            /// <summary>
            /// 是否值类型
            /// </summary>
            protected static readonly bool isStruct;
            /// <summary>
            /// 是否序列化接口类型
            /// </summary>
            protected static readonly bool isISerialize;
            /// <summary>
            /// 是否序列化接口类型
            /// </summary>
            protected static readonly bool isIStreamSerialize;
            /// <summary>
            /// 可空类型的泛型参数类型
            /// </summary>
            protected static readonly Type nullType;
            /// <summary>
            /// 动态成员分组
            /// </summary>
            protected static memberGroup<valueType> memberGroup;
            /// <summary>
            /// 是否允许空值位图
            /// </summary>
            protected static fixedMap isNullMap;
            /// <summary>
            /// 是否值类型序列化位图
            /// </summary>
            protected static fixedMap isMemberSerializeMap;
            /// <summary>
            /// 成员排序
            /// </summary>
            protected static pointer memberSort;
            /// <summary>
            /// 序列化基本字节数
            /// </summary>
            protected internal static int serializeSize;
            /// <summary>
            /// 序列化版本号
            /// </summary>
            protected internal static int version;
            /// <summary>
            /// 是否用于测试用例
            /// </summary>
            protected static bool isTestCase;
            /// <summary>
            /// 所有成员数量
            /// </summary>
            protected static int memberCount;
            /// <summary>
            /// 成员位图字节长度
            /// </summary>
            protected static int memberMapSize;
            /// <summary>
            /// 成员排序
            /// </summary>
            protected static setup.memberInfo[] sortMembers;
            /// <summary>
            /// 初始化 动态成员分组
            /// </summary>
            private unsafe static void setMemberGroup()
            {
                serialize serialize = typeof(valueType).customAttribute<serialize>() ?? serialize.SerializeAttribute;
                version = serialize.Version;
                isTestCase = serialize.IsTestCase;
                memberGroup = memberGroup<valueType>.Create<serialize>(serialize.IsAttribute, serialize.IsBaseTypeAttribute, serialize.IsInheritAttribute
                    , value => value.CanGet & value.CanSet && (serialize.IsObject || value.MemberType.SerializeType.Type != typeof(object))
                        && (serialize.IsInterface || !value.MemberType.SerializeType.Type.IsInterface));
                memberCount = memberGroup<valueType>.MemberCount;
                memberMapSize = ((memberCount + 31) >> 5) << 2;
                sortMembers = memberGroup.Members.sortDesc(value => value.MemberType.SerializeSize);
                pointer[] pointers = unmanaged.Get(true, sortMembers.Length * sizeof(int), memberMapSize, memberMapSize);
                memberSort = pointers[0];
                isMemberSerializeMap = new fixedMap(pointers[1].Byte);
                isNullMap = new fixedMap(pointers[2].Byte);
                int* memberIndex = memberSort.Int;
                foreach (setup.memberInfo member in sortMembers)
                {
                    serializeSize += member.MemberType.SerializeSize;
                    *memberIndex++ = member.MemberIndex;
                    if (member.MemberType.SerializeType.IsNull) isNullMap.Set(member.MemberIndex);
                    if (member.MemberType.IsMemberSerialize) isMemberSerializeMap.Set(member.MemberIndex);
                }
                serializeSize = (serializeSize + 3) & (int.MaxValue - 3);
            }
            /// <summary>
            /// 是否未知类型泛型包装
            /// </summary>
            protected static readonly bool isUnknownValue;
            /// <summary>
            /// 未知类型字段
            /// </summary>
            protected static fieldInfo unknownField;
            /// <summary>
            /// 未知类型是否值类型
            /// </summary>
            protected static bool isUnknownMemberSerialize;
            /// <summary>
            /// 未知类型是否允许空值
            /// </summary>
            protected static bool isUnknownNull;
            /// <summary>
            /// 初始化 未知类型成员
            /// </summary>
            private unsafe static void setUnknownMember()
            {
                unknownField = (fieldInfo)memberGroup<valueType>.GetAllMembers()[0];
                serializeSize = (unknownField.MemberType.SerializeSize + 3) & (int.MaxValue - 3);
                if (unknownField.MemberType.SerializeType.IsNull) isUnknownNull = true;
                if (unknownField.MemberType.IsMemberSerialize) isUnknownMemberSerialize = true;
            }

            static serializer()
            {
                Type type = typeof(valueType);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(unknownValue<>))
                {
                    isUnknownValue = isStruct = true;
                    setUnknownMember();
                }
                else
                {
                    if (type.isStruct()) isStruct = true;
                    memberType memberType = type;
                    if (memberType.IsISerialize) isISerialize = true;
                    if (memberType.IsIStreamSerialize) isIStreamSerialize = true;
                    if (!isISerialize || !isIStreamSerialize)
                    {
                        if (isStruct)
                        {
                            nullType = type.nullableType();
                            if (nullType == null && !memberType.IsMemberSerialize) setMemberGroup();
                        }
                        else if (!type.IsArray && type != typeof(string)) setMemberGroup();
                    }
                }
            }
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="reflectionSerializerType">反射序列化器类型</typeparam>
        internal abstract class serializer<valueType, reflectionSerializerType> : serializer<valueType>
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            protected static action<reflectionSerializerType, valueType> getter;
            /// <summary>
            /// 成员对象序列化 集合
            /// </summary>
            protected static action<reflectionSerializerType, object>[] memberGetters;
            /// <summary>
            /// 成员序列化类型转换器集合
            /// </summary>
            protected static func<object, object>[] converters;
            /// <summary>
            /// 未知类型成员对象序列化
            /// </summary>
            protected static action<reflectionSerializerType, object> unknownMemberGetter;
            /// <summary>
            /// 未知类型成员序列化类型转换器
            /// </summary>
            protected static func<object, object> unknownConverter;
            /// <summary>
            /// 初始化成员序列化器
            /// </summary>
            /// <param name="getter">成员序列化器获取器</param>
            protected static void setMemberGetter(func<Type, action<reflectionSerializerType, object>> getter)
            {
                if (isUnknownValue)
                {
                    keyValue<action<reflectionSerializerType, object>, func<object, object>> memberGetter = getMemberGetter(unknownField, getter);
                    unknownMemberGetter = memberGetter.Key;
                    unknownConverter = memberGetter.Value;
                }
                else
                {
                    memberGetters = new action<reflectionSerializerType, object>[memberCount];
                    converters = new func<object, object>[memberCount];
                    foreach (setup.memberInfo member in sortMembers)
                    {
                        keyValue<action<reflectionSerializerType, object>, func<object, object>> memberGetter = getMemberGetter(member, getter);
                        memberGetters[member.MemberIndex] = memberGetter.Key;
                        converters[member.MemberIndex] = memberGetter.Value;
                    }
                }
            }
            /// <summary>
            /// 获取成员序列化器
            /// </summary>
            /// <param name="member">成员信息</param>
            /// <param name="getter">成员序列化器获取器</param>
            /// <returns>成员序列化器</returns>
            private static keyValue<action<reflectionSerializerType, object>, func<object, object>> getMemberGetter
                (setup.memberInfo member, func<Type, action<reflectionSerializerType, object>> getter)
            {
                action<reflectionSerializerType, object> memberGetter = null;
                func<object, object> converter = null;
                bool isArray = false;
                if (member.MemberType.Type.IsArray)
                {
                    memberType genericType = member.MemberType.Type.GetElementType();
                    if (genericType.SerializeType.Type != genericType.Type)
                    {
                        isArray = true;
                        memberGetter = getter(genericType.SerializeType.Type.MakeArrayType());
                        converter = reflection.converter.GetArray(genericType.Type, genericType.SerializeType.Type);
                    }
                }
                if (!isArray)
                {
                    if (member.MemberType.SerializeType.Type.IsArray)
                    {
                        memberType genericType = member.MemberType.SerializeType.Type.GetElementType();
                        if (genericType.SerializeType.Type != genericType.Type)
                        {
                            isArray = true;
                            memberGetter = getter(genericType.SerializeType.Type.MakeArrayType());
                            converter = new converter2
                            {
                                Converter1 = reflection.converter.Get(member.MemberType.Type, member.MemberType.SerializeType.Type),
                                Converter2 = reflection.converter.GetArray(genericType.Type, genericType.SerializeType.Type)
                            }.Convert;
                        }
                    }
                }
                if (!isArray)
                {
                    memberGetter = getter(member.MemberType.SerializeType);
                    if (member.MemberType.SerializeType.Type != member.MemberType.Type)
                    {
                        converter = reflection.converter.Get(member.MemberType.Type, member.MemberType.SerializeType.Type);
                    }
                }
                return new keyValue<action<reflectionSerializerType, object>, func<object, object>>(memberGetter, converter);
            }
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal class dataSerialize<valueType> : serializer<valueType, reflectionDataSerializer>
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <returns>序列化数据</returns>
            private static byte[] getObject(object value, setup.memberFilter filter)
            {
                return getVersionMemerMap((valueType)value, filter, default(memberMap<valueType>));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                return getVersionMemerMap(value, filter, memberMap);
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            private static byte[] getVersionMemerMap(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                using (memoryStream stream = new memoryStream())
                {
                    reflectionDataSerializer serializer = new reflectionDataSerializer(stream, memberMap, filter);
                    if ((!isStruct || nullType != null) && value == null) stream.Write(NullValue);
                    else
                    {
                        if (!isStruct) serializer.checkPoint(value);
                        if (getter != null) getter(serializer, value);
                        else GetVersionMemerMap(serializer, value);
                    }
                    serializer.Finally();
                    return stream.ToArray();
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            public static void GetVersionMemerMap(reflectionDataSerializer serializer, valueType value)
            {
                if (isUnknownValue) getUnknownMember(serializer, value);
                else
                {
                    serializer.versionMemerMap(version);
                    getMember(serializer, value);
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            private unsafe static void getMember(reflectionDataSerializer serializer, valueType value)
            {
                list<keyValue<setup.memberInfo, object>> memberValues = memberGroup.GetMemberValue(value, serializer.memberFilter, (memberMap<valueType>)serializer.memberMap);
                byte* isValue = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValue, memberMapSize);
                keyValue<object, object>[] values = new keyValue<object, object>[memberCount];
                foreach (keyValue<setup.memberInfo, object> memberValue in memberValues)
                {
                    int memberIndex = memberValue.Key.MemberIndex;
                    func<object, object> converter = converters[memberIndex];
                    isValueMap.Set(memberIndex);
                    values[memberIndex].Set(memberValue.Value, converter == null ? memberValue.Value : converter(memberValue.Value));
                }
                memoryStream dataStream = serializer.dataStream;
                memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                int length = serializer.memberMap.SerializeSize;
                dataStream.PrepLength(length + serializeSize);
                byte[] data = dataStream.Array;
                fixed (byte* dataFixed = data)
                {
                    serializer.write = dataFixed + dataStream.Length;
                    fixedMap nullMap = new fixedMap(serializer.write);
                    fastCSharp.unsafer.memory.Fill(serializer.write, (uint)0, length >> 2);
                    serializer.write += length;
                    for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                    {
                        int memberIndex = *memberIndexStart;
                        if (isValueMap.Get(memberIndex))
                        {
                            object objectValue = values[memberIndex].Value;
                            if (objectValue == null) nullMap.Set(memberIndex);
                            else if (isMemberSerializeMap.Get(memberIndex)) memberGetters[memberIndex](serializer, objectValue);
                        }
                    }
                    unsafeStream.AddLength(((int)(serializer.write - dataFixed - dataStream.Length) + 3) & (int.MaxValue - 3));
                    for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                    {
                        int memberIndex = *memberIndexStart;
                        if (!isMemberSerializeMap.Get(memberIndex) && isValueMap.Get(memberIndex) && !nullMap.Get(memberIndex))
                        {
                            keyValue<object, object> objectValue = values[memberIndex];
                            if (!isNullMap.Get(memberIndex) || serializer.checkPoint(objectValue.Key))
                            {
                                memberGetters[memberIndex](serializer, objectValue.Value);
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            private unsafe static void getUnknownMember(reflectionDataSerializer serializer, valueType value)
            {
                object memberValue = unknownField.Getter(value), converterValue = unknownConverter == null ? memberValue : unknownConverter(memberValue);
                memoryStream dataStream = serializer.dataStream;
                memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                dataStream.PrepLength(sizeof(int) + serializeSize);
                byte[] data = dataStream.Array;
                fixed (byte* dataFixed = data)
                {
                    serializer.write = dataFixed + dataStream.Length;
                    if (converterValue == null)
                    {
                        *(int*)serializer.write = 1;
                        unsafeStream.AddLength(sizeof(int));
                    }
                    else
                    {
                        *(int*)serializer.write = 0;
                        if (isUnknownMemberSerialize)
                        {
                            serializer.write += sizeof(int);
                            unknownMemberGetter(serializer, converterValue);
                            unsafeStream.AddLength(((int)(serializer.write - dataFixed - dataStream.Length) + 3) & (int.MaxValue - 3));
                        }
                        else
                        {
                            unsafeStream.AddLength(sizeof(int));
                            unknownMemberGetter(serializer, converterValue);
                        }
                    }
                }
            }
            static dataSerialize()
            {
                if (isUnknownValue)
                {
                    setMemberGetter(reflectionDataSerializer.GetMemberGetter);
                    return;
                }
                Type type = typeof(valueType);
                if (isISerialize)
                {
                    getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getISerializeType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    return;
                }
                if (isStruct)
                {
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, bool?>)reflectionDataSerializer.GetBool;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, byte?>)reflectionDataSerializer.GetByte;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, sbyte?>)reflectionDataSerializer.GetSByte;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, short?>)reflectionDataSerializer.GetShort;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ushort?>)reflectionDataSerializer.GetUShort;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, int?>)reflectionDataSerializer.GetInt;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, uint?>)reflectionDataSerializer.GetUInt;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, long?>)reflectionDataSerializer.GetLong;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ulong?>)reflectionDataSerializer.GetULong;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, char?>)reflectionDataSerializer.GetChar;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, DateTime?>)reflectionDataSerializer.GetDateTime;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, float?>)reflectionDataSerializer.GetFloat;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, double?>)reflectionDataSerializer.GetDouble;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, decimal?>)reflectionDataSerializer.GetDecimal;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, Guid?>)reflectionDataSerializer.GetGuid;
                            return;
                        }
                        getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getNullType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        return;
                    }
                    if (type == typeof(bool))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, bool>)reflectionDataSerializer.GetBool;
                        return;
                    }
                    if (type == typeof(byte))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, byte>)reflectionDataSerializer.GetByte;
                        return;
                    }
                    if (type == typeof(sbyte))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, sbyte>)reflectionDataSerializer.GetSByte;
                        return;
                    }
                    if (type == typeof(short))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, short>)reflectionDataSerializer.GetShort;
                        return;
                    }
                    if (type == typeof(ushort))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ushort>)reflectionDataSerializer.GetUShort;
                        return;
                    }
                    if (type == typeof(int))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, int>)reflectionDataSerializer.GetInt;
                        return;
                    }
                    if (type == typeof(uint))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, uint>)reflectionDataSerializer.GetUInt;
                        return;
                    }
                    if (type == typeof(long))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, long>)reflectionDataSerializer.GetLong;
                        return;
                    }
                    if (type == typeof(ulong))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ulong>)reflectionDataSerializer.GetULong;
                        return;
                    }
                    if (type == typeof(char))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, char>)reflectionDataSerializer.GetChar;
                        return;
                    }
                    if (type == typeof(DateTime))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, DateTime>)reflectionDataSerializer.GetDateTime;
                        return;
                    }
                    if (type == typeof(float))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, float>)reflectionDataSerializer.GetFloat;
                        return;
                    }
                    if (type == typeof(double))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, double>)reflectionDataSerializer.GetDouble;
                        return;
                    }
                    if (type == typeof(decimal))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, decimal>)reflectionDataSerializer.GetDecimal;
                        return;
                    }
                    if (type == typeof(Guid))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, Guid>)reflectionDataSerializer.GetGuid;
                        return;
                    }
                    setMemberGetter(reflectionDataSerializer.GetMemberGetter);
                    return;
                }
                if (type.IsArray)
                {
                    Type enumerableType = ((memberType)type).EnumerableArgumentType;
                    if (((memberType)enumerableType).IsISerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                            return;
                        }
                        getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getISerializeTypeArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, bool?[]>)reflectionDataSerializer.GetBoolNullArray;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, byte?[]>)reflectionDataSerializer.GetByteNullArray;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, sbyte?[]>)reflectionDataSerializer.GetSByteNullArray;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, short?[]>)reflectionDataSerializer.GetShortNullArray;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ushort?[]>)reflectionDataSerializer.GetUShortNullArray;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, int?[]>)reflectionDataSerializer.GetIntNullArray;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, uint?[]>)reflectionDataSerializer.GetUIntNullArray;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, long?[]>)reflectionDataSerializer.GetLongNullArray;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ulong?[]>)reflectionDataSerializer.GetULongNullArray;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, char?[]>)reflectionDataSerializer.GetCharNullArray;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, DateTime?[]>)reflectionDataSerializer.GetDateTimeNullArray;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, float?[]>)reflectionDataSerializer.GetFloatNullArray;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, double?[]>)reflectionDataSerializer.GetDoubleNullArray;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, decimal?[]>)reflectionDataSerializer.GetDecimalNullArray;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, Guid?[]>)reflectionDataSerializer.GetGuidNullArray;
                            return;
                        }
                        getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getArrayNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    if (enumerableType == typeof(bool))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, bool[]>)reflectionDataSerializer.GetBoolArray;
                        return;
                    }
                    if (enumerableType == typeof(byte))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, byte[]>)reflectionDataSerializer.GetByteArray;
                        return;
                    }
                    if (enumerableType == typeof(sbyte))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, sbyte[]>)reflectionDataSerializer.GetSByteArray;
                        return;
                    }
                    if (enumerableType == typeof(short))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, short[]>)reflectionDataSerializer.GetShortArray;
                        return;
                    }
                    if (enumerableType == typeof(ushort))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ushort[]>)reflectionDataSerializer.GetUShortArray;
                        return;
                    }
                    if (enumerableType == typeof(int))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, int[]>)reflectionDataSerializer.GetIntArray;
                        return;
                    }
                    if (enumerableType == typeof(uint))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, uint[]>)reflectionDataSerializer.GetUIntArray;
                        return;
                    }
                    if (enumerableType == typeof(long))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, long[]>)reflectionDataSerializer.GetLongArray;
                        return;
                    }
                    if (enumerableType == typeof(ulong))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, ulong[]>)reflectionDataSerializer.GetULongArray;
                        return;
                    }
                    if (enumerableType == typeof(char))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, char[]>)reflectionDataSerializer.GetCharArray;
                        return;
                    }
                    if (enumerableType == typeof(DateTime))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, DateTime[]>)reflectionDataSerializer.GetDateTimeArray;
                        return;
                    }
                    if (enumerableType == typeof(float))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, float[]>)reflectionDataSerializer.GetFloatArray;
                        return;
                    }
                    if (enumerableType == typeof(double))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, double[]>)reflectionDataSerializer.GetDoubleArray;
                        return;
                    }
                    if (enumerableType == typeof(decimal))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, decimal[]>)reflectionDataSerializer.GetDecimalArray;
                        return;
                    }
                    if (enumerableType == typeof(Guid))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, Guid[]>)reflectionDataSerializer.GetGuidArray;
                        return;
                    }
                    if (enumerableType == typeof(string))
                    {
                        getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, string[]>)reflectionDataSerializer.GetStringArray;
                        return;
                    }
                    if (enumerableType.isStruct())
                    {
                        getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    getter = (action<reflectionDataSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionDataSerializer, valueType>), typeof(reflectionDataSerializer).GetMethod("getArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    return;
                }
                if (type == typeof(string))
                {
                    getter = (action<reflectionDataSerializer, valueType>)(Delegate)(action<reflectionDataSerializer, string>)reflectionDataSerializer.GetString;
                    return;
                }
                setMemberGetter(reflectionDataSerializer.GetMemberGetter);
            }
        }
        /// <summary>
        /// 对象序列化器(反射模式)
        /// </summary>
        internal class reflectionStreamSerializer : streamSerializer
        {
            /// <summary>
            /// 对象序列化器
            /// </summary>
            /// <param name="stream">序列化流</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="serializeSize">序列化基本长度</param>
            /// <param name="memberFilter">成员选择</param>
            public reflectionStreamSerializer(Stream stream, IMemberMap memberMap, int serializeSize, setup.memberFilter memberFilter)
                : base(stream, memberMap, serializeSize, memberFilter)
            {
            }
            /// <summary>
            /// 对象序列化器
            /// </summary>
            /// <param name="parentSerializer">序列化</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <param name="serializeSize">序列化基本长度</param>
            public reflectionStreamSerializer(streamSerializer parentSerializer, IMemberMap memberMap, int serializeSize)
                :base(parentSerializer, memberMap, serializeSize)
            {
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(reflectionStreamSerializer serializer, bool value)
            {
                serializer.dataStream.WriteByte(value ? (byte)1 : (byte)0);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            public static void GetBool(reflectionStreamSerializer serializer, bool? value)
            {
                GetBool(serializer, (bool)value);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="array">逻辑值数组</param>
            public static void GetBoolArray(reflectionStreamSerializer serializer, bool[] array)
            {
                serializer.serializeNoPoint(array);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetByte(reflectionStreamSerializer serializer, byte value)
            {
                serializer.dataStream.WriteByte(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetByte(reflectionStreamSerializer serializer, byte? value)
            {
                GetByte(serializer, (byte)value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetByteArray(reflectionStreamSerializer serializer, byte[] value)
            {
                serializer.byteArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetByteNullArray(reflectionStreamSerializer serializer, byte?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetSByte(reflectionStreamSerializer serializer, sbyte value)
            {
                serializer.dataStream.WriteByte((byte)value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetSByte(reflectionStreamSerializer serializer, sbyte? value)
            {
                GetSByte(serializer, (sbyte)value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetSByteArray(reflectionStreamSerializer serializer, sbyte[] value)
            {
                serializer.serializeNoPoint(value, sizeof(sbyte) * value.Length);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            public static void GetSByteNullArray(reflectionStreamSerializer serializer, sbyte?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetShort(reflectionStreamSerializer serializer, short value)
            {
                *(short*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(short));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetShort(reflectionStreamSerializer serializer, short? value)
            {
                GetShort(serializer, (short)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetShortArray(reflectionStreamSerializer serializer, short[] value)
            {
                serializer.serializeNoPoint(value, sizeof(short) * value.Length);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetShortNullArray(reflectionStreamSerializer serializer, short?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetUShort(reflectionStreamSerializer serializer, ushort value)
            {
                *(ushort*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(ushort));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetUShort(reflectionStreamSerializer serializer, ushort? value)
            {
                GetUShort(serializer, (ushort)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUShortArray(reflectionStreamSerializer serializer, ushort[] value)
            {
                serializer.serializeNoPoint(value, sizeof(ushort) * value.Length);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUShortNullArray(reflectionStreamSerializer serializer, ushort?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetInt(reflectionStreamSerializer serializer, int value)
            {
                *(int*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(int));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetInt(reflectionStreamSerializer serializer, int? value)
            {
                GetInt(serializer, (int)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetIntArray(reflectionStreamSerializer serializer, int[] value)
            {
                serializer.serializeNoPoint(value, sizeof(int) * value.Length);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetIntNullArray(reflectionStreamSerializer serializer, int?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetUInt(reflectionStreamSerializer serializer, uint value)
            {
                *(uint*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(uint));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetUInt(reflectionStreamSerializer serializer, uint? value)
            {
                GetUInt(serializer, (uint)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUIntNullArray(reflectionStreamSerializer serializer, uint?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetUIntArray(reflectionStreamSerializer serializer, uint[] value)
            {
                serializer.serializeNoPoint(value, sizeof(uint) * value.Length);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetLong(reflectionStreamSerializer serializer, long value)
            {
                *(long*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(long));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetLong(reflectionStreamSerializer serializer, long? value)
            {
                GetLong(serializer, (long)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetLongNullArray(reflectionStreamSerializer serializer, long?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetLongArray(reflectionStreamSerializer serializer, long[] value)
            {
                serializer.serializeNoPoint(value, sizeof(long) * value.Length);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public unsafe static void GetULong(reflectionStreamSerializer serializer, ulong value)
            {
                *(ulong*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(ulong));
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            public static void GetULong(reflectionStreamSerializer serializer, ulong? value)
            {
                GetULong(serializer, (ulong)value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetULongNullArray(reflectionStreamSerializer serializer, ulong?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            public static void GetULongArray(reflectionStreamSerializer serializer, ulong[] value)
            {
                serializer.serializeNoPoint(value, sizeof(ulong) * value.Length);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            public unsafe static void GetChar(reflectionStreamSerializer serializer, char value)
            {
                *(char*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(char));
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            public static void GetChar(reflectionStreamSerializer serializer, char? value)
            {
                GetChar(serializer, (char)value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            public static void GetCharArray(reflectionStreamSerializer serializer, char[] value)
            {
                serializer.serializeNoPoint(value, sizeof(char) * value.Length);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            public static void GetCharNullArray(reflectionStreamSerializer serializer, char?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            public static void GetDateTime(reflectionStreamSerializer serializer, DateTime value)
            {
                GetLong(serializer, value.Ticks);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            public static void GetDateTime(reflectionStreamSerializer serializer, DateTime? value)
            {
                GetLong(serializer, ((DateTime)value).Ticks);
            }
            /// <summary>
            /// 序列化日期值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值数组</param>
            public static void GetDateTimeArray(reflectionStreamSerializer serializer, DateTime[] value)
            {
                serializer.serializeNoPoint(value);
            }
            /// <summary>
            /// 序列化日期值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值数组</param>
            public static void GetDateTimeNullArray(reflectionStreamSerializer serializer, DateTime?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetFloat(reflectionStreamSerializer serializer, float value)
            {
                *(float*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(float));
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetFloat(reflectionStreamSerializer serializer, float? value)
            {
                GetFloat(serializer, (float)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetFloatArray(reflectionStreamSerializer serializer, float[] value)
            {
                serializer.serializeNoPoint(value, sizeof(float) * value.Length);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetFloatNullArray(reflectionStreamSerializer serializer, float?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetDouble(reflectionStreamSerializer serializer, double value)
            {
                *(double*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(double));
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetDouble(reflectionStreamSerializer serializer, double? value)
            {
                GetDouble(serializer, (double)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDoubleArray(reflectionStreamSerializer serializer, double[] value)
            {
                serializer.serializeNoPoint(value, sizeof(double) * value.Length);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDoubleNullArray(reflectionStreamSerializer serializer, double?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public unsafe static void GetDecimal(reflectionStreamSerializer serializer, decimal value)
            {
                *(decimal*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(decimal));
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            public static void GetDecimal(reflectionStreamSerializer serializer, decimal? value)
            {
                GetDecimal(serializer, (decimal)value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDecimalArray(reflectionStreamSerializer serializer, decimal[] value)
            {
                serializer.serializeNoPoint(value);
            }
            /// <summary>
            /// 序列化浮点值数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值数组</param>
            public static void GetDecimalNullArray(reflectionStreamSerializer serializer, decimal?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            public unsafe static void GetGuid(reflectionStreamSerializer serializer, Guid value)
            {
                *(Guid*)serializer.dataPoint = value;
                serializer.dataStream.Write(serializer.valueData, 0, sizeof(Guid));
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            public static void GetGuid(reflectionStreamSerializer serializer, Guid? value)
            {
                GetGuid(serializer, (Guid)value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            public unsafe static void GetGuidArray(reflectionStreamSerializer serializer, Guid[] value)
            {
                serializer.serializeNoPoint(value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            public static void GetGuidNullArray(reflectionStreamSerializer serializer, Guid?[] value)
            {
                serializer.nullArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化字符串
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串</param>
            public unsafe static void GetString(reflectionStreamSerializer serializer, string value)
            {
                serializer.serializeStringNoPoint(value);
            }
            /// <summary>
            /// 序列化字符串数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="array">字符串数组</param>
            public static void GetStringArray(reflectionStreamSerializer serializer, string[] array)
            {
                serializer.stringArrayNoPoint(array);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullType<valueType>(reflectionStreamSerializer serializer, Nullable<valueType> value) where valueType : struct
            {
                serializer.memberMap = default(memberMap<valueType>);
                streamSerialize<valueType>.GetVersionMemerMap(serializer, value.Value);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeType<valueType>(reflectionStreamSerializer serializer, valueType value) where valueType : IStreamSerialize
            {
                value.Serialize(serializer);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeTypeObjectNotNull<valueType>(reflectionStreamSerializer serializer, object value) where valueType : IStreamSerialize
            {
                ((valueType)value).Serialize(serializer);
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">序列化接口数据</param>
            private static void getISerializeTypeObject<valueType>(reflectionStreamSerializer serializer, object value) where valueType : IStreamSerialize
            {
                serializer.iSerializeNoPoint((valueType)value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayNotNull<valueType>(reflectionStreamSerializer serializer, valueType[] value) where valueType : IStreamSerialize
            {
                serializer.iSerializeArrayNotNullNoPoint(value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArray<valueType>(reflectionStreamSerializer serializer, valueType[] value) where valueType : IStreamSerialize
            {
                serializer.iSerializeArrayNoPoint(value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayNotNullObject<valueType>(reflectionStreamSerializer serializer, object value) where valueType : IStreamSerialize
            {
                serializer.iSerializeArrayNotNullNoPoint((valueType[])value);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static void getISerializeTypeArrayObject<valueType>(reflectionStreamSerializer serializer, object value) where valueType : IStreamSerialize
            {
                serializer.iSerializeArrayNoPoint((valueType[])value);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            public unsafe static void GetBoolNullArray(reflectionStreamSerializer serializer, bool?[] array)
            {
                serializer.serializeNoPoint(array);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            private unsafe static void getBool(reflectionStreamSerializer serializer, object value)
            {
                *(bool*)serializer.write = (bool)value;
                serializer.write += sizeof(bool);
            }
            /// <summary>
            /// 序列化逻辑值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">逻辑值</param>
            private unsafe static void getBoolNull(reflectionStreamSerializer serializer, object value)
            {
                *(bool*)serializer.write = (bool)(bool?)value;
                serializer.write += sizeof(bool);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getByte(reflectionStreamSerializer serializer, object value)
            {
                *(byte*)serializer.write = (byte)value;
                serializer.write += sizeof(byte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getByteNull(reflectionStreamSerializer serializer, object value)
            {
                *(byte*)serializer.write = (byte)(byte?)value;
                serializer.write += sizeof(byte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getSByte(reflectionStreamSerializer serializer, object value)
            {
                *(sbyte*)serializer.write = (sbyte)value;
                serializer.write += sizeof(sbyte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getSByteNull(reflectionStreamSerializer serializer, object value)
            {
                *(sbyte*)serializer.write = (sbyte)(sbyte?)value;
                serializer.write += sizeof(sbyte);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getShort(reflectionStreamSerializer serializer, object value)
            {
                *(short*)serializer.write = (short)value;
                serializer.write += sizeof(short);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getShortNull(reflectionStreamSerializer serializer, object value)
            {
                *(short*)serializer.write = (short)(short?)value;
                serializer.write += sizeof(short);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUShort(reflectionStreamSerializer serializer, object value)
            {
                *(ushort*)serializer.write = (ushort)value;
                serializer.write += sizeof(ushort);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUShortNull(reflectionStreamSerializer serializer, object value)
            {
                *(ushort*)serializer.write = (ushort)(ushort?)value;
                serializer.write += sizeof(ushort);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getInt(reflectionStreamSerializer serializer, object value)
            {
                *(int*)serializer.write = (int)value;
                serializer.write += sizeof(int);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getIntNull(reflectionStreamSerializer serializer, object value)
            {
                *(int*)serializer.write = (int)(int?)value;
                serializer.write += sizeof(int);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUInt(reflectionStreamSerializer serializer, object value)
            {
                *(uint*)serializer.write = (uint)value;
                serializer.write += sizeof(uint);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getUIntNull(reflectionStreamSerializer serializer, object value)
            {
                *(uint*)serializer.write = (uint)(uint?)value;
                serializer.write += sizeof(uint);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getLong(reflectionStreamSerializer serializer, object value)
            {
                *(long*)serializer.write = (long)value;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getLongNull(reflectionStreamSerializer serializer, object value)
            {
                *(long*)serializer.write = (long)(long?)value;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getULong(reflectionStreamSerializer serializer, object value)
            {
                *(ulong*)serializer.write = (ulong)value;
                serializer.write += sizeof(ulong);
            }
            /// <summary>
            /// 序列化整数值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数值</param>
            private unsafe static void getULongNull(reflectionStreamSerializer serializer, object value)
            {
                *(ulong*)serializer.write = (ulong)(ulong?)value;
                serializer.write += sizeof(ulong);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            private unsafe static void getChar(reflectionStreamSerializer serializer, object value)
            {
                *(char*)serializer.write = (char)value;
                serializer.write += sizeof(char);
            }
            /// <summary>
            /// 序列化字符
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符</param>
            private unsafe static void getCharNull(reflectionStreamSerializer serializer, object value)
            {
                *(char*)serializer.write = (char)(char?)value;
                serializer.write += sizeof(char);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            private unsafe static void getDateTime(reflectionStreamSerializer serializer, object value)
            {
                *(long*)serializer.write = ((DateTime)value).Ticks;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化日期值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">日期值</param>
            private unsafe static void getDateTimeNull(reflectionStreamSerializer serializer, object value)
            {
                *(long*)serializer.write = ((DateTime)(DateTime?)value).Ticks;
                serializer.write += sizeof(long);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getFloat(reflectionStreamSerializer serializer, object value)
            {
                *(float*)serializer.write = (float)value;
                serializer.write += sizeof(float);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getFloatNull(reflectionStreamSerializer serializer, object value)
            {
                *(float*)serializer.write = (float)(float?)value;
                serializer.write += sizeof(float);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDouble(reflectionStreamSerializer serializer, object value)
            {
                *(double*)serializer.write = (double)value;
                serializer.write += sizeof(double);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDoubleNull(reflectionStreamSerializer serializer, object value)
            {
                *(double*)serializer.write = (double)(double?)value;
                serializer.write += sizeof(double);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDecimal(reflectionStreamSerializer serializer, object value)
            {
                *(decimal*)serializer.write = (decimal)value;
                serializer.write += sizeof(decimal);
            }
            /// <summary>
            /// 序列化浮点值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点值</param>
            private unsafe static void getDecimalNull(reflectionStreamSerializer serializer, object value)
            {
                *(decimal*)serializer.write = (decimal)(decimal?)value;
                serializer.write += sizeof(decimal);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            private unsafe static void getGuid(reflectionStreamSerializer serializer, object value)
            {
                *(Guid*)serializer.write = (Guid)value;
                serializer.write += sizeof(Guid);
            }
            /// <summary>
            /// 序列化Guid值
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid值</param>
            private unsafe static void getGuidNull(reflectionStreamSerializer serializer, object value)
            {
                *(Guid*)serializer.write = (Guid)(Guid?)value;
                serializer.write += sizeof(Guid);
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">可空类型数据</param>
            private static void getNullTypeObject<valueType>(reflectionStreamSerializer serializer, object value) where valueType : struct
            {
                serializer.memberMap = default(memberMap<valueType>);
                streamSerialize<valueType>.GetVersionMemerMap(serializer, ((Nullable<valueType>)value).Value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getBoolArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.serializeNoPoint((bool[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getBoolNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.serializeNoPoint((bool?[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getByteNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((byte?[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getSByteArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetSByteArray(serializer, (sbyte[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getSByteNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((sbyte?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getShortArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetShortArray(serializer, (short[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getShortNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((short?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUShortArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetUShortArray(serializer, (ushort[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUShortNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((ushort?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getIntArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetIntArray(serializer, (int[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getIntNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((int?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUIntArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetUIntArray(serializer, (uint[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getUIntNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((uint?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getLongArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetLongArray(serializer, (long[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getLongNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((long?[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getULongArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetULongArray(serializer, (ulong[])value);
            }
            /// <summary>
            /// 序列化整数数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">整数数组</param>
            private static void getULongNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((ulong?[])value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            private static void getCharArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetCharArray(serializer, (char[])value);
            }
            /// <summary>
            /// 序列化字符数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符数组</param>
            private static void getCharNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((char?[])value);
            }
            /// <summary>
            /// 序列化时间数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">时间数组</param>
            private static void getDateTimeArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetDateTimeArray(serializer, (DateTime[])value);
            }
            /// <summary>
            /// 序列化时间数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">时间数组</param>
            private static void getDateTimeNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((DateTime?[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getFloatArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetFloatArray(serializer, (float[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getFloatNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((float?[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getDoubleArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetDoubleArray(serializer, (double[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getDoubleNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((double?[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getDecimalArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetDecimalArray(serializer, (decimal[])value);
            }
            /// <summary>
            /// 序列化浮点数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">浮点数组</param>
            private static void getDecimalNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((decimal?[])value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            private static void getGuidArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetGuidArray(serializer, (Guid[])value);
            }
            /// <summary>
            /// 序列化Guid数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">Guid数组</param>
            private static void getGuidNullArrayObject(reflectionStreamSerializer serializer, object value)
            {
                serializer.nullArrayNoPoint((Guid?[])value);
            }
            /// <summary>
            /// 序列化字节数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字节数组</param>
            private static void getByteArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetByteArray(serializer, (byte[])value);
            }
            /// <summary>
            /// 序列化字符串
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串</param>
            private static void getStringObject(reflectionStreamSerializer serializer, object value)
            {
                GetString(serializer, (string)value);
            }
            /// <summary>
            /// 序列化字符串数组
            /// </summary>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">字符串数组</param>
            private static void getStringArrayObject(reflectionStreamSerializer serializer, object value)
            {
                GetStringArray(serializer, (string[])value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArray<valueType>(reflectionStreamSerializer serializer, valueType[] value)
            {
                serializer.unknownArray<valueType>(value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayObject<valueType>(reflectionStreamSerializer serializer, object value)
            {
                serializer.unknownArray<valueType>((valueType[])value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayNotNull<valueType>(reflectionStreamSerializer serializer, valueType[] value)
            {
                serializer.unknownArrayNotNull<valueType>(value);
            }
            /// <summary>
            /// 序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组</param>
            private static void getArrayNotNullObject<valueType>(reflectionStreamSerializer serializer, object value)
            {
                serializer.unknownArrayNotNull<valueType>((valueType[])value);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            private static void getArrayNull<nullType>(reflectionStreamSerializer serializer, Nullable<nullType>[] array)
                where nullType : struct
            {
                serializer.unknownArrayNull<nullType>(array);
            }
            /// <summary>
            /// 可空对象数组序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="array">对象数组</param>
            private unsafe static void getArrayNullObject<nullType>(reflectionStreamSerializer serializer, object array)
                where nullType : struct
            {
                serializer.unknownArrayNull<nullType>((Nullable<nullType>[])array);
            }
            /// <summary>
            /// 获取 成员对象序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象序列化</returns>
            private static action<reflectionStreamSerializer, object> getMemberGetter(Type type)
            {
                memberType memberType = type;
                if (type.isStruct())
                {
                    if (memberType.IsIStreamSerialize)
                    {
                        return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeObjectNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    }
                    Type nullType = type.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNull;
                        if (nullType == typeof(byte)) return getByteNull;
                        if (nullType == typeof(sbyte)) return getSByteNull;
                        if (nullType == typeof(short)) return getShortNull;
                        if (nullType == typeof(ushort)) return getUShortNull;
                        if (nullType == typeof(int)) return getIntNull;
                        if (nullType == typeof(uint)) return getUIntNull;
                        if (nullType == typeof(long)) return getLongNull;
                        if (nullType == typeof(ulong)) return getULongNull;
                        if (nullType == typeof(DateTime)) return getDateTimeNull;
                        if (nullType == typeof(char)) return getCharNull;
                        if (nullType == typeof(float)) return getFloatNull;
                        if (nullType == typeof(double)) return getDoubleNull;
                        if (nullType == typeof(decimal)) return getDecimalNull;
                        if (nullType == typeof(Guid)) return getGuidNull;
                        return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getNullTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                    }
                    if (type == typeof(bool)) return getBool;
                    if (type == typeof(byte)) return getByte;
                    if (type == typeof(sbyte)) return getSByte;
                    if (type == typeof(short)) return getShort;
                    if (type == typeof(ushort)) return getUShort;
                    if (type == typeof(int)) return getInt;
                    if (type == typeof(uint)) return getUInt;
                    if (type == typeof(long)) return getLong;
                    if (type == typeof(ulong)) return getULong;
                    if (type == typeof(DateTime)) return getDateTime;
                    if (type == typeof(char)) return getChar;
                    if (type == typeof(float)) return getFloat;
                    if (type == typeof(double)) return getDouble;
                    if (type == typeof(decimal)) return getDecimal;
                    if (type == typeof(Guid)) return getGuid;
                    return getObjectByType(type, true);
                }
                if (memberType.IsIStreamSerialize)
                {
                    return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                }
                if (type.IsArray)
                {
                    Type enumerableType = memberType.EnumerableArgumentType;
                    if (((memberType)enumerableType).IsIStreamSerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        }
                        return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNullArrayObject;
                        if (nullType == typeof(byte)) return getByteNullArrayObject;
                        if (nullType == typeof(sbyte)) return getSByteNullArrayObject;
                        if (nullType == typeof(short)) return getShortNullArrayObject;
                        if (nullType == typeof(ushort)) return getUShortNullArrayObject;
                        if (nullType == typeof(int)) return getIntNullArrayObject;
                        if (nullType == typeof(uint)) return getUIntNullArrayObject;
                        if (nullType == typeof(long)) return getLongNullArrayObject;
                        if (nullType == typeof(ulong)) return getULongNullArrayObject;
                        if (nullType == typeof(DateTime)) return getDateTimeNullArrayObject;
                        if (nullType == typeof(char)) return getCharNullArrayObject;
                        if (nullType == typeof(float)) return getFloatNullArrayObject;
                        if (nullType == typeof(double)) return getDoubleNullArrayObject;
                        if (nullType == typeof(decimal)) return getDecimalNullArrayObject;
                        if (nullType == typeof(Guid)) return getGuidNullArrayObject;
                        return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getArrayNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    if (enumerableType == typeof(bool)) return getBoolArrayObject;
                    if (enumerableType == typeof(byte)) return getByteArrayObject;
                    if (enumerableType == typeof(sbyte)) return getSByteArrayObject;
                    if (enumerableType == typeof(short)) return getShortArrayObject;
                    if (enumerableType == typeof(ushort)) return getUShortArrayObject;
                    if (enumerableType == typeof(int)) return getIntArrayObject;
                    if (enumerableType == typeof(uint)) return getUIntArrayObject;
                    if (enumerableType == typeof(long)) return getLongArrayObject;
                    if (enumerableType == typeof(ulong)) return getULongArrayObject;
                    if (enumerableType == typeof(DateTime)) return getDateTimeArrayObject;
                    if (enumerableType == typeof(char)) return getCharArrayObject;
                    if (enumerableType == typeof(float)) return getFloatArrayObject;
                    if (enumerableType == typeof(double)) return getDoubleArrayObject;
                    if (enumerableType == typeof(decimal)) return getDecimalArrayObject;
                    if (enumerableType == typeof(Guid)) return getGuidArrayObject;
                    if (enumerableType == typeof(string)) return reflectionStreamSerializer.getStringArrayObject;
                    if (enumerableType.isStruct())
                    {
                        return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    return (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod("getArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                }
                if (type == typeof(string)) return reflectionStreamSerializer.getStringObject;
                return getObjectByType(type, false);
            }
            /// <summary>
            /// 获取 成员对象序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象序列化</returns>
            public static action<reflectionStreamSerializer, object> GetMemberGetter(Type type)
            {
                action<reflectionStreamSerializer, object> getMember;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getMemberLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getMemberTypes.TryGetValue(hashType, out getMember)) getMemberTypes.Add(hashType, getMember = getMemberGetter(type));
                }
                finally { getMemberLock = 0; }
                return getMember;
            }
            /// <summary>
            /// 成员对象序列化器访问锁
            /// </summary>
            private static int getMemberLock;
            /// <summary>
            /// 成员对象序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<reflectionStreamSerializer, object>> getMemberTypes = new Dictionary<hashCode<Type>, action<reflectionStreamSerializer, object>>();
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数据对象</param>
            private static void getObjectNotNull<valueType>(reflectionStreamSerializer serializer, object value)
            {
                serializer.memberMap = default(memberMap<valueType>);
                streamSerialize<valueType>.GetVersionMemerMap(serializer, (valueType)value);
            }
            /// <summary>
            /// 未知类型序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数据对象</param>
            private static void getObject<valueType>(reflectionStreamSerializer serializer, object value)
            {
                serializer.unknownNoPoint<valueType>((valueType)value);
            }
            /// <summary>
            /// 获取 未知类型序列化器
            /// </summary>
            /// <param name="type">未知类型</param>
            /// <param name="isStruct">是否值类型</param>
            /// <returns>未知类型序列化器</returns>
            private static action<reflectionStreamSerializer, object> getObjectByType(Type type, bool isStruct)
            {
                action<reflectionStreamSerializer, object> getObject;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getObjectLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getObjectTypes.TryGetValue(hashType, out getObject))
                    {
                        getObjectTypes.Add(hashType, getObject = (action<reflectionStreamSerializer, object>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, object>), typeof(reflectionStreamSerializer).GetMethod(isStruct ? "getObjectNotNull" : "getObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type)));
                    }
                }
                finally { getObjectLock = 0; }
                return getObject;
            }
            /// <summary>
            /// 未知类型序列化器访问锁
            /// </summary>
            private static int getObjectLock;
            /// <summary>
            /// 未知类型序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, action<reflectionStreamSerializer, object>> getObjectTypes = new Dictionary<hashCode<Type>, action<reflectionStreamSerializer, object>>();
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        public static class streamSerialize
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <typeparam name="valueType">对象类型</typeparam>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共字段成员</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get<valueType>(valueType value
                    , setup.memberFilter filter = setup.memberFilter.InstanceField, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return streamSerialize<valueType>.Get(value, filter, memberMap);
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择,默认为公共字段成员</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get(object value, setup.memberFilter filter = setup.memberFilter.InstanceField)
            {
                return value != null ? getter(value.GetType())(value, filter) : BitConverter.GetBytes(NullValue);
            }
            /// <summary>
            /// 获取 对象序列化委托
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns>对象序列化委托</returns>
            private static func<object, setup.memberFilter, byte[]> getter(Type type)
            {
                func<object, setup.memberFilter, byte[]> value;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getterLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getters.TryGetValue(hashType, out value))
                    {
                        getters.Add(hashType, value = (func<object, setup.memberFilter, byte[]>)Delegate.CreateDelegate(typeof(func<object, setup.memberFilter, byte[]>), typeof(streamSerialize<>).MakeGenericType(type).GetMethod("getObject", BindingFlags.Static | BindingFlags.NonPublic)));
                    }
                }
                finally { getterLock = 0; }
                return value;
            }
            /// <summary>
            /// 对象序列化委托 访问锁
            /// </summary>
            private static int getterLock;
            /// <summary>
            /// 对象序列化委托 集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<object, setup.memberFilter, byte[]>> getters = new Dictionary<hashCode<Type>, func<object, setup.memberFilter, byte[]>>();
        }
        /// <summary>
        /// 对象序列化(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal class streamSerialize<valueType> : serializer<valueType, reflectionStreamSerializer>
        {
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <returns>序列化数据</returns>
            private static byte[] getObject(object value, setup.memberFilter filter)
            {
                return getVersionMemerMap((valueType)value, filter, default(memberMap<valueType>));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            public static byte[] Get(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                return getVersionMemerMap(value, filter, memberMap);
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="filter">成员选择</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>序列化数据</returns>
            private unsafe static byte[] getVersionMemerMap(valueType value, setup.memberFilter filter, memberMap<valueType> memberMap)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    reflectionStreamSerializer serializer = new reflectionStreamSerializer(stream, memberMap, serializeSize, filter);
                    fixed (byte* dataFixed = serializer.valueData)
                    {
                        serializer.dataPoint = dataFixed;
                        if ((!isStruct || nullType != null) && value == null) stream.Write(BitConverter.GetBytes(NullValue), 0, sizeof(int));
                        else
                        {
                            if (!isStruct) serializer.checkPoint(value);
                            if (getter != null) getter(serializer, value);
                            else GetVersionMemerMap(serializer, value);
                        }
                        serializer.Finally();
                    }
                    return stream.ToArray();
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            public static void GetVersionMemerMap(reflectionStreamSerializer serializer, valueType value)
            {
                if (isUnknownValue) getUnknownMember(serializer, value);
                else
                {
                    serializer.versionMemerMap(version);
                    getMember(serializer, value);
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            private unsafe static void getMember(reflectionStreamSerializer serializer, valueType value)
            {
                list<keyValue<setup.memberInfo, object>> memberValues = memberGroup.GetMemberValue(value, serializer.memberFilter, (memberMap<valueType>)serializer.memberMap);
                byte* isValue = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValue, memberMapSize);
                keyValue<object, object>[] values = new keyValue<object, object>[memberCount];
                foreach (keyValue<setup.memberInfo, object> memberValue in memberValues)
                {
                    int memberIndex = memberValue.Key.MemberIndex;
                    func<object, object> converter = converters[memberIndex];
                    isValueMap.Set(memberIndex);
                    values[memberIndex].Set(memberValue.Value, converter == null ? memberValue.Value : converter(memberValue.Value));
                }
                int length = serializer.memberMap.SerializeSize;
                byte* dataPoint = serializer.dataPoint;
                fixedMap nullMap = new fixedMap(dataPoint);
                fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                serializer.write = dataPoint + length;
                for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                {
                    int memberIndex = *memberIndexStart;
                    if (isValueMap.Get(memberIndex))
                    {
                        object objectValue = values[memberIndex].Value;
                        if (objectValue == null) nullMap.Set(memberIndex);
                        else if (isMemberSerializeMap.Get(memberIndex)) memberGetters[memberIndex](serializer, objectValue);
                    }
                }
                Stream dataStream = serializer.dataStream;
                byte[] valueData = serializer.valueData;
                if (isTestCase)
                {
                    int writeLength = (int)(serializer.write - dataPoint) & 3;
                    if (writeLength != 0)
                    {
                        while (writeLength++ != 4) *serializer.write++ = 0;
                    }
                }
                dataStream.Write(valueData, 0, ((int)(serializer.write - dataPoint) + 3) & (int.MaxValue - 3));
                byte* mapFixed = stackalloc byte[length];
                fastCSharp.unsafer.memory.Copy(dataPoint, mapFixed, length);
                nullMap = new fixedMap(mapFixed);
                for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                {
                    int memberIndex = *memberIndexStart;
                    if (!isMemberSerializeMap.Get(memberIndex) && isValueMap.Get(memberIndex) && !nullMap.Get(memberIndex))
                    {
                        keyValue<object, object> objectValue = values[memberIndex];
                        if (!isNullMap.Get(memberIndex) || serializer.checkPoint(objectValue.Key))
                        {
                            memberGetters[memberIndex](serializer, objectValue.Value);
                        }
                    }
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            /// <param name="value">数据对象</param>
            private unsafe static void getUnknownMember(reflectionStreamSerializer serializer, valueType value)
            {
                object memberValue = unknownField.Getter(value), converterValue = unknownConverter == null ? memberValue : unknownConverter(memberValue);
                byte* dataPoint = serializer.dataPoint;
                Stream dataStream = serializer.dataStream;
                if (converterValue == null)
                {
                    *(int*)dataPoint = 1;
                    dataStream.Write(serializer.valueData, 0, sizeof(int));
                }
                else
                {
                    *(int*)dataPoint = 0;
                    dataStream.Write(serializer.valueData, 0, sizeof(int));
                    if (isUnknownMemberSerialize)
                    {
                        int length = (int)dataStream.Length;
                        unknownMemberGetter(serializer, converterValue);
                        length -= (int)dataStream.Length;
                        if ((length &= 3) != 0)
                        {
                            *(int*)dataPoint = 0;
                            dataStream.Write(serializer.valueData, 0, length);
                        }
                    }
                    else unknownMemberGetter(serializer, converterValue);
                }
            }
            static streamSerialize()
            {
                if (isUnknownValue)
                {
                    setMemberGetter(reflectionStreamSerializer.GetMemberGetter);
                    return;
                }
                Type type = typeof(valueType);
                if (isIStreamSerialize)
                {
                    getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getISerializeType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    return;
                }
                if (isStruct)
                {
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, bool?>)reflectionStreamSerializer.GetBool;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, byte?>)reflectionStreamSerializer.GetByte;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, sbyte?>)reflectionStreamSerializer.GetSByte;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, short?>)reflectionStreamSerializer.GetShort;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ushort?>)reflectionStreamSerializer.GetUShort;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, int?>)reflectionStreamSerializer.GetInt;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, uint?>)reflectionStreamSerializer.GetUInt;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, long?>)reflectionStreamSerializer.GetLong;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ulong?>)reflectionStreamSerializer.GetULong;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, char?>)reflectionStreamSerializer.GetChar;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, DateTime?>)reflectionStreamSerializer.GetDateTime;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, float?>)reflectionStreamSerializer.GetFloat;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, double?>)reflectionStreamSerializer.GetDouble;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, decimal?>)reflectionStreamSerializer.GetDecimal;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, Guid?>)reflectionStreamSerializer.GetGuid;
                            return;
                        }
                        getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getNullType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        return;
                    }
                    if (type == typeof(bool))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, bool>)reflectionStreamSerializer.GetBool;
                        return;
                    }
                    if (type == typeof(byte))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, byte>)reflectionStreamSerializer.GetByte;
                        return;
                    }
                    if (type == typeof(sbyte))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, sbyte>)reflectionStreamSerializer.GetSByte;
                        return;
                    }
                    if (type == typeof(short))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, short>)reflectionStreamSerializer.GetShort;
                        return;
                    }
                    if (type == typeof(ushort))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ushort>)reflectionStreamSerializer.GetUShort;
                        return;
                    }
                    if (type == typeof(int))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, int>)reflectionStreamSerializer.GetInt;
                        return;
                    }
                    if (type == typeof(uint))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, uint>)reflectionStreamSerializer.GetUInt;
                        return;
                    }
                    if (type == typeof(long))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, long>)reflectionStreamSerializer.GetLong;
                        return;
                    }
                    if (type == typeof(ulong))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ulong>)reflectionStreamSerializer.GetULong;
                        return;
                    }
                    if (type == typeof(char))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, char>)reflectionStreamSerializer.GetChar;
                        return;
                    }
                    if (type == typeof(DateTime))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, DateTime>)reflectionStreamSerializer.GetDateTime;
                        return;
                    }
                    if (type == typeof(float))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, float>)reflectionStreamSerializer.GetFloat;
                        return;
                    }
                    if (type == typeof(double))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, double>)reflectionStreamSerializer.GetDouble;
                        return;
                    }
                    if (type == typeof(decimal))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, decimal>)reflectionStreamSerializer.GetDecimal;
                        return;
                    }
                    if (type == typeof(Guid))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, Guid>)reflectionStreamSerializer.GetGuid;
                        return;
                    }
                    setMemberGetter(reflectionStreamSerializer.GetMemberGetter);
                    return;
                }

                if (type.IsArray)
                {
                    Type enumerableType = ((memberType)type).EnumerableArgumentType;
                    if (((memberType)enumerableType).IsIStreamSerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                            return;
                        }
                        getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getISerializeTypeArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, bool?[]>)reflectionStreamSerializer.GetBoolNullArray;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, byte?[]>)reflectionStreamSerializer.GetByteNullArray;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, sbyte?[]>)reflectionStreamSerializer.GetSByteNullArray;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, short?[]>)reflectionStreamSerializer.GetShortNullArray;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ushort?[]>)reflectionStreamSerializer.GetUShortNullArray;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, int?[]>)reflectionStreamSerializer.GetIntNullArray;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, uint?[]>)reflectionStreamSerializer.GetUIntNullArray;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, long?[]>)reflectionStreamSerializer.GetLongNullArray;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ulong?[]>)reflectionStreamSerializer.GetULongNullArray;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, char?[]>)reflectionStreamSerializer.GetCharNullArray;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, DateTime?[]>)reflectionStreamSerializer.GetDateTimeNullArray;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, float?[]>)reflectionStreamSerializer.GetFloatNullArray;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, double?[]>)reflectionStreamSerializer.GetDoubleNullArray;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, decimal?[]>)reflectionStreamSerializer.GetDecimalNullArray;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, Guid?[]>)reflectionStreamSerializer.GetGuidNullArray;
                            return;
                        }
                        getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getArrayNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    if (enumerableType == typeof(bool))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, bool[]>)reflectionStreamSerializer.GetBoolArray;
                        return;
                    }
                    if (enumerableType == typeof(byte))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, byte[]>)reflectionStreamSerializer.GetByteArray;
                        return;
                    }
                    if (enumerableType == typeof(sbyte))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, sbyte[]>)reflectionStreamSerializer.GetSByteArray;
                        return;
                    }
                    if (enumerableType == typeof(short))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, short[]>)reflectionStreamSerializer.GetShortArray;
                        return;
                    }
                    if (enumerableType == typeof(ushort))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ushort[]>)reflectionStreamSerializer.GetUShortArray;
                        return;
                    }
                    if (enumerableType == typeof(int))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, int[]>)reflectionStreamSerializer.GetIntArray;
                        return;
                    }
                    if (enumerableType == typeof(uint))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, uint[]>)reflectionStreamSerializer.GetUIntArray;
                        return;
                    }
                    if (enumerableType == typeof(long))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, long[]>)reflectionStreamSerializer.GetLongArray;
                        return;
                    }
                    if (enumerableType == typeof(ulong))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, ulong[]>)reflectionStreamSerializer.GetULongArray;
                        return;
                    }
                    if (enumerableType == typeof(char))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, char[]>)reflectionStreamSerializer.GetCharArray;
                        return;
                    }
                    if (enumerableType == typeof(DateTime))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, DateTime[]>)reflectionStreamSerializer.GetDateTimeArray;
                        return;
                    }
                    if (enumerableType == typeof(float))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, float[]>)reflectionStreamSerializer.GetFloatArray;
                        return;
                    }
                    if (enumerableType == typeof(double))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, double[]>)reflectionStreamSerializer.GetDoubleArray;
                        return;
                    }
                    if (enumerableType == typeof(decimal))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, decimal[]>)reflectionStreamSerializer.GetDecimalArray;
                        return;
                    }
                    if (enumerableType == typeof(Guid))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, Guid[]>)reflectionStreamSerializer.GetGuidArray;
                        return;
                    }
                    if (enumerableType == typeof(string))
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, string[]>)reflectionStreamSerializer.GetStringArray;
                        return;
                    }
                    if (enumerableType.isStruct())
                    {
                        getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    getter = (action<reflectionStreamSerializer, valueType>)Delegate.CreateDelegate(typeof(action<reflectionStreamSerializer, valueType>), typeof(reflectionStreamSerializer).GetMethod("getArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    return;
                }
                if (type == typeof(string))
                {
                    getter = (action<reflectionStreamSerializer, valueType>)(Delegate)(action<reflectionStreamSerializer, string>)reflectionStreamSerializer.GetString;
                    return;
                }
                setMemberGetter(reflectionStreamSerializer.GetMemberGetter);
            }
        }
        /// <summary>
        /// 对象反序列化器(反射模式)
        /// </summary>
        internal abstract unsafe class reflectionDeSerializer : deSerializer
        {
            /// <summary>
            /// 版本号
            /// </summary>
            protected int version;
            /// <summary>
            /// 对象反序列化器
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="dataFixed">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="version">版本号</param>
            protected reflectionDeSerializer(byte[] data, byte* dataFixed, int startIndex, int version)
                : base(data, dataFixed, startIndex, serializeVersion.serialize)
            {
                this.version = version;
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="version">版本号</param>
            protected reflectionDeSerializer(deSerializer parentDeSerializer, int version)
                : base(parentDeSerializer)
            {
                this.version = version;
            }
            /// <summary>
            /// 反序列化逻辑值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值</returns>
            public unsafe static bool GetBool(reflectionDeSerializer serializer)
            {
                return *serializer.read++ != 0;
            }
            /// <summary>
            /// 反序列化逻辑值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值</returns>
            public unsafe static bool? GetBoolNull(reflectionDeSerializer serializer)
            {
                return *serializer.read++ != 0;
            }
            /// <summary>
            /// 反序列化逻辑值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值数组</returns>
            public unsafe static bool[] GetBoolArray(reflectionDeSerializer serializer)
            {
                return serializer.boolArrayNoPoint();
            }
            /// <summary>
            /// 可空对象数组反序列化
            /// </summary>
            /// <param name="serializer">对象反序列化器</param>
            /// <returns>可空对象数组</returns>
            public unsafe static bool?[] GetBoolNullArray(reflectionDeSerializer serializer)
            {
                return serializer.boolNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static byte GetByte(reflectionDeSerializer serializer)
            {
                return *serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static byte? GetByteNull(reflectionDeSerializer serializer)
            {
                return *serializer.read++;
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            public static byte[] GetByteArray(reflectionDeSerializer serializer)
            {
                return serializer.byteArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            public static byte?[] GetByteNullArray(reflectionDeSerializer serializer)
            {
                return serializer.byteNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static sbyte GetSByte(reflectionDeSerializer serializer)
            {
                return (sbyte)*serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static sbyte? GetSByteNull(reflectionDeSerializer serializer)
            {
                return (sbyte?)(sbyte)*serializer.read++;
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            public static sbyte[] GetSByteArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<sbyte>(sizeof(sbyte));
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            public static sbyte?[] GetSByteNullArray(reflectionDeSerializer serializer)
            {
                return serializer.sByteNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static short GetShort(reflectionDeSerializer serializer)
            {
                short value = *(short*)serializer.read;
                serializer.read += sizeof(short);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static short? GetShortNull(reflectionDeSerializer serializer)
            {
                short value = *(short*)serializer.read;
                serializer.read += sizeof(short);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static short[] GetShortArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<short>(sizeof(short));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static short?[] GetShortNullArray(reflectionDeSerializer serializer)
            {
                return serializer.shortNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static ushort GetUShort(reflectionDeSerializer serializer)
            {
                ushort value = *(ushort*)serializer.read;
                serializer.read += sizeof(ushort);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static ushort? GetUShortNull(reflectionDeSerializer serializer)
            {
                ushort value = *(ushort*)serializer.read;
                serializer.read += sizeof(ushort);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static ushort[] GetUShortArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<ushort>(sizeof(ushort));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static ushort?[] GetUShortNullArray(reflectionDeSerializer serializer)
            {
                return serializer.uShortNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static int GetInt(reflectionDeSerializer serializer)
            {
                int value = *(int*)serializer.read;
                serializer.read += sizeof(int);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static int? GetIntNull(reflectionDeSerializer serializer)
            {
                int value = *(int*)serializer.read;
                serializer.read += sizeof(int);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static int[] GetIntArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<int>(sizeof(int));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static int?[] GetIntNullArray(reflectionDeSerializer serializer)
            {
                return serializer.intNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static uint GetUInt(reflectionDeSerializer serializer)
            {
                uint value = *(uint*)serializer.read;
                serializer.read += sizeof(uint);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static uint? GetUIntNull(reflectionDeSerializer serializer)
            {
                uint value = *(uint*)serializer.read;
                serializer.read += sizeof(uint);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static uint[] GetUIntArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<uint>(sizeof(uint));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static uint?[] GetUIntNullArray(reflectionDeSerializer serializer)
            {
                return serializer.uIntNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static long GetLong(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static long? GetLongNull(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static long[] GetLongArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<long>(sizeof(long));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static long?[] GetLongNullArray(reflectionDeSerializer serializer)
            {
                return serializer.longNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static ulong GetULong(reflectionDeSerializer serializer)
            {
                ulong value = *(ulong*)serializer.read;
                serializer.read += sizeof(ulong);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            public unsafe static ulong? GetULongNull(reflectionDeSerializer serializer)
            {
                ulong value = *(ulong*)serializer.read;
                serializer.read += sizeof(ulong);
                return value;
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static ulong[] GetULongArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<ulong>(sizeof(ulong));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            public static ulong?[] GetULongNullArray(reflectionDeSerializer serializer)
            {
                return serializer.uLongNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字符
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符</returns>
            public unsafe static char GetChar(reflectionDeSerializer serializer)
            {
                char value = *(char*)serializer.read;
                serializer.read += sizeof(char);
                return value;
            }
            /// <summary>
            /// 反序列化字符
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符</returns>
            public unsafe static char? GetCharNull(reflectionDeSerializer serializer)
            {
                char value = *(char*)serializer.read;
                serializer.read += sizeof(char);
                return (char?)value;
            }
            /// <summary>
            /// 反序列化字符数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符数组</returns>
            public static char[] GetCharArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<char>(sizeof(char));
            }
            /// <summary>
            /// 反序列化字符数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符数组</returns>
            public static char?[] GetCharNullArray(reflectionDeSerializer serializer)
            {
                return serializer.charNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化日期值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值</returns>
            public unsafe static DateTime GetDateTime(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return new DateTime(value);
            }
            /// <summary>
            /// 反序列化日期值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值</returns>
            public unsafe static DateTime? GetDateTimeNull(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return new DateTime(value);
            }
            /// <summary>
            /// 反序列化日期值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值数组</returns>
            public static DateTime[] GetDateTimeArray(reflectionDeSerializer serializer)
            {
                return serializer.dateTimeArrayNoPoint();
            }
            /// <summary>
            /// 反序列化日期值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值数组</returns>
            public static DateTime?[] GetDateTimeNullArray(reflectionDeSerializer serializer)
            {
                return serializer.dateTimeNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static float GetFloat(reflectionDeSerializer serializer)
            {
                float value = *(float*)serializer.read;
                serializer.read += sizeof(float);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static float? GetFloatNull(reflectionDeSerializer serializer)
            {
                float value = *(float*)serializer.read;
                serializer.read += sizeof(float);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static float[] GetFloatArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<float>(sizeof(float));
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static float?[] GetFloatNullArray(reflectionDeSerializer serializer)
            {
                return serializer.floatNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static double GetDouble(reflectionDeSerializer serializer)
            {
                double value = *(double*)serializer.read;
                serializer.read += sizeof(double);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static double? GetDoubleNull(reflectionDeSerializer serializer)
            {
                double value = *(double*)serializer.read;
                serializer.read += sizeof(double);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static double[] GetDoubleArray(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<double>(sizeof(double));
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static double?[] GetDoubleNullArray(reflectionDeSerializer serializer)
            {
                return serializer.doubleNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static decimal GetDecimal(reflectionDeSerializer serializer)
            {
                decimal value = *(decimal*)serializer.read;
                serializer.read += sizeof(decimal);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            public unsafe static decimal? GetDecimalNull(reflectionDeSerializer serializer)
            {
                decimal value = *(decimal*)serializer.read;
                serializer.read += sizeof(decimal);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static decimal[] GetDecimalArray(reflectionDeSerializer serializer)
            {
                return serializer.decimalArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static decimal?[] GetDecimalNullArray(reflectionDeSerializer serializer)
            {
                return serializer.decimalNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化Guid值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid值</returns>
            public unsafe static Guid GetGuid(reflectionDeSerializer serializer)
            {
                Guid value = *(Guid*)serializer.read;
                serializer.read += sizeof(Guid);
                return value;
            }
            /// <summary>
            /// 反序列化Guid值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid值</returns>
            public unsafe static Guid? GetGuidNull(reflectionDeSerializer serializer)
            {
                Guid value = *(Guid*)serializer.read;
                serializer.read += sizeof(Guid);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static Guid[] GetGuidArray(reflectionDeSerializer serializer)
            {
                return serializer.guidArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            public static Guid?[] GetGuidNullArray(reflectionDeSerializer serializer)
            {
                return serializer.guidNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字符串
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <param name="value">字符串</param>
            public unsafe static string GetString(reflectionDeSerializer serializer)
            {
                return serializer.getStringNoPoint();
            }
            /// <summary>
            /// 反序列化字符串数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符串数组</returns>
            public static string[] GetStringArray(reflectionDeSerializer serializer)
            {
                return serializer.stringArrayNoPoint();
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>可空类型数据</returns>
            private static Nullable<valueType> getNullType<valueType>(reflectionDeSerializer serializer) where valueType : struct
            {
                return serializer.unknownNull<valueType>();
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <returns>序列化接口数据</returns>
            private static valueType getISerializeType<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                valueType value = constructor<valueType>.New;
                value.DeSerialize(serializer);
                return value;
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <returns>序列化接口数据</returns>
            private static object getISerializeTypeObjectNotNull<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                valueType value = constructor<valueType>.New;
                value.DeSerialize(serializer);
                return value;
            }
            /// <summary>
            /// 序列化接口
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <returns>序列化接口数据</returns>
            private static object getISerializeTypeObject<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                return serializer.iSerializeNoPoint<valueType>(null);
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <returns>数组数据</returns>
            private static valueType[] getISerializeTypeArrayNotNull<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                return serializer.iSerializeArrayNotNullNoPoint<valueType>();
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static valueType[] getISerializeTypeArray<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                return serializer.iSerializeArrayNoPoint<valueType>();
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <returns>数组数据</returns>
            private static object getISerializeTypeArrayNotNullObject<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                return serializer.iSerializeArrayNotNullNoPoint<valueType>();
            }
            /// <summary>
            /// 序列化接口数组
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">序列化器</param>
            /// <param name="value">数组数据</param>
            private static object getISerializeTypeArrayObject<valueType>(reflectionDeSerializer serializer) where valueType : ISerialize
            {
                return serializer.iSerializeArrayNoPoint<valueType>();
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static valueType[] getArray<valueType>(reflectionDeSerializer serializer)
            {
                return serializer.unknownArray<valueType>();
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static Nullable<valueType>[] getArrayNull<valueType>(reflectionDeSerializer serializer) where valueType : struct
            {
                return serializer.unknownArrayNull<valueType>();
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static valueType[] getArrayNotNull<valueType>(reflectionDeSerializer serializer)
            {
                return serializer.unknownArrayNotNull<valueType>();
            }
            /// <summary>
            /// 反序列化逻辑值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值</returns>
            private unsafe static object getBool(reflectionDeSerializer serializer)
            {
                return *serializer.read++ != 0;
            }
            /// <summary>
            /// 反序列化逻辑值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值</returns>
            private unsafe static object getBoolNull(reflectionDeSerializer serializer)
            {
                return (bool?)(*serializer.read++ != 0);
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getByte(reflectionDeSerializer serializer)
            {
                return *serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getByteNull(reflectionDeSerializer serializer)
            {
                return (byte?)*serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getSByte(reflectionDeSerializer serializer)
            {
                return (sbyte)*serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getSByteNull(reflectionDeSerializer serializer)
            {
                return (sbyte?)(sbyte)*serializer.read++;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getShort(reflectionDeSerializer serializer)
            {
                short value = *(short*)serializer.read;
                serializer.read += sizeof(short);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getShortNull(reflectionDeSerializer serializer)
            {
                short value = *(short*)serializer.read;
                serializer.read += sizeof(short);
                return (short?)value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getUShort(reflectionDeSerializer serializer)
            {
                ushort value = *(ushort*)serializer.read;
                serializer.read += sizeof(ushort);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getUShortNull(reflectionDeSerializer serializer)
            {
                ushort value = *(ushort*)serializer.read;
                serializer.read += sizeof(ushort);
                return (ushort?)value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getInt(reflectionDeSerializer serializer)
            {
                int value = *(int*)serializer.read;
                serializer.read += sizeof(int);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getIntNull(reflectionDeSerializer serializer)
            {
                int value = *(int*)serializer.read;
                serializer.read += sizeof(int);
                return (int?)value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getUInt(reflectionDeSerializer serializer)
            {
                uint value = *(uint*)serializer.read;
                serializer.read += sizeof(uint);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getUIntNull(reflectionDeSerializer serializer)
            {
                uint value = *(uint*)serializer.read;
                serializer.read += sizeof(uint);
                return (uint?)value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getLong(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getLongNull(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return (long?)value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getULong(reflectionDeSerializer serializer)
            {
                ulong value = *(ulong*)serializer.read;
                serializer.read += sizeof(ulong);
                return value;
            }
            /// <summary>
            /// 反序列化整数值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数值</returns>
            private unsafe static object getULongNull(reflectionDeSerializer serializer)
            {
                ulong value = *(ulong*)serializer.read;
                serializer.read += sizeof(ulong);
                return (ulong?)value;
            }
            /// <summary>
            /// 反序列化字符
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符</returns>
            private unsafe static object getChar(reflectionDeSerializer serializer)
            {
                char value = *(char*)serializer.read;
                serializer.read += sizeof(char);
                return value;
            }
            /// <summary>
            /// 反序列化字符
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符</returns>
            private unsafe static object getCharNull(reflectionDeSerializer serializer)
            {
                char value = *(char*)serializer.read;
                serializer.read += sizeof(char);
                return (char?)value;
            }
            /// <summary>
            /// 反序列化日期值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值</returns>
            private unsafe static object getDateTime(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return new DateTime(value);
            }
            /// <summary>
            /// 反序列化日期值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>日期值</returns>
            private unsafe static object getDateTimeNull(reflectionDeSerializer serializer)
            {
                long value = *(long*)serializer.read;
                serializer.read += sizeof(long);
                return (DateTime?)new DateTime(value);
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getFloat(reflectionDeSerializer serializer)
            {
                float value = *(float*)serializer.read;
                serializer.read += sizeof(float);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getFloatNull(reflectionDeSerializer serializer)
            {
                float value = *(float*)serializer.read;
                serializer.read += sizeof(float);
                return (float?)value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getDouble(reflectionDeSerializer serializer)
            {
                double value = *(double*)serializer.read;
                serializer.read += sizeof(double);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getDoubleNull(reflectionDeSerializer serializer)
            {
                double value = *(double*)serializer.read;
                serializer.read += sizeof(double);
                return (double?)value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getDecimal(reflectionDeSerializer serializer)
            {
                decimal value = *(decimal*)serializer.read;
                serializer.read += sizeof(decimal);
                return value;
            }
            /// <summary>
            /// 反序列化浮点值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值</returns>
            private unsafe static object getDecimalNull(reflectionDeSerializer serializer)
            {
                decimal value = *(decimal*)serializer.read;
                serializer.read += sizeof(decimal);
                return (decimal?)value;
            }
            /// <summary>
            /// 反序列化Guid值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid值</returns>
            private unsafe static object getGuid(reflectionDeSerializer serializer)
            {
                Guid value = *(Guid*)serializer.read;
                serializer.read += sizeof(Guid);
                return value;
            }
            /// <summary>
            /// 反序列化Guid值
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid值</returns>
            private unsafe static object getGuidNull(reflectionDeSerializer serializer)
            {
                Guid value = *(Guid*)serializer.read;
                serializer.read += sizeof(Guid);
                return (Guid?)value;
            }
            /// <summary>
            /// 可空类型
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>可空类型数据</returns>
            private static object getNullTypeObject<valueType>(reflectionDeSerializer serializer) where valueType : struct
            {
                return (Nullable<valueType>)(serialize.deSerialize<valueType>.GetVersionMemerMap(serializer, (int)(serializer.read - serializer.dataStart)));
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            private static object getSByteArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<sbyte>(sizeof(sbyte));
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            private static object getSByteNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.sByteNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getShortArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<short>(sizeof(short));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getShortNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.shortNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getUShortArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<ushort>(sizeof(ushort));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getUShortNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.uShortNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getIntArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<int>(sizeof(int));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getIntNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.intNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getUIntArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<uint>(sizeof(uint));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getUIntNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.uIntNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getLongArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<long>(sizeof(long));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getLongNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.longNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getULongArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<ulong>(sizeof(ulong));
            }
            /// <summary>
            /// 反序列化整数数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>整数数组</returns>
            private static object getULongNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.uLongNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字符数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符数组</returns>
            private static object getCharArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<char>(sizeof(char));
            }
            /// <summary>
            /// 反序列化字符数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符数组</returns>
            private static object getCharNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.charNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化时间数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>时间数组</returns>
            private static object getDateTimeArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.dateTimeArrayNoPoint();
            }
            /// <summary>
            /// 反序列化时间数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>时间数组</returns>
            private static object getDateTimeNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.dateTimeNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getFloatArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<float>(sizeof(float));
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getFloatNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.floatNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getDoubleArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.getArrayNoPoint<double>(sizeof(double));
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getDoubleNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.doubleNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getDecimalArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.decimalArrayNoPoint();
            }
            /// <summary>
            /// 反序列化浮点值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>浮点值数组</returns>
            private static object getDecimalNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.decimalNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化Guid数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid数组</returns>
            private static object getGuidArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.guidArrayNoPoint();
            }
            /// <summary>
            /// 反序列化Guid数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>Guid数组</returns>
            private static object getGuidNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.guidNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化逻辑值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值数组</returns>
            private static object getBoolArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.boolArrayNoPoint();
            }
            /// <summary>
            /// 反序列化逻辑值数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>逻辑值数组</returns>
            private static object getBoolNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.boolNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            private static object getByteArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.byteArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字节数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字节数组</returns>
            private static object getByteNullArrayObject(reflectionDeSerializer serializer)
            {
                return serializer.byteNullArrayNoPoint();
            }
            /// <summary>
            /// 反序列化字符串
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符串</returns>
            private static object getStringObject(reflectionDeSerializer serializer)
            {
                return serializer.getStringNoPoint();
            }
            /// <summary>
            /// 反序列化字符串数组
            /// </summary>
            /// <param name="serializer">反序列化器</param>
            /// <returns>字符串数组</returns>
            private static object getStringArrayObject(reflectionDeSerializer serializer)
            {
                return GetStringArray(serializer);
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static object getArrayObject<valueType>(reflectionDeSerializer serializer)
            {
                return serializer.unknownArray<valueType>();
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static object getArrayNullObject<valueType>(reflectionDeSerializer serializer) where valueType : struct
            {
                return serializer.unknownArrayNull<valueType>();
            }
            /// <summary>
            /// 反序列化数组
            /// </summary>
            /// <typeparam name="valueType">数组数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数组</returns>
            private static object getArrayNotNullObject<valueType>(reflectionDeSerializer serializer)
            {
                return serializer.unknownArrayNotNull<valueType>();
            }
            /// <summary>
            /// 获取 成员对象反序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象反序列化</returns>
            private static func<reflectionDeSerializer, object> getMemberGetter(Type type)
            {
                memberType memberType = type;
                if (type.isStruct())
                {
                    if (memberType.IsISerialize)
                    {
                        return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeObjectNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    }
                    Type nullType = type.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNull;
                        if (nullType == typeof(byte)) return getByteNull;
                        if (nullType == typeof(sbyte)) return getSByteNull;
                        if (nullType == typeof(short)) return getShortNull;
                        if (nullType == typeof(ushort)) return getUShortNull;
                        if (nullType == typeof(int)) return getIntNull;
                        if (nullType == typeof(uint)) return getUIntNull;
                        if (nullType == typeof(long)) return getLongNull;
                        if (nullType == typeof(ulong)) return getULongNull;
                        if (nullType == typeof(DateTime)) return getDateTimeNull;
                        if (nullType == typeof(char)) return getCharNull;
                        if (nullType == typeof(float)) return getFloatNull;
                        if (nullType == typeof(double)) return getDoubleNull;
                        if (nullType == typeof(decimal)) return getDecimalNull;
                        if (nullType == typeof(Guid)) return getGuidNull;
                        return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getNullTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                    }
                    if (type == typeof(bool)) return getBool;
                    if (type == typeof(byte)) return getByte;
                    if (type == typeof(sbyte)) return getSByte;
                    if (type == typeof(short)) return getShort;
                    if (type == typeof(ushort)) return getUShort;
                    if (type == typeof(int)) return getInt;
                    if (type == typeof(uint)) return getUInt;
                    if (type == typeof(long)) return getLong;
                    if (type == typeof(ulong)) return getULong;
                    if (type == typeof(DateTime)) return getDateTime;
                    if (type == typeof(char)) return getChar;
                    if (type == typeof(float)) return getFloat;
                    if (type == typeof(double)) return getDouble;
                    if (type == typeof(decimal)) return getDecimal;
                    if (type == typeof(Guid)) return getGuid;
                    return getObjectByType(type, true);
                }
                if (memberType.IsISerialize)
                {
                    return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                }
                if (type.IsArray)
                {
                    Type enumerableType = memberType.EnumerableArgumentType;
                    if (((memberType)enumerableType).IsISerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        }
                        return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool)) return getBoolNullArrayObject;
                        if (nullType == typeof(byte)) return getByteNullArrayObject;
                        if (nullType == typeof(sbyte)) return getSByteNullArrayObject;
                        if (nullType == typeof(short)) return getShortNullArrayObject;
                        if (nullType == typeof(ushort)) return getUShortNullArrayObject;
                        if (nullType == typeof(int)) return getIntNullArrayObject;
                        if (nullType == typeof(uint)) return getUIntNullArrayObject;
                        if (nullType == typeof(long)) return getLongNullArrayObject;
                        if (nullType == typeof(ulong)) return getULongNullArrayObject;
                        if (nullType == typeof(DateTime)) return getDateTimeNullArrayObject;
                        if (nullType == typeof(char)) return getCharNullArrayObject;
                        if (nullType == typeof(float)) return getFloatNullArrayObject;
                        if (nullType == typeof(double)) return getDoubleNullArrayObject;
                        if (nullType == typeof(decimal)) return getDecimalNullArrayObject;
                        if (nullType == typeof(Guid)) return getGuidNullArrayObject;
                        return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getArrayNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    }
                    if (enumerableType == typeof(bool)) return getBoolArrayObject;
                    if (enumerableType == typeof(byte)) return getByteArrayObject;
                    if (enumerableType == typeof(sbyte)) return getSByteArrayObject;
                    if (enumerableType == typeof(short)) return getShortArrayObject;
                    if (enumerableType == typeof(ushort)) return getUShortArrayObject;
                    if (enumerableType == typeof(int)) return getIntArrayObject;
                    if (enumerableType == typeof(uint)) return getUIntArrayObject;
                    if (enumerableType == typeof(long)) return getLongArrayObject;
                    if (enumerableType == typeof(ulong)) return getULongArrayObject;
                    if (enumerableType == typeof(DateTime)) return getDateTimeArrayObject;
                    if (enumerableType == typeof(char)) return getCharArrayObject;
                    if (enumerableType == typeof(float)) return getFloatArrayObject;
                    if (enumerableType == typeof(double)) return getDoubleArrayObject;
                    if (enumerableType == typeof(decimal)) return getDecimalArrayObject;
                    if (enumerableType == typeof(Guid)) return getGuidArrayObject;
                    if (enumerableType == typeof(string)) return reflectionDeSerializer.getStringArrayObject;
                    if (enumerableType.isStruct())
                    {
                        return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getArrayNotNullObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    } 
                    return (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod("getArrayObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(((memberType)type).EnumerableArgumentType));
                }
                if (type == typeof(string)) return reflectionDeSerializer.getStringObject;
                return getObjectByType(type, false);
            }
            /// <summary>
            /// 获取 成员对象反序列化
            /// </summary>
            /// <param name="type">成员类型</param>
            /// <returns>成员对象反序列化</returns>
            public static func<reflectionDeSerializer, object> GetMemberGetter(Type type)
            {
                func<reflectionDeSerializer, object> getMember;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getMemberLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getMemberTypes.TryGetValue(hashType, out getMember)) getMemberTypes.Add(hashType, getMember = getMemberGetter(type));
                }
                finally { getMemberLock = 0; }
                return getMember;
            }
            /// <summary>
            /// 成员对象反序列化器访问锁
            /// </summary>
            private static int getMemberLock;
            /// <summary>
            /// 成员对象反序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<reflectionDeSerializer, object>> getMemberTypes = new Dictionary<hashCode<Type>, func<reflectionDeSerializer, object>>();
            /// <summary>
            /// 未知类型反序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数据对象</returns>
            private static object getObjectNotNull<valueType>(reflectionDeSerializer serializer)
            {
                return serialize.deSerialize<valueType>.GetVersionMemerMap(serializer, (int)(serializer.read - serializer.dataStart));
            }
            /// <summary>
            /// 未知类型反序列化
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="serializer">反序列化器</param>
            /// <returns>数据对象</returns>
            private static object getObject<valueType>(reflectionDeSerializer serializer)
            {
                return serializer.unknownNoPoint<valueType>();
            }
            /// <summary>
            /// 获取 未知类型反序列化器
            /// </summary>
            /// <param name="type">未知类型</param>
            /// <param name="isStruct">是否值类型</param>
            /// <returns>未知类型反序列化器</returns>
            private static func<reflectionDeSerializer, object> getObjectByType(Type type, bool isStruct)
            {
                func<reflectionDeSerializer, object> getObject;
                hashCode<Type> hashType = type;
                while (Interlocked.CompareExchange(ref getObjectLock, 1, 0) != 0) Thread.Sleep(1);
                try
                {
                    if (!getObjectTypes.TryGetValue(hashType, out getObject))
                    {
                        getObjectTypes.Add(hashType, getObject = (func<reflectionDeSerializer, object>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, object>), typeof(reflectionDeSerializer).GetMethod(isStruct ? "getObjectNotNull" : "getObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type)));
                    }
                }
                finally { getObjectLock = 0; }
                return getObject;
            }
            /// <summary>
            /// 未知类型反序列化器访问锁
            /// </summary>
            private static int getObjectLock;
            /// <summary>
            /// 未知类型反序列化器集合
            /// </summary>
            private static readonly Dictionary<hashCode<Type>, func<reflectionDeSerializer, object>> getObjectTypes = new Dictionary<hashCode<Type>, func<reflectionDeSerializer, object>>();
        }
        /// <summary>
        /// 对象反序列化器(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal unsafe class reflectionDeSerializer<valueType> : reflectionDeSerializer
        {
            /// <summary>
            /// 成员位图
            /// </summary>
            protected internal memberMap<valueType> memberMap = default(memberMap<valueType>);
            /// <summary>
            /// 对象反序列化器
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="dataFixed">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="version">版本号</param>
            /// <param name="memberMap">成员位图</param>
            public reflectionDeSerializer(byte[] data, byte* dataFixed, int startIndex, int version)
                : base(data, dataFixed, startIndex, version)
            {
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="version">版本号</param>
            public reflectionDeSerializer(deSerializer parentDeSerializer, int version) : base(parentDeSerializer, version) { }
            /// <summary>
            /// 版本号+成员位图接口
            /// </summary>
            protected override void versionMemberMap()
            {
                if (version != dataVersion)
                {
                    fastCSharp.log.Default.Throw("序列化版本号错误 " + version.toString() + " != " + dataVersion.toString(), true, false);
                }
                read = memberMap.DeSerialize(read += sizeof(int));
            }
            /// <summary>
            /// 版本号+成员位图接口
            /// </summary>
            protected internal void VersionMemberMap()
            {
                this.dataVersion = *(int*)read;
                versionMemberMap();
            }
        }
        /// <summary>
        /// 对象反序列化(反射模式)
        /// </summary>
        public static class deSerialize
        {
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            /// <returns>反序列化数据</returns>
            public static valueType Get<valueType>(byte[] data)
            {
                return deSerialize<valueType>.Get(constructor<valueType>.New, data, 0, out deSerializer.OutEndIndex);
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            /// <returns>反序列化数据</returns>
            public static valueType Get<valueType>(byte[] data, int startIndex, out int endIndex)
            {
                return deSerialize<valueType>.Get(constructor<valueType>.New, data, startIndex, out endIndex);
            }
        }
        /// <summary>
        /// 对象反序列化(反射模式)
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        internal class deSerialize<valueType> : serializer<valueType>
        {
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="data">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            /// <returns>目标对象</returns>
            public unsafe static valueType Get(valueType value, byte[] data, int startIndex, out int endIndex)
            {
                fixed (byte* dataFixed = data)
                {
                    reflectionDeSerializer<valueType> deSerializer = new reflectionDeSerializer<valueType>(data, dataFixed, startIndex, version);
                    if (*(int*)deSerializer.read == NullValue)
                    {
                        deSerializer.read += sizeof(int);
                        value = default(valueType);
                    }
                    else
                    {
                        deSerializer.points = new Dictionary<int, object>();
                        if (getter != null) value = getter(deSerializer);
                        else
                        {
                            if (!isStruct) deSerializer.points.Add(-sizeof(int), value);
                            deSerializer.VersionMemberMap();
                            value = getMember(deSerializer, value);
                        }
                    }
                    deSerializer.checkEnd();
                    endIndex = deSerializer.EndIndex;
                }
                return value;
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="parentDeSerializer">对象反序列化器</param>
            /// <param name="point">历史对象指针</param>
            /// <returns>数据对象</returns>
            public unsafe static valueType GetVersionMemerMap(deSerializer parentDeSerializer, int point)
            {
                reflectionDeSerializer<valueType> deSerializer = new reflectionDeSerializer<valueType>(parentDeSerializer, version);
                if (!isUnknownValue) deSerializer.VersionMemberMap();
                valueType value = get(deSerializer, point);
                parentDeSerializer.read = deSerializer.read;
                return value;
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="deSerializer">对象反序列化器</param>
            /// <param name="point">历史对象指针</param>
            /// <returns>数据对象</returns>
            private static valueType get(reflectionDeSerializer<valueType> deSerializer, int point)
            {
                if (getter != null) return getter(deSerializer);
                if (!isStruct)
                {
                    valueType value = constructor<valueType>.New;
                    deSerializer.points.Add(-point, value);
                    return getMember(deSerializer, value);
                }
                if (isUnknownValue) return getUnknownMember(deSerializer);
                return getMember(deSerializer, default(valueType));
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="deSerializer">对象反序列化器</param>
            /// <param name="value">数据对象</param>
            /// <returns>目标对象</returns>
            private unsafe static valueType getMember(reflectionDeSerializer<valueType> deSerializer, valueType value)
            {
                byte* isValueFixed = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValueFixed, memberMapSize);
                object[] values = new object[memberCount];
                memberMap<valueType> memberMap = deSerializer.memberMap;
                memberMap.And(andMemberMap);
                fixedMap nullMap = new fixedMap(deSerializer.read);
                deSerializer.read += memberMap.SerializeSize;
                object reference;
                for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                {
                    int memberIndex = *memberIndexStart;
                    if (memberMap.IsMember(memberIndex))
                    {
                        if (nullMap.Get(memberIndex)) values[memberIndex] = defaultValues[memberIndex];
                        else if (isMemberSerializeMap.Get(memberIndex))
                        {
                            func<object, object> converter = converters[memberIndex];
                            reference = memberGetters[memberIndex](deSerializer);
                            values[memberIndex] = converter == null ? reference : converter(reference);
                        }
                        isValueMap.Set(memberIndex);
                    }
                }
                int length = (int)(deSerializer.read - nullMap.Map);
                if ((length & 3) != 0) deSerializer.read += -length & 3;
                Dictionary<int, object> points = deSerializer.points;
                for (int* memberIndexStart = memberSort.Int, memberIndexEnd = memberIndexStart + memberGroup.Count; memberIndexStart != memberIndexEnd; ++memberIndexStart)
                {
                    int memberIndex = *memberIndexStart;
                    if (!isMemberSerializeMap.Get(memberIndex) && memberMap.IsMember(memberIndex) && !nullMap.Get(memberIndex))
                    {
                        bool isNull = isNullMap.Get(memberIndex);
                        if (isNull)
                        {
                            length = *(int*)deSerializer.read;
                            if (points.TryGetValue(length, out reference)) deSerializer.read += sizeof(int);
                            else reference = memberGetters[memberIndex](deSerializer);
                        }
                        else reference = memberGetters[memberIndex](deSerializer);
                        func<object, object> converter = converters[memberIndex];
                        values[memberIndex] = converter == null ? reference : converter(reference);
                    }
                }
                if (isStruct) return memberGroup.SetMemberValue(value, values, isValueMap);
                memberGroup.SetMember(value, values, isValueMap);
                return value;
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            /// <param name="deSerializer">对象反序列化器</param>
            /// <returns>目标对象</returns>
            private unsafe static valueType getUnknownMember(reflectionDeSerializer<valueType> deSerializer)
            {
                byte* isNull = deSerializer.read;
                deSerializer.read += sizeof(int);
                object memberValue;
                if (*(int*)isNull == 0)
                {
                    memberValue = unknownMemberGetter(deSerializer);
                    if (unknownConverter != null) memberValue = unknownConverter(memberValue);
                    if (isUnknownMemberSerialize)
                    {
                        int length = (int)(deSerializer.read - isNull);
                        if ((length & 3) != 0) deSerializer.read += -length & 3;
                    }
                }
                else memberValue = unknownDefaultValue;
                object value = default(valueType);
                unknownField.Setter(value, memberValue);
                return (valueType)value;
            }
            /// <summary>
            /// 对象反序列化
            /// </summary>
            private static readonly func<reflectionDeSerializer, valueType> getter;
            /// <summary>
            /// 成员位图
            /// </summary>
            private static readonly memberMap<valueType> andMemberMap = default(memberMap<valueType>);
            /// <summary>
            /// 默认成员值集合
            /// </summary>
            private static object[] defaultValues;
            /// <summary>
            /// 成员对象反序列化 集合
            /// </summary>
            private static func<reflectionDeSerializer, object>[] memberGetters;
            /// <summary>
            /// 成员反序列化类型转换器集合
            /// </summary>
            private static func<object, object>[] converters;
            /// <summary>
            /// 初始化成员序列化器
            /// </summary>
            private static void setMemberGetter()
            {
                defaultValues = new object[memberCount];
                memberGetters = new func<reflectionDeSerializer, object>[memberCount];
                converters = new func<object, object>[memberCount];
                foreach (setup.memberInfo member in sortMembers)
                {
                    andMemberMap.SetMember(member.MemberIndex);
                    defaultValues[member.MemberIndex] = constructor.GetNull(member.MemberType);

                    keyValue<func<reflectionDeSerializer, object>, func<object, object>> memberGetter = getMemberGetter(member);
                    memberGetters[member.MemberIndex] = memberGetter.Key;
                    converters[member.MemberIndex] = memberGetter.Value;
                }
            }
            /// <summary>
            /// 默认成员值集合
            /// </summary>
            private static object unknownDefaultValue;
            /// <summary>
            /// 成员对象反序列化 集合
            /// </summary>
            private static func<reflectionDeSerializer, object> unknownMemberGetter;
            /// <summary>
            /// 成员反序列化类型转换器集合
            /// </summary>
            private static func<object, object> unknownConverter;
            /// <summary>
            /// 初始化未知类型成员序列化器
            /// </summary>
            private static void setUnknownMemberGetter()
            {
                unknownDefaultValue = constructor.GetNull(unknownField.MemberType);
                keyValue<func<reflectionDeSerializer, object>, func<object, object>> values = getMemberGetter(unknownField);
                unknownMemberGetter = values.Key;
                unknownConverter = values.Value;
            }
            /// <summary>
            /// 获取成员对象反序列化器
            /// </summary>
            /// <param name="member">成员信息</param>
            /// <returns>成员对象反序列化器</returns>
            private static keyValue<func<reflectionDeSerializer, object>, func<object, object>> getMemberGetter(setup.memberInfo member)
            {
                func<reflectionDeSerializer, object> memberGetter = null;
                func<object, object> converter = null;
                bool isArray = false;
                if (member.MemberType.Type.IsArray)
                {
                    memberType genericType = member.MemberType.Type.GetElementType();
                    if (genericType.SerializeType.Type != genericType.Type)
                    {
                        isArray = true;
                        memberGetter = reflectionDeSerializer.GetMemberGetter(genericType.SerializeType.Type.MakeArrayType());
                        converter = reflection.converter.GetArray(genericType.SerializeType.Type, genericType.Type);
                    }
                }
                if (!isArray)
                {
                    if (member.MemberType.SerializeType.Type.IsArray)
                    {
                        memberType genericType = member.MemberType.SerializeType.Type.GetElementType();
                        if (genericType.SerializeType.Type != genericType.Type)
                        {
                            isArray = true;
                            memberGetter = reflectionDeSerializer.GetMemberGetter(genericType.SerializeType.Type.MakeArrayType());
                            converter = new converter2
                            {
                                Converter1 = reflection.converter.GetArray(genericType.SerializeType.Type, genericType.Type),
                                Converter2 = reflection.converter.Get(member.MemberType.SerializeType.Type, member.MemberType.Type)
                            }.Convert;
                        }
                    }
                }
                if (!isArray)
                {
                    memberGetter = reflectionDeSerializer.GetMemberGetter(member.MemberType.SerializeType);
                    if (member.MemberType.SerializeType.Type != member.MemberType.Type)
                    {
                        converter = reflection.converter.Get(member.MemberType.SerializeType.Type, member.MemberType.Type);
                    }
                }
                return new keyValue<func<reflectionDeSerializer, object>, func<object, object>>(memberGetter, converter);
            }
            static deSerialize()
            {
                if (isUnknownValue)
                {
                    setUnknownMemberGetter();
                    return;
                }
                Type type = typeof(valueType);
                if (isISerialize)
                {
                    getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getISerializeType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type));
                    return;
                }
                if (isStruct)
                {
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, bool?>)reflectionDeSerializer.GetBoolNull;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, byte?>)reflectionDeSerializer.GetByteNull;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, sbyte?>)reflectionDeSerializer.GetSByteNull;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, short?>)reflectionDeSerializer.GetShortNull;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ushort?>)reflectionDeSerializer.GetUShortNull;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, int?>)reflectionDeSerializer.GetIntNull;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, uint?>)reflectionDeSerializer.GetUIntNull;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, long?>)reflectionDeSerializer.GetLongNull;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ulong?>)reflectionDeSerializer.GetULongNull;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, char?>)reflectionDeSerializer.GetCharNull;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, DateTime?>)reflectionDeSerializer.GetDateTimeNull;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, float?>)reflectionDeSerializer.GetFloatNull;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, double?>)reflectionDeSerializer.GetDoubleNull;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, decimal?>)reflectionDeSerializer.GetDecimalNull;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, Guid?>)reflectionDeSerializer.GetGuidNull;
                            return;
                        }
                        getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getNullType", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(nullType));
                        return;
                    }
                    if (type == typeof(bool))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, bool>)reflectionDeSerializer.GetBool;
                        return;
                    }
                    if (type == typeof(byte))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, byte>)reflectionDeSerializer.GetByte;
                        return;
                    }
                    if (type == typeof(sbyte))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, sbyte>)reflectionDeSerializer.GetSByte;
                        return;
                    }
                    if (type == typeof(short))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, short>)reflectionDeSerializer.GetShort;
                        return;
                    }
                    if (type == typeof(ushort))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ushort>)reflectionDeSerializer.GetUShort;
                        return;
                    }
                    if (type == typeof(int))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, int>)reflectionDeSerializer.GetInt;
                        return;
                    }
                    if (type == typeof(uint))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, uint>)reflectionDeSerializer.GetUInt;
                        return;
                    }
                    if (type == typeof(long))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, long>)reflectionDeSerializer.GetLong;
                        return;
                    }
                    if (type == typeof(ulong))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ulong>)reflectionDeSerializer.GetULong;
                        return;
                    }
                    if (type == typeof(char))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, char>)reflectionDeSerializer.GetChar;
                        return;
                    }
                    if (type == typeof(DateTime))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, DateTime>)reflectionDeSerializer.GetDateTime;
                        return;
                    }
                    if (type == typeof(float))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, float>)reflectionDeSerializer.GetFloat;
                        return;
                    }
                    if (type == typeof(double))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, double>)reflectionDeSerializer.GetDouble;
                        return;
                    }
                    if (type == typeof(decimal))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, decimal>)reflectionDeSerializer.GetDecimal;
                        return;
                    }
                    if (type == typeof(Guid))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, Guid>)reflectionDeSerializer.GetGuid;
                        return;
                    }
                    setMemberGetter();
                    return;
                }

                if (type.IsArray)
                {
                    Type enumerableType = ((memberType)type).EnumerableArgumentType;
                    if (((memberType)enumerableType).IsISerialize)
                    {
                        if (enumerableType.isStruct())
                        {
                            getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                            return;
                        }
                        getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getISerializeTypeArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    Type nullType = enumerableType.nullableType();
                    if (nullType != null)
                    {
                        if (nullType == typeof(bool))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, bool?[]>)reflectionDeSerializer.GetBoolNullArray;
                            return;
                        }
                        if (nullType == typeof(byte))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, byte?[]>)reflectionDeSerializer.GetByteNullArray;
                            return;
                        }
                        if (nullType == typeof(sbyte))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, sbyte?[]>)reflectionDeSerializer.GetSByteNullArray;
                            return;
                        }
                        if (nullType == typeof(short))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, short?[]>)reflectionDeSerializer.GetShortNullArray;
                            return;
                        }
                        if (nullType == typeof(ushort))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ushort?[]>)reflectionDeSerializer.GetUShortNullArray;
                            return;
                        }
                        if (nullType == typeof(int))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, int?[]>)reflectionDeSerializer.GetIntNullArray;
                            return;
                        }
                        if (nullType == typeof(uint))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, uint?[]>)reflectionDeSerializer.GetUIntNullArray;
                            return;
                        }
                        if (nullType == typeof(long))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, long?[]>)reflectionDeSerializer.GetLongNullArray;
                            return;
                        }
                        if (nullType == typeof(ulong))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ulong?[]>)reflectionDeSerializer.GetULongNullArray;
                            return;
                        }
                        if (nullType == typeof(char))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, char?[]>)reflectionDeSerializer.GetCharNullArray;
                            return;
                        }
                        if (nullType == typeof(DateTime))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, DateTime?[]>)reflectionDeSerializer.GetDateTimeNullArray;
                            return;
                        }
                        if (nullType == typeof(float))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, float?[]>)reflectionDeSerializer.GetFloatNullArray;
                            return;
                        }
                        if (nullType == typeof(double))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, double?[]>)reflectionDeSerializer.GetDoubleNullArray;
                            return;
                        }
                        if (nullType == typeof(decimal))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, decimal?[]>)reflectionDeSerializer.GetDecimalNullArray;
                            return;
                        }
                        if (nullType == typeof(Guid))
                        {
                            getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, Guid?[]>)reflectionDeSerializer.GetGuidNullArray;
                            return;
                        }
                        getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getArrayNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    if (enumerableType == typeof(bool))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, bool[]>)reflectionDeSerializer.GetBoolArray;
                        return;
                    }
                    if (enumerableType == typeof(byte))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, byte[]>)reflectionDeSerializer.GetByteArray;
                        return;
                    }
                    if (enumerableType == typeof(sbyte))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, sbyte[]>)reflectionDeSerializer.GetSByteArray;
                        return;
                    }
                    if (enumerableType == typeof(short))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, short[]>)reflectionDeSerializer.GetShortArray;
                        return;
                    }
                    if (enumerableType == typeof(ushort))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ushort[]>)reflectionDeSerializer.GetUShortArray;
                        return;
                    }
                    if (enumerableType == typeof(int))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, int[]>)reflectionDeSerializer.GetIntArray;
                        return;
                    }
                    if (enumerableType == typeof(uint))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, uint[]>)reflectionDeSerializer.GetUIntArray;
                        return;
                    }
                    if (enumerableType == typeof(long))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, long[]>)reflectionDeSerializer.GetLongArray;
                        return;
                    }
                    if (enumerableType == typeof(ulong))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, ulong[]>)reflectionDeSerializer.GetULongArray;
                        return;
                    }
                    if (enumerableType == typeof(char))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, char[]>)reflectionDeSerializer.GetCharArray;
                        return;
                    }
                    if (enumerableType == typeof(DateTime))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, DateTime[]>)reflectionDeSerializer.GetDateTimeArray;
                        return;
                    }
                    if (enumerableType == typeof(float))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, float[]>)reflectionDeSerializer.GetFloatArray;
                        return;
                    }
                    if (enumerableType == typeof(double))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, double[]>)reflectionDeSerializer.GetDoubleArray;
                        return;
                    }
                    if (enumerableType == typeof(decimal))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, decimal[]>)reflectionDeSerializer.GetDecimalArray;
                        return;
                    }
                    if (enumerableType == typeof(Guid))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, Guid[]>)reflectionDeSerializer.GetGuidArray;
                        return;
                    }
                    if (enumerableType == typeof(string))
                    {
                        getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, string[]>)reflectionDeSerializer.GetStringArray;
                        return;
                    }
                    if (enumerableType.isStruct())
                    {
                        getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getArrayNotNull", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                        return;
                    }
                    getter = (func<reflectionDeSerializer, valueType>)Delegate.CreateDelegate(typeof(func<reflectionDeSerializer, valueType>), typeof(reflectionDeSerializer).GetMethod("getArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(enumerableType));
                    return;
                }
                if (type == typeof(string))
                {
                    getter = (func<reflectionDeSerializer, valueType>)(Delegate)(func<reflectionDeSerializer, string>)reflectionDeSerializer.GetString;
                    return;
                }
                setMemberGetter();
            }
        }
        /// <summary>
        /// 空对象长度
        /// </summary>
        public const int NullValue = int.MinValue;
        /// <summary>
        /// 是否反射模式
        /// </summary>
        public bool IsReflection;
        /// <summary>
        /// 序列化版本号(不能小于0)
        /// </summary>
        public int Version;
        /// <summary>
        /// 序列化隐式转换类型
        /// </summary>
        public Type SerializeType;
        /// <summary>
        /// 是否支持序列化到流
        /// </summary>
        public bool IsStreamSerialize;
        /// <summary>
        /// 是否支持object序列化
        /// </summary>
        public bool IsObject;
        /// <summary>
        /// 是否支持接口序列化
        /// </summary>
        public bool IsInterface;
        /// <summary>
        /// 是否用于测试用例
        /// </summary>
        public bool IsTestCase;
        /// <summary>
        /// 生成部分代码[showjim.setup]
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="partName">部分名称</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="members">成员集合</param>
        /// <param name="attribute">自定义属性</param>
        /// <param name="isMemberMap">是否生成成员位图</param>
        /// <returns>部分代码</returns>
        public static string PartCode(Assembly assembly, string partName, string typeName, setup.cSharp.memberInfo[] members, serialize attribute, bool isMemberMap)
        {
            return cSharp.PartCode(assembly, partName, typeName, members, attribute, isMemberMap);
        }
        /// <summary>
        /// 序列化代码生成
        /// </summary
        [auto(Name = "序列化", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : member<serialize>
        {
            /// <summary>
            /// 未知的序列化类型
            /// </summary>
            private readonly static HashSet<Type> unknownSerializeTypes = new HashSet<Type>();
            /// <summary>
            /// 序列化代码生成自定义属性
            /// </summary>
            public serialize SerializeAttribute
            {
                get { return Attribute; }
            }
            /// <summary>
            /// 序列化基本字节数
            /// </summary>
            public int SerializeSize;
            /// <summary>
            /// 是否存在引用类型
            /// </summary>
            public bool AnyReferenceMember
            {
                get
                {
                    return Members.any(value => !value.MemberType.IsSerializeUnknown && value.MemberType.SerializeType.IsNull);
                }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                if (!Attribute.IsReflection && Attribute.SerializeType == null && isConstructor())
                {
                    Members = getMembers().getFindArray(value => value.CanSet && value.CanGet
                        && (Attribute.IsObject || value.MemberType.SerializeType.Type != typeof(object))
                        && (Attribute.IsInterface || !value.MemberType.SerializeType.Type.IsInterface))
                        .sortDesc(value => value.MemberType.SerializeSize);
                    SerializeSize = (Members.sum(member => member.MemberType.SerializeSize) + 3) & (int.MaxValue - 3);
                    memberMap.create(type);
                    copy.create(type);
                    create(true);
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                if (unknownSerializeTypes.Count != 0)
                {
                    error.Message("没有生成序列化代码 : " + unknownSerializeTypes.joinString(',', value => value.fullName()));
                    unknownSerializeTypes.Clear();
                }
            }
            /// <summary>
            /// 未知序列化类型
            /// </summary>
            /// <param name="type">类型</param>
            public static void UnknownSerialize(Type type)
            {
                if (type != null && type.FullName != null && type.customAttribute<serialize>() == null) unknownSerializeTypes.Add(type);
            }
            /// <summary>
            /// 生成部分代码
            /// </summary>
            /// <param name="assembly">程序集</param>
            /// <param name="partName">部分名称</param>
            /// <param name="typeName">类型名称</param>
            /// <param name="members">成员集合</param>
            /// <param name="attribute">自定义属性</param>
            /// <param name="isMemberMap">是否生成成员位图</param>
            /// <returns>部分代码</returns>
            public static string PartCode(Assembly assembly, string partName, string typeName, memberInfo[] members, serialize attribute, bool isMemberMap)
            {
                cSharp coder = new cSharp();
                coder.assembly = assembly;
                coder.type = new memberType(typeName);
                coder.Attribute = attribute;
                coder.Members = members.getArray(member => member.create()).sortDesc(value => value.MemberType.SerializeSize);
                coder.SerializeSize = (coder.Members.sum(member => member.MemberType.SerializeSize) + 3) & (int.MaxValue - 3);
                coder.create(false);
                string code = coder._partCodes_[partName];
                if (isMemberMap) code += setup.cSharp.memberMap.cSharp.PartCode("BODY", coder.type, coder.Members);
                return code;
            }
        }
    }
}
namespace fastCSharp.setup
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal unsafe partial class memberType
    {
        /// <summary>
        /// 序列化基本字节数
        /// </summary>
        private static readonly staticDictionary<hashCode<Type>, int> memorySerializeSizes = new staticDictionary<hashCode<Type>, int>
            (new keyValue<hashCode<Type>, int>[]
            {
                new keyValue<hashCode<Type>, int>(typeof(bool), sizeof(bool)),
                new keyValue<hashCode<Type>, int>(typeof(byte), sizeof(byte)),
                new keyValue<hashCode<Type>, int>(typeof(sbyte), sizeof(sbyte)),
                new keyValue<hashCode<Type>, int>(typeof(short), sizeof(short)),
                new keyValue<hashCode<Type>, int>(typeof(ushort), sizeof(ushort)),
                new keyValue<hashCode<Type>, int>(typeof(int), sizeof(int)),
                new keyValue<hashCode<Type>, int>(typeof(uint), sizeof(uint)),
                new keyValue<hashCode<Type>, int>(typeof(long), sizeof(long)),
                new keyValue<hashCode<Type>, int>(typeof(ulong), sizeof(ulong)),
                new keyValue<hashCode<Type>, int>(typeof(char), sizeof(char)),
                new keyValue<hashCode<Type>, int>(typeof(DateTime), sizeof(long)),
                new keyValue<hashCode<Type>, int>(typeof(float), sizeof(float)),
                new keyValue<hashCode<Type>, int>(typeof(double), sizeof(double)),
                new keyValue<hashCode<Type>, int>(typeof(decimal), sizeof(decimal)),
                new keyValue<hashCode<Type>, int>(typeof(Guid), sizeof(Guid))
            });
        /// <summary>
        /// 序列化基本字节数
        /// </summary>
        private static readonly staticHashSet<hashCode<Type>> blockCopySerializeTypes = new staticHashSet<hashCode<Type>>(new hashCode<Type>[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(double) });
        /// <summary>
        /// 位置序列化类型最大基本字节数
        /// </summary>
        public static readonly int MaxSerializeSize = memorySerializeSizes.Values.MaxKey(value => value, sizeof(int));
        /// <summary>
        /// 序列化类型
        /// </summary>
        private memberType serializeType;
        /// <summary>
        /// 序列化类型
        /// </summary>
        public memberType SerializeType
        {
            get
            {
                if (serializeType == null)
                {
                    cSharp.serialize serialize = Type.customAttribute<cSharp.serialize>();
                    if (serialize != null && serialize.SerializeType != null)
                    {
                        Type type = serialize.SerializeType;
                        if (Type.IsGenericType)
                        {
                            if (type.IsArray)
                            {
                                if (type.GetElementType() == typeof(cSharp.serialize.ISerializeGeneric)) type = Type.GetGenericArguments()[0].MakeArrayType();
                            }
                            else
                            {
                                Type[] types = type.GetGenericArguments();
                                int index = 0;
                                foreach (Type nextType in Type.GetGenericArguments())
                                {
                                    while (types[index] != typeof(cSharp.serialize.ISerializeGeneric)) ++index;
                                    types[index] = nextType;
                                }
                                type = type.GetGenericTypeDefinition().MakeGenericType(types);
                            }
                        }
                        serializeType = type;
                    }
                    else if (Type.IsEnum) serializeType = System.Enum.GetUnderlyingType(Type);
                    else serializeType = Type;
                }
                return serializeType;
            }
        }
        /// <summary>
        /// 序列化类型是否一致
        /// </summary>
        public bool IsSerializeType
        {
            get
            {
                return SerializeType.Type == Type;
            }
        }
        /// <summary>
        /// 序列化类型是否不一致,或者枚举类型
        /// </summary>
        public bool NotSerializeTypeEnum
        {
            get
            {
                return !IsSerializeType || Type.IsEnum;
            }
        }
        /// <summary>
        /// 序列化基本字节数
        /// </summary>
        private int serializeSize;
        /// <summary>
        /// 序列化基本字节数
        /// </summary>
        public int SerializeSize
        {
            get
            {
                if (serializeSize == 0) serializeSize = memorySerializeSizes.Get(SerializeType.Type.nullableType() ?? SerializeType, sizeof(int));
                return serializeSize;
            }
        }
        /// <summary>
        /// 是否值类型序列化
        /// </summary>
        private bool? isMemberSerialize;
        /// <summary>
        /// 是否值类型序列化
        /// </summary>
        public bool IsMemberSerialize
        {
            get
            {
                if (isMemberSerialize == null) isMemberSerialize = memorySerializeSizes.ContainsKey(SerializeType.Type.nullableType() ?? SerializeType);
                return (bool)isMemberSerialize;
            }
        }
        /// <summary>
        /// 是否内存复制序列化
        /// </summary>
        public bool IsSerializeBlockCopy
        {
            get
            {
                return blockCopySerializeTypes.Contains(SerializeType.Type.nullableType() ?? SerializeType);
            }
        }
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.serialize.ISerialize
        /// </summary>
        private bool? isISerialize;
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.serialize.ISerialize
        /// </summary>
        public bool IsISerialize
        {
            get
            {
                if (isISerialize == null) isISerialize = SerializeType.Type.isInterface(typeof(fastCSharp.setup.cSharp.serialize.ISerialize));
                return (bool)isISerialize;
            }
        }
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.serialize.IStreamSerialize
        /// </summary>
        private bool? isIStreamSerialize;
        /// <summary>
        /// 是否继承接口fastCSharp.setup.cSharp.serialize.IStreamSerialize
        /// </summary>
        public bool IsIStreamSerialize
        {
            get
            {
                if (isIStreamSerialize == null) isIStreamSerialize = SerializeType.Type.isInterface(typeof(fastCSharp.setup.cSharp.serialize.IStreamSerialize));
                return (bool)isIStreamSerialize;
            }
        }
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        private bool? serializeUnknown;
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        private bool SerializeUnknown
        {
            get
            {
                if (serializeUnknown == null)
                {
                    serializeUnknown = !(IsMemberSerialize || SerializeType.IsString || SerializeType.Type.IsArray);
                }
                return (bool)serializeUnknown;
            }
        }
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        private bool? isSerializeUnknown;
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        public bool IsSerializeUnknown
        {
            get
            {
                if (isSerializeUnknown == null)
                {
                    if (SerializeUnknown)
                    {
                        if (SerializeType.IsISerialize) isSerializeUnknown = false;
                        else
                        {
                            fastCSharp.setup.cSharp.serialize.cSharp.UnknownSerialize(SerializeType.Type.IsArray ? SerializeType.EnumerableArgumentType.SerializeType.Type : SerializeType.Type);
                            isSerializeUnknown = true;
                        }
                    }
                    else isSerializeUnknown = false;
                }
                return (bool)isSerializeUnknown;
            }
        }
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        private bool? isStreamSerializeUnknown;
        /// <summary>
        /// 未知序列化类型
        /// </summary>
        public bool IsStreamSerializeUnknown
        {
            get
            {
                if (isStreamSerializeUnknown == null)
                {
                    if (SerializeUnknown)
                    {
                        if (SerializeType.IsIStreamSerialize) isStreamSerializeUnknown = false;
                        else
                        {
                            if (IsSerializeUnknown)
                            {
                                fastCSharp.setup.cSharp.serialize.cSharp.UnknownSerialize(SerializeType.Type.IsArray ? SerializeType.EnumerableArgumentType.SerializeType.Type : SerializeType.Type);
                            }
                            isStreamSerializeUnknown = true;
                        }
                    }
                    else isStreamSerializeUnknown = false;
                }
                return (bool)isStreamSerializeUnknown;
            }
        }
        /// <summary>
        /// 是否序列化数组
        /// </summary>
        public bool IsSerializeArray
        {
            get { return Type.IsArray && !IsByteArray; }
        }
        /// <summary>
        /// 是否序列化未知类型数组
        /// </summary>
        public bool IsSerializeArrayUnknown
        {
            get
            {
                return !IsMemberSerialize && !IsString && !IsISerialize;
            }
        }
        public bool IsStreamSerializeArrayUnknown
        {
            get
            {
                return !IsMemberSerialize && !IsString && !IsIStreamSerialize;
            }
        }
    }
}
