using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WareHouse.io;
using static WareHouse.Wii.brres.ResCommon;

namespace WareHouse.Wii.brres
{
    public class ResAnmTexPatInfoData
    {
        public ResAnmTexPatInfoData(MemoryFile file)
        {
            mNumFrame = file.ReadUInt16();
            mNumMaterial = file.ReadUInt16();
            mNumTexture = file.ReadUInt16();
            mNumPalette = file.ReadUInt16();
            mPolicy = (AnmPolicy)file.ReadUInt32();
        }

        ushort mNumFrame;
        ushort mNumMaterial;
        ushort mNumTexture;
        ushort mNumPalette;
        AnmPolicy mPolicy;
    }

    public class ResAnmTexPatFrmData
    {
        public ResAnmTexPatFrmData(MemoryFile file)
        {
            mFrame = file.ReadSingle();
            mTexIdx = file.ReadUInt16();
            mPlttIdx = file.ReadUInt16();
        }

        float mFrame;
        ushort mTexIdx;
        ushort mPlttIdx;
    }

    public class ResAnmTexPatAnmData
    {
        public ResAnmTexPatAnmData(MemoryFile file, bool isConst)
        {
            if (isConst)
            {
                mTexIdx = file.ReadUInt16();
                mPlttIndex = file.ReadUInt16();
            }
            else
            {
                mNumFrames = file.ReadUInt16();
                file.Skip(2);
                mInvKeyFrameRange = file.ReadSingle();

                for (int i = 0; i < mNumFrames; i++)
                {
                    mFrames.Add(new(file));
                }
            }
        }

        bool mIsConstAnm;
        ushort mTexIdx;
        ushort mPlttIndex;

        ushort mNumFrames;
        float mInvKeyFrameRange;
        List<ResAnmTexPatFrmData> mFrames = new();
    }

    public class ResAnmTexPatMatData
    {
        enum Target
        {
            Texture0,
            Texture1,
            Texture2,
            Texture3,
            Texture4,
            Texture5,
            Texture6,
            Texture7
        }
        public ResAnmTexPatMatData(MemoryFile file)
        {
            int basePos = file.Position();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlags = file.ReadUInt32();

            for (int i = 0; i < 8; i++)
            {
                uint targetFlags = (mFlags >> i * 4) & 0xF;
                bool anmExist = (targetFlags & 0x1) != 0;
                bool anmConst = (targetFlags & 0x2) != 0;
                bool texAnmExist = (targetFlags & 0x4) != 0;
                bool pltAnmExist = (targetFlags & 0x8) != 0;

                if (anmExist)
                {
                    if (!anmConst)
                    {
                        file.Seek(basePos + file.ReadInt32());
                    }
                    
                    mAnims.Add((Target)i, new(file, anmConst));
                }
                else
                {
                    mAnims.Add((Target)i, null);
                }
            }
        }

        string mName;
        uint mFlags;
        Dictionary<Target, ResAnmTexPatAnmData> mAnims = new();
    }

    public class ResAnmTexPat : IResource
    {
        public ResAnmTexPat(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "PAT0")
            {
                throw new Exception("ResAnmTexPat::ResAnmTexPat(MemoryFile) -- Invalid PAT0 magic.");
            }

            file.Skip(4);
            mRevision = file.ReadUInt32();
            file.Skip(4);
            int texPatDictOffs = file.ReadInt32();
            int texNameArrayOffs = file.ReadInt32();
            int plttNameOffs = file.ReadInt32();
            int resTexArrOffs = file.ReadInt32();
            int resPlttOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);
            mInfo = new(file);

            if (texPatDictOffs != 0)
            {
                file.Seek(texPatDictOffs + basePos);
                ResDict texPatDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in texPatDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mAnims.Add(kvp.Key, new(file));
                }
            }
        }

        uint mRevision;
        string mName;
        ResAnmTexPatInfoData mInfo;
        Dictionary<string, ResAnmTexPatMatData> mAnims = new();
    }
}
