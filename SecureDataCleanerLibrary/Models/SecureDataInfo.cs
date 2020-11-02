using System.Collections.Generic;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Models
{
    public class SecureDataInfo
    {
        public string SecureKey { get; set; }

        public Dictionary<PropertyType, HashSet<string>> LocationsInfo { get; set; }
    }
}
