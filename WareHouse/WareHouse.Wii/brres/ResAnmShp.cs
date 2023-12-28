using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres
{
    public class ResAnmShp : IResource
    {
        public ResAnmShp(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "SHP0")
            {
                throw new Exception("ResAnmShp::ResAnmShp(MemoryFile) -- Invalid SHP0 magic.");
            }
        }
    }
}
