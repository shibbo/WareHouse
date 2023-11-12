using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.AnmRes
{
    public class ResAnmData
    {
        public ResAnmData(MemoryFile file, bool isConstant)
        {
            if (isConstant)
            {
                mConst = file.ReadSingle();
            }
            else
            {
                int savePos = file.Position() + 4;
                int loc = file.Position() + file.ReadInt32();
                file.Seek(loc);
                mKeyFrameData = new ResKeyFrameAnmData(file);
                file.Seek(savePos);
            }
        }

        float mConst;
        ResKeyFrameAnmData mKeyFrameData;
    }

    public class ResKeyFrameAnmData
    {
        public struct ResKeyFrameData
        {
            public float mFrame;
            public float mValue;
            public float mSlope;
        }
        public ResKeyFrameAnmData(MemoryFile file)
        {
            mKeyFrameCount = file.ReadUInt16();
            file.Skip(2);
            mInverseTime = file.ReadSingle();

            for (ushort i = 0; i < mKeyFrameCount; i++)
            {
                ResKeyFrameData frame = new();
                frame.mFrame = file.ReadSingle();
                frame.mValue = file.ReadSingle();
                frame.mSlope = file.ReadSingle();
                mKeyFrames.Add(frame);
            }
        }

        ushort mKeyFrameCount;
        float mInverseTime;
        List<ResKeyFrameData> mKeyFrames = new();
    }
}
