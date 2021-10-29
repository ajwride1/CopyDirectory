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

        public async Task Copy(string targetDirectory, ILogger display, bool overwrite = true)
        {
            if (Directory.Exists(targetDirectory) && overwrite)
            {
                Source targetDirectorySource = new Source(targetDirectory);

                display.Print($"Emptying the target directory and all of it's contents : {targetDirectory}");

                targetDirectorySource.Empty(display);
            }

            display.Print($"Creating the new directory {targetDirectory}");

            Directory.CreateDirectory(targetDirectory);

            foreach (Source subDirectory in SubDirectories)
            {
                string newSubDirectory = subDirectory.Info.FullName.Replace(this.Info.FullName, targetDirectory);

                display.Print($"Copying from {subDirectory.Info.FullName} to {newSubDirectory}");

                await subDirectory.Copy(newSubDirectory, display, overwrite);
            }

            foreach (FileInfo file in Files)
            {
                string newFileName = file.FullName.Replace(Info.FullName, targetDirectory);

                display.Print($"Copying {file.FullName} to {newFileName}");

                File.Copy(file.FullName, newFileName, overwrite);
            }
        }

        public void Empty(ILogger display)
        {
            foreach (Source subDirectory in SubDirectories)
            {
                subDirectory.Empty(display);
            }

            foreach (FileInfo fileInfo in Files)
            {
                display.Print($"Deleting {fileInfo.FullName}");

                File.Delete(fileInfo.FullName);
            }
        }
    }
}
