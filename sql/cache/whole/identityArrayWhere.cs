using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.cache.whole
{
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public abstract class identityArrayWhereBase<valueType, memberType> : identityArrayBase<valueType, memberType>
        where valueType : class
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private func<valueType, bool> isValue;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">数据成员位图</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="addLength">数组长度递增</param>
        public identityArrayWhereBase(sqlTable.sqlToolBase<valueType, memberType> sqlTool, memberType memberMap, func<valueType, int> getIdentity
            , func<valueType, bool> isValue, int addLength = 0)
            : base(sqlTool, memberMap, getIdentity, false, addLength)
        {
            if (isValue == null) log.Default.Throw(log.exceptionType.Null);
            this.isValue = isValue;

            sqlTool.OnInsertedLock += onInserted;
            sqlTool.OnUpdatedLock += onUpdated;
            sqlTool.OnDeletedLock += onDeleted;

            reset();
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void reset()
        {
            reset(SqlTool.Where(null, memberMap).getFindArray(isValue));
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private new void onInserted(valueType value)
        {
            if (isValue(value)) base.onInserted(value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private new void onUpdated(valueType value, valueType oldValue, memberType memberMap)
        {
            if (isValue(value)) base.onUpdated(value, oldValue, memberMap);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private new void onDeleted(valueType value)
        {
            if (isValue(value)) base.onDeleted(value);
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="memberType">数据成员类型</typeparam>
    public class identityArrayWhere<valueType, memberType> : identityArrayWhereBase<valueType, memberType>
        where valueType : class, sqlTable.ISqlIdentity<valueType, memberType, int>
        where memberType : IMemberMap<memberType>
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityArrayWhere(sqlTable.sqlToolBase<valueType, memberType> sqlTool, func<valueType, bool> isValue
            , memberType memberMap = default(memberType), int addLength = 0)
            : base(sqlTool, memberMap, value => value.SqlIdentity32, isValue, addLength)
        {
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存(反射模式)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public class identityArrayWhere<valueType> : identityArrayWhereBase<valueType, memberMap<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getIdentity">自增ID获取器</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="addLength">数组长度递增</param>
        /// <param name="memberMap">数据成员位图</param>
        public identityArrayWhere(sqlTable.sqlToolBase<valueType, memberMap<valueType>> sqlTool, func<valueType, int> getIdentity
            , func<valueType, bool> isValue, memberMap<valueType> memberMap = default(memberMap<valueType>), int addLength = 0)
            : base(sqlTool, memberMap, getIdentity, isValue, addLength)
        {
        }
    }
}
