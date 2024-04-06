using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WareHouse.io.archive
{
    public class RARCArchive : IArchive
    {
        public RARCArchive(MemoryFile file)
        {
            string magic = file.ReadString(4);

            if (magic != "RARC")
            {
                throw new Exception("RARCArchive::RARCArchive() -- Invalid magic.");
            }

            mFile = file;
            file.Seek(0xC);
            int fileDataOffs = file.ReadInt32() + 0x20;
            file.Seek(0x20);
            int dirNodeCount = file.ReadInt32();
            int dirNodeOffset = file.ReadInt32() + 0x20;
            int fileEntryCount = file.ReadInt32();
            int fileEntryOffset = file.ReadInt32() + 0x20;
            file.Skip(4);
            int stringTableOffset = file.ReadInt32() + 0x20;
            file.Skip(4); // this is known now, will just implement later

            mDirectoryEntries = new();
            mFileEntries = new();

            DirectoryEntry rootEntry = new();
            rootEntry.mParent = null;

            file.Seek(dirNodeOffset + 0x6);
            int rootNodeOffs = file.ReadInt16();
            file.Seek(stringTableOffset + rootNodeOffs);
            rootEntry.mName = file.ReadString();
            rootEntry.mFullName = "/" + rootEntry.mName;
            rootEntry.mTempID = 0;

            mDirectoryEntries.Add("/", rootEntry);

            for (int i = 0; i < dirNodeCount; i++)
            {
                DirectoryEntry? parent = null;

                foreach (DirectoryEntry e in mDirectoryEntries.Values)
                {
                    if (e.mTempID == i)
                    {
                        parent = e;
                        break;
                    }
                }

                file.Seek(dirNodeOffset + (i * 0x10) + 0xA);

                short numEntries = file.ReadInt16();
                int firstEntryOffs = file.ReadInt32();

                for (int j = 0; j < numEntries; j++)
                {
                    int entryOffset = fileEntryOffset + ((j + firstEntryOffs) * 0x14);
                    file.Seek(entryOffset);
                    file.Skip(0x4);

                    int entryType = file.ReadInt16() & 0xFFFF;
                    int nameOffs = file.ReadInt16() & 0xFFFF;
                    int dataOffs = file.ReadInt32();
                    int dataSize = file.ReadInt32();

                    file.Seek(stringTableOffset + nameOffs);
                    string name = file.ReadString();

                    if (name == "." || name == "..")
                        continue;

                    if (parent == null)
                    {
                        throw new Exception("RARCArchive::RARCArchive() -- parent is null.");
                    }

                    string fullName = parent.mFullName + "/" + name;

                    if (entryType == 0x0200)
                    {
                        DirectoryEntry d = new DirectoryEntry
                        {
                            mParent = parent,
                            mName = name,
                            mFullName = fullName,
                            mTempID = dataOffs
                        };

                        mDirectoryEntries.Add(PathToKey(fullName), d);
                        parent.mChildren.Add(d);
                    }
                    else
                    {
                        FileEntry f = new()
                        {
                            mParent = parent,
                            mDataOffset = fileDataOffs + dataOffs,
                            mDataSize = dataSize,
                            mName = name,
                            mFullName = fullName,
                            mData = null
                        };

                        mFileEntries.Add(PathToKey(fullName), f);
                        parent.mChildrenFiles.Add(f);
                    }
                }
            }
        }

        private string PathToKey(string path)
        {
            string ret = path.ToLower();
            ret = ret.Substring(1);

            if (ret.IndexOf("/") == -1)
                return "/";

            return ret.Substring(ret.IndexOf("/"));
        }

        public override byte[]? GetFileData(string fileName)
        {
            var f = mFileEntries[PathToKey(fileName)];

            if (f.mData != null)
            {
                byte[] dest = new byte[f.mDataSize];
                Array.Copy(f.mData, dest, f.mDataSize);
                return dest;
            }

            mFile.Seek(f.mDataOffset);
            return mFile.ReadBytes(f.mDataSize);
        }

        public override string[]? GetFiles()
        {
            List<string> ret = new();

            DirectoryEntry d = mDirectoryEntries[PathToKey("/")];
            foreach (DirectoryEntry e in d.mChildren)
            {
                foreach(FileEntry fe in e.mChildrenFiles)
                {
                    if (fe.mName == null)
                        continue;

                    ret.Add($"{e.mFullName}/{fe.mName}");
                }   
            }

            return ret.ToArray();
        }

        public override string[]? GetFiles(string directory)
        {
            if (!mDirectoryEntries.ContainsKey(PathToKey(directory)))
                return null;

            DirectoryEntry d = mDirectoryEntries[PathToKey(directory)];

            List<string> ret = new();

            foreach (FileEntry e in d.mChildrenFiles)
            {
                if (e.mName == null)
                    continue;

                ret.Add(e.mName);
            }

            return ret.ToArray();
        }

        public override string[]? GetFilesWithExt(string ext)
        {
            string[]? files = GetFiles();
            List<string> ret = new();

            foreach (string f in files)
            {
                if (f.EndsWith(ext))
                {
                    ret.Add(f);
                }
            }

            return ret.ToArray();
        }

        private class FileEntry
        {
            public int mDataOffset;
            public int mDataSize;
            public DirectoryEntry? mParent;
            public string? mName;
            public string? mFullName;

            public byte[]? mData = null;
        }

        private class DirectoryEntry
        {

            public DirectoryEntry? mParent;
            public List<DirectoryEntry> mChildren = new();
            public List<FileEntry> mChildrenFiles = new();

            public string? mName;
            public string? mFullName;
            public int mTempID;
            public int mTempNameOffset;
        }

        private MemoryFile mFile;
        private Dictionary<string, FileEntry> mFileEntries;
        private Dictionary<string, DirectoryEntry> mDirectoryEntries;
    }
}
