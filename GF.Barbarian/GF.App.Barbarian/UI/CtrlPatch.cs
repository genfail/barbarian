using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GF.Barbarian.Midi;

namespace GF.Barbarian.UI
{
	public partial class CtrlPatch : UserControl
	{
		private Patch patch;

		private CtrlPatch()
		{
			InitializeComponent();
		}
		public CtrlPatch(Patch p):this()
		{
			patch = p;
			InitializeComponent();
		}

		private void CtrlPatch_Load(object sender, EventArgs e)
		{
			lblPatchNr.Text = patch.Count.ToString();
			txtPatchName.Text = patch.Name;
		}
	}
}
