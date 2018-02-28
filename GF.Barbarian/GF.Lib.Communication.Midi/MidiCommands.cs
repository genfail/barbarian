using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GF.Lib.Communication.Midi
{
	public static class MidiCommands
	{
        private const int MAXPNAMELEN = 32;

        public enum MMSYSERR : uint
        {
            MMSYSERR_NOERROR = 0,
            MMSYSERR_BASE = 0,
            MMSYSERR_BADDEVICEID = (MMSYSERR_BASE + 2),     //device ID out of range
            MMSYSERR_INVALPARAM = (MMSYSERR_BASE + 11),     //invalid parameter passed
            MMSYSERR_NODRIVER = (MMSYSERR_BASE + 6),        //no device driver present
            MMSYSERR_NOMEM = (MMSYSERR_BASE + 7),           //memory allocation error
            MMSYSERR_INVALHANDLE = (MMSYSERR_BASE + 5),     //device handle is invalid
            MMSYSERR_ALLOCATED = (MMSYSERR_BASE + 4),       //device already allocated
            MMSYSERR_INVALFLAG = (MMSYSERR_BASE + 10)       //invalid flag passed
        }

        public enum MIDIERR : uint
        {
            MIDIERR_BASE = 64,
            MIDIERR_NODEVICE = (MIDIERR_BASE + 4),          //port no longer connected
            MIDIERR_STILLPLAYING = (MIDIERR_BASE + 1),      //still something playing
            MIDIERR_NOTREADY = (MIDIERR_BASE + 3),          //hardware is still busy
            MIDIERR_BADOPENMODE = (MIDIERR_BASE + 6),       //operation unsupported w/ open mode
            MIDIERR_UNPREPARED = (MIDIERR_BASE + 0)         //header not prepared
        }

        public enum MMRESULT : uint
        {
            MMSYSERR_NOERROR = 0,
            MMSYSERR_ERROR = 1,
            MMSYSERR_BADDEVICEID = 2,
            MMSYSERR_NOTENABLED = 3,
            MMSYSERR_ALLOCATED = 4,
            MMSYSERR_INVALHANDLE = 5,
            MMSYSERR_NODRIVER = 6,
            MMSYSERR_NOMEM = 7,
            MMSYSERR_NOTSUPPORTED = 8,
            MMSYSERR_BADERRNUM = 9,
            MMSYSERR_INVALFLAG = 10,
            MMSYSERR_INVALPARAM = 11,
            MMSYSERR_HANDLEBUSY = 12,
            MMSYSERR_INVALIDALIAS = 13,
            MMSYSERR_BADDB = 14,
            MMSYSERR_KEYNOTFOUND = 15,
            MMSYSERR_READERROR = 16,
            MMSYSERR_WRITEERROR = 17,
            MMSYSERR_DELETEERROR = 18,
            MMSYSERR_VALNOTFOUND = 19,
            MMSYSERR_NODRIVERCB = 20,
            WAVERR_BADFORMAT = 32,
            WAVERR_STILLPLAYING = 33,
            WAVERR_UNPREPARED = 34
        }

		[DllImport("winmm.dll", SetLastError=true)]
		private static extern uint midiOutGetNumDevs();

		[DllImport("winmm.dll", SetLastError=true)]
		public static extern uint midiInGetNumDevs();

	    [DllImport("winmm.dll", SetLastError=true)]
		private static extern uint midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS lpMidiInCaps,  uint uSize);

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

		[StructLayout(LayoutKind.Sequential)]
		private struct MIDIINCAPS
		{
			public ushort wMid;
			public ushort wPid;
			public uint vDriverVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAXPNAMELEN)]
			public string szPname;
			public uint dwSupport;
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

		public static uint ListDevicesMidiIn(out string[] deviceList)
        {
            MIDIINCAPS MyMIDIInDevCaps = new MIDIINCAPS();
            uint returnValue, lngNumOfDevices;
            List<string> devices = new List<string>();

            //Get the number of MIDI in devices installed.
            lngNumOfDevices = midiInGetNumDevs();
            if(lngNumOfDevices > 0) 
            {
                //Loop through all devices and store the device names.
                for(uint intLoop=0;intLoop<lngNumOfDevices;intLoop++)
                {
                    returnValue = (uint)midiInGetDevCaps((UIntPtr)intLoop, ref MyMIDIInDevCaps, (uint)Marshal.SizeOf(MyMIDIInDevCaps));
                    if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        devices.Add(MyMIDIInDevCaps.szPname);
                    }
                    else
                    {
                        //An Error occurred
                        ErrorHandler(returnValue);
                        break;
                    }
                }
            }

            deviceList = devices.ToArray();
            return lngNumOfDevices;
        }



        private static void ErrorHandler(uint midiErrorNumber)
        {
            string strError;

            switch(midiErrorNumber)
            {
                case (uint)MMSYSERR.MMSYSERR_ALLOCATED:
                    strError = "Allocated";
                    break;
                case (uint)MMSYSERR.MMSYSERR_BADDEVICEID:
                    strError = "Bad Device ID";
                    break;
                case (uint)MMSYSERR.MMSYSERR_INVALFLAG:
                    strError = "Invalid Flag";
                    break;
                case (uint)MMSYSERR.MMSYSERR_INVALPARAM:
                    strError = "Invalid Parameter";
                    break;
                case (uint)MMSYSERR.MMSYSERR_NODRIVER:
                    strError = "No Driver";
                    break;
                case (uint)MMSYSERR.MMSYSERR_NOMEM:
                    strError = "No Mem";
                    break;
                case (uint)MMSYSERR.MMSYSERR_INVALHANDLE:
                    strError = "Invalid Handle";
                    break;
                case (uint)MIDIERR.MIDIERR_STILLPLAYING:
                    strError = "Still Playing";
                    break;
                case (uint)MIDIERR.MIDIERR_UNPREPARED:
                    strError = "Unprepared";
                    break;
                default:
                    strError = "Unknown Error = " + midiErrorNumber.ToString();
                    break;
            }
/*
            //Send error to instatiating thread via event.
            if (!disposed)
            {
                syncContext.Send(state =>
                {
                    if (ReceiveError != null)
                        ReceiveError(this, new MidiErrorEventArgs(new Exception(strError)));
                }, null);
            }
*/
			Debug.WriteLine("MIDI error: " + strError);
        }
	}
}
