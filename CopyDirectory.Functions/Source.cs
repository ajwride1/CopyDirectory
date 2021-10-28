using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CopyDirectory.Functions.Exceptions;

namespace CopyDirectory.Functions
{
    public class Source
    {
        public DirectoryInfo Info { get; set; }
        public FileInfo[] AllFiles { get; set; }
        public List<Source> SubDirectories { get; set; }

        public Source(string directoryPath)
        {
            if(string.IsNullOrWhiteSpace(directoryPath))
                throw new DirectoryNotGiven();

            if(!Directory.Exists(directoryPath))
                throw new DirectoryNotFound(directoryPath);

            Info = new DirectoryInfo(directoryPath);

            AllFiles = Info.GetFiles();

            SubDirectories = new List<Source>();

            List<string> subDirectories = Info.GetDirectories()
                .Select(sd => sd.FullName)
                .ToList();

            foreach (string subDirectory in subDirectories)
            {
                SubDirectories.Add(new Source(subDirectory));
            }
        }
    }
}
