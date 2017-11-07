namespace GF.barbarian.Gui
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.eXitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.libraryMaodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStripBarbarian = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1.SuspendLayout();
			this.statusStripBarbarian.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1107, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eXitToolStripMenuItem,
            this.fileModeToolStripMenuItem,
            this.libraryMaodeToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// eXitToolStripMenuItem
			// 
			this.eXitToolStripMenuItem.Name = "eXitToolStripMenuItem";
			this.eXitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.eXitToolStripMenuItem.Text = "e&Xit";
			this.eXitToolStripMenuItem.Click += new System.EventHandler(this.eXitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.settingsToolStripMenuItem.Text = "&Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// fileModeToolStripMenuItem
			// 
			this.fileModeToolStripMenuItem.Name = "fileModeToolStripMenuItem";
			this.fileModeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.fileModeToolStripMenuItem.Text = "&File Mode";
			this.fileModeToolStripMenuItem.Click += new System.EventHandler(this.fileModeToolStripMenuItem_Click);
			// 
			// libraryMaodeToolStripMenuItem
			// 
			this.libraryMaodeToolStripMenuItem.Name = "libraryMaodeToolStripMenuItem";
			this.libraryMaodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.libraryMaodeToolStripMenuItem.Text = "&Library Maode";
			this.libraryMaodeToolStripMenuItem.Click += new System.EventHandler(this.libraryMaodeToolStripMenuItem_Click);
			// 
			// statusStripBarbarian
			// 
			this.statusStripBarbarian.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStripBarbarian.Location = new System.Drawing.Point(0, 666);
			this.statusStripBarbarian.Name = "statusStripBarbarian";
			this.statusStripBarbarian.Size = new System.Drawing.Size(1107, 22);
			this.statusStripBarbarian.TabIndex = 2;
			this.statusStripBarbarian.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(115, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusMode";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1107, 688);
			this.Controls.Add(this.statusStripBarbarian);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Barbarian";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStripBarbarian.ResumeLayout(false);
			this.statusStripBarbarian.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem eXitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileModeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem libraryMaodeToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStripBarbarian;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
	}
}

