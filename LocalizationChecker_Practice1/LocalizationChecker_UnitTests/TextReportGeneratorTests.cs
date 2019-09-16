using System;
using System.IO;
using LocalizationChecker_Practice1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace LocalizationChecker_UnitTests
{
    [TestClass]
    public class TextReportGeneratorTests
    {
        private readonly TextReportGenerator instance;

        public TextReportGeneratorTests()
        {
            this.instance = new TextReportGenerator();
        }

        [TestMethod]
        public void NullResult()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => this.instance.Generate(null));
            exception.ParamName.ShouldBe("result");
        }

        [TestMethod]
        public void SingleLanguageFileAllOk()
        {
            Result languageResult = new Result("languagefile.xml", 10, 5, new Phrase[0], new Phrase[0]);
            ParentResult result = new ParentResult("masterfile.xml", "pathFilter", 10, 5, new Result[] { languageResult });
            var output = this.instance.Generate(result);

            using (StreamReader reader = new StreamReader(output))
            {
                var report = reader.ReadToEnd();

                // Header
                report.ShouldContain("Master File: masterfile.xml");
                report.ShouldContain("Report Generated:");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Path filter: pathFilter");
                report.ShouldContain("1 language files analyzed:");
                report.ShouldContain("    languagefile.xml");

                // Language 1 results
                report.ShouldContain("The file name is: languagefile.xml");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Total filtered phrases: 5");

                report.ShouldContain("Untranslated phrases: 0");
                report.ShouldContain("Missing phrases: 0");


                // TODO: Assert more stuff on report...
            }
        }


        [TestMethod]
        public void SingleLanguageFileMissingAndUntranslatedPhrases()
        {
            var untranslatedPhrases = new Phrase[]
            {
                new Phrase()
                {
                   TranslatedValue="Translated Value 1",
                   Key = "Key1",
                   LineNumber = 1,
                    
                    // etc
                },
                new Phrase()
                {
                   TranslatedValue="Translated Value 2",
                   Key = "Key2",
                   LineNumber = 2,
                }
            };

            var missingPhrases = new Phrase[]
           {
                new Phrase()
                {
                   TranslatedValue="Missing Value 1",
                   Key = "Key3",
                   LineNumber = 3,
                    
                    // etc
                },
                new Phrase()
                {
                   TranslatedValue="Missing Value 2",
                   Key = "Key4",
                   LineNumber = 4,
                }
           };

            Result languageResult = new Result("languagefile.xml", 10, 5, untranslatedPhrases, missingPhrases);
            ParentResult result = new ParentResult("masterfile.xml", "pathFilter", 10, 5, new Result[] { languageResult });
            var output = this.instance.Generate(result);

            using (StreamReader reader = new StreamReader(output))
            {
                var report = reader.ReadToEnd();

                // Header
                report.ShouldContain("Master File: masterfile.xml");
                report.ShouldContain("Report Generated:");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Path filter: pathFilter");
                report.ShouldContain("1 language files analyzed:");
                report.ShouldContain("    languagefile.xml");

                // Language 1 results
                report.ShouldContain("The file name is: languagefile.xml");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Total filtered phrases: 5");

                report.ShouldContain("Untranslated phrases: 2");
                foreach (var untranslatedPhrase in untranslatedPhrases)
                {
                    report.ShouldContain($" Line={untranslatedPhrase.LineNumber}");
                    report.ShouldContain($" Value={untranslatedPhrase.TranslatedValue}");
                    report.ShouldContain($" Key={untranslatedPhrase.Key}");
                }
                

                report.ShouldContain("Missing phrases: 2");
                foreach (var missingPhrase in missingPhrases)
                {
                    report.ShouldContain($" Line={missingPhrase.LineNumber}");
                    report.ShouldContain($" Value={missingPhrase.TranslatedValue}");
                    report.ShouldContain($" Key={missingPhrase.Key}");
                }



                // TODO: Assert more stuff on report...
            }
        }

        [TestMethod]
        public void TwoLanguageFilesAllOk()
        {
            Result languageResult1 = new Result("languagefile1.xml", 10, 5, new Phrase[0], new Phrase[0]);
            Result languageResult2 = new Result("languagefile2.xml", 20, 10, new Phrase[0], new Phrase[0]);
            ParentResult result = new ParentResult("masterfile.xml", "pathFilter", 10, 5, new Result[] { languageResult1, languageResult2 });
            var output = this.instance.Generate(result);

            using (StreamReader reader = new StreamReader(output))
            {
                var report = reader.ReadToEnd();

                // Header
                report.ShouldContain("Master File: masterfile.xml");
                report.ShouldContain("Report Generated:");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Path filter: pathFilter");
                report.ShouldContain("2 language files analyzed:");
                report.ShouldContain("    languagefile1.xml");
                report.ShouldContain("    languagefile2.xml");


                // Language 1 results
                report.ShouldContain("The file name is: languagefile1.xml");
                report.ShouldContain("Total phrases: 10");
                report.ShouldContain("Total filtered phrases: 5");

                report.ShouldContain("Untranslated phrases: 0");
                report.ShouldContain("Missing phrases: 0");

                // Language 2 results
                report.ShouldContain("The file name is: languagefile2.xml");
                report.ShouldContain("Total phrases: 20");
                report.ShouldContain("Total filtered phrases: 10");

                report.ShouldContain("Untranslated phrases: 0");
                report.ShouldContain("Missing phrases: 0");


                // TODO: Assert more stuff on report...
            }
        }
    }
}
