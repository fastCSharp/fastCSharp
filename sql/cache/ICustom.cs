using System;
using System.Collections.Generic;

namespace fastCSharp.sql.cache
{
    /// <summary>
    /// 自定义缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    public interface ICustom<valueType>
    {
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        void Add(valueType value);
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>是否存在被删除数据</returns>
        bool Remove(valueType value);
        /// <summary>
        /// 所有缓存数据
        /// </summary>
        IEnumerable<valueType> Values { get; }
    }
}
