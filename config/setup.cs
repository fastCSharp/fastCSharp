using System;
using System.Reflection;
using System.IO;

namespace fastCSharp.config
{
    /// <summary>
    /// 安装配置
    /// </summary>
    public class setup : include
    {
        /// <summary>
        /// fastCSharp项目路径
        /// </summary>
        private string fastCSharpPath = @"d:\" + fastCSharp.pub.fastCSharp + @"\";
        /// <summary>
        /// fastCSharp项目路径
        /// </summary>
        public string FastCSharpPath
        {
            get { return fastCSharpPath; }
        }
        /// <summary>
        /// 自定义模板相对项目路径
        /// </summary>
        private string simpleTemplatePath = fastCSharp.setup.cSharp.simpleTemplate.DefaultTemplatePath;
        /// <summary>
        /// 自定义模板相对项目路径
        /// </summary>
        public string SimpleTemplatePath
        {
            get { return simpleTemplatePath; }
        }
        /// <summary>
        /// 安装项目基本命名空间
        /// </summary>
        private string baseNamespace = "diantou";
        /// <summary>
        /// 安装项目基本命名空间
        /// </summary>
        public string BaseNamespace
        {
            get { return baseNamespace; }
        }
        /// <summary>
        /// 安装项目查找路径
        /// </summary>
        private string projectPath = @"d:\diantou\";
        /// <summary>
        /// 安装项目查找路径
        /// </summary>
        public string ProjectPath
        {
            get { return projectPath; }
        }
        /// <summary>
        /// 安装标题
        /// </summary>
        private string setupTitle = "diantou";
        /// <summary>
        /// 安装标题
        /// </summary>
        public string SetupTitle
        {
            get { return setupTitle; }
        }
        /// <summary>
        /// TCP服务注册检测超时秒数
        /// </summary>
        private int tcpRegisterCheckTimeoutSeconds = 2;
        /// <summary>
        /// TCP服务注册检测超时秒数
        /// </summary>
        public int TcpRegisterCheckTimeoutSeconds
        {
            get { return tcpRegisterCheckTimeoutSeconds; }
        }
        /// <summary>
        /// TCP服务注册起始端口号
        /// </summary>
        private int tcpRegisterPortStart = 9000;
        /// <summary>
        /// TCP服务注册起始端口号
        /// </summary>
        public int TcpRegisterPortStart
        {
            get { return tcpRegisterPortStart; }
        }
        /// <summary>
        /// TCP服务注册验证
        /// </summary>
        public string TcpRegisterVerify;
        /// <summary>
        /// TCP服务注册验证最大连续失败次数
        /// </summary>
        private int tcpRegisterMaxVerifyError = 5;
        /// <summary>
        /// TCP服务注册验证最大连续失败次数
        /// </summary>
        public int TcpRegisterMaxVerifyError
        {
            get { return tcpRegisterMaxVerifyError; }
        }
        /// <summary>
        /// 安装配置
        /// </summary>
        private setup()
        {
            pub.Default.LoadConfig(this);
            if (fastCSharpPath == null) fastCSharpPath = new DirectoryInfo(fastCSharp.pub.ApplicationPath).Parent.Parent.fullName();
            fastCSharpPath = fastCSharpPath.toLower();
            projectPath = projectPath.pathSuffix().toLower();
        }
        /// <summary>
        /// 默认安装配置
        /// </summary>
        public static readonly setup Default = new setup();
    }
}
