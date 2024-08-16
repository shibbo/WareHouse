using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class ChanCtrl
    {
        public ChanCtrl(FileBase file)
        {
            mColorMaterialSource = file.ReadByte();
            mAlphaMaterialSource = file.ReadByte();
            file.Skip(2);
        }

        byte mColorMaterialSource;
        byte mAlphaMaterialSource;
    }
}
