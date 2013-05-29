#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;
#if DOTNET35
using fastCSharp.sql.expression;
#else
using System.Linq.Expressions;
#endif

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// 委托关联表达式转SQL表达式
    /// </summary>
    internal class converter : sql.expression.converter
    {
        /// <summary>
        /// SQL表达式
        /// </summary>
        private string sql
        {
            get
            {
                using (stream = new charStream())
                {
                    expressionConverter.Default[expression.Body.NodeType](this, expression.Body);
                    return stream.ToString();
                }
            }
        }
        /// <summary>
        /// 转换器
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="nameType">获取参数名称方式</param>
        private converter(LambdaExpression expression, getNameType nameType) : base(expression, nameType) { }
        /// <summary>
        /// 转换结果缓存
        /// </summary>
        private static readonly Dictionary<Expression, nameCache> expressionCache = new Dictionary<Expression, nameCache>();
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="nameType">获取参数名称方式</param>
        /// <returns>SQL表达式,参数成员名称集合</returns>
        public static keyValue<string, list<string>> Convert(LambdaExpression expression, bool isCache, getNameType nameType)
        {
            nameCache cache;
            if (isCache && expressionCache.TryGetValue(expression, out cache))
            {
                string sql = cache[nameType];
                if (sql != null) return new keyValue<string, list<string>>(sql, cache.ParameterMemberNames);
                converter converter = new converter(expression, nameType);
                cache[nameType] = sql = converter.sql;
                cache.ParameterMemberNames = converter.parameterMemberNames;
                return new keyValue<string, list<string>>(sql, cache.ParameterMemberNames);
            }
            else
            {
                converter converter = new converter(expression, nameType);
                string sql = converter.sql;
                if (isCache)
                {
                    expressionCache[expression] = cache = new nameCache();
                    cache.ParameterMemberNames = converter.parameterMemberNames;
                    cache[nameType] = sql;
                }
                return new keyValue<string, list<string>>(sql, converter.parameterMemberNames);
            }
        }
    }
}
