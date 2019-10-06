
namespace LocalizationChecker.Model
{
    /// <summary>
    /// Container to keep required nodes of xml file.
    /// </summary>
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
