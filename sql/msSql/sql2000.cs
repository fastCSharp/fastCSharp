#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq.Expressions;
using fastCSharp.setup;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// SQL Server2000客户端
    /// </summary>
    public class sql2000 : client
    {
        ///// <summary>
        ///// 最大参数数量
        ///// </summary>
        //public const int MaxParameterCount = 2100 - 3;
        /// <summary>
        /// SQL Server2000客户端
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        public sql2000(connection connection) : base(connection) { }
        /// <summary>
        /// 根据SQL连接类型获取SQL连接
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        /// <param name="isAsynchronous">是否异步连接</param>
        /// <returns>SQL连接</returns>
        protected override DbConnection getConnection(bool isAsynchronous = false)
        {
            return new SqlConnection(isAsynchronous ? this.connection.Connection + ";Asynchronous Processing=true" : this.connection.Connection);
        }
        /// <summary>
        /// 获取SQL命令
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>SQL命令</returns>
        public override DbCommand GetCommand
            (DbConnection connection, string sql, SqlParameter[] parameters = null, CommandType type = CommandType.Text)
        {
            DbCommand command = new SqlCommand(sql, (SqlConnection)connection);
            command.CommandType = type;
            if (parameters != null) command.Parameters.AddRange(parameters);
            return command;
        }
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据适配器</returns>
        protected override DbDataAdapter getAdapter(DbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }
        /// <summary>
        /// 是否支持DataTable导入
        /// </summary>
        protected override bool isImport
        {
            get { return true; }
        }
        /// <summary>
        /// 导入数据集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="data">数据集合</param>
        /// <param name="batchSize">批处理数量</param>
        /// <param name="timeout">超时秒数</param>
        protected override void import(DbConnection connection, DataTable data, int batchSize, int timeout)
        {
            using (SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)connection))
            {
                if (timeout != 0) copy.BulkCopyTimeout = timeout;
                if (batchSize != 0) copy.BatchSize = batchSize;
                copy.DestinationTableName = data.TableName;
                foreach (DataColumn column in data.Columns) copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                copy.WriteToServer(data);
            }
        }
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格是否存在</returns>
        protected override bool isTable(DbConnection connection, string tableName)
        {
            string sql = "select top 1 id from dbo.sysobjects where id=object_id(N'[" + this.connection.Owner + "].[" + tableName + "]')and objectproperty(id,N'IsUserTable')=1";
            using (DbCommand command = GetCommand(connection, sql)) return GetValue<int>(command, 0) != 0;
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="table">表格信息</param>
        protected override bool createTable(DbConnection connection, table table)
        {
            string tableName = table.Columns.Name;
            charStream sqlStream = new charStream();
            sqlStream.Write("create table[");
            sqlStream.Write(this.connection.Owner);
            sqlStream.Write("].[");
            sqlStream.Write(tableName);
            sqlStream.Write("](");
            bool isTextImage = false, isNext = false;
            foreach (column column in table.Columns.Columns)
            {
                if (isNext) sqlStream.Write(',');
                appendColumn(sqlStream, column, column == table.Identity);
                if (!isTextImage) isTextImage = column.DbType.isTextImageType();
                isNext = true;
            }
            columnCollection primaryKey = table.PrimaryKey;
            if (primaryKey != null && primaryKey.Columns.length() != 0)
            {
                isNext = false;
                sqlStream.Write(",primary key(");
                foreach (column column in primaryKey.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    sqlStream.Write(column.Name);
                    isNext = true;
                }
                sqlStream.Write(')');
            }
            sqlStream.Write(")on[primary]");
            if (isTextImage) sqlStream.Write(" textimage_on[primary]");
            foreach (column column in table.Columns.Columns)
            {
                if (column.Remark.length() != 0)
                {
                    sqlStream.Write(@"
exec dbo.sp_addextendedproperty @name=N'MS_Description',@value=N");
                    sqlStream.Write(constantConverter.Default[typeof(string)](column.Remark));
                    sqlStream.Write(",@level0type=N'USER',@level0name=N'");
                    sqlStream.Write(this.connection.Owner);
                    sqlStream.Write("',@level1type=N'TABLE',@level1name=N'");
                    sqlStream.Write(tableName);
                    sqlStream.Write("', @level2type=N'COLUMN',@level2name=N'");
                    sqlStream.Write(column.Name);
                    sqlStream.Write('\'');
                }
            }
            if (table.Indexs != null)
            {
                foreach (columnCollection columns in table.Indexs)
                {
                    if (columns != null && columns.Columns.length() != 0)
                    {
                        sqlStream.Write(@"
create");
                        if (columns.Type == columnCollection.type.UniqueIndex) sqlStream.Write(" unique");
                        sqlStream.Write(" index[");
                        appendIndexName(sqlStream, tableName, columns);
                        sqlStream.Write("]on[");
                        sqlStream.Write(this.connection.Owner);
                        sqlStream.Write("].[");
                        sqlStream.Write(tableName);
                        sqlStream.Write("](");
                        isNext = false;
                        foreach (column column in columns.Columns)
                        {
                            if (isNext) sqlStream.Write(',');
                            sqlStream.Write('[');
                            sqlStream.Write(column.Name);
                            sqlStream.Write(']');
                            isNext = true;
                        }
                        sqlStream.Write(")on[primary]");
                    }
                }
            }
            return executeNonQuery(connection, sqlStream.ToString()) != ExecuteNonQueryError;
        }
        /// <summary>
        /// 最大字符串长度
        /// </summary>
        private const int maxStringSize = 4000;
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="sqlMember">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal override column GetColumn(setup.memberInfo member, sqlMember sqlMember)
        {
            SqlDbType sqlType = SqlDbType.NVarChar;
            int size = maxStringSize;
            memberType memberType = sqlMember.SqlType != null ? (memberType)sqlMember.SqlType : member.MemberType;
            if (memberType.IsString)
            {
                if (sqlMember.MaxLength > 0 && sqlMember.MaxLength <= maxStringSize)
                {
                    if (sqlMember.IsFixedLength) sqlType = sqlMember.IsAscii ? SqlDbType.Char : SqlDbType.NChar;
                    else sqlType = sqlMember.IsAscii ? SqlDbType.VarChar : SqlDbType.NVarChar;
                    size = sqlMember.MaxLength <= maxStringSize ? sqlMember.MaxLength : maxStringSize;
                }
                else if (!sqlMember.IsFixedLength && sqlMember.MaxLength == -1)
                {
                    sqlType = sqlMember.IsAscii ? SqlDbType.VarChar : SqlDbType.NVarChar;
                    size = sqlMember.MaxLength <= maxStringSize ? sqlMember.MaxLength : maxStringSize;
                }
                else
                {
                    sqlType = sqlMember.IsAscii ? SqlDbType.Text : SqlDbType.NText;
                    size = int.MaxValue;
                }
            }
            else
            {
                sqlType = memberType.Type.formCSharpType();
                size = sqlType.getSize();
            }
            return new column
            {
                Name = member.MemberName,
                DbType = sqlType,
                Size = size,
                IsNull = (sqlMember.IsDefaultSqlMember && !memberType.IsString ? member.MemberType.IsNull : sqlMember.IsNull),
                DefaultValue = sqlMember.DefaultValue
            };
        }
        /// <summary>
        /// 删除表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        protected override bool dropTable(DbConnection connection, string tableName)
        {
            return executeNonQuery(connection, "drop table[" + this.connection.Owner + "].[" + tableName + "]") != ExecuteNonQueryError;
        }
        /// <summary>
        /// 写入索引名称
        /// </summary>
        /// <param name="sqlStream">SQL语句流</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        private static void appendIndexName(charStream sqlStream, string tableName, columnCollection columnCollection)
        {
            if (columnCollection.Name.length() == 0)
            {
                sqlStream.Write("ix_", tableName);
                foreach (column column in columnCollection.Columns)
                {
                    sqlStream.Write('_');
                    sqlStream.Write(column.Name);
                }
            }
            else sqlStream.Write(columnCollection.Name);
        }
        /// <summary>
        /// 写入列信息
        /// </summary>
        /// <param name="sqlStream">SQL语句流</param>
        /// <param name="column">列信息</param>
        /// <param name="isIdentity">是否自增列</param>
        private static void appendColumn(charStream sqlStream, column column, bool isIdentity)
        {
            sqlStream.Write('[');
            sqlStream.Write(column.Name);
            sqlStream.Write("][");
            sqlStream.Write(column.DbType.ToString());
            sqlStream.Write(']');
            if (isIdentity) sqlStream.Write(" identity(1, 1)not");
            else
            {
                if (column.DbType.isStringType() && column.Size != int.MaxValue)
                {
                    if (column.Size == -1) sqlStream.Write("(max)");
                    else
                    {
                        sqlStream.Write('(');
                        sqlStream.Write(column.Size.toString());
                        sqlStream.Write(')');
                    }
                }
                if (column.DefaultValue != null)
                {
                    sqlStream.Write(" default ");
                    sqlStream.Write(column.DefaultValue);
                }
                if (!column.IsNull) sqlStream.Write(" not");
            }
            sqlStream.Write(" null");
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        protected override bool createIndex(DbConnection connection, string tableName, columnCollection columnCollection)
        {
            charStream sqlStream = new charStream();
            sqlStream.Write(@"
create index[");
            appendIndexName(sqlStream, tableName, columnCollection);
            sqlStream.Write("]on[");
            sqlStream.Write(this.connection.Owner);
            sqlStream.Write("].[");
            sqlStream.Write(tableName);
            sqlStream.Write("]([");
            bool isFirst = true;
            foreach (column column in columnCollection.Columns)
            {
                if (isFirst) isFirst = false;
                else sqlStream.Write(',');
                sqlStream.Write(column.Name);
            }
            sqlStream.Write("])on[primary]");
            return executeNonQuery(connection, sqlStream.ToString()) != ExecuteNonQueryError;
        }
        /// <summary>
        /// 新增列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">新增列集合</param>
        /// <param name="owner">所有者</param>
        protected override bool addFields(DbConnection connection, columnCollection columnCollection)
        {
            string tableName = columnCollection.Name;
            charStream sqlStream = new charStream();
            foreach (column column in columnCollection.Columns)
            {
                sqlStream.Write(@"
alter table [");
                sqlStream.Write(this.connection.Owner);
                sqlStream.Write("].[");
                sqlStream.Write(tableName);
                sqlStream.Write(@"]add ");
                if (!column.IsNull && column.DefaultValue == null)
                {
                    column.DefaultValue = column.DbType.getDefaultValue();
                    if (column.DefaultValue == null) column.IsNull = true;
                }
                appendColumn(sqlStream, column, false);
            }
            return executeNonQuery(connection, sqlStream.ToString()) != ExecuteNonQueryError;
        }
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <returns>表格名称集合</returns>
        protected override list<string> getTableNames(DbConnection connection)
        {
            using (DbCommand command = GetCommand(connection, GetTableNameSql))
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
            {
                list<string> value = new list<string>();
                while (reader.Read()) value.Add((string)reader[0]);
                return value;
            }
        }
        /// <summary>
        /// 获取表格名称的SQL语句
        /// </summary>
        protected virtual string GetTableNameSql
        {
            get
            {
                return "select name from sysobjects where(status&0xe0000000)=0x60000000 and objectproperty(id,'IsUserTable')=1";
            }
        }
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="name">表格名称</param>
        /// <returns>表格信息</returns>
        protected override table getTable(DbConnection connection, string tableName)
        {
            using (DbCommand command = GetCommand(connection, GetTableSql(tableName)))
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
            {
                column identity = null;
                Dictionary<short, column> columns = new Dictionary<short, column>();
                while (reader.Read())
                {
                    SqlDbType type = sqlDbType.GetType((short)reader["xusertype"]);
                    int size = (int)(short)reader["length"];
                    if (type == SqlDbType.NChar || type == SqlDbType.NVarChar) size >>= 1;
                    else if (type == SqlDbType.Text || type == SqlDbType.NText) size = int.MaxValue;
                    column column = new column
                    {
                        Name = reader["name"].ToString(),
                        DbType = type,
                        Size = size,
                        DefaultValue = formatDefaultValue(reader["defaultValue"]),
                        Remark = reader["remark"].ToString(),
                        //GetColumnRemark(table, connection, name),
                        IsNull = (int)reader["isnullable"] == 1,
                    };
                    columns.Add((short)reader["colid"], column);
                    if ((int)reader["isidentity"] == 1) identity = column;
                }
                list<columnCollection> columnCollections = new list<columnCollection>();
                if (reader.NextResult())
                {
                    short indexId = -1;
                    string indexName = null;
                    columnCollection.type columnType = columnCollection.type.Index;
                    list<short> columnId = null;
                    while (reader.Read())
                    {
                        if (indexId != (short)reader["indid"])
                        {
                            if (indexId != -1)
                            {
                                column[] indexs = columnId.GetArray(columnIndex => columns[columnIndex]);
                                columnCollections.Add(new columnCollection
                                {
                                    Type = columnType,
                                    Name = indexName,
                                    Columns = indexs
                                });
                            }
                            columnId = new list<short>();
                            indexId = (short)reader["indid"];
                            indexName = reader["name"].ToString();
                            string type = reader["type"].ToString();
                            if (type == "PK") columnType = columnCollection.type.PrimaryKey;
                            else if (type == "UQ") columnType = columnCollection.type.UniqueIndex;
                            else columnType = columnCollection.type.Index;
                        }
                        columnId.Add((short)reader["colid"]);
                    }
                    if (indexId != -1)
                    {
                        columnCollections.Add(new columnCollection
                        {
                            Type = columnType,
                            Name = indexName,
                            Columns = columnId.GetArray(columnIndex => columns[columnIndex])
                        });
                    }
                }
                if (columns.Count != 0)
                {
                    columnCollection primaryKey = columnCollections.firstOrDefault(columnCollection => columnCollection.Type == columnCollection.type.PrimaryKey);
                    return new table
                    {
                        Columns = new columnCollection
                        {
                            Name = tableName,
                            Columns = columns.Values.getArray(),
                            Type = sql.columnCollection.type.None
                        },
                        Identity = identity,
                        PrimaryKey = primaryKey,
                        Indexs = columnCollections.GetFindArray(columnCollection => columnCollection.Type != columnCollection.type.PrimaryKey)
                    };
                }
                return null;
            }
        }
        /// <summary>
        /// 根据表格名称获取表格信息的SQL语句
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息的SQL语句</returns>
        protected virtual string GetTableSql(string tableName)
        {
            return @"declare @id int
set @id=object_id(N'[dbo].[" + tableName + @"]')
if(select top 1 id from sysobjects where id=@id and objectproperty(id,N'IsUserTable')=1)is not null begin
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultValue,isnull((select top 1 cast(value as varchar(256))from sysproperties where id=syscolumns.id and smallid=syscolumns.colid),'')as remark from syscolumns where id=@id order by colid
 if @@rowcount<>0 begin
  select a.indid,a.colid,b.name,(case when b.status=2 then 'UQ' else(select top 1 xtype from sysobjects where name=b.name)end)as type from sysindexkeys a left join sysindexes b on a.id=b.id and a.indid=b.indid where a.id=@id order by a.indid,a.keyno
 end
end";
        }
        /// <summary>
        /// 删除默认值左右括号()
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <returns>默认值</returns>
        protected static string formatDefaultValue(object defaultValue)
        {
            if (defaultValue != null)
            {
                string value = defaultValue.ToString();
                if (value.Length != 0)
                {
                    int valueIndex = 0, index = 0;
                    int[] valueIndexs = new int[value.Length];
                    for (int length = value.Length; index != length; ++index)
                    {
                        if (value[index] == '(') ++valueIndex;
                        else if (value[index] == ')') valueIndexs[--valueIndex] = index;
                    }
                    index = 0;
                    for (int length = value.Length - 1; valueIndexs[index] == length && value[index] == '('; --length) ++index;
                    value = value.Substring(index, value.Length - (index << 1));
                }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 对象转换成SQL字符串
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>SQL字符串</returns>
        public override string ToString(object value)
        {
            if (value != null)
            {
                func<object, string> toString = constantConverter.Default[value.GetType()];
                if (toString != null) return toString(value);
                return constantConverter.convertConstantStringMssql(value.ToString());
            }
            return "null";
        }
        ///// <summary>
        ///// like转义字符位图
        ///// </summary>
        //private static readonly String.asciiMap likeMap = new String.asciiMap(@"[]*_%", true);
        ///// <summary>
        ///// like转义
        ///// </summary>
        //private struct toLiker
        //{
        //    /// <summary>
        //    /// 源字符串
        //    /// </summary>
        //    public string Value;
        //    /// <summary>
        //    /// 字符串like转义
        //    /// </summary>
        //    /// <param name="map">转义索引位图</param>
        //    /// <returns>转义后的字符串</returns>
        //    public unsafe string Get(fixedMap map)
        //    {
        //        int count = 0;
        //        String.asciiMap likeMap = sql2000.likeMap;
        //        fixed (char* valueFixed = Value)
        //        {
        //            for (char* start = valueFixed, end = valueFixed + Value.Length; start != end; ++start)
        //            {
        //                if (likeMap.Get(*start))
        //                {
        //                    map.Set((int)(start - valueFixed));
        //                    ++count;
        //                }
        //            }
        //            if (count != 0)
        //            {
        //                string newValue = new string(']', Value.Length + (count << 1));
        //                fixed (char* newValueFixed = newValue)
        //                {
        //                    char* write = newValueFixed, read = valueFixed;
        //                    for (int index = 0; index != Value.Length; ++index)
        //                    {
        //                        if (map.Get(index))
        //                        {
        //                            *write++ = '[';
        //                            *write++ = *read++;
        //                            ++write;
        //                        }
        //                        else *write++ = *read++;
        //                    }
        //                }
        //                return newValue;
        //            }
        //        }
        //        return Value;
        //    }
        //}
        ///// <summary>
        ///// 字符串like转义
        ///// </summary>
        ///// <param name="value">字符串</param>
        ///// <returns>转义后的字符串</returns>
        //public unsafe string ToLike(string value)
        //{
        //    return value.length() != 0 ? fixedMap.GetMap<string>(value.Length, new toLiker { Value = value }.Get) : value;
        //}
        /// <summary>
        /// 获取插入数据SQL表达式
        /// </summary>
        /// <typeparam name="valueType">SQL表格类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberGetter">成员值获取器</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>插入数据SQL表达式</returns>
        private keyValue<list<string>, list<string>> insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberMap.memberGetter<valueType> memberGetter, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            list<string>.unsafer names = new list<string>(sqlTool.MemberInfo.MemberCount).Unsafer;
            list<string>.unsafer values = new list<string>(sqlTool.MemberInfo.MemberCount).Unsafer;
            IMemberInfo memberInfo = sqlTool.MemberInfo;
            memberMap = sqlTool.GetMemberMapClearIdentity(memberMap);
            for (int index = 0, count = sqlTool.MemberInfo.MemberCount; index != count; ++index)
            {
                if (memberMap.IsMember(index))
                {
                    names.Add(memberInfo.GetName(index));
                    values.Add(ToString(memberGetter[index]));
                }
            }
            return new keyValue<list<string>, list<string>>(names.List, values.List);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待插入数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        protected override bool insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
        {
            keyValue<list<string>, list<string>> values = insert(sqlTool, sqlTool.GetMemberGetter(value), memberMap);
            if (values.Key.Count != 0)
            {
                if (sqlTool.IdentityMemberIndex != sqlTable.NullIdentityMemberIndex)
                {
                    string sql = "insert into[" + sqlTool.TableName + "](" + values.Key.joinString(',') + ")values(" + values.Value.joinString(',') + @")
select top 1 " + getMemberNames(sqlTool.MemberInfo, sqlTool.MemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex) + "=@@IDENTITY";
                    if (set<valueType, memberType>(sql, sqlTool, value, sqlTool.MemberMap))
                    {
                        if (sqlTool.IsLockWrite) sqlTool.CallOnInsertedLock(value);
                        return true;
                    }
                }
                else if (sqlTool.IsPrimaryKey)
                {
                    string sql = "insert into[" + sqlTool.TableName + "](" + values.Key.joinString(',') + ")values(" + values.Value.joinString(',') + @")
if @@ROWCOUNT<>0 begin
 select top 1 " + getMemberNames(sqlTool.MemberInfo, sqlTool.MemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + sqlTool.GetPrimaryKey(value).joinString(" and ", primaryKey => primaryKey.Key + "=" + ToString(primaryKey.Value)) + @"
end";
                    if (set<valueType, memberType>(sql, sqlTool, value, sqlTool.MemberMap))
                    {
                        if (sqlTool.IsLockWrite) sqlTool.CallOnInsertedLock(value);
                        return true;
                    }
                }
                else
                {
                    if (ExecuteNonQuery("insert into[" + sqlTool.TableName + "](" + values.Key.joinString(',') + ")values(" + values.Value.joinString(',') + ")") > 0)
                    {
                        if (sqlTool.IsLockWrite) sqlTool.CallOnInsertedLock(value);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="values">待插入数据集合</param>
        /// <param name="dataTable">待插入数据集合</param>
        protected override valueType[] insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType[] values, DataTable dataTable)
        {
            using (DbConnection connection = GetConnection())
            {
                if (sqlTool.IdentityMemberIndex != sqlTable.NullIdentityMemberIndex)
                {
                    valueType identityValue = fastCSharp.setup.constructor<valueType>.New;
                    string identityName = sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex);
                    memberType identityMemberMap = default(memberType);
                    identityMemberMap.SetMember(sqlTool.IdentityMemberIndex);
                    if (set<valueType, memberType>("select top 1 " + identityName + " from[" + sqlTool.TableName + "]order by " + identityName + " desc", sqlTool, identityValue, identityMemberMap))
                    {
                        import(connection, dataTable, 0, 0);
                        list<valueType> newValues = new list<valueType>(values.Length);
                        string sql = "select " + getMemberNames(sqlTool.MemberInfo, sqlTool.MemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + identityName + ">" + ToString(sqlTool.GetMemberGetter(identityValue)[sqlTool.IdentityMemberIndex]);
                        using (DbCommand command = GetCommand(connection, sql))
                        {
                            try
                            {
                                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
                                {
                                    while (reader.Read())
                                    {
                                        valueType value = fastCSharp.setup.constructor<valueType>.New;
                                        sqlTool.Set(value, reader);
                                        newValues.Add(value);
                                    }
                                }
                            }
                            catch (Exception error)
                            {
                                fastCSharp.log.Default.Add(error, sql, false);
                                throw error;
                            }
                        }
                        values = newValues.ToArray();
                        if (sqlTool.IsLockWrite)
                        {
                            foreach (valueType value in values) sqlTool.CallOnInsertedLock(value);
                        }
                        return values;
                    }
                }
                else
                {
                    import(connection, dataTable, 0, 0);
                    if (sqlTool.IsLockWrite)
                    {
                        foreach (valueType value in values) sqlTool.CallOnInsertedLock(value);
                    }
                    return values;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取更新数据SQL表达式
        /// </summary>
        /// <typeparam name="valueType">SQL表格类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberGetter">成员值获取器</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>插入数据SQL表达式</returns>
        private list<string> update<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberMap.memberGetter<valueType> memberGetter, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            list<string>.unsafer values = new list<string>(sqlTool.MemberInfo.MemberCount).Unsafer;
            IMemberInfo memberInfo = sqlTool.MemberInfo;
            for (int index = 0, count = sqlTool.MemberInfo.MemberCount; index != count; ++index)
            {
                if (memberMap.IsMember(index))
                {
                    string name = memberInfo.GetName(index);
                    values.Add(name + "=" + ToString(memberGetter[index]));
                }
            }
            return values.List;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新前的数据对象,null表示失败</returns>
        protected override valueType update<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
        {
            valueType oldValue = fastCSharp.setup.constructor<valueType>.New;
            memberMap.memberGetter<valueType> memberGetter = sqlTool.GetMemberGetter(value);
            list<string> values = update(sqlTool, memberGetter, memberMap);
            if (values.Count != 0)
            {
                memberType selectMemberMap = sqlTool.GetSelectMemberMap(memberMap);
                string where = sqlTool.GetPrimaryKey(value).joinString(" and ", primaryKey => primaryKey.Key + "=" + ToString(primaryKey.Value));
                string select = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where;
                string sql = select + @"
if @@ROWCOUNT<>0 begin
 update[" + sqlTool.TableName + "]set " + values.joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
 " + select + @"
end";
                if (set<valueType, memberType>(sql, sqlTool, value, oldValue, selectMemberMap))
                {
                    sqlTool.CallOnUpdatedLock(value, oldValue, memberMap);
                    return oldValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新前的数据对象,null表示失败</returns>
        protected override valueType update<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
        {
            valueType oldValue = fastCSharp.setup.constructor<valueType>.New;
            list<keyValue<string, string>> values = sqlExpression.Get(memberMap);
            if (values.Count != 0)
            {
                memberType selectMemberMap = sqlTool.GetSelectMemberMap(memberMap);
                string where = sqlTool.GetPrimaryKey(value).joinString(" and ", primaryKey => primaryKey.Key + "=" + ToString(primaryKey.Value));
                string select = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where;
                string sql = select + @"
if @@ROWCOUNT<>0 begin
 update[" + sqlTool.TableName + "]set " + values.joinString(',', nameValue => nameValue.Key + "=" + nameValue.Value) + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
 " + select + @"
end";
                if (set<valueType, memberType>(sql, sqlTool, value, oldValue, selectMemberMap))
                {
                    sqlTool.CallOnUpdatedLock(value, oldValue, memberMap);
                    return oldValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新前的数据对象,null表示失败</returns>
        protected override valueType updateByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
        {
            valueType oldValue = fastCSharp.setup.constructor<valueType>.New;
            memberMap.memberGetter<valueType> memberGetter = sqlTool.GetMemberGetter(value);
            list<string> values = update(sqlTool, memberGetter, memberMap);
            if (values.Count != 0)
            {
                memberType selectMemberMap = sqlTool.GetSelectMemberMap(memberMap);
                string where = sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex) + "=" + ToString(memberGetter[sqlTool.IdentityMemberIndex]);
                string select = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where;
                string sql = select + @"
if @@ROWCOUNT<>0 begin
 update[" + sqlTool.TableName + "]set " + values.joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
 " + select + @"
end";
                if (set<valueType, memberType>(sql, sqlTool, value, oldValue, selectMemberMap))
                {
                    sqlTool.CallOnUpdatedLock(value, oldValue, memberMap);
                    //sqlTool.CallOnUpdatedByIdentityLock(value, oldValue, memberMap);
                    return oldValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新前的数据对象,null表示失败</returns>
        protected override valueType updateByIdentity<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
        {
            valueType oldValue = fastCSharp.setup.constructor<valueType>.New;
            list<keyValue<string, string>> values = sqlExpression.Get(memberMap);
            if (values.Count != 0)
            {
                memberType selectMemberMap = sqlTool.GetSelectMemberMap(memberMap);
                string where = sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex) + "=" + ToString(sqlTool.GetMemberGetter(value)[sqlTool.IdentityMemberIndex]);
                string select = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where;
                string sql = select + @"
if @@ROWCOUNT<>0 begin
 update[" + sqlTool.TableName + "]set " + values.joinString(',', nameValue => nameValue.Key + "=" + nameValue.Value) + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
 " + select + @"
end";
                if (set<valueType, memberType>(sql, sqlTool, value, oldValue, selectMemberMap))
                {
                    sqlTool.CallOnUpdatedLock(value, oldValue, memberMap);
                    //sqlTool.CallOnUpdatedByIdentityLock(value, oldValue, memberMap);
                    return oldValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待删除数据</param>
        /// <returns>是否成功</returns>
        protected override bool delete<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
        {
            string where = sqlTool.GetPrimaryKey(value).joinString(" and ", primaryKey => primaryKey.Key + "=" + ToString(primaryKey.Value));
            string sql = "select top 1 " + getMemberNames(sqlTool.MemberInfo, sqlTool.SelectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
if @@ROWCOUNT<>0 begin
 delete[" + sqlTool.TableName + "]from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
end";
            if (set<valueType, memberType>(sql, sqlTool, value, sqlTool.SelectMemberMap))
            {
                sqlTool.CallOnDeletedLock(value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待删除数据</param>
        /// <returns>是否成功</returns>
        protected override bool deleteByIdentity<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
        {
            string where = sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex) + "=" + ToString(sqlTool.GetMemberGetter(value)[sqlTool.IdentityMemberIndex]);
            string sql = "select top 1 " + getMemberNames(sqlTool.MemberInfo, sqlTool.SelectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
if @@ROWCOUNT<>0 begin
 delete[" + sqlTool.TableName + "]from[" + sqlTool.TableName + "]with(nolock)where " + where + @"
end";
            if (set<valueType, memberType>(sql, sqlTool, value, sqlTool.SelectMemberMap))
            {
                //sqlTool.CallOnDeletedByIdentityLock(value);
                sqlTool.CallOnDeletedLock(value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="logicConstantWhere">逻辑常量值</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="nameType">获取参数名称方式</param>
        /// <returns>SQL表达式(null表示常量条件),参数成员名称集合</returns>
        public override keyValue<string, list<string>> GetWhere(LambdaExpression expression, ref bool logicConstantWhere
            , bool isCache = false, sql.expression.converter.getNameType nameType = sql.expression.converter.getNameType.None)
        {
            if (expression != null)
            {
#if DOTNET35
                fastCSharp.sql.expression.LambdaExpression sqlExpression = fastCSharp.sql.expression.LambdaExpression.convert(expression);
#else
                LambdaExpression sqlExpression = expression;
#endif
                if (!sqlExpression.IsLogicConstantExpression) return converter.Convert(sqlExpression, isCache, nameType);
                logicConstantWhere = sqlExpression.LogicConstantValue;
            }
            else logicConstantWhere = true;
            return new keyValue<string, list<string>>();
        }
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="nameType">获取参数名称方式</param>
        /// <returns>SQL表达式</returns>
        public override keyValue<string, list<string>> GetSql(LambdaExpression expression
            , bool isCache = false, sql.expression.converter.getNameType nameType = sql.expression.converter.getNameType.None)
        {
            if (expression != null)
            {
#if DOTNET35
                fastCSharp.sql.expression.LambdaExpression sqlExpression = fastCSharp.sql.expression.LambdaExpression.convert(expression);
#else
                LambdaExpression sqlExpression = expression;
#endif
                return converter.Convert(sqlExpression, isCache, nameType);
            }
            return default(keyValue<string, list<string>>);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="expression">查询表达式</param>
        /// <returns>记录数,失败返回-1</returns>
        public override int Count<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, Expression<func<valueType, bool>> expression)
        {
            string sql = "select count(*)from[" + sqlTool.TableName + "]with(nolock)";
            if (expression != null)
            {
                bool logicConstantWhere = false;
                keyValue<string, list<string>> where = GetWhere(expression, ref logicConstantWhere);
                if (where.Key != null)
                {
                    sql += " where " + where.Key;
                    if (where.Value.count() != 0) sqlTool.CreateIndex(where.Value[0]);
                }
                else if (!logicConstantWhere) return 0;
            }
            return GetValue(sql, -1);
        }
        /// <summary>
        /// 格式化查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="query">查询信息</param>
        /// <returns>格式化查询信息</returns>
        private selectQuery createSelectQuery<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery<valueType> query)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (query == null) query = selectQuery<valueType>.NullQuery;
            else if (query.SkipCount < 0 || query.GetCount < 0) log.Default.Throw(log.exceptionType.IndexOutOfRange);
            selectQuery newQuery = new selectQuery { };
            bool logicConstantWhere = false;
            keyValue<string, list<string>> where = GetWhere(query.Where, ref logicConstantWhere);
            if (where.Key != null)
            {
                newQuery.Where = where.Key;
                if (where.Value.count() != 0)
                {
                    newQuery.IsCreatedIndex = true;
                    sqlTool.CreateIndex(where.Value[0]);
                }
            }
            else if (!logicConstantWhere) return newQuery;
            newQuery.IsSelect = true;
            if (query.Orders.length() != 0)
            {
                int orderIndex = 0, descCount = 0, size = 0;
                keyValue<string, bool>[] orders = new keyValue<string, bool>[query.Orders.Length];
                //list<string> orderMemberNames = new list<string>(query.Orders.Length);
                foreach (keyValue<Expression<action<valueType>>, bool> order in query.Orders)
                {
                    where = GetWhere(order.Key, ref logicConstantWhere);
                    if (!newQuery.IsCreatedIndex)
                    {
                        if (orderIndex == 0 && where.Value.count() != 0) sqlTool.CreateIndex(where.Value[0]);
                        newQuery.IsCreatedIndex = true;
                    }
                    if (order.Value) ++descCount;
                    size += where.Key.Length;
                    orders[orderIndex++] = new keyValue<string, bool>(where.Key, order.Value);
                    //if (where.Value != null)
                    //{
                    //    foreach (string name in where.Value)
                    //    {
                    //        if (orderMemberNames.indexOf(name) == -1) orderMemberNames.Add(name);
                    //    }
                    //}
                }
                newQuery.Orders = orders;
                newQuery.OrderDescCount = descCount;
                newQuery.OrderSize = size;
                //newQuery.OrderMemberNames = orderMemberNames;
            }
            newQuery.SkipCount = query.SkipCount;
            newQuery.GetCount = query.GetCount;
            return newQuery;
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
        public override IEnumerable<valueType> Select<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery<valueType> query = null, memberType memberMap = default(memberType))
        {
            selectQuery newQuery = createSelectQuery(sqlTool, query);
            if (!newQuery.IsSelect) return nullValue<valueType>.Array;
            memberType selectMemberMap = sqlTool.MemberMap.Copy();
            if (!memberMap.IsDefault) selectMemberMap.And(memberMap);
            return select(sqlTool, newQuery, selectMemberMap);
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
        internal IEnumerable<valueType> selectNoOrder<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery query, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            int count = query.SkipCount + query.GetCount;
            string sql = "select " + (count == 0 ? null : ("top " + count.toString() + " ")) + getMemberNames(sqlTool.MemberInfo, memberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)" + (query.Where != null ? "where " + query.Where + " " : null) + query.Order;
            return select<valueType, memberType>(sql, sqlTool, query.SkipCount, memberMap);
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
        internal virtual IEnumerable<valueType> select<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery query, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
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
            string where = keyName + " not in(select top " + query.SkipCount.toString() + " " + keyName + " from[" + sqlTool.TableName + "]with(nolock)" + (query.Where != null ? "where " + query.Where + " " : null) + order + ")";
            if (query.Where != null) where = "(" + query.Where + ")and " + where;
            string sql = @"select " + getMemberNames(sqlTool.MemberInfo, memberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock)where " + keyName + " in(select top " + query.GetCount.toString() + " " + keyName + " from[" + sqlTool.TableName + "]with(nolock)where " + where + order + ")";
            return select<valueType, memberType>(sql, sqlTool, 0, memberMap);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        public override valueType GetByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap = default(memberType))
        {
            memberType selectMemberMap = sqlTool.MemberMap.Copy();
            if (!memberMap.IsDefault) selectMemberMap.And(memberMap);
            string sql = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock) where " + sqlTool.MemberInfo.GetName(sqlTool.IdentityMemberIndex) + "=" + ToString(sqlTool.GetMemberGetter(value)[sqlTool.IdentityMemberIndex]);
            return set<valueType, memberType>(sql, sqlTool, value, selectMemberMap) ? value : default(valueType);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        public override valueType GetByPrimaryKey<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap = default(memberType))
        {
            memberType selectMemberMap = sqlTool.MemberMap.Copy();
            if (!memberMap.IsDefault) selectMemberMap.And(memberMap);
            selectMemberMap.Or(sqlTool.PrimaryKeyMemberMap);
            string sql = "select top 1 " + getMemberNames(sqlTool.MemberInfo, selectMemberMap).joinString(',') + " from[" + sqlTool.TableName + "]with(nolock) where " + sqlTool.GetPrimaryKey(value).joinString(" and ", primaryKey => primaryKey.Key + "=" + ToString(primaryKey.Value));
            return set<valueType, memberType>(sql, sqlTool, value, selectMemberMap) ? value : default(valueType);
        }
    }
}
