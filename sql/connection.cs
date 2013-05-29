using System;
using System.Reflection;
using fastCSharp.reflection;
using fastCSharp.setup;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public unsafe class connection// : Attribute
    {
        /// <summary>
        /// SQL类型
        /// </summary>
        public fastCSharp.sql.typeInfo.type Type;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection;
        /// <summary>
        /// 数据库表格所有者
        /// </summary>
        public string Owner = "dbo";
        /// <summary>
        /// 代码生成路径,null表示不生成代码
        /// </summary>
        public string ModelCodePath;
        /// <summary>
        /// SQL客户端
        /// </summary>
        private client client;
        /// <summary>
        /// SQL客户端
        /// </summary>
        public client Client
        {
            get
            {
                if (client == null)
                {
                    client = (client)Enum<fastCSharp.sql.typeInfo.type, fastCSharp.sql.typeInfo>.Array(Type).ClientType
                        .GetConstructor(new Type[] { typeof(connection) }).Invoke(new object[] { this });
                }
                return client;
            }
        }

        /// <summary>
        /// 是否需要检测链接
        /// </summary>
        public static bool IsCheckConnection
        {
            get
            {
                return config.sql.Default.CheckConnection.Length != 0;
            }
        }
        /// <summary>
        /// 根据连接类型获取连接信息
        /// </summary>
        /// <param name="type">连接类型</param>
        /// <returns>连接信息</returns>
        public static connection GetConnection(string type)
        {
            return config.pub.Default.LoadConfig(new connection(), type.ToString());
        }
        /// <summary>
        /// 检测链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void checkConnection(object sender, AssemblyLoadEventArgs args)
        {
            checkConnection(args.LoadedAssembly);
        }
        /// <summary>
        /// 检测链接程序集名称集合
        /// </summary>
        private static readonly staticHashSet<string> checkConnectionAssemblyNames = new staticHashSet<string>(new string[] { "mscorlib", fastCSharp.pub.fastCSharp, "System", "Microsoft", "vshost" });
        /// <summary>
        /// 检测链接程序集名称分隔符位图
        /// </summary>
        private static readonly pointer checkConnectionAssemblyNameMap;
        /// <summary>
        /// 检测链接
        /// </summary>
        /// <param name="assembly">程序集</param>
        private unsafe static void checkConnection(Assembly assembly)
        {
            bool isAssembly;
            string assemblyName = assembly.FullName;
            fixed (char* nameFixed = assemblyName)
            {
                char* splitIndex = unsafer.String.findAscii(nameFixed, nameFixed + assemblyName.Length, checkConnectionAssemblyNameMap.Byte, ',');
                isAssembly = splitIndex == null || !checkConnectionAssemblyNames.Contains(assemblyName.Substring(0, (int)(splitIndex - nameFixed)));
            }
            if (isAssembly)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    list<keyValue<Type, keyValue<Type, sqlTable>>> tables = types
                        .getList(type => new keyValue<Type, Type>(type, fastCSharp.setup.cSharp.sqlTable.GetSqlType(type)))
                        .remove(type => type.Value == null)
                        .getList(type => new keyValue<Type, keyValue<Type, sqlTable>>(type.Key, new keyValue<Type, sqlTable>(type.Value, type.Value.customAttribute<sqlTable>(false, fastCSharp.setup.cSharp.sqlTable.cSharp.Default.IsInheritAttribute))))
                        .remove(type => type.Value.Value == null || !type.Value.Value.IsSetup || type.Value.Value.IsReflection || config.sql.Default.CheckConnection.indexOf(type.Value.Value.ConnectionType) == -1);
                    foreach (keyValue<Type, keyValue<Type, sqlTable>> table in tables) checkSqlTable(table);

                    list<keyValue<Type, sqlTable>> reflectionTables = types
                        .getList(type => new keyValue<Type, sqlTable>(type, type.customAttribute<sqlTable>(false, fastCSharp.setup.cSharp.sqlTable.cSharp.Default.IsInheritAttribute)))
                        .remove(type => type.Value == null || !type.Value.IsSetup || !type.Value.IsReflection || config.sql.Default.CheckConnection.indexOf(type.Value.ConnectionType) == -1);
                    foreach (keyValue<Type, sqlTable> table in reflectionTables)
                    {
                        checkSqlTable(new keyValue<Type, keyValue<Type, sqlTable>>(table.Key, table));
                    }
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, null, true);
                }
            }
        }
        /// <summary>
        /// 检测SQL表格
        /// </summary>
        /// <param name="table">SQL表格</param>
        private static void checkSqlTable(keyValue<Type, keyValue<Type, sqlTable>> table)
        {
            table memberTable = setup.cSharp.sqlTable.cSharp.GetTable(table.Key, table.Value.Value, setup.cSharp.sqlTable.cSharp.GetMembers(table.Value.Key, table.Value.Value));
            client sqlClient = GetConnection(table.Value.Value.ConnectionType).Client;
            table sqlTable = sqlClient.GetTable(memberTable.Columns.Name);
            if (sqlTable == null)
            {
                if (!sqlClient.CreateTable(memberTable)) fastCSharp.log.Default.Real("表格 " + memberTable.Columns.Name + " 创建失败", false, false);
            }
            else
            {
                staticDictionary<string, column> sqlColumnNames = new staticDictionary<string, column>(sqlTable.Columns.Columns, value => value.Name);
                list<column> newColumns = memberTable.Columns.Columns.getFind(value => !sqlColumnNames.ContainsKey(value.Name));
                if (newColumns.count() != 0)
                {
                    if (sqlClient.AddFields(new columnCollection { Name = memberTable.Columns.Name, Columns = newColumns.toArray() }))
                    {
                        sqlTable.Columns.Columns = newColumns.add(sqlTable.Columns.Columns).toArray();
                    }
                    else
                    {
                        fastCSharp.log.Default.Real("表格 " + memberTable.Columns.Name + " 字段添加失败 : " + newColumns.joinString(',', value => value.Name), false, false);
                    }
                }
                newColumns = memberTable.Columns.Columns.getFind(value => !value.IsMatch(sqlColumnNames.Get(value.Name, null)));
                if (newColumns.count() != 0)
                {
                    fastCSharp.log.Default.Real("表格 " + memberTable.Columns.Name + " 字段类型不匹配 : " + newColumns.joinString(',', value => value.Name), false, false);
                }
            }
        }

        static connection()
        {
            if (config.sql.Default.CheckConnection.Length != 0)
            {
                checkConnectionAssemblyNameMap = new String.asciiMap(unmanaged.Get(String.asciiMap.mapBytes), ".,-").Pointer;
                AppDomain.CurrentDomain.AssemblyLoad += checkConnection;
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) checkConnection(assembly);
            }
        }
    }
}
