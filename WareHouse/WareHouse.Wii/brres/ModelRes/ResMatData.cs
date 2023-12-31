﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResChanData
    {
        public ResChanData(MemoryFile file)
        {
            mFlag = file.ReadUInt32();
            mMatColor = new(file);
            mAmbColor = new(file);
            mParamChanCtrlColor = file.ReadUInt32();
            mParamChanCtrlAlpha = file.ReadUInt32();
        }

        uint mFlag;
        GXColor mMatColor;
        GXColor mAmbColor;
        uint mParamChanCtrlColor;
        uint mParamChanCtrlAlpha;
        
    }
    public class ResGenModeData
    {
        public ResGenModeData(MemoryFile file)
        {
            mNumTexGens = file.ReadByte();
            mNumChans = file.ReadByte();
            mNumTevs = file.ReadByte();
            mNumInds = file.ReadByte();
            mCullMode = (GXCullMode)file.ReadInt32();
        }

        byte mNumTexGens;
        byte mNumChans;
        byte mNumTevs;
        byte mNumInds;
        GXCullMode mCullMode;
    }

    public class ResMatMiscData
    {
        enum IndirectMethod
        {
            WARP,
            NORMAL_MAP,
            NORMAL_MAP_SPECULAR,
            FUR,
            UNK_4,
            UNK_5,
            USER0,
            USER1,
        }

        public ResMatMiscData(MemoryFile file)
        {
            mZCompLoc = file.ReadByte() != 0;
            mLightSetIdx = file.ReadByte();
            mFogIdx = file.ReadByte();
            file.Skip(1);
            mIndirectMethod = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                mIndirectMethod[i] = file.ReadByte();
            }

            mNormalMapLight = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                mNormalMapLight[i] = file.ReadByte();
            }
        }

        bool mZCompLoc;
        byte mLightSetIdx;
        byte mFogIdx;
        byte[] mIndirectMethod;
        byte[] mNormalMapLight;
    }

    public class TexSrt
    {
        public TexSrt(MemoryFile file)
        {
            mSu = file.ReadSingle();
            mSv = file.ReadSingle();
            mR = file.ReadSingle();
            mTu = file.ReadSingle();
            mTv = file.ReadSingle();
        }

        float mSu;
        float mSv;
        float mR;
        float mTu;
        float mTv;
    }

    public class TexMtxEffect
    {
        public TexMtxEffect(MemoryFile file)
        {
            mCamera = file.ReadByte();
            mLight = file.ReadByte();
            mMapMode = file.ReadByte();
            mFlags = file.ReadByte();
            mEffectMatrix = new(file);
        }

        byte mCamera;
        byte mLight;
        byte mMapMode;
        byte mFlags;
        Matrix3x4 mEffectMatrix;
    }

    public class ResTexSrtData
    {
        enum MatrixMode
        {
            Maya = 0,
            XSI = 1,
            Max = 2
        }
        public ResTexSrtData(MemoryFile file)
        {
            mFlag = file.ReadUInt32();
            mMatrixMode = (MatrixMode)file.ReadUInt32();

            mTexSrt = new TexSrt[8];

            for (int i = 0; i < 8; i++)
            {
                mTexSrt[i] = new(file);
            }

            mEffect = new TexMtxEffect[8];

            for (int i = 0; i < 8; i++)
            {
                mEffect[i] = new(file);
            }
        }

        uint mFlag;
        MatrixMode mMatrixMode;
        TexSrt[] mTexSrt;
        TexMtxEffect[] mEffect;
    }

    public class ResPixDL
    {
        public ResPixDL(MemoryFile file)
        {
            mAlphaCompare = new byte[5];

            for (int i = 0; i < 5; i++)
            {
                mAlphaCompare[i] = file.ReadByte();
            }

            mZMode = new byte[5];

            for (int i = 0; i < 5; i++)
            {
                mZMode[i] = file.ReadByte();
            }

            mBlendMode = new byte[10];

            for (int i = 0; i < 10; i++)
            {
                mBlendMode[i] = file.ReadByte();
            }

            mSetDstAlpha = new byte[5];

            for (int i = 0; i < 5; i++)
            {
                mSetDstAlpha[i] = file.ReadByte();
            }

            file.Skip(7);
        }

        byte[] mAlphaCompare;
        byte[] mZMode;
        byte[] mBlendMode;
        byte[] mSetDstAlpha;
    }

    public class ResTevColorDL
    {
        public ResTevColorDL(MemoryFile file)
        {
            mTevColor = new byte[3, 20];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    mTevColor[i, j] = file.ReadByte();
                }
            }

            file.Skip(4);

            mTevKColor = new byte[4, 10];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    mTevKColor[i, j] = file.ReadByte();
                }
            }

            file.Skip(24);
        }

        byte[,] mTevColor;
        byte[,] mTevKColor;
    }

    public class ResIndMtxAndScaleDL
    {
        public ResIndMtxAndScaleDL(MemoryFile file)
        {
            mIndTexCoordScale = new byte[2, 5];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mIndTexCoordScale[i, j] = file.ReadByte();
                }
            }

            mIndTexMtx0 = new byte[15];

            for (int i = 0; i < 15; i++)
            {
                mIndTexMtx0[i] = file.ReadByte();
            }

            file.Skip(7);

            mIndTexMtx1 = new byte[15];

            for (int i = 0; i < 15; i++)
            {
                mIndTexMtx1[i] = file.ReadByte();
            }

            mIndTexMtx2 = new byte[15];

            for (int i = 0; i < 15; i++)
            {
                mIndTexMtx2[i] = file.ReadByte();
            }

            file.Skip(2);
        }

        byte[,] mIndTexCoordScale;
        byte[] mIndTexMtx0;
        byte[] mIndTexMtx1;
        byte[] mIndTexMtx2;
    }

    public class ResTexCoordGenDL
    {
        public ResTexCoordGenDL(MemoryFile file)
        {
            mTexCoordGen = new byte[8, 18];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    mTexCoordGen[i, j] = file.ReadByte();
                }
            }

            file.Skip(16);
        }

        byte[,] mTexCoordGen;
    }

    public class ResMatDLData
    {
        public ResMatDLData(MemoryFile file)
        {
            mPixDL = new(file);
            mTevColorDL = new(file);
            mIndMtxDL = new(file);
            mTexCoordGenDL = new(file);
        }

        ResPixDL mPixDL;
        ResTevColorDL mTevColorDL;
        ResIndMtxAndScaleDL mIndMtxDL;
        ResTexCoordGenDL mTexCoordGenDL;
    }

    public class ResTexPlttInfoData
    {
        public ResTexPlttInfoData(MemoryFile file)
        {
            int basePos = file.Position();
            mTexName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mPaletteName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            // nw4r code puts two pointers to the ResTexData and the ResPlttData here
            file.Skip(8);
            mTexMapID = (GXTexMapID)file.ReadInt32();
            mTlutID = (GXTlut)file.ReadInt32();
            mWrapS = (GXTexWrapMode)file.ReadInt32();
            mWrapT = (GXTexWrapMode)file.ReadInt32();
            mMinFilt = (GXTexFilter)file.ReadInt32();
            mMaxFilt = (GXTexFilter)file.ReadInt32();
            mLodBias = file.ReadSingle();
            mMaxAniso = (GXAnisotropy)file.ReadInt32();
            mBiasClamp = file.ReadByte() != 0;
            mEdgeLod = file.ReadByte() != 0;
            file.Skip(2);
        }

        string mTexName;
        string mPaletteName;
        GXTexMapID mTexMapID;
        GXTlut mTlutID;
        GXTexWrapMode mWrapS;
        GXTexWrapMode mWrapT;
        GXTexFilter mMinFilt;
        GXTexFilter mMaxFilt;
        float mLodBias;
        GXAnisotropy mMaxAniso;
        bool mBiasClamp;
        bool mEdgeLod;
    }

    public class ResTexObjData
    {
        public ResTexObjData(MemoryFile file)
        {
            mTexMapIDFlags = file.ReadUInt32();
            mTexObjs = new GXTexObj[8];

            for (int i = 0; i < 8; i++)
            {
                mTexObjs[i] = new(file);
            }
        }

        uint mTexMapIDFlags;
        GXTexObj[] mTexObjs;
    }

    public class ResTlutObjData
    {
        public ResTlutObjData(MemoryFile file)
        {
            mTlutIDFlags = file.ReadUInt32();
            mTluts = new GXTlutObj[8];

            for (int i = 0; i < 8; i++)
            {
                mTluts[i] = new GXTlutObj(file);
            }
        }

        uint mTlutIDFlags;
        GXTlutObj[] mTluts;
    }

    public class ResMatFurData
    {
        public enum Interval
        {
            Uniform = 0,
            Tip = 1
        }

        public ResMatFurData(MemoryFile file)
        {
            mLength = file.ReadSingle();
            mLayerSize = file.ReadUInt32();
            mLayerInterval = (Interval)file.ReadInt32();
            mAlphaCurve = file.ReadSingle();
            mSpecularCurve = file.ReadSingle();
        }

        float mLength;
        uint mLayerSize;
        Interval mLayerInterval;
        float mAlphaCurve;
        float mSpecularCurve;
    }

    public class ResMatData
    {
        public ResMatData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mFlag = file.ReadUInt32();
            mGenModeData = new(file);
            mMatMiscData = new(file);

            int tevDataOffs = file.ReadInt32();
            uint numTexPaletteInfo = file.ReadUInt32();
            int texPlltOffs = file.ReadInt32();
            int matFurOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();
            int matDLOffs = file.ReadInt32();

            /* this is just data that is setup at runtime */
            mTexObjData = new(file);
            mTlutObjData = new(file);
            mTexSrtData = new(file);
            mChanData = new(file);

            if (tevDataOffs != 0)
            {
                file.Seek(tevDataOffs + basePos);
                mTevData = new(file);
            }

            if (texPlltOffs != 0)
            {
                file.Seek(texPlltOffs + basePos);
                mPaletteInfo = new ResTexPlttInfoData[numTexPaletteInfo];

                for (int i = 0; i < numTexPaletteInfo; i++)
                {
                    mPaletteInfo[i] = new(file);
                }
            }

            if (matFurOffs != 0)
            {
                file.Seek(matFurOffs + basePos);
                mMatFurData = new(file);
            }

            if (userDataOffs != 0)
            {
                throw new Exception("you have found userdata. let shibbo know");
            }

            if (matDLOffs != 0)
            {
                file.Seek(matDLOffs + basePos);
                mMatDLData = new(file);
            }
        }

        string mName;
        uint mID;
        uint mFlag;
        ResGenModeData mGenModeData;
        ResMatMiscData mMatMiscData;
        ResTevData mTevData;
        ResTexPlttInfoData[] mPaletteInfo;
        ResMatFurData mMatFurData;
        ResTexObjData mTexObjData;
        ResTlutObjData mTlutObjData;
        ResTexSrtData mTexSrtData;
        ResChanData mChanData;
        ResMatDLData mMatDLData;
    }
}
