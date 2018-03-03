using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GF.Lib.Communication.Midi
{
	public static class MidiCommands
	{
        public const int MAXPNAMELEN = 32;
        public const int CALLBACK_FUNCTION= 0x30000;        //dwCallback is a FARPROC

		[DllImport("winmm.dll")]
		public static extern uint midiInAddBuffer(IntPtr hMIDIIn, IntPtr lpMidiInHdr, uint uSize);

		[DllImport("winmm.dll", SetLastError=true)]
		private static extern uint midiOutGetNumDevs();

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInGetNumDevs();

	    [DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS lpMidiInCaps,  uint uSize);

		[DllImport("winmm.dll")]
		private static extern int waveInGetNumDevs();

		[DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps")]
		private static extern int waveInGetDevCapsA(int uDeviceID, ref WaveInCaps lpCaps, int uSize);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInOpen(out IntPtr lphMIDIIn, uint uDeviceID, MidiInCallbackDel dwCallback, IntPtr dwInstance, uint dwFlags);

		[DllImport("winmm.dll")]
		public static extern uint midiInStop(IntPtr hMIDIIn);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInReset(IntPtr hMIDIIn);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInClose(IntPtr hMIDIIn);

		[DllImport("winmm.dll")]
		public static extern uint midiInPrepareHeader(IntPtr hMIDIIn, IntPtr lpMidiInHdr, uint uSize);

		[DllImport("winmm.dll")]
		public static extern uint midiInUnprepareHeader(IntPtr hMIDIIn, IntPtr lpMidiInHdr, uint uSize);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInStart(IntPtr hMIDIIn);

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
