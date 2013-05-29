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
    /// 条件表达式
    /// </summary>
    public class ConditionalExpression : Expression
    {
        /// <summary>
        /// 测试条件
        /// </summary>
        public Expression Test { get; private set; }
        /// <summary>
        /// 真表达式
        /// </summary>
        public Expression IfTrue { get; private set; }
        /// <summary>
        /// 假表达式
        /// </summary>
        public Expression IfFalse { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Test.IsConstant)
                {
                    object value = ((ConstantExpression)Test).Value;
                    return value != null && (bool)value ? IfTrue : IfFalse;
                }
                return this;
            }
        }
        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <param name="test">测试条件</param>
        /// <param name="ifTrue">真表达式</param>
        /// <param name="ifFalse">假表达式</param>
        internal ConditionalExpression(Expression test, Expression ifTrue, Expression ifFalse)
        {
            NodeType = ExpressionType.Conditional;
            Test = test.SimpleExpression;
            IfTrue = ifTrue.SimpleExpression;
            IfFalse = ifFalse.SimpleExpression;
        }
    }
}
