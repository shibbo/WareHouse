using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brlyt.material
{
    public class TexMap
    {
        public TexMap(FileBase file)
        {
            mTextureCount = file.ReadUInt16();

            byte v1 = file.ReadByte();
            byte v2 = file.ReadByte();

            mWrapS = (int)BitUtil.ExtractBits(v1, 2, 30);
            mWrapT = (int)BitUtil.ExtractBits(v2, 2, 30);
            mMinFilter = (int)(BitUtil.ExtractBits(v1, 3, 27) + 1) & 0x7;
            mMaxFilter = (int)(BitUtil.ExtractBits(v2, 1, 29) + 1) & 0x1;
        }

        ushort mTextureCount;
        int mWrapS;
        int mWrapT;
        int mMinFilter;
        int mMaxFilter;
    }
}
