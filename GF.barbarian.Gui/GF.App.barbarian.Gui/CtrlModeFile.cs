using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.barbarian.Gui
{
	public partial class CtrlModeFile : UserControl
	{
		public CtrlModeFile()
		{
			InitializeComponent();
		}

		public void AddPatchFiles(string[] _fnames)
		{
			foreach(string fname in _fnames)
			{
				AddPatchFile(fname);
			}
		}
		public void AddPatchFile(string _fname)
		{

		}
	}
}
