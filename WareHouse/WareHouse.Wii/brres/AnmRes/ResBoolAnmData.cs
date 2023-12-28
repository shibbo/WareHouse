using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.AnmRes
{
    public class ResBoolAnmData
    {
        public ResBoolAnmData(MemoryFile file, ushort numFrames) 
        {
            numFrames = (ushort)Math.Ceiling((float)numFrames / 32);

            int[] frames = new int[numFrames];
            for (ushort i = 0; i < numFrames; i++)
            {
                frames[i] = file.ReadInt32();
            }

            foreach(int frame in frames)
            {
                for (int i = 0; i < 32; i++)
                {
                    if (((frame >> i) & 0x1) != 0) {
                        mFrameData.Add(true);
                    }
                    else
                    {
                        mFrameData.Add(false);
                    }
                }
            }
        }

        List<bool> mFrameData = new();
    }
}
