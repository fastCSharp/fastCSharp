using System;
using System.Reflection;
using fastCSharp;
using fastCSharp.sql;

namespace fastCSharp.config
{
    /// <summary>
    /// SQL相关参数
    /// </summary>
    public class sql : include
    {
        /// <summary>
        /// 检测链接类型集合
        /// </summary>
        private string[] checkConnection = null;
        /// <summary>
        /// 检测链接类型集合
        /// </summary>
        public string[] CheckConnection
        {
            get
            {
                return checkConnection ?? nullValue<string>.Array;
            }
        }
        /// <summary>
        /// 默认自增ID列名称
        /// </summary>
        private string defaultIdentityName = "id";
        /// <summary>
        /// 默认自增ID列名称
        /// </summary>
        public string DefaultIdentityName
        {
            get { return defaultIdentityName; }
        }
        /// <summary>
        /// 缓存默认最大容器大小
        /// </summary>
        private int cacheMaxCount = 1 << 14;
        /// <summary>
        /// 缓存默认最大容器大小
        /// </summary>
        public int CacheMaxCount
        {
            get { return cacheMaxCount; }
        }
        /// <summary>
        /// SQL相关参数
        /// </summary>
        private sql()
        {
            pub.Default.LoadConfig(this);
            fastCSharp.log.Default.Add("数据库链接处理类型: " + checkConnection.joinString(',', value => value.ToString()), false, false);
        }
        /// <summary>
        /// 默认SQL相关参数
        /// </summary>
        public static readonly sql Default = new sql();
    }
}
