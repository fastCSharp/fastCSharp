using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 成员方法委托
    /// </summary>
    public static class methodDelegate
    {
        /// <summary>
        /// 创建方法委托
        /// </summary>
        /// <typeparam name="parameterType">参数类型</typeparam>
        /// <param name="method">方法信息</param>
        /// <returns>方法委托</returns>
        public static action<object, parameterType> Create<parameterType>(MethodInfo method)
        {
            if (method != null)
            {
                try
                {
                    return ((IMethodDelegate<parameterType>)Activator.CreateInstance(typeof(methodDelegate<,>).MakeGenericType(method.DeclaringType, typeof(parameterType)), method)).Invoke;
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, method.fullName(), true);
                }
            }
            return null;
        }
    }
    /// <summary>
    /// 方法委托
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    public static class methodTargetDelegate<targetType>
    {
        /// <summary>
        /// 创建方法委托
        /// </summary>
        /// <typeparam name="parameterType">参数类型</typeparam>
        /// <param name="method">方法信息</param>
        /// <returns>方法委托</returns>
        public static action<targetType, parameterType> Create<parameterType>(MethodInfo method)
        {
            if (method != null)
            {
                try
                {
                    return (action<targetType, parameterType>)Delegate.CreateDelegate(typeof(action<targetType, parameterType>), null, method);
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, method.fullName(), true);
                }
            }
            return null;
        }
        /// <summary>
        /// 创建方法委托
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="parameterType">参数类型</param>
        /// <returns>方法委托</returns>
        public static action<targetType, object> Create(MethodInfo method, Type parameterType)
        {
            if (method != null && parameterType != null)
            {
                try
                {
                    return ((IMethodTargetDelegate<targetType>)Activator.CreateInstance(typeof(methodDelegate<,>).MakeGenericType(method.DeclaringType, parameterType), method)).Invoke;
                }
                catch (Exception error)
                {
                    fastCSharp.log.Default.Add(error, method.fullName() + " + " + parameterType.FullName, true);
                }
            }
            return null;
        }
    }
    /// <summary>
    /// 方法委托接口
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    internal interface IMethodTargetDelegate<targetType>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="parameter">参数值</param>
        void Invoke(targetType target, object parameter);
    }
    /// <summary>
    /// 方法委托接口
    /// </summary>
    /// <typeparam name="parameterType">参数类型</typeparam>
    internal interface IMethodDelegate<parameterType>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="parameter">参数值</param>
        void Invoke(object target, parameterType parameter);
    }
    /// <summary>
    /// 方法委托
    /// </summary>
    /// <typeparam name="targetType">目标对象类型</typeparam>
    /// <typeparam name="parameterType">参数类型</typeparam>
    internal struct methodDelegate<targetType, parameterType> : IMethodTargetDelegate<targetType>, IMethodDelegate<parameterType>
    {
        /// <summary>
        /// 方法委托
        /// </summary>
        private action<targetType, parameterType> method;
        /// <summary>
        /// 方法委托
        /// </summary>
        /// <param name="method">方法信息</param>
        public methodDelegate(MethodInfo method)
        {
            this.method = (action<targetType, parameterType>)Delegate.CreateDelegate(typeof(action<targetType, parameterType>), null, method);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="parameter">参数值</param>
        public void Invoke(targetType target, object parameter)
        {
            method(target, (parameterType)parameter);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="parameter">参数值</param>
        public void Invoke(object target, parameterType parameter)
        {
            method((targetType)target, parameter);
        }
    }
}
