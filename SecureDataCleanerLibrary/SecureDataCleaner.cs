using System.Collections.Generic;
using SecureDataCleanerLibrary.Models;
using SecureDataCleanerLibrary.Models.Enums;
using SecureDataCleanerLibrary.Cleaners;

namespace SecureDataCleanerLibrary
{
    public class SecureDataCleaner
    {
        private readonly List<SecureDataInfo> _secureDataInfoList;

        private readonly List<ICleaner> _cleaners;

        public SecureDataCleaner(List<SecureDataInfo> secureDataInfoList)
        {
            _secureDataInfoList = secureDataInfoList;
            _cleaners = new List<ICleaner>();
            _cleaners.Add(new UrlCleaner());
            _cleaners.Add(new XmlCleaner());
            _cleaners.Add(new JsonCleaner());
        }

        public SecureDataCleaner(List<SecureDataInfo> secureDataInfoList, List<ICleaner> customCleaners)
            : this(secureDataInfoList)
        {
            _cleaners.AddRange(customCleaners);
        }

        public SecureDataCleaner(List<SecureDataInfo> secureDataInfoList, ICleaner customCleaner)
            : this(secureDataInfoList)
        {
            _cleaners.Add(customCleaner);
        }

        public HttpResult CleanHttpResult(HttpResult httpResult)
        {
            var url = httpResult.Url;
            var requestBody = httpResult.RequestBody;
            var responseBody = httpResult.ResponseBody;

            foreach (var secureDataInfo in _secureDataInfoList)
            {
                var locations = secureDataInfo.LocationsInfo;
                var secureKey = secureDataInfo.SecureKey;
                HashSet<string> secureDataLocations = null;
                if (locations.ContainsKey(PropertyType.Url))
                {
                    secureDataLocations = locations[PropertyType.Url];
                    url = CleanSecureData(url, secureKey, secureDataLocations);
                }

                if (locations.ContainsKey(PropertyType.RequestBody))
                {
                    secureDataLocations = locations[PropertyType.RequestBody];
                    requestBody = CleanSecureData(requestBody, secureKey, secureDataLocations);
                }

                if (locations.ContainsKey(PropertyType.ResponseBody))
                {
                    secureDataLocations = locations[PropertyType.ResponseBody];
                    responseBody = CleanSecureData(responseBody, secureKey, secureDataLocations);
                }
            }

            httpResult.Url = url;
            httpResult.RequestBody = requestBody;
            httpResult.ResponseBody = responseBody;

            return httpResult;
        }

        private string CleanSecureData(string data, string secureKey, HashSet<string> secureDataLocations)
        {
            foreach (var location in secureDataLocations)
            {
                foreach (var cleaner in _cleaners)
                {
                    if (cleaner.ProcessedLocations.Contains(location))
                    {
                        data = cleaner.CleanSecureData(data, secureKey, location);
                    }
                }
            }

            return data;
        }
    }
}
