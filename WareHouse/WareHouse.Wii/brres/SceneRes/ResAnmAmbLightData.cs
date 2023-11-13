using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;

namespace WareHouse.Wii.brres.SceneRes
{
    public class ResAnmAmbLightData : IResourceBase
    {
        const int FLAG_HAS_COLOR = (1 << 0);
        const int FLAG_HAS_ALPHA = (1 << 1);
        const int FLAG_COLOR = (1 << 31);
        public ResAnmAmbLightData(MemoryFile file, ushort numAnimFrames) : base(file)
        {
            mFlags = file.ReadUInt32();

            mHasColor = (mFlags & FLAG_HAS_COLOR) == 1;
            mHasAlpha = (mFlags & FLAG_HAS_ALPHA) == 1;
            mColorFrames = new(file, (mFlags & FLAG_COLOR) != 0, numAnimFrames);
        }

        uint mFlags;
        ResColorAnmData mColorFrames;
        bool mHasColor;
        bool mHasAlpha;
    }
}
