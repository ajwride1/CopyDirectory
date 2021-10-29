using System;
using CopyDirectory.Functions;
using CopyDirectory.Functions.Exceptions;

namespace CopyDirectory.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            _WriteSeparator();
            Console.WriteLine("Welcome to the copy directory app");
            _WriteSeparator();

            Console.WriteLine("This app will allow you to copy files from a source directory into a new directory");

            bool running = true;

            while (running)
            {
                bool validDirectory = false;
                bool repeat = false;

                Source sourceDirectory;

                while (!validDirectory)
                {
                    try
                    {
                        if (!repeat)
                            Console.WriteLine("Let's start by selecting the source directory, please type out the source directory below");
                        else
                            Console.WriteLine("Let's try again, please type out the source directory below");

                        string sourceDirectoryPath = Console.ReadLine();

                        sourceDirectory = new Source(sourceDirectoryPath);

                        Console.WriteLine($"Source directory selected : {sourceDirectory.Info.FullName}");
                        Console.WriteLine("Please type yes or no to confirm whether or not this is the correct directory");
                        string rawConfirmation = Console.ReadLine();

                        if(string.IsNullOrWhiteSpace(rawConfirmation))
                            Console.WriteLine("Unable to determine if the selected directory was confirmed");
                        else
                        {
                            if (rawConfirmation.ToUpper() == "Y" || rawConfirmation.ToUpper() == "YES" ||
                                rawConfirmation == "1")
                            {
                                Console.WriteLine($"You have confirmed the source directory as {sourceDirectory.Info.FullName}");
                                validDirectory = true;
                            }
                            else
                            {
                                Console.WriteLine(
                                    $"Ok {sourceDirectory.Info.FullName} is not the correct source directory");
                            }
                        }

                        repeat = true;
                    }
                    catch (DirectoryNotGiven notGivenException)
                    {
                        Console.WriteLine(notGivenException.Message);
                        repeat = true;
                    }
                    catch (DirectoryNotFound notFoundException)
                    {
                        Console.WriteLine(notFoundException.Message);
                        repeat = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unfortunately an error has occurred during the selection of the source directory");
                        Console.WriteLine(e.Message);
                        running = false;
                        throw;
                    }
                }
            }
        }

        private static void _WriteSeparator()
        {
            Console.WriteLine("======================================================");
        }
    }
}
