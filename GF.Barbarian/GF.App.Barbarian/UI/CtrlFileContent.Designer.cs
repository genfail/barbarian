namespace GF.Barbarian
{
	partial class CtrlFileContent
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
			this.txtCntPatches = new System.Windows.Forms.TextBox();
			this.lstPatches = new System.Windows.Forms.ListView();
			this.btnLoadSelectedPatch = new System.Windows.Forms.Button();
			this.lblSelectedPatch = new System.Windows.Forms.Label();
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
			this.txtFname.Location = new System.Drawing.Point(60, 3);
			this.txtFname.Name = "txtFname";
			this.txtFname.ReadOnly = true;
			this.txtFname.Size = new System.Drawing.Size(578, 20);
			this.txtFname.TabIndex = 1;
			// 
			// txtCntPatches
			// 
			this.txtCntPatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCntPatches.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.txtCntPatches.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCntPatches.Location = new System.Drawing.Point(644, 3);
			this.txtCntPatches.Name = "txtCntPatches";
			this.txtCntPatches.ReadOnly = true;
			this.txtCntPatches.Size = new System.Drawing.Size(94, 20);
			this.txtCntPatches.TabIndex = 1;
			// 
			// lstPatches
			// 
			this.lstPatches.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstPatches.FullRowSelect = true;
			this.lstPatches.HideSelection = false;
			this.lstPatches.Location = new System.Drawing.Point(6, 29);
			this.lstPatches.MultiSelect = false;
			this.lstPatches.Name = "lstPatches";
			this.lstPatches.Size = new System.Drawing.Size(318, 299);
			this.lstPatches.TabIndex = 3;
			this.lstPatches.UseCompatibleStateImageBehavior = false;
			this.lstPatches.View = System.Windows.Forms.View.Details;
			this.lstPatches.SelectedIndexChanged += new System.EventHandler(this.lstPatches_SelectedIndexChanged);
			// 
			// btnLoadSelectedPatch
			// 
			this.btnLoadSelectedPatch.Location = new System.Drawing.Point(330, 56);
			this.btnLoadSelectedPatch.Name = "btnLoadSelectedPatch";
			this.btnLoadSelectedPatch.Size = new System.Drawing.Size(136, 60);
			this.btnLoadSelectedPatch.TabIndex = 4;
			this.btnLoadSelectedPatch.Text = "Load patch";
			this.btnLoadSelectedPatch.UseVisualStyleBackColor = true;
			this.btnLoadSelectedPatch.Click += new System.EventHandler(this.btnLoadSelectedPatch_Click);
			// 
			// lblSelectedPatch
			// 
			this.lblSelectedPatch.AutoSize = true;
			this.lblSelectedPatch.Location = new System.Drawing.Point(330, 40);
			this.lblSelectedPatch.Name = "lblSelectedPatch";
			this.lblSelectedPatch.Size = new System.Drawing.Size(61, 13);
			this.lblSelectedPatch.TabIndex = 5;
			this.lblSelectedPatch.Text = "Load patch";
			// 
			// CtrlFileContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblSelectedPatch);
			this.Controls.Add(this.btnLoadSelectedPatch);
			this.Controls.Add(this.lstPatches);
			this.Controls.Add(this.txtCntPatches);
			this.Controls.Add(this.txtFname);
			this.Controls.Add(this.lblFname);
			this.Name = "CtrlFileContent";
			this.Size = new System.Drawing.Size(742, 331);
			this.Load += new System.EventHandler(this.CtrlPatchFile_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFname;
		private System.Windows.Forms.TextBox txtFname;
		private System.Windows.Forms.TextBox txtCntPatches;
		private System.Windows.Forms.ListView lstPatches;
		private System.Windows.Forms.Button btnLoadSelectedPatch;
		private System.Windows.Forms.Label lblSelectedPatch;
	}
}
