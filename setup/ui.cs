using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using fastCSharp.reflection;
using System.Diagnostics;

namespace fastCSharp.setup
{
    /// <summary>
    /// 安装界面
    /// </summary>
    internal partial class ui : Form
    {
        /// <summary>
        /// 安装控件容器名称
        /// </summary>
        public const string SetupControlName = "Setup";
        /// <summary>
        /// 部署控件容器名称
        /// </summary>
        public const string DeployControlName = "Deploy";
        /// <summary>
        /// 当前程序集
        /// </summary>
        public static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        /// <summary>
        /// 部署函数集合
        /// </summary>
        private static readonly keyValue<MethodInfo, auto>[] deployMethods = CurrentAssembly.GetTypes()
                .concat(type => methodInfo.GetMethods(type, memberFilter.Static, false).getArray(method => method.Method))
                .getFindArray(method => method.isParameter(typeof(bool), typeof(auto.parameter)))
                .getArray(method => new keyValue<MethodInfo, auto>(method, method.customAttribute<auto>()))
                .getFindArray(method => method.Value != null && method.Value.IsSetup);
        /// <summary>
        /// 部署函数名称集合
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> deployMethodNames = deployMethods.getDictionary(method => method.Key.fullName(), method => method.Key);
        /// <summary>
        /// 安装界面
        /// </summary>
        public ui()
        {
            InitializeComponent();
            threading.task.Default.Add(autoDeploy);
            Text = config.setup.Default.SetupTitle + " " + pub.fastCSharp + "安装界面";
            projectPath.Text = pub.ApplicationPath;
            createSetup();
            createDeploy();
        }
        /// <summary>
        /// 生成创建安装类型按钮
        /// </summary>
        private void createSetup()
        {
            FlowLayoutPanel createTypePanel = new FlowLayoutPanel();
            createTypePanel.Dock = DockStyle.Fill;
            createTypePanel.Name = SetupControlName;
            createTypePanel.AutoSize = true;
            this.type.Controls.Add(createTypePanel);

            keyValue<Type, auto>[] autos = CurrentAssembly.GetTypes()
                .getFind(type => !type.IsInterface && !type.IsAbstract && type.isInterface(typeof(IAuto)))
                .getArray(type => new keyValue<Type, auto>(type, type.customAttribute<auto>() ?? auto.NullAuto));
            foreach (keyValue<Type, auto> auto in autos.getFind(auto => auto.Value.IsSetup).notNull())
            {
                CheckBox createTypeCheckBox = new CheckBox();
                createTypeCheckBox.AutoSize = true;
                createTypeCheckBox.Name = auto.Key.FullName;
                createTypeCheckBox.Text = auto.Value.ShowName(auto.Key);
                createTypePanel.Controls.Add(createTypeCheckBox);
            }
        }
        /// <summary>
        /// 生成创建部署类型按钮
        /// </summary>
        private void createDeploy()
        {
            FlowLayoutPanel createDeployPanel = new FlowLayoutPanel();
            createDeployPanel.Dock = DockStyle.Fill;
            createDeployPanel.Name = DeployControlName;
            createDeployPanel.AutoSize = true;
            this.deploy.Controls.Add(createDeployPanel);

            foreach (keyValue<MethodInfo, auto> button in deployMethods)
            {
                CheckBox createDeployCheckBox = new CheckBox();
                createDeployCheckBox.AutoSize = true;
                createDeployCheckBox.Name = button.Key.fullName();
                createDeployCheckBox.Text = button.Value.ShowName(button.Key.DeclaringType);
                createDeployPanel.Controls.Add(createDeployCheckBox);
            }
        }
        /// <summary>
        /// 自动部署调用
        /// </summary>
        private void autoDeploy()
        {
            deployMethod(deployMethods.getFindArray(method => method.Value.IsAuto)
                .getArray(method => method.Key), checkConfig.Checked);
            setup.error.Open(true);
        }
        /// <summary>
        /// 部署
        /// </summary>
        /// <param name="methods">部署函数集合</param>
        /// <param name="parameter">部署参数</param>
        private void deployMethod(ICollection<MethodInfo> methods, bool isConfig)
        {
            object[] parameters = new object[1];
            foreach (MethodInfo method in methods)
            {
                try
                {
                    parameters[0] = isConfig ? config.pub.Default.LoadConfig(autoParameter, method.DeclaringType.Name + "." + method.Name) : autoParameter;
                    if (!(bool)method.Invoke(null, parameters)) error.Add(method.fullName() + " 部署失败");
                }
                catch (Exception error)
                {
                    setup.error.Add(error);
                }
            }
        }
        /// <summary>
        /// 选择安装目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFolderButton_Click(object sender, EventArgs e)
        {
            openFolder.SelectedPath = projectPath.Text;
            openFolder.ShowDialog();
            if (openFolder.SelectedPath != null && openFolder.SelectedPath.Length != 0) projectPath.Text = openFolder.SelectedPath;
        }
        /// <summary>
        /// 选择安装文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFile.FileName = assemblyPath.Text;
            openFile.ShowDialog();
            if (openFile.FileName != null && openFile.FileName.Length != 0)
            {
                assemblyPath.Text = openFile.FileName;
                if (openFile.FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) || openFile.FileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    FileInfo file = new FileInfo(openFile.FileName);
                    projectName.Text = defaultNamespace.Text = file.Name.Substring(0, file.Name.Length - 4);
                    if (projectName.Text.StartsWith(config.setup.Default.BaseNamespace + ".", StringComparison.Ordinal))
                    {
                        projectName.Text = projectName.Text.Substring(config.setup.Default.BaseNamespace.Length + 1);
                    }
                    projectPath.Text = file.DirectoryName;
                    int pathIndex = (projectPath.Text + @"\").LastIndexOf(@"\bin\");
                    if (++pathIndex != 0) projectPath.Text = projectPath.Text.Substring(0, pathIndex);
                }
            }
        }
        /// <summary>
        /// 安装参数
        /// </summary>
        private auto.parameter autoParameter
        {
            get
            {
                return new auto.parameter(projectName.Text, projectPath.Text, assemblyPath.Text, defaultNamespace.Text);
            }
        }
        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setup_Click(object sender, EventArgs e)
        {
            try
            {
                keyValue<Type, auto>[] autos = this.type.Controls[SetupControlName].Controls.toGeneric<Control>()
                    .getFind(control => ((CheckBox)control).Checked)
                    .getArray(control => CurrentAssembly.GetType(control.Name))
                    .getArray(type => new keyValue<Type, auto>(type, type.customAttribute<setup.auto>() ?? auto.NullAuto));
                Setup(autos, autoParameter, checkConfig.Checked);

                deployMethod(deploy.Controls[DeployControlName].Controls.toGeneric<Control>()
                    .getFind(control => ((CheckBox)control).Checked)
                    .getArray(control => deployMethodNames[control.Name]), checkConfig.Checked);
            }
            catch (Exception error)
            {
                setup.error.Add(error);
            }
            setup.error.Open(false);
            if (!setup.error.IsError) MessageBox.Show("安装完毕");
            setup.error.Clear();
        }
        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="autos">安装类型属性</param>
        /// <param name="parameter">安装参数</param>
        /// <returns>安装是否成功</returns>
        internal static bool Setup(ICollection<keyValue<Type, auto>> autos, auto.parameter parameter, bool isConfig)
        {
            bool isSetup = true;
            if (autos != null)
            {
                try
                {
                    HashSet<Type> types = autos.getHash(value => value.Key);
                    keyValue<Type, Type>[] depends = autos
                        .getFind(value => value.Value.DependType != null && types.Contains(value.Value.DependType))
                        .getArray(value => new keyValue<Type, Type>(value.Key, value.Value.DependType));
                    foreach (Type type in algorithm.topologySort.Sort(depends, types, true))
                    {
                        //Stopwatch time = new Stopwatch();
                        //time.Start();
                        if (!(CurrentAssembly.CreateInstance(type.FullName) as IAuto)
                            .Run(isConfig ? config.pub.Default.LoadConfig(parameter.Copy(), type.ToString()) : parameter))
                        {
                            error.Add(type.fullName() + " 安装失败");
                            isSetup = false;
                        }
                        //time.Stop();
                        //error.Message(parameter.ProjectName + " " + type.FullName + " : " + time.ElapsedMilliseconds.ToString() + "ms");
                    }
                }
                finally
                {
                    cSharp.coder.Output(parameter);
                }
            }
            return isSetup;
        }
        /// <summary>
        /// 显示界面
        /// </summary>
        public static void ShowSetup()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ui());
        }
    }
}
