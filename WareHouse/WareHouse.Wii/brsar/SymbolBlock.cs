using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar
{
    public class SymbolBlock
    {
        public SymbolBlock(MemoryFile file)
        {
            if (file.ReadString(4) != "SYMB")
            {
                throw new Exception("blah");
            }

            file.Skip(4);

            int basePos = file.Position();
            int tblOffs = file.ReadInt32();
            int soundTreeOffs = file.ReadInt32();
            int playerTreeOffs = file.ReadInt32();
            int groupTreeOffs = file.ReadInt32();
            int bankTreeOffs = file.ReadInt32();

            file.Seek(basePos + tblOffs);
            uint numEntries = file.ReadUInt32();
            List<string> fileNames = new();

            for (uint i = 0; i < numEntries; i++)
            {
                int offs = basePos + file.ReadInt32();
                fileNames.Add(file.ReadStringAtNT(offs));
            }

            file.Seek(basePos + soundTreeOffs);
            uint sndRootIdx = file.ReadUInt32();
            uint sndNodeCount = file.ReadUInt32();

            for (uint i = 0; i < sndNodeCount; i++)
            {
                mSoundTree.Add(new(file));
            }

            file.Seek(basePos + playerTreeOffs);
            uint plrRootIdx = file.ReadUInt32();
            uint plrNodeCount = file.ReadUInt32();

            for (uint i = 0; i < plrNodeCount; i++)
            {
                mPlayerTree.Add(new(file));
            }

            file.Seek(basePos + groupTreeOffs);
            uint grpRootIdx = file.ReadUInt32();
            uint grpNodeCount = file.ReadUInt32();

            for (uint i = 0; i < grpNodeCount; i++)
            {
                mGroupTree.Add(new(file));
            }

            file.Seek(basePos + bankTreeOffs);
            uint bankRootIdx = file.ReadUInt32();
            uint bankNodeCount = file.ReadUInt32();

            for (uint i = 0; i < bankNodeCount; i++)
            {
                mBankTree.Add(new(file));
            }
        }

        List<TreeNode> mSoundTree = new();
        List<TreeNode> mPlayerTree = new();
        List<TreeNode> mGroupTree = new();
        List<TreeNode> mBankTree = new();
    }
}
