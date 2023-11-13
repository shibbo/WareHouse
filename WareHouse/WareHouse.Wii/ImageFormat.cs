using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii
{
    public static class ImageFormat
    {
        public static byte[]? DecodeImage(GXTexFmt format, MemoryFile file, uint width, uint height)
        {
            switch (format)
            {
                case GXTexFmt.GX_TF_I8:
                    return DecodeI8(file, width, height);
                case GXTexFmt.GX_TF_IA8:
                    return DecodeIA8(file, width, height);
                case GXTexFmt.GX_TF_CMPR:
                    return DecodeCMPR(file, width, height);
            }

            return null;
        }

        private static byte[] DecodeI8(MemoryFile file, uint width, uint height)
        {
            uint numBlocksWidth = width / 8;
            uint numBlocksHeight = height / 4;

            byte[] output = new byte[width * height * 4];

            for (int yBlock = 0; yBlock < numBlocksHeight; yBlock++)
            {
                for (int xBlock = 0; xBlock < numBlocksWidth; xBlock++)
                {
                    for (int pY = 0; pY < 4; pY++)
                    {
                        for (int pX = 0; pX < 8; pX++)
                        {
                            if ((xBlock * 8 + pX >= width) || (yBlock * 4 + pY >= height))
                            {
                                continue;
                            }

                            byte data = file.ReadByte();
                            uint idx = (uint)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 8) + pX));

                            output[idx] = data;
                            output[idx + 1] = data;
                            output[idx + 2] = data;
                            output[idx + 3] = data;
                        }
                    }
                }
            }

            return output;
        }

        private static byte[] DecodeIA8(MemoryFile file, uint width, uint height)
        {
            uint numBlocksWidth = width / 4;
            uint numBlocksHeight = height / 4;

            byte[] decode = new byte[width * height * 4];

            for (int yBlock = 0; yBlock < numBlocksHeight; yBlock++)
            {
                for (int xBlock = 0; xBlock < numBlocksWidth; xBlock++)
                {
                    for (int pY = 0; pY < 4; pY++)
                    {
                        for (int pX = 0; pX < 4; pX++)
                        {
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                            {
                                continue;
                            }

                            uint destIndex = (uint)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX));
                            byte byte0 = file.ReadByte();
                            byte byte1 = file.ReadByte();
                            decode[destIndex + 3] = byte0;
                            decode[destIndex + 2] = byte1;
                            decode[destIndex + 1] = byte1;
                            decode[destIndex + 0] = byte1;
                        }
                    }
                }
            }

            return decode;
        }

        private static byte[] DecodeCMPR(MemoryFile file, uint width, uint height)
        {
            byte[] buffer = new byte[width * height * 4];

            for (int y = 0; y < height / 4; y += 2)
            {
                for (int x = 0; x < width / 4; x += 2)
                {
                    for (int dy = 0; dy < 2; ++dy)
                    {
                        for (int dx = 0; dx < 2; ++dx)
                        {
                            if (4 * (x + dx) < width && 4 * (y + dy) < height)
                            {
                                byte[] data = file.ReadBytes(8);
                                Buffer.BlockCopy(data, 0, buffer, (int)(8 * ((y + dy) * width / 4 + x + dx)), 8);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < width * height / 2; i += 8)
            {
                Swap(ref buffer[i], ref buffer[i + 1]);
                Swap(ref buffer[i + 2], ref buffer[i + 3]);

                buffer[i + 4] = S3TC1Reverse(buffer[i + 4]);
                buffer[i + 5] = S3TC1Reverse(buffer[i + 5]);
                buffer[i + 6] = S3TC1Reverse(buffer[i + 6]);
                buffer[i + 7] = S3TC1Reverse(buffer[i + 7]);
            }

            return DecompressDXT1(buffer, width, height);
        }

        private static byte[] DecompressDXT1(byte[] src, uint width, uint height)
        {
            uint dataOffs = 0;
            byte[] decompressedData = new byte[width * height * 4];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    /* two colors in RGB565 format */
                    ushort _1 = Swap16(src, dataOffs);
                    ushort _2 = Swap16(src, dataOffs + 2);
                    uint data = Swap32(src, dataOffs + 4);
                    dataOffs += 8;

                    byte[][] tbl = new byte[4][];
                    for (uint i = 0; i < 4; i++)
                    {
                        tbl[i] = new byte[4];
                    }

                    RGB565ToRGBA8(_1, ref tbl[0], 0);
                    RGB565ToRGBA8(_2, ref tbl[1], 0);

                    if (_1 > _2)
                    {
                        tbl[2][0] = (byte)((2 * tbl[0][0] + tbl[1][0] + 1) / 3);
                        tbl[2][1] = (byte)((2 * tbl[0][1] + tbl[1][1] + 1) / 3);
                        tbl[2][2] = (byte)((2 * tbl[0][2] + tbl[1][2] + 1) / 3);
                        tbl[2][3] = 0xFF;

                        tbl[3][0] = (byte)((tbl[0][0] + 2 *  tbl[1][0] + 1) / 3);
                        tbl[3][1] = (byte)((tbl[0][1] + 2 * tbl[1][1] + 1) / 3);
                        tbl[3][2] = (byte)((tbl[0][2] + 2 * tbl[1][2] + 1) / 3);
                        tbl[3][3] = 0xFF;
                    }
                    else
                    {
                        tbl[2][0] = (byte)((tbl[0][0] + tbl[1][0] + 1) / 2);
                        tbl[2][1] = (byte)((tbl[0][1] + tbl[1][1] + 1) / 2);
                        tbl[2][2] = (byte)((tbl[0][2] + tbl[1][2] + 1) / 2);
                        tbl[2][3] = 0xFF;

                        tbl[3][0] = (byte)((tbl[0][0] + 2 * tbl[1][0] + 1) / 3);
                        tbl[3][1] = (byte)((tbl[0][1] + 2 * tbl[1][1] + 1) / 3);
                        tbl[3][2] = (byte)((tbl[0][2] + 2 * tbl[1][2] + 1) / 3);
                        tbl[3][3] = 0;
                    }

                    for (int iy = 0; iy < 4; ++iy)
                    {
                        for (int ix = 0; ix < 4; ++ix)
                        {
                            if (((x + ix) < width) && ((y + iy) < height))
                            {
                                int di = (int)(4 * ((y + iy) * width + x + ix));
                                int si = (int)(data & 3);
                                decompressedData[di] = tbl[si][0];
                                decompressedData[di + 1] = tbl[si][1];
                                decompressedData[di + 2] = tbl[si][2];
                                decompressedData[di + 3] = tbl[si][3];
                            }

                            data >>= 2;
                        }
                    }
                }
            }

            return decompressedData;
        }

        private static void RGB565ToRGBA8(ushort src, ref byte[] dest, int destOffs)
        {
            byte r, g, b;
            r = (byte)((src & 0xF100) >> 11);
            g = (byte)((src & 0x7E0) >> 5);
            b = (byte)((src & 0x1F));

            r = (byte)((r << 3) | (r >> 2));
            g = (byte)((g << 2) | (g >> 4));
            b = (byte)((b << 3) | (b >> 2));

            dest[destOffs] = r;
            dest[destOffs + 1] = g;
            dest[destOffs + 2] = b;
            dest[destOffs + 3] = 0xFF;
        }

        private static void Swap(ref byte _1, ref byte _2)
        {
            byte t = _1;
            _1 = _2;
            _2 = t;
        }

        private static ushort Swap16(byte[] data, uint offs)
        {
            return (ushort)((Buffer.GetByte(data, (int)offs + 1) << 8) | Buffer.GetByte(data, (int)offs));
        }

        private static uint Swap32(byte[] data, uint offs)
        {
            return (uint)((Buffer.GetByte(data, (int)offs + 3) << 24) | (Buffer.GetByte(data, (int)offs + 2) << 16) | (Buffer.GetByte(data, (int)offs + 1) << 8) | Buffer.GetByte(data, (int)offs));
        }

        private static byte S3TC1Reverse(byte data)
        {
            return (byte)(((data & 3) << 6) | ((data & 0xC) << 2) | ((data & 0x30) >> 2) | ((data & 0xC0) >> 6));
        }
    }
}
