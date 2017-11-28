namespace GF.Barbarian.UI
{
	partial class CtrlFolderTree2
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
			this.dirsTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// dirsTreeView
			// 
			this.dirsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dirsTreeView.Location = new System.Drawing.Point(3, 3);
			this.dirsTreeView.Name = "dirsTreeView";
			this.dirsTreeView.Size = new System.Drawing.Size(386, 378);
			this.dirsTreeView.TabIndex = 0;
			this.dirsTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.dirsTreeView_BeforeExpand);
			// 
			// CtrlFolderTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dirsTreeView);
			this.Name = "CtrlFolderTree";
			this.Size = new System.Drawing.Size(392, 384);
			this.Load += new System.EventHandler(this.CtrlFolderTree_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView dirsTreeView;
	}
}
