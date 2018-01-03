using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace GF.Barbarian
{
	public partial class CtrlPatchFile : UserControl
	{
		private string fullFileName;
		public string FileName
		{
			get
			{
				return fullFileName;
			}
			set
			{
				fullFileName = value;
				txtFname.Text = Path.GetFileNameWithoutExtension(value);
				Reload();
			}
		}

		public CtrlPatchFile()
		{
			InitializeComponent();
		}

		private void CtrlPatchFile_Load(object sender, EventArgs e)
		{

		}

		private void Reload()
		{
			Debug.WriteLine($"Loading file: {fullFileName}");
		}
	}
}
