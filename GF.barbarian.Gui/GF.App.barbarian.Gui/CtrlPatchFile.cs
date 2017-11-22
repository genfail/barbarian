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

namespace GF.barbarian.Gui
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
			}
		}

		public CtrlPatchFile()
		{
			InitializeComponent();
		}

		private void CtrlPatchFile_Load(object sender, EventArgs e)
		{

		}
	}
}
