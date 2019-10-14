using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDO.Operations.Windows.DateTaken
{
    public class FileLastModified : AbstractDateTakenSetter
    {
        protected override DateTime GetDateTime(FileInfo fileInfo)
        {
            return fileInfo.LastWriteTime;
        }
    }
}
