using System;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.testCase
{
    /// <summary>
    /// 公共函数
    /// </summary>
    public static class pub
    {
        /// <summary>
        /// 基本比较类型
        /// </summary>
        private static readonly HashSet<Type> equalTypes = new HashSet<Type>(new Type[] { typeof(bool), typeof(bool?), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(double), typeof(decimal), typeof(DateTime), typeof(string) });
        /// <summary>
        /// 测试等于比较
        /// </summary>
        /// <typeparam name="valueType">比较数据类型</typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否等于</returns>
        public static bool Equals<valueType>(valueType left, valueType right)
        {
            if (left != null)
            {
                if (right != null)
                {
                    Type type = typeof(valueType);
                    if (type.IsEnum || equalTypes.Contains(type)) return left.Equals(right);
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (field.FieldType.IsArray)
                        {
                            if (!arrayEquals(field.GetValue(left), field.GetValue(right))) return false;
                        }
                        else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(list<>))
                        {
                            if (!(bool)listEqualsMethod.MakeGenericMethod(field.FieldType.GetGenericArguments()).Invoke(null, new object[] { field.GetValue(left), field.GetValue(right) }))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            object leftValue = field.GetValue(left), rightValue = field.GetValue(right);
                            if (leftValue != null ? !leftValue.Equals(rightValue) : rightValue != null) return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            return right == null;
        }
        /// <summary>
        /// 单向动态数组比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否等于</returns>
        private static bool listEquals<valueType>(object left, object right)
        {
            return arrayEquals((valueType[])(list<valueType>)left, (valueType[])(list<valueType>)right);
        }
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否等于</returns>
        private static bool arrayEquals(object left, object right)
        {
            if (left != null)
            {
                if (right != null)
                {
                    Array leftArray = (Array)left, rightArray = (Array)right;
                    if (leftArray.Length != rightArray.Length) return false;
                    int index = 0;
                    foreach (object leftValue in leftArray)
                    {
                        object rightValue = rightArray.GetValue(index++);
                        if (leftValue != null ? !leftValue.Equals(rightValue) : rightValue != null) return false;
                    }
                    return true;
                }
                return false;
            }
            return right == null;
        }
        /// <summary>
        /// 清空成员
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">待清空对象</param>
        /// <returns>清空成员后的数据</returns>
        public static valueType ClearMember<valueType>(valueType value)
        {
            if (value != null)
            {
                object objectValue = value;
                foreach (FieldInfo field in typeof(valueType).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    field.SetValue(objectValue, defaultValueMethod.MakeGenericMethod(field.FieldType).Invoke(null, null));
                }
                return (valueType)objectValue;
            }
            return value;
        }
        /// <summary>
        /// 默认值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <returns>默认值</returns>
        private static object defaultValue<valueType>()
        {
            return default(valueType);
        }
        /// <summary>
        /// 默认值函数信息
        /// </summary>
        private static readonly MethodInfo defaultValueMethod = typeof(pub).GetMethod("defaultValue", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// list比较函数信息
        /// </summary>
        private static readonly MethodInfo listEqualsMethod = typeof(pub).GetMethod("listEquals", BindingFlags.Static | BindingFlags.NonPublic);
    }
}
