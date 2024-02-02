using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar.info
{
    public class PlayerInfo
    {
        public PlayerInfo(MemoryFile file)
        {
            mStringID = file.ReadUInt32();
            mPlayableSoundCount = file.ReadByte();
            file.Skip(3);
            mHeapSize = file.ReadUInt32();
            mReserved = file.ReadUInt32();
        }

        uint mStringID;
        byte mPlayableSoundCount;
        uint mHeapSize;
        readonly uint mReserved;
    }
}
