using System;
using System.Reflection;

namespace fastCSharp.reflection
{
    /// <summary>
    /// 静态方法委托
    /// </summary>
    public class staticMethodDelegate
    {
        /// <summary>
        /// 创建方法委托
        /// </summary>
        /// <typeparam name="parameterType1">参数1类型</typeparam>
        /// <typeparam name="parameterType2">参数2类型</typeparam>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="method">方法信息</param>
        /// <returns>方法委托</returns>
        public static func<parameterType1, parameterType2, returnType> Create<parameterType1, parameterType2, returnType>(MethodInfo method)
        {
            if (method != null)
            {
                try
                {
                    return (func<parameterType1, parameterType2, returnType>)Delegate.CreateDelegate(typeof(func<parameterType1, parameterType2, returnType>), method);
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
        /// <returns>方法委托</returns>
        public static func<object, object> Create(MethodInfo method)
        {
            if (method != null && method.ReturnType != typeof(void))
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.length() == 1)
                {
                    try
                    {
                        return ((IStaticMethodDelegate)Activator.CreateInstance(typeof(staticMethodDelegate<,>).MakeGenericType(parameters[0].ParameterType, method.ReturnType), method)).Invoke;
                    }
                    catch (Exception error)
                    {
                        fastCSharp.log.Default.Add(error, method.fullName(), true);
                    }
                }
            }
            return null;
        }
    }
    /// <summary>
    /// 方法委托
    /// </summary>
    internal interface IStaticMethodDelegate
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值</returns>
        object Invoke(object parameter);
    }
    /// <summary>
    /// 2个参数的方法委托
    /// </summary>
    /// <typeparam name="parameterType1">参数1类型</typeparam>
    /// <typeparam name="parameterType2">参数2类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal struct staticMethodDelegate<parameterType, returnType> : IStaticMethodDelegate
    {

        /// <summary>
        /// 方法委托
        /// </summary>
        private func<parameterType, returnType> func;
        /// <summary>
        /// 方法委托
        /// </summary>
        /// <param name="method">方法信息</param>
        public staticMethodDelegate(MethodInfo method)
        {
            this.func = (func<parameterType, returnType>)Delegate.CreateDelegate(typeof(func<parameterType, returnType>), method);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值</returns>
        public object Invoke(object parameter)
        {
            return func((parameterType)parameter);
        }
    }
}
