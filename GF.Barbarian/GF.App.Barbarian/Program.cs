using System;
using System.Windows.Forms;

namespace GF.Barbarian
{
	static class Program
	{
		public static ApplicationSettings AppSettings { get; set;}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			new SingleInstanceApp().Run(Environment.GetCommandLineArgs());
		}
	}
}
