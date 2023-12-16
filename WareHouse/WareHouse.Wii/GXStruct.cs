using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii
{
    public class GXTexObj
    {
        public GXTexObj(MemoryFile file)
        {
            mMode0 = file.ReadUInt32();
            mMode1 = file.ReadUInt32();
            mImage0 = file.ReadUInt32();
            mImage3 = file.ReadUInt32();
            mUserData = file.ReadUInt32();
            mTexFormat = (GXTexFmt)file.ReadUInt32();
            mTlutName = file.ReadUInt32();
            mLoadCount = file.ReadUInt16();
            mLoadFormat = file.ReadByte();
            mFlags = file.ReadByte();
        }

        uint mMode0;
        uint mMode1;
        uint mImage0;
        uint mImage3;
        uint mUserData;
        GXTexFmt mTexFormat;
        uint mTlutName;
        ushort mLoadCount;
        byte mLoadFormat;
        byte mFlags;
    }

    public class GXTlutObj
    {
        public GXTlutObj(MemoryFile file)
        {
            mTlut = file.ReadUInt32();
            mLoadTlut0 = file.ReadUInt32();
            mNumTluts = file.ReadUInt16();
            file.Skip(2);
        }

        uint mTlut;
        uint mLoadTlut0;
        ushort mNumTluts;
    }
}
