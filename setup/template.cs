using System;
using System.Collections.Generic;
using fastCSharp;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 树节点模板
    /// </summary>
    abstract class template
    {
        /// <summary>
        /// 成员信息缓存集合
        /// </summary>
        private static Dictionary<hashCode<Type>, Dictionary<string, memberInfo>> memberCache = new Dictionary<hashCode<Type>, Dictionary<string, memberInfo>>();
        /// <summary>
        /// 成员树节点
        /// </summary>
        internal class memberNode
        {
            /// <summary>
            /// 树节点模板
            /// </summary>
            public template Template;
            /// <summary>
            /// 成员类型
            /// </summary>
            public memberType Type;
            /// <summary>
            /// 成员名称+成员信息集合
            /// </summary>
            public Dictionary<string, memberInfo> Members
            {
                get
                {
                    Dictionary<string, memberInfo> values;
                    hashCode<Type> hashType = Type.Type;
                    if (!memberCache.TryGetValue(hashType, out values))
                    {
                        try
                        {
                            memberCache[hashType] = values = memberInfo.GetMembers(Type, memberFilter.Instance)
                                .getDictionary(value => value.MemberName);
                        }
                        catch (Exception error)
                        {
                            string output = memberInfo.GetMembers(Type, memberFilter.Instance)
                                .groupCount(value => value.MemberName)
                                .getFind(value => value.Value != 1)
                                .joinString(',', value => value.Key);
                            log.Default.ThrowReal(error, Type.FullName + " : " + output, true);
                        }
                    }
                    return values;
                }
            }
            /// <summary>
            /// 当前节点成员名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 父节点成员
            /// </summary>
            public memberNode Parent;
            /// <summary>
            /// 节点路径
            /// </summary>
            public string Path;
            /// <summary>
            /// 节点路径全称
            /// </summary>
            public string FullPath
            {
                get
                {
                    if (Parent != null)
                    {
                        collection<string> path = new collection<string>();
                        for (memberNode member = this; member.Parent != null; member = member.Parent) path.AddExpand("." + member.Name);
                        return string.Concat(path.ToArray()).Substring(1);
                    }
                    return null;
                }
            }
            /// <summary>
            /// 根据成员名称获取子节点成员
            /// </summary>
            /// <param name="name">成员名称</param>
            /// <returns>子节点成员</returns>
            public memberNode Get(string name)
            {
                Dictionary<string, memberNode> paths;
                if (!Template.memberPaths.TryGetValue(this, out paths))
                {
                    Template.memberPaths[this] = paths = new Dictionary<string, memberNode>();
                }
                memberNode value;
                if (paths.TryGetValue(name, out value)) return value;
                if (name.Length != 0)
                {
                    memberInfo member;
                    if (Members.TryGetValue(name, out member)) value = new memberNode { Type = member.MemberType, Name = name };
                }
                else value = new memberNode { Type = Type.EnumerableArgumentType };
                if (value != null)
                {
                    value.Parent = this;
                    value.Template = Template;
                    paths[name] = value;
                }
                return value;
            }
            /// <summary>
            /// 节点路径上是否有下级路径
            /// </summary>
            public bool IsNextPath
            {
                get
                {
                    Dictionary<string, memberNode> paths;
                    return Template.memberPaths.TryGetValue(this, out paths) && paths.Count != 0;
                }
            }
        }
        /// <summary>
        /// 模板命令类型
        /// </summary>
        internal enum command
        {
            /// <summary>
            /// 输出绑定的数据
            /// </summary>
            AT,
            /// <summary>
            /// 子代码段
            /// </summary>
            PUSH,
            /// <summary>
            /// 循环
            /// </summary>
            LOOP,
            /// <summary>
            /// 循环=LOOP
            /// </summary>
            FOR,
            /// <summary>
            /// 屏蔽代码段输出
            /// </summary>
            NOTE,
            /// <summary>
            /// 绑定的数据为true非0非null时输出代码
            /// </summary>
            IF,
            /// <summary>
            /// 绑定的数据为false或者0或者null时输出代码
            /// </summary>
            NOT,
            /// <summary>
            /// 用于标识一个子段模板，可以被别的模板引用
            /// </summary>
            NAME,
            /// <summary>
            /// 引用NAME标识一个子段模板
            /// </summary>
            FROMNAME,
            /// <summary>
            /// 用于标识一个子段程序代码，用于代码的分类输出
            /// </summary>
            PART,
        }
        /// <summary>
        /// 模板数据视图类型
        /// </summary>
        protected Type viewType;
        /// <summary>
        /// 当前代码字符串
        /// </summary>
        protected stringBuilder code = new stringBuilder();
        /// <summary>
        /// 当前代码字符串
        /// </summary>
        public string Code
        {
            get
            {
                pushCode(null);
                return code.ToString();
            }
        }
        /// <summary>
        /// 当前代码字符串常量
        /// </summary>
        protected stringBuilder pushCodes = new stringBuilder();
        /// <summary>
        /// 子段程序代码集合
        /// </summary>
        protected Dictionary<string, string> partCodes = new Dictionary<string, string>();
        /// <summary>
        /// 当前成员节点集合
        /// </summary>
        protected list<memberNode> currentMembers = new list<memberNode>();
        /// <summary>
        /// 成员树
        /// </summary>
        protected Dictionary<memberNode, Dictionary<string, memberNode>> memberPaths = new Dictionary<memberNode, Dictionary<string, memberNode>>();
        /// <summary>
        /// 临时逻辑变量名称
        /// </summary>
        protected string ifName = "_if_";
        /// <summary>
        /// 是否记录循环集合
        /// </summary>
        protected virtual bool isLoopValue
        {
            get { return false; }
        }
        /// <summary>
        /// 获取临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>变量名称</returns>
        protected string path(int index)
        {
            return "_value" + (index == 0 ? (currentMembers.Count - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环索引临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环索引变量名称</returns>
        protected string loopIndex(int index)
        {
            return "_loopIndex" + (index == 0 ? (currentMembers.Count - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环数量临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环数量变量名称</returns>
        protected string loopCount(int index)
        {
            return "_loopCount" + (index == 0 ? (currentMembers.Count - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环集合临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环集合变量名称</returns>
        protected string loopValues(int index)
        {
            return "_loopValues" + (index == 0 ? (currentMembers.Count - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 获取循环内临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环内变量名称</returns>
        protected string loopValue(int index)
        {
            return "_loopValue" + (index == 0 ? (currentMembers.Count - 1) : index).ToString() + "_";
        }
        /// <summary>
        /// 根据成员名称获取成员树节点
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索,false表示当前节点子节点</param>
        /// <returns>成员树节点</returns>
        protected memberNode getMember(string memberName, out bool isDepth)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            memberName = memberName.Substring(memberIndex);
            memberIndex = currentMembers.Count - memberIndex - 1;
            if (memberIndex < 0) memberIndex = 0;
            memberNode value = currentMembers[memberIndex];
            isDepth = false;
            if (memberName.Length != 0)
            {
                string[] names = memberName.Split('.');
                for (; memberIndex >= 0; --memberIndex)
                {
                    if ((value = currentMembers[memberIndex].Get(names[0])) != null)
                    {
                        value.Path = memberIndex == 0 ? names[0] : (path(memberIndex) + "." + names[0]);
                        for (int nameIndex = 1; nameIndex != names.Length; ++nameIndex)
                        {
                            isDepth = true;
                            if ((value = value.Get(names[nameIndex])) == null) break;
                            value.Path = value.Parent.Path + "." + names[nameIndex];
                        }
                        if (value == null) break;
                        else return value;
                    }
                }
                error.Add(viewType.fullName() + " 未找到属性 " + currentMembers.lastOrDefault().FullPath + " . " + memberName);
            }
            return value;
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码,null表示截断字符串</param>
        protected void pushCode(string code)
        {
            if (code != null) pushCodes.Add(code);
            else
            {
                code = pushCodes.ToString();
                if (code.Length != 0)
                {
                    this.code.Append(@"
            _code_.Add(@""", code.Replace(@"""", @""""""), @""");");
                }
                pushCodes.Empty();
            }
        }
        /// <summary>
        /// 添加当前成员节点
        /// </summary>
        /// <param name="member">成员节点</param>
        protected void pushMember(memberNode member)
        {
            currentMembers.Add(member);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isSkip">是否跳跃层次</param>
        protected void ifStart(string memberName, bool isSkip)
        {
            bool isDepth;
            memberNode member = getMember(memberName, out isDepth);
            pushMember(member);
            if (isSkip) pushMember(member);
            string name = path(0);
            code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = ", member.Path, ";");
            ifStart(member.Type, name, null);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">if临时变量名称</param>
        protected void ifStart(memberType type, string name, string ifName)
        {
            if (type.IsStruct || type.Type.IsEnum)
            {
                if (type.IsBool)
                {
                    code.Append(@"
                    if (", name, ")");
                }
                else
                {
                    code.Append(@"
                    if (", name, " != default(", type.FullName, "))");
                }
            }
            else
            {
                code.Append(@"
                    if (", name, " != null)");
            }
            code.Append(@"
                    {");
            if (ifName != null)
            {
                code.Append(@"
                        ", ifName, " = true;");
            }
        }
        /// <summary>
        /// if结束代码段
        /// </summary>
        /// <param name="isMember">是否删除成员节点</param>
        protected void ifEnd(bool isMember)
        {
            if (isMember) currentMembers.Pop();
            code.Append(@"
                    }
                }");
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索</param>
        /// <param name="doMember">成员处理函数</param>
        protected void ifThen(memberNode member, string memberName, bool isDepth, action<memberNode> doMember)
        {
            if (isDepth)
            {
                pushCode(null);
                subString[] names = splitMemberName(memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(names[index], false);
                doMember(getMember(names[names.Length - 1], out isDepth));
                pushCode(null);
                for (int index = 0; index != names.Length - 1; ++index) ifEnd(true);
            }
            else doMember(member);
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="member">成员节点</param>
        protected void at(memberNode member)
        {
            pushCode(null);
            if (member.Type.IsString)
            {
                code.Append(@"
            _code_.Add(", member.Path, ");");
            }
            else if (member.Type.IsBool && member.Type.IsStruct)
            {
                code.Append(@"
            _code_.Add(", member.Path, @" ? ""true"" : ""false"");");
            }
            else
            {
                code.Append(@"
            _code_.Add(", member.Path, ".ToString());");
            }
        }
        /// <summary>
        /// 分解成员名称
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>成员名称集合</returns>
        protected static subString[] splitMemberName(string memberName)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            string value = memberName.Substring(0, memberIndex);
            subString[] names = new subString(memberName, memberIndex).Split('.').ToArray();
            names[0] = value + names[0];
            return names;
        }
    }
    /// <summary>
    /// 树节点模板
    /// </summary>
    /// <typeparam name="nodeType">树节点类型</typeparam>
    abstract class template<nodeType> : template where nodeType : template<nodeType>.INode
    {
        /// <summary>
        /// 模板代码节点接口
        /// </summary>
        internal interface INode
        {
            /// <summary>
            /// 模板命令
            /// </summary>
            string TemplateCommand { get; }
            /// <summary>
            /// 模板成员名称
            /// </summary>
            string TemplateMemberName { get; }
            /// <summary>
            /// 模板文本代码
            /// </summary>
            string TemplateCode { get; }
            /// <summary>
            /// 子节点数量
            /// </summary>
            int ChildCount { get; }
            /// <summary>
            /// 子节点集合
            /// </summary>
            IEnumerable<nodeType> Childs { get; }
        }
        /// <summary>
        /// 模板command+解析器
        /// </summary>
        protected Dictionary<string, action<nodeType>> creators = new Dictionary<string, action<nodeType>>();
        /// <summary>
        /// 引用代码树节点
        /// </summary>
        protected Dictionary<string, nodeType> nameNodes = new Dictionary<string, nodeType>();
        /// <summary>
        /// 树节点模板
        /// </summary>
        /// <param name="type">模板数据视图</param>
        protected template(Type type)
        {
            creators[command.NOTE.ToString()] = note;
            creators[command.LOOP.ToString()] = creators[command.FOR.ToString()] = loop;
            creators[command.AT.ToString()] = at;
            creators[command.PUSH.ToString()] = push;
            creators[command.IF.ToString()] = ifThen;
            creators[command.NOT.ToString()] = not;
            creators[command.NAME.ToString()] = name;
            creators[command.FROMNAME.ToString()] = fromName;
            creators[command.PART.ToString()] = part;
            currentMembers.Add(new memberNode { Template = this, Type = viewType = type ?? GetType() });
        }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void skin(nodeType node)
        {
            action<nodeType> creator;
            foreach (nodeType son in node.Childs)
            {
                string command = son.TemplateCommand;
                if (command == null) pushCode(son.TemplateCode);
                else if (creators.TryGetValue(command, out creator)) creator(son);
                else error.Add(viewType.fullName() + " 未找到命名处理函数 " + command + " : " + son.TemplateMemberName);
            }
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void at(nodeType node)
        {
            bool isDepth;
            string memberName = node.TemplateMemberName;
            memberNode member = getMember(memberName, out isDepth);
            if (member != null) ifThen(member, memberName, isDepth, value => at(value));
        }
        /// <summary>
        /// 注释处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void note(nodeType node)
        {
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void loop(memberNode member, nodeType node, string name, int popCount)
        {
            code.Append(@"
                    ", name, " = ", member.Path, ";");
            if (popCount != 0) currentMembers.Pop();
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (member.Type.EnumerableType == null)
            {
                error.Add(viewType.fullName() + " 属性不可枚举 " + currentMembers.lastOrDefault().FullPath);
                return;
            }
            pushMember(member);
            string valueName = path(currentMembers.Count);
            code.Append(@"
                    if (", name, @" != null)
                    {
                        int ", loopIndex(0), @" = _loopIndex_, ", loopCount(0), @" = _loopCount_;");
            if (isLoopValue)
            {
                code.Append(@"
                        var ", loopValues(0), @" = _loopValues_, ", loopValue(0), @" = _loopValue_;
                        _loopValues_ = ", name, ";");
            }
            code.Append(@"
                        _loopIndex_ = 0;
                        _loopCount_ = ", name, member.Type.Type.IsArray ? ".Length" : ".count()", @";
                        foreach (", member.Type.EnumerableArgumentType.FullName, " " + valueName + " in ", name, @")
                        {");
            if (isLoopValue)
            {
                code.Append(@"
                            _loopValue_ = ", valueName, ";");
            }
            memberNode loopMember = member.Get(string.Empty);
            loopMember.Path = valueName;
            pushMember(loopMember);
            skin(node);
            currentMembers.Pop();
            pushCode(null);
            code.Append(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";
                        _loopCount_ = ", loopCount(0), @";");
            if (isLoopValue)
            {
                code.Append(@"
                        _loopValue_ = ", loopValue(0), @";
                        _loopValues_ = ", loopValues(0), ";");
            }
            code.Append(@"
                    }");
            currentMembers.Pop();
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void loop(nodeType node)
        {
            bool isDepth;
            string memberName = node.TemplateMemberName;
            memberNode member = getMember(memberName, out isDepth);
            if (member == null) return;
            pushCode(null);
            string name = path(currentMembers.Count);
            code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
            if (isDepth)
            {
                subString[] names = splitMemberName(memberName);
                ifStart(names[0], true);
                for (int index = 1; index != names.Length - 1; ++index) ifStart(names[index], false);
                loop(getMember(names[names.Length - 1], out isDepth), node, name, names.Length - 1);
            }
            else loop(member, node, name, 0);
            code.Append(@"
                }");
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void push(memberNode member, nodeType node, string name, int popCount)
        {
            code.Append(@"
                    ", name, " = ", member.Path, ";");
            if (popCount != 0) currentMembers.Pop();
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            pushMember(member);
            code.Append(@"
            ", ifName, " = false;");
            ifThen(node, member.Type, name, ifName, false, 0);
            currentMembers.Pop();
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void push(nodeType node)
        {
            bool isDepth;
            string memberName = node.TemplateMemberName;
            memberNode member = getMember(memberName, out isDepth);
            if (member != null && node.ChildCount != 0)
            {
                pushCode(null);
                string name = path(currentMembers.Count);
                code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
                if (isDepth)
                {
                    subString[] names = splitMemberName(memberName);
                    ifStart(names[0], true);
                    for (int index = 1; index != names.Length - 1; ++index) ifStart(names[index], false);
                    push(getMember(names[names.Length - 1], out isDepth), node, name, names.Length - 1);
                }
                else push(member, node, name, 0);
                code.Append(@"
                }");
            }
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="isMember">是否删除当前成员节点</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void ifThen(nodeType node, memberType type, string name, string ifName, bool isMember, int popCount)
        {
            ifStart(type, name, ifName);
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (isMember) currentMembers.Pop();
            code.Append(@"
                }
            if (", ifName, @")
            {");
            skin(node);
            pushCode(null);
            code.Append(@"
            }");
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void ifThen(nodeType node, memberType type, string name, string value, string ifName, int popCount)
        {
            if (type.IsStruct || type.Type.IsEnum)
            {
                code.Append(@"
                if (", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
            }
            else
            {
                code.Append(@"
                if (", name, @" != null && ", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
            }
            code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            code.Append(@"
            if (", ifName, @")
            {");
            skin(node);
            pushCode(null);
            code.Append(@"
            }");
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void ifThen(memberNode member, nodeType node, string value, string ifName, int popCount)
        {
            if (value == null) ifThen(node, member.Type, member.Path, ifName, false, popCount);
            else ifThen(node, member.Type, member.Path, value, ifName, popCount);
        }
        /// <summary>
        /// 绑定的数据为true非0非null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void ifThen(nodeType node)
        {
            string value = null, memberName = node.TemplateMemberName;
            int valueIndex = memberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = memberName.Substring(valueIndex + 1);
                memberName = memberName.Substring(0, valueIndex);
            }
            bool isDepth;
            memberNode member = getMember(memberName, out isDepth);
            if (member == null) return;
            pushCode(null);
            code.Append(@"
            ", ifName, " = false;");
            if (isDepth)
            {
                subString[] names = splitMemberName(memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(names[index], false);
                ifThen(getMember(names[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1);
            }
            else ifThen(member, node, value, ifName, 0);
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void not(memberNode member, nodeType node, string value, string ifName, int popCount)
        {
            if (member.Type.IsStruct || member.Type.Type.IsEnum)
            {
                if (value != null)
                {
                    code.Append(@"
                if (", member.Path, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
                }
                else if (member.Type.IsBool)
                {
                    code.Append(@"
                if (!(bool)", member.Path, ")");
                }
                else
                {
                    code.Append(@"
                if (", member.Path, " == default(", member.Type.FullName, "))");
                }
            }
            else if (value != null)
            {
                code.Append(@"
                if (", member.Path, @" == null || ", member.Path, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
            }
            else
            {
                code.Append(@"
                if (", member.Path, " == null)");
            }
            code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            code.Append(@"
            if (", ifName, @")
            {");
            skin(node);
            pushCode(null);
            code.Append(@"
            }");
        }
        /// <summary>
        /// 绑定的数据为false或者0或者null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void not(nodeType node)
        {
            string value = null, memberName = node.TemplateMemberName;
            int valueIndex = memberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = memberName.Substring(valueIndex + 1);
                memberName = memberName.Substring(0, valueIndex);
            }
            bool isDepth;
            memberNode member = getMember(memberName, out isDepth);
            if (member == null) return;
            pushCode(null);
            code.Append(@"
            ", ifName, " = false;");
            if (isDepth)
            {
                subString[] names = splitMemberName(memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(names[index], false);
                not(getMember(names[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1);
            }
            else not(member, node, value, ifName, 0);
        }
        /// <summary>
        /// 子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void name(nodeType node)
        {
            string memberName = node.TemplateMemberName;
            if (nameNodes.ContainsKey(memberName)) error.Add(viewType.fullName() + " NAME " + memberName + " 重复定义");
            nameNodes[memberName] = node;
            if (node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected abstract nodeType fromNameNode(string typeName, string name);
        /// <summary>
        /// 引用子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void fromName(nodeType node)
        {
            string memberName = node.TemplateMemberName;
            int typeIndex = memberName.IndexOf('.');
            if (typeIndex == -1)
            {
                if (!nameNodes.TryGetValue(memberName, out node)) error.Add(viewType.fullName() + " NAME " + memberName + " 未定义");
            }
            else
            {
                node = fromNameNode(memberName.Substring(0, typeIndex), memberName.Substring(++typeIndex));
            }
            if (node != null && node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 子段程序代码处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void part(nodeType node)
        {
            string memberName = node.TemplateMemberName;
            pushCode(null);
            code.Add(@"
            stringBuilder _PART_" + memberName + @"_ = _code_;
            _code_ = new stringBuilder();");
            stringBuilder historyCode = code;
            code = new stringBuilder();
            skin(node);
            pushCode(null);
            partCodes[memberName] = code.ToString();
            code = historyCode;
            code.Add(partCodes[memberName]);
            code.Add(@"
            _partCodes_[""" + memberName + @"""] = _code_.ToString();
            _code_ = _PART_" + memberName + @"_;
            _code_.Add(_partCodes_[""" + memberName + @"""]);");
        }
    }
}
