using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Reflection;
using fastCSharp.reflection;
using fastCSharp.setup.cSharp;

namespace fastCSharp.config
{
    /// <summary>
    /// 基本配置
    /// </summary>
    [fastCSharp.setup.cSharp.ajax(Filter = fastCSharp.setup.memberFilter.Instance, IsParseJson = true, IsParseAll = true)]
    public partial class pub : include
    {
        /// <summary>
        /// 默认配置文件
        /// </summary>
        private const string defaultConfigFile = fastCSharp.pub.fastCSharp + ".config";
        /// <summary>
        /// 配置文件路径
        /// </summary>
        [fastCSharp.setup.cSharp.ajax(IsIgnore = true)]
        protected internal string configPath = fastCSharp.pub.ApplicationPath;
        /// <summary>
        /// 配置信息
        /// </summary>
        [fastCSharp.setup.cSharp.ajax(IsIgnore = true)]
        private ajax.jsonNode configs;
        /// <summary>
        /// 程序工作主目录
        /// </summary>
        public string WorkPath { get; private set; }
        /// <summary>
        /// 日志文件主目录
        /// </summary>
        public string LogPath { get; private set; }
        /// <summary>
        /// 缓存文件主目录
        /// </summary>
        public string CachePath { get; private set; }
        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebug { get; private set; }

