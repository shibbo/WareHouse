using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io.util
{
    public class Matrix3x4
    {
        public Matrix3x4(MemoryFile file) 
        {
            m = new float[3][];

            for (int i = 0; i < 3; i++)
            {
                m[i] = new float[4];
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m[i][j] = file.ReadSingle();
                }
            }
        }

        float[][] m;
    }
}
