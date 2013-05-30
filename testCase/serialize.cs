using System;
using System.IO;
using fastCSharp.reflection;

namespace fastCSharp.testCase
{
    [fastCSharp.setup.cSharp.serialize(IsStreamSerialize = true, IsTestCase = true)]
    public partial class serialize : data
    {
        [fastCSharp.setup.cSharp.serialize(IsStreamSerialize = true, IsTestCase = true)]
        public partial class iStreamSerializeClass
        {
            public enumByte EnumByte = enumByte.B;
            public bool? BoolNotNull = true;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                iStreamSerializeClass other = obj as iStreamSerializeClass;
                return other != null && EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly iStreamSerializeClass iStreamSerializeClassValue = new iStreamSerializeClass();
        private static readonly iStreamSerializeClass[] iStreamSerializeClassArray = new iStreamSerializeClass[] { null, iStreamSerializeClassValue, null };
        private static readonly list<iStreamSerializeClass> iStreamSerializeClassList = new list<iStreamSerializeClass>(iStreamSerializeClassArray);

        public iStreamSerializeClass IStreamSerializeClass = iStreamSerializeClassValue;
        public iStreamSerializeClass IStreamSerializeClassPoint = iStreamSerializeClassValue;
        public iStreamSerializeClass IStreamSerializeClassNull = null;
        public iStreamSerializeClass[] IStreamSerializeClassArray = iStreamSerializeClassArray;
        public iStreamSerializeClass[] IStreamSerializeClassArrayPoint = iStreamSerializeClassArray;
        public iStreamSerializeClass[] IStreamSerializeClassArrayEmpty = new iStreamSerializeClass[0];
        public iStreamSerializeClass[] IStreamSerializeClassArrayNull = null;
        public list<iStreamSerializeClass> IStreamSerializeClassList = iStreamSerializeClassList;
        public list<iStreamSerializeClass> IStreamSerializeClassListPoint = iStreamSerializeClassList;
        public list<iStreamSerializeClass> IStreamSerializeClassListNull = null;

        [fastCSharp.setup.cSharp.serialize(IsStreamSerialize = false, IsTestCase = true)]
        public partial class iSerializeClass
        {
            public enumByte EnumByte = enumByte.B;
            public bool? BoolNotNull = true;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                iSerializeClass other = obj as iSerializeClass;
                return other != null && EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly iSerializeClass iSerializeClassValue = new iSerializeClass();
        private static readonly iSerializeClass[] iSerializeClassArray = new iSerializeClass[] { null, iSerializeClassValue, null };
        private static readonly list<iSerializeClass> iSerializeClassList = new list<iSerializeClass>(iSerializeClassArray);

        public iSerializeClass ISerializeClass = iSerializeClassValue;
        public iSerializeClass ISerializeClassPoint = iSerializeClassValue;
        public iSerializeClass ISerializeClassNull = null;
        public iSerializeClass[] ISerializeClassArray = iSerializeClassArray;
        public iSerializeClass[] ISerializeClassArrayPoint = iSerializeClassArray;
        public iSerializeClass[] ISerializeClassArrayEmpty = new iSerializeClass[0];
        public iSerializeClass[] ISerializeClassArrayNull = null;
        public list<iSerializeClass> ISerializeClassList = iSerializeClassList;
        public list<iSerializeClass> ISerializeClassListPoint = iSerializeClassList;
        public list<iSerializeClass> ISerializeClassListNull = null;

        [fastCSharp.setup.cSharp.serialize(IsStreamSerialize = true, IsTestCase = true)]
        public partial struct iStreamSerializeStruct
        {
            public enumByte EnumByte;
            public bool? BoolNotNull;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                iStreamSerializeStruct other = (iStreamSerializeStruct)obj;
                return EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly iStreamSerializeStruct iStreamSerializeStructValue = new iStreamSerializeStruct { EnumByte = enumByte.B, BoolNotNull = true };
        private static readonly iStreamSerializeStruct[] iStreamSerializeStructArray = new iStreamSerializeStruct[] { default(iStreamSerializeStruct), iStreamSerializeStructValue };
        private static readonly list<iStreamSerializeStruct> iStreamSerializeStructList = new list<iStreamSerializeStruct>(iStreamSerializeStructArray);

        public iStreamSerializeStruct IStreamSerializeStruct = iStreamSerializeStructValue;
        public iStreamSerializeStruct[] IStreamSerializeStructArray = iStreamSerializeStructArray;
        public iStreamSerializeStruct[] IStreamSerializeStructArrayPoint = iStreamSerializeStructArray;
        public iStreamSerializeStruct[] IStreamSerializeStructArrayEmpty = new iStreamSerializeStruct[0];
        public iStreamSerializeStruct[] IStreamSerializeStructArrayNull = null;
        public list<iStreamSerializeStruct> IStreamSerializeStructList = iStreamSerializeStructList;
        public list<iStreamSerializeStruct> IStreamSerializeStructListPoint = iStreamSerializeStructList;
        public list<iStreamSerializeStruct> IStreamSerializeStructListNull = null;

