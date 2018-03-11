using GF.Barbarian.Midi;
using System;
using System.Collections.Generic;
using System.Drawing;
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

	public enum MSgSeverity
	{
		Message = 1,
		Warning = 2,
		Error = 3
	}
	public enum SelectDirection
	{
		Previous = 1,
		Current = 2,
		Next = 3
	}

	public interface ICtrlMode
	{
		ProgramMode Mode{ get;}
		void ApplySettings();
		void SaveSettings();
	}

	public static class Global
	{
		public static T[] SubArrayCopy<T>(this T[] data, uint index, uint length)
		{
			T[] result = new T[length];
			Array.Copy(data, index, result, 0, length);
			return result;
		}

		public static bool Replace<T>(this T[] data, uint indexData,  T[] replace)
		{
			if (data == null || indexData > data.Length - 1 || indexData + replace.Length > data.Length )
				return false;

			return Replace(data, indexData,  replace, 0,  (uint)replace.Length);
		}

		public static bool Replace<T>(this T[] data, uint indexData,  T[] replace, uint indexReplace,  uint lengthReplace)
		{
			for(int i = 0; i<lengthReplace;i++)
			{
				data[indexData+i] = replace[indexReplace + i];
			}
			return true;
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

		public static int LocateFirst (this byte [] self, byte [] candidate)
		{
			if (IsEmptyLocate (self, candidate))
				return -1;

			for (int i = 0; i < self.Length; i++) {
				if (!IsMatch (self, i, candidate))
					continue;

				return i;
			}
			return -1;
		}


		private static bool IsMatch (byte [] array, int position, byte [] candidate)
		{
			if (candidate.Length > (array.Length - position))
				return false;

			for (int i = 0; i < candidate.Length; i++)
				if (array [position + i] != candidate [i])
					return false;

			return true;
		}

		private static bool IsEmptyLocate (byte [] array, byte [] candidate)
		{
			return array == null
			       || candidate == null
			       || array.Length == 0
			       || candidate.Length == 0
			       || candidate.Length > array.Length;
		}

		public static Image GetMSgSeverityIcon(MSgSeverity _sev)
		{
			switch (_sev)
			{
				case MSgSeverity.Message: return Properties.Resources.msg_info;
				case MSgSeverity.Warning: return Properties.Resources.msg_warning;
				case MSgSeverity.Error:   return Properties.Resources.msg_error;
				default: return Properties.Resources.Question;
			}
		}

		public static Image GetConnectedIcon(MidiConnectionState _state)
		{
			switch (_state)
			{
				case MidiConnectionState.Unavailable:
					return Properties.Resources.ConnectionUnavailable;
				case MidiConnectionState.Available:
					return Properties.Resources.ConnectionAvailable;
				case MidiConnectionState.Connected:
					return Properties.Resources.ConnectionConnected;
				case MidiConnectionState.Uncertain:
					return Properties.Resources.ConnectionUncertain;
				case MidiConnectionState.Undefined:
				default:
					return Properties.Resources.Question;
			}
		}
	}
}
