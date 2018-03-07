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
	public class ConnectionMidiIn : ConnectionMidiBase
	{
		protected IntPtr MIDIInHandle;
		protected MidiInCallbackDel MidiCall;

		public ConnectionMidiIn() : base()
		{
			MidiDeviceType = DeviceTypeMidi.MidiIn;
		}

		protected override bool OpenPort(uint deviceNumber)
        {
            uint returnValue;
            bool startSuccess, openSuccess;

            MidiCall = new MidiInCallbackDel(MidiCallback);

            returnValue = MidiCommands.midiInOpen(out MIDIInHandle, deviceNumber, MidiCall, new IntPtr(), (uint)MidiCommands.CALLBACK_FUNCTION);

            if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
            {
                startSuccess = StartRecording();
                if (startSuccess == false)
                {
                    ErrorHandler(returnValue);
                }
                mPortOpen = true;
                openSuccess = true;
            }
            else
            {
                ErrorHandler(returnValue);

                mPortOpen = false;
                openSuccess = false;
            }
            return openSuccess;
        }

        protected override bool ClosePort()
        {
            uint returnValue = 0;
            bool stopSuccess, closeSuccess = false;

            if(mPortOpen)
            {
                mPortOpen = false;

                stopSuccess = StopRecording();
                if (stopSuccess)
                {
                    returnValue = MidiCommands.midiInClose(MIDIInHandle);
                    if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        closeSuccess = true;
                    }
                    else
                    {
                        ErrorHandler(returnValue);
                    }
                }
                else
                {
                    ErrorHandler(returnValue);
                }
            }
            else
            {
                closeSuccess = true;
            }
            
            return closeSuccess;
        }

		/// <summary>
		/// </summary>
		/// <param name="countDevices"></param>
		/// <param name="deviceList"></param>
		/// <returns>Midi error </returns>
		public override uint GetDevices(out uint countDevices, out string[] deviceList)
        {
            MIDIINCAPS MyMIDIInDevCaps = new MIDIINCAPS();
            uint returnValue = (uint)MMSYSERR.MMSYSERR_NOERROR;
            List<string> devices = new List<string>();

            countDevices = GetDeviceCount(); //Get number of MIDI output devices installed.
            if(countDevices > 0) 
            {
                //Loop through all devices and store the device names.
                for(uint intLoop=0;intLoop < countDevices;intLoop++)
                {
                    returnValue = MidiCommands.midiInGetDevCaps((UIntPtr)intLoop, ref MyMIDIInDevCaps, (uint)Marshal.SizeOf(MyMIDIInDevCaps));
                    if (returnValue == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        devices.Add(MyMIDIInDevCaps.szPname);
                    }
                    else
                    {
                        break; //An Error occurred
                    }
                }
            }

            deviceList = devices.ToArray();
            return returnValue;
        }

		public override uint GetDeviceCount()
		{
			return MidiCommands.midiInGetNumDevs();
		}

		protected void MidiCallback(int handle, int msg, int instance, int param1, int param2)
        {
            string message = "";
            bool statusMsg;

            statusMsg = true;
            switch (msg)
            {
                case (int)MIM.MM_MIM_OPEN:
                    message = $"MIDI {MidiDeviceType}, Open";
                    break;
                case (int)MIM.MM_MIM_CLOSE:
                    message = $"MIDI {MidiDeviceType}, Close";
                    break;
                case (int)MIM.MM_MIM_DATA:
                    ShortMessageReceived((uint)param1);
                    statusMsg = false;
                    break;
                case (int)MIM.MM_MIM_LONGDATA:
                    LongMessageReceived();
                    statusMsg = false;
                    break;
                case (int)MIM.MM_MIM_ERROR:
                    message = $"MIDI {MidiDeviceType}, Error";
                    break;
                case (int)MIM.MM_MIM_LONGERROR:
                    message = $"MIDI {MidiDeviceType}, Long Error";
                    break;
                case (int)MIM.MM_MIM_MOREDATA:
                    message = $"MIDI {MidiDeviceType}, More Data";
                    break;
                default:
                    message = $"MIDI {MidiDeviceType} Unknown Message Recieved!";
                    break;
            }

           if (statusMsg)
            {
                //Fire the event on the class instatiating thread.
                if (!disposed && syncContext != null)
                {
                    syncContext.Send(state =>
                    {
                       FireMessageReceived(message);
                    }, null);
                }
            }
        }

        private bool StartRecording()
        {
            uint tempReturn;
            bool StartSuccess = false;
            MIDIHDR InHeader = new MIDIHDR();
            byte[] MIDIInBuffer = new byte[mInBufferLength];

            System.Diagnostics.Debug.Assert(mInBufferLength > 0);

            //Setup MIDI header for input data
            InHeader.dwBufferLength = (uint)mInBufferLength;
            InHeader.dwBytesRecorded = 0;
            InHeader.dwUser = 0;
            InHeader.dwFlags = 0;
            for (int intLoop = 0; intLoop < mInBufferLength; intLoop++)
            {
                MIDIInBuffer[intLoop] = 0;
            }

            try
            {
                //Assign the Data point to an unmananged data block and fill in the data from the managed block
                InHeader.lpData = Marshal.AllocHGlobal(mInBufferLength);
                Marshal.Copy(MIDIInBuffer, 0, InHeader.lpData, mInBufferLength);

                //Get a pointer to the unmanaged structure memory and fill in the data from the managed block
                DataBufferPointer = Marshal.AllocHGlobal(Marshal.SizeOf(InHeader));
                Marshal.StructureToPtr(InHeader, DataBufferPointer, true);
            }
            catch(OutOfMemoryException ex)
            {
                //Send error to instatiating thread via event.
                if (!disposed && syncContext != null)
                {
                    syncContext.Send(state =>
                    {
						FireReceiveError(ex);
                    }, null);
                }
            }

            //Prepare the header, add it to the input port and start recording data.
            tempReturn = MidiCommands.midiInPrepareHeader(MIDIInHandle, DataBufferPointer, (uint)Marshal.SizeOf(new MIDIHDR()));
            if(tempReturn==(uint)MMSYSERR.MMSYSERR_NOERROR)
            {
                tempReturn = MidiCommands.midiInAddBuffer(MIDIInHandle, DataBufferPointer, (uint)Marshal.SizeOf(new MIDIHDR()));
                if(tempReturn!=(uint)MMSYSERR.MMSYSERR_NOERROR)
                {
                    ErrorHandler(tempReturn);
                }
            }
            else
            {
                ErrorHandler(tempReturn);
            }

            tempReturn = MidiCommands.midiInStart(MIDIInHandle);
            if(tempReturn==(uint)MMSYSERR.MMSYSERR_NOERROR)
            {
                StartSuccess = true;
            }
            else
            {
                ErrorHandler(tempReturn);
            }
            return StartSuccess;
        }

        private bool StopRecording()
        {
            uint tempReturn;
            bool stopSuccess;
            MIDIHDR tempHeader;

            stopSuccess = false;
            tempReturn = MidiCommands.midiInStop(MIDIInHandle);
            if(tempReturn==(uint)MMSYSERR.MMSYSERR_NOERROR)
            {
                tempReturn = MidiCommands.midiInReset(MIDIInHandle);
                if(tempReturn==(uint)MMSYSERR.MMSYSERR_NOERROR)
                {
                    //Unprepare header so it can be unassigned.
                    tempReturn = MidiCommands.midiInUnprepareHeader(MIDIInHandle, DataBufferPointer, 
                        (uint)Marshal.SizeOf(new MIDIHDR()));
                    if (tempReturn == (uint)MMSYSERR.MMSYSERR_NOERROR)
                    {
                        tempHeader = (MIDIHDR)Marshal.PtrToStructure(DataBufferPointer, typeof(MIDIHDR));
                        Marshal.Release(tempHeader.lpData);
                        Marshal.Release(DataBufferPointer);
                        stopSuccess = true;
                    }
                    else
                    {
                        ErrorHandler(tempReturn);
                    }
                }
                else
                {
                    ErrorHandler(tempReturn);
                }
            }
            else
            {
                ErrorHandler(tempReturn);
            }

            return stopSuccess;
        }

		protected void LongMessageReceived()
        {
            System.Diagnostics.Debug.Assert(mInBufferLength > 0);
            
            bool tempResult;
            MIDIHDR InHeader = new MIDIHDR();
            byte[] MIDIInBuffer = new byte[mInBufferLength];

            if(mPortOpen)
            {
                //Stop recording while processing the current data.
                tempResult = StopRecording();

                InHeader = (MIDIHDR) Marshal.PtrToStructure(DataBufferPointer, typeof(MIDIHDR));
                Marshal.Copy(InHeader.lpData, MIDIInBuffer, 0, mInBufferLength);

                if(InHeader.dwBytesRecorded > 0)
                {
                    //Fire the event on the class instatiating thread.
                    if (!disposed && syncContext != null)
                    {
                        syncContext.Send(state =>
                        {
							FireLongMessage(MIDIInBuffer);
                        }, null);
                    }
                }

                if (mPortOpen)
                {
                    tempResult = StartRecording();
                }
            }
        }

		protected void ShortMessageReceived(uint MIDIMessage)
        {
            byte MIDIStatus;
            byte MIDIData1, MIDIData2;

            MIDIData2 = (byte)((MIDIMessage / 65536) & 0xFF);
            MIDIData1 = (byte)((MIDIMessage / 256) & 0xFF);
            MIDIStatus = (byte)(MIDIMessage & 0xFF);

            //Fire the event on the class instatiating thread.
            if (!disposed && syncContext != null)
            {
                syncContext.Send(state =>
                {
                    FireShortMessage(MIDIStatus, MIDIData1, MIDIData2);
                }, null);
            }
        }
	}
}
