using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace fastCSharp.setup
{
    /// <summary>
    /// 远程类型
    /// </summary>
    [fastCSharp.setup.cSharp.serialize(Filter = memberFilter.NonPublicInstanceField, IsStreamSerialize = true)]
    public partial struct remoteType
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        private string assemblyName;
        /// <summary>
        /// 类型名称
        /// </summary>
        private string name;
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type
        {
            get
            {
                Assembly assembly = reflection.assembly.Get(assemblyName);
                if (assembly != null)
                {
                    Type type = assembly.GetType(name);
                    if (type != null) return type;
                }
                fastCSharp.log.Default.Throw(null, "未能加载类型 : " + name + " in " + assemblyName, true);
                return null;
            }
        }
        /// <summary>
        /// 远程类型
        /// </summary>
        /// <param name="type">类型</param>
        public remoteType(Type type)
        {
            name = type.FullName;
            assemblyName = type.Assembly.FullName;
        }
        /// <summary>
        /// 类型隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>远程类型</returns>
        public static implicit operator remoteType(Type type) { return new remoteType(type); }
    }
}
