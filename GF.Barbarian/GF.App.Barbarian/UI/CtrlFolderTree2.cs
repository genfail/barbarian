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

namespace GF.Barbarian.UI
{
	public partial class CtrlFolderTree2 : UserControl
	{
		public CtrlFolderTree2()
		{
			InitializeComponent();
		}

		private void CtrlFolderTree_Load(object sender, EventArgs e)
		{
			FillTree();
		}

		private void FillTree()
		{
			//get a list of the drives
			string[] drives = Environment.GetLogicalDrives();

			foreach (string drive in drives)
			{
				DriveInfo di = new DriveInfo(drive);
				int driveImage;

				switch (di.DriveType)    //set the drive's icon
				{
					case DriveType.CDRom:
						driveImage = 3;
						break;
					case DriveType.Network:
						driveImage = 6;
						break;
					case DriveType.NoRootDirectory:
						driveImage = 8;
						break;
					case DriveType.Unknown:
						driveImage = 8;
						break;
					default:
						driveImage = 2;
						break;
				}

				TreeNode node = new TreeNode(drive.Substring(0, 1), driveImage, driveImage);
				node.Tag = drive;

				if (di.IsReady == true)
					node.Nodes.Add("...");

				dirsTreeView.Nodes.Add(node);
			}
		}

		private void dirsTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count > 0)
			{
				if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
				{
					e.Node.Nodes.Clear();

					//get the list of sub direcotires
					string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

					foreach (string dir in dirs)
					{
						DirectoryInfo di = new DirectoryInfo(dir);
						TreeNode node = new TreeNode(di.Name, 0, 1);

						try
						{
							//keep the directory's full path in the tag for use later
							node.Tag = dir;

							//if the directory has sub directories add the place holder
							if (di.GetDirectories().Count() > 0)
								node.Nodes.Add(null, "...", 0, 0);
						}
						catch (UnauthorizedAccessException)
						{
							//display a locked folder icon
							node.ImageIndex = 12;
							node.SelectedImageIndex = 12;
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message, "DirectoryLister",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						finally
						{
							e.Node.Nodes.Add(node);
						}
					}
				}
			}
		}
	}
}
