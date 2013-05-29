using System;
using fastCSharp;

namespace fastCSharp.setup.cSharp.template
{
    class urlQuery : pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            /*NOTE*/
            object MemberName = null;/*NOTE*/
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="geter">查询字符串获取器</param>
            /// <param name="memberMap">成员位图</param>
            private void getUrlQeury(fastCSharp.setup.cSharp.urlQuery.IGetter geter, @type.FullName/**/.memberMap memberMap)
            {
                if (memberMap.IsDefault)
                {
                    #region LOOP Members
                    #region NAME UrlQuery
                    #region IF MemberType.IsUrlQueryType
                    geter.Get("@MemberName", this.@MemberName);
                    #endregion IF MemberType.IsUrlQueryType
                    #region NOT MemberType.IsUrlQueryType
                    #region IF MemberType.IsNull
                    geter.Get("@MemberName", this.@MemberName);
                    #endregion IF MemberType.IsNull
                    #region NOT MemberType.IsNull
                    geter.GetValue("@MemberName", /*NOTE*/(int)(object)/*NOTE*/this.@MemberName);
                    #endregion NOT MemberType.IsNull
                    #endregion NOT MemberType.IsUrlQueryType
                    #endregion NAME UrlQuery
                    #endregion LOOP Members
                }
                else
                {
                    #region LOOP Members
                    if (memberMap.IsMember(@MemberIndex))
                    {
                        #region FROMNAME UrlQuery
                        #endregion FROMNAME UrlQuery
                    }
                    #endregion LOOP Members
                }
            }
        }
        #region IF Attribute.IsQuery
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : fastCSharp.setup.cSharp.urlQuery.IQuery<@type.FullName/**/.memberMap>
        {
            /// <summary>
            /// GET查询字符串获取器
            /// </summary>
            private class queryGetter : fastCSharp.setup.cSharp.urlQuery.queryGetter
            {
                /// <summary>
                /// GET查询字符串获取器
                /// </summary>
                /// <param name="encoding">编码格式</param>
                public queryGetter(System.Text.Encoding encoding)
                {
                    this.encoding = encoding;
                    querys = new list<string>(@Members.Length).Unsafer;
                }
            }
            /// <summary>
            /// 获取查询字符串
            /// </summary>
            /// <param name="encoding">编码格式</param>
            /// <param name="memberMap">成员位图</param>
            /// <returns>查询字符串</returns>
            public string GetQuery(System.Text.Encoding encoding, @type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
            {
                queryGetter queryGetter = new queryGetter(encoding);
                getUrlQeury(new queryGetter(encoding), memberMap);
                return queryGetter.Query;
            }
        }
        #endregion IF Attribute.IsQuery
        #region IF Attribute.IsForm
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition : fastCSharp.setup.cSharp.urlQuery.IForm<@type.FullName/**/.memberMap>
        {
            /// <summary>
            /// POST表单获取器
            /// </summary>
            private class formGetter : fastCSharp.setup.cSharp.urlQuery.formGetter
            {
                /// <summary>
                /// POST表单获取器
                /// </summary>
                public formGetter()
                {
                    Form = new System.Collections.Specialized.NameValueCollection(@Members.Length);
                }
            }
            /// <summary>
            /// 获取POST表单
            /// </summary>
            /// <param name="memberMap">成员位图</param>
            /// <returns>POST表单</returns>
            public System.Collections.Specialized.NameValueCollection GetForm(@type.FullName/**/.memberMap memberMap = default(@type.FullName/**/.memberMap))
            {
                formGetter formGetter = new formGetter();
                getUrlQeury(formGetter, memberMap);
                return formGetter.Form;
            }
        }
        #endregion IF Attribute.IsForm
        #endregion PART CLASS
    }
}
