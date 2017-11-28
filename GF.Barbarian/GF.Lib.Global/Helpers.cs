using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Lib.Global
{
	public static class Helpers
	{
		public static string FormatDate(DateTime dtDate)
		{
			//Get date and time in short format
			return dtDate.ToShortDateString().ToString() + " " + dtDate.ToShortTimeString().ToString();
		}

		public static string FormatSize(Int64 lSize)
		{
			//Format number to KB
			string stringSize = "";
			NumberFormatInfo myNfi = new NumberFormatInfo();
			Int64 lKBSize = 0;

			if (lSize < 1024)
			{
				if (lSize == 0)
				{
					//zero byte
					stringSize = "0";
				}
				else
				{
					//less than 1K but not zero byte
					stringSize = "1";
				}
			}
			else
			{
				//convert to KB
				lKBSize = lSize / 1024;
				//format number with default format
				stringSize = lKBSize.ToString("n", myNfi);
				//remove decimal
				stringSize = stringSize.Replace(".00", "");
			}

			return stringSize + " KB";
		}
	}
}
