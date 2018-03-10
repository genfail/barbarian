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

		public int SelectedPatchIndex
		{
			get
			{
				return lstPatches.SelectedIndices?.Count > 0? lstPatches.SelectedIndices[0] : -1;
			}
			set
			{
				if (value != -1 &&  lstPatches.Items.Count > value)
					lstPatches.Items[value].Selected = true;
				else
					lstPatches.SelectedItems.Clear();
			}
		}

		public CtrlFileContent()
		{
			InitializeComponent();

			this.lstPatches.Columns.Add("#",60, HorizontalAlignment.Center);
			this.lstPatches.Columns.Add("Patch",200, HorizontalAlignment.Left);
		}

		private void CtrlPatchFile_Load(object sender, EventArgs e)
		{
			SetSelectedPatches();
			this.lstPatches.DoubleClick += LstPatches_DoubleClick;
			this.lstPatches.MouseDoubleClick += LstPatches_MouseDoubleClick;
		}

		private void Reload()
		{
			if (String.IsNullOrEmpty(fullFileName))
				return;

			lstPatches.Items.Clear();
			activeFile = new FileOpen(fullFileName);
			activeFile.Load();
			txtCntPatches.Text = $"{activeFile.PatchList.Count} patches";

			foreach (KeyValuePair<int,SysxPatch> kvp in activeFile.PatchList)
			{
				ListItemPatch ctrl = new ListItemPatch(kvp.Value);
				lstPatches.Items.Add(ctrl);
			}
			if (lstPatches.Items.Count > 0 && lstPatches.SelectedItems.Count == 0)
				lstPatches.Items[0].Selected = true;
			SetSelectedPatches();
		}

		private void lstPatches_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetSelectedPatches();
			SetSelectedPatches();
		}

		private void LstPatches_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			LoadPatch();
		}

		private void LstPatches_DoubleClick(object sender, EventArgs e)
		{
			LoadPatch();
		}

		private void lstPatches_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				LoadPatch();
		}

		private void btnLoadSelectedPatch_Click(object sender, EventArgs e)
		{
			LoadPatch();
		}

		private void btnLoadPreviousPatch_Click(object sender, EventArgs e)
		{
			// Is disabled when at begin, so no validation needed
			lstPatches.Items[lstPatches.SelectedItems[0].Index-1].Selected = true;
			SetSelectedPatches();
			LoadPatch();
		}

		private void btnLoadNextPatch_Click(object sender, EventArgs e)
		{
			// Is disabled when at end, so no validation needed
			lstPatches.Items[lstPatches.SelectedItems[0].Index+1].Selected = true;
			SetSelectedPatches();
			LoadPatch();
		}

		private void SetSelectedPatches()
		{
			if (lstPatches?.SelectedItems?.Count == 0)
			{
				prev = null;
				curr = null;
				next = null;
				return;
			}

			int i = lstPatches.SelectedItems[0].Index;
			curr = (ListItemPatch)lstPatches.Items[i];			
			prev = i > 0 ? (ListItemPatch)lstPatches.Items[i-1] : null;
			next = i < lstPatches.Items.Count-1 ? (ListItemPatch)lstPatches.Items[i+1] : null;

			lblPrevPatch.Text = prev == null ? "-" : prev.Name;
			lblCurrPatch.Text = curr == null ? "-" : curr.Name;
			lblNextPatch.Text = next == null ? "-" : next.Name;

			btnLoadPrevPatch.Enabled = prev != null;
			btnLoadCurrPatch.Enabled = curr != null;
			btnLoadNextPatch.Enabled = next != null;
		}

		ListItemPatch prev = null;
		ListItemPatch curr = null;
		ListItemPatch next = null;

		private void LoadPatch()
		{
			if (lstPatches?.SelectedItems?.Count < 1)
			{
				Program.Mainform.SetMessage(MSgSeverity.Warning, $"Wrong selection");
				return;
			}

			ListItemPatch p = ((ListItemPatch)lstPatches?.SelectedItems[0]);
			if(Program.Midi.Out.SendLongMessage(p.Data))
			{
				Program.Mainform.SetMessage(MSgSeverity.Message, $"Loaded: [{p.Name}]");
			}
			else
			{
				Program.Mainform.SetMessage(MSgSeverity.Error, $"Error loading: [{p.Name}]");
			}
		}
	}

	public class ListItemPatch : ListViewItem
	{
		public int PatchCount { get{ return patch.Count;} }
		public byte[] Data { get{ return patch.Data;} }

		private SysxPatch patch;
		public ListItemPatch(SysxPatch p)
		{
			patch = p;
			this.Text = p.Count.ToString();
			this.SubItems.Add(p.Name);
			Name = patch.Name;
		}
	}
}
