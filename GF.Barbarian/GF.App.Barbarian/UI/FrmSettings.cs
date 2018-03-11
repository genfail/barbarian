using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.Barbarian
{
	public partial class FrmSettings : Form
	{
		public FrmSettings()
		{
			InitializeComponent();
		}

		private void frmCloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FrmSettings_Load(object sender, EventArgs e)
		{
			chkAutoConnect.Checked = Properties.Settings.Default.AutoConnect;
			chkWrapFiles.Checked = Properties.Settings.Default.WrapFiles;
		}

		private void FrmSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
			{
				Properties.Settings.Default.AutoConnect = chkAutoConnect.Checked;
				Properties.Settings.Default.WrapFiles = chkWrapFiles.Checked;
			}
		}
	}
}
