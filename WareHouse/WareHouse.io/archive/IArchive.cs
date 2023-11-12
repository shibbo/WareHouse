using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io.archive
{
    public abstract class IArchive
    {
        public abstract byte[]? GetFileData(string fileName);
        public abstract string[]? GetFiles();
        public abstract string[]? GetFiles(string directory);
        public abstract string[]? GetFilesWithExt(string ext);
    }
}
