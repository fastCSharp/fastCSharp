using System;
using fastCSharp.setup.cSharp;
using fastCSharp.reflection;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// SQL操作缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class sqlTool<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// SQL操作工具
        /// </summary>
        internal sqlTable.sqlToolBase<valueType, memberType> SqlTool { get; private set; }
        /// <summary>
        /// 数据成员位图
        /// </summary>
        protected memberType memberMap;
        /// <summary>
        /// 数据成员位图
        /// </summary>
        public memberType MemberMap
        {
            get { return memberMap; }
        }
        /// <summary>
        /// 获取更新成员位图
        /// </summary>
        /// <param name="memberMap">更新成员位图</param>
        /// <returns>更新成员位图</returns>
        protected memberType updateMemberMap(memberType memberMap)
        {
            memberType newMemberMap = memberMap.Copy();
            newMemberMap.And(memberMap);
            return newMemberMap;
        }
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        protected sqlTool(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap)
        {
            if (sqlTool == null) log.Default.Throw(log.exceptionType.Null);
            SqlTool = sqlTool;
            this.memberMap = sqlTool.MemberMap;
            if (!memberMap.IsDefault) this.memberMap.And(memberMap);
        }
    }
}
