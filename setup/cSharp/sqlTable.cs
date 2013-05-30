#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using System.Collections.Generic;
using fastCSharp.config;
using fastCSharp.sql;
using fastCSharp.reflection;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Threading;
using System.Reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// SQL表格
    /// </summary>
    public partial class sqlTable : memberFilter.publicInstance
    {
        /// <summary>
        /// 默认SQL表格配置
        /// </summary>
        private static readonly sqlTable nullSqlTable = new sqlTable();
        /// <summary>
        /// 空自增列成员索引
        /// </summary>
        internal const int NullIdentityMemberIndex = -1;
        /// <summary>
        /// SQL操作接口
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface ISql<valueType, memberType> where memberType : IMemberMap
        {
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="reader">字段读取器</param>
            /// <param name="memberMap">成员位图</param>
            void Set(DbDataReader reader, memberType memberMap = default(memberType));
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <param name="memberMap">数据成员位图</param>
            /// <returns>是否插入成功</returns>
            bool SqlInsert(bool isIgnoreTransaction = false, memberType memberMap = default(memberType));
            /// <summary>
            /// 是否通过SQL默认验证
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>是否通过SQL默认验证</returns>
            bool IsSqlVerify(memberType memberMap);
        }
        /// <summary>
        /// 获取SQL表格定义类型
        /// </summary>
        /// <param name="type">SQL表格绑定类型</param>
        /// <returns>SQL表格定义类型,失败返回null</returns>
        internal static Type GetSqlType(Type type)
        {
            Type sqlType = type.getGenericInterface(typeof(ISql<,>));
            if (sqlType != null && sqlType.GetGenericArguments()[0] == type)
            {
                while ((type = type.BaseType).isInterface(sqlType)) ;
                return type;
            }
            return null;
        }
        /// <summary>
        /// 自增表格SQL操作接口
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <typeparam name="identityType">自增列类型</typeparam>
        public interface ISqlIdentity<valueType, memberType, identityType> : ISql<valueType, memberType> where memberType : IMemberMap
        {
            /// <summary>
            /// 32位自增列
            /// </summary>
            int SqlIdentity32 { get; }
            /// <summary>
            /// 64位自增列
            /// </summary>
            long SqlIdentity64 { get; }
            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="memberMap">数据成员位图</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否更新成功</returns>
            bool SqlUpdateByIdentity(memberType memberMap = default(memberType), bool isIgnoreTransaction = false);
            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否删除成功</returns>
            bool SqlDeleteByIdentity(bool isIgnoreTransaction = false);
        }
        /// <summary>
        /// 关键字SQL操作接口
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        /// <typeparam name="keyType">SQL关键字类型</typeparam>
        public interface ISqlKey<valueType, memberType, keyType> : ISql<valueType, memberType>
            where memberType : IMemberMap
            where keyType : IEquatable<keyType>
        {
            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="memberMap">数据成员位图</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否更新成功</returns>
            bool SqlUpdate(memberType memberMap = default(memberType), bool isIgnoreTransaction = false);
            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否删除成功</returns>
            bool SqlDelete(bool isIgnoreTransaction = false);
            /// <summary>
            /// SQL关键字
            /// </summary>
            keyType SqlPrimaryKey { get; }
        }
        /// <summary>
        /// SQL表达式接口
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public interface ISqlExpression<valueType, memberType> where memberType : IMemberMap
        {
            /// <summary>
            /// SQL表达式成员位图
            /// </summary>
            memberType MemberMap { get; }
            /// <summary>
            /// 获取SQL表达式集合
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>SQL表达式集合</returns>
            list<keyValue<string, string>> Get(memberType memberMap);
        }
        /// <summary>
        /// SQL表达式
        /// </summary>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public abstract class sqlExpressionMember<memberType> where memberType : IMemberMap
        {
            /// <summary>
            /// SQL表达式成员位图
            /// </summary>
            protected memberType memberMap;
            /// <summary>
            /// SQL表达式成员位图
            /// </summary>
            public memberType MemberMap
            {
                get { return memberMap; }
            }
        }
        /// <summary>
        /// 取消确认
        /// </summary>
        public class cancel
        {
            /// <summary>
            /// 是否取消
            /// </summary>
            private bool isCancel;
            /// <summary>
            /// 是否取消
            /// </summary>
            public bool IsCancel
            {
                get { return isCancel; }
                set { if (value) isCancel = true; }
            }
        }
        /// <summary>
        /// SQL操作工具类
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public abstract class sqlToolBase<valueType, memberType>
            where valueType : class
            where memberType : IMemberMap<memberType>
        {
            /// <summary>
            /// SQL操作客户端
            /// </summary>
            public client Client { get; protected set; }
            /// <summary>
            /// 表格名称
            /// </summary>
            protected internal string TableName { get; protected set; }
            /// <summary>
            /// SQL数据成员
            /// </summary>
            public memberType MemberMap { get; protected set; }
            /// <summary>
            /// 更新查询SQL数据成员
            /// </summary>
            protected memberType selectMemberMap;
            /// <summary>
            /// 更新查询SQL数据成员
            /// </summary>
            internal memberType SelectMemberMap
            {
                get
                {
                    return selectMemberMap;
                }
            }
            /// <summary>
            /// 成员信息获取器
            /// </summary>
            public IMemberInfo MemberInfo { get; protected set; }
            /// <summary>
            /// 获取SQL成员值的委托
            /// </summary>
            protected func<valueType, object>[] getSqlMembers;
            /// <summary>
            /// 自增列成员索引
            /// </summary>
            public int IdentityMemberIndex { get; protected set; }
            /// <summary>
            /// 关键字成员位图
            /// </summary>
            protected internal memberType PrimaryKeyMemberMap { get; protected set; }
            /// <summary>
            /// 关键字成员索引集合
            /// </summary>
            protected int[] primaryKeyMemberIndexs;
            /// <summary>
            /// 关键字数量
            /// </summary>
            internal int PrimaryKeyCount
            {
                get { return primaryKeyMemberIndexs.length(); }
            }
            /// <summary>
            /// 是否存在关键字
            /// </summary>
            internal bool IsPrimaryKey
            {
                get
                {
                    return !PrimaryKeyMemberMap.IsDefault;
                }
            }
            /// <summary>
            /// 第一个关键字名称
            /// </summary>
            internal string FirstPrimaryKeyName
            {
                get
                {
                    return primaryKeyMemberIndexs != null ? MemberInfo.GetName(primaryKeyMemberIndexs[0]) : null;
                }
            }
            /// <summary>
            /// 待创建一级索引的成员名称集合
            /// </summary>
            protected staticHashSet<string> noIndexMemberNames;
            /// <summary>
            /// 数据库表格是否加载成功
            /// </summary>
            public bool IsTable { get; protected set; }
            /// <summary>
            /// 是否锁定更新操作
            /// </summary>
            protected internal bool IsLockWrite { get; protected set; }
            /// <summary>
            /// SQL访问锁
            /// </summary>
            public readonly object Lock = new object();
            /// <summary>
            /// SQL操作工具类
            /// </summary>
            /// <param name="client">SQL操作客户端</param>
            /// <param name="tableName">表格名称</param>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <param name="identityMemberIndex">自增列成员索引</param>
            /// <param name="primaryKeyMemberMap">关键字成员位图</param>
            protected sqlToolBase(client client, string tableName, IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite
                , int identityMemberIndex, memberType primaryKeyMemberMap)
            {
                Client = client;
                TableName = tableName;
                MemberInfo = memberInfo;
                this.getSqlMembers = getSqlMembers;
                memberType memberMap = default(memberType);
                int memberIndex = 0;
                foreach (func<valueType, object> getSqlMember in getSqlMembers)
                {
                    if (getSqlMember != null) memberMap.SetMember(memberIndex);
                    ++memberIndex;
                }
                if (memberMap.IsDefault) log.Default.Throw(log.exceptionType.Null);
                MemberMap = memberMap;
                IsLockWrite = isLockWrite;
                try
                {
                    table table = client.GetTable(tableName);
                    noIndexMemberNames = new staticHashSet<string>(table.Columns.Columns.getArray(value => value.Name));
                    if (table.Indexs != null)
                    {
                        foreach (columnCollection column in table.Indexs) noIndexMemberNames.Remove(column.Columns[0].Name);
                    }
                    if (table.PrimaryKey != null) noIndexMemberNames.Remove(table.PrimaryKey.Columns[0].Name);
                    IsTable = true;
                }
                catch (Exception error)
                {
                    log.Default.Add(error, tableName, false);
                }
                IdentityMemberIndex = identityMemberIndex;
                if (identityMemberIndex != NullIdentityMemberIndex)
                {
                    createIndex(memberInfo.GetName(identityMemberIndex), true);
                    selectMemberMap.SetMember(identityMemberIndex);
                }
                PrimaryKeyMemberMap = primaryKeyMemberMap;
                if (IsPrimaryKey)
                {
                    list<int>.unsafer primaryKeyMemberIndexs = new list<int>(memberInfo.MemberCount).Unsafer;
                    for (int index = 0, count = memberInfo.MemberCount; index != count; ++index)
                    {
                        if (primaryKeyMemberMap.IsMember(index)) primaryKeyMemberIndexs.Add(index);
                    }
                    this.primaryKeyMemberIndexs = primaryKeyMemberIndexs.List.GetArray();
                    selectMemberMap.Or(primaryKeyMemberMap);
                }
                if (selectMemberMap.IsDefault) selectMemberMap = memberMap;
            }
            /// <summary>
            /// 创建索引
            /// </summary>
            /// <param name="name">列名称</param>
            internal void CreateIndex(string name)
            {
                createIndex(name, false);
            }
            /// <summary>
            /// 创建索引
            /// </summary>
            /// <param name="name">列名称</param>
            /// <param name="isUnique">是否唯一值</param>
            private void createIndex(string name, bool isUnique)
            {
                if (noIndexMemberNames.Contains(name))
                {
                    bool isIndex = false;
                    Exception exception = null;
                    Monitor.Enter(Lock);
                    try
                    {
                        if (noIndexMemberNames.Remove(name))
                        {
                            isIndex = true;
                            if (Client.CreateIndex(TableName, new columnCollection
                            {
                                Columns = new column[] { new column { Name = name } },
                                Type = isUnique ? columnCollection.type.UniqueIndex : columnCollection.type.Index
                            }))
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        exception = error;
                    }
                    finally { Monitor.Exit(Lock); }
                    if (isIndex) log.Default.Add(exception, "索引 " + TableName + "." + name + " 创建失败", false);
                }
            }
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnInsert;
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            /// <param name="value">待插入数据</param>
            /// <returns>是否可插入数据库</returns>
            public bool CallOnInsert(valueType value)
            {
                if (OnInsert != null)
                {
                    cancel cancel = new cancel();
                    OnInsert(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            /// <param name="values">待插入数据集合</param>
            /// <returns>是否可插入数据库</returns>
            public bool CallOnInsert(valueType[] values)
            {
                if (OnInsert != null)
                {
                    cancel cancel = new cancel();
                    foreach (valueType value in values)
                    {
                        OnInsert(value, cancel);
                        if (cancel.IsCancel) return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnInsertLock;
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            /// <param name="value">待插入数据</param>
            /// <returns>是否可插入数据库</returns>
            public bool CallOnInsertLock(valueType value)
            {
                if (OnInsertLock != null)
                {
                    cancel cancel = new cancel();
                    OnInsertLock(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 添加数据之前的验证事件
            /// </summary>
            /// <param name="values">待插入数据集合</param>
            /// <returns>是否可插入数据库</returns>
            public bool CallOnInsertLock(valueType[] values)
            {
                if (OnInsertLock != null)
                {
                    cancel cancel = new cancel();
                    foreach (valueType value in values)
                    {
                        OnInsertLock(value, cancel);
                        if (cancel.IsCancel) return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 添加数据之后的处理事件
            /// </summary>
            public event action<valueType> OnInsertedLock;
            /// <summary>
            /// 添加数据之后的处理事件
            /// </summary>
            /// <param name="value">被插入的数据</param>
            public void CallOnInsertedLock(valueType value)
            {
                if (OnInsertedLock != null) OnInsertedLock(value);
            }
            /// <summary>
            /// 添加数据之后的处理事件
            /// </summary>
            public event action<valueType> OnInserted;
            /// <summary>
            /// 添加数据之后的处理事件
            /// </summary>
            /// <param name="value">被插入的数据</param>
            public void CallOnInserted(valueType value)
            {
                if (OnInserted != null) OnInserted(value);
            }
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            public event action<valueType, memberType, cancel> OnUpdateByIdentity;
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            /// <param name="value">待更新数据</param>
            /// <param name="memberMap">更新成员位图</param>
            /// <returns>是否可更新数据库</returns>
            public bool CallOnUpdateByIdentity(valueType value, memberType memberMap)
            {
                if (OnUpdateByIdentity != null)
                {
                    cancel cancel = new cancel();
                    OnUpdateByIdentity(value, memberMap, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            public event action<valueType, memberType, cancel> OnUpdateByIdentityLock;
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            /// <param name="value">待更新数据</param>
            /// <param name="memberMap">更新成员位图</param>
            /// <returns>是否可更新数据库</returns>
            public bool CallOnUpdateByIdentityLock(valueType value, memberType memberMap)
            {
                if (OnUpdateByIdentityLock != null)
                {
                    cancel cancel = new cancel();
                    OnUpdateByIdentityLock(value, memberMap, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            public event action<valueType, memberType, cancel> OnUpdate;
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            /// <param name="value">待更新数据</param>
            /// <param name="memberMap">更新成员位图</param>
            /// <returns>是否可更新数据库</returns>
            public bool CallOnUpdate(valueType value, memberType memberMap)
            {
                if (OnUpdate != null)
                {
                    cancel cancel = new cancel();
                    OnUpdate(value, memberMap, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            public event action<valueType, memberType, cancel> OnUpdateLock;
            /// <summary>
            /// 更新数据之前的验证事件
            /// </summary>
            /// <param name="value">待更新数据</param>
            /// <param name="memberMap">更新成员位图</param>
            /// <returns>是否可更新数据库</returns>
            public bool CallOnUpdateLock(valueType value, memberType memberMap)
            {
                if (OnUpdateLock != null)
                {
                    cancel cancel = new cancel();
                    OnUpdateLock(value, memberMap, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 更新数据之后的处理事件
            /// </summary>
            public event action<valueType, valueType, memberType> OnUpdatedLock;
            /// <summary>
            /// 更新数据之后的处理事件
            /// </summary>
            /// <param name="value">更新后的数据</param>
            /// <param name="oldValue">更新前的数据</param>
            /// <param name="memberMap">更新成员位图</param>
            public void CallOnUpdatedLock(valueType value, valueType oldValue, memberType memberMap)
            {
                if (OnUpdatedLock != null) OnUpdatedLock(value, oldValue, memberMap);
            }
            /// <summary>
            /// 更新数据之后的处理事件
            /// </summary>
            public event action<valueType, valueType, memberType> OnUpdated;
            /// <summary>
            /// 更新数据之后的处理事件
            /// </summary>
            /// <param name="value">更新后的数据</param>
            /// <param name="oldValue">更新前的数据</param>
            /// <param name="memberMap">更新成员位图</param>
            public void CallOnUpdated(valueType value, valueType oldValue, memberType memberMap)
            {
                if (OnUpdated != null) OnUpdated(value, oldValue, memberMap);
            }
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnDeleteByIdentity;
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <returns>是否可删除数据</returns>
            public bool CallOnDeleteByIdentity(valueType value)
            {
                if (OnDeleteByIdentity != null)
                {
                    cancel cancel = new cancel();
                    OnDeleteByIdentity(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnDeleteByIdentityLock;
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <returns>是否可删除数据</returns>
            public bool CallOnDeleteByIdentityLock(valueType value)
            {
                if (OnDeleteByIdentityLock != null)
                {
                    cancel cancel = new cancel();
                    OnDeleteByIdentityLock(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnDelete;
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <returns>是否可删除数据</returns>
            public bool CallOnDelete(valueType value)
            {
                if (OnDelete != null)
                {
                    cancel cancel = new cancel();
                    OnDelete(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            public event action<valueType, cancel> OnDeleteLock;
            /// <summary>
            /// 删除数据之前的验证事件
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <returns>是否可删除数据</returns>
            public bool CallOnDeleteLock(valueType value)
            {
                if (OnDeleteLock != null)
                {
                    cancel cancel = new cancel();
                    OnDeleteLock(value, cancel);
                    return !cancel.IsCancel;
                }
                return true;
            }
            /// <summary>
            /// 删除数据之后的处理事件
            /// </summary>
            public event action<valueType> OnDeletedLock;
            /// <summary>
            /// 删除数据之后的处理事件
            /// </summary>
            /// <param name="value">被删除的数据</param>
            public void CallOnDeletedLock(valueType value)
            {
                if (OnDeletedLock != null) OnDeletedLock(value);
            }
            /// <summary>
            /// 删除数据之后的处理事件
            /// </summary>
            public event action<valueType> OnDeleted;
            /// <summary>
            /// 删除数据之后的处理事件
            /// </summary>
            /// <param name="value">被删除的数据</param>
            public void CallOnDeleted(valueType value)
            {
                if (OnDeleted != null) OnDeleted(value);
            }
            /// <summary>
            /// 添加数据是否启用应用程序事务
            /// </summary>
            internal bool IsInsertTransaction
            {
                get
                {
                    return OnInsertedLock != null || OnInserted != null;
                }
            }
            /// <summary>
            /// 添加数据是否启用应用程序事务
            /// </summary>
            internal bool IsUpdateTransaction
            {
                get
                {
                    return OnUpdatedLock != null || OnUpdated != null;
                }
            }
            /// <summary>
            /// 删除数据是否启用应用程序事务
            /// </summary>
            internal bool IsDeleteTransaction
            {
                get
                {
                    return OnDeletedLock != null || OnDeleted != null;
                }
            }
            /// <summary>
            /// 获取关键字名称与数据值集合
            /// </summary>
            /// <param name="value">对象数据</param>
            /// <returns>关键字名称与数据值集合</returns>
            internal keyValue<string, object>[] GetPrimaryKey(valueType value)
            {
                return primaryKeyMemberIndexs.getArray(index => new keyValue<string, object>(MemberInfo.GetName(index), getSqlMembers[index](value)));
            }
            /// <summary>
            /// 获取成员位图
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>成员位图</returns>
            internal memberType GetMemberMapClearIdentity(memberType memberMap)
            {
                memberType value = MemberMap.Copy();
                if (!memberMap.IsDefault) value.And(memberMap);
                if (IdentityMemberIndex != NullIdentityMemberIndex) value.ClearMember(IdentityMemberIndex);
                return value;
            }
            /// <summary>
            /// 设置更新查询SQL数据成员
            /// </summary>
            /// <param name="memberIndex">数据成员索引</param>
            public void SetSelectMember(int memberIndex)
            {
                selectMemberMap.SetMember(memberIndex);
            }
            /// <summary>
            /// 获取更新查询SQL数据成员
            /// </summary>
            /// <param name="memberMap">查询SQL数据成员</param>
            /// <returns>更新查询SQL数据成员</returns>
            internal memberType GetSelectMemberMap(memberType memberMap)
            {
                memberType selectMemberMap = this.selectMemberMap.Copy();
                selectMemberMap.Or(memberMap);
                return selectMemberMap;
            }
            /// <summary>
            /// 获取成员设置器
            /// </summary>
            /// <param name="value">对象数据</param>
            /// <returns>成员设置器</returns>
            internal memberMap.memberGetter<valueType> GetMemberGetter(valueType value)
            {
                return new memberMap.memberGetter<valueType>(value, getSqlMembers);
            }
            /// <summary>
            /// 数据集合转DataTable
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="values">数据集合</param>
            /// <returns>数据集合</returns>
            internal DataTable GetDataTable(valueType[] values)
            {
                DataTable dataTable = new DataTable(TableName);
                list<int>.unsafer memberIndexList = new list<int>(MemberInfo.MemberCount).Unsafer;
                for (int memberIndex = 0, memberCount = MemberInfo.MemberCount; memberIndex != memberCount; ++memberIndex)
                {
                    if (MemberMap.IsMember(memberIndex) && memberIndex != IdentityMemberIndex)
                    {
                        memberIndexList.Add(memberIndex);
                        dataTable.Columns.Add(new DataColumn(MemberInfo.GetName(memberIndex), MemberInfo.GetType(memberIndex)));
                    }
                }
                int[] memberIndexs = memberIndexList.List.ToArray();
                foreach (valueType value in values)
                {
                    object[] memberValues = new object[memberIndexs.Length];
                    for (int index = 0; index != memberValues.Length; ++index) memberValues[index] = getSqlMembers[memberIndexs[index]](value);
                    dataTable.Rows.Add(memberValues);
                }
                return dataTable;
            }
            /// <summary>
            /// 字符串验证
            /// </summary>
            /// <param name="memberName">成员名称</param>
            /// <param name="value">成员值</param>
            /// <param name="length">最大长度</param>
            /// <param name="isAscii">是否ASCII</param>
            /// <param name="isNull">是否可以为null</param>
            /// <returns>字符串是否通过默认验证</returns>
            public unsafe bool StringVerify(string memberName, string value, int length, bool isAscii, bool isNull)
            {
                if (!isNull && value == null)
                {
                    NullVerify(memberName);
                    return false;
                }
                if (length != 0)
                {
                    if (isAscii)
                    {
                        int nextLength = length - value.Length;
                        if (nextLength >= 0 && value.length() > (length >> 1))
                        {
                            fixed (char* valueFixed = value)
                            {
                                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                                {
                                    if ((*start & 0xff00) != 0 && --nextLength < 0) break;
                                }
                            }
                        }
                        if (nextLength < 0)
                        {
                            log.Default.Add(TableName + "." + memberName + " 超长 " + length.toString(), true, true);
                            return false;
                        }
                    }
                    else
                    {
                        if (value.length() > length)
                        {
                            log.Default.Add(TableName + "." + memberName + " 超长 " + value.Length.toString() + " > " + length.toString(), true, false);
                            return false;
                        }
                    }
                }
                return true;
            }
            /// <summary>
            /// 成员值不能为null
            /// </summary>
            /// <param name="memberName">成员名称</param>
            public void NullVerify(string memberName)
            {
                log.Default.Add(TableName + "." + memberName + " 不能为null", true, true);
            }
            /// <summary>
            /// 是否通过SQL默认验证
            /// </summary>
            /// <param name="value">待验证数据</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>是否通过SQL默认验证</returns>
            public abstract bool IsSqlVerify(valueType value, memberType memberMap);
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="value">待设置数据</param>
            /// <param name="reader">字段读取器</param>
            /// <param name="memberMap">成员位图</param>
            public abstract void Set(valueType value, DbDataReader reader, memberType memberMap = default(memberType));
            /// <summary>
            /// 添加到数据库
            /// </summary>
            /// <param name="values">数据集合</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>添加是否成功</returns>
            public bool Insert(valueType[] values, bool isIgnoreTransaction = false)
            {
                if (values.length() != 0) Client.Insert(this, values, isIgnoreTransaction);
                return false;
            }
            /// <summary>
            /// SQL表达式转换
            /// </summary>
            /// <typeparam name="memberValueType">数据类型</typeparam>
            /// <param name="value">lambda表达式</param>
            /// <returns>类型转换表达式</returns>
            public Expression<func<valueType, memberValueType>> GetExpression<memberValueType>(Expression<func<valueType, memberValueType>> value)
            {
                return value;
            }
            /// <summary>
            /// 查询数据集合
            /// </summary>
            /// <param name="expression">查询条件表达式</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据集合</returns>
            public IEnumerable<valueType> Where(Expression<func<valueType, bool>> expression = null, memberType memberMap = default(memberType))
            {
                return Client.Select(this, (selectQuery<valueType>)expression, memberMap);
            }
            /// <summary>
            /// 查询数据集合
            /// </summary>
            /// <param name="query">查询信息</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据集合</returns>
            public IEnumerable<valueType> Where(selectQuery<valueType> query, memberType memberMap = default(memberType))
            {
                return Client.Select(this, query, memberMap);
            }
            /// <summary>
            /// 查询数据集合
            /// </summary>
            /// <param name="expression">查询条件表达式</param>
            /// <returns>数据集合,失败返回-1</returns>
            public int Count(Expression<func<valueType, bool>> expression = null)
            {
                return Client.Count(this, expression);
            }
            /// <summary>
            /// 根据关键字获取数据对象
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">关键字数据对象</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据对象</returns>
            public valueType GetByPrimaryKey(valueType value, memberType memberMap = default(memberType))
            {
                if (!IsPrimaryKey) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.GetByPrimaryKey(this, value, memberMap);
            }
            /// <summary>
            /// 根据自增id获取数据对象
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">关键字数据对象</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据对象</returns>
            public valueType GetByIdentity(valueType value, memberType memberMap = default(memberType))
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                return getByIdentity(value, memberMap);
            }
            /// <summary>
            /// 根据自增id获取数据对象
            /// </summary>
            /// <typeparam name="valueType">数据类型</typeparam>
            /// <param name="value">关键字数据对象</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据对象</returns>
            protected valueType getByIdentity(valueType value, memberType memberMap)
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.GetByIdentity(this, value, memberMap);
            }
            /// <summary>
            /// 添加到数据库
            /// </summary>
            /// <param name="value">待添加数据</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>添加是否成功</returns>
            public bool Insert(valueType value, bool isIgnoreTransaction = false, memberType memberMap = default(memberType))
            {
                return Client.Insert(this, value, memberMap, isIgnoreTransaction);
            }
            /// <summary>
            /// 修改数据库记录
            /// </summary>
            /// <param name="value">待修改数据</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否修改成功</returns>
            public bool UpdateByIdentity(valueType value, memberType memberMap = default(memberType), bool isIgnoreTransaction = false)
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.UpdateByIdentity(this, value, memberMap, isIgnoreTransaction);
            }
            /// <summary>
            /// 删除数据库记录
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否成功</returns>
            public bool DeleteByIdentity(valueType value, bool isIgnoreTransaction = false)
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.DeleteByIdentity(this, value, isIgnoreTransaction);
            }
            /// <summary>
            /// 修改数据库记录
            /// </summary>
            /// <param name="value">待修改数据</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否修改成功</returns>
            public bool UpdateByPrimaryKey(valueType value, memberType memberMap = default(memberType), bool isIgnoreTransaction = false)
            {
                if (!IsPrimaryKey) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.Update(this, value, memberMap, isIgnoreTransaction);
            }
            /// <summary>
            /// 删除数据库记录
            /// </summary>
            /// <param name="value">待删除数据</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>是否成功</returns>
            public bool DeleteByPrimaryKey(valueType value, bool isIgnoreTransaction = false)
            {
                if (!IsPrimaryKey) log.Default.Throw(log.exceptionType.ErrorOperation);
                return Client.Delete(this, value, isIgnoreTransaction);
            }
        }
        /// <summary>
        /// SQL操作工具类
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public class sqlTool<valueType, memberType> : sqlToolBase<valueType, memberType>
            where valueType : class, ISql<valueType, memberType>
            where memberType : IMemberMap<memberType>
        {
            /// <summary>
            /// SQL操作工具类
            /// </summary>
            /// <param name="client">SQL操作客户端</param>
            /// <param name="tableName">表格名称</param>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <param name="identityMemberIndex">自增列成员索引</param>
            /// <param name="primaryKeyMemberMap">关键字成员位图</param>
            private sqlTool(client client, string tableName, IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite
                , int identityMemberIndex, memberType primaryKeyMemberMap)
                : base(client, tableName, memberInfo, getSqlMembers, isLockWrite, identityMemberIndex, primaryKeyMemberMap)
            {
            }
            /// <summary>
            /// 是否通过SQL默认验证
            /// </summary>
            /// <param name="value">待验证数据</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>是否通过SQL默认验证</returns>
            public override bool IsSqlVerify(valueType value, memberType memberMap)
            {
                return value.IsSqlVerify(memberMap);
            }
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="value">待设置数据</param>
            /// <param name="reader">字段读取器</param>
            /// <param name="memberMap">成员位图</param>
            public override void Set(valueType value, System.Data.Common.DbDataReader reader, memberType memberMap = default(memberType))
            {
                value.Set(reader, memberMap);
            }
            /// <summary>
            /// SQL表达式转换
            /// </summary>
            /// <typeparam name="memberValueType">源数据类型</typeparam>
            /// <typeparam name="convertType">目标数据类型</typeparam>
            /// <param name="value">lambda表达式</param>
            /// <returns>类型转换表达式</returns>
            public Expression<func<valueType, convertType>> GetExpression<memberValueType, convertType>(Expression<func<valueType, memberValueType>> value)
            {
                return Expression.Lambda<func<valueType, convertType>>(Expression.Convert(value.Body, typeof(convertType)), value.Parameters
#if DOTNET35
                    .getArray()
#endif
                    );
            }
            /// <summary>
            /// 获取SQL操作工具类
            /// </summary>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <param name="identityMemberIndex">自增列成员索引</param>
            /// <param name="primaryKeyMemberMap">关键字成员位图</param>
            /// <returns>SQL操作工具类</returns>
            public static sqlTool<valueType, memberType> Get
                (IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite
                , int identityMemberIndex = NullIdentityMemberIndex, memberType primaryKeyMemberMap = default(memberType))
            {
                Type type = typeof(valueType);
                sqlTable sqlTable = type.customAttribute<sqlTable>(true, cSharp.Default.IsInheritAttribute);
                if (sqlTable != null && Array.IndexOf(fastCSharp.config.sql.Default.CheckConnection, sqlTable.ConnectionType) != -1)
                {
                    return new sqlTool<valueType, memberType>(connection.GetConnection(sqlTable.ConnectionType).Client, sqlTable.GetTableName(type), memberInfo, getSqlMembers, isLockWrite, identityMemberIndex, primaryKeyMemberMap);
                }
                return null;
            }
            /// <summary>
            /// 获取SQL操作工具类
            /// </summary>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <param name="primaryKeyMemberMap">关键字成员位图</param>
            /// <returns>SQL操作工具类</returns>
            public static sqlTool<valueType, memberType> Get(IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite
                , memberType primaryKeyMemberMap)
            {
                return Get(memberInfo, getSqlMembers, isLockWrite, NullIdentityMemberIndex, primaryKeyMemberMap);
            }
            /// <summary>
            /// 获取SQL操作工具类
            /// </summary>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <returns>SQL操作工具类</returns>
            public static sqlTool<valueType, memberType> Get(IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite)
            {
                return Get(memberInfo, getSqlMembers, isLockWrite, NullIdentityMemberIndex, default(memberType));
            }
        }
        /// <summary>
        /// SQL表达式(反射模式)
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        /// <typeparam name="memberType">成员位图类型</typeparam>
        public class sqlExpression<valueType> : sqlExpressionMember<memberMap<valueType>>, ISqlExpression<valueType, memberMap<valueType>>
             where valueType : class
        {
            /// <summary>
            /// 表达式集合
            /// </summary>
            private LambdaExpression[] expressions;
            /// <summary>
            /// SQL表达式
            /// </summary>
            private string[] sqls;
            /// <summary>
            /// 设置成员表达式
            /// </summary>
            /// <param name="returnType">表达式类型</param>
            /// <param name="expression">成员表达式</param>
            public void Set<returnType>(Expression<func<valueType, returnType>> expression)
            {
                if (expression != null)
                {
                    if (expressions == null)
                    {
                        expressions = new LambdaExpression[memberCount];
                        sqls = new string[memberCount];
                    }
                    set(expression);
                }
            }
            /// <summary>
            /// 设置成员表达式
            /// </summary>
            /// <param name="returnType">表达式类型</param>
            /// <param name="expression">成员表达式</param>
            private void set<returnType>(Expression<func<valueType, returnType>> expression)
            {
                keyValue<string, list<string>> sql = sqlTool.Client.GetSql(expression);
                if (sql.Value.count() == 0) log.Default.Throw(log.exceptionType.Null);
                int memberIndex = memberIndexs.Get(sql.Value.Unsafer.Array[0], -1);
                if (memberIndex == -1) log.Default.Throw(typeof(valueType).fullName() + " 找不到SQL字段 " + sql.Value.Unsafer.Array[0], false, true);
                expressions[memberIndex] = expression;
                sqls[memberIndex] = sql.Key;
                memberMap.SetMember(memberIndex);
            }
            /// <summary>
            /// 获取SQL表达式集合
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>SQL表达式集合</returns>
            public list<keyValue<string, string>> Get(memberMap<valueType> memberMap)
            {
                fastCSharp.setup.cSharp.IMemberInfo memberInfo = sqlTool.MemberInfo;
                fastCSharp.sql.client client = sqlTool.Client;
                list<keyValue<string, string>>.unsafer values = new list<keyValue<string, string>>(memberInfo.MemberCount).Unsafer;
                foreach (setup.memberInfo member in sqlTool<valueType>.members)
                {
                    int memberIndex = member.MemberIndex;
                    if (memberMap.IsMember(memberIndex) && expressions[memberIndex] != null)
                    {
                        values.Add(new keyValue<string, string>(memberInfo.GetName(memberIndex), sqls[memberIndex]));
                    }
                }
                return values.List;
            }
            /// <summary>
            /// SQL表达式
            /// </summary>
            /// <param name="returnType">表达式类型</param>
            /// <param name="expressions">成员表达式集合</param>
            /// <returns>SQL表达式</returns>
            public static sqlExpression<valueType> Create<returnType>(params Expression<func<valueType, returnType>>[] expressions)
            {
                sqlExpression<valueType> value = new sqlExpression<valueType>();
                if (expressions.length() != 0)
                {
                    bool isFrist = true;
                    foreach (Expression<func<valueType, returnType>> expression in expressions)
                    {
                        if (expression != null)
                        {
                            if (isFrist)
                            {
                                value.Set(expression);
                                isFrist = false;
                            }
                            else value.set(expression);
                        }
                    }
                }
                return value;
            }
            /// <summary>
            /// SQL操作工具类
            /// </summary>
            private static readonly sqlTool<valueType> sqlTool;
            /// <summary>
            /// 成员数量
            /// </summary>
            private static readonly int memberCount;
            /// <summary>
            /// 成员名称索引表
            /// </summary>
            private static readonly staticDictionary<string, int> memberIndexs;
            static sqlExpression()
            {
                sqlTool = sqlTool<valueType>.Default;
                if (sqlTool != null)
                {
                    memberCount = memberGroup<valueType>.MemberCount;
                    memberIndexs = new staticDictionary<string,int>(sqlTool<valueType>.members.getArray(value => new keyValue<string, int>(value.MemberName, value.MemberIndex)));
                }
            }
        }
        /// <summary>
        /// SQL操作工具类(反射模式)
        /// </summary>
        /// <typeparam name="valueType">表格绑定类型</typeparam>
        public class sqlTool<valueType> : sqlToolBase<valueType, memberMap<valueType>> where valueType : class
        {
            /// <summary>
            /// SQL操作工具类
            /// </summary>
            /// <param name="client">SQL操作客户端</param>
            /// <param name="tableName">表格名称</param>
            /// <param name="memberInfo">数据信息获取器</param>
            /// <param name="getSqlMembers">获取SQL成员值的委托</param>
            /// <param name="isLockWrite">是否锁定更新操作</param>
            /// <param name="identityMemberIndex">自增列成员索引</param>
            /// <param name="primaryKeyMemberMap">关键字成员位图</param>
            private sqlTool(client client, string tableName, IMemberInfo memberInfo, func<valueType, object>[] getSqlMembers, bool isLockWrite
                , int identityMemberIndex, memberMap<valueType> primaryKeyMemberMap)
                : base(client, tableName, memberInfo, getSqlMembers, isLockWrite, identityMemberIndex, primaryKeyMemberMap)
            {
            }
            /// <summary>
            /// 是否通过SQL默认验证
            /// </summary>
            /// <param name="value">待验证数据</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>是否通过SQL默认验证</returns>
            public override bool IsSqlVerify(valueType value, memberMap<valueType> memberMap)
            {
                list<keyValue<setup.memberInfo, object>> members = verifyMemberGroup.GetMemberValue(value, setup.memberFilter.Instance, memberMap);
                foreach (keyValue<setup.memberInfo, object> member in members)
                {
                    int memberIndex = member.Key.MemberIndex;
                    if (verifyStringMap.Get(memberIndex))
                    {
                        sqlMember sqlMember = sqlMembers[memberIndex];
                        func<object, object> converter = convertGetters[memberIndex];
                        if (!StringVerify(MemberInfo.GetName(memberIndex), (string)(converter == null ? member.Value : converter(member.Value)), sqlMember.MaxLength, sqlMember.IsAscii, sqlMember.IsNull))
                        {
                            return false;
                        }
                    }
                    else if (member.Value == null)
                    {
                        NullVerify(MemberInfo.GetName(memberIndex));
                        return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="value">待设置数据</param>
            /// <param name="reader">字段读取器</param>
            /// <param name="memberMap">成员位图</param>
            public override unsafe void Set(valueType value, System.Data.Common.DbDataReader reader, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                int index = -1;
                object[] values = new object[memberCount];
                object readerValue;
                byte* isValue = stackalloc byte[memberMapSize];
                fixedMap isValueMap = new fixedMap(isValue, memberMapSize);
                if (memberMap.IsDefault)
                {
                    foreach (setup.memberInfo member in members)
                    {
                        int memberIndex = member.MemberIndex;
                        if ((readerValue = reader[++index]) != DBNull.Value)
                        {
                            func<object, object> converter = convertSetters[memberIndex];
                            values[memberIndex] = converter == null ? readerValue : converter(readerValue);
                        }
                        else values[memberIndex] = defaultValues[memberIndex];
                        isValueMap.Set(memberIndex);
                    }
                }
                else
                {
                    foreach (setup.memberInfo member in members)
                    {
                        if (memberMap.IsMember(member.MemberIndex))
                        {
                            int memberIndex = member.MemberIndex;
                            if ((readerValue = reader[++index]) != DBNull.Value)
                            {
                                func<object, object> converter = convertSetters[memberIndex];
                                values[memberIndex] = converter == null ? readerValue : converter(readerValue);
                            }
                            else values[memberIndex] = defaultValues[memberIndex];
                            isValueMap.Set(memberIndex);
                        }
                    }
                }
                memberGroup.SetMember(value, values, isValueMap);
            }
            /// <summary>
            /// 获取自增ID
            /// </summary>
            /// <param name="value">目标数据</param>
            /// <returns>自增ID</returns>
            public int GetIdentity(valueType value)
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                return (int)getSqlMembers[IdentityMemberIndex](value);
            }
            /// <summary>
            /// 获取自增ID数据
            /// </summary>
            /// <param name="identity">自增id</param>
            /// <returns>自增ID数据</returns>
            private valueType getByIdentity(object identity)
            {
                if (IdentityMemberIndex == NullIdentityMemberIndex) log.Default.Throw(log.exceptionType.ErrorOperation);
                valueType value = fastCSharp.setup.constructor<valueType>.New;
                func<object, object> converter = convertSetters[IdentityMemberIndex];
                setIdentity(value, converter == null ? identity : converter(identity));
                return value;
            }
            /// <summary>
            /// 获取数据库记录
            /// </summary>
            /// <param name="identity">自增id</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>数据对象</returns>
            public unsafe valueType GetByIdentity(object identity, memberMap<valueType> memberMap = default(memberMap<valueType>))
            {
                return getByIdentity(getByIdentity(identity), memberMap);
            }
            /// <summary>
            /// 修改数据库记录
            /// </summary>
            /// <param name="identity">自增id</param>
            /// <param name="sqlExpression">SQL表达式</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>修改后的数据,失败返回null</returns>
            public valueType UpdateByIdentity(object identity, sqlExpression<valueType> sqlExpression, bool isIgnoreTransaction = false)
            {
                if (sqlExpression != null)
                {
                    valueType value = getByIdentity(identity);
                    if (Client.UpdateByIdentity(this, value, sqlExpression, isIgnoreTransaction)) return value;
                }
                return null;
            }
            /// <summary>
            /// 修改数据库记录
            /// </summary>
            /// <param name="primaryKey">SQL关键字</param>
            /// <param name="sqlExpression">SQL表达式</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            /// <returns>修改后的数据,失败返回null</returns>
            public valueType UpdateByPrimaryKey(valueType primaryKey, sqlExpression<valueType> sqlExpression, bool isIgnoreTransaction = false)
            {
                if (sqlExpression != null)
                {
                    if (Client.Update(this, primaryKey, sqlExpression, isIgnoreTransaction)) return primaryKey;
                }
                return null;
            }
            /// <summary>
            /// SQL操作工具类
            /// </summary>
            public static readonly sqlTool<valueType> Default;
            /// <summary>
            /// SQL成员信息
            /// </summary>
            private static readonly memberGroup<valueType> memberGroup;
            /// <summary>
            /// 成员数量
            /// </summary>
            private static readonly int memberCount;
            /// <summary>
            /// 成员位图字节长度
            /// </summary>
            private static readonly int memberMapSize;
            /// <summary>
            /// 成员集合
            /// </summary>
            internal static readonly setup.memberInfo[] members;
            /// <summary>
            /// SQL成员信息集合
            /// </summary>
            private static readonly sqlMember[] sqlMembers;
            /// <summary>
            /// 获取类型转换函数
            /// </summary>
            private static readonly func<object, object>[] convertGetters;
            /// <summary>
            /// 设置类型转换函数
            /// </summary>
            private static readonly func<object, object>[] convertSetters;
            /// <summary>
            /// 默认成员值集合
            /// </summary>
            private static readonly object[] defaultValues;
            /// <summary>
            /// 设置自增ID委托
            /// </summary>
            private static readonly action<valueType, object> setIdentity;
            /// <summary>
            /// 默认验证SQL成员信息
            /// </summary>
            private static readonly memberGroup<valueType> verifyMemberGroup;
            /// <summary>
            /// 验证SQL字符串成员位图
            /// </summary>
            private static readonly fixedMap verifyStringMap;
            /// <summary>
            /// SQL操作工具 连接实例集合
            /// </summary>
            private static Dictionary<string, sqlTool<valueType>> connections;
            /// <summary>
            /// SQL操作工具 连接实例 访问锁
            /// </summary>
            private static int connectionLock;
            /// <summary>
            /// SQL操作工具
            /// </summary>
            /// <param name="sqlConnection">数据库连接类型</param>
            /// <returns>SQL操作工具</returns>
            public static sqlTool<valueType> Get(string type)
            {
                if (Default != null)
                {
                    sqlTool<valueType> value;
                    if (!connections.TryGetValue(type, out value))
                    {
                        while (Interlocked.CompareExchange(ref connectionLock, 1, 0) != 0) Thread.Sleep(1);
                        try
                        {
                            if (!connections.TryGetValue(type, out value))
                            {
                                connections.Add(type, value = new sqlTool<valueType>(connection.GetConnection(type).Client, Default.TableName, Default.MemberInfo, Default.getSqlMembers, Default.IsLockWrite, Default.IdentityMemberIndex, Default.PrimaryKeyMemberMap));
                            }
                        }
                        finally { connectionLock = 0; }
                        return value;
                    }
                }
                return null;
            }
            static sqlTool()
            {
                Type type = typeof(valueType);
                if (type.IsClass)
                {
                    sqlTable sqlTable = type.customAttribute<sqlTable>(false, cSharp.Default.IsInheritAttribute);
                    if (sqlTable != null && sqlTable.IsSetup
                        && Array.IndexOf(fastCSharp.config.sql.Default.CheckConnection, sqlTable.ConnectionType) != -1)
                    {
                        memberGroup = memberGroup<valueType>.Create<sqlTable>(sqlTable.IsAttribute, sqlTable.IsBaseTypeAttribute, sqlTable.IsInheritAttribute, value => value.CanGet && value.CanSet, sqlTable.filter);
                        memberCount = memberGroup<valueType>.MemberCount;
                        memberMapSize = (memberCount + 7) >> 3;
                        members = memberGroup.Members;
                        memberMap<valueType> primaryKeyMemberMap = default(memberMap<valueType>);
                        IMemberInfo memberInfo = primaryKeyMemberMap.MemberInfo;
                        sqlMembers = memberGroup.SqlMembers;
                        verifyMemberGroup = memberGroup<valueType>.Create<sqlTable>(sqlTable.IsAttribute, sqlTable.IsBaseTypeAttribute, sqlTable.IsInheritAttribute, value => value.CanGet && value.CanSet && cSharp.IsMemberVerify(new keyValue<setup.memberInfo, sqlMember>(value, sqlMembers[value.MemberIndex])), sqlTable.filter);
                        verifyStringMap = new fixedMap(unmanaged.Get(memberMapSize));
                        foreach (setup.memberInfo member in members)
                        {
                            if (member.MemberType.IsString) verifyStringMap.Set(member.MemberIndex);
                        }
                        int identityMemberIndex = NullIdentityMemberIndex, identityNameIndex = NullIdentityMemberIndex, index = 0;
                        foreach (sqlMember member in sqlMembers)
                        {
                            if (member != null)
                            {
                                if (identityMemberIndex == NullIdentityMemberIndex)
                                {
                                    if (member.IsIdentity) identityMemberIndex = index;
                                    else if (memberInfo.GetName(index) == fastCSharp.config.sql.Default.DefaultIdentityName) identityNameIndex = index;
                                }
                                if (member.IsPrimaryKey)
                                {
                                    if (primaryKeyMemberMap.IsDefault) primaryKeyMemberMap = new memberMap<valueType>();
                                    primaryKeyMemberMap.SetMember(index);
                                }
                            }
                            ++index;
                        }
                        if (identityMemberIndex == NullIdentityMemberIndex) identityMemberIndex = identityNameIndex;
                        func<valueType, object>[] getSqlMembers = new func<valueType, object>[memberCount];
                        convertGetters = new func<object, object>[memberCount];
                        convertSetters = new func<object, object>[memberCount];
                        defaultValues = new object[memberCount];
                        foreach (setup.memberInfo member in members)
                        {
                            int memberIndex = member.MemberIndex;
                            sqlMember sqlMember = sqlMembers[memberIndex];
                            if (sqlMember == null || sqlMember.SqlMemberType == null)
                            {
                                getSqlMembers[memberIndex] = memberGroup<valueType>.GetValue(member);
                            }
                            else
                            {
                                getSqlMembers[memberIndex] = memberGroup<valueType>.GetConvertValue(member, sqlMember.SqlMemberType.Type);
                                convertGetters[memberIndex] = reflection.converter.Get(member.MemberType.Type, sqlMember.SqlMemberType.Type);
                                convertSetters[memberIndex] = reflection.converter.Get(sqlMember.SqlMemberType.Type, member.MemberType.Type);
                            }
                            defaultValues[memberIndex] = constructor.GetNull(member.MemberType);
                            if (memberIndex == identityMemberIndex) setIdentity = memberGroup<valueType>.SetValue(member);
                        }
                        connections = new Dictionary<string, sqlTool<valueType>>();
                        Default = new sqlTool<valueType>(connection.GetConnection(sqlTable.ConnectionType).Client, sqlTable.GetTableName(typeof(valueType)), memberInfo, getSqlMembers, sqlTable.IsLockWrite, identityMemberIndex, primaryKeyMemberMap);
                        connections.Add(sqlTable.ConnectionType, Default);
                    }
                }
            }
        }
        /// <summary>
        /// 链接类型
        /// </summary>
        public string ConnectionName;
        /// <summary>
        /// 链接类型
        /// </summary>
        public virtual string ConnectionType
        {
            get { return ConnectionType;}
        }
        /// <summary>
        /// 表格名称
        /// </summary>
        public string TableName;
        /// <summary>
        /// 写操作是否加锁
        /// </summary>
        public bool IsLockWrite = true;
        /// <summary>
        /// 是否反射模式
        /// </summary>
        public bool IsReflection;
        /// <summary>
        /// 获取表格名称
        /// </summary>
        /// <param name="type">表格绑定类型</param>
        /// <returns>表格名称</returns>
        private string GetTableName(Type type)
        {
            return TableName ?? type.Name;
        }
        /// <summary>
        /// SQL表格操作代码生成
        /// </summary
        [auto(Name = "SQL操作", DependType = typeof(coder.cSharper), IsAuto = true)]
        internal partial class cSharp : member<sqlTable>
        {
            /// <summary>
            /// 默认SQL表格操作代码生成
            /// </summary>
            public static readonly cSharp Default = new cSharp();
            /// <summary>
            /// SQL成员信息
            /// </summary>
            internal struct sqlMemberInfo
            {
                /// <summary>
                /// 成员信息
                /// </summary>
                public setup.memberInfo Key;
                /// <summary>
                /// SQL成员信息
                /// </summary>
                public sqlMember Value;
                /// <summary>
                /// 表达式成员名称
                /// </summary>
                public string ExpressionMemberName
                {
                    get
                    {
                        return "expression_" + Key.MemberName;
                    }
                }
                /// <summary>
                /// 表达式静态值成员名称
                /// </summary>
                public string ExpressionMemberValueName
                {
                    get
                    {
                        return Key.MemberName + "_value";
                    }
                }
                /// <summary>
                /// 表达式成员类型
                /// </summary>
                public memberType SqlExpressionType
                {
                    get
                    {
                        return Value.SqlMemberType ?? Key.MemberType;
                    }
                }
            }
            /// <summary>
            /// 自定义属性是否可继承
            /// </summary>
            public override bool IsInheritAttribute
            {
                get { return true; }
            }
            /// <summary>
            /// 表格信息
            /// </summary>
            public table Table;
            /// <summary>
            /// 自增列
            /// </summary>
            public setup.memberInfo Identity;
            /// <summary>
            /// 关键字
            /// </summary>
            public setup.memberInfo[] PrimaryKey;
            /// <summary>
            /// 是否有键值
            /// </summary>
            public bool IsSqlKey
            {
                get { return Identity != null || PrimaryKey != null; }
            }
            /// <summary>
            /// 是否单主键
            /// </summary>
            public bool IsSinglePrimaryKey
            {
                get { return PrimaryKey.length() == 1; }
            }
            /// <summary>
            /// SQL成员信息
            /// </summary>
            public sqlMemberInfo[] SqlMembers;
            /// <summary>
            /// 默认验证SQL成员信息
            /// </summary>
            public keyValue<setup.memberInfo, sqlMember>[] VerifyMembers;
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void NextCreate()
            {
                if (!Attribute.IsReflection)
                {
                    keyValue<setup.memberInfo, sqlMember>[] sqlMembers = GetMembers(type, Attribute);
                    SqlMembers = sqlMembers.getArray(value => new sqlMemberInfo { Key = value.Key, Value = value.Value });
                    Members = sqlMembers.getArray(value => value.Key);
                    Table = GetTable(type, Attribute, sqlMembers);
                    Identity = Table.Identity != null ? Members.firstOrDefault(value => value.MemberName == Table.Identity.Name) : null;
                    PrimaryKey = Table.PrimaryKey != null ? Members.getFindArray(value => Table.PrimaryKey.Columns.any(column => column.Name == value.MemberName)) : null;
                    VerifyMembers = sqlMembers.getFindArray(value => IsMemberVerify(value));
                    memberMap.create(type);
                    copy.create(type);
                    create(true);
                }
            }
            /// <summary>
            /// 是否需要默认验证
            /// </summary>
            /// <param name="member">成员信息</param>
            /// <returns>是否需要默认验证</returns>
            public static bool IsMemberVerify(keyValue<setup.memberInfo, sqlMember> member)
            {
                if (member.Key.MemberType.IsNull)
                {
                    if (!member.Value.IsNull) return true;
                    if (member.Key.MemberType.IsString && member.Value.MaxLength != 0) return true;
                }
                return false;
            }
            /// <summary>
            /// 获取SQL成员信息集合
            /// </summary>
            /// <param name="type">SQL绑定类型</param>
            /// <param name="sqlTable">SQL表格信息</param>
            /// <returns>SQL成员信息集合</returns>
            public static keyValue<setup.memberInfo, sqlMember>[] GetMembers(Type type, sqlTable sqlTable)
            {
                return setup.memberInfo.GetMembers<sqlMember>(type, sqlTable.filter, sqlTable.IsAttribute, sqlTable.IsBaseTypeAttribute, sqlTable.IsInheritAttribute)
                    .getFind(value => value.CanSet && value.CanGet)
                    .getArray(value => new keyValue<setup.memberInfo, sqlMember>(value, sqlMember.Get(value)));
            }
            /// <summary>
            /// 获取表格信息
            /// </summary>
            /// <param name="type">SQL绑定类型</param>
            /// <param name="sqlTable">SQL表格信息</param>
            /// <param name="sqlMembers">SQL成员信息集合</param>
            /// <returns>表格信息</returns>
            public static table GetTable(Type type, sqlTable sqlTable, keyValue<setup.memberInfo, sqlMember>[] sqlMembers)
            {
                client client = connection.GetConnection(sqlTable.ConnectionType).Client;
                table table = new table
                {
                    Columns = new columnCollection
                    {
                        Name = sqlTable.GetTableName(type),
                        Columns = sqlMembers.getArray(value => client.GetColumn(value.Key, value.Value))
                    }
                };
                setup.memberInfo identity = sqlMembers.firstOrDefault(value => value.Value.IsIdentity).Key;
                string identityName = identity != null ? identity.MemberName : fastCSharp.config.sql.Default.DefaultIdentityName;
                table.Identity = table.Columns.Columns.firstOrDefault(value => value.Name == identityName);
                string[] primaryKeys = sqlMembers.getFind(value => value.Value.IsPrimaryKey).getArray(value => value.Key.MemberName);
                if (primaryKeys.count() != 0)
                {
                    table.PrimaryKey = new columnCollection
                    {
                        Columns = table.Columns.Columns.getFindArray(value => primaryKeys.indexOf(value.Name) != -1)
                    };
                }
                return table;
            }
        }
    }
}