using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResByteCodeData
    {

        public ResByteCodeData(MemoryFile file)
        {
            Command.Mnemomic? curCmd = null;

            while ((curCmd = (Command.Mnemomic)file.ReadByte()) != Command.Mnemomic.RET)
            {
                switch (curCmd)
                {
                    case Command.Mnemomic.NODE_DESC:
                        mCommands.Add(new NodeDescCommand(file));
                        break;
                    case Command.Mnemomic.NODE_MIX:
                        mCommands.Add(new NodeMixCommand(file));
                        break;
                    case Command.Mnemomic.DRAW:
                        mCommands.Add(new NodeDrawCommand(file));
                        break;
                    case Command.Mnemomic.EVP_MTX:
                        mCommands.Add(new NodeEvpMtxCommand(file));
                        break;
                    case Command.Mnemomic.MTX_DUP:
                        mCommands.Add(new NodeMtxDuplicateCommand(file));
                        break;
                    case Command.Mnemomic.NOP:
                        break;
                }
            }
        }

        List<Command> mCommands = new();
    }

    public abstract class Command
    {
        public enum Mnemomic
        {
            NOP = 0,
            RET = 1,
            NODE_DESC = 2,
            NODE_MIX = 3,
            DRAW = 4,
            EVP_MTX = 5,
            MTX_DUP = 6
        }

        public Command(MemoryFile file)
        {

        }
    }

    public class NOPCommand : Command
    {
        public NOPCommand(MemoryFile file) : base(file) { }
    }

    public class RETCommand : Command
    {
        public RETCommand(MemoryFile file) : base(file) { }
    }

    public class NodeDescCommand : Command
    {
        public NodeDescCommand(MemoryFile file) : base(file)
        {
            mNodeID = file.ReadUInt16();
            mMatrixID = file.ReadUInt16();
        }

        ushort mNodeID;
        ushort mMatrixID;
    }

    public class NodeMixCommand : Command
    {
        public struct MtxRatio
        {
            public ushort ID;
            public float Ratio;
        }

        public NodeMixCommand(MemoryFile file) : base(file)
        {
            mTargetMtxID = file.ReadUInt16();
            mBlendMtxCount = file.ReadByte();

            for (ushort i = 0; i < mBlendMtxCount; i++)
            {
                MtxRatio mtx = new();
                mtx.ID = file.ReadUInt16();
                mtx.Ratio = file.ReadSingle();
                mMtxRatio.Add(mtx);
            }
        }

        ushort mTargetMtxID;
        byte mBlendMtxCount;
        List<MtxRatio> mMtxRatio = new();
    }
    
    public class NodeDrawCommand : Command
    {
        public NodeDrawCommand(MemoryFile file) : base(file)
        {
            mObjIdx = file.ReadUInt16();
            mMaterialIdx = file.ReadUInt16();
            mBoneIdx = file.ReadUInt16();
            mPriority = file.ReadByte();
        }

        ushort mObjIdx;
        ushort mMaterialIdx;
        ushort mBoneIdx;
        byte mPriority;
    }

    public class NodeEvpMtxCommand : Command
    {
        public NodeEvpMtxCommand(MemoryFile file) : base(file)
        {
            mMtxID = file.ReadUInt16();
            mWeightMtx = file.ReadUInt16();
        }

        ushort mMtxID;
        ushort mWeightMtx;
    }

    public class NodeMtxDuplicateCommand : Command
    {
        public NodeMtxDuplicateCommand(MemoryFile file) : base(file)
        {
            mToMtx = file.ReadUInt16();
            mFromMtx = file.ReadUInt16();
        }

        ushort mToMtx;
        ushort mFromMtx;
    }
}
