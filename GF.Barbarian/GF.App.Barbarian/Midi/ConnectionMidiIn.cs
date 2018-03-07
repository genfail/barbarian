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
	public enum MidiConnectionState
	{
		Unavailable,
		Available,
		Connected,
	}

	public class ConnectionMidiIn
	{
		private bool threadRunning = false;

		// Needed for midi interaction
        private SynchronizationContext syncContext;
		private MidiInCallbackDel MidiCall;
		private IntPtr MIDIInHandle;
		private int mInBufferLength;
		private IntPtr DataBufferPointer;

		/// <summary> Updates status changes to the MIDI in port.</summary>
        public event MessageInDel MessageReceived;
		/// <summary> Short message received. MIDI status and parameter data are specified as parameters. Port is automatically reset to receive next message. </summary>
		public event ShortDel ShortMessage;
		/// <summary>Long message received. MIDI system exclusive data is sent as byte array parameter. Port is automatically reset to receive next message. </summary>
		public event LongDel LongMessage;
		/// <summary> Any Exceptions occurring within the class are sent to the instantiating class via this event. </summary>
		public event ReceiveErrorDel ReceiveError;


		public bool DeviceAvailable { get { return gr55DeviceId != -1; } }
		private int gr55DeviceId = -1;
		private string gr55DeviceName = "";
		public string DeviceConnectedText { get; private set;} 

		public event EventHandler<EventArgs> OnConnectionStateChanged; 

		private MidiConnectionState __connectState = MidiConnectionState.Connected;
		public MidiConnectionState ConnectState { get{return __connectState; }}

		private void SetState(MidiConnectionState _connState)
		{
			if(__connectState != _connState)
			{
				__connectState = _connState;

				switch (__connectState)
				{
					case MidiConnectionState.Unavailable:
						DeviceConnectedText = "GR55 Not available";
						break;
					case MidiConnectionState.Available:
						DeviceConnectedText = "GR55 Available";
						break;
					case MidiConnectionState.Connected:
						DeviceConnectedText = "GR55 Connected";
						break;
					default:
						break;
				}
				OnConnectionStateChanged?.Invoke(this, new EventArgs());
			}
		}

		public ConnectionMidiIn()
		{
			SetState(MidiConnectionState.Unavailable);
			syncContext = SynchronizationContext.Current;
			mInBufferLength = 256;
		}

		#region Dispose
        ~ConnectionMidiIn()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;

            if (disposing)
            {
                //Free any managed objects here.
            }

            //Free unmanaged objects here;
            ClosePort();
        }
		#endregion

		private bool mPortOpen = false;
		public bool Connected { get{ return mPortOpen; } }
		public void Connect()
		{
			if (DeviceAvailable && !Connected)
				OpenPort((uint)gr55DeviceId);
		}
		public void Disconnect()
		{
			ClosePort();
		}
		public void ToggleConnect()
		{
			if (DeviceAvailable && !Connected)
				Connect();
			else if (DeviceAvailable && Connected)
				Disconnect();
		}

		public void Init()
		{
			if (threadRunning)
				return; // nothing to do

			Task task = new Task(new Action(CheckConnected));
			task.Start();
		}

		public void Shutdown()
		{
			threadRunning = false;
		}

		private void CheckConnected()
		{
			int cnt = 10;
			threadRunning = true;
			while(threadRunning)
			{
				if (cnt-- <= 0)
				{
					cnt = 10;

					bool hasChanged = IsConnetionAvailableChanged();
					CheckConnetionOpen();
				}
				Thread.Sleep(100);
			}
		}

		private bool OpenPort(uint deviceNumber)
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

        /// <summary>
        /// Close the currently open port. If no port is open then this method does nothing.
        /// </summary>
        /// <returns>Returns true is the port is closed correctly. Otherwise returns false.</returns>
        private bool ClosePort()
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


       private void MidiCallback(int handle, int msg, int instance, int param1, int param2)
        {
            string message = "";
            bool statusMsg;

            statusMsg = true;
            switch (msg)
            {
                case (int)MIM.MM_MIM_OPEN:
                    message = "MIDI In, Open";
                    break;
                case (int)MIM.MM_MIM_CLOSE:
                    message = "MIDI In, Close";
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
                    message = "MIDI In, Error";
                    break;
                case (int)MIM.MM_MIM_LONGERROR:
                    message = "MIDI In, Long Error";
                    break;
                case (int)MIM.MM_MIM_MOREDATA:
                    message = "MIDI In, More Data";
                    break;
                default:
                    message = "Unknown Message Recieved!";
                    break;
            }

           if (statusMsg)
            {
                //Fire the event on the class instatiating thread.
                if (!disposed)
                {
                    syncContext.Send(state =>
                    {
                        if (MessageReceived != null)
                            MessageReceived(this, new MidiMsgEventArgs(message));
                    }, null);
                }
            }
        }

		private void CheckConnetionOpen()
		{
			if (DeviceAvailable)
			{
				if (Connected)
					SetState(MidiConnectionState.Connected);
				else
					SetState(MidiConnectionState.Available);
			}
			else
			{
				SetState(MidiConnectionState.Unavailable);
				Disconnect();
			}
		}

		// Returns true if something has changed
		private bool IsConnetionAvailableChanged()
		{
			bool hasChanged = false;
			bool hasFound = false;
			string[] devices;
			if (ListDevicesMidiIn(out devices) > 0)
			{
				for (int i = 0; i < devices.Length; i++)
				{
					if (devices[i].Contains("GR-55"))
					{
						hasFound = true;
						if (gr55DeviceId != i)
						{
							gr55DeviceName = devices[i];
							gr55DeviceId = i;
							hasChanged = true;
							break;
						}
					}
				}
			}
			if (!hasFound)
			{
				if (gr55DeviceId != -1)
				{
					gr55DeviceName = "";
					gr55DeviceId = -1;
					hasChanged = true;
				}
			}
			return hasChanged;
		}

