using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxPosData
    {
        public ResVtxPosData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int vtxPosArray = basePos + file.ReadInt32();
            int namePos = basePos + file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(namePos - 4);
            mID = file.ReadUInt32();
            mCompCnt = (GXCompCnt)file.ReadUInt32();
            mCompType = (GXCompType)file.ReadUInt32();
            mFrac = file.ReadByte();
            mStride = file.ReadByte();
            mNumVerts = file.ReadUInt16();
            mMin = file.ReadVec3();
            mMax = file.ReadVec3();

            file.Seek(vtxPosArray);
            for (ushort i = 0; i < mNumVerts; i++)
            {
                float x = 0.0f, y = 0.0f, z = 0.0f;
                float divisor = (float)Math.Pow(2, mFrac);

                if (mCompType == GXCompType.GX_U8 || mCompType == GXCompType.GX_S8)
                {
                    x = file.ReadByte() / divisor;
                    y = file.ReadByte() / divisor;
                    z = file.ReadByte() / divisor;
                }
                else if (mCompType == GXCompType.GX_U16)
                {
                    x = file.ReadUInt16() / divisor;
                    y = file.ReadUInt16() / divisor;
                    z = file.ReadUInt16() / divisor;
                }
                else if (mCompType == GXCompType.GX_S16)
                {
                    x = file.ReadInt16() / divisor;
                    y = file.ReadInt16() / divisor;
                    z = file.ReadInt16() / divisor;
                }
                else if (mCompType == GXCompType.GX_F32)
                {
                    x = file.ReadSingle();
                    y = file.ReadSingle();
                    z = file.ReadSingle();
                }

                mVerticies.Add(new Vector3(x, y, z));
            }
        }

        public string mName;
        public uint mID;
        GXCompCnt mCompCnt;
        GXCompType mCompType;
        byte mFrac;
        byte mStride;
        ushort mNumVerts;
        Vector3? mMin;
        Vector3? mMax;

        List<Vector3> mVerticies = new();
    }
}
