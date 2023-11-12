using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WareHouse.io;
using WareHouse.Wii.brres;

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

        private void LoadDictionaryData(ResDict dictionary, MemoryFile file)
        {
            foreach (var dict in dictionary.GetDictData())
            {
                file.Seek(dict.Value.DataOffset);
                uint magic = file.ReadUInt32();

                switch (magic)
                {
                    /* MDL0 */
                    case 0x4D444C30:
                        break;
                    /* TEX0 */
                    case 0x54455830:
                        break;
                    /* SRT0 */
                    case 0x53525430:
                        break;
                    /* CHR0 */
                    case 0x43485230:
                        break;
                    /* PAT0 */
                    case 0x50415430:
                        break;
                    /* CLR0 */
                    case 0x434C5230:
                        break;
                    /* SHP0 */
                    case 0x53485030:
                        break;
                    /* SCN0 */
                    case 0x53434E30:
                        break;
                    /* our default case is that this is a ResDict instead of another subfile type */
                    default:
                        /* go back 4 bytes since we read our data above */
                        file.Seek(file.Position() - 4);
                        ResDict subDict = new ResDict(file, true);
                        LoadDictionaryData(subDict, file);
                        break;
                }
            }
        }

        ResDict? mResourceDictionary;
        ushort mFileVersion;
    }
}
