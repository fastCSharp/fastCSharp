namespace fastCSharp.setup
{
    partial class ui
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.openFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.assemblyPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.projectPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.projectName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.defaultNamespace = new System.Windows.Forms.TextBox();
            this.page = new System.Windows.Forms.TabControl();
            this.path = new System.Windows.Forms.TabPage();
            this.type = new System.Windows.Forms.TabPage();
            this.deploy = new System.Windows.Forms.TabPage();
            this.test = new System.Windows.Forms.TabPage();
            this.setupButton = new System.Windows.Forms.Button();
            this.checkConfig = new System.Windows.Forms.CheckBox();
            this.page.SuspendLayout();
            this.path.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(259, 166);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "选择文件";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(107, 166);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(75, 23);
            this.openFolderButton.TabIndex = 1;
            this.openFolderButton.Text = "选择目录";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // assemblyPath
            // 
            this.assemblyPath.Location = new System.Drawing.Point(89, 6);
            this.assemblyPath.Name = "assemblyPath";
            this.assemblyPath.Size = new System.Drawing.Size(352, 21);
            this.assemblyPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "程序集文件：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "项目路径：";
            // 
            // projectPath
            // 
            this.projectPath.Location = new System.Drawing.Point(89, 42);
            this.projectPath.Name = "projectPath";
            this.projectPath.Size = new System.Drawing.Size(352, 21);
            this.projectPath.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "项目名称：";
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(89, 81);
            this.projectName.Name = "projectName";
            this.projectName.Size = new System.Drawing.Size(182, 21);
            this.projectName.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "命名空间：";
            // 
            // defaultNamespace
            // 
            this.defaultNamespace.Location = new System.Drawing.Point(89, 125);
            this.defaultNamespace.Name = "defaultNamespace";
            this.defaultNamespace.Size = new System.Drawing.Size(182, 21);
            this.defaultNamespace.TabIndex = 8;
            // 
            // page
            // 
            this.page.Controls.Add(this.path);
            this.page.Controls.Add(this.type);
            this.page.Controls.Add(this.deploy);
            this.page.Controls.Add(this.test);
            this.page.Location = new System.Drawing.Point(12, 12);
            this.page.Name = "page";
            this.page.SelectedIndex = 0;
            this.page.Size = new System.Drawing.Size(461, 226);
            this.page.TabIndex = 11;
            // 
            // path
            // 
            this.path.Controls.Add(this.label1);
            this.path.Controls.Add(this.openFileButton);
            this.path.Controls.Add(this.openFolderButton);
            this.path.Controls.Add(this.assemblyPath);
            this.path.Controls.Add(this.label4);
            this.path.Controls.Add(this.projectPath);
            this.path.Controls.Add(this.defaultNamespace);
            this.path.Controls.Add(this.label2);
            this.path.Controls.Add(this.label3);
            this.path.Controls.Add(this.projectName);
            this.path.Location = new System.Drawing.Point(4, 22);
            this.path.Name = "path";
            this.path.Padding = new System.Windows.Forms.Padding(3);
            this.path.Size = new System.Drawing.Size(453, 200);
            this.path.TabIndex = 0;
            this.path.Text = "路径";
            this.path.UseVisualStyleBackColor = true;
            // 
            // type
            // 
            this.type.Location = new System.Drawing.Point(4, 22);
            this.type.Name = "type";
            this.type.Padding = new System.Windows.Forms.Padding(3);
            this.type.Size = new System.Drawing.Size(453, 200);
            this.type.TabIndex = 1;
            this.type.Text = "安装类型";
            this.type.UseVisualStyleBackColor = true;
            // 
            // deploy
            // 
            this.deploy.Location = new System.Drawing.Point(4, 22);
            this.deploy.Name = "deploy";
            this.deploy.Size = new System.Drawing.Size(453, 200);
            this.deploy.TabIndex = 2;
            this.deploy.Text = "部署类型";
            this.deploy.UseVisualStyleBackColor = true;
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(4, 22);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(453, 200);
            this.test.TabIndex = 3;
            this.test.Text = "测试";
            this.test.UseVisualStyleBackColor = true;
            // 
            // setupButton
            // 
            this.setupButton.Location = new System.Drawing.Point(275, 246);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(75, 23);
            this.setupButton.TabIndex = 12;
            this.setupButton.Text = "安装";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setup_Click);
            // 
            // checkConfig
            // 
            this.checkConfig.AutoSize = true;
            this.checkConfig.Checked = true;
            this.checkConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkConfig.Location = new System.Drawing.Point(123, 250);
            this.checkConfig.Name = "checkConfig";
            this.checkConfig.Size = new System.Drawing.Size(72, 16);
            this.checkConfig.TabIndex = 13;
            this.checkConfig.Text = "配置绑定";
            this.checkConfig.UseVisualStyleBackColor = true;
            // 
            // setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 276);
            this.Controls.Add(this.checkConfig);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.page);
            this.Name = "setup";
            this.page.ResumeLayout(false);
            this.path.ResumeLayout(false);
            this.path.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.FolderBrowserDialog openFolder;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.TextBox assemblyPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox projectPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox projectName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox defaultNamespace;
        private System.Windows.Forms.TabControl page;
        private System.Windows.Forms.TabPage path;
        private System.Windows.Forms.TabPage type;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.TabPage deploy;
        private System.Windows.Forms.TabPage test;
        private System.Windows.Forms.CheckBox checkConfig;

    }
}

