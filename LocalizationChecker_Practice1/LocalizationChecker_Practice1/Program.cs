using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LocalizationChecker_Practice1
{
    class Program
    {
        static string englishFile = @"C:\Users\Afidah\Desktop\keys.xml";
        static string denmarkFile = @"C:\Users\Afidah\Desktop\denmark.xml";

        public static File  ReadXMLFile(string fileName)
        {
            // read from XML file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            File file = new File();

            // put it in collection using list
            foreach (XmlNode firstnode in xmlDocument.ChildNodes)
            {
                if (firstnode.Name == "sitecore")
                {
                    foreach (XmlNode childNode in firstnode.ChildNodes)
                    {
                        if (childNode.Name == "phrase")
                        {
                            var key = childNode.Attributes["key"]?.Value;
                            var itemId = childNode.Attributes["itemid"]?.Value;
                            var path = childNode.Attributes["path"]?.Value;
                            var fieldName = childNode.Attributes["fieldName"]?.Value;
                            var template = childNode.Attributes["template"]?.Value;
                            var valueNode = childNode.Attributes[0];
                            var translatedValue = valueNode.InnerText;

                            Phrase phrase = new Phrase()
                            {
                                Key = key,
                                ItemId = itemId,
                                Path = path,
                                FieldName = fieldName,
                                Template = template,
                                TranslatedValue = translatedValue
                            };

                            file.Phrases.Add(phrase);
                        }
                    }
                }
            }
            return file;
        }
        

        static void Main(string[] args)
        {
            File EnglishFile = ReadXMLFile(englishFile);
            File DenmarkFile = ReadXMLFile(denmarkFile);
            var selectedPath = "/sitecore/client/Applications/MarketingAutomation";

            // Filter data based on selected Path
            var englishContext = EnglishFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(selectedPath));
            var denmarkContext = DenmarkFile.Phrases.Where(x => x.Path != null && x.Path.StartsWith(selectedPath));

            // Missing Phrase
            var missingDenmarkPhrase = englishContext.Where(x => !denmarkContext.Any(y => y.Key == x.Key));

            // Not Translated Phrase
            var untranslatedDenmarkPhrase = englishContext.Where(x => denmarkContext.Any(y => y.Key == x.Key && y.TranslatedValue == x.TranslatedValue));

            // Display denmark
            Result denmarkResult = new Result("DenmarkFile", DenmarkFile.Phrases.Count(), missingDenmarkPhrase, untranslatedDenmarkPhrase);

            List<Result> languageResults = new List<Result>();
            languageResults.Add(denmarkResult);

            foreach( var language in languageResults)
            {
                Console.WriteLine(language.FileName);
                Console.WriteLine(language.TotalPhraseCount);

                foreach(var lan in language)
                {

                }
                Console.WriteLine(language.MissingPhrases);
            }

            //ParentResult parentResult = new ParentResult( selectedPath, EnglishFile.Phrases.Count,DenmarkFile.Phrases.Count,languageResults);            
            

            Console.ReadKey();
        }
    }
}
