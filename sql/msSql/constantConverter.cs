using System;

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// 常量转换
    /// </summary>
    internal class constantConverter : fastCSharp.sql.expression.constantConverter
    {
        /// <summary>
        /// 常量转换
        /// </summary>
        private constantConverter()
        {
            converters[typeof(string)] = convertConstantString;
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="value">常量</param>
        /// <returns>字符串</returns>
        private string convertConstantString(object value)
        {
            return value != null ? convertConstantStringMssql((string)value) : "null";
        }
        /// <summary>
        /// 常量转换
        /// </summary>
        internal static readonly constantConverter Default = new constantConverter();
    }
}
