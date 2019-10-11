using System;
using System.IO;

namespace DateSetter
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
            //bool fromFileName = false;
            AbstarctDirectoryOperation operation = new Iterator();
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
                    case "-d" when DateTime.TryParse(args[i + 1], out date): //format: YYYY-MM-DD HH:mm:ss
                        i++;
                        break;
                    case "--from-filename" when !args[i + 1].StartsWith("-"):
                        //fromFileName = true;
                        operation = new FileNameExtractor(args[i+1]);
                        i++;
                        break;
                    case "--iterator":
                        operation = new Iterator();
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
                operation.PerfromRecursively(dirName, match, output);
            }
            else
            {
                operation.Perform(dirName, match, output);
            }
        }
    }
}
