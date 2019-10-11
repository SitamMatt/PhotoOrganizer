using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Formats.Png
{
    public class PNGImage
    {
        private List<AbstractChunk> Chunks;
        public static PNGImage From(Stream stream)
        {
            List<AbstractChunk> chunks = new List<AbstractChunk>();
            Span<byte> buffer = stackalloc byte[8];
            stream.Read(buffer);
            if (!IsPngFormat(buffer))
            {
                throw new Exception("Not png format");
            }

            ChunkReader reader = new ChunkReader(stream);
            while (!reader.EndOfData)
            {
                chunks.Add(reader.ReadNext());
            }
            return new PNGImage()
            {
                Chunks = chunks
            };
        }

        private static bool IsPngFormat(ReadOnlySpan<byte> signature)
        {
            Span<byte> sequence = stackalloc byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
            return signature.SequenceEqual(sequence);
        }

        public void AddTextualData(string keyword, string text)
        {
            TextualChunk textualChunk = new TextualChunk(keyword, text);
            Chunks.Insert(Chunks.Count-2, textualChunk);
        }

        public void RemoveTextualData(string keyword)
        {
            int indexToRemove = Chunks.FindIndex(chunk => chunk.Type == "tEXt" && (chunk as TextualChunk).Keyword == keyword);
            if(indexToRemove != -1)
            {
                Chunks.RemoveAt(indexToRemove);
            }
        }

        public void Save(string path)
        {
            using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.SetLength(0);
                Span<byte> sequence = stackalloc byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
                fs.Write(sequence);
                for (int i = 0; i < Chunks.Count; i++)
                {
                    fs.Write(Chunks[i].GetBytes());
                }
                fs.Close();
            }
        }
    }
}
