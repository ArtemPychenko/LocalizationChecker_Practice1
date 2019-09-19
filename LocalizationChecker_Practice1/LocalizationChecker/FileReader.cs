using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LocalizationChecker
{
    public class FileReader
    {
        public File ReadXMLFile(string fileName)
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
                            var lineNumber = ((IXmlLineInfo)childNode).LineNumber;
                            var attributes = childNode.Attributes().ToDictionary(x => x.Name, y => y);
                            var key = attributes.GetValueOrDefault("key")?.Value;
                            var itemId = attributes.GetValueOrDefault("itemid")?.Value;
                            var path = attributes.GetValueOrDefault("path")?.Value;
                            var fieldName = attributes.GetValueOrDefault("fieldName")?.Value;
                            var template = attributes.GetValueOrDefault("template")?.Value;
                            var translatedValue = childNode.HasElements ? ((XElement)childNode.Nodes().First()).Value : null;

                            Phrase phrase = new Phrase()
                            {
                                LineNumber = lineNumber,
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
    }
}
