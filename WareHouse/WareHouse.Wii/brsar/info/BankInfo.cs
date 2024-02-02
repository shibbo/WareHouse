using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar.info
{
    public class BankInfo
    {
        public BankInfo(MemoryFile file)
        {
            mStringID = file.ReadUInt32();
            mFileID = file.ReadUInt32();
            file.Skip(4);
        }

        uint mStringID;
        uint mFileID;
    }
}
