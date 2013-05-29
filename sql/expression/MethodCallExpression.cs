#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Collections;
using System.Reflection;
using fastCSharp;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 函数调用表达式
    /// </summary>
    public sealed class MethodCallExpression : Expression
    {
        /// <summary>
        /// 动态函数对象表达式
        /// </summary>
        public Expression Instance { get; private set; }
        /// <summary>
        /// 函数信息
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// 调用参数
        /// </summary>
        public Expression[] Arguments { get; private set; }
        /// <summary>
        /// 简单表达式
        /// </summary>
        public override Expression SimpleExpression
        {
            get
            {
                if (Method.ReflectedType != typeof(fastCSharp.sql.expressionCall))
                {
                    return new ConstantExpression(Method.Invoke(Instance == null ? null : ((ConstantExpression)Instance).Value
                        , Arguments.getArray(argumentExpression => ((ConstantExpression)argumentExpression).Value)));
                }
                if (Method.Name == "In")
                {
                    object values = ((ConstantExpression)Arguments[1]).Value;
                    if (values != null)
                    {
                        int index = 0;
                        object firstValue = null;
                        foreach (object value in (IEnumerable)values)
                        {
                            if (index != 0) return new BinaryExpression(ExpressionType.InSet, Arguments[0], Arguments[1], null);
                            firstValue = value;
                            ++index;
                        }
                        if (index != 0)
                        {
                            return new BinaryExpression(ExpressionType.Equal, Arguments[0], new ConstantExpression(firstValue), null).SimpleExpression;
                        }
                    }
                    return new logicConstantExpression(false);
                }
                return this;
            }
        }
        /// <summary>
        /// 函数调用表达式
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <param name="instance">动态函数对象表达式</param>
        /// <param name="arguments">调用参数</param>
        internal MethodCallExpression(MethodInfo method, Expression instance, Expression[] arguments)
        {
            NodeType = ExpressionType.Call;
            Instance = instance == null ? null : instance.SimpleExpression;
            Method = method;
            Arguments = arguments.getArray(value => value.SimpleExpression);
            IsSimple = true;
        }
    }
}
