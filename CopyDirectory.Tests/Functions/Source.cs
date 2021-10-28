using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyDirectory.Tests.Functions
{
    [TestClass]
    public class Source
    {
        private const string _ValidDirectory = @"C:\Scratch\CopyDirectory\Testing";
        private List<string> _SubDirectories = new List<string>();
        private List<string> _FileNames = new List<string>();
        private List<string> _LargeFiles = new List<string>();

        [TestInitialize]
        public void SetUpTestDirectories()
        {
            if (!Directory.Exists(_ValidDirectory))
                Directory.CreateDirectory(_ValidDirectory);

            Random randomNumberGenerator = new Random();
            int subDirectoryCount = randomNumberGenerator.Next(5);
            int fileNameCount = randomNumberGenerator.Next(20);

            for (int i = 0; i < subDirectoryCount; i++)
            {
                string subDirectory = $@"{_ValidDirectory}\Sub{i}";

                _SubDirectories.Add(subDirectory);

                if (!Directory.Exists(subDirectory))
                    Directory.CreateDirectory(subDirectory);
            }

            for (int i = 0; i < fileNameCount; i++)
            {
                _FileNames.Add($"File{i}.txt");
            }

            foreach (string subDirectory in _SubDirectories)
            {
                foreach (string fileName in _FileNames)
                {
                    if (!File.Exists($@"{subDirectory}\{fileName}"))
                        File.Create($@"{subDirectory}\{fileName}");
                }
            }

            _LargeFiles.Add($@"{_ValidDirectory}\LargeFile0.txt");
            _LargeFiles.Add($@"{_ValidDirectory}\LargeFile1.txt");
            _LargeFiles.Add($@"{_ValidDirectory}\LargeFile3.txt");

            foreach (string largeFile in _LargeFiles)
            {
                if(File.Exists(largeFile))
                    File.Delete(largeFile);

                int multiplier = _LargeFiles.IndexOf(largeFile) + 1;

                int megabytes = multiplier * 1000000;

                File.WriteAllBytes(largeFile, new byte[megabytes * 3]);
            }
        }

        [TestMethod]
        public void DirectoryNotFound()
        {
            string someInvalidDirectory = "TestyMcTest";

            try
            {
                CopyDirectory.Functions.Source sourceDirectory = new CopyDirectory.Functions.Source(someInvalidDirectory);
            }
            catch (CopyDirectory.Functions.Exceptions.DirectoryNotFound directoryNotFound)
            {
                Console.WriteLine(directoryNotFound.Message);

                Assert.IsTrue(directoryNotFound.Message.Contains(someInvalidDirectory));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [TestMethod]
        public void DirectoryNotGiven()
        {
            try
            {
                CopyDirectory.Functions.Source sourceDirectory = new CopyDirectory.Functions.Source(null);
            }
            catch (CopyDirectory.Functions.Exceptions.DirectoryNotGiven directoryNotGiven)
            {
                Console.WriteLine(directoryNotGiven.Message);

                Assert.IsTrue(!string.IsNullOrWhiteSpace(directoryNotGiven.Message));
            }
            catch (Exception e)
            {
                throw;
            }

            try
            {
                CopyDirectory.Functions.Source sourceDirectory = new CopyDirectory.Functions.Source("");
            }
            catch (CopyDirectory.Functions.Exceptions.DirectoryNotGiven directoryNotGiven)
            {
                Console.WriteLine(directoryNotGiven.Message);

                Assert.IsTrue(!string.IsNullOrWhiteSpace(directoryNotGiven.Message));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [TestMethod]
        public void ValidConstructor()
        {
            CopyDirectory.Functions.Source sourceDirectory = new CopyDirectory.Functions.Source(_ValidDirectory);

            foreach (string subDirectory in _SubDirectories)
            {
                Assert.IsTrue(sourceDirectory.SubDirectories
                    .Select(s => s.Info.FullName)
                    .Contains(subDirectory));
            }

            foreach (string largeFile in _LargeFiles)
            {
                Assert.IsTrue(sourceDirectory.AllFiles
                    .Select(f => f.FullName)
                    .Contains(largeFile));
            }

            foreach (string fileName in _FileNames)
            {
                foreach (CopyDirectory.Functions.Source sourceDirectorySubDirectory in sourceDirectory.SubDirectories)
                {
                    Assert.IsTrue(sourceDirectorySubDirectory.AllFiles
                        .Select(f => f.Name)
                        .Contains(fileName));
                }
            }
        }
    }
}
