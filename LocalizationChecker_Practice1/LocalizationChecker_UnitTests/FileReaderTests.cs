using System;
using System.Linq;
using LocalizationChecker;
using LocalizationChecker.Reading;
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
            var file = reader.ReadAndFilterXmlFile(@"TestFiles\NoPhrases.xml");
            Assert.IsFalse(file.Phrases.Any());
        }
    }
}
