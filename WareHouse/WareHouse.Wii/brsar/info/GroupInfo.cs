using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.Wii.brsar.info
{
    public class GroupInfo
    {
        int mStringID;
        int mEntryNum;
        uint mOffset;
        uint mSize;
        uint mWaveDataOffset;
        uint mWaveDataSize;
        List<GroupItemInfo> mItemInfo = new();
    }

    public class GroupItemInfo
    {
        int mFileID;
        uint mOffset;
        uint mSize;
        uint mWaveDataOffset;
        uint mWaveDataSize;
    }
}
