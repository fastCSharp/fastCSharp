using System;

namespace fastCSharp.setup
{
    /// <summary>
    /// 禁止安装属性
    /// </summary>
    public abstract class ignore : Attribute
    {
        /// <summary>
        /// 是否禁止所有安装
        /// </summary>
        public bool IsIgnore;
        /// <summary>
        /// 是否禁止当前安装
        /// </summary>
        public bool IsIgnoreCurrent;
        /// <summary>
        /// 是否安装[showjim.setup]
        /// </summary>
        public bool IsSetup
        {
            get
            {
                return !IsIgnore && !IsIgnoreCurrent;
            }
        }

    }
}