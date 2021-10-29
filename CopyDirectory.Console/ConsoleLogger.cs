using CopyDirectory.Functions;

namespace CopyDirectory.Console
{
    public class ConsoleLogger : ILogger
    {
        public void Print(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
