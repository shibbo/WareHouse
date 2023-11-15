using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxFurVecData
    {
        public ResVtxFurVecData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int furVecOffs = basePos + file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mVecCount = file.ReadUInt16();

            file.Seek(furVecOffs);
            for (ushort i = 0; i < mVecCount; i++)
            {
                mFurArray.Add(file.ReadVec3());
            }
        }

        string mName;
        uint mID;
        ushort mVecCount;
        List<Vector3> mFurArray = new();
    }
}
