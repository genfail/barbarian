using GF.Barbarian.Midi;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GF.Barbarian
{
	static class Program
	{
		public static FrmMain Mainform { get; set; }
		public static ConnectionMidi Midi { get; private set; }
		public static ApplicationSettings AppSettings { get; set;}
		public static ApplicationData AppData { get; private set;}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			AppData = new ApplicationData();
			AppData.Init();

			Midi = new ConnectionMidi();
			Midi.Init();

			new SingleInstanceApp().Run(Environment.GetCommandLineArgs());

			Midi.Shutdown();
			AppData.Shutdown();
		}
	}
}
