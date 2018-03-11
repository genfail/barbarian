namespace GF.Barbarian
{
	partial class CtrlModeFile
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.splitContainerExplorer = new System.Windows.Forms.SplitContainer();
			this.shellTreeView1 = new GongSolutions.Shell.ShellTreeView();
			this.shellViewFileList = new GongSolutions.Shell.ShellView();
			this.cmbFavoriteFolders = new System.Windows.Forms.ComboBox();
			this.btnAddToFavoriteFolders = new System.Windows.Forms.Button();
			this.btnRootFolder = new System.Windows.Forms.Button();
			this.btnOneFolderUp = new System.Windows.Forms.Button();
			this.fileFilterComboBox1 = new GongSolutions.Shell.FileFilterComboBox();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOpenFileExplorer = new System.Windows.Forms.Button();
			this.toolTipControl = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.contextMenuStripForComboBox = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.removePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.activePatch = new GF.Barbarian.CtrlFileContent();
			this.goToPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerExplorer)).BeginInit();
			this.splitContainerExplorer.Panel1.SuspendLayout();
			this.splitContainerExplorer.Panel2.SuspendLayout();
			this.splitContainerExplorer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.contextMenuStripForComboBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerExplorer
			// 
			this.splitContainerExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerExplorer.Location = new System.Drawing.Point(0, 0);
			this.splitContainerExplorer.Name = "splitContainerExplorer";
			// 
			// splitContainerExplorer.Panel1
			// 
			this.splitContainerExplorer.Panel1.Controls.Add(this.shellTreeView1);
			// 
			// splitContainerExplorer.Panel2
			// 
			this.splitContainerExplorer.Panel2.Controls.Add(this.cmbFavoriteFolders);
			this.splitContainerExplorer.Panel2.Controls.Add(this.btnAddToFavoriteFolders);
			this.splitContainerExplorer.Panel2.Controls.Add(this.btnRootFolder);
			this.splitContainerExplorer.Panel2.Controls.Add(this.btnOneFolderUp);
			this.splitContainerExplorer.Panel2.Controls.Add(this.fileFilterComboBox1);
			this.splitContainerExplorer.Panel2.Controls.Add(this.shellViewFileList);
			this.splitContainerExplorer.Size = new System.Drawing.Size(804, 265);
			this.splitContainerExplorer.SplitterDistance = 282;
			this.splitContainerExplorer.TabIndex = 3;
			// 
			// shellTreeView1
			// 
			this.shellTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.shellTreeView1.Location = new System.Drawing.Point(0, 0);
			this.shellTreeView1.Name = "shellTreeView1";
			this.shellTreeView1.ShellView = this.shellViewFileList;
			this.shellTreeView1.Size = new System.Drawing.Size(282, 265);
			this.shellTreeView1.TabIndex = 0;
			this.shellTreeView1.SelectionChanged += new System.EventHandler(this.shellTreeView1_SelectionChanged);
			// 
			// shellViewFileList
			// 
			this.shellViewFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shellViewFileList.Location = new System.Drawing.Point(3, 25);
			this.shellViewFileList.MultiSelect = false;
			this.shellViewFileList.Name = "shellViewFileList";
			this.shellViewFileList.SelectedItems = new GongSolutions.Shell.ShellItem[0];
			this.shellViewFileList.Size = new System.Drawing.Size(512, 237);
			this.shellViewFileList.StatusBar = null;
			this.shellViewFileList.TabIndex = 0;
			this.shellViewFileList.Text = "shellView1";
			this.shellViewFileList.View = GongSolutions.Shell.ShellViewStyle.Details;
			this.shellViewFileList.Navigated += new System.EventHandler(this.shellViewFileList_Navigated);
			this.shellViewFileList.LocationChanged += new System.EventHandler(this.shellViewFileList_LocationChanged);
			// 
			// cmbFavoriteFolders
			// 
			this.cmbFavoriteFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFavoriteFolders.ContextMenuStrip = this.contextMenuStripForComboBox;
			this.cmbFavoriteFolders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFavoriteFolders.FormattingEnabled = true;
			this.cmbFavoriteFolders.Location = new System.Drawing.Point(109, 1);
			this.cmbFavoriteFolders.Name = "cmbFavoriteFolders";
			this.cmbFavoriteFolders.Size = new System.Drawing.Size(181, 21);
			this.cmbFavoriteFolders.TabIndex = 3;
			this.cmbFavoriteFolders.SelectedIndexChanged += new System.EventHandler(this.cmbFavoriteFolders_SelectedIndexChanged);
			// 
			// btnAddToFavoriteFolders
			// 
			this.btnAddToFavoriteFolders.ContextMenuStrip = this.contextMenuStripForComboBox;
			this.btnAddToFavoriteFolders.Location = new System.Drawing.Point(73, 1);
			this.btnAddToFavoriteFolders.Margin = new System.Windows.Forms.Padding(0);
			this.btnAddToFavoriteFolders.Name = "btnAddToFavoriteFolders";
			this.btnAddToFavoriteFolders.Size = new System.Drawing.Size(33, 21);
			this.btnAddToFavoriteFolders.TabIndex = 2;
			this.btnAddToFavoriteFolders.Text = "*";
			this.toolTipControl.SetToolTip(this.btnAddToFavoriteFolders, "Set current folder in favorite list");
			this.btnAddToFavoriteFolders.UseVisualStyleBackColor = true;
			this.btnAddToFavoriteFolders.Click += new System.EventHandler(this.btnbtnAddToFavoriteFolders_Click);
			// 
			// btnRootFolder
			// 
			this.btnRootFolder.Location = new System.Drawing.Point(40, 1);
			this.btnRootFolder.Margin = new System.Windows.Forms.Padding(0);
			this.btnRootFolder.Name = "btnRootFolder";
			this.btnRootFolder.Size = new System.Drawing.Size(33, 21);
			this.btnRootFolder.TabIndex = 2;
			this.btnRootFolder.Text = "\\";
			this.toolTipControl.SetToolTip(this.btnRootFolder, "Go to the root of this drive");
			this.btnRootFolder.UseVisualStyleBackColor = true;
			this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
			// 
			// btnOneFolderUp
			// 
			this.btnOneFolderUp.Location = new System.Drawing.Point(3, 1);
			this.btnOneFolderUp.Margin = new System.Windows.Forms.Padding(0);
			this.btnOneFolderUp.Name = "btnOneFolderUp";
			this.btnOneFolderUp.Size = new System.Drawing.Size(33, 21);
			this.btnOneFolderUp.TabIndex = 2;
			this.btnOneFolderUp.Text = "..";
			this.toolTipControl.SetToolTip(this.btnOneFolderUp, "Go one folder up");
			this.btnOneFolderUp.UseVisualStyleBackColor = true;
			this.btnOneFolderUp.Click += new System.EventHandler(this.btnOneFolderUp_Click);
			// 
			// fileFilterComboBox1
			// 
			this.fileFilterComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.fileFilterComboBox1.Filter = "*.g5l";
			this.fileFilterComboBox1.FilterItems = "All Files (*.*)|*.*|Patch Files (*.g5l)|*.g5l";
			this.fileFilterComboBox1.FormattingEnabled = true;
			this.fileFilterComboBox1.Location = new System.Drawing.Point(292, 2);
			this.fileFilterComboBox1.Name = "fileFilterComboBox1";
			this.fileFilterComboBox1.ShellView = this.shellViewFileList;
			this.fileFilterComboBox1.Size = new System.Drawing.Size(226, 21);
			this.fileFilterComboBox1.TabIndex = 1;
			// 
			// txtFolder
			// 
			this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolder.Location = new System.Drawing.Point(90, 2);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(674, 20);
			this.txtFolder.TabIndex = 4;
			this.txtFolder.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFolder_KeyUp);
			this.txtFolder.Leave += new System.EventHandler(this.txtFolder_Leave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Selected folder:";
			// 
			// btnOpenFileExplorer
			// 
			this.btnOpenFileExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenFileExplorer.BackgroundImage = global::GF.Barbarian.Properties.Resources.FileExplorer;
			this.btnOpenFileExplorer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnOpenFileExplorer.Location = new System.Drawing.Point(766, 0);
			this.btnOpenFileExplorer.Name = "btnOpenFileExplorer";
			this.btnOpenFileExplorer.Size = new System.Drawing.Size(41, 24);
			this.btnOpenFileExplorer.TabIndex = 7;
			this.toolTipControl.SetToolTip(this.btnOpenFileExplorer, "Open file explorer in this folder");
			this.btnOpenFileExplorer.UseVisualStyleBackColor = true;
			this.btnOpenFileExplorer.Click += new System.EventHandler(this.btnOpenFileExplorer_Click);
			// 
			// splitContainerMain
			// 
			this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerMain.Location = new System.Drawing.Point(3, 25);
			this.splitContainerMain.Name = "splitContainerMain";
			this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.splitContainerExplorer);
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.activePatch);
			this.splitContainerMain.Size = new System.Drawing.Size(804, 520);
			this.splitContainerMain.SplitterDistance = 265;
			this.splitContainerMain.TabIndex = 8;
			// 
			// contextMenuStripForComboBox
			// 
			this.contextMenuStripForComboBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToPathToolStripMenuItem,
            this.removePathToolStripMenuItem});
			this.contextMenuStripForComboBox.Name = "contextMenuStrip1";
			this.contextMenuStripForComboBox.Size = new System.Drawing.Size(181, 70);
			// 
			// removePathToolStripMenuItem
			// 
			this.removePathToolStripMenuItem.Name = "removePathToolStripMenuItem";
			this.removePathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.removePathToolStripMenuItem.Text = "Remove path";
			this.removePathToolStripMenuItem.Click += new System.EventHandler(this.removePathToolStripMenuItem_Click);
			// 
			// activePatch
			// 
			this.activePatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activePatch.FileName = null;
			this.activePatch.Location = new System.Drawing.Point(0, 0);
			this.activePatch.Name = "activePatch";
			this.activePatch.SelectedPatchIndex = -1;
			this.activePatch.Size = new System.Drawing.Size(804, 251);
			this.activePatch.TabIndex = 6;
			// 
			// goToPathToolStripMenuItem
			// 
			this.goToPathToolStripMenuItem.Name = "goToPathToolStripMenuItem";
			this.goToPathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.goToPathToolStripMenuItem.Text = "Go to path";
			this.goToPathToolStripMenuItem.Click += new System.EventHandler(this.goToPathToolStripMenuItem_Click);
			// 
			// CtrlModeFile
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainerMain);
			this.Controls.Add(this.btnOpenFileExplorer);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFolder);
			this.Name = "CtrlModeFile";
			this.Size = new System.Drawing.Size(810, 547);
			this.Load += new System.EventHandler(this.CtrlModeFile_Load);
			this.splitContainerExplorer.Panel1.ResumeLayout(false);
			this.splitContainerExplorer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerExplorer)).EndInit();
			this.splitContainerExplorer.ResumeLayout(false);
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
			this.splitContainerMain.ResumeLayout(false);
			this.contextMenuStripForComboBox.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.SplitContainer splitContainerExplorer;
		private GongSolutions.Shell.ShellTreeView shellTreeView1;
		private GongSolutions.Shell.ShellView shellViewFileList;
		private GongSolutions.Shell.FileFilterComboBox fileFilterComboBox1;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
		private CtrlFileContent activePatch;
		private System.Windows.Forms.Button btnOpenFileExplorer;
		private System.Windows.Forms.ToolTip toolTipControl;
		private System.Windows.Forms.SplitContainer splitContainerMain;
		private System.Windows.Forms.Button btnRootFolder;
		private System.Windows.Forms.Button btnOneFolderUp;
		private System.Windows.Forms.ComboBox cmbFavoriteFolders;
		private System.Windows.Forms.Button btnAddToFavoriteFolders;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripForComboBox;
		private System.Windows.Forms.ToolStripMenuItem removePathToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem goToPathToolStripMenuItem;
	}
}
