namespace GF.Barbarian.UI
{
	partial class CtrlPatch
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
			this.txtPatchName = new System.Windows.Forms.TextBox();
			this.lblPatchNr = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtPatchName
			// 
			this.txtPatchName.Location = new System.Drawing.Point(120, 1);
			this.txtPatchName.Name = "txtPatchName";
			this.txtPatchName.ReadOnly = true;
			this.txtPatchName.Size = new System.Drawing.Size(132, 20);
			this.txtPatchName.TabIndex = 0;
			// 
			// lblPatchNr
			// 
			this.lblPatchNr.AutoSize = true;
			this.lblPatchNr.Location = new System.Drawing.Point(3, 5);
			this.lblPatchNr.Name = "lblPatchNr";
			this.lblPatchNr.Size = new System.Drawing.Size(13, 13);
			this.lblPatchNr.TabIndex = 1;
			this.lblPatchNr.Text = "..";
			// 
			// CtrlPatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblPatchNr);
			this.Controls.Add(this.txtPatchName);
			this.Name = "CtrlPatch";
			this.Size = new System.Drawing.Size(362, 24);
			this.Load += new System.EventHandler(this.CtrlPatch_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtPatchName;
		private System.Windows.Forms.Label lblPatchNr;
	}
}
