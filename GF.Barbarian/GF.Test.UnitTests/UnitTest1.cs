using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using GF.Lib.Communication.Midi;
using System.Linq;
using System.IO;
using System.Threading;

namespace GF.Test.UnitTests
{
	[TestFixture]
	public class TestMidi
	{
		[TestCase]
		public void TestWriteSysx()
		{
			GF.Barbarian.Midi.ConnectionMidi Midi = new Barbarian.Midi.ConnectionMidi();
			Midi.Init();
			Midi.Connect();

			string pth = @"D:\Data\Project.src\barbarian\GF.Barbarian\Patches\ceiling.syx";
			byte[] b = File.ReadAllBytes(pth);
			bool success = Midi.Out.SendLongMessage(b);
			Assert.IsTrue(success, "Should be true");

			Midi.Disconnect();
			Midi.Shutdown();
		}
		[TestCase]
		public void xx()
		{
		}
	}
}
