using System;

namespace CopyDirectory.Functions.Exceptions
{
    public class DirectoryNotGiven : Exception
    {
        public DirectoryNotGiven() : base(
            "There was no directory supplied so we cannot find the source directory detail")
        { }
    }
}
