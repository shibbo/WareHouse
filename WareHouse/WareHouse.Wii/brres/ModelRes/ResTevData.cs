using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResTevCommonDL
    {
        public ResTevCommonDL(MemoryFile file)
        {
            mSwapModeTable = new byte[4, 20];
            mIndTexOrder = new byte[1, 5];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    mSwapModeTable[i,j] = file.ReadByte();
                }
            }

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mIndTexOrder[i,j] = file.ReadByte();
                }
            }

            file.Skip(11);
        }

        byte[,] mSwapModeTable;
        byte[,] mIndTexOrder;
    }

    public class ResTevVariableDL
    {
        public ResTevVariableDL(MemoryFile file)
        {
            mTevKonstSel = new byte[10];

            for (int i = 0; i < 10; i++)
            {
                mTevKonstSel[i] = file.ReadByte();
            }

            mTevOrder = new byte[5];

            for (int i = 0; i < 5; i++)
            {
                mTevOrder[i] = file.ReadByte();
            }

            mTevColorCalc = new byte[2, 5];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mTevColorCalc[i, j] = file.ReadByte();
                }
            }

            mAlphaCalc = new byte[2, 5];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mAlphaCalc[i, j] = file.ReadByte();
                }
            }

            mIndTev = new byte[2, 5];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mIndTev[i, j] = file.ReadByte();
                }
            }

            file.Skip(3);
        }

        byte[] mTevKonstSel;
        byte[] mTevOrder;
        byte[,] mTevColorCalc;
        byte[,] mAlphaCalc;
        byte[,] mIndTev;
    }

    public class ResTevDL
    {
        public ResTevDL(MemoryFile file)
        {
            mCommonDL = new(file);
            mVariableDLs = new ResTevVariableDL[8];

            for (int i = 0; i < 8; i++)
            {
                mVariableDLs[i] = new(file);
            }
        }

        ResTevCommonDL mCommonDL;
        ResTevVariableDL[] mVariableDLs;
    }

    public class ResTevData
    {
        public ResTevData(MemoryFile file)
        {
            file.Skip(8);
            mID = file.ReadUInt32();
            mNumStages = file.ReadByte();
            file.Skip(3);
            mTexCoordToTexMap = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                mTexCoordToTexMap[i] = file.ReadByte();
            }

            mTevDL = new(file);
        }

        uint mID;
        byte mNumStages;
        byte[] mTexCoordToTexMap;
        ResTevDL mTevDL;
    }
}
