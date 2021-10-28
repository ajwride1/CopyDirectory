using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyDirectory.Tests.Functions
{
    [TestClass]
    public class Source
    {
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
    }
}
