using System;
using System.Collections.Generic;
using System.IO;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 自定义简单组合模板
    /// </summary>
    [auto(Name = "自定义模板", IsAuto = true)]
    internal class simpleTemplate : IAuto
    {
        /// <summary>
        /// 自定义模板相对项目路径
        /// </summary>
        public const string DefaultTemplatePath = @"template\";
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        public bool Run(auto.parameter parameter)
        {
            if (parameter != null)
            {
                string path = parameter.ProjectPath + (parameter.ProjectPath == config.setup.Default.FastCSharpPath ? DefaultTemplatePath : config.setup.Default.SimpleTemplatePath).pathSuffix();
                if (Directory.Exists(path))
                {
                    list<string>[] codes = Directory.GetFiles(path, "*.cs").getArray(name => code(name));
                    if (!codes.any(code => code == null))
                    {
                        cSharp.coder.Add(string.Concat(codes.getArray(code => code.ToArray()).getArray()));
                        return true;
                    }
                    return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 一个关键字的模板替换信息
        /// </summary>
        private class replace
        {
            /// <summary>
            /// 原始数组中的替换位置集合
            /// </summary>
            public list<int>[] indexs;
            /// <summary>
            /// 替换的目标组合集合
            /// </summary>
            public list<subString>[] values;
            /// <summary>
            /// 当前组合索引
            /// </summary>
            public int index;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="fileName">模板文件名</param>
        /// <returns>代码</returns>
        private static list<string> code(string fileName)
        {
            string code = File.ReadAllText(fileName);
            int index = code.IndexOf(@"*/

", StringComparison.Ordinal);
            if (index != -1)
            {
                int startIndex = index += 4;
                Dictionary<string, replace> keys = new Dictionary<string, replace>();
                replace replace;
                foreach (string keyValue in code.Substring(0, index).Split(new string[] { "/*" }, StringSplitOptions.None).sub(1))
                {
                    if ((index = keyValue.IndexOf(':')) != -1 && keyValue.EndsWith(@"*/
", StringComparison.Ordinal))
                    {
                        keys[keyValue.Substring(0, index)] = replace = new replace
                        {
                            values = new subString(keyValue, ++index, keyValue.Length - index - 4).Split(';').getArray(value => value.Split(','))
                        };
                        replace.indexs = new list<int>[replace.values[0].Count];
                        for (index = 0; index != replace.indexs.Length; replace.indexs[index++] = new list<int>()) ;
                    }
                    else error.Add("自定义数据错误 : " + keyValue);
                }
                list<string> codeList = new list<string>();
                for (code = code.Substring(startIndex), startIndex = 0, index = code.IndexOf("/*", StringComparison.Ordinal); index != -1; index = code.IndexOf("/*", startIndex))
                {
                    codeList.Add(code.Substring(startIndex, index - startIndex));
                    if ((startIndex = code.IndexOf("*/", index, StringComparison.Ordinal) + 2) != 1 && startIndex != index + 4)
                    {
                        string name = code.Substring(index, startIndex - index);
                        if ((index = code.IndexOf(name, startIndex, StringComparison.Ordinal)) != -1)
                        {
                            startIndex = index + name.Length;
                            name = name.Substring(2, name.Length - 4);
                            if (name[name.Length - 1] == ']')
                            {
                                int arrayIndex = name.LastIndexOf('[') + 1;
                                if (arrayIndex == 0 || !int.TryParse(name.Substring(arrayIndex, name.Length - arrayIndex - 1), out index))
                                {
                                    error.Add("自定义名称索引错误 : " + name);
                                    index = 0;
                                }
                                else name = name.Substring(0, arrayIndex - 1);
                            }
                            else index = 0;
                            if (keys.TryGetValue(name, out replace))
                            {
                                replace.indexs[index].Add(codeList.Count);
                                codeList.Add(string.Empty);
                            }
                            else error.Add("自定义名称不匹配 : " + name);
                        }
                        else error.Add("自定义名称不匹配 : " + name);
                    }
                    else
                    {
                        error.Add("自定义名称错误 : " + code.Substring(index));
                        startIndex = code.Length;
                    }
                }
                codeList.Add(code.Substring(startIndex));
                string[] codes = codeList.ToArray();
                list<string> values = new list<string>();
                replace[] replaces = keys.Values.getArray();
                do
                {
                    index = replaces.Length;
                    while (--index >= 0 && ++replaces[index].index == replaces[index].values.Length) ;
                    if (index == -1) break;
                    while (++index != replaces.Length) replaces[index].index = 0;
                    for (index = replaces.Length; --index >= 0; )
                    {
                        replace = replaces[index];
                        list<subString> replaceCode = replace.values[replace.index];
                        for (startIndex = replace.indexs.Length; --startIndex >= 0; )
                        {
                            foreach (int codeIndex in replace.indexs[startIndex]) codes[codeIndex] = replaceCode[startIndex];
                        }
                    }
                    values.Add(string.Concat(codes));
                }
                while (true);
                return values;
            }
            else error.Add("自定义文件错误 : " + fileName);
            return null;
        }
    }
}
