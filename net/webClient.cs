using System;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;

namespace fastCSharp.net
{
    /// <summary>
    /// WebClient相关操作
    /// </summary>
    public class webClient : WebClient
    {
        /// <summary>
        /// 来源页面名称
        /// </summary>
        public const string RefererName = "Referer";
        /// <summary>
        /// 浏览器参数名称
        /// </summary>
        public const string UserAgentName = "User-Agent";
        /// <summary>
        /// 默认浏览器参数
        /// </summary>
        public const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1;)";
        /// <summary>
        /// 表单提交内容类型名称
        /// </summary>
        public const string ContentTypeName = "Content-Type";
        /// <summary>
        /// 压缩编码名称
        /// </summary>
        public const string ContentEncodingName = "Content-Encoding";
        /// <summary>
        /// 字符集标识
        /// </summary>
        public const string CharsetName = "charset=";
        /// <summary>
        /// 默认表单提交内容类型
        /// </summary>
        public const string DefaultPostContentType = "application/x-www-form-urlencoded";
        /// <summary>
        /// 空页面地址
        /// </summary>
        public const string BlankUrl = "about:blank";
        /// <summary>
        /// ServicePointManager.Expect100Continue访问锁
        /// </summary>
        public static readonly object Expect100ContinueLock = new object();

