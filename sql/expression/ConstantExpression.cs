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
    /// 常量表达式
    /// </summary>
    public class ConstantExpression : Expression
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// 运算符表达式
        /// </summary>
        /// <param name="value">数据</param>
        internal ConstantExpression(object value)
        {
            NodeType = ExpressionType.Constant;
            Value = value;
            IsSimple = IsConstant = true;
        }
    }
}
