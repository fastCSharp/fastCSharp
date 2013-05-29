using System;
using System.Reflection;
using System.IO;

namespace fastCSharp.setup
{
    /// <summary>
    /// 自动安装属性
    /// </summary>
    internal class auto : ignore
    {
        /// <summary>
        /// 安装属性空值
        /// </summary>
        internal static readonly auto NullAuto = new auto();
        /// <summary>
        /// 自动安装参数
        /// </summary>
        public class parameter
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { get; private set; }
            /// <summary>
            /// 项目路径
            /// </summary>
            public string ProjectPath { get; private set; }
            /// <summary>
            /// 程序集文件名全称
            /// </summary>
            public string AssemblyPath { get; private set; }
            /// <summary>
            /// 项目默认命名空间
            /// </summary>
            public string DefaultNamespace { get; private set; }
            /// <summary>
            /// 程序集
            /// </summary>
            private Assembly assembly;
            /// <summary>
            /// 程序集
            /// </summary>
            public Assembly Assembly
            {
                get
                {
                    if (assembly == null)
                    {
                        try
                        {
                            string assemblyFile = AssemblyPath.Substring(AssemblyPath.LastIndexOf('\\') + 1);
                            foreach (Assembly value in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                if (value.ManifestModule.Name == assemblyFile) return assembly = value;
                            }
                            //System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = new FileInfo(AssemblyPath).DirectoryName;
                            assembly = Assembly.LoadFrom(AssemblyPath);
                        }
                        catch (Exception error)
                        {
                            setup.error.Add(error);
                        }
                    }
                    return assembly;
                }
            }
            /// <summary>
            /// 类型集合
            /// </summary>
            private Type[] types;
            /// <summary>
            /// 类型集合
            /// </summary>
            public Type[] Types
            {
                get
                {
                    if (types == null && Assembly != null)
                    {
                        try
                        {
                            types = assembly.GetTypes();
                        }
                        catch (Exception error)
                        {
                            types = nullValue<Type>.Array;
                            setup.error.Add(error);
                        }
                    }
                    return types;
                }
            }
            /// <summary>
            /// 自动安装参数
            /// </summary>
            /// <param name="projectName">项目名称</param>
            /// <param name="projectPath">项目路径</param>
            /// <param name="assemblyPath">程序集文件名全称</param>
            /// <param name="defaultNamespace">项目默认命名空间</param>
            public parameter(string projectName, string projectPath, string assemblyPath, string defaultNamespace)
            {
                ProjectName = projectName;
                ProjectPath = new DirectoryInfo(projectPath).fullName().toLower();
                AssemblyPath = assemblyPath;
                DefaultNamespace = defaultNamespace;
            }
            /// <summary>
            /// 复制安装参数
            /// </summary>
            /// <returns>安装参数</returns>
            public parameter Copy()
            {
                return new parameter(ProjectName, ProjectPath, AssemblyPath, DefaultNamespace);
            }
        }
        /// <summary>
        /// 是否自动安装
        /// </summary>
        public bool IsAuto;
        /// <summary>
        /// 自动安装依赖,指定当前安装必须后于依赖安装
        /// </summary>
        public Type DependType;
        /// <summary>
        /// 安装名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 是否cSharp代码生成
        /// </summary>
        public bool isCSharper = true;
        /// <summary>
        /// 安装名称
        /// </summary>
        public string ShowName(Type type)
        {
            if (Name != null) return Name;
            if (type != null) return type.Name;
            return "未知";
        }
        /// <summary>
        /// CSharp模板文件
        /// </summary>
        public string CSharpTemplate;
        /// <summary>
        /// 获取CSharp模板文件名
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <returns>CSharp模板文件名</returns>
        public string GetFileName(Type type)
        {
            return CSharpTemplate ?? (type.Name == "cSharp" ? type.DeclaringType.Name : type.Name);
        }
    }
    /// <summary>
    /// 自动安装接口
    /// </summary>
    internal interface IAuto
    {
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        bool Run(auto.parameter parameter);
    }
}
