using System;
using System.Collections.Generic;
using fastCSharp.setup.cSharp;
using System.Linq.Expressions;

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// SQL Server2005客户端
    /// </summary>
    public class sql2005 : sql2000
    {
        /// <summary>
        /// 排序名称
        /// </summary>
        private const string orderOverName = "_ROW_";
        /// <summary>
        /// SQL Server2005客户端
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        public sql2005(connection connection) : base(connection) { }
        /// <summary>
        /// 获取表格名称的SQL语句
        /// </summary>
        protected override string GetTableNameSql
        {
            get
            {
                return "select name from sysobjects where objectproperty(id,'IsUserTable')=1";
            }
        }
        /// <summary>
        /// 根据表格名称获取表格信息的SQL语句
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息的SQL语句</returns>
        protected override string GetTableSql(string tableName)
        {
            return @"declare @id int
set @id=object_id(N'[dbo].[" + tableName + @"]')
if(select top 1 id from sysobjects where id=@id and objectproperty(id,N'IsUserTable')=1)is not null begin
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultValue
  ,isnull((select value from ::fn_listextendedproperty(null,'user','dbo','table','" + tableName + @"','column',syscolumns.name)as property where property.name='MS_Description'),'')as remark
  from syscolumns where id=@id order by colid
 if @@rowcount<>0 begin
  select a.indid,a.colid,b.name,(case when b.status=2 then 'UQ' else(select top 1 xtype from sysobjects where name=b.name)end)as type from sysindexkeys a left join sysindexes b on a.id=b.id and a.indid=b.indid where a.id=@id order by a.indid,a.keyno
 end
end";
            //备注
            //"select top 1 value from ::fn_listextendedproperty(null,'user','dbo','table','" + tableName + "','column','" + reader["name"].ToString() + "')as property where property.name='MS_Description'"
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="query">查询信息</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        internal override IEnumerable<valueType> select<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery query, memberType memberMap)
        {
            if (query.SkipCount != 0 && query.Orders.length() != 0)
            {
                if (sqlTool.PrimaryKeyCount == 1)
                {
                    return selectKeys(sqlTool, query, sqlTool.FirstPrimaryKeyName, memberMap);
                }
                if (sqlTool.IdentityMemberIndex != sqlTable.NullIdentityMemberIndex)
                {
                    return selectKeys(sqlTool, query, sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex), memberMap);
                }
                return selectRows(sqlTool, query, memberMap);
            }
            return selectNoOrder(sqlTool, query, memberMap);
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="query">查询信息</param>
        /// <param name="keyName">关键之名称</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        private IEnumerable<valueType> selectKeys<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery query, string keyName, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            string order = query.Order;
            string sql = @"select " + getMemberNames(sqlTool.MemberInfo, memberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + keyName + " in(select " + keyName + " from(select " + keyName + ",row_number()over(" + order + ")as " + orderOverName + " from[" + sqlTool.TableName + "]with(nolock)" + (query.Where != null ? " where " + query.Where + " " : null) + ")as T where " + orderOverName + " between " + query.SkipCount.toString() + " and " + (query.SkipCount + query.GetCount - 1).toString() + ")" + order;
            return select<valueType, memberType>(sql, sqlTool, 0, memberMap);
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="query">查询信息</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        private IEnumerable<valueType> selectRows<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery query, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            string sql = @"select * from(select " + getMemberNames(sqlTool.MemberInfo, memberMap).joinString(',') + ",row_number()over(" + query.Order + ")as " + orderOverName + " from[" + sqlTool.TableName + "]with(nolock)" + (query.Where != null ? "where " + query.Where + " " : null) + ")as T where " + orderOverName + " between " + query.SkipCount.toString() + " and " + (query.SkipCount + query.GetCount - 1).toString();
            return select<valueType, memberType>(sql, sqlTool, 0, memberMap);
        }
    }
}
