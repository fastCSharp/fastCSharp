using System;
using System.Threading;
using fastCSharp.threading;

namespace fastCSharp
{
    /// <summary>
    /// 函数调用委托
    /// </summary>
    public delegate void action();
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType">输入参数类型</typeparam>
    /// <param name="parameter">输入参数</param>
    public delegate void action<in parameterType>(parameterType parameter);
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType1">输入参数类型</typeparam>
    /// <typeparam name="parameterType2">输入参数类型</typeparam>
    /// <param name="parameter1">输入参数</param>
    /// <param name="parameter2">输入参数</param>
    public delegate void action<in parameterType1, in parameterType2>(parameterType1 parameter1, parameterType2 parameter2);
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType1">输入参数类型</typeparam>
    /// <typeparam name="parameterType2">输入参数类型</typeparam>
    /// <typeparam name="parameterType3">输入参数类型</typeparam>
    /// <param name="parameter1">输入参数</param>
    /// <param name="parameter2">输入参数</param>
    /// <param name="parameter3">输入参数</param>
    public delegate void action<in parameterType1, in parameterType2, in parameterType3>(parameterType1 parameter1, parameterType2 parameter2, parameterType3 parameter3);

    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <returns>返回值</returns>
    public delegate returnType func<out returnType>();
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType">输入参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <param name="parameter">输入参数</param>
    /// <returns>返回值</returns>
    public delegate returnType func<in parameterType, out returnType>(parameterType parameter);
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType1">输入参数类型</typeparam>
    /// <typeparam name="parameterType2">输入参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <param name="parameter1">输入参数</param>
    /// <param name="parameter2">输入参数</param>
    /// <returns>返回值</returns>
    public delegate returnType func<in parameterType1, in parameterType2, out returnType>(parameterType1 parameter1, parameterType2 parameter2);
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType1">输入参数类型</typeparam>
    /// <typeparam name="parameterType2">输入参数类型</typeparam>
    /// <typeparam name="parameterType3">输入参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <param name="parameter1">输入参数</param>
    /// <param name="parameter2">输入参数</param>
    /// <param name="parameter3">输入参数</param>
    /// <returns>返回值</returns>
    public delegate returnType func<in parameterType1, in parameterType2, in parameterType3, out returnType>(parameterType1 parameter1, parameterType2 parameter2, parameterType3 parameter3);
    /// <summary>
    /// 函数调用委托
    /// </summary>
    /// <typeparam name="parameterType1">输入参数类型</typeparam>
    /// <typeparam name="parameterType2">输入参数类型</typeparam>
    /// <typeparam name="parameterType3">输入参数类型</typeparam>
    /// <typeparam name="parameterType4">输入参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    /// <param name="parameter1">输入参数</param>
    /// <param name="parameter2">输入参数</param>
    /// <param name="parameter3">输入参数</param>
    /// <param name="parameter4">输入参数</param>
    /// <returns>返回值</returns>
    public delegate returnType func<in parameterType1, in parameterType2, in parameterType3, in parameterType4, out returnType>(parameterType1 parameter1, parameterType2 parameter2, parameterType3 parameter3, parameterType4 parameter4);
    /// <summary>
    /// 字符串转换委托
    /// </summary>
    /// <typeparam name="returnType">目标类型</typeparam>
    /// <param name="stringValue">字符串</param>
    /// <param name="value">目标对象</param>
    /// <returns>是否转换成功</returns>
    public delegate bool tryParse<returnType>(string stringValue, out returnType value);
}
