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

        public FileFolderInfo(string[] args, string SubFolderName)
        {

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("/maxsize"))
                {
                    if ((++i < args.Length) && (decimal.TryParse(args[i], out decimal result)))
                        MaxSummaryFileSize = result * 1024 * 1024;
                }
                else if (args[i].Contains("/source"))
                {
                    if (++i < args.Length) FromFolderName = args[i];
                }
                else if (args[i].Contains("/target"))
                    if (++i < args.Length)
                        ToFolderName = args[i] + "\\" + SubFolderName;
            }
        }

    }
}
