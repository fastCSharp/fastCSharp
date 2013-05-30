using System;

namespace fastCSharp.testCase
{
    public partial class data
    {
        public enum enumByte : byte
        {
            A,
            B
        }
        public enum enumSByte : sbyte
        {
            A,
            B
        }
        public enum enumShort : short
        {
            A,
            B
        }
        public enum enumUShort : ushort
        {
            A,
            B
        }
        public enum enumInt : int
        {
            A,
            B
        }
        public enum enumUInt : uint
        {
            A,
            B
        }
        public enum enumLong : long
        {
            A,
            B
        }
        public enum enumULong : ulong
        {
            A,
            B
        }

        public enumByte EnumByte = enumByte.B;
        public enumSByte EnumSByte = enumSByte.B;
        public enumShort EnumShort = enumShort.B;
        public enumUShort EnumUShort = enumUShort.B;
        public enumInt EnumInt = enumInt.B;
        public enumUInt EnumUInt = enumUInt.B;
        public enumLong EnumLong = enumLong.B;
        public enumULong EnumULong = enumULong.B;

        public bool Bool = true;
        public byte Byte = 1;
        public sbyte SByte = -1;
        public short Short = -2;
        public ushort UShort = 2;
        public int Int = -3;
        public uint UInt = 3;
        public long Long = -4;
        public ulong ULong = 4;
        public char Char = 'A';
        public DateTime DateTime = new DateTime(2000, 1, 1);
        public float Float = 1.1F;
        public double Double = 2.2;
        public decimal Decimal = 3.3M;
        public Guid Guid = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

        public bool? BoolNotNull = true;
        public byte? ByteNotNull = 1;
        public sbyte? SByteNotNull = -1;
        public short? ShortNotNull = -2;
        public ushort? UShortNotNull = 2;
        public int? IntNotNull = -3;
        public uint? UIntNotNull = 3;
        public long? LongNotNull = -4;
        public ulong? ULongNotNull = 4;
        public char? CharNotNull = 'A';
        public DateTime? DateTimeNotNull = new DateTime(2000, 1, 1);
        public float? FloatNotNull = 1.1F;
        public double? DoubleNotNull = 2.2;
        public decimal? DecimalNotNull = 3.3M;
        public Guid? GuidNotNull = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

        public bool? BoolNull = null;
        public byte? ByteNull = null;
        public sbyte? SByteNull = null;
        public short? ShortNull = null;
        public ushort? UShortNull = null;
        public int? IntNull = null;
        public uint? UIntNull = null;
        public long? LongNull = null;
        public ulong? ULongNull = null;
        public char? CharNull = null;
        public DateTime? DateTimeNull = null;
        public float? FloatNull = null;
        public double? DoubleNull = null;
        public decimal? DecimalNull = null;
        public Guid? GuidNull = null;

        public string String = "String";
        public string StringPoint = "String";
        public string StringEmpty = "";
        public string StringNull = null;

        protected static readonly enumByte[] enumByteArray = new enumByte[] { enumByte.A, enumByte.B };
        protected static readonly enumSByte[] enumSByteArray = new enumSByte[] { enumSByte.A, enumSByte.B };
        protected static readonly enumShort[] enumShortArray = new enumShort[] { enumShort.A, enumShort.B };
        protected static readonly enumUShort[] enumUShortArray = new enumUShort[] { enumUShort.A, enumUShort.B };
        protected static readonly enumInt[] enumIntArray = new enumInt[] { enumInt.A, enumInt.B };
        protected static readonly enumUInt[] enumUIntArray = new enumUInt[] { enumUInt.A, enumUInt.B };
        protected static readonly enumLong[] enumLongArray = new enumLong[] { enumLong.A, enumLong.B };
        protected static readonly enumULong[] enumULongArray = new enumULong[] { enumULong.A, enumULong.B };
        protected static readonly list<enumByte> enumByteList = new list<enumByte>(enumByteArray);

        public enumByte[] EnumByteArray = enumByteArray;
        public enumSByte[] EnumSByteArray = enumSByteArray;
        public enumShort[] EnumShortArray = enumShortArray;
        public enumUShort[] EnumUShortArray = enumUShortArray;
        public enumInt[] EnumIntArray = enumIntArray;
        public enumUInt[] EnumUIntArray = enumUIntArray;
        public enumLong[] EnumLongArray = enumLongArray;
        public enumULong[] EnumULongArray = enumULongArray;
        public list<enumByte> EnumByteList = enumByteList;

