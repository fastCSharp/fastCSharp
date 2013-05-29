using System;
using fastCSharp;

namespace fastCSharp.web
{
    /// <summary>
    /// HTML代码参数及其相关操作
    /// </summary>
    public static class html
    {
        /// <summary>
        /// 文档类型属性
        /// </summary>
        public class docType : Attribute
        {
            /// <summary>
            /// 标准文档类型头部
            /// </summary>
            public string Html;

            /// <summary>
            /// 标准引用类型
            /// </summary>
            public enum enumType
            {
                /// <summary>
                /// 过渡(HTML4.01)
                /// </summary>
                [docType(Html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">")]
                Transitional = 0,
                /// <summary>
                /// 严格(不能使用任何表现层的标识和属性，例如<br>)
                /// </summary>
                [docType(Html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">")]
                Strict,
                /// <summary>
                /// 框架(专门针对框架页面设计使用的DTD，如果你的页面中包含有框架，需要采用这种DTD)
                /// </summary>
                [docType(Html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Frameset//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd"">")]
                Frameset,
                /// <summary>
                /// 
                /// </summary>
                Xhtml11,
                /// <summary>
                /// HTML5
                /// </summary>
                [docType(Html = @"<!DOCTYPE html>")]
                Html5,
            }
        }
        /// <summary>
        /// 标准文档集合
        /// </summary>
        private static readonly docType[] docTypes = fastCSharp.Enum.GetAttributes<docType.enumType, docType>();
        /// <summary>
        /// 获取标准引用代码
        /// </summary>
        /// <returns>文档类型</returns>
        public static string GetHtml(this docType.enumType type)
        {
            int typeIndex = (int)type;
            if (typeIndex < 0 || typeIndex >= docTypes.Length) typeIndex = 0;
            return docTypes[typeIndex].Html + @"
<html xmlns=""http://www.w3.org/1999/xhtml"">
";
        }
        /// <summary>
        /// 字符集类型属性
        /// </summary>
        public class charsetType : Attribute
        {
            /// <summary>
            /// 字符串表示
            /// </summary>
            public string CharsetString;

