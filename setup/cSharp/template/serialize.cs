using System;
using fastCSharp;

namespace fastCSharp.setup.cSharp.template
{
    class serialize : pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : /*NOTE*/copy.TypeNameDefinition, /*NOTE*/
            fastCSharp.setup.cSharp.serialize.ISerialize<@type.FullName/**/.memberMap>/*IF:Attribute.IsStreamSerialize*/, fastCSharp.setup.cSharp.serialize.IStreamSerialize<@type.FullName/**/.memberMap> /*IF:Attribute.IsStreamSerialize*/
        {
            #region PART BODY
            /// <summary>
            /// 序列化
            /// </summary>
            unsafe class serializer : fastCSharp.setup.cSharp.serialize.dataSerializer
            {
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">序列化流</param>
                /// <param name="memberMap">成员位图接口</param>
                public serializer(memoryStream stream, fastCSharp.setup.cSharp.IMemberMap memberMap) : base(stream, memberMap) { }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="parentSerializer">序列化</param>
                /// <param name="memberMap">成员位图接口</param>
                public serializer(fastCSharp.setup.cSharp.serialize.dataSerializer parentSerializer)
                    : base(parentSerializer, default(@type.FullName/**/.memberMap)) { }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name="value">对象</param>
                #region IF type.IsNull
                /// <param name="isPoint">是否记录历史指针</param>
                #endregion IF type.IsNull
                public void Serialize(@type.FullName value/*IF:type.IsNull*/, bool isPoint/*IF:type.IsNull*/)
                {
                    #region IF type.IsNull
                    if (value != null)
                    #endregion IF type.IsNull
                    {
                        #region IF type.IsNull
                        if (isPoint) points[value] = dataStream.Length - streamStartIndex;
                        #endregion IF type.IsNull
                        versionMemerMap(@SerializeAttribute.Version);
                        serialize(value);
                    }
                    #region IF type.IsNull
                    else dataStream.Write(fastCSharp.setup.cSharp.serialize.NullValue);
                    #endregion IF type.IsNull
                }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name="value">对象</param>
                private void serialize(@type.FullName value)
                {
                    memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                    int length = memberMap.SerializeSize;
                    dataStream.PrepLength(length + @SerializeSize);
                    fixed (byte* dataFixed = dataStream.Array)
                    {
                        write = dataFixed + dataStream.Length;
                        fixedMap nullMap = new fixedMap(write);
                        fastCSharp.unsafer.memory.Fill(write, (uint)0, length >> 2);
                        write += length;
                        if (memberMap.IsDefault)
                        {
                            #region LOOP Members
                            #region NAME MemberSerialize
                            #region IF MemberType.SerializeType.IsNull
                            if (/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName == null) nullMap.Set(@MemberIndex);
                            #region IF MemberType.IsMemberSerialize
                            else
                            #endregion IF MemberType.IsMemberSerialize
                            #endregion IF MemberType.SerializeType.IsNull
                            #region IF MemberType.IsMemberSerialize
                            {
                                *(@MemberType.SerializeType.StructNotNullType*)write = (@MemberType.SerializeType.StructNotNullType)/*NOTE*/(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName;
                                write += sizeof(@MemberType.SerializeType.StructNotNullType);
                            }
                            #endregion IF MemberType.IsMemberSerialize
                            #endregion NAME MemberSerialize
                            #endregion LOOP Members
                        }
                        else
                        {
                            #region LOOP Members
                            if (memberMap.IsMember(@MemberIndex))
                            {
                                #region FROMNAME MemberSerialize
                                #endregion FROMNAME MemberSerialize
                            }
                            #endregion LOOP Members
                        }
                        unsafeStream.AddLength(((int)(write - dataFixed - dataStream.Length) + 3) & (int.MaxValue - 3));
                    }
                    if (memberMap.IsDefault)
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        #region NAME SerializeReference
                        #region IF MemberType.SerializeType.IsString
                        serializeString(/*NOT:MemberType.IsSerializeType*/(string)/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsString
                        #region IF MemberType.SerializeType.IsByteArray
                        byteArray(/*NOT:MemberType.IsSerializeType*/(byte[])/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsByteArray
                        #region IF MemberType.IsISerialize
                        #region IF MemberType.SerializeType.IsNull
                        iSerialize(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsNull
                        #region NOT MemberType.SerializeType.IsNull
                        iSerializeNotNull(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.IsNull
                        #endregion IF MemberType.IsISerialize
                        #region IF MemberType.SerializeType.IsSerializeArray
                        {
                            @MemberType.SerializeType.EnumerableArgumentType.FullName[] enumerable = /*NOTE*/(MemberType.SerializeType.EnumerableArgumentType.FullName[])(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName;
                            if (checkPoint(enumerable))
                            {
                                @MemberType.SerializeType.EnumerableArgumentType.SerializeType.FullName[] array = enumerable/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/.getArray(arrayValue => (@MemberType.SerializeType.EnumerableArgumentType.SerializeType.FullName)arrayValue)/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/;
                                #region PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                                #region IF IsMemberSerialize
                                #region IF IsBool
                                unsafeStream.PrepSerialize(/*NOTE*/(bool[])(object)/*NOTE*/array);
                                #endregion IF IsBool
                                #region NOT IsBool
                                #region IF NullType
                                nullArrayNoPoint(/*NOTE*/(byte?[])(object)/*NOTE*/array);
                                #endregion IF NullType
                                #region NOT NullType
                                #region IF IsSerializeBlockCopy
                                unsafeStream.PrepSerialize(array, sizeof(@StructNotNullType));
                                #endregion IF IsSerializeBlockCopy
                                #region IF IsDateTime
                                unsafeStream.PrepSerialize(/*NOTE*/(DateTime[])(object)/*NOTE*/array);
                                #endregion IF IsDateTime
                                #region IF IsDecimal
                                unsafeStream.PrepSerialize(/*NOTE*/(decimal[])(object)/*NOTE*/array);
                                #endregion IF IsDecimal
                                #region IF IsGuid
                                unsafeStream.PrepSerialize(/*NOTE*/(Guid[])(object)/*NOTE*/array);
                                #endregion IF IsGuid
                                #endregion NOT NullType
                                #endregion NOT IsBool
                                #endregion IF IsMemberSerialize
                                #region IF IsString
                                stringArrayNoPoint(/*NOTE*/(string[])(object)/*NOTE*/array);
                                #endregion IF IsString
                                #region IF IsISerialize
                                #region IF IsNull
                                iSerializeArrayNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize[])(object)/*NOTE*/array);
                                #endregion IF IsNull
                                #region NOT IsNull
                                iSerializeArrayNotNullNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize[])(object)/*NOTE*/array);
                                #endregion NOT IsNull
                                #endregion IF IsISerialize

                                #region IF IsSerializeArrayUnknown
                                #region IF IsNull
                                #region IF NullType
                                unknownArrayNull(/*NOTE*/(int?[])(object)/*NOTE*/array);
                                #endregion IF NullType
                                #region NOT NullType
                                unknownArray(array);
                                #endregion NOT NullType
                                #endregion IF IsNull
                                #region NOT IsNull
                                unknownArrayNotNull(array);
                                #endregion NOT IsNull
                                #endregion IF IsSerializeArrayUnknown
                                #endregion PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                            }
                        }
                        #endregion IF MemberType.SerializeType.IsSerializeArray
                        #region IF MemberType.IsSerializeUnknown
                        #region IF MemberType.SerializeType.IsNull
                        #region IF MemberType.SerializeType.NullType
                        unknownNull(/*NOTE*/(int?)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.NullType
                        #region NOT MemberType.SerializeType.NullType
                        unknown(/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.NullType
                        #endregion IF MemberType.SerializeType.IsNull
                        #region NOT MemberType.SerializeType.IsNull
                        unknownNotNull(/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.IsNull
                        #endregion IF MemberType.IsSerializeUnknown
                        #endregion NAME SerializeReference
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                    else
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        if (memberMap.IsMember(@MemberIndex))
                        {
                            #region FROMNAME SerializeReference
                            #endregion FROMNAME SerializeReference
                        }
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <returns>序列化数据</returns>
            public byte[] Serialize()
            {
                using (memoryStream stream = new memoryStream())
                {
                    Serialize(stream, default(@type.FullName/**/.memberMap));
                    return stream.ToArray();
                }
                #region NOTE
                //using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                //{
                //    Serialize(stream, default(@type.FullName/**/.memberMap));
                //    if (stream.Length != stream.Position) stream.SetLength(stream.Position);
                //    return stream.ToArray();
                //}
                #endregion NOTE
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            public void Serialize(memoryStream stream)
            {
                Serialize(stream, default(@type.FullName/**/.memberMap));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            /// <param name="memberMap">成员位图接口</param>
            public void Serialize(memoryStream stream, @type.FullName/**/.memberMap memberMap)
            {
                serializer serializer = new serializer(stream, memberMap);
                serializer.Serialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this/*IF:type.IsNull*/, true/*IF:type.IsNull*/);
                serializer.Finally();
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="serializer">对象序列化器</param>
            public void Serialize(fastCSharp.setup.cSharp.serialize.dataSerializer parentSerializer)
            {
                new serializer(parentSerializer).Serialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this/*IF:type.IsNull*/, false/*IF:type.IsNull*/);
            }
            #region IF Attribute.IsStreamSerialize
            /// <summary>
            /// 序列化
            /// </summary>
            unsafe class streamSerializer : fastCSharp.setup.cSharp.serialize.streamSerializer
            {
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">序列化流</param>
                /// <param name="memberMap">成员位图接口</param>
                public streamSerializer(System.IO.Stream stream, fastCSharp.setup.cSharp.IMemberMap memberMap)
                    : base(stream, memberMap, @SerializeSize) { }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="parentSerializer">对象序列化器</param>
                public streamSerializer(fastCSharp.setup.cSharp.serialize.streamSerializer parentSerializer)
                    : base(parentSerializer, default(@type.FullName/**/.memberMap), @SerializeSize) { }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name="value">对象</param>
                #region IF type.IsNull
                /// <param name="isPoint">是否记录历史指针</param>
                #endregion IF type.IsNull
                public void Serialize(@type.FullName value/*IF:type.IsNull*/, bool isPoint/*IF:type.IsNull*/)
                {
                    #region IF type.IsNull
                    if (value != null)
                    #endregion IF type.IsNull
                    {
                        fixed (byte* dataFixed = valueData)
                        {
                            dataPoint = dataFixed;
                            #region IF type.IsNull
                            if(isPoint) points[value] = (int)dataStream.Position - streamStartIndex;
                            #endregion IF type.IsNull
                            versionMemerMap(@SerializeAttribute.Version);
                            serialize(value);
                        }
                    }
                    #region IF type.IsNull
                    else dataStream.Write(nullLength, 0, sizeof(int));
                    #endregion IF type.IsNull
                }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name="value">对象</param>
                private void serialize(@type.FullName value)
                {
                    int length = memberMap.SerializeSize;
                    fixedMap nullMap = new fixedMap(dataPoint);
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    write = dataPoint + length;
                    if (memberMap.IsDefault)
                    {
                        #region LOOP Members
                        #region FROMNAME MemberSerialize
                        #endregion FROMNAME MemberSerialize
                        #endregion LOOP Members
                    }
                    else
                    {
                        #region LOOP Members
                        if (memberMap.IsMember(@MemberIndex))
                        {
                            #region FROMNAME MemberSerialize
                            #endregion FROMNAME MemberSerialize
                        }
                        #endregion LOOP Members
                    }
                    #region Attribute.IsTestCase
                    if ((length = (int)(write - dataPoint) & 3) != 0)
                    {
                        while (length++ != 4) *write++ = 0;
                    }
                    #endregion Attribute.IsTestCase
                    dataStream.Write(valueData, 0, ((int)(write - dataPoint) + 3) & (int.MaxValue - 3));
                    if (memberMap.IsDefault)
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        #region NAME StreamSerializeReference
                        #region IF MemberType.SerializeType.IsString
                        serializeString(/*NOT:MemberType.IsSerializeType*/(string)/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsString
                        #region IF MemberType.SerializeType.IsByteArray
                        byteArray(/*NOT:MemberType.IsSerializeType*/(byte[])/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsByteArray
                        #region IF MemberType.IsIStreamSerialize
                        #region IF MemberType.SerializeType.IsNull
                        iSerialize(/*NOTE*/(fastCSharp.setup.cSharp.serialize.IStreamSerialize)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.IsNull
                        #region NOT MemberType.SerializeType.IsNull
                        iSerializeNotNull(/*NOTE*/(fastCSharp.setup.cSharp.serialize.IStreamSerialize)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.IsNull
                        #endregion IF MemberType.IsIStreamSerialize
                        #region IF MemberType.SerializeType.IsSerializeArray
                        {
                            @MemberType.SerializeType.EnumerableArgumentType.FullName[] enumerable = /*NOTE*/(MemberType.SerializeType.EnumerableArgumentType.FullName[])(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName;
                            if (checkPoint(enumerable))
                            {
                                @MemberType.SerializeType.EnumerableArgumentType.SerializeType.FullName[] array = enumerable/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/.getArray(arrayValue => (@MemberType.SerializeType.EnumerableArgumentType.SerializeType.FullName)arrayValue)/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/;
                                #region PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                                #region IF IsMemberSerialize
                                #region IF IsBool
                                serializeNoPoint(/*NOTE*/(bool[])(object)/*NOTE*/array);
                                #endregion IF IsBool
                                #region NOT IsBool
                                #region IF NullType
                                nullArrayNoPoint(/*NOTE*/(byte?[])(object)/*NOTE*/array);
                                #endregion IF NullType
                                #region NOT NullType
                                #region IF IsSerializeBlockCopy
                                serializeNoPoint(/*NOTE*/(int[])(object)/*NOTE*/array, array.Length * sizeof(@StructNotNullType));
                                #endregion IF IsSerializeBlockCopy
                                #region IF IsDateTime
                                serializeNoPoint(/*NOTE*/(DateTime[])(object)/*NOTE*/array);
                                #endregion IF IsDateTime
                                #region IF IsDecimal
                                serializeNoPoint(/*NOTE*/(decimal[])(object)/*NOTE*/array);
                                #endregion IF IsDecimal
                                #region IF IsGuid
                                serializeNoPoint(/*NOTE*/(Guid[])(object)/*NOTE*/array);
                                #endregion IF IsGuid
                                #endregion NOT NullType
                                #endregion NOT IsBool
                                #endregion IF IsMemberSerialize
                                #region IF IsString
                                stringArrayNoPoint(/*NOTE*/(string[])(object)/*NOTE*/array);
                                #endregion IF IsString
                                #region IF IsIStreamSerialize
                                #region IF IsNull
                                iSerializeArrayNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.IStreamSerialize[])(object)/*NOTE*/array);
                                #endregion IF IsNull
                                #region NOT IsNull
                                iSerializeArrayNotNullNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.IStreamSerialize[])(object)/*NOTE*/array);
                                #endregion NOT IsNull
                                #endregion IF IsIStreamSerialize

                                #region IF IsStreamSerializeArrayUnknown
                                #region IF IsNull
                                #region IF NullType
                                unknownArrayNull(/*NOTE*/(int?[])(object)/*NOTE*/array);
                                #endregion IF NullType
                                #region NOT NullType
                                unknownArray(array);
                                #endregion NOT NullType
                                #endregion IF IsNull
                                #region NOT IsNull
                                unknownArrayNotNull(array);
                                #endregion NOT IsNull
                                #endregion IF IsStreamSerializeArrayUnknown
                                #endregion PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                            }
                        }
                        #endregion IF MemberType.SerializeType.IsSerializeArray
                        #region IF MemberType.IsStreamSerializeUnknown
                        #region IF MemberType.SerializeType.IsNull
                        #region IF MemberType.SerializeType.NullType
                        unknownNull(/*NOTE*/(int?)(object)/*NOTE*//*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion IF MemberType.SerializeType.NullType
                        #region NOT MemberType.SerializeType.NullType
                        unknown(/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.NullType
                        #endregion IF MemberType.SerializeType.IsNull
                        #region NOT MemberType.SerializeType.IsNull
                        unknownNotNull(/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*/value.@MemberName);
                        #endregion NOT MemberType.SerializeType.IsNull
                        #endregion IF MemberType.IsStreamSerializeUnknown
                        #endregion NAME StreamSerializeReference
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                    else
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        if (memberMap.IsMember(@MemberIndex))
                        {
                            #region FROMNAME StreamSerializeReference
                            #endregion FROMNAME StreamSerializeReference
                        }
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            public void Serialize(System.IO.Stream stream)
            {
                Serialize(stream, default(@type.FullName/**/.memberMap));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            /// <param name="memberMap">成员位图接口</param>
            public void Serialize(System.IO.Stream stream, @type.FullName/**/.memberMap memberMap)
            {
                streamSerializer streamSerializer = new streamSerializer(stream, memberMap);
                streamSerializer.Serialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this/*IF:type.IsNull*/, true/*IF:type.IsNull*/);
                streamSerializer.Finally();
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name="parentSerializer">对象序列化器</param>
            public void Serialize(fastCSharp.setup.cSharp.serialize.streamSerializer parentSerializer)
            {
                new streamSerializer(parentSerializer).Serialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this/*IF:type.IsNull*/, false/*IF:type.IsNull*/);
            }
            #endregion IF Attribute.IsStreamSerialize
            /// <summary>
            /// 对象反序列化
            /// </summary>
            unsafe class deSerializer : fastCSharp.setup.cSharp.serialize.deSerializer
            {
                /// <summary>
                /// 成员位图接口
                /// </summary>
                public @type.FullName/**/.memberMap MemberMap/*NOTE*/ = null/*NOTE*/;
                /// <summary>
                /// 对象反序列化
                /// </summary>
                /// <param name="data">序列化数据</param>
                public deSerializer(byte[] data) : base(data) { }
                /// <summary>
                /// 对象反序列化
                /// </summary>
                /// <param name="parentDeSerializer">对象反序列化器</param>
                public deSerializer(fastCSharp.setup.cSharp.serialize.deSerializer parentDeSerializer)
                    : base(parentDeSerializer) { }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="value">反序列化目标对象</param>
                /// <param name="startIndex">数据起始位置</param>
                /// <returns>数据对象</returns>
                public @type.FullName DeSerialize(@type.FullName value, int startIndex)
                {
                    fixed (byte* dataFixed = data)
                    {
                        this.dataFixed = dataFixed;
                        dataStart = read = dataFixed + startIndex;
                        dataVersion = *(int*)read;
                        if (dataVersion != (int)fastCSharp.setup.cSharp.serializeVersion.serialize)
                        {
                            fastCSharp.log.Default.Throw("序列化格式版本号不匹配 fastCSharp.setup.cSharp.serializeVersion.serialize[" + ((int)fastCSharp.setup.cSharp.serializeVersion.serialize).toString() + "] != " + dataVersion.toString(), true, false);
                        }
                        dataVersion = *(int*)(read += sizeof(int));
                        #region IF type.IsNull
                        if (dataVersion != fastCSharp.setup.cSharp.serialize.NullValue)
                        #endregion IF type.IsNull
                        {
                            versionMemberMap();
                            value = DeSerializeData(value);
                            checkEnd();
                            return value;
                        }
                    }
                    #region IF type.IsNull
                    return default(@type.FullName);
                    #endregion IF type.IsNull
                }
                /// <summary>
                /// 版本号+成员位图接口
                /// </summary>
                protected override void versionMemberMap()
                {
                    if (dataVersion != @SerializeAttribute.Version)
                    {
                        fastCSharp.log.Default.Throw("序列化版本号错误 @SerializeAttribute.Version != " + dataVersion.toString(), true, false);
                    }
                    read = MemberMap.DeSerialize(read += sizeof(int));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="value">反序列化目标对象</param>
                /// <returns>数据对象</returns>
                public @type.FullName DeSerialize(@type.FullName value)
                {
                    dataVersion = *(int*)read;
                    versionMemberMap();
                    return DeSerializeData(value);
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="value">反序列化目标对象</param>
                /// <returns>数据对象</returns>
                public @type.FullName DeSerializeData(@type.FullName value)
                {
                    fixedMap nullMap = new fixedMap(read);
                    read += MemberMap.SerializeSize;
                    if (MemberMap.IsDefault)
                    {
                        #region LOOP Members
                        #region NAME MemberDeSerialize
                        #region IF MemberType.SerializeType.IsNull
                        if (nullMap.Get(@MemberIndex)) value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/default(@MemberType.SerializeType.FullName);
                        #region IF MemberType.IsMemberSerialize
                        else
                        #endregion IF MemberType.IsMemberSerialize
                        #endregion IF MemberType.SerializeType.IsNull
                        #region IF MemberType.IsMemberSerialize
                        {
                            value.@MemberName = /*IF:MemberType.NotSerializeTypeEnum*/(@MemberType.FullName)/*IF:MemberType.NotSerializeTypeEnum*/(/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/*(@MemberType.SerializeType.StructNotNullType*)read);
                            read += sizeof(@MemberType.SerializeType.StructNotNullType);
                        }
                        #endregion IF MemberType.IsMemberSerialize
                        #endregion NAME MemberDeSerialize
                        #endregion LOOP Members
                    }
                    else
                    {
                        #region LOOP Members
                        if (MemberMap.IsMember(@MemberIndex))
                        {
                            #region FROMNAME MemberDeSerialize
                            #endregion FROMNAME MemberDeSerialize
                        }
                        #endregion LOOP Members
                    }
                    int length = (int)(read - nullMap.Map);
                    if ((length & 3) != 0) read += -length & 3;
                    if (MemberMap.IsDefault)
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        #region NAME DeSerializeReference
                        #region IF MemberType.SerializeType.IsNull
                        if (!nullMap.Get(@MemberIndex))
                        #endregion IF MemberType.SerializeType.IsNull
                        {
                            #region IF MemberType.SerializeType.IsString
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/getString();
                            #endregion IF MemberType.SerializeType.IsString
                            #region IF MemberType.SerializeType.IsByteArray
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*//*NOTE*/(object)/*NOTE*/byteArray();
                            #endregion IF MemberType.SerializeType.IsByteArray
                            #region IF MemberType.IsISerialize
                            #region IF MemberType.SerializeType.IsNull
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/iSerialize(() => /*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)(object)/*NOTE*/new @MemberType.SerializeType.FullName());
                            #endregion IF MemberType.SerializeType.IsNull
                            #region NOT MemberType.SerializeType.IsNull
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/iSerializeNotNull<@MemberType.SerializeType.FullName>(/*NOTE*/0/*NOTE*/);
                            #endregion NOT MemberType.SerializeType.IsNull
                            #endregion IF MemberType.IsISerialize
                            #region IF MemberType.SerializeType.IsSerializeArray
                            {
                                object reference = getPoint();
                                if (reference != null) value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/(@MemberType.SerializeType.FullName)reference;
                                else
                                {
                                    length = *(int*)read;
                                    @MemberType.SerializeType.EnumerableArgumentType.FullName[] enumerable = new @MemberType.SerializeType.EnumerableArgumentType.FullName[length];
                                    points.Add(-(int)(read - dataStart), enumerable);
                                    read += sizeof(int);
                                    #region PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                                    @FullName[] array = new @FullName[length];
                                    #region IF IsMemberSerialize
                                    #region IF IsBool
                                    deSerializeNoPoint(/*NOTE*/(bool[])(object)/*NOTE*/array);
                                    #endregion IF IsBool
                                    #region NOT IsBool
                                    #region IF NullType
                                    nullArrayNoPoint(/*NOTE*/(byte?[])(object)/*NOTE*/array);
                                    #endregion IF NullType
                                    #region NOT NullType
                                    #region IF IsSerializeBlockCopy
                                    deSerializeNoPoint(array, sizeof(@StructNotNullType));
                                    #endregion IF IsSerializeBlockCopy
                                    #region IF IsDateTime
                                    deSerializeNoPoint(/*NOTE*/(DateTime[])(object)/*NOTE*/array);
                                    #endregion IF IsDateTime
                                    #region IF IsDecimal
                                    deSerializeNoPoint(/*NOTE*/(decimal[])(object)/*NOTE*/array);
                                    #endregion IF IsDecimal
                                    #region IF IsGuid
                                    deSerializeNoPoint(/*NOTE*/(Guid[])(object)/*NOTE*/array);
                                    #endregion IF IsGuid
                                    #endregion NOT NullType
                                    #endregion NOT IsBool
                                    #endregion IF IsMemberSerialize
                                    #region IF IsString
                                    stringArrayNoPoint(/*NOTE*/(string[])(object)/*NOTE*/array);
                                    #endregion IF IsString
                                    #region IF IsISerialize
                                    #region IF IsNull
                                    iSerializeArrayNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize[])(object)/*NOTE*/array, () => /*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize)(object)/*NOTE*/new @FullName());
                                    #endregion IF IsNull
                                    #region NOT IsNull
                                    iSerializeArrayNotNullNoPoint(/*NOTE*/(fastCSharp.setup.cSharp.serialize.ISerialize[])(object)/*NOTE*/array);
                                    #endregion NOT IsNull
                                    #endregion IF IsISerialize

                                    #region IF IsSerializeArrayUnknown
                                    #region IF IsNull
                                    #region IF NullType
                                    unknownArrayNull(/*NOTE*/(int?[])(object)/*NOTE*/array);
                                    #endregion IF NullType
                                    #region NOT NullType
                                    unknownArray(array);
                                    #endregion NOT NullType
                                    #endregion IF IsNull
                                    #region NOT IsNull
                                    unknownArrayNotNull(array);
                                    #endregion NOT IsNull
                                    #endregion IF IsSerializeArrayUnknown
                                    #endregion PUSH MemberType.SerializeType.EnumerableArgumentType.SerializeType
                                    Array.Copy(array/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/.getArray(arrayValue => (@MemberType.SerializeType.EnumerableArgumentType.FullName)/*NOTE*/(object)/*NOTE*/arrayValue)/*NOT:MemberType.SerializeType.EnumerableArgumentType.IsSerializeType*/, 0, enumerable, 0, array.Length);
                                    value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/enumerable;
                                }
                            }
                            #endregion IF MemberType.SerializeType.IsSerializeArray
                            #region IF MemberType.IsSerializeUnknown
                            #region IF MemberType.SerializeType.IsNull
                            #region IF MemberType.SerializeType.NullType
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/unknownNull<@MemberType.SerializeType.StructNotNullType>();
                            #endregion IF MemberType.SerializeType.NullType
                            #region NOT MemberType.SerializeType.NullType
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/unknown<@MemberType.SerializeType.FullName>();
                            #endregion NOT MemberType.SerializeType.NullType
                            #endregion IF MemberType.SerializeType.IsNull
                            #region NOT MemberType.SerializeType.IsNull
                            value.@MemberName = /*NOT:MemberType.IsSerializeType*/(@MemberType.FullName)/*NOT:MemberType.IsSerializeType*/unknownNotNull<@MemberType.SerializeType.FullName>();
                            #endregion NOT MemberType.SerializeType.IsNull
                            #endregion IF MemberType.IsSerializeUnknown
                        }
                        #endregion NAME DeSerializeReference
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                    else
                    {
                        #region LOOP Members
                        #region NOT MemberType.IsMemberSerialize
                        if (MemberMap.IsMember(@MemberIndex))
                        {
                            #region FROMNAME DeSerializeReference
                            #endregion FROMNAME DeSerializeReference
                        }
                        #endregion NOT MemberType.IsMemberSerialize
                        #endregion LOOP Members
                    }
                    return value;
                }
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            public bool DeSerialize(byte[] data)
            {
                return DeSerialize(data, 0, out deSerializer.OutEndIndex);
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="data">序列化数据</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            public bool DeSerialize(byte[] data, int startIndex, out int endIndex)
            {
                deSerializer deSerializer = new deSerializer(data);
                @type.FullName value = deSerializer.DeSerialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this, startIndex);
                /*NOT:type.IsNull*/
                CopyFrom(value);/*NOT:type.IsNull*/
                endIndex = deSerializer.EndIndex;
                return /*IF:type.IsNull*/value != null && /*IF:type.IsNull*/endIndex <= data.Length;
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="parentDeSerializer">对象反序列化器</param>
            public unsafe void DeSerialize(fastCSharp.setup.cSharp.serialize.deSerializer parentDeSerializer)
            {
                deSerializer deSerializer = new deSerializer(parentDeSerializer);
                /*NOT:type.IsNull*/
                @type.FullName value = /*NOT:type.IsNull*/deSerializer.DeSerialize(/*NOTE*/(@type.FullName)(object)/*NOTE*/this);
                /*NOT:type.IsNull*/
                CopyFrom(value);/*NOT:type.IsNull*/
                parentDeSerializer.SetRead(deSerializer);
            }
            #endregion PART BODY
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 序列化基本字节数
        /// </summary>
        public const int SerializeSize = 0;
        /// <summary>
        /// 序列化代码生成自定义属性
        /// </summary>
        public static cSharp.serialize SerializeAttribute = null;
        /// <summary>
        /// 序列化类型
        /// </summary>
        public class SerializeType : pub
        {
        }
        /// <summary>
        /// 未知类型转换接口
        /// </summary>
        public class unknownSerializeType : pub
        {
        }
    }
    #endregion NOTE
}
