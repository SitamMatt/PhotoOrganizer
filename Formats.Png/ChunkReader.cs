using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Formats.Png
{
    class ChunkReader
    {
        Stream source;
        public ChunkReader(Stream stream)
        {
            source = stream;
        }

        public bool EndOfData;

        public AbstractChunk ReadNext()
        {
            long position = source.Position;
            AbstractChunk result = AbstractChunk.From(source);
            if(result.Type == "IEND")
            {
                EndOfData = true;
            }
            return result;
        }
    }
}
