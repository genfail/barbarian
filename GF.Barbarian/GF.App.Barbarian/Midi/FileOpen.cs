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
		private Dictionary<int,SysxPatch> patchList = new Dictionary<int, SysxPatch>();
		public Dictionary<int, SysxPatch> PatchList { get => patchList; set => patchList = value; }

		private byte[] fileBytes = null;
		private string fullFileName = null;

		public bool IsDataOk{ get { return fileBytes != null && fileBytes.Length > 1000; }}


		public FileOpen()
		{
			PatchList.Clear();
		}

		public FileOpen(string _fullFileName):this()
		{
			fullFileName = _fullFileName;
		}

		public void Unload()
		{
			fileBytes = null;
			PatchList.Clear();
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
			Debug.WriteLine($"Loading file: [{fullFileName}]");
			if (String.IsNullOrEmpty(fullFileName))
				return FileLoadResult.ErrorFileNameNull;
			if (!File.Exists(fullFileName))
				return FileLoadResult.ErrorFileNameNotExist;
			if (!ReadAllbytesFromFile())
				return FileLoadResult.ErrorFileAccess;
			if (!ValidateFileFormat())
				return FileLoadResult.ErrorBadStartBytes;

			//int cnt = FindPatches();

			Debug.WriteLine($"  found {PatchList.Count} patches");
			foreach (SysxPatch p in PatchList.Values)
			{
				Debug.WriteLine($"  - " + p.ToString());
			}
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
				Debug.WriteLine("  This file has G5L format");
				return DoG5L();
			}
			else if(IsSyx())
			{
				Debug.WriteLine("  This file has Syx format");
				return DoSyx();
			}
			else if(IsSMF())
			{
				Debug.WriteLine("  This file has SMF format");
				return false;
			}
			return false;
		}

		private bool IsG5L()
		{
			byte[] g5lDefaultHeader = Barbarian.Properties.Resources.Default_G5L.SubArrayCopy(3,20);
			if (fileBytes.LocateFirst(g5lDefaultHeader) > -1)  // expect 3
				return true;

			return false;
		}
		private bool IsSyx()
		{
			return Path.GetExtension(fullFileName).ToLower().Equals(".syx");
		}
		private bool IsSMF()
		{
			byte[] sysxDefaultHeader = Barbarian.Properties.Resources.Default_SYX.SubArrayCopy(0,7);
			return false; // not yet supported
		}

		private bool DoG5L()
		{
			byte msb = fileBytes[34];     // find patch count msb bit in G5L file at byte 34
			byte lsb = fileBytes[35];     // find patch count lsb bit in G5L file at byte 35
			uint patchCount = (uint)((msb<<8) + lsb);
			Debug.WriteLine($"PatchCount according file: {patchCount} ");

			if (patchCount > 1)
			{
				uint m_step = 162;
                uint a = m_step + 10;
                PatchList.Clear();

				// for each patch
				for (int patchNumber=1; patchNumber <= patchCount; patchNumber++)
                {
					msb = fileBytes[m_step];     // find patch size msb bit
                    lsb = fileBytes[m_step+1];   // find patch size lsb bit and calculate jump to next patch.
					uint size = (uint)((msb<<8) + lsb);

					m_step += 4 + size;

					//Debug.WriteLine($"  - ({patchNumber}) - [{name}] size:{m_step}");

					SysxPatch p = SysxPatch.MakePatchFromG5L(patchNumber, fileBytes, a, size);
					if (p != null)
						PatchList.Add(patchNumber, p);
                    a = m_step + 10;   // move to start of next patch
                }
			}
			return true;
		}

		private bool DoSyx()
		{
			SysxPatch p = SysxPatch.MakePatchFromSyx(1, fileBytes);
			if (p != null)
			{
				PatchList.Add(1, p);
				return true;
			}
			return false;
		}
	}
}
