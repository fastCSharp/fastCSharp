#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 参数表达式
    /// </summary>
    public class ParameterExpression : Expression
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type Type { get; private set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 参数表达式
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="name">参数名称</param>
        internal ParameterExpression(Type type, string name)
        {
            NodeType = ExpressionType.Parameter;
            Type = type;
            Name = name;
        }
#if DOTNET35
        /// <summary>
        /// 表达式转换
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>表达式</returns>
        internal static ParameterExpression convert(System.Linq.Expressions.ParameterExpression expression)
        {
            return new ParameterExpression(expression.Type, expression.Name);
        }
#endif
    }
}
