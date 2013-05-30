using System;

namespace fastCSharp.setup.cSharp.template
{
    class ajax : pub
    {
        #region PART CLASS
        #region IF Attribute.IsToJson
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : fastCSharp.setup.cSharp.ajax.IToJson<@type.FullName/**/.memberMap>
        {
            #region PART TOJSON
            /// <summary>
            /// 对象转换JSON字符串
            /// </summary>
            class toJsoner : fastCSharp.setup.cSharp.ajax.toJsoner
            {
                public toJsoner() : base(@IsPushParent) { }
                public toJsoner(fastCSharp.setup.cSharp.ajax.toJsoner parent) : base(parent) { }
                /// <summary>
                /// 对象转换成JSON字符串
                /// </summary>
                /// <param name="value">对象</param>
                /// <param name="memberMap">成员位图接口</param>
                public void ToJson(@type.FullName value, @type.FullName/**/.memberMap memberMap)
                {
                    #region IF type.IsNull
                    if (value != null)
                    #endregion IF type.IsNull
                    {
                        #region IF IsPushParent
                        Push(value);
                        #endregion IF IsPushParent
                        jsonStream.Write('{');
                        isFirstMember = true;
                        #region LOOP Members
                        #region IF ToJsonAttribute.IsToJson
                        if (memberMap.IsMember(@MemberIndex))
                        {
                            name("@MemberName");
                            #region IF MemberType.IsNull
                            if (value.@MemberName == null) jsonStream.Write(fastCSharp.web.ajax.Null);
                            else
                            #endregion IF MemberType.IsNull
                            {
                                #region IF MemberType.IsToJson
                                (/*NOTE*/(fastCSharp.setup.cSharp.ajax.IToJson)/*NOTE*/value.@MemberName).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                #endregion IF MemberType.IsToJson

                                #region IF MemberType.Type.IsEnum
                                base.value(value.@MemberName/**/.ToString());
                                #endregion IF MemberType.Type.IsEnum

                                #region IF MemberType.PairKeyType
                                base.keyValue(/*NOTE*/(System.Collections.Generic.KeyValuePair<@PairKeyType.FullName, @PairKeyType.FullName>)(object)/*NOTE*/value.@MemberName);
                                #endregion IF MemberType.PairKeyType

                                #region IF MemberType.KeyValueType
                                base.keyValue(/*NOTE*/(keyValue<@KeyValueType.FullName, @KeyValueType.FullName>)(object)/*NOTE*/value.@MemberName);
                                #endregion IF MemberType.KeyValueType

                                #region IF MemberType.IsToJsonUnknown
                                base.value((@MemberType.NotNullType.FullName)value.@MemberName);
                                #endregion IF MemberType.IsToJsonUnknown

                                #region IF MemberType.IsEnumerable
                                #region PUSH MemberType.EnumerableArgumentType
                                {
                                    bool isNext = false;
                                    jsonStream.Write('[');
                                    foreach (@FullName nextValue in value.@MemberName)
                                    {
                                        if (isNext) jsonStream.Write(',');
                                        #region IF IsToJson
                                        if (value != null) (/*NOTE*/(fastCSharp.setup.cSharp.ajax.IToJson)/*NOTE*/nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        else jsonStream.Write(fastCSharp.web.ajax.Null);
                                        #endregion IF IsToJson

                                        #region IF Type.IsEnum
                                        base.value(nextValue.ToString());
                                        #endregion IF Type.IsEnum

                                        #region IF PairKeyType
                                        //(/*NOTE*/(fastCSharp.setup.cSharp.ajax.IToJson)/*NOTE*/nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        base.keyValue(/*NOTE*/(System.Collections.Generic.KeyValuePair<@PairKeyType.FullName, @PairKeyType.FullName>)(object)/*NOTE*/nextValue);
                                        #endregion IF PairKeyType

                                        #region IF KeyValueType
                                        //(/*NOTE*/(fastCSharp.setup.cSharp.ajax.IToJson)/*NOTE*/nextValue).ToJson((fastCSharp.setup.cSharp.ajax.toJsoner)this);
                                        base.keyValue(/*NOTE*/(keyValue<@KeyValueType.FullName, @KeyValueType.FullName>)(object)/*NOTE*/nextValue);
                                        #endregion IF KeyValueType

                                        #region IF IsToJsonUnknown
                                        {
                                            #region IF IsNull
                                            if (nextValue == null) jsonStream.Write(fastCSharp.web.ajax.Null);
                                            else
                                            #endregion IF IsNull
                                            {
                                                base.value((@NotNullType.FullName)nextValue);
                                            }
                                        }
                                        #endregion IF IsToJsonUnknown
                                        isNext = true;
                                    }
                                    jsonStream.Write(']');
                                }
                                #endregion PUSH MemberType.EnumerableArgumentType
                                #endregion IF MemberType.IsEnumerable
                            }
                        }
                        #endregion IF ToJsonAttribute.IsToJson
                        #endregion LOOP Members
                        isFirstMember = false;
                        jsonStream.Write('}');
                        #region IF IsPushParent
                        parents.Unsafer.AddLength(-1);
                        #endregion IF IsPushParent
                    }
                    #region IF type.IsNull
                    else jsonStream.Write(fastCSharp.web.ajax.Null);
                    #endregion IF type.IsNull
                }
                /// <summary>
                /// 对象集合转换成JSON字符串
                /// </summary>
                /// <param name="values">对象集合</param>
                /// <param name="memberMap">成员位图接口</param>
                #region IF type.IsNull
                public void ToJson<JsonValueType>(System.Collections.Generic.IEnumerable<JsonValueType> values, @type.FullName/**/.memberMap memberMap)
                    where JsonValueType : @type.FullName
                {
                    #region IF IsPushParent
                    ToJsonPush<@type.FullName, @type.FullName/**/.memberMap, JsonValueType>(values, memberMap, ToJson);
                    #endregion IF IsPushParent
                    #region NOT IsPushParent
                    ToJson<@type.FullName, @type.FullName/**/.memberMap, JsonValueType>(values, memberMap, ToJson);
                    #endregion NOT IsPushParent
                }
                #endregion IF type.IsNull
                #region NOT type.IsNull
                public void ToJson(System.Collections.Generic.IEnumerable<@type.FullName> values, @type.FullName/**/.memberMap memberMap)
                {
                    #region IF IsPushParent
                    ToJsonPush<@type.FullName, @type.FullName/**/.memberMap>(values, memberMap, ToJson);
                    #endregion IF IsPushParent
                    #region NOT IsPushParent
                    ToJson<@type.FullName, @type.FullName/**/.memberMap>(values, memberMap, ToJson);
                    #endregion NOT IsPushParent
                }
                #endregion NOT type.IsNull
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <returns>JSON字符串</returns>
            public string ToJson()
            {
                return ToJson(default(@type.FullName/**/.memberMap));
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="memberMap">成员位图接口</param>
            /// <returns>JSON字符串</returns>
            public string ToJson(@type.FullName/**/.memberMap memberMap)
            {
                toJsoner json = new toJsoner();
                json.ToJson(/*NOTE*/(@type.FullName)(object)/*NOTE*/this, memberMap);
                return json.ToString();
            }
            /// <summary>
            /// 对象转换成JSON字符串
            /// </summary>
            /// <param name="jsoner">对象转换JSON字符串</param>
            public void ToJson(fastCSharp.setup.cSharp.ajax.toJsoner parent)
            {
                new toJsoner(parent).ToJson(/*NOTE*/(@type.FullName)(object)/*NOTE*/this, default(@type.FullName/**/.memberMap));
            }
            /// <summary>
            /// 对象集合转换成JSON字符串
            /// </summary>
            /// <param name="values">对象集合</param>
            /// <param name="memberMap">成员位图接口</param>
            /// <returns>JSON字符串</returns>
            #region NOT type.IsNull
            public static string ToJson(System.Collections.Generic.IEnumerable<@type.FullName> values, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
            #endregion NOT type.IsNull
                /*NOTE*/{ return null; }/*NOTE*/
            #region IF type.IsNull
            public static string ToJson<JsonValueType>(System.Collections.Generic.IEnumerable<JsonValueType> values, @type.FullName/**/.memberMap memberMap)
                where JsonValueType : @type.FullName
            #endregion IF type.IsNull
            {
                toJsoner json = new toJsoner();
                json.ToJson(values, memberMap);
                return json.ToString();
            }
            #endregion PART TOJSON
        }
        #endregion IF Attribute.IsToJson

        #region IF Attribute.IsParseJson
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : /*NOTE*/copy.TypeNameDefinition, /*NOTE*/fastCSharp.setup.cSharp.ajax.IParseJson
        {
            #region PART PARSEJSON
            /// <summary>
            /// JSON解析节点转换对象
            /// </summary>
            class jsonParser : fastCSharp.setup.cSharp.ajax.nodeParser
            {
                /// <summary>
                /// 解析对象
                /// </summary>
                /// <param name="value">目标对象</param>
                /// <returns>目标对象</returns>
                public @type.FullName Parse(@type.FullName value)
                {
                    if (dictionary != null)
                    {
                        fastCSharp.setup.cSharp.ajax.jsonNode node;
                        #region LOOP Members
                        #region IF IsParseJson
                        if (dictionary.TryGetValue(currentMemberName = "@MemberName", out node))
                        {
                            if (node.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Null) value.@MemberName = default(@MemberType.FullName);
                            else
                            {
                                #region IF MemberType.IsObject
                                value.@MemberName = /*NOTE*/(pub)(object)/*NOTE*/node;
                                #endregion IF MemberType.IsObject

                                #region IF MemberType.IsString
                                value.@MemberName = /*NOTE*/(pub)(object)/*NOTE*/node.String;
                                #endregion IF MemberType.IsString

                                #region IF MemberType.IsChar
                                value.@MemberName = /*NOTE*/(pub)(object)/*NOTE*/node.Char;
                                #endregion IF MemberType.IsChar

                                #region IF MemberType.IsBool
                                value.@MemberName = /*NOTE*/(pub)(object)/*NOTE*/node.Bool;
                                #endregion IF MemberType.IsBool

                                #region IF MemberType.IsDateTime
                                value.@MemberName = /*NOTE*/(pub)(object)/*NOTE*/node.DateTime;
                                #endregion IF MemberType.IsDateTime

                                #region IF MemberType.Type.IsEnum
                                value.@MemberName = fastCSharp.Enum.parseEnum<@MemberType.FullName>(node.String, /*NOTE*/(@MemberType.FullName)/*NOTE*/value.@MemberName);
                                #endregion IF MemberType.Type.IsEnum

                                #region IF MemberType.IsTryParseJson
                                #region IF MemberType.IsTryParseInt
                                if (node.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Int) value.@MemberName = (@MemberType.NotNullType.FullName)/*NOTE*/(object)/*NOTE*/node.Int;
                                else
                                #endregion IF MemberType.IsTryParseInt
                                {
                                    #region IF MemberType.NullType
                                    @MemberType.NotNullType.FullName nullValue;
                                    if (@MemberType.NotNullType.FullName/**/.TryParse(node.NumberString, out nullValue)) value.@MemberName = nullValue;
                                    else
                                    #endregion IF MemberType.NullType
                                        #region NOT MemberType.NullType
                                        if (!@MemberType.FullName/**/.TryParse(node.NumberString, out value.@MemberName))
                                        #endregion NOT MemberType.NullType
                                        {
                                            fastCSharp.log.Default.Throw("@MemberType.FullName : " + currentMemberName + @" 转换失败
" + node.NumberString, true, false);
                                        }
                                }
                                #endregion IF MemberType.IsTryParseJson

                                #region IF MemberType.IsParseJson
                                @MemberType.FullName parseValue = new @MemberType.FullName();
                                (/*NOTE*/(fastCSharp.setup.cSharp.ajax.IParseJson)/*NOTE*/parseValue).FromJson(node);
                                value.@MemberName = parseValue;
                                #endregion IF MemberType.IsParseJson

                                #region NOT MemberType.CanParseJson
                                value.@MemberName = unknown<@MemberType.FullName>(node);
                                #endregion NOT MemberType.CanParseJson

                                #region IF MemberType.IsEnumerable
                                #region PUSH MemberType.EnumerableArgumentType
                                #region IF CanParseJsonEnumerable
                                {
                                    if (node.Type != fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.List) fastCSharp.log.Default.Throw(currentMemberName + " 不可枚举", true, false);
                                    list<fastCSharp.setup.cSharp.ajax.jsonNode> nodes = node.List;
                                    @FullName[] array = new @FullName[nodes.Count];
                                    int arrayIndex = -1;
                                    foreach (fastCSharp.setup.cSharp.ajax.jsonNode arrayNode in nodes)
                                    {
                                        if (arrayNode.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Null) array[++arrayIndex] = default(@NotNullType.FullName);
                                        else
                                        {
                                            #region IF IsObject
                                            array[++arrayIndex] = /*NOTE*/(@FullName)(object)/*NOTE*/arrayNode;
                                            #endregion IF IsObject

                                            #region IF IsString
                                            array[++arrayIndex] = /*NOTE*/(@FullName)(object)/*NOTE*/arrayNode.String;
                                            #endregion IF IsString

                                            #region IF IsChar
                                            array[++arrayIndex] = /*NOTE*/(@FullName)(object)/*NOTE*/arrayNode.Char;
                                            #endregion IF IsChar

                                            #region IF IsBool
                                            array[++arrayIndex] = /*NOTE*/(@FullName)(object)/*NOTE*/arrayNode.Bool;
                                            #endregion IF IsBool

                                            #region IF IsDateTime
                                            array[++arrayIndex] = /*NOTE*/(@FullName)(object)/*NOTE*/arrayNode.DateTime;
                                            #endregion IF IsDateTime

                                            #region IF Type.IsEnum
                                            array[++arrayIndex] = fastCSharp.Enum.parseEnum<@FullName>(arrayNode.String, default(@FullName));
                                            #endregion IF Type.IsEnum

                                            #region IF IsTryParseJson
                                            #region IF IsTryParseInt
                                            if (arrayNode.Type == fastCSharp.setup.cSharp.ajax.jsonNode.nodeType.Int) array[++arrayIndex] = (@NotNullType.FullName)/*NOTE*/(object)/*NOTE*/arrayNode.Int;
                                            else
                                            #endregion IF IsTryParseInt
                                            {
                                                #region IF NullType
                                                @NotNullType.FullName arrayNullValue;
                                                ++arrayIndex;
                                                if (@NotNullType.FullName/**/.TryParse(arrayNode.NumberString, out arrayNullValue)) array[arrayIndex] = arrayNullValue;
                                                else
                                                #endregion IF NullType
                                                    #region NOT NullType
                                                    if (!@FullName/**/.TryParse(arrayNode.NumberString, out array[++arrayIndex]))
                                                    #endregion NOT NullType
                                                    {
                                                        fastCSharp.log.Default.Throw("@MemberType.FullName : " + currentMemberName + @" 转换失败
" + arrayNode.NumberString, true, false);
                                                    }
                                            }
                                            #endregion IF IsTryParseJson

                                            #region IF IsParseJson
                                            @FullName arrayValue = new @FullName();
                                            (/*NOTE*/(fastCSharp.setup.cSharp.ajax.IParseJson)/*NOTE*/arrayValue).FromJson(arrayNode);
                                            array[++arrayIndex] = arrayValue;
                                            #endregion IF IsParseJson
                                        }
                                    }
                                    #region IF MemberType.IsArrayOrInterface
                                    value.@MemberName = array;
                                    #endregion IF MemberType.IsArrayOrInterface

                                    #region NOT MemberType.IsArrayOrInterface
                                    value.@MemberName = new @MemberType.FullName(array);
                                    #endregion NOT MemberType.IsArrayOrInterface
                                }
                                #endregion IF CanParseJsonEnumerable
                                #endregion PUSH MemberType.EnumerableArgumentType
                                #endregion IF MemberType.IsEnumerable
                            }
                        }
                        #region IF MemberType.IsParseJson
                        #region NOT ToJsonAttribute.IsParseNode
                        else
                        {
                            @MemberType.FullName newValue = new @MemberType.FullName();
                            ((fastCSharp.setup.cSharp.ajax.IParseJson)newValue).FromJson(Node);
                            value.@MemberName = newValue;
                        }
                        #endregion NOT ToJsonAttribute.IsParseNode
                        #endregion IF MemberType.IsParseJson
                        #endregion IF IsParseJson
                        #endregion LOOP Members
                    }
                    return value;
                }
            }
            /// <summary>
            /// JSON字符串转换成对象
            /// </summary>
            /// <param name="json">JSON字符串</param>
            public void FromJson(string json)
            {
                FromJson(new fastCSharp.setup.cSharp.ajax.jsonParser().Parse(json));
            }
            /// <summary>
            /// JSON字符串解析节点换成对象
            /// </summary>
            /// <param name="node">JSON字符串解析节点</param>
            public void FromJson(fastCSharp.setup.cSharp.ajax.jsonNode node)
            {
                jsonParser parser = new jsonParser { Node = node };
                /*NOT:type.IsNull*/
                @type.FullName value = /*NOT:type.IsNull*/parser.Parse(/*NOTE*/(@type.FullName)(object)/*NOTE*/this);
                /*NOT:type.IsNull*/
                CopyFrom(value);/*NOT:type.IsNull*/
            }
            #endregion PART PARSEJSON
        }
        #endregion IF Attribute.IsParseJson
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 是否需要循环检测
        /// </summary>
        public const bool IsPushParent = false;
    }
    #endregion NOTE
}

