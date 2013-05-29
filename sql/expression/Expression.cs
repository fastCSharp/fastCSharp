#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Reflection;
using System.Collections.Generic;
using fastCSharp;
using fastCSharp.reflection;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 表达式
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// 表达式类型
        /// </summary>
        public ExpressionType NodeType { get; protected set; }
        /// <summary>
        /// 是否简单表达式
        /// </summary>
        public bool IsSimple { get; protected set; }
        /// <summary>
        /// 是否常量表达式
        /// </summary>
        public bool IsConstant { get; protected set; }
        /// <summary>
        /// 常量值是否为null
        /// </summary>
        public bool IsConstantNull
        {
            get
            {
                return IsConstant && ((ConstantExpression)this).Value == null;
            }
        }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public virtual Expression SimpleExpression
        {
            get { return this; }
        }

        /// <summary>
        /// lambda隐式转换成表达式
        /// </summary>
        /// <typeparam name="DelegateType">委托类型</typeparam>
        /// <param name="body">表达式主体</param>
        /// <param name="parameters">参数</param>
        /// <returns>委托关联表达式</returns>
        public static Expression<DelegateType> Lambda<DelegateType>(Expression body, params ParameterExpression[] parameters)
        {
            return new Expression<DelegateType>(body, parameters);
        }
        /// <summary>
        /// 表达式参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="name">参数名称</param>
        /// <returns>表达式参数</returns>
        public static ParameterExpression Parameter(Type type, string name)
        {
            return new ParameterExpression(type, name);
        }
        /// <summary>
        /// 创建字段表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="field">字段信息</param>
        /// <returns>字段表达式</returns>
        public static MemberExpression Field(Expression expression, FieldInfo field)
        {
            if (field.IsStatic ^ expression == null)
            {
                fastCSharp.log.Default.Throw(fastCSharp.log.exceptionType.ErrorOperation);
            }
            return new fieldExpression(expression, field);
        }
        /// <summary>
        /// 创建字段表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">字段所属类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段表达式</returns>
        public static MemberExpression Field(Expression expression, Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (field == null)
            {
                field = type.GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
            }
            if (field == null) fastCSharp.log.Default.Throw(type.ToString() + " 未找到字段 " + fieldName, true, true);
            return Field(expression, field);
        }
        /// <summary>
        /// 创建属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="property">属性信息</param>
        /// <returns>属性表达式</returns>
        public static MemberExpression Property(Expression expression, PropertyInfo property)
        {
            MethodInfo methodInfo = property.GetGetMethod(true);
            if (methodInfo == null || (methodInfo.IsStatic ^ expression == null))
            {
                fastCSharp.log.Default.Throw(fastCSharp.log.exceptionType.ErrorOperation);
            }
            return new propertyExpression(expression, property);
        }
        /// <summary>
        /// 创建属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">属性所属类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性表达式</returns>
        public static MemberExpression Property(Expression expression, Type type, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (property == null)
            {
                property = type.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            }
            if (property == null) fastCSharp.log.Default.Throw(type.ToString() + " 未找到属性 " + propertyName, true, true);
            return Property(expression, property);
        }
        /// <summary>
        /// 创建属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="propertyAccessor">属性关联函数信息</param>
        /// <returns>属性表达式</returns>
        public static MemberExpression Property(Expression expression, MethodInfo propertyAccessor)
        {
            return Property(expression, getProperty(propertyAccessor));
        }
        /// <summary>
        /// 根据函数信息获取关联属性信息
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <returns>属性信息</returns>
        private static PropertyInfo getProperty(MethodInfo method)
        {
            foreach (PropertyInfo property in method.DeclaringType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | (method.IsStatic ? BindingFlags.Static : BindingFlags.Instance)))
            {
                if (property.CanRead && checkMethod(method, property.GetGetMethod(true))) return property;
            }
            fastCSharp.log.Default.Throw(method.DeclaringType.fullName() + "." + method.Name + " 未找到对应属性", true, true);
            return null;
        }
        /// <summary>
        /// 检测函数信息与属性函数信息是否匹配
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <param name="propertyMethod">属性函数信息</param>
        /// <returns>是否关联属性</returns>
        private static bool checkMethod(MethodInfo method, MethodInfo propertyMethod)
        {
            if (method == propertyMethod) return true;
            Type declaringType = method.DeclaringType;
            return ((declaringType.IsInterface && (method.Name == propertyMethod.Name)) && (declaringType.GetMethod(method.Name) == propertyMethod));
        }
        /// <summary>
        /// 创建二元||表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元||表达式</returns>
        public static BinaryExpression OrElse(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.OrElse, left, right, null);
        }
        /// <summary>
        /// 创建||二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元||表达式</returns>
        public static BinaryExpression OrElse(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.OrElse, left, right, method);
        }
        /// <summary>
        /// 创建二元&&表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元&&表达式</returns>
        public static BinaryExpression AndAlso(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.AndAlso, left, right, null);
        }
        /// <summary>
        /// 创建&&二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元&&表达式</returns>
        public static BinaryExpression AndAlso(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.AndAlso, left, right, method);
        }
        /// <summary>
        /// 创建!一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>!一元表达式</returns>
        public static UnaryExpression Not(Expression expression)
        {
            return new UnaryExpression(ExpressionType.Not, expression, null);
        }
        /// <summary>
        /// 创建!二元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元!表达式</returns>
        public static UnaryExpression Not(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.Not, expression, method);
        }
        /// <summary>
        /// 创建==二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元==表达式</returns>
        public static BinaryExpression Equal(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.Equal, left, right, null);
        }
        /// <summary>
        /// 创建==二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元==表达式</returns>
        public static BinaryExpression Equal(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Equal, left, right, method);
        }
        /// <summary>
        /// 创建!=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元!=表达式</returns>
        public static BinaryExpression NotEqual(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.NotEqual, left, right, null);
        }
        /// <summary>
        /// 创建!=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元!=表达式</returns>
        public static BinaryExpression NotEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.NotEqual, left, right, method);
        }
        /// <summary>
        /// 创建>=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元>=表达式</returns>
        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.GreaterThanOrEqual, left, right, null);
        }
        /// <summary>
        /// 创建>=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元>=表达式</returns>
        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.GreaterThanOrEqual, left, right, method);
        }
        /// <summary>
        /// 创建>二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元>表达式</returns>
        public static BinaryExpression GreaterThan(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.GreaterThan, left, right, null);
        }
        /// <summary>
        /// 创建>二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元>表达式</returns>
        public static BinaryExpression GreaterThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.GreaterThan, left, right, method);
        }
        /// <summary>
        /// 创建<二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元<表达式</returns>
        public static BinaryExpression LessThan(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.LessThan, left, right, null);
        }
        /// <summary>
        /// 创建<二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元<表达式</returns>
        public static BinaryExpression LessThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.LessThan, left, right, method);
        }
        /// <summary>
        /// 创建<=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元<=表达式</returns>
        public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
        {
            return new BinaryExpression(ExpressionType.LessThanOrEqual, left, right, null);
        }
        /// <summary>
        /// 创建<=二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="liftToNull">是否可空类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元<=表达式</returns>
        public static BinaryExpression LessThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.LessThanOrEqual, left, right, method);
        }
        /// <summary>
        /// 创建常量表达式
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>运算符表达式</returns>
        public static ConstantExpression Constant(object value)
        {
            return new ConstantExpression(value);
        }
        /// <summary>
        /// 创建常量表达式
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="type">数据类型</param>
        /// <returns>运算符表达式</returns>
        public static ConstantExpression Constant(object value, Type type)
        {
            return new ConstantExpression(value);
        }
        /// <summary>
        /// 创建拆箱表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">数据类型</param>
        /// <returns>拆箱表达式</returns>
        public static UnaryExpression Unbox(Expression expression, Type type)
        {
            return new UnaryExpression(ExpressionType.Unbox, expression, null);
        }
        /// <summary>
        /// 创建-一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>一元-表达式</returns>
        public static UnaryExpression Negate(Expression expression)
        {
            return Negate(expression, null);
        }
        /// <summary>
        /// 创建-一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元-表达式</returns>
        public static UnaryExpression Negate(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.Negate, expression, method);
        }
        /// <summary>
        /// 创建-一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>一元-表达式</returns>
        public static UnaryExpression NegateChecked(Expression expression)
        {
            return NegateChecked(expression, null);
        }
        /// <summary>
        /// 创建-一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元-表达式</returns>
        public static UnaryExpression NegateChecked(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.NegateChecked, expression, method);
        }
        /// <summary>
        /// 创建+一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>一元+表达式</returns>
        public static UnaryExpression UnaryPlus(Expression expression)
        {
            return UnaryPlus(expression, null);
        }
        /// <summary>
        /// 创建+一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元+表达式</returns>
        public static UnaryExpression UnaryPlus(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.UnaryPlus, expression, method);
        }
        /// <summary>
        /// 创建+二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元+表达式</returns>
        public static BinaryExpression Add(Expression left, Expression right)
        {
            return Add(left, right, null);
        }
        /// <summary>
        /// 创建+二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元+表达式</returns>
        public static BinaryExpression Add(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Add, left, right, method);
        }
        /// <summary>
        /// 创建+二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元+表达式</returns>
        public static BinaryExpression AddChecked(Expression left, Expression right)
        {
            return AddChecked(left, right, null);
        }
        /// <summary>
        /// 创建+二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元+表达式</returns>
        public static BinaryExpression AddChecked(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.AddChecked, left, right, method);
        }
        /// <summary>
        /// 创建-二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元-表达式</returns>
        public static BinaryExpression Subtract(Expression left, Expression right)
        {
            return Subtract(left, right, null);
        }
        /// <summary>
        /// 创建-二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元-表达式</returns>
        public static BinaryExpression Subtract(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Subtract, left, right, method);
        }
        /// <summary>
        /// 创建-二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元-表达式</returns>
        public static BinaryExpression SubtractChecked(Expression left, Expression right)
        {
            return SubtractChecked(left, right, null);
        }
        /// <summary>
        /// 创建-二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元-表达式</returns>
        public static BinaryExpression SubtractChecked(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.SubtractChecked, left, right, method);
        }
        /// <summary>
        /// 创建*二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元*表达式</returns>
        public static BinaryExpression Multiply(Expression left, Expression right)
        {
            return Multiply(left, right, null);
        }
        /// <summary>
        /// 创建*二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元*表达式</returns>
        public static BinaryExpression Multiply(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Multiply, left, right, method);
        }
        /// <summary>
        /// 创建*二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元*表达式</returns>
        public static BinaryExpression MultiplyChecked(Expression left, Expression right)
        {
            return MultiplyChecked(left, right, null);
        }
        /// <summary>
        /// 创建*二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元*表达式</returns>
        public static BinaryExpression MultiplyChecked(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.MultiplyChecked, left, right, method);
        }
        /// <summary>
        /// 创建/二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元/表达式</returns>
        public static BinaryExpression Divide(Expression left, Expression right)
        {
            return Divide(left, right, null);
        }
        /// <summary>
        /// 创建/二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元/表达式</returns>
        public static BinaryExpression Divide(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Divide, left, right, method);
        }
        /// <summary>
        /// 创建%二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元%表达式</returns>
        public static BinaryExpression Modulo(Expression left, Expression right)
        {
            return Modulo(left, right, null);
        }
        /// <summary>
        /// 创建%二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元%表达式</returns>
        public static BinaryExpression Modulo(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Modulo, left, right, method);
        }
        /// <summary>
        /// 创建**二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元**表达式</returns>
        public static BinaryExpression Power(Expression left, Expression right)
        {
            return Power(left, right, null);
        }
        /// <summary>
        /// 创建**二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元**表达式</returns>
        public static BinaryExpression Power(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Power, left, right, method);
        }
        /// <summary>
        /// 创建|二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元|表达式</returns>
        public static BinaryExpression Or(Expression left, Expression right)
        {
            return Or(left, right, null);
        }
        /// <summary>
        /// 创建|二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元|表达式</returns>
        public static BinaryExpression Or(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.Or, left, right, method);
        }
        /// <summary>
        /// 创建&二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元&表达式</returns>
        public static BinaryExpression And(Expression left, Expression right)
        {
            return And(left, right, null);
        }
        /// <summary>
        /// 创建&二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元&表达式</returns>
        public static BinaryExpression And(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.And, left, right, method);
        }
        /// <summary>
        /// 创建^二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元^表达式</returns>
        public static BinaryExpression ExclusiveOr(Expression left, Expression right)
        {
            return ExclusiveOr(left, right, null);
        }
        /// <summary>
        /// 创建^二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元^表达式</returns>
        public static BinaryExpression ExclusiveOr(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.ExclusiveOr, left, right, method);
        }
        /// <summary>
        /// 创建<<二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元<<表达式</returns>
        public static BinaryExpression LeftShift(Expression left, Expression right)
        {
            return LeftShift(left, right, null);
        }
        /// <summary>
        /// 创建<<二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元<<表达式</returns>
        public static BinaryExpression LeftShift(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.LeftShift, left, right, method);
        }
        /// <summary>
        /// 创建>>二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns>二元>>表达式</returns>
        public static BinaryExpression RightShift(Expression left, Expression right)
        {
            return RightShift(left, right, null);
        }
        /// <summary>
        /// 创建>>二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>二元>>表达式</returns>
        public static BinaryExpression RightShift(Expression left, Expression right, MethodInfo method)
        {
            return new BinaryExpression(ExpressionType.RightShift, left, right, method);
        }
        /// <summary>
        /// 创建类型转换一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">目标类型</param>
        /// <returns>类型转换一元表达式</returns>
        public static UnaryExpression Convert(Expression expression, Type type)
        {
            return new ConvertExpression(expression, type, null);
        }
        /// <summary>
        /// 创建类型转换一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">目标类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元类型转换表达式</returns>
        public static UnaryExpression Convert(Expression expression, Type type, MethodInfo method)
        {
            return new ConvertExpression(expression, type, method);
        }
        /// <summary>
        /// 创建类型转换一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">目标类型</param>
        /// <returns>类型转换一元表达式</returns>
        public static UnaryExpression ConvertChecked(Expression expression, Type type)
        {
            return new UnaryExpression(ExpressionType.ConvertChecked, expression, null);
        }
        /// <summary>
        /// 创建类型转换一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">目标类型</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元类型转换表达式</returns>
        public static UnaryExpression ConvertChecked(Expression expression, Type type, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.ConvertChecked, expression, method);
        }
        /// <summary>
        /// 创建条件表达式
        /// </summary>
        /// <param name="test">测试条件</param>
        /// <param name="ifTrue">真表达式</param>
        /// <param name="ifFalse">假表达式</param>
        /// <returns>条件表达式</returns>
        public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
        {
            return new ConditionalExpression(test, ifTrue, ifFalse);
        }
        /// <summary>
        /// 创建条件表达式
        /// </summary>
        /// <param name="test">测试条件</param>
        /// <param name="ifTrue">真表达式</param>
        /// <param name="ifFalse">假表达式</param>
        /// <param name="type">表达式结果类型</param>
        /// <returns>条件表达式</returns>
        public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type)
        {
            return new ConditionalExpression(test, ifTrue, ifFalse);
        }
        /// <summary>
        /// 创建真值判定一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>真值判定一元表达式</returns>
        public static UnaryExpression IsTrue(Expression expression)
        {
            return new UnaryExpression(ExpressionType.IsTrue, expression, null);
        }
        /// <summary>
        /// 创建真值判定一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>一元真值判定表达式</returns>
        public static UnaryExpression IsTrue(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.IsTrue, expression, method);
        }
        /// <summary>
        /// 创建假值判定一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>假值判定一元表达式</returns>
        public static UnaryExpression IsFalse(Expression expression)
        {
            return new UnaryExpression(ExpressionType.IsFalse, expression, null);
        }
        /// <summary>
        /// 创建假值判定一元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        /// <returns>假元真值判定表达式</returns>
        public static UnaryExpression IsFalse(Expression expression, MethodInfo method)
        {
            return new UnaryExpression(ExpressionType.IsFalse, expression, method);
        }
        /// <summary>
        /// 创建函数调用表达式
        /// </summary>
        /// <param name="instance">动态函数对象表达式</param>
        /// <param name="method">函数信息</param>
        /// <returns>函数调用表达式</returns>
        public static MethodCallExpression Call(Expression instance, MethodInfo method)
        {
            return new MethodCallExpression(method, instance, null);
        }
        /// <summary>
        /// 创建函数调用表达式
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <param name="arguments">调用参数</param>
        /// <returns>函数调用表达式</returns>
        public static MethodCallExpression Call(MethodInfo method, params Expression[] arguments)
        {
            return new MethodCallExpression(method, null, arguments);
        }
        /// <summary>
        /// 创建函数调用表达式
        /// </summary>
        /// <param name="instance">动态函数对象表达式</param>
        /// <param name="method">函数信息</param>
        /// <param name="arguments">调用参数</param>
        /// <returns>函数调用表达式</returns>
        public static MethodCallExpression Call(Expression instance, MethodInfo method, params Expression[] arguments)
        {
            return new MethodCallExpression(method, instance, arguments);
        }
        /// <summary>
        /// 创建函数调用表达式
        /// </summary>
        /// <param name="instance">动态函数对象表达式</param>
        /// <param name="method">函数信息</param>
        /// <param name="arguments">调用参数</param>
        /// <returns>函数调用表达式</returns>
        public static MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
        {
            return new MethodCallExpression(method, instance, arguments.getArray());
        }
