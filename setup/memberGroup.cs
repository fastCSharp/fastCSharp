using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 动态成员分组
    /// </summary>
    internal struct memberGroup
    {
        /// <summary>
        /// 公有动态字段
        /// </summary>
        public FieldInfo[] PublicFields;
        /// <summary>
        /// 非公有动态字段
        /// </summary>
        public FieldInfo[] NonPublicFields;
        /// <summary>
        /// 公有动态属性
        /// </summary>
        public PropertyInfo[] PublicProperties;
        /// <summary>
        /// 非公有动态属性
        /// </summary>
        public PropertyInfo[] NonPublicProperties;
        /// <summary>
        /// 动态成员分组
        /// </summary>
        /// <param name="type">目标类型</param>
        public memberGroup(Type type)
        {
            PublicFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            NonPublicFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).getFindArray(value => value.Name[0] != '<');
            PublicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            NonPublicProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
    /// <summary>
    /// 动态成员分组
    /// </summary>
    /// <typeparam name="valueType">目标类型</typeparam>
    internal class memberGroup<valueType>
    {
        /// <summary>
        /// 公有动态字段
        /// </summary>
        private fieldInfo[] publicFields;
        /// <summary>
        /// 非公有动态字段
        /// </summary>
        private fieldInfo[] nonPublicFields;
        /// <summary>
        /// 公有动态属性
        /// </summary>
        private propertyInfo<valueType>[] publicProperties;
        /// <summary>
        /// 非公有动态属性
        /// </summary>
        private propertyInfo<valueType>[] nonPublicProperties;
        /// <summary>
        /// 成员数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 成员集合
        /// </summary>
        public memberInfo[] Members
        {
            get
            {
                list<memberInfo> members = new list<memberInfo>(Count);
                members.Add(publicFields.toGeneric<memberInfo>());
                members.Add(nonPublicFields.toGeneric<memberInfo>());
                members.Add(publicProperties.toGeneric<memberInfo>());
                members.Add(nonPublicProperties.toGeneric<memberInfo>());
                return members.ToArray();
            }
        }
        /// <summary>
        /// SQL成员信息集合
        /// </summary>
        public sqlMember[] SqlMembers
        {
            get
            {
                sqlMember[] values = new sqlMember[MemberCount];
                foreach (fieldInfo value in publicFields) values[value.MemberIndex] = sqlMember.Get(value);
                foreach (fieldInfo value in nonPublicFields) values[value.MemberIndex] =sqlMember.Get(value);
                foreach (propertyInfo<valueType> value in publicProperties) values[value.MemberIndex] = sqlMember.Get(value);
                foreach (propertyInfo<valueType> value in nonPublicProperties) values[value.MemberIndex] = sqlMember.Get(value);
                return values;
            }
        }
        /// <summary>
        /// 动态成员分组
        /// </summary>
        /// <param name="publicFields">公有动态字段</param>
        /// <param name="nonPublicFields">非公有动态字段</param>
        /// <param name="publicProperties">公有动态属性</param>
        /// <param name="nonPublicProperties">非公有动态属性</param>
        /// <param name="filter">成员选择</param>
        private memberGroup(list<fieldInfo> publicFields, list<fieldInfo> nonPublicFields
            , list<propertyInfo<valueType>> publicProperties, list<propertyInfo<valueType>> nonPublicProperties
            , memberFilter filter = memberFilter.Instance)
        {
            this.publicFields = (filter & memberFilter.PublicInstanceField) != 0 ? publicFields.toArray() : nullValue<fieldInfo>.Array;
            this.nonPublicFields = (filter & memberFilter.NonPublicInstanceField) != 0 ? nonPublicFields.toArray() : nullValue<fieldInfo>.Array;
            this.publicProperties = (filter & memberFilter.PublicInstanceProperty) != 0 ? publicProperties.toArray() : nullValue<propertyInfo<valueType>>.Array;
            this.nonPublicProperties = (filter & memberFilter.NonPublicInstanceProperty) != 0 ? nonPublicProperties.toArray() : nullValue<propertyInfo<valueType>>.Array;
            Count = this.publicFields.Length + this.nonPublicFields.Length
                + this.publicProperties.Length + this.nonPublicProperties.Length;
        }
        /// <summary>
        /// 获取成员名称与成员值集合
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>成员名称与成员值集合</returns>
        public list<keyValue<memberInfo, object>> GetMemberValue(valueType value
            , memberFilter filter = memberFilter.Instance, cSharp.memberMap<valueType> memberMap = default(cSharp.memberMap<valueType>))
        {
            return isStruct ? getMemberValueValue(value, filter, memberMap) : getMemberValue(value, filter, memberMap);
        }
        /// <summary>
        /// 引用类型获取成员名称与成员值集合
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>成员名称与成员值集合</returns>
        private list<keyValue<memberInfo, object>> getMemberValue(valueType value, memberFilter filter, cSharp.memberMap<valueType> memberMap)
        {
            int count = 0;
            keyValue<memberInfo, object>[] values = new keyValue<memberInfo, object>[Count];
            object objectValue = value;
            if (memberMap.IsDefault)
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        values[count++].Set(property, property.Getter(value));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        values[count++].Set(property, property.Getter(value));
                    }
                }
            }
            else
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex))
                            values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex))
                            values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                            values[count++].Set(property, property.Getter(value));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                            values[count++].Set(property, property.Getter(value));
                    }
                }
            }
            return new list<keyValue<memberInfo, object>>(values, 0, count, true);
        }
        /// <summary>
        /// 值类型获取成员名称与成员值集合
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>成员名称与成员值集合</returns>
        private list<keyValue<memberInfo, object>> getMemberValueValue(valueType value, memberFilter filter, cSharp.memberMap<valueType> memberMap)
        {
            keyValue<memberInfo, object>[] values = new keyValue<memberInfo, object>[Count];
            object objectValue = value;
            int count = 0;
            if (memberMap.IsDefault)
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        values[count++].Set(property, property.GetValue(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        values[count++].Set(property, property.GetValue(objectValue));
                    }
                }
            }
            else
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex))
                            values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex))
                            values[count++].Set(field, field.Getter(objectValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                            values[count++].Set(property, property.GetValue(objectValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                            values[count++].Set(property, property.GetValue(objectValue));
                    }
                }
            }
            return new list<keyValue<memberInfo, object>>(values, 0, count, true);
        }
        /// <summary>
        /// 引用类型设置成员值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="values">成员值数组</param>
        /// <param name="isValueMap">成员值设置位图</param>
        public void SetMember(valueType value, object[] values, fixedMap isValueMap)
        {
            object objectValue = value;
            foreach (fieldInfo field in publicFields)
            {
                if (isValueMap.Get(field.MemberIndex)) field.Setter(objectValue, values[field.MemberIndex]);
            }
            foreach (fieldInfo field in nonPublicFields)
            {
                if (isValueMap.Get(field.MemberIndex)) field.Setter(objectValue, values[field.MemberIndex]);
            }
            foreach (propertyInfo<valueType> property in publicProperties)
            {
                if (isValueMap.Get(property.MemberIndex)) property.Setter(value, values[property.MemberIndex]);
            }
            foreach (propertyInfo<valueType> property in nonPublicProperties)
            {
                if (isValueMap.Get(property.MemberIndex)) property.Setter(value, values[property.MemberIndex]);
            }
        }
        /// <summary>
        /// 值类型设置成员值
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="values">成员值数组</param>
        /// <param name="isValueMap">成员值设置位图</param>
        /// <returns>目标对象</returns>
        public valueType SetMemberValue(valueType value, object[] values, fixedMap isValueMap)
        {
            object objectValue = value;
            object[] setValues = new object[1];
            foreach (fieldInfo field in publicFields)
            {
                if (isValueMap.Get(field.MemberIndex)) field.Setter(objectValue, values[field.MemberIndex]);
            }
            foreach (fieldInfo field in nonPublicFields)
            {
                if (isValueMap.Get(field.MemberIndex)) field.Setter(objectValue, values[field.MemberIndex]);
            }
            foreach (propertyInfo<valueType> property in publicProperties)
            {
                if (isValueMap.Get(property.MemberIndex))
                {
                    setValues[0] = values[property.MemberIndex];
                    property.SetValue(objectValue, setValues);
                }
            }
            foreach (propertyInfo<valueType> property in nonPublicProperties)
            {
                if (isValueMap.Get(property.MemberIndex))
                {
                    setValues[0] = values[property.MemberIndex];
                    property.SetValue(objectValue, setValues);
                }
            }
            return (valueType)objectValue;
        }
        /// <summary>
        /// 引用类型成员复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="copyValue">被复制对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        public void Copy(valueType value, valueType copyValue, memberFilter filter, cSharp.memberMap<valueType> memberMap)
        {
            object objectValue = value, objectCopyValue = copyValue;
            if (memberMap.IsDefault)
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields) field.Setter(objectValue, field.Getter(objectCopyValue));
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields) field.Setter(objectValue, field.Getter(objectCopyValue));
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties) property.Copyer(value, copyValue);
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties) property.Copyer(value, copyValue);
                }
            }
            else
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex)) field.Setter(objectValue, field.Getter(objectCopyValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex)) field.Setter(objectValue, field.Getter(objectCopyValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex)) property.Copyer(value, copyValue);
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex)) property.Copyer(value, copyValue);
                    }
                }
            }
        }
        /// <summary>
        /// 值类型成员复制
        /// </summary>
        /// <param name="value">目标对象</param>
        /// <param name="copyValue">被复制对象</param>
        /// <param name="filter">成员选择</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>目标对象</returns>
        public valueType CopyValue(valueType value, valueType copyValue, memberFilter filter, cSharp.memberMap<valueType> memberMap)
        {
            if (publicFields.Length + nonPublicFields.Length == allPublicFields.Length + allNonPublicFields.Length
                && (filter & memberFilter.InstanceField) == memberFilter.InstanceField && memberMap.IsDefault) return copyValue;
            object objectValue = value, objectCopyValue = copyValue;
            object[] setValues = new object[1];
            if (memberMap.IsDefault)
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields) field.Setter(objectValue, field.Getter(objectCopyValue));
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields) field.Setter(objectValue, field.Getter(objectCopyValue));
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        setValues[0] = property.GetValue(objectCopyValue);
                        property.SetValue(objectValue, setValues);
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        setValues[0] = property.GetValue(objectCopyValue);
                        property.SetValue(objectValue, setValues);
                    }
                }
            }
            else
            {
                if ((filter & memberFilter.PublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in publicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex)) field.Setter(objectValue, field.Getter(objectCopyValue));
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceField) != 0)
                {
                    foreach (fieldInfo field in nonPublicFields)
                    {
                        if (memberMap.IsMember(field.MemberIndex)) field.Setter(objectValue, field.Getter(objectCopyValue));
                    }
                }
                if ((filter & memberFilter.PublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in publicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                        {
                            setValues[0] = property.GetValue(objectCopyValue);
                            property.SetValue(objectValue, setValues);
                        }
                    }
                }
                if ((filter & memberFilter.NonPublicInstanceProperty) != 0)
                {
                    foreach (propertyInfo<valueType> property in nonPublicProperties)
                    {
                        if (memberMap.IsMember(property.MemberIndex))
                        {
                            setValues[0] = property.GetValue(objectCopyValue);
                            property.SetValue(objectValue, setValues);
                        }
                    }
                }
            }
            return (valueType)objectValue;
        }
        /// <summary>
        /// 是否值类型
        /// </summary>
        private readonly static bool isStruct = typeof(valueType).isStruct();
        /// <summary>
        /// 公有动态字段
        /// </summary>
        private readonly static fieldInfo[] allPublicFields;
        /// <summary>
        /// 非公有动态字段
        /// </summary>
        private readonly static fieldInfo[] allNonPublicFields;
        /// <summary>
        /// 公有动态属性
        /// </summary>
        private readonly static propertyInfo<valueType>[] allPublicProperties;
        /// <summary>
        /// 非公有动态属性
        /// </summary>
        private readonly static propertyInfo<valueType>[] allNonPublicProperties;
        /// <summary>
        /// 所有成员数量
        /// </summary>
        public readonly static int MemberCount;
        /// <summary>
        /// 成员集合
        /// </summary>
        public static memberInfo[] GetAllMembers()
        {
            list<memberInfo> members = new list<memberInfo>(MemberCount);
            members.Add(allPublicFields.toGeneric<memberInfo>());
            members.Add(allNonPublicFields.toGeneric<memberInfo>());
            members.Add(allPublicProperties.toGeneric<memberInfo>());
            members.Add(allNonPublicProperties.toGeneric<memberInfo>());
            return members.ToArray();
        }
        /// <summary>
        /// 成员判定器
        /// </summary>
        /// <param name="value">成员信息</param>
        /// <returns>true</returns>
        private static bool defaultIsValue(memberInfo value)
        {
            return true;
        }
        /// <summary>
        /// 获取动态成员分组
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">是否搜索父类属性</param>
        /// <param name="isInheritAttribute">是否包含继承属性</param>
        /// <param name="isValue">成员判定器</param>
        /// <param name="filter">成员选择</param>
        /// <returns>动态成员分组</returns>
        public static memberGroup<valueType> Create<attributeType>(bool isAttribute, bool isBaseType, bool isInheritAttribute
            , func<memberInfo, bool> isValue, memberFilter filter = memberFilter.Instance)
            where attributeType : fastCSharp.setup.ignore
        {
            if (isValue == null) isValue = defaultIsValue;
            if (isAttribute)
            {
                return new memberGroup<valueType>(allPublicFields.getFind(value => defaultIsValue(value) && value.IsAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allNonPublicFields.getFind(value => defaultIsValue(value) && value.IsAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allPublicProperties.getFind(value => defaultIsValue(value) && value.IsAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allNonPublicProperties.getFind(value => defaultIsValue(value) && value.IsAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , filter);
            }
            else
            {
                return new memberGroup<valueType>(allPublicFields.getFind(value => defaultIsValue(value) && !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allNonPublicFields.getFind(value => defaultIsValue(value) && !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allPublicProperties.getFind(value => defaultIsValue(value) && !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , allNonPublicProperties.getFind(value => defaultIsValue(value) && !value.IsIgnoreAttribute<attributeType>(isBaseType, isInheritAttribute))
                    , filter);
            }
        }
        /// <summary>
        /// 获取动态成员分组
        /// </summary>
        /// <param name="isValue">成员判定器</param>
        /// <returns>动态成员分组</returns>
        public static memberGroup<valueType> Create(func<memberInfo, bool> isValue)
        {
            if (isValue == null) isValue = defaultIsValue;
            return new memberGroup<valueType>(allPublicFields.getFind(value => !value.IsIgnore && defaultIsValue(value))
                    , allNonPublicFields.getFind(value => !value.IsIgnore && defaultIsValue(value))
                    , allPublicProperties.getFind(value => !value.IsIgnore && defaultIsValue(value))
                    , allNonPublicProperties.getFind(value => !value.IsIgnore && defaultIsValue(value)));
        }
        /// <summary>
        /// 获取成员值委托
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <returns>获取成员值委托</returns>
        public static func<valueType, object> GetValue(memberInfo member)
        {
            fieldInfo field = member as fieldInfo;
            if (field != null) return field.GetValue<valueType>;
            propertyInfo<valueType> property = (propertyInfo<valueType>)member;
            return isStruct ? property.GetValue : property.Getter;
        }
        /// <summary>
        /// 获取成员值委托
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="convertType">成员转换类型</param>
        /// <returns>获取成员值委托</returns>
        public static func<valueType, object> GetConvertValue(memberInfo member, Type convertType)
        {
            func<object, object> converter = reflection.converter.Get(member.MemberType.Type, convertType);
            return converter != null ? new converter { GetValue = GetValue(member), Converter = converter }.Get : GetValue(member);
        }
        /// <summary>
        /// 设置成员值委托
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <returns>设置成员值委托</returns>
        public static action<valueType, object> SetValue(memberInfo member)
        {
            fieldInfo field = member as fieldInfo;
            if (field != null) return field.SetValue<valueType>;
            propertyInfo<valueType> property = (propertyInfo<valueType>)member;
            return isStruct ? property.SetValue : property.Setter;
        }
        /// <summary>
        /// 类型转换委托
        /// </summary>
        private struct converter
        {
            /// <summary>
            /// 成员值获取委托
            /// </summary>
            public func<valueType, object> GetValue;
            /// <summary>
            /// 类型转换委托
            /// </summary>
            public func<object, object> Converter;
            /// <summary>
            /// 成员值类型转换委托
            /// </summary>
            /// <param name="value">目标对象</param>
            /// <returns>返回值</returns>
            public object Get(valueType value)
            {
                return Converter(GetValue(value));
            }
        }
        static memberGroup()
        {
            Type type = typeof(valueType);
            memberGroup memberGroup = new memberGroup(type);
            int index = 0;
            if (type.getTypeName() == null)
            {
                allPublicFields = memberGroup.PublicFields.getArray(value => new fieldInfo(value, memberFilter.PublicInstanceField, index++));
                allNonPublicFields = memberGroup.NonPublicFields.getArray(value => new fieldInfo(value, memberFilter.NonPublicInstanceField, index++));
                allPublicProperties = memberGroup.PublicProperties.getArray(value => new propertyInfo<valueType>(value, memberFilter.PublicInstanceProperty, index++, false, isStruct));
                allNonPublicProperties = memberGroup.NonPublicProperties.getArray(value => new propertyInfo<valueType>(value, memberFilter.NonPublicInstanceProperty, index++, true, isStruct));
            }
            else
            {
                allPublicFields = allNonPublicFields = nullValue<fieldInfo>.Array;
                allPublicProperties = allNonPublicProperties = nullValue<propertyInfo<valueType>>.Array;
            }
            MemberCount = index;
        }
    }
}
