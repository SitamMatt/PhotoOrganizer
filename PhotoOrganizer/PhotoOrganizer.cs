using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static PhotoOrganizer.Utils;

namespace PhotoOrganizer
{
    public class PhotoOrganizer
    {
        public static void OrganizeByDateTakenRecursively(string directory, string output = "")
        {
            string[] entries = Directory.GetDirectories(directory);
            OrganizeByDateTaken(directory, output);
            foreach (string dir in entries)
            {
                string path = string.IsNullOrEmpty(output) ? "" : Path.Combine(output, Path.GetFileName(dir));
                OrganizeByDateTaken(dir, path);
                OrganizeByDateTakenRecursively(dir, path);
            }
        }

        public static void OrganizeByDateTaken(string directory, string output = "")
        {
            string flag = output;
            if (string.IsNullOrEmpty(output))
            {
                output = directory;
            }
            if (!string.IsNullOrEmpty(output) && !Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            string[] files = Directory.GetFiles(directory);
            List<FileInfo> fileInfos = new List<FileInfo>();
            for (int i = 0; i < files.Length; i++)
            {
                if (supportedFileExtensions.Any(ext => ext == Path.GetExtension(files[i])))
                {
                    fileInfos.Add(new FileInfo(files[i]));
                }
            }
            List<string> handledFiles = new List<string>();
            string format = "yyyy_MM_dd";

            for (int i = 0; i < fileInfos.Count; i++)
            {
                var fileinfo = fileInfos[i];
                if (supportedFileExtensions.Any(ext => ext == fileinfo.Extension))
                {
                    using FileStream fs = new FileStream(fileinfo.FullName, FileMode.Open);
                    Image image = Image.Load(fs);
                    ExifProfile exif = image.Metadata.ExifProfile;
                    ExifValue dateValue = exif.GetValue(ExifTag.DateTime);
                    var dateFormated = DateTime.ParseExact(dateValue.Value.ToString(), "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
                    string filename = dateFormated.ToString(format);
                    if (File.Exists(Path.Combine(output, filename + fileinfo.Extension)) && handledFiles.Contains(Path.Combine(output, filename + fileinfo.Extension).ToString()))
                    {
                        int index = handledFiles.Where(path => path.Contains(filename)).OrderBy(path => path).Count();
                        filename += $"_{index}";
                    }
                    fs.Close();
                    try
                    {
                        if (string.IsNullOrEmpty(flag))
                        {
                            fileinfo.Rename(filename);
                        }
                        else
                        {
                            handledFiles.Add(fileinfo.CopyTo(Path.Combine(output, filename + fileinfo.Extension), true).FullName);
                        }
                    }
                    catch (Exception e)
                    {
                        if (string.IsNullOrEmpty(flag))
                        {
                            FileInfo existingFile = fileInfos.Where(file => file.Name == (filename + fileinfo.Extension)).First();
                            existingFile.Rename(GetRandomFilenameWithoutExtension());
                            fileinfo.Rename(filename);
                        }
                        else
                        {
                            Console.WriteLine($"Error moving file to {Path.Combine(output, filename + fileinfo.Extension)} : {e.Message}");
                        }
                    }
                    handledFiles.Add(fileinfo.FullName);
                }
            }
        }
    }
}