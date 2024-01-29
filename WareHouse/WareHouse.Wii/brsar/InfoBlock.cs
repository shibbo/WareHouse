using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brsar.info;

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
}
