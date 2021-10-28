using System;
using System.IO;
using CopyDirectory.Functions.Exceptions;

namespace CopyDirectory.Functions
{
    public class Source
    {
        public DirectoryInfo Info { get; set; }

        public Source(string directoryPath)
        {
            if(string.IsNullOrWhiteSpace(directoryPath))
                throw new DirectoryNotGiven();

            if(!Directory.Exists(directoryPath))
                throw new DirectoryNotFound(directoryPath);

            Info = new DirectoryInfo(directoryPath);
        }
    }
}
