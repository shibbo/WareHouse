using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.SceneRes;

namespace WareHouse.Wii.brres
{
    public class Scene : IResource
    {
        public Scene(MemoryFile file)
        {
            int basePos = file.Position();
            if (file.ReadString(4) != "SCN0")
            {
                throw new Exception("Scene::Scene(MemoryFile) -- Invalid SCN0 magic.");
            }

            uint fileLength = file.ReadUInt32();
            uint revision = file.ReadUInt32();
            file.Skip(0x4);

            /* ResDict parsing */
            uint dictOffs = file.ReadUInt32();
            int save = file.Position();
            file.Seek((int)(basePos + dictOffs));
            mResourceDictionary = new ResDict(file, true);
            file.Seek(save);

            /* data array offsets */
            uint lightSetOffs = file.ReadUInt32();
            uint ambientLightOffs = file.ReadUInt32();
            uint lightDataOffs = file.ReadUInt32();
            uint fogDataOffs = file.ReadUInt32();
            uint cameraDataOffs = file.ReadUInt32();
            
            if (revision > 4)
            {
                uint userData = file.ReadUInt32();
            }

            uint sceneNameOffs = file.ReadUInt32();
            file.Skip(4);

            mSceneInfo = new();
            mSceneInfo.NumAnimFrames = file.ReadUInt16();
            mSceneInfo.SpecularLightCount = file.ReadUInt16();
            mSceneInfo.LoopSetting = file.ReadUInt32();
            mSceneInfo.NumLightSets = file.ReadUInt16();
            mSceneInfo.NumAmbLights = file.ReadUInt16();
            mSceneInfo.NumLights = file.ReadUInt16();
            mSceneInfo.NumFogs = file.ReadUInt16();
            mSceneInfo.NumCameras = file.ReadUInt16();

            file.Seek((int)(basePos + ambientLightOffs));
            for (ushort i = 0; i < mSceneInfo.NumAmbLights; i++)
            {
                ResAnmAmbLightData ambLight = new(file, mSceneInfo.NumAnimFrames);
                mAmbientLightData.Add(ambLight.mResourceName, ambLight);
            }

            file.Seek((int)(basePos + cameraDataOffs));
            for (ushort i = 0; i < mSceneInfo.NumCameras; i++)
            {
                ResAnmCameraData camera = new(file, mSceneInfo.NumAnimFrames);
                mCameraData.Add(camera.mResourceName, camera);
            }

            file.Seek((int)(basePos + fogDataOffs));
            for (ushort i = 0; i < mSceneInfo.NumFogs; i++)
            {
                ResAnmFogData fog = new(file, mSceneInfo.NumAnimFrames);
                mFogData.Add(fog.mResourceName, fog);
            }
        }

        public struct ResAnmScnInfoData
        {
            public ushort NumAnimFrames;
            public ushort SpecularLightCount;
            public uint LoopSetting;
            public ushort NumLightSets;
            public ushort NumAmbLights;
            public ushort NumLights;
            public ushort NumFogs;
            public ushort NumCameras;
        }

        ResAnmScnInfoData mSceneInfo;
        ResDict? mResourceDictionary;
        Dictionary<string, ResAnmAmbLightData> mAmbientLightData = new();
        Dictionary<string, ResAnmCameraData> mCameraData = new();
        Dictionary<string, ResAnmFogData> mFogData = new();
    }
}
