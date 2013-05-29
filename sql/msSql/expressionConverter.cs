#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Collections;
#if DOTNET35
using fastCSharp.sql.expression;
#else
using System.Linq.Expressions;
#endif

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// 表达式转换
    /// </summary>
    internal class expressionConverter : fastCSharp.sql.expression.expressionConverter
    {
         /// <summary>
        /// 表达式转换
        /// </summary>
        private expressionConverter()
        {
            converters[(int)ExpressionType.Constant] = convertConstant;
            converters[(int)ExpressionType.InSet] = convertInSet;
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="converter">表达式转换器</param>
        /// <param name="expression">表达式</param>
        private void convertConstant(sql.expression.converter converter, Expression expression)
        {
            object value = ((ConstantExpression)expression).Value;
            if (value != null)
            {
                func<object, string> toString = constantConverter.Default[value.GetType()];
                if (toString != null) converter.Stream.Write(toString(value));
                else converter.Stream.Write(constantConverter.convertConstantStringMssql(value.ToString()));
            }
            else converter.Stream.Write("null");
        }

        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="converter">表达式转换器</param>
        /// <param name="expression">表达式</param>
        private void convertInSet(sql.expression.converter converter, Expression expression)
        {
            charStream stream = converter.Stream;
            BinaryExpression binaryExpression = (BinaryExpression)expression;
            Expression left = binaryExpression.Left;
            converters[(int)left.NodeType](converter, left);
            stream.Write(" In(");
            func<object, string> toString = null;
            int index = -1;
            foreach (object value in (IEnumerable)((ConstantExpression)binaryExpression.Right).Value)
            {
                if (++index == 0) toString = constantConverter.Default[value.GetType()];
                else stream.Write(',');
                stream.Write(toString != null ? toString(value) : constantConverter.convertConstantStringMssql(value.ToString()));
            }
            stream.Write(')');
        }
        /// <summary>
        /// 表达式转换
        /// </summary>
        internal static readonly expressionConverter Default = new expressionConverter();
    }
}
