using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class TexCoordGen
    {
        public TexCoordGen(FileBase file)
        {
            mGenType = file.ReadByte();
            mSource = file.ReadByte();
            mMtx = file.ReadByte();
            file.Skip(0x1);
        }

        byte mGenType;
        byte mSource;
        byte mMtx;
    }
}
