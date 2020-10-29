using System.Collections.Generic;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Models
{
    public class SecureDataInfo
    {
        public string SecureKey { get; set; }
        public Dictionary<PropertyType, HashSet<SecureDataLocation>> LocationsInfo { get; set; }
    }
}
