using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii
{
    public class TPL : IImageContainer
    {

        public TPL(MemoryFile file)
        {
            if (file.ReadUInt32() != 0x20AF30)
            {
                throw new Exception("TPL::TPL(MemoryFile) -- Invalid magic.");
            }

            uint numImages = file.ReadUInt32();

            if (numImages > 1)
            {
                throw new Exception("TPL::TPL(MemoryFile) -- More than 1 image. Not quite supported yet.");
            }

            file.Skip(4);
            int startOff = 0xC;

            for (int i = 0; i < numImages; i++)
            {
                file.Seek(startOff + (i * 4));

                int offs = file.ReadInt32();
                file.Seek(offs);
                mImages.Add(new(file));
            }
        }

        public byte[]? GetImageData(int idx)
        {
            return mImages[idx].GetImageData();
        }

        List<TPLImage> mImages = new();
    }

    public class TPLImage
    {
        public TPLImage(MemoryFile file)
        {
            mHeight = file.ReadUInt16();
            mWidth = file.ReadUInt16();
            mFormat = (GXTexFmt)file.ReadUInt32();

            int dataOffs = file.ReadInt32();

            mWrapS = file.ReadUInt32();
            mWrapT = file.ReadUInt32();
            mMinFilt = file.ReadUInt32();
            mMagFilt = file.ReadUInt32();
            mLODBias = file.ReadSingle();
            mEdgeLODEnable = file.ReadByte();
            mMinLOD = file.ReadByte();
            mMaxLOD = file.ReadByte();

            file.Seek(dataOffs);

            mImageData = ImageFormat.DecodeImage(mFormat, file, mWidth, mHeight);
        }

        public byte[]? GetImageData()
        {
            return mImageData;
        }

        ushort mHeight;
        ushort mWidth;
        GXTexFmt mFormat;
        uint mWrapS;
        uint mWrapT;
        uint mMinFilt;
        uint mMagFilt;
        float mLODBias;
        byte mEdgeLODEnable;
        byte mMinLOD;
        byte mMaxLOD;
        byte[]? mImageData;
    }
}
