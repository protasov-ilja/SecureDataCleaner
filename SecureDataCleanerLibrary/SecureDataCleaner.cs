﻿using System.Collections.Generic;
using SecureDataCleanerLibrary.Models;
using SecureDataCleanerLibrary.Models.Enums;
using SecureDataCleanerLibrary.Url;

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
                HashSet<SecureDataLocation> secureDataLocations = null;
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

        private string CleanSecureData(string data, string secureKey, HashSet<SecureDataLocation> secureDataLocations)
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
