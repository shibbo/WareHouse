using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;
using WareHouse.Wii.brres.ModelRes;

namespace WareHouse.Wii.brres
{
    public class Model : IResource
    {
        public Model(MemoryFile file)
        {
            int basePos = file.Position();

            if (file.ReadString(4) != "MDL0")
            {
                throw new Exception("Model::Model(MemoryFile) -- Invalid header magic.");
            }

            file.Skip(4);
            uint revision = file.ReadUInt32();
            file.Skip(4);

            /* each of these offsets are to dictionaries that contain data on said section */
            int codeDataOffs = file.ReadInt32();
            int resNodeOffs = file.ReadInt32();
            int vtxPosOffs = file.ReadInt32();
            int vtxNrmOffs = file.ReadInt32();
            int vtxClrOffs = file.ReadInt32();
            int vtxTexCrdOffs = file.ReadInt32();

            int vtxFurVecOffs = 0;
            int vtxFurPosOffs = 0;

            if (revision >= 10)
            {
                vtxFurVecOffs = file.ReadInt32();
                vtxFurPosOffs = file.ReadInt32();
            }

            int matOffs = file.ReadInt32();
            int tevOffs = file.ReadInt32();
            int shpoffs = file.ReadInt32();
            int texNamePltOffs = file.ReadInt32();
            int pltToTexNameOffs = file.ReadInt32();
            int userDataOffs = file.ReadInt32();

            file.Seek(basePos + codeDataOffs);
            ResDict codeDataDict = new(file, true);

            foreach (KeyValuePair<string, ResDict.ResDictData> kvp in codeDataDict.GetDictData())
            {
                file.Seek(kvp.Value.DataOffset);
                mCodeData.Add(kvp.Key, new ResByteCodeData(file));
            }

            file.Seek(basePos + resNodeOffs);
            ResDict nodeDataDict = new(file, true);

            foreach (KeyValuePair<string, ResDict.ResDictData> kvp in nodeDataDict.GetDictData())
            {
                file.Seek(kvp.Value.DataOffset);
                mNodeData.Add(kvp.Key, new ResNodeData(file));
            }

            if (vtxPosOffs != 0)
            {
                file.Seek(basePos + vtxPosOffs);
                ResDict vtxPosDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in vtxPosDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mVtxPosData.Add(kvp.Key, new ResVtxPosData(file));
                }
            }

            if (vtxNrmOffs != 0)
            {
                file.Seek(basePos + vtxNrmOffs);
                ResDict vtxNrmDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in vtxNrmDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mVtxNrmData.Add(kvp.Key, new ResVtxNrmData(file));
                }
            }


            if (vtxClrOffs != 0)
            {
                file.Seek(basePos + vtxClrOffs);
                ResDict vtxClrDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in vtxClrDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mVtxClrData.Add(kvp.Key, new ResVtxClrData(file));
                }
            }
            
            if (vtxTexCrdOffs != 0)
            {
                file.Seek(basePos + vtxTexCrdOffs);
                ResDict vtxTexCoord = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in vtxTexCoord.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mVtxTexCoordData.Add(kvp.Key, new ResVtxTexCoordData(file));
                }
            }

            if (vtxFurVecOffs != 0)
            {
                file.Seek(basePos + vtxFurVecOffs);
                ResDict vtxFurVec = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in vtxFurVec.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mVtxFurData.Add(kvp.Key, new ResVtxFurVecData(file));
                }
            }

            // TODO -- Fur Layer Positions

            if (matOffs != 0)
            {
                file.Seek(basePos + matOffs);
                ResDict matDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in matDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mMatData.Add(kvp.Key, new ResMatData(file));
                }
            }

            if (shpoffs != 0)
            {
                file.Seek(basePos + shpoffs);
                ResDict shpDict = new(file, true);

                foreach (KeyValuePair<string, ResDict.ResDictData> kvp in shpDict.GetDictData())
                {
                    file.Seek(kvp.Value.DataOffset);
                    mShapeData.Add(kvp.Key, new(file));
                }
            }
        }

        Dictionary<string, ResByteCodeData> mCodeData = new();
        Dictionary<string, ResNodeData> mNodeData = new();
        Dictionary<string, ResVtxPosData> mVtxPosData = new();
        Dictionary<string, ResVtxNrmData> mVtxNrmData = new();
        Dictionary<string, ResVtxClrData> mVtxClrData = new();
        Dictionary<string, ResVtxTexCoordData> mVtxTexCoordData = new();
        Dictionary<string, ResVtxFurVecData> mVtxFurData = new();
        Dictionary<string, ResMatData> mMatData = new();
        Dictionary<string, ResShpData> mShapeData = new();
    }
}
