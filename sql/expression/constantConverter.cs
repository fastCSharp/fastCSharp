using System;

namespace fastCSharp.sql.expression
{
    /// <summary>
    /// 常量转换
    /// </summary>
    internal abstract class constantConverter
    {
        /// <summary>
        /// 常量转换处理集合
        /// </summary>
        protected staticDictionary<hashCode<Type>, func<object, string>> converters;
        /// <summary>
        /// 获取常量转换处理函数
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>失败返回null</returns>
        public func<object, string> this[Type type]
        {
            get
            {
                return converters.Get(type, null);
            }
        }
        /// <summary>
        /// 常量转换
        /// </summary>
        protected constantConverter()
        {
            list<keyValue<hashCode<Type>, func<object, string>>> values = new list<keyValue<hashCode<Type>, func<object, string>>>();
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(bool), convertConstantBoolTo01));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(bool?), convertConstantBoolNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(byte), convertConstantByte));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(byte?), convertConstantByteNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(sbyte), convertConstantSByte));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(sbyte?), convertConstantSByteNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(short), convertConstantShort));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(short?), convertConstantShortNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(ushort), convertConstantUShort));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(ushort?), convertConstantUShortNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(int), convertConstantInt));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(int?), convertConstantIntNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(uint), convertConstantUInt));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(uint?), convertConstantUIntNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(long), convertConstantLong));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(long?), convertConstantLongNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(ulong), convertConstantULong));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(ulong?), convertConstantULongNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(float), convertConstantFloat));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(float?), convertConstantFloatNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(double), convertConstantDouble));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(double?), convertConstantDoubleNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(decimal), convertConstantDecimal));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(decimal?), convertConstantDecimalNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(DateTime), convertConstantDateTimeMillisecond));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(DateTime?), convertConstantDateTimeMillisecondNullable));
            values.Add(new keyValue<hashCode<Type>, func<object, string>>(typeof(string), null));
            converters = new staticDictionary<hashCode<Type>, func<object, string>>(values);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantBoolTo01(object value)
        {
            return (bool)value ? "1" : "0";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantBoolNullable(object value)
        {
            return value != null ? ((bool)value ? "1" : "0") : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantByte(object value)
        {
            return ((byte)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantByteNullable(object value)
        {
            return value != null ? ((byte)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantSByte(object value)
        {
            return ((sbyte)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantSByteNullable(object value)
        {
            return value != null ? ((sbyte)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantShort(object value)
        {
            return ((short)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantShortNullable(object value)
        {
            return value != null ? ((short)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantUShort(object value)
        {
            return ((ushort)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantUShortNullable(object value)
        {
            return value != null ? ((ushort)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantInt(object value)
        {
            return ((int)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantIntNullable(object value)
        {
            return value != null ? ((int)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantUInt(object value)
        {
            return ((uint)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantUIntNullable(object value)
        {
            return value != null ? ((uint)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantLong(object value)
        {
            return ((long)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantLongNullable(object value)
        {
            return value != null ? ((long)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantULong(object value)
        {
            return ((ulong)value).toString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantULongNullable(object value)
        {
            return value != null ? ((ulong)value).toString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantFloat(object value)
        {
            return ((float)value).ToString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantFloatNullable(object value)
        {
            return value != null ? ((float)value).ToString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDouble(object value)
        {
            return ((double)value).ToString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDoubleNullable(object value)
        {
            return value != null ? ((double)value).ToString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDecimal(object value)
        {
            return ((decimal)value).ToString();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDecimalNullable(object value)
        {
            return value != null ? ((decimal)value).ToString() : "null";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDateTimeMillisecond(object value)
        {
            return "'" + ((DateTime)value).toSqlMillisecond() + "'";
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantDateTimeMillisecondNullable(object value)
        {
            return value != null ? ("'" + ((DateTime)value).toSqlMillisecond() + "'") : "null";
        }
        /// <summary>
        /// SQL语句字符串格式化
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <returns>格式化后的字符串</returns>
        internal static unsafe string convertConstantStringMssql(string value)
        {
            int length = (value.Length + 1) << 1;
            if (length <= (fastCSharp.config.pub.Default.StreamBufferLength >> 1))
            {
                char* formatFixed = stackalloc char[length];
                return convertConstantStringMssql(value, formatFixed);
            }
            else
            {
                char[] format = new char[length];
                fixed (char* formatFixed = format) return convertConstantStringMssql(value, formatFixed);
            }
        }
        /// <summary>
        /// SQL语句字符串格式化
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="formatFixed">格式化字符串</param>
        /// <returns>格式化后的字符串</returns>
        private static unsafe string convertConstantStringMssql(string value, char* formatFixed)
        {
            fixed (char* valueFixed = value)
            {
                char* write = formatFixed;
                *write = '\'';
                for (char* next = valueFixed, end = valueFixed + value.Length; next != end; ++next)
                {
                    if (*next == '\'') *++write = '\'';
                    *++write = *next;
                }
                *++write = '\'';
                return new string(formatFixed, 0, (int)(++write - formatFixed));
            }
        }
    }
}
