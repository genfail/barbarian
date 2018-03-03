using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MIDI
{
    public partial class frmMain : Form
    {
        private CMIDIOutDevice objMIDIOut;
        private CMIDIInDevice objMIDIIn;
        private byte[] mSendBuffer;
        private byte[] mDataBuffer;
        private int mSendDataPointer;
        private bool mStartNewData;
        private CGT8Functions.MessageTypes mMsgType;
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {            
            objMIDIOut = new CMIDIOutDevice();
            objMIDIIn = new CMIDIInDevice(CGT8Functions.IN_BUFFER_LEN);

            objMIDIOut.MessageReceived += objMIDIOut_MessageReceived;
            objMIDIIn.MessageReceived += objMIDIIn_MessageReceived;
            objMIDIIn.ShortMessage += objMIDIIn_ShortMessage;
            objMIDIIn.LongMessage += objMIDIIn_LongMessage;
            objMIDIIn.ReceiveError += objMIDIIn_ReceiveError;

            ListDevices();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            objMIDIIn.Dispose();
            objMIDIOut.Dispose();
        }

        private void btnOutOpenClose_Click1(object sender, System.EventArgs e)
        {
            bool blnReturn;

            if(objMIDIOut.PortOpen)
            {
                //Close
                objMIDIOut.ClosePort();
                btnOutOpenClose.Text = "Open";
                //btnSend.PLong.Enabled = False
            }
            else
            {
                blnReturn = objMIDIOut.OpenPort((uint)cboMIDIDevices.SelectedIndex);
                btnOutOpenClose.Text = "Close";
                //btnSendLong.Enabled = True
            }
        }

        private void btnInOpenClose_Click(object sender, System.EventArgs e)
        {
            if(objMIDIIn.PortOpen)
            {
                //Close
                objMIDIIn.ClosePort();
                btnInOpenClose.Text = "Open";
            }
            else
            {
                objMIDIIn.OpenPort((uint)cboMIDIInDevices.SelectedIndex);
                btnInOpenClose.Text = "Close";
            }
        }

        private void btnSetPatch_Click(object sender, EventArgs e)
        {
            //Program Change message is 0xCX. The last parameter is not used.
            //This assumes that the GT-8 MIDI address is set to 16 (0xXF)
            uint bankSel, patchSel, sendData;

            bankSel = uint.Parse(txtBank.Text);
            patchSel = uint.Parse(cboPatch.Text);

            sendData = (4 * (bankSel - 1)) + patchSel - 1;

            objMIDIOut.SendShortMessage(0xCF, sendData, 0x0);
        }

        //###########################################################
        //System Data.
        private void btnSystemUpload_Click(object sender, EventArgs e)
        {
            //GT-8 Request
            mSendBuffer = CGT8Functions.GT8RQ1(0, 0x4000000);

            mMsgType = CGT8Functions.MessageTypes.SystemData;
            mStartNewData = true;
            objMIDIOut.SendLongMessage(mSendBuffer);
        }

        private void btnSystemDownload_Click(object sender, EventArgs e)
        {
            mSendDataPointer = 0;

            tmrSendTimer.Enabled = true;
        }

        private void btnSystemSave_Click(object sender, EventArgs e)
        {
            string filePath;

            filePath = Application.StartupPath;

            if(!filePath.EndsWith(@"\"))
                filePath += @"\";

            filePath += "System.gt8";

            StreamWriter MyFileWrite = new StreamWriter(filePath, false);

            for (int dataPointer = 0; dataPointer < mDataBuffer.Count(); dataPointer++)
            {
                MyFileWrite.Write(mDataBuffer[dataPointer].ToString("X2"));
                if (mDataBuffer[dataPointer] == 0xF7)
                {
                    MyFileWrite.Write(Environment.NewLine);
                }
                else
                {
                    MyFileWrite.Write(" ");
                }
            }
            MyFileWrite.Close();
        }

        private void btnSystemLoad_Click(object sender, EventArgs e)
        {
            string fileLine;
            string[] splitLine;
            List<byte> fileBuffer = new List<byte>();
            string filePath;

            filePath = Application.StartupPath;
            if (filePath.EndsWith(@"\"))
                filePath += @"\";

            filePath += "System.gt8";

            StreamReader MyFileStream = new StreamReader(filePath);
            while (!MyFileStream.EndOfStream)
            {
                fileLine = MyFileStream.ReadLine();
                splitLine = fileLine.Split(' ');
                for (int dataPointer = 0; dataPointer < splitLine.Count(); dataPointer++)
                {
                    fileBuffer.Add(byte.Parse(splitLine[dataPointer], System.Globalization.NumberStyles.HexNumber));
                }
                mDataBuffer = fileBuffer.ToArray();
            }
            MyFileStream.Close();

            mMsgType = CGT8Functions.MessageTypes.SystemData;
        }

        //###########################################################
        //Patch Data.
        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            string fileLine;
            string[] splitLine;
            List<byte> fileBuffer = new List<byte>();
            
            openFileDialog1.Filter = "GT8 Patches (*.GT8)|*.GT8|AllFiles (*.*)|*.*";
            openFileDialog1.ShowDialog();

            StreamReader MyFileStream = new StreamReader(openFileDialog1.FileName);
            while (!MyFileStream.EndOfStream)
            {
                fileLine = MyFileStream.ReadLine();
                splitLine = fileLine.Split(' ');
                for (int dataPointer = 0; dataPointer < splitLine.Count(); dataPointer++)
                {
                    fileBuffer.Add(byte.Parse(splitLine[dataPointer], System.Globalization.NumberStyles.HexNumber));
                }
                mDataBuffer = fileBuffer.ToArray();
            }
            MyFileStream.Close();

            mMsgType = CGT8Functions.MessageTypes.Patch;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            saveFileDialog1.Filter = "GT8 Patches (*.GT8)|*.GT8|AllFiles (*.*)|*.*";
            saveFileDialog1.ShowDialog();

            StreamWriter MyFileWrite = new StreamWriter(saveFileDialog1.FileName, false);

            for (int dataPointer = 0; dataPointer < mDataBuffer.Count(); dataPointer++)
            {
                MyFileWrite.Write(mDataBuffer[dataPointer].ToString("X2"));
                if(mDataBuffer[dataPointer]==0xF7)
                {
                    MyFileWrite.Write(Environment.NewLine);
                }
                else
                {
                    MyFileWrite.Write(" ");
                }
            }
            MyFileWrite.Close();
        }

        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            uint address;
            int bank, patch;

            if (int.Parse(txtBank.Text) < 1 || int.Parse(txtBank.Text) > 35)
            {
                MessageBox.Show("Bank number out of range");
            }
            else
            {
                bank = int.Parse(txtBank.Text);
                patch = int.Parse(cboPatch.Text);
                address = CGT8Functions.CalculatePatchAddress(bank, patch);

                //GT-8 Request
                mSendBuffer = CGT8Functions.GT8RQ1(address, 0x10000);

                mStartNewData = true;
                objMIDIOut.SendLongMessage(mSendBuffer);

                mMsgType = CGT8Functions.MessageTypes.Patch;
            }
        }

        private void btnDownload_Click(object sender, System.EventArgs e)
        {
            if(int.Parse(txtBank.Text) < 1 || int.Parse(txtBank.Text) > 35)
            {
                MessageBox.Show("Bank number out of range");
            }
            else
            {
                //Download mstrDataBuffer to MIDI client
                btnDownload.Enabled = false;

                mSendDataPointer = 0;

                //Split data on carrage return
                tmrSendTimer.Enabled = true;
            }
        }

        //###########################################################
        //System Functions
        private void tmrSendTimer_Tick(object sender, System.EventArgs e)
        {
            uint address;
            byte[] bytDataBuffer;
            List<byte> dataBuffer = new List<byte>();
            int bank, patch;

            tmrSendTimer.Enabled = false;

            //Split current dataline to create temporary send buffer.
            do
            {
                dataBuffer.Add(mDataBuffer[mSendDataPointer]);
                mSendDataPointer++;
            } while (mSendDataPointer < mDataBuffer.Count() && mDataBuffer[mSendDataPointer - 1] != 0xF7);
            bytDataBuffer = dataBuffer.ToArray();

            if (mMsgType == CGT8Functions.MessageTypes.Patch)
            {
                //Use the address of the selected patch to send the data to.
                bank = int.Parse(txtBank.Text);
                patch = int.Parse(cboPatch.Text);
                address = CGT8Functions.CalculatePatchAddress(bank, patch);
                bytDataBuffer = CGT8Functions.GT8DT1(address, bytDataBuffer);
            }
           
            objMIDIOut.SendLongMessage(bytDataBuffer);

            //If more data is waiting to be sent then wait before sending the rest of the data.
            if(mSendDataPointer < mDataBuffer.Count())
            {
                tmrSendTimer.Enabled = true;
            }
            else
            {
                btnDownload.Enabled = true;
                txtLog.Text += "Send Complete." + Environment.NewLine;
            }
        }

        private void objMIDIOut_MessageReceived(object sender, MidiMsgEventArgs e)
        {
            txtLog.Text += e.MessageText;
        }

        private void objMIDIIn_MessageReceived(object sender, MidiMsgEventArgs e)
        {
            txtLog.Text += e.MessageText;
        }

        private void objMIDIIn_ShortMessage(object sender, MidiShortMsgEventArgs e)
        {
            uint bank;
            uint patch;

            if (e.Status.ToString("X") == "CF")
            {
                bank = (uint)(e.Data1 / 4) + 1;
                patch = (uint)(e.Data1 % 4) + 1;

                txtBank.Text = bank.ToString();
                cboPatch.SelectedIndex = (int)patch - 1;
            }
            else
            {
                txtLog.Text += ("Short Message Received: " +
                    "Status = " + e.Status.ToString("X") +
                    ", Data1 = " + e.Data1.ToString("X") +
                    ", Data2 = " + e.Data2.ToString("X") + Environment.NewLine);
            }
        }

        private void objMIDIIn_LongMessage(object sender, MidiLongMsgEventArgs e)
        {
            int checkSumPos = -1;
            int dataPointer;
            bool msgEnd;
            List<byte> allDataBuffer;
            uint msgAddress = 0;

            //Convert input buffer to hex string
            if(mStartNewData)
            {
                allDataBuffer = new List<byte>();
                mStartNewData = false;
            }
            else
            {
                allDataBuffer = mDataBuffer.ToList<byte>();
            }
            
            //Append the data to the memory buffer
            dataPointer = 0;
            msgEnd = false;
            while (dataPointer < e.MessageData.Length && !msgEnd)
            {
                //record message address.
                if (dataPointer == 0x7) { msgAddress += (uint)(e.MessageData[dataPointer] * 0x1000000); }
                if (dataPointer == 0x8) { msgAddress += (uint)(e.MessageData[dataPointer] * 0x10000); }
                if (dataPointer == 0x9) { msgAddress += (uint)(e.MessageData[dataPointer] * 0x100); }
                if (dataPointer == 0xA) { msgAddress += e.MessageData[dataPointer]; }

                //Add new data to all data buffer.
                allDataBuffer.Add(e.MessageData[dataPointer]);
                if (e.MessageData[dataPointer] == 0xF7)
                {
                    checkSumPos = dataPointer - 1;
                    msgEnd = true;
                }
                else
                {
                    dataPointer++;
                }
            }

            mDataBuffer = allDataBuffer.ToArray();

            //Check the cheksum for the new data.
            if(checkSumPos!=-1)
            {
                byte checkSumValue;

                checkSumValue = CGT8Functions.CalcCheckSum(e.MessageData);
                if(checkSumValue==0x80)
                {
                    txtLog.Text += "Long message Received, Checksum OK." + Environment.NewLine;
                }
                else
                {
                    txtLog.Text += "Long message Received, Checksum Error." + Environment.NewLine;
                }
            }
            else
            {
                txtLog.Text += "Long message Received, Checksum Missing." + Environment.NewLine;
            }

            //Check for message end.
            if(CGT8Functions.CheckForMessageEnd(mMsgType, msgAddress))
            {
                txtLog.Text += "Data transfer complete." + Environment.NewLine;

                if(mMsgType == CGT8Functions.MessageTypes.SystemData)
                {
                    mSendBuffer = CGT8Functions.GT8RQ1(0x4000000, 0x1000000);
                    mMsgType = CGT8Functions.MessageTypes.QuickFX;
                    objMIDIOut.SendLongMessage(mSendBuffer);
                }
                else if (mMsgType == CGT8Functions.MessageTypes.QuickFX)
                {
                    mSendBuffer = CGT8Functions.GT8RQ1(0x6000000, 0x1000000);
                    mMsgType = CGT8Functions.MessageTypes.QuickFXName;
                    objMIDIOut.SendLongMessage(mSendBuffer);
                }
            }
        }

        private void objMIDIIn_ReceiveError(object sender, MidiErrorEventArgs e)
        {
            MessageBox.Show(e.ExceptionData.Message);
        }

        private void ListDevices()
        {
            string[] strDevices;
            uint intNumOfDevices;

            intNumOfDevices = objMIDIOut.ListDevices(out strDevices);
            if (intNumOfDevices > 0)
            {
                for (int intLoop = 0; intLoop < intNumOfDevices; intLoop++)
                {
                    cboMIDIDevices.Items.Add(strDevices[intLoop]);
                }
                cboMIDIDevices.SelectedIndex = 0;
            }

            intNumOfDevices = objMIDIIn.ListDevices(out strDevices);
            if (intNumOfDevices > 0)
            {
                for (uint intLoop = 0; intLoop < intNumOfDevices; intLoop++)
                {
                    cboMIDIInDevices.Items.Add(strDevices[intLoop]);
                }
                cboMIDIInDevices.SelectedIndex = 0;
            }
            else
            {
                cboMIDIInDevices.Text = "None";
                cboMIDIInDevices.Enabled = false;
                btnInOpenClose.Enabled = false;
            }
        }
    }
}
