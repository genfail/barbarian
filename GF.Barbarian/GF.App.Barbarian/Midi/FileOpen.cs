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

					SysxPatch p = SysxPatch.MakePatch(patchNumber, fileBytes, a, size);
					if (p != null)
						PatchList.Add(patchNumber, p);
                    a = m_step + 10;   // move to start of next patch
                }
			}
			return true;
		}
	}

	public class SysxPatch
	{
		public int Count { get; private set; }
		public string Name { get; private set; } = "NotSet";

		public byte[] Data { get; private set; } = null;

		public SysxPatch(int cnt)
		{
			Count = cnt;
		}

		public static SysxPatch MakePatch(int pchNr, byte[] fileData, uint offset, uint length)
		{
			if (fileData == null)
			{
				Debug.WriteLine($"Size error: ");
				return null;
			}

			if (length < 1171+52)
			{
				Debug.WriteLine($"Size error. Expected {1171+52} but according to data only {length} bytes");
				return null;
			}
			else if (fileData.Length < offset + 1171+52)
			{
				Debug.WriteLine($"Size error. Expected {offset + length} but got {fileData.Length} bytes");
				return null;
			}

			SysxPatch p = new SysxPatch(pchNr);

			byte[] buf = fileData.SubArrayCopy(offset + 1, 16);
			p.Name = System.Text.Encoding.ASCII.GetString(buf).Trim();

			// copy base data from clean file
			p.Data = Properties.Resources.Default_SYX.SubArrayCopy(0, (uint)Properties.Resources.Default_SYX.Length);

			//temp = data.mid(a, 128);
            //default_data.replace(11, 128, temp);      //address "00"
			p.Data.Replace(11, fileData, offset + 0, 128);//address "00"

            //temp = data.mid(a+128, 114);
            //default_data.replace(152, 114, temp);     //address "01"
			p.Data.Replace(152, fileData, offset + 128, 114); //address "01"

            //temp = data.mid(a+250, 14);
            //default_data.replace(266, 14, temp);     //address "01"
			p.Data.Replace(266, fileData, offset + 250, 14); //address "01"

            //temp = data.mid(a+264, 78);
            //default_data.replace(293, 78, temp);     //address "02" +
			p.Data.Replace(293, fileData, offset + 264, 78); //address "02"

			//temp = data.mid(a+350, 128);
            //default_data.replace(384, 128, temp);     //address "03" +
			p.Data.Replace(384, fileData, offset + 350, 128); //address "03"

			//temp = data.mid(a+478, 72);
            //default_data.replace(525, 72, temp);     //address "04" +
			p.Data.Replace(525, fileData, offset + 478, 72); //address "04"

			//temp = data.mid(a+606, 18);
            //default_data.replace(666, 18, temp);     //address "05" +
			p.Data.Replace(666, fileData, offset + 606, 18); //address "05"

			//temp = data.mid(a+640, 30);
            //default_data.replace(697, 30, temp);     //address "06" +
			p.Data.Replace(697, fileData, offset + 640, 30); //address "06"

			//temp = data.mid(a+678, 125);
            //default_data.replace(740, 125, temp);     //address "07" +
			p.Data.Replace(740, fileData, offset + 678, 125); //address "07"

			//temp = data.mid(a+811, 128);
            //default_data.replace(878, 128, temp);     //address "10" +
			p.Data.Replace(878, fileData, offset + 811, 128); //address "10"

			//temp = data.mid(a+939, 86);
            //default_data.replace(1019, 86, temp);     //address "11" +
			p.Data.Replace(1019, fileData, offset + 939, 86); //address "11"

			//temp = data.mid(a+1033, 35);
            //default_data.replace(1118, 35, temp);    //address "20" +
			p.Data.Replace(1118, fileData, offset + 1033, 35); //address "20"

			//temp = data.mid(a+1072, 35);
            //default_data.replace(1166, 35, temp);    //address "21" +
			p.Data.Replace(1166, fileData, offset + 1072, 35); //address "21"

			//temp = data.mid(a+1115, 52);
            //default_data.replace(1214, 52, temp);    //address "30" +
			p.Data.Replace(1214, fileData, offset + 1115, 52); //address "30"

			//temp = data.mid(a+1171, 52);
            //default_data.replace(1279, 52, temp);    //address "31" +
			p.Data.Replace(1279, fileData, offset + 1171, 52); //address "31"

#if _DBG_BYTE
			for (int i = 0; i < sysxData.Length;i++)
			{
				byte b = sysxData[i];
				if (char.IsControl((char)b))
					Debug.WriteLine($"{i, 4}\t \t{(int)b}");
				else
					Debug.WriteLine($"{i, 4}\t'{(char)b}'\t{(int)b}");
			}
#endif			
			return p;
		}

		public override string ToString()
		{
			return $"Patch[{Count, 3}]: [{Name,-16}] {(Data==null?"NULL":Data.Length.ToString())} Bytes";
		}
	}
}
