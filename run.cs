using System;

namespace fastCSharp
{
    /// <summary>
    /// 泛型任务信息
    /// </summary>
    /// <typeparam name="parameterType">任务执行参数类型</typeparam>
    public struct run<parameterType>
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        private action<parameterType> func;
        /// <summary>
        /// 任务执行参数
        /// </summary>
        private parameterType parameter;
        /// <summary>
        /// 执行任务
        /// </summary>
        private void action()
        {
            func(parameter);
        }
        /// <summary>
        /// 泛型任务信息
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">任务执行参数</param>
        /// <returns>泛型任务信息</returns>
        public static action Create(action<parameterType> action, parameterType parameter)
        {
            return new run<parameterType> { func = action, parameter = parameter }.action;
        }
    }
    /// <summary>
    /// 泛型任务信息
    /// </summary>
    /// <typeparam name="parameterType">任务执行参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public struct run<parameterType, returnType>
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        private func<parameterType, returnType> func;
        /// <summary>
        /// 返回值执行委托
        /// </summary>
        private action<returnType> onReturn;
        /// <summary>
        /// 任务执行参数
        /// </summary>
        private parameterType parameter;
        /// <summary>
        /// 执行任务
        /// </summary>
        private void action()
        {
            onReturn(func(parameter));
        }
        /// <summary>
        /// 泛型任务信息
        /// </summary>
        /// <param name="func">任务执行委托</param>
        /// <param name="parameter">任务执行参数</param>
        /// <param name="onReturn">返回值执行委托</param>
        /// <returns>泛型任务信息</returns>
        public static action Create(func<parameterType, returnType> func, parameterType parameter, action<returnType> onReturn)
        {
            return new run<parameterType, returnType> { func = func, parameter = parameter, onReturn = onReturn }.action;
        }
    }
}
