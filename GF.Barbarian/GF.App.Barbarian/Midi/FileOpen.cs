using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian.Midi
{
	public enum FileLoadResult
	{
		Undefined = 0,
		Ok = 1,
		ErrorFileNameNull,
		ErrorFileNameNotExist,
		ErrorFileAccess,
		ErrorBadStartBytes,
		ErrorUnspecified,
	}

	public class FileOpen
	{
		private Dictionary<int,Patch> patchList = new Dictionary<int, Patch>();
		private byte[] fileBytes = null;
		private string fullFileName = null;

		public bool IsDataOk{ get { return fileBytes != null && fileBytes.Length > 1000; }}

		public FileOpen()
		{
			patchList.Clear();
		}

		public FileOpen(string _fullFileName):this()
		{
			fullFileName = _fullFileName;
		}

		public void Unload()
		{
			fileBytes = null;
			patchList.Clear();
		}

		public FileLoadResult Load(string _fullFileName)
		{
			fullFileName = _fullFileName;
			return Load();
		}
		public FileLoadResult Load()
		{
			FileLoadResult result = InternalLoad();
			if (result != FileLoadResult.Ok)
				Unload();
			return result;
		}

		private FileLoadResult InternalLoad()
		{
			Debug.WriteLine($"Loading file: {fullFileName}");
			if (String.IsNullOrEmpty(fullFileName))
				return FileLoadResult.ErrorFileNameNull;
			if (!File.Exists(fullFileName))
				return FileLoadResult.ErrorFileNameNotExist;
			if (!ReadAllbytesFromFile())
				return FileLoadResult.ErrorFileAccess;
			if (!ValidateFileFormat())
				return FileLoadResult.ErrorBadStartBytes;

			//int cnt = FindPatches();
/*
			Debug.WriteLine($"  found {cnt} patches");
			foreach (Patch p in patches.Values)
			{
				Debug.WriteLine($"  - " + p.ToString());
			}
			*/
			return FileLoadResult.Ok;
		}

		private bool ReadAllbytesFromFile()
		{
			try
			{
				fileBytes = File.ReadAllBytes(fullFileName);
				return true;
			}
			catch (Exception ex)
			{
				fileBytes = null;
				Debug.WriteLine("Excepion reading G5L file: " + ex.Message);
				return false;
			}			
		}

		private bool ValidateFileFormat()
		{
			if(IsG5L())
			{
				return DoG5L();
			}
			else if(IsSMF())
			{
				return false;
			}
			return false;
		}

		private bool IsG5L()
		{
			byte[] g5lDefaultHeader = Barbarian.Properties.Resources.Default_G5L.SubArray(3,20);
			if (fileBytes.LocateFirst(g5lDefaultHeader) > -1)  // expect 3
				return true;

			return false;
		}
		private bool IsSMF()
		{
			byte[] sysxDefaultHeader = Barbarian.Properties.Resources.Default_SYX.SubArray(0,7);
			return false; // not yet supported
		}

		private bool DoG5L()
		{
			byte msb = fileBytes[34];     // find patch count msb bit in G5L file at byte 34
			byte lsb = fileBytes[35];     // find patch count lsb bit in G5L file at byte 35
			int patchCount = (msb<<8) + lsb;
			Debug.WriteLine($"PatchCount according file: {patchCount} ");

//			byte[] DefaultG5L = Barbarian.Properties.Resources.Default_G5L;
//			byte[] DefaultSYX = Barbarian.Properties.Resources.Default_SYX;
//			byte[] dataHeader = fileBytes.SubArray(0,7);

			int cnt = 0;

			if (patchCount > 1)
			{
				int m_step = 162;
                int a = m_step + 10;
                patchList.Clear();

				// for each patch
				for (int h=0; h < patchCount; h++)
                {
					byte[] buf = fileBytes.SubArray(a, 17);
					string name = System.Text.Encoding.ASCII.GetString(buf).Trim();
					int patchNumber = h+1;


					msb = fileBytes[m_step];     // find patch size msb bit
                    lsb = fileBytes[m_step+1];   // find patch size lsb bit and calculate jump to next patch.
					m_step = (msb<<8) + lsb;
                    a = m_step + 10;   // move to start of patch
                };
			}

			return true;
		}
	}

	public class Patch
	{
		public int Count { get; private set; }
		public int Offset { get; private set; }
		public string Name { get; private set; }
		public Patch(int cnt, int offset, string name)
		{
			Count = cnt;
			Offset = offset;
			Name = name;
		}

		public override string ToString()
		{
			return $"Patch[{Count, 3}]: [{Name,-16}] Offset:{Offset}";
		}
	}
}
