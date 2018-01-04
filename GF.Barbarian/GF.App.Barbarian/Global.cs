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
		public static T[] SubArray<T>(this T[] data, int index, int length)
		{
			T[] result = new T[length];
			Array.Copy(data, index, result, 0, length);
			return result;
		}

		static readonly int [] Empty = new int [0];

		public static int [] Locate (this byte [] self, byte [] candidate)
		{
			if (IsEmptyLocate (self, candidate))
				return Empty;

			var list = new List<int> ();

			for (int i = 0; i < self.Length; i++) {
				if (!IsMatch (self, i, candidate))
					continue;

				list.Add (i);
			}

			return list.Count == 0 ? Empty : list.ToArray ();
		}

		static bool IsMatch (byte [] array, int position, byte [] candidate)
		{
			if (candidate.Length > (array.Length - position))
				return false;

			for (int i = 0; i < candidate.Length; i++)
				if (array [position + i] != candidate [i])
					return false;

			return true;
		}


		static bool IsEmptyLocate (byte [] array, byte [] candidate)
		{
			return array == null
			       || candidate == null
			       || array.Length == 0
			       || candidate.Length == 0
			       || candidate.Length > array.Length;
		}
	}
}
