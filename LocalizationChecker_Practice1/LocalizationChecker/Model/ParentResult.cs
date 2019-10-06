using System.Collections.Generic;

namespace LocalizationChecker.Model
{
    public class ParentResult
    {
        public ParentResult(string fileName, string pathFilter, int totalPhraseCount, int totalFilteredPhraseCount, IEnumerable<Result> languageResults)
        {
            FileName = fileName;
            PathFilter = pathFilter;
            TotalPhraseCount = totalPhraseCount;
            TotalFilteredPhraseCount = totalFilteredPhraseCount;
            LanguageResults = languageResults;
        }

        public string FileName { get; set; }
        public string PathFilter { get; set; }
        public int TotalPhraseCount { get; set; }
        public int TotalFilteredPhraseCount { get; set; }
        public IEnumerable<Result> LanguageResults { get; set; }
    }
}
