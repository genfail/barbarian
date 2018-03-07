﻿namespace GF.Barbarian
{
	partial class FrmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.libraryModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.eXitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStripBarbarian = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusMidi = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelMain = new System.Windows.Forms.Panel();
			this.toolStripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.fileModeToolStripMenuItem,
            this.libraryModeToolStripMenuItem,
            this.eXitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
			// 
			// fileModeToolStripMenuItem
			// 
			this.fileModeToolStripMenuItem.Name = "fileModeToolStripMenuItem";
			this.fileModeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.fileModeToolStripMenuItem.Text = "&File Mode";
			this.fileModeToolStripMenuItem.Click += new System.EventHandler(this.fileModeToolStripMenuItem_Click);
			// 
			// libraryModeToolStripMenuItem
			// 
			this.libraryModeToolStripMenuItem.Name = "libraryModeToolStripMenuItem";
			this.libraryModeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.libraryModeToolStripMenuItem.Text = "&Library Mode";
			this.libraryModeToolStripMenuItem.Click += new System.EventHandler(this.libraryModeToolStripMenuItem_Click);
			// 
			// eXitToolStripMenuItem
			// 
			this.eXitToolStripMenuItem.Name = "eXitToolStripMenuItem";
			this.eXitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.eXitToolStripMenuItem.Text = "e&Xit";
			this.eXitToolStripMenuItem.Click += new System.EventHandler(this.eXitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.connectionToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			this.toolsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.toolsToolStripMenuItem_DropDownOpening);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.settingsToolStripMenuItem.Text = "&Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// connectionToolStripMenuItem
			// 
			this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
			this.connectionToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.connectionToolStripMenuItem.Text = "Device connection";
			this.connectionToolStripMenuItem.Click += new System.EventHandler(this.connectionToolStripMenuItem_Click);
			// 
			// statusStripBarbarian
			// 
			this.statusStripBarbarian.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusMidi,
            this.toolStripStatusMessage});
			this.statusStripBarbarian.Location = new System.Drawing.Point(0, 666);
			this.statusStripBarbarian.Name = "statusStripBarbarian";
			this.statusStripBarbarian.Size = new System.Drawing.Size(1107, 22);
			this.statusStripBarbarian.TabIndex = 2;
			this.statusStripBarbarian.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusMode";
			// 
			// toolStripStatusMidi
			// 
			this.toolStripStatusMidi.Image = global::GF.Barbarian.Properties.Resources.ConnectionUnavailable;
			this.toolStripStatusMidi.Name = "toolStripStatusMidi";
			this.toolStripStatusMidi.Size = new System.Drawing.Size(47, 17);
			this.toolStripStatusMidi.Text = "Midi";
			// 
			// panelMain
			// 
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 24);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(1107, 642);
			this.panelMain.TabIndex = 3;
			// 
			// toolStripStatusMessage
			// 
			this.toolStripStatusMessage.Name = "toolStripStatusMessage";
			this.toolStripStatusMessage.Size = new System.Drawing.Size(28, 17);
			this.toolStripStatusMessage.Text = "Info";
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1107, 688);
			this.Controls.Add(this.panelMain);
			this.Controls.Add(this.statusStripBarbarian);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FrmMain";
			this.Text = "Barbarian";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
			this.Load += new System.EventHandler(this.FrmMain_Load);
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
		private System.Windows.Forms.ToolStripMenuItem libraryModeToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStripBarbarian;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMidi;
		private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessage;
	}
}

