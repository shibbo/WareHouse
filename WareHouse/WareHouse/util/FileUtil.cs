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

            if (bytes.Length < 5)
            {
                isYaz0 = false;
            }
            else if (bytes[0] == 'Y' && bytes[1] == 'a' && bytes[2] == 'z' && bytes[3] == '0')
            {
                isYaz0 = true;
            }

            return isYaz0;
        }

        public static bool IsFileRARC(ref byte[] bytes)
        {
            if (bytes[0] == 'R' && bytes[1] == 'A' && bytes[2] == 'R' && bytes[3] == 'C')
            {
                return true;
            }

            return false;
        }

        public static bool IsFileU8(ref byte[] bytes)
        {
            if (bytes[0] == 0x55 && bytes[1] == 0xAA && bytes[2] == 0x38)
            {
                return true;
            }

            return false;
        }
    }
}
