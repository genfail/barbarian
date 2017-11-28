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
		public const int idxFolderOther = 2;
		public const int idxFolderSelected = 3;
		public const int idxFolderForbidden = 9;
		public const int idxFolderDummy = 10;

		public CtrlFolderTree()
		{
			InitializeComponent();
		}

		private void CtrlFolderTree_Load(object sender, EventArgs e)
		{
			this.ImageListTreeView.Images.Add(Properties.Resources.Forbidden);
			this.ImageListTreeView.Images.SetKeyName(idxFolderForbidden, "");
			this.ImageListTreeView.Images.Add(Properties.Resources.Dummy);
			this.ImageListTreeView.Images.SetKeyName(idxFolderDummy, "");

			PopulateTreeDriveList();
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

			//get current selected drive or folder
			TreeNode nodeCurrent = e.Node;

			//clear all sub-folders (remove dummy)
			nodeCurrent.Nodes.Clear();

			if (nodeCurrent.SelectedImageIndex == 0)
				PopulateTreeDriveList();//Selected My Computer - repopulate drive list
			else
				PopulateTreeDirectory(nodeCurrent);  //populate sub-folders and folder files

			this.Cursor = Cursors.Default;
		}

		private void tvFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			TreeNode nodeCurrent = e.Node;
			DeleteDummy(nodeCurrent);
		}

		private void DeleteDummy(TreeNode nodeCurrent)
		{
			//clear all sub-folders (remove dummy)
			TreeNode node = nodeCurrent.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Name == "DUMMY");
			if (node != null)
				nodeCurrent.Nodes.Remove(node);
		}

		private void tvFolders_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode nodeCurrent = e.Node;
			DeleteDummy(nodeCurrent);
		}

#region Populate
		private void PopulateTreeDriveList()
		{
			TreeNode nodeTreeNode;
			int imageIndex = 0;
			int selectIndex = 0;

			const int Removable = 2;
			const int LocalDisk = 3;
			const int Network = 4;
			const int CD = 5;
			//const int RAMDrive = 6;

			this.Cursor = Cursors.WaitCursor;
			//clear TreeView
			tvFolders.Nodes.Clear();
			TreeNode root = new TreeNode("My Computer", 0, 0);
			tvFolders.Nodes.Add(root);

			//set node collection
			TreeNodeCollection nodeCollection = root.Nodes;

			ManagementObjectCollection queryCollection = getDrives();
			foreach (ManagementObject mo in queryCollection)
			{
				switch (int.Parse(mo["DriveType"].ToString()))
				{
					case Removable:         //removable drives
						imageIndex = 5;
						selectIndex = 5;
						break;
					case LocalDisk:         //Local drives
						imageIndex = 6;
						selectIndex = 6;
						break;
					case CD:                //CD rom drives
						imageIndex = 7;
						selectIndex = 7;
						break;
					case Network:           //Network drives
						imageIndex = 8;
						selectIndex = 8;
						break;
					default:                //defalut to folder
						imageIndex = 2;
						selectIndex = 3;
						break;
				}
				nodeTreeNode = new TreeNode(mo["Name"].ToString() + "\\", imageIndex, selectIndex);//create new drive node
				nodeCollection.Add(nodeTreeNode);
			}

			root.ExpandAll();
			InitListView();
			this.Cursor = Cursors.Default;
		}

		protected void PopulateTreeDirectory(TreeNode nodeCurrent)
		{
			TreeNode nodeDir;
			if (nodeCurrent.SelectedImageIndex != 0)
			{
				//populate treeview with folders
				try
				{
					//check path
					if (Directory.Exists(getFullPath(nodeCurrent.FullPath)) == false)
					{
						MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
					}
					else
					{
						//populate files
						PopulateListFiles(nodeCurrent);

						string[] stringDirectories = Directory.GetDirectories(getFullPath(nodeCurrent.FullPath));
						string stringFullPath = "";
						string stringPathName = "";

						//loop throught all directories
						foreach (string stringDir in stringDirectories)
						{
							stringFullPath = stringDir;
							stringPathName = GetPathName(stringFullPath);

							//create node for directories
							nodeDir = new TreeNode(stringPathName.ToString(), idxFolderOther, idxFolderSelected);
							nodeCurrent.Nodes.Add(nodeDir);

							AddDummy(nodeDir);
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

		private void AddDummy(TreeNode nodeCurrent)
		{
			try
			{
				// add placeholders
				string[] dirs = Directory.GetDirectories(getFullPath(nodeCurrent.FullPath));
				if (dirs?.Length > 0)
					nodeCurrent.Nodes.Add(null, "DUMMY", idxFolderDummy, idxFolderDummy);
			}
			catch (Exception)
			{
				nodeCurrent.ImageIndex = idxFolderForbidden;
			}
		}

		protected void PopulateListFiles(TreeNode nodeCurrent)
		{
			//Populate listview with files
			string[] lvData = new string[4];

			//clear list
			InitListView();

			if (nodeCurrent.SelectedImageIndex != 0)
			{
				//check path
				if (Directory.Exists((string)getFullPath(nodeCurrent.FullPath)) == false)
				{
					MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
				}
				else
				{
					try
					{
						string[] stringFiles = Directory.GetFiles(getFullPath(nodeCurrent.FullPath));
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
							lvData[0] = GetPathName(stringFileName);
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
		protected string GetPathName(string stringPath)
		{
			//Get Name of folder
			string[] stringSplit = stringPath.Split('\\');
			int _maxIndex = stringSplit.Length;
			return stringSplit[_maxIndex - 1];
		}
		protected string getFullPath(string stringPath)
		{
			//Get Full path
			string stringParse = "";
			//remove My Computer from path.
			stringParse = stringPath.Replace("My Computer\\", "");

			return stringParse;
		}

		protected ManagementObjectCollection getDrives()
		{
			//get drive collection
			ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * From Win32_LogicalDisk ");
			ManagementObjectCollection queryCollection = query.Get();
			return queryCollection;
		}

		#endregion
	}
}
