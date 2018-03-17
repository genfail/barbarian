using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GongSolutions.Shell;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
			shellTreeView1.SelectionChanged += ShellTreeView1_SelectionChanged;
			ApplySettings();
		}

		private void CheckFavourites()
		{
			ShellItem si = shellTreeView1.SelectedFolder;
			int i = cmbFavoriteFolders.FindString(si.FileSystemPath);
			cmbFavoriteFolders.SelectedIndex = i;
		}

		private void ShellTreeView1_SelectionChanged(object sender, EventArgs e)
		{
			CheckFavourites();
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

			string[] folders = Properties.Settings.Default.FavouriteFolders.Split(new[]{'|'},StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
			cmbFavoriteFolders.Items.AddRange(folders);
			CheckFavourites();
		}

		public void SaveSettings()
		{
			Properties.Settings.Default.LastSelectedFolder = shellTreeView1.SelectedFolder.FileSystemPath;

			Properties.Settings.Default.FavouriteFolders = String.Join("|", cmbFavoriteFolders.Items.Cast<Object>().Select(item => item.ToString()).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToArray());

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
			ShellItem si = shellViewFileList.SelectedItems[0];

			if (si == null)
				return false;

			int i = Array.IndexOf(shellViewFileList.Items, si);
			int iPrev = i;
			if (_dir == SelectDirection.Previous && i > 0)
			{
				while(!Ok(--i));
			}
			else if (_dir == SelectDirection.Next && i < shellViewFileList.Items.Length - 1)
			{
				while(!Ok(++i));
			}

			if (i != iPrev) // Avoid loop setting (each set causes event)
			{
				ShellItem si2 = shellViewFileList.Items[i];
				shellViewFileList.Select(si2.DisplayName);
				return true;
			}
			return false;
		}

		private bool Ok(int i)
		{
			ShellItem si = shellViewFileList.Items[i];
			if (si == null)
				return  true;
			return FitsMask(si.DisplayName , fileFilterComboBox1.Filter);
		}

		private bool FitsMask(string sFileName, string sFileMask)
		{
			Regex mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
			return mask.IsMatch(sFileName);
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

		private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == backButton)
				shellViewFileList.NavigateBack();
			else if (e.Button == forwardButton)
				shellViewFileList.NavigateForward();
			else if (e.Button == upButton)
				shellViewFileList.NavigateParent();
			else if (e.Button == navigateRootButton)
			{
				string fldr = Path.GetPathRoot(txtFolder.Text);
				SetTree(fldr);
			}
			else if (e.Button == addToFavorites)
			{
				string cur = shellTreeView1.SelectedFolder.FileSystemPath;

				if (!cmbFavoriteFolders.Items.Contains(cur))
					cmbFavoriteFolders.Items.Add(cur);
				CheckFavourites();
			}
		}

		void backButton_Popup(object sender, EventArgs e)
        {
            List<MenuItem> items = new List<MenuItem>();

            backButtonMenu.MenuItems.Clear();
            foreach (ShellItem f in shellViewFileList.History.HistoryBack)
            {
                MenuItem item = new MenuItem(f.DisplayName);
                item.Tag = f;
                item.Click += new EventHandler(backButtonMenuItem_Click);
                items.Insert(0, item);
            }
            backButtonMenu.MenuItems.AddRange(items.ToArray());
        }

        void forwardButton_Popup(object sender, EventArgs e)
        {
            forwardButtonMenu.MenuItems.Clear();
            foreach (ShellItem f in shellViewFileList.History.HistoryForward)
            {
                MenuItem item = new MenuItem(f.DisplayName);
                item.Tag = f;
                item.Click += new EventHandler(forwardButtonMenuItem_Click);
                forwardButtonMenu.MenuItems.Add(item);
            }
        }

        void backButtonMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            ShellItem folder = (ShellItem)item.Tag;
            shellViewFileList.NavigateBack(folder);
        }

        void forwardButtonMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            ShellItem folder = (ShellItem)item.Tag;
            shellViewFileList.NavigateForward(folder);
        }
	}
}
