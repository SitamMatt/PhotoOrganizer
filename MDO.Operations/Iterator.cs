using System.IO;

namespace MDO.Operations
{
    public class Iterator : AbstarctDirectoryOperation
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
