using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WareHouse.io;
using static WareHouse.Wii.brres.ResAnmChrAnmData;
using static WareHouse.Wii.brres.ResAnmChrNodeData;
using static WareHouse.Wii.brres.ResCommon;

namespace WareHouse.Wii.brres
{
    public class ResAnmChrInfoData
    {
        enum ScalingRule
        {
            Standard,
            SoftImage,
            Maya
        }

        public ResAnmChrInfoData(MemoryFile file)
        {
            mNumFrame = file.ReadUInt16();
            mNumNode = file.ReadUInt16();
            mPolicy = (AnmPolicy)file.ReadUInt32();
            mScaleRule = (ScalingRule)file.ReadUInt32();
        }

        public ushort mNumFrame;
        ushort mNumNode;
        AnmPolicy mPolicy;
        ScalingRule mScaleRule;
    }

    public class ResAnmChrFrm32Data
    {
        public ResAnmChrFrm32Data(MemoryFile file)
        {
            mFrameData = file.ReadUInt32();
        }

        uint mFrameData;
    }

    public class ResAnmChrFrm48Data
    {
        public ResAnmChrFrm48Data(MemoryFile file)
        {
            mFrame = file.ReadInt16();
            mValue = file.ReadUInt16();
            mSlope = file.ReadInt16();
        }

        short mFrame;
        ushort mValue;
        short mSlope;
    }

    public class ResAnmChrFrm96Data
    {
        public ResAnmChrFrm96Data(MemoryFile file)
        {
            mFrame = file.ReadSingle();
            mValue = file.ReadSingle();
            mSlope = file.ReadSingle();
        }

        float mFrame;
        float mValue;
        float mSlope;
    }

    public class ResAnmChrFVS32Data
    {
        public ResAnmChrFVS32Data(MemoryFile file, ushort numFrames)
        {
            mScale = file.ReadSingle();
            mOffset = file.ReadSingle();

            for (int i = 0; i < numFrames; i++)
            {
                mFrames.Add(new(file));
            }
        }

        float mScale;
        float mOffset;
        List<ResAnmChrFrm32Data> mFrames = new();
    }

    public class ResAnmChrFVS48Data
    {
        public ResAnmChrFVS48Data(MemoryFile file, ushort numFrames)
        {
            mScale = file.ReadSingle();
            mOffset = file.ReadSingle();

            for (int i = 0; i < numFrames; i++)
            {
                mFrames.Add(new(file));
            }
        }

        float mScale;
        float mOffset;
        List<ResAnmChrFrm48Data> mFrames = new();
    }

    public class ResAnmChrFVS96Data
    {
        public ResAnmChrFVS96Data(MemoryFile file, ushort numFrames)
        {
            for (int i = 0; i < numFrames; i++)
            {
                mFrames.Add(new(file));
            }
        }

        List<ResAnmChrFrm96Data> mFrames = new();
    }

    public class ResAnmChrCV8Data
    {
        public ResAnmChrCV8Data(MemoryFile file, ushort numFrames)
        {
            mScale = file.ReadSingle();
            mOffset = file.ReadSingle();

            for (int i = 0; i < numFrames; i++)
            {
                mFrameData.Add(file.ReadByte());
            }
        }

        float mScale;
        float mOffset;
        List<byte> mFrameData = new();
    }

    public class ResAnmChrCV16Data
    {
        public ResAnmChrCV16Data(MemoryFile file, ushort numFrames)
        {
            mScale = file.ReadSingle();
            mOffset = file.ReadSingle();

            for (int i = 0; i < numFrames; i++)
            {
                mFrameData.Add(file.ReadUInt16());
            }
        }

        float mScale;
        float mOffset;
        List<ushort> mFrameData = new();
    }

    public class ResAnmChrCV32Data
    {
        public ResAnmChrCV32Data(MemoryFile file, ushort numFrames)
        {
            for (int i = 0; i < numFrames; i++)
            {
                mFrames.Add(file.ReadSingle());
            }
        }

        List<float> mFrames = new();
    }

    public class ResAnmChrFVSData
    {
        public ResAnmChrFVSData(MemoryFile file, uint type, ushort numFrames)
        {
            /* only keyframe types use this */
            if (type >= 1 && type <= 3)
            {
                mNumFrames = file.ReadUInt16();
                file.Skip(2);
                mInvKeyFrameRange = file.ReadSingle();
            }
            

            /* for every type, cases 1, 2, 3 are all 32-bit, 48-bit, and 96-bit, respectively */
            /* the remaining cases are regular frame values */

            switch (type)
            {
                case 1:
                    mAnimData = new ResAnmChrFVS32Data(file, mNumFrames);
                    break;
                case 2:
                    mAnimData = new ResAnmChrFVS48Data(file, mNumFrames);
                    break;
                case 3:
                    mAnimData = new ResAnmChrFVS96Data(file, mNumFrames);
                    break;
                case 4:
                    mAnimData = new ResAnmChrCV8Data(file, numFrames);
                    break;
                case 5:
                    mAnimData = new ResAnmChrCV16Data(file, numFrames);
                    break;
                case 6:
                    mAnimData = new ResAnmChrCV32Data(file, numFrames);
                    break;
            }
        }

