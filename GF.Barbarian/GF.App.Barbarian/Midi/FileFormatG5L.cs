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

	public class FileFormatG5L
	{
		private Dictionary<int,Patch> patches = new Dictionary<int, Patch>();
		private byte[] startBytes = null;
		private byte[] prexixPatchNameBytes = new byte[]{0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xF2, 0x00};
		private byte[] fileBytes = null;
		private string fullFileName = null;

		public bool IsDataOk{ get { return fileBytes != null && fileBytes.Length > 1000; }}

		public FileFormatG5L()
		{
			startBytes = Encoding.ASCII.GetBytes("G5LLibrarianFile0000");
		}

		public FileFormatG5L(string _fullFileName):this()
		{
			fullFileName = _fullFileName;
		}

		public void Unload()
		{
			fileBytes = null;
			patches.Clear();
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

			int cnt = FindPatches();
			Debug.WriteLine($"  contains {cnt} patches");

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
			for (int i = 0; i < startBytes.Length; i++)
			{
				if (fileBytes[i] != startBytes[i])
					return false;
			}
			return true;
		}

		private int FindPatches()
		{
			int cnt = 0;
			foreach (int position in fileBytes.Locate(prexixPatchNameBytes))
			{
				byte[] buf = fileBytes.SubArray(position + prexixPatchNameBytes.Length, 16);
				byte[] buf2 = new byte[16];
				Array.Copy(buf, buf2, 16);

				bool isOk = true;
				for (int i = 0; i < buf2.Length; i++)
				{
					if (buf2[i] < 32 || buf2[i] > 126)
					{
						buf2[i] = (byte)'.';
						isOk = false;
					}
				}
				string s = System.Text.Encoding.ASCII.GetString(buf2).Trim();
//				if (s.Equals("Boss Dist w SYNT"))
	//				Debug.WriteLine("-----------");  // wxHexEditor-64Bit

				if (isOk)
					patches.Add(++cnt, new Patch(cnt, position, s));
				else
					Debug.WriteLine ($"Found error patch [{s,-16}] on pos: {position}");
			}
			return cnt;
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
			return $"Patch: [{Name,-16}] Offset:{Offset}";
		}
	}

}
