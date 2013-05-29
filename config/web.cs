using System;
using System.Reflection;

namespace fastCSharp.config
{
    /// <summary>
    /// 网站模块相关参数
    /// </summary>
    public class web : include
    {
        /// <summary>
        /// json查询对象名称
        /// </summary>
        private string queryJsonName = "json";
        /// <summary>
        /// json查询对象名称
        /// </summary>
        public string QueryJsonName
        {
            get { return queryJsonName; }
        }
        /// <summary>
        /// Ajax返回值名称
        /// </summary>
        private string ajaxReturnName = "_return_";
        /// <summary>
        /// Ajax返回值名称
        /// </summary>
        public string AjaxReturnName
        {
            get { return ajaxReturnName; }
        }
        /// <summary>
        /// Json转换时间差
        /// </summary>
        private DateTime javascriptMinTime = new DateTime(1970, 1, 1, 8, 0, 0);
        /// <summary>
        /// Json转换时间差
        /// </summary>
        public DateTime JavascriptMinTime
        {
            get { return javascriptMinTime;}
        }
        /// <summary>
        /// Json转换时间差
        /// </summary>
        private DateTime parseJavascriptMinTime = new DateTime(1970, 1, 1, 0, 0, 0);
        /// <summary>
        /// Json转换时间差
        /// </summary>
        public DateTime ParseJavascriptMinTime
        {
            get { return parseJavascriptMinTime; }
        }
        /// <summary>
        /// 网站模块相关参数
        /// </summary>
        private web()
        {
            pub.Default.LoadConfig(this);
        }
        /// <summary>
        /// 默认网站模块相关参数
        /// </summary>
        public static readonly web Default = new web();
    }
}
