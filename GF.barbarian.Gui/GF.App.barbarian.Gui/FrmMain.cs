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
				((CtrlFileMode)activeControl).AddPatchFile(Program.Arguments.LibraryPathFileName);
			}
		}

		private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FrmSettings frm = new FrmSettings();
			frm.ShowDialog(this);
		}

		private void fileModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetMode(ProgramMode.File);
		}

		private void libraryMaodeToolStripMenuItem_Click(object sender, EventArgs e)
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
					ReplaceCtrl(new CtrlFileMode());
					break;
				case ProgramMode.Library:
					break;
				default:
					break;
			}
		}

		private Control activeControl = null;
		private void ReplaceCtrl(Control _c)
		{
			// remove previous
			if (this.Controls.Contains(activeControl))
				this.Controls.Remove(activeControl);

			// add the new one
			activeControl = _c;
			activeControl.Location = new System.Drawing.Point(177, 51);
			activeControl.Name = "ctrlFileMode1";
			activeControl.Size = new System.Drawing.Size(624, 582);
			activeControl.TabIndex = 1;
			this.Controls.Add(activeControl);
		}
	}
}
