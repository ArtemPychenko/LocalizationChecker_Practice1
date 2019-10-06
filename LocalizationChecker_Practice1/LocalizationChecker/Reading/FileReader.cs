using System.Linq;
using System.Xml;
using System.Xml.Linq;
using LocalizationChecker.Model;

namespace LocalizationChecker.Reading
{
    public class FileReader
    {
        /// <summary>
        /// Reads and filters xml file, saves filtering results to container.
        /// </summary>
        /// <param name="fileName">Xml file to filter.</param>
        /// <returns>Container with filtered results.</returns>
        public File ReadAndFilterXmlFile(string fileName)
        {
            // read from XML file
            XDocument xmlDocument = XDocument.Load(fileName, LoadOptions.SetLineInfo);

            File file = new File()
            {
                FileName = fileName
            };

            // put it in collection using list
            foreach (XElement firstnode in xmlDocument.Nodes())
            {
                if (firstnode.Name == "sitecore")
                {
                    foreach (XElement childNode in firstnode.Nodes())
                    {
                        if (childNode.Name == "phrase")
                        {
                            var attributes = childNode.Attributes().ToDictionary(x => x.Name, y => y);

                            Phrase phrase = new Phrase()
                            {
                                LineNumber = ((IXmlLineInfo)childNode).LineNumber,
                                Key = attributes.GetValueOrDefault("key")?.Value,
                                ItemId = attributes.GetValueOrDefault("itemid")?.Value,
                                Path = attributes.GetValueOrDefault("path")?.Value,
                                FieldName = attributes.GetValueOrDefault("fieldName")?.Value,
                                Template = attributes.GetValueOrDefault("template")?.Value,
                                TranslatedValue = childNode.HasElements ? ((XElement)childNode.Nodes().First()).Value : null
                            };

                            file.Phrases.Add(phrase);
                        }
                    }
                }
            }
            return file;
        }
    }
}