        [fastCSharp.setup.cSharp.serialize(IsStreamSerialize = false, IsTestCase = true)]
        public partial struct iSerializeStruct
        {
            public enumByte EnumByte;
            public bool? BoolNotNull;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                iSerializeStruct other = (iSerializeStruct)obj;
                return EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly iSerializeStruct iSerializeStructValue = new iSerializeStruct { EnumByte = enumByte.B, BoolNotNull = true };
        private static readonly iSerializeStruct[] iSerializeStructArray = new iSerializeStruct[] { default(iSerializeStruct), iSerializeStructValue };
        private static readonly list<iSerializeStruct> iSerializeStructList = new list<iSerializeStruct>(iSerializeStructArray);

        public iSerializeStruct ISerializeStruct = iSerializeStructValue;
        public iSerializeStruct[] ISerializeStructArray = iSerializeStructArray;
        public iSerializeStruct[] ISerializeStructArrayPoint = iSerializeStructArray;
        public iSerializeStruct[] ISerializeStructArrayEmpty = new iSerializeStruct[0];
        public iSerializeStruct[] ISerializeStructArrayNull = null;
        public list<iSerializeStruct> ISerializeStructList = iSerializeStructList;
        public list<iSerializeStruct> ISerializeStructListPoint = iSerializeStructList;
        public list<iSerializeStruct> ISerializeStructListNull = null;

        [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsTestCase = true)]
        public partial class unknownClass
        {
            public enumByte EnumByte = enumByte.B;
            public bool? BoolNotNull = true;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                unknownClass other = obj as unknownClass;
                return other != null && EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly unknownClass unknownClassValue = new unknownClass();
        private static readonly unknownClass[] unknownClassArray = new unknownClass[] { null, unknownClassValue, null };
        private static readonly list<unknownClass> unknownClassList = new list<unknownClass>(unknownClassArray);

        public unknownClass UnknownClass = unknownClassValue;
        public unknownClass UnknownClassPoint = unknownClassValue;
        public unknownClass UnknownClassNull = null;
        public unknownClass[] UnknownClassArray = unknownClassArray;
        public unknownClass[] UnknownClassArrayPoint = unknownClassArray;
        public unknownClass[] UnknownClassArrayEmpty = new unknownClass[0];
        public unknownClass[] UnknownClassArrayNull = null;
        public list<unknownClass> UnknownClassList = unknownClassList;
        public list<unknownClass> UnknownClassListPoint = unknownClassList;
        public list<unknownClass> UnknownClassListNull = null;

        [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsTestCase = true)]
        public partial struct unknownStruct
        {
            public enumByte EnumByte;
            public bool? BoolNotNull;
            public override int GetHashCode()
            {
                return EnumByte.GetHashCode() ^ BoolNotNull.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                unknownStruct other = (unknownStruct)obj;
                return EnumByte == other.EnumByte && BoolNotNull == other.BoolNotNull;
            }
        }
        private static readonly unknownStruct unknownStructValue = new unknownStruct { EnumByte = enumByte.B, BoolNotNull = true };
        private static readonly unknownStruct[] unknownStructArray = new unknownStruct[] { default(unknownStruct), unknownStructValue };
        private static readonly list<unknownStruct> unknownStructList = new list<unknownStruct>(unknownStructArray);

        public unknownStruct UnknownStruct = unknownStructValue;
        public unknownStruct[] UnknownStructArray = unknownStructArray;
        public unknownStruct[] UnknownStructArrayPoint = unknownStructArray;
        public unknownStruct[] UnknownStructArrayEmpty = new unknownStruct[0];
        public unknownStruct[] UnknownStructArrayNull = null;
        public list<unknownStruct> UnknownStructList = unknownStructList;
        public list<unknownStruct> UnknownStructListPoint = unknownStructList;
        public list<unknownStruct> UnknownStructListNull = null;

