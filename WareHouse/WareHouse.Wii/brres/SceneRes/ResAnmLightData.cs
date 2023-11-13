using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;

namespace WareHouse.Wii.brres.SceneRes
{
    public class ResAnmLightData : IResourceBase
    {
        const int FLAG_LIGHT_TYPE = (3 << 0);
        const int FLAG_HAS_SPEC = (1 << 3);
        const int FLAG_HAS_COLOR = (1 << 4);
        const int FLAG_HAS_ALPHA = (1 << 5);
        const int FLAG_POS_X = (1 << 19);
        const int FLAG_POS_Y = (1 << 20);
        const int FLAG_POS_Z = (1 << 21);
        const int FLAG_COLOR = (1 << 22);
        const int FLAG_ENABLE = (1 << 23);
        const int FLAG_AIM_X = (1 << 24);
        const int FLAG_AIM_Y = (1 << 25);
        const int FLAG_AIM_Z = (1 << 26);
        const int FLAG_CUTOFF = (1 << 27);
        const int FLAG_REFDIST = (1 << 28);
        const int FLAG_REFBRIGHT = (1 << 29);
        const int FLAG_SPECCOLOR = (1 << 30);
        const int FLAG_SHININESS = (1 << 31);

        public ResAnmLightData(MemoryFile file, ushort numFrames) : base(file)
        {
            mSpecLightObjIdx = file.ReadUInt32();
            mUserDataOffs = file.ReadUInt32();
            mFlags = file.ReadUInt32();
            mEnable = new ResBoolAnmData(file, numFrames);

            mXPosition = new(file, (mFlags & FLAG_POS_X) != 0);
            mYPosition = new(file, (mFlags & FLAG_POS_Y) != 0);
            mZPosition = new(file, (mFlags & FLAG_POS_Z) != 0);
            mColor = new(file, (mFlags & FLAG_COLOR) != 0, numFrames);

            mXAim = new(file, (mFlags & FLAG_AIM_X) != 0);
            mYAim = new(file, (mFlags & FLAG_AIM_Y) != 0);
            mZAim = new(file, (mFlags & FLAG_AIM_Z) != 0);

            mDistFunc = (GXDistAttnFn)file.ReadUInt32();
            mRefDistance = new(file, (mFlags & FLAG_REFDIST) != 0);
            mRefBrightness = new(file, (mFlags & FLAG_REFBRIGHT) != 0);
            mSpotFunc = (GXSpotFn)file.ReadUInt32();
            mCutoff = new(file, (mFlags & FLAG_CUTOFF) != 0);
            mSpecularColor = new(file, (mFlags & FLAG_SPECCOLOR) != 0, numFrames);
            mShiniess = new(file, (mFlags & FLAG_SHININESS) != 0);
        }

        uint mSpecLightObjIdx;
        uint mUserDataOffs;
        uint mFlags;
        ResBoolAnmData mEnable;
        ResAnmData mXPosition;
        ResAnmData mYPosition;
        ResAnmData mZPosition;
        ResColorAnmData mColor;
        ResAnmData mXAim;
        ResAnmData mYAim;
        ResAnmData mZAim;
        GXDistAttnFn mDistFunc;
        ResAnmData mRefDistance;
        ResAnmData mRefBrightness;
        GXSpotFn mSpotFunc;
        ResAnmData mCutoff;
        ResColorAnmData mSpecularColor;
        ResAnmData mShiniess;
    }
}
