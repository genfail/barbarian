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

		#region MIDI in
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

		[DllImport("winmm.dll")]
		public static extern uint midiInAddBuffer(IntPtr hMIDIIn, IntPtr lpMidiInHdr, uint uSize);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInStart(IntPtr hMIDIIn);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInGetNumDevs();

	    [DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS lpMidiInCaps,  uint uSize);
		#endregion

		#region MIDI out
		[DllImport("winmm.dll")]
		public static extern uint midiOutOpen(out IntPtr lphMidiOut, uint uDeviceID, MidiOutCallbackDel dwCallBack, IntPtr dwInstance, uint dwFlags);

		[DllImport("winmm.dll")]
		public static extern uint midiOutClose(IntPtr hMidiOut);

		[DllImport("winmm.dll")]
		public static extern uint midiOutReset(IntPtr hMidiOut);

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiOutGetNumDevs();

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiOutGetDevCaps(UIntPtr uDeviceID, ref MIDIOUTCAPS lpCaps, uint uSize);

		[DllImport("winmm.dll")]
		public static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

		[DllImport("winmm.dll")]
		public static extern uint midiOutPrepareHeader(IntPtr hMidiOut, IntPtr lpMidiOutHdr, uint uSize);

		[DllImport("winmm.dll")]
		public static extern uint midiOutUnprepareHeader(IntPtr hMidiOut, IntPtr lpMidiOutHdr, uint uSize);

		[DllImport("winmm.dll")]
		public static extern uint midiOutLongMsg(IntPtr hMidiOut, IntPtr lpMidiOutHdr, uint uSize);
		#endregion

		#region WAV in
		[DllImport("winmm.dll")]
		public static extern int waveInGetNumDevs();

		[DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps")]
		private static extern int waveInGetDevCapsA(int uDeviceID, ref WaveInCaps lpCaps, int uSize);
		#endregion

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

		public static MMSYSERR ToEnun(uint midiErrorCode)
		{
			if(Enum.IsDefined(typeof(MMSYSERR), midiErrorCode))
				return (MMSYSERR)midiErrorCode;

			return MMSYSERR.MMSYSERR_UNKNOWN_ERROR;
		}

		public static string ErrorText(uint midiErrorNumber)
        {
            switch(midiErrorNumber)
            {
                case (uint)MMSYSERR.MMSYSERR_ALLOCATED:
                    return "Allocated";
                case (uint)MMSYSERR.MMSYSERR_BADDEVICEID:
                    return "Bad Device ID";
                case (uint)MMSYSERR.MMSYSERR_INVALFLAG:
                    return "Invalid Flag";
                case (uint)MMSYSERR.MMSYSERR_INVALPARAM:
                    return "Invalid Parameter";
                case (uint)MMSYSERR.MMSYSERR_NODRIVER:
                    return "No Driver";
                case (uint)MMSYSERR.MMSYSERR_NOMEM:
                    return "No Mem";
                case (uint)MMSYSERR.MMSYSERR_INVALHANDLE:
                    return "Invalid Handle";
                case (uint)MIDIERR.MIDIERR_STILLPLAYING:
                    return "Still Playing";
                case (uint)MIDIERR.MIDIERR_UNPREPARED:
                    return "Unprepared";
                case (uint)MMSYSERR.MMSYSERR_UNKNOWN_ERROR:
                    return "Unknown error (unknown code)";
                default:
                    return "Unknown Error = " + midiErrorNumber.ToString();
            }
		}
	}
}
