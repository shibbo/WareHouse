using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;
using WareHouse.Wii.brres.ModelRes;
using WareHouse.Wii.brres.SceneRes;
using static WareHouse.Wii.brres.ResCommon;

namespace WareHouse.Wii.brres
{
    public class ResAnmTexSrtInfoData
    {
        public ResAnmTexSrtInfoData(MemoryFile file)
        {
            mFrameCount = file.ReadUInt16();
            mMaterialCount = file.ReadUInt16();
            mMatrixMode = (TexMatrixMode)file.ReadUInt32();
            mAnmPolicy = (AnmPolicy)file.ReadUInt32();
        }

        public ushort mFrameCount;
        ushort mMaterialCount;
        TexMatrixMode mMatrixMode;
        AnmPolicy mAnmPolicy;
    }

    public class ResAnmTexSrtMatData
    {
        public ResAnmTexSrtMatData(MemoryFile file)
        {
            int basePos = file.Position();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlags = file.ReadUInt32();
            mIndFlags = file.ReadUInt32();

            for (int i = 0; i < 11; i++)
            {
                /* TexSRT 0-7 check */
                if (i < 8)
                {
                    if (((mFlags >> i) & 0x1) != 0)
                    {
                        file.Seek(basePos + file.ReadInt32());
                        mTexData.Add(new(file));
                    }
                    else
                    {
                        mTexData.Add(null);
                    }
                }
                /* IndTexSRT 0-2 check */
                else
                {
                    if (((mIndFlags >> i) & 0x1) != 0)
                    {
                        file.Seek(basePos + file.ReadInt32());
                        mTexData.Add(new(file));
                    }
                    else
                    {
                        mTexData.Add(null);
                    }
                }
            }
        }

        string mName;
        uint mFlags;
        uint mIndFlags;
        List<ResAnmTexSrtTexData> mTexData = new(11);
    }

    public class ResAnmTexSrtTexData
    {
        public enum Target
        {
            ScaleS,
            ScaleT,
            Rotate,
            TranslateS,
            TranlateT
        }

        int[] ignoreMaskArray = [ 0x2, 0x2, 0x4, 0x8, 0x8 ];
        int[] constMaskArray = [ 0x20, 0x40, 0x80, 0x100, 0x200 ];

        public ResAnmTexSrtTexData(MemoryFile file)
        {
            mFlags = file.ReadUInt32();

            for (uint i = 0; i < 5; i++)
            {
                if ((mFlags & ignoreMaskArray[i]) != 0)
                {
                    float defVal = 0.0f;
                    if (i == 0 || i == 1)
                    {
                        defVal = 1.0f;
                    }
                    mAnms.Add((Target)i, defVal);
                    continue;
                }

                bool isConst = false;

                if ((mFlags & constMaskArray[i]) != 0)
                {
                    isConst = true;
                }

                mAnms.Add((Target)i, new ResAnmData(file, isConst));
            }
        }

        uint mFlags;
        Dictionary<Target, object> mAnms = new();
    }

    public class ResAnmTexSrt : IResource
    {
        public ResAnmTexSrt(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);

            mVersionNum = file.ReadUInt32();
            file.Skip(4);

            int texSrtDicOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);

            mInfoData = new(file);

            if (texSrtDicOffs != 0)
            {
                file.Seek(texSrtDicOffs + basePos);
                ResDict texSirtDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in texSirtDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mTexData.Add(kvp.Key, new(file));
                }
            }
        }

        string mName;
        uint mVersionNum;
        ResAnmTexSrtInfoData mInfoData;
        Dictionary<string, ResAnmTexSrtMatData> mTexData = new();
    }
}
