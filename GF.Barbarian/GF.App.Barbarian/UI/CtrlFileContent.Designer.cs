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
			this.components = new System.ComponentModel.Container();
			this.lblFname = new System.Windows.Forms.Label();
			this.txtFname = new System.Windows.Forms.TextBox();
			this.txtCntPatches = new System.Windows.Forms.TextBox();
			this.lstPatches = new System.Windows.Forms.ListView();
			this.btnLoadCurrPatch = new System.Windows.Forms.Button();
			this.lblCurrPatch = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnLoadPrevPatch = new System.Windows.Forms.Button();
			this.lblPrevPatch = new System.Windows.Forms.Label();
			this.btnLoadNextPatch = new System.Windows.Forms.Button();
			this.lblNextPatch = new System.Windows.Forms.Label();
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
			this.lstPatches.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstPatches.TabIndex = 3;
			this.toolTip1.SetToolTip(this.lstPatches, "Doubleclick, enter or button Load patch to load the patch into the device");
			this.lstPatches.UseCompatibleStateImageBehavior = false;
			this.lstPatches.View = System.Windows.Forms.View.Details;
			this.lstPatches.SelectedIndexChanged += new System.EventHandler(this.lstPatches_SelectedIndexChanged);
			this.lstPatches.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstPatches_KeyUp);
			// 
			// btnLoadCurrPatch
			// 
			this.btnLoadCurrPatch.Location = new System.Drawing.Point(423, 56);
			this.btnLoadCurrPatch.Name = "btnLoadCurrPatch";
			this.btnLoadCurrPatch.Size = new System.Drawing.Size(136, 26);
			this.btnLoadCurrPatch.TabIndex = 4;
			this.btnLoadCurrPatch.Text = "Load &Selected";
			this.btnLoadCurrPatch.UseVisualStyleBackColor = true;
			this.btnLoadCurrPatch.Click += new System.EventHandler(this.btnLoadSelectedPatch_Click);
			// 
			// lblCurrPatch
			// 
			this.lblCurrPatch.AutoSize = true;
			this.lblCurrPatch.Location = new System.Drawing.Point(456, 40);
			this.lblCurrPatch.Name = "lblCurrPatch";
			this.lblCurrPatch.Size = new System.Drawing.Size(61, 13);
			this.lblCurrPatch.TabIndex = 5;
			this.lblCurrPatch.Text = "Load patch";
			// 
			// btnLoadPrevPatch
			// 
			this.btnLoadPrevPatch.Location = new System.Drawing.Point(342, 56);
			this.btnLoadPrevPatch.Name = "btnLoadPrevPatch";
			this.btnLoadPrevPatch.Size = new System.Drawing.Size(67, 26);
			this.btnLoadPrevPatch.TabIndex = 4;
			this.btnLoadPrevPatch.Text = "Load &Prev";
			this.btnLoadPrevPatch.UseVisualStyleBackColor = true;
			this.btnLoadPrevPatch.Click += new System.EventHandler(this.btnLoadPreviousPatch_Click);
			// 
			// lblPrevPatch
			// 
			this.lblPrevPatch.AutoSize = true;
			this.lblPrevPatch.Location = new System.Drawing.Point(345, 40);
			this.lblPrevPatch.Name = "lblPrevPatch";
			this.lblPrevPatch.Size = new System.Drawing.Size(61, 13);
			this.lblPrevPatch.TabIndex = 5;
			this.lblPrevPatch.Text = "Load patch";
			// 
			// btnLoadNextPatch
			// 
			this.btnLoadNextPatch.Location = new System.Drawing.Point(581, 56);
			this.btnLoadNextPatch.Name = "btnLoadNextPatch";
			this.btnLoadNextPatch.Size = new System.Drawing.Size(67, 26);
			this.btnLoadNextPatch.TabIndex = 4;
			this.btnLoadNextPatch.Text = "Load &Next";
			this.btnLoadNextPatch.UseVisualStyleBackColor = true;
			this.btnLoadNextPatch.Click += new System.EventHandler(this.btnLoadNextPatch_Click);
			// 
			// lblNextPatch
			// 
			this.lblNextPatch.AutoSize = true;
			this.lblNextPatch.Location = new System.Drawing.Point(584, 40);
			this.lblNextPatch.Name = "lblNextPatch";
			this.lblNextPatch.Size = new System.Drawing.Size(61, 13);
			this.lblNextPatch.TabIndex = 5;
			this.lblNextPatch.Text = "Load patch";
			// 
			// CtrlFileContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblNextPatch);
			this.Controls.Add(this.lblPrevPatch);
			this.Controls.Add(this.lblCurrPatch);
			this.Controls.Add(this.btnLoadNextPatch);
			this.Controls.Add(this.btnLoadPrevPatch);
			this.Controls.Add(this.btnLoadCurrPatch);
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
		private System.Windows.Forms.Button btnLoadCurrPatch;
		private System.Windows.Forms.Label lblCurrPatch;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnLoadPrevPatch;
		private System.Windows.Forms.Label lblPrevPatch;
		private System.Windows.Forms.Button btnLoadNextPatch;
		private System.Windows.Forms.Label lblNextPatch;
	}
}
