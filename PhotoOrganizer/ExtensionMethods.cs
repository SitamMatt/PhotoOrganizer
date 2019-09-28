using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhotoOrganizer
{
    public static class ExtensionMethods
    {
        public static void Rename(this FileInfo fileinfo, string newName)
        {
            fileinfo.MoveTo(Path.Combine(fileinfo.Directory.FullName, newName + fileinfo.Extension));
        }
    }
}