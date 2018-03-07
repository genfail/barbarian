using GF.Lib.Communication.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GF.Barbarian.Midi
{
	public class ConnectionMidiOut : ConnectionMidiBase
	{
		protected IntPtr MIDIOutHandle;
		protected MidiOutCallbackDel MidiCall;

		public ConnectionMidiOut() : base()
		{
			MidiDeviceType = DeviceTypeMidi.MidiOut;
		}

		protected override bool OpenPort(uint deviceNumber)
        {
            uint returnValue;
            bool openSuccess;

            MidiCall = new MidiOutCallbackDel(MidiCallback);

            returnValue = MidiCommands.midiOutOpen(out MIDIOutHandle, deviceNumber, MidiCall, new IntPtr(), MidiCommands.CALLBACK_FUNCTION);
            if(returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
            {
                mPortOpen = true;
                openSuccess = true;
            }
            else
            {
                //An Error occurred
                ErrorHandler(returnValue);
                mPortOpen = false;
                openSuccess = false;
            }
            return openSuccess;
        }


		/// <summary>
        /// Close the currently open port. If no port is open then this method does nothing.
        /// </summary>
        /// <returns>Returns true is the port is closed correctly. Otherwise returns false.</returns>
        protected override bool ClosePort()
        {
            uint returnValue;
            bool closeSuccess;

            if (mPortOpen)
            {
                //Reset device so it will close correctly.
                returnValue = MidiCommands.midiOutReset(MIDIOutHandle);
                if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                {
                    returnValue = MidiCommands.midiOutClose(MIDIOutHandle);

                    if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        closeSuccess = true;
                        mPortOpen = false;
                    }
                    else
                    {
                        //An Error has occurred
                        ErrorHandler(returnValue);
                        closeSuccess = false;
                    }
                }
                else
                {
                    //An Error has occurred
                    ErrorHandler(returnValue);
                    closeSuccess = false;
                }
            }
            else
            {
                closeSuccess = true;
            }

            return closeSuccess;
        }

       private void MidiCallback(int handle, int msg, int instance, int param1, int param2)
       {
            string message = "";
            
            switch (msg)
            {
                case (int)MOM.MOM_OPEN:
                    message = "MIDI Out, Open";
                    break;
                case (int)MOM.MOM_CLOSE:
                    message = "MIDI Out, Close";
                    break;
                case (int)MOM.MOM_DONE:
                    message = "MIDI Out, Done";
                    break;
                default:
                    message = "Unknown Message Recieved!";
                    break;
            }

            //Fire the event on the class instatiating thread.
            if (!disposed && syncContext != null)
            {
                syncContext.Send(state =>
                {
                       FireMessageReceived(message);
                }, null);
            }
       }

		public override uint GetDevices(out uint countDevices, out string[] deviceList)
		{
            MIDIOUTCAPS typMIDIDevCaps = new MIDIOUTCAPS();
            List<string> devices = new List<string>();
            uint lngReturn = (uint)MMSYSERR.MMSYSERR_NOERROR;
        
            countDevices = GetDeviceCount(); //Get number of MIDI output devices installed.
            if (countDevices > 0)
            {
                //Loop through all devices and record the name.
                for (uint lngLoop = 0; lngLoop < countDevices; lngLoop++)
                {
                    lngReturn = MidiCommands.midiOutGetDevCaps((UIntPtr)lngLoop, ref typMIDIDevCaps, (uint)Marshal.SizeOf(typMIDIDevCaps));
                    if (lngReturn == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        devices.Add(typMIDIDevCaps.szPname);
                    }
                    else
                    {  
                        ErrorHandler(lngReturn); //An Error occurred
                        break;
                    }
                }

                deviceList = devices.ToArray();
            }
            else
            {
                deviceList = null;
            }
            return lngReturn;
        }

		public override uint GetDeviceCount()
		{
			return MidiCommands.midiOutGetNumDevs();
		}


		        /// <summary>
        /// Send a short MIDI message via the currently open port. 
        /// If the port is not open then this method will return false and no data will be sent.
        /// </summary>
        /// <param name="lngStatus">MIDI status value for the message.</param>
        /// <param name="lngData1">MIDI parameter 1 for the message.</param>
        /// <param name="lngData2">MIDI parameter 2 for the message.</param>
        /// <returns>Returns true if the message is send succesfully. Otherwise returns false.</returns>
        public bool SendShortMessage(uint status, uint data1, uint data2) 
        {
            uint returnValue, lowWord, highWord;
            uint MIDIMessage;
            bool sendSuccess;

            if (mPortOpen)
            {
                lowWord = (data1 * 256) + status;
                highWord = data2 * 65536;
                MIDIMessage = lowWord + highWord;
                returnValue = MidiCommands.midiOutShortMsg(MIDIOutHandle, MIDIMessage);
                if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                {
                    sendSuccess = true;
                }
                else
                {
                    ErrorHandler(returnValue);
                    sendSuccess = false;
                }
            }
            else
            {
                sendSuccess = false;
            }

            return sendSuccess;
        }

        /// <summary>
        /// Send Long MIDI message via the currently open port. 
        /// If no port is currently open then the function will return false and no data will be sent.
        /// </summary>
        /// <param name="bytBuffer">Byte buffer data of full message to send.</param>
        /// <returns>Returns true is the data is sent correctly. Otherwise returns false.</returns>
        public bool SendLongMessage(byte[] messageBuffer)
        {
            IntPtr DataBufferPointer = IntPtr.Zero;
            uint lngReturn;
            MIDIHDR typMsgHeader = new MIDIHDR();
            bool blnResult;

            blnResult = false;

            if (mPortOpen)
            {
                typMsgHeader.dwBufferLength = (uint)messageBuffer.Count();
                typMsgHeader.dwFlags = 0;

                try
                {
                    typMsgHeader.lpData = Marshal.AllocHGlobal(messageBuffer.Count());
                    Marshal.Copy(messageBuffer, 0, typMsgHeader.lpData, messageBuffer.Count());

                    DataBufferPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typMsgHeader));
                    Marshal.StructureToPtr(typMsgHeader, DataBufferPointer, true);
                }
                catch (OutOfMemoryException ex)
                {
                    throw (ex);
                }

                if (DataBufferPointer != IntPtr.Zero)
                {
                    //Header must be prepared before use.
                    lngReturn = MidiCommands.midiOutPrepareHeader(MIDIOutHandle, DataBufferPointer,
                        (uint)Marshal.SizeOf(typMsgHeader));
                    if (lngReturn == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        lngReturn = (uint)MidiCommands.midiOutLongMsg(MIDIOutHandle, DataBufferPointer,
                            (uint)Marshal.SizeOf(typMsgHeader));
                        if (lngReturn == (uint)MMSYSERR.MMSYSERR_NOERROR)
                        {
                            blnResult = true;
                        }
                        else
                        {
                            ErrorHandler(lngReturn);
                        }
                    }
                    else
                    {
                        ErrorHandler(lngReturn);
                    }

                    //Unprepare header before it is deleted.
                    lngReturn = MidiCommands.midiOutUnprepareHeader(MIDIOutHandle, DataBufferPointer,
                        (uint)Marshal.SizeOf(typMsgHeader));
                    if (lngReturn != (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        ErrorHandler(lngReturn);
                    }
                    else
                    {
                        Marshal.Release(typMsgHeader.lpData);
                        Marshal.Release(DataBufferPointer);
                    }
                }
            }

            return blnResult;
        }
	}
}
