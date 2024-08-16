using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brlyt.material
{
    public class TevSwapTable
    {
        public TevSwapTable(FileBase file)
        {
            mColors = new GXColor[4];
            for (int i = 0; i < 4; i++)
            {
                byte val = file.ReadByte();
                GXColor color = new GXColor();
                color.r = (byte)BitUtil.ExtractBits(val, 2, 32 - 8);
                color.g = (byte)BitUtil.ExtractBits(val, 2, 32 - 6);
                color.b = (byte)BitUtil.ExtractBits(val, 2, 32 - 4);
                color.a = (byte)BitUtil.ExtractBits(val, 2, 32 - 2);
                mColors[i] = color;
            }
        }

        GXColor[] mColors;
    }
}
