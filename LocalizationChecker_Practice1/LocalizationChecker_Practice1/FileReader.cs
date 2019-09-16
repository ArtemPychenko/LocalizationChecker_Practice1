using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LocalizationChecker_Practice1
{
    public class FileReader
    {
        public File ReadXMLFile(string fileName)
        {
            // read from XML file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            File file = new File()
            {
                FileName = fileName
            };

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
                            var translatedValue = childNode.ChildNodes.Count == 0 ? null : childNode.ChildNodes[0].InnerText;

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
    }
}
