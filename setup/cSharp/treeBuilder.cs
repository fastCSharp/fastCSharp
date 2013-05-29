using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// CSharp代码建树器
    /// </summary>
    internal class treeBuilder
    {
        /// <summary>
        /// #region代码段分组名称
        /// </summary>
        public const string RegionGroupName = "region";
        /// <summary>
        /// command代码段分组名称
        /// </summary>
        public const string CommandGroupName = "command";
        /// <summary>
        /// 内容分组名称
        /// </summary>
        public const string ContentGroupName = "content";
        /// <summary>
        /// @取值command
        /// </summary>
        public readonly static string AtCommand = setup.template.command.AT.ToString();
        /// <summary>
        /// 所有command
        /// </summary>
        public readonly static string Commands = Enum.Array<setup.template.command>().joinString('|', value => value.ToString());
        /// <summary>
        /// command后续取值范围
        /// </summary>
        public const string CommandContent = @"[0-9A-Za-z\.]+";
        /// <summary>
        /// #region代码段
        /// </summary>
        private static readonly Regex regionTag = new Regex(@"\r?\n *#(?<" + RegionGroupName + @">" + RegionGroupName + @"|endregion) +(?<" + CommandGroupName + @">" + Commands + @")( +(?<" + ContentGroupName + @">" + CommandContent + @"))? *(?=\r?\n)", RegexOptions.Compiled);
        /// <summary>
        /// /**/注释代码段
        /// </summary>
        private static readonly Regex noteTag = new Regex(@"\/\*(?<" + CommandGroupName + @">" + Commands + @")(\:(?<" + ContentGroupName + @">" + CommandContent + @"))?\*\/", RegexOptions.Compiled);
        /// <summary>
        /// @取值
        /// </summary>
        private static readonly Regex atTag = new Regex(@"@(?<" + ContentGroupName + @">" + CommandContent + @")", RegexOptions.Compiled);
        /// <summary>
        /// CSharp代码树节点
        /// </summary>
        internal class node : template<node>.INode, setup.treeBuilder<node, tag>.INode
        {
            /// <summary>
            /// 树节点标识
            /// </summary>
            public tag Tag { get; set; }
            /// <summary>
            /// 模板命令
            /// </summary>
            public string TemplateCommand
            {
                get { return Tag.command; }
            }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            public string TemplateMemberName
            {
                get { return Tag.command != null ? Tag.content : null; }
            }
            /// <summary>
            /// 模板文本代码
            /// </summary>
            public string TemplateCode
            {
                get { return Tag.command == null ? Tag.content : null; }
            }
            /// <summary>
            /// 子节点数量
            /// </summary>
            public int ChildCount
            {
                get { return childs.count(); }
            }
            /// <summary>
            /// 子节点集合
            /// </summary>
            public IEnumerable<node> Childs
            {
                get { return childs.notNull(); }
            }
            /// <summary>
            /// 子节点集合
            /// </summary>
            private list<node> childs;
            /// <summary>
            /// 设置子节点集合
            /// </summary>
            /// <param name="childs">子节点集合</param>
            public void SetChilds(node[] childs)
            {
                this.childs = new list<node>(childs, true);
            }
            /// <summary>
            /// 获取第一个匹配的子孙节点
            /// </summary>
            /// <param name="command">模板命令类型</param>
            /// <param name="content">内容</param>
            /// <returns>匹配的CSharp代码树节点</returns>
            public node GetFirstNodeByTag(setup.template.command command, string content)
            {
                if (Tag.command == command.ToString() && Tag.content == content) return this;
                if (childs != null)
                {
                    foreach(node son in childs)
                    {
                        node value = son.GetFirstNodeByTag(command, content);
                        if (value != null) return value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// CSharp代码树节点标识
        /// </summary>
        public class tag : IEquatable<tag>
        {
            /// <summary>
            /// 树节点标识类型
            /// </summary>
            public enum type
            {
                /// <summary>
                /// 普通代码段
                /// </summary>
                Code,
                /// <summary>
                /// #region代码段
                /// </summary>
                Region,
                /// <summary>
                /// /**/注释代码段
                /// </summary>
                Note,
                /// <summary>
                /// @取值代码
                /// </summary>
                At,
            }
            /// <summary>
            /// 树节点标识类型
            /// </summary>
            public type tagType;
            /// <summary>
            /// 标识command
            /// </summary>
            public string command;
            /// <summary>
            /// 内容
            /// </summary>
            public string content;
            /// <summary>
            /// 判断树节点标识是否相等
            /// </summary>
            /// <param name="other">树节点标识</param>
            /// <returns>是否相等</returns>
            public bool Equals(tag other)
            {
                return other != null && other.tagType == tagType && other.command == command && other.content == content;
            }
            /// <summary>
            /// 转换成字符串
            /// </summary>
            /// <returns>字符串</returns>
            public override string ToString()
            {
                switch (tagType)
                {
                    case type.Region:
                        return "#region " + command + " " + content;
                    case type.Note:
                        return "/*" + command + ":" + content + "*/";
                    case type.At:
                        return "@" + content;
                }
                if (content != null)
                {
                    string code = content.Replace('\r', ' ').Replace('\n', ' ');
                    if (code.length() > 64) return code.Substring(0, 32) + " ... " + code.Substring(code.Length - 32);
                    return code;
                }
                return null;
            }
        }
        /// <summary>
        /// 建树器
        /// </summary>
        private setup.treeBuilder<node, tag> tree;
        /// <summary>
        /// CSharp代码建树器
        /// </summary>
        public treeBuilder()
        {
            tree = new setup.treeBuilder<node, tag>(value => value.Tag.tagType == tag.type.Region || value.Tag.tagType == tag.type.Note);
        }
        /// <summary>
        /// 根据代码获取代码树
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>代码树</returns>
        internal node create(string code)
        {
            if (code.length() != 0)
            {
                try
                {
                    tree.Empty();
                    region(code);
                    node boot = new node();
                    boot.SetChilds(tree.End());
                    return boot;
                }
                catch (Exception error)
                {
                    setup.error.Add(error);
                }
            }
            return null;
        }
        /// <summary>
        /// 解析#region代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void region(string code)
        {
            int index = 0;
            foreach (Match match in regionTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) note(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                tag tag = new tag
                {
                    tagType = tag.type.Region,
                    command = match.Groups[CommandGroupName].Value,
                    content = name != null ? name.Value : null
                };
                if (match.Groups[RegionGroupName].Value == RegionGroupName) tree.Append(new node { Tag = tag });
                else tree.Round(tag);
                index = match.Index + match.Length;
            }
            if (index != code.Length) note(code.Substring(index));
        }
        /// <summary>
        /// 解析/**/注释代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void note(string code)
        {
            int index = 0;
            foreach (Match match in noteTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) at(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                tag tag = new tag
                {
                    tagType = tag.type.Note,
                    command = match.Groups[CommandGroupName].Value,
                    content = name != null ? name.Value : null
                };
                if (!tree.IsRound(tag)) tree.Append(new node { Tag = tag });
                index = match.Index + match.Length;
            }
            if (index != code.Length) at(code.Substring(index));
        }
        /// <summary>
        /// 解析@取值
        /// </summary>
        /// <param name="code">代码</param>
        private void at(string code)
        {
            int index = 0;
            foreach (Match match in atTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) this.code(code.Substring(index, length));
                tree.Append(new node
                {
                    Tag = new tag
                    {
                        tagType = tag.type.At,
                        command = AtCommand,
                        content = match.Groups[ContentGroupName].Value
                    }
                });
                index = match.Index + match.Length;
            }
            this.code(code.Substring(index));
        }
        /// <summary>
        /// 普通代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void code(string code)
        {
            tree.Append(new node
            {
                Tag = new tag
                {
                    tagType = tag.type.Code,
                    content = code
                }
            });
        }
    }
}
