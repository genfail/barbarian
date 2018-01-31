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
using Melanchall.DryWetMidi.Smf;
using GF.Barbarian.Midi;

namespace GF.Barbarian
{
	public partial class CtrlFileContent : UserControl
	{
		private FileOpen activeFile = null;
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
				Reload(); // File is read, patches are known now
			}
		}

		public CtrlFileContent()
		{
			InitializeComponent();
		}

		private void CtrlPatchFile_Load(object sender, EventArgs e)
		{

		}

		private void Reload()
		{
			if (String.IsNullOrEmpty(fullFileName))
				return;

			activeFile = new FileOpen(fullFileName);
			activeFile.Load();
		}
	}
}
