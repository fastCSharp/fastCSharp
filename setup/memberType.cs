using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Threading;
using fastCSharp.reflection;
using fastCSharp.sql;

namespace fastCSharp.setup
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal partial class memberType
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; protected set; }
        /// <summary>
        /// SQL类型
        /// </summary>
        protected Type sqlType;
        /// <summary>
        /// 自定义类型名称
        /// </summary>
        public string TypeName { get; private set; }
        /// <summary>
        /// 自定义类型名称
        /// </summary>
        public string TypeOnlyName { get; private set; }
        /// <summary>
        /// 类型全名
        /// </summary>
        private string fullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        public string FullName
        {
            get
            {
                if (fullName == null) fullName = Type != null ? Type.fullName() : TypeName;
                return fullName;
            }
        }
        /// <summary>
        /// 是否object
        /// </summary>
        public bool IsObject
        {
            get { return Type == typeof(object); }
        }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsString
        {
            get { return Type == typeof(string); }
        }
        /// <summary>
        /// 是否字符类型(包括可空类型)
        /// </summary>
        public bool IsChar
        {
            get { return Type == typeof(char) || Type == typeof(char?); }
        }
        /// <summary>
        /// 是否逻辑类型(包括可空类型)
        /// </summary>
        public bool IsBool
        {
            get { return Type == typeof(bool) || Type == typeof(bool?); }
        }
        /// <summary>
        /// 是否时间类型(包括可空类型)
        /// </summary>
        public bool IsDateTime
        {
            get { return Type == typeof(DateTime) || Type == typeof(DateTime?); }
        }
        /// <summary>
        /// 是否数字类型(包括可空类型)
        /// </summary>
        public bool IsDecimal
        {
            get { return Type == typeof(decimal) || Type == typeof(decimal?); }
        }
        /// <summary>
        /// 是否Guid类型(包括可空类型)
        /// </summary>
        public bool IsGuid
        {
            get { return Type == typeof(Guid) || Type == typeof(Guid?); }
        }
        /// <summary>
        /// 是否字节数组
        /// </summary>
        public bool IsByteArray
        {
            get { return Type == typeof(byte[]); }
        }
        /// <summary>
        /// 是否引用类型
        /// </summary>
        public bool IsNull
        {
            get { return Type == null || Type.isNull(); }
        }
        /// <summary>
        /// 是否值类型(排除可空类型)
        /// </summary>
        public bool IsStruct
        {
            get { return Type.isStruct() && Type.nullableType() == null; }
        }
        /// <summary>
        /// 是否数组或者接口
        /// </summary>
        public bool IsArrayOrInterface
        {
            get
            {
                return Type.IsArray || Type.IsInterface;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="name">类型名称</param>
        public memberType(string name)
        {
            TypeName = TypeOnlyName = name;
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="type">类型</param>
        protected memberType(Type type)
        {
            this.Type = type;
            TypeName = type != null ? type.name() : null;
            TypeOnlyName = type != null ? type.onlyName() : null;
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sqlType">SQL类型</param>
        public memberType(Type type, Type sqlType)
            : this(type)
        {
            this.sqlType = sqlType;
        }
        /// <summary>
        /// 空类型
        /// </summary>
        public static readonly memberType Null = new memberType((Type)null);
        /// <summary>
        /// 成员类型隐式转换集合
        /// </summary>
        protected static readonly Dictionary<hashCode<Type>, memberType> types = new Dictionary<hashCode<Type>, memberType>();
        /// <summary>
        /// 隐式转换集合转换锁
        /// </summary>
        private static int typeLock;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">成员类型</param>
        /// <returns>类型</returns>
        public static implicit operator Type(memberType value)
        {
            return value != null ? value.Type : null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员类型</returns>
        public static implicit operator memberType(Type type)
        {
            if (type == null) return Null;
            memberType value;
            hashCode<Type> hashType = type;
            while (Interlocked.CompareExchange(ref typeLock, 1, 0) != 0) Thread.Sleep(1);
            try
            {
                if (!types.TryGetValue(hashType, out value)) types.Add(hashType, value = new memberType(type));
            }
            finally { typeLock = 0; }
            return value;
        }
        /// <summary>
        /// 可枚举泛型构造信息
        /// </summary>
        public ConstructorInfo EnumerableConstructor { get; private set; }
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        private memberType enumerableType;
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        public memberType EnumerableType
        {
            get
            {
                if (enumerableType == null)
                {
                    if (!IsString)
                    {
                        Type value = Type.getGenericInterface(typeof(IEnumerable<>));
                        if (value != null)
                        {
                            if (Type.IsInterface)
                            {
                                Type interfaceType = Type.GetGenericTypeDefinition();
                                if (interfaceType == typeof(IEnumerable<>) || interfaceType == typeof(ICollection<>)
                                    || interfaceType == typeof(IList<>))
                                {
                                    enumerableArgumentType = value.GetGenericArguments()[0];
                                    enumerableType = value;
                                }
                            }
                            else if (Type.IsArray)
                            {
                                enumerableArgumentType = value.GetGenericArguments()[0];
                                enumerableType = value;
                            }
                            else
                            {
                                Type enumerableArgumentType = value.GetGenericArguments()[0];
                                Type[] parameters = new Type[1];
                                parameters[0] = enumerableArgumentType.MakeArrayType();
                                EnumerableConstructor = Type.GetConstructor(parameters);
                                if (EnumerableConstructor != null)
                                {
                                    this.enumerableArgumentType = enumerableArgumentType;
                                    enumerableType = value;
                                }
                                else
                                {
                                    parameters[0] = typeof(IList<>).MakeGenericType(enumerableArgumentType);
                                    EnumerableConstructor = Type.GetConstructor(parameters);
                                    if (EnumerableConstructor != null)
                                    {
                                        this.enumerableArgumentType = enumerableArgumentType;
                                        enumerableType = value;
                                    }
                                    else
                                    {
                                        parameters[0] = typeof(ICollection<>).MakeGenericType(enumerableArgumentType);
                                        EnumerableConstructor = Type.GetConstructor(parameters);
                                        if (EnumerableConstructor != null)
                                        {
                                            this.enumerableArgumentType = enumerableArgumentType;
                                            enumerableType = value;
                                        }
                                        else
                                        {
                                            parameters[0] = typeof(IEnumerable<>).MakeGenericType(enumerableArgumentType);
                                            EnumerableConstructor = Type.GetConstructor(parameters);
                                            if (EnumerableConstructor != null)
                                            {
                                                this.enumerableArgumentType = enumerableArgumentType;
                                                enumerableType = value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (enumerableType == null) enumerableType = Null;
                }
                return enumerableType.Type != null ? enumerableType : null;
            }
        }
        /// <summary>
        /// 是否可枚举类型
        /// </summary>
        public bool IsEnumerable
        {
            get
            {
                return EnumerableType != null;
            }
        }
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        private memberType enumerableArgumentType;
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        public memberType EnumerableArgumentType
        {
            get
            {
                return EnumerableType != null ? enumerableArgumentType : null;
            }
        }
        /// <summary>
        /// 可控类型的值类型
        /// </summary>
        private memberType nullType;
        /// <summary>
        /// 可控类型的值类型
        /// </summary>
        public memberType NullType
        {
            get
            {
                if (nullType == null) nullType = (memberType)Type.nullableType();
                return nullType.Type != null ? nullType : null;
            }
        }
        /// <summary>
        /// 非可控类型为null
        /// </summary>
        public memberType NotNullType
        {
            get { return NullType != null ? nullType : this; }
        }
        /// <summary>
        /// 非可控类型为null
        /// </summary>
        public string StructNotNullType
        {
            get { return NotNullType.FullName; }
        }
        ///// <summary>
        ///// 结构体非可空类型
        ///// </summary>
        //private string structType;
        ///// <summary>
        ///// 结构体非可空类型
        ///// </summary>
        //public string StructType
        //{
        //    get
        //    {
        //        if (structType == null)
        //        {
        //            Type type = Type.nullableType();
        //            structType = type == null ? fullName : type.fullName();
        //        }
        //        return structType;
        //    }
        //}
        /// <summary>
        /// 是否拥有静态转换函数
        /// </summary>
        private bool? isTryParse;
        /// <summary>
        /// 是否拥有静态转换函数
        /// </summary>
        public bool IsTryParse
        {
            get
            {
                if (isTryParse == null) isTryParse = (Type.nullableType() ?? Type).getTryParse() != null;
                return (bool)isTryParse;
            }
        }
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public memberType pairKeyType;
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public memberType PairKeyType
        {
            get
            {
                if (pairKeyType == null)
                {
                    if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                    {
                        pairKeyType = Type.GetGenericArguments()[0];
                    }
                    else pairKeyType = Null;
                }
                return pairKeyType.Type != null ? pairKeyType : null;
            }
        }
        /// <summary>
        /// 键值对值类型
        /// </summary>
        public memberType pairValueType;
        /// <summary>
        /// 键值对值类型
        /// </summary>
        public memberType PairValueType
        {
            get
            {
                if (pairValueType == null)
                {
                    if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                    {
                        pairValueType = Type.GetGenericArguments()[1];
                    }
                    else pairValueType = Null;
                }
                return pairValueType.Type != null ? pairValueType : null;
            }
        }
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public memberType keyValueType;
        /// <summary>
        /// 键值对键类型
        /// </summary>
        public memberType KeyValueType
        {
            get
            {
                if (keyValueType == null)
                {
                    if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(keyValue<,>))
                    {
                        keyValueType = Type.GetGenericArguments()[0];
                    }
                    else keyValueType = Null;
                }
                return keyValueType.Type != null ? keyValueType : null;
            }
        }
        /// <summary>
        /// 泛型参数集合
        /// </summary>
        private memberType[] genericParameters;
        /// <summary>
        /// 泛型参数集合
        /// </summary>
        public memberType[] GenericParameters
        {
            get
            {
                if (genericParameters == null)
                {
                    genericParameters = Type.IsGenericType ? Type.GetGenericArguments().getArray(value => (memberType)value) : nullValue<memberType>.Array;
                }
                return genericParameters;
            }
        }
        /// <summary>
        /// 泛型参数名称
        /// </summary>
        public string GenericParameterNames
        {
            get
            {
                return GenericParameters.joinString(',', value => value.FullName);
            }
        }
    }
}
