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
			this.label1 = new System.Windows.Forms.Label();
			this.ctrlFolderTree1 = new GF.Barbarian.UI.CtrlFolderTree();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "File Mode";
			// 
			// ctrlFolderTree1
			// 
			this.ctrlFolderTree1.Cursor = System.Windows.Forms.Cursors.Default;
			this.ctrlFolderTree1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlFolderTree1.Location = new System.Drawing.Point(0, 0);
			this.ctrlFolderTree1.Name = "ctrlFolderTree1";
			this.ctrlFolderTree1.Size = new System.Drawing.Size(411, 611);
			this.ctrlFolderTree1.TabIndex = 2;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 32);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ctrlFolderTree1);
			this.splitContainer1.Size = new System.Drawing.Size(872, 611);
			this.splitContainer1.SplitterDistance = 411;
			this.splitContainer1.TabIndex = 3;
			// 
			// CtrlModeFile
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label1);
			this.Name = "CtrlModeFile";
			this.Size = new System.Drawing.Size(878, 646);
			this.Load += new System.EventHandler(this.CtrlModeFile_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private UI.CtrlFolderTree ctrlFolderTree1;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}
