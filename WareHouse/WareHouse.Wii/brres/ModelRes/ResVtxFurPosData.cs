using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxFurPosData
    {
        public ResVtxFurPosData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int furPosArr = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mComponentCount = (GXCompCnt)file.ReadInt32();
            mComponentType = (GXCompType)file.ReadInt32();
            mFrac = file.ReadByte();
            mStride = file.ReadByte();
            mNumFurPos = file.ReadUInt16();
            mNumFurLayers = file.ReadUInt32();
            
        }

        string mName;
        uint mID;
        GXCompCnt mComponentCount;
        GXCompType mComponentType;
        byte mFrac;
        byte mStride;
        ushort mNumFurPos;
        uint mNumFurLayers;
        List<Vector3> mPositions = new();
    }
}
