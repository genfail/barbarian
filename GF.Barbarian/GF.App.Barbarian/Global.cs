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

	}
}
