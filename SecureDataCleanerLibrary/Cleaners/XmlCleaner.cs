using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Cleaners
{
    public class XmlCleaner : ICleaner
    {
        private const char SymbolForEncoding = 'X';

        public List<string> ProcessedLocations { get; private set; }

        public XmlCleaner()
        {
            ProcessedLocations = new List<string>
            {
                SecureDataLocation.XmlElementValue,
                SecureDataLocation.XmlAttribute
            };
        }

        public string CleanSecureData(string data, string key, string dataLocationType)
        {
            switch (dataLocationType)
            {
                case SecureDataLocation.XmlElementValue:
                    return CleanSecureDataInXmlElementValue(data, key);
                case SecureDataLocation.XmlAttribute:
                    return CleanSecureDataInXmlAttribute(data, key);
            }

            return data;
        }

        private string CleanSecureDataInXmlElementValue(string data, string key)
        {
            XDocument xDocument = XDocument.Parse(data);
            XElement xmlRoot = xDocument.Root;
            var elements = xmlRoot.DescendantsAndSelf(key).ToList();

            foreach (var element in elements)
            {
                var valueLength = element.Value.Length;
                element.Value = new string(SymbolForEncoding, valueLength);
            }

            var encodedData = xDocument.ToString();

            return encodedData;
        }

        private string CleanSecureDataInXmlAttribute(string data, string key)
        {
            XDocument xDocument = XDocument.Parse(data);
            XElement xmlRoot = xDocument.Root;
            var elements = xmlRoot.DescendantsAndSelf().ToList();

            foreach (var element in elements)
            {
                var attribute = element.Attribute(key);
                if (attribute != null)
                {
                    var valueLength = attribute.Value.Length;
                    attribute.Value = new string(SymbolForEncoding, valueLength);
                }
            }

            var encodedData = xDocument.ToString();

            return encodedData;
        }
    }
}
