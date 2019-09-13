using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker_Practice1
{
   
    public class Result
    {
        public Result(string fileName, int totalPhraseCount, IEnumerable<Phrase> untranslatedPhrases, IEnumerable<Phrase> missingPhrases)
        {
            this.FileName = fileName;
            this.TotalPhraseCount = totalPhraseCount;
            this.UntranslatedPhrases = untranslatedPhrases;
            this.MissingPhrases = missingPhrases;
        }

        public string FileName { get; set; }
        public int TotalPhraseCount { get; set; }
        public IEnumerable<Phrase> UntranslatedPhrases { get; set; }
        public IEnumerable<Phrase> MissingPhrases { get; set; }
    }
}
