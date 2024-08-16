using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class TexSRT
    {
        public TexSRT(FileBase file)
        {
            x = file.ReadSingle();
            y = file.ReadSingle();
            mRotate = file.ReadSingle();
            mScaleX = file.ReadSingle();
            mScaleY = file.ReadSingle();
        }

        float x;
        float y;
        float mRotate;
        float mScaleX;
        float mScaleY;
    }
}
