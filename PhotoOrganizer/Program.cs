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
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            /// Flags
            bool recursive = false;
            string directoryPath = "";
            string outputDirectory = "";
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case var path when (Directory.Exists(path) && i == 0):
                        directoryPath = path;
                        break;
                    case "-r":
                        recursive = true;
                        break;
                    case "-o" when (!args[i + 1].StartsWith('-')):
                        i++;
                        outputDirectory = args[i];
                        break;
                    default:
                        throw new Exception("Invalid arguments");
                }
            }

            Console.WriteLine($"Organizing directory {directoryPath}");
            if (recursive)
            {
                PhotoOrganizer.OrganizeByDateTakenRecursively(directoryPath, outputDirectory);
            }
            else
            {
                PhotoOrganizer.OrganizeByDateTaken(directoryPath, outputDirectory);
            }
        }
    }
}