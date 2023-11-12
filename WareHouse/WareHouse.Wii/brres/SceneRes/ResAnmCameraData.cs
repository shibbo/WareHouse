using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.AnmRes;

namespace WareHouse.Wii.brres.SceneRes
{
    public class ResAnmCameraData : IResourceBase
    {
        const int FLAG_CAM_TYPE = (1 << 0);
        const int FLAG_POS_X = (1 << 17);
        const int FLAG_POS_Y = (1 << 18);
        const int FLAG_POS_Z = (1 << 19);
        const int FLAG_ASPECT = (1 << 20);
        const int FLAG_NEAR = (1 << 21);
        const int FLAG_FAR = (1 << 22);
        const int FLAG_PERSPFOVY = (1 << 23);
        const int FLAG_ORTHOHEIGHT = (1 << 24);
        const int FLAG_AIM_X = (1 << 25);
        const int FLAG_AIM_Y = (1 << 26);
        const int FLAG_AIM_Z = (1 << 27);
        const int FLAG_TWIST = (1 << 28);
        const int FLAG_ROT_X = (1 << 29);
        const int FLAG_ROT_Y = (1 << 30);
        const int FLAG_ROT_Z = (1 << 31);

        enum CameraType
        {
            CameraType_Rotate = 0,
            CameraType_Aim = 1
        };

        public ResAnmCameraData(MemoryFile file, ushort numFrames) : base(file)
        {
            mProjectionType = file.ReadUInt32();
            mFlags = file.ReadUInt32();
            mUserDataOffs = file.ReadUInt32();

            mCameraType = (CameraType)(mFlags & FLAG_CAM_TYPE);
            mXPosition = new ResAnmData(file, (mFlags & FLAG_POS_X) != 0);
            mYPosition = new ResAnmData(file, (mFlags & FLAG_POS_Y) != 0);
            mZPosition = new ResAnmData(file, (mFlags & FLAG_POS_Z) != 0);
            mAspect = new ResAnmData(file, (mFlags & FLAG_ASPECT) != 0);
            mNear = new ResAnmData(file, (mFlags & FLAG_NEAR) != 0);
            mFar = new ResAnmData(file, (mFlags & FLAG_FAR) != 0);
            mXRotation = new ResAnmData(file, (mFlags & FLAG_ROT_X) != 0);
            mYRotation = new ResAnmData(file, (mFlags & FLAG_ROT_Y) != 0);
            mZRotation = new ResAnmData(file, (mFlags & FLAG_ROT_Z) != 0);
            mXAim = new ResAnmData(file, (mFlags & FLAG_AIM_X) != 0);
            mYAim = new ResAnmData(file, (mFlags & FLAG_AIM_Y) != 0);
            mZAim = new ResAnmData(file, (mFlags & FLAG_AIM_Z) != 0);
            mTwist = new ResAnmData(file, (mFlags & FLAG_TWIST) != 0);
            mPerspFovy = new ResAnmData(file, (mFlags & FLAG_PERSPFOVY) != 0);
            mOrthoHeight = new ResAnmData(file, (mFlags & FLAG_ORTHOHEIGHT) != 0);
        }

        uint mProjectionType;
        uint mFlags;
        uint mUserDataOffs;
        CameraType mCameraType;
        ResAnmData mXPosition;
        ResAnmData mYPosition;
        ResAnmData mZPosition;
        ResAnmData mAspect;
        ResAnmData mNear;
        ResAnmData mFar;
        ResAnmData mXRotation;
        ResAnmData mYRotation;
        ResAnmData mZRotation;
        ResAnmData mXAim;
        ResAnmData mYAim;
        ResAnmData mZAim;
        ResAnmData mTwist;
        ResAnmData mPerspFovy;
        ResAnmData mOrthoHeight;
    }
}
