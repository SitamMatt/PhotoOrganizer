using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhotoOrganizer
{
    public static class Utils
    {
        public static string GetRandomFilenameWithoutExtension() => Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

        public static string[] supportedFileExtensions = { ".jpg", ".png", ".jpeg" };
    }
}