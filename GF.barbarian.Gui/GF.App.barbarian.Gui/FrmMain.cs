using System;
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
	public partial class Form1 : Form
	{
		ProgramMode mode = ProgramMode.NotSet;

		public Form1()
		{
			InitializeComponent();
			SetMode(Program.Arguments.Mode);
		}

		public void StartWithDataFile(string _dataFileToOpen)
		{
			if (!String.IsNullOrEmpty(Program.Arguments.LibraryPathFileName))
			{
				SetMode(ProgramMode.File);
				((CtrlModeFile)activeControl).AddPatchFile(Program.Arguments.LibraryPathFileName);
			}
		}

		private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
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

		public void SetMode(ProgramMode _mode)
		{
			mode = _mode;
			switch (mode)
			{
				case ProgramMode.NotSet:
					activeControl = null;
					Trace.WriteLine("Not used!!");
					break;
				case ProgramMode.File:
					ReplaceCtrl(new CtrlModeFile());
					break;
				case ProgramMode.Library:
					ReplaceCtrl(new CtrlModeLibrary());
					break;
				default:
					break;
			}
		}

		private Control activeControl = null;
		private void ReplaceCtrl(Control _c)
		{
			// remove previous
			if (panelMain.Controls.Contains(activeControl))
				panelMain.Controls.Remove(activeControl);

			// add the new one
			activeControl = _c;
			activeControl.Location = new System.Drawing.Point(177, 51);
			activeControl.Name = "ctrlMode";
			activeControl.Size = new System.Drawing.Size(624, 582);
			activeControl.TabIndex = 1;
			activeControl.BackColor = Color.DarkGray;
			activeControl.Dock = DockStyle.Fill;
			panelMain.Controls.Add(activeControl);
		}

		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileModeToolStripMenuItem.Checked = (activeControl is CtrlModeFile);
			libraryModeToolStripMenuItem.Checked = (activeControl is CtrlModeLibrary);
		}
	}
}
