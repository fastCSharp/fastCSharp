///本文件由程序自动生成,请不要自行修改
using System;
using fastCSharp;

namespace fastCSharp.setup.cSharp
{
    public partial class urlQuery
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {
            
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name=""geter"">查询字符串获取器</param>
            /// <param name=""memberMap"">成员位图</param>
            private void getUrlQeury(fastCSharp.setup.cSharp.urlQuery.IGetter geter, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
            {
                if (memberMap.IsDefault)
                {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsUrlQueryType)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.Get(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsUrlQueryType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.Get(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.GetValue(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }
                else
                {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                    {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsUrlQueryType)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.Get(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsUrlQueryType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.Get(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    geter.GetValue(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                    }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }
            }
        }");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.urlQuery _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsQuery)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.urlQuery.IQuery<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
        {
            /// <summary>
            /// GET查询字符串获取器
            /// </summary>
            private class queryGetter : fastCSharp.setup.cSharp.urlQuery.queryGetter
            {
                /// <summary>
                /// GET查询字符串获取器
                /// </summary>
                /// <param name=""encoding"">编码格式</param>
                public queryGetter(System.Text.Encoding encoding)
                {
                    this.encoding = encoding;
                    querys = new list<string>(");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = Members;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@").Unsafer;
                }
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name=""encoding"">编码格式</param>
            /// <param name=""memberMap"">成员位图</param>
            /// <returns>查询字符串</returns>
            public string GetQuery(System.Text.Encoding encoding, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
            {
                queryGetter queryGetter = new queryGetter(encoding);
                getUrlQeury(new queryGetter(encoding), memberMap);
                return queryGetter.Query;
            }
        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.urlQuery _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsForm)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.urlQuery.IForm<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
        {
            /// <summary>
            /// POST表单获取器
            /// </summary>
            private class formGetter : fastCSharp.setup.cSharp.urlQuery.formGetter
            {
                /// <summary>
                /// POST表单获取器
                /// </summary>
                public formGetter()
                {
                    Form = new System.Collections.Specialized.NameValueCollection(");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = Members;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@");
                }
            }
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name=""memberMap"">成员位图</param>
            /// <returns>POST表单</returns>
            public System.Collections.Specialized.NameValueCollection GetForm(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
            {
                formGetter formGetter = new formGetter();
                getUrlQeury(formGetter, memberMap);
                return formGetter.Form;
            }
        }");
            }
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class serialize
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : 
            fastCSharp.setup.cSharp.serialize.ISerialize<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsStreamSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", fastCSharp.setup.cSharp.serialize.IStreamSerialize<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap> ");
            }
            _code_.Add(@"
        {");
            stringBuilder _PART_BODY_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
            /// <summary>
            /// 序列化
            /// </summary>
            unsafe class serializer : fastCSharp.setup.cSharp.serialize.dataSerializer
            {
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">序列化流</param>
                /// <param name=""memberMap"">成员位图接口</param>
                public serializer(memoryStream stream, fastCSharp.setup.cSharp.IMemberMap memberMap) : base(stream, memberMap) { }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""parentSerializer"">序列化</param>
                /// <param name=""memberMap"">成员位图接口</param>
                public serializer(fastCSharp.setup.cSharp.serialize.dataSerializer parentSerializer)
                    : base(parentSerializer, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap)) { }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name=""value"">对象</param>");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                /// <param name=""isPoint"">是否记录历史指针</param>");
            }
            _code_.Add(@"
                public void Serialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", bool isPoint");
            }
            _code_.Add(@")
                {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (value != null)");
            }
            _code_.Add(@"
                    {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (isPoint) points[value] = dataStream.Length - streamStartIndex;");
            }
            _code_.Add(@"
                        versionMemerMap(");
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = SerializeAttribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Version.ToString());
                    }
                }
            _code_.Add(@");
                        serialize(value);
                    }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    else dataStream.Write(fastCSharp.setup.cSharp.serialize.NullValue);");
            }
            _code_.Add(@"
                }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name=""value"">对象</param>
                private void serialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
                {
                    memoryStream.unsafer unsafeStream = dataStream.Unsafer;
                    int length = memberMap.SerializeSize;
                    dataStream.PrepLength(length + ");
            _code_.Add(SerializeSize.ToString());
            _code_.Add(@");
                    fixed (byte* dataFixed = dataStream.Array)
                    {
                        write = dataFixed + dataStream.Length;
                        fixedMap nullMap = new fixedMap(write);
                        fastCSharp.unsafer.memory.Fill(write, (uint)0, length >> 2);
                        write += length;
                        if (memberMap.IsDefault)
                        {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if (");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) nullMap.Set(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                *(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)write = (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@")");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                                write += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                            }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                        }
                        else
                        {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                            if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if (");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) nullMap.Set(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                *(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)write = (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@")");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                                write += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                            }");
            }
            _code_.Add(@"
                            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                        }
                        unsafeStream.AddLength(((int)(write - dataFixed - dataStream.Length) + 3) & (int.MaxValue - 3));
                    }
                    if (memberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        serializeString(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(string)");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        byteArray(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(byte[])");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerialize(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerializeNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                            if (checkPoint(enumerable))
                            {
                                ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] array = enumerable");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@";");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array, sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                            }
                        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknown(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        serializeString(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(string)");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        byteArray(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(byte[])");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerialize(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerializeNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                            if (checkPoint(enumerable))
                            {
                                ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] array = enumerable");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@";");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array, sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unsafeStream.PrepSerialize(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                            }
                        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknown(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
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
                    Serialize(stream, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap));
                    return stream.ToArray();
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""stream"">数据流</param>
            public void Serialize(memoryStream stream)
            {
                Serialize(stream, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""stream"">数据流</param>
            /// <param name=""memberMap"">成员位图接口</param>
            public void Serialize(memoryStream stream, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
            {
                serializer serializer = new serializer(stream, memberMap);
                serializer.Serialize(this");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", true");
            }
            _code_.Add(@");
                serializer.Finally();
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""serializer"">对象序列化器</param>
            public void Serialize(fastCSharp.setup.cSharp.serialize.dataSerializer parentSerializer)
            {
                new serializer(parentSerializer).Serialize(this");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", false");
            }
            _code_.Add(@");
            }");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsStreamSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 序列化
            /// </summary>
            unsafe class streamSerializer : fastCSharp.setup.cSharp.serialize.streamSerializer
            {
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">序列化流</param>
                /// <param name=""memberMap"">成员位图接口</param>
                public streamSerializer(System.IO.Stream stream, fastCSharp.setup.cSharp.IMemberMap memberMap)
                    : base(stream, memberMap, ");
            _code_.Add(SerializeSize.ToString());
            _code_.Add(@") { }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""parentSerializer"">对象序列化器</param>
                public streamSerializer(fastCSharp.setup.cSharp.serialize.streamSerializer parentSerializer)
                    : base(parentSerializer, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap), ");
            _code_.Add(SerializeSize.ToString());
            _code_.Add(@") { }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name=""value"">对象</param>");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                /// <param name=""isPoint"">是否记录历史指针</param>");
            }
            _code_.Add(@"
                public void Serialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", bool isPoint");
            }
            _code_.Add(@")
                {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (value != null)");
            }
            _code_.Add(@"
                    {
                        fixed (byte* dataFixed = valueData)
                        {
                            dataPoint = dataFixed;");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if(isPoint) points[value] = (int)dataStream.Position - streamStartIndex;");
            }
            _code_.Add(@"
                            versionMemerMap(");
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = SerializeAttribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Version.ToString());
                    }
                }
            _code_.Add(@");
                            serialize(value);
                        }
                    }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    else dataStream.Write(nullLength, 0, sizeof(int));");
            }
            _code_.Add(@"
                }
                /// <summary>
                /// 对象序列化
                /// </summary>
                /// <param name=""value"">对象</param>
                private void serialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
                {
                    int length = memberMap.SerializeSize;
                    fixedMap nullMap = new fixedMap(dataPoint);
                    fastCSharp.unsafer.memory.Fill(dataPoint, (uint)0, length >> 2);
                    write = dataPoint + length;
                    if (memberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if (");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) nullMap.Set(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                *(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)write = (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@")");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                                write += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                            }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if (");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) nullMap.Set(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                *(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)write = (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@")");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                                write += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                            }");
            }
            _code_.Add(@"
                        }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    #region Attribute.IsTestCase
                    if ((length = (int)(write - dataPoint) & 3) != 0)
                    {
                        while (length++ != 4) *write++ = 0;
                    }
                    #endregion Attribute.IsTestCase
                    dataStream.Write(valueData, 0, ((int)(write - dataPoint) + 3) & (int.MaxValue - 3));
                    if (memberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        serializeString(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(string)");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        byteArray(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(byte[])");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsIStreamSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerialize(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerializeNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                            if (checkPoint(enumerable))
                            {
                                ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] array = enumerable");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@";");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array, array.Length * sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsIStreamSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsStreamSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                            }
                        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsStreamSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknown(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        serializeString(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(string)");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        byteArray(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(byte[])");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsIStreamSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerialize(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        iSerializeNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                            if (checkPoint(enumerable))
                            {
                                ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] array = enumerable");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.SerializeType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@";");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array, array.Length * sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                serializeNoPoint(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsIStreamSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsStreamSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                            }
                        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsStreamSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknown(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        unknownNotNull(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                }
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""stream"">数据流</param>
            public void Serialize(System.IO.Stream stream)
            {
                Serialize(stream, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap));
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""stream"">数据流</param>
            /// <param name=""memberMap"">成员位图接口</param>
            public void Serialize(System.IO.Stream stream, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
            {
                streamSerializer streamSerializer = new streamSerializer(stream, memberMap);
                streamSerializer.Serialize(this");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", true");
            }
            _code_.Add(@");
                streamSerializer.Finally();
            }
            /// <summary>
            /// 对象序列化
            /// </summary>
            /// <param name=""parentSerializer"">对象序列化器</param>
            public void Serialize(fastCSharp.setup.cSharp.serialize.streamSerializer parentSerializer)
            {
                new streamSerializer(parentSerializer).Serialize(this");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", false");
            }
            _code_.Add(@");
            }");
            }
            _code_.Add(@"
            /// <summary>
            /// 对象反序列化
            /// </summary>
            unsafe class deSerializer : fastCSharp.setup.cSharp.serialize.deSerializer
            {
                /// <summary>
                /// 成员位图接口
                /// </summary>
                public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap MemberMap;
                /// <summary>
                /// 对象反序列化
                /// </summary>
                /// <param name=""data"">序列化数据</param>
                public deSerializer(byte[] data) : base(data) { }
                /// <summary>
                /// 对象反序列化
                /// </summary>
                /// <param name=""parentDeSerializer"">对象反序列化器</param>
                public deSerializer(fastCSharp.setup.cSharp.serialize.deSerializer parentDeSerializer)
                    : base(parentDeSerializer) { }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""value"">反序列化目标对象</param>
                /// <param name=""startIndex"">数据起始位置</param>
                /// <returns>数据对象</returns>
                public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" DeSerialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, int startIndex)
                {
                    fixed (byte* dataFixed = data)
                    {
                        this.dataFixed = dataFixed;
                        dataStart = read = dataFixed + startIndex;
                        dataVersion = *(int*)read;
                        if (dataVersion != (int)fastCSharp.setup.cSharp.serializeVersion.serialize)
                        {
                            fastCSharp.log.Default.Throw(""序列化格式版本号不匹配 fastCSharp.setup.cSharp.serializeVersion.serialize["" + ((int)fastCSharp.setup.cSharp.serializeVersion.serialize).toString() + ""] != "" + dataVersion.toString(), true, false);
                        }
                        dataVersion = *(int*)(read += sizeof(int));");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (dataVersion != fastCSharp.setup.cSharp.serialize.NullValue)");
            }
            _code_.Add(@"
                        {
                            versionMemberMap();
                            value = DeSerializeData(value);
                            checkEnd();
                            return value;
                        }
                    }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    return default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");");
            }
            _code_.Add(@"
                }
                /// <summary>
                /// 版本号+成员位图接口
                /// </summary>
                protected override void versionMemberMap()
                {
                    if (dataVersion != ");
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = SerializeAttribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Version.ToString());
                    }
                }
            _code_.Add(@")
                    {
                        fastCSharp.log.Default.Throw(""序列化版本号错误 ");
                {
                    fastCSharp.setup.cSharp.serialize _value1_ = SerializeAttribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Version.ToString());
                    }
                }
            _code_.Add(@" != "" + dataVersion.toString(), true, false);
                    }
                    read = MemberMap.DeSerialize(read += sizeof(int));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""value"">反序列化目标对象</param>
                /// <returns>数据对象</returns>
                public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" DeSerialize(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
                {
                    dataVersion = *(int*)read;
                    versionMemberMap();
                    return DeSerializeData(value);
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""value"">反序列化目标对象</param>
                /// <returns>数据对象</returns>
                public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" DeSerializeData(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
                {
                    fixedMap nullMap = new fixedMap(read);
                    read += MemberMap.SerializeSize;
                    if (MemberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (nullMap.Get(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"default(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.NotSerializeTypeEnum)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"*(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)read);
                            read += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        if (MemberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (nullMap.Get(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"default(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        else");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        {
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.NotSerializeTypeEnum)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"(");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"*(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@"*)read);
                            read += sizeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@");
                        }");
            }
            _code_.Add(@"
                        }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    int length = (int)(read - nullMap.Map);
                    if ((length & 3) != 0) read += -length & 3;
                    if (MemberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (!nullMap.Get(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))");
            }
            _code_.Add(@"
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"getString();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"byteArray();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"iSerialize(() => new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@"());");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"iSerializeNotNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                object reference = getPoint();
                                if (reference != null) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")reference;
                                else
                                {
                                    length = *(int*)read;
                                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[length];
                                    points.Add(-(int)(read - dataStart), enumerable);
                                    read += sizeof(int);");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[] array = new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[length];");
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array, sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    iSerializeArrayNoPoint(array, () => new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"());");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                                    Array.Copy(array");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@", 0, enumerable, 0, array.Length);
                                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"enumerable;
                                }
                            }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknownNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknown<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknownNotNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _code_.Add(@"
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsMemberSerialize)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (MemberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (!nullMap.Get(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))");
            }
            _code_.Add(@"
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"getString();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsByteArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"byteArray();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"iSerialize(() => new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@"());");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"iSerializeNotNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsSerializeArray)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            {
                                object reference = getPoint();
                                if (reference != null) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")reference;
                                else
                                {
                                    length = *(int*)read;
                                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[] enumerable = new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@"[length];
                                    points.Add(-(int)(read - dataStart), enumerable);
                                    read += sizeof(int);");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.SerializeType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.EnumerableArgumentType;
                    if (_value6_ != null)
                    {
                    _value3_ = _value6_.SerializeType;
                    }
                }
                    }
                }
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[] array = new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[length];");
            _if_ = false;
                    if (_value3_.IsMemberSerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                if (!(bool)_value3_.IsBool)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    nullArrayNoPoint(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsSerializeBlockCopy)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array, sizeof(");
            _code_.Add(_value3_.StructNotNullType);
            _code_.Add(@"));");
            }
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsDecimal)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsGuid)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    deSerializeNoPoint(array);");
            }
            }
            }
            }
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    stringArrayNoPoint(array);");
            }
            _if_ = false;
                    if (_value3_.IsISerialize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    iSerializeArrayNoPoint(array, () => new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"());");
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    iSerializeArrayNotNullNoPoint(array);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsSerializeArrayUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArrayNull(array);");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArray(array);");
            }
            }
            _if_ = false;
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                    unknownArrayNotNull(array);");
            }
            }
            }
                }
            _code_.Add(@"
                                    Array.Copy(array");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
                if (!(bool)_value5_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@".getArray(arrayValue => (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.EnumerableArgumentType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
                    }
                }
                    }
                }
            _code_.Add(@")arrayValue)");
            }
            _code_.Add(@", 0, enumerable, 0, array.Length);
                                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"enumerable;
                                }
                            }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsSerializeUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                    if (_value4_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknownNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.StructNotNullType);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (_value4_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknown<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSerializeType)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"unknownNotNull<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.SerializeType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">();");
            }
            }
            _code_.Add(@"
                        }
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    return value;
                }
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name=""data"">序列化数据</param>
            public bool DeSerialize(byte[] data)
            {
                return DeSerialize(data, 0, out deSerializer.OutEndIndex);
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name=""data"">序列化数据</param>
            /// <param name=""startIndex"">起始位置</param>
            /// <param name=""endIndex"">结束位置</param>
            public bool DeSerialize(byte[] data, int startIndex, out int endIndex)
            {
                deSerializer deSerializer = new deSerializer(data);
                ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = deSerializer.DeSerialize(this, startIndex);
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                CopyFrom(value);");
            }
            _code_.Add(@"
                endIndex = deSerializer.EndIndex;
                return ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"value != null && ");
            }
            _code_.Add(@"endIndex <= data.Length;
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name=""parentDeSerializer"">对象反序列化器</param>
            public unsafe void DeSerialize(fastCSharp.setup.cSharp.serialize.deSerializer parentDeSerializer)
            {
                deSerializer deSerializer = new deSerializer(parentDeSerializer);
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = ");
            }
            _code_.Add(@"deSerializer.DeSerialize(this);
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                CopyFrom(value);");
            }
            _code_.Add(@"
                parentDeSerializer.SetRead(deSerializer);
            }");
            _partCodes_["BODY"] = _code_.ToString();
            _code_ = _PART_BODY_;
            _code_.Add(_partCodes_["BODY"]);
            _code_.Add(@"
        }");
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class tcpCall
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _if_ = false;
                if (!(bool)IsAllType)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            stringBuilder _PART_SERVERCALL_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != null)
                    {
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.tcpCall(IsGenericTypeServerMethod = true, IsIgnore = true)]");
            }
            _code_.Add(@"
            internal static class ");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"
            {");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                public static ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.ReturnType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodIndexGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterTypeRefName);
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
                {");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value3_.IsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    return ");
            }
                {
                    fastCSharp.setup.memberType _value4_ = type;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(_value3_.StaticMethodGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value4_ = default(fastCSharp.setup.parameterInfo[]);
                    _value4_ = _value3_.Parameters;
                    if (_value4_ != null)
                    {
                        int _loopIndex4_ = _loopIndex_, _loopCount4_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value4_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value5_ in _value4_)
                        {
            _code_.Add(_value5_.ParameterJoinRefName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex4_;
                        _loopCount_ = _loopCount4_;
                    }
                }
            _code_.Add(@");");
            }
                }
            _code_.Add(@"
                }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public static readonly System.Reflection.MethodInfo ");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@";");
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != null)
                    {
                if (!(bool)_value2_.IsGenericType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                    if (IsAnyGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                static ");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"()
                {
                    System.Collections.Generic.Dictionary<fastCSharp.setup.cSharp.tcpBase.genericMethod, System.Reflection.MethodInfo> genericMethods = fastCSharp.setup.cSharp.tcpCall.GetGenericMethods(typeof(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@" = ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"genericMethods[new fastCSharp.setup.cSharp.tcpBase.genericMethod(""");
            _code_.Add(_value3_.MethodName);
            _code_.Add(@""", ");
                {
                    fastCSharp.setup.memberType[] _value4_ = _value3_.GenericParameters;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.Length.ToString());
                    }
                }
                {
                    fastCSharp.setup.parameterInfo[] _value4_ = default(fastCSharp.setup.parameterInfo[]);
                    _value4_ = _value3_.Parameters;
                    if (_value4_ != null)
                    {
                        int _loopIndex4_ = _loopIndex_, _loopCount4_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value4_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value5_ in _value4_)
                        {
            _code_.Add(@", """);
            _code_.Add(_value5_.ParameterRef);
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.ParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
            _code_.Add(@"""");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex4_;
                        _loopCount_ = _loopCount4_;
                    }
                }
            _code_.Add(@")]");
            }
                }
            _code_.Add(@";");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }");
            }
            }
            _code_.Add(@"
            }
        }");
            _partCodes_["SERVERCALL"] = _code_.ToString();
            _code_ = _PART_SERVERCALL_;
            _code_.Add(_partCodes_["SERVERCALL"]);
            stringBuilder _PART_CLIENTCALL_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
        public static partial class tcpCall
        {
            ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
            {");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                private static readonly byte[] ");
            _code_.Add(_value2_.MethodCommandName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyFullName);
                    }
                }
            _code_.Add(@""");");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpCall _value3_ = _value2_.Attribute;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsClientSynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public static ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterTypeRefName);
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
                {");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value3_ = _value2_.ServiceAttribute;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsSegmentation)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ClientPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.defaultTcpServer.IsServer)
                    {
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        return ");
            }
                {
                    fastCSharp.setup.memberType _value3_ = type;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@".");
            _code_.Add(_value2_.MethodIndexGenericName);
            }
                }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterJoinRefName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@");
                    }");
            }
            }
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"
                    {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = type;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"),");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0");
                {
                    fastCSharp.setup.memberType[] _value3_ = default(fastCSharp.setup.memberType[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.GenericParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.memberType _value4_ in _value3_)
                        {
            _code_.Add(@", typeof(");
            _code_.Add(_value4_.FullName);
            _code_.Add(@")");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"),");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"),");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                if (!(bool)_value4_.IsOut)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                    };");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" _outputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"();
                    if (");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ClientPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.DefaultPool.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", _outputParameter_))
                    {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value5_ = _value2_.Method;
                    if (_value5_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value6_ = _value5_.Method;
                    if (_value6_ != null)
                    {
                    if (_value6_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_outputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                        return ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_outputParameter_.");
            _code_.Add(_value2_.ReturnName);
            }
            _code_.Add(@";
                    }");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ClientPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.DefaultPool.Call");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@")) return;");
            }
            _code_.Add(@"
                    throw new Exception();
                }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpCall _value3_ = _value2_.Attribute;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsClientAsynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public static fastCSharp.net.tcpClient ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@"> _onReturn_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
                {
                    bool _isCall_ = false;
                    try
                    {
                        fastCSharp.setup.cSharp.tcpCall.clientPool _client_ = ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ClientPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.DefaultPool;");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = type;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"),");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0");
                {
                    fastCSharp.setup.memberType[] _value3_ = default(fastCSharp.setup.memberType[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.GenericParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.memberType _value4_ in _value3_)
                        {
            _code_.Add(@", typeof(");
            _code_.Add(_value4_.FullName);
            _code_.Add(@")");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"),");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"),");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                if (!(bool)_value4_.IsOut)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                        };");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" _outputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"();
                        _isCall_ = true;");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        return _client_.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturnGeneric<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@"> { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", (");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@")(object)_outputParameter_);");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericParameter)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        return _client_.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturn<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"> { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", _outputParameter_);");
            }
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        _isCall_ = true;
                        return _client_.Call");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@");");
            }
            _code_.Add(@"
                    }
                    catch (Exception _error_)
                    {
                        fastCSharp.log.Default.Add(_error_, null, false);
                    }
                    finally
                    {
                        if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@"{ IsReturn = false });
                    }
                    return null;
                }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
            }
        }");
            _partCodes_["CLIENTCALL"] = _code_.ToString();
            _code_ = _PART_CLIENTCALL_;
            _code_.Add(_partCodes_["CLIENTCALL"]);
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (IsAllType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            stringBuilder _PART_SERVER_ = _code_;
            _code_ = new stringBuilder();
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsAsynchronous)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public class ");
            _code_.Add(ServiceName);
            _code_.Add(@" : fastCSharp.net.tcpServer
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name=""attribute"">TCP调用服务器端配置信息</param>
            /// <param name=""verify"">TCP验证实例</param>
            public ");
            _code_.Add(ServiceName);
            _code_.Add(@"(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerify verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
            _code_.Add(ServiceName);
            _code_.Add(@"""), verify");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@" ?? new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@")
            {
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@" + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyFullName);
                    }
                }
            _code_.Add(@"""), new keyValue<action<socket, int>, bool>(");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@", ");
            _code_.Add(_value2_.IsInputParameter ? "true" : "false");
            _code_.Add(@")));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                this.onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(onCommands.List);
            }");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@";");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _code_.Add(@"
            private void ");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"(socket _socket_, int _identity_)
            {
                bool _isError_ = false;");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                byte[] _data_ = null;");
            }
            _code_.Add(@"
                try
                {");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"};");
            }
            }
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" _return_ = ");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpCall.InvokeGenericTypeMethod(_inputParameter_.");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@", """);
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"""");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            }
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpCall.InvokeGenericMethod(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.MethodIndexGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterRef);
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@");");
            }
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = (");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")_invokeParameter_[");
            _code_.Add(_value4_.ParameterIndex.ToString());
            _code_.Add(@"];");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            }
            _code_.Add(@"
                    _data_ = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"
                    {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = _return_");
            }
            _code_.Add(@"
                    }).Serialize();");
            }
            _code_.Add(@"
                }
                catch (Exception error)
                {
                    _isError_ = true;
                    fastCSharp.log.Default.Add(error, null, true);
                    if (send(_socket_, fastCSharp.net.tcpServer.status.Error, _identity_)) close(_socket_, _identity_);
                    _socket_.Dispose();
                }
                if (!_isError_)
                {");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_, _data_))");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_))");
            }
            _code_.Add(@"
                            _socket_.Dispose();
                }
            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsAsynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        /// <summary>
        /// TCP调用服务端
        /// </summary>
        public class ");
            _code_.Add(ServiceNameAsynchronous);
            _code_.Add(@" : fastCSharp.net.tcpServerAsynchronous
        {
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name=""attribute"">TCP调用服务器端配置信息</param>
            /// <param name=""verify"">TCP验证实例</param>
            public ");
            _code_.Add(ServiceNameAsynchronous);
            _code_.Add(@"(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
            _code_.Add(ServiceName);
            _code_.Add(@"""), verify");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@" ?? new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@")
            {
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@" + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyFullName);
                    }
                }
            _code_.Add(@"""), new keyValue<action<socket, int>, bool>(");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@", ");
            _code_.Add(_value2_.IsInputParameter ? "true" : "false");
            _code_.Add(@")));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                this.onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(onCommands.List);
            }");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@";");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            private struct ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@"
            {
                public ");
            _code_.Add(ServiceNameAsynchronous);
            _code_.Add(@" Server;
                public socket Socket;
                public int Identity;
                public void Callback(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@" returnValue)
                {");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    byte[] data = null;
                    if (returnValue.IsReturn)
                    {
                        try
                        {
                            data = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" { ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = returnValue.Value }).Serialize();
                        }
                        catch (Exception error)
                        {
                            returnValue.IsReturn = false;
                            fastCSharp.log.Default.Add(error, null, true);
                        }
                    }");
            }
            _code_.Add(@"
                    if (returnValue.IsReturn) Server.send(Socket, Server.receiveCommand, fastCSharp.net.tcpServer.status.Success, Identity");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", data");
            }
            _code_.Add(@");
                    else Server.send(Socket, socket.Close, fastCSharp.net.tcpServer.status.Error, Identity);
                }
            }");
            }
            _code_.Add(@"
            private void ");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"(socket _socket_, int _identity_)
            {");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                bool _isAsync_ = false;");
            }
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                bool _isError_ = false;");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                byte[] _data_ = null;");
            }
            }
            _code_.Add(@"
                try
                {");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            }
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", ");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.GetGenericParameterCallback(_inputParameter_.");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@", ");
            }
            _if_ = false;
                if (!(bool)_value2_.IsGenericParameterCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@">)");
            }
            _code_.Add(@"new ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@" { Server = this, Socket = _socket_, Identity = _identity_ }.Callback");
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@")");
            }
            _code_.Add(@" };");
            }
            _code_.Add(@"
                    _isAsync_ = true;");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    fastCSharp.setup.cSharp.tcpCall.InvokeGenericTypeMethod(_inputParameter_.");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@", """);
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"""");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            }
            _code_.Add(@", _invokeParameter_);");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    fastCSharp.setup.cSharp.tcpCall.InvokeGenericMethod(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@", _invokeParameter_);");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.MethodIndexGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterRef);
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", ");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"new ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@" { Server = this, Socket = _socket_, Identity = _identity_ }.Callback);");
            }
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"};");
            }
            }
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" _return_ = ");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpCall.InvokeGenericTypeMethod(_inputParameter_.");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@", """);
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"""");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            }
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpCall.InvokeGenericMethod(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.MethodIndexGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterRef);
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@");");
            }
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = (");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")_invokeParameter_[");
            _code_.Add(_value4_.ParameterIndex.ToString());
            _code_.Add(@"];");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            }
            _code_.Add(@"
                    _data_ = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"
                    {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = _return_");
            }
            _code_.Add(@"
                    }).Serialize();");
            }
            }
            _code_.Add(@"
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, null, true);");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (!_isAsync_)");
            }
            _code_.Add(@"
                    {");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        _isError_ = true;");
            }
            _code_.Add(@"
                        send(_socket_, socket.Close, fastCSharp.net.tcpServer.status.Error, _identity_);
                    }
                }");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!_isError_)
                {");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_, _data_);");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_);");
            }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
        }");
            }
            _partCodes_["SERVER"] = _code_.ToString();
            _code_ = _PART_SERVER_;
            _code_.Add(_partCodes_["SERVER"]);
            stringBuilder _PART_CLIENT_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
        public class ");
            _code_.Add(ServiceName);
            _code_.Add(@"
        {");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsSegmentation)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpCall>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(TypeGenericParameterName);
            _code_.Add(@";");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            }
            _code_.Add(@"
            /// <summary>
            /// 默认TCP调用服务器端配置信息
            /// </summary>
            protected internal static readonly fastCSharp.setup.cSharp.tcpServer defaultTcpServer;
            /// <summary>
            /// 客户端TCP调用池
            /// </summary>
            public class clientPool
            {
                /// <summary>
                /// 客户端TCP调用池
                /// </summary>
                public static readonly fastCSharp.setup.cSharp.tcpCall.clientPool ClientPool;
                static clientPool()
                {");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsSegmentation)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (defaultTcpServer.IsServer) fastCSharp.log.Default.Add(""请确认 ");
            _code_.Add(ServiceName);
            _code_.Add(@" 服务器端是否本地调用"");");
            }
            _code_.Add(@"
                    ClientPool = new fastCSharp.setup.cSharp.tcpCall.clientPool(defaultTcpServer, true, """);
            _code_.Add(ServiceName);
            _code_.Add(@"""");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@");
                }
            }
            /// <summary>
            /// 默认客户端TCP调用池
            /// </summary>
            public static fastCSharp.setup.cSharp.tcpCall.clientPool DefaultPool
            {
                get { return clientPool.ClientPool; }
            }
            static ");
            _code_.Add(ServiceName);
            _code_.Add(@"()
            {
                defaultTcpServer = fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
            _code_.Add(ServiceName);
            _code_.Add(@""");");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsSegmentation)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                defaultTcpServer.IsServer = false;");
            }
            _code_.Add(@"
            }
        }");
            _partCodes_["CLIENT"] = _code_.ToString();
            _code_ = _PART_CLIENT_;
            _code_.Add(_partCodes_["CLIENT"]);
            }
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class memberMap
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
            stringBuilder _PART_BODY_ = _code_;
            _code_ = new stringBuilder();
            _if_ = false;
                    if (IsMemberMap)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 成员位图
            /// </summary>
            public struct memberMap : fastCSharp.setup.cSharp.IMemberMap<memberMap>
            {
                /// <summary>
                /// 成员位图
                /// </summary>
                private byte[] map;
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <param name=""members"">成员集合</param>
                public memberMap(params member[] members)
                {
                    map = new byte[((");
            _code_.Add(MaxMemberIndex.ToString());
            _code_.Add(@" >> 5) + 1) << 2];
                    foreach (member member in members) map[(int)member >> 3] |= (byte)(1 << ((int)member & 7));
                }
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <returns>成员位图</returns>
                public memberMap Copy()
                {
                    return new memberMap { map = this.map == null ? null : this.map.copy() };
                }
                /// <summary>
                /// 是否默认全部成员有效
                /// </summary>
                public bool IsDefault
                {
                    get { return map == null; }
                }
                /// <summary>
                /// 序列化字节长度
                /// </summary>
                public int SerializeSize
                {
                    get
                    {
                        return ((");
            _code_.Add(MaxMemberIndex.ToString());
            _code_.Add(@" >> 5) + 1) << 2;
                    }
                }
                /// <summary>
                /// 设置成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    if (map == null) map = new byte[((");
            _code_.Add(MaxMemberIndex.ToString());
            _code_.Add(@" >> 5) + 1) << 2];
                    map[memberIndex >> 3] |= (byte)(1 << (memberIndex & 7));
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name=""member"">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    if (map != null) map[memberIndex >> 3] &= (byte)(byte.MaxValue ^ (1 << (memberIndex & 7)));
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public unsafe void And(memberMap memberMap)
                {
                    if (map != null)
                    {
                        if (memberMap.map != null)
                        {
                            fixed (byte* mapFixed = map, memberFixed = memberMap.map)
                            {
                                for (byte* write = mapFixed, end = mapFixed + map.Length, read = memberFixed; write != end; write += sizeof(uint), read += sizeof(uint))
                                {
                                    *(uint*)write &= *(uint*)read;
                                }
                            }
                        }
                        else map = null;
                    }
                }
                /// <summary>
                /// 成员异或运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public unsafe void Xor(memberMap memberMap)
                {
                    if (map != null)
                    {
                        if (memberMap.map != null)
                        {
                            fixed (byte* mapFixed = map, memberFixed = memberMap.map)
                            {
                                for (byte* write = mapFixed, end = mapFixed + map.Length, read = memberFixed; write != end; write += sizeof(uint), read += sizeof(uint))
                                {
                                    *(uint*)write ^= *(uint*)read;
                                }
                            }
                        }
                        else map = null;
                    }
                }
                /// <summary>
                /// 成员并集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public unsafe void Or(memberMap memberMap)
                {
                    if (map != null)
                    {
                        if (memberMap.map != null)
                        {
                            fixed (byte* mapFixed = map, memberFixed = memberMap.map)
                            {
                                for (byte* write = mapFixed, end = mapFixed + map.Length, read = memberFixed; write != end; write += sizeof(uint), read += sizeof(uint))
                                {
                                    *(uint*)write |= *(uint*)read;
                                }
                            }
                        }
                        else map = null;
                    }
                }
                /// <summary>
                /// 判断成员索引是否有效
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == null || (map[memberIndex >> 3] & (1 << (memberIndex & 7))) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name=""member"">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public unsafe void Serialize(memoryStream stream)
                {
                    fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""data"">数据</param>
                /// <returns>结束位置</returns>
                public unsafe byte* DeSerialize(byte* data)
                {
                    if (map == null) map = new byte[SerializeSize];
                    fixed (byte* mapFixed = map) fastCSharp.unsafer.memory.Copy(data, mapFixed, SerializeSize);
                    return data + SerializeSize;
                }
                /// <summary>
                /// 成员获取器
                /// </summary>
                /// <returns>成员获取器</returns>
                public fastCSharp.setup.cSharp.IMemberInfo MemberInfo
                {
                    get { return new memberInfo(); }
                }
            }");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (Member32 != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 成员位图
            /// </summary>
            public struct ");
            _code_.Add(Member32);
            _code_.Add(@" : fastCSharp.setup.cSharp.IMemberMap<");
            _code_.Add(Member32);
            _code_.Add(@">
            {
                /// <summary>
                /// 成员位图
                /// </summary>
                private uint map;
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <param name=""members"">成员集合</param>
                public ");
            _code_.Add(Member32);
            _code_.Add(@"(params member[] members)
                {
                    map = 0;
                    foreach (member member in members) SetMember((int)member);
                }
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <returns>成员位图</returns>
                public ");
            _code_.Add(Member32);
            _code_.Add(@" Copy()
                {
                    return new ");
            _code_.Add(Member32);
            _code_.Add(@" { map = map };
                }
                /// <summary>
                /// 是否默认全部成员有效
                /// </summary>
                public bool IsDefault
                {
                    get { return map == 0; }
                }
                /// <summary>
                /// 序列化字节长度
                /// </summary>
                public int SerializeSize
                {
                    get
                    {
                        return sizeof(uint);
                    }
                }
                /// <summary>
                /// 设置成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    map |= 1U << (int)memberIndex;
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    map &= uint.MaxValue ^ (1U << (int)memberIndex);
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name=""member"">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void And(");
            _code_.Add(Member32);
            _code_.Add(@" memberMap)
                {
                    map &= memberMap.map;
                }
                /// <summary>
                /// 成员异或运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void Xor(");
            _code_.Add(Member32);
            _code_.Add(@" memberMap)
                {
                    map ^= memberMap.map;
                }
                /// <summary>
                /// 成员并集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void Or(");
            _code_.Add(Member32);
            _code_.Add(@" memberMap)
                {
                    map |= memberMap.map;
                }
                /// <summary>
                /// 判断成员索引是否有效
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == 0 || (map & (1 << memberIndex)) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name=""member"">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public void Serialize(memoryStream stream)
                {
                    stream.Write(map);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    stream.Write(BitConverter.GetBytes(map), 0, sizeof(uint));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""data"">数据</param>
                /// <returns>结束位置</returns>
                public unsafe byte* DeSerialize(byte* data)
                {
                    map = *(uint*)data;
                    return data + sizeof(uint);
                }
                /// <summary>
                /// 成员获取器
                /// </summary>
                /// <returns>成员获取器</returns>
                public fastCSharp.setup.cSharp.IMemberInfo MemberInfo
                {
                    get { return new memberInfo(); }
                }
            }");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (Member64 != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 成员位图
            /// </summary>
            public struct ");
            _code_.Add(Member64);
            _code_.Add(@" : fastCSharp.setup.cSharp.IMemberMap<");
            _code_.Add(Member64);
            _code_.Add(@">
            {
                /// <summary>
                /// 成员位图
                /// </summary>
                private ulong map;
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <param name=""members"">成员集合</param>
                public ");
            _code_.Add(Member64);
            _code_.Add(@"(params member[] members)
                {
                    map = 0;
                    foreach (member member in members) SetMember((int)member);
                }
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <returns>成员位图</returns>
                public ");
            _code_.Add(Member64);
            _code_.Add(@" Copy()
                {
                    return new ");
            _code_.Add(Member64);
            _code_.Add(@" { map = map };
                }
                /// <summary>
                /// 是否默认全部成员有效
                /// </summary>
                public bool IsDefault
                {
                    get { return map == 0; }
                }
                /// <summary>
                /// 序列化字节长度
                /// </summary>
                public int SerializeSize
                {
                    get
                    {
                        return sizeof(ulong);
                    }
                }
                /// <summary>
                /// 设置成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    map |= 1UL << (int)memberIndex;
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    map &= ulong.MaxValue ^ (1UL << (int)memberIndex);
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name=""member"">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name=""memberIndex"">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void And(");
            _code_.Add(Member64);
            _code_.Add(@" memberMap)
                {
                    map &= memberMap.map;
                }
                /// <summary>
                /// 成员异或运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void Xor(");
            _code_.Add(Member64);
            _code_.Add(@" memberMap)
                {
                    map ^= memberMap.map;
                }

                /// <summary>
                /// 成员并集运算,忽略默认成员
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                public void Or(");
            _code_.Add(Member64);
            _code_.Add(@" memberMap)
                {
                    map |= memberMap.map;
                }
                /// <summary>
                /// 判断成员索引是否有效
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == 0 || (map & (1UL << memberIndex)) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name=""member"">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public void Serialize(memoryStream stream)
                {
                    stream.Write(map);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name=""stream"">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    stream.Write(BitConverter.GetBytes(map), 0, sizeof(ulong));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name=""data"">数据</param>
                /// <returns>结束位置</returns>
                public unsafe byte* DeSerialize(byte* data)
                {
                    map = *(ulong*)data;
                    return data + sizeof(ulong);
                }
                /// <summary>
                /// 成员获取器
                /// </summary>
                /// <returns>成员获取器</returns>
                public fastCSharp.setup.cSharp.IMemberInfo MemberInfo
                {
                    get { return new memberInfo(); }
                }
            }");
            }
            _code_.Add(@"

            /// <summary>
            /// 成员
            /// </summary>
            public enum member
            {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericParameter)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                [fastCSharp.setup.cSharp.member(Type = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"))]");
            }
            _code_.Add(@"
                ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@",");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 成员信息获取器
            /// </summary>
            private struct memberInfo : fastCSharp.setup.cSharp.IMemberInfo
            {
                /// <summary>
                /// 获取成员名称
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                /// <returns>成员名称</returns>
                public string GetName(int memberIndex)
                {
                    return ((member)memberIndex).ToString();
                }
                /// <summary>
                /// 获取成员类型
                /// </summary>
                /// <param name=""memberIndex"">成员索引</param>
                /// <returns>成员类型</returns>
                public Type GetType(int memberIndex)
                {
                    return fastCSharp.Enum<member, fastCSharp.setup.cSharp.member>.Array(memberIndex).Type;
                }
                /// <summary>
                /// 所有成员数量
                /// </summary>
                public int MemberCount
                {
                    get
                    {
                        return ");
            _code_.Add(MaxMemberIndex.ToString());
            _code_.Add(@" + 1;
                    }
                }
            }");
            _partCodes_["BODY"] = _code_.ToString();
            _code_ = _PART_BODY_;
            _code_.Add(_partCodes_["BODY"]);
            _code_.Add(@"
        }");
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class ajax
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.ajax _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsToJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.ajax.IToJson<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
        {");
            stringBuilder _PART_TOJSON_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            class toJsoner : fastCSharp.setup.cSharp.ajax.toJsoner
            {
                public toJsoner() : base(");
            _code_.Add(IsPushParent ? "true" : "false");
            _code_.Add(@") { }
                public toJsoner(fastCSharp.setup.cSharp.ajax.toJsoner parent) : base(parent) { }
                /// <summary>
                /// 对象转换成JSON字符串
                /// </summary>
                /// <param name=""value"">对象</param>
                /// <param name=""memberMap"">成员位图接口</param>
                public void ToJson(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (value != null)");
            }
            _code_.Add(@"
                    {");
            _if_ = false;
                    if (IsPushParent)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        Push(value);");
            }
            _code_.Add(@"
                        jsonStream.Write('{');
                        isFirstMember = true;");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.ajax _value3_ = _value2_.ToJsonAttribute;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsToJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                        {
                            name(""");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""");");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            if (value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) jsonStream.Write(fastCSharp.web.ajax.Null);
                            else");
            }
            _code_.Add(@"
                            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsToJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                (value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@").ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                base.value(value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"/**/.ToString());");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.PairKeyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                base.keyValue(value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.KeyValueType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                base.keyValue(value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsToJsonUnknown)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                base.value((");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsEnumerable)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.EnumerableArgumentType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                {
                                    bool isNext = false;
                                    jsonStream.Write('[');
                                    foreach (");
            _code_.Add(_value3_.FullName);
            _code_.Add(@" nextValue in value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@")
                                    {
                                        if (isNext) jsonStream.Write(',');");
            _if_ = false;
                    if (_value3_.IsToJson)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                        if (value != null) (nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        else jsonStream.Write(fastCSharp.web.ajax.Null);");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                        base.value(nextValue.ToString());");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.PairKeyType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                        //(nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        base.keyValue(nextValue);");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.KeyValueType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                        //(nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        base.keyValue(nextValue);");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsToJsonUnknown)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                        {");
            _if_ = false;
                    if (_value3_.IsNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            if (nextValue == null) jsonStream.Write(fastCSharp.web.ajax.Null);
                                            else");
            }
            _code_.Add(@"
                                            {
                                                base.value((");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@")nextValue);
                                            }
                                        }");
            }
            _code_.Add(@"
                                        isNext = true;
                                    }
                                    jsonStream.Write(']');
                                }");
            }
                }
            }
            _code_.Add(@"
                            }
                        }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                        isFirstMember = false;
                        jsonStream.Write('}');");
            _if_ = false;
                    if (IsPushParent)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        parents.Unsafer.AddLength(-1);");
            }
            _code_.Add(@"
                    }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    else jsonStream.Write(fastCSharp.web.ajax.Null);");
            }
            _code_.Add(@"
                }
                /// <summary>
                /// 对象集合转换成JSON字符串
                /// </summary>
                /// <param name=""values"">对象集合</param>
                /// <param name=""memberMap"">成员位图接口</param>");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public void ToJson<JsonValueType>(System.Collections.Generic.IEnumerable<JsonValueType> values, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                    where JsonValueType : ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"
                {");
            _if_ = false;
                    if (IsPushParent)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ToJsonPush<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap, JsonValueType>(values, memberMap, ToJson);");
            }
            _if_ = false;
                if (!(bool)IsPushParent)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ToJson<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap, JsonValueType>(values, memberMap, ToJson);");
            }
            _code_.Add(@"
                }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                public void ToJson(System.Collections.Generic.IEnumerable<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> values, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                {");
            _if_ = false;
                    if (IsPushParent)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ToJsonPush<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>(values, memberMap, ToJson);");
            }
            _if_ = false;
                if (!(bool)IsPushParent)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ToJson<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>(values, memberMap, ToJson);");
            }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <returns>JSON字符串</returns>
            public string ToJson()
            {
                return ToJson(default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap));
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name=""memberMap"">成员位图接口</param>
            /// <returns>JSON字符串</returns>
            public string ToJson(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
            {
                toJsoner json = new toJsoner();
                json.ToJson(this, memberMap);
                return json.ToString();
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name=""jsoner"">对象转换JSON字符串</param>
            public void ToJson(fastCSharp.setup.cSharp.ajax.toJsoner parent)
            {
                new toJsoner(parent).ToJson(this, default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap));
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <param name=""values"">对象集合</param>
            /// <param name=""memberMap"">成员位图接口</param>
            /// <returns>JSON字符串</returns>");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
            public static string ToJson(System.Collections.Generic.IEnumerable<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> values, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))");
            }
            _code_.Add(@"
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            public static string ToJson<JsonValueType>(System.Collections.Generic.IEnumerable<JsonValueType> values, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                where JsonValueType : ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            }
            _code_.Add(@"
            {
                toJsoner json = new toJsoner();
                json.ToJson(values, memberMap);
                return json.ToString();
            }");
            _partCodes_["TOJSON"] = _code_.ToString();
            _code_ = _PART_TOJSON_;
            _code_.Add(_partCodes_["TOJSON"]);
            _code_.Add(@"
        }");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.ajax _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsParseJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.ajax.IParseJson
        {");
            stringBuilder _PART_PARSEJSON_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
            /// <summary>
            /// JSON解析节点转换对象
            /// </summary>
            class jsonParser : fastCSharp.setup.cSharp.ajax.nodeParser
            {
                /// <summary>
                /// 解析对象
                /// </summary>
                /// <param name=""value"">目标对象</param>
                /// <returns>目标对象</returns>
                public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" Parse(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
                {
                    if (dictionary != null)
                    {
                        fastCSharp.setup.cSharp.ajax.jsonNode node;");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _if_ = false;
                    if (IsParseJson)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (dictionary.TryGetValue(currentMemberName = """);
            _code_.Add(_value2_.MemberName);
            _code_.Add(@""", out node))
                        {
                            if (node.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Null) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = default(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@");
                            else
                            {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = node.String;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsChar)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = node.Char;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = node.Bool;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = node.DateTime;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = fastCSharp.Enum.parseEnum<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">(node.String, value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsTryParseJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsTryParseInt)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                if (node.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Int) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")node.Int;
                                else");
            }
            _code_.Add(@"
                                {");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" nullValue;
                                    if (");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@"/**/.TryParse(node.NumberString, out nullValue)) value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = nullValue;
                                    else");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                                        if (!");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.TryParse(node.NumberString, out value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"))");
            }
            _code_.Add(@"
                                        {
                                            fastCSharp.log.Default.Throw(""");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" : "" + currentMemberName + @"" 转换失败
"" + node.NumberString, true, false);
                                        }
                                }");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsParseJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" parseValue = new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"();
                                (parseValue).FromJson(node);
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = parseValue;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.CanParseJson)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                                value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = unknown<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">(node);");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsEnumerable)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.EnumerableArgumentType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.CanParseJsonEnumerable)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                {
                                    if (node.Type != fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.List) fastCSharp.log.Default.Throw(currentMemberName + "" 不可枚举"", true, false);
                                    list<fastCSharp.setup.cSharp.ajax.jsonNode> nodes = node.List;
                                    ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[] array = new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"[nodes.Count];
                                    int arrayIndex = -1;
                                    foreach (fastCSharp.setup.cSharp.ajax.jsonNode arrayNode in nodes)
                                    {
                                        if (arrayNode.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Null) array[++arrayIndex] = default(");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@");
                                        else
                                        {");
            _if_ = false;
                    if (_value3_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            array[++arrayIndex] = arrayNode.String;");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsChar)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            array[++arrayIndex] = arrayNode.Char;");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsBool)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            array[++arrayIndex] = arrayNode.Bool;");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsDateTime)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            array[++arrayIndex] = arrayNode.DateTime;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                            array[++arrayIndex] = fastCSharp.Enum.parseEnum<");
            _code_.Add(_value3_.FullName);
            _code_.Add(@">(arrayNode.String, default(");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"));");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsTryParseJson)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value3_.IsTryParseInt)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            if (arrayNode.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Int) array[++arrayIndex] = (");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@")arrayNode.Int;
                                            else");
            }
            _code_.Add(@"
                                            {");
            _if_ = false;
                    if (_value3_.NullType != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                                ");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@" arrayNullValue;
                                                ++arrayIndex;
                                                if (");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.NotNullType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@"/**/.TryParse(arrayNode.NumberString, out arrayNullValue)) array[arrayIndex] = arrayNullValue;
                                                else");
            }
            _if_ = false;
                if (_value3_.NullType == null)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                                    if (!");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"/**/.TryParse(arrayNode.NumberString, out array[++arrayIndex]))");
            }
            _code_.Add(@"
                                                    {
                                                        fastCSharp.log.Default.Throw(""");
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@" : "" + currentMemberName + @"" 转换失败
"" + arrayNode.NumberString, true, false);
                                                    }
                                            }");
            }
            _code_.Add(@"
");
            _if_ = false;
                    if (_value3_.IsParseJson)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                                            ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@" arrayValue = new ");
            _code_.Add(_value3_.FullName);
            _code_.Add(@"();
                                            (arrayValue).FromJson(arrayNode);
                                            array[++arrayIndex] = arrayValue;");
            }
            _code_.Add(@"
                                        }
                                    }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsArrayOrInterface)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = array;");
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsArrayOrInterface)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = new ");
                {
                    fastCSharp.setup.memberType _value4_ = _value2_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@"(array);");
            }
            _code_.Add(@"
                                }");
            }
            }
                }
            }
            _code_.Add(@"
                            }
                        }");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsParseJson)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.ajax _value3_ = _value2_.ToJsonAttribute;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsParseNode)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        else
                        {
                            ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" newValue = new ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"();
                            ((fastCSharp.setup.cSharp.ajax.IParseJson)newValue).FromJson(Node);
                            value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = newValue;
                        }");
            }
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    return value;
                }
            }
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <param name=""json"">JSON字符串</param>
            public void FromJson(string json)
            {
                FromJson(new fastCSharp.setup.cSharp.ajax.jsonParser().Parse(json));
            }
            /// <summary>
            /// JSON字符串解析节点换成对象
            /// </summary>
            /// <param name=""node"">JSON字符串解析节点</param>
            public void FromJson(fastCSharp.setup.cSharp.ajax.jsonNode node)
            {
                jsonParser parser = new jsonParser { Node = node };
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = ");
            }
            _code_.Add(@"parser.Parse(this);
                ");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                CopyFrom(value);");
            }
            _code_.Add(@"
            }");
            _partCodes_["PARSEJSON"] = _code_.ToString();
            _code_ = _PART_PARSEJSON_;
            _code_.Add(_partCodes_["PARSEJSON"]);
            _code_.Add(@"
        }");
            }
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class sqlTable
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
            _if_ = false;
                    if (PrimaryKey != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 关键字
            /// </summary>
            public struct sqlPrimaryKey : IEquatable<sqlPrimaryKey>
            {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name=""other"">关键字</param>
                /// <returns>是否相等</returns>
                public bool Equals(sqlPrimaryKey other)
                {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    if (");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" != other.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@") return false;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    return true;
                }
                /// <summary>
                /// 哈希编码
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return ");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"/**/.GetHashCode() ^ ");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@" 0;
                }");
            _if_ = false;
                    if (IsSinglePrimaryKey)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                public static implicit operator sqlPrimaryKey(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" value) { return new sqlPrimaryKey { ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = value }; }
                public static implicit operator ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"(sqlPrimaryKey value) { return value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"; }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            }
            _code_.Add(@"
            }");
            }
            _code_.Add(@"

            /// <summary>
            /// 数据库表格
            /// </summary>
            /// <typeparam name=""valueType"">表格映射类型</typeparam>
            public class sqlTable<valueType> : ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", fastCSharp.setup.cSharp.sqlTable.ISql<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
                ");
            _if_ = false;
                    if (Identity != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", fastCSharp.setup.cSharp.sqlTable.ISqlIdentity<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap, ");
                {
                    fastCSharp.setup.memberInfo _value1_ = Identity;
                    if (_value1_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value2_ = _value1_.MemberType;
                    if (_value2_ != null)
                    {
            _code_.Add(_value2_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@"
                ");
            _if_ = false;
                    if (PrimaryKey != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", fastCSharp.setup.cSharp.sqlTable.ISqlKey<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap, sqlPrimaryKey>");
            }
            _code_.Add(@"
                where valueType : sqlTable<valueType>
            {
                /// <summary>
                /// SQL操作工具
                /// </summary>
                public static readonly fastCSharp.setup.cSharp.sqlTable.sqlTool<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap> SqlTool;
                /// <summary>
                /// 设置字段值
                /// </summary>
                /// <param name=""reader"">字段读取器</param>
                /// <param name=""memberMap"">成员位图</param>
                public void Set(System.Data.Common.DbDataReader reader, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
                {
                    int index = -1;
                    object value;
                    if (memberMap.IsDefault)
                    {");
                {
                    fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[] _value1_ = default(fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[]);
                    _value1_ = SqlMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        value = reader[++index];
                        ");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        this.");
            _code_.Add(_value3_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"value == DBNull.Value ? default(");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@") : ");
            }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@")");
            }
                }
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.SqlMemberType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
            _code_.Add(_value3_.FullName);
            _code_.Add(@")");
            }
                }
            _code_.Add(@"value;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                    else
                    {");
                {
                    fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[] _value1_ = default(fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[]);
                    _value1_ = SqlMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        ");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (memberMap.IsMember(");
            _code_.Add(_value3_.MemberIndex.ToString());
            _code_.Add(@"))
                        {
                            value = reader[++index];
                            this.");
            _code_.Add(_value3_.MemberName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsNull)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"value == DBNull.Value ? default(");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@") : ");
            }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@")");
            }
                }
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.SqlMemberType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
            _code_.Add(_value3_.FullName);
            _code_.Add(@")");
            }
                }
            _code_.Add(@"value;
                        }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    }
                }
                /// <summary>
                /// 是否通过SQL默认验证
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                /// <returns>是否通过SQL默认验证</returns>
                public bool IsSqlVerify(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                {");
                {
                    fastCSharp.keyValue<fastCSharp.setup.memberInfo,fastCSharp.setup.sqlMember>[] _value1_ = default(fastCSharp.keyValue<fastCSharp.setup.memberInfo,fastCSharp.setup.sqlMember>[]);
                    _value1_ = VerifyMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.keyValue<fastCSharp.setup.memberInfo,fastCSharp.setup.sqlMember> _value2_ in _value1_)
                        {
            _code_.Add(@"
                    if (memberMap.IsMember(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@"))
                    {");
            _if_ = false;
                {
                    fastCSharp.setup.memberInfo _value3_ = _value2_.Key;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsString)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.sqlMember _value3_ = _value2_.Value;
                    if (_value3_ != null)
                    {
                    if (_value3_.MaxLength != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (!SqlTool.StringVerify(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"""");
            _code_.Add(_value3_.MemberName);
            _code_.Add(@""", ");
            _code_.Add(_value3_.MemberName);
            }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.sqlMember _value3_ = _value2_.Value;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MaxLength.ToString());
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.sqlMember _value3_ = _value2_.Value;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.IsAscii ? "true" : "false");
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.sqlMember _value3_ = _value2_.Value;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.IsNull ? "true" : "false");
                    }
                }
            _code_.Add(@"))
                        {
                            return false;
                        }");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                {
                    fastCSharp.setup.memberInfo _value3_ = _value2_.Key;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsString)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.sqlMember _value3_ = _value2_.Value;
                    if (_value3_ != null)
                    {
                if (!(bool)_value3_.IsNull)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (");
            _code_.Add(_value3_.MemberName);
            _code_.Add(@" == null)
                        {
                            SqlTool.NullVerify(""");
            _code_.Add(_value3_.MemberName);
            _code_.Add(@""");
                            return false;
                        }");
            }
                }
            }
            }
            _code_.Add(@"
                    }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    return true;
                }
                /// <summary>
                /// 添加到数据库
                /// </summary>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <param name=""memberMap"">成员位图</param>
                /// <returns>添加是否成功</returns>
                public bool SqlInsert(bool isIgnoreTransaction = false, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
                {
                    return SqlTool.Client.Insert(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }");
            _if_ = false;
                    if (IsSqlKey)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                /// <summary>
                /// SQL表达式
                /// </summary>
                public class sqlExpression : fastCSharp.setup.cSharp.sqlTable.sqlExpressionMember<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>, fastCSharp.setup.cSharp.sqlTable.ISqlExpression<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
                {");
                {
                    fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[] _value1_ = default(fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[]);
                    _value1_ = SqlMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    private System.Linq.Expressions.Expression<func<valueType, ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.SqlExpressionType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">> ");
            _code_.Add(_value2_.ExpressionMemberName);
            _code_.Add(@";
                    public System.Linq.Expressions.Expression<func<valueType, ");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            }
                }
            _code_.Add(@">> ");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberName);
            }
                }
            _code_.Add(@"
                    {
                        set
                        {
                            if (value != null)
                            {
                                ");
            _code_.Add(_value2_.ExpressionMemberName);
            _code_.Add(@" = SqlTool.GetExpression<");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            }
                }
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.SqlMemberType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            _code_.Add(_value3_.FullName);
            }
                }
            _code_.Add(@">(value);
                                memberMap.SetMember(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@");
                            }
                            else
                            {
                                ");
            _code_.Add(_value2_.ExpressionMemberName);
            _code_.Add(@" = null;
                                memberMap.ClearMember(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@");
                            }
                        }
                    }
                    public ");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.MemberType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.ExpressionMemberValueName);
            _code_.Add(@"
                    {
                        set
                        {
                            ");
            _code_.Add(_value2_.ExpressionMemberName);
            _code_.Add(@" = _value_ => (");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.SqlMemberType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
            _code_.Add(_value3_.FullName);
            _code_.Add(@")");
            }
                }
            _code_.Add(@"value);
                            memberMap.SetMember(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@");
                        }
                    }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    /// <summary>
                    /// 获取SQL表达式集合
                    /// </summary>
                    /// <param name=""memberMap"">成员位图</param>
                    /// <returns>SQL表达式集合</returns>
                    public list<keyValue<string, string>> Get(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap)
                    {
                        fastCSharp.setup.cSharp.IMemberInfo memberInfo = SqlTool.MemberInfo;
                        fastCSharp.sql.client client = SqlTool.Client;
                        list<keyValue<string, string>>.unsafer values = new list<keyValue<string, string>>(memberInfo.MemberCount).Unsafer;");
                {
                    fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[] _value1_ = default(fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[]);
                    _value1_ = SqlMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        if (memberMap.IsMember(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@")) values.Add(new keyValue<string, string>(memberInfo.GetName(");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@"), client.GetSql(");
            _code_.Add(_value2_.ExpressionMemberName);
            _code_.Add(@").Key));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                        return values.List;
                    }
                }");
            }
            _if_ = false;
                    if (Identity != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                /// <summary>
                /// 32位自增列
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public int SqlIdentity32
                {
                    get { return (int)");
                {
                    fastCSharp.setup.memberInfo _value1_ = Identity;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.MemberName);
                    }
                }
            _code_.Add(@"; }
                }
                /// <summary>
                /// 64位自增列
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public long SqlIdentity64
                {
                    get { return (long)");
                {
                    fastCSharp.setup.memberInfo _value1_ = Identity;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.MemberName);
                    }
                }
            _code_.Add(@"; }
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>是否修改成功</returns>
                public bool SqlUpdateByIdentity(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap), bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.UpdateByIdentity(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }
                /// <summary>
                /// 删除数据库记录
                /// </summary>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>是否成功</returns>
                public bool SqlDeleteByIdentity(bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.DeleteByIdentity(SqlTool, (valueType)this, isIgnoreTransaction);
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name=""");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.MemberName);
            }
                }
            _code_.Add(@""">自增id</param>
                /// <param name=""sqlExpression"">SQL表达式</param>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>修改后的数据,失败返回null</returns>
                public static valueType SqlUpdateByIdentity(");
                {
                    fastCSharp.setup.memberInfo _value1_ = Identity;
                    if (_value1_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value2_ = _value1_.MemberType;
                    if (_value2_ != null)
                    {
            _code_.Add(_value2_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.MemberName);
            }
                }
            _code_.Add(@", sqlExpression sqlExpression, bool isIgnoreTransaction = false)
                {
                    if (sqlExpression != null)
                    {
                        valueType value = fastCSharp.setup.constructor<valueType>.New;");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        value.");
            _code_.Add(_value1_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value1_.MemberName);
            _code_.Add(@";");
            }
                }
            _code_.Add(@"
                        if (SqlTool.Client.UpdateByIdentity(SqlTool, value, sqlExpression, isIgnoreTransaction)) return value;
                    }
                    return null;
                }
                /// <summary>
                /// 获取数据库记录
                /// </summary>
                /// <param name=""");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.MemberName);
            }
                }
            _code_.Add(@""">自增id</param>
                /// <param name=""memberMap"">成员位图</param>
                /// <returns>数据对象</returns>
                public static valueType SqlGetByIdentity(");
                {
                    fastCSharp.setup.memberInfo _value1_ = Identity;
                    if (_value1_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value2_ = _value1_.MemberType;
                    if (_value2_ != null)
                    {
            _code_.Add(_value2_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.MemberName);
            }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
                {
                    valueType value = fastCSharp.setup.constructor<valueType>.New;");
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    value.");
            _code_.Add(_value1_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value1_.MemberName);
            _code_.Add(@";");
            }
                }
            _code_.Add(@"
                    return SqlTool.GetByIdentity(value, memberMap);
                }");
            }
            _if_ = false;
                    if (PrimaryKey != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                /// <summary>
                /// SQL关键字
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public sqlPrimaryKey SqlPrimaryKey
                {
                    get
                    {
                        sqlPrimaryKey value = new sqlPrimaryKey();");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                        value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                        return value;
                    }
                }
                /// <summary>
                ///  关键字转换数据容器
                /// </summary>
                /// <param name=""sqlPrimaryKey"">SQL关键字</param>
                /// <returns>数据对象</returns>
                private static valueType getBySqlPrimaryKey(sqlPrimaryKey primaryKey)
                {
                    valueType value = fastCSharp.setup.constructor<valueType>.New;");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = primaryKey.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                    return value;
                }
                /// <summary>
                /// 获取数据库记录
                /// </summary>
                /// <param name=""primaryKey"">SQL关键字</param>
                /// <param name=""memberMap"">成员位图</param>
                /// <returns>数据对象</returns>
                public static valueType SqlGetPrimaryKey(sqlPrimaryKey primaryKey, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
                {
                    return SqlTool.GetByPrimaryKey(getBySqlPrimaryKey(primaryKey), memberMap);
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name=""primaryKey"">SQL关键字</param>
                /// <param name=""sqlExpression"">SQL表达式</param>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>修改后的数据,失败返回null</returns>
                public static valueType SqlUpdate(sqlPrimaryKey primaryKey, sqlExpression sqlExpression, bool isIgnoreTransaction = false)
                {
                    if (sqlExpression != null)
                    {
                        valueType value = getBySqlPrimaryKey(primaryKey);
                        if (SqlTool.Client.Update(SqlTool, value, sqlExpression, isIgnoreTransaction)) return value;
                    }
                    return null;
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name=""memberMap"">成员位图</param>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>是否修改成功</returns>
                public bool SqlUpdate(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap), bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.Update(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }
                /// <summary>
                /// 删除数据库记录
                /// </summary>
                /// <param name=""isIgnoreTransaction"">是否忽略应用程序事务</param>
                /// <returns>是否成功</returns>
                public bool SqlDelete(bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.Delete(SqlTool, (valueType)this, isIgnoreTransaction);
                }");
            }
            _code_.Add(@"
                static sqlTable()
                {
                    func<valueType, object>[] getMembers = new func<valueType, object>[(new memberInfo()).MemberCount];");
                {
                    fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[] _value1_ = default(fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo[]);
                    _value1_ = SqlMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.sqlTable.cSharp.sqlMemberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    getMembers[");
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MemberIndex.ToString());
            }
                }
            _code_.Add(@"] = value => (object)");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                {
                    fastCSharp.setup.sqlMember _value4_ = _value2_.Value;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.SqlMemberType;
                    }
                }
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
            _code_.Add(_value3_.FullName);
            _code_.Add(@")");
            }
                }
                {
                    fastCSharp.setup.memberInfo _value3_ = default(fastCSharp.setup.memberInfo);
                    _value3_ = _value2_.Key;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"value.");
            _code_.Add(_value3_.MemberName);
            }
                }
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _if_ = false;
                    if (PrimaryKey != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap sqlPrimaryKeyMemberMap = new ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap();");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = PrimaryKey;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    sqlPrimaryKeyMemberMap.SetMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            }
            _code_.Add(@"
                    SqlTool = fastCSharp.setup.cSharp.sqlTable.sqlTool<valueType, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>.Get(new memberInfo(), getMembers, ");
                {
                    fastCSharp.setup.cSharp.sqlTable _value1_ = Attribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.IsLockWrite ? "true" : "false");
                    }
                }
                {
                    fastCSharp.setup.memberInfo _value1_ = default(fastCSharp.setup.memberInfo);
                    _value1_ = Identity;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            _code_.Add(_value1_.MemberIndex.ToString());
            }
                }
            _if_ = false;
                    if (PrimaryKey != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", sqlPrimaryKeyMemberMap");
            }
            _code_.Add(@");
                }
            }
        }");
                outEnd(definition);
            }
        }
    }
    }
}
namespace fastCSharp.setup.cSharp
{
     partial class copy
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.ICopy<");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap>
        {
            
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name=""value"">被复制对象</param>
            /// <param name=""memberMap"">复制成员位图</param>
            public void CopyFrom(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap memberMap = default(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"/**/.memberMap))
            {
                if (memberMap.IsDefault)
                {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }
                else
                {");
                {
                    fastCSharp.setup.memberInfo[] _value1_ = default(fastCSharp.setup.memberInfo[]);
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.memberInfo _value2_ in _value1_)
                        {
            _code_.Add(@"
                    if (memberMap.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")) this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }
            }
            /// <summary>
            /// 浅复制对象
            /// </summary>
            /// <returns>复制的对象</returns>
            public ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" CopyMember()
            {
                return (");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@")MemberwiseClone();
            }");
            _if_ = false;
                    if (IsConstructor)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 无参数构造函数调用
            /// </summary>
            private static ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(ConstructorName);
            _code_.Add(@"
            {
                get
                {
                    return new ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"();
                }
            }");
            }
            _code_.Add(@"
        }");
                outEnd(definition);
            }
        }
    }
}
namespace fastCSharp.setup.cSharp
{
    public partial class tcpServer
    {
     partial class cSharp
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                
            _if_ = false;
                    if (IsServerCall)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" : fastCSharp.setup.cSharp.tcpServer.ITcpServer
        {
            internal static class ");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"
            {");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                public static ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.ReturnType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = type;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" _value_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
            _code_.Add(_value4_.ParameterTypeRefName);
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
                {");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value3_.IsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    return ");
            }
            _code_.Add(@"_value_.");
            _code_.Add(_value3_.MethodGenericName);
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value4_ = default(fastCSharp.setup.parameterInfo[]);
                    _value4_ = _value3_.Parameters;
                    if (_value4_ != null)
                    {
                        int _loopIndex4_ = _loopIndex_, _loopCount4_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value4_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value5_ in _value4_)
                        {
            _code_.Add(_value5_.ParameterJoinRefName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex4_;
                        _loopCount_ = _loopCount4_;
                    }
                }
            _code_.Add(@");");
            }
                }
            _code_.Add(@"
                }");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public static readonly System.Reflection.MethodInfo ");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@";");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _if_ = false;
                    if (IsAnyGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                static ");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"()
                {
                    System.Collections.Generic.Dictionary<fastCSharp.setup.cSharp.tcpBase.genericMethod, System.Reflection.MethodInfo> genericMethods = fastCSharp.setup.cSharp.tcpServer.GetGenericMethods(typeof(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@" = ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"genericMethods[new fastCSharp.setup.cSharp.tcpBase.genericMethod(""");
            _code_.Add(_value3_.MethodName);
            _code_.Add(@""", ");
                {
                    fastCSharp.setup.memberType[] _value4_ = _value3_.GenericParameters;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.Length.ToString());
                    }
                }
                {
                    fastCSharp.setup.parameterInfo[] _value4_ = default(fastCSharp.setup.parameterInfo[]);
                    _value4_ = _value3_.Parameters;
                    if (_value4_ != null)
                    {
                        int _loopIndex4_ = _loopIndex_, _loopCount4_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value4_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value5_ in _value4_)
                        {
            _code_.Add(@", """);
            _code_.Add(_value5_.ParameterRef);
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.ParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
            _code_.Add(@"""");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex4_;
                        _loopCount_ = _loopCount4_;
                    }
                }
            _code_.Add(@")]");
            }
                }
            _code_.Add(@";");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }
        }");
            }
            _if_ = false;
                if (!(bool)IsServerCall)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            stringBuilder _PART_SERVER_ = _code_;
            _code_ = new stringBuilder();
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                if (!(bool)_value1_.IsAsynchronous)
                {
                    _if_ = true;
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
        /// <summary>
        /// TCP服务
        /// </summary>
        public class ");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.TypeOnlyName);
            }
                }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != null)
                    {
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.GenericParameterNames);
            }
                }
            _code_.Add(@">");
            }
            _code_.Add(@" : fastCSharp.net.tcpServer
        {
            /// <summary>
            /// TCP服务目标对象
            /// </summary>
            private readonly ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" _value_ = new ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"();
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name=""attribute"">TCP调用服务器端配置信息</param>
            /// <param name=""verify"">TCP验证实例</param>
            public ");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.TypeOnlyName);
            }
                }
            _code_.Add(@"(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerify verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.ServiceName);
                    }
                }
            _code_.Add(@""", typeof(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@")), verify");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@" ?? new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@")
            {
                _value_.SetTcpServer(this);
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@" + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyName);
                    }
                }
            _code_.Add(@"""), new keyValue<action<socket, int>, bool>(");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@", ");
            _code_.Add(_value2_.IsInputParameter ? "true" : "false");
            _code_.Add(@")));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                this.onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(onCommands.List);
            }");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _code_.Add(@"
            private void ");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"(socket _socket_, int _identity_)
            {
                bool _isError_ = false;");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                byte[] _data_ = null;");
            }
            _code_.Add(@"
                try
                {");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"};");
            }
            }
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" _return_ = ");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpServer.InvokeGenericMethod(_value_, ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@".");
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(_value_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
            _code_.Add(_value4_.ParameterRef);
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    System.Type _value6_ = _value5_.Type;
                    if (_value6_ != null)
                    {
                    if (_value6_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@");");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = (");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")_invokeParameter_[");
            _code_.Add(_value4_.ParameterIndex.ToString());
            _code_.Add(@"];");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            }
            _code_.Add(@"
                    _data_ = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"
                    {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = _return_");
            }
            _code_.Add(@"
                    }).Serialize();");
            }
            _code_.Add(@"
                }
                catch (Exception error)
                {
                    _isError_ = true;
                    fastCSharp.log.Default.Add(error, null, true);
                    if (send(_socket_, fastCSharp.net.tcpServer.status.Error, _identity_)) close(_socket_, _identity_);
                    _socket_.Dispose();
                }
                if (!_isError_)
                {");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_, _data_))");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        if (!send(_socket_, fastCSharp.net.tcpServer.status.Success, _identity_))");
            }
            _code_.Add(@"
                            _socket_.Dispose();
                }
            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
        }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = ServiceAttribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsAsynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
        /// <summary>
        /// TCP服务
        /// </summary>
        public class ");
            _code_.Add(TypeNameAsynchronous);
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != null)
                    {
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.GenericParameterNames);
            }
                }
            _code_.Add(@">");
            }
            _code_.Add(@" : fastCSharp.net.tcpServerAsynchronous
        {
            /// <summary>
            /// TCP服务目标对象
            /// </summary>
            private readonly ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" _value_ = new ");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"();
            /// <summary>
            /// TCP调用服务端
            /// </summary>
            /// <param name=""attribute"">TCP调用服务器端配置信息</param>
            /// <param name=""verify"">TCP验证实例</param>
            public ");
            _code_.Add(TypeNameAsynchronous);
            _code_.Add(@"(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyAsynchronous verify = null)
                : base(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.ServiceName);
                    }
                }
            _code_.Add(@""", typeof(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@")), verify");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@" ?? new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@")
            {
                _value_.SetTcpServer(this);
                list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>.unsafer onCommands = new list<keyValue<hashBytes, keyValue<action<socket, int>, bool>>>(");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.Length.ToString());
                    }
                }
            _code_.Add(@" + 1).Unsafer;
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(closeCommandData, new keyValue<action<socket, int>, bool>(new action<socket, int>(close), false)));");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                onCommands.Add(new keyValue<hashBytes, keyValue<action<socket, int>, bool>>(formatMethodKeyName(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyName);
                    }
                }
            _code_.Add(@"""), new keyValue<action<socket, int>, bool>(");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@", ");
            _code_.Add(_value2_.IsInputParameter ? "true" : "false");
            _code_.Add(@")));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                this.onCommands = new staticDictionary<hashBytes, keyValue<action<socket, int>, bool>>(onCommands.List);
            }");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            private struct ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@"
            {
                public ");
            _code_.Add(TypeNameAsynchronous);
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = type;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = default(fastCSharp.setup.memberType);
                    _value3_ = type;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.GenericParameterNames);
            }
                }
            _code_.Add(@">");
            }
            _code_.Add(@" Server;
                public socket Socket;
                public int Identity;
                public void Callback(fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@" returnValue)
                {");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    byte[] data = null;
                    if (returnValue.IsReturn)
                    {
                        try
                        {
                            data = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" { ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = returnValue.Value }).Serialize();
                        }
                        catch (Exception error)
                        {
                            returnValue.IsReturn = false;
                            fastCSharp.log.Default.Add(error, null, true);
                        }
                    }");
            }
            _code_.Add(@"
                    if (returnValue.IsReturn) Server.send(Socket, Server.receiveCommand, fastCSharp.net.tcpServer.status.Success, Identity");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", data");
            }
            _code_.Add(@");
                    else Server.send(Socket, socket.Close, fastCSharp.net.tcpServer.status.Error, Identity);
                }
            }");
            }
            _code_.Add(@"
            private void ");
            _code_.Add(_value2_.MethodIndexName);
            _code_.Add(@"(socket _socket_, int _identity_)
            {");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                bool _isAsync_ = false;");
            }
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                bool _isError_ = false;");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                byte[] _data_ = null;");
            }
            }
            _code_.Add(@"
                try
                {");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            }
            _if_ = false;
                    if (_value2_.IsInvokeGenericMethod)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", ");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.GetGenericParameterCallback(_inputParameter_.");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@", ");
            }
            _if_ = false;
                if (!(bool)_value2_.IsGenericParameterCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@">)");
            }
            _code_.Add(@"new ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@" { Server = this, Socket = _socket_, Identity = _identity_ }.Callback");
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@")");
            }
            _code_.Add(@" };");
            }
            _code_.Add(@"
                    _isAsync_ = true;");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    fastCSharp.setup.cSharp.tcpServer.InvokeGenericMethod(_value_, ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@", _invokeParameter_);");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@".");
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(_value_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
            _code_.Add(_value4_.ParameterRef);
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    System.Type _value6_ = _value5_.Type;
                    if (_value6_ != null)
                    {
                    if (_value6_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@", new ");
            _code_.Add(_value2_.AsynchronousCallbackIndexName);
            _code_.Add(@" { Server = this, Socket = _socket_, Identity = _identity_ }.Callback);");
            }
            }
            _code_.Add(@"
");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"();
                    (_inputParameter_).DeSerialize(_socket_.CurrentData);");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    object[] _invokeParameter_ = new object[] { ");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"};");
            }
            }
            _code_.Add(@"
                    ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" _return_ = ");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.tcpServer.InvokeGenericMethod(_value_, ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.GenericMethodInfoName);
            _code_.Add(@", _inputParameter_.");
            _code_.Add(GenericParameterTypeName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _invokeParameter_");
            }
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericMethod)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(GenericTypeServerName);
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@".");
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(_value_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
            _code_.Add(_value4_.ParameterRef);
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    System.Type _value6_ = _value5_.Type;
                    if (_value6_ != null)
                    {
                    if (_value6_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_inputParameter_.");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@");");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = (");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@")_invokeParameter_[");
            _code_.Add(_value4_.ParameterIndex.ToString());
            _code_.Add(@"];");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            }
            _code_.Add(@"
                    _data_ = (new ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"
                    {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                        ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = _inputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value2_.ReturnName);
            _code_.Add(@" = _return_");
            }
            _code_.Add(@"
                    }).Serialize();");
            }
            }
            _code_.Add(@"
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, null, true);");
            _if_ = false;
                    if (_value2_.IsAsynchronousCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (!_isAsync_)");
            }
            _code_.Add(@"
                    {");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        _isError_ = true;");
            }
            _code_.Add(@"
                        send(_socket_, socket.Close, fastCSharp.net.tcpServer.status.Error, _identity_);
                    }
                }");
            _if_ = false;
                if (!(bool)_value2_.IsAsynchronousCallback)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!_isError_)
                {");
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_, _data_);");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    send(_socket_, receiveCommand, fastCSharp.net.tcpServer.status.Success, _identity_);");
            }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
        }");
            }
            _partCodes_["SERVER"] = _code_.ToString();
            _code_ = _PART_SERVER_;
            _code_.Add(_partCodes_["SERVER"]);
            stringBuilder _PART_CLIENT_ = _code_;
            _code_ = new stringBuilder();
            _code_.Add(@"
        public class ");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.TypeOnlyName);
            }
                }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != null)
                    {
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.GenericParameterNames);
            }
                }
            _code_.Add(@">");
            }
            _code_.Add(@": IDisposable
        {
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            private fastCSharp.net.tcpClient _tcpClient_;
            /// <summary>
            /// TCP调用客户端是否已启动
            /// </summary>
            public bool _IsClientStart_
            {
                get
                {
                    return _tcpClient_ != null && _tcpClient_.IsStart;
                }
            }
            /// <summary>
            /// TCP调用客户端
            /// </summary>
            /// <param name=""attribute"">TCP调用服务器端配置信息</param>
            /// <param name=""verify"">TCP验证实例</param>
            /// <param name=""isStart"">是否启动连接</param>
            public ");
                {
                    fastCSharp.setup.memberType _value1_ = default(fastCSharp.setup.memberType);
                    _value1_ = type;
            _if_ = false;
                    if (_value1_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value1_.TypeOnlyName);
            }
                }
            _code_.Add(@"(fastCSharp.setup.cSharp.tcpServer attribute = null, fastCSharp.setup.cSharp.tcpBase.ITcpVerifyClient verify = null, bool isStart = true)
            {
                _tcpClient_ = new fastCSharp.net.tcpClient(attribute ?? fastCSharp.setup.cSharp.tcpServer.GetConfig(""");
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.ServiceName);
                    }
                }
            _code_.Add(@""", typeof(");
                {
                    fastCSharp.setup.memberType _value1_ = type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@")), verify");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.VerifyType != null)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@" ?? new ");
            _code_.Add(TcpVerifyType);
            _code_.Add(@"()");
            }
            _code_.Add(@", isStart);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                if (_tcpClient_ != null)
                {
                    _tcpClient_.Dispose();
                    _tcpClient_ = null;
                }
            }
");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value1_ = Attribute;
                    if (_value1_ != null)
                    {
                    if (_value1_.IsSegmentation)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            [fastCSharp.setup.cSharp.serialize(IsIgnore = true, IsObject = true, IsInterface = true)]
            internal class ");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" : fastCSharp.setup.cSharp.serialize.ISerialize
            {");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType[] ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@";");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                public fastCSharp.setup.remoteType ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@";");
            }
            _code_.Add(_value2_.InputParameterSerialize);
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            internal class ");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" : ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"fastCSharp.setup.cSharp.tcpBase.returnParameter<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value4_ = _value3_.GenericParameterType;
                    if (_value4_ != null)
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@">, ");
            }
            _code_.Add(@"fastCSharp.setup.cSharp.serialize.ISerialize
            {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.OutputParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@"
                public ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
                {
                    fastCSharp.setup.memberType _value6_ = _value5_.GenericParameterType;
                    if (_value6_ != null)
                    {
            _code_.Add(_value6_.FullName);
                    }
                }
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(_value2_.OutputParameterSerialize);
            _code_.Add(@"
            }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            }
            _code_.Add(@"
");
                {
                    fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[] _value1_ = default(fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex[]);
                    _value1_ = MethodIndexs;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (fastCSharp.setup.cSharp.tcpBase.cSharp<fastCSharp.setup.cSharp.tcpServer>.methodIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
            private static readonly byte[] ");
            _code_.Add(_value2_.MethodCommandName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetMethodKeyNameCommand(""");
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.MethodKeyName);
                    }
                }
            _code_.Add(@""");");
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value3_ = _value2_.Attribute;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsClientSynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            public ");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(_value4_.ParameterTypeRefName);
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
            {");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"
                {");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0");
                {
                    fastCSharp.setup.memberType[] _value3_ = default(fastCSharp.setup.memberType[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.GenericParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.memberType _value4_ in _value3_)
                        {
            _code_.Add(@", typeof(");
            _code_.Add(_value4_.FullName);
            _code_.Add(@")");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"),");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"),");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                if (!(bool)_value4_.IsOut)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                };");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" _outputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"();
                if (_tcpClient_.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", _outputParameter_))
                {");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                    if (_value4_.IsRefOrOut)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value5_ = _value2_.Method;
                    if (_value5_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value6_ = _value5_.Method;
                    if (_value6_ != null)
                    {
                    if (_value6_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_outputParameter_.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@";");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                    return ");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            }
            _code_.Add(@"_outputParameter_.");
            _code_.Add(_value2_.ReturnName);
            }
            _code_.Add(@";
                }");
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (_tcpClient_.Call");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@")) return;");
            }
            _code_.Add(@"
                throw new Exception();
            }");
            }
            _if_ = false;
                {
                    fastCSharp.setup.cSharp.tcpServer _value3_ = _value2_.Attribute;
                    if (_value3_ != null)
                    {
                    if (_value3_.IsClientAsynchronous)
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            public void ");
                {
                    fastCSharp.setup.methodInfo _value3_ = default(fastCSharp.setup.methodInfo);
                    _value3_ = _value2_.Method;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.MethodGenericName);
            }
                }
            _code_.Add(@"(action<fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@"> _onReturn_");
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _code_.Add(@", ");
                {
                    fastCSharp.setup.memberType _value5_ = _value4_.ParameterType;
                    if (_value5_ != null)
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@")
            {
                bool _isCall_ = false;
                try
                {
                    fastCSharp.net.tcpClient _client_ = _tcpClient_;
                    if (_client_ != null)
                    {");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@" _inputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@"
                        {");
            _if_ = false;
                {
                    fastCSharp.setup.methodInfo _value3_ = _value2_.Method;
                    if (_value3_ != null)
                    {
                {
                    System.Reflection.MethodInfo _value4_ = _value3_.Method;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericMethod)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(GenericParameterTypeName);
            _code_.Add(@" = fastCSharp.setup.cSharp.tcpBase.GetGenericParameters(0");
                {
                    fastCSharp.setup.memberType[] _value3_ = default(fastCSharp.setup.memberType[]);
                {
                    fastCSharp.setup.methodInfo _value4_ = _value2_.Method;
                    if (_value4_ != null)
                    {
                    _value3_ = _value4_.GenericParameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.memberType _value4_ in _value3_)
                        {
            _code_.Add(@", typeof(");
            _code_.Add(_value4_.FullName);
            _code_.Add(@")");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"),");
            }
            _if_ = false;
                    if (_value2_.IsGenericParameterCallback)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(_value2_.ReturnTypeName);
            _code_.Add(@" = typeof(");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"),");
            }
                {
                    fastCSharp.setup.parameterInfo[] _value3_ = default(fastCSharp.setup.parameterInfo[]);
                    _value3_ = _value2_.MethodParameters;
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_, _loopCount3_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value3_.Length;
                        foreach (fastCSharp.setup.parameterInfo _value4_ in _value3_)
                        {
            _if_ = false;
                if (!(bool)_value4_.IsOut)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                            ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" = ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                        _loopCount_ = _loopCount3_;
                    }
                }
            _code_.Add(@"
                        };");
            }
            _if_ = false;
                    if (_value2_.IsOutputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        ");
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@" _outputParameter_ = new ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"();
                        _isCall_ = true;");
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                    if (_value4_.IsGenericParameter)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                        _client_.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturnGeneric<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@"> { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", (");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterGenericTypeName);
            _code_.Add(@")(object)_outputParameter_);");
            }
            _if_ = false;
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != null)
                    {
                if (!(bool)_value4_.IsGenericParameter)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                        _client_.Get");
            _if_ = false;
                    if (_value2_.IsAnyParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@", ");
            }
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturn<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@", ");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.OutputParameterTypeName);
            _code_.Add(@"> { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@", _outputParameter_);");
            }
            }
            _if_ = false;
                if (!(bool)_value2_.IsOutputParameter)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                        _isCall_ = true;
                        _client_.Call");
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.auto.parameter _value3_ = default(fastCSharp.setup.auto.parameter);
                    _value3_ = AutoParameter;
            _if_ = false;
                    if (_value3_ != null)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.DefaultNamespace);
            }
                }
            _code_.Add(@".");
            _code_.Add(ParameterPart);
            _code_.Add(@"/**/.");
            _code_.Add(ServiceName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.InputParameterTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(new fastCSharp.setup.cSharp.tcpBase.asyncReturn { OnReturn = _onReturn_ }.CallOnReturn, ");
            _code_.Add(_value2_.MethodCommandName);
            _if_ = false;
                    if (_value2_.IsInputParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", _inputParameter_");
            }
            _code_.Add(@");");
            }
            _code_.Add(@"
                    }
                }
                catch (Exception _error_)
                {
                    fastCSharp.log.Default.Add(_error_, null, false);
                }
                finally
                {
                    if (!_isCall_) _onReturn_(new fastCSharp.setup.cSharp.tcpBase.asynchronousReturn");
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
                {
                    fastCSharp.setup.memberType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">");
            }
            _code_.Add(@"{ IsReturn = false });
                }
            }");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
        }");
            _partCodes_["CLIENT"] = _code_.ToString();
            _code_ = _PART_CLIENT_;
            _code_.Add(_partCodes_["CLIENT"]);
            }
                outEnd(definition);
            }
        }
    }
    }
}