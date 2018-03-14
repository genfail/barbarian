using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Barbarian.Midi
{
	public class SysxPatch
	{
		public int Count { get; private set; }
		public string Name { get; private set; } = "NotSet";

		public byte[] Data { get; private set; } = null;

		public SysxPatch(int cnt)
		{
			Count = cnt;
		}

		public static SysxPatch MakePatchFromSyx(int pchNr, byte[] fileData)
		{
			if (fileData == null)
			{
				Debug.WriteLine($"Size error: ");
				return null;
			}

			try
			{
				SysxPatch p = new SysxPatch(pchNr);
				p.Data = fileData;

				byte[] buf = fileData.SubArrayCopy(12, 16);
				p.Name = System.Text.Encoding.ASCII.GetString(buf).Trim();
				return p;
			}
			catch(Exception)
			{
				return null;
			}
		}


		public static SysxPatch MakePatchFromG5L(int pchNr, byte[] fileData, uint offset, uint length)
		{
			if (fileData == null)
			{
				Debug.WriteLine($"Size error: ");
				return null;
			}

			if (length < 1171+52)
			{
				Debug.WriteLine($"Size error. Expected {1171+52} but according to data only {length} bytes");
				return null;
			}
			else if (fileData.Length < offset + 1171+52)
			{
				Debug.WriteLine($"Size error. Expected {offset + length} but got {fileData.Length} bytes");
				return null;
			}

			SysxPatch p = new SysxPatch(pchNr);

			byte[] buf = fileData.SubArrayCopy(offset + 1, 16);
			p.Name = System.Text.Encoding.ASCII.GetString(buf).Trim();

			// copy base data from clean file
			p.Data = Properties.Resources.Default_SYX.SubArrayCopy(0, (uint)Properties.Resources.Default_SYX.Length);

			//temp = data.mid(a, 128);
            //default_data.replace(11, 128, temp);      //address "00"
			p.Data.Replace(11, fileData, offset + 0, 128);//address "00"

            //temp = data.mid(a+128, 114);
            //default_data.replace(152, 114, temp);     //address "01"
			p.Data.Replace(152, fileData, offset + 128, 114); //address "01"

            //temp = data.mid(a+250, 14);
            //default_data.replace(266, 14, temp);     //address "01"
			p.Data.Replace(266, fileData, offset + 250, 14); //address "01"

            //temp = data.mid(a+264, 78);
            //default_data.replace(293, 78, temp);     //address "02" +
			p.Data.Replace(293, fileData, offset + 264, 78); //address "02"

			//temp = data.mid(a+350, 128);
            //default_data.replace(384, 128, temp);     //address "03" +
			p.Data.Replace(384, fileData, offset + 350, 128); //address "03"

			//temp = data.mid(a+478, 72);
            //default_data.replace(525, 72, temp);     //address "04" +
			p.Data.Replace(525, fileData, offset + 478, 72); //address "04"

			//temp = data.mid(a+606, 18);
            //default_data.replace(666, 18, temp);     //address "05" +
			p.Data.Replace(666, fileData, offset + 606, 18); //address "05"

			//temp = data.mid(a+640, 30);
            //default_data.replace(697, 30, temp);     //address "06" +
			p.Data.Replace(697, fileData, offset + 640, 30); //address "06"

			//temp = data.mid(a+678, 125);
            //default_data.replace(740, 125, temp);     //address "07" +
			p.Data.Replace(740, fileData, offset + 678, 125); //address "07"

			//temp = data.mid(a+811, 128);
            //default_data.replace(878, 128, temp);     //address "10" +
			p.Data.Replace(878, fileData, offset + 811, 128); //address "10"

			//temp = data.mid(a+939, 86);
            //default_data.replace(1019, 86, temp);     //address "11" +
			p.Data.Replace(1019, fileData, offset + 939, 86); //address "11"

			//temp = data.mid(a+1033, 35);
            //default_data.replace(1118, 35, temp);    //address "20" +
			p.Data.Replace(1118, fileData, offset + 1033, 35); //address "20"

			//temp = data.mid(a+1072, 35);
            //default_data.replace(1166, 35, temp);    //address "21" +
			p.Data.Replace(1166, fileData, offset + 1072, 35); //address "21"

			//temp = data.mid(a+1115, 52);
            //default_data.replace(1214, 52, temp);    //address "30" +
			p.Data.Replace(1214, fileData, offset + 1115, 52); //address "30"

			//temp = data.mid(a+1171, 52);
            //default_data.replace(1279, 52, temp);    //address "31" +
			p.Data.Replace(1279, fileData, offset + 1171, 52); //address "31"

#if _DBG_BYTE
			for (int i = 0; i < sysxData.Length;i++)
			{
				byte b = sysxData[i];
				if (char.IsControl((char)b))
					Debug.WriteLine($"{i, 4}\t \t{(int)b}");
				else
					Debug.WriteLine($"{i, 4}\t'{(char)b}'\t{(int)b}");
			}
#endif			
			return p;
		}

		public override string ToString()
		{
			return $"Patch[{Count, 3}]: [{Name,-16}] {(Data==null?"NULL":Data.Length.ToString())} Bytes";
		}
	}
}
