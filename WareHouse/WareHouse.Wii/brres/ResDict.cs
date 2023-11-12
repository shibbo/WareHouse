using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using static WareHouse.Wii.bfres.BRRES;

namespace WareHouse.Wii.brres
{
    public class ResDict
    {
        public ResDict(MemoryFile file, bool ignoreRoot)
        {
            if (!ignoreRoot)
            {
                if (file.ReadString(4) != "root")
                {
                    throw new Exception("ResDict::ResDict(MemoryFile) -- 'root' magic for index group invalid.");
                }

                file.Skip(4);
            }

            int rootPos = file.Position();

            ResDictData root = new();
            root.Size = file.ReadUInt32();
            root.NumData = file.ReadUInt32();

            file.Skip(0x10);

            for (int i = 0; i < root.NumData; i++)
            {
                ResDictData node = new();
                node.Reference = file.ReadUInt16();
                node.Flag = file.ReadUInt16();
                node.IndexLeft = file.ReadUInt16();
                node.IndexRight = file.ReadUInt16();
                node.DataNameOffset = file.ReadInt32();
                node.DataOffset = rootPos + file.ReadInt32();

                int save = file.Position();
                file.Seek(rootPos + node.DataNameOffset - 4);
                int nameLen = file.ReadInt32();
                mDict.Add(file.ReadString(nameLen), node);
                file.Seek(save);
            }
        }

        public class ResDictData : IRes
        {
            public ResDictData() { }

            public uint Size;
            public uint NumData;
            public ushort Reference;
            public ushort Flag;
            public ushort IndexLeft;
            public ushort IndexRight;
            public int DataNameOffset;
            public int DataOffset;
        }

        public Dictionary<string, ResDictData> GetDictData()
        {
            return mDict;
        }

        Dictionary<string, ResDictData> mDict = new();
    }
}
