using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WareHouse.io;
using WareHouse.Wii.brres;
using WareHouse.Wii.brres.AnmRes;

namespace WareHouse.Wii.bfres
{
    public class BRRES : IModel
    {
        public enum SubType
        {
            ResDict,
            Scene
        }

        public BRRES(MemoryFile file)
        {
            if (file.ReadString(4) != "bres")
            {
                throw new Exception("BRRES::BRRES(MemoryFile) -- Invalid 'bres' header magic.");
            }

            if (file.ReadUInt16() != 0xFEFF)
            {
                throw new Exception("BRRES::BRRES(MemoryFile) -- Invalid endianess.");
            }

            mFileVersion = file.ReadUInt16();
            file.Skip(4);

            if (file.ReadUInt16() != 0x10)
            {
                throw new Exception("BRRES::BRRES(MemoryFile) -- Invalid header size.");
            }

            ushort dataBlockCount = file.ReadUInt16();
            mResourceDictionary = new ResDict(file, false);
            LoadDictionaryData(mResourceDictionary, file);
        }

        private void LoadDictionaryData(ResDict dictionary, MemoryFile file, string parent = "")
        {
            foreach (var dict in dictionary.GetDictData())
            {
                file.Seek(dict.Value.DataOffset);
                uint magic = file.ReadUInt32();

                /* go back 4 bytes since we read our data above */
                file.Seek(file.Position() - 4);

                if (parent != "" && !mSubFiles.ContainsKey(parent))
                {
                    mSubFiles.Add(parent, new());
                }

                switch (magic)
                {
                    /* MDL0 */
                    case 0x4D444C30:
                        mSubFiles[parent].Add(dict.Key, new ResMdl(file));
                        break;
                    /* TEX0 */
                    case 0x54455830:
                        mSubFiles[parent].Add(dict.Key, new ResTex(file));
                        break;
                    /* SRT0 */
                    case 0x53525430:
                        mSubFiles[parent].Add(dict.Key, new ResAnmTexSrt(file));
                        break;
                    /* CHR0 */
                    case 0x43485230:
                        break;
                    /* PAT0 */
                    case 0x50415430:
                        break;
                    /* CLR0 */
                    case 0x434C5230:
                        mSubFiles[parent].Add(dict.Key, new ResAnmClr(file));
                        break;
                    /* SHP0 */
                    case 0x53485030:
                        mSubFiles[parent].Add(dict.Key, new ResAnmShp(file));
                        break;
                    /* SCN0 */
                    case 0x53434E30:
                        mSubFiles[parent].Add(dict.Key, new ResAnmScn(file));
                        break;
                    /* VIS0 */
                    case 0x56495330:
                        mSubFiles[parent].Add(dict.Key, new ResAnmVis(file));
                        break;
                    /* our default case is that this is a ResDict instead of another subfile type */
                    default:
                        ResDict subDict = new ResDict(file, true);
                        LoadDictionaryData(subDict, file, dict.Key);
                        break;
                }
            }
        }

        ResDict? mResourceDictionary;
        ushort mFileVersion;
        Dictionary<string, Dictionary<string, IResource>> mSubFiles = new();
    }
}
