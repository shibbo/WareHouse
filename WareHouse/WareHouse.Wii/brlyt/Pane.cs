using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brlyt
{
    public class Pane
    {
        public enum OriginType
        {
            Left = 0, Top = 0,
            Center = 1,
            Right = 2, Bottom = 2 
        }

        public Pane(MemoryFile file)
        {
            mSectionSize = file.ReadUInt32();

            byte flags = file.ReadByte();

            mIsVisible = (flags & 0x1) != 0;
            mIsInfluenceAlpha = (flags & 0x2) != 0;
            mIsWideScreen = (flags & 0x4) != 0;

            byte origin = file.ReadByte();
            mHorizOrigin = (OriginType)(origin % 3);
            mVertOrigin = (OriginType)(origin / 3);
            mAlpha = file.ReadByte();
            file.Skip(1);

            mPaneName = file.ReadString(0x10).Replace("\0", string.Empty);
            mUserData = file.ReadBytes(8);

            mTranslate = file.ReadVec3();
            mRotate = file.ReadVec3();
            mScale = file.ReadVec2();

            mWidth = file.ReadSingle();
            mHeight = file.ReadSingle();
        }

        public void SetParent(Pane pane)
        {
            mParent = pane;
        }

        public void AddChild(Pane pane)
        {
            mChildren.Add(pane);
        }
        
        public Pane? GetParent()
        {
            return mParent;
        }

        public uint mSectionSize;
        Pane? mParent;
        List<Pane> mChildren = new();
        bool mIsVisible;
        bool mIsInfluenceAlpha;
        bool mIsWideScreen;
        OriginType mHorizOrigin;
        OriginType mVertOrigin;
        byte mAlpha;
        string mPaneName;
        byte[]? mUserData;
        Vector3? mTranslate;
        Vector3? mRotate;
        Vector2? mScale;
        float mWidth;
        float mHeight;
    }
}
