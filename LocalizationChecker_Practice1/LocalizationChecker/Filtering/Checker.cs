using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalizationChecker.Model;

namespace LocalizationChecker.Filtering
{
    /// <summary>
    /// Provides functionality to compare the files.
    /// </summary>
    public class Checker
    {
        /// <summary>
        /// Compares language files with master file.
        /// </summary>
        /// <param name="masterFile">Main file to compare with.</param>
        /// <param name="languageFiles">Translated files to compare with master file.</param>
        /// <param name="pathFilter">Path to filter nodes for specific component.</param>
        /// <returns>Result of comparing language files with master file.</returns>
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

            if (!languageFiles.Any())
            {
                throw new ArgumentException("At least one language file should be provided", nameof(languageFiles));
            }

            var masterContext = masterFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(pathFilter));

            List<Result> languageResults = new List<Result>();

            Parallel.ForEach(languageFiles, (languageFile) =>
            {
                var languageContext = languageFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(pathFilter));

                var missingPhrases = masterContext.Where(x => languageContext.All(y => y.Key != x.Key)).ToArray();
                var untranslatedPhrases = languageContext.Where(x => masterContext.Any(y => y.Key == x.Key && y.TranslatedValue == x.TranslatedValue)).ToArray();

                Result languageResult = new Result(languageFile.FileName, languageFile.Phrases.Count(), languageContext.Count(), untranslatedPhrases, missingPhrases);
                languageResults.Add(languageResult);
            });

            return new ParentResult(masterFile.FileName, pathFilter, masterFile.Phrases.Count(), masterContext.Count(), languageResults);
        }
    }
}
