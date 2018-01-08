using GF.Lib.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian
{
	class ApplicationData
	{
		public byte[] DefaultG5L = null;
		public byte[] DefaultSYX = null;

		public void Init()
		{
			DefaultG5L = Barbarian.Properties.Resources.Default_G5L;
			DefaultSYX = Barbarian.Properties.Resources.Default_SYX;
		}
		public void Shutdown()
		{
			DefaultG5L = null;
			DefaultSYX = null;
		}
	}
}
