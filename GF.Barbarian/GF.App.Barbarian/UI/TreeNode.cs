using System.Windows.Forms;

namespace GF.Barbarian.UI
{
	public class BaseTreeNode : TreeNode
	{
		public const int IdxFolderOther = 2;
		public const int IdxFolderSelected = 3;
		public const int IdxFolderForbidden = 9;
		public const int IdxFolderDummy = 10;

		// To be able to show a + box before the folder icon we need to see ig there is data inside
		// so when we fill a level, we have to retrieve a level deeper as well.
		// But for network drives we do not pre-read the folders (slow)
		public bool WantPrefetchFolders { get; protected set; } = false;

		public BaseTreeNode()
		{}
		public BaseTreeNode(string text) : base(text)
		{}

		// remove roots My Computer and also the backslash in the drive letter
		public string Path => base.FullPath.Replace("My Computer\\", "").Replace("\\\\","\\");

		public void SetForbidden()
		{
			SelectedImageIndex = IdxFolderForbidden;
		}

		public static BaseTreeNode GetNodeFromPath(BaseTreeNode rootNode, string path)
		{
			BaseTreeNode foundNode = null;
			foreach (BaseTreeNode tn in rootNode.Nodes)
			{
				if (tn.FullPath == path)
				{                 
					return tn;
				}
				else if (tn.Nodes.Count > 0)
				{
					foundNode = GetNodeFromPath(tn, path);
				}
				if (foundNode != null)
					return foundNode;
			}
			return null;
		}
	}


	public class RootTreeNode : BaseTreeNode
	{
		public RootTreeNode(string text) : base(text)
		{
			Name = text;
			SelectedImageIndex = 0;
			ImageIndex = 0;
		}
	}

	public class DriveTreeNode : BaseTreeNode
	{
		public DriveTreeNode(string txt, int driveType)
		{
			Text = txt + "\\";
			Name = Text;
			SetImageIndexes(driveType);
		}

		const int Removable = 2;
		const int LocalDisk = 3;
		const int Network = 4;
		const int CD = 5;
		//const int RAMDrive = 6;
		private void SetImageIndexes(int driveType)
		{
			switch (driveType)
			{
				case Removable:         //removable drives
					ImageIndex = 5;
					SelectedImageIndex = 5;
					break;
				case LocalDisk:         //Local drives
					ImageIndex = 6;
					SelectedImageIndex = 6;
					WantPrefetchFolders = true; // other drive types not prefetched
					break;
				case CD:                //CD rom drives
					ImageIndex = 7;
					SelectedImageIndex = 7;
					break;
				case Network:           //Network drives
					ImageIndex = 8;
					SelectedImageIndex = 8;
					break;
				default:                //defalut to folder
					ImageIndex = 2;
					SelectedImageIndex = 3;
					break;
			}
		}
	}

	public class FolderTreeNode : BaseTreeNode
	{
		public FolderTreeNode(string text):base(text)
		{
			WantPrefetchFolders = true; // When we are browsing on a disk we want prefetch
			Name = text;
			ImageIndex = IdxFolderOther;
			SelectedImageIndex = IdxFolderSelected;
		}
	}

}