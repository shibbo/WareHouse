using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResVtxClrData
    {
        enum CompCount
        {
            RGB = 0,
            RGBA = 1
        }
        enum CompType
        {
            RGB565 = 0,
            RGB8 = 1,
            RGBX8 = 2,
            RGBA4 = 3,
            RGBA6 = 4,
            RGBA8 = 5
        }

        public ResVtxClrData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            int vtxClrArr = basePos + file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mCompCount = (CompCount)file.ReadInt32();
            mCompType = (CompType)file.ReadInt32();
            mStride = file.ReadByte();
            file.Skip(1);
            mNumColors = file.ReadUInt16();

            file.Seek(vtxClrArr);
            for (ushort i = 0; i < mNumColors; i++)
            {
                uint val = file.ReadUInt32();

                if (mCompType == CompType.RGBA8)
                {
                    mColors.Add(val);
                }
            }
        }

        string mName;
        uint mID;
        CompCount mCompCount;
        CompType mCompType;
        byte mStride;
        ushort mNumColors;
        List<uint> mColors = new();
    }
}
