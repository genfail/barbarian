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
		public ConnectionMidiIn MidiIn { get; }

		private ProgramMode mode = ProgramMode.File;
		private Dictionary<ProgramMode,ICtrlMode> modes = null;
		private ICtrlMode activeControl { get{ return modes[mode];} }

		public FrmMain()
		{
			MidiIn = new ConnectionMidiIn();
			MidiIn.Init();

			InitializeComponent();
			modes = new Dictionary<ProgramMode, ICtrlMode>();
			modes.Add(ProgramMode.File, new CtrlModeFile());
			modes.Add(ProgramMode.Library, new CtrlModeLibrary());
			SetMode(Program.AppSettings.Mode);

			MidiIn.MessageReceived += MidiIn_MessageReceived;
			MidiIn.ReceiveError += MidiIn_ReceiveError;
		}

		private void MidiIn_ReceiveError(object sender, Lib.Communication.Midi.MidiErrorEventArgs e)
		{
			Debug.WriteLine($"MidiIn ERR: [{e.MidiException}]");
			SetMessage(MSgSeverity.Error, e.MidiException.ToString());
		}

		private void MidiIn_MessageReceived(object sender, Lib.Communication.Midi.MidiMsgEventArgs e)
		{
			Debug.WriteLine($"MidiIn MSG:  [{e.MsgText}]");

			SetMessage(MSgSeverity.Message, e.MsgText);
		}

		public void SetMessage(MSgSeverity _sev, string _msg)
		{
			toolStripStatusMessage.Text = _msg;
			toolStripStatusMessage.Image = Global.GetMSgSeverityIcon(_sev);
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			MidiIn.OnConnectionStateChanged += Connection_ConnectionChanged;
			Connection_ConnectionChanged(null, null);
			SetMessage(MSgSeverity.Message, "Application started");
		}

		private void Connection_ConnectionChanged(object sender, EventArgs e)
		{
			toolStripStatusMidi.Text = MidiIn.DeviceConnectedText;
			toolStripStatusMidi.Image = Global.GetConnectedIcon(MidiIn.ConnectState);
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

	#region Menu
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

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			MidiIn.Shutdown();
			Properties.Settings.Default.LastProgramMode = (int)mode;
			foreach (ICtrlMode c in modes.Values)
			{
				c.SaveSettings();
			}
			Properties.Settings.Default.Save();
		}

		private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			connectionToolStripMenuItem.Enabled = MidiIn.ConnectState != MidiConnectionState.Unavailable;
			connectionToolStripMenuItem.Checked = MidiIn.ConnectState == MidiConnectionState.Connected;

			switch (MidiIn.ConnectState)
			{
				case MidiConnectionState.Unavailable: connectionToolStripMenuItem.Text = "Connection unavailable";		break;
				case MidiConnectionState.Available:   connectionToolStripMenuItem.Text = "Open connection";				break;
				case MidiConnectionState.Connected:   connectionToolStripMenuItem.Text = "Close connection";			break;
				default:
					connectionToolStripMenuItem.Text = "Connection unknown"; break;
			}
		}

		private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MidiIn.ToggleConnect();

		}
	}
}
