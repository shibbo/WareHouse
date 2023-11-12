namespace WareHouse.io
{
    public class FileBase : IDisposable
    {
        /// <summary>
        /// Saves the current file.
        /// </summary>
        public virtual void Save() { }
        /// <summary>
        /// Closes the file stream.
        /// </summary>
        public virtual void Close() { }
        /// <summary>
        /// Releases the file.
        /// </summary>
        public virtual void Release() { }
        /// <summary>
        /// Set the flag determining if the binary reader / writer uses big or little endian.
        /// </summary>
        /// <param name="isBig">true if big endian, false if little endian</param>
        public virtual void SetBigEndian(bool isBig) { }
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <returns></returns>
        public virtual int GetLength() { return 0; }
        /// <summary>
        /// Sets the length of the file.
        /// </summary>
        /// <param name="len"></param>
        public virtual void SetLength(int len) { }
        /// <summary>
        /// Returns the current position of the file.
        /// </summary>
        /// <returns>The current position.</returns>
        public virtual int Position() { return 0; }
        /// <summary>
        /// Seeks to a given position in the file.
        /// </summary>
        /// <param name="where">Where to seek to.</param>
        public virtual void Seek(int where) { }
        /// <summary>
        /// Skips over an amount of bytes in the stream.
        /// </summary>
        /// <param name="numBytes">The number of bytes to skip.</param>
        public virtual void Skip(int numBytes) { }
        /// <summary>
        /// Compares the current FileBase to another type of file.
        /// </summary>
        /// <param name="file">The file to compare against.</param>
        /// <returns>true if the two files are equal. false if not.</returns>
        public virtual bool IsType(FileBase file) { return false; }
        /// <summary>
        /// Reads a single char from the stream.
        /// </summary>
        /// <returns>The read char.</returns>
        public virtual char ReadChar() { return (char)0; }
        /// <summary>
        /// Reads a single byte from the stream.
        /// </summary>
        /// <returns>The read byte.</returns>
        public virtual byte ReadByte() { return 0; }
        /// <summary>
        /// Reads a single 16-bit integer from the stream.
        /// </summary>
        /// <returns>The read integer.</returns>
        public virtual short ReadInt16() { return 0; }
        /// <summary>
        /// Reads a single 16-bit integer from the stream.
        /// </summary>
        /// <returns>The read integer.</returns>
        public virtual int ReadInt32() { return 0; }
        /// <summary>
        /// Reads a single 16-bit integer from the stream.
        /// </summary>
        /// <returns>The read integer.</returns>
        public virtual ushort ReadUInt16() { return 0; }
        /// <summary>
        /// Reads a single 16-bit integer from the stream.
        /// </summary>
        /// <returns>The read integer.</returns>
        public virtual uint ReadUInt32() { return 0; }
        /// <summary>
        /// Reads a single 32-bit integer from a given offset in the stream.
        /// </summary>
        /// <param name="loc">The location of the integer to read in the stream.</param>
        /// <returns>The read integer.</returns>
        public virtual int ReadInt32At(int loc) { return 0; }
        /// <summary>
        /// Reads a single 32-bit integer from a given offset in the stream.
        /// </summary>
        /// <param name="loc">The location of the integer to read in the stream.</param>
        /// <returns>The read integer.</returns>
        public virtual uint ReadUInt32At(int loc) { return 0; }
        /// <summary>
        /// Reads a given amount of bytes from the stream.
        /// </summary>
        /// <param name="num">The number of bytes to read.</param>
        /// <returns>The array of bytes read.</returns>
        public virtual byte[]? ReadBytes(int num) { return null; }
        /// <summary>
        /// Reads a single-precision floating point value from the stream.
        /// </summary>
        /// <returns>The float value.</returns>
        public virtual float ReadSingle() { return 0.0f; }
        public virtual string ReadStringLenPrefix() { return ""; }
        public virtual string ReadString() { return ""; }
        public virtual string ReadString(int len) { return ""; }
        public virtual string ReadStringAt(int loc) { return ""; }
        public virtual string ReadStringUTF16() { return ""; }
        public virtual void Write(byte val) { }
        public virtual void Write(char val) { }
        public virtual void Write(short val) { }
        public virtual void Write(int val) { }
        public virtual void Write(ushort val) { }
        public virtual void Write(uint val) { }
        public virtual void Write(float val) { }
        public virtual void Write(double val) { }
        public virtual void Write(byte[] val) { }
        public virtual void Write(char[] val) { }
        public virtual void WritePadding(byte padVal, int howMany) { }
        public virtual int WriteString(string val) { return 0; }
        public virtual int WriteStringNT(string val) { return 0; }

        public virtual byte[]? GetBuffer() { return null; }
        public virtual void SetBuffer(byte[] buffer) { }

        public void Dispose()
        {
            Close();
        }
    }
}
