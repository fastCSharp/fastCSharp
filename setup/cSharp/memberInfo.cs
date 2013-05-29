using System;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// 成员信息获取器接口
    /// </summary>
    public interface IMemberInfo
    {
        /// <summary>
        /// 获取成员名称
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员名称</returns>
        string GetName(int memberIndex);
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员类型</returns>
        Type GetType(int memberIndex);
        /// <summary>
        /// 所有成员数量
        /// </summary>
        int MemberCount { get; }
    }
    /// <summary>
    /// 自定义成员信息[showjim.setup]
    /// </summary>
    public class memberInfo
    {
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type MemberType;
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName;
        /// <summary>
        /// 成员编号
        /// </summary>
        public int MemberIndex;
        /// <summary>
        /// 转换成员信息
        /// </summary>
        /// <returns>成员信息</returns>
        internal setup.memberInfo create()
        {
            return new setup.memberInfo(MemberType, MemberName, MemberIndex);
        }
    }
}