        /// <summary>
        /// 序列化接口测试
        /// </summary>
        /// <returns></returns>
        //[fastCSharp.setup.testCase]
        internal static bool ISerialize()
        {
            serialize value = pub.ClearMember(new serialize()), value1 = new serialize();
            byte[] data1 = value1.Serialize();
            log.Default.Add(data1.joinString(','), false, false);

            serialize value2 = (serialize)value.MemberwiseClone();
            value2.DeSerialize(data1);
            if (!pub.Equals(value1, value2)) throw new Exception("DeSerialize");

            byte[] data2;
            using (MemoryStream stream = new MemoryStream())
            {
                value1.Serialize(stream);
                if (stream.Length != stream.Position) stream.SetLength(stream.Position);
                data2 = stream.ToArray();
            }
            if (!data1.equal(data2)) throw new Exception(@"dataSerializer <> streamSerializer
            " + data1.joinString(',') + @"
            " + data2.joinString(','));


            data2 = fastCSharp.setup.cSharp.serialize.dataSerialize.Get(value1);
            if (!data1.equal(data2)) throw new Exception(@"reflection dataSerialize
            " + data1.joinString(',') + @"
            " + data2.joinString(','));

            data2 = fastCSharp.setup.cSharp.serialize.streamSerialize.Get(value1);
            if (!data1.equal(data2)) throw new Exception(@"reflection streamSerialize
            " + data1.joinString(',') + @"
            " + data2.joinString(','));

            value2 = fastCSharp.setup.cSharp.serialize.deSerialize.Get<serialize>(data1);
            if (!pub.Equals(value1, value2)) throw new Exception("reflection deSerialize");

            return true;
        }
        /// <summary>
        /// 未知类型测试
        /// </summary>
        /// <returns></returns>
        //[fastCSharp.setup.testCase]
        internal static bool Unknown1()
        {
            data value1 = new data();
            value1.Byte = 2;
            byte[] data1 = fastCSharp.setup.cSharp.serialize.dataSerialize.Get(value1);
            log.Default.Add(data1.joinString(','), false, false);

            data value2 = fastCSharp.setup.cSharp.serialize.deSerialize.Get<data>(data1);
            if (!pub.Equals(value1, value2)) throw new Exception("reflection deSerialize");

            byte[] data2 = fastCSharp.setup.cSharp.serialize.streamSerialize.Get(value1);
            if (!data1.equal(data2)) throw new Exception(@"reflection streamSerialize
            " + data1.joinString(',') + @"
            " + data2.joinString(','));

            return true;
        }
        /// <summary>
        /// 未知类型测试
        /// </summary>
        /// <returns></returns>
        //[fastCSharp.setup.testCase]
        internal static bool Unknown2()
        {
            unknownStruct value1 = new unknownStruct();
            value1.BoolNotNull = false;
            byte[] data1 = fastCSharp.setup.cSharp.serialize.dataSerialize.Get(value1);
            log.Default.Add(data1.joinString(','), false, false);

            unknownStruct value2 = fastCSharp.setup.cSharp.serialize.deSerialize.Get<unknownStruct>(data1);
            if (!value1.Equals(value2)) throw new Exception("reflection deSerialize");

            byte[] data2 = fastCSharp.setup.cSharp.serialize.streamSerialize.Get(value1);
            if (!data1.equal(data2)) throw new Exception(@"reflection streamSerialize
            " + data1.joinString(',') + @"
            " + data2.joinString(','));

            return true;
        }
        /// <summary>
        /// 基本类型序列化测试
        /// </summary>
        /// <returns></returns>
        //[fastCSharp.setup.testCase]
        internal static bool MemberSerialize()
        {
            serialize value = new serialize();
            //memberSerialize(value.EnumByte);
            memberSerialize(value.Bool);
            memberSerialize(value.BoolNotNull);
            memberSerialize(value.BoolNull);
            memberSerialize(value.String);
            //memberSerialize(value.EnumByteArray);
            //memberSerialize(value.EnumSByteArray);
            //memberSerialize(value.EnumByteArrayEmpty);
            //memberSerialize(value.EnumSByteArrayEmpty);
            //memberSerialize(value.EnumByteArrayNull);
            //memberSerialize(value.EnumSByteArrayNull);
            memberSerialize(value.BoolArray);
            memberSerialize(value.ByteArray);
            memberSerialize(value.SByteArray);
            memberSerialize(value.DateTimeArray);
            memberSerialize(value.BoolArrayEmpty);
            memberSerialize(value.ByteArrayEmpty);
            memberSerialize(value.SByteArrayEmpty);
            memberSerialize(value.DateTimeArrayEmpty);
            memberSerialize(value.BoolArrayNull);
            memberSerialize(value.ByteArrayNull);
            memberSerialize(value.SByteArrayNull);
            memberSerialize(value.DateTimeArrayNull);
            memberSerialize(value.BoolNullArray);
            memberSerialize(value.ByteNullArray);
            memberSerialize(value.BoolNullArrayEmpty);
            memberSerialize(value.ByteNullArrayEmpty);
            memberSerialize(value.BoolNullArrayNull);
            memberSerialize(value.ByteNullArrayNull);
            memberSerialize(value.StringArray);
            memberSerialize(value.StringArrayEmpty);
            memberSerialize(value.StringArrayNull);
            return true;
        }
        private static void memberSerialize<valueType>(valueType value1)
        {
            byte[] data1 = fastCSharp.setup.cSharp.serialize.dataSerialize.Get(value1);
            log.Default.Add(data1.joinString(','), false, false);

            valueType value2 = fastCSharp.setup.cSharp.serialize.deSerialize.Get<valueType>(data1);
            if (!pub.Equals(value1, value2)) throw new Exception("reflection deSerialize [" + typeof(valueType).fullName() + @"]
" + value1.ToString() + @"
" + value2.ToString());

            byte[] data2 = fastCSharp.setup.cSharp.serialize.streamSerialize.Get(value1);
            if (!data1.equal(data2)) throw new Exception("reflection streamSerialize [" + typeof(valueType).fullName() + @"]
            " + data1.joinString(',') + @"
            " + data2.joinString(','));
        }
    }
}
