using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io.util
{
    public class UVCoord
    {
        public UVCoord(FileBase file)
        {
            mTopLeft = new float[2];
            mTopRight = new float[2];
            mBottomLeft = new float[2];
            mBottomRight = new float[2];

            mTopLeft[0] = file.ReadSingle();
            mTopLeft[1] = file.ReadSingle();
            mTopRight[0] = file.ReadSingle();
            mTopRight[1] = file.ReadSingle();
            mBottomLeft[0] = file.ReadSingle();
            mBottomLeft[1] = file.ReadSingle();
            mBottomRight[0] = file.ReadSingle();
            mBottomRight[1] = file.ReadSingle();
        }

        float[] mTopLeft;
        float[] mTopRight;
        float[] mBottomLeft;
        float[] mBottomRight;
    }
}
