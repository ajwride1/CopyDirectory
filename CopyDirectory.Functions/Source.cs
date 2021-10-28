using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CopyDirectory.Functions.Exceptions;

namespace CopyDirectory.Functions
{
    public class Source
    {
        public DirectoryInfo Info { get; set; }
        public FileInfo[] Files { get; set; }
        public List<Source> SubDirectories { get; set; }

        public Source(string directoryPath)
        {
            if(string.IsNullOrWhiteSpace(directoryPath))
                throw new DirectoryNotGiven();

            if(!Directory.Exists(directoryPath))
                throw new DirectoryNotFound(directoryPath);

            Info = new DirectoryInfo(directoryPath);

            Files = Info.GetFiles();

            SubDirectories = new List<Source>();

            List<string> subDirectories = Info.GetDirectories()
                .Select(sd => sd.FullName)
                .ToList();

            foreach (string subDirectory in subDirectories)
            {
                SubDirectories.Add(new Source(subDirectory));
            }
        }

        public async Task Copy(string targetDirectory, bool overwrite = true)
        {
            if (Directory.Exists(targetDirectory) && overwrite)
            {
                Source targetDirectorySource = new Source(targetDirectory);

                targetDirectorySource.Empty();
            }

            Directory.CreateDirectory(targetDirectory);

            foreach (Source subDirectory in SubDirectories)
            {
                string newSubDirectory = subDirectory.Info.FullName.Replace(this.Info.FullName, targetDirectory);

                await subDirectory.Copy(newSubDirectory);
            }

            foreach (FileInfo file in Files)
            {
                string newFileName = file.FullName.Replace(Info.FullName, targetDirectory);

                File.Copy(file.FullName, newFileName, overwrite);
            }
        }

        public void Empty()
        {
            foreach (Source subDirectory in SubDirectories)
            {
                subDirectory.Empty();
            }

            foreach (FileInfo fileInfo in Files)
            {
                File.Delete(fileInfo.FullName);
            }
        }
    }
}
