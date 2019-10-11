using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Formats.Png
{
    class Testing
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream(@"F:\testdir\Screenshot_2018-01-01-01-31-28.png", FileMode.OpenOrCreate);
            var img = PNGImage.From(fs);
            img.AddTextualData("Author", "Matt");
            img.RemoveTextualData("Creation Time");
            img.Save(@"F:\testdir\output.png");
            fs.Close();
        }
    }
}
