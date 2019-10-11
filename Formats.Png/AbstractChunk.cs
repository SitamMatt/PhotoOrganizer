using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Formats.Png
{
    public abstract class AbstractChunk
    { 
        public AbstractChunk() { }
        public static AbstractChunk From(Stream stream)
        {
            AbstractChunk chunk;
            Span<byte> buffer = stackalloc byte[4];
            stream.Read(buffer);
            if (BitConverter.IsLittleEndian)
            {
                buffer.Reverse();
            }
            int length = (int)Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(buffer));
            stream.Read(buffer);
            string type = Encoding.ASCII.GetString(buffer);
            switch (type)
            {
                case "tEXt":
                    chunk = new TextualChunk();
                    break;
                default:
                    chunk = new RawChunk();
                    break;
            }
            chunk.Length = length;
            chunk.Type = type;
            chunk.HandleChunkData(stream);
            stream.Read(buffer);
            if (BitConverter.IsLittleEndian)
            {
                buffer.Reverse();
            }
            chunk.Crc = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(buffer));
            return chunk;
        }
        protected abstract void HandleChunkData(Stream stream);
        public int Length;
        public string Type;
        public uint Crc;
        protected abstract  byte[] GetDataBytes();
        public byte[] GetBytes()
        {
            byte[] result = new byte[12 + Length];
            //Span<byte> header = stackalloc byte[8];
            var l = (uint)Length;
            byte[] len = BitConverter.GetBytes(l);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(len);
            }
            len.CopyTo(result, 0);
            byte[] type = Encoding.ASCII.GetBytes(Type);
            type.CopyTo(result, 4);
            GetDataBytes().CopyTo(result, 8);
            byte[] crcBytes = BitConverter.GetBytes(Crc);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(crcBytes);
            }
            crcBytes.CopyTo(result, 8 + Length);
            return result;
        }
    }
}
