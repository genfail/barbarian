using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Globalization;

namespace MIDI
{    
    class splitData
    {
        public byte byte3 { get; private set; }
        public byte byte2 { get; private set; }
        public byte byte1 { get; private set; }
        public byte byte0 { get; private set; }

        public splitData(uint value)
        {
            byte0 = (byte)(value & 0xFF);
            byte1 = (byte)((value & 0xFF00) / 0x100);
            byte2 = (byte)((value & 0xFF0000) / 0x10000);
            byte3 = (byte)((value & 0xFF000000) / 0x1000000);
        }
    }

    class CGT8Functions
    {
        public enum MessageTypes
        {
            SystemData,
            QuickFX,
            QuickFXName,
            Patch
        }

        public const int IN_BUFFER_LEN = 256;

        public static uint CalculatePatchAddress(int bank, int patch)
        {
            int intAddressTotal;

            intAddressTotal = ((bank - 1) * 4) + (patch - 1);

            return (uint)((intAddressTotal + 0x800) * 0x10000);
        }

        public static byte[] GT8DT1(uint address, byte[] dataBuffer)
        {
            List<byte> messageBuffer = new List<byte>();
            byte[] messageBytes;
            
            //Force the header and address information.
            messageBuffer.Add(0xF0);
            messageBuffer.Add(0x41);
            messageBuffer.Add(0x0);      //Device address (set in parameters)
            messageBuffer.Add(0x0);
            messageBuffer.Add(0x0);
            messageBuffer.Add(0x6);
            messageBuffer.Add(0x12);     //Command DT1

            splitData dataSplitter = new splitData(address);
            
            messageBuffer.Add(dataSplitter.byte3);  //Set the patch number to the selected value.
            messageBuffer.Add(dataSplitter.byte2);
            messageBuffer.Add(dataBuffer[9]);       //Use the original LSB address from the data.
            messageBuffer.Add(dataBuffer[10]);

            //Copy all the data from the data section wihout the footer
            for (int dataCounter = 11; dataCounter < (dataBuffer.Count()-3); dataCounter++)
            {
                messageBuffer.Add(dataBuffer[dataCounter]);
            }

            //Add the footer information.
            messageBuffer.Add(0);        //CheckSum (fill later)
            messageBuffer.Add(0xF7);     //End Message

            messageBytes = messageBuffer.ToArray();
            //Fill in check sum value.
            messageBytes[messageBytes.Count() - 2] = CalcCheckSum(messageBytes);

            return messageBytes;
        }

        public static byte[] GT8RQ1(uint address, uint size)
        {
            List<byte> messageBuffer = new List<byte>();
            byte[] messageBytes;
            splitData dataSplitter;
            
            //GT-8 Request
            messageBuffer.Add(0xF0);
            messageBuffer.Add(0x41);
            messageBuffer.Add(0x0);      //Device address (set in parameters)
            messageBuffer.Add(0x0);
            messageBuffer.Add(0x0);
            messageBuffer.Add(0x6);
            messageBuffer.Add(0x11);     //Command RQ1

            dataSplitter = new splitData(address);

            messageBuffer.Add(dataSplitter.byte3);
            messageBuffer.Add(dataSplitter.byte2);
            messageBuffer.Add(dataSplitter.byte1);
            messageBuffer.Add(dataSplitter.byte0);

            dataSplitter = new splitData(size);

            messageBuffer.Add(dataSplitter.byte3);
            messageBuffer.Add(dataSplitter.byte2);
            messageBuffer.Add(dataSplitter.byte1);
            messageBuffer.Add(dataSplitter.byte0);

            messageBuffer.Add(0);       //Checksum (fill later)
            messageBuffer.Add(0xF7);    //End Message

            messageBytes =  messageBuffer.ToArray();
            //Fill in check sum value.
            messageBytes[messageBytes.Count() - 2] = CalcCheckSum(messageBytes);

            return messageBytes;
        }

        public static bool CheckForMessageEnd(MessageTypes msgType, uint address)
        {
            switch(msgType)
            {
                case MessageTypes.SystemData:
                    return SystemMsg(address);
                case MessageTypes.QuickFX:
                    return QuickFX(address);
                case MessageTypes.QuickFXName:
                    return QuickFXName(address);
                case MessageTypes.Patch:
                    return PatchMsg(address);
                default:
                    return false;
            }
        }

        private static bool SystemMsg(uint address)
        {
            if (address == 0x03050200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool QuickFX(uint address)
        {
            if ((address & 0xFFFF) == 0x521C)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool QuickFXName(uint address)
        {
            if ((address & 0xFFFF) == 0x520B)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool PatchMsg(uint address)
        {
            splitData tempAddress = new splitData(address);

            if(tempAddress.byte1==0x1E)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //If the checksum value is set to 0 before calling this function then the correct checksum will be added to the data
        //If the data is sent to this function with a checksum already present then the return value will be 120 (80h)
        public static byte CalcCheckSum(byte[] sysexMsg)
        {
            int sumValue = 0;
            byte remainderValue = 0;
            int dataIndex = 0;
            bool dataOK = false;


            while(dataIndex<sysexMsg.Count() && !dataOK)
            {
                if(sysexMsg[dataIndex]==0x11 || sysexMsg[dataIndex]==0x12)
                {
                    dataOK = true;
                }
                dataIndex++;
            }

            if (dataOK)
            {
                dataOK = false;

                while(dataIndex<sysexMsg.Count() && !dataOK)
                {
                    if (sysexMsg[dataIndex] == 0xF7)
                    {
                        dataOK = true;
                    }
                    else
                    {
                        sumValue += sysexMsg[dataIndex];
                    }
                    dataIndex++;
                }
            }

            if(dataOK)
            {
                remainderValue = (byte)(sumValue % 128);
                return (byte)(128 - remainderValue);
            }
            else
            {
                return 0;
            }
        }
    }
}
