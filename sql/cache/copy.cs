using System;
using System.Reflection;
using fastCSharp.setup.cSharp;
using fastCSharp.reflection;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// 复制缓存
    /// </summary>
    internal static class copy
    {
        /// <summary>
        /// 复制成员数据
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="memberType">数据成员类型</typeparam>
        /// <param name="value">缓存数据</param>
        /// <param name="newValue">更新后的新数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private static void copyReflection<valueType>(valueType value, valueType newValue, memberMap<valueType> memberMap)
        {
            fastCSharp.setup.cSharp.copy<valueType>.Copy(value, newValue, setup.memberFilter.Instance, memberMap);
        }
        /// <summary>
        /// 复制成员数据
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="valueType">表格模型类型</typeparam>
        /// <typeparam name="memberType">数据成员类型</typeparam>
        /// <param name="value">缓存数据</param>
        /// <param name="newValue">更新后的新数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private static void copyInterface<valueType, modelType, memberType>(valueType value, valueType newValue, memberType memberMap)
            where valueType : modelType
            where modelType : ICopy<modelType, memberType>
            where memberType : IMemberMap<memberType>
        {
            value.CopyFrom(newValue, memberMap);
        }
        /// <summary>
        /// 获取复制成员委托
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="memberType">数据成员类型</typeparam>
        /// <returns>复制成员委托</returns>
        public static action<valueType, valueType, memberType> Get<valueType, memberType>()
        {
            if (typeof(memberType) == typeof(memberMap<valueType>))
            {
                return (action<valueType, valueType, memberType>)Delegate.CreateDelegate(typeof(action<valueType, valueType, memberType>), copyReflectionMethod.MakeGenericMethod(typeof(valueType)));
            }
            foreach (Type copyType in typeof(valueType).getGenericInterfaces(typeof(ICopy<,>)))
            {
                Type[] types = copyType.GetGenericArguments();
                if (types[1] == typeof(memberType))
                {
                    return (action<valueType, valueType, memberType>)Delegate.CreateDelegate(typeof(action<valueType, valueType, memberType>), copyInterfaceMethod.MakeGenericMethod(typeof(valueType), types[0], typeof(memberType)));
                }
            }
            log.Default.Throw(log.exceptionType.Null);
            return null;
        }
        /// <summary>
        /// 复制成员数据函数
        /// </summary>
        private static readonly MethodInfo copyReflectionMethod = typeof(copy).GetMethod("copyReflection", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 复制成员数据
        /// </summary>
        private static readonly MethodInfo copyInterfaceMethod = typeof(copy).GetMethod("copyInterface", BindingFlags.Static | BindingFlags.NonPublic);

    }
    /// <summary>
    /// 复制缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class copy<valueType, memberType> : sqlTool<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 复制成员数据
        /// </summary>
        protected static action<valueType, valueType, memberType> copyFrom = copy.Get<valueType, memberType>();
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <param name="newValue">更新后的新数据</param>
        /// <param name="memberMap">更新成员位图</param>
        protected void update(valueType value, valueType newValue, memberType memberMap)
        {
            copyFrom(value, newValue, updateMemberMap(memberMap));
        }
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <param name="newValue">更新后的新数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="updateMemberMap">更新成员位图</param>
        protected void update(valueType value, valueType newValue, valueType oldValue, memberType updateMemberMap)
        {
            memberType oldMemberMap = memberMap.Copy();
            oldMemberMap.Xor(updateMemberMap);
            copyFrom(oldValue, value, oldMemberMap);
            copyFrom(value, newValue, updateMemberMap);
        }
        /// <summary>
        /// 复制缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        protected copy(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap)
            : base(sqlTool, memberMap)
        {
        }
    }
}
