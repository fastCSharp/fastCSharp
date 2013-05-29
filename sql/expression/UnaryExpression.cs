#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Reflection;
using fastCSharp;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 一元表达式
    /// </summary>
    public class UnaryExpression : Expression
    {
        /// <summary>
        /// 表达式
        /// </summary>
        public Expression Expression { get; private set; }
        /// <summary>
        /// 运算符重载函数
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Expression.IsConstant && Method != null)
                {
                    return new ConstantExpression(Method.Invoke(null, new object[] { ((ConstantExpression)Expression).Value }));
                }
                func<UnaryExpression, Expression> getSimple = getSimpleExpressions[(int)NodeType];
                if (getSimple != null) return getSimple(this);
                return this;
            }
        }
        /// <summary>
        /// 一元表达式
        /// </summary>
        /// <param name="type">表达式类型</param>
        /// <param name="expression">表达式</param>
        /// <param name="method">运算符重载函数</param>
        internal UnaryExpression(ExpressionType type, Expression expression, MethodInfo method)
        {
            NodeType = type;
            Expression = expression.SimpleExpression;
            Method = method;
        }

        /// <summary>
        /// 获取简单表达式
        /// </summary>
        private static readonly func<UnaryExpression, Expression>[] getSimpleExpressions;
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">一元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleNot(UnaryExpression binaryExpression)
        {
            if (binaryExpression.Expression.IsConstant)
            {
                return new ConstantExpression(!(bool)((ConstantExpression)binaryExpression.Expression).Value);
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">一元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleIsTrue(UnaryExpression binaryExpression)
        {
            if (binaryExpression.Expression.IsConstant)
            {
                return new logicConstantExpression((bool)((ConstantExpression)binaryExpression.Expression).Value);
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">一元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleIsFalse(UnaryExpression binaryExpression)
        {
            if (binaryExpression.Expression.IsConstant)
            {
                return new logicConstantExpression(!(bool)((ConstantExpression)binaryExpression.Expression).Value);
            }
            return binaryExpression;
        }

        static UnaryExpression()
        {
            getSimpleExpressions = new func<UnaryExpression, Expression>[fastCSharp.Enum.GetMaxValue<ExpressionType>(-1) + 1];
            getSimpleExpressions[(int)ExpressionType.Not] = getSimpleNot;
            //getSimpleExpressions[(int)expressionType.Negate] = getSimpleNegate;
            //getSimpleExpressions[(int)expressionType.NegateChecked] = getSimpleNegate;
            //getSimpleExpressions[(int)expressionType.UnaryPlus] = getSimpleUnaryPlus;
            getSimpleExpressions[(int)ExpressionType.IsTrue] = getSimpleIsTrue;
            getSimpleExpressions[(int)ExpressionType.IsFalse] = getSimpleIsFalse;
            //getSimpleExpressions[(int)expressionType.Convert] = getSimpleConvert;
            //getSimpleExpressions[(int)expressionType.ConvertChecked] = getSimpleConvert;
        }
    }
}
