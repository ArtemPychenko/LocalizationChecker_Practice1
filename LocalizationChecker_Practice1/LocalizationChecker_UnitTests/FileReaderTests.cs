using System;
using System.Linq;
using LocalizationChecker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalizationChecker_UnitTests
{
    [TestClass]
    public class FileReaderTests
    {
        [TestMethod]
        public void NoPhrases()
        {
            FileReader reader = new FileReader();
            var file = reader.ReadXMLFile(@"TestFiles\NoPhrases.xml");
            Assert.IsFalse(file.Phrases.Any());
        }
    }
}
