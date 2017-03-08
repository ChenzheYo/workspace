using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;
using System.IO;

namespace ClipLineCore
{
    public sealed class ClipLineCore
    {
        public void uploadFile(StorageFile file)
        {
            File.Copy(file.Path, "C:\\Program Files (x86)\\Apache Software Foundation\\Tomcat 9.0\\webapps\\ROOT\\ClipLine\\a.mp4");
        }
    }
}
