using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.Barbarian
{
	public enum ProgramMode
	{
		File = 1,
		Library = 2
	}

	public interface ICtrlMode
	{
		ProgramMode Mode{ get;}
		void ApplySettings();
		void SaveSettings();
	}

	public class ApplicationSettings
	{
		public ProgramMode Mode { get; set; }
		public string FileModeDirectory { get; set; }
		public string FileModeFileName { get; set; }
		public string Test { get; set; }
		public bool Reset { get; set; } = false;

		public string FileModePath
		{
			get
			{
				return Path.Combine(FileModeDirectory, FileModeFileName);
			}
			set
			{
				FileModeDirectory = Path.GetDirectoryName(value);
				FileModeFileName = Path.GetFileName(value);
			}
		}
	}

	public static class Global
	{
		public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
		{
			IEnumerable<Control> controls = control.Controls.Cast<Control>();
			return controls
				.OfType<T>()
				.Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));		
		}

		public static string GetLastFolder(string stringPath)
		{
			//Get Name of folder
			string[] stringSplit = stringPath.Split('\\');
			int _maxIndex = stringSplit.Length;
			return stringSplit[_maxIndex - 1];
		}

		public static ManagementObjectCollection GetDrives()
		{
			//get drive collection
			ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * From Win32_LogicalDisk ");
			ManagementObjectCollection queryCollection = query.Get();
			return queryCollection;
		}
	}
}
