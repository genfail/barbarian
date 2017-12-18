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
using System.Management; // System.Management.dll
using System.Globalization;
using System.Diagnostics;
using GongSolutions.Shell;

namespace GF.Barbarian.UI
{
	public partial class CtrlFolderTree : UserControl
	{
		private RootTreeNode myComputer = null;

		public string SelectedPath
		{
			get { return txtSelectedFolder.Text; }
			set
			{
				txtSelectedFolder.Text = value;
				SetFolder(txtSelectedFolder.Text);
			}
		}

		public CtrlFolderTree()
		{
			InitializeComponent();
		}

		private void CtrlFolderTree_Load(object sender, EventArgs e)
		{
			// Add some exta images to the system imagelist
			this.ImageListTreeView.Images.Add(Properties.Resources.Forbidden);
			this.ImageListTreeView.Images.SetKeyName(BaseTreeNode.IdxFolderForbidden, "");
			this.ImageListTreeView.Images.Add(Properties.Resources.Dummy);
			this.ImageListTreeView.Images.SetKeyName(BaseTreeNode.IdxFolderDummy, "");

			InitListView();
			CreateRoot();

			if (DesignMode)
				return;
			SetFolder(Program.AppSettings.FileModeDirectory);
			SetFile(Program.AppSettings.FileModeFileName);


			ShellItem si = new ShellItem(Program.AppSettings.FileModeDirectory);
			shellTreeView1.SelectedFolder = si;
		}

		protected void InitListView()
		{
			lvFiles.Clear();
			lvFiles.Columns.Add("Name", 150, System.Windows.Forms.HorizontalAlignment.Left);
			lvFiles.Columns.Add("Size", 75, System.Windows.Forms.HorizontalAlignment.Right);
			lvFiles.Columns.Add("Created", 140, System.Windows.Forms.HorizontalAlignment.Left);
			lvFiles.Columns.Add("Modified", 140, System.Windows.Forms.HorizontalAlignment.Left);
			cmbFileFilter.SelectedIndex = 0;
		}
		private void CreateRoot()
		{
			tvFolders.Nodes.Clear();
			myComputer = new RootTreeNode("My Computer");
			tvFolders.Nodes.Add(myComputer);
			PopulateTreeDriveList();//Selected My Computer - repopulate drive list
		}

		//Populate folders and files when a folder is selected
		private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Debug.WriteLine("tvFolders_AfterSelect: " + e.Node.Text);
			this.Cursor = Cursors.WaitCursor;

			BaseTreeNode nodeCurrent = (BaseTreeNode)e.Node;
			if (nodeCurrent.SelectedImageIndex == 0)
				PopulateTreeDriveList();//Selected My Computer - repopulate drive list
			else
				AddHiddenChidrenForExpandBox(nodeCurrent, true);
			
			PopulateListFiles(nodeCurrent);
			txtSelectedFolder.Text = nodeCurrent.Path;

			this.Cursor = Cursors.Default;
		}

		private void tvFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			Debug.WriteLine("tvFolders_BeforeExpand");
			BaseTreeNode nodeCurrent = (BaseTreeNode)e.Node;
			AddHiddenChidrenForExpandBox(nodeCurrent);
		}

