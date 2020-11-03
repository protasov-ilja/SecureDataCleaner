using System.Collections.Generic;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary.Models
{
    public class SecureDataInfo
    {
        public string SecureKey { get; set; }

        /// <summary>
        /// Места расположения secure-данных относительно 
        /// каждого свойства класса HttpResult (Url, RequestBody, ResponseBody)
        /// </summary>
        public Dictionary<PropertyType, HashSet<string>> LocationsInfo { get; set; }
    }
}
