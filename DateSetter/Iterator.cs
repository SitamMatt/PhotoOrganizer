using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DateSetter
{
    class Iterator : AbstarctDirectoryOperation
    {
        private int iterator = 0;
        protected override void Operation(string path, string output = "")
        {
            var fileInfo = new FileInfo(path);
            string fileName = Path.Combine(output, iterator++.ToString() + fileInfo.Extension);
            fileInfo.CopyTo(fileName, true);
        }
    }
}
