#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using fastCSharp;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// lambda表达式
    /// </summary>
    public class LambdaExpression : Expression
    {
        /// <summary>
        /// 表达式主体
        /// </summary>
        public Expression Body { get; private set; }
        /// <summary>
        /// 参数
        /// </summary>
        public ParameterExpression[] Parameters { get; private set; }
        /// <summary>
        /// 是否逻辑常量表达式
        /// </summary>
        public bool IsLogicConstantExpression
        {
            get { return Body.NodeType == ExpressionType.LogicConstant; }
        }
        /// <summary>
        /// 逻辑常量值
        /// </summary>
        public bool LogicConstantValue
        {
            get { return ((logicConstantExpression)Body).Value; }
        }
        /// <summary>
        /// 委托关联表达式
        /// </summary>
        /// <param name="body">表达式主体</param>
        /// <param name="name">名称</param>
        /// <param name="tailCall">是否尾调用</param>
        /// <param name="parameters">参数</param>
        internal LambdaExpression(Expression body, ParameterExpression[] parameters)
        {
            NodeType = ExpressionType.Lambda;
            Body = body.SimpleExpression;
            this.Parameters = parameters;
        }
        ///// <summary>
        ///// 根据参数名称获取参数表达式
        ///// </summary>
        ///// <param name="name">参数名称</param>
        ///// <returns>参数表达式</returns>
        //public ParameterExpression GetParameter(string name)
        //{
        //    if (Parameters != null)
        //    {
        //        foreach (ParameterExpression parameter in Parameters)
        //        {
        //            if (parameter.Name == name) return parameter;
        //        }
        //    }
        //    return null;
        //}
#if DOTNET35
        /// <summary>
        /// 表达式转换
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>表达式</returns>
        internal static LambdaExpression convert(System.Linq.Expressions.LambdaExpression expression)
        {
            return new LambdaExpression(Expression.convert(expression.Body)
                , expression.Parameters.getArray(value => ParameterExpression.convert(value)));
        }
#endif
    }
}
