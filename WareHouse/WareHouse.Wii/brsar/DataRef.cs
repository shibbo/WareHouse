using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar
{
    public class DataRef
    {
        enum ReferenceType
        {
            Address = 0,
            Offset = 1
        }

        public DataRef(MemoryFile file)
        {
            mType = (ReferenceType)file.ReadByte();
            mDataType = file.ReadByte();
            file.Skip(2);
            mValue = file.ReadUInt32();
        }

        public uint GetValue()
        {
            return mValue;
        }

        public uint GetDataType()
        {
            return mDataType;
        }

        ReferenceType mType;
        byte mDataType;
        uint mValue;
    }
}
