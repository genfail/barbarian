using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace GF.Barbarian
{
	public partial class CtrlModeFile : UserControl, ICtrlMode
	{
		public CtrlModeFile()
		{
			InitializeComponent();
			PrepareDragDrop();
		}

		private void CtrlModeFile_Load(object sender, EventArgs e)
		{}

		public ProgramMode Mode{ get{return ProgramMode.File; }}

		public void ApplySettings()
		{
			lblSelectedFolder.Text = Program.AppSettings.FileModeDirectory;
		}

		public void SaveSettings()
		{
			Properties.Settings.Default.LastSelectedFolder = lblSelectedFolder.Text;
			Properties.Settings.Default.LastSelectedFile = "";
		}


		private void LoadFolder(string _folder, string _selectedFile = null)
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
				AddPatchFile(fname);
			}
		}
		public void AddPatchFile(string _fname)
		{
			CtrlPatchFile p = new CtrlPatchFile();
			p.FileName = _fname;
			flowLayoutPatches.Controls.Add(p);
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
				lblSelectedFolder.Text = Path.GetDirectoryName(files[0]);

				LoadFolder(Path.GetDirectoryName(files[0]), files[0]);
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
	}
}
