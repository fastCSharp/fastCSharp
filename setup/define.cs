using System;
using System.IO;
using fastCSharp.web;

namespace fastCSharp.setup
{
    /// <summary>
    /// 预定义生成
    /// </summary>
    internal static class define
    {
        /// <summary>
        /// 预定义开始
        /// </summary>
        private const string defineStart = @"#region FASTCSHARPDEFINE";
        /// <summary>
        /// 预定义结束
        /// </summary>
        private const string defineEnd = @"#endregion FASTCSHARPDEFINE";
        /// <summary>
        /// 预定义生成
        /// </summary>
        /// <param name="parameter">安装参数</param>
        public static void Setup(auto.parameter parameter)
        {
            try
            {
                string csproj = parameter.ProjectPath + "fastCSharp.csproj";
                if (File.Exists(csproj))
                {
                    list<htmlNode> nodes = new htmlNode(File.ReadAllText(csproj)).Path(@"TargetFrameworkVersion");
                    if (nodes != null)
                    {
                        string define = null;
                        foreach (htmlNode node in nodes)
                        {
                            string version = nodes[0].Text.Trim();
                            if (version.Length > 1 && version[0] == 'v')
                            {
                                int leftVersion, dotIndex = version.IndexOf('.');
                                if (int.TryParse(dotIndex != -1 ? version.Substring(1, dotIndex - 1) : version.Substring(1), out leftVersion))
                                {
                                    define = @"
";
                                    if (leftVersion == 3 ? dotIndex != -1 && ++dotIndex != version.Length && version[dotIndex] >= '5' : leftVersion > 3)
                                    {
                                        define = @"
#define DOTNET35
";
                                    }
                                    break;
                                }
                            }
                            error.Add("不可识别的 TargetFrameworkVersion : " + version);
                        }
                        if (define != null)
                        {
                            foreach (string file in Directory.GetFiles(parameter.ProjectPath, "*.cs", SearchOption.AllDirectories))
                            {
                                if (file.IndexOf('@') == -1)
                                {
                                    string code = File.ReadAllText(file);
                                    if (code.StartsWith(defineStart, StringComparison.Ordinal)
                                        && !new subString(code, defineStart.Length).StartsWith(define + defineEnd))
                                    {
                                        int endIndex = code.IndexOf(defineEnd);
                                        if (endIndex != -1)
                                        {
                                            cSharp.coder.MoveFile(file, defineStart + define + defineEnd + code.Substring(endIndex + defineEnd.Length));
                                        }
                                        else error.Add(file + " 没有找到结束符 " + defineEnd);
                                    }
                                }
                            }
                            return;
                        }
                    }
                    error.Add("没有找到 TargetFrameworkVersion");
                }
                else error.Add("没有找到项目文件");
            }
            catch (Exception error)
            {
                setup.error.Add(error);
            }
            finally { error.Open(true); }
        }
    }
}
