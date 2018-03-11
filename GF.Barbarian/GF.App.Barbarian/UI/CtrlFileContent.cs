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
			SelectPatch(LoadPatch.Current, false);
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
			// If nothing was selected then select the first one in list
			if (lstPatches.Items.Count > 0 && lstPatches.SelectedItems.Count == 0)
				lstPatches.Items[0].Selected = true;
			SelectPatch(LoadPatch.Current, false);
		}

		private void lstPatches_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectPatch(LoadPatch.Current, false);
		}

		private void LstPatches_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			LoadCurrentPatch();
		}

		private void LstPatches_DoubleClick(object sender, EventArgs e)
		{
			LoadCurrentPatch();
		}

		private void lstPatches_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				LoadCurrentPatch();
		}

		private void btnLoadSelectedPatch_Click(object sender, EventArgs e)
		{
			SelectPatch(LoadPatch.Current, true);
		}

		private void btnLoadPreviousPatch_Click(object sender, EventArgs e)
		{
			SelectPatch(LoadPatch.Previous, true);
		}

		private void btnLoadNextPatch_Click(object sender, EventArgs e)
		{
			SelectPatch(LoadPatch.Next, true);
		}

		public void SelectPatch(LoadPatch _dir, bool _load)
		{
			ListItemPatch prev = null;
			ListItemPatch curr = null;
			ListItemPatch next = null;

			if (lstPatches.Items.Count == 0 || lstPatches?.SelectedItems?.Count == 0)
			{
				prev = null;
				curr = null;
				next = null;
			}
			else
			{
				int i = lstPatches.SelectedItems[0].Index;

				if (_dir == LoadPatch.Previous && i > 0)
					i--;
				else
				if (_dir == LoadPatch.Next && i < lstPatches.Items.Count-1)
					i++;

				if (i != lstPatches.SelectedItems[0].Index) // Avoid loop setting (each set causes event)
					lstPatches.Items[i].Selected = true;

				curr = (ListItemPatch)lstPatches.Items[i];			
				prev = i > 0 ? (ListItemPatch)lstPatches.Items[i-1] : null;
				next = i < lstPatches.Items.Count-1 ? (ListItemPatch)lstPatches.Items[i+1] : null;

				curr.EnsureVisible();
			}

			lblPrevPatch.Text = prev == null ? "-" : prev.Name;
			lblCurrPatch.Text = curr == null ? "-" : curr.Name;
			lblNextPatch.Text = next == null ? "-" : next.Name;

			btnLoadPrevPatch.Enabled = prev != null;
			btnLoadCurrPatch.Enabled = curr != null;
			btnLoadNextPatch.Enabled = next != null;

			if (_load)
				LoadCurrentPatch();
		}

		private void LoadCurrentPatch()
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
			this.Text = $"{p.Count,2} {p.Name}";
			//this.SubItems.Add(p.Name);
			Name = patch.Name;
		}
	}
}
