using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar
{
    public class BRSAR : ISoundArchive
    {
        public BRSAR(MemoryFile file)
        {
            if (file.ReadString(4) != "RSAR")
            {
                throw new Exception("BRSAR::BRSAR(MemoryFile) -- Invalid RSAR magic.");
            }

            file.Skip(2);
            mVersion = file.ReadUInt16();
            file.Skip(6);
            ushort numBlocks = file.ReadUInt16();

            int symDataOffs = file.ReadInt32();
            int symDataSize = file.ReadInt32();
            int infoOffset = file.ReadInt32();
            int infoSize = file.ReadInt32();
            int fileImgOffs = file.ReadInt32();
            int fileImgSize = file.ReadInt32();

            if (symDataOffs != 0)
            {
                file.Seek(symDataOffs);
                mSymBlock = new(file);
            }
            
            if (infoOffset != 0)
            {
                file.Seek(infoOffset);
                mInfoBlock = new(file);
            }
        }

        public static ushort GetCurVersion()
        {
            return mVersion;
        }

        static ushort mVersion;
        SymbolBlock mSymBlock;
        InfoBlock mInfoBlock;
    }

    public class TreeNode
    {
        public TreeNode(MemoryFile file)
        {
            mFlags = file.ReadUInt16();
            mBit = file.ReadUInt16();
            mLeftIdx = file.ReadUInt32();
            mRightIdx = file.ReadUInt32();
            mStringIdx = file.ReadInt32();
            mIdx = file.ReadUInt32();
        }

        ushort mFlags;
        ushort mBit;
        uint mLeftIdx;
        uint mRightIdx;
        int mStringIdx;
        uint mIdx;
    }
}
