using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker_Practice1
{
    public class Phrase
    {
        public int? LineNumber { get; set; }
        public string Key { get; set; }
        public string ItemId { get; set; }
        public string Path { get; set; }
        public string FieldName { get; set; }
        public string ValueNode { get; set; }
        public string Template { get; set; }
        public string TranslatedValue { get; set; }

    }
}
