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

namespace GF.Barbarian.UI
{
	public partial class CtrlFolderTree : UserControl
	{
		private RootTreeNode root = null;
/*
		public const int idxFolderOther = 2;
		public const int idxFolderSelected = 3;
		public const int idxFolderForbidden = 9;
		public const int idxFolderDummy = 10;
*/

		public CtrlFolderTree()
		{
			InitializeComponent();

/*
			BaseTreeNode r1 = new BaseTreeNode("r1");
			BaseTreeNode r2 = new BaseTreeNode("r2");
			BaseTreeNode r3 = new BaseTreeNode("r3");
			tvFolders.Nodes.Add(r1);
			r1.Nodes.Add(r2);
			r2.Nodes.Add(r3);
			string s1 = r3.FullPath;

			r1.Name = "n1";
			r2.Name = "n2";
			string s2 = r3.FullPath;

			Debug.WriteLine("----");
*/
		}

		private void CtrlFolderTree_Load(object sender, EventArgs e)
		{
			// Add some exta images to the system imagelist
			this.ImageListTreeView.Images.Add(Properties.Resources.Forbidden);
			this.ImageListTreeView.Images.SetKeyName(BaseTreeNode.IdxFolderForbidden, "");
			this.ImageListTreeView.Images.Add(Properties.Resources.Dummy);
			this.ImageListTreeView.Images.SetKeyName(BaseTreeNode.IdxFolderDummy, "");
			PopulateTreeDriveList();
		}

		public void SetFolder(string suggested)
		{
			BaseTreeNode someNode = root;
			string[] parts = suggested.Split(new char[] {'\\'});
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = parts[i].Replace(":", ":\\"); // Drive is not 'c:' but 'c:\\'
/*
				foreach (BaseTreeNode node in someNode.Nodes)
				{
					Debug.WriteLine("Folder  name: [" + node.Name + "] txt: [" + node.Text + "]");
				}
*/
				TreeNode[] foundNodes = someNode.Nodes.Find(parts[i], false);
				if (foundNodes?.Length > 0) // should return 1 item, not possible to have 2 folders with same name
					someNode = (BaseTreeNode)foundNodes[0];
			}
			someNode.ExpandAll();
		}

		protected void InitListView()
		{
			//init ListView control
			lvFiles.Clear();        //clear control
			//create column header for ListView
			lvFiles.Columns.Add("Name", 150, System.Windows.Forms.HorizontalAlignment.Left);
			lvFiles.Columns.Add("Size", 75, System.Windows.Forms.HorizontalAlignment.Right);
			lvFiles.Columns.Add("Created", 140, System.Windows.Forms.HorizontalAlignment.Left);
			lvFiles.Columns.Add("Modified", 140, System.Windows.Forms.HorizontalAlignment.Left);
		}

		private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//Populate folders and files when a folder is selected
			this.Cursor = Cursors.WaitCursor;

			BaseTreeNode nodeCurrent = (BaseTreeNode)e.Node;
			if (nodeCurrent.SelectedImageIndex == 0)
				PopulateTreeDriveList();//Selected My Computer - repopulate drive list
			else
				AddHiddenChidrenForExpandBox(nodeCurrent, true);
			
			PopulateListFiles(nodeCurrent);
			if (nodeCurrent.Parent != null)
				txtSelectedFolder.Text = nodeCurrent.Path;

			this.Cursor = Cursors.Default;
		}

		private void tvFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			BaseTreeNode nodeCurrent = (BaseTreeNode)e.Node;
			AddHiddenChidrenForExpandBox(nodeCurrent);
		}

#region Populate
		private void PopulateTreeDriveList()
		{
			BaseTreeNode nodeTreeNode;

			this.Cursor = Cursors.WaitCursor;
			//clear TreeView
			tvFolders.Nodes.Clear();
			root = new RootTreeNode("My Computer");
			tvFolders.Nodes.Add(root);

			//set node collection
			TreeNodeCollection nodeCollection = root.Nodes;

			ManagementObjectCollection queryCollection = getDrives();
			foreach (ManagementObject mo in queryCollection)
			{
				nodeTreeNode = new DriveTreeNode(mo["Name"].ToString(), int.Parse(mo["DriveType"].ToString()) );
				nodeCollection.Add(nodeTreeNode);
				RootAddHiddenChidrenForExpandBox(nodeTreeNode);
			}
			// preset, expand
			root.Expand();
			if (root.Nodes.Count > 0)
			{
				tvFolders.SelectedNode = root.Nodes[0];
				tvFolders.SelectedNode.Expand();
			}
			InitListView();
			this.Cursor = Cursors.Default;
		}

