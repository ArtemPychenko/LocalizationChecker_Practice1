using System.Collections.Generic;

namespace LocalizationChecker.Model
{
    /// <summary>
    /// Container to keep filtering results.
    /// </summary>
    public class File
    {
        public string FileName;
        public List<Phrase> Phrases;

        public File()
        {
            Phrases = new List<Phrase>();
        }
    }
}