        /// <summary>
        /// cookie状态
        /// </summary>
        public CookieContainer Cookies { get; private set; }
        /// <summary>
        /// 超时毫秒数
        /// </summary>
        public int TimeOut;
        /// <summary>
        /// 浏览器参数
        /// </summary>
        public string UserAgent = DefaultUserAgent;
        /// <summary>
        /// HTTP请求
        /// </summary>
        private WebRequest webRequest;
        /// <summary>
        /// HTTP请求
        /// </summary>
        public HttpWebRequest HttpRequest
        {
            get
            {
                return webRequest == null ? null : webRequest as HttpWebRequest;
            }
        }
        /// <summary>
        /// HTTP回应
        /// </summary>
        //private WebResponse response;
        /// <summary>
        /// 是否允许跳转
        /// </summary>
        public bool AllowAutoRedirect = true;
        /// <summary>
        /// 获取最后一次操作是否发生重定向
        /// </summary>
        public bool IsRedirect
        {
            get
            {
                return webRequest != null && webRequest is HttpWebRequest
                    && webRequest.RequestUri.Equals((webRequest as HttpWebRequest).Address);
            }
        }
        /// <summary>
        /// 获取最后一次重定向地址
        /// </summary>
        public Uri RedirectUri
        {
            get
            {
                return IsRedirect ? (webRequest as HttpWebRequest).Address : null;
            }
        }
        /// <summary>
        /// HTTP回应压缩类型
        /// </summary>
        public stream.compression CompressionType
        {
            get
            {
                stream.compression type = stream.compression.None;
                if (ResponseHeaders != null)
                {
                    switch (ResponseHeaders[ContentEncodingName])
                    {
                        case "gzip":
                            type = stream.compression.GZip;
                            break;
                        case "deflate":
                            type = stream.compression.Deflate;
                            break;
                    }
                }
                return type;
            }
        }
        /// <summary>
        /// HTTP回应编码字符集
        /// </summary>
        public Encoding TextEncoding
        {
            get
            {
                if (ResponseHeaders != null)
                {
                    string contentType = ResponseHeaders[webClient.ContentTypeName];
                    if (contentType != null) return getEncoding(contentType);
                }
                return null;
            }
        }
        /// <summary>
        /// 获取重定向地址
        /// </summary>
        public string Location
        {
            get
            {
                WebResponse response = webRequest == null ? null : webRequest.GetResponse();
                return response == null ? null : response.Headers["Location"];
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cookies">cookie状态</param>
        /// <param name="proxy">代理</param>
        public webClient(CookieContainer cookies = null, WebProxy proxy = null)
        {
            Credentials = new CredentialCache();
            Cookies = cookies == null ? new CookieContainer() : cookies;
            Credentials = CredentialCache.DefaultCredentials;

            Proxy = proxy;
            //string header = client.ResponseHeaders["Set-Cookie"];
            //client.Headers.Add("Cookie", header);
        }
        /// <summary>
        /// 获取HTTP请求
        /// </summary>
        /// <param name="address">URI地址</param>
        /// <returns>HTTP请求</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            webRequest = base.GetWebRequest(address);
            HttpWebRequest request = HttpRequest;
            if (request != null)
            {
                request.AllowAutoRedirect = AllowAutoRedirect;
                request.CookieContainer = Cookies;
                if (TimeOut > 0) request.Timeout = TimeOut;
            }
            return request;
        }
        ///// <summary>
        ///// 获取HTTP回应
        ///// </summary>
        ///// <param name="request">HTTP请求</param>
        ///// <returns>HTTP回应</returns>
        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    Response = base.GetWebResponse(request);
        //    if (TimeOut > 0)
        //    {
        //        HttpWebResponse response = Response as HttpWebResponse;
        //        if (response != null) response.GetResponseStream().ReadTimeout = TimeOut;
        //    }
        //    return Response;
        //}
        ///// <summary>
        ///// 获取HTTP回应
        ///// </summary>
        ///// <param name="request">HTTP请求</param>
        ///// <param name="result"></param>
        ///// <returns>HTTP回应</returns>
        //protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        //{
        //    Response = base.GetWebResponse(request, result);
        //    if (TimeOut > 0)
        //    {
        //        HttpWebResponse response = Response as HttpWebResponse;
        //        if (response != null) response.GetResponseStream().ReadTimeout = TimeOut;
        //    }
        //    return Response;
        //}
        /// <summary>
        /// 添加COOKIE
        /// </summary>
        /// <param name="address">URI地址</param>
        /// <param name="cookieString">COOKIE字符串</param>
        public void AddCookie(Uri address, string cookieString)
        {
            if (address != null && cookieString != null && cookieString.Length != 0)
            {
                int cookieIndex;
                foreach (subString cookie in cookieString.split(';'))
                {
                    if ((cookieIndex = cookie.IndexOf('=')) > 0)
                    {
                        try
                        {
                            Cookies.Add(address, new Cookie(web.cookie.FormatCookieName(cookie.Substring(0, cookieIndex).Trim()), web.cookie.FormatCookieValue(cookie.Substring(cookieIndex + 1).Trim()), "/"));
                        }
                        catch { }
                    }
                }
            }
        }
        /// <summary>
        /// 合并同域cookie(用于处理跨域BUG)
        /// </summary>
        /// <param name="address">URI地址</param>
        /// <param name="cookies">默认cookie集合信息</param>
        /// <param name="documentCookie">登录后的cookie信息</param>
        /// <param name="httpOnlyCookie">登录后的httpOnly相关cookie信息</param>
        public void MergeDomainCookie(Uri address, list<Cookie> cookies, string documentCookie, string httpOnlyCookie)
        {
            if (cookies != null)
            {
                foreach (Cookie cookie in cookies) Cookies.Add(address, cookie);
            }
            if (address != null)
            {
                list<Cookie> newCookies = new list<Cookie>();
                Dictionary<string, int> nameCounts = null;
                list<string> documentCookies = new list<string>(2);
                if (documentCookie.length() != 0) documentCookies.Unsafer.Add(documentCookie);
                if (httpOnlyCookie.length() != 0) documentCookies.Unsafer.Add(httpOnlyCookie);
                if (documentCookies.Count != 0)
                {
                    int index, nextCount, count;
                    string name;
                    Cookie newCookie;
                    Dictionary<string, int> nextNameCounts = new Dictionary<string, int>();
                    nameCounts = new Dictionary<string, int>();
                    foreach (string nextCookie in documentCookies)
                    {
                        nextNameCounts.Clear();
                        foreach (subString cookie in nextCookie.Split(';'))
                        {
                            if (cookie.Length != 0 && (index = cookie.IndexOf('=')) != 0)
                            {
                                if (index == -1)
                                {
                                    name = web.cookie.FormatCookieName(cookie.Trim());
                                }
                                else name = web.cookie.FormatCookieName(cookie.Substring(0, index).Trim());
                                if (nextNameCounts.TryGetValue(name, out nextCount)) nextNameCounts[name] = ++nextCount;
                                else nextNameCounts.Add(name, nextCount = 1);
                                if (!nameCounts.TryGetValue(name, out count)) count = 0;
                                if (nextCount > count)
                                {
                                    if (index == -1) newCookie = new Cookie(name, string.Empty);
                                    else newCookie = new Cookie(name, web.cookie.FormatCookieValue(cookie.Substring(index + 1)));
                                    newCookies.Add(newCookie);
                                    if (count != 0) newCookie.HttpOnly = true;
                                    if (count == 0) nameCounts.Add(name, nextCount);
                                    else nameCounts[name] = nextCount;
                                }
                            }
                        }
                    }
                }
                foreach (Cookie cookie in Cookies.GetCookies(address))
                {
                    if (nameCounts != null && nameCounts.ContainsKey(cookie.Name)) cookie.Expired = true;
                }
                if (newCookies.Count != 0)
                {
                    try
                    {
                        foreach (Cookie cookie in newCookies) Cookies.Add(address, cookie);
                    }
                    catch (Exception error)
                    {
                        log.Default.Add(error, "合并同域cookie失败", true);
                    }
                }
            }
        }
        /// <summary>
        /// 合并同域cookie(用于处理跨域BUG)
        /// </summary>
        /// <param name="address">URI地址</param>
        /// <param name="responseHeaderCookie">HTTP头cookie信息</param>
        /// <param name="replaceCookie">需要替换的cookie</param>
        public void MergeDomainCookie(Uri address, string responseHeaderCookie, string replaceCookie)
        {
            if (address != null)
            {
                int index;
                string name;
                Cookie newCookie;
                CookieCollection cookies = new CookieCollection();
                Dictionary<string, Cookie> replaceCookies = null;
                if (responseHeaderCookie != null && responseHeaderCookie.Length != 0)
                {
                    replaceCookies = new Dictionary<string, Cookie>();
                    DateTime expires;
                    string value, domain, path, expiresString;
                    string lastCookie = null;
                    list<string> newCookies = new list<string>();
                    foreach (string cookie in responseHeaderCookie.Split(','))
                    {
                        if (lastCookie == null)
                        {
                            string lowerCookie = cookie.ToLower();
                            index = lowerCookie.IndexOf("; expires=", StringComparison.Ordinal);
                            if (index == -1) index = lowerCookie.IndexOf(";expires=", StringComparison.Ordinal);
                            if (index == -1 || cookie.IndexOf(';', index + 10) != -1) newCookies.Add(cookie);
                            else lastCookie = cookie;
                        }
                        else
                        {
                            newCookies.Add(lastCookie + "," + cookie);
                            lastCookie = null;
                        }
                    }
                    Dictionary<string, string> cookieInfo = new Dictionary<string, string>();
                    foreach (string cookie in newCookies)
                    {
                        newCookie = null;
                        foreach (subString values in cookie.Split(';'))
                        {
                            if ((index = values.IndexOf('=')) != 0)
                            {
                                if ((index = values.IndexOf('=')) == -1)
                                {
                                    name = values.Trim();
                                    value = string.Empty;
                                }
                                else
                                {
                                    name = values.Substring(0, index).Trim();
                                    value = values.Substring(index + 1);
                                }
                                if (newCookie == null) newCookie = new Cookie(web.cookie.FormatCookieName(name), web.cookie.FormatCookieValue(value));
                                else cookieInfo[name.toLower()] = value;
                            }
                        }
                        if (cookieInfo.TryGetValue("expires", out expiresString)
                            && DateTime.TryParse(expiresString, out expires))
                        {
                            newCookie.Expires = expires;
                        }
                        if (cookieInfo.TryGetValue("path", out path)) newCookie.Path = path;
                        replaceCookies[newCookie.Name] = newCookie;
                        newCookie = new Cookie(newCookie.Name, newCookie.Value, newCookie.Path);
                        if (cookieInfo.TryGetValue("domain", out domain)) newCookie.Domain = domain;
                        Cookies.Add(address, newCookie);
                        cookieInfo.Clear();
                    }
                }
                if (replaceCookie != null && replaceCookie.Length != 0)
                {
                    if (replaceCookies == null) replaceCookies = new Dictionary<string, Cookie>();
                    foreach (subString cookie in replaceCookie.Split(';'))
                    {
                        if ((index = cookie.IndexOf('=')) != 0)
                        {
                            if (index == -1)
                            {
                                name = web.cookie.FormatCookieName(cookie.Trim());
                                newCookie = new Cookie(name, string.Empty);
                            }
                            else
                            {
                                name = web.cookie.FormatCookieName(cookie.Substring(0, index).Trim());
                                newCookie = new Cookie(name, web.cookie.FormatCookieValue(cookie.Substring(index + 1)));
                            }
                            if (replaceCookies.ContainsKey(name)) replaceCookies[name].Value = newCookie.Value;
                            else replaceCookies.Add(name, newCookie);
                        }
                    }
                }
                bool isCookie;
                foreach (Cookie cookie in Cookies.GetCookies(address))
                {
                    if (isCookie = replaceCookies != null && replaceCookies.ContainsKey(cookie.Name))
                    {
                        newCookie = replaceCookies[cookie.Name];
                    }
                    else newCookie = new Cookie(cookie.Name, cookie.Value, HttpUtility.UrlDecode((cookie.Path)));
                    cookies.Add(newCookie);
                    if (isCookie) replaceCookies.Remove(cookie.Name);
                    newCookie.Expires = cookie.Expires;
                    cookie.Expired = true;
                }
                if (replaceCookies != null)
                {
                    foreach (Cookie cookie in replaceCookies.Values) cookies.Add(cookie);
                }
                if (cookies.Count != 0)
                {
                    try { Cookies.Add(address, cookies); }
                    catch (Exception error)
                    {
                        log.Default.Add(error, "合并同域cookie失败", true);
                    }
                }
            }
        }
        //public void mergeDomainCookie(Uri address, Cookie[] replaceCookie)
        //{
        //    if (address != null)
        //    {
        //        Cookie newCookie;
        //        CookieCollection cookies = new CookieCollection();
        //        Dictionary<string, Cookie> replaceCookies = null;
        //        if (replaceCookie != null && replaceCookie.Length != 0)
        //        {
        //            replaceCookies = new Dictionary<string, Cookie>();
        //            foreach (Cookie cookie in replaceCookie)
        //            {
        //                if (replaceCookies.ContainsKey(cookie.Name)) replaceCookies[cookie.Name] = cookie;
        //                else replaceCookies.Add(cookie.Name, cookie);
        //            }
        //        }
        //        bool isCookie;
        //        foreach (Cookie cookie in cookieContainer.GetCookies(address))
        //        {
        //            cookies.Add(newCookie = (isCookie = replaceCookies != null && replaceCookies.ContainsKey(cookie.Name)) ? replaceCookies[cookie.Name] : new Cookie(cookie.Name, cookie.Value, cookie.Path));
        //            if (isCookie) replaceCookies.Remove(cookie.Name);
        //            newCookie.Expires = cookie.Expires;
        //            cookie.Expired = true;
        //        }
        //        if (replaceCookies != null)
        //        {
        //            foreach (Cookie cookie in replaceCookies.Values) cookies.Add(cookie);
        //        }
        //        if (cookies.Count != 0) cookieContainer.Add(address, cookies);
        //    }
        //}
        //private void BugFix_CookieDomain(CookieContainer cookieContainer)
        //{
        //    Hashtable table = (Hashtable)typeof(CookieContainer).InvokeMember("m_domainTable",
        //                               System.Reflection.BindingFlags.NonPublic |
        //                               System.Reflection.BindingFlags.GetField |
        //                               System.Reflection.BindingFlags.Instance,
        //                               null,
        //                               cookieContainer,
        //                               new object[] { });
        //    ArrayList keys = new ArrayList(table.Keys);
        //    foreach (string keyObj in keys)
        //    {
        //        string key = (keyObj as string);
        //        if (key[0] == '.')
        //        {
        //            string newKey = key.Remove(0, 1);
        //            table[newKey] = table[keyObj];
        //        }
        //    }
        //}
        //public void addCookie(string address, CookieCollection cookies)
        //{
        //    cookieContainer.Add(new Uri(address), cookies);
        //}
        /// <summary>
        /// URI请求信息
        /// </summary>
        public class request
        {
            /// <summary>
            /// 页面地址
            /// </summary>
            public Uri Uri;
            /// <summary>
            /// 页面地址
            /// </summary>
            public string UriString
            {
                set { Uri = uri.Create(value); }
            }
            /// <summary>
            /// POST内容
            /// </summary>
            public NameValueCollection Form;
            /// <summary>
            /// 来源页面地址
            /// </summary>
            public string RefererUrl;
            /// <summary>
            /// 出错时是否写日志
            /// </summary>
            public bool IsErrorOut = true;
            /// <summary>
            /// 出错时是否输出页面地址
            /// </summary>
            public bool IsErrorOutUri = true;
            /// <summary>
            /// URI隐式转换为请求信息
            /// </summary>
            /// <param name="uri">URI</param>
            /// <returns>请求信息</returns>
            public static implicit operator request(Uri uri) { return new request { Uri = uri }; }
            /// <summary>
            /// URI隐式转换为请求信息
            /// </summary>
            /// <param name="uri">URI</param>
            /// <returns>请求信息</returns>
            public static implicit operator request(string uri) { return new request { Uri = new Uri(uri) }; }
        }
        /// <summary>
        /// 将网页保存到文件
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <param name="fileName">保存文件名</param>
        /// <returns>是否保存成功</returns>
        public bool SaveFile(request request, string fileName)
        {
            if (request != null && request.Uri != null && fileName != null)
            {
                try
                {
                    Headers.Add(UserAgentName, UserAgent);
                    Headers.Add(RefererName, request.RefererUrl == null || request.RefererUrl.Length == 0 ? request.Uri.AbsoluteUri : request.RefererUrl);
                    DownloadFile(request.Uri, fileName);
                    return true;
                }
                catch (Exception error)
                {
                    if (request.IsErrorOut)
                    {
                        log.Default.Add(error, (request.IsErrorOutUri ? request.Uri.AbsoluteUri : null) + " 抓取失败", !request.IsErrorOutUri);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 抓取页面字节流
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <returns>页面字节流,失败返回null</returns>
        public byte[] CrawlData(request request)
        {
            if (request != null && request.Uri != null)
            {
                try
                {
                    Headers.Add(UserAgentName, UserAgent);
                    Headers.Add(RefererName, request.RefererUrl == null || request.RefererUrl.Length == 0 ? request.Uri.AbsoluteUri : request.RefererUrl);
                    return deCompress(request.Form == null ? DownloadData(request.Uri) : UploadValues(request.Uri, web.http.methodType.POST.ToString(), request.Form), request);
                }
                catch (Exception error)
                {
                    onError(error, request);
                }
            }
            return null;
        }
        /// <summary>
        /// 异步抓取页面HTML代码
        /// </summary>
        private class dataCrawler
        {
            /// <summary>
            /// Web客户端
            /// </summary>
            public webClient WebClient;
            /// <summary>
            /// 异步事件
            /// </summary>
            public action<byte[]> OnCrawlData;
            /// <summary>
            /// URI请求信息
            /// </summary>
            public request Request;
            /// <summary>
            /// 抓取页面字节流
            /// </summary>
            public void Crawl()
            {
                if (Request != null && Request.Uri != null)
                {
                    try
                    {
                        WebClient.Headers.Add(UserAgentName, WebClient.UserAgent);
                        WebClient.Headers.Add(RefererName, Request.RefererUrl == null || Request.RefererUrl.Length == 0 ? Request.Uri.AbsoluteUri : Request.RefererUrl);
                        if (Request.Form == null) downloadData();
                        else upload();
                        return;
                    }
                    catch (Exception error)
                    {
                        WebClient.onError(error, Request);
                    }
                }
                OnCrawlData(null);
            }
            /// <summary>
            /// 抓取页面字节流
            /// </summary>
            private void downloadData()
            {
                try
                {
                    WebClient.DownloadDataCompleted += onDownload;
                    WebClient.DownloadDataAsync(Request.Uri, WebClient);
                }
                catch (Exception error)
                {
                    WebClient.DownloadDataCompleted -= onDownload;
                    WebClient.onError(error, Request);
                    OnCrawlData(null);
                }
            }
            /// <summary>
            /// 抓取页面字节流
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void onDownload(object sender, DownloadDataCompletedEventArgs e)
            {
                try
                {
                    WebClient.DownloadDataCompleted -= onDownload;
                    if (e.Error != null) WebClient.onError(e.Error, Request);
                }
                finally
                {
                    OnCrawlData(e.Error == null ? WebClient.deCompress(e.Result, Request) : null);
                }
            }
            /// <summary>
            /// 抓取页面字节流
            /// </summary>
            private void upload()
            {
                try
                {
                    WebClient.UploadValuesCompleted += onUpload;
                    WebClient.UploadValuesAsync(Request.Uri, web.http.methodType.POST.ToString(), Request.Form, WebClient);
                }
                catch (Exception error)
                {
                    WebClient.UploadValuesCompleted -= onUpload;
                    WebClient.onError(error, Request);
                    OnCrawlData(null);
                }
            }
            /// <summary>
            /// 抓取页面字节流
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void onUpload(object sender, UploadValuesCompletedEventArgs e)
            {
                try
                {
                    WebClient.UploadValuesCompleted -= onUpload;
                    if (e.Error != null) WebClient.onError(e.Error, Request);
                }
                finally
                {
                    OnCrawlData(e.Error == null ? WebClient.deCompress(e.Result, Request) : null);
                }
            }
        }
        /// <summary>
        /// 抓取页面字节流
        /// </summary>
        /// <param name="onCrawlData">异步事件</param>
        /// <param name="request">URI请求信息</param>
        public void CrawlData(action<byte[]> onCrawlData, request request)
        {
            new dataCrawler { WebClient = this, OnCrawlData = onCrawlData, Request = request }.Crawl();
        }
        /// <summary>
        /// 抓取页面HTML代码
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <param name="encoding">页面编码</param>
        /// <returns>页面HTML代码,失败返回null</returns>
        public string CrawlHtml(request request, Encoding encoding = null)
        {
            return CrawlData(request).toString(encoding ?? this.TextEncoding);
        }
        /// <summary>
        /// 异步抓取页面HTML代码
        /// </summary>
        private struct htmlCrawler
        {
            /// <summary>
            /// Web客户端
            /// </summary>
            public webClient WebClient;
            /// <summary>
            /// 异步事件
            /// </summary>
            public action<string> OnCrawlHtml;
            /// <summary>
            /// 页面编码
            /// </summary>
            public Encoding Encoding;
            /// <summary>
            /// 抓取页面字节流事件
            /// </summary>
            /// <param name="data">页面字节流</param>
            public void onCrawlData(byte[] data)
            {
                OnCrawlHtml(data.toString(Encoding ?? WebClient.TextEncoding));
            }
        }
        /// <summary>
        /// 异步抓取页面HTML代码
        /// </summary>
        /// <param name="onCrawlHtml">异步事件</param>
        /// <param name="request">URI请求信息</param>
        /// <param name="encoding">页面编码</param>
        /// <returns>页面HTML代码,失败返回null</returns>
        public void CrawlHtml(action<string> onCrawlHtml, request request, Encoding encoding = null)
        {
            CrawlData(new htmlCrawler { WebClient = this, OnCrawlHtml = onCrawlHtml, Encoding = encoding }.onCrawlData, request);
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="error">异常信息</param>
        /// <param name="request">请求信息</param>
        private void onError(Exception error, request request)
        {
            if (request.IsErrorOut)
            {
                log.Default.Add(error, (request.IsErrorOutUri ? request.Uri.AbsoluteUri : null) + " 抓取失败", !request.IsErrorOutUri);
            }
        }
        /// <summary>
        /// 数据解压缩
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="request">请求信息</param>
        /// <returns>解压缩数据</returns>
        private byte[] deCompress(byte[] data, request request)
        {
            if (CompressionType != stream.compression.None)
            {
                try
                {
                    return data.getDeCompress(CompressionType, 0, data.Length);
                }
                catch (Exception error)
                {
                    onError(error, request);
                    return null;
                }
            }
            return data;
        }
        /// <summary>
        /// 根据提交类型获取编码字符集
        /// </summary>
        /// <param name="contentType">提交类型</param>
        /// <returns>编码字符集</returns>
        private static Encoding getEncoding(string contentType)
        {
            foreach (subString value in contentType.Split(';'))
            {
                subString key = value.Trim();
                if (key.StartsWith(webClient.CharsetName))
                {
                    try
                    {
                        Encoding encoding = Encoding.GetEncoding(key.Substring(webClient.CharsetName.Length));
                        return encoding;
                    }
                    catch { }
                }
            }
            return null;
        }
        ///// <summary>
        ///// 根据HTML代码获取编码字符集
        ///// </summary>
        ///// <param name="contentType">提交类型</param>
        ///// <returns>编码字符集</returns>
        //public static Encoding GetEncoding(string html)
        //{
        //    Encoding encoding = null;
        //    if (html != null)
        //    {
        //        string[] metas = html.ToLower().Split(new string[] { "<meta " }, StringSplitOptions.None);
        //        for (int index = 1, length = metas.Length; index != length; ++index)
        //        {
        //            string meta = metas[index];
        //            int endIndex = meta.IndexOf('>');
        //            if (endIndex != -1)
        //            {
        //                int contentIndex = meta.replace('"', '\'').IndexOf("http-equiv='content-type'",  StringComparison.Ordinal);
        //                if (contentIndex != -1 && contentIndex < endIndex)
        //                {
        //                    string content = "content='";
        //                    contentIndex = meta.IndexOf(content,  StringComparison.Ordinal);
        //                    if (contentIndex != -1 && contentIndex != meta.Length)
        //                    {
        //                        endIndex = meta.IndexOf('\'', contentIndex += content.Length);
        //                        if (endIndex != -1) encoding = GetEncoding(meta.Substring(contentIndex, endIndex - contentIndex));
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return encoding;
        //}
    }
}
