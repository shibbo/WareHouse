﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.io
{
    public class MemoryFile : FileBase
    {
        public MemoryFile(byte[] buffer)
        {
            mBuffer = buffer;
            mPosition = 0;
            mIsBigEndian = true;
            mSize = buffer.Length;
            mEncoding = Encoding.ASCII;
        }

        public MemoryFile(byte[] buffer, Encoding encoding)
        {
            mBuffer = buffer;
            mPosition = 0;
            mIsBigEndian = true;
            mSize = buffer.Length;
            mEncoding = encoding;
        }

        public override void Release()
        {
            mBuffer = null;
            mSize = 0;
        }

        public override void SetBigEndian(bool isBig)
        {
            mIsBigEndian = isBig;
        }

        public override int GetLength()
        {
            return mSize;
        }

        public override void SetLength(int len)
        {
            Resize(len);
            mSize = len;
        }

        public override int Position()
        {
            return mPosition;
        }

        public override void Seek(int where)
        {
            mPosition = where;
        }

        public override void Skip(int numBytes)
        {
            mPosition += numBytes;
        }

        public override char ReadChar()
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::ReadChar() -- mBuffer is null.");
            }

            if (mPosition + sizeof(char) > GetLength())
                throw new Exception("MemoryFile::ReadChar() - Read is out of bounds.");

            return Convert.ToChar(mBuffer[mPosition++]);
        }

        public override byte ReadByte()
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::ReadByte() -- mBuffer is null.");
            }

            if (mPosition + sizeof(byte) > GetLength())
                throw new Exception("MemoryFile::ReadByte() - Read is out of bounds.");

            return mBuffer[mPosition++];
        }

        public override short ReadInt16()
        {
            if (mPosition + sizeof(short) > GetLength())
                throw new Exception("MemoryFile::ReadInt16() - Read is out of bounds.");

            byte[] val = ReadBytes(0x2);

            if (mIsBigEndian)
            {
                Array.Reverse(val);
            }

            return BitConverter.ToInt16(val, 0);
        }

        public override int ReadInt32()
        {
            if (mPosition + sizeof(int) > GetLength())
                throw new Exception("MemoryFile::ReadInt32() - Read is out of bounds.");

            byte[] val = ReadBytes(0x4);

            if (mIsBigEndian)
            {
                Array.Reverse(val);
            }

            return BitConverter.ToInt32(val, 0);
        }

        public override ushort ReadUInt16()
        {
            if (mPosition + sizeof(ushort) > GetLength())
                throw new Exception("MemoryFile::ReadUInt16() - Read is out of bounds.");

            byte[] val = ReadBytes(0x2);

            if (mIsBigEndian)
            {
                Array.Reverse(val);
            }

            return BitConverter.ToUInt16(val, 0);
        }

        public override uint ReadUInt32()
        {
            if (mPosition + sizeof(uint) > GetLength())
                throw new Exception("MemoryFile::ReadUInt32() - Read is out of bounds.");

            byte[] val = ReadBytes(0x4);

            if (mIsBigEndian)
            {
                Array.Reverse(val);
            }

            return BitConverter.ToUInt32(val, 0);
        }

        public override byte[] ReadBytes(int num)
        {
            if (mPosition + num > GetLength())
                throw new Exception("MemoryFile::ReadBytes() - Read is out of bounds.");

            byte[] output = new byte[num];

            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::ReadBytes() -- mBuffer is null.");
            }

            Array.Copy(mBuffer, mPosition, output, 0, num);
            mPosition += num;
            return output;
        }

        public override string ReadStringLenPrefix()
        {
            byte len = ReadByte();
            return ReadString(len);
        }

        public override string ReadStringLenPrefixU32()
        {
            int len = ReadInt32();
            return ReadString(len);
        }

        public override float ReadSingle()
        {
            if (mPosition + sizeof(float) > GetLength())
                throw new Exception("MemoryFile::ReadSingle() - Read is out of bounds.");

            byte[] output = ReadBytes(4);

            if (mIsBigEndian)
            {
                Array.Reverse(output);
                return BitConverter.ToSingle(output, 0);
            }

            return BitConverter.ToSingle(output, 0);
        }

        public override string ReadString()
        {
            List<byte> bytes = new List<byte>();
            byte b;
            while ((b = ReadByte()) != 0)
            {
                bytes.Add(b);
            }

            return mEncoding.GetString(bytes.ToArray());
        }

        public override string ReadString(int len)
        {
            return mEncoding.GetString(ReadBytes(len));
        }

        public override string ReadStringUTF16()
        {
            List<ushort> chars = [];

            while (true)
            {
                ushort val = ReadUInt16();

                if (val == 0)
                {
                    chars.Add(0);
                    break;
                }

                chars.Add(val);
            }

            byte[] dest = new byte[chars.Count * 2];
            System.Buffer.BlockCopy(chars.ToArray(), 0, dest, 0, chars.Count);

            return Encoding.Unicode.GetString(dest);
        }

        public override int ReadInt32At(int loc)
        {
            int ret;

            int oldLoc = Position();
            Seek(loc);
            ret = ReadInt32();
            Seek(oldLoc);
            return ret;
        }

        public override uint ReadUInt32At(int loc)
        {
            uint ret;

            int oldLoc = Position();
            Seek(loc);
            ret = ReadUInt32();
            Seek(oldLoc);
            return ret;
        }

        public override string ReadStringAt(int loc)
        {
            string ret;

            int oldLoc = Position();
            Seek(loc);
            byte len = ReadByte();
            ret = ReadString(len);
            Seek(oldLoc);
            return ret;
        }

        public string ReadStringAtNT(int loc)
        {
            string ret;

            int oldLoc = Position();
            Seek(loc);
            ret = ReadString();
            Seek(oldLoc);
            return ret;
        }

        public override string ReadStringLenPrefixU32At(int loc)
        {
            string ret;

            int oldLoc = Position();
            Seek(loc);
            ret = ReadStringLenPrefixU32();
            Seek(oldLoc);
            return ret;
        }

        public override Vector2? ReadVec2()
        {
            float x = ReadSingle();
            float y = ReadSingle();
            return new Vector2(x, y);
        }

        public override Vector3? ReadVec3()
        {
            float x = ReadSingle();
            float y = ReadSingle();
            float z = ReadSingle();
            return new Vector3(x, y, z);
        }

        public override void Write(byte val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(byte) -- mBuffer is null.");
            }

            Expand(mPosition + 1);
            mBuffer[mPosition++] = val;
        }

        public override void Write(char val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(char) -- mBuffer is null.");
            }

            Expand(mPosition + 1);
            mBuffer[mPosition++] = Convert.ToByte(val);
        }

        public override void Write(short val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(short) -- mBuffer is null.");
            }

            Expand(mPosition + 2);

            if (mIsBigEndian)
            {
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
            }
            else
            {
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
            }
        }

        public override void Write(int val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(int) -- mBuffer is null.");
            }

            Expand(mPosition + 4);
            if (mIsBigEndian)
            {
                mBuffer[mPosition++] = Convert.ToByte((val >> 24) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 16) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
            }
            else
            {
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 16) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 24) & 0xFF);
            }
        }

        public override void Write(ushort val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(ushort) -- mBuffer is null.");
            }

            Expand(mPosition + 2);

            if (mIsBigEndian)
            {
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
            }
            else
            {
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
            }
        }

        public override void Write(uint val)
        {
            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Write(uint) -- mBuffer is null.");
            }

            Expand(mPosition + 4);
            if (mIsBigEndian)
            {
                mBuffer[mPosition++] = Convert.ToByte((val >> 24) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 16) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
            }
            else
            {
                mBuffer[mPosition++] = Convert.ToByte(val & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 8) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 16) & 0xFF);
                mBuffer[mPosition++] = Convert.ToByte((val >> 24) & 0xFF);
            }
        }

        public override void Write(float val)
        {
            byte[] data = BitConverter.GetBytes(val);

            if (mIsBigEndian)
            {
                Array.Reverse(data);
            }

            Write(data);
        }

        public override void Write(double val)
        {
            byte[] data = BitConverter.GetBytes(val);

            if (mIsBigEndian)
            {
                Array.Reverse(data);
            }

            Write(data);
        }

        public override void Write(byte[] val)
        {
            foreach (byte b in val)
            {
                Write(b);
            }
        }

        public override void Write(char[] val)
        {
            foreach (byte b in val)
            {
                Write(b);
            }
        }

        public override int WriteString(string val)
        {
            int start = Position();
            byte[] data = mEncoding.GetBytes(val);

            Write(data);

            return (Position() - start);
        }

        public override int WriteStringNT(string val)
        {
            int start = Position();

            byte[] data = mEncoding.GetBytes(val);

            Write(data);
            Write('\0');

            return (Position() - start);
        }

        public override void WritePadding(byte padVal, int howMany)
        {
            for (int i = 0; i < howMany; i++)
                Write(padVal);
        }

        public override byte[]? GetBuffer()
        {
            return mBuffer;
        }

        public override void SetBuffer(byte[] buffer)
        {
            mBuffer = buffer;
        }

        private void Resize(int newSize)
        {
            if (newSize > 0)
            {
                Array.Resize(ref mBuffer, newSize);
            }
        }

        private void Expand(int newSize)
        {
            if (mSize < newSize)
            {
                mSize = newSize;
            }

            if (mBuffer == null)
            {
                throw new Exception("MemoryFile::Expand() -- mBuffer is null.");
            }

            if (mBuffer.Length < newSize)
            {
                Resize((mBuffer.Length > 0) ? mBuffer.Length * 2 : newSize);
            }
        }

        protected byte[]? mBuffer = null;
        private int mPosition;
        private bool mIsBigEndian;
        protected int mSize;
        readonly Encoding mEncoding;
    }
}
