using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.io.util;

namespace WareHouse.Wii.brres.ModelRes
{
    public class ResNodeData
    {
        public enum Billboard
        {
            Off = 0,
            STD = 1,
            PerspectiveSTD = 2,
            Rot = 3,
            PerspectiveRot = 4,
            Y = 5,
            PerspectiveY = 6
        }
        public ResNodeData(MemoryFile file)
        {
            int basePos = file.Position();
            file.Skip(8);
            mNodeName = file.ReadStringLenPrefixU32At(basePos + file.ReadInt32() - 4);
            mNodeID = file.ReadUInt32();
            mMtxID = file.ReadUInt32();
            mFlags = file.ReadUInt32();
            mBillboard = (Billboard)file.ReadUInt32();
            mBillboardRefNodeID = file.ReadUInt32();

            mScale = file.ReadVec3();
            mRotation = file.ReadVec3();
            mTranslation = file.ReadVec3();

            mVolumeMin = file.ReadVec3();
            mVolumeMax = file.ReadVec3();

            int parentNodeOffs = file.ReadInt32();
            int childNodeOffs = file.ReadInt32();
            int nextSiblingOffs = file.ReadInt32();
            int prevSiblingOffs = file.ReadInt32();
            mUserDataOffs = file.ReadInt32();

            mModelMtx = new(file);
            mInvModelMtx = new(file);

            /* let's just read the strings in the nodes to store our parent / children / siblings */
            /* our new offsets are based on the node we are querying */
            if (parentNodeOffs != 0)
            {
                int nodeBase = basePos + parentNodeOffs;
                int strOffs = file.ReadInt32At(basePos + parentNodeOffs + 0x8);
                mParent = file.ReadStringLenPrefixU32At(nodeBase + strOffs - 4);
            }

            if (childNodeOffs != 0)
            {
               
                int nodeBase = basePos + childNodeOffs;
                int strOffs = file.ReadInt32At(basePos + childNodeOffs + 0x8);
                mChild = file.ReadStringLenPrefixU32At(nodeBase + strOffs - 4);
            }

            if (nextSiblingOffs != 0)
            {
                int nodeBase = basePos + nextSiblingOffs;
                int strOffs = file.ReadInt32At(basePos + nextSiblingOffs + 0x8);
                mNextSibling = file.ReadStringLenPrefixU32At(nodeBase + strOffs - 4);
            }

            if (prevSiblingOffs != 0)
            {
                int nodeBase = basePos + prevSiblingOffs;
                int strOffs = file.ReadInt32At(basePos + prevSiblingOffs + 0x8);
                mPrevSibling = file.ReadStringLenPrefixU32At(nodeBase + strOffs - 4);
            }
        }

        public string mNodeName;
        public uint mNodeID;
        public uint mMtxID;
        public uint mFlags;
        Billboard mBillboard;
        public uint mBillboardRefNodeID;
        Vector3? mScale;
        Vector3? mRotation;
        Vector3? mTranslation;
        Vector3? mVolumeMin;
        Vector3? mVolumeMax;
        int mUserDataOffs;
        Matrix3x4 mModelMtx;
        Matrix3x4 mInvModelMtx;

        string mParent = "";
        string mChild = "";
        string mPrevSibling = "";
        string mNextSibling = "";
    }
}
