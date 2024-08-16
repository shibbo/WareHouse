using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class IndStage
    {
        public IndStage(FileBase file)
        {
            mTexCoord = file.ReadByte();
            mTexMap = file.ReadByte();
            mWrapS = file.ReadByte();
            mWrapT = file.ReadByte();
        }

        byte mTexCoord;
        byte mTexMap;
        byte mWrapS;
        byte mWrapT;
    }
}
