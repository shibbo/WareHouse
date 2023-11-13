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
            for (ushort i = 0; i < numFrames; i++)
            {
                uint val = file.ReadUInt32At(file.ReadInt32());
                mFrameData.Add(val);
            }
        }

        List<uint> mFrameData = new();
    }
}