        ushort mNumFrames;
        float mInvKeyFrameRange;
        object mAnimData;
    }

    public class ResAnmChrAnmData
    {
        public ResAnmChrAnmData(MemoryFile file, Target target, uint type, bool isConst, ushort frameCount = 0)
        {
            if (isConst)
            {
                mAnim = file.ReadSingle();
                return;
            }
            else
            {
                mAnim = new ResAnmChrFVSData(file, type, frameCount);
            }
        }

        public ResAnmChrAnmData()
        {
            mAnim = 1.0f;
        }

        object mAnim;
    }

    public class ResAnmChrNodeData
    {
        public enum Target
        {
            ScaleX,
            ScaleY,
            ScaleZ,
            RotateX,
            RotateY,
            RotateZ,
            TranslateX,
            TranslateY,
            TranslateZ
        }

        public ResAnmChrNodeData(MemoryFile file, ushort numFrames)
        {
            int basePos = file.Position();
            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlags = file.ReadUInt32();

            mScaleUseModel = (mFlags & 0x80) != 0;
            mRotateUseModel = (mFlags & 0x100) != 0;
            mTranslateUseModel = (mFlags & 0x200) != 0;
            mScaleCompApply = (mFlags & 0x400) != 0;
            mScaleCompParent = (mFlags & 0x800) != 0;
            mClassicScaleOff = (mFlags & 0x1000) != 0;

            uint scaleType = (mFlags >> 25) & 0x3;
            uint rotateType = (mFlags >> 27) & 0x7;
            uint translateType = (mFlags >> 30) & 0x3;

            bool scaleOne = ((mFlags >> 3) & 0x1) != 0;
            bool scaleUniform = ((mFlags >> 4) & 0x1) != 0;

            bool rotateZero = ((mFlags >> 5) & 0x1) != 0;
            bool translateZero = ((mFlags >> 6) & 0x1) != 0;

            if (rotateZero)
            {

            }

            int loopStart = file.Position();
            int curDataIdx = 0;

            for (int i = 0; i < 9; i++)
            {
                if ((i >= 0 && i < 3) && scaleOne)
                {
                    mAnims.Add((Target)i, new());
                    continue;
                }

                if ((i >= 0 && i < 3) && scaleUniform)
                {
                    mAnims.Add((Target)i, null);
                    mAnims[(Target)i] = mAnims[Target.ScaleX];
                    continue;
                }

                if ((i >= 3 && i < 6) && rotateZero)
                {
                    mAnims.Add((Target)i, null);
                    continue;
                }

                if ((i >= 6 && i < 9) && translateZero)
                {
                    mAnims.Add((Target)i, null);
                    continue;
                }

                file.Seek(loopStart + (curDataIdx * 4));
                
                bool isConst = ((mFlags >> (13 + i)) & 0x1) != 0;

                if (!isConst)
                {
                    file.Seek(basePos + file.ReadInt32());
                }

                switch ((Target)i)
                {
                    case Target.ScaleX:
                    case Target.ScaleY:
                    case Target.ScaleZ:
                        mAnims.Add((Target)i, new(file, (Target)i, scaleType, isConst, numFrames));
                        break;
                    case Target.RotateX:
                    case Target.RotateY:
                    case Target.RotateZ:
                        mAnims.Add((Target)i, new(file, (Target)i, rotateType, isConst, numFrames));
                        break;
                    case Target.TranslateX:
                    case Target.TranslateY:
                    case Target.TranslateZ:
                        mAnims.Add((Target)i, new(file, (Target)i, translateType, isConst, numFrames));
                        break;
                }

                curDataIdx++;
            }
        }

        string mName;
        uint mFlags;

        bool mScaleUseModel;
        bool mRotateUseModel;
        bool mTranslateUseModel;
        bool mScaleCompApply;
        bool mScaleCompParent;
        bool mClassicScaleOff;

        Dictionary<Target, ResAnmChrAnmData> mAnims = new();
    }

    public class ResAnmChr : IResource
    {
        public ResAnmChr(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "CHR0")
            {
                throw new Exception("ResAnmChr::ResAnmChr(MemoryFile) -- Invalid CHR0 magic.");
            }

            file.Skip(4);
            mRevision = file.ReadUInt32();
            file.Skip(4);

            int chrDictOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();

            mName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            file.Skip(4);
            mInfo = new(file);

            if (chrDictOffs != 0)
            {
                file.Seek(chrDictOffs + basePos);
                ResDict chrDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in chrDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mAnims.Add(kvp.Key, new(file, mInfo.mNumFrame));
                }
            }
        }

        uint mRevision;
        string mName;
        ResAnmChrInfoData mInfo;
        Dictionary<string, ResAnmChrNodeData> mAnims = new();
    }
}
