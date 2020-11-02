using System.Collections.Generic;
using SecureDataCleanerLibrary;
using SecureDataCleanerLibrary.Models;
using SecureDataCleanerLibrary.Models.Enums;
using Xunit;

namespace SecureDataCleanerLibraryTests
{
    public class HttpHandlerTests
    {
        [Fact]
        public void HttpHandler_Process_BookingcomHttpResult_ClearSecureData()
        {
            //Arrange
            var bookingcomHttpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };
            var httpHandler = new HttpHandler();

            var dataLocationQuery = new HashSet<string> { SecureDataLocation.UrlQuery };
            var dataLocationRest = new HashSet<string> { SecureDataLocation.UrlRest };

            var secureKey1 = "user";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo1.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo1.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKey1,
                LocationsInfo = locationsInfo1
            };

            var secureKey2 = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo2.Add(PropertyType.Url, dataLocationQuery);
            locationsInfo2.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo2.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKey2,
                LocationsInfo = locationsInfo2
            };

            var secureKey3 = "users";
            var locationsInfo3 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo3.Add( PropertyType.Url, dataLocationRest);
            var secureDataInfo3 = new SecureDataInfo
            {
                SecureKey = secureKey3,
                LocationsInfo = locationsInfo3
            };

            var secureDataInfoList = new List<SecureDataInfo>();
            secureDataInfoList.Add(secureDataInfo1);
            secureDataInfoList.Add(secureDataInfo2);
            secureDataInfoList.Add(secureDataInfo3);

            //Act
            httpHandler.Process(bookingcomHttpResult.Url, bookingcomHttpResult.RequestBody, bookingcomHttpResult.ResponseBody, secureDataInfoList);

            //Assert
            Assert.Equal("http://test.com/users/XXX/info?pass=XXXXXX", httpHandler.CurrentLog.Url);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.ResponseBody);
        }
    }
}
