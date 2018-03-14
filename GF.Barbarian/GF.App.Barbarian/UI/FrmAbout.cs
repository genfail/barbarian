using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.Barbarian
{
	public partial class FrmAbout : Form
	{
		public FrmAbout()
		{
			InitializeComponent();
		}

		private void FrmAbout_Load(object sender, EventArgs e)
		{
			lblTitle.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;
			lblDescr.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileDescription; 

			lblProduct.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName; 
			lblCopyright.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright;

			lblFileVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; 
			lblAssyVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
		}
	}
}
