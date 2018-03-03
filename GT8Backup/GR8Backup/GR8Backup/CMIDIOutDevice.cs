using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MIDI
{
    delegate void MessageOutDel(object sender, MidiMsgEventArgs e);

    class CMIDIOutDevice : IDisposable
    {
        /// <summary>
        /// Updates status changes to the MIDI out port.
        /// </summary>
        public event MessageOutDel MessageReceived;

        private SynchronizationContext mSyncContext;
        private CExternals.MidiOutCallbackDel mMidiCall;
        private IntPtr mMIDIOutHandle;
        private bool mPortOpen;
        private bool disposed = false;

        /// <summary>
        /// To instantiate the class it is necessary to provide a System Exclusive message buffer length.
        /// </summary>
        /// <param name="outBufferLength">System Exclusive message buffer length.</param>
        public CMIDIOutDevice()
        {
            mSyncContext = SynchronizationContext.Current;
        }
        
        ~CMIDIOutDevice()
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

            if(disposing)
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
        /// Get a list of the available MIDI out devices on the PC. 
        /// </summary>
        /// <param name="strDevices">List of the human readable names of available devices. 
        /// The index of the list is used to select the desired device.</param>
        /// <returns>The function return value is the number of devices on the current PC.</returns>
        public uint ListDevices(out string[] deviceList)
        {
            CExternals.MIDIOUTCAPS typMIDIDevCaps = new CExternals.MIDIOUTCAPS();
            List<string> devices = new List<string>();
            uint lngReturn;
            uint lngNumOfDevices;
        
            //Get number of MIDI output devices installed.
            lngNumOfDevices = CExternals.midiOutGetNumDevs();
            if (lngNumOfDevices > 0)
            {
                //Loop through all devices and record the name.
                for (uint lngLoop = 0; lngLoop < lngNumOfDevices; lngLoop++)
                {
                    lngReturn = CExternals.midiOutGetDevCaps((UIntPtr)lngLoop, ref typMIDIDevCaps,
                        (uint)Marshal.SizeOf(typMIDIDevCaps));
                    if (lngReturn == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                    {
                        devices.Add(typMIDIDevCaps.szPname);
                    }
                    else
                    {   //An Error occurred
                        ErrorHandler(lngReturn);
                        break;
                    }
                }

                deviceList = devices.ToArray();
            }
            else
            {
                deviceList = null;
            }
            
            return lngNumOfDevices;
        }

        /// <summary>
        /// Open the MIDI out device specified
        /// </summary>
        /// <param name="lngDevice">MIDI out port to open. Index corresponds to port name list from ListDevices().</param>
        /// <returns>Returns true if the port can be opened. Otherwise returns false.</returns>
        public bool OpenPort(uint deviceNumber)
        {
            uint returnValue;
            bool openSuccess;

            mMidiCall = new CExternals.MidiOutCallbackDel(MidiCallback);

            returnValue = CExternals.midiOutOpen(out mMIDIOutHandle, deviceNumber, 
                mMidiCall, new IntPtr(), CExternals.CALLBACK_FUNCTION);
            if(returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
        public bool ClosePort() 
        {
            uint returnValue;
            bool closeSuccess;

            if (mPortOpen)
            {
                //Reset device so it will close correctly.
                returnValue = CExternals.midiOutReset(mMIDIOutHandle);
                if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                {
                    returnValue = CExternals.midiOutClose(mMIDIOutHandle);
                    if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
                case (int)CExternals.MOM.MOM_OPEN:
                    message = "MIDI Out, Open" + Environment.NewLine;
                    break;
                case (int)CExternals.MOM.MOM_CLOSE:
                    message = "MIDI Out, Close" + Environment.NewLine;
                    break;
                case (int)CExternals.MOM.MOM_DONE:
                    message = "MIDI Out, Done" + Environment.NewLine;
                    break;
                default:
                    message = "Unknown Message Recieved!" + Environment.NewLine;
                    break;
            }

            //Fire the event on the class instatiating thread.
            if (!disposed)
            {
                mSyncContext.Send(state =>
                {
                    if (MessageReceived != null)
                        MessageReceived(this, new MidiMsgEventArgs(message));
                }, null);
            }
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
                returnValue = CExternals.midiOutShortMsg(mMIDIOutHandle, MIDIMessage);
                if (returnValue == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
            CExternals.MIDIHDR typMsgHeader = new CExternals.MIDIHDR();
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
                    lngReturn = CExternals.midiOutPrepareHeader(mMIDIOutHandle, DataBufferPointer,
                        (uint)Marshal.SizeOf(typMsgHeader));
                    if (lngReturn == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
                    {
                        lngReturn = (uint)CExternals.midiOutLongMsg(mMIDIOutHandle, DataBufferPointer,
                            (uint)Marshal.SizeOf(typMsgHeader));
                        if (lngReturn == (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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
                    lngReturn = CExternals.midiOutUnprepareHeader(mMIDIOutHandle, DataBufferPointer,
                        (uint)Marshal.SizeOf(typMsgHeader));
                    if (lngReturn != (uint)CExternals.MMSYSERR.MMSYSERR_NOERROR)
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

        private void ErrorHandler(uint midiErrorNumber)
        {
            string errorMsg;

            switch(midiErrorNumber)
            {
                case (uint)CExternals.MIDIERR.MIDIERR_NODEVICE:
                    errorMsg = "No Device";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_ALLOCATED:
                    errorMsg = "Allocated";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_BADDEVICEID:
                    errorMsg = "Bad Device ID";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_INVALPARAM:
                    errorMsg = "Invalid Parameter";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_NODRIVER:
                    errorMsg = "No Driver";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_NOMEM:
                    errorMsg = "No Mem";
                    break;
                case (uint)CExternals.MMSYSERR.MMSYSERR_INVALHANDLE:
                    errorMsg = "Invalid Handle";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_BADOPENMODE:
                    errorMsg = "Bad Open Mode";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_NOTREADY:
                    errorMsg = "Not Ready";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_UNPREPARED:
                    errorMsg = "Unprepared";
                    break;
                case (uint)CExternals.MIDIERR.MIDIERR_STILLPLAYING:
                    errorMsg = "Still Playing";
                    break;
                default:
                    errorMsg = "Unknown Error";
                    break;
            }
            throw (new Exception(errorMsg));
        }
    }
}
