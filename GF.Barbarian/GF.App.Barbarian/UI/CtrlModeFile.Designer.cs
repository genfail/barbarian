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
			this.flowLayoutPatches = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblSelectedFolder = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// flowLayoutPatches
			// 
			this.flowLayoutPatches.AllowDrop = true;
			this.flowLayoutPatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPatches.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.flowLayoutPatches.Location = new System.Drawing.Point(14, 88);
			this.flowLayoutPatches.Name = "flowLayoutPatches";
			this.flowLayoutPatches.Size = new System.Drawing.Size(275, 188);
			this.flowLayoutPatches.TabIndex = 0;
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Folder:";
			// 
			// lblSelectedFolder
			// 
			this.lblSelectedFolder.AutoSize = true;
			this.lblSelectedFolder.Location = new System.Drawing.Point(48, 16);
			this.lblSelectedFolder.Name = "lblSelectedFolder";
			this.lblSelectedFolder.Size = new System.Drawing.Size(19, 13);
			this.lblSelectedFolder.TabIndex = 1;
			this.lblSelectedFolder.Text = "....";
			// 
			// CtrlModeFile
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblSelectedFolder);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.flowLayoutPatches);
			this.Name = "CtrlModeFile";
			this.Size = new System.Drawing.Size(301, 293);
			this.Load += new System.EventHandler(this.CtrlModeFile_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPatches;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblSelectedFolder;
	}
}
