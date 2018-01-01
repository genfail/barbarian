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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.shellTreeView1 = new GongSolutions.Shell.ShellTreeView();
			this.shellViewFileList = new GongSolutions.Shell.ShellView();
			this.fileFilterComboBox1 = new GongSolutions.Shell.FileFilterComboBox();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 46);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.shellTreeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.fileFilterComboBox1);
			this.splitContainer1.Panel2.Controls.Add(this.shellViewFileList);
			this.splitContainer1.Size = new System.Drawing.Size(770, 323);
			this.splitContainer1.SplitterDistance = 271;
			this.splitContainer1.TabIndex = 3;
			// 
			// shellTreeView1
			// 
			this.shellTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.shellTreeView1.Location = new System.Drawing.Point(0, 0);
			this.shellTreeView1.Name = "shellTreeView1";
			this.shellTreeView1.ShellView = this.shellViewFileList;
			this.shellTreeView1.Size = new System.Drawing.Size(271, 323);
			this.shellTreeView1.TabIndex = 0;
			this.shellTreeView1.SelectionChanged += new System.EventHandler(this.shellTreeView1_SelectionChanged);
			// 
			// shellViewFileList
			// 
			this.shellViewFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shellViewFileList.Location = new System.Drawing.Point(3, 30);
			this.shellViewFileList.MultiSelect = false;
			this.shellViewFileList.Name = "shellViewFileList";
			this.shellViewFileList.Size = new System.Drawing.Size(489, 290);
			this.shellViewFileList.StatusBar = null;
			this.shellViewFileList.TabIndex = 0;
			this.shellViewFileList.Text = "shellView1";
			this.shellViewFileList.View = GongSolutions.Shell.ShellViewStyle.Details;
			this.shellViewFileList.Navigated += new System.EventHandler(this.shellViewFileList_Navigated);
			this.shellViewFileList.LocationChanged += new System.EventHandler(this.shellViewFileList_LocationChanged);
			// 
			// fileFilterComboBox1
			// 
			this.fileFilterComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileFilterComboBox1.Filter = "*.g5l";
			this.fileFilterComboBox1.FilterItems = "All Files (*.*)|*.*|Patch Files (*.g5l)|*.g5l";
			this.fileFilterComboBox1.FormattingEnabled = true;
			this.fileFilterComboBox1.Location = new System.Drawing.Point(2, 3);
			this.fileFilterComboBox1.Name = "fileFilterComboBox1";
			this.fileFilterComboBox1.ShellView = this.shellViewFileList;
			this.fileFilterComboBox1.Size = new System.Drawing.Size(490, 21);
			this.fileFilterComboBox1.TabIndex = 1;
			// 
			// txtFolder
			// 
			this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolder.Location = new System.Drawing.Point(3, 20);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(770, 20);
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
			// CtrlModeFile
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFolder);
			this.Controls.Add(this.splitContainer1);
			this.Name = "CtrlModeFile";
			this.Size = new System.Drawing.Size(878, 684);
			this.Load += new System.EventHandler(this.CtrlModeFile_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.SplitContainer splitContainer1;
		private GongSolutions.Shell.ShellTreeView shellTreeView1;
		private GongSolutions.Shell.ShellView shellViewFileList;
		private GongSolutions.Shell.FileFilterComboBox fileFilterComboBox1;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
	}
}
