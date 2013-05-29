using System;
using fastCSharp.setup.cSharp;

namespace fastCSharp.sql.msSql
{
    /// <summary>
    /// 格式化查询信息
    /// </summary>
    internal unsafe class selectQuery
    {
        /// <summary>
        /// 跳过记录数量
        /// </summary>
        public int SkipCount;
        /// <summary>
        /// 获取记录数量,0表示不限
        /// </summary>
        public int GetCount;
        /// <summary>
        /// 查询字符串
        /// </summary>
        public string Where;
        /// <summary>
        /// 排序字符串集合,false为升序,true为降序
        /// </summary>
        public keyValue<string, bool>[] Orders;
        ///// <summary>
        ///// 排序成员名称集合
        ///// </summary>
        //public list<string> OrderMemberNames;
        /// <summary>
        /// 降序排序字符串字节数
        /// </summary>
        public int OrderSize;
        /// <summary>
        /// 降序排序字符串数量
        /// </summary>
        public int OrderDescCount;
        /// <summary>
        /// 是否需要查询
        /// </summary>
        public bool IsSelect;
        /// <summary>
        /// 是否已经创建查询索引
        /// </summary>
        public bool IsCreatedIndex;
        /// <summary>
        /// 排序字符串
        /// </summary>
        public string Order
        {
            get
            {
                if (Orders != null)
                {
                    string orderString = new string(',', OrderSize + (OrderDescCount << 2) + OrderDescCount + Orders.Length + 9);
                    fixed (char* orderFixed = orderString)
                    {
                        char* write = orderFixed;
                        fastCSharp.unsafer.String.Copy(" order by ", write);
                        write += 10;
                        foreach (keyValue<string, bool> order in Orders)
                        {
                            fastCSharp.unsafer.String.Copy(order.Key, write);
                            if (order.Value)
                            {
                                fastCSharp.unsafer.String.Copy(" desc", write += order.Key.Length);
                                ++write;
                            }
                            else write += order.Key.Length + 1;
                        }
                    }
                    return orderString;
                }
                return null;
            }
        }
        /// <summary>
        /// 是否关键字单排序
        /// </summary>
        /// <param name="name">关键字名称</param>
        /// <returns>是否关键字单排序</returns>
        public bool IsOrder(string name)
        {
            return Orders != null && Orders.Length == 1 && Orders[0].Key == name;
        }
    }
}
