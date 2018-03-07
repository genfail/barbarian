using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices; // needs ref to Microsoft.VisualBasic
using System.Configuration;
using Fclp;
using System.Diagnostics;

namespace GF.Barbarian
{
	class SingleInstanceApp : WindowsFormsApplicationBase
	{
		public SingleInstanceApp() : base()
		{
			this.IsSingleInstance = true;
		}
 
		protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e)
		{
			base.OnStartupNextInstance(e);
 
			string[] secondInstanceArgumens = e.CommandLine.ToArray();
 
			// Handle command line arguments of second instance
 
			if (e.BringToForeground)
			{
				ParseArgs(secondInstanceArgumens);
				((FrmMain)this.MainForm).ApplySettings();
				this.MainForm.BringToFront();
			}
		}

		protected override void OnCreateMainForm()
		{
			base.OnCreateMainForm();
			ParseArgs(Environment.GetCommandLineArgs());
			this.MainForm = new FrmMain();
			((FrmMain)this.MainForm).ApplySettings();
		}

		private bool ParseArgs(string[] args)
		{
			// Install-Package FluentCommandLineParser -Version 1.5.0.7-commands

			// create a generic parser for the ApplicationArguments type
			var p = new FluentCommandLineParser<ApplicationSettings>();

			p.Object.AutoConnect = Properties.Settings.Default.AutoConnect;

			p.Setup<string>(arg => arg.FileModePath).As('f', "file").UseForOrphanArguments(); // if no option specified then values are bound to this option
			p.Setup<bool>(arg => arg.Reset).As('r', "reset");
			p.Setup<bool>(arg => arg.AutoConnect).As('a', "autoconnect");
			p.Setup<ProgramMode>(arg => arg.Mode).As('m', "mode").SetDefault(ProgramMode.File);

			p.SetupHelp("?", "help")
			.Callback(text => Console.WriteLine(ShowHelp()));
			p.SetupHelp("h", "help")
			.Callback(text => Console.WriteLine(ShowHelp()));

			// arg[0] is exe name, so skip
			args = args.Skip(1).ToArray(); 
			var result = p.Parse(args);// /test="D:\admin\Roland_GR55\patches\Mustang Sally.g5l"

			Configuration userConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
			string sss= userConfig.FilePath;
			Trace.WriteLine("Settings are in file: " + sss);

			if(result.HelpCalled)
			{
				Console.WriteLine(ShowHelp());
			}
			if (!result.HasErrors) // when correct arguments, or empty
			{
				if (p.Object.Reset)
				{
					try
					{
						Properties.Settings.Default.Reset();

					}
					catch (Exception ex)
					{
						Debug.WriteLine("Cannot reset settings: " + ex.Message);
					}
				}

				if (String.IsNullOrEmpty(p.Object.FileModeDirectory))
					p.Object.FileModeDirectory = Properties.Settings.Default.LastSelectedFolder;
				if (String.IsNullOrEmpty(p.Object.FileModeDirectory)) // still nothing found
					p.Object.FileModeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				if (String.IsNullOrEmpty(p.Object.FileModeFileName))
					p.Object.FileModeFileName = Properties.Settings.Default.LastSelectedFile;

				Program.AppSettings = p.Object;
				return true;
			}
			else // if error
			{
				// Use standard settings
				Program.AppSettings = new ApplicationSettings();
				Program.AppSettings.Mode = ProgramMode.File;
				return false;
			}
		}

		private string ShowHelp()
		{
			return "Barbarian c:\\path\\libraryfile.g5l  -mode=[File|Library] -reset";
		}
	}
}
