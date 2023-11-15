using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.util
{
    public class FileUtil
    {
        public static bool IsFileYaz0(string path)
        {
            bool isYaz0 = false;

            byte[] bytes = File.ReadAllBytes(path);

            if (bytes[0] == 'Y' && bytes[1] == 'a' && bytes[2] == 'z' && bytes[3] == '0')
            {
                isYaz0 = true;
            }

            return isYaz0;
        }
    }
}
