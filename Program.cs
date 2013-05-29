using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using fastCSharp.reflection;

namespace fastCSharp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //args = new string[] { "", null, null, null };
            switch (args.Length)
            {
                case 4:
                    auto(args[1] != null ? new setup.auto.parameter(args[0], args[1], args[2], args[3]) : createParameter(args[0]));
                    break;
                case 1:
                    setup.define.Setup(new setup.auto.parameter(null, args[0], null, null));
                    break;
                case 0:
                    setup.ui.ShowSetup();
                    break;
                default:
                    Console.WriteLine("未知安装");
                    Console.ReadKey();
                    break;
            }
        }
        /// <summary>
        /// 自动安装
        /// </summary>
        /// <param name="parameter">安装参数</param>
        static void auto(setup.auto.parameter parameter)
        {
            if (parameter.ProjectPath.length() != 0 && Directory.Exists(parameter.ProjectPath))
            {
                try
                {
                    list<keyValue<Type, setup.auto>> autos = setup.ui.CurrentAssembly.GetTypes()
                        .getFind(type => !type.IsInterface && !type.IsAbstract && type.isInterface(typeof(setup.IAuto)))
                        .getArray(type => new keyValue<Type, setup.auto>(type, type.customAttribute<setup.auto>()))
                        .getFind(value => value.Value != null && value.Value.IsSetup && value.Value.IsAuto);
                    setup.ui.Setup(autos, parameter, false);
                }
                catch (Exception error)
                {
                    setup.error.Add(error);
                }
            }
            else setup.error.Add("项目路径不存在 : " + parameter.ProjectPath);
            setup.error.Open(true);
        }
        /// <summary>
        /// 根据项目名称构造安装参数
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <returns>安装参数</returns>
        static setup.auto.parameter createParameter(string projectName)
        {
            string projectPath = null, assemblyPath = null, projectNamespace = null;
            if (projectName.length() != 0)
            {
                projectPath = config.setup.Default.ProjectPath + projectName + @"\";
                projectNamespace = config.setup.Default.BaseNamespace + "." + projectName;
                assemblyPath = checkAssemblyPath(projectPath + @"bin\Release\" + projectNamespace)
                    ?? checkAssemblyPath(projectPath + @"bin\Debug\" + projectNamespace)
                    ?? checkAssemblyPath(projectPath + @"bin\" + projectNamespace)
                    ?? checkAssemblyPath(projectPath + projectNamespace);
            }
            else
            {
                projectName = projectNamespace = pub.fastCSharp;
                projectPath = config.setup.Default.FastCSharpPath;
                assemblyPath = pub.ApplicationPath + pub.fastCSharp + ".exe";
            }
            return new setup.auto.parameter(projectName, projectPath, assemblyPath, projectNamespace);
        }
        /// <summary>
        /// 检测程序集文件路径
        /// </summary>
        /// <param name="path">程序集文件路径</param>
        /// <returns>程序集文件路径,失败返回null</returns>
        static string checkAssemblyPath(string path)
        {
            if (File.Exists(path + ".dll")) return path + ".dll";
            if (File.Exists(path + ".exe")) return path + ".exe";
            return null;
        }
    }
}
