﻿using System;
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
using System.Drawing.Drawing2D;

namespace GF.Barbarian
{
	public partial class CtrlFileContent : UserControl
	{
		private ListItemPatch loaded = null;
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

		#region OwnerDraw colors
		private Color colTxtLoaded = Color.FromArgb(0x70,0x07,0x27);
		private Color Txt_usel = Color.FromArgb(0xFF,0xFC,0xE1);
		private Color Txt_sel = Color.FromArgb(0x01,0x1E,0x34);
		// yellow
		private Color Sel_light = Color.FromArgb(0xD6,0xCF,0x82);
		private Color Sel_dark  = Color.FromArgb(0x7E,0x73,0x07);
		// blue
		private Color USel_light = Color.FromArgb(0x95,0xA0,0xA8);
		private Color USel_dark  = Color.FromArgb(0x29,0x50,0x6D);

		private Font FontItemBold = new Font("Arial Narrow", 11,FontStyle.Bold);
		private Font FontItem = new Font("Arial", 10,FontStyle.Regular);

		StringFormat format = new StringFormat();
		#endregion

		public CtrlFileContent()
		{
			InitializeComponent();
			lstPatches.OwnerDraw = true;
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = StringAlignment.Near;
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
			SelectPatch(LoadPatch.Current, true);
		}

		private void LstPatches_DoubleClick(object sender, EventArgs e)
		{
			SelectPatch(LoadPatch.Current, true);
		}

		private void lstPatches_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				SelectPatch(LoadPatch.Current, true);
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
				LoadCurrentPatch(curr);
		}

		private void LoadCurrentPatch(ListItemPatch _toLoad)
		{
			if(Program.Midi.Out.SendLongMessage(_toLoad.Data))
			{
				loaded = _toLoad;
				Program.Mainform.SetMessage(MSgSeverity.Message, $"Loaded: [{_toLoad.Name}]");
			}
			else
			{
				loaded = null;
				Program.Mainform.SetMessage(MSgSeverity.Error, $"Error loading: [{_toLoad.Name}]");
			}

			Program.Mainform.SetPatchName(_toLoad.Text);
			lstPatches.Invalidate(); // redraw
		}

		private void lstPatches_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			ListItemPatch itm = (ListItemPatch)e.Item;

			Color colTxt = Txt_sel;
			if (e.Item.Selected)
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, Sel_light, Sel_dark, LinearGradientMode.ForwardDiagonal))
				{
					e.Graphics.FillRectangle(brush, e.Bounds);
				}

				// Draw the background and focus rectangle for a selected item.
				//e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
				e.DrawFocusRectangle();
			}
			else
			{
				colTxt = Txt_usel;
				// Draw the background for an unselected item.
				using (LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, USel_light, USel_dark, LinearGradientMode.ForwardDiagonal))
				{
					e.Graphics.FillRectangle(brush, e.Bounds);
				}
			}

			Font f = FontItem;
			if (itm == loaded)
			{
				f = FontItemBold;
				colTxt = colTxtLoaded;
			}
			using (SolidBrush drawBrush = new SolidBrush(colTxt))
			{
				Point p = e.Bounds.Location;
				p.Y += 8;
				e.Graphics.DrawString(itm.PatchCount.ToString(), f, drawBrush, p, format);
				p.X += 20;
				e.Graphics.DrawString(itm.Name, f, drawBrush, p, format);
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
