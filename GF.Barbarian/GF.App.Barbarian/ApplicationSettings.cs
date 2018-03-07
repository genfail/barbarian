using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian
{
	public class ApplicationSettings
	{
		public ProgramMode Mode { get; set; }
		public string FileModeDirectory { get; set; }
		public string FileModeFileName { get; set; }
		public bool AutoConnect { get; set; }
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
}
