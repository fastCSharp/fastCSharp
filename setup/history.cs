using System;
using fastCSharp.reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 对象转换历史对象,用于检测循环引用
    /// </summary>
    public abstract class history
    {
        /// <summary>
        /// 历史对象
        /// </summary>
        protected internal list<object> parents;
        /// <summary>
        /// 添加历史对象
        /// </summary>
        /// <param name="value">历史对象</param>
        public void Push(object value)
        {
            if (value != null)
            {
                if (parents.IndexOf(value) != -1)
                {
                    log.Default.Throw(@"循环引用 : " + value.GetType().fullName() + @"
" + parents.joinString(" => ", loopValue => loopValue.GetType().fullName()), true, true);
                }
                parents.Add(this);
            }
        }
        /// <summary>
        /// 弹出历史对象
        /// </summary>
        /// <param name="value">历史对象</param>
        public void Pop(object value)
        {
            if (value != null)
            {
                if (parents.lastOrDefault() != value)
                {
                    log.Default.Throw(@"历史对象不匹配 : " + value.GetType().fullName() + @"
" + parents.joinString(" => ", loopValue => loopValue.GetType().fullName()), true, true);
                }
                parents.Pop();
            }
        }
    }
}