        public enumByte[] EnumByteArrayPoint = enumByteArray;
        public enumSByte[] EnumSByteArrayPoint = enumSByteArray;
        public enumShort[] EnumShortArrayPoint = enumShortArray;
        public enumUShort[] EnumUShortArrayPoint = enumUShortArray;
        public enumInt[] EnumIntArrayPoint = enumIntArray;
        public enumUInt[] EnumUIntArrayPoint = enumUIntArray;
        public enumLong[] EnumLongArrayPoint = enumLongArray;
        public enumULong[] EnumULongArrayPoint = enumULongArray;
        public list<enumByte> EnumByteListPoint = enumByteList;

        public enumByte[] EnumByteArrayEmpty = new enumByte[0];
        public enumSByte[] EnumSByteArrayEmpty = new enumSByte[0];
        public enumShort[] EnumShortArrayEmpty = new enumShort[0];
        public enumUShort[] EnumUShortArrayEmpty = new enumUShort[0];
        public enumInt[] EnumIntArrayEmpty = new enumInt[0];
        public enumUInt[] EnumUIntArrayEmpty = new enumUInt[0];
        public enumLong[] EnumLongArrayEmpty = new enumLong[0];
        public enumULong[] EnumULongArrayEmpty = new enumULong[0];

        public enumByte[] EnumByteArrayNull = null;
        public enumSByte[] EnumSByteArrayNull = null;
        public enumShort[] EnumShortArrayNull = null;
        public enumUShort[] EnumUShortArrayNull = null;
        public enumInt[] EnumIntArrayNull = null;
        public enumUInt[] EnumUIntArrayNull = null;
        public enumLong[] EnumLongArrayNull = null;
        public enumULong[] EnumULongArrayNull = null;
        public list<enumByte> EnumByteListNull = null;

        protected static readonly bool[] boolArray = new bool[] { true, false };
        protected static readonly byte[] byteArray = "ByteArray".getBytes();
        protected static readonly sbyte[] sByteArray = "SByteArray".getArray(value => (sbyte)value);
        protected static readonly short[] shortArray = "ShortArray".getArray(value => (short)value);
        protected static readonly ushort[] uShortArray = "UShortArray".getArray(value => (ushort)value);
        protected static readonly int[] intArray = "IntArray".getArray(value => (int)value);
        protected static readonly uint[] uIntArray = "UIntArray".getArray(value => (uint)value);
        protected static readonly long[] longArray = "LongArray".getArray(value => (long)value);
        protected static readonly ulong[] uLongArray = "ULongArray".getArray(value => (ulong)value);
        protected static readonly char[] charArray = "CharArray".ToCharArray();
        protected static readonly DateTime[] dateTimeArray = new DateTime[] { new DateTime(2000, 1, 1), new DateTime(2010, 1, 1) };
        protected static readonly float[] floatArray = "FloatArray".getArray(value => (float)value);
        protected static readonly double[] doubleArray = "DoubleArray".getArray(value => (double)value);
        protected static readonly decimal[] decimalArray = "DecimalArray".getArray(value => (decimal)value);
        protected static readonly Guid[] guidArray = new Guid[] { new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11), new Guid(11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21) };
        protected static readonly list<bool> boolList = new list<bool>(boolArray);
        protected static readonly list<byte> byteList = new list<byte>(byteArray);
        protected static readonly list<sbyte> sByteList = new list<sbyte>(sByteArray);
        protected static readonly list<DateTime> dateTimeList = new list<DateTime>(dateTimeArray);

        public bool[] BoolArray = boolArray;
        public byte[] ByteArray = byteArray;
        public sbyte[] SByteArray = sByteArray;
        public short[] ShortArray = shortArray;
        public ushort[] UShortArray = uShortArray;
        public int[] IntArray = intArray;
        public uint[] UIntArray = uIntArray;
        public long[] LongArray = longArray;
        public ulong[] ULongArray = uLongArray;
        public char[] CharArray = charArray;
        public DateTime[] DateTimeArray = dateTimeArray;
        public float[] FloatArray = floatArray;
        public double[] DoubleArray = doubleArray;
        public decimal[] DecimalArray = decimalArray;
        public Guid[] GuidArray = guidArray;
        public list<bool> BoolList = boolList;
        public list<byte> ByteList = byteList;
        public list<sbyte> SByteList = sByteList;
        public list<DateTime> DateTimeList = dateTimeList;

