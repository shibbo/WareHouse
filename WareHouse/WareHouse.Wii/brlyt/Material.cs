using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.io.util;
using WareHouse.Wii.brlyt.material;

namespace WareHouse.Wii.brlyt
{
    public class Material
    {
        public Material(MemoryFile file)
        {
            mMaterialName = file.ReadString(20).Replace("\0", string.Empty);
            mForeColor = new GXColorS10(file);
            mBackColor = new GXColorS10(file);
            mColorRegister3 = new GXColorS10(file);

            mTev = new GXColor[4];

            for (int i = 0; i < 4; i++)
            {
                mTev[i] = new GXColor(file);
            }

            uint flags = file.ReadUInt32();
            uint hasMaterialColor = BitUtil.ExtractBits(flags, 4, 1);
            uint hasChannelCtrl = BitUtil.ExtractBits(flags, 6, 1);
            uint hasBlendMode = BitUtil.ExtractBits(flags, 7, 1);
            uint hasAlphaCmp = BitUtil.ExtractBits(flags, 8, 1);
            uint tevStageCount = BitUtil.ExtractBits(flags, 9, 5);
            uint indStageCount = BitUtil.ExtractBits(flags, 14, 3);
            uint indTexSRTCount = BitUtil.ExtractBits(flags, 17, 2);
            uint hasTevSwapTbl = BitUtil.ExtractBits(flags, 19, 1);
            uint texCoordGenCount = BitUtil.ExtractBits(flags, 20, 4);
            uint texSRTCount = BitUtil.ExtractBits(flags, 24, 4);
            uint texMapCount = BitUtil.ExtractBits(flags, 28, 4);

            for (int i = 0; i < texMapCount; i++)
            {
                mTexMaps.Add(new TexMap(file));
            }

            for (int i = 0; i < texSRTCount; i++)
            {
                mTexSRTs.Add(new TexSRT(file));
            }

            for (int i = 0; i < texCoordGenCount; i++)
            {
                mTexCoords.Add(new TexCoordGen(file));
            }

            if (hasChannelCtrl == 1)
            {
                mChanCtrl = new ChanCtrl(file);
            }

            if (hasMaterialColor == 1)
            {
                mMatColor = new GXColor(file);
            }

            if (hasTevSwapTbl == 1)
            {
                mSwapTable = new TevSwapTable(file);
            }

            for (int i = 0; i < indTexSRTCount; i++)
            {
                mIndTexSRTs.Add(new TexSRT(file));
            }

            for (int i = 0; i < indStageCount; i++)
            {
                mIndTexStages.Add(new IndStage(file));
            }

            for (int i = 0; i < tevStageCount; i++)
            {
                mTevStages.Add(new TevStage(file));
            }

            if (hasAlphaCmp == 1)
            {
                mAlphaCompare = new AlphaCompare(file);
            }

            if (hasBlendMode == 1)
            {
                mBlendMode = new BlendMode(file);
            }
        }

        string mMaterialName;
        GXColorS10 mForeColor;
        GXColorS10 mBackColor;
        GXColorS10 mColorRegister3;
        GXColor[] mTev;
        GXColor mMaterialColor;

        List<TexMap> mTexMaps = new List<TexMap>();
        List<TexSRT> mTexSRTs = new List<TexSRT>();
        List<TexSRT> mIndTexSRTs = new List<TexSRT>();
        List<TexCoordGen> mTexCoords = new List<TexCoordGen>();
        ChanCtrl mChanCtrl;
        GXColor mMatColor;
        TevSwapTable mSwapTable;
        List<IndStage> mIndTexStages = new List<IndStage>();
        List<TevStage> mTevStages = new List<TevStage>();
        AlphaCompare mAlphaCompare;
        BlendMode mBlendMode;
    }
}
