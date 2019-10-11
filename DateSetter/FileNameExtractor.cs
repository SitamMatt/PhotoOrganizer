using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Image = SixLabors.ImageSharp.Image;
using MetadataExtractor;
using Formats.Png;

namespace DateSetter
{
    class FileNameExtractor : AbstarctDirectoryOperation
    {
        private string format;
        public FileNameExtractor(string format)
        {
            this.format = format;
        }
        private DateTime ExtractDate(string date)
        { 
            if(date.Length == format.Length)
            {
                List<char> dateChars = new List<char>();
                List<char> formatChars = new List<char>();
                for (int i = 0; i < date.Length; i++)
                {
                    if(date[i] != format[i])
                    {
                        dateChars.Add(date[i]);
                        formatChars.Add(format[i]);
                    }
                }
                DateTime result = DateTime.ParseExact(String.Concat(dateChars), String.Concat(formatChars), null);
                return result;
            }
            else
            {
                throw new Exception("Invalid format");
            }
        }
        protected override void Operation(string path, string output = "")
        {
            FileInfo fileInfo = new FileInfo(path);
            DateTime date = ExtractDate(Path.GetFileNameWithoutExtension(fileInfo.Name));
            string fileName = Path.Combine(output, fileInfo.Name); 
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                switch (fileInfo.Extension)
                {
                    case ".png":
                        var pngImage = PNGImage.From(fs);
                        pngImage.AddTextualData("Creation Time", date.ToString("yyyy:MM:dd HH:mm:ss"));
                        pngImage.Save(fileName);
                        break;
                    case ".jpg":
                    case ".tif":
                        var jpgImage = Image.Load(fs);
                        if (jpgImage.Metadata.ExifProfile == null)
                        {
                            jpgImage.Metadata.ExifProfile = new ExifProfile();
                        }
                        jpgImage.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, date.ToString("yyyy:MM:dd HH:mm:ss"));
                        jpgImage.Save(fileName);
                        break;
                    default:
                        throw new NotSupportedException();
                }
                fs.Close();
            }
        }
    }
}
