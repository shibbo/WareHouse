using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brlyt.material
{
    public class AlphaCompare
    {
        public AlphaCompare(FileBase file)
        {
            byte val = file.ReadByte();
            mComp0 = (byte)BitUtil.ExtractBits(val, 4, 32 - 4);
            mComp1 = (byte)BitUtil.ExtractBits(val, 4, 32 - 8);
            mOperation = file.ReadByte();
            mRef0 = file.ReadByte();
            mRef1 = file.ReadByte();
        }

        byte mOperation;
        byte mComp0;
        byte mComp1;
        byte mRef0;
        byte mRef1;
    }
}