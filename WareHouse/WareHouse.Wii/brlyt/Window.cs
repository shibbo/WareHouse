using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brlyt
{
    public class Window : Pane
    {
        public Window(MemoryFile file) : base(file)
        {
            int startPos = file.Position() - 0x4C;

            mCoords = new float[4];
            for (int i = 0; i < 4; i++)
            {
                mCoords[i] = file.ReadSingle();
            }

            mNumFrames = file.ReadByte();
            file.Skip(0x3);
            int windowContentOffs = file.ReadInt32();
            int windowFrameOffs = file.ReadInt32();
            mTopLeftColor = new GXColor(file);
            mTopRightColor = new GXColor(file);
            mBottomLeftColor = new GXColor(file);
            mBottomRightColor = new GXColor(file);
            mMaterialIndex = file.ReadUInt16();
            byte uvCount = file.ReadByte();
            mUnk7B = file.ReadByte();

            for (int i = 0; i < uvCount; i++)
            {
                mUVSets.Add(new UVCoord(file));
            }

            file.Seek(startPos + windowFrameOffs);

            List<int> frameOffsets = new List<int>();

            for (int i = 0; i < mNumFrames; i++)
            {
                frameOffsets.Add(file.ReadInt32());
            }

            foreach (int offs in frameOffsets)
            {
                file.Seek(startPos + offs);
                mWindowFrames.Add(new WindowFrame(file));
            }
        }

        float[] mCoords;
        byte mNumFrames;

        GXColor mTopLeftColor;
        GXColor mTopRightColor;
        GXColor mBottomLeftColor;
        GXColor mBottomRightColor;
        ushort mMaterialIndex;
        List<UVCoord> mUVSets = new List<UVCoord>();
        byte mUnk7B;
        List<WindowFrame> mWindowFrames = new List<WindowFrame>();
    }

    class WindowFrame
    {
        public WindowFrame(MemoryFile file)
        {
            mMaterialIndex = file.ReadUInt16();
            mFlipType = file.ReadByte();
            file.Skip(1);
        }

        ushort mMaterialIndex;
        byte mFlipType;
    }
}
