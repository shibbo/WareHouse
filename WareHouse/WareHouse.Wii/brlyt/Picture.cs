using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brlyt
{
    public class Picture : Pane
    {
        public Picture(MemoryFile file) : base(file)
        {
            mTopLeftColor = new GXColor(file);
            mTopRightColor = new GXColor(file);
            mBottomLeftColor = new GXColor(file);
            mBottomRightColor = new GXColor(file);
            mMaterialIndex = file.ReadUInt16();
            byte uv_count = file.ReadByte();
            file.Skip(1);

            for (int i = 0; i < uv_count; i++)
            {
                mUVs.Add(new(file));
            }
        }

        GXColor mTopLeftColor;
        GXColor mTopRightColor;
        GXColor mBottomLeftColor;
        GXColor mBottomRightColor;
        ushort mMaterialIndex;
        List<UVCoord> mUVs = new();
    }
}
