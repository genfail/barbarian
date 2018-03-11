using GF.Lib.Communication.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		Undefined,
		Uncertain,
		Unavailable,
		Available,
		Connected,
	}

	public abstract class ConnectionMidiBase
	{
		protected abstract bool OpenPort(uint deviceNumber);
		protected abstract bool ClosePort();
		public abstract uint GetDevices(out uint countDevices, out string[] deviceList);
		public abstract uint GetDeviceCount();

		protected bool threadRunning = false;
		protected bool threadStopped = false;

		// Needed for midi interaction
        protected SynchronizationContext syncContext = null;
		protected int mInBufferLength;
		protected IntPtr DataBufferPointer;

		protected DeviceTypeMidi MidiDeviceType = DeviceTypeMidi.MidiIn;


		/// <summary> Updates status changes to the MIDI in port.</summary>
        public event MessageInDel MessageReceived;
		public void FireMessageReceived(string message)
		{
			if (MessageReceived != null)
                MessageReceived(this, new MidiMsgEventArgs(MidiDeviceType, message));
		}

		/// <summary> Short message received. MIDI status and parameter data are specified as parameters. Port is automatically reset to receive next message. </summary>
		public event ShortDel ShortMessage;
		public void FireShortMessage(byte MIDIStatus, byte MIDIData1, byte MIDIData2)
		{
			if (ShortMessage != null)
                ShortMessage(this, new MidiShortMsgEventArgs(MIDIStatus, MIDIData1, MIDIData2));
		}

		/// <summary>Long message received. MIDI system exclusive data is sent as byte array parameter. Port is automatically reset to receive next message. </summary>
		public event LongDel LongMessage;
		public void FireLongMessage(byte[] b)
		{
			if (LongMessage != null)
                LongMessage(this, new MidiLongMsgEventArgs(b));
		}

		/// <summary> Any Exceptions occurring within the class are sent to the instantiating class via this event. </summary>
		public event ReceiveErrorDel ReceiveError;
		public void FireReceiveError(Exception ex)
		{
			if (ReceiveError != null)
                ReceiveError(this, new MidiErrorEventArgs(MidiDeviceType, ex));
		}

		public bool DeviceAvailable { get { return gr55DeviceId != -1; } }
		protected int gr55DeviceId = -1;
		protected string gr55DeviceName = "";
		public string DeviceConnectedText { get; private set;} 

		public event EventHandler<EventArgs> OnConnectionStateChanged; 

		protected MidiConnectionState __connectState = MidiConnectionState.Connected;
		public MidiConnectionState ConnectState { get{return __connectState; }}

		protected void SetState(MidiConnectionState _connState)
		{
			if(__connectState != _connState)
			{
				__connectState = _connState;

				switch (__connectState)
				{
					case MidiConnectionState.Unavailable:
						DeviceConnectedText = $"GR55 {MidiDeviceType} Not available";
						break;
					case MidiConnectionState.Available:
						DeviceConnectedText = $"GR55 {MidiDeviceType} Available";
						break;
					case MidiConnectionState.Connected:
						DeviceConnectedText = $"GR55 {MidiDeviceType} Connected";
						break;
					default:
						DeviceConnectedText = $"GR55 {MidiDeviceType} unknown state: {__connectState}";
						break;
				}
				OnConnectionStateChanged?.Invoke(this, new EventArgs());
			}
		}

		public ConnectionMidiBase()
		{
			SetState(MidiConnectionState.Unavailable);
			if (SynchronizationContext.Current != null)
				syncContext = SynchronizationContext.Current;
			else
				Debug.WriteLine("syncContext null, no callbacks possible");
			mInBufferLength = 256;
		}

		public void SetSynchronizationContext()
		{
			if (SynchronizationContext.Current != null)
				syncContext = SynchronizationContext.Current;
			else
				Debug.WriteLine("syncContext null, no callbacks possible");
		}

		#region Dispose
        ~ConnectionMidiBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected bool disposed = false;
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

		protected bool mPortOpen = false;
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

		private Task task = null;
		public void Init()
		{
			if (threadRunning)
				return; // nothing to do

			
			IsConnetionAvailableChanged(); // Call once to set values

			task = new Task(new Action(ProcessCheckConnected));
			task.Start();
		}

		public void Shutdown()
		{
			if (DeviceAvailable && Connected)
				Disconnect();

			threadRunning = false;
			int cnt = 50;
			while(!threadStopped && cnt-- > 0)
			{
				Thread.Sleep(50);
			}
			Debug.WriteLine($"stopped {cnt}");
		}

		private void ProcessCheckConnected()
		{
			int cnt = 10;
			threadRunning = true;
			threadStopped = false;
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
			threadStopped = true;
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
			uint midiErrorNumber = (uint)MMSYSERR.MMSYSERR_NOERROR;
			bool hasChanged = false;
			bool hasFound = false;

			midiErrorNumber= GetDevices(out uint countDevices, out string[] deviceList);
				
			if (midiErrorNumber == (uint)MMSYSERR.MMSYSERR_NOERROR && deviceList != null && countDevices > 0)
			{
				for (int i = 0; i < deviceList.Length; i++)
				{
					if (deviceList[i].Contains("GR-55"))
					{
						hasFound = true;
						if (gr55DeviceId != i)
						{
							gr55DeviceName = deviceList[i];
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

		protected void ErrorHandler(uint midiErrorNumber)
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
            if (!disposed && syncContext != null)
            {
                syncContext.Send(state =>
                {
                    if (ReceiveError != null)
                        ReceiveError(this, new MidiErrorEventArgs(MidiDeviceType, new Exception(strError)));
                }, null);
            }
        }
	}
}
