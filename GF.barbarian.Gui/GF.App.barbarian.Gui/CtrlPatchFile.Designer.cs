namespace GF.barbarian.Gui
{
	partial class CtrlPatchFile
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
			this.lblFname = new System.Windows.Forms.Label();
			this.txtFname = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblFname
			// 
			this.lblFname.AutoSize = true;
			this.lblFname.Location = new System.Drawing.Point(2, 3);
			this.lblFname.Name = "lblFname";
			this.lblFname.Size = new System.Drawing.Size(54, 13);
			this.lblFname.TabIndex = 0;
			this.lblFname.Text = "File Name";
			// 
			// txtFname
			// 
			this.txtFname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFname.Location = new System.Drawing.Point(60, 2);
			this.txtFname.Name = "txtFname";
			this.txtFname.ReadOnly = true;
			this.txtFname.Size = new System.Drawing.Size(328, 20);
			this.txtFname.TabIndex = 1;
			// 
			// CtrlPatchFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtFname);
			this.Controls.Add(this.lblFname);
			this.Name = "CtrlPatchFile";
			this.Size = new System.Drawing.Size(391, 81);
			this.Load += new System.EventHandler(this.CtrlPatchFile_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFname;
		private System.Windows.Forms.TextBox txtFname;
	}
}
