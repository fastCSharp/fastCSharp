using System;
using System.Collections.Generic;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    public static class type
    {
        /// <summary>
        /// 类型名称泛型分隔符
        /// </summary>
        public const char GenericSplit = '`';
        /// <summary>
        /// 类型名称集合
        /// </summary>
        private static readonly staticDictionary<hashCode<Type>, string> typeNames;
        /// <summary>
        /// 字段类型
        /// </summary>
        public static readonly Type RuntimeType = typeof(Type).Assembly.GetType("System.RuntimeType");
        /// <summary>
        /// 获取基本类型简称
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>基本类型简称,失败返回null</returns>
        public static string getTypeName(this Type type)
        {
            return typeNames.Get(type, null);
        }
        /// <summary>
        /// 类型名称生成器
        /// </summary>
        private struct nameBuilder
        {
            /// <summary>
            /// 名称缓存
            /// </summary>
            private charStream nameStream;
            /// <summary>
            /// 获取类型名称
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>类型名称</returns>
            public string GetFullName(Type type)
            {
                if (type.IsArray)
                {
                    nameStream = new charStream();
                    array(type, true);
                    return nameStream.ToString();
                }
                if (type.IsGenericType)
                {
                    nameStream = new charStream();
                    genericFullName(type);
                    return nameStream.ToString();
                }
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null) return type.Namespace + "." + type.Name;
                nameStream = new charStream();
                this.reflectedType(type, reflectedType);
                return nameStream.ToString();
            }
            /// <summary>
            /// 获取类型名称
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>类型名称</returns>
            public string GetName(Type type)
            {
                if (type.IsArray)
                {
                    nameStream = new charStream();
                    array(type, false);
                    return nameStream.ToString();
                }
                if (type.IsGenericType)
                {
                    nameStream = new charStream();
                    genericName(type);
                    return nameStream.ToString();
                }
                return type.Name;
            }
            /// <summary>
            /// 任意类型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void getFullName(Type type)
            {
                string value = typeNames.Get(type, null);
                if (value != null) nameStream.Write(value);
                else if (type.IsGenericParameter) nameStream.Write(type.Name);
                else if (type.IsArray) array(type, true);
                else if (type.IsGenericType) genericFullName(type);
                else
                {
                    Type reflectedType = type.ReflectedType;
                    if (reflectedType == null)
                    {
                        nameStream.Write(type.Namespace);
                        nameStream.Write('.');
                        nameStream.Write(type.Name);
                    }
                    else this.reflectedType(type, reflectedType);
                }
            }
            /// <summary>
            /// 任意类型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void getNameNoArray(Type type)
            {
                string value = typeNames.Get(type, null);
                if (value != null) nameStream.Write(value);
                else if (type.IsGenericParameter) nameStream.Write(type.Name);
                else if (type.IsGenericType) genericName(type);
                else nameStream.Write(type.Name);
            }
            /// <summary>
            /// 数组处理
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="isFullName">是否全称</param>
            private unsafe void array(Type type, bool isFullName)
            {
                int* ranks = stackalloc int[1024];
                int* currentRank = ranks, endRank = ranks + 1024;
                do
                {
                    if (currentRank == endRank) log.Default.Throw(log.exceptionType.IndexOutOfRange);
                    *currentRank++ = type.GetArrayRank();
                }
                while ((type = type.GetElementType()).IsArray);
                if (isFullName) getFullName(type);
                else getNameNoArray(type);
                while (currentRank != ranks)
                {
                    nameStream.Write('[');
                    int rank = *--currentRank;
                    if (--rank != 0) nameStream.Write(rank.toString());
                    nameStream.Write(']');
                }
            }
            /// <summary>
            /// 泛型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void genericFullName(Type type)
            {
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        getFullName(type.GetGenericArguments()[0]);
                        nameStream.Write('?');
                        return;
                    }
                    string name = type.Name;
                    nameStream.Write(type.Namespace);
                    nameStream.Write('.');
                    nameStream.Write(name, 0, name.IndexOf(GenericSplit));
                    genericParameter(type);
                    return;
                }
                list<Type> reflectedTypeList = new list<Type>();
                do
                {
                    reflectedTypeList.Add(reflectedType);
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                Type[] reflectedTypeArray = reflectedTypeList.Unsafer.Array;
                int reflectedTypeIndex = reflectedTypeList.Count - 1;
                reflectedType = reflectedTypeArray[reflectedTypeIndex];
                nameStream.Write(reflectedType.Namespace);
                Type[] parameterTypes = type.GetGenericArguments();
                int parameterIndex = 0;
                do
                {
                    nameStream.Write('.');
                    if (reflectedType.IsGenericType)
                    {
                        string name = reflectedType.Name;
                        int splitIndex = name.IndexOf(GenericSplit);
                        if (splitIndex != -1)
                        {
                            nameStream.Write(name, 0, splitIndex);
                            int parameterCount = reflectedType.GetGenericArguments().Length;
                            genericParameter(parameterTypes, parameterIndex, parameterCount);
                            parameterIndex = parameterCount;
                        }
                        else nameStream.Write(name);
                    }
                    else nameStream.Write(reflectedType.Name);
                    if (reflectedTypeIndex == 0)
                    {
                        reflectedType = type;
                        type = null;
                    }
                    else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
                }
                while (reflectedType != null);
            }
            /// <summary>
            /// 泛型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void genericName(Type type)
            {
                string name = type.Name;
                int splitIndex = name.IndexOf(GenericSplit);
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    nameStream.Write(name, 0, splitIndex);
                    genericParameter(type);
                    return;
                }
                if (splitIndex == -1)
                {
                    nameStream.Write(name);
                    return;
                }
                Type[] parameterTypes = type.GetGenericArguments();
                int parameterIndex = 0;
                do
                {
                    if (reflectedType.IsGenericType)
                    {
                        int parameterCount = reflectedType.GetGenericArguments().Length;
                        if (parameterCount != parameterTypes.Length)
                        {
                            parameterIndex = parameterCount;
                            break;
                        }
                    }
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                nameStream.Write(name, 0, splitIndex);
                genericParameter(parameterTypes, parameterIndex, parameterTypes.Length);
            }
            /// <summary>
            /// 泛型参数处理
            /// </summary>
            /// <param name="type">类型</param>
            private void genericParameter(Type type)
            {
                nameStream.Write('<');
                int index = 0;
                foreach (Type parameter in type.GetGenericArguments())
                {
                    if (index != 0) nameStream.Write(',');
                    getFullName(parameter);
                    ++index;
                }
                nameStream.Write('>');
            }
            /// <summary>
            /// 泛型参数处理
            /// </summary>
            /// <param name="parameterTypes">参数类型集合</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            private void genericParameter(Type[] parameterTypes, int startIndex, int endIndex)
            {
                nameStream.Write('<');
                for (getFullName(parameterTypes[startIndex]); ++startIndex != endIndex; getFullName(parameterTypes[startIndex])) nameStream.Write(',');
                nameStream.Write('>');
            }
            /// <summary>
            /// 嵌套类型处理
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="reflectedType">上层类型</param>
            private void reflectedType(Type type, Type reflectedType)
            {
                list<Type> reflectedTypeList = new list<Type>();
                do
                {
                    reflectedTypeList.Add(reflectedType);
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                Type[] reflectedTypeArray = reflectedTypeList.Unsafer.Array;
                int reflectedTypeIndex = reflectedTypeList.Count - 1;
                reflectedType = reflectedTypeArray[reflectedTypeIndex];
                nameStream.Write(reflectedType.Namespace);
                do
                {
                    nameStream.Write('.');
                    nameStream.Write(reflectedType.Name);
                    if (reflectedTypeIndex == 0)
                    {
                        reflectedType = type;
                        type = null;
                    }
                    else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
                }
                while (reflectedType != null);
            }
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public static string fullName(this Type type)
        {
            if (type == null) return null;
            string value = typeNames.Get(type, null);
            if (value != null) return value;
            if (type.IsGenericParameter) return type.Name;
            return new nameBuilder().GetFullName(type);
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public static string name(this Type type)
        {
            if (type == null) return null;
            string value = typeNames.Get(type, null);
            if (value != null) return value;
            if (type.IsGenericParameter) return type.Name;
            return new nameBuilder().GetName(type);
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public static string onlyName(this Type type)
        {
            string value = typeNames.Get(type, null);
            if (value != null) return value;
            if (type.IsGenericTypeDefinition)
            {
                value = type.Name;
                return value.Substring(0, value.IndexOf(GenericSplit));
            }
            return type.Name;
        }
        /// <summary>
        /// 判断类型是否引用类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否引用类型</returns>
        public static bool isNull(this Type type)
        {
            return type != null && (type.IsClass || type.IsInterface || nullableType(type) != null);
        }
        /// <summary>
        /// 是否值类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否值类型</returns>
        public static bool isStruct(this Type type)
        {
            return type != null && !type.IsClass && !type.IsInterface && !type.IsEnum;
        }
        /// <summary>
        /// 获取可空类型的值类型
        /// </summary>
        /// <param name="type">可空类型</param>
        /// <returns>值类型,失败返回null</returns>
        public static Type nullableType(this Type type)
        {
            if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
                if (!type.IsClass && !type.IsInterface && !type.IsEnum && !type.IsGenericType) return type;
            }
            return null;
        }
        /// <summary>
        /// 值类型转换为可空类型
        /// </summary>
        /// <param name="type">值类型</param>
        /// <returns>可空类型,失败返回null</returns>
        public static Type toNullableType(this Type type)
        {
            if (type != null && !type.IsClass && !type.IsInterface && !type.IsEnum && !type.IsGenericType)
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return null;
        }
        /// <summary>
        /// 判断当前类型是否继承接口
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">接口</param>
        /// <returns>是否继承接口</returns>
        public static bool isInterface(this Type type, Type interfaceType)
        {
            if (type != null && interfaceType != null && interfaceType.IsInterface)
            {
                if (type == interfaceType) return true;
                else
                {
                    foreach (Type nextType in type.GetInterfaces())
                    {
                        if (nextType == interfaceType) return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取泛型接口类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">泛型接口类型定义</param>
        /// <returns>泛型接口类型,失败返回null</returns>
        public static Type getGenericInterface(this Type type, Type interfaceType)
        {
            foreach (Type nextType in getGenericInterfaces(type, interfaceType)) return nextType;
            return null;
        }
        /// <summary>
        /// 获取泛型接口类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">泛型接口类型定义</param>
        /// <returns>泛型接口类型,失败返回null</returns>
        public static IEnumerable<Type> getGenericInterfaces(this Type type, Type interfaceType)
        {
            if (type != null && interfaceType != null && interfaceType.IsInterface)
            {
                if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType) yield return type;
                foreach (Type nextType in type.GetInterfaces())
                {
                    if (nextType.IsGenericType && nextType.GetGenericTypeDefinition() == interfaceType) yield return nextType;
                }
            }
        }
        /// <summary>
        /// 获取自定义属性定义类型
        /// </summary>
        /// <typeparam name="attributeType">自定义属性</typeparam>
        /// <param name="type">类型</param>
        /// <returns>自定义属性定义类型</returns>
        public static keyValue<Type, attributeType> customAttributeType<attributeType>(this Type type)
            where attributeType : Attribute
        {
            attributeType attribute = type.customAttribute<attributeType>();
            while (attribute == null)
            {
                type = type.BaseType;
                if (type == typeof(object)) return new keyValue<Type,attributeType>();
                attribute = type.customAttribute<attributeType>();
            }
            return new keyValue<Type, attributeType>(type, attribute);
        }
        /// <summary>
        /// 获取TryParse函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>TryParse函数,失败返回null</returns>
        public static MethodInfo getTryParse(this Type type)
        {
            if (type != null)
            {
                MethodInfo tryParse = type.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), type.MakeByRefType() }, null);
                if (tryParse != null && tryParse.ReturnType == typeof(bool)) return tryParse;
            }
            return null;
        }
        /// <summary>
        /// 判断类型是否继承关系
        /// </summary>
        /// <param name="type">子类型</param>
        /// <param name="baseType">父类型</param>
        /// <returns>是否继承关系</returns>
        public static bool isInherit(this Type type, Type baseType)
        {
            if (type != null && baseType != null)
            {
                if (baseType.IsClass)
                {
                    if (type.IsClass)
                    {
                        while (type != null)
                        {
                            if (type == baseType) return true;
                            type = type.BaseType;
                        }
                    }
                }
                else if (baseType.IsInterface && type == baseType || type.GetInterfaces().indexOf(baseType) != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取隐式类型转换函数
        /// </summary>
        /// <param name="type">原数据类型</param>
        /// <param name="convertType">目标数据类型</param>
        /// <returns>隐式类型转换函数</returns>
        public static MethodInfo getImplicitMethod(this Type type, Type convertType)
        {
            return getImplicitExplicitMethod(type, convertType, type) ?? getImplicitExplicitMethod(type, convertType, convertType);
        }
         /// <summary>
        /// 获取隐式类型转换函数
        /// </summary>
        /// <param name="type">原数据类型</param>
        /// <param name="convertType">目标数据类型</param>
        /// <param name="methodType">函数搜索类型</param>
        /// <returns>隐式类型转换函数</returns>
        private static MethodInfo getImplicitExplicitMethod(Type type, Type convertType, Type methodType)
        {
            foreach (MethodInfo method in methodType.GetMethods())
            {
                if (method.IsStatic && (method.Name == "op_Implicit" || method.Name == "op_Explicit") && method.ReturnType == convertType)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.length() == 1 && parameters[0].ParameterType == type) return method;
                }
            }
            return null;
        }

        static type()
        {
            #region 初始化 类型名称集合
            keyValue<hashCode<Type>, string>[] types = new keyValue<hashCode<Type>, string>[]
            {
                new keyValue<hashCode<Type>,string>(typeof(bool), "bool"),
                new keyValue<hashCode<Type>,string>(typeof(byte), "byte"),
                new keyValue<hashCode<Type>,string>(typeof(sbyte), "sbyte"),
                new keyValue<hashCode<Type>,string>(typeof(short), "short"),
                new keyValue<hashCode<Type>,string>(typeof(ushort), "ushort"),
                new keyValue<hashCode<Type>,string>(typeof(int), "int"),
                new keyValue<hashCode<Type>,string>(typeof(uint), "uint"),
                new keyValue<hashCode<Type>,string>(typeof(long), "long"),
                new keyValue<hashCode<Type>,string>(typeof(ulong), "ulong"),
                new keyValue<hashCode<Type>,string>(typeof(float), "float"),
                new keyValue<hashCode<Type>,string>(typeof(double), "double"),
                new keyValue<hashCode<Type>,string>(typeof(decimal), "decimal"),
                new keyValue<hashCode<Type>,string>(typeof(char), "char"),
                new keyValue<hashCode<Type>,string>(typeof(string), "string"),
                new keyValue<hashCode<Type>,string>(typeof(object), "object"),
                new keyValue<hashCode<Type>,string>(typeof(void), "void")
            };
            typeNames = new staticDictionary<hashCode<Type>, string>(types);
            #endregion
        }
    }
}
