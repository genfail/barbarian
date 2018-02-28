using GF.Lib.Communication.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GF.Barbarian
{
	public class ConnectionManager
	{
		private string connectedDevice = "";
		public string ConnectedDevice { get => connectedDevice; set => connectedDevice = value; }

		public event EventHandler<EventArgs> ConnectionChanged; 
		private bool __connected = false;
		public bool Connected { get{return __connected; }}


		private void SetConnected(bool _connected, string _conStr = null)
		{
			if(__connected != _connected)
			{
				__connected = _connected;
				ConnectedDevice = _connected ? _conStr : "Not connected";						
				ConnectionChanged?.Invoke(this, new EventArgs());
			}
		}

		private bool running = false;

		public ConnectionManager()
		{
		}

		public void Init()
		{
			if (running)
				return; // nothing to do

			Task task = new Task(new Action(CheckConnected));
			task.Start();
		}

		public void Shutdown()
		{
			running = false;
		}

		private void CheckConnected()
		{
			int cnt = 10;
			running = true;
			while(running)
			{
				if (cnt-- <= 0)
				{
					cnt = 10;
					string[] devices = MidiCommands.GetDeviceNamesWaveIn().Where(s => s.Contains("GR-55")).ToArray();
					if (devices != null && (devices.Count() > 0) )
						SetConnected(true, devices[0]);
					else
						SetConnected(false);
				}
				Thread.Sleep(100);
			}
		}
	}
}
