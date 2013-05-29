using System;
using System.Linq.Expressions;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql
{
    /// <summary>
    /// 查询信息
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public class selectQuery<valueType>
    {
        /// <summary>
        /// 默认空查询
        /// </summary>
        internal static readonly selectQuery<valueType> NullQuery = new selectQuery<valueType>();
        /// <summary>
        /// 跳过记录数量
        /// </summary>
        public int SkipCount;
        /// <summary>
        /// 获取记录数量,0表示不限
        /// </summary>
        public int GetCount;
        /// <summary>
        /// 查询条件表达式
        /// </summary>
        public Expression<func<valueType, bool>> Where;
        /// <summary>
        /// 排序表达式集合,false为升序,true为降序
        /// </summary>
        public keyValue<Expression<action<valueType>>, bool>[] Orders;
        /// <summary>
        /// 查询条件表达式隐式转换为查询信息
        /// </summary>
        /// <param name="expression">查询条件表达式</param>
        /// <returns>查询信息</returns>
        public static implicit operator selectQuery<valueType>(Expression<func<valueType, bool>> expression)
        {
            return expression == null ? NullQuery : new selectQuery<valueType> { Where = expression };
        }
    }
}
