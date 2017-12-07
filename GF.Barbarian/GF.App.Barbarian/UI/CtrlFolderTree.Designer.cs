namespace GF.Barbarian.UI
{
	partial class CtrlFolderTree
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlFolderTree));
			this.tvFolders = new System.Windows.Forms.TreeView();
			this.ImageListTreeView = new System.Windows.Forms.ImageList(this.components);
			this.lvFiles = new System.Windows.Forms.ListView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtSelectedFolder = new System.Windows.Forms.TextBox();
			this.cmbFileFilter = new System.Windows.Forms.ComboBox();
			this.lblFileListInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvFolders
			// 
			this.tvFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFolders.ImageIndex = 0;
			this.tvFolders.ImageList = this.ImageListTreeView;
			this.tvFolders.Location = new System.Drawing.Point(0, 0);
			this.tvFolders.Name = "tvFolders";
			this.tvFolders.SelectedImageIndex = 0;
			this.tvFolders.Size = new System.Drawing.Size(239, 296);
			this.tvFolders.TabIndex = 0;
			this.tvFolders.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFolders_BeforeExpand);
			this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
			// 
			// ImageListTreeView
			// 
			this.ImageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageListTreeView.ImageStream")));
			this.ImageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
			this.ImageListTreeView.Images.SetKeyName(0, "");
			this.ImageListTreeView.Images.SetKeyName(1, "");
			this.ImageListTreeView.Images.SetKeyName(2, "");
			this.ImageListTreeView.Images.SetKeyName(3, "");
			this.ImageListTreeView.Images.SetKeyName(4, "");
			this.ImageListTreeView.Images.SetKeyName(5, "");
			this.ImageListTreeView.Images.SetKeyName(6, "");
			this.ImageListTreeView.Images.SetKeyName(7, "");
			this.ImageListTreeView.Images.SetKeyName(8, "");
			// 
			// lvFiles
			// 
			this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvFiles.Location = new System.Drawing.Point(0, 26);
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(312, 251);
			this.lvFiles.TabIndex = 5;
			this.lvFiles.UseCompatibleStateImageBehavior = false;
			this.lvFiles.View = System.Windows.Forms.View.Details;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 29);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tvFolders);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lblFileListInfo);
			this.splitContainer1.Panel2.Controls.Add(this.cmbFileFilter);
			this.splitContainer1.Panel2.Controls.Add(this.lvFiles);
			this.splitContainer1.Size = new System.Drawing.Size(555, 296);
			this.splitContainer1.SplitterDistance = 239;
			this.splitContainer1.TabIndex = 6;
			// 
			// txtSelectedFolder
			// 
			this.txtSelectedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSelectedFolder.Location = new System.Drawing.Point(3, 3);
			this.txtSelectedFolder.Name = "txtSelectedFolder";
			this.txtSelectedFolder.Size = new System.Drawing.Size(555, 20);
			this.txtSelectedFolder.TabIndex = 7;
			this.txtSelectedFolder.Leave += new System.EventHandler(this.txtSelectedFolder_Leave);
			// 
			// cmbFileFilter
			// 
			this.cmbFileFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFileFilter.FormattingEnabled = true;
			this.cmbFileFilter.Items.AddRange(new object[] {
            "*.*",
            "*.g5l"});
			this.cmbFileFilter.Location = new System.Drawing.Point(3, 2);
			this.cmbFileFilter.Name = "cmbFileFilter";
			this.cmbFileFilter.Size = new System.Drawing.Size(121, 21);
			this.cmbFileFilter.TabIndex = 8;
			this.cmbFileFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFileFilter_SelectedIndexChanged);
			// 
			// lblFileListInfo
			// 
			this.lblFileListInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFileListInfo.AutoSize = true;
			this.lblFileListInfo.Location = new System.Drawing.Point(2, 280);
			this.lblFileListInfo.Name = "lblFileListInfo";
			this.lblFileListInfo.Size = new System.Drawing.Size(55, 13);
			this.lblFileListInfo.TabIndex = 9;
			this.lblFileListInfo.Text = "file list info";
			// 
			// CtrlFolderTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtSelectedFolder);
			this.Controls.Add(this.splitContainer1);
			this.Name = "CtrlFolderTree";
			this.Size = new System.Drawing.Size(561, 328);
			this.Load += new System.EventHandler(this.CtrlFolderTree_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView tvFolders;
		private System.Windows.Forms.ListView lvFiles;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ImageList ImageListTreeView;
		private System.Windows.Forms.TextBox txtSelectedFolder;
		private System.Windows.Forms.ComboBox cmbFileFilter;
		private System.Windows.Forms.Label lblFileListInfo;
	}
}
