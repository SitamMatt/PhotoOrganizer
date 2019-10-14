using Formats.Png;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.IO;

namespace MDO.Operations.Windows.DateTaken
{
    public abstract class AbstractDateTakenSetter : AbstarctDirectoryOperation
    {
        protected abstract DateTime GetDateTime(FileInfo fileInfo);
        protected override void Operation(string path, string output = "")
        {
            FileInfo fileInfo = new FileInfo(path);
            string fileName = Path.Combine(output, fileInfo.Name);
            DateTime dateTime = GetDateTime(fileInfo);
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                switch (fileInfo.Extension)
                {
                    case ".png":
                    case ".PNG":
                        var pngImage = PNGImage.From(fs);
                        fs.Close();
                        pngImage.AddTextualData("Creation Time", dateTime.ToString("yyyy:MM:dd HH:mm:ss"));
                        pngImage.Save(fileName);
                        break;
                    case ".jpg":
                    case ".JPG":
                    case ".jpeg":
                        var jpgImage = Image.Load(fs);
                        fs.Close();
                        if (jpgImage.Metadata.ExifProfile == null)
                        {
                            jpgImage.Metadata.ExifProfile = new ExifProfile();
                        }
                        jpgImage.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, dateTime.ToString("yyyy:MM:dd HH:mm:ss"));
                        jpgImage.Save(fileName);
                        break;
                    default:
                        throw new NotSupportedException();
                }

            }
        }
    }
}
