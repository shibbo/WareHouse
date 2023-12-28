using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;
using static WareHouse.Wii.brres.ResCommon;

namespace WareHouse.Wii.brres
{
    public class ResAnmVisInfoData
    {
        public ResAnmVisInfoData(MemoryFile file)
        {
            mNumFrame = file.ReadUInt16();
            mNumNodes = file.ReadUInt16();
            mPolicy = (AnmPolicy)file.ReadUInt32();
        }

        public ushort mNumFrame;
        ushort mNumNodes;
        AnmPolicy mPolicy;
    }

    public class ResAnmVisAnmData
    {
        public ResAnmVisAnmData(MemoryFile file, ushort numFrame)
        {
            int basePos = file.Position();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlags = file.ReadUInt32();
            
            if ((mFlags & 0x2) == 0)
            {
                mVisbility = new(file, numFrame);
            }
        }

        string mName;
        uint mFlags;
        ResBoolAnmData mVisbility;
    }

    public class ResAnmVis : IResource
    {
        public ResAnmVis(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "VIS0")
            {
                throw new Exception("ResAnmVis::ResAnmVis(MemoryFile) -- Invalid VIS0 magic.");
            }

            file.Skip(4);
            mRevision = file.ReadUInt32();
            file.Skip(4);
            int visDictOffs = file.ReadInt32();
            int userDataOFfs = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);
            mInfoData = new(file);

            if (visDictOffs != 0)
            {
                file.Seek(visDictOffs + basePos);
                ResDict visDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in visDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mAnims.Add(kvp.Key, new(file, mInfoData.mNumFrame));
                }
            }
        }

        uint mRevision;
        string mName;
        ResAnmVisInfoData mInfoData;
        Dictionary<string, ResAnmVisAnmData> mAnims = new();
    }
}
