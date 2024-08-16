using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brlyt
{
    public class Group
    {
        public Group(MemoryFile file)
        {
            int pos = file.Position();
            int section_size = file.ReadInt32();
            mGroupName = file.ReadString(0x10).Replace("\0", "");
            mEntryCount = file.ReadUInt16();
            file.Skip(2);

            for (int i = 0; i < mEntryCount; i++)
            {
                mEntries.Add(file.ReadString(0x10).Replace("\0", ""));
            }
        }

        public void AddChild(Group child)
        {
            mChildren.Add(child);
        }

        public void SetParent(Group parent)
        {
            mParent = parent;
        }

        public Group GetParent()
        {
            return mParent;
        }

        string mGroupName;
        ushort mEntryCount;
        List<string> mEntries = new();
        List<Group> mChildren = new();
        Group mParent;
    }
}
