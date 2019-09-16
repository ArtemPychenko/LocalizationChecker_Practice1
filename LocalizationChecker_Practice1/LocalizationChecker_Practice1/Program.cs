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
        static string denmarkFile = @"C:\Users\Afidah\Desktop\keys-denmark.xml";


        static void Main(string[] args)
        {
            FileReader reader = new FileReader();
            File EnglishFile = reader.ReadXMLFile(englishFile);
            File DenmarkFile = reader.ReadXMLFile(denmarkFile);
            var selectedPath = "/sitecore/client/Applications/MarketingAutomation";

            Checker checker = new Checker();
            var result = checker.Check(EnglishFile, new File[] { DenmarkFile }, selectedPath);

            //// Display result
            foreach (var language in result.LanguageResults)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("The file name is: " + language.FileName);
                Console.WriteLine("Total phrases: " + language.TotalPhraseCount);
                Console.WriteLine("Total filtered phrases: " + language.FilteredPhraseCount);

                Console.WriteLine();
                Console.WriteLine("Untranslated phrases: " + language.UntranslatedPhrases.Count());
                int untranslatedPhraseIndex = 0;
                foreach (var untranslatedPhrase in language.UntranslatedPhrases)
                {
                    Console.WriteLine($"  [{untranslatedPhraseIndex}] Value={untranslatedPhrase.TranslatedValue} Key={untranslatedPhrase.Key}");
                    untranslatedPhraseIndex++;
                }

                Console.WriteLine();
                Console.WriteLine("Missing phrases: " + language.MissingPhrases.Count());
                foreach (var missingPhrase in language.MissingPhrases)
                {
                    Console.WriteLine(missingPhrase.Key);
                }
            }

            Console.WriteLine("Message End !");
            Console.ReadKey();
        }

        
    }
}
