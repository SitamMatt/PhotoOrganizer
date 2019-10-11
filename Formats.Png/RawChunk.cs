using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Formats.Png
{
    class RawChunk : AbstractChunk
    {
        public RawChunk()
        {

        }
        public RawChunk(byte[] data)
        {

        }
        public RawChunk(Stream stream)
        {

        }

        public byte[] data;

        protected override void HandleChunkData(Stream stream)
        {
            data = new byte[Length];
            stream.Read(data, 0, Length);
        }

        protected override byte[] GetDataBytes()
        {
            return data;
        }
    }
}