        public bool[] BoolArrayPoint = boolArray;
        public byte[] ByteArrayPoint = byteArray;
        public sbyte[] SByteArrayPoint = sByteArray;
        public short[] ShortArrayPoint = shortArray;
        public ushort[] UShortArrayPoint = uShortArray;
        public int[] IntArrayPoint = intArray;
        public uint[] UIntArrayPoint = uIntArray;
        public long[] LongArrayPoint = longArray;
        public ulong[] ULongArrayPoint = uLongArray;
        public char[] CharArrayPoint = charArray;
        public DateTime[] DateTimeArrayPoint = dateTimeArray;
        public float[] FloatArrayPoint = floatArray;
        public double[] DoubleArrayPoint = doubleArray;
        public decimal[] DecimalArrayPoint = decimalArray;
        public Guid[] GuidArrayPoint = guidArray;
        public list<bool> BoolListPoint = boolList;
        public list<byte> ByteListPoint = byteList;
        public list<sbyte> SByteListPoint = sByteList;
        public list<DateTime> DateTimeListPoint = dateTimeList;

        public bool[] BoolArrayEmpty = new bool[0];
        public byte[] ByteArrayEmpty = new byte[0];
        public sbyte[] SByteArrayEmpty = new sbyte[0];
        public short[] ShortArrayEmpty = new short[0];
        public ushort[] UShortArrayEmpty = new ushort[0];
        public int[] IntArrayEmpty = new int[0];
        public uint[] UIntArrayEmpty = new uint[0];
        public long[] LongArrayEmpty = new long[0];
        public ulong[] ULongArrayEmpty = new ulong[0];
        public char[] CharArrayEmpty = new char[0];
        public DateTime[] DateTimeArrayEmpty = new DateTime[0];
        public float[] FloatArrayEmpty = new float[0];
        public double[] DoubleArrayEmpty = new double[0];
        public decimal[] DecimalArrayEmpty = new decimal[0];
        public Guid[] GuidArrayEmpty = new Guid[0];

        public bool[] BoolArrayNull = null;
        public byte[] ByteArrayNull = null;
        public sbyte[] SByteArrayNull = null;
        public short[] ShortArrayNull = null;
        public ushort[] UShortArrayNull = null;
        public int[] IntArrayNull = null;
        public uint[] UIntArrayNull = null;
        public long[] LongArrayNull = null;
        public ulong[] ULongArrayNull = null;
        public char[] CharArrayNull = null;
        public DateTime[] DateTimeArrayNull = null;
        public float[] FloatArrayNull = null;
        public double[] DoubleArrayNull = null;
        public decimal[] DecimalArrayNull = null;
        public Guid[] GuidArrayNull = null;
        public list<bool> BoolListNull = null;
        public list<byte> ByteListNull = null;
        public list<sbyte> SByteListNull = null;
        public list<DateTime> DateTimeListNull = null;

        protected static readonly bool?[] boolNullArray = boolArray.getArray(value => (bool?)value).getAdd(null);
        protected static readonly byte?[] byteNullArray = byteArray.getArray(value => (byte?)value).getAdd(null);
        protected static readonly sbyte?[] sByteNullArray = sByteArray.getArray(value => (sbyte?)value).getAdd(null);
        protected static readonly short?[] shortNullArray = shortArray.getArray(value => (short?)value).getAdd(null);
        protected static readonly ushort?[] uShortNullArray = uShortArray.getArray(value => (ushort?)value).getAdd(null);
        protected static readonly int?[] intNullArray = intArray.getArray(value => (int?)value).getAdd(null);
        protected static readonly uint?[] uIntNullArray = uIntArray.getArray(value => (uint?)value).getAdd(null);
        protected static readonly long?[] longNullArray = longArray.getArray(value => (long?)value).getAdd(null);
        protected static readonly ulong?[] uLongNullArray = uLongArray.getArray(value => (ulong?)value).getAdd(null);
        protected static readonly char?[] charNullArray = charArray.getArray(value => (char?)value).getAdd(null);
        protected static readonly DateTime?[] dateTimeNullArray = dateTimeArray.getArray(value => (DateTime?)value).getAdd(null);
        protected static readonly float?[] floatNullArray = floatArray.getArray(value => (float?)value).getAdd(null);
        protected static readonly double?[] doubleNullArray = doubleArray.getArray(value => (double?)value).getAdd(null);
        protected static readonly decimal?[] decimalNullArray = decimalArray.getArray(value => (decimal?)value).getAdd(null);
        protected static readonly Guid?[] guidNullArray = guidArray.getArray(value => (Guid?)value).getAdd(null);
        protected static readonly list<bool?> boolNullList = new list<bool?>(boolNullArray);
        protected static readonly list<byte?> byteNullList = new list<byte?>(byteNullArray);

