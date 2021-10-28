using System;

namespace CopyDirectory.Functions.Exceptions
{
    public class DirectoryNotFound : Exception
    {
        public DirectoryNotFound(string directoryPath) 
            : base($"Directory {directoryPath} does not exist so cannot be used as the source") 
        { }
    }
}
