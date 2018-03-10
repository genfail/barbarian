using System;
using System.Collections.Generic;
using GF.Lib.Global;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GF.Barbarian.Midi;

namespace GF.Barbarian
{
	public partial class FrmMain : Form
	{
		private ProgramMode mode = ProgramMode.File;
		private Dictionary<ProgramMode,ICtrlMode> modes = null;
		private ICtrlMode activeControl { get{ return modes[mode];} }

		public FrmMain()
		{
			Program.Mainform = this;
			InitializeComponent();
			modes = new Dictionary<ProgramMode, ICtrlMode>();
			modes.Add(ProgramMode.File, new CtrlModeFile());
			modes.Add(ProgramMode.Library, new CtrlModeLibrary());
			SetMode(Program.AppSettings.Mode);
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			Program.Midi.SetSynchronizationContext();
			Program.Midi.OnConnectionStateChanged += Connection_ConnectionChanged;

			Program.Midi.In.MessageReceived += Midi_MessageReceived;
			Program.Midi.In.ReceiveError += Midi_ReceiveError;
			Program.Midi.Out.MessageReceived += Midi_MessageReceived;
			Program.Midi.Out.ReceiveError += Midi_ReceiveError;

			Connection_ConnectionChanged(null, null); // Fire to set first time
			SetMessage(MSgSeverity.Message, "Application started");

			if (Properties.Settings.Default.AutoConnect)
				Program.Midi.Connect();
		}

		private void Midi_ReceiveError(object sender, Lib.Communication.Midi.MidiErrorEventArgs e)
		{
			Debug.WriteLine($"--> {e.DeviceType,7} ERR: [{e.MidiException}]");
			SetMessage(MSgSeverity.Error, e.MidiException.ToString());
		}

		private void Midi_MessageReceived(object sender, Lib.Communication.Midi.MidiMsgEventArgs e)
		{
			Debug.WriteLine($"--> {e.DeviceType,7} MSG:  [{e.MsgText}]");
			SetMessage(MSgSeverity.Message, e.MsgText);
		}

		public void SetMessage(MSgSeverity _sev, string _msg)
		{
			toolStripStatusMessage.Text = _msg;
			toolStripStatusMessage.Image = Global.GetMSgSeverityIcon(_sev);
		}

		private void Connection_ConnectionChanged(object sender, EventArgs e)
		{
			toolStripStatusMidi.Text = Program.Midi.DeviceConnectedText;
			toolStripStatusMidi.Image = Global.GetConnectedIcon(Program.Midi.ConnectState);
		}

		public void ApplySettings()
		{
			activeControl.ApplySettings();
		}

		public void SetMode(ProgramMode _mode)
		{
			mode = _mode;

			// remove previous
			List<ICtrlMode> lst = panelMain.FindAllChildrenByType<ICtrlMode>().ToList();
			foreach (Control c in lst)
			{
				panelMain.Controls.Remove(c);
			}

			// add the new one
			((Control)activeControl).Location = panelMain.Location;// new System.Drawing.Point(177, 51);
			((Control)activeControl).Size = panelMain.Size; // new System.Drawing.Size(624, 582);
			((Control)activeControl).Name = "ctrlMode" + activeControl.GetType().ToString();
			((Control)activeControl).TabIndex = 1;
			((Control)activeControl).BackColor = Color.DarkGray;
			((Control)activeControl).Dock = DockStyle.Fill;
			panelMain.Controls.Add((Control)activeControl);
		}

	#region Menu File
		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileModeToolStripMenuItem.Checked = (activeControl is CtrlModeFile);
			libraryModeToolStripMenuItem.Checked = (activeControl is CtrlModeLibrary);
		}
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FrmSettings frm = new FrmSettings();
			frm.StartPosition = FormStartPosition.CenterParent;
			frm.ShowDialog(this);
		}
		private void fileModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetMode(ProgramMode.File);
		}

		private void libraryModeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetMode(ProgramMode.Library);
		}
		private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion
		#region Menu Tools
		private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			connectionToolStripMenuItem.Enabled = Program.Midi.ConnectState != MidiConnectionState.Unavailable;
			connectionToolStripMenuItem.Checked = Program.Midi.ConnectState == MidiConnectionState.Connected;

			switch (Program.Midi.ConnectState)
			{
				case MidiConnectionState.Unavailable: connectionToolStripMenuItem.Text = "Connection unavailable";		break;
				case MidiConnectionState.Available:   connectionToolStripMenuItem.Text = "Open connection";				break;
				case MidiConnectionState.Connected:   connectionToolStripMenuItem.Text = "Close connection";			break;
				default:
					connectionToolStripMenuItem.Text = "Connection unknown"; break;
			}
		}
		#endregion

		private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.Midi.ToggleConnect();
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.Midi.Disconnect();
			Program.Midi.Shutdown();
			Properties.Settings.Default.LastProgramMode = (int)mode;
			foreach (ICtrlMode c in modes.Values)
			{
				c.SaveSettings();
			}
			Properties.Settings.Default.Save();
		}

	}
}
