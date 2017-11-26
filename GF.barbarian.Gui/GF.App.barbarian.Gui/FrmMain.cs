﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.barbarian.Gui
{
	public partial class FrmMain : Form
	{
		private ProgramMode mode = ProgramMode.File;
		private Dictionary<ProgramMode,ICtrlMode> modes = null;
		private ICtrlMode activeControl { get{ return modes[mode];} }

		public FrmMain()
		{
			InitializeComponent();
			modes = new Dictionary<ProgramMode, ICtrlMode>();
			modes.Add(ProgramMode.File, new CtrlModeFile());
			modes.Add(ProgramMode.Library, new CtrlModeLibrary());
			SetMode(Program.AppSettings.Mode);
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{}

		public void ApplySettings()
		{
			activeControl.ApplySettings();
		}

		public void SetMode(ProgramMode _mode)
		{
			mode = _mode;

			// remove previous
			List<ICtrlMode> lst = panelMain.FindAllChildrenByType<ICtrlMode>().ToList();
			foreach (Control c in lst)
			{
				panelMain.Controls.Remove(c);
			}

			// add the new one
			((Control)activeControl).Location = panelMain.Location;// new System.Drawing.Point(177, 51);
			((Control)activeControl).Size = panelMain.Size; // new System.Drawing.Size(624, 582);
			((Control)activeControl).Name = "ctrlMode" + activeControl.GetType().ToString();
			((Control)activeControl).TabIndex = 1;
			((Control)activeControl).BackColor = Color.DarkGray;
			((Control)activeControl).Dock = DockStyle.Fill;
			panelMain.Controls.Add((Control)activeControl);
		}

	#region Menu
		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileModeToolStripMenuItem.Checked = (activeControl is CtrlModeFile);
			libraryModeToolStripMenuItem.Checked = (activeControl is CtrlModeLibrary);
		}
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FrmSettings frm = new FrmSettings();
			frm.StartPosition = FormStartPosition.CenterParent;
			frm.ShowDialog(this);
		}
		private void fileModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetMode(ProgramMode.File);
		}

		private void libraryModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetMode(ProgramMode.Library);
		}
		private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion

		private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Properties.Settings.Default.LastProgramMode = (int)mode;
			foreach (ICtrlMode c in modes.Values)
			{
				c.SaveSettings();
			}
			Properties.Settings.Default.Save();
		}
	}
}
