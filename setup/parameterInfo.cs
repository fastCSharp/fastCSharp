using System;
using System.Reflection;

namespace fastCSharp.setup
{
    /// <summary>
    /// 参数信息
    /// </summary>
    internal class parameterInfo
    {
        /// <summary>
        /// 参数信息
        /// </summary>
        public ParameterInfo Parameter;
        /// <summary>
        /// 参数索引位置
        /// </summary>
        public int ParameterIndex;
        /// <summary>
        /// 参数类型
        /// </summary>
        public memberType ParameterType;
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName;
        /// <summary>
        /// 参数连接名称，最后一个参数不带逗号
        /// </summary>
        public string ParameterJoinName;
        /// <summary>
        /// 带引用修饰的参数名称
        /// </summary>
        public string ParameterTypeRefName
        {
            get
            {
                return getRefName(ParameterType.FullName);
            }
        }
        /// <summary>
        /// 带引用修饰的参数连接名称，最后一个参数不带逗号
        /// </summary>
        public string ParameterJoinRefName
        {
            get
            {
                return getRefName(ParameterJoinName);
            }
        }
        /// <summary>
        /// 是否引用参数
        /// </summary>
        public bool IsRef;
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool IsOut;
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool IsRefOrOut
        {
            get { return IsRef || IsOut; }
        }
        /// <summary>
        /// 参数引用前缀
        /// </summary>
        public string ParameterRef
        {
            get
            {
                return getRefName(null);
            }
        }
        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="parameter">参数信息</param>
        /// <param name="index">参数索引位置</param>
        /// <param name="isLast">是否最后一个参数</param>
        private parameterInfo(ParameterInfo parameter, int index, bool isLast)
        {
            Parameter = parameter;
            ParameterIndex = index;
            Type parameterType = parameter.ParameterType;
            if (parameterType.IsByRef)
            {
                if (parameter.IsOut) IsOut = true;
                else IsRef = true;
                ParameterType = parameterType.GetElementType();
            }
            else ParameterType = parameterType;
            ParameterName = Parameter.Name;
            ParameterJoinName = isLast ? ParameterName : (ParameterName + ", ");
        }
        /// <summary>
        /// 获取带引用修饰的名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>带引用修饰的名称</returns>
        private string getRefName(string name)
        {
            if (IsOut) return "out " + name;
            if (IsRef) return "ref " + name;
            return name;
        }
        /// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <returns>参数信息集合</returns>
        public static parameterInfo[] Get(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.length() != 0)
            {
                int index = 0;
                return parameters.getArray(value => new parameterInfo(value, index, ++index == parameters.Length));
            }
            return nullValue<parameterInfo>.Array;
        }/// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="parameters">参数信息集合</param>
        /// <returns>参数信息集合</returns>
        public static parameterInfo[] Get(parameterInfo[] parameters)
        {
            if (parameters.length() != 0)
            {
                parameterInfo parameter = parameters[parameters.Length - 1];
                parameters[parameters.Length - 1] = new parameterInfo(parameter.Parameter, parameter.ParameterIndex, true);
            }
            return parameters;
        }
    }
}
