using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxNrmData
    {
        public ResVtxNrmData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int vtxNormArr = basePos + file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mCompCnt = (GXCompCnt)file.ReadInt32();
            mCompType = (GXCompType)file.ReadInt32();
            mFrac = file.ReadByte();
            mStride = file.ReadByte();
            mNumNormals = file.ReadUInt16();

            file.Seek(vtxNormArr);
            for (ushort i = 0; i < mNumNormals; i++)
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

                mNormals.Add(new Vector3(x, y, z));
            }
        }

        string mName;
        uint mID;
        GXCompCnt mCompCnt;
        GXCompType mCompType;
        byte mFrac;
        byte mStride;
        ushort mNumNormals;
        List<Vector3> mNormals = new();
    }
}