#if DOTNET35
        /// <summary>
        /// 表达式转换
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>表达式</returns>
        internal static Expression convert(System.Linq.Expressions.Expression expression)
        {
            func<System.Linq.Expressions.Expression, Expression> converter = converters[(int)expressionTypes[(int)expression.NodeType]];
            if (converter == null) fastCSharp.log.Default.Throw("不可识别的表达式类型 : " + expression.NodeType.ToString(), false, true);
            return converter(expression);
        }
        /// <summary>
        /// 表达式类型转换集合
        /// </summary>
        private static ExpressionType[] expressionTypes;
        /// <summary>
        /// 表达式类型集合
        /// </summary>
        private static func<System.Linq.Expressions.Expression, Expression>[] converters;

        static Expression()
        {
            #region 表达式类型转换集合
            expressionTypes = new ExpressionType[Enum.GetMaxValue<System.Linq.Expressions.ExpressionType>(-1) + 1];
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Add] = ExpressionType.Add;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.AddChecked] = ExpressionType.AddChecked;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.And] = ExpressionType.And;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.AndAlso] = ExpressionType.AndAlso;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.ArrayLength] = ExpressionType.ArrayLength;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.ArrayIndex] = ExpressionType.ArrayIndex;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Call] = ExpressionType.Call;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Coalesce] = ExpressionType.Coalesce;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Conditional] = ExpressionType.Conditional;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Constant] = ExpressionType.Constant;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Convert] = ExpressionType.Convert;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.ConvertChecked] = ExpressionType.ConvertChecked;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Divide] = ExpressionType.Divide;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Equal] = ExpressionType.Equal;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.ExclusiveOr] = ExpressionType.ExclusiveOr;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.GreaterThan] = ExpressionType.GreaterThan;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.GreaterThanOrEqual] = ExpressionType.GreaterThanOrEqual;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Invoke] = ExpressionType.Invoke;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Lambda] = ExpressionType.Lambda;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.LeftShift] = ExpressionType.LeftShift;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.LessThan] = ExpressionType.LessThan;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.LessThanOrEqual] = ExpressionType.LessThanOrEqual;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.ListInit] = ExpressionType.ListInit;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.MemberAccess] = ExpressionType.MemberAccess;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.MemberInit] = ExpressionType.MemberInit;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Modulo] = ExpressionType.Modulo;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Multiply] = ExpressionType.Multiply;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.MultiplyChecked] = ExpressionType.MultiplyChecked;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Negate] = ExpressionType.Negate;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.UnaryPlus] = ExpressionType.UnaryPlus;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.NegateChecked] = ExpressionType.NegateChecked;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.New] = ExpressionType.New;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.NewArrayInit] = ExpressionType.NewArrayInit;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.NewArrayBounds] = ExpressionType.NewArrayBounds;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Not] = ExpressionType.Not;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.NotEqual] = ExpressionType.NotEqual;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Or] = ExpressionType.Or;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.OrElse] = ExpressionType.OrElse;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Parameter] = ExpressionType.Parameter;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Power] = ExpressionType.Power;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Quote] = ExpressionType.Quote;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.RightShift] = ExpressionType.RightShift;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.Subtract] = ExpressionType.Subtract;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.SubtractChecked] = ExpressionType.SubtractChecked;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.TypeAs] = ExpressionType.TypeAs;
            expressionTypes[(int)System.Linq.Expressions.ExpressionType.TypeIs] = ExpressionType.TypeIs;
            #endregion

            #region 表达式类型集合
            converters = new func<System.Linq.Expressions.Expression, Expression>[Enum.GetMaxValue<ExpressionType>(-1) + 1];
            converters[(int)ExpressionType.Add] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Add(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Add(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.AddChecked] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return AddChecked(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return AddChecked(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.And] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return And(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return And(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.AndAlso] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return AndAlso(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return AndAlso(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Call] = expression =>
            {
                System.Linq.Expressions.MethodCallExpression methodCallExpression = (System.Linq.Expressions.MethodCallExpression)expression;
                if (methodCallExpression.Object == null)
                {
                    return Call(methodCallExpression.Method, methodCallExpression.Arguments.getArray(value => convert(value)));
                }
                if (methodCallExpression.Arguments.count() == 0)
                {
                    return Call(convert(methodCallExpression.Object), methodCallExpression.Method);
                }
                return Call(convert(methodCallExpression.Object), methodCallExpression.Method, methodCallExpression.Arguments.getArray(value => convert(value)));
            };
            converters[(int)ExpressionType.Conditional] = expression =>
            {
                System.Linq.Expressions.ConditionalExpression conditionalExpression = (System.Linq.Expressions.ConditionalExpression)expression;
                if (conditionalExpression.Type == null)
                {
                    return Condition(convert(conditionalExpression.Test), convert(conditionalExpression.IfTrue), convert(conditionalExpression.IfFalse));
                }
                return Condition(convert(conditionalExpression.Test), convert(conditionalExpression.IfTrue), convert(conditionalExpression.IfFalse), conditionalExpression.Type);
            };
            converters[(int)ExpressionType.Constant] = expression =>
            {
                System.Linq.Expressions.ConstantExpression constantExpression = (System.Linq.Expressions.ConstantExpression)expression;
                if (constantExpression.Type == null)
                {
                    return Constant(constantExpression.Value);
                }
                return Constant(constantExpression.Value, constantExpression.Type);
            };
            converters[(int)ExpressionType.Convert] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return Convert(convert(unaryExpression.Operand), unaryExpression.Type);
                }
                return Convert(convert(unaryExpression.Operand), unaryExpression.Type, unaryExpression.Method);
            };
            converters[(int)ExpressionType.ConvertChecked] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return ConvertChecked(convert(unaryExpression.Operand), unaryExpression.Type);
                }
                return ConvertChecked(convert(unaryExpression.Operand), unaryExpression.Type, unaryExpression.Method);
            };
            converters[(int)ExpressionType.Divide] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Divide(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Divide(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Equal] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Equal(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Equal(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.ExclusiveOr] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return ExclusiveOr(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return ExclusiveOr(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.GreaterThan] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return GreaterThan(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return GreaterThan(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.GreaterThanOrEqual] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return GreaterThanOrEqual(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return GreaterThanOrEqual(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.Lambda] = expression =>
            {
                System.Linq.Expressions.LambdaExpression lambdaExpression = (System.Linq.Expressions.LambdaExpression)expression;
                return new LambdaExpression(convert(lambdaExpression.Body), lambdaExpression.Parameters.getArray(value => ParameterExpression.convert(value)));
            };
            converters[(int)ExpressionType.LeftShift] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return LeftShift(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return LeftShift(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.LessThan] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return LessThan(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return LessThan(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.LessThanOrEqual] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return LessThanOrEqual(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return LessThanOrEqual(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.MemberAccess] = expression =>
            {
                System.Linq.Expressions.MemberExpression memberExpression = (System.Linq.Expressions.MemberExpression)expression;
                FieldInfo field = memberExpression.Member as FieldInfo;
                if (field != null)
                {
                    return Field(convert(memberExpression.Expression), field);
                }
                return Property(convert(memberExpression.Expression), (PropertyInfo)memberExpression.Member);
            };
            converters[(int)ExpressionType.Modulo] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Modulo(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Modulo(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Multiply] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Multiply(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Multiply(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.MultiplyChecked] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return MultiplyChecked(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return MultiplyChecked(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Negate] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return Negate(convert(unaryExpression.Operand));
                }
                return Negate(convert(unaryExpression.Operand), unaryExpression.Method);
            };
            converters[(int)ExpressionType.UnaryPlus] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return UnaryPlus(convert(unaryExpression.Operand));
                }
                return UnaryPlus(convert(unaryExpression.Operand), unaryExpression.Method);
            };
            converters[(int)ExpressionType.NegateChecked] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return NegateChecked(convert(unaryExpression.Operand));
                }
                return NegateChecked(convert(unaryExpression.Operand), unaryExpression.Method);
            };
            converters[(int)ExpressionType.Not] = expression =>
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)expression;
                if (unaryExpression.Method == null)
                {
                    return Not(convert(unaryExpression.Operand));
                }
                return Not(convert(unaryExpression.Operand), unaryExpression.Method);
            };
            converters[(int)ExpressionType.NotEqual] =  expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return NotEqual(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return NotEqual(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
            };
            converters[(int)ExpressionType.Or] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Or(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Or(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.OrElse] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return OrElse(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return OrElse(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Parameter] = expression =>
            {
                System.Linq.Expressions.ParameterExpression parameterExpression = (System.Linq.Expressions.ParameterExpression)expression;
                return Parameter(parameterExpression.Type, parameterExpression.Name);
            };
            converters[(int)ExpressionType.Power] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Power(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Power(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.RightShift] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return RightShift(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return RightShift(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.Subtract] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return Subtract(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return Subtract(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            converters[(int)ExpressionType.SubtractChecked] = expression =>
            {
                System.Linq.Expressions.BinaryExpression binaryExpression = (System.Linq.Expressions.BinaryExpression)expression;
                if (binaryExpression.Method == null)
                {
                    return SubtractChecked(convert(binaryExpression.Left), convert(binaryExpression.Right));
                }
                return SubtractChecked(convert(binaryExpression.Left), convert(binaryExpression.Right), binaryExpression.Method);
            };
            #endregion
        }
#endif
    }
    /// <summary>
    /// 委托关联表达式
    /// </summary>
    /// <typeparam name="DelegateType">委托类型</typeparam>
    public sealed class Expression<DelegateType> : LambdaExpression
    {
        /// <summary>
        /// 委托关联表达式
        /// </summary>
        /// <param name="body">表达式主体</param>
        /// <param name="name">名称</param>
        /// <param name="tailCall">是否尾调用</param>
        /// <param name="parameters">参数</param>
        internal Expression(Expression body, ParameterExpression[] parameters) : base(body, parameters) { }
    }
}