#region Populate
		private void PopulateTreeDriveList()
		{
			Debug.WriteLine("PopulateTreeDriveList");

			this.Cursor = Cursors.WaitCursor;

			BaseTreeNode nodeTreeNode;
			ManagementObjectCollection queryCollection = Global.GetDrives();
			foreach (ManagementObject mo in queryCollection)
			{
				nodeTreeNode = new DriveTreeNode(mo["Name"].ToString(), int.Parse(mo["DriveType"].ToString()) );
				myComputer.Nodes.Add(nodeTreeNode);
				// Childrens children are added via Expand()
			}
			if (myComputer.Nodes.Count > 0)
			{
				tvFolders.SelectedNode = myComputer.Nodes[0]; // Select first child, most probably c:
				tvFolders.SelectedNode.Expand();
			}
			this.Cursor = Cursors.Default;
		}

		// see if we can locate the correct node
		public void SetFolder(string suggested)
		{
			if (myComputer == null)
				return;
			BaseTreeNode someNode = myComputer;
			string[] parts = suggested.Split(new char[] {'\\'},StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = parts[i].Replace(":", ":\\"); // Drive is not 'c:' but 'c:\\'

				TreeNode[] foundNodes = someNode.Nodes.Find(parts[i], false);
				if (foundNodes?.Length > 0) // should return 1 item, not possible to have 2 folders with same name
				{
					someNode = (BaseTreeNode) foundNodes[0]; // first found
					// node might not yet been browsed. OnSelect will populate children
					tvFolders.SelectedNode = someNode;
					someNode.Expand();
				}
			}
		}

		public void SetFile(string suggested)
		{
			foreach (ListViewItem lvi in lvFiles.Items)
			{
				if (lvi.Text == suggested)
				{
					lvi.Selected = true;
					break;
				}
			}
			lvFiles.Select();// so we see what's selected
		}

		private bool RootAddHiddenChidrenForExpandBox(BaseTreeNode nodeCurrent, bool forcePrefetch = false)
		{
			if (nodeCurrent == myComputer)
				return true;

			Debug.WriteLine($"RootAddHiddenChidrenForExpandBox({nodeCurrent.Text}, {forcePrefetch})");

			try
			{
				nodeCurrent.Nodes.Clear();

				if (nodeCurrent.WantPrefetchFolders || forcePrefetch)
				{
					// add placeholders
					string[] dirs = Directory.GetDirectories(nodeCurrent.Path);
					if (dirs?.Length > 0)
					{
						//loop throught all directories
						foreach (string stringDir in dirs)
						{
							string folder = Global.GetLastFolder(stringDir);

							//create node for directories
							FolderTreeNode nodeDir = new FolderTreeNode(folder);
							nodeCurrent.Nodes.Add(nodeDir);
						}
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("GetDirectories exception: " + ex.Message);
				nodeCurrent.SetForbidden();
				return false;
			}
		}

		private void AddHiddenChidrenForExpandBox(BaseTreeNode nodeCurrent, bool forcePrefetch = false)
		{
			Debug.WriteLine($"AddHiddenChidrenForExpandBox({nodeCurrent.Text}, {forcePrefetch})");

			RootAddHiddenChidrenForExpandBox(nodeCurrent, forcePrefetch);

			Debug.WriteLine($"Loop {nodeCurrent.Nodes.Count} children");

			foreach (BaseTreeNode tn in nodeCurrent.Nodes)
			{
				RootAddHiddenChidrenForExpandBox(tn, forcePrefetch);
			}
		}

		protected void PopulateListFiles()
		{
			if (tvFolders.SelectedNode != null)
				PopulateListFiles((BaseTreeNode)tvFolders.SelectedNode);
		}

		protected void PopulateListFiles(BaseTreeNode nodeCurrent)
		{
			//Populate listview with files
			lvFiles.Items.Clear();

			if (nodeCurrent.SelectedImageIndex != 0)
			{
				//check path
				if (Directory.Exists(nodeCurrent.Path))
				{
					try
					{
						string[] lvData = new string[4];
						string[] stringFiles = Directory.GetFiles(nodeCurrent.Path, cmbFileFilter.Text);
						string stringFileName = "";
						DateTime dtCreateDate, dtModifyDate;
						Int64 lFileSize = 0;

						//loop throught all files
						foreach (string stringFile in stringFiles)
						{
							stringFileName = stringFile;
							FileInfo fi = new FileInfo(stringFileName);
							lFileSize = fi.Length;
							dtCreateDate = fi.CreationTime; //GetCreationTime(stringFileName);
							dtModifyDate = fi.LastWriteTime; //GetLastWriteTime(stringFileName);

							//create listview data
							lvData[0] = Global.GetLastFolder(stringFileName);
							lvData[1] = GF.Lib.Global.Helpers.FormatSize(lFileSize);

							//check if file is in local current day light saving time
							if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dtCreateDate) == false)
								lvData[2] = GF.Lib.Global.Helpers.FormatDate(dtCreateDate.AddHours(1));   //not in day light saving time adjust time
							else
								lvData[2] = GF.Lib.Global.Helpers.FormatDate(dtCreateDate);   //is in day light saving time adjust time

							//check if file is in local current day light saving time
							if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dtModifyDate) == false)
								lvData[3] = GF.Lib.Global.Helpers.FormatDate(dtModifyDate.AddHours(1));   //not in day light saving time adjust time
							else
								lvData[3] = GF.Lib.Global.Helpers.FormatDate(dtModifyDate); //not in day light saving time adjust time

							//Create actual list item
							ListViewItem lvItem = new ListViewItem(lvData, 0);
							lvFiles.Items.Add(lvItem);
						}
						//lblFileListInfo.Text = $"{lvFiles.Items.Count} items in list";
					}
					catch (IOException)
					{
						//MessageBox.Show("Error: Drive not ready or directory does not exist.");
						nodeCurrent.SetForbidden();
						SetNeigbour(nodeCurrent);
					}
					catch (UnauthorizedAccessException)
					{
						//MessageBox.Show("Error: Drive or directory access denided.");
						nodeCurrent.SetForbidden();
						SetNeigbour(nodeCurrent);
					}
					catch (Exception)
					{
						//MessageBox.Show("Error: " + e);
						nodeCurrent.SetForbidden();
						SetNeigbour(nodeCurrent);
					}
				}
			}
		}

		private void SetNeigbour(BaseTreeNode nodeCurrent)
		{
			Debug.WriteLine("find neighbour");
			BaseTreeNode parent = (BaseTreeNode)nodeCurrent.Parent;
			int i = parent.Nodes.IndexOf(nodeCurrent);

			// i=0, cnt=1 --> select parent
			// i=0, cnt>1 --> select i+1
			// i>0        --> select i-1

			if (i > 0)
				tvFolders.SelectedNode = parent.Nodes[i-1];
			else if (i==0 && parent.Nodes.Count > 1)
				tvFolders.SelectedNode = parent.Nodes[1];
			else
				tvFolders.SelectedNode = parent;
		}
#endregion

		private void txtSelectedFolder_Leave(object sender, EventArgs e)
		{
			SetFolder(txtSelectedFolder.Text);
		}

		private void cmbFileFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateListFiles();
		}

		private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvFiles.SelectedItems?.Count > 0)
			Debug.WriteLine("Selected: " + lvFiles.SelectedItems[0]);
		}
	}
}
