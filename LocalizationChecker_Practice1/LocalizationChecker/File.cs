using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker
{
    public class File
    {
        public string FileName;
        public List<Phrase> Phrases;
        public File()
        {
            this.Phrases = new List<Phrase>();
        }
    }
}
