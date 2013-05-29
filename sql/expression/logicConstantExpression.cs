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
    /// 逻辑常量表达式
    /// </summary>
    public class logicConstantExpression : Expression
    {
        /// <summary>
        /// 逻辑值
        /// </summary>
        public bool Value { get; private set; }
        /// <summary>
        /// 运算符表达式
        /// </summary>
        /// <param name="value">逻辑值</param>
        internal logicConstantExpression(bool value)
        {
            NodeType = ExpressionType.LogicConstant;
            Value = value;
            IsSimple = IsConstant = true;
        }
    }
}