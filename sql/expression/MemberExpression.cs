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
    /// 成员表达式
    /// </summary>
    public abstract class MemberExpression : Expression
    {
        /// <summary>
        /// 表达式
        /// </summary>
        public Expression Expression { get; private set; }
        /// <summary>
        /// 成员表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        protected MemberExpression(Expression expression)
        {
            Expression = expression.SimpleExpression;
            IsSimple = true;
            IsConstant = Expression.IsConstant;
        }
    }
}
