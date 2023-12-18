using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResTexPlttInfoOffsetData
    {
        public struct Pair
        {
            public uint MaterialOffs;
            public uint TexPlttOffs;
        }

        public ResTexPlttInfoOffsetData(MemoryFile file)
        {
            uint offsCount = file.ReadUInt32();

            for (int i = 0; i < offsCount; i++)
            {
                Pair pair = new();
                pair.MaterialOffs = file.ReadUInt32();
                pair.TexPlttOffs = file.ReadUInt32();
                mOffsetPairs.Add(pair);
            }
        }

        List<Pair> mOffsetPairs = new();
    }
}
