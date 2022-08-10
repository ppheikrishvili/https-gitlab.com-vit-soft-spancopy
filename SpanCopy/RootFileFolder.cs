using System.IO;

namespace SpanCopy
{
    public class RootFileFolder
    {
        public RootFileFolder(string fullName, string FromFolderName)
        {
            FullName = fullName;
            FileName = Path.GetFileName(fullName);
            FolderName = Path.GetDirectoryName(fullName)?.Substring(FromFolderName.Length);
            FolderName = (FolderName == "") ? FolderName : FolderName.Substring(1);
        }


        public string FullName { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
    }
}
