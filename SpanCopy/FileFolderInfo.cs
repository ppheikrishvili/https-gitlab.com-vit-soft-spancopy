using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpanCopy
{
    public class FileFolderInfo
    {
        public decimal MaxSummaryFileSize { get; set; }
        public string FromFolderName { get; set; }
        public string ToFolderName { get; set; }

        public bool IsOk()
        {
            if ( (MaxSummaryFileSize == 0 ) || ( !Directory.Exists(FromFolderName) ) ) return false;
            if ( !Directory.Exists(ToFolderName) ) Directory.CreateDirectory(ToFolderName);
            return true;
        }
    }
}
