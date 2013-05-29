using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using fastCSharp.setup;
using fastCSharp.setup.cSharp;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading;

namespace fastCSharp.sql
{
    /// <summary>
    /// SQL客户端操作
    /// </summary>
    public abstract class client
    {
        /// <summary>
        /// 执行错误返回值
        /// </summary>
        public const int ExecuteNonQueryError = int.MinValue;
        /// <summary>
        /// SQL连接信息
        /// </summary>
        protected connection connection;
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        protected client(connection connection)
        {
            this.connection = connection;
        }
        /// <summary>
        /// 根据SQL连接类型获取SQL连接
        /// </summary>
        /// <param name="isAsynchronous">是否异步连接</param>
        /// <returns>SQL连接</returns>
        protected abstract DbConnection getConnection(bool isAsynchronous);
        /// <summary>
        /// 根据SQL连接类型获取SQL连接
        /// </summary>
        /// <param name="isAsynchronous">是否异步连接</param>
        /// <returns>SQL连接</returns>
        public DbConnection GetConnection(bool isAsynchronous = false)
        {
            DbConnection connection = getConnection(isAsynchronous);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception error)
            {
                connection.Dispose();
                fastCSharp.log.Default.Add(error, null, true);
                return null;
            }
        }
        /// <summary>
        /// 获取SQL命令
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>SQL命令</returns>
        public abstract DbCommand GetCommand
            (DbConnection connection, string sql, SqlParameter[] parameters = null, CommandType type = CommandType.Text);
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据适配器</returns>
        protected abstract DbDataAdapter getAdapter(DbCommand command);
        /// <summary>
        /// 获取数据集并关闭SQL命令
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据集</returns>
        public DataSet GetDataSet(DbCommand command)
        {
            using (command)
            {
                DbDataAdapter adapter = getAdapter(command);
                if (adapter != null)
                {
                    DataSet data = new DataSet();
                    adapter.Fill(data);
                    return data;
                }
                return null;
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="command">SQL命令</param>
        /// <param name="errorValue">错误值</param>
        /// <returns>数据</returns>
        public valueType GetValue<valueType>(DbCommand command, valueType errorValue = default(valueType))
        {
            object value = command.ExecuteScalar();
            return value != null && value != DBNull.Value ? (valueType)value : errorValue;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="connection">SQL连接</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="errorValue">错误值</param>
        /// <returns>数据</returns>
        protected valueType getValue<valueType>(DbConnection connection, string sql, valueType errorValue)
        {
            try
            {
                using (DbCommand command = GetCommand(connection, sql)) return GetValue(command, errorValue);
            }
            catch (Exception error)
            {
                fastCSharp.log.Default.Add(error, sql, false);
            }
            return errorValue;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="errorValue">错误值</param>
        /// <param name="connection">SQL连接</param>
        /// <returns>数据</returns>
        public valueType GetValue<valueType>
            (string sql, valueType errorValue = default(valueType), DbConnection connection = null)
        {
            if (sql.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return getValue(connection, sql, errorValue);
                }
                else return getValue(connection, sql, errorValue);
            }
            return errorValue;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数,错误返回ExecuteNonQueryError</returns>
        protected int executeNonQuery(DbConnection connection, string sql)
        {
            try
            {
                using (DbCommand command = GetCommand(connection, sql)) return command.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                fastCSharp.log.Default.Add(error, sql, false);
            }
            return ExecuteNonQueryError;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="connection">SQL连接</param>
        /// <returns>受影响的行数,错误返回ExecuteNonQueryError</returns>
        public int ExecuteNonQuery(string sql, DbConnection connection = null)
        {
            if (sql.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return executeNonQuery(connection, sql);
                }
                else return executeNonQuery(connection, sql);
            }
            return ExecuteNonQueryError;
        }
        /// <summary>
        /// 是否支持DataTable导入
        /// </summary>
        protected virtual bool isImport
        {
            get { return false; }
        }
        /// <summary>
        /// 导入数据集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="data">数据集合</param>
        /// <param name="batchSize">批处理数量</param>
        /// <param name="timeout">超时秒数</param>
        protected abstract void import(DbConnection connection, DataTable data, int batchSize = 0, int timeout = 0);
        /// <summary>
        /// 导入数据集合
        /// </summary>
        /// <param name="data">数据集合</param>
        /// <param name="batchSize">批处理数量,0表示默认数量</param>
        /// <param name="timeout">超时秒数,0表示不设置超时</param>
        /// <param name="connection">SQL连接</param>
        public void Import(DataTable data, int batchSize = 0, int timeout = 0, DbConnection connection = null)
        {
            if (data != null && data.Rows.Count != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) import(connection, data, batchSize, timeout);
                }
                else import(connection, data, batchSize, timeout);
            }
        }
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格是否存在</returns>
        protected abstract bool isTable(DbConnection connection, string tableName);
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="connection">SQL连接</param>
        /// <returns>表格是否存在</returns>
        public bool IsTable(string tableName, DbConnection connection = null)
        {
            if (tableName.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return isTable(connection, tableName);
                }
                return isTable(connection, tableName);
            }
            return false;
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="table">表格信息</param>
        protected abstract bool createTable(DbConnection connection, table table);
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="table">表格信息</param>
        /// <param name="connection">SQL连接</param>
        public bool CreateTable(table table, DbConnection connection = null)
        {
            if (table != null && table.Columns != null && table.Columns.Name.length() != 0 && table.Columns.Columns.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return createTable(connection, table);
                }
                return createTable(connection, table);
            }
            return false;
        }
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="sqlMember">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal abstract column GetColumn(setup.memberInfo member, sqlMember sqlMember);
        /// <summary>
        /// 删除表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        protected abstract bool dropTable(DbConnection connection, string tableName);
        /// <summary>
        /// 删除表格
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="connection">SQL连接</param>
        public bool DropTable(string tableName, DbConnection connection = null)
        {
            if (tableName.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return dropTable(connection, tableName);
                }
                return dropTable(connection, tableName);
            }
            return false;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        protected abstract bool createIndex(DbConnection connection, string tableName, columnCollection columnCollection);
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        /// <param name="connection">SQL连接</param>
        public bool CreateIndex(string tableName, columnCollection columnCollection, DbConnection connection = null)
        {
            if (tableName.length() != 0 && columnCollection != null && columnCollection.Columns.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return createIndex(connection, tableName, columnCollection);
                }
                return createIndex(connection, tableName, columnCollection);
            }
            return false;
        }
        /// <summary>
        /// 新增列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">新增列集合</param>
        protected abstract bool addFields(DbConnection connection, columnCollection columnCollection);
        /// <summary>
        /// 新增列集合
        /// </summary>
        /// <param name="columnCollection">新增列集合</param>
        /// <param name="connection">SQL连接</param>
        public bool AddFields(columnCollection columnCollection, DbConnection connection = null)
        {
            if (columnCollection != null && columnCollection.Columns.length() != 0 && columnCollection.Name.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return addFields(connection, columnCollection);
                }
                return addFields(connection, columnCollection);
            }
            return false;
        }
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <returns>表格名称集合</returns>
        protected abstract list<string> getTableNames(DbConnection connection);
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <returns>表格名称集合</returns>
        public list<string> GetTableNames(DbConnection connection = null)
        {
            if (connection == null)
            {
                using (connection = GetConnection()) return getTableNames(connection);
            }
            return getTableNames(connection);
        }
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="name">表格名称</param>
        /// <returns>表格信息</returns>
        protected abstract table getTable(DbConnection connection, string tableName);
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="name">表格名称</param>
        /// <returns>表格信息</returns>
        public table GetTable(string tableName, DbConnection connection = null)
        {
            if (tableName.length() != 0)
            {
                if (connection == null)
                {
                    using (connection = GetConnection()) return getTable(connection, tableName);
                }
                return getTable(connection, tableName);
            }
            return null;
        }
        /// <summary>
        /// 获取成员名称集合
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="memberInfo">成员信息获取器</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>成员名称集合</returns>
        protected static list<string> getMemberNames<memberType>(IMemberInfo memberInfo, memberType memberMap) 
            where memberType : IMemberMap<memberType>
        {
            list<string>.unsafer names = new list<string>(memberInfo.MemberCount).Unsafer;
            for (int index = 0, count = memberInfo.MemberCount; index != count; ++index)
            {
                if (memberMap.IsMember(index)) names.Add(memberInfo.GetName(index));
            }
            return names.List;
        }
        /// <summary>
        /// 对象转换成SQL字符串
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>SQL字符串</returns>
        public abstract string ToString(object value);
        /// <summary>
        /// 执行SQL语句并更新成员
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">目标对象</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新是否成功</returns>
        protected bool set<valueType, memberType>
            (string sql, sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            using (DbConnection connection = GetConnection())
            using (DbCommand command = GetCommand(connection, sql))
            {
                try
                {
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            sqlTool.Set(value, reader, memberMap);
                            return true;
                        }
                    }
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, sql, false);
                }
            }
            return false;
        }
        /// <summary>
        /// 执行SQL语句并更新成员
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">目标对象</param>
        /// <param name="oldValue">更新前的目标对象</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>更新是否成功</returns>
        protected bool set<valueType, memberType>
            (string sql, sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, valueType oldValue, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            using (DbConnection connection = GetConnection())
            using (DbCommand command = GetCommand(connection, sql))
            {
                try
                {
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            sqlTool.Set(oldValue, reader, memberMap);
                            if (reader.NextResult() && reader.Read())
                            {
                                sqlTool.Set(value, reader, memberMap);
                                return true;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, sql, false);
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
        /// <param name="value">待插入数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        protected abstract bool insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待插入数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        private bool insertLock<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            bool isInsert = false;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnInsertLock(value)) isInsert = insert(sqlTool, value, memberMap);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else isInsert = insert(sqlTool, value, memberMap);
            if (isInsert) sqlTool.CallOnInserted(value);
            return isInsert;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待插入数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool Insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (sqlTool.IsSqlVerify(value, memberMap) && sqlTool.CallOnInsert(value))
            {
                if (!isIgnoreTransaction && sqlTool.IsInsertTransaction)
                {
                    if (domainUnload.TransactionStart())
                    {
                        try
                        {
                            return insertLock(sqlTool, value, memberMap);
                        }
                        finally { domainUnload.TransactionEnd(); }
                    }
                }
                else return insertLock(sqlTool, value, memberMap);
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
        /// <returns>插入的数据集合</returns>
        protected abstract valueType[] insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType[] values, DataTable dataTable)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="values">待插入数据集合</param>
        /// <param name="dataTable">待插入数据集合</param>
        /// <returns>插入的数据集合</returns>
        private valueType[] insertLock<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType[] values, DataTable dataTable)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            valueType[] newValues = null;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnInsertLock(values)) newValues = insert(sqlTool, values, dataTable);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else newValues = insert(sqlTool, values, dataTable);
            if (newValues != null)
            {
                foreach (valueType value in newValues) sqlTool.CallOnInserted(value);
            }
            return newValues;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="valuse">待插入数据集合</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>插入的数据集合,失败返回null</returns>
        public valueType[] Insert<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType[] values, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (isImport)
            {
                memberType memberMap = default(memberType);
                if (values.count(value => sqlTool.IsSqlVerify(value, memberMap)) == values.Length && sqlTool.CallOnInsert(values))
                {
                    DataTable dataTable = sqlTool.GetDataTable(values);
                    if (!isIgnoreTransaction && sqlTool.IsInsertTransaction)
                    {
                        if (domainUnload.TransactionStart())
                        {
                            try
                            {
                                return insertLock(sqlTool, values, dataTable);
                            }
                            finally { domainUnload.TransactionEnd(); }
                        }
                    }
                    else return insertLock(sqlTool, values, dataTable);
                }
            }
            else
            {
                list<valueType>.unsafer newValues = new list<valueType>(values.Length).Unsafer;
                foreach (valueType value in values)
                {
                    if (Insert(sqlTool, value, default(memberType))) newValues.Add(value);
                }
                return newValues.List.ToArray();
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
        protected abstract valueType update<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        private bool updateLock<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            valueType oldValue = null;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnUpdateLock(value, memberMap)) oldValue = update(sqlTool, value, memberMap);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else oldValue = update(sqlTool, value, memberMap);
            if (oldValue != null)
            {
                sqlTool.CallOnUpdated(value, oldValue, memberMap);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool Update<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            memberMap = sqlTool.GetMemberMapClearIdentity(memberMap);
            if (sqlTool.IsSqlVerify(value, memberMap) && sqlTool.CallOnUpdate(value, memberMap))
            {
                if (!isIgnoreTransaction && sqlTool.IsUpdateTransaction)
                {
                    if (domainUnload.TransactionStart())
                    {
                        try
                        {
                            return updateLock(sqlTool, value, memberMap);
                        }
                        finally { domainUnload.TransactionEnd(); }
                    }
                }
                else return updateLock(sqlTool, value, memberMap);
            }
            return false;
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
        protected abstract valueType update<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        private bool updateLock<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            valueType oldValue = null;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnUpdateLock(value, memberMap)) oldValue = update(sqlTool, value, sqlExpression, memberMap);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else oldValue = update(sqlTool, value, sqlExpression, memberMap);
            if (oldValue != null)
            {
                sqlTool.CallOnUpdated(value, oldValue, memberMap);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool Update<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value
            , sqlTable.ISqlExpression<valueType, memberType> sqlExpression, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (!sqlExpression.MemberMap.IsDefault)
            {
                memberType memberMap = sqlTool.GetMemberMapClearIdentity(sqlExpression.MemberMap);
                if (sqlTool.CallOnUpdate(value, memberMap))
                {
                    if (!isIgnoreTransaction && sqlTool.IsUpdateTransaction)
                    {
                        if (domainUnload.TransactionStart())
                        {
                            try
                            {
                                return updateLock(sqlTool, value, sqlExpression, memberMap);
                            }
                            finally { domainUnload.TransactionEnd(); }
                        }
                    }
                    else return updateLock(sqlTool, value, sqlExpression, memberMap);
                }
            }
            return false;
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
        protected abstract valueType updateByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        private bool updateByIdentityLock<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            valueType oldValue = null;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnUpdateByIdentityLock(value, memberMap)) oldValue = updateByIdentity(sqlTool, value, memberMap);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else oldValue = updateByIdentity(sqlTool, value, memberMap);
            if (oldValue != null)
            {
                sqlTool.CallOnUpdated(value, oldValue, memberMap);
                //sqlTool.CallOnUpdatedByIdentity(value, oldValue, memberMap);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool UpdateByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            memberMap = sqlTool.GetMemberMapClearIdentity(memberMap);
            if (sqlTool.IsSqlVerify(value, memberMap) && sqlTool.CallOnUpdateByIdentity(value, memberMap))
            {
                if (!isIgnoreTransaction && sqlTool.IsUpdateTransaction)
                {
                    if (domainUnload.TransactionStart())
                    {
                        try
                        {
                            return updateByIdentityLock(sqlTool, value, memberMap);
                        }
                        finally { domainUnload.TransactionEnd(); }
                    }
                }
                else return updateByIdentityLock(sqlTool, value, memberMap);
            }
            return false;
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
        protected abstract valueType updateByIdentity<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="memberMap">目标成员位图</param>
        /// <returns>是否成功</returns>
        private bool updateByIdentityLock<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool
            , valueType value, sqlTable.ISqlExpression<valueType, memberType> sqlExpression, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            valueType oldValue = null;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnUpdateByIdentityLock(value, memberMap)) oldValue = updateByIdentity(sqlTool, value, sqlExpression, memberMap);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else oldValue = updateByIdentity(sqlTool, value, sqlExpression, memberMap);
            if (oldValue != null)
            {
                sqlTool.CallOnUpdated(value, oldValue, memberMap);
                //sqlTool.CallOnUpdatedByIdentity(value, oldValue, memberMap);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待更新数据自增id标识</param>
        /// <param name="sqlExpression">待更新数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool UpdateByIdentity<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value
            , sqlTable.ISqlExpression<valueType, memberType> sqlExpression, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (!sqlExpression.MemberMap.IsDefault)
            {
                memberType memberMap = sqlTool.GetMemberMapClearIdentity(sqlExpression.MemberMap);
                if (sqlTool.CallOnUpdateByIdentity(value, memberMap))
                {
                    if (!isIgnoreTransaction && sqlTool.IsUpdateTransaction)
                    {
                        if (domainUnload.TransactionStart())
                        {
                            try
                            {
                                return updateByIdentityLock(sqlTool, value, sqlExpression, memberMap);
                            }
                            finally { domainUnload.TransactionEnd(); }
                        }
                    }
                    else return updateByIdentityLock(sqlTool, value, sqlExpression, memberMap);
                }
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
        protected abstract bool delete<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待删除数据</param>
        /// <returns>是否成功</returns>
        private bool deleteLock<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            bool isDelete = false;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnDeleteLock(value)) isDelete = delete(sqlTool, value);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else isDelete = delete(sqlTool, value);
            if (isDelete)
            {
                sqlTool.CallOnDeleted(value);
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
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool Delete<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (sqlTool.CallOnDelete(value))
            {
                if (!isIgnoreTransaction && sqlTool.IsDeleteTransaction)
                {
                    if (domainUnload.TransactionStart())
                    {
                        try
                        {
                            return deleteLock(sqlTool, value);
                        }
                        finally { domainUnload.TransactionEnd(); }
                    }
                }
                else return deleteLock(sqlTool, value);
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
        protected abstract bool deleteByIdentity<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">待删除数据</param>
        /// <returns>是否成功</returns>
        private bool deleteByIdentityLock<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            bool isDelete = false;
            if (sqlTool.IsLockWrite)
            {
                fastCSharp.threading.lockCheck.Enter(sqlTool.Lock);
                try
                {
                    if (sqlTool.CallOnDeleteByIdentityLock(value)) isDelete = deleteByIdentity(sqlTool, value);
                }
                finally { fastCSharp.threading.lockCheck.Exit(sqlTool.Lock); }
            }
            else isDelete = deleteByIdentity(sqlTool, value);
            if (isDelete)
            {
                sqlTool.CallOnDeleted(value);
                //sqlTool.CallOnDeletedByIdentity(value);
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
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        public bool DeleteByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, bool isIgnoreTransaction = false)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            if (sqlTool.CallOnDeleteByIdentity(value))
            {
                if (!isIgnoreTransaction && sqlTool.IsDeleteTransaction)
                {
                    if (domainUnload.TransactionStart())
                    {
                        try
                        {
                            return deleteByIdentityLock(sqlTool, value);
                        }
                        finally { domainUnload.TransactionEnd(); }
                    }
                }
                else return deleteByIdentityLock(sqlTool, value);
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
        public abstract keyValue<string, list<string>> GetWhere(LambdaExpression expression, ref bool logicConstantWhere
            , bool isCache = false, sql.expression.converter.getNameType nameType = sql.expression.converter.getNameType.None);
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="nameType">获取参数名称方式</param>
        /// <returns>SQL表达式</returns>
        public abstract keyValue<string, list<string>> GetSql(LambdaExpression expression
            , bool isCache = false, sql.expression.converter.getNameType nameType = sql.expression.converter.getNameType.None);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="expression">查询表达式</param>
        /// <returns>记录数,失败返回-1</returns>
        public abstract int Count<valueType, memberType>(sqlTable.sqlToolBase<valueType, memberType> sqlTool, Expression<func<valueType, bool>> expression)
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 执行SQL语句并返回数据集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据集合</returns>
        protected IEnumerable<valueType> select<valueType, memberType>
            (string sql, sqlTable.sqlToolBase<valueType, memberType> sqlTool, int skipCount, memberType memberMap)
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            using (DbConnection connection = GetConnection())
            using (DbCommand command = GetCommand(connection, sql))
            {
                DbDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader(CommandBehavior.Default);
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, sql, false);
                }
                if (reader != null)
                {
                    using (reader)
                    {
                        while (skipCount != 0 && reader.Read()) --skipCount;
                        if (skipCount == 0)
                        {
                            while (reader.Read())
                            {
                                valueType value = fastCSharp.setup.constructor<valueType>.New;
                                sqlTool.Set(value, reader, memberMap);
                                yield return value;
                            }
                        }
                    }
                }
            }
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
        public abstract IEnumerable<valueType> Select<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, selectQuery<valueType> query = null, memberType memberMap = default(memberType))
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        public abstract valueType GetByIdentity<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap = default(memberType))
            where valueType : class
            where memberType : IMemberMap<memberType>;
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>对象集合</returns>
        public abstract valueType GetByPrimaryKey<valueType, memberType>
            (sqlTable.sqlToolBase<valueType, memberType> sqlTool, valueType value, memberType memberMap = default(memberType))
            where valueType : class
            where memberType : IMemberMap<memberType>;
    }
}
