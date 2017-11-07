using Fclp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.barbarian.Gui
{
	static class Program
	{
		private static ApplicationArguments arguments = null;
		public static ApplicationArguments Arguments { get{ return arguments;} }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			ParseArgs();
			Form1 frm = new Form1();

			if (!String.IsNullOrEmpty(arguments.LibraryPathFileName) && File.Exists(arguments.LibraryPathFileName))
				frm.StartWithDataFile(arguments.LibraryPathFileName);
			else
				MessageBox.Show("Can't open file " + arguments.LibraryPathFileName);
			Application.Run(frm);
		}

		public static bool ParseArgs()
		{
			// create a generic parser for the ApplicationArguments type
			var p = new FluentCommandLineParser<ApplicationArguments>();

			p.Setup<string>(arg => arg.LibraryPathFileName).As('f', "file");

			// specify which property the value will be assigned too.
			p.Setup<ProgramMode>(arg => arg.Mode)
			.As('m', "mode")
			.SetDefault(ProgramMode.File);

			p.SetupHelp("?", "help")
			.Callback(text => Console.WriteLine(ShowHelp));
			p.SetupHelp("h", "help")
			.Callback(text => Console.WriteLine(ShowHelp));

//			p.Setup(arg => arg.LibraryPathFileName);
//			.As('v', "value")
//			.Required();

			p.Setup(arg => arg.Silent)
			.As('s', "silent")
			.SetDefault(true);

			string[] args = Environment.GetCommandLineArgs();

			// First arg is exe name
			 for (var i = 1; i < args.Length; i++) 
                if (!args[i][0].Equals('-') && !args[i][0].Equals('/'))
                    args[i] = "/f:" + args[i];


			var result = p.Parse(args);

			if(result.HelpCalled)
			{
			// help
			}

			if (!result.HasErrors)
			{
				arguments = p.Object;
				return true;
			}
			else
			{
				arguments = new ApplicationArguments();
				arguments.Mode = ProgramMode.File;
				return false;
			}
		}

		private static string ShowHelp{get
			{
				return "Barbarian c:\\path]libraryfile.g5l  -mode=[File|Library]";
			}
		}
	}

	public class ApplicationArguments
	{
		public ProgramMode Mode { get; set; }
		public bool Silent { get; set; }
		public string LibraryPathFileName { get; set; }
	}
}
