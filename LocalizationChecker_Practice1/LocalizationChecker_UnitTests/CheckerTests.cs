using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalizationChecker;

namespace LocalizationChecker_UnitTests
{
    [TestClass]
    public class CheckerTests
    {
        private readonly Checker instance;
        private string pathFilter = "/sitecore/client/Applications/MarketingAutomation";

        public CheckerTests()
        {
            this.instance = new Checker();
        }

        [TestMethod]
        public void NullMasterFile()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => this.instance.Check(null, new File[0], this.pathFilter));
            Assert.AreEqual("masterFile", exception.ParamName);
        }

        [TestMethod]
        public void NullLanguageFiles()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => this.instance.Check(new File(), null, this.pathFilter));
            Assert.AreEqual("languageFiles", exception.ParamName);
        }

        [TestMethod]
        public void EmptyLanguageFiles()
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => this.instance.Check(new File(), new File[0], this.pathFilter));
            exception.ParamName.ShouldBe("languageFiles");
            exception.Message.ShouldContain("At least one language file should be provided");
        }

        [TestMethod]
        public void MasterFileSameContentAsLanguageFile_NoPhrases()
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadXMLFile(@"TestFiles\NoPhrases.xml");
            var languageFile = reader.ReadXMLFile(@"TestFiles\NoPhrases.xml");
            File[] languageFiles = new File[]
            {
                languageFile
            };

            var result = this.instance.Check(masterFile, languageFiles, this.pathFilter);
            result.FileName.ShouldBe(@"TestFiles\NoPhrases.xml");
            result.TotalPhraseCount.ShouldBe(0);
            result.TotalFilteredPhraseCount.ShouldBe(0);
            result.PathFilter.ShouldBe(this.pathFilter);
            result.LanguageResults.Count().ShouldBe(1);

            var languageResult = result.LanguageResults.First();
            languageResult.ShouldNotBeNull();
            languageResult.FileName.ShouldBe(@"TestFiles\NoPhrases.xml");
            languageResult.MissingPhrases.ShouldBeEmpty();
            languageResult.UntranslatedPhrases.ShouldBeEmpty();
            languageResult.TotalPhraseCount.ShouldBe(0);
            languageResult.FilteredPhraseCount.ShouldBe(0);
        }

        [TestMethod]
        public void MasterFileSinglePhrase_PathFilterMatches()
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadXMLFile(@"TestFiles\SinglePhrase.xml");
            var languageFile = reader.ReadXMLFile(@"TestFiles\NoPhrases.xml");
            File[] languageFiles = new File[]
            {
                languageFile
            };

            var result = this.instance.Check(masterFile, languageFiles, this.pathFilter);
            result.FileName.ShouldBe(@"TestFiles\SinglePhrase.xml");
            result.TotalPhraseCount.ShouldBe(1);
            result.TotalFilteredPhraseCount.ShouldBe(1);
            result.PathFilter.ShouldBe(this.pathFilter);
            result.LanguageResults.Count().ShouldBe(1);

            var languageResult = result.LanguageResults.First();
            languageResult.ShouldNotBeNull();
            languageResult.FileName.ShouldBe(@"TestFiles\NoPhrases.xml");
            languageResult.MissingPhrases.Count().ShouldBe(1);
            languageResult.MissingPhrases.First().TranslatedValue.ShouldBe("Test1");
            languageResult.UntranslatedPhrases.ShouldBeEmpty();
            languageResult.TotalPhraseCount.ShouldBe(0);
            languageResult.FilteredPhraseCount.ShouldBe(0);
        }

        [TestMethod]
        public void MasterFileSinglePhrase_PathFilterDoesNotMatch()
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadXMLFile(@"TestFiles\SinglePhrase.xml");
            var languageFile = reader.ReadXMLFile(@"TestFiles\NoPhrases.xml");
            File[] languageFiles = new File[]
            {
                languageFile
            };

            var result = this.instance.Check(masterFile, languageFiles, "dummyFilter");
            result.FileName.ShouldBe(@"TestFiles\SinglePhrase.xml");
            result.TotalPhraseCount.ShouldBe(1);
            result.TotalFilteredPhraseCount.ShouldBe(0);
            result.PathFilter.ShouldBe("dummyFilter");
            result.LanguageResults.Count().ShouldBe(1);

            var languageResult = result.LanguageResults.First();
            languageResult.ShouldNotBeNull();
            languageResult.FileName.ShouldBe(@"TestFiles\NoPhrases.xml");
            languageResult.MissingPhrases.ShouldBeEmpty();
            languageResult.UntranslatedPhrases.Count().ShouldBe(0);
            languageResult.TotalPhraseCount.ShouldBe(0);
            languageResult.FilteredPhraseCount.ShouldBe(0);
        }

        [TestMethod]
        public void MasterFileTwoPhrases_LanguageFileSinglePhrase()
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadXMLFile(@"TestFiles\TwoPhrases.xml");
            var languageFile = reader.ReadXMLFile(@"TestFiles\SinglePhrase.xml");
            File[] languageFiles = new File[]
            {
                languageFile
            };

            var result = this.instance.Check(masterFile, languageFiles, this.pathFilter);
            result.FileName.ShouldBe(@"TestFiles\TwoPhrases.xml");
            result.TotalPhraseCount.ShouldBe(2);
            result.TotalFilteredPhraseCount.ShouldBe(2);
            result.PathFilter.ShouldBe(this.pathFilter);
            result.LanguageResults.Count().ShouldBe(1);

            var languageResult = result.LanguageResults.First();
            languageResult.ShouldNotBeNull();
            languageResult.FileName.ShouldBe(@"TestFiles\SinglePhrase.xml");
            languageResult.MissingPhrases.Count().ShouldBe(1);
            languageResult.UntranslatedPhrases.Count().ShouldBe(1);
            languageResult.UntranslatedPhrases.First().TranslatedValue.ShouldBe("Test1");
            languageResult.TotalPhraseCount.ShouldBe(1);
            languageResult.FilteredPhraseCount.ShouldBe(1);
        }

        [TestMethod]
        public void MasterFileTwoPhrases_LanguageFilesSinglePhrase()
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadXMLFile(@"TestFiles\ThreePhrases.xml");
            var languageFile1 = reader.ReadXMLFile(@"TestFiles\TwoPhrases.xml");
            var languageFile2 = reader.ReadXMLFile(@"TestFiles\SinglePhrase.xml");
            File[] languageFiles = new File[]
            {
                languageFile1, languageFile2
            };

            var result = this.instance.Check(masterFile, languageFiles, this.pathFilter);
            result.FileName.ShouldBe(@"TestFiles\ThreePhrases.xml");
            result.TotalPhraseCount.ShouldBe(3);
            result.TotalFilteredPhraseCount.ShouldBe(3);
            result.PathFilter.ShouldBe(this.pathFilter);
            result.LanguageResults.Count().ShouldBe(2);

            var languageResult1 = result.LanguageResults.ToArray()[0];
            languageResult1.ShouldNotBeNull();
            languageResult1.FileName.ShouldBe(@"TestFiles\TwoPhrases.xml");
            languageResult1.MissingPhrases.Count().ShouldBe(1);
            languageResult1.UntranslatedPhrases.Count().ShouldBe(2);
            languageResult1.TotalPhraseCount.ShouldBe(2);
            languageResult1.FilteredPhraseCount.ShouldBe(2);

            var languageResult2 = result.LanguageResults.ToArray()[1];
            languageResult2.ShouldNotBeNull();
            languageResult2.FileName.ShouldBe(@"TestFiles\SinglePhrase.xml");
            languageResult2.MissingPhrases.Count().ShouldBe(2);
            languageResult2.UntranslatedPhrases.Count().ShouldBe(1);
            languageResult2.TotalPhraseCount.ShouldBe(1);
            languageResult2.FilteredPhraseCount.ShouldBe(1);
        }
    }
}
