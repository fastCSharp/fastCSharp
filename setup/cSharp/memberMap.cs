using System;
using System.IO;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 成员位图接口
    /// </summary>
    public unsafe interface IMemberMap
    {
        /// <summary>
        /// 是否默认全部成员有效
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        /// 序列化字节长度
        /// </summary>
        int SerializeSize { get; }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        void SetMember(int memberIndex);
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        void ClearMember(int memberIndex);
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        bool IsMember(int memberIndex);
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream">数据流</param>
        void Serialize(memoryStream stream);
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream">数据流</param>
        void Serialize(Stream stream);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结束位置</returns>
        byte* DeSerialize(byte* data);
        /// <summary>
        /// 成员获取器
        /// </summary>
        /// <returns>成员获取器</returns>
        IMemberInfo MemberInfo { get; }
    }
    /// <summary>
    /// 成员位图接口
    /// </summary>
    /// <typeparam name="memberType">成员位图类型</typeparam>
    public interface IMemberMap<memberType> : IMemberMap
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        memberType Copy();
        /// <summary>
        /// 成员交集运算,忽略默认成员
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        void And(memberType memberMap);
        /// <summary>
        /// 成员异或运算,忽略默认成员
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        void Xor(memberType memberMap);
        /// <summary>
        /// 成员并集运算,忽略默认成员
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        void Or(memberType memberMap);
    }
    /// <summary>
    /// 成员位图代码生成自定义属性
    /// </summary>
    public partial class memberMap : ignore
    {
        /// <summary>
        /// 成员设置获取器
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        internal struct memberGetter<valueType>
        {
            /// <summary>
            /// 数据对象
            /// </summary>
            private valueType value;
            /// <summary>
            /// 获取对象成员的委托集合
            /// </summary>
            private func<valueType, object>[] getValues;
            /// <summary>
            /// 成员设置获取器
            /// </summary>
            /// <param name="value">数据对象</param>
            /// <param name="getValues">获取对象成员的委托集合</param>
            public memberGetter(valueType value, func<valueType, object>[] getValues)
            {
                this.value = value;
                this.getValues = getValues;
            }
            /// <summary>
            /// 获取成员值
            /// </summary>
            /// <param name="index">成员索引</param>
            /// <returns>成员值</returns>
            public object this[int index]
            {
                get { return getValues[index](value); }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="map">成员位图</param>
        /// <param name="serializeSize">序列化字节长度</param>
        /// <param name="stream">数据流</param>
        public static unsafe void Serialize(byte[] map, int serializeSize, memoryStream stream)
        {
            if (map == null)
            {
                stream.PrepLength(serializeSize);
                fixed (byte* dataFixed = stream.Array)
                {
                    fastCSharp.unsafer.memory.Fill(dataFixed + stream.Length, 0xffffffffU, serializeSize >> 2);
                }
                stream.Unsafer.AddLength(serializeSize);
            }
            else stream.Write(map, 0, serializeSize);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="map">成员位图</param>
        /// <param name="serializeSize">序列化字节长度</param>
        /// <param name="stream">数据流</param>
        public static void Serialize(byte[] map, int serializeSize, Stream stream)
        {
            if (map == null)
            {
                while (serializeSize-- != 0) stream.WriteByte(255);
            }
            else stream.Write(map, 0, serializeSize);
        }
        /// <summary>
        /// 成员位图代码生成
        /// </summary
        [auto(Name = "成员位图", DependType = typeof(coder.cSharper))]
        internal partial class cSharp : cSharper
        {
            /// <summary>
            /// 成员位图类型名称
            /// </summary>
            public const string MemberMapTypeName = "memberMap";
            /// <summary>
            /// 成员集合
            /// </summary>
            public setup.memberInfo[] Members;
            /// <summary>
            /// 最大成员索引
            /// </summary>
            private int maxMemberIndex;
            /// <summary>
            /// 最大成员索引
            /// </summary>
            public int MaxMemberIndex
            {
                get
                {
                    if (maxMemberIndex == 0) maxMemberIndex = Members.maxKey(member => member.MemberIndex, 0);
                    return maxMemberIndex;
                }
            }
            /// <summary>
            /// 32位成员位图类型名称
            /// </summary>
            public string Member32
            {
                get
                {
                    return MaxMemberIndex < 32 ? MemberMapTypeName : null;
                }
            }
            /// <summary>
            /// 64位成员位图类型名称
            /// </summary>
            public string Member64
            {
                get
                {
                    return MaxMemberIndex >= 32 && MaxMemberIndex < 64 ? MemberMapTypeName : null;
                }
            }
            /// <summary>
            /// 成员数量是否超过64
            /// </summary>
            public bool IsMemberMap
            {
                get
                {
                    return MaxMemberIndex >= 64;
                }
            }
            /// <summary>
            /// 可赋值成员位图
            /// </summary>
            public ulong CanSetMember
            {
                get
                {
                    return Members.getFind(value => value.CanSet)
                        .sum(value => 1UL << value.MemberIndex);
                }
            }
            /// <summary>
            /// 可赋值成员位图
            /// </summary>
            public string CanSetMemberMap
            {
                get
                {
                    bitMap.unsafer map = new bitMap(Members.Length).Unsafer;
                    foreach (setup.memberInfo member in Members)
                    {
                        if (member.CanSet) map.Set(member.MemberIndex);
                    }
                    return map.Map.joinString(',');
                }
            }
            /// <summary>
            /// 生成成员位图
            /// </summary>
            /// <param name="type">类型</param>
            public void create(Type type)
            {
                maxMemberIndex = 0;
                Members = setup.memberInfo.GetMembers(this.type = type, setup.memberFilter.Instance);
                create(true);
            }
            /// <summary>
            /// 生成部分代码
            /// </summary>
            /// <param name="partName">部分名称</param>
            /// <param name="typeName">类型名称</param>
            /// <param name="members">成员集合</param>
            /// <returns>部分代码</returns>
            public static string PartCode(string partName, memberType type, setup.memberInfo[] members)
            {
                cSharp coder = new cSharp();
                coder.type = type;
                coder.Members = members;
                coder.create(false);
                return coder._partCodes_[partName];
            }
        }
    }
    /// <summary>
    /// 成员位图(反射模式)
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public struct memberMap<valueType> : IMemberMap<memberMap<valueType>>
    {
        /// <summary>
        /// 成员信息集合
        /// </summary>
        private readonly static setup.memberInfo[] memberInfos = memberGroup<valueType>.GetAllMembers();
        /// <summary>
        /// 成员名称索引查找表
        /// </summary>
        private readonly static staticDictionary<string, int> nameIndexs;
        /// <summary>
        /// 序列化字节长度
        /// </summary>
        private readonly static int serializeSize;
        /// <summary>
        /// 成员位图
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <param name="members">成员名称集合</param>
        public memberMap(params string[] members)
        {

            map = new byte[serializeSize];
            foreach (string member in members)
            {
                int index = getIndex(member);
                map[index >> 3] |= (byte)(1 << (index & 7));
            }
        }
        /// <summary>
        /// 获取成员索引
        /// </summary>
        /// <param name="member">成员名称</param>
        /// <returns>成员索引</returns>
        private int getIndex(string member)
        {
            int index = nameIndexs.Get(member, -1);
            if (index == -1) log.Default.Throw(typeof(valueType).fullName() + " 未找到成员 " + member, true, true);
            return index;
        }
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        public memberMap<valueType> Copy()
        {
            return new memberMap<valueType> { map = this.map == null ? null : this.map.copy() };
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
                return serializeSize;
            }
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        public void SetMember(int memberIndex)
        {
            if (map == null) map = new byte[serializeSize];
            map[memberIndex >> 3] |= (byte)(1 << (memberIndex & 7));
        }
        /// <summary>
        /// 设置成员,忽略默认成员
        /// </summary>
        /// <param name="member">成员名称</param>
        public void SetMember(string member)
        {
            SetMember(getIndex(member));
        }
        /// <summary>
        /// 清除成员,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员名称</param>
        public void ClearMember(string member)
        {
            ClearMember(getIndex(member));
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
        public unsafe void And(memberMap<valueType> memberMap)
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
        public unsafe void Xor(memberMap<valueType> memberMap)
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
        public unsafe void Or(memberMap<valueType> memberMap)
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
        /// <param name="member">成员名称</param>
        /// <returns>成员是否有效</returns>
        public bool IsMember(string member)
        {
            return IsMember(getIndex(member));
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream">数据流</param>
        public unsafe void Serialize(memoryStream stream)
        {
            if (map == null && serializeSize <= 8)
            {
                if (serializeSize == 4) stream.Write((uint)0);
                else stream.Write((ulong)0);
            }
            else fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream">数据流</param>
        public void Serialize(System.IO.Stream stream)
        {
            if (map == null && serializeSize <= 8)
            {
                if (serializeSize == 4) stream.Write(BitConverter.GetBytes((uint)0), 0, sizeof(uint));
                else stream.Write(BitConverter.GetBytes((ulong)0), 0, sizeof(ulong));
            }
            else fastCSharp.setup.cSharp.memberMap.Serialize(map, SerializeSize, stream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结束位置</returns>
        public unsafe byte* DeSerialize(byte* data)
        {
            if (serializeSize == 4)
            {
                uint map = *(uint*)data;
                if (map == 0) this.map = null;
                else
                {
                    if (this.map == null) this.map = new byte[sizeof(uint)];
                    fixed (byte* mapFixed = this.map) *(uint*)mapFixed = map;
                }
            }
            else if (serializeSize == 8)
            {
                ulong map = *(ulong*)data;
                if (map == 0) this.map = null;
                else
                {
                    if (this.map == null) this.map = new byte[sizeof(ulong)];
                    fixed (byte* mapFixed = this.map) *(ulong*)mapFixed = map;
                }
            }
            else
            {
                if (map == null) map = new byte[SerializeSize];
                fixed (byte* mapFixed = map) fastCSharp.unsafer.memory.Copy(data, mapFixed, SerializeSize);
            }
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
        /// <summary>
        /// 成员信息获取器
        /// </summary>
        private struct memberInfo : IMemberInfo
        {
            /// <summary>
            /// 获取成员名称
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            /// <returns>成员名称</returns>
            public string GetName(int memberIndex)
            {
                return memberInfos[memberIndex].MemberName;
            }
            /// <summary>
            /// 获取成员类型
            /// </summary>
            /// <param name="memberIndex">成员索引</param>
            /// <returns>成员类型</returns>
            public Type GetType(int memberIndex)
            {
                return memberInfos[memberIndex].MemberType;
            }
            /// <summary>
            /// 所有成员数量
            /// </summary>
            public int MemberCount
            {
                get
                {
                    return memberInfos.Length;
                }
            }
        }
        static memberMap()
        {
            nameIndexs = new staticDictionary<string, int>(memberInfos.getArray(value => new keyValue<string, int>(value.MemberName, value.MemberIndex)));
            serializeSize = (((memberInfos.Length - 1) >> 5) + 1) << 2;
        }
    }
}