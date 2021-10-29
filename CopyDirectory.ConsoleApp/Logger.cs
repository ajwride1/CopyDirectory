using System;
using CopyDirectory.Functions;

namespace CopyDirectory.ConsoleApp
{
    public class Logger : ILogger
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