            /// <summary>
            /// 字符集类型
            /// </summary>
            public enum enumType
            {
                /// <summary>
                /// UTF-8
                /// </summary>
                [charsetType(CharsetString = "UTF-8")]
                Utf8,
                /// <summary>
                /// GB2312
                /// </summary>
                [charsetType(CharsetString = "GB2312")]
                Gb2312,
            }
        }
        /// <summary>
        /// 字符集类型名称集合
        /// </summary>
        private static readonly charsetType[] CharsetTypes = fastCSharp.Enum.GetAttributes<charsetType.enumType, charsetType>();
        /// <summary>
        /// 获取字符集代码
        /// </summary>
        /// <returns>字符集代码</returns>
        public static string GetHtml(this charsetType.enumType type)
        {
            int typeIndex = (int)type;
            if (typeIndex >= CharsetTypes.Length) typeIndex = -1;
            string html = string.Empty;
            if (typeIndex >= 0) html = @"<meta http-equiv=""content-type"" content=""text/html; charset=" + CharsetTypes[typeIndex].CharsetString + @""">
";
            return html;
        }
        /// <summary>
        /// 注释开始
        /// </summary>
        public const string NoteStart = @"
<![CDATA[
";
        /// <summary>
        /// 注释结束
        /// </summary>
        public const string NoteEnd = @"
]]>
";
        /// <summary>
        /// javscript开始
        /// </summary>
        public const string JsStart = @"
<script language=""javascript"" type=""text/javascript"">
//<![CDATA[
";
        /// <summary>
        /// javscript结束
        /// </summary>
        public const string JsEnd = @"
//]]>
</script>
";
        /// <summary>
        /// 加载js文件
        /// </summary>
        /// <param name="fileName">被加载的js文件地址</param>
        /// <returns>加载js文件的HTML代码</returns>
        public static string JsFile(string fileName)
        {
            return @"<script language=""javascript"" type=""text/javascript"" src=""" + fileName + @"""></script>";
        }
        /// <summary>
        /// style开始
        /// </summary>
        public const string StyleStart = @"
<style type=""text/css"">
<![CDATA[
";
        /// <summary>
        /// style结束
        /// </summary>
        public const string StyletEnd = @"
]]>
</style>
";
        /// <summary>
        /// 加载css文件
        /// </summary>
        /// <param name="fileName">被加载的css文件地址</param>
        /// <returns>加载css文件的HTML代码</returns>
        public static string CssFile(string fileName)
        {
            return @"<style type=""text/css"" link=""" + fileName + @"""></style>";
        }
        /// <summary>
        /// 允许不回合的标签名称集合
        /// </summary>
        public static readonly staticHashSet<string> CanNonRoundTagNames = new staticHashSet<string>(new string[] { "area", "areatext", "basefont", "br", "col", "colgroup", "hr", "img", "input", "li", "p", "spacer" });
        /// <summary>
        /// 必须回合的标签名称集合
        /// </summary>
        public static readonly staticHashSet<string> MustRoundTagNames = new staticHashSet<string>(new string[] { "a", "b", "bgsound", "big", "body", "button", "caption", "center", "div", "em", "embed", "font", "form", "h1", "h2", "h3", "h4", "h5", "h6", "hn", "html", "i", "iframe", "map", "marquee", "multicol", "nobr", "ol", "option", "pre", "s", "select", "small", "span", "strike", "strong", "sub", "sup", "table", "tbody", "td", "textarea", "tfoot", "th", "thead", "tr", "u", "ul" });
        /// <summary>
        /// 脚本安全属性名称集合
        /// </summary>
        public static readonly staticHashSet<string> SafeAttributes = new staticHashSet<string>(new string[] { "align", "allowtransparency", "alt", "behavior", "bgcolor", "border", "bordercolor", "bordercolordark", "bordercolorlight", "cellpadding", "cellspacing", "checked", "class", "clear", "color", "cols", "colspan", "controls", "coords", "direction", "face", "frame", "frameborder", "gutter", "height", "hspace", "loop", "loopdelay", "marginheight", "marginwidth", "maxlength", "method", "multiple", "rows", "rowspan", "rules", "scrollamount", "scrolldelay", "scrolling", "selected", "shape", "size", "span", "start", "target", "title", "type", "unselectable", "usemap", "valign", "value", "vspace", "width", "wrap" });
        /// <summary>
        /// 脚本不安全属性名称集合
        /// </summary>
        public static readonly staticHashSet<string> UnsafeAttributes = new staticHashSet<string>(new string[] { "action", "background", "dynsrc", "href", "src", "style" });
        /// <summary>
        /// 非解析标签名称集合
        /// </summary>
        public static readonly staticHashSet<string> NonanalyticTagNames = new staticHashSet<string>(new string[] { "script", "style", "!", "/" });
        /// <summary>
        /// 非文本标签名称集合
        /// </summary>
        public static readonly staticHashSet<string> NoTextTagNames = new staticHashSet<string>(new string[] { "script", "style", "pre", "areatext", "!", "/", "input", "iframe", "img", "link", "head" });
        /// <summary>
        /// HTML编码器
        /// </summary>
        public interface IEncoder
        {
            /// <summary>
            /// 文本转HTML
            /// </summary>
            /// <param name="value">文本值</param>
            /// <returns>HTML编码</returns>
            string ToHtml(string value);
        }
        /// <summary>
        /// HTML编码器
        /// </summary>
        internal unsafe struct encoder : IEncoder
        {
            /// <summary>
            /// HTML转义字符集合
            /// </summary>
            private readonly uint* htmls;
            /// <summary>
            /// 最大值
            /// </summary>
            private readonly int size;
            /// <summary>
            /// HTML编码器
            /// </summary>
            /// <param name="htmls">HTML转义字符集合</param>
            public encoder(string htmls)
            {
                size = (htmls.max((char)0) + 1);
                this.htmls = unmanaged.Get(size * sizeof(uint), true).UInt;
                foreach (char value in htmls)
                {
                    int div = (value * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                    this.htmls[value] = (uint)(((value - div * 10) << 16) | div | 0x300030);
                }
            }
            /// <summary>
            /// HTML转义
            /// </summary>
            /// <param name="start">起始位置</param>
            /// <param name="end">结束位置</param>
            /// <param name="write">写入位置</param>
            private unsafe void toHtml(char* start, char* end, char* write)
            {
                while (start != end)
                {
                    char code = *start++;
                    if (code < size)
                    {
                        uint html = htmls[code];
                        if (html == 0) *write++ = code;
                        else
                        {
                            *(int*)write = '&' + ('#' << 16);
                            write += 2;
                            *(uint*)write = html;
                            write += 2;
                            *write++ = ';';
                        }
                    }
                    else *write++ = code;
                }
            }
            /// <summary>
            /// 文本转HTML
            /// </summary>
            /// <param name="value">文本值</param>
            /// <returns>HTML编码</returns>
            public unsafe string ToHtml(string value)
            {
                if (value != null)
                {
                    int count = 0, length = value.Length;
                    fixed (char* valueFixed = value)
                    {
                        char* end = valueFixed + length;
                        for (char* start = valueFixed; start != end; ++start)
                        {
                            if (*start < size && htmls[*start] != 0) ++count;
                        }
                        if (count != 0)
                        {
                            value = new string((char)0, length += count << 2);
                            fixed (char* data = value) toHtml(valueFixed, end, data);
                        }
                    }
                }
                return value;
            }
        }
        /// <summary>
        /// 默认HTML编码器
        /// </summary>
        public readonly static IEncoder Encoder = new encoder(@"& <>""'");
        /// <summary>
        /// TextArea编码器
        /// </summary>
        public readonly static IEncoder TextAreaEncoder = new encoder(@"&<>");
    }
}
