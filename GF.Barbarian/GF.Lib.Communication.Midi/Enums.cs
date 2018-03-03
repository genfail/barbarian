using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Lib.Communication.Midi
{
	public enum MIM : uint
    {
        MM_MIM_OPEN = 0x3C1,                            //MIDI input
        MM_MIM_CLOSE = 0x3C2,
        MM_MIM_DATA = 0x3C3,
        MM_MIM_LONGDATA = 0x3C4,
        MM_MIM_ERROR = 0x3C5,
        MM_MIM_LONGERROR = 0x3C6,
        MM_MIM_MOREDATA = 0x3CC,
        MIM_OPEN = MM_MIM_OPEN,
        MIM_CLOSE = MM_MIM_CLOSE,
        MIM_DATA = MM_MIM_DATA,
        MIM_LONGDATA = MM_MIM_LONGDATA,
        MIM_ERROR = MM_MIM_ERROR,
        MIM_LONGERROR  = MM_MIM_LONGERROR,
        MIM_MOREDATA = MM_MIM_MOREDATA              //MIM_DONE w/ pending events
    }

    public enum MMSYSERR : uint
    {
        MMSYSERR_NOERROR = 0,
        MMSYSERR_BASE = 0,
        MMSYSERR_BADDEVICEID = (MMSYSERR_BASE + 2),     //device ID out of range
        MMSYSERR_INVALPARAM = (MMSYSERR_BASE + 11),     //invalid parameter passed
        MMSYSERR_NODRIVER = (MMSYSERR_BASE + 6),        //no device driver present
        MMSYSERR_NOMEM = (MMSYSERR_BASE + 7),           //memory allocation error
        MMSYSERR_INVALHANDLE = (MMSYSERR_BASE + 5),     //device handle is invalid
        MMSYSERR_ALLOCATED = (MMSYSERR_BASE + 4),       //device already allocated
        MMSYSERR_INVALFLAG = (MMSYSERR_BASE + 10)       //invalid flag passed
    }

    public enum MIDIERR : uint
    {
        MIDIERR_BASE = 64,
        MIDIERR_NODEVICE = (MIDIERR_BASE + 4),          //port no longer connected
        MIDIERR_STILLPLAYING = (MIDIERR_BASE + 1),      //still something playing
        MIDIERR_NOTREADY = (MIDIERR_BASE + 3),          //hardware is still busy
        MIDIERR_BADOPENMODE = (MIDIERR_BASE + 6),       //operation unsupported w/ open mode
        MIDIERR_UNPREPARED = (MIDIERR_BASE + 0)         //header not prepared
    }

    public enum MMRESULT : uint
    {
        MMSYSERR_NOERROR = 0,
        MMSYSERR_ERROR = 1,
        MMSYSERR_BADDEVICEID = 2,
        MMSYSERR_NOTENABLED = 3,
        MMSYSERR_ALLOCATED = 4,
        MMSYSERR_INVALHANDLE = 5,
        MMSYSERR_NODRIVER = 6,
        MMSYSERR_NOMEM = 7,
        MMSYSERR_NOTSUPPORTED = 8,
        MMSYSERR_BADERRNUM = 9,
        MMSYSERR_INVALFLAG = 10,
        MMSYSERR_INVALPARAM = 11,
        MMSYSERR_HANDLEBUSY = 12,
        MMSYSERR_INVALIDALIAS = 13,
        MMSYSERR_BADDB = 14,
        MMSYSERR_KEYNOTFOUND = 15,
        MMSYSERR_READERROR = 16,
        MMSYSERR_WRITEERROR = 17,
        MMSYSERR_DELETEERROR = 18,
        MMSYSERR_VALNOTFOUND = 19,
        MMSYSERR_NODRIVERCB = 20,
        WAVERR_BADFORMAT = 32,
        WAVERR_STILLPLAYING = 33,
        WAVERR_UNPREPARED = 34
    }
}
