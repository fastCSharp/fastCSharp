#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Reflection;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 类型转换表达式
    /// </summary>
    public sealed class ConvertExpression : UnaryExpression
    {
        /// <summary>
        /// 转换目标类型
        /// </summary>
        public Type ConvertType { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Expression.IsConstant)
                {
                    return new ConstantExpression((Method ?? convertMethod.MakeGenericMethod(ConvertType)).Invoke(null, new object[] { ((ConstantExpression)Expression).Value }));
                }
                return this;
            }
        }
        /// <summary>
        /// 类型转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="convertType">转换目标类型</param>
        /// <param name="method">运算符重载函数</param>
        internal ConvertExpression(Expression expression, Type convertType, MethodInfo method)
            : base(ExpressionType.Convert, expression, method)
        {
            ConvertType = convertType;
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="value">被转换的数据</param>
        /// <returns>转换后的数据</returns>
        private static valueType convert<valueType>(object value)
        {
            return (valueType)value;
        }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        private static readonly MethodInfo convertMethod = typeof(ConvertExpression).GetMethod("convert", BindingFlags.Static | BindingFlags.NonPublic);
    }
}