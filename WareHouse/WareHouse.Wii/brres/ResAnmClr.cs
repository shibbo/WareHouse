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
    public class ResAnmClrInfoData
    {
        public ResAnmClrInfoData(MemoryFile file)
        {
            mNumFrame = file.ReadUInt16();
            mNumMaterial = file.ReadUInt16();
            mPolicy = (AnmPolicy)file.ReadUInt32();
        }

        public ushort mNumFrame;
        ushort mNumMaterial;
        AnmPolicy mPolicy;
    }

    public class ResAnmClrAnmData
    {
        public ResAnmClrAnmData(MemoryFile file, bool isConst, ushort frameCount)
        {
            mMask = file.ReadUInt32();
            mColorAnim = new(file, isConst, frameCount);
        }

        uint mMask;
        ResColorAnmData mColorAnim;
    }

    public class ResAnmClrMatData
    {
        enum Target
        {
            Material0,
            Material1,
            Ambient0,
            Ambient1,
            Tev0,
            Tev1,
            Tev2,
            TevConst0,
            TevConst1,
            TevConst2,
            TevConst3
        }

        public ResAnmClrMatData(MemoryFile file, ushort frameCount)
        {
            int basePos = file.Position();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlags = file.ReadUInt32();

            for (int i = 0; i < 11; i++)
            {
                uint targetFlags = (mFlags >> i * 2) & 0x3;
                bool isExist = (targetFlags & 0x1) != 0;
                bool isConst = (targetFlags & 0x2) != 0;

                if (isExist)
                {
                    mAnims.Add((Target)i, new(file, isConst, frameCount));
                }
                else
                {
                    mAnims.Add((Target)i, null);
                }
            }
        }

        string mName;
        uint mFlags;
        Dictionary<Target, ResAnmClrAnmData> mAnims = new();
    }

    public class ResAnmClr : IResource
    {
        public ResAnmClr(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "CLR0")
            {
                throw new Exception("ResAnmClr::ResAnmClr(MemoryFile) -- Invalid CLR0 magic.");
            }

            file.Skip(4);
            mRevision = file.ReadUInt32();
            file.Skip(4);
            int colorDictOffs = file.ReadInt32();
            int userDataOFfs = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);
            mInfoData = new(file);

            if (colorDictOffs != 0)
            {
                file.Seek(colorDictOffs + basePos);
                ResDict clrDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in clrDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mColorData.Add(kvp.Key, new(file, mInfoData.mNumFrame));
                }
            }
        }

        uint mRevision;
        string mName;
        ResAnmClrInfoData mInfoData;
        Dictionary<string, ResAnmClrMatData> mColorData = new();
    }
}
