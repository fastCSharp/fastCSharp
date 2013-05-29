using System;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 成员复制接口
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    /// <typeparam name="memberType">成员位图类型</typeparam>
    public interface ICopy<valueType, memberType> where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 成员复制
        /// </summary>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">复制成员位图</param>
        void CopyFrom(valueType value, memberType memberMap = default(memberType));
        /// <summary>
        /// 浅复制对象
        /// </summary>
        /// <returns>复制的对象</returns>
        valueType CopyMember();
    }
    /// <summary>
    /// 成员复制代码生成
    /// </summary
    [auto(Name = "成员复制", DependType = typeof(coder.cSharper))]
    internal partial class copy : cSharper
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        protected memberMap.cSharp memberMap = new memberMap.cSharp();
        /// <summary>
        /// 成员集合
        /// </summary>
        public setup.memberInfo[] Members;
        /// <summary>
        /// 无参数构造函数调用属性名称
        /// </summary>
        public string ConstructorName = constructor.ConstructorName;
        /// <summary>
        /// 是否存在无参构造函数
        /// </summary>
        public bool IsConstructor;
        /// <summary>
        /// 生成成员位图
        /// </summary>
        /// <param name="type">类型</param>
        public void create(Type type)
        {
            Members = setup.memberInfo.GetMembers(this.type = type, setup.memberFilter.Instance)
                .getFindArray(value => value.CanGet && value.CanSet && !value.IsIgnore);
            IsConstructor = !type.isStruct() && type.GetConstructor(nullValue<Type>.Array) != null;
            memberMap.create(type);
            create(true);
        }
    }
    /// <summary>
    /// 成员复制(反射模式)
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public static class copy<valueType>
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="copyValue">被复制对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        public static valueType Copy(valueType value, valueType copyValue
            , setup.memberFilter filter = setup.memberFilter.InstanceField, memberMap<valueType> memberMap = default(memberMap<valueType>))
        {
            if (isStruct) return memberGroup.CopyValue(value, copyValue, filter, memberMap);
            else
            {
                if (value == null || copyValue == null) log.Default.Throw(log.exceptionType.Null);
                memberGroup.Copy(value, copyValue, filter, memberMap);
                return value;
            }
        }
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isStruct = typeof(valueType).isStruct() || typeof(valueType).IsEnum;
        /// <summary>
        /// 动态成员分组
        /// </summary>
        private static readonly memberGroup<valueType> memberGroup = memberGroup<valueType>.Create(value => value.CanGet && value.CanSet);
    }
}
