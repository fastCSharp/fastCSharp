using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public class constructor : ignore
    {
        /// <summary>
        /// 默认构造函数所在类型(不能是泛型类),所有无参数静态函数都将被缓存
        /// </summary>
        public Type Type;
        /// <summary>
        /// 是否克隆生成新对象
        /// </summary>
        public bool IsClone;
        /// <summary>
        /// 无参数构造函数调用属性名称
        /// </summary>
        public const string ConstructorName = "fastCSharpConstructor";
        /// <summary>
        /// 默认构造函数缓存
        /// </summary>
        private static readonly Dictionary<hashCode<Type>, Dictionary<hashCode<Type>, MethodInfo>> cache = new Dictionary<hashCode<Type>, Dictionary<hashCode<Type>, MethodInfo>>();
        /// <summary>
        /// 默认构造函数缓存访问锁
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 获取默认构造函数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="type">默认构造函数所在类型</param>
        /// <returns>默认构造函数,失败返回null</returns>
        protected internal static func<valueType> get<valueType>(Type type)
        {
            if (type != null)
            {
                if (type.IsGenericType) log.Default.Throw(log.exceptionType.ErrorOperation);
                hashCode<Type> hashType = type;
                Monitor.Enter(cacheLock);
                try
                {
                    Dictionary<hashCode<Type>, MethodInfo> constructors;
                    if (!cache.TryGetValue(hashType, out constructors))
                    {
                        constructors = new Dictionary<hashCode<Type>, MethodInfo>();
                        foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            Type returnType = method.ReturnType;
                            if (returnType != typeof(void) && method.GetParameters().Length == 0)
                            {
                                if (method.IsGenericMethodDefinition)
                                {
                                    if (returnType.IsGenericType)
                                    {
                                        constructors[returnType.GetGenericTypeDefinition()] = method.GetGenericMethodDefinition();
                                    }
                                }
                                else constructors[method.ReturnType] = method;
                            }
                        }
                        cache.Add(hashType, constructors);
                    }
                    return get<valueType>(constructors);
                }
                finally { Monitor.Exit(cacheLock); }
            }
            return null;
        }
        /// <summary>
        /// 获取默认构造函数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="constructors">默认构造函数集合</param>
        /// <returns>默认构造函数,失败返回null</returns>
        private static func<valueType> get<valueType>(Dictionary<hashCode<Type>, MethodInfo> constructors)
        {
            Type type = typeof(valueType);
            if (type.IsGenericType)
            {
                MethodInfo method;
                if (constructors.TryGetValue(type.GetGenericTypeDefinition(), out method))
                {
                    if (!method.IsGenericMethodDefinition)
                    {
                        log.Default.Throw(type.fullName() + " 无法匹配构造函数 " + method.fullName(), false, true);
                    }
                    return (func<valueType>)Delegate.CreateDelegate(typeof(func<valueType>), method.MakeGenericMethod(type.GetGenericArguments()));
                }
            }
            else
            {
                MethodInfo method;
                hashCode<Type> hashType = type;
                if (constructors.TryGetValue(hashType, out method))
                {
                    if (method.IsGenericMethodDefinition)
                    {
                        log.Default.Throw(type.fullName() + " 无法匹配构造函数 " + method.fullName(), false, true);
                    }
                    constructors.Remove(hashType);
                    return (func<valueType>)Delegate.CreateDelegate(typeof(func<valueType>), method);
                }
            }
            return null;
        }
        /// <summary>
        /// 构造新对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>新对象</returns>
        public static object Get(Type type)
        {
            return ((func<object>)Delegate.CreateDelegate(typeof(func<object>), getNewMethod.MakeGenericMethod(type)))();
        }
        /// <summary>
        /// 构造新对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>新对象</returns>
        private static object getNew<valueType>()
        {
            return constructor<valueType>.New;
        }
        /// <summary>
        /// 获取默认空值 函数信息
        /// </summary>
        private static readonly MethodInfo getNewMethod = typeof(constructor).GetMethod("getNew", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取默认空值
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>默认空值</returns>
        public static object GetNull(Type type)
        {
            return ((func<object>)Delegate.CreateDelegate(typeof(func<object>), getNullMethod.MakeGenericMethod(type)))();
        }
        /// <summary>
        /// 获取默认空值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>默认空值</returns>
        private static object getNull<valueType>()
        {
            return default(valueType);
        }
        /// <summary>
        /// 获取默认空值 函数信息
        /// </summary>
        private static readonly MethodInfo getNullMethod = typeof(constructor).GetMethod("getNull", BindingFlags.Static | BindingFlags.NonPublic);
    }
    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public static class constructor<valueType>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private static readonly func<valueType> method;
        /// <summary>
        /// 对象复制器
        /// </summary>
        private static readonly func<valueType, object> clone;
        /// <summary>
        /// 构造函数信息
        /// </summary>
        private static readonly ConstructorInfo constructorInfo;
        /// <summary>
        /// 默认对象值
        /// </summary>
        private static readonly valueType value;
        /// <summary>
        /// 默认空值
        /// </summary>
        /// <returns>默认空值</returns>
        public static valueType Null()
        {
            return default(valueType);
        }
        /// <summary>
        /// 构造新对象
        /// </summary>
        public static valueType New
        {
            get
            {
                return method();
            }
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns>对象</returns>
        private static valueType callClone()
        {
            return (valueType)clone(value);
        }
        /// <summary>
        /// 调用构造函数生成对象
        /// </summary>
        /// <returns>对象</returns>
        private static valueType callConstructor()
        {
            return (valueType)constructorInfo.Invoke(null);
        }

        static constructor()
        {
            Type type = typeof(valueType);
            if (type.isStruct() || type.IsEnum || type.IsArray || type == typeof(string)) method = Null;
            else
            {
                constructor attribute = type.customAttribute<constructor>();
                if (attribute != null && attribute.Type != null) method = constructor.get<valueType>(attribute.Type);
                if (method == null)
                {
                    PropertyInfo propertyInfo = type.GetProperty(constructor.ConstructorName, BindingFlags.Static | BindingFlags.NonPublic);
                    if (propertyInfo != null && propertyInfo.PropertyType == type && propertyInfo.CanRead)
                    {
                        method = reflection.propertyGetter<valueType>.StaticCreator.Create(propertyInfo);
                    }
                    if (method == null)
                    {
                        if (!type.IsInterface && !type.IsAbstract)
                        {
                            constructorInfo = type.GetConstructor(nullValue<Type>.Array);
                            if (constructorInfo != null)
                            {
                                if (attribute == null || !attribute.IsClone) method = callConstructor;
                                else
                                {
                                    clone = (func<valueType, object>)Delegate.CreateDelegate(typeof(func<valueType, object>), type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
                                    value = callConstructor();
                                    method = callClone;
                                }
                            }
                        }
                        if (method == null)
                        {
                            method = Null;
                            log.Default.Add(type.fullName() + " 没有合适的找到构造函数", true, false);
                        }
                    }
                }
            }
        }
    }
}
