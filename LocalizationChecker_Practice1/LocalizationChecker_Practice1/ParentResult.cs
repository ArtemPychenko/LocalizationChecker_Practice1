using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker_Practice1
{
    public class ParentResult
    {
        public ParentResult(string pathFilter, int totalPhraseCount, int totalFilteredPhraseCount, IEnumerable<Result> languageResults)
        {
            
            this.PathFilter = pathFilter;
            this.TotalPhraseCount = totalPhraseCount;
            this.TotalFilteredPhraseCount = totalFilteredPhraseCount;
            this.LanguageResults = languageResults;
        }

        public string PathFilter { get; set; }
        public int TotalPhraseCount { get; set; }
        public int TotalFilteredPhraseCount { get; set; }
        public IEnumerable<Result> LanguageResults { get; set; }
    }
}
