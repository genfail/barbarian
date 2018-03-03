using System;
using System.Windows.Forms;

namespace GF.Barbarian
{
	static class Program
	{
		public static ApplicationSettings AppSettings { get;  set;}
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
			new SingleInstanceApp().Run(Environment.GetCommandLineArgs());
			AppData.Shutdown();
		}
	}
}
