using System;
using System.Collections.Generic;
using System.Text;
using WareHouse.io;

namespace WareHouse.Wii.brlyt.material
{
    public class TevStage
    {
        public TevStage(FileBase file)
        {
            file.Skip(0x10);
        }
    }
}
