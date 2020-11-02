using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Cleaners
{
    public class JsonCleaner : ICleaner
    {
        private const char SymbolForEncoding = 'X';

        public List<string> ProcessedLocations { get; private set; }

        public JsonCleaner()
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
                case SecureDataLocation.JsonElementValue:
                    return CleanSecureDataInJsonElementValue(data, key);
                case SecureDataLocation.JsonAttribute:
                    return CleanSecureDataInJsonAttribute(data, key);
            }

            return data;
        }

        private string CleanSecureDataInJsonElementValue(string data, string key)
        {
            var jsonObject = JObject.Parse(data);

            var properties = jsonObject.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name == key);

            foreach (var property in properties)
            {
                EncodePropertyValue(property);
            }

            var encodedData = jsonObject.ToString(Formatting.None);

            return encodedData;
        }

        private string CleanSecureDataInJsonAttribute(string data, string key)
        {
            var jsonObject = JObject.Parse(data);

            var properties = jsonObject.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name == key);

            foreach (var property in properties)
            {
                var token = property.Value;
                if (!token.HasValues)
                {
                    EncodePropertyValue(property);
                }
                else
                {
                    ReplaceValuesInChildren(token.Children<JProperty>());
                }
            }
            
            var encodedData = jsonObject.ToString(Formatting.None);

            return encodedData;
        }

        private void ReplaceValuesInChildren(JEnumerable<JProperty> children)
        {
            foreach (var child in children)
            {
                if (child.Value.HasValues)
                {
                    ReplaceValuesInChildren(child.Value.Children<JProperty>());
                }
                else
                {
                    EncodePropertyValue(child);
                }
            }
        }

        private void EncodePropertyValue(JProperty property)
        {
            var valueLength = property.Value.ToString().Length;
            property.Value = new string(SymbolForEncoding, valueLength);
        }
    }
}
