using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GF.Lib.Communication.Midi
{
	public delegate void MidiInCallbackDel(int handle, int msg, int instance, int param1, int param2);
	public delegate void MessageInDel(object sender, MidiMsgEventArgs e);
	public delegate void ShortDel(object sender, MidiShortMsgEventArgs e);
	public delegate void LongDel(object sender, MidiLongMsgEventArgs e);
	public delegate void ReceiveErrorDel(object sender, MidiErrorEventArgs e);

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIHDR
    {
        public IntPtr lpData;
        public uint dwBufferLength;
        public uint dwBytesRecorded;
        public int dwUser;
        public uint dwFlags;
        public IntPtr lpNext;
        public int Reserved;
        public uint dwOffset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
        public int[] reservedArray;
    }

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WaveInCaps
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
	public struct MIDIINCAPS
	{
		public ushort wMid;
		public ushort wPid;
		public uint vDriverVersion;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MidiCommands.MAXPNAMELEN)]
		public string szPname;
		public uint dwSupport;
	}

	public class MidiMsgEventArgs : EventArgs
	{
		public string MsgText { get;}

		public MidiMsgEventArgs(string msgText)
		{
			MsgText = msgText;
		}
	}

	public class MidiShortMsgEventArgs : EventArgs
	{
		public uint Status{ get;}
		public uint Data1 { get;}
		public uint Data2 { get;}

		public MidiShortMsgEventArgs(uint status, uint data1, uint data2)
		{
			Status = status;
			Data1 = data1;
			Data2 = data2;
		}
	}

	public class MidiLongMsgEventArgs : EventArgs
    {
        public byte[] MsgData { get;}

        public MidiLongMsgEventArgs(byte[] msgData)
        {
            MsgData = msgData;
        }
    }

	public class MidiErrorEventArgs : EventArgs
    {
        public Exception MidiException { get;}

        public MidiErrorEventArgs(Exception ex)
        {
            MidiException = ex;
        }
    }

}
