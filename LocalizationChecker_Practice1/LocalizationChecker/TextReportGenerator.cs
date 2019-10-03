using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker
{
    public class TextReportGenerator : IReportGenerator
    {
        public Stream Generate(ParentResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            //Header
            writer.WriteLine($"Master File: {result.FileName}");
            writer.WriteLine($"Report Generated: {DateTime.UtcNow:o}");
            writer.WriteLine($"Total phrases: {result.TotalPhraseCount}");
            writer.WriteLine($"Total filtered phrases: " + result.TotalFilteredPhraseCount);
            writer.WriteLine($"Path filter: {result.PathFilter}");
            writer.WriteLine($"{result.LanguageResults.Count()} language files analyzed:");
            foreach (var language in result.LanguageResults)
            {
                writer.WriteLine($"    {language.FileName}");
            }

            // Language 1 results
            foreach (var language in result.LanguageResults)
            {
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine("The file name is: " + language.FileName);
                writer.WriteLine("Total phrases: " + language.TotalPhraseCount);
                writer.WriteLine("Total filtered phrases: " + language.FilteredPhraseCount);

                writer.WriteLine();
                writer.WriteLine("Untranslated phrases: " + language.UntranslatedPhrases.Count());
                int untranslatedPhraseIndex = 0;
                foreach (var untranslatedPhrase in language.UntranslatedPhrases)
                {
                    writer.WriteLine($"  [{untranslatedPhraseIndex}] Line={untranslatedPhrase.LineNumber} Value={untranslatedPhrase.TranslatedValue} Key={untranslatedPhrase.Key}");
                    untranslatedPhraseIndex++;
                }

                writer.WriteLine();
                writer.WriteLine("Missing phrases: " + language.MissingPhrases.Count());
                int missingPhraseIndex = 0;
                foreach (var missingPhrase in language.MissingPhrases)
                {
                    writer.WriteLine($"  [{missingPhraseIndex}] Line={missingPhrase.LineNumber} Value={missingPhrase.TranslatedValue} Key={missingPhrase.Key}");
                    missingPhraseIndex++;
                }
            }


            // TODO: Write out data from ParentResult

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
