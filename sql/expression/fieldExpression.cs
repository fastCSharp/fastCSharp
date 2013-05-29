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
    /// 字段表达式
    /// </summary>
    public class fieldExpression : MemberExpression
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        public FieldInfo FieldInfo { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Expression.IsConstant)
                {
                    return new ConstantExpression(FieldInfo.GetValue(((ConstantExpression)Expression).Value));
                }
                return this;
            }
        }
        /// <summary>
        /// 字段表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="field">字段信息</param>
        internal fieldExpression(Expression expression, FieldInfo field) : base(expression)
        {
            NodeType = ExpressionType.FieldAccess;
            FieldInfo = field;
        }
    }
}
