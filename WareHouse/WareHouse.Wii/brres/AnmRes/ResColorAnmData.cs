using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.AnmRes
{
    public class ResColorAnmData
    {
        public ResColorAnmData(MemoryFile file, bool isOneFrame, ushort frameCount)
        {
            if (isOneFrame)
            {
                mColorFrame = file.ReadUInt32();
            }
            else
            {
                for (ushort i = 0; i < frameCount; i++)
                {
                    uint frame = file.ReadUInt32At((int)(file.Position() + file.ReadUInt32()));
                    mColorFrames.Add(frame);
                }
            }
        }

        uint mColorFrame;
        List<uint> mColorFrames = new();
    }
}
