using MDO.Operations;
using MDO.Operations.Windows.DateTaken;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace MDO.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirName = ".";
            string match = "*.*";
            bool recursive = false;
            string output = "";
            DateTime date;
            string exclude = "";
            AbstarctDirectoryOperation operation = new Indentity();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case var path when Directory.Exists(path):
                        dirName = path;
                        break;
                    case "-m" when !args[i + 1].StartsWith("-"):
                        match = args[i + 1];
                        i++;
                        break;
                    case "-r":
                        recursive =true;
                        break;
                    case "-o" when !string.IsNullOrEmpty(Path.GetFullPath(args[i + 1])):
                        output = args[i + 1];
                        i++;
                        break;
                    case "-e" when !args[i + 1].StartsWith("-"):
                        exclude = args[i + 1];
                        i++;
                        break;
                    case "--exact-date" when DateTime.TryParseExact(args[i + 1], "yyyy-MM-dd-HH-mm-ss", null, DateTimeStyles.None, out date): //format: yyyy-MM-dd-HH-mm-ss
                        operation = new ExactDateSetter(args[i + 1]);
                        i++;
                        break;
                    case "--last-modified":
                        operation = new FileLastModified(); 
                        break;
                    case "--from-filename" when !args[i + 1].StartsWith("-"):
                        operation = new FileNameExtractor(args[i + 1]);
                        i++;
                        break;
                    case "--iterator":
                        operation = new Iterator();
                        break;
                    case "--debug":
                        Debugger.Launch();
                        break;
                    default:
                        throw new Exception("Invalid argument");
                }
            }
            if (string.IsNullOrEmpty(output))
            {
                output = dirName;
            }
            if (recursive)
            {
                operation.PerfromRecursively(dirName, match, exclude, output);
            }
            else
            {
                operation.Perform(dirName, match, exclude, output);
            }
        }
    }
}
