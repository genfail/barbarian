using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDI
{
    class MidiMsgEventArgs : EventArgs
    {
        private string mMsgText;

        public MidiMsgEventArgs(string msgText)
        {
            mMsgText = msgText;
        }

        public string MessageText
        {
            get
            {
                return mMsgText;
            }
        }
    }

    class MidiShortMsgEventArgs : EventArgs
    {
        private uint mStatus;
        private uint mData1;
        private uint mData2;

        public MidiShortMsgEventArgs(uint status, uint data1, uint data2)
        {
            mStatus = status;
            mData1 = data1;
            mData2 = data2;
        }

        public uint Status
        {
            get
            {
                return mStatus;
            }
        }

        public uint Data1
        {
            get
            {
                return mData1;
            }
        }

        public uint Data2
        {
            get
            {
                return mData2;
            }
        }
    }

    class MidiLongMsgEventArgs : EventArgs
    {
        private byte[] mMsgData;

        public MidiLongMsgEventArgs(byte[] msgData)
        {
            mMsgData = msgData;
        }

        public byte[] MessageData
        {
            get
            {
                return mMsgData;
            }
        }
    }

    class MidiErrorEventArgs : EventArgs
    {
        private Exception mMidiException;

        public MidiErrorEventArgs(Exception ex)
        {
            mMidiException = ex;
        }

        public Exception ExceptionData
        {
            get
            {
                return mMidiException;
            }
        }
    }
}
