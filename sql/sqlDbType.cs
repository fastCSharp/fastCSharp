using System;
using System.Data;

namespace fastCSharp.sql
{
    /// <summary>
    /// SQL数据类型相关操作
    /// </summary>
    internal unsafe static class sqlDbType
    {
        /// <summary>
        /// C#类型转SQL数据类型集合
        /// </summary>
        private static readonly staticDictionary<hashCode<Type>, SqlDbType> formCSharpTypes;
        /// <summary>
        /// 根据C#类型获取SQL数据类型
        /// </summary>
        /// <param name="type">C#类型</param>
        /// <returns>SQL数据类型</returns>
        public static SqlDbType formCSharpType(this Type type)
        {
            SqlDbType value = SqlDbType.NVarChar;
            Type nullableType = fastCSharp.reflection.type.nullableType(type);
            formCSharpTypes.Get((nullableType == null ? type : nullableType), ref value);
            return value;
        }
        /// <summary>
        /// 字符串类型占位集合
        /// </summary>
        private static readonly fixedMap.unsafer stringTypeMap;
        /// <summary>
        /// 判断是否字符串类型
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>是否字符串类型</returns>
        public static bool isStringType(this SqlDbType type) { return stringTypeMap.Get((int)type); }
        /// <summary>
        /// 文本类型占位集合
        /// </summary>
        private static readonly fixedMap.unsafer textImageTypeMap;
        /// <summary>
        /// 判断是否文本类型
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>是否文本类型</returns>
        public static bool isTextImageType(this SqlDbType type) { return textImageTypeMap.Get((int)type); }

        /// <summary>
        /// 未知数据长度
        /// </summary>
        public const int UnknownSize = -1;
        /// <summary>
        /// 类型默认长度
        /// </summary>
        private static readonly pointer sizes;
        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns>数据长度</returns>
        public static int getSize(this SqlDbType type)
        {
            return sizes.Int[(int)type];
        }
        /// <summary>
        /// SQL数据类型最大枚举值
        /// </summary>
        private static readonly int MaxEnumValue;

        unsafe static sqlDbType()
        {
            #region C#类型转SQL数据类型集合
            list<keyValue<hashCode<Type>, SqlDbType>> formCSharpType = new list<keyValue<hashCode<Type>, SqlDbType>>();
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(bool), SqlDbType.Bit));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(byte), SqlDbType.TinyInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(sbyte), SqlDbType.TinyInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(short), SqlDbType.SmallInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(ushort), SqlDbType.SmallInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(int), SqlDbType.Int));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(uint), SqlDbType.Int));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(long), SqlDbType.BigInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(ulong), SqlDbType.BigInt));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(decimal), SqlDbType.Decimal));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(float), SqlDbType.Real));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(double), SqlDbType.Float));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(string), SqlDbType.NVarChar));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(DateTime), SqlDbType.DateTime));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(Guid), SqlDbType.UniqueIdentifier));
            formCSharpType.Add(new keyValue<hashCode<Type>, SqlDbType>(typeof(byte[]), SqlDbType.VarBinary));
            formCSharpTypes = new staticDictionary<hashCode<Type>, SqlDbType>(formCSharpType);
            #endregion

            MaxEnumValue = fastCSharp.Enum.GetMaxValue<SqlDbType>(-1) + 1;
            int dataIndex = 0, mapSize = ((MaxEnumValue + 31) >> 5) << 2;
            pointer[] datas = unmanaged.Get(true, mapSize, mapSize, MaxEnumValue * sizeof(int));
            stringTypeMap = new fixedMap(datas[dataIndex++]).Unsafer;
            textImageTypeMap = new fixedMap(datas[dataIndex++]).Unsafer;
            sizes = datas[dataIndex++];

            #region 字符串类型占位集合
            stringTypeMap.Set((int)SqlDbType.Char);
            stringTypeMap.Set((int)SqlDbType.NChar);
            stringTypeMap.Set((int)SqlDbType.VarChar);
            stringTypeMap.Set((int)SqlDbType.NVarChar);
            stringTypeMap.Set((int)SqlDbType.Text);
            stringTypeMap.Set((int)SqlDbType.NText);
            #endregion

            #region 文本类型占位集合
            textImageTypeMap.Set((int)SqlDbType.Text);
            textImageTypeMap.Set((int)SqlDbType.NText);
            textImageTypeMap.Set((int)SqlDbType.Image);
            #endregion

            #region 类型默认长度
            int* sizeData = sizes.Int;
            for (int i = 0; i != MaxEnumValue; i++) sizeData[i] = UnknownSize;
            sizeData[(int)SqlDbType.BigInt] = sizeof(long);
            sizeData[(int)SqlDbType.Binary] = 8000;
            sizeData[(int)SqlDbType.Bit] = sizeof(bool);
            sizeData[(int)SqlDbType.Char] = 8000;
            //TypeSize[(int)SqlDbType.Date] = sizeof(long);
            sizeData[(int)SqlDbType.DateTime] = sizeof(long);
            //TypeSize[(int)SqlDbType.DateTime2] = sizeof(long);
            //TypeSize[(int)SqlDbType.DateTimeOffset] = sizeof(long);
            sizeData[(int)SqlDbType.Decimal] = sizeof(decimal);
            sizeData[(int)SqlDbType.Float] = sizeof(double);
            sizeData[(int)SqlDbType.Image] = int.MaxValue;
            sizeData[(int)SqlDbType.Int] = sizeof(int);
            sizeData[(int)SqlDbType.Money] = sizeof(decimal);
            //TypeSize[(int)SqlDbType.NChar] = MaxStringSize;
            sizeData[(int)SqlDbType.NText] = int.MaxValue;
            //TypeSize[(int)SqlDbType.NVarChar] = MaxStringSize;
            sizeData[(int)SqlDbType.Real] = sizeof(float);
            sizeData[(int)SqlDbType.UniqueIdentifier] = 8;
            sizeData[(int)SqlDbType.SmallDateTime] = sizeof(long);
            sizeData[(int)SqlDbType.SmallInt] = sizeof(short);
            sizeData[(int)SqlDbType.SmallMoney] = sizeof(decimal);
            sizeData[(int)SqlDbType.Text] = int.MaxValue;
            //TypeSize[(int)SqlDbType.Time] = 8;
            sizeData[(int)SqlDbType.Timestamp] = 8;
            sizeData[(int)SqlDbType.TinyInt] = sizeof(byte);
            sizeData[(int)SqlDbType.VarBinary] = 8000;
            sizeData[(int)SqlDbType.VarChar] = 8000;
            //TypeSize[(int)SqlDbType.Xml] = -1;
            #endregion
        }
    }
}
