using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brlyt
{
    public class TextBox : Pane
    {
        public TextBox(MemoryFile file) : base(file)
        {
            int startPos = file.Position() + 0x4C;

            mStringSize = file.ReadUInt16();
            mMaxStringSize = file.ReadUInt16();
            mMaterialIndex = file.ReadUInt16();
            mFontIndex = file.ReadUInt16();
            mStringOrigin = file.ReadByte();
            mLineAlignment = file.ReadByte();
            file.Skip(0x2);
            int text_offset = file.ReadInt32();
            mTopColor = new GXColor(file);
            mBottomColor = new GXColor(file);
            mFontSizeX = file.ReadSingle();
            mFontSizeY = file.ReadSingle();
            mCharSize = file.ReadSingle();
            mLineSize = file.ReadSingle();

            if (mStringSize != 0)
            {
                file.Seek(startPos + text_offset);
                mString = file.ReadStringUTF16().Replace("\0", "");
            }

            file.Seek(startPos + (int)mSectionSize);
        }

        ushort mStringSize;
        ushort mMaxStringSize;
        ushort mMaterialIndex;
        ushort mFontIndex;
        byte mStringOrigin;
        byte mLineAlignment;
        GXColor mTopColor;
        GXColor mBottomColor;
        float mFontSizeX;
        float mFontSizeY;
        float mCharSize;
        float mLineSize;
        string? mString;
    }
}
