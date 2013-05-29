using System;
using System.Diagnostics;
using System.Reflection;

namespace fastCSharp
{
    /// <summary>
    /// 调用堆栈函数
    /// </summary>
    public static class stackFrame
    {
        /// <summary>
        /// 调用堆栈函数转字符串
        /// </summary>
        /// <param name="value">调用堆栈函数</param>
        /// <returns>字符串</returns>
        public static string toString(this StackFrame value)
        {
            if (value != null)
            {
                list<string>.unsafer values = new list<string>(9).Unsafer;
                MethodBase method = value.GetMethod();
                values.Add(method.ReflectedType.FullName);
                values.Add(" + ");
                values.Add(method.ToString());
                string file = value.GetFileName();
                if (file != null)
                {
                    values.Add(" in ");
                    values.Add(file);
                    values.Add(" line ");
                    values.Add(value.GetFileLineNumber().toString());
                    values.Add(" col ");
                    values.Add(value.GetFileColumnNumber().toString());
                }
                return string.Concat(values.List.ToArray());
            }
            return null;
        }
    }

}
