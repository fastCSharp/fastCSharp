using System;

namespace fastCSharp.setup.cSharp.template
{
    class memberMap : pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            #region PART BODY
            #region IF IsMemberMap
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
                /// <param name="members">成员集合</param>
                public memberMap(params member[] members)
                {
                    map = new byte[((@MaxMemberIndex >> 5) + 1) << 2];
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
                        return ((@MaxMemberIndex >> 5) + 1) << 2;
                    }
                }
                /// <summary>
                /// 设置成员索引,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    if (map == null) map = new byte[((@MaxMemberIndex >> 5) + 1) << 2];
                    map[memberIndex >> 3] |= (byte)(1 << (memberIndex & 7));
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name="member">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    if (map != null) map[memberIndex >> 3] &= (byte)(byte.MaxValue ^ (1 << (memberIndex & 7)));
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
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
                /// <param name="memberMap">成员位图</param>
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
                /// <param name="memberMap">成员位图</param>
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
                /// <param name="memberIndex">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == null || (map[memberIndex >> 3] & (1 << (memberIndex & 7))) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name="member">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public unsafe void Serialize(memoryStream stream)
                {
                    fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="data">数据</param>
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
            }
            #endregion IF IsMemberMap

            #region IF Member32
            /// <summary>
            /// 成员位图
            /// </summary>
            public struct @Member32 : fastCSharp.setup.cSharp.IMemberMap<@Member32>
            {
                /// <summary>
                /// 成员位图
                /// </summary>
                private uint map;
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <param name="members">成员集合</param>
                public @Member32(params member[] members)
                {
                    map = 0;
                    foreach (member member in members) SetMember((int)member);
                }
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <returns>成员位图</returns>
                public @Member32 Copy()
                {
                    return new @Member32 { map = map };
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
                /// <param name="memberIndex">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    map |= 1U << (int)memberIndex;
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    map &= uint.MaxValue ^ (1U << (int)memberIndex);
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name="member">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void And(@Member32 memberMap)
                {
                    map &= memberMap.map;
                }
                /// <summary>
                /// 成员异或运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void Xor(@Member32 memberMap)
                {
                    map ^= memberMap.map;
                }
                /// <summary>
                /// 成员并集运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void Or(@Member32 memberMap)
                {
                    map |= memberMap.map;
                }
                /// <summary>
                /// 判断成员索引是否有效
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == 0 || (map & (1 << memberIndex)) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name="member">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public void Serialize(memoryStream stream)
                {
                    stream.Write(map);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    stream.Write(BitConverter.GetBytes(map), 0, sizeof(uint));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="data">数据</param>
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
            }
            #endregion IF Member32

            #region IF Member64
            /// <summary>
            /// 成员位图
            /// </summary>
            public struct @Member64 : fastCSharp.setup.cSharp.IMemberMap<@Member64>
            {
                /// <summary>
                /// 成员位图
                /// </summary>
                private ulong map;
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <param name="members">成员集合</param>
                public @Member64(params member[] members)
                {
                    map = 0;
                    foreach (member member in members) SetMember((int)member);
                }
                /// <summary>
                /// 成员位图
                /// </summary>
                /// <returns>成员位图</returns>
                public @Member64 Copy()
                {
                    return new @Member64 { map = map };
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
                /// <param name="memberIndex">成员索引</param>
                public void SetMember(int memberIndex)
                {
                    map |= 1UL << (int)memberIndex;
                }
                /// <summary>
                /// 清除成员索引,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                public void ClearMember(int memberIndex)
                {
                    map &= ulong.MaxValue ^ (1UL << (int)memberIndex);
                }
                /// <summary>
                /// 设置成员,忽略默认成员
                /// </summary>
                /// <param name="member">成员</param>
                public void SetMember(member member)
                {
                    SetMember((int)member);
                }
                /// <summary>
                /// 清除成员,忽略默认成员
                /// </summary>
                /// <param name="memberIndex">成员</param>
                public void ClearMember(member member)
                {
                    ClearMember((int)member);
                }
                /// <summary>
                /// 成员交集运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void And(@Member64 memberMap)
                {
                    map &= memberMap.map;
                }
                /// <summary>
                /// 成员异或运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void Xor(@Member64 memberMap)
                {
                    map ^= memberMap.map;
                }

                /// <summary>
                /// 成员并集运算,忽略默认成员
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                public void Or(@Member64 memberMap)
                {
                    map |= memberMap.map;
                }
                /// <summary>
                /// 判断成员索引是否有效
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                /// <returns>成员索引是否有效</returns>
                public bool IsMember(int memberIndex)
                {
                    return map == 0 || (map & (1UL << memberIndex)) != 0;
                }
                /// <summary>
                /// 判断成员是否有效
                /// </summary>
                /// <param name="member">成员</param>
                /// <returns>成员是否有效</returns>
                public bool IsMember(member member)
                {
                    return IsMember((int)member);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public void Serialize(memoryStream stream)
                {
                    stream.Write(map);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="stream">数据流</param>
                public void Serialize(System.IO.Stream stream)
                {
                    stream.Write(BitConverter.GetBytes(map), 0, sizeof(ulong));
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="data">数据</param>
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
            }
            #endregion IF Member64

            /// <summary>
            /// 成员
            /// </summary>
            public enum member
            {
                #region LOOP Members
                #region NOT MemberType.Type.IsGenericParameter
                [fastCSharp.setup.cSharp.member(Type = typeof(@MemberType.FullName))]
                #endregion NOT MemberType.Type.IsGenericParameter
                @MemberName = @MemberIndex,
                #endregion LOOP Members
            }
            /// <summary>
            /// 成员信息获取器
            /// </summary>
            private struct memberInfo : fastCSharp.setup.cSharp.IMemberInfo
            {
                /// <summary>
                /// 获取成员名称
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
                /// <returns>成员名称</returns>
                public string GetName(int memberIndex)
                {
                    return ((member)memberIndex).ToString();
                }
                /// <summary>
                /// 获取成员类型
                /// </summary>
                /// <param name="memberIndex">成员索引</param>
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
                        return @MaxMemberIndex + 1;
                    }
                }
            }
            #endregion PART BODY
        }
        #endregion PART CLASS
    }
    #region NOTE
    internal partial class pub
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        public class memberMap : pub, IMemberMap<memberMap>
        {
            /// <summary>
            /// 成员位图
            /// </summary>
            /// <returns>成员位图</returns>
            public memberMap Copy() { return this; }
            /// <summary>
            /// 是否默认全部成员有效
            /// </summary>
            public bool IsDefault { get { return false; } }
            /// <summary>
            /// 序列化字节长度
            /// </summary>
            public new int SerializeSize { get { return 0; } }
            /// <summary>
            /// 设置成员索引
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            public void SetMember(int memberIndex) { }
            /// <summary>
            /// 清除成员索引
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            public void ClearMember(int memberIndex) { }
            /// <summary>
            /// 成员交集运算,忽略默认成员
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            public void And(memberMap memberMap) { }
            /// <summary>
            /// 成员异或运算,忽略默认成员
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            public void Xor(memberMap memberMap) { }
            /// <summary>
            /// 成员并集运算,忽略默认成员
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            public void Or(memberMap memberMap) { }
            /// <summary>
            /// 判断成员索引是否有效
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            /// <returns>成员索引是否有效</returns>
            public bool IsMember(int memberIndex) { return false; }
            /// <summary>
            /// 获取成员名称
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            /// <returns>成员名称</returns>
            public string GetMemberName(int memberIndex) { return null; }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            public void Serialize(memoryStream stream) { }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="stream">数据流</param>
            public void Serialize(System.IO.Stream stream) { }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="data">数据</param>
            /// <returns>结束位置</returns>
            public unsafe byte* DeSerialize(byte* data) { return data; }
            /// <summary>
            /// 成员获取器
            /// </summary>
            /// <returns>成员获取器</returns>
            public IMemberInfo MemberInfo
            {
                get { return null; }
            }
        }
    }
    #endregion NOTE
}
