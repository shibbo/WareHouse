using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class BlendMode
    {
        public BlendMode(FileBase file)
        {
            mType = file.ReadByte();
            mSourceFactor = file.ReadByte();
            mDestinationFactor = file.ReadByte();
            mOperation = file.ReadByte();
        }

        byte mType;
        byte mSourceFactor;
        byte mDestinationFactor;
        byte mOperation;
    }
}