/*
		protected void xxPopulateTreeDirectory(BaseTreeNode nodeCurrent)
		{
			BaseTreeNode nodeDir;
			if (nodeCurrent.SelectedImageIndex != 0)
			{
				//populate treeview with folders
				try
				{
					//check path
					if (Directory.Exists(nodeCurrent.Path) == false)
					{
						MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
					}
					else
					{
						//populate files
						PopulateListFiles(nodeCurrent);

						string[] stringDirectories = Directory.GetDirectories(nodeCurrent.Path);
						string stringFullPath = "";
						string stringPathName = "";

						//loop throught all directories
						foreach (string stringDir in stringDirectories)
						{
							stringFullPath = stringDir;
							stringPathName = GetLastFolder(stringFullPath);

							//create node for directories
							nodeDir = new FolderTreeNode(stringPathName.ToString(), idxFolderOther, idxFolderSelected);
							nodeCurrent.Nodes.Add(nodeDir);

							AddHiddenChidrenForExpandBox(nodeDir);
						}
					}
				}
				catch (IOException)
				{
					MessageBox.Show("Error: Drive not ready or directory does not exist.");
				}
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("Error: Drive or directory access denided.");
				}
				catch (Exception e)
				{
					MessageBox.Show("Error: " + e);
				}
			}
		}
*/

		private void RootAddHiddenChidrenForExpandBox(BaseTreeNode nodeCurrent, bool forcePrefetch = false)
		{
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
							string folder = GetLastFolder(stringDir);

							//create node for directories
							FolderTreeNode nodeDir = new FolderTreeNode(folder);
							nodeCurrent.Nodes.Add(nodeDir);
						}
					}
				}
			}
			catch (Exception)
			{
				nodeCurrent.SetForbidden();
			}
		}

		private void AddHiddenChidrenForExpandBox(BaseTreeNode nodeCurrent, bool forcePrefetch = false)
		{
			if (forcePrefetch)
				RootAddHiddenChidrenForExpandBox(nodeCurrent, forcePrefetch);

			foreach (BaseTreeNode tn in nodeCurrent.Nodes)
			{
				RootAddHiddenChidrenForExpandBox(tn, forcePrefetch);
			}
		}

		protected void PopulateListFiles(BaseTreeNode nodeCurrent)
		{
			//Populate listview with files
			string[] lvData = new string[4];

			//clear list
			InitListView();

			if (nodeCurrent.SelectedImageIndex != 0)
			{
				//check path
				if (Directory.Exists(nodeCurrent.Path) == false)
				{
					MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
				}
				else
				{
					try
					{
						string[] stringFiles = Directory.GetFiles(nodeCurrent.Path);
						string stringFileName = "";
						DateTime dtCreateDate, dtModifyDate;
						Int64 lFileSize = 0;

						//loop throught all files
						foreach (string stringFile in stringFiles)
						{
							stringFileName = stringFile;
							FileInfo objFileSize = new FileInfo(stringFileName);
							lFileSize = objFileSize.Length;
							dtCreateDate = objFileSize.CreationTime; //GetCreationTime(stringFileName);
							dtModifyDate = objFileSize.LastWriteTime; //GetLastWriteTime(stringFileName);

							//create listview data
							lvData[0] = GetLastFolder(stringFileName);
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
					}
					catch (IOException)
					{
						MessageBox.Show("Error: Drive not ready or directory does not exist.");
					}
					catch (UnauthorizedAccessException)
					{
						MessageBox.Show("Error: Drive or directory access denided.");
					}
					catch (Exception e)
					{
						MessageBox.Show("Error: " + e);
					}
				}
			}
		}
#endregion

#region Helpers
		protected string GetLastFolder(string stringPath)
		{
			//Get Name of folder
			string[] stringSplit = stringPath.Split('\\');
			int _maxIndex = stringSplit.Length;
			return stringSplit[_maxIndex - 1];
		}
		protected ManagementObjectCollection getDrives()
		{
			//get drive collection
			ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * From Win32_LogicalDisk ");
			ManagementObjectCollection queryCollection = query.Get();
			return queryCollection;
		}
		#endregion

		private void txtSelectedFolder_Leave(object sender, EventArgs e)
		{
			SetFolder(txtSelectedFolder.Text);
		}
	}
}
