using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GongSolutions.Shell;

namespace GF.Barbarian
{
	public partial class CtrlModeFile : UserControl, ICtrlMode
	{
		public ProgramMode Mode{ get{return ProgramMode.File; }}

		public CtrlModeFile()
		{
			InitializeComponent();
			if (this.DesignMode)
				return;
			PrepareDragDrop();
		}

		private void CtrlModeFile_Load(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;
			shellViewFileList.SelectionChanged += ShellViewFileList_SelectionChanged;
			ApplySettings();
		}

		// Selection for some reason change fires 3 SelectionChanged events: old, empty and new. We only want load once
		string prevSel = ""; 
		private void ShellViewFileList_SelectionChanged(object sender, EventArgs e)
		{
			if (shellViewFileList.SelectedItems.Length > 0)
			{
				ShellItem si = shellViewFileList.SelectedItems[0];
				if (prevSel != si.FileSystemPath)
				{
					activePatch.FileName = si.FileSystemPath; // Patches loaded inside here
					prevSel = si.FileSystemPath;  
				}
			}
		}

		public void ApplySettings()
		{
			if (this.DesignMode || !this.Visible) // if not yet loaded
				return;

			SelectPath(Path.Combine(Program.AppSettings.FileModeDirectory));

			shellViewFileList.Select(Program.AppSettings.FileModeFileName);

			activePatch.SelectedPatchIndex = Properties.Settings.Default.LastSelectedPatchIndex;

			string[] folders = Properties.Settings.Default.FavouriteFolders.Split(new[]{'|'});
			cmbFavoriteFolders.Items.AddRange(folders);
		}

		public void SaveSettings()
		{
			Properties.Settings.Default.LastSelectedFolder = shellTreeView1.SelectedFolder.FileSystemPath;

			Properties.Settings.Default.FavouriteFolders = String.Join("|", cmbFavoriteFolders.Items.Cast<Object>().Select(item => item.ToString()).ToArray());

			if (shellViewFileList.SelectedItems != null && shellViewFileList.SelectedItems.Length > 0)
			{
				ShellItem si = shellViewFileList.SelectedItems[0];
				Properties.Settings.Default.LastSelectedFile = si.DisplayName;
				Properties.Settings.Default.LastSelectedPatchIndex = activePatch.SelectedPatchIndex;
			}
			else
				Properties.Settings.Default.LastSelectedFile = "";
		}

		private void SelectPath(string _pth)
		{
			if (Directory.Exists(_pth))
			{
				string complete = Path.Combine(_pth);
				ShellItem si = new ShellItem(complete);
				shellTreeView1.SelectedFolder = si;
			}
		}

		private void LoadFolder(string _folder)
		{
			if (String.IsNullOrEmpty(_folder) || !Directory.Exists(_folder) )
			{
				Debug.WriteLine("Could not set folder");
				return;
			}

			string [] fileEntries = Directory.GetFiles(_folder);
			AddPatchFiles(fileEntries);
		}

		public void AddPatchFiles(string[] _fnames)
		{
			if (_fnames == null)
				return;

			foreach(string fname in _fnames)
			{
				AddFileContent(fname);
			}
		}
		public void AddFileContent(string _fname)
		{
			CtrlFileContent p = new CtrlFileContent();
			p.FileName = _fname;
//			flowLayoutPatches.Controls.Add(p);
		}

		#region Prepare drag&Drop
		private void PrepareDragDrop()
		{
			// We want to have all child controlls respond to the drag&drop
			AddDragDropHandlers(this);
			foreach (Control c in this.Controls)
			{
				AddDragDropHandlers(c);
			}
		}
		private void AddDragDropHandlers(Control c)
		{
			c.DragDrop += new System.Windows.Forms.DragEventHandler(this.CtrlModeFile_DragDrop);
			c.DragEnter += new System.Windows.Forms.DragEventHandler(this.CtrlModeFile_DragEnter);
			c.DragOver += new System.Windows.Forms.DragEventHandler(this.CtrlModeFile_DragOver);
			c.DragLeave += new System.EventHandler(this.CtrlModeFile_DragLeave);
		}
		#endregion

