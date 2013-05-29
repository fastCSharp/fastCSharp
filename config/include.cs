using System;
using System.Reflection;
using System.IO;
using fastCSharp.setup.cSharp;

namespace fastCSharp.config
{
    /// <summary>
    /// 配置文件包含
    /// </summary>
    public abstract class include
    {
        /// <summary>
        /// 包含配置文件
        /// </summary>
        protected string[] includeFile
        {
            set
            {
                foreach (string path in value.notNull())
                {
                    ajax.jsonNode config = load(Path.Combine(pub.Default.configPath, path));
                    if (config.Type == ajax.jsonNode.nodeType.Dictionary)
                    {
                        ajax.parseJson.GetConfig((object)this, config);
                    }
                }
            }
        }
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFile">配置文件</param>
        /// <returns>配置JSON节点</returns>
        private static ajax.jsonNode load(string configFile)
        {
            if (File.Exists(configFile))
            {
                try
                {
                    return new ajax.jsonParser().Parse(File.ReadAllText(configFile));
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                }
            }
            else log.Default.Add("找不到配置文件 : " + configFile, true, false);
            return default(ajax.jsonNode);
        }
    }
}
