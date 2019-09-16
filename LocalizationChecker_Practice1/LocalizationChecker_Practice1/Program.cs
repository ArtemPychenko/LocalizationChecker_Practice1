using System;
using System.Collections.Generic;
using System.IO;
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

            TextReportGenerator reportGenerator = new TextReportGenerator();
            var report = reportGenerator.Generate(result);

            using (StreamReader streamReader = new StreamReader(report))
            {
                var reportContent = streamReader.ReadToEnd();
                Console.WriteLine(reportContent);

            }
            
                Console.WriteLine("Message End !");
                Console.ReadKey();
            
        }

        
    }
}
