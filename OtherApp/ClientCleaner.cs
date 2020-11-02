using System.Collections.Generic;
using SecureDataCleanerLibrary;

namespace OtherApp
{
    public class ClientCleaner : ICleaner
    {
        private const char SymbolForEncoding = 'X';

        public List<string> ProcessedLocations { get; private set; }

        public ClientCleaner()
        {
            ProcessedLocations = new List<string>
            {
                "OtherLocation"
            };
        }

        public string CleanSecureData(string data, string key, string dataLocationType)
        {
            return new string(SymbolForEncoding, data.Length);
        }
    }
}
