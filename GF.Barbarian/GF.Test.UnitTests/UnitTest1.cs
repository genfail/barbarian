using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using GF.Lib.Communication.Midi;
using System.Linq;

namespace GF.Test.UnitTests
{
	[TestFixture]
	public class TestMidi
	{
		[TestCase]
		public void TestDeviceInquiry()
		{
			int cnt = MidiCommands.GetDeviceCountWaveIn();

			string[] devices = MidiCommands.GetDeviceNamesWaveIn();

			Assert.IsTrue(devices != null, "Returned null");
			Assert.IsTrue(devices.Contains("GR-55"), "does not contain ");

		}
	}
}
