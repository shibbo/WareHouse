using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io.archive
{
    public class Yaz0Archive
    {
        public static void Decompress(ref byte[] data)
        {
            if (data[0] != 'Y' || data[1] != 'a' || data[2] != 'z' || data[3] != '0')
                return;

            int fullsize = (data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7];
            byte[] output = new byte[fullsize];

            int inpos = 16, outpos = 0;
            while (outpos < fullsize)
            {
                byte block = data[inpos++];

                for (int i = 0; i < 8; i++)
                {
                    if ((block & 0x80) != 0)
                    {
                        // copy one plain byte
                        output[outpos++] = data[inpos++];
                    }
                    else
                    {
                        // copy N compressed bytes
                        byte b1 = data[inpos++];
                        byte b2 = data[inpos++];

                        int dist = ((b1 & 0xF) << 8) | b2;
                        int copysrc = outpos - (dist + 1);

                        int nbytes = b1 >> 4;
                        if (nbytes == 0) nbytes = data[inpos++] + 0x12;
                        else nbytes += 2;

                        for (int j = 0; j < nbytes; j++)
                            output[outpos++] = output[copysrc++];
                    }

                    block <<= 1;
                    if (outpos >= fullsize || inpos >= data.Length)
                        break;
                }
            }

            Array.Resize(ref data, fullsize);
            output.CopyTo(data, 0);
        }
    }
}
