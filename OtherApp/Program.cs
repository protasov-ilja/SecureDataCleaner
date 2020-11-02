using System;
using System.Collections.Generic;
using SecureDataCleanerLibrary;
using SecureDataCleanerLibrary.Models;
using SecureDataCleanerLibrary.Models.Enums;

namespace OtherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var dataLocationCustom = new HashSet<string>
            {
                "OtherLocation",
            };

            var secureKey = "user";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo1.Add(PropertyType.Url, dataLocationCustom);
            locationsInfo1.Add(PropertyType.RequestBody, dataLocationCustom);
            locationsInfo1.Add(PropertyType.ResponseBody, dataLocationCustom);
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo1
            };

            var secureDataInfoList = new List<SecureDataInfo>
            {
                secureDataInfo1,
            };

            var clientCleaner = new ClientCleaner();
            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList, clientCleaner);
            httpResult = secureDataCleaner.CleanHttpResult(httpResult);

            Console.WriteLine(httpResult.Url);
            Console.WriteLine(httpResult.RequestBody);
            Console.WriteLine(httpResult.ResponseBody);
        }
    }
}
