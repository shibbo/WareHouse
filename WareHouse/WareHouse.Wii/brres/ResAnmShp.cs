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
    public class ResAnmShp : IResource
    {
        public class ResAnmShpInfoData
        {
            public ResAnmShpInfoData(MemoryFile file)
            {
                mNumFrames = file.ReadUInt16();
                mNumVtxNames = file.ReadUInt16();
                mPolicy = (AnmPolicy)file.ReadUInt32();
            }

            ushort mNumFrames;
            ushort mNumVtxNames;
            AnmPolicy mPolicy;
        }

        public class ResAnmShpAnmData
        {
            public ResAnmShpAnmData(MemoryFile file)
            {
                int basePos = file.Position();
                mFlags = file.ReadUInt32();
                mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
                mVtxIdx = file.ReadUInt16();
                mNumKeyShapes = file.ReadUInt16();
                mConstFlags = file.ReadUInt32();

                int anmIdxToVtxIdxTblOffs = file.ReadInt32();

                for (int i = 0; i < mNumKeyShapes; i++)
                {
                    bool isConst = false;

                    if ((mConstFlags & (1 << i)) != 0)
                    {
                        isConst = true;
                    }

                    mAnims.Add(new(file, isConst));
                }
            }

            uint mFlags;
            string mName;
            ushort mVtxIdx;
            ushort mNumKeyShapes;
            uint mConstFlags;
            List<ResAnmData> mAnims = new();
        }

        public ResAnmShp(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "SHP0")
            {
                throw new Exception("ResAnmShp::ResAnmShp(MemoryFile) -- Invalid SHP0 magic.");
            }

            file.Skip(4);
            mRevision = file.ReadUInt32();
            file.Skip(4);
            int dictOffs = file.ReadInt32();
            int vtxNameArrOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);
            mInfo = new(file);

            if (dictOffs != 0)
            {
                file.Seek(dictOffs + basePos);
                ResDict shpDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in shpDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mShapeAnims.Add(kvp.Key, new(file));
                }
            }
        }

        uint mRevision;
        string mName;
        ResAnmShpInfoData mInfo;
        Dictionary<string, ResAnmShpAnmData> mShapeAnims = new();
    }
}
