using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar.info
{
    public class SoundCommonInfo
    {
        public SoundCommonInfo(MemoryFile file)
        {

        }

        uint mStringIdx;
        uint mFileIdx;
        uint mPlayerId;

    }

    public class Sound3DParam
    {
        public Sound3DParam(MemoryFile file)
        {

        }

        uint mFlags;
        byte mDecayCurve;
        byte mDecayRatio;
        byte mDopplerFactor;
    }
}
