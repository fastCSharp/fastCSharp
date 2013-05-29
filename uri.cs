using System;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp
{
    /// <summary>
    /// URI相关操作
    /// </summary>
    public static class uri
    {
        [Flags]
        public enum flags : ulong
        {
            AllUriInfoSet = 0x80000000L,
            AuthorityFound = 0x100000L,
            BackslashInPath = 0x8000L,
            BasicHostType = 0x50000L,
            CannotDisplayCanonical = 0x7fL,
            CanonicalDnsHost = 0x2000000L,
            DnsHostType = 0x30000L,
            DosPath = 0x8000000L,
            E_CannotDisplayCanonical = 0x1f80L,
            E_FragmentNotCanonical = 0x1000L,
            E_HostNotCanonical = 0x100L,
            E_PathNotCanonical = 0x400L,
            E_PortNotCanonical = 0x200L,
            E_QueryNotCanonical = 0x800L,
            E_UserNotCanonical = 0x80L,
            ErrorOrParsingRecursion = 0x4000000L,
            FirstSlashAbsent = 0x4000L,
            FragmentIriCanonical = 0x40000000000L,
            FragmentNotCanonical = 0x40L,
            HasUnicode = 0x200000000L,
            HasUserInfo = 0x200000L,
            HostNotCanonical = 4L,
            HostNotParsed = 0L,
            HostTypeMask = 0x70000L,
            HostUnicodeNormalized = 0x400000000L,
            IdnHost = 0x100000000L,
            ImplicitFile = 0x20000000L,
            IndexMask = 0xffffL,
            IntranetUri = 0x2000000000L,
            IPv4HostType = 0x20000L,
            IPv6HostType = 0x10000L,
            IriCanonical = 0x78000000000L,
            LoopbackHost = 0x400000L,
            MinimalUriInfoSet = 0x40000000L,
            NotDefaultPort = 0x800000L,
            PathIriCanonical = 0x10000000000L,
            PathNotCanonical = 0x10L,
            PortNotCanonical = 8L,
            QueryIriCanonical = 0x20000000000L,
            QueryNotCanonical = 0x20L,
            RestUnicodeNormalized = 0x800000000L,
            SchemeNotCanonical = 1L,
            ShouldBeCompressed = 0x2000L,
            UncHostType = 0x40000L,
            UncPath = 0x10000000L,
            UnicodeHost = 0x1000000000L,
            UnknownHostType = 0x70000L,
            UnusedHostType = 0x60000L,
            UseOrigUncdStrOffset = 0x4000000000L,
            UserDrivenParsing = 0x1000000L,
            UserEscaped = 0x80000L,
            UserIriCanonical = 0x8000000000L,
            UserNotCanonical = 2L,
            Zero = 0L
        }
        /// <summary>
        /// Uri.m_Info
        /// </summary>
        private static readonly func<object, object> uriInfo;
        /// <summary>
        /// Uri.m_Flags
        /// </summary>
        private static readonly fieldGetSetValue uriFlags;
        /// <summary>
        /// System.Uri.UriInfo.MoreInfo
        /// </summary>
        private static readonly func<object, object> uriMoreInfo;
        /// <summary>
        /// System.Uri.MoreInfo.AbsoluteUri
        /// </summary>
        private static readonly action<object, string> uriAbsolute;
        /// <summary>
        /// URI结尾的小数点BUG是否可修复
        /// </summary>
        public static readonly bool IsUriField;
        /// <summary>
        /// 创建URI并修复小数点结尾的BUG
        /// </summary>
        /// <param name="url">URI字符串</param>
        /// <returns>创建的URI</returns>
        public static Uri Create(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) && IsUriField && unsafer.String.Last(url) == '.')
            {
                setAbsolute(uri, uri.AbsoluteUri + ".", flags.ShouldBeCompressed);
            }
            return uri;
        }
        /// <summary>
        /// 创建URI并修复绝对URI
        /// </summary>
        /// <param name="url">URI字符串</param>
        /// <param name="removeFlags">删除状态,比如flags.ShouldBeCompressed | flags.E_QueryNotCanonical</param>
        /// <returns>创建的URI</returns>
        public static Uri CreateAbsolute(string url, flags removeFlags)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) && IsUriField && uri.AbsoluteUri != url) setAbsolute(uri, url, removeFlags);
            return uri;
        }
        /// <summary>
        /// 修复绝对URI
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="absoluteUri">绝对URI</param>
        /// <param name="removeFlags">删除状态</param>
        private static void setAbsolute(Uri uri, string absoluteUri, flags removeFlags)
        {
            uriAbsolute(uriMoreInfo(uriInfo(uri)), absoluteUri);
            uriFlags.Setter(uri, (ulong)uriFlags.Getter(uri) & (ulong.MaxValue ^ (ulong)removeFlags));
        }

        static uri()
        {
            Assembly uriAssembly = typeof(Uri).Assembly;
            uriInfo = fieldGetValue.Creator.Create(typeof(Uri), "m_Info");
            uriFlags = fieldGetSetValue.Creator.Create(typeof(Uri), "m_Flags");
            uriMoreInfo = fieldGetValue.Creator.Create(uriAssembly, "System.Uri+UriInfo", "MoreInfo", false);
            uriAbsolute = fieldSetValue.createFieldSetValue<string>.Default.Create(uriAssembly, "System.Uri+MoreInfo", "AbsoluteUri", false);
            IsUriField = uriInfo != null && uriFlags.Getter != null && uriFlags.Setter != null && uriMoreInfo != null && uriAbsolute != null;
        }
    }
}
