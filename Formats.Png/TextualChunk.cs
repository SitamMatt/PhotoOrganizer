using Force.Crc32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Formats.Png
{
    class TextualChunk : AbstractChunk
    {
        private void SetData(Stream stream)
        {

        }
        public TextualChunk(string keyword, string text)
        {
            Keyword = keyword;
            Text = text;
            Type = "tEXt";
            Encoding latin1 = Encoding.GetEncoding(28591);
            Span<byte> data = latin1.GetBytes(keyword + "\0" + text + "\0\0\0\0");
            Length = data.Length - 4;
            Encoding.ASCII.GetBytes(Type).CopyTo(data.Slice(Length, data.Length - Length));
            uint crc = Crc32Algorithm.Compute(data.ToArray());
            Crc = crc;
        }
        public TextualChunk()
        {

        }
        public TextualChunk(byte[] data)
        {

        }
        public TextualChunk(Stream stream)
        {
            Span<byte> buffer = stackalloc byte[4];
            stream.Read(buffer);
            if (BitConverter.IsLittleEndian)
            {
                buffer.Reverse();
            }
            int length = Convert.ToInt32(BitConverter.ToUInt32(buffer));
            stream.Read(buffer);
            string type = Encoding.ASCII.GetString(buffer);
            Span<byte> chunkData = stackalloc byte[length];
            stream.Read(chunkData);
            int i = 0;
            while(chunkData[i] != 0)
            {
                i++;
            }
            string keyword = Encoding.UTF8.GetString(chunkData.Slice(0, i));
            string text = Encoding.UTF8.GetString(chunkData.Slice(i + 1, length - i - 1));
            stream.Read(buffer);
            if (BitConverter.IsLittleEndian)
            {
                buffer.Reverse();
            }
            uint crc = BitConverter.ToUInt32(buffer);
            this.Crc = crc;
            this.Length = length;
            this.Type = type;
            this.Keyword = keyword;
            this.Text = text;
        }

        private string _keyword;

        public string Keyword
        {
            get
            {
                return _keyword;
            }

            set {
                Encoding latin1 = Encoding.GetEncoding(28591);
                if (latin1.GetBytes(value).Length < 80 )
                {
                    _keyword = value;
                }
                else
                {
                    throw new Exception("keyword length is longer than 79 bytes");
                }
            }
        }

        public string Text;

        public static TextualChunk CreateTEXTChunk(string keyword, string text)
        {
            throw new NotImplementedException();
        }

        protected override void HandleChunkData(Stream stream)
        {
            Span<byte> buffer = stackalloc byte[Length];
            stream.Read(buffer);
            int i = 0;
            while (buffer[i] != 0)
            {
                i++;
            }
            Keyword = Encoding.UTF8.GetString(buffer.Slice(0, i));
            Text = Encoding.UTF8.GetString(buffer.Slice(i + 1, Length - i - 1));
        }

        protected override byte[] GetDataBytes()
        {
            Encoding latin1 = Encoding.GetEncoding(28591);
            Span<byte> data = latin1.GetBytes(Keyword + "\0" + Text);
            return data.ToArray();
        }
    }
}
