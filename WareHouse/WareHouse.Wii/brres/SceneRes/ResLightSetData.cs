using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.SceneRes
{
    public class ResLightSetData : IResourceBase
    {
        public ResLightSetData(MemoryFile file) : base(file)
        {
            int basePos = file.Position() - 0x14;
            int offs = file.ReadInt32() + basePos - 4;
            mAmbientLight = file.ReadStringLenPrefixU32At(offs);
            mLightID = file.ReadUInt16();
            mDiffuseLightCount = file.ReadByte();
            file.Skip(1);

            basePos = file.Position();

            for (byte i = 0; i < 8; i++)
            {
                int curOffs = file.ReadInt32();

                if (curOffs != 0)
                {
                    mLightNames[i] = file.ReadStringLenPrefixU32At(curOffs + basePos - 4);
                }
            }

            for (byte i = 0; i < 8; i++)
            {
                mLightIDs[i] = file.ReadUInt16();
            }
        }

        string mAmbientLight;
        ushort mLightID;
        byte mDiffuseLightCount;
        string[] mLightNames = new string[8];
        ushort[] mLightIDs = new ushort[8];
    }
}
