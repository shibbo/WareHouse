using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres
{
    /// <summary>
    /// Represents a single resource in a BRRES file.
    /// </summary>
    public interface IResource
    {

    }

    public class IResourceBase
    {
        public IResourceBase(MemoryFile file)
        {
            int startPos = file.Position();
            file.Skip(0x8);
            int resOffs = file.ReadInt32();
            mResourceName = file.ReadStringLenPrefixU32At(startPos + resOffs - 4);
            mResourceId = file.ReadUInt32();
            mReferenceNum = file.ReadUInt32();
        }

        public string? mResourceName;
        public uint mResourceId;
        public uint mReferenceNum;
    }
}
