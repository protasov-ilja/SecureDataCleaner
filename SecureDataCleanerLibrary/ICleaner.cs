using System.Collections.Generic;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary
{
    public interface ICleaner
    {
        public string CleanSecureData(string data, string key, SecureDataLocation dataLocationType);
        public List<SecureDataLocation> ProcessedLocations { get; }
    }
}
