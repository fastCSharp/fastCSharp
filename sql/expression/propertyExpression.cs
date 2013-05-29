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
    /// 属性表达式
    /// </summary>
    public class propertyExpression : MemberExpression
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Expression.IsConstant)
                {
                    return new ConstantExpression(PropertyInfo.GetValue(((ConstantExpression)Expression).Value, null));
                }
                return this;
            }
        }
        /// <summary>
        /// 属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="property">属性信息</param>
        internal propertyExpression(Expression expression, PropertyInfo property)
            : base(expression)
        {
            NodeType = ExpressionType.PropertyAccess;
            PropertyInfo = property;
        }
    }
}