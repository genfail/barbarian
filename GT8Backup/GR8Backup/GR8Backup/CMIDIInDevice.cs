using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MIDI
{
    delegate void MessageInDel(object sender, MidiMsgEventArgs e);
    delegate void ShortDel(object sender, MidiShortMsgEventArgs e);
    delegate void LongDel(object sender, MidiLongMsgEventArgs e);
    delegate void ReceiveErrorDel(object sender, MidiErrorEventArgs e);
    
    class CMIDIInDevice
    {
        /// <summary>
        /// Updates status changes to the MIDI in port.
        /// </summary>
        public event MessageInDel MessageReceived;
        /// <summary>
        /// Short message received. MIDI status and parameter data are specified as parameters. 
        /// Port is automatically reset to receive next message. 
        /// </summary>
        public event ShortDel ShortMessage;
        /// <summary>
        /// Long message received. MIDI system exclusive data is sent as byte array parameter. 
        /// Port is automatically reset to receive next message.
        /// </summary>
        public event LongDel LongMessage;
        /// <summary>
        /// Any Exceptions occurring within the class are sent to the instantiating class via this event.
        /// </summary>
        public event ReceiveErrorDel ReceiveError;

        private SynchronizationContext syncContext;
        private CExternals.MidiInCallbackDel MidiCall;
        private IntPtr MIDIInHandle;
        private IntPtr DataBufferPointer;
        private bool mPortOpen;
        private int mInBufferLength;
        private bool disposed = false;

        /// <summary>
        /// To instantiate the class it is necessary to provide a System Exclusive message buffer length.
        /// </summary>
        /// <param name="inBufferLength">System Exclusive message buffer length.</param>
        public CMIDIInDevice(int inBufferLength)
        {
            syncContext = SynchronizationContext.Current;
            mInBufferLength = inBufferLength;
        }
        
        ~CMIDIInDevice()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

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

        public bool PortOpen
        {
            get
            {
                return mPortOpen;
            }
        }

        /// <summary>
        /// Get a list of the available MIDI in devices on the PC.
        /// </summary>
        /// <param name="deviceList">The index of the list is used to select the desired device.
        /// The index of the list is used to select the desired device.</param>
        /// <returns>The function return value is the number of devices on the current PC.</returns>
        public uint ListDevices(out string[] deviceList)
        {
            CExternals.MIDIINCAPS MyMIDIInDevCaps = new CExternals.MIDIINCAPS();
            uint returnValue, lngNumOfDevices;
            List<string> devices = new List<string>();

            //Get the number of MIDI in devices installed.
            lngNumOfDevices = CExternals.midiInGetNumDevs();
            if(lngNumOfDevices > 0) 
            {
                //Loop through all devices and store the device names.
                for(uint intLoop=0;intLoop<lngNumOfDevices;intLoop++)
                {
                    returnValue = (uint)CExternals.midiInGetDevCaps((UIntPtr)intLoop, ref MyMIDIInDevCaps, (uint)Marshal.SizeOf(MyMIDIInDevCaps));
                    if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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

        /// <summary>
        /// Open the MIDI in device specified
        /// </summary>
        /// <param name="deviceNumber">MIDI in port to open. Index corresponds to port name list from ListDevices().</param>
        /// <returns>Returns true if the port can be opened. Otherwise returns false.</returns>
        public bool OpenPort(uint deviceNumber)
        {
            uint returnValue;
            bool startSuccess, openSuccess;

            MidiCall = new CExternals.MidiInCallbackDel(MidiCallback);

            returnValue = CExternals.midiInOpen(out MIDIInHandle, deviceNumber, 
                MidiCall, new IntPtr(), (uint)CExternals.CALLBACK_FUNCTION);
            if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
        public bool ClosePort()
        {
            uint returnValue = 0;
            bool stopSuccess, closeSuccess = false;

            if(mPortOpen)
            {
                mPortOpen = false;

                stopSuccess = StopRecording();
                if (stopSuccess)
                {
                    returnValue = CExternals.midiInClose(MIDIInHandle);
                    if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
                case (int)CExternals.MIM.MM_MIM_OPEN:
                    message = "MIDI In, Open" + Environment.NewLine;
                    break;
                case (int)CExternals.MIM.MM_MIM_CLOSE:
                    message = "MIDI In, Close" + Environment.NewLine;
                    break;
                case (int)CExternals.MIM.MM_MIM_DATA:
                    ShortMessageReceived((uint)param1);
                    statusMsg = false;
                    break;
                case (int)CExternals.MIM.MM_MIM_LONGDATA:
                    LongMessageReceived();
                    statusMsg = false;
                    break;
                case (int)CExternals.MIM.MM_MIM_ERROR:
                    message = "MIDI In, Error" + Environment.NewLine;
                    break;
                case (int)CExternals.MIM.MM_MIM_LONGERROR:
                    message = "MIDI In, Long Error" + Environment.NewLine;
                    break;
                case (int)CExternals.MIM.MM_MIM_MOREDATA:
                    message = "MIDI In, More Data" + Environment.NewLine;
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
            CExternals.MIDIHDR InHeader = new CExternals.MIDIHDR();
            byte[] MIDIInBuffer = new byte[mInBufferLength];

            if(mPortOpen)
            {
                //Stop recording while processing the current data.
                tempResult = StopRecording();

                InHeader = (CExternals.MIDIHDR) Marshal.PtrToStructure(DataBufferPointer, typeof(CExternals.MIDIHDR));
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
            CExternals.MIDIHDR InHeader = new CExternals.MIDIHDR();
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
            tempReturn = CExternals.midiInPrepareHeader(MIDIInHandle, DataBufferPointer, 
                (uint)Marshal.SizeOf(new CExternals.MIDIHDR()));
            if(tempReturn==(uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
            {
                tempReturn = CExternals.midiInAddBuffer(MIDIInHandle, DataBufferPointer, 
                    (uint)Marshal.SizeOf(new CExternals.MIDIHDR()));
                if(tempReturn!=(uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                {
                    ErrorHandler(tempReturn);
                }
            }
            else
            {
                ErrorHandler(tempReturn);
            }

            tempReturn = CExternals.midiInStart(MIDIInHandle);
            if(tempReturn==(uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
            CExternals.MIDIHDR tempHeader;

            stopSuccess = false;
            tempReturn = CExternals.midiInStop(MIDIInHandle);
            if(tempReturn==(uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
            {
                tempReturn = CExternals.midiInReset(MIDIInHandle);
                if(tempReturn==(uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                {
                    //Unprepare header so it can be unassigned.
                    tempReturn = CExternals.midiInUnprepareHeader(MIDIInHandle, DataBufferPointer, 
                        (uint)Marshal.SizeOf(new CExternals.MIDIHDR()));
                    if (tempReturn == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                    {
                        tempHeader = (CExternals.MIDIHDR)Marshal.PtrToStructure(DataBufferPointer, typeof(CExternals.MIDIHDR));
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
                case (uint)CExternals.MMSYSERR.MMSYSERR_ALLOCATED:
                    strError = "Allocated";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_BADDEVICEID:
                    strError = "Bad Device ID";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_INVALFLAG:
                    strError = "Invalid Flag";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_INVALPARAM:
                    strError = "Invalid Parameter";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_NODRIVER:
                    strError = "No Driver";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_NOMEM:
                    strError = "No Mem";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_INVALHANDLE:
                    strError = "Invalid Handle";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_STILLPLAYING:
                    strError = "Still Playing";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_UNPREPARED:
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
