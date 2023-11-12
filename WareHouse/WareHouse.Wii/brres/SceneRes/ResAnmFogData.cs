using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;

namespace WareHouse.Wii.brres.SceneRes
{
    public class ResAnmFogData : IResourceBase
    {
        const int FLAG_START_Z = (1 << 29);
        const int FLAG_END_Z = (1 << 30);
        const int FLAG_COLOR = (1 << 31);

        public ResAnmFogData(MemoryFile file, ushort numFrames) : base(file)
        {
            mFlags = file.ReadUInt32();
            mFogType = file.ReadUInt32();
            mStartZ = new ResAnmData(file, (mFlags & FLAG_START_Z) != 0);
            mEndZ = new ResAnmData(file, (mFlags & FLAG_END_Z) != 0);
            bool isConst = (mFlags & FLAG_COLOR) != 0;
            numFrames = (ushort)(isConst ? 1 : numFrames);
            mColor = new ResColorAnmData(file, numFrames);
        }

        uint mFlags;
        uint mFogType;
        ResAnmData mStartZ;
        ResAnmData mEndZ;
        ResColorAnmData mColor;
    }
}
