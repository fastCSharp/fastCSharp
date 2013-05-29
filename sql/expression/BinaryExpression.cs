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
    /// 二元表达式
    /// </summary>
    public class BinaryExpression : Expression
    {
        /// <summary>
        /// 左表达式
        /// </summary>
        public Expression Left { get; private set; }
        /// <summary>
        /// 右表达式
        /// </summary>
        public Expression Right { get; private set; }
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
                if (Left.IsConstant && Right.IsConstant && Method != null)
                {
                    return new ConstantExpression(Method.Invoke(null, new object[] { ((ConstantExpression)Left).Value, ((ConstantExpression)Right).Value }));
                }
                func<BinaryExpression, Expression> getSimpleExpression = getSimpleExpressions[(int)NodeType];
                if (getSimpleExpression != null) return getSimpleExpression(this);
                return this;
            }
        }
        /// <summary>
        /// 二元表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">运算符重载函数</param>
        internal BinaryExpression(ExpressionType type, Expression left, Expression right, MethodInfo method)
        {
            NodeType = type;
            Left = left.SimpleExpression;
            Right = right.SimpleExpression;
            Method = method;
        }

        /// <summary>
        /// 获取简单表达式
        /// </summary>
        private static readonly func<BinaryExpression, Expression>[] getSimpleExpressions;
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleOrElse(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.NodeType == ExpressionType.LogicConstant)
            {
                if (((logicConstantExpression)binaryExpression.Left).Value) return binaryExpression.Left;
                return binaryExpression.Right;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LogicConstant)
            {
                if (((logicConstantExpression)binaryExpression.Right).Value) return binaryExpression.Right;
                return binaryExpression.Left;
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleAndAlso(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.NodeType == ExpressionType.LogicConstant)
            {
                if (!((logicConstantExpression)binaryExpression.Left).Value) return binaryExpression.Left;
                return binaryExpression.Right;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LogicConstant)
            {
                if (!((logicConstantExpression)binaryExpression.Right).Value) return binaryExpression.Right;
                return binaryExpression.Left;
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleEqual(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                object leftValue = ((ConstantExpression)binaryExpression.Left).Value;
                object rightValue = ((ConstantExpression)binaryExpression.Right).Value;
                return new logicConstantExpression(leftValue == null ? rightValue == null : leftValue.Equals(rightValue));
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleNotEqual(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                object leftValue = ((ConstantExpression)binaryExpression.Left).Value;
                object rightValue = ((ConstantExpression)binaryExpression.Right).Value;
                return new logicConstantExpression(leftValue == null ? rightValue != null : !leftValue.Equals(rightValue));
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleGreaterThanOrEqual(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                return new logicConstantExpression(((IComparable)((ConstantExpression)binaryExpression.Left).Value)
                    .CompareTo(((ConstantExpression)binaryExpression.Right).Value) >= 0);
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleGreaterThan(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                return new logicConstantExpression(((IComparable)((ConstantExpression)binaryExpression.Left).Value)
                    .CompareTo(((ConstantExpression)binaryExpression.Right).Value) > 0);
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleLessThan(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                return new logicConstantExpression(((IComparable)((ConstantExpression)binaryExpression.Left).Value)
                    .CompareTo(((ConstantExpression)binaryExpression.Right).Value) < 0);
            }
            return binaryExpression;
        }
        /// <summary>
        /// 获取简单表达式
        /// </summary>
        /// <param name="binaryExpression">二元表达式</param>
        /// <returns>简单表达式</returns>
        private static Expression getSimpleLessThanOrEqual(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.IsConstant && binaryExpression.Right.IsConstant)
            {
                return new logicConstantExpression(((IComparable)((ConstantExpression)binaryExpression.Left).Value)
                    .CompareTo(((ConstantExpression)binaryExpression.Right).Value) <= 0);
            }
            return binaryExpression;
        }

        static BinaryExpression()
        {
            getSimpleExpressions = new func<BinaryExpression, Expression>[fastCSharp.Enum.GetMaxValue<ExpressionType>(-1) + 1];
            getSimpleExpressions[(int)ExpressionType.OrElse] = getSimpleOrElse;
            getSimpleExpressions[(int)ExpressionType.AndAlso] = getSimpleAndAlso;
            getSimpleExpressions[(int)ExpressionType.Equal] = getSimpleEqual;
            getSimpleExpressions[(int)ExpressionType.NotEqual] = getSimpleNotEqual;
            getSimpleExpressions[(int)ExpressionType.GreaterThanOrEqual] = getSimpleGreaterThanOrEqual;
            getSimpleExpressions[(int)ExpressionType.GreaterThan] = getSimpleGreaterThan;
            getSimpleExpressions[(int)ExpressionType.LessThan] = getSimpleLessThan;
            getSimpleExpressions[(int)ExpressionType.LessThanOrEqual] = getSimpleLessThanOrEqual;
            //getSimpleExpressions[(int)expressionType.Add] = getSimpleAdd;
            //getSimpleExpressions[(int)expressionType.AddChecked] = getSimpleAdd;
            //getSimpleExpressions[(int)expressionType.Subtract] = getSimpleSubtract;
            //getSimpleExpressions[(int)expressionType.SubtractChecked] = getSimpleSubtract;
            //getSimpleExpressions[(int)expressionType.Multiply] = getSimpleMultiply;
            //getSimpleExpressions[(int)expressionType.MultiplyChecked] = getSimpleMultiply;
            //getSimpleExpressions[(int)expressionType.Divide] = getSimpleDivide;
            //getSimpleExpressions[(int)expressionType.Modulo] = getSimpleModulo;
            //getSimpleExpressions[(int)expressionType.Power] = getSimplePower;
            //getSimpleExpressions[(int)expressionType.Or] = getSimpleOr;
            //getSimpleExpressions[(int)expressionType.And] = getSimpleAnd;
            //getSimpleExpressions[(int)expressionType.ExclusiveOr] = getSimpleExclusiveOr;
            //getSimpleExpressions[(int)expressionType.LeftShift] = getSimpleLeftShift;
            //getSimpleExpressions[(int)expressionType.RightShift] = getSimpleRightShift;
        }
    }
}
