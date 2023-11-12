using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WareHouse.io.archive
{
    public class U8Archive : IArchive
    {
        public class FileNode
        {
            public bool IsDirectory;
            public string? NodeName;
            public uint DataOffsetOrIdx;
            public uint DataSizeOrIdx;
            public List<FileNode> Children = new();
        }

        public struct U8File
        {
            public byte[] Data;
        }

        public U8Archive(MemoryFile file)
        {
            uint magic = file.ReadUInt32();

            if (magic != 0x55AA382D)
            {
                throw new Exception("U8Archive::U8Archive() -- Invalid magic.");
            }

            int firstNodeIdx = file.ReadInt32();
            int fullSize = file.ReadInt32();
            mDataOffs = file.ReadInt32();
            file.Skip(0x10);

            FileNode root = new();
            root.NodeName = "";
            /* skip the first 8 bytes */
            file.Skip(0x8);
            root.DataSizeOrIdx = file.ReadUInt32();
            /* the last child will tells us where our node table ends, and the string table starts */
            mStringPoolOffs = (int)(file.Position() + ((root.DataSizeOrIdx - 1) * 0xC));
            mCurrentIndex = 1;
            ReadDirectory(file, root);
        }

        public void ReadDirectory(MemoryFile stream, FileNode node)
        {
            while (mCurrentIndex < node.DataSizeOrIdx)
            {
                mCurrentIndex++;
                FileNode newNode = new();
                uint data = stream.ReadUInt32();
                newNode.IsDirectory = (int)(data >> 24) == 1;
                newNode.NodeName = $"{node.NodeName}/{stream.ReadStringAtNT(mStringPoolOffs + (int)(data & 0xFFFFFF))}";
                newNode.DataOffsetOrIdx = stream.ReadUInt32();
                newNode.DataSizeOrIdx = stream.ReadUInt32();

                if (newNode.IsDirectory)
                {
                    node.Children.Add(newNode);
                    ReadDirectory(stream, newNode);
                }
                else
                {
                    U8File file = new();
                    int save = stream.Position();
                    stream.Seek((int)(newNode.DataOffsetOrIdx));
                    byte[] bytes = stream.ReadBytes((int)newNode.DataSizeOrIdx);
                    stream.Seek(save);
                    file.Data = bytes;
                    mFiles.Add(newNode.NodeName, file);
                    node.Children.Add(newNode);
                }
            }
        }

        public override byte[]? GetFileData(string fileName)
        {
            if (mFiles.ContainsKey(fileName))
            {
                return mFiles[fileName].Data;
            }

            return null;
        }

        public override string[] GetFiles()
        {
            return mFiles.Keys.ToArray();
        }

        public override string[] GetFiles(string directory)
        {
            List<string> files = new();
            foreach (string file in mFiles.Keys)
            {
                if (file.StartsWith(directory))
                {
                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        public override string[] GetFilesWithExt(string ext)
        {
            List<string> files = new();
            foreach (string file in mFiles.Keys)
            {
                if (file.EndsWith(ext))
                {
                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        public int mStringPoolOffs;
        public int mDataOffs;
        private uint mCurrentIndex;
        public Dictionary<string, U8File> mFiles = new();
    }
}
