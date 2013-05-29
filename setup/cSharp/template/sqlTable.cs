using System;

namespace fastCSharp.setup.cSharp.template
{
    class sqlTable : pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            #region IF PrimaryKey
            /// <summary>
            /// 关键字
            /// </summary>
            public struct sqlPrimaryKey : IEquatable<sqlPrimaryKey>
            {
                #region LOOP PrimaryKey
                public @MemberType.FullName @MemberName;
                #endregion LOOP PrimaryKey
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other">关键字</param>
                /// <returns>是否相等</returns>
                public bool Equals(sqlPrimaryKey other)
                {
                    #region LOOP PrimaryKey
                    if (@MemberName != other.@MemberName) return false;
                    #endregion LOOP PrimaryKey
                    return true;
                }
                /// <summary>
                /// 哈希编码
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return /*LOOP:PrimaryKey*/@MemberName/**/.GetHashCode() ^ /*LOOP:PrimaryKey*/ 0;
                }
                #region IF IsSinglePrimaryKey
                #region LOOP PrimaryKey
                public static implicit operator sqlPrimaryKey(@MemberType.FullName value) { return new sqlPrimaryKey { @MemberName = value }; }
                public static implicit operator @MemberType.FullName(sqlPrimaryKey value) { return value.@MemberName; }
                #endregion LOOP PrimaryKey
                #endregion IF IsSinglePrimaryKey
            }
            #endregion IF PrimaryKey

            /// <summary>
            /// 数据库表格
            /// </summary>
            /// <typeparam name="valueType">表格映射类型</typeparam>
            public class sqlTable<valueType> : @type.FullName, fastCSharp.setup.cSharp.sqlTable.ISql<valueType, @type.FullName/**/.memberMap>
                /*IF:Identity*/, fastCSharp.setup.cSharp.sqlTable.ISqlIdentity<valueType, @type.FullName/**/.memberMap, @Identity.MemberType.FullName>/*IF:Identity*/
                /*IF:PrimaryKey*/, fastCSharp.setup.cSharp.sqlTable.ISqlKey<valueType, @type.FullName/**/.memberMap, sqlPrimaryKey>/*IF:PrimaryKey*/
                where valueType : sqlTable<valueType>
            {
                #region NOTE
                new FullName MemberName;
                static setup.cSharp.sqlTable Attribute = null;
                new static memberInfo Identity = null;
                static sqlMember Value = null;
                #endregion NOTE
                /// <summary>
                /// SQL操作工具
                /// </summary>
                public static readonly fastCSharp.setup.cSharp.sqlTable.sqlTool<valueType, @type.FullName/**/.memberMap> SqlTool;
                /// <summary>
                /// 设置字段值
                /// </summary>
                /// <param name="reader">字段读取器</param>
                /// <param name="memberMap">成员位图</param>
                public void Set(System.Data.Common.DbDataReader reader, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
                {
                    int index = -1;
                    if (memberMap.IsDefault)
                    {
                        #region LOOP SqlMembers
                        /*PUSH:Key*/
                        this.@MemberName = (@MemberType.FullName)/*PUSH:Key*//*PUSH:Value.SqlMemberType*/(@FullName)/*PUSH:Value.SqlMemberType*/reader[++index];
                        #endregion LOOP SqlMembers
                    }
                    else
                    {
                        #region LOOP SqlMembers
                        /*PUSH:Key*/
                        if (memberMap.IsMember(@MemberIndex)) this.@MemberName = (@MemberType.FullName)/*PUSH:Key*//*PUSH:Value.SqlMemberType*/(@FullName)/*PUSH:Value.SqlMemberType*/reader[++index];
                        #endregion LOOP SqlMembers
                    }
                }
                /// <summary>
                /// 是否通过SQL默认验证
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                /// <returns>是否通过SQL默认验证</returns>
                public bool IsSqlVerify(@type.FullName/**/.memberMap memberMap)
                {
                    #region LOOP VerifyMembers
                    if (memberMap.IsMember(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/))
                    {
                        #region IF Key.MemberType.IsString
                        #region IF Value.MaxLength
                        if (!SqlTool.StringVerify(/*PUSH:Key*/"@MemberName", /*NOTE*/(string)(object)/*NOTE*/@MemberName/*PUSH:Key*/, @Value.MaxLength, @Value.IsAscii, @Value.IsNull))
                        {
                            return false;
                        }
                        #endregion IF Value.MaxLength
                        #endregion IF Key.MemberType.IsString

                        #region NOT Key.MemberType.IsString
                        #region NOT Value.IsNull
                        #region PUSH Key
                        if (@MemberName == null)
                        {
                            SqlTool.NullVerify("@MemberName");
                            return false;
                        }
                        #endregion PUSH Key
                        #endregion NOT Value.IsNull
                        #endregion NOT Key.MemberType.IsString
                    }
                    #endregion LOOP VerifyMembers
                    return true;
                }
                /// <summary>
                /// 添加到数据库
                /// </summary>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <param name="memberMap">成员位图</param>
                /// <returns>添加是否成功</returns>
                public bool SqlInsert(bool isIgnoreTransaction = false, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
                {
                    return SqlTool.Client.Insert(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }
                #region IF IsSqlKey
                /// <summary>
                /// SQL表达式
                /// </summary>
                public class sqlExpression : fastCSharp.setup.cSharp.sqlTable.sqlExpressionMember<@type.FullName/**/.memberMap>, fastCSharp.setup.cSharp.sqlTable.ISqlExpression<valueType, @type.FullName/**/.memberMap>
                {
                    #region LOOP SqlMembers
                    private System.Linq.Expressions.Expression<func<valueType, @SqlExpressionType.FullName>> @ExpressionMemberName;
                    public System.Linq.Expressions.Expression<func<valueType, /*PUSH:Key*/@MemberType.FullName/*PUSH:Key*/>> /*PUSH:Key*/@MemberName/*PUSH:Key*/
                    {
                        set
                        {
                            if (value != null)
                            {
                                @ExpressionMemberName = SqlTool.GetExpression</*PUSH:Key*/@MemberType.FullName/*PUSH:Key*//*PUSH:Value.SqlMemberType*/, @FullName/*PUSH:Value.SqlMemberType*/>(value);
                                memberMap.SetMember(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/);
                            }
                            else
                            {
                                @ExpressionMemberName = null;
                                memberMap.ClearMember(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/);
                            }
                        }
                    }
                    public /*PUSH:Key*/@MemberType.FullName/*PUSH:Key*/ @ExpressionMemberValueName
                    {
                        set
                        {
                            @ExpressionMemberName = _value_ => (/*PUSH:Value.SqlMemberType*/(@FullName)/*PUSH:Value.SqlMemberType*/value);
                            memberMap.SetMember(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/);
                        }
                    }
                    #endregion LOOP SqlMembers
                    /// <summary>
                    /// 获取SQL表达式集合
                    /// </summary>
                    /// <param name="memberMap">成员位图</param>
                    /// <returns>SQL表达式集合</returns>
                    public list<keyValue<string, string>> Get(@type.FullName/**/.memberMap memberMap)
                    {
                        fastCSharp.setup.cSharp.IMemberInfo memberInfo = SqlTool.MemberInfo;
                        fastCSharp.sql.client client = SqlTool.Client;
                        list<keyValue<string, string>>.unsafer values = new list<keyValue<string, string>>(memberInfo.MemberCount).Unsafer;
                        #region LOOP SqlMembers
                        if (memberMap.IsMember(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/)) values.Add(new keyValue<string, string>(memberInfo.GetName(/*PUSH:Key*/@MemberIndex/*PUSH:Key*/), client.GetSql(@ExpressionMemberName).Key));
                        #endregion LOOP SqlMembers
                        return values.List;
                    }
                }
                #endregion IF IsSqlKey
                #region IF Identity
                /// <summary>
                /// 32位自增列
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public int SqlIdentity32
                {
                    get { return (int)/*NOTE*/(object)/*NOTE*/@Identity.MemberName; }
                }
                /// <summary>
                /// 64位自增列
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public long SqlIdentity64
                {
                    get { return (long)/*NOTE*/(object)/*NOTE*/@Identity.MemberName; }
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>是否修改成功</returns>
                public bool SqlUpdateByIdentity(@type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap), bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.UpdateByIdentity(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }
                /// <summary>
                /// 删除数据库记录
                /// </summary>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>是否成功</returns>
                public bool SqlDeleteByIdentity(bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.DeleteByIdentity(SqlTool, (valueType)this, isIgnoreTransaction);
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name="/*PUSH:Identity*/@MemberName/*PUSH:Identity*/">自增id</param>
                /// <param name="sqlExpression">SQL表达式</param>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>修改后的数据,失败返回null</returns>
                public static valueType SqlUpdateByIdentity(@Identity.MemberType.FullName /*PUSH:Identity*/@MemberName/*PUSH:Identity*/, sqlExpression sqlExpression, bool isIgnoreTransaction = false)
                {
                    if (sqlExpression != null)
                    {
                        valueType value = fastCSharp.setup.constructor<valueType>.New;
                        #region PUSH Identity
                        value.@MemberName = @MemberName;
                        #endregion PUSH Identity
                        if (SqlTool.Client.UpdateByIdentity(SqlTool, value, sqlExpression, isIgnoreTransaction)) return value;
                    }
                    return null;
                }
                /// <summary>
                /// 获取数据库记录
                /// </summary>
                /// <param name="/*PUSH:Identity*/@MemberName/*PUSH:Identity*/">自增id</param>
                /// <param name="memberMap">成员位图</param>
                /// <returns>数据对象</returns>
                public static valueType SqlGetByIdentity(@Identity.MemberType.FullName /*PUSH:Identity*/@MemberName/*PUSH:Identity*/, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
                {
                    valueType value = fastCSharp.setup.constructor<valueType>.New;
                    #region PUSH Identity
                    value.@MemberName = @MemberName;
                    #endregion PUSH Identity
                    return SqlTool.GetByIdentity(value, memberMap);
                }
                #endregion IF Identity
                #region IF PrimaryKey
                /// <summary>
                /// SQL关键字
                /// </summary>
                [fastCSharp.setup.sqlMember(IsIgnore = true)]
                public sqlPrimaryKey SqlPrimaryKey
                {
                    get
                    {
                        sqlPrimaryKey value = new sqlPrimaryKey();
                        #region LOOP PrimaryKey
                        value.@MemberName = @MemberName;
                        #endregion LOOP PrimaryKey
                        return value;
                    }
                }
                /// <summary>
                ///  关键字转换数据容器
                /// </summary>
                /// <param name="sqlPrimaryKey">SQL关键字</param>
                /// <returns>数据对象</returns>
                private static valueType getBySqlPrimaryKey(sqlPrimaryKey primaryKey)
                {
                    valueType value = fastCSharp.setup.constructor<valueType>.New;
                    #region LOOP PrimaryKey
                    value.@MemberName = primaryKey.@MemberName;
                    #endregion LOOP PrimaryKey
                    return value;
                }
                /// <summary>
                /// 获取数据库记录
                /// </summary>
                /// <param name="primaryKey">SQL关键字</param>
                /// <param name="memberMap">成员位图</param>
                /// <returns>数据对象</returns>
                public static valueType SqlGetPrimaryKey(sqlPrimaryKey primaryKey, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
                {
                    return SqlTool.GetByPrimaryKey(getBySqlPrimaryKey(primaryKey), memberMap);
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name="primaryKey">SQL关键字</param>
                /// <param name="sqlExpression">SQL表达式</param>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>修改后的数据,失败返回null</returns>
                public static valueType SqlUpdate(sqlPrimaryKey primaryKey, sqlExpression sqlExpression, bool isIgnoreTransaction = false)
                {
                    if (sqlExpression != null)
                    {
                        valueType value = getBySqlPrimaryKey(primaryKey);
                        if (SqlTool.Client.Update(SqlTool, value, sqlExpression, isIgnoreTransaction)) return value;
                    }
                    return null;
                }
                /// <summary>
                /// 修改数据库记录
                /// </summary>
                /// <param name="memberMap">成员位图</param>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>是否修改成功</returns>
                public bool SqlUpdate(@type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap), bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.Update(SqlTool, (valueType)this, memberMap, isIgnoreTransaction);
                }
                /// <summary>
                /// 删除数据库记录
                /// </summary>
                /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
                /// <returns>是否成功</returns>
                public bool SqlDelete(bool isIgnoreTransaction = false)
                {
                    return SqlTool.Client.Delete(SqlTool, (valueType)this, isIgnoreTransaction);
                }
                #endregion IF PrimaryKey
                static sqlTable()
                {
                    func<valueType, object>[] getMembers = new func<valueType, object>[(/*NOTE*/(IMemberInfo)(object)/*NOTE*/new memberInfo()).MemberCount];
                    #region LOOP SqlMembers
                    getMembers[/*PUSH:Key*/@MemberIndex/*PUSH:Key*/] = value => (object)/*PUSH:Value.SqlMemberType*/(@FullName)/*PUSH:Value.SqlMemberType*//*PUSH:Key*/value.@MemberName/*PUSH:Key*/;
                    #endregion LOOP SqlMembers
                    #region IF PrimaryKey
                    @type.FullName/**/.memberMap sqlPrimaryKeyMemberMap = new @type.FullName/**/.memberMap();
                    #region LOOP PrimaryKey
                    sqlPrimaryKeyMemberMap.SetMember(@MemberIndex);
                    #endregion LOOP PrimaryKey
                    #endregion IF PrimaryKey
                    SqlTool = fastCSharp.setup.cSharp.sqlTable.sqlTool<valueType, @type.FullName/**/.memberMap>.Get(/*NOTE*/(IMemberInfo)(object)/*NOTE*/new memberInfo(), getMembers, @Attribute.IsLockWrite/*PUSH:Identity*/, @MemberIndex/*PUSH:Identity*//*IF:PrimaryKey*/, sqlPrimaryKeyMemberMap/*IF:PrimaryKey*/);
                }
            }
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class pub
    {
        /// <summary>
        /// 自增列类型
        /// </summary>
        public class Identity : pub { }
        /// <summary>
        /// SQL表达式类型
        /// </summary>
        public class SqlExpressionType : pub { }
    }
    #endregion NOTE
}
