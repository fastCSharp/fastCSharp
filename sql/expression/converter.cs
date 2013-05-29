#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;
using fastCSharp.reflection;
#if DOTNET35
using fastCSharp.sql.expression;
#else
using System.Linq.Expressions;
#endif

namespace fastCSharp.sql.expression
{
    /// <summary>
    /// 委托关联表达式转SQL表达式
    /// </summary>
    public abstract class converter
    {
        /// <summary>
        /// 获取名称类型
        /// </summary>
        public enum getNameType
        {
            /// <summary>
            /// 无名称
            /// </summary>
            None,
            /// <summary>
            /// 参数名称
            /// </summary>
            Parameter,
            /// <summary>
            /// 实体名称
            /// </summary>
            Entity,
        }
        /// <summary>
        /// 表达式缓存
        /// </summary>
        protected class nameCache
        {
            /// <summary>
            /// 无名称
            /// </summary>
            public string None;
            /// <summary>
            /// 参数名称
            /// </summary>
            public string Parameter;
            /// <summary>
            /// 实体名称
            /// </summary>
            public string Entity;
            /// <summary>
            /// 参数成员名称集合
            /// </summary>
            public list<string> ParameterMemberNames;
            /// <summary>
            /// 获取表达式缓存
            /// </summary>
            /// <param name="type">获取名称类型</param>
            /// <returns>表达式缓存</returns>
            public string this[getNameType nameType]
            {
                get
                {
                    switch (nameType)
                    {
                        case getNameType.Parameter: return Parameter;
                        case getNameType.Entity: return Entity;
                        default: return None;
                    }
                }
                set
                {
                    switch (nameType)
                    {
                        case getNameType.Parameter:
                            Parameter = value;
                            break;
                        case getNameType.Entity:
                            Entity = value;
                            break;
                        default:
                            None = value;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// lambda表达式
        /// </summary>
        protected LambdaExpression expression;
        /// <summary>
        /// 获取参数名称方式
        /// </summary>
        protected getNameType nameType;
        /// <summary>
        /// 获取参数名称方式
        /// </summary>
        public getNameType NameType { get { return nameType; } }
        /// <summary>
        /// 表格名称
        /// </summary>
        protected string name;
        /// <summary>
        /// 表格名称
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// 表格名称集合
        /// </summary>
        protected staticDictionary<string, string> names;
        /// <summary>
        /// SQL流
        /// </summary>
        protected charStream stream;
        /// <summary>
        /// SQL流
        /// </summary>
        public charStream Stream { get { return stream; } }
        /// <summary>
        /// 参数成员名称集合
        /// </summary>
        public list<string> parameterMemberNames;
        /// <summary>
        /// 转换器
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="nameType">获取参数名称方式</param>
        protected converter(LambdaExpression expression, getNameType nameType)
        {
            this.expression = expression;
            this.nameType = nameType;
            int parameterCount = expression.Parameters.length();
            switch (nameType)
            {
                case getNameType.Parameter:
                case getNameType.Entity:
                    if (parameterCount == 0) this.nameType = getNameType.None;
                    else if (parameterCount == 1) name = getName(expression.Parameters[0]);
                    else names = new staticDictionary<string, string>(expression.Parameters.getArray(parameter => new keyValue<string, string>(parameter.Name, getName(parameter))));
                    break;
                default:
                    if (parameterCount > 1)
                    {
                        fastCSharp.log.Default.Add("不止一个参数 " + parameterCount.toString(), true, true);
                        nameType = getNameType.Entity;
                    }
                    break;
            }
        }
        /// <summary>
        /// 根据参数表达式获取参数名称
        /// </summary>
        /// <param name="parameter">参数表达式</param>
        /// <returns>参数名称</returns>
        private string getName(ParameterExpression parameter)
        {
            if (NameType == getNameType.Entity)
            {
                Type type = parameter.Type;
                setup.cSharp.sqlTable attributeType = type.customAttribute<setup.cSharp.sqlTable>();
                if (attributeType != null) return attributeType.TableName ?? type.Name;
                return type.Name;
            }
            else return parameter.Name;
        }
        /// <summary>
        /// 获取表格名称
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>表格名称,失败返回null</returns>
        public string GetName(string name)
        {
            return names.Get(name, null);
        }
    }
}
