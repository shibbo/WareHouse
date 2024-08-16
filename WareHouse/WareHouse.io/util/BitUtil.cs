using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io.util
{
    public static class BitUtil
    {
        public static uint ExtractBits(uint input, int start, int length)
        {
            uint mask = 0;

            for (int i = start; i < start + length; i++)
            {
                mask |= (0x80000000 >> i);
            }

            return (input & mask) >> (32 - (start + length));
        }
    }
}
