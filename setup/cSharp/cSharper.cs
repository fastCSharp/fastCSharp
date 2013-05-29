using System;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp.setup.cSharp
{
    /// <summary>
    /// CSharp模板生成基类
    /// </summary>
    internal abstract class cSharper
    {
        /// <summary>
        /// 类定义生成
        /// </summary>
        public class definition
        {
            /// <summary>
            /// 类定义开始
            /// </summary>
            private stringBuilder start = new stringBuilder();
            /// <summary>
            /// 类定义开始
            /// </summary>
            public string Start
            {
                get
                {
                    return start.ToString();
                }
            }
            /// <summary>
            /// 类定义结束
            /// </summary>
            private stringBuilder end = new stringBuilder();
            /// <summary>
            /// 类定义结束
            /// </summary>
            public string End
            {
                get
                {
                    return end.ToString();
                }
            }
            /// <summary>
            /// 类定义生成
            /// </summary>
            /// <param name="type">类型</param>
            public definition(Type type, bool isPartial, bool isClass)
            {
                create(type, isPartial, isClass);
                end.Reverse();
            }
            /// <summary>
            /// 生成类定义
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="isPartial">是否部分定义</param>
            /// <param name="isClass">是否建立类定义</param>
            private void create(Type type, bool isPartial, bool isClass)
            {
                if (type.ReflectedType == null)
                {
                    start.Add("namespace " + type.Namespace + @"
{");
                    end.Add(@"
}");
                }
                else
                {
                    create(type.ReflectedType.IsGenericType ? type.ReflectedType.MakeGenericType(type.GetGenericArguments()) : type.ReflectedType, true, true);
                }
                if (isClass)
                {
                    start.Add(@"
    " + (type.IsPublic ? "public" : null)
              + (type.IsAbstract ? " abstract" : null)
              + (isPartial ? " partial" : null)
              + (type.IsInterface ? " interface" : " class")
              + " " + type.Name + (type.IsGenericType ? "<" + type.GetGenericArguments().joinString(", ", x => x.fullName()) + ">" : null) + @"
    {");
                    end.Add(@"
    }");
                }
            }
        }
        /// <summary>
        /// 类型
        /// </summary>
        internal memberType type;
        /// <summary>
        /// 注册类型
        /// </summary>
        internal memberType coderType;
        /// <summary>
        /// 注册类型
        /// </summary>
        internal memberType CodeType
        {
            get
            {
                return coderType ?? type;
            }
        }
        /// <summary>
        /// 类名称定义
        /// </summary>
        public string TypeNameDefinition
        {
            get
            {
                if (type.Type == null) return null;
                return (type.Type.IsPublic ? "public " : null) + "partial " + (type.IsNull ? "class" : "struct") + " " + type.TypeName;
            }
        }
        /// <summary>
        /// 类名称
        /// </summary>
        public string TypeName
        {
            get { return type.TypeName; }
        }
        /// <summary>
        /// 生成的代码
        /// </summary>
        protected stringBuilder _code_ = new stringBuilder();
        /// <summary>
        /// 代码段
        /// </summary>
        protected Dictionary<string, string> _partCodes_ = new Dictionary<string, string>();
        /// <summary>
        /// 临时逻辑变量
        /// </summary>
        protected bool _if_;
        /// <summary>
        /// 当前循环索引
        /// </summary>
        protected int _loopIndex_;
        /// <summary>
        /// 当前循环数量
        /// </summary>
        protected int _loopCount_;
        ///// <summary>
        ///// 当前循环集合
        ///// </summary>
        //protected object _loopValues_;
        ///// <summary>
        ///// 当前循环值
        ///// </summary>
        //protected object _loopValue_;
        /// <summary>
        /// 输出类定义开始段代码
        /// </summary>
        /// <returns>类定义</returns>
        protected definition outStart()
        {
            if (fastCSharp.setup.cSharp.coder.Add(GetType(), CodeType.Type, null))
            {
                definition definition = new definition(type, true, false);
                _code_.Empty();
                _code_.Add(definition.Start);
                return definition;
            }
            return null;
        }
        /// <summary>
        /// 输出类定义结束段代码
        /// </summary>
        /// <param name="definition">类定义</param>
        protected void outEnd(fastCSharp.setup.cSharp.cSharper.definition definition)
        {
            if (definition != null)
            {
                _code_.Add(definition.End);
                fastCSharp.setup.cSharp.coder.Add(GetType(), CodeType.Type, _code_.ToString());
            }
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected virtual void create(bool isOut)
        {
            log.Default.Throw(log.exceptionType.ErrorOperation);
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        protected void create()
        {
            create(true);
        }
    }
    /// <summary>
    /// 自定义属性模板生成基类
    /// </summary>
    /// <typeparam name="attributeType">自定义属性类型</typeparam>
    internal abstract class cSharper<attributeType> : cSharper, IAuto
        where attributeType : setup.ignore
    {
        /// <summary>
        /// 自动安装参数
        /// </summary>
        protected auto.parameter AutoParameter;
        /// <summary>
        /// 程序集
        /// </summary>
        protected Assembly assembly;
        /// <summary>
        /// 自定义属性
        /// </summary>
        public attributeType Attribute;
        /// <summary>
        /// 是否搜索父类属性
        /// </summary>
        public virtual bool IsBaseType
        {
            get { return false; }
        }
        /// <summary>
        /// 自定义属性是否可继承
        /// </summary>
        public virtual bool IsInheritAttribute
        {
            get { return false; }
        }
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        public bool Run(auto.parameter parameter)
        {
            if (parameter != null)
            {
                AutoParameter = parameter;
                assembly = parameter.Assembly;
                keyValue<Type, attributeType>[] types = parameter.Types
                    .getArray(type => new keyValue<Type, attributeType>(type, type.customAttribute<attributeType>(IsBaseType, IsInheritAttribute)))
                    .getFindArray(attribute => attribute.Value != null && attribute.Value.IsSetup);
                foreach (keyValue<Type, attributeType> ajax in types)
                {
                    type = ajax.Key;
                    Attribute = ajax.Value;
                    NextCreate();
                }
                onCreated();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否可调用构造函数
        /// </summary>
        /// <returns>是否可调用构造函数</returns>
        protected bool isConstructor()
        {
            if (type.Type.IsAbstract || type.Type.IsInterface || type.Type.IsEnum)
            {
                error.Message(type.Type.fullName() + " 无法创建 接口/抽象类/枚举 的实例");
                return false;
            }
            else if (type.Type.IsClass && type.Type.GetConstructor(nullValue<Type>.Array) == null)
            {
                error.Message(type.Type.fullName() + " 找不到无参构造函数");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected abstract void NextCreate();
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected abstract void onCreated();
    }
}