        public bool?[] BoolNullArray = boolNullArray;
        public byte?[] ByteNullArray = byteNullArray;
        public sbyte?[] SByteNullArray = sByteNullArray;
        public short?[] ShortNullArray = shortNullArray;
        public ushort?[] UShortNullArray = uShortNullArray;
        public int?[] IntNullArray = intNullArray;
        public uint?[] UIntNullArray = uIntNullArray;
        public long?[] LongNullArray = longNullArray;
        public ulong?[] ULongNullArray = uLongNullArray;
        public char?[] CharNullArray = charNullArray;
        public DateTime?[] DateTimeNullArray = dateTimeNullArray;
        public float?[] FloatNullArray = floatNullArray;
        public double?[] DoubleNullArray = doubleNullArray;
        public decimal?[] DecimalNullArray = decimalNullArray;
        public Guid?[] GuidNullArray = guidNullArray;
        public list<bool?> BoolNullList = boolNullList;
        public list<byte?> ByteNullList = byteNullList;

        public bool?[] BoolNullArrayPoint = boolNullArray;
        public byte?[] ByteNullArrayPoint = byteNullArray;
        public sbyte?[] SByteNullArrayPoint = sByteNullArray;
        public short?[] ShortNullArrayPoint = shortNullArray;
        public ushort?[] UShortNullArrayPoint = uShortNullArray;
        public int?[] IntNullArrayPoint = intNullArray;
        public uint?[] UIntNullArrayPoint = uIntNullArray;
        public long?[] LongNullArrayPoint = longNullArray;
        public ulong?[] ULongNullArrayPoint = uLongNullArray;
        public char?[] CharNullArrayPoint = charNullArray;
        public DateTime?[] DateTimeNullArrayPoint = dateTimeNullArray;
        public float?[] FloatNullArrayPoint = floatNullArray;
        public double?[] DoubleNullArrayPoint = doubleNullArray;
        public decimal?[] DecimalNullArrayPoint = decimalNullArray;
        public Guid?[] GuidNullArrayPoint = guidNullArray;
        public list<bool?> BoolNullListPoint = boolNullList;
        public list<byte?> ByteNullListPoint = byteNullList;

        public bool?[] BoolNullArrayEmpty = new bool?[0];
        public byte?[] ByteNullArrayEmpty = new byte?[0];
        public sbyte?[] SByteNullArrayEmpty = new sbyte?[0];
        public short?[] ShortNullArrayEmpty = new short?[0];
        public ushort?[] UShortNullArrayEmpty = new ushort?[0];
        public int?[] IntNullArrayEmpty = new int?[0];
        public uint?[] UIntNullArrayEmpty = new uint?[0];
        public long?[] LongNullArrayEmpty = new long?[0];
        public ulong?[] ULongNullArrayEmpty = new ulong?[0];
        public char?[] CharNullArrayEmpty = new char?[0];
        public DateTime?[] DateTimeNullArrayEmpty = new DateTime?[0];
        public float?[] FloatNullArrayEmpty = new float?[0];
        public double?[] DoubleNullArrayEmpty = new double?[0];
        public decimal?[] DecimalNullArrayEmpty = new decimal?[0];
        public Guid?[] GuidNullArrayEmpty = new Guid?[0];

        public bool?[] BoolNullArrayNull = null;
        public byte?[] ByteNullArrayNull = null;
        public sbyte?[] SByteNullArrayNull = null;
        public short?[] ShortNullArrayNull = null;
        public ushort?[] UShortNullArrayNull = null;
        public int?[] IntNullArrayNull = null;
        public uint?[] UIntNullArrayNull = null;
        public long?[] LongNullArrayNull = null;
        public ulong?[] ULongNullArrayNull = null;
        public char?[] CharNullArrayNull = null;
        public DateTime?[] DateTimeNullArrayNull = null;
        public float?[] FloatNullArrayNull = null;
        public double?[] DoubleNullArrayNull = null;
        public decimal?[] DecimalNullArrayNull = null;
        public Guid?[] GuidNullArrayNull = null;
        public list<bool?> BoolNullListNull = null;
        public list<byte?> ByteNullListNull = null;

        protected static readonly string[] stringArray = new string[] { null, "String", "StringArray", null };
        protected static readonly list<string> stringList = new list<string>(stringArray);

        public string[] StringArray = stringArray;
        public string[] StringArrayPoint = stringArray;
        public string[] StringArrayEmpty = new string[0];
        public string[] StringArrayNull = null;
        public list<string> StringList = stringList;
        public list<string> StringListPoint = stringList;
        public list<string> StringListNull = null;
    }
}
