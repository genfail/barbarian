using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian.Midi
{
	public class SysxFile
	{
		private string fullFileName = null;
		public SysxFile()
		{
		}

		public SysxFile(string _fullFileName)
		{
			fullFileName = _fullFileName;
		}

		public void Load(string _fullFileName)
		{
			fullFileName = _fullFileName;
		}

		public void Load()
		{
			if (String.IsNullOrEmpty(fullFileName))
				return;
			byte[] fileBytes = File.ReadAllBytes(fullFileName);

			Debug.WriteLine($"Loading file: {fullFileName}");
		}
	}
}