//---------------------
		private uint ListDevicesMidiIn(out string[] deviceList)
        {
            MIDIINCAPS MyMIDIInDevCaps = new MIDIINCAPS();
            uint returnValue, lngNumOfDevices;
            List<string> devices = new List<string>();

            //Get the number of MIDI in devices installed.
            lngNumOfDevices = MidiCommands.midiInGetNumDevs();
            if(lngNumOfDevices > 0) 
            {
                //Loop through all devices and store the device names.
                for(uint intLoop=0;intLoop<lngNumOfDevices;intLoop++)
                {
                    returnValue = (uint)MidiCommands.midiInGetDevCaps((UIntPtr)intLoop, ref MyMIDIInDevCaps, (uint)Marshal.SizeOf(MyMIDIInDevCaps));
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

		private void ShortMessageReceived(uint MIDIMessage)
        {
            byte MIDIStatus;
            byte MIDIData1, MIDIData2;

            MIDIData2 = (byte)((MIDIMessage / 65536) & 0xFF);
            MIDIData1 = (byte)((MIDIMessage / 256) & 0xFF);
            MIDIStatus = (byte)(MIDIMessage & 0xFF);

            //Fire the event on the class instatiating thread.
            if (!disposed)
            {
                syncContext.Send(state =>
                {
                    if (ShortMessage != null)
                        ShortMessage(this, new MidiShortMsgEventArgs(MIDIStatus, MIDIData1, MIDIData2));
                }, null);
            }
        }

        private void LongMessageReceived()
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
                    if (!disposed)
                    {
                        syncContext.Send(state =>
                        {
                            if (LongMessage != null)
                                LongMessage(this, new MidiLongMsgEventArgs(MIDIInBuffer));
                        }, null);
                    }
                }

                if (mPortOpen)
                {
                    tempResult = StartRecording();
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
                if (!disposed)
                {
                    syncContext.Send(state =>
                    {
                        if (ReceiveError != null)
                            ReceiveError(this, new MidiErrorEventArgs(ex));
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

        private void ErrorHandler(uint midiErrorNumber)
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

            //Send error to instatiating thread via event.
            if (!disposed)
            {
                syncContext.Send(state =>
                {
                    if (ReceiveError != null)
                        ReceiveError(this, new MidiErrorEventArgs(new Exception(strError)));
                }, null);
            }
        }
	}
}
