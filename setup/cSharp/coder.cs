using System;
using System.Collections.Generic;
using System.IO;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// CSharp代码生成器
    /// </summary>
    internal class coder : template<treeBuilder.node>
    {
        /// <summary>
        /// 声明与警告
        /// </summary>
        public const string WarningCode = @"///本文件由程序自动生成,请不要自行修改
using System;
using fastCSharp;
";
        /// <summary>
        /// 已经生成的代码
        /// </summary>
        protected static stringBuilder codes = new stringBuilder();
        /// <summary>
        /// 已经生成代码的类型
        /// </summary>
        protected static Dictionary<hashCode<Type>, HashSet<hashCode<Type>>> codeTypes = new Dictionary<hashCode<Type>, HashSet<hashCode<Type>>>();
        /// <summary>
        /// CSharp代码树节点缓存
        /// </summary>
        private static Dictionary<string, treeBuilder.node> nodeCache = new Dictionary<string, treeBuilder.node>();
        /// <summary>
        /// CSharp代码生成器
        /// </summary>
        /// <param name="type">模板数据视图</param>
        public coder(Type type)
            : base(type)
        {
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected override treeBuilder.node fromNameNode(string typeName, string name)
        {
            treeBuilder.node node = getNode(typeName);
            if (node != null)
            {
                node = node.GetFirstNodeByTag(setup.template.command.NAME, name);
                if (node == null) error.Add("模板文件 " + getTemplateFileName(typeName) + " 未找到NAME " + name);
            }
            return node;
        }

        /// <summary>
        /// 创建CSharp代码生成器
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <param name="auto">安装属性</param>
        /// <returns>生成器代码</returns>
        private static string createClass(Type type, auto auto)
        {
            coder code = new coder(type);
            code.skin(getNode(auto.GetFileName(type)));
            return code.partCodes["CLASS"];
        }
        /// <summary>
        /// 根据类型名称获取模板文件名
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>模板文件名</returns>
        private static string getTemplateFileName(string typeName)
        {
            return config.setup.Default.FastCSharpPath + @"setup\cSharp\template\" + typeName + ".cs";
        }
        /// <summary>
        /// 根据类型名称获取CSharp代码树节点
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>CSharp代码树节点</returns>
        private static treeBuilder.node getNode(string fileName)
        {
            treeBuilder.node node;
            if (!nodeCache.TryGetValue(fileName, out node))
            {
                fileName = getTemplateFileName(fileName);
                if (File.Exists(fileName))
                {
                    nodeCache.Add(fileName, node = new treeBuilder().create(File.ReadAllText(fileName)));
                }
                else error.Add("未找到模板文件 " + fileName);
            }
            return node;
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码</param>
        public static void Add(string code)
        {
            codes.Add(code);
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="cSharperType">模板类型</param>
        /// <param name="type">实例类型</param>
        /// <param name="code">代码,null表示锁定</param>
        /// <returns>锁定是否成功</returns>
        public static bool Add(hashCode<Type> cSharperType, hashCode<Type> type, string code)
        {
            if (code == null)
            {
                HashSet<hashCode<Type>> types;
                if (!codeTypes.TryGetValue(cSharperType, out types)) codeTypes.Add(cSharperType, types = new HashSet<hashCode<Type>>());
                if (types.Contains(type)) return false;
                types.Add(type);
            }
            else Add(code);
            return true;
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        public static void Output(auto.parameter parameter)
        {
            string[] codeArray = codes.getArray();
            codes.Clear();
            codeTypes.Clear();
            error.ThrowError();
            string code = string.Concat(codeArray);
            if (code.length() != 0)
            {
                string fileName = parameter.ProjectPath + parameter.DefaultNamespace + "." + pub.fastCSharp + ".cs";
                if (WriteFile(fileName, WarningCode + code)) fastCSharp.log.Default.ThrowReal(fileName + " 被修改", false, false);
            }
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static bool WriteFile(string fileName, string content)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    if (File.ReadAllText(fileName) != content) return MoveFile(fileName, content);
                }
                else
                {
                    File.WriteAllText(fileName, content);
                    return true;
                }
            }
            catch (Exception error)
            {
                log.Default.ThrowReal(error, "文件创建失败 : " + fileName, false);
            }
            return false;
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static bool MoveFile(string fileName, string content)
        {
            try
            {
                int index = fileName.LastIndexOf('\\') + 1;
                string backFileName = fileName.Substring(0, index) + "@" + DateTime.Now.Ticks.ToString() + "_" + fileName.Substring(index);
                if (File.Exists(backFileName)) File.Delete(backFileName);
                File.Move(fileName, backFileName);
                File.WriteAllText(fileName, content);
                return true;
            }
            catch (Exception error)
            {
                log.Default.ThrowReal(error, "文件创建失败 : " + fileName, false);
            }
            return false;
        }

        /// <summary>
        /// CSharp代码生成数据视图生成
        /// </summary>
        [auto(Name = "C#", IsAuto = true, CSharpTemplate = null)]
        internal class cSharper : IAuto
        {
            /// <summary>
            /// 类定义生成
            /// </summary>
            private class definition
            {
                /// <summary>
                /// 类型
                /// </summary>
                public Type Type;
                /// <summary>
                /// 安装属性
                /// </summary>
                public auto Auto;
                /// <summary>
                /// 生成类定义字符串
                /// </summary>
                /// <returns>类定义字符串</returns>
                public override string ToString()
                {
                    cSharp.cSharper.definition definition = new cSharp.cSharper.definition(Type, true, true);
                    coder code = new coder(Type);
                    code.skin(getNode(Auto.GetFileName(Type)));
                    return @"
" + definition.Start + @"
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name=""isOut"">是否输出代码</param>
        protected override void create(bool isOut)
        {
            fastCSharp.setup.cSharp.cSharper.definition definition = isOut ? outStart() : null;
            if (!isOut || definition != null)
            {
                " + code.partCodes["CLASS"] + @"
                outEnd(definition);
            }
        }" + definition.End;
                }
            }
            /// <summary>
            /// 安装入口
            /// </summary>
            /// <param name="parameter">安装参数</param>
            /// <returns>是否安装成功</returns>
            public bool Run(auto.parameter parameter)
            {
                if (parameter != null)
                {
                    if (fastCSharp.config.setup.Default.FastCSharpPath == parameter.ProjectPath)
                    {
                        string[] codes = parameter.Types.getArray(type => new definition { Type = type, Auto = type.customAttribute<auto>() })
                            .getFind(type => type.Auto != null && type.Auto.IsSetup && type.Auto.isCSharper && type.Auto.DependType == typeof(cSharper))
                            .getArray(type => type.ToString());
                        if (!error.IsError)
                        {
                            string fileName = fastCSharp.config.setup.Default.FastCSharpPath + pub.fastCSharp + ".cSharper.cs";
                            if (WriteFile(fileName, WarningCode + string.Concat(codes)))
                            {
                                error.Add(fileName + " 被修改");
                                throw new Exception();
                            }
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