        /// <summary>
        /// 全局默认编码
        /// </summary>
        private string encoding;
        /// <summary>
        /// 全局默认编码
        /// </summary>
        [fastCSharp.setup.cSharp.ajax(IsIgnore = true)]
        private Encoding encodingValue;
        /// <summary>
        /// 全局默认编码
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                if (encodingValue == null)
                {
                    if (encoding != null)
                    {
                        try
                        {
                            encodingValue = System.Text.Encoding.GetEncoding(encoding);
                        }
                        catch (Exception error)
                        {
                            log.Default.Add(error, null, false);
                        }
                    }
                    if (encodingValue == null) encodingValue = fastCSharp.pub.Gb2312;
                }
                return encodingValue;
            }
        }
        /// <summary>
        /// 流缓冲区大小
        /// </summary>
        private int streamBufferLength = 32 * 1024;
        /// <summary>
        /// 流缓冲区大小
        /// </summary>
        public int StreamBufferLength
        {
            get { return streamBufferLength; }
        }
        /// <summary>
        /// 默认分页大小
        /// </summary>
        private int pageSize = 10;
        /// <summary>
        /// 默认分页大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
        }
        /// <summary>
        /// 默认分页大小
        /// </summary>
        private int maxEnumArraySize = 1024;
        /// <summary>
        /// 最大枚举数组数量
        /// </summary>
        public int MaxEnumArraySize
        {
            get { return maxEnumArraySize; }
        }
        /// <summary>
        /// 默认任务线程数
        /// </summary>
        private int taskThreadCount = 128;
        /// <summary>
        /// 默认任务线程数
        /// </summary>
        public int TaskThreadCount
        {
            get
            {
                if (taskThreadCount > taskMaxThreadCount)
                {
                    log.Default.Add("默认任务线程数[" + taskThreadCount.toString() + "] 超出 任务最大线程数[" + taskMaxThreadCount.toString() + "]");
                    return taskMaxThreadCount;
                }
                return taskThreadCount;
            }
        }
        /// <summary>
        /// 任务最大线程数
        /// </summary>
        private int taskMaxThreadCount = 65536;
        /// <summary>
        /// 任务最大线程数
        /// </summary>
        public int TaskMaxThreadCount
        {
            get { return taskMaxThreadCount; }
        }
        /// <summary>
        /// 死锁检测分钟数,0表示不检测
        /// </summary>
        private int lockCheckMinutes;
        /// <summary>
        /// 死锁检测分钟数,0表示不检测
        /// </summary>
        public int LockCheckMinutes
        {
            get { return lockCheckMinutes; }
        }
        /// <summary>
        /// 默认线程池线程堆栈大小
        /// </summary>
        private int defaultThreadPoolStackSize = 1 << 20;
        /// <summary>
        /// 默认线程池线程堆栈大小
        /// </summary>
        public int DefaultThreadPoolStackSize
        {
            get { return defaultThreadPoolStackSize; }
        }
        /// <summary>
        /// 默认微型线程池线程堆栈大小
        /// </summary>
        private int tinyThreadPoolStackSize = 128 << 10;
        /// <summary>
        /// 默认微型线程池线程堆栈大小
        /// </summary>
        public int TinyThreadPoolStackSize
        {
            get { return tinyThreadPoolStackSize; }
        }
        /// <summary>
        /// 微型线程任务线程数
        /// </summary>
        private int tinyThreadCount = 65536;
        /// <summary>
        /// 微型线程任务线程数
        /// </summary>
        public int TinyThreadCount
        {
            get
            {
                if (tinyThreadCount > taskMaxThreadCount)
                {
                    log.Default.Add("微型线程任务线程数[" + tinyThreadCount.toString() + "] 超出 任务最大线程数[" + taskMaxThreadCount.toString() + "]");
                    return taskMaxThreadCount;
                }
                return tinyThreadCount;
            }
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <returns>配置</returns>
        private ajax.jsonNode getConfig(Type type)
        {
            if (configs.Type != ajax.jsonNode.nodeType.Null)
            {
                string name = type.FullName, fastCSharpName = fastCSharp.pub.fastCSharp + ".";
                if (name.StartsWith(fastCSharpName, StringComparison.Ordinal))
                {
                    string fastCSharpConfig = fastCSharpName + "config.";
                    name = name.Substring(name.StartsWith(fastCSharpConfig, StringComparison.Ordinal) ? fastCSharpConfig.Length : fastCSharpName.Length);
                }
                ajax.jsonNode config = configs;
                foreach (string tagName in name.Split('.'))
                {
                    if (config.Type != ajax.jsonNode.nodeType.Dictionary
                        || !config.Dictionary.TryGetValue(tagName, out config))
                    {
                        return default(ajax.jsonNode);
                    }
                }
                return config;
            }
            return default(ajax.jsonNode);
        }
        /// <summary>
        /// 配置加载
        /// </summary>
        /// <param name="value">配置对象</param>
        /// <param name="name">配置名称,null表示只匹配类型</param>
        public valueType LoadConfig<valueType>(valueType value, string name = null)
        {
            if (value == null) log.Default.Throw(log.exceptionType.Null);
            ajax.jsonNode config = getConfig(value.GetType());
            if (name == null) return ajax.parseJson.GetConfig<valueType>(value, config);
            if (config.Type == ajax.jsonNode.nodeType.Dictionary && config.Dictionary.TryGetValue(name, out config))
            {
                return ajax.parseJson.GetConfig<valueType>(value, config);
            }
            //log.Default.Add("没有找到配置 " + typeof(valueType).fullName() + "." + name, true, false);
            return value;
        }
        /// <summary>
        /// 判断配置名称是否存在
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称</param>
        /// <returns>配置是否加载存在</returns>
        public bool IsConfigName(Type type, string name)
        {
            ajax.jsonNode config = getConfig(type);
            if (config.Type == ajax.jsonNode.nodeType.Dictionary && config.Dictionary.TryGetValue(name, out config))
            {
                return config.Type != ajax.jsonNode.nodeType.Null;
            }
            return false;
        }
        /// <summary>
        /// 默认基本配置
        /// </summary>
        public static readonly pub Default;
        static pub()
        {
            Default = new pub();
            string configFile = ConfigurationManager.AppSettings["configFile"];
            if (configFile == null) configFile = fastCSharp.pub.ApplicationPath + defaultConfigFile;
            else if (configFile.IndexOf(':') == -1) configFile = fastCSharp.pub.ApplicationPath + configFile;
            else Default.configPath = new FileInfo(configFile).Directory.fullName().toLower();

            if (File.Exists(configFile))
            {
                if (Default.LogPath == null || !directory.Create(Default.LogPath = Default.LogPath.pathSuffix().toLower())) Default.LogPath = fastCSharp.pub.ApplicationPath;
                try
                {
                    Default.configs = new ajax.jsonParser().Parse(File.ReadAllText(configFile));
                    Default.FromJson(Default.getConfig(typeof(pub)));
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                }
            }
            else log.Default.Add("找不到配置文件 : " + configFile, false, false);

            if (Default.WorkPath == null) Default.WorkPath = fastCSharp.pub.ApplicationPath;
            else Default.WorkPath = Default.WorkPath.pathSuffix().toLower();
            if (Default.LogPath == null || !directory.Create(Default.LogPath = Default.LogPath.pathSuffix().toLower())) Default.LogPath = fastCSharp.pub.ApplicationPath;
            if (Default.CachePath == null || !directory.Create(Default.CachePath = Default.CachePath.pathSuffix().toLower())) Default.CachePath = fastCSharp.pub.ApplicationPath;
        }
    }
}
