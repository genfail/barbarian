using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian.Midi
{
	public class ConnectionMidi
	{
		public event EventHandler<EventArgs> OnConnectionStateChanged; 

		protected MidiConnectionState __connectState = MidiConnectionState.Connected;
		public MidiConnectionState ConnectState { get{return __connectState; }}

		public ConnectionMidiIn In { get; private set;}
		public ConnectionMidiOut Out { get; private set;}

		public ConnectionMidi()
		{
			In = new ConnectionMidiIn();
			Out = new ConnectionMidiOut();
		}
		public void Init()
		{
			In.Init();
			Out.Init();

			In.OnConnectionStateChanged += Connection_ConnectionChanged;
			Out.OnConnectionStateChanged += Connection_ConnectionChanged;

		}

		public void Connect()
		{
			if (!In.Connected)
				In.Connect();
			if (!Out.Connected)
				Out.Connect();
		}
		
		public void Disconnect()
		{
			if (In.Connected)
				In.Disconnect();
			if (Out.Connected)
				Out.Disconnect();
		}
		public void Shutdown()
		{
			In.Shutdown();
			Out.Shutdown();
		}
		
		public void ToggleConnect()
		{
			In.ToggleConnect();
			Out.ToggleConnect();
		}

		private void Connection_ConnectionChanged(object sender, EventArgs e)
		{
			if (In.ConnectState == MidiConnectionState.Connected && Out.ConnectState == MidiConnectionState.Connected)
				SetState(MidiConnectionState.Connected);
			else if (In.ConnectState == MidiConnectionState.Available && Out.ConnectState == MidiConnectionState.Available)
				SetState(MidiConnectionState.Available);
			else
				SetState(MidiConnectionState.Unavailable);
		}

		public string DeviceConnectedText { get; private set;} 

		protected void SetState(MidiConnectionState _connState)
		{
			if(__connectState != _connState)
			{
				__connectState = _connState;

				switch (__connectState)
				{
					case MidiConnectionState.Unavailable:
						DeviceConnectedText = $"GR55 Not available";
						break;
					case MidiConnectionState.Available:
						DeviceConnectedText = $"GR55 Available";
						break;
					case MidiConnectionState.Connected:
						DeviceConnectedText = $"GR55 Connected";
						break;
					default:
						DeviceConnectedText = $"GR55 unknown state: {__connectState}";
						break;
				}
				OnConnectionStateChanged?.Invoke(this, new EventArgs());
			}
		}

		public void SetSynchronizationContext()
		{
			In.SetSynchronizationContext();
			Out.SetSynchronizationContext();
		}
	}
}
