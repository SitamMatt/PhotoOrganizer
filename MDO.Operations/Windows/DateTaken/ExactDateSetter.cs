using Formats.Png;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDO.Operations.Windows.DateTaken
{
    public class ExactDateSetter : AbstractDateTakenSetter
    {
        public ExactDateSetter(string dateString)
        {
            DateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd-HH-mm-ss", null);
        }

        private DateTime DateTime;
        //protected override void Operation(string path, string output = "")
        //{
        //    FileInfo fileInfo = new FileInfo(path);
        //    string fileName = Path.Combine(output, fileInfo.Name);
        //    using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
        //    {
        //        switch (fileInfo.Extension)
        //        {
        //            case ".png":
        //                var pngImage = PNGImage.From(fs);
        //                fs.Close();
        //                pngImage.AddTextualData("Creation Time", DateTime.ToString("yyyy:MM:dd HH:mm:ss"));
        //                pngImage.Save(fileName);
        //                break;
        //            case ".jpg":
        //            case ".JPG":
        //            case ".jpeg":
        //                var jpgImage = Image.Load(fs);
        //                fs.Close();
        //                if (jpgImage.Metadata.ExifProfile == null)
        //                {
        //                    jpgImage.Metadata.ExifProfile = new ExifProfile();
        //                }
        //                jpgImage.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, DateTime.ToString("yyyy:MM:dd HH:mm:ss"));
        //                jpgImage.Save(fileName);
        //                break;
        //            default:
        //                throw new NotSupportedException();
        //        }

        //    }
        //}

        protected override DateTime GetDateTime(FileInfo fileInfo)
        {
            return DateTime;
        }
    }
}
