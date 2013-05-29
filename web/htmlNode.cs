using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using fastCSharp.net;

namespace fastCSharp.web
{
    /// <summary>
    /// HTML节点
    /// </summary>
    public unsafe class htmlNode
    {
        /// <summary>
        /// 空隔字符位图
        /// </summary>
        private readonly static String.asciiMap spaceMap;
        /// <summary>
        /// 空隔+结束字符位图
        /// </summary>
        private readonly static pointer spaceSplitMap;
        /// <summary>
        /// 标签名称结束字符位图
        /// </summary>
        private readonly static pointer tagNameMap;
        /// <summary>
        /// 标签名称开始字符位图
        /// </summary>
        private readonly static pointer tagNameSplitMap;
        /// <summary>
        /// 标签属性分隔结束字符位图
        /// </summary>
        private readonly static pointer attributeSplitMap;
        /// <summary>
        /// 标签属性名称结束字符位图
        /// </summary>
        private readonly static pointer attributeNameSplitMap;
        /// <summary>
        /// 初始化特殊属性名称集合(不能用全小写字母表示的属性名称)
        /// </summary>
        private readonly static staticDictionary<string, string> noLowerAttributeNames = new staticDictionary<string, string>(nullValue<keyValue<string, string>>.Array);
        /// <summary>
        /// 检测属性名称
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性名称</returns>
        private static string checkName(string name)
        {
            return noLowerAttributeNames.Get(name, name);
        }
        /// <summary>
        /// HTML编码与文本值
        /// </summary>
        private struct htmlText
        {
            /// <summary>
            /// 编码过的HTML编码
            /// </summary>
            public string FormatHtml;
            /// <summary>
            /// HTML编码
            /// </summary>
            public string Html
            {
                get
                {
                    if (FormatHtml == null && FormatText != null) FormatHtml = HttpUtility.HtmlEncode(FormatText);
                    return FormatHtml;
                }
                set
                {
                    FormatHtml = value;
                    FormatText = null;
                }
            }
            /// <summary>
            /// 编码过的HTML文本值
            /// </summary>
            public string FormatText;
            /// <summary>
            /// HTML文本值
            /// </summary>
            public string Text
            {
                get
                {
                    if (FormatText == null && FormatHtml != null) FormatText = HttpUtility.HtmlDecode(FormatHtml);
                    return FormatText;
                }
                set
                {
                    FormatText = value;
                    FormatHtml = null;
                }
            }
        }
        /// <summary>
        /// 文本节点值
        /// </summary>
        private htmlText nodeText;
        /// <summary>
        /// 父节点
        /// </summary>
        public htmlNode Parent { get; private set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; private set; }
        /// <summary>
        /// 子节点集合
        /// </summary>
        private list<htmlNode> children;
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildrenCount
        {
            get { return children.count(); }
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        public htmlNode[] Children
        {
            get { return children == null ? null : children.ToArray(); }
        }
        /// <summary>
        /// 子节点索引位置
        /// </summary>
        /// <param name="value">子节点</param>
        /// <returns>索引位置</returns>
        public int this[htmlNode value]
        {
            get
            {
                int index = -1;
                if (value != null && value.Parent == this && children != null)
                {
                    index = children.Count;
                    while (--index >= 0 && children[index] != value) ;
                }
                return index;
            }
        }
        /// <summary>
        /// 属性
        /// </summary>
        private Dictionary<string, htmlText> attributes;
        /// <summary>
        /// 获取或设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public string this[string name]
        {
            get
            {
                if (attributes != null)
                {
                    htmlText value = new htmlText();
                    if (name != null && name.Length != 0 && attributes.TryGetValue(name = checkName(name.ToLower()), out value))
                    {
                        if (value.FormatText != value.Text) attributes[name] = value;
                    }
                    return value.Text;
                }
                return null;
            }
            set
            {
                if (name != null && name.Length != 0)
                {
                    if (value != null)
                    {
                        if (attributes == null) attributes = new Dictionary<string, htmlText>();
                        attributes[checkName(name.ToLower())] = new htmlText { FormatText = value };
                    }
                    else if (attributes != null) attributes.Remove(checkName(name.ToLower()));
                }
            }
        }
        /// <summary>
        /// 获取或设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>是否存在属性名称</returns>
        public bool Get(string name, ref string value)
        {
            if (attributes != null)
            {
                htmlText attribute = new htmlText();
                if (name != null && name.Length != 0 && attributes.TryGetValue(name = checkName(name.ToLower()), out attribute))
                {
                    value = attribute.Text;
                    if (attribute.FormatText != attribute.Text) attributes[name] = attribute;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 属性名称集合
        /// </summary>
        public string[] AttributeNames
        {
            get
            {
                return attributes != null ? attributes.Keys.getArray() : nullValue<string>.Array;
            }
        }
        /// <summary>
        /// HTML节点
        /// </summary>
        private htmlNode() { }
        /// <summary>
        /// 根据HTML解析节点
        /// </summary>
        /// <param name="html">HTML</param>
        public htmlNode(string html)
        {
            createCheck(html);
        }
        /// <summary>
        /// 根据网址解析HTML节点
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="encoding">编码</param>
        public htmlNode(Uri url, Encoding encoding = null)
        {
            using (webClient webClient = new webClient()) createCheck(webClient.CrawlHtml(new webClient.request { Uri = url }, encoding));
        }
        /// <summary>
        /// 根据HTML解析节点
        /// </summary>
        /// <param name="html"></param>
        private void createCheck(string html)
        {
            if (html != null && html.Length != 0)
            {
                char endChar = html[html.Length - 1];
                if (!spaceMap.Get(endChar)) html = html + " ";
                try
                {
                    create(html);
                }
                catch (Exception error)
                {
                    log.Default.Add(error, null, false);
                }
            }
            TagName = string.Empty;
        }
        /// <summary>
        /// 解析HTML节点
        /// </summary>
        /// <param name="html"></param>
        private void create(string html)
        {
            int length = html.Length;
            children = new list<htmlNode>();
            if (length < 2)
            {
                children.Add(new htmlNode { nodeText = new htmlText { FormatHtml = html }, Parent = this });
            }
            else
            {
                int nextIndex, nodeCount;
                htmlNode nextNode;
                fixed (char* htmlFixed = html + "<")
                {
                    fixedMap spaceFixedMap = new fixedMap(spaceMap.Map);
                    fixedMap spaceSplitFixedMap = new fixedMap(spaceSplitMap);
                    fixedMap tagNameFixedMap = new fixedMap(tagNameMap);
                    fixedMap tagNameSplitFixedMap = new fixedMap(tagNameSplitMap);
                    fixedMap attributeSplitFixedMap = new fixedMap(attributeSplitMap);
                    fixedMap attributeNameSplitFixedMap = new fixedMap(attributeNameSplitMap);
                    int startIndex, tagNameLength;
                    string name, htmlValue;
                    char* startChar = htmlFixed, currentChar = htmlFixed, endChar = htmlFixed + length, scriptChar;
                    char splitChar;
                    while (currentChar != endChar)
                    {
                        for (*endChar = '<'; *currentChar != '<'; ++currentChar) ;
                        if (currentChar != endChar)
                        {
                            if ((*++currentChar & 0xff80) == 0)
                            {
                                if (tagNameFixedMap.Get(*currentChar))
                                {
                                    while ((*startChar & 0xffc0) == 0 && spaceFixedMap.Get(*startChar)) ++startChar;
                                    if (startChar != currentChar - 1)
                                    {
                                        for (scriptChar = currentChar - 2; (*scriptChar & 0xffc0) == 0 && spaceFixedMap.Get(*scriptChar); --scriptChar) ;
                                        children.Add(new htmlNode { nodeText = new htmlText { FormatHtml = html.Substring((int)(startChar - htmlFixed), (int)(scriptChar - startChar) + 1) } });
                                    }
                                    if (*currentChar == '/')
                                    {
                                        #region 标签回合
                                        startChar = currentChar - 1;
                                        if (++currentChar != endChar)
                                        {
                                            while ((*currentChar & 0xffc0) == 0 && spaceFixedMap.Get(*currentChar)) ++currentChar;
                                            if (currentChar != endChar)
                                            {
                                                if ((uint)((*currentChar | 0x20) - 'a') <= 26)
                                                {
                                                    for (*endChar = '>'; (*currentChar & 0xffc0) != 0 || !tagNameSplitFixedMap.Get(*currentChar); ++currentChar) ;
                                                    TagName = html.Substring((int)((startChar += 2) - htmlFixed), (int)(currentChar - startChar)).toLower();
                                                    for (startIndex = children.Count - 1; startIndex >= 0 && (children[startIndex].nodeText.FormatHtml != null || children[startIndex].TagName != TagName); --startIndex) ;
                                                    if (startIndex != -1)
                                                    {
                                                        for (nextIndex = children.Count - 1; nextIndex != startIndex; --nextIndex)
                                                        {
                                                            nextNode = children[nextIndex];
                                                            if (nextNode.nodeText.FormatHtml == null)
                                                            {
                                                                if (web.html.MustRoundTagNames.Contains(nextNode.TagName) && (nodeCount = (children.Count - nextIndex - 1)) != 0)
                                                                {
                                                                    nextNode.children = new list<htmlNode>(children.GetSub(nextIndex + 1, nodeCount), true);
                                                                    children.RemoveRange(nextIndex + 1, nodeCount);
                                                                    foreach (htmlNode value in nextNode.children) value.Parent = nextNode;
                                                                }
                                                            }
                                                            else if (nextNode.nodeText.FormatHtml.Length == 0) nextNode.nodeText.FormatHtml = null;
                                                        }
                                                        nextNode = children[startIndex];
                                                        if ((nodeCount = children.Count - ++startIndex) != 0)
                                                        {
                                                            nextNode.children = new list<htmlNode>(children.GetSub(startIndex, nodeCount), true);
                                                            children.RemoveRange(startIndex, nodeCount);
                                                            foreach (htmlNode value in nextNode.children) value.Parent = nextNode;
                                                        }
                                                        nextNode.nodeText.FormatHtml = string.Empty;//已回合标识
                                                    }
                                                    while (*currentChar != '>') ++currentChar;
                                                    if (currentChar != endChar) ++currentChar;
                                                }
                                                else
                                                {
                                                    for (*endChar = '>'; *currentChar != '>'; ++currentChar) ;
                                                    if (currentChar != endChar) ++currentChar;
                                                    htmlValue = html.Substring((int)(startChar - htmlFixed), (int)(currentChar - startChar));
                                                    children.Add(new htmlNode { TagName = "/", nodeText = new htmlText { FormatHtml = htmlValue, FormatText = htmlValue } });
                                                }
                                                startChar = currentChar;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (*currentChar != '!')
                                    {
                                        #region 标签开始
                                        startChar = currentChar;
                                        children.Add(nextNode = new htmlNode());
                                        for (*endChar = '>'; (*currentChar & 0xffc0) != 0 || !tagNameSplitFixedMap.Get(*currentChar); ++currentChar) ;
                                        nextNode.TagName = html.Substring((int)(startChar - htmlFixed), (int)(currentChar - startChar)).toLower();
                                        if (currentChar == endChar) startChar = endChar;
                                        else
                                        {
                                            #region 属性解析
                                            if (*currentChar != '>')
                                            {
                                                startChar = ++currentChar;
                                                while (currentChar != endChar)
                                                {
                                                    while ((*currentChar & 0xffc0) == 0 && attributeSplitFixedMap.Get(*currentChar)) ++currentChar;
                                                    if (*currentChar == '>')
                                                    {
                                                        if (currentChar != endChar)
                                                        {
                                                            if (*(currentChar - 1) == '/') nextNode.nodeText.FormatHtml = string.Empty;
                                                            startChar = ++currentChar;
                                                        }
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        for (startChar = currentChar++; (*currentChar & 0xffc0) != 0 || !tagNameSplitFixedMap.Get(*currentChar); ++currentChar) ;
                                                        htmlValue = name = checkName(html.Substring((int)(startChar - htmlFixed), (int)(currentChar - startChar)).toLower());
                                                        if (currentChar != endChar && ((*currentChar & 0xffc0) != 0 || !attributeNameSplitFixedMap.Get(*currentChar)))
                                                        {
                                                            if (*currentChar != '=')
                                                            {
                                                                while ((*currentChar & 0xffc0) == 0 && spaceFixedMap.Get(*currentChar)) ++currentChar;
                                                            }
                                                            if (*currentChar == '=')
                                                            {
                                                                while ((*++currentChar & 0xffc0) == 0 && spaceFixedMap.Get(*currentChar)) ;
                                                                if ((splitChar = *currentChar) != '>')
                                                                {
                                                                    if (splitChar == '"' || splitChar == '\'')
                                                                    {
                                                                        for (startChar = ++currentChar, *endChar = splitChar; *currentChar != splitChar; ++currentChar) ;
                                                                        *endChar = '>';
                                                                    }
                                                                    else
                                                                    {
                                                                        for (startChar = currentChar++; (*currentChar & 0xffc0) != 0 || !spaceSplitFixedMap.Get(*currentChar); ++currentChar) ;
                                                                    }
                                                                    htmlValue = html.Substring((int)(startChar - htmlFixed), (int)(currentChar - startChar));
                                                                }
                                                            }
                                                        }
                                                        if (nextNode.attributes == null) nextNode.attributes = new Dictionary<string, htmlText>();
                                                        nextNode.attributes[name] = new htmlText { FormatHtml = htmlValue };
                                                        if (currentChar != endChar)
                                                        {
                                                            if (*currentChar == '>')
                                                            {
                                                                if (*(currentChar - 1) == '/') nextNode.nodeText.FormatHtml = string.Empty;
                                                                startChar = ++currentChar;
                                                                break;
                                                            }
                                                            startChar = ++currentChar;
                                                        }
                                                    }
                                                }
                                            }
                                            else startChar = ++currentChar;
                                            #endregion

                                            #region 非解析标签
                                            if (currentChar == endChar) startChar = endChar;
                                            else if (web.html.NonanalyticTagNames.Contains(TagName = nextNode.TagName))
                                            {
                                                scriptChar = endChar;
                                                tagNameLength = TagName.Length + 2;
                                                fixed (char* tagNameFixed = TagName)
                                                {
                                                    while ((int)(endChar - currentChar) > tagNameLength)
                                                    {
                                                        for (currentChar += tagNameLength; *currentChar != '>'; ++currentChar) ;
                                                        if (currentChar != endChar && *(int*)(currentChar - tagNameLength) == (('/' << 16) + '<'))
                                                        {
                                                            if (unsafer.String.EqualCase(currentChar - TagName.Length, tagNameFixed, TagName.Length))
                                                            {
                                                                scriptChar = currentChar - tagNameLength;
                                                                if (currentChar != endChar) ++currentChar;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (startChar != scriptChar)
                                                {
                                                    nextNode.nodeText.FormatHtml = nextNode.nodeText.FormatText = html.Substring((int)(startChar - htmlFixed), (int)(scriptChar - startChar));
                                                }
                                                if (scriptChar == endChar) currentChar = endChar;
                                                startChar = currentChar;
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 注释
                                        startChar = currentChar - 1;
                                        if (++currentChar != endChar)
                                        {
                                            *endChar = '>';
                                            if ((length = (int)(endChar - currentChar)) > 2 && *(int*)currentChar == (('-' << 16) + '-'))
                                            {
                                                for (currentChar += 2; *currentChar != '>'; ++currentChar) ;
                                                while (currentChar != endChar && *(int*)(currentChar - 2) != (('-' << 16) + '-'))
                                                {
                                                    if ((currentChar += 3) < endChar)
                                                    {
                                                        while (*currentChar != '>') ++currentChar;
                                                    }
                                                    else currentChar = endChar;
                                                }
                                            }
                                            else if (length > 9
                                                && (*(int*)currentChar & 0x200000) == ('[' + ('c' << 16))
                                                && (*(int*)(currentChar + 2) & 0x200020) == ('d' + ('a' << 16))
                                                && (*(int*)(currentChar + 4) & 0x200020) == ('t' + ('a' << 16))
                                                && *(currentChar + 6) == '[')
                                            {
                                                for (currentChar += 9; *currentChar != '>'; ++currentChar) ;
                                                while (currentChar != endChar && *(int*)(currentChar - 2) != ((']' << 16) + ']'))
                                                {
                                                    if ((currentChar += 3) < endChar)
                                                    {
                                                        while (*currentChar != '>') ++currentChar;
                                                    }
                                                    else currentChar = endChar;
                                                }
                                            }
                                            else
                                            {
                                                while (*currentChar != '>') ++currentChar;
                                            }
                                            if (currentChar != endChar) ++currentChar;
                                        }
                                        htmlValue = html.Substring((int)(startChar - htmlFixed), (int)(currentChar - startChar) + (*(currentChar - 1) == '>' ? 0 : 1));
                                        children.Add(new htmlNode { TagName = "!", nodeText = new htmlText { FormatHtml = htmlValue, FormatText = htmlValue } });
                                        startChar = currentChar;
                                        #endregion
                                    }
                                }
                            }
                            else ++currentChar;
                        }
                    }
                    if (startChar != endChar)
                    {
                        *endChar = '>';
                        while ((*startChar & 0xffc0) == 0 && spaceFixedMap.Get(*startChar)) ++startChar;
                        if (startChar != endChar)
                        {
                            for (scriptChar = endChar - 1; (*scriptChar & 0xffc0) == 0 && spaceFixedMap.Get(*scriptChar); --scriptChar) ;
                            children.Add(new htmlNode { nodeText = new htmlText { FormatHtml = html.Substring((int)(startChar - htmlFixed), (int)(scriptChar - startChar) + 1) } });
                        }
                    }
                }
                for (nextIndex = children.Count - 1; nextIndex != -1; nextIndex--)
                {
                    nextNode = children[nextIndex];
                    if (nextNode.nodeText.FormatHtml == null)
                    {
                        if (web.html.MustRoundTagNames.Contains(nextNode.TagName)
                            && (nodeCount = (children.Count - nextIndex - 1)) != 0)
                        {
                            nextNode.children = new list<htmlNode>(children.GetSub(nextIndex + 1, nodeCount), true);
                            children.RemoveRange(nextIndex + 1, nodeCount);
                            foreach (htmlNode value in children) value.Parent = nextNode;
                        }
                    }
                    else if (nextNode.nodeText.FormatHtml.Length == 0) nextNode.nodeText.FormatHtml = null;
                }
                foreach (htmlNode value in children) value.Parent = this;
            }
        }
        /// <summary>
        /// 节点索引
        /// </summary>
        private struct nodeIndex
        {
            /// <summary>
            /// 节点集合
            /// </summary>
            public list<htmlNode> Values;
            /// <summary>
            /// 当前访问位置
            /// </summary>
            public int Index;
        }
        /// <summary>
        /// 子孙节点枚举
        /// </summary>
        public IEnumerable<htmlNode> Nodes
        {
            get
            {
                if (children.count() != 0)
                {
                    htmlNode node;
                    list<nodeIndex> values = new list<nodeIndex>();
                    nodeIndex index = new nodeIndex { Values = children };
                    while (true)
                    {
                        if (index.Values == null)
                        {
                            if (values.Count == 0) break;
                            else index = values.Pop();
                        }
                        yield return node = index.Values[index.Index];
                        if (node.children.count() == 0)
                        {
                            if (++index.Index == index.Values.Count) index.Values = null;
                        }
                        else
                        {
                            if (++index.Index != index.Values.Count) values.Add(index);
                            index.Values = node.children;
                            index.Index = 0;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 节点筛选器
        /// </summary>
        private class filter
        {
            /// <summary>
            /// 功能字符集合
            /// </summary>
            public const string Filters = @"\/.#*:@";
            /// <summary>
            /// 功能字符位图
            /// </summary>
            private static readonly pointer filterMap = new String.asciiMap(unmanaged.Get(String.asciiMap.mapBytes), Filters, true).Pointer;
            /// <summary>
            /// 节点筛选器解析缓存
            /// </summary>
            private static readonly Dictionary<string, filter> cache = new Dictionary<string, filter>();
            /// <summary>
            /// 当前筛选节点功能调用
            /// </summary>
            private func<filter, keyValue<list<htmlNode>, bool>, keyValue<list<htmlNode>, bool>> filterMethod;
            /// <summary>
            /// 下级筛选器
            /// </summary>
            private filter nextFilter;
            /// <summary>
            /// 当前筛选节点匹配名称
            /// </summary>
            private string name;
            /// <summary>
            /// 当前筛选节点匹配值
            /// </summary>
            private string value;
            /// <summary>
            /// 当前筛选节点匹配多值集合
            /// </summary>
            private staticHashSet<string> values;
            /// <summary>
            /// 当前筛选节点匹配位置
            /// </summary>
            private int index = -1;
            /// <summary>
            /// 当前筛选节点匹配多位置集合
            /// </summary>
            private staticHashSet<int> indexs;
            /// <summary>
            /// 功能字符位图
            /// </summary>
            private byte* filterFixedMap;
            /// <summary>
            /// 节点筛选器解析
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            private filter(char* start, char* end)
            {
                filterFixedMap = filterMap.Byte;
                switch (*start)
                {
                    case '/':
                        filterMethod = filterChild;
                        if (++start != end && start != (end = next(start, end)))
                        {
                            char* index = end;
                            if (*--index == ']' && (index = unsafer.String.Find(start, index, '[')) != null)
                            {
                                unsafer.String.ToLower(start, index);
                                getValue(start, index);
                                getIndex(++index, --end);
                            }
                            else
                            {
                                unsafer.String.ToLower(start, end);
                                getValues(start, end);
                            }
                        }
                        break;
                    case '.':
                        filterMethod = filterClass;
                        if (++start != end) getValues(start, end = next(start, end));
                        break;
                    case '#':
                        name = "id";
                        filterMethod = filterValue;
                        if (++start != end) getValues(start, end = next(start, end));
                        break;
                    case '*':
                        name = "name";
                        filterMethod = filterValue;
                        if (++start != end)
                        {
                            end = next(start, end);
                            unsafer.String.ToLower(start, end);
                            getValues(start, end);
                        }
                        break;
                    case ':':
                        name = "type";
                        filterMethod = filterValue;
                        if (++start != end) getValues(start, end = next(start, end));
                        break;
                    case '@':
                        filterMethod = filterValue;
                        if (++start != end)
                        {
                            end = next(start, end);
                            char* value = unsafer.String.Find(start, end, '=');
                            if (value != null)
                            {
                                getName(start, value);
                                if (++value == end) this.value = string.Empty;
                                else getValues(value, end);
                            }
                            else getName(start, end);
                        }
                        break;
                    default:
                        filterMethod = filterNode;
                        if (*start == '\\') ++start;
                        if (start != end)
                        {
                            end = next(start, end);
                            unsafer.String.ToLower(start, end);
                            getValues(start, end);
                        }
                        break;
                }
            }
            /// <summary>
            /// 解析下一个筛选功能
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            /// <returns>字符位置</returns>
            private char* next(char* start, char* end)
            {
                start = unsafer.String.findAscii(start, end, filterFixedMap, Filters[0]);
                if (start != null)
                {
                    nextFilter = new filter(start, end);
                    return start;
                }
                return end;
            }
            /// <summary>
            /// 解析多值集合
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            private void getValues(char* start, char* end)
            {
                if (start != end)
                {
                    value = new string(start, 0, (int)(end - start));
                    if (unsafer.String.Find(start, end, ',') != null)
                    {
                        values = new staticHashSet<string>(value.Split(','));
                        value = null;
                    }
                }
            }
            /// <summary>
            /// 解析值
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            private void getValue(char* start, char* end)
            {
                if (start != end) value = new string(start, 0, (int)(end - start));
            }
            /// <summary>
            /// 解析名称
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            private void getName(char* start, char* end)
            {
                if (start != end)
                {
                    unsafer.String.ToLower(start, end);
                    name = htmlNode.checkName(new string(start, 0, (int)(end - start)));
                }
            }
            /// <summary>
            /// 解析索引位置
            /// </summary>
            /// <param name="start">起始字符位置</param>
            /// <param name="end">结束字符位置</param>
            private void getIndex(char* start, char* end)
            {
                if (start != end)
                {
                    value = new string(start, 0, (int)(end - start));
                    if (unsafer.String.Find(start, end, ',') == null)
                    {
                        if (!int.TryParse(value, out index)) index = -1;
                    }
                    else
                    {
                        list<int> indexs = value.splitIntNoCheck(',');
                        if (indexs.Count != 0)
                        {
                            if (indexs.Count != 1) this.indexs = new staticHashSet<int>(indexs);
                            else index = (int)indexs[0];
                        }
                    }
                    value = null;
                }
            }
            /// <summary>
            /// 获取匹配HTML节点集合
            /// </summary>
            /// <param name="value">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            private list<htmlNode> get(keyValue<list<htmlNode>, bool> value)
            {
                value = filterMethod(this, value);
                return nextFilter == null || value.Key == null ? value.Key : nextFilter.get(value);
            }
            /// <summary>
            /// 根据筛选路径解析筛选器
            /// </summary>
            /// <param name="path">筛选路径</param>
            /// <returns>筛选器</returns>
            private static filter get(string path)
            {
                filter value;
                if (!cache.TryGetValue(path, out value))
                {
                    fixed (char* pathFixed = path)
                    {
                        cache[path] = value = new filter(pathFixed, pathFixed + path.Length);
                    }
                }
                return value;
            }
            /// <summary>
            /// 根据筛选路径值匹配HTML节点集合
            /// </summary>
            /// <param name="path">筛选路径</param>
            /// <param name="node">筛选节点</param>
            /// <returns>匹配的HTML节点集合</returns>
            public static list<htmlNode> Get(string path, htmlNode node)
            {
                if (path != null && path.Length != 0)
                {
                    list<htmlNode> nodes = new list<htmlNode>();
                    nodes.Add(node);
                    return get(path).get(new keyValue<list<htmlNode>, bool>(nodes, false));
                }
                return null;
            }
            /// <summary>
            /// 根据筛选路径值匹配HTML节点集合
            /// </summary>
            /// <param name="path">筛选路径</param>
            /// <param name="nodes">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            public static list<htmlNode> Get(string path, list<htmlNode> nodes)
            {
                if (path != null && path.Length != 0)
                {
                    return get(path).get(new keyValue<list<htmlNode>, bool>(nodes, true)) ?? new list<htmlNode>();
                }
                return null;
            }

            /// <summary>
            /// 子孙节点筛选
            /// </summary>
            /// <param name="path">筛选器</param>
            /// <param name="value">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            private static keyValue<list<htmlNode>, bool> filterNode(filter path, keyValue<list<htmlNode>, bool> value)
            {
                list<nodeIndex> values = new list<nodeIndex>();
                nodeIndex index = new nodeIndex { Values = value.Key.getList() };
                if (index.Values.Count != 0)
                {
                    if (value.Value)
                    {
                        HashSet<htmlNode> newValues = new HashSet<htmlNode>(), historyNodes = new HashSet<htmlNode>();
                        if (path.values == null)
                        {
                            if (path.value != null)
                            {
                                string tagName = path.value;
                                while (true)
                                {
                                    if (index.Values == null)
                                    {
                                        if (values.Count == 0) break;
                                        else index = values.Pop();
                                    }
                                    htmlNode node = index.Values[index.Index];
                                    if (node.TagName == tagName) newValues.Add(node);
                                    if (node.children.count() == 0 || historyNodes.Contains(node))
                                    {
                                        if (++index.Index == index.Values.Count) index.Values = null;
                                    }
                                    else
                                    {
                                        if (++index.Index != index.Values.Count) values.Add(index);
                                        historyNodes.Add(node);
                                        index.Values = node.children;
                                        index.Index = 0;
                                    }
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    if (index.Values == null)
                                    {
                                        if (values.Count == 0) break;
                                        else index = values.Pop();
                                    }
                                    htmlNode node = index.Values[index.Index];
                                    newValues.Add(node);
                                    if (node.children.count() == 0 || historyNodes.Contains(node))
                                    {
                                        if (++index.Index == index.Values.Count) index.Values = null;
                                    }
                                    else
                                    {
                                        if (++index.Index != index.Values.Count) values.Add(index);
                                        historyNodes.Add(node);
                                        index.Values = node.children;
                                        index.Index = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            staticHashSet<string> tagNames = path.values;
                            while (true)
                            {
                                if (index.Values == null)
                                {
                                    if (values.Count == 0) break;
                                    else index = values.Pop();
                                }
                                htmlNode node = index.Values[index.Index];
                                if (tagNames.Contains(node.TagName)) newValues.Add(node);
                                if (node.children.count() == 0 || historyNodes.Contains(node))
                                {
                                    if (++index.Index == index.Values.Count) index.Values = null;
                                }
                                else
                                {
                                    if (++index.Index != index.Values.Count) values.Add(index);
                                    historyNodes.Add(node);
                                    index.Values = node.children;
                                    index.Index = 0;
                                }
                            }
                        }
                        if (newValues.Count != 0)
                        {
                            return new keyValue<list<htmlNode>, bool>(new list<htmlNode>(newValues), newValues.Count > 1);
                        }
                    }
                    else
                    {
                        list<htmlNode> newValues = new list<htmlNode>();
                        if (path.values == null)
                        {
                            if (path.value != null)
                            {
                                string tagName = path.value;
                                while (true)
                                {
                                    if (index.Values == null)
                                    {
                                        if (values.Count == 0) break;
                                        else index = values.Pop();
                                    }
                                    htmlNode node = index.Values[index.Index];
                                    if (node.TagName == tagName) newValues.Add(node);
                                    if (node.children.count() == 0)
                                    {
                                        if (++index.Index == index.Values.Count) index.Values = null;
                                    }
                                    else
                                    {
                                        if (++index.Index != index.Values.Count) values.Add(index);
                                        index.Values = node.children;
                                        index.Index = 0;
                                    }
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    if (index.Values == null)
                                    {
                                        if (values.Count == 0) break;
                                        else index = values.Pop();
                                    }
                                    htmlNode node = index.Values[index.Index];
                                    newValues.Add(node);
                                    if (node.children.count() == 0)
                                    {
                                        if (++index.Index == index.Values.Count) index.Values = null;
                                    }
                                    else
                                    {
                                        if (++index.Index != index.Values.Count) values.Add(index);
                                        index.Values = node.children;
                                        index.Index = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            staticHashSet<string> tagNames = path.values;
                            while (true)
                            {
                                if (index.Values == null)
                                {
                                    if (values.Count == 0) break;
                                    else index = values.Pop();
                                }
                                htmlNode node = index.Values[index.Index];
                                if (tagNames.Contains(node.TagName)) newValues.Add(node);
                                if (node.children.count() == 0)
                                {
                                    if (++index.Index == index.Values.Count) index.Values = null;
                                }
                                else
                                {
                                    if (++index.Index != index.Values.Count) values.Add(index);
                                    index.Values = node.children;
                                    index.Index = 0;
                                }
                            }
                        }
                        if (newValues.Count != 0)
                        {
                            return new keyValue<list<htmlNode>, bool>(newValues, newValues.Count > 1);
                        }
                    }
                }
                return new keyValue<list<htmlNode>, bool>(null, false);
            }
            /// <summary>
            /// class样式筛选
            /// </summary>
            /// <param name="path">筛选器</param>
            /// <param name="value">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            private static keyValue<list<htmlNode>, bool> filterClass(filter path, keyValue<list<htmlNode>, bool> value)
            {
                list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                if (path.values == null)
                {
                    string name = path.value;
                    foreach (htmlNode node in value.Key)
                    {
                        string className = node["class"];
                        if (className != null && className.Split(' ').indexOf(name) != -1) newValues.Add(node);
                    }
                }
                else
                {
                    staticHashSet<string> names = path.values;
                    foreach (htmlNode node in value.Key)
                    {
                        string className = node["class"];
                        if (className != null)
                        {
                            foreach(string name in className.Split(' '))
                            {
                                if (names.Contains(name))
                                {
                                    newValues.Add(node);
                                    break;
                                }
                            }
                        }
                    }
                }
                return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
            }
            /// <summary>
            /// 子节点筛选
            /// </summary>
            /// <param name="path">筛选器</param>
            /// <param name="value">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            private static keyValue<list<htmlNode>, bool> filterChild(filter path, keyValue<list<htmlNode>, bool> value)
            {
                if (path.index < 0)
                {
                    if (path.indexs == null)
                    {
                        if (path.values == null)
                        {
                            if (path.value != null)
                            {
                                string tagName = path.value;
                                list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                                foreach (htmlNode nodes in value.Key)
                                {
                                    if (nodes.children.count() > 0)
                                    {
                                        foreach (htmlNode node in nodes.children)
                                        {
                                            if (node.TagName == tagName) newValues.Add(node);
                                        }
                                    }
                                }
                                return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
                            }
                            else
                            {
                                int index = 0;
                                foreach (htmlNode nodes in value.Key) if (nodes.children != null) index += nodes.children.Count;
                                if (index != 0)
                                {
                                    htmlNode[] newValues = new htmlNode[index];
                                    index = 0;
                                    foreach (htmlNode nodes in value.Key)
                                    {
                                        if (nodes.children != null)
                                        {
                                            nodes.children.CopyTo(newValues, index);
                                            index += nodes.children.Count;
                                        }
                                    }
                                    return new keyValue<list<htmlNode>, bool>(new list<htmlNode>(newValues, true), value.Value && newValues.Length != 1);
                                }
                                return new keyValue<list<htmlNode>, bool>(null, false);
                            }
                        }
                        else
                        {
                            staticHashSet<string> tagNames = path.values;
                            list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                            foreach (htmlNode nodes in value.Key)
                            {
                                if (nodes.children.count() != 0)
                                {
                                    foreach (htmlNode node in nodes.children)
                                    {
                                        if (tagNames.Contains(node.TagName)) newValues.Add(node);
                                    }
                                }
                            }
                            return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
                        }
                    }
                    else
                    {
                        list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                        if (path.value != null)
                        {
                            string tagName = path.value;
                            staticHashSet<int> indexs = path.indexs;
                            foreach (htmlNode nodes in value.Key)
                            {
                                if (nodes.children.count() != 0)
                                {
                                    int index = 0;
                                    foreach (htmlNode node in nodes.children)
                                    {
                                        if (node.TagName == tagName)
                                        {
                                            if (indexs.Contains(index)) newValues.Add(node);
                                            ++index;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int[] indexs = path.indexs.GetList().ToArray();
                            foreach (htmlNode nodes in value.Key)
                            {
                                int count = nodes.children.count();
                                if (count > 0)
                                {
                                    list<htmlNode> children = nodes.children;
                                    for (int index = indexs.Length; --index >= 0; )
                                    {
                                        if (index < count) newValues.Add(children[index]);
                                    }
                                }
                            }
                        }
                        return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
                    }
                }
                else
                {
                    list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                    if (path.value != null)
                    {
                        string tagName = path.value;
                        int index = path.index;
                        foreach (htmlNode nodes in value.Key)
                        {
                            if (nodes.children.count() != 0)
                            {
                                int count = 0;
                                foreach (htmlNode node in nodes.children)
                                {
                                    if (node.TagName == tagName)
                                    {
                                        if (count == index)
                                        {
                                            newValues.Add(node);
                                            break;
                                        }
                                        ++count;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int index = path.index;
                        foreach (htmlNode nodes in value.Key)
                        {
                            if (index < nodes.children.count()) newValues.List.Add(nodes.children[index]);
                        }
                    }
                    return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
                }
            }
            /// <summary>
            /// 属性值筛选
            /// </summary>
            /// <param name="path">筛选器</param>
            /// <param name="value">筛选节点集合</param>
            /// <returns>匹配的HTML节点集合</returns>
            private static keyValue<list<htmlNode>, bool> filterValue(filter path, keyValue<list<htmlNode>, bool> value)
            {
                string name = path.name;
                list<htmlNode>.unsafer newValues = new list<htmlNode>(value.Key.Count).Unsafer;
                if (path.values == null && path.value == null)
                {
                    foreach (htmlNode node in value.Key)
                    {
                        if (node.attributes != null && node.attributes.ContainsKey(name)) newValues.Add(node);
                    }
                }
                else
                {
                    if (path.values == null)
                    {
                        string nameValue = path.value;
                        foreach (htmlNode node in value.Key)
                        {
                            if (node[name] == nameValue) newValues.Add(node);
                        }
                    }
                    else
                    {
                        staticHashSet<string> values = path.values;
                        foreach (htmlNode node in value.Key)
                        {
                            if (values.Contains(node[name])) newValues.Add(node);
                        }
                    }
                }
                return new keyValue<list<htmlNode>, bool>(newValues.List.Count != 0 ? newValues.List : null, value.Value && newValues.List.Count > 1);
            }
        }
        /// <summary>
        /// 根据筛选路径值匹配HTML节点集合
        /// </summary>
        /// <param name="value">筛选路径</param>
        /// <param name="nodes">筛选节点集合</param>
        /// <returns>匹配HTML节点集合</returns>
        public static list<htmlNode> Path(string value, list<htmlNode> nodes)
        {
            return filter.Get(value, nodes);
        }
        /// <summary>
        /// 根据筛选路径值匹配HTML节点集合
        /// </summary>
        /// <param name="value">筛选路径</param>
        /// <returns>匹配HTML节点集合</returns>
        public list<htmlNode> Path(string value)
        {
            return filter.Get(value, this);
        }
        /// <summary>
        /// 删除匹配的子孙节点
        /// </summary>
        /// <param name="isRemove">删除节点匹配器</param>
        public void Remove(func<htmlNode, bool> isRemove)
        {
            if (isRemove != null)
            {
                removeChilds(isRemove);
                if (children.count() != 0)
                {
                    htmlNode node;
                    list<nodeIndex> values = new list<nodeIndex>();
                    nodeIndex index = new nodeIndex { Values = children };
                    while (true)
                    {
                        if (index.Values == null)
                        {
                            if (values.Count == 0) break;
                            else index = values.Pop();
                        }
                        node = index.Values[index.Index];
                        if (node.children != null) node.removeChilds(isRemove);
                        if (node.children.count() == 0)
                        {
                            if (++index.Index == index.Values.Count) index.Values = null;
                        }
                        else
                        {
                            if (++index.Index != index.Values.Count) values.Add(index);
                            index.Values = node.children;
                            index.Index = 0;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 根据节点名称获取第一个子节点
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>第一个匹配子节点</returns>
        public htmlNode GetFirstChildByTagName(string tagName)
        {
            if (children != null)
            {
                string lowerTagName = tagName.ToLower();
                foreach (htmlNode value in children)
                {
                    if (value.TagName == lowerTagName) return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据节点名称获取子节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>子节点集合</returns>
        public list<htmlNode> GetChildsByTagName(string tagName)
        {
            return tagName != null && tagName.Length != 0 ? getChildsByTagName(tagName.ToLower()) : new list<htmlNode>();
        }
        /// <summary>
        /// 根据节点名称获取子节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>子节点集合</returns>
        public list<htmlNode> getChildsByTagName(string tagName)
        {
            list<htmlNode>.unsafer values = new list<htmlNode>(children.count()).Unsafer;
            if (children != null)
            {
                foreach (htmlNode value in children)
                {
                    if (value.TagName == tagName) values.Add(value);
                }
            }
            return values.List;
        }
        /// <summary>
        /// 根据节点名称删除子节点
        /// </summary>
        /// <param name="tagName">节点名称</param>
        public void RemoveChildsByTagName(string tagName)
        {
            if (tagName != null && tagName.Length != 0) removeChildsByTagName(tagName.ToLower());
        }
        /// <summary>
        /// 根据节点名称删除子节点
        /// </summary>
        /// <param name="tagName">节点名称</param>
        public void removeChildsByTagName(string tagName)
        {
            if (children != null)
            {
                int count = children.Count;
                while (--count >= 0 && children[count].TagName != tagName) ;
                if (count >= 0)
                {
                    list<htmlNode>.unsafer values = new list<htmlNode>(children.Count).Unsafer;
                    int index = 0;
                    for (; index != count; ++index)
                    {
                        if (children[index].TagName != tagName) values.Add(children[index]);
                    }
                    for (count = children.Count; ++index != count; values.Add(children[index])) ;
                    children = values.List;
                }
            }
        }
        /// <summary>
        /// 删除匹配的子节点
        /// </summary>
        /// <param name="isRemove">删除子节点匹配器</param>
        public void RemoveChilds(func<htmlNode, bool> isRemove)
        {
            if (isRemove != null) removeChilds(isRemove);
        }
        /// <summary>
        /// 删除匹配的子节点
        /// </summary>
        /// <param name="isRemove">删除子节点匹配器</param>
        private void removeChilds(func<htmlNode, bool> isRemove)
        {
            if (children != null)
            {
                int count = children.Count;
                while (--count >= 0 && !isRemove(children[count])) ;
                if (count >= 0)
                {
                    list<htmlNode>.unsafer values = new list<htmlNode>(children.Count).Unsafer;
                    int index = 0;
                    for (; index != count; ++index)
                    {
                        if (!isRemove(children[index])) values.Add(children[index]);
                    }
                    for (count = children.Count; ++index != count; values.Add(children[index])) ;
                    children = values.List;
                }
            }
        }
        /// <summary>
        /// 根据节点名称获取子孙节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>匹配的子孙节点集合</returns>
        public list<htmlNode> GetNodesByTagName(string tagName)
        {
            return tagName != null && tagName.Length != 0 ? getNodesByTagName(tagName.ToLower()) : new list<htmlNode>();
        }
        /// <summary>
        /// 根据节点名称获取子孙节点集合
        /// </summary>
        /// <param name="tagName">节点名称</param>
        /// <returns>匹配的子孙节点集合</returns>
        public list<htmlNode> getNodesByTagName(string tagName)
        {
            list<htmlNode> values = new list<htmlNode>();
            foreach (htmlNode value in Nodes)
            {
                if (value.TagName == tagName) values.Add(value);
            }
            return values;
        }
        /// <summary>
        /// 判断是否存在匹配的子孙节点
        /// </summary>
        /// <param name="node">匹配节点</param>
        /// <returns>是否存在匹配的子孙节点</returns>
        public bool IsNode(htmlNode node)
        {
            while (node != null && node != this) node = node.Parent;
            return node != null;
        }
        /// <summary>
        /// 解析HTML节点并插入
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="html"></param>
        /// <returns>是否插入成功</returns>
        public bool InsertChild(int index, string html)
        {
            bool isInsert = false;
            if (TagName != null && !web.html.NonanalyticTagNames.Contains(TagName))
            {
                htmlNode value = new htmlNode(html);
                if (value.children != null)
                {
                    foreach (htmlNode child in value.children) child.Parent = this;
                    if (children == null) children = value.children;
                    else if (index >= children.Count) children.Add(value.children);
                    else children.Insert(index < 0 ? 0 : index, value.children);
                }
                isInsert = true;
            }
            return isInsert;
        }
        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="node">待删除的子节点</param>
        /// <returns>是否存在子节点</returns>
        public bool RemoveChild(htmlNode node)
        {
            int index = this[node];
            if (index != -1) children.RemoveAt(index);
            return index != -1;
        }
        /// <summary>
        /// 替换子节点
        /// </summary>
        /// <param name="oldNode">待替换的子节点</param>
        /// <param name="newNode">新的子节点</param>
        /// <returns>是否存在待替换的子节点</returns>
        public bool ReplaceChild(htmlNode oldNode, htmlNode newNode)
        {
            bool isReplace = false;
            if (oldNode != null && newNode != null)
            {
                int oldIndex = this[oldNode];
                if (oldIndex != -1)
                {
                    if (newNode.TagName == string.Empty)
                    {
                        oldNode.Parent = null;
                        if (newNode.children == null) children.RemoveAt(oldIndex);
                        else
                        {
                            foreach (htmlNode value in newNode.children) value.Parent = this;
                            if (newNode.children.Count == 1) children[oldIndex] = newNode.children[0];
                            else if (oldIndex == children.Count - 1)
                            {
                                children.RemoveAt(oldIndex);
                                children.Add(newNode.children);
                            }
                            else
                            {
                                list<htmlNode>.unsafer values = new list<htmlNode>(children.Count + newNode.children.Count).Unsafer;
                                int newIndex = 0;
                                while (newIndex != oldIndex) values.Add(children[newIndex++]);
                                values.List.Add(newNode.children);
                                for (oldIndex = children.Count; ++newIndex != oldIndex; values.Add(children[newIndex])) ;
                                children = values.List;
                            }
                        }
                        newNode.children = null;
                        isReplace = true;
                    }
                    else if (!newNode.IsNode(this))
                    {
                        int newIndex = this[newNode];
                        if (newIndex == -1)
                        {
                            if (newNode.Parent != null) newNode.Parent.RemoveChild(newNode);
                            newNode.Parent = this;
                            children[oldIndex] = newNode;
                            oldNode.Parent = null;
                        }
                        else
                        {
                            children[oldIndex] = newNode;
                            oldNode.Parent = null;
                            children.RemoveAt(newIndex);
                        }
                        isReplace = true;
                    }
                }
            }
            return isReplace;
        }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text
        {
            get
            {
                if (TagName == null) return nodeText.Text;
                else if (!web.html.NoTextTagNames.Contains(TagName))
                {
                    if (children.count() != 0)
                    {
                        htmlNode node;
                        list<nodeIndex> values = new list<nodeIndex>();
                        nodeIndex index = new nodeIndex { Values = children };
                        bool isSpace = false, isEnter = false;
                        using (charStream strings = new charStream())
                        {
                            while (true)
                            {
                                if (index.Values == null)
                                {
                                    if (values.Count == 0) break;
                                    else
                                    {
                                        index = values.Pop();
                                        string nodeTagName = index.Values[index.Index].TagName;
                                        isEnter = nodeTagName == "p" || nodeTagName == "div";
                                        if (++index.Index == index.Values.Count)
                                        {
                                            index.Values = null;
                                            continue;
                                        }
                                    }
                                }
                                node = index.Values[index.Index];
                                if (node.TagName == "p" || node.TagName == "br")
                                {
                                    isSpace = isEnter = false;
                                    strings.Write(@"
");
                                }
                                if (node.children.count() == 0
                                    || node.TagName == null || web.html.NoTextTagNames.Contains(node.TagName))
                                {
                                    if (node.TagName == null)
                                    {
                                        if (isEnter) strings.Write(@"
");
                                        else if (isSpace) strings.Write(' ');
                                        strings.Write(node.nodeText.Text);
                                        isSpace = true;
                                    }
                                    if (++index.Index == index.Values.Count) index.Values = null;
                                }
                                else
                                {
                                    values.Add(index);
                                    index.Values = node.children;
                                    index.Index = 0;
                                }
                            }
                            return strings.ToString();
                        }
                    }
                    else return string.Empty;
                }
                return null;
            }
        }
        /// <summary>
        /// 生成标签html
        /// </summary>
        /// <param name="strings">html流</param>
        private void tagHtml(charStream strings)
        {
            if (TagName.Length != 0)
            {
                strings.Write('<');
                strings.Write(TagName);
                if (attributes != null)
                {
                    foreach (KeyValuePair<string, htmlText> attribute in attributes)
                    {
                        strings.Write(' ');
                        strings.Write(HttpUtility.HtmlEncode(attribute.Key));
                        strings.Write(@"=""");
                        strings.Write(attribute.Value.Html);
                        strings.Write(@"""");
                    }
                }
                if (web.html.CanNonRoundTagNames.Contains(TagName) && children == null && nodeText.Html == null) strings.Write(" /");
                strings.Write('>');
            }
        }
        /// <summary>
        /// 生成标签结束
        /// </summary>
        /// <param name="strings">html流</param>
        private void tagRound(charStream strings)
        {
            if (TagName.Length != 0
                && (!web.html.CanNonRoundTagNames.Contains(TagName) || children != null || nodeText.Html != null))
            {
                strings.Write("</");
                strings.Write(TagName);
                strings.Write(">");
            }
        }
        /// <summary>
        /// HTML
        /// </summary>
        public string InnerHTML
        {
            get
            {
                return Html(false);
            }
        }
        /// <summary>
        /// 生成HTML
        /// </summary>
        /// <param name="isTag">是否包含当前标签</param>
        /// <returns>HTML</returns>
        public string Html(bool isTag)
        {
            if (TagName != null)
            {
                if (web.html.NonanalyticTagNames.Contains(TagName))
                {
                    if (isTag && TagName.Length != 1)
                    {
                        using (charStream strings = new charStream())
                        {
                            tagHtml(strings);
                            strings.Write(nodeText.Html);
                            tagRound(strings);
                            return strings.ToString();
                        }
                    }
                }
                else
                {
                    using (charStream strings = new charStream())
                    {
                        if (isTag) tagHtml(strings);
                        if (children.count() != 0)
                        {
                            htmlNode node;
                            list<nodeIndex> values = new list<nodeIndex>();
                            nodeIndex index = new nodeIndex { Values = children };
                            while (true)
                            {
                                if (index.Values == null)
                                {
                                    if (values.Count == 0) break;
                                    {
                                        index = values.Pop();
                                        index.Values[index.Index].tagRound(strings);
                                        if (++index.Index == index.Values.Count)
                                        {
                                            index.Values = null;
                                            continue;
                                        }
                                    }
                                }
                                node = index.Values[index.Index];
                                string nodeTagName = node.TagName;
                                bool isNonanalyticTagNames = nodeTagName != null && web.html.NonanalyticTagNames.Contains(nodeTagName);
                                if (node.children.count() == 0 || nodeTagName == null || isNonanalyticTagNames)
                                {
                                    if (nodeTagName != null && nodeTagName.Length != 1) node.tagHtml(strings);
                                    strings.Write(node.nodeText.Html);
                                    if (nodeTagName != null && nodeTagName.Length != 1) node.tagRound(strings);
                                    if (++index.Index == index.Values.Count) index.Values = null;
                                }
                                else
                                {
                                    node.tagHtml(strings);
                                    values.Add(index);
                                    index.Values = node.children;
                                    index.Index = 0;
                                }
                            }
                        }
                        if (isTag) tagRound(strings);
                        return strings.ToString();
                    }
                }
            }
            return nodeText.Html;
        }

        static htmlNode()
        {
            int dataIndex = 0;
            pointer[] datas = unmanaged.Get(true
                , String.asciiMap.mapBytes, String.asciiMap.mapBytes, String.asciiMap.mapBytes
                , String.asciiMap.mapBytes, String.asciiMap.mapBytes, String.asciiMap.mapBytes);
            spaceMap = new String.asciiMap(datas[dataIndex++], "\t\r\n ", true);
            spaceSplitMap = new String.asciiMap(datas[dataIndex++], "\t\r\n >", true).Pointer;
            tagNameSplitMap = new String.asciiMap(datas[dataIndex++], "\t\r\n \"'/=>", true).Pointer;
            attributeSplitMap = new String.asciiMap(datas[dataIndex++], "\t\r\n \"'/=", true).Pointer;
            attributeNameSplitMap = new String.asciiMap(datas[dataIndex++], "\"'/>", true).Pointer;
            String.asciiMap tagNameAsciiMap = new String.asciiMap(tagNameMap = datas[dataIndex++], "!/", true);
            tagNameAsciiMap.Unsafer.Set('a', 26);
            tagNameAsciiMap.Unsafer.Set('A', 26);
            noLowerAttributeNames = new staticDictionary<string, string>(new string[] { "readOnly", "className" }, value => value.ToLower());
        }
    }
}
