using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxTexCoordData
    {
        public enum ComponentCount
        {
            TEX_S = 0,
            TEX_T = 1
        }

        public ResVtxTexCoordData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int vtxOffs = basePos + file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mCompCount = (ComponentCount)file.ReadInt32();
            mCompType = (GXCompType)file.ReadInt32();
            mFrac = file.ReadByte();
            mStride = file.ReadByte();
            mNumTexCoord = file.ReadUInt16();
            mMin = file.ReadVec2();
            mMax = file.ReadVec2();

            file.Seek(vtxOffs);

            for (ushort i = 0; i < mNumTexCoord; i++)
            {
                float x = 0.0f, y = 0.0f;
                float divisor = (float)Math.Pow(2, mFrac);

                if (mCompType == GXCompType.GX_U8 || mCompType == GXCompType.GX_S8)
                {
                    x = file.ReadByte() / divisor;
                    y = file.ReadByte() / divisor;
                }
                else if (mCompType == GXCompType.GX_U16)
                {
                    x = file.ReadUInt16() / divisor;
                    y = file.ReadUInt16() / divisor;
                }
                else if (mCompType == GXCompType.GX_S16)
                {
                    x = file.ReadInt16() / divisor;
                    y = file.ReadInt16() / divisor;
                }
                else if (mCompType == GXCompType.GX_F32)
                {
                    x = file.ReadSingle();
                    y = file.ReadSingle();
                }

                mTexCoords.Add(new Vector2(x, y));
            }
        }

        string mName;
        uint mID;
        ComponentCount mCompCount;
        GXCompType mCompType;
        byte mFrac;
        byte mStride;
        ushort mNumTexCoord;
        Vector2? mMin;
        Vector2? mMax;
        List<Vector2> mTexCoords = new();
    }
}
