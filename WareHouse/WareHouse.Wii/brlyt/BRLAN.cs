using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres;

namespace WareHouse.Wii.brlyt
{
    public class BRLAN : ILayoutAnimation
    {
        public BRLAN(MemoryFile file)
        {
            if (file.ReadString(4) != "RLAN")
            {
                throw new Exception("BRLAN::BRLAN() -- Invalid file magic.");
            }

            mBOM = file.ReadUInt16();

            if (mBOM != 0xFEFF)
            {
                throw new Exception("BRLAN::BRLAN() -- BOM is not set to big endian");
            }

            mVersion = file.ReadUInt16();
            file.Skip(6);
            mBlockCount = file.ReadUInt16();

            for (int i = 0; i < mBlockCount; i++)
            {
                string section = file.ReadString(4);

                switch (section)
                {

                }
            }
        }

        ushort mBOM;
        ushort mVersion;
        ushort mBlockCount;
    }


}
