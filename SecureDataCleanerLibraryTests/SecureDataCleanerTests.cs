using System.Collections.Generic;
using SecureDataCleanerLibrary;
using SecureDataCleanerLibrary.Models;
using SecureDataCleanerLibrary.Models.Enums;
using Xunit;

namespace SecureDataCleanerLibraryTests
{
    public class SecureDataCleanerTests
    {
        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearOneSecureKeyInOneDataLocationInUrlProperty_OneSecureKeyInOneDataLocationInUrlPropertyCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var secureKey = "pass";
            var locationsInfo = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery
            };

            locationsInfo.Add(PropertyType.Url, dataLocations);
            var secureDataInfoList = new List<SecureDataInfo>();
            var secureDataInfo = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo
            };

            secureDataInfoList.Add(secureDataInfo);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedResult = "http://test.com/users/max/info?pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult( httpResult );

            // Assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearSecureKeyInMultipleDataLocationsInUrlProperty_ClearSecureKeyInMultipleDataLocationsInUrlProperty()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/pass/123/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var secureKey = "pass";
            var locationsInfo = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
                SecureDataLocation.UrlRest
            };

            locationsInfo.Add(PropertyType.Url, dataLocations);
            var secureDataInfoList = new List<SecureDataInfo>();
            var secureDataInfo = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo
            };

            secureDataInfoList.Add(secureDataInfo);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedResult = "http://test.com/users/max/pass/XXX/info?pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearOneSecureKeyInMultipleDataLocationsInRequestBodyProperty_OneSecureKeyInMultipleDataLocationsInRequestBodyPropertyCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com/pass/123?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var secureKey = "pass";
            var locationsInfo = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
                SecureDataLocation.UrlRest
            };

            locationsInfo.Add(PropertyType.RequestBody, dataLocations);
            var secureDataInfoList = new List<SecureDataInfo>();
            var secureDataInfo = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo
            };

            secureDataInfoList.Add(secureDataInfo);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedResult = "http://test.com/pass/XXX?user=max&pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearOneSecureKeyInMultipleDataLocationsInResponseBodyProperty_OneSecureKeyInMultipleDataLocationsInResponseBodyPropertyCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com/pass/123?user=max&pass=123456"
            };

            var secureKey = "pass";
            var locationsInfo = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
                SecureDataLocation.UrlRest
            };

            locationsInfo.Add(PropertyType.ResponseBody, dataLocations);
            var secureDataInfoList = new List<SecureDataInfo>();
            var secureDataInfo = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo
            };

            secureDataInfoList.Add(secureDataInfo);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedResult = "http://test.com/pass/XXX?user=max&pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedResult, cleanedHttpResult.ResponseBody);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearOneSecureKeyInMultipleDataLocationsInMultipleProperties_OneSecureKeyInMultipleDataLocationsInMultiplePropertiesCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/pass/123/info?pass=123456",
                RequestBody = "http://test.com/pass/123?user=max&pass=123456",
                ResponseBody = "http://test.com/pass/123?user=max&pass=123456"
            };

            var secureKey = "pass";
            var locationsInfo = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
                SecureDataLocation.UrlRest
            };

            locationsInfo.Add(PropertyType.Url, dataLocations);
            locationsInfo.Add(PropertyType.RequestBody, dataLocations);
            locationsInfo.Add(PropertyType.ResponseBody, dataLocations);

            var secureDataInfoList = new List<SecureDataInfo>();
            var secureDataInfo = new SecureDataInfo
            {
                SecureKey = secureKey,
                LocationsInfo = locationsInfo
            };

            secureDataInfoList.Add(secureDataInfo);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedUrl = "http://test.com/users/max/pass/XXX/info?pass=XXXXXX";
            var expectedRequestBody = "http://test.com/pass/XXX?user=max&pass=XXXXXX";
            var expectedResponseBody = "http://test.com/pass/XXX?user=max&pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedUrl, cleanedHttpResult.Url);
            Assert.Equal(expectedRequestBody, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResponseBody, cleanedHttpResult.ResponseBody);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearMultipleSecureKeysInUrlProperty_MultipleSecureKeysInMultiplePropertiesCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var secureKey1 = "pass";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations1 = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
            };
            locationsInfo1.Add(PropertyType.Url, dataLocations1);
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKey1,
                LocationsInfo = locationsInfo1
            };

            var secureKey2 = "users";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            var dataLocations2 = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlRest
            };
            locationsInfo2.Add(PropertyType.Url, dataLocations2);
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKey2,
                LocationsInfo = locationsInfo2
            };

            var secureDataInfoList = new List<SecureDataInfo>();

            secureDataInfoList.Add(secureDataInfo1);
            secureDataInfoList.Add(secureDataInfo2);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }

        [Fact]
        public void SecureDataCleaner_CleanHttpResult_ClearBookingcomHttpResult_bookingcomHttpResultCleared()
        {
            // Arrange
            var httpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };

            var dataLocationQuery = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlQuery,
            };

            var dataLocationRest = new HashSet<SecureDataLocation>
            {
                SecureDataLocation.UrlRest
            };

            var secureKey1 = "user";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            locationsInfo1.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo1.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKey1,
                LocationsInfo = locationsInfo1
            };

            var secureKey2 = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            locationsInfo2.Add(PropertyType.Url, dataLocationQuery);
            locationsInfo2.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo2.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKey2,
                LocationsInfo = locationsInfo2
            };

            var secureKey3 = "users";
            var locationsInfo3 = new Dictionary<PropertyType, HashSet<SecureDataLocation>>();
            locationsInfo3.Add(PropertyType.Url, dataLocationRest);
            var secureDataInfo3 = new SecureDataInfo
            {
                SecureKey = secureKey3,
                LocationsInfo = locationsInfo3
            };

            var secureDataInfoList = new List<SecureDataInfo>();
            secureDataInfoList.Add(secureDataInfo1);
            secureDataInfoList.Add(secureDataInfo2);
            secureDataInfoList.Add(secureDataInfo3);

            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);

            var expectedUrl = "http://test.com/users/XXX/info?pass=XXXXXX";
            var expectedRequestBody = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedResponseBody = "http://test.com?user=XXX&pass=XXXXXX";

            // Act
            var cleanedHttpResult = secureDataCleaner.CleanHttpResult(httpResult);

            // Assert
            Assert.Equal(expectedUrl, cleanedHttpResult.Url);
            Assert.Equal(expectedRequestBody, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResponseBody, cleanedHttpResult.ResponseBody);
        }
    }
}
