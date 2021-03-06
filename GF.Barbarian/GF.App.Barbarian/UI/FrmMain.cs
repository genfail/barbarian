﻿using System;
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
using System.IO;
using GongSolutions.Shell;

namespace GF.Barbarian
{
	public partial class FrmMain : Form
	{
		private ProgramMode mode = ProgramMode.File;
		private Dictionary<ProgramMode,ICtrlMode> modes = null;
		public ICtrlMode ActiveModeControl { get{ return modes[mode];} }

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
			SetPatchName();

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

		public void SetPatchName(string _name = null)
		{
			if (String.IsNullOrEmpty(_name))
				toolStripLoadedPatchName.Text = "No patch loaded";
			else
				toolStripLoadedPatchName.Text = "Loaded:" + _name;
		}

		public void SetMessage(MSgSeverity _sev, string _msg)
		{
			toolStripStatusMessage.Text = _msg;
			toolStripStatusMessage.Image = Global.GetMSgSeverityIcon(_sev);
		}

		private void Connection_ConnectionChanged(object sender, EventArgs e)
		{
			if (!IsDisposed)
			{
				try
				{
					toolStripStatusMidi.Text = Program.Midi.DeviceConnectedText;
					toolStripStatusMidi.Image = Global.GetConnectedIcon(Program.Midi.ConnectState);
				}
				catch (Exception)
				{
					Debug.WriteLine("Can not set status");
				}
			}
		}

		public void ApplySettings()
		{
			ActiveModeControl.ApplySettings();
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
			((Control)ActiveModeControl).Location = panelMain.Location;// new System.Drawing.Point(177, 51);
			((Control)ActiveModeControl).Size = panelMain.Size; // new System.Drawing.Size(624, 582);
			((Control)ActiveModeControl).Name = "ctrlMode" + ActiveModeControl.GetType().ToString();
			((Control)ActiveModeControl).TabIndex = 1;
			((Control)ActiveModeControl).BackColor = Color.DarkGray;
			((Control)ActiveModeControl).Dock = DockStyle.Fill;
			panelMain.Controls.Add((Control)ActiveModeControl);
		}

		#region Menu File
		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			fileModeToolStripMenuItem.Checked = (ActiveModeControl is CtrlModeFile);
			libraryModeToolStripMenuItem.Checked = (ActiveModeControl is CtrlModeLibrary);
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

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FrmAbout frm = new FrmAbout();
			frm.ShowDialog(this);
		}

		private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] locations = new []
			{
				Path.Combine(Application.StartupPath, ".\\Html\\Introduction.html"),// normal user
				Path.Combine(Application.StartupPath, "..\\..\\..\\Help\\Html\\Introduction.html")// Developer
			};

			foreach (string l in locations)
			{
				if (File.Exists(l))
				{
					System.Diagnostics.Process.Start(l);
					return;
				}
			}
		}
	}
}
