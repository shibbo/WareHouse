using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResCacheVtxDescv
    {
        public ResCacheVtxDescv(MemoryFile file)
        {
            mData = new byte[12];

            for (int i = 0; i < 12; i++)
            {
                mData[i] = file.ReadByte();
            }
        }

        byte[] mData;
    }

    public class ResTagDLData
    {
        public ResTagDLData(MemoryFile file)
        {
            int basePos = file.Position();
            mBufferSize = file.ReadUInt32();
            mCommandSize = file.ReadUInt32();
            mDisplayListOffs = file.ReadInt32() + basePos;
        }

        uint mBufferSize;
        uint mCommandSize;
        int mDisplayListOffs;
    }

    public class ResMtxSetUsed
    {
        public ResMtxSetUsed(MemoryFile file)
        {
            mNumMtxID = file.ReadUInt32();
            mVecMtxID = new ushort[mNumMtxID];

            for (int i = 0; i < mNumMtxID; i++)
            {
                mVecMtxID[i] = file.ReadUInt16();
            }
        }

        uint mNumMtxID;
        ushort[] mVecMtxID;
    }

    public class ResShpData
    {
        public ResShpData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            mCurMtxIdx = file.ReadInt32();
            mVtxCache = new(file);
            mPrePrimDL = new(file);
            mPrimDL = new(file);

            mVCDBitmap = file.ReadUInt32();
            mFlag = file.ReadUInt32();
            mName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mID = file.ReadUInt32();
            mVerticies = file.ReadUInt32();
            mNumPolys = file.ReadUInt32();
            mVtxPosID = file.ReadInt16();
            mVtxNrmID = file.ReadInt16();

            mVtxColorID = new short[2];

            for (int i = 0; i < 2; i++)
            {
                mVtxColorID[i] = file.ReadInt16();
            }

            mVtxTexClrID = new short[8];

            for (int i = 0; i < 8; i++)
            {
                mVtxTexClrID[i] = file.ReadInt16();
            }

            mFurVecID = file.ReadInt16();
            mFurPosID = file.ReadInt16();

            int mtxSetUsedOffs = file.ReadInt32();

            if (mtxSetUsedOffs != 0)
            {
                file.Seek(mtxSetUsedOffs + basePos);
                mMtxSetUsed = new(file);
            }

            /* after this data is the actual display list which contains commands on how to process the verticies / shapes */
            /* will implement this at a later date */
        }

        int mCurMtxIdx;
        ResCacheVtxDescv mVtxCache;
        ResTagDLData mPrePrimDL;
        ResTagDLData mPrimDL;
        uint mVCDBitmap;
        uint mFlag;
        string mName;
        uint mID;
        uint mVerticies;
        uint mNumPolys;
        short mVtxPosID;
        short mVtxNrmID;
        short[] mVtxColorID;
        short[] mVtxTexClrID;
        short mFurVecID;
        short mFurPosID;
        ResMtxSetUsed mMtxSetUsed;
    }
}
