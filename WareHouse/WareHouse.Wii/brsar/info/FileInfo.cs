using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brsar.info
{
    public class FileInfo
    {
        public FileInfo(MemoryFile file, int basePos)
        {
            mFileSize = file.ReadUInt32();
            mWaveDataFileSize = file.ReadUInt32();
            mEntryCount = file.ReadInt32();

            DataRef filePathRef = new(file);
            DataRef filePosTableRef = new(file);

            /* file path */
            file.Seek(basePos + (int)filePathRef.GetValue());
            mFilePath = file.ReadString();

            /* file pos table */
            file.Seek(basePos + (int)filePosTableRef.GetValue());
            int filePosEntryCount = file.ReadInt32();
            List<DataRef> filePosRefs = new();

            for (int i = 0; i < filePosEntryCount; i++)
            {
                filePosRefs.Add(new(file));
            }

            foreach (DataRef r in filePosRefs)
            {
                file.Seek(basePos + (int)r.GetValue());
                mFilePosArr.Add(new(file));
            }
        }

        uint mFileSize;
        uint mWaveDataFileSize;
        int mEntryCount;
        string mFilePath;
        List<FilePos> mFilePosArr = new();
    }

    public class FilePos
    {
        public FilePos(MemoryFile file)
        {
            mGroupID = file.ReadInt32();
            mIdx = file.ReadUInt32();
        }

        int mGroupID;
        uint mIdx;
    }
}
