using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace WareHouse.Wii.brres
{
    public class ResTex : IResource
    {
        public ResTex(MemoryFile file)
        {
            int basePos = file.Position();

            if (file.ReadString(4) != "TEX0")
            {
                throw new Exception("Texture::Texture(MemoryFile) -- Invalid header magic.");
            }

            uint fileSize = file.ReadUInt32();
            mRevision = file.ReadUInt32();
            file.Skip(4);
            mTexDataOffset = file.ReadInt32();
            mTextureName = file.ReadStringLenPrefixU32At(file.ReadInt32() + basePos - 4);
            mFlag = file.ReadUInt32();
            mWidth = file.ReadUInt16();
            mHeight = file.ReadUInt16();
            mFormat = (GXTexFmt)file.ReadUInt32();
            mMipMapLevel = file.ReadUInt32();
            mMinLOD = file.ReadSingle();
            mMaxLOD = file.ReadSingle();
            file.Skip(4);

            if (mRevision > 1)
            {
                mUserDataOffs = file.ReadUInt32();
            }

            int texLength = (int)((basePos + fileSize) - (basePos + mTexDataOffset));
            file.Seek(basePos + mTexDataOffset);

            mImageData = ImageFormat.DecodeImage(mFormat, file, mWidth, mHeight);
        }

        uint mRevision;
        int mTexDataOffset;
        public string mTextureName;
        uint mFlag;
        ushort mWidth;
        ushort mHeight;
        GXTexFmt mFormat;
        uint mMipMapLevel;
        float mMinLOD;
        float mMaxLOD;
        uint mUserDataOffs;
        byte[]? mImageData;
    }
}
