using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpanCopy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string SubFolderName = DateTime.Today.ToString("yyyyMMdd");
            FileFolderInfo fileFolderInfo = new FileFolderInfo(args, SubFolderName);

            if (!fileFolderInfo.IsOk())
            {
                Console.WriteLine(
                    "Use SpanCopy.exe /maxsize 'fileSizeGB' /source 'FromFolderName' /target 'ToFolderName'");
                return;
            }

            string[] files = Directory.GetFiles(fileFolderInfo.FromFolderName, "*", SearchOption.AllDirectories);

            List<RootFileFolder> rootFileFolders = files.Select(s => new RootFileFolder(s, fileFolderInfo.FromFolderName)).ToList();

            List<string> folders = rootFileFolders.Select(s => s.FolderName).Distinct().ToList();

            int dirCounter = GetEnumSource(fileFolderInfo.ToFolderName)+1;

            List<string> CopedFiles = new List<string>();

            var backUpFolderName = CreateDestFolder(fileFolderInfo.ToFolderName, dirCounter.ToString(), "");
            
            decimal totalLength = 0;
            
            foreach (string folderName in folders)
            {
                var copyFiles = rootFileFolders.Where(s => s.FolderName == folderName).ToList();

                string fullBackUpName = "";

                if (copyFiles.Any()) fullBackUpName = CreateDestFolder(backUpFolderName, "", folderName);

                foreach (var file in copyFiles)
                {
                    decimal fileSizeByte = new FileInfo(file.FullName).Length;

                    if (fileSizeByte > fileFolderInfo.MaxSummaryFileSize) continue;
                    
                    if (totalLength + fileSizeByte <= fileFolderInfo.MaxSummaryFileSize) totalLength += fileSizeByte;
                    else
                    {
                        CreateIndexFile(backUpFolderName, CopedFiles);
                        backUpFolderName = CreateDestFolder(fileFolderInfo.ToFolderName, (++dirCounter).ToString(), "");
                        totalLength = fileSizeByte;
                        fullBackUpName = CreateDestFolder(backUpFolderName, "", folderName);
                        CopedFiles = new List<string>();
                    }

                    File.Copy(file.FullName, Path.Combine(fullBackUpName, file.FileName), true);
                    CopedFiles.Add(Path.Combine(fullBackUpName, file.FileName));
                }
            }

            CreateIndexFile(backUpFolderName, CopedFiles);

            string CreateDestFolder(string ToRootFolderName, string delName, string FolderName)
            {
                string newFolderName = Path.Combine(ToRootFolderName, (delName + FolderName));
                if (!Directory.Exists(newFolderName)) Directory.CreateDirectory(newFolderName);
                return newFolderName;
            }
        }

        static int GetEnumSource(string pathOfDir)
        {
            int retResult = 0;
            DirectoryInfo dir = new DirectoryInfo(pathOfDir);
            DirectoryInfo[] Dir = dir.GetDirectories();
            foreach (var t in Dir)
                if ( ( int.TryParse(t.Name, out int result) ) && (result > retResult) ) retResult = result;

            return retResult;
        }


        static void CreateIndexFile(string backUpFolderName, List<string> copedFiles)
        {
            if (!copedFiles.Any()) return;
            StreamWriter file = new StreamWriter(Path.Combine(backUpFolderName, "index.txt"));
            file.WriteLine(string.Join(Environment.NewLine, copedFiles.ToArray()));
            file.Close();
        }
    }
}
