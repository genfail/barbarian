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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnLoadPrevPatch = new System.Windows.Forms.Button();
			this.btnLoadNextPatch = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtCurrPatchNumber = new System.Windows.Forms.TextBox();
			this.txtCurrPatchName = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
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
			this.txtFname.Size = new System.Drawing.Size(410, 20);
			this.txtFname.TabIndex = 1;
			// 
			// txtCntPatches
			// 
			this.txtCntPatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCntPatches.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.txtCntPatches.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCntPatches.Location = new System.Drawing.Point(476, 3);
			this.txtCntPatches.Name = "txtCntPatches";
			this.txtCntPatches.ReadOnly = true;
			this.txtCntPatches.Size = new System.Drawing.Size(94, 20);
			this.txtCntPatches.TabIndex = 1;
			// 
			// lstPatches
			// 
			this.lstPatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstPatches.FullRowSelect = true;
			this.lstPatches.HideSelection = false;
			this.lstPatches.Location = new System.Drawing.Point(6, 29);
			this.lstPatches.MultiSelect = false;
			this.lstPatches.Name = "lstPatches";
			this.lstPatches.Size = new System.Drawing.Size(383, 193);
			this.lstPatches.TabIndex = 3;
			this.lstPatches.TileSize = new System.Drawing.Size(160, 20);
			this.toolTip1.SetToolTip(this.lstPatches, "Doubleclick, enter or button Load patch to load the patch into the device");
			this.lstPatches.UseCompatibleStateImageBehavior = false;
			this.lstPatches.View = System.Windows.Forms.View.Tile;
			this.lstPatches.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lstPatches_DrawItem);
			this.lstPatches.SelectedIndexChanged += new System.EventHandler(this.lstPatches_SelectedIndexChanged);
			this.lstPatches.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstPatches_KeyUp);
			// 
			// btnLoadCurrPatch
			// 
			this.btnLoadCurrPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadCurrPatch.Location = new System.Drawing.Point(55, 75);
			this.btnLoadCurrPatch.Name = "btnLoadCurrPatch";
			this.btnLoadCurrPatch.Size = new System.Drawing.Size(68, 40);
			this.btnLoadCurrPatch.TabIndex = 4;
			this.btnLoadCurrPatch.Text = "&Selected";
			this.btnLoadCurrPatch.UseVisualStyleBackColor = true;
			this.btnLoadCurrPatch.Click += new System.EventHandler(this.btnLoadSelectedPatch_Click);
			// 
			// btnLoadPrevPatch
			// 
			this.btnLoadPrevPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLoadPrevPatch.Location = new System.Drawing.Point(5, 75);
			this.btnLoadPrevPatch.Name = "btnLoadPrevPatch";
			this.btnLoadPrevPatch.Size = new System.Drawing.Size(48, 40);
			this.btnLoadPrevPatch.TabIndex = 4;
			this.btnLoadPrevPatch.Text = "&Prev";
			this.btnLoadPrevPatch.UseVisualStyleBackColor = true;
			this.btnLoadPrevPatch.Click += new System.EventHandler(this.btnLoadPreviousPatch_Click);
			// 
			// btnLoadNextPatch
			// 
			this.btnLoadNextPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadNextPatch.Location = new System.Drawing.Point(125, 75);
			this.btnLoadNextPatch.Name = "btnLoadNextPatch";
			this.btnLoadNextPatch.Size = new System.Drawing.Size(48, 40);
			this.btnLoadNextPatch.TabIndex = 4;
			this.btnLoadNextPatch.Text = "&Next";
			this.btnLoadNextPatch.UseVisualStyleBackColor = true;
			this.btnLoadNextPatch.Click += new System.EventHandler(this.btnLoadNextPatch_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtCurrPatchNumber);
			this.groupBox1.Controls.Add(this.txtCurrPatchName);
			this.groupBox1.Controls.Add(this.btnLoadNextPatch);
			this.groupBox1.Controls.Add(this.btnLoadPrevPatch);
			this.groupBox1.Controls.Add(this.btnLoadCurrPatch);
			this.groupBox1.Location = new System.Drawing.Point(393, 29);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(177, 122);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Active patch";
			// 
			// txtCurrPatchNumber
			// 
			this.txtCurrPatchNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCurrPatchNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCurrPatchNumber.Font = new System.Drawing.Font("Arial Black", 14F, System.Drawing.FontStyle.Bold);
			this.txtCurrPatchNumber.Location = new System.Drawing.Point(5, 19);
			this.txtCurrPatchNumber.Name = "txtCurrPatchNumber";
			this.txtCurrPatchNumber.ReadOnly = true;
			this.txtCurrPatchNumber.Size = new System.Drawing.Size(168, 27);
			this.txtCurrPatchNumber.TabIndex = 5;
			this.txtCurrPatchNumber.Text = "1234";
			this.txtCurrPatchNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtCurrPatchName
			// 
			this.txtCurrPatchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCurrPatchName.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCurrPatchName.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold);
			this.txtCurrPatchName.Location = new System.Drawing.Point(5, 46);
			this.txtCurrPatchName.Name = "txtCurrPatchName";
			this.txtCurrPatchName.ReadOnly = true;
			this.txtCurrPatchName.Size = new System.Drawing.Size(168, 19);
			this.txtCurrPatchName.TabIndex = 5;
			this.txtCurrPatchName.Text = "Patch Name";
			this.txtCurrPatchName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// CtrlFileContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lstPatches);
			this.Controls.Add(this.txtCntPatches);
			this.Controls.Add(this.txtFname);
			this.Controls.Add(this.lblFname);
			this.Name = "CtrlFileContent";
			this.Size = new System.Drawing.Size(574, 225);
			this.Load += new System.EventHandler(this.CtrlPatchFile_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFname;
		private System.Windows.Forms.TextBox txtFname;
		private System.Windows.Forms.TextBox txtCntPatches;
		private System.Windows.Forms.ListView lstPatches;
		private System.Windows.Forms.Button btnLoadCurrPatch;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnLoadPrevPatch;
		private System.Windows.Forms.Button btnLoadNextPatch;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtCurrPatchName;
		private System.Windows.Forms.TextBox txtCurrPatchNumber;
	}
}
