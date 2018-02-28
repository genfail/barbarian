using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GF.Lib.Communication.Midi
{
	public static class MidiCommands
	{
		[DllImport("winmm.dll", SetLastError=true)]
		private static extern uint midiOutGetNumDevs();

		[DllImport("winmm.dll")]
		private static extern int waveInGetNumDevs();

		[DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps")]
		private static extern int waveInGetDevCapsA(int uDeviceID, ref WaveInCaps lpCaps, int uSize);


		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct WaveInCaps
		{
			public short wMid;
			public short wPid;
			public int vDriverVersion;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szPname;
			public uint dwFormats;
			public short wChannels;
			public short wReserved1;
		} 

		public static int GetDeviceCountMidiOut()
		{
			return (int)midiOutGetNumDevs();
		}
		public static int GetDeviceCountWaveIn()
		{
			return waveInGetNumDevs();
		}

		public static string[] GetDeviceNamesWaveIn()
		{
			int waveInDevicesCount = waveInGetNumDevs(); //get total
			string[] devices = new string[waveInDevicesCount];
			if (waveInDevicesCount > 0)
			{
				for (int uDeviceID = 0; uDeviceID < waveInDevicesCount; uDeviceID++)
				{
					WaveInCaps waveInCaps = new WaveInCaps();
					waveInGetDevCapsA(uDeviceID,ref waveInCaps, Marshal.SizeOf(typeof(WaveInCaps)));
					devices[uDeviceID] = new string(waveInCaps.szPname).Remove(new string(waveInCaps.szPname).IndexOf('\0')).Trim();
				}
	        }
			return devices;
		}
	}
}
