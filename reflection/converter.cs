using System;
using System.Reflection;
using System.Collections.Generic;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 类型转换函数
    /// </summary>
    public static class converter
    {
        /// <summary>
        /// 数组转换器
        /// </summary>
        private interface IArrayConverter
        {
            /// <summary>
            /// 数组转换
            /// </summary>
            /// <param name="arrayObject">原数组</param>
            /// <returns>目标数组</returns>
            object Get(object arrayObject);
        }
        /// <summary>
        /// 数组转换器
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="convertType">目标类型</typeparam>
        private struct arrayConverter<valueType, convertType> : IArrayConverter
        {
            /// <summary>
            /// 类型转换器
            /// </summary>
            private func<object, object> converter;
            /// <summary>
            /// 数组转换器
            /// </summary>
            /// <param name="converter">类型转换器</param>
            public arrayConverter(func<object, object> converter)
            {
                this.converter = converter;
            }
            /// <summary>
            /// 数组转换
            /// </summary>
            /// <param name="arrayObject">原数组</param>
            /// <returns>目标数组</returns>
            public object Get(object arrayObject)
            {
                if (arrayObject == null) return null;
                valueType[] array = (valueType[])arrayObject;
                convertType[] newArray = new convertType[array.Length];
                int index = 0;
                foreach (valueType value in array) newArray[index++] = (convertType)converter(value);
                return newArray;
            }
        }
        /// <summary>
        /// 获取数组类型转换函数
        /// </summary>
        /// <param name="type">原类型</param>
        /// <param name="convertType">目标类型</param>
        /// <returns>数组类型转换函数,失败返回null</returns>
        public static func<object, object> GetArray(Type type, Type convertType)
        {
            func<object, object> value = Get(type, convertType);
            if (value != null)
            {
                return ((IArrayConverter)Activator.CreateInstance(typeof(arrayConverter<,>).MakeGenericType(type, convertType), value)).Get;
            }
            return null;
        }
        /// <summary>
        /// 获取类型转换函数
        /// </summary>
        /// <param name="type">原类型</param>
        /// <param name="convertType">目标类型</param>
        /// <returns>类型转换函数,失败返回null</returns>
        public static func<object, object> Get(Type type, Type convertType)
        {
            MethodInfo method = getMethod(type, convertType);
            if (method != null) return staticMethodDelegate.Create(method);
            if (type.isInterface(typeof(IConvertible)))
            {
                func<object, object> func = getConvertibles.Get(convertType, null);
                if (func != null) return func;
                return staticMethodDelegate.Create(getConvertibleMethodInfo.MakeGenericMethod(convertType));
            }
            return null;
        }
        /// <summary>
        /// 获取类型转换函数
        /// </summary>
        /// <param name="type">原类型</param>
        /// <param name="convertType">目标类型</param>
        /// <returns>类型转换函数,失败返回null</returns>
        private static MethodInfo getMethod(Type type, Type convertType)
        {
            if (type.isInherit(convertType)) return getBaseMethodInfo.MakeGenericMethod(type, convertType);
            if (convertType.isInherit(type)) return getMethodInfo.MakeGenericMethod(type, convertType);
            MethodInfo method = type.getImplicitMethod(convertType);
            if (method != null) return method;
            if (convertType.IsEnum) return getEnumMethods.Get(type, getEnumMethodInfo).MakeGenericMethod(convertType);
            return null;
        }
        /// <summary>
        /// 转换为基类函数
        /// </summary>
        /// <typeparam name="valueType">原类型</typeparam>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getBase<valueType, convertType>(object value) where valueType : convertType
        {
            return (convertType)(valueType)value;
        }
        /// <summary>
        /// 转换为子类函数
        /// </summary>
        /// <typeparam name="valueType">原类型</typeparam>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object get<valueType, convertType>(object value) where convertType : valueType
        {
            return (convertType)(valueType)value;
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getConvertible<convertType>(object value)
        {
            return ((IConvertible)value).ToType(typeof(convertType), null);
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumByte<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToByte(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumSByte<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToSByte(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumUShort<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToUInt16(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumShort<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToInt16(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumUInt<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToUInt32(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumInt<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToInt32(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumULong<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToUInt64(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnumLong<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), ((IConvertible)value).ToInt64(null));
        }
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        /// <typeparam name="convertType">目标类型</typeparam>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getEnum<convertType>(object value)
        {
            return System.Enum.ToObject(typeof(convertType), value);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getBool(object value)
        {
            return ((IConvertible)value).ToBoolean(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getByte(object value)
        {
            return ((IConvertible)value).ToByte(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getChar(object value)
        {
            return ((IConvertible)value).ToChar(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getDateTime(object value)
        {
            return ((IConvertible)value).ToDateTime(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getDecimal(object value)
        {
            return ((IConvertible)value).ToDecimal(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getDouble(object value)
        {
            return ((IConvertible)value).ToDouble(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getShort(object value)
        {
            return ((IConvertible)value).ToInt16(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getInt(object value)
        {
            return ((IConvertible)value).ToInt32(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getLong(object value)
        {
            return ((IConvertible)value).ToInt64(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getSByte(object value)
        {
            return ((IConvertible)value).ToSByte(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getFloat(object value)
        {
            return ((IConvertible)value).ToSingle(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getString(object value)
        {
            return ((IConvertible)value).ToString(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getUShort(object value)
        {
            return ((IConvertible)value).ToUInt16(null);
        }
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getUInt(object value)
        {
            return ((IConvertible)value).ToUInt32(null);
        }
        /// <summary>
        /// <summary>
        /// 转换接口函数
        /// </summary>
        /// <param name="value">原数据</param>
        /// <returns>目标数据</returns>
        private static object getULong(object value)
        {
            return ((IConvertible)value).ToUInt64(null);
        }
        /// 转换为基类函数
        /// </summary>
        private static readonly MethodInfo getBaseMethodInfo;
        /// <summary>
        /// 转换为子类函数
        /// </summary>
        private static readonly MethodInfo getMethodInfo;
        /// <summary>
        /// 转换接口函数
        /// </summary>
        private static readonly MethodInfo getConvertibleMethodInfo;
        /// <summary>
        /// 转换接口函数集合
        /// </summary>
        private static readonly staticDictionary<hashCode<Type>, func<object, object>> getConvertibles;
        /// <summary>
        /// 转换枚举函数
        /// </summary>
        private static readonly MethodInfo getEnumMethodInfo;
        /// <summary>
        /// 转换枚举函数集合
        /// </summary>
        private static readonly staticDictionary<hashCode<Type>, MethodInfo> getEnumMethods;
        static converter()
        {
            list<keyValue<hashCode<Type>, func<object, object>>> getConvertibleList = new list<keyValue<hashCode<Type>, func<object, object>>>();
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(bool), getBool));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(byte), getByte));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(char), getChar));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(DateTime), getDateTime));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(decimal), getDecimal));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(double), getDouble));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(short), getShort));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(int), getInt));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(long), getLong));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(sbyte), getSByte));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(float), getFloat));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(string), getString));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(ushort), getUShort));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(uint), getUInt));
            getConvertibleList.Add(new keyValue<hashCode<Type>, func<object, object>>(typeof(ulong), getULong));
            getConvertibles = new staticDictionary<hashCode<Type>, func<object, object>>(getConvertibleList);

            Dictionary<string, MethodInfo> methods = typeof(converter).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).getDictionary(value => value.Name);
            getBaseMethodInfo = methods["getBase"];
            getMethodInfo = methods["get"];
            getConvertibleMethodInfo = methods["getConvertible"];
            getEnumMethodInfo = methods["getEnum"];

            list<keyValue<hashCode<Type>, MethodInfo>> getEnumMethodList = new list<keyValue<hashCode<Type>, MethodInfo>>();
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(byte), methods["getEnumByte"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(sbyte), methods["getEnumSByte"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(short), methods["getEnumShort"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(ushort), methods["getEnumUShort"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(int), methods["getEnumInt"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(uint), methods["getEnumUInt"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(long), methods["getEnumLong"]));
            getEnumMethodList.Add(new keyValue<hashCode<Type>, MethodInfo>(typeof(ulong), methods["getEnumULong"]));
            getEnumMethods = new staticDictionary<hashCode<Type>, MethodInfo>(getEnumMethodList);
        }
    }
}
