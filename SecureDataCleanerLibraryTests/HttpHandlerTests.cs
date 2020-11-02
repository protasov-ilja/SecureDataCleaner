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
            // Arrange
            var bookingcomHttpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };
            var httpHandler = new HttpHandler();

            var dataLocationQuery = new HashSet<string> { SecureDataLocation.UrlQuery };
            var dataLocationRest = new HashSet<string> { SecureDataLocation.UrlRest };

            var secureKeyUser = "user";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo1.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo1.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo1
            };

            var secureKeyPass = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo2.Add(PropertyType.Url, dataLocationQuery);
            locationsInfo2.Add(PropertyType.RequestBody, dataLocationQuery);
            locationsInfo2.Add(PropertyType.ResponseBody, dataLocationQuery);
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo2
            };

            var secureKey3 = "users";
            var locationsInfo3 = new Dictionary<PropertyType, HashSet<string>>();
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

            // Act
            httpHandler.Process(bookingcomHttpResult.Url, 
                bookingcomHttpResult.RequestBody, bookingcomHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com/users/XXX/info?pass=XXXXXX", httpHandler.CurrentLog.Url);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.ResponseBody);
        }

        [Fact]
        public void HttpHandler_Process_YandexHttpResult_ClearSecureData()
        {
            // Arrange
            var yandexHttpResult = new HttpResult
            {
                Url = "http://test.com?user=max&pass=123456",
                RequestBody = "<auth><user>max</user><pass>123456</pass></auth>",
                ResponseBody = "<auth user=\"max\" pass=\"123456\"/>"
            };

            var httpHandler = new HttpHandler();

            var dataLocationQuery = new HashSet<string> { SecureDataLocation.UrlQuery };
            var dataLocationAttribute = new HashSet<string> { SecureDataLocation.XmlAttribute };
            var dataLocationElement = new HashSet<string> { SecureDataLocation.XmlElementValue };

            var secureKeyUser = "user";
            var locationsInfo = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationQuery },
                { PropertyType.RequestBody, dataLocationElement },
                { PropertyType.ResponseBody, dataLocationAttribute }
            };
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo
            };

            var secureKeyPass = "pass";
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo
            };

            var secureDataInfoList = new List<SecureDataInfo>
            {
                secureDataInfo1,
                secureDataInfo2
            };

            // Act
            httpHandler.Process(yandexHttpResult.Url, 
                yandexHttpResult.RequestBody, yandexHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.Url);
            Assert.Equal("<auth><user>XXX</user><pass>XXXXXX</pass></auth>", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("<auth user=\"XXX\" pass=\"XXXXXX\" />", httpHandler.CurrentLog.ResponseBody);
        }

        [Fact]
        public void HttpHandler_Process_OstrovokHttpResult_ClearSecureData()
        {
            // Arrange
            var ostrovokHttpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info",
                RequestBody = "{user:\"max\",pass:\"123456\"}",
                ResponseBody = "{user:{value:\"max\"},pass:{value:\"123456\"}}"
            };

            var httpHandler = new HttpHandler();

            var dataLocationRest = new HashSet<string> { SecureDataLocation.UrlRest };
            var dataLocationAttribute = new HashSet<string> { SecureDataLocation.JsonAttribute };
            var dataLocationElement = new HashSet<string> { SecureDataLocation.JsonElementValue };

            var secureKeyUsers = "users";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>();
            locationsInfo1.Add( PropertyType.Url, dataLocationRest );
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUsers,
                LocationsInfo = locationsInfo1
            };

            var secureKeyPass = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.RequestBody, dataLocationElement },
                { PropertyType.ResponseBody, dataLocationAttribute }
            };
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo2
            };

            var secureKeyUser = "user";
            var secureDataInfo3 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo2
            };

            var secureDataInfoList = new List<SecureDataInfo>
            {
                secureDataInfo1,
                secureDataInfo2,
                secureDataInfo3
            };

            // Act
            httpHandler.Process(ostrovokHttpResult.Url, 
                ostrovokHttpResult.RequestBody, ostrovokHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com/users/XXX/info", httpHandler.CurrentLog.Url);
            Assert.Equal("{\"user\":\"XXX\",\"pass\":\"XXXXXX\"}", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("{\"user\":{\"value\":\"XXX\"},\"pass\":{\"value\":\"XXXXXX\"}}", httpHandler.CurrentLog.ResponseBody);
        }

        [Fact]
        public void HttpHandler_Process_AgodaHttpResult_ClearSecureData()
        {
            // Arrange
            var agodaHttpResult = new HttpResult
            {
                Url = "http://test.com?user=max&pass=123456",
                RequestBody = @"
                    <auth>
                        <user>max</user>
                        <pass>123456</pass>
                    </auth>",
                ResponseBody = "<auth user='max' pass='123456'/>"
            };


            var httpHandler = new HttpHandler();

            var dataLocationQuery = new HashSet<string> { SecureDataLocation.UrlQuery };
            var dataLocationAttribute = new HashSet<string> { SecureDataLocation.XmlAttribute };
            var dataLocationElement = new HashSet<string> { SecureDataLocation.XmlElementValue };

            var secureKeyUser = "user";
            var locationsInfo = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationQuery },
                { PropertyType.RequestBody, dataLocationElement },
                { PropertyType.ResponseBody, dataLocationAttribute }
            };
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo
            };

            var secureKeyPass = "pass";
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo
            };

            var secureDataInfoList = new List<SecureDataInfo>();
            secureDataInfoList.Add(secureDataInfo1);
            secureDataInfoList.Add(secureDataInfo2);

            // Act
            httpHandler.Process(agodaHttpResult.Url, 
                agodaHttpResult.RequestBody, agodaHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.Url);
            Assert.Equal("<auth><user>XXX</user><pass>XXXXXX</pass></auth>", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("<auth user=\"XXX\" pass=\"XXXXXX\" />", httpHandler.CurrentLog.ResponseBody);
        }

        [Fact]
        public void HttpHandler_Process_GoogleHttpResult_ClearSecureData()
        {
            // Arrange
            var googleHttpResult = new HttpResult
            {
                Url = "http://test.com?user=max&pass=123456",
                RequestBody = @"
                    <auth user=""max"">
                        <pass>123456</pass>
                    </auth>",
                ResponseBody = @"
                    <auth pass=""123456"">
                        <user>max</user>
                    </auth>"
            };

            var httpHandler = new HttpHandler();

            var dataLocationQuery = new HashSet<string> { SecureDataLocation.UrlQuery };
            var dataLocationAttribute = new HashSet<string> { SecureDataLocation.XmlAttribute };
            var dataLocationElement = new HashSet<string> { SecureDataLocation.XmlElementValue };

            var secureKeyUser = "user";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationQuery },
                { PropertyType.RequestBody, dataLocationAttribute },
                { PropertyType.ResponseBody, dataLocationElement }
            };
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo1
            };

            var secureKeyPass = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationQuery },
                { PropertyType.RequestBody, dataLocationElement },
                { PropertyType.ResponseBody, dataLocationAttribute }
            };
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo2
            };

            var secureDataInfoList = new List<SecureDataInfo>
            {
                secureDataInfo1,
                secureDataInfo2
            };

            // Act
            httpHandler.Process(googleHttpResult.Url, 
                googleHttpResult.RequestBody, googleHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpHandler.CurrentLog.Url);
            Assert.Equal("<auth user=\"XXX\"><pass>XXXXXX</pass></auth>", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("<auth pass=\"XXXXXX\"><user>XXX</user></auth>", httpHandler.CurrentLog.ResponseBody);
        }

        [Fact]
        public void HttpHandler_Process_ExpediaHttpResult_ClearSecureData()
        {
            // Arrange
            var expediaHttpResult = new HttpResult
            {
                Url = "http://test.com/users/max/info",
                RequestBody = @"
                    {
                        user: 'max',
                        pass: '123456'
                    }",
                ResponseBody = @"
                    {
                        user: {
                            value: 'max'
                        },
                        pass: {
                            value: '123456'
                        }
                    }"
            };

            var httpHandler = new HttpHandler();

            var dataLocationRest = new HashSet<string> { SecureDataLocation.UrlRest };
            var dataLocationAttribute = new HashSet<string> { SecureDataLocation.JsonAttribute };
            var dataLocationElement = new HashSet<string> { SecureDataLocation.JsonElementValue };

            var secureKeyUsers = "users";
            var locationsInfo1 = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationRest },
            };
            var secureDataInfo1 = new SecureDataInfo
            {
                SecureKey = secureKeyUsers,
                LocationsInfo = locationsInfo1
            };

            var secureKeyPass = "pass";
            var locationsInfo2 = new Dictionary<PropertyType, HashSet<string>>
            {
                { PropertyType.Url, dataLocationRest },
                { PropertyType.RequestBody, dataLocationElement },
                { PropertyType.ResponseBody, dataLocationAttribute }
            };
            var secureDataInfo2 = new SecureDataInfo
            {
                SecureKey = secureKeyPass,
                LocationsInfo = locationsInfo2
            };

            var secureKeyUser = "user";
            var secureDataInfo3 = new SecureDataInfo
            {
                SecureKey = secureKeyUser,
                LocationsInfo = locationsInfo2
            };

            var secureDataInfoList = new List<SecureDataInfo>
            {
                secureDataInfo1,
                secureDataInfo2,
                secureDataInfo3
            };

            // Act
            httpHandler.Process(expediaHttpResult.Url, 
                expediaHttpResult.RequestBody, expediaHttpResult.ResponseBody, secureDataInfoList);

            // Assert
            Assert.Equal("http://test.com/users/XXX/info", httpHandler.CurrentLog.Url);
            Assert.Equal("{\"user\":\"XXX\",\"pass\":\"XXXXXX\"}", httpHandler.CurrentLog.RequestBody);
            Assert.Equal("{\"user\":{\"value\":\"XXX\"},\"pass\":{\"value\":\"XXXXXX\"}}", httpHandler.CurrentLog.ResponseBody);
        }
    }
}
