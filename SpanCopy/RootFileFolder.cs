using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpanCopy
{
    public class RootFileFolder
    {
        public RootFileFolder(string fullName, string folderName, string fileName)
        {
            FullName = fullName;
            FolderName = (folderName == "") ? folderName : folderName.Substring(1);
            FileName = fileName;
        }


        public string FullName { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }



    }
}
