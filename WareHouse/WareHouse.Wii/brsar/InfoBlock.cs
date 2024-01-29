using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar
{
    public class InfoBlock
    {
        public InfoBlock(MemoryFile file)
        {
            if (file.ReadString(4) != "INFO")
            {
                throw new Exception("blah");
            }

            file.Skip(4);
            mInfo = new(file);
        }

        Info mInfo;
    }

    public class Info
    {
        public Info(MemoryFile file)
        {
            int basePos = file.Position();

            DataRef soundDataRef = new(file);
            DataRef soundBankRef = new(file);
            DataRef playerInfRef = new(file);
            DataRef collectionTableRef = new(file);
            DataRef groupTableRef = new(file);
            DataRef soundArchiveRef = new(file);

            file.Seek(basePos + (int)soundDataRef.GetValue());
            int count = file.ReadInt32();
            List<DataRef> soundInfoRefs = new(count);

            for (int i = 0; i < count; i++)
            {
                soundInfoRefs.Add(new(file));
            }

            foreach(DataRef r in soundInfoRefs)
            {
                file.Seek(basePos + (int)r.GetValue());
                mSoundInfo.Add(new(file, basePos));
            }
        }

        List<SoundCommonInfo> mSoundInfo = new();
    }

    public class Sound3DParam
    {
        public Sound3DParam(MemoryFile file)
        {
            mFlags = file.ReadUInt32();
            mDecayCurve = file.ReadByte();
            mDecayRatio = file.ReadByte();
            mDopplerFactor = file.ReadByte();
            file.Skip(5);
        }

        uint mFlags;
        byte mDecayCurve;
        byte mDecayRatio;
        byte mDopplerFactor;
    }

    public class SeqSoundInfo
    {
        public SeqSoundInfo(MemoryFile file)
        {
            mDataOffset = file.ReadUInt32();
            mBankID = file.ReadUInt32();
            mAllocTrack = file.ReadUInt32();
            mChannelPrio = file.ReadByte();
            mReleasePrioFix = file.ReadByte();
        }

        uint mDataOffset;
        uint mBankID;
        uint mAllocTrack;
        byte mChannelPrio;
        byte mReleasePrioFix;
    }

    public class StrmSoundInfo
    {
        public StrmSoundInfo(MemoryFile file)
        {
            mStartPosition = file.ReadUInt32();
            mAllocChannelCount = file.ReadUInt16();
            mAllocTrackFlag = file.ReadUInt16();
        }

        uint mStartPosition;
        ushort mAllocChannelCount;
        ushort mAllocTrackFlag;
    }

    public class WaveSoundInfo
    {
        public WaveSoundInfo(MemoryFile file)
        {
            mSubNo = file.ReadInt32();
            mAllocTrack = file.ReadUInt32();
            mChannelPrio = file.ReadByte();
            mReleasePrioFix = file.ReadByte();
        }

        int mSubNo;
        uint mAllocTrack;
        byte mChannelPrio;
        byte mReleasePrioFix;
    }

    public class SoundCommonInfo
    {
        public SoundCommonInfo(MemoryFile file, int basePos)
        {
            mStringID = file.ReadUInt32();
            mFileID = file.ReadUInt32();
            mPlayerID = file.ReadUInt32();

            DataRef param3DRef = new(file);

            mVolume = file.ReadByte();
            mPlayerPriority = file.ReadByte();
            mSoundType = file.ReadByte();
            mRemoteFilter = file.ReadByte();

            DataRef soundInfoRef = new(file);

            mUserData[0] = file.ReadUInt32();
            mUserData[1] = file.ReadUInt32();
            mPanMode = file.ReadByte();
            mPanCurve = file.ReadByte();
            mActorPlayerID = file.ReadByte();
            file.Skip(1);
            
            file.Seek(basePos + (int)param3DRef.GetValue());
            m3DParam = new(file);

            /* shouldn't happen but let's check anyways */
            if (mSoundType != (int)soundInfoRef.GetDataType())
            {
                throw new Exception("SoundCommonInfo::SoundCommonInfo(MemoryFile, int) -- SoundInfo DataRef type does not match common info's data type");
            }

            file.Seek(basePos + (int)soundInfoRef.GetValue());

            switch (mSoundType)
            {
                // sequence
                case 1:
                    mSoundInfo = new SeqSoundInfo(file);
                    break;
                // stream
                case 2:
                    mSoundInfo = new StrmSoundInfo(file);
                    break;
                // wave
                case 3:
                    mSoundInfo = new WaveSoundInfo(file);
                    break;
            }
        }

        uint mStringID;
        uint mFileID;
        uint mPlayerID;
        Sound3DParam m3DParam;
        byte mVolume;
        byte mPlayerPriority;
        byte mSoundType;
        byte mRemoteFilter;

        uint[] mUserData = new uint[2];
        byte mPanMode;
        byte mPanCurve;
        byte mActorPlayerID;
        object mSoundInfo;
    }
}
