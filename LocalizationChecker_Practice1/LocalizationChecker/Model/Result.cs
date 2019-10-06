using System.Collections.Generic;
using LocalizationChecker.Reading;

namespace LocalizationChecker.Model
{
    public class Result
    {
        public Result(string fileName, int totalPhraseCount, int filteredPhraseCount, IEnumerable<Phrase> untranslatedPhrases, IEnumerable<Phrase> missingPhrases)
        {
            FileName = fileName;
            TotalPhraseCount = totalPhraseCount;
            FilteredPhraseCount = filteredPhraseCount;
            UntranslatedPhrases = untranslatedPhrases;
            MissingPhrases = missingPhrases;
        }

        public string FileName { get; set; }
        public int TotalPhraseCount { get; set; }
        public int FilteredPhraseCount { get; set; }
        public IEnumerable<Phrase> UntranslatedPhrases { get; set; }
        public IEnumerable<Phrase> MissingPhrases { get; set; }
    }
}
