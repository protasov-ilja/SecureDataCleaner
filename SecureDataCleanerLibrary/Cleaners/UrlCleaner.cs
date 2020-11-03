using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Cleaners
{
    public class UrlCleaner : ICleaner
    {
        private const char SymbolForEncoding = 'X';

        public List<string> ProcessedLocations { get; private set; }

        public UrlCleaner()
        {
            ProcessedLocations = new List<string>
            {
                SecureDataLocation.UrlQuery,
                SecureDataLocation.UrlRest
            };
        }

        public string CleanSecureData(string data, string key, string dataLocationType)
        {
            switch (dataLocationType)
            {
                case SecureDataLocation.UrlQuery:
                    return CleanSecureDataInQuery(data, key);
                case SecureDataLocation.UrlRest:
                    return CleanSecureDataInRest(data, key );
            }

            return data;
        }

        private string CleanSecureDataInQuery(string data, string key)
        {
            var queryArgumentPattern = @$"([\?|\&]{key}=)([^\&]*)";

            var encodedData = EncodeData(data, queryArgumentPattern);
 
            return encodedData;
        }

        private string CleanSecureDataInRest(string data, string key)
        {
            var restArgumentPattern = @$"(/{key}/)([^/|\?]*)";

            var encodedData = EncodeData(data, restArgumentPattern);

            return encodedData;
        }

        private string EncodeData(string data, string pattern)
        {
            return Regex.Replace(data, pattern,
                m => m.Groups[1].Value + new string(SymbolForEncoding, m.Groups[2].Value.Length));
        }
    }
}
