using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DateSetter
{
    abstract class AbstarctDirectoryOperation
    {
        public void PerfromRecursively(string path, string pattern = "*.*", string output = "")
        {
            Perform(path, pattern, output);
            string[] directories = Directory.GetDirectories(path);
            for (int i = 0; i < directories.Length; i++)
            {
                string fixedOutput = Path.Combine(Path.GetFullPath(output), Path.GetFileName(directories[i]));
                PerfromRecursively(directories[i], pattern, fixedOutput);
            }
        }
        public void Perform(string path, string pattern = "*.*", string output = "")
        {
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            string[] files = Directory.GetFiles(path, pattern);
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"Performing operation on {files[i]} with output to {output}");
                Operation(files[i], output);
            }
        }

        protected abstract void Operation(string path, string output = "");
    }
}