		#region Handle Drag&Drop
		private void CtrlModeFile_DragDrop(object sender, DragEventArgs e)
		{
			Debug.WriteLine("File dropped");
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			if (files != null && files.Length > 0)
			{
				ShellItem si = new ShellItem(Path.GetDirectoryName(files[0]));
				shellTreeView1.SelectedFolder = si;

				LoadFolder(Path.GetDirectoryName(files[0]));
			}
		}

		private void CtrlModeFile_DragEnter(object sender, DragEventArgs e)
		{
			Debug.WriteLine("File entered");

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void CtrlModeFile_DragLeave(object sender, EventArgs e)
		{
			Debug.WriteLine("File leaving");
		}

		private void CtrlModeFile_DragOver(object sender, DragEventArgs e)
		{
			Debug.WriteLine("File over");
		}

		#endregion

		public bool SelectItem(SelectDirection _dir)
		{

			if (shellViewFileList.Items.Length == 0 || shellViewFileList.SelectedItems.Length == 0)
			{
				return false;
			}
			else
			{
				ShellItem si = shellViewFileList.SelectedItems[0];

				int i = Array.IndexOf(shellViewFileList.Items, si);
				int i2 = i;
				if (_dir == SelectDirection.Previous && i > 0)
					i2 = i - 1;
				else
				if (_dir == SelectDirection.Next && i < shellViewFileList.Items.Length - 1)
					i2 = i + 1;

				if (i != i2) // Avoid loop setting (each set causes event)
				{
					ShellItem si2 = shellViewFileList.Items[i2];
					shellViewFileList.Select(si2.DisplayName);
					return true;
				}
				return false;
			}
		}

		private void shellTreeView1_SelectionChanged(object sender, EventArgs e)
		{
			txtFolder.Text = shellTreeView1.SelectedFolder.FileSystemPath;
		}

		private void txtFolder_Leave(object sender, EventArgs e)
		{
			SetTree(txtFolder.Text);
		}

		private void txtFolder_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				SetTree(txtFolder.Text);
		}

		private void SetTree(string _proposedPath)
		{
			if (Directory.Exists(_proposedPath))
			{
				ShellItem si = new ShellItem(_proposedPath);
				shellTreeView1.SelectedFolder = si;
			}
		}

		private void shellViewFileList_LocationChanged(object sender, EventArgs e)
		{
			Debug.WriteLine("loc changed");
		}

		private void shellViewFileList_Navigated(object sender, EventArgs e)
		{
			Debug.WriteLine("nav: " + shellTreeView1.SelectedFolder);

		}

		private void btnOpenFileExplorer_Click(object sender, EventArgs e)
		{
			ProcessStartInfo StartInformation = new ProcessStartInfo();
			StartInformation.FileName = txtFolder.Text;
			Process process = Process.Start(StartInformation);
		}

		private void btnOneFolderUp_Click(object sender, EventArgs e)
		{
			shellViewFileList.NavigateParent();
		}

		private void btnRootFolder_Click(object sender, EventArgs e)
		{
			string fldr = Path.GetPathRoot(txtFolder.Text);
			SetTree(fldr);
		}

		private void btnbtnAddToFavoriteFolders_Click(object sender, EventArgs e)
		{
			string cur = shellTreeView1.SelectedFolder.FileSystemPath;

			if (!cmbFavoriteFolders.Items.Contains(cur))
				cmbFavoriteFolders.Items.Add(cur);
		}

		private void cmbFavoriteFolders_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectPath(cmbFavoriteFolders.Text);
		}

		private void removePathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			cmbFavoriteFolders.Items.Remove(cmbFavoriteFolders.SelectedItem);
		}

		private void goToPathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectPath(cmbFavoriteFolders.Text);
		}


		protected override bool ProcessCmdKey(ref Message message, Keys keys)
		{
			switch (keys)
			{
				case Keys.P://prev
					activePatch.SelectPatch(SelectDirection.Previous, true);
					return true;
				case Keys.C: //current
					activePatch.SelectPatch(SelectDirection.Current, true);
					return true;
				case Keys.N: //next
					activePatch.SelectPatch(SelectDirection.Next, true);
					return true;
			}
			return false;
		}

	}
}
