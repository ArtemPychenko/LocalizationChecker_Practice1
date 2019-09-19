using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalizationChecker
{
    public class Checker
    {
        public ParentResult Check(File masterFile, IEnumerable<File> languageFiles, string pathFilter)
        {
            if (masterFile == null)
            {
                throw new ArgumentNullException(nameof(masterFile));
            }

            if (languageFiles == null)
            {
                throw new ArgumentNullException(nameof(languageFiles));
            }

            if (languageFiles.Count() == 0)
            {
                throw new ArgumentException("At least one language file should be provided", nameof(languageFiles));
            }

            var masterContext = masterFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(pathFilter));

            List<Result> languageResults = new List<Result>();
            foreach (var languageFile in languageFiles)
            {
                var languageContext = languageFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(pathFilter));

                var missingPhrases = masterContext.Where(x => !languageContext.Any(y => y.Key == x.Key));
                var untranslatedPhrases = languageContext.Where(x => masterContext.Any(y => y.Key == x.Key && y.TranslatedValue == x.TranslatedValue));

                Result languageResult = new Result(languageFile.FileName, languageFile.Phrases.Count(), languageContext.Count(), untranslatedPhrases, missingPhrases);
                languageResults.Add(languageResult);
            }

            return new ParentResult(masterFile.FileName, pathFilter, masterFile.Phrases.Count(), masterContext.Count(), languageResults);
        }
    }
}
