using SecureDataCleanerLibrary.Models.Enums;
using SecureDataCleanerLibrary.Url;
using Xunit;

namespace SecureDataCleanerLibraryTests
{
    public class UrlCleanerTests
    {
        [Fact]
        public void UrlCleaner_CleanSecureData_ClearOneSecureKeyInUrlQuery_OneSecureKeyInUrlQueryCleared()
        {
            // Arrange
            var url = "http://test.com?user=max&pass=123456";
            var secureKey = "pass";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com?user=max&pass=XXXXXX";

            // Act
            var cleanedUrl = urlCleaner.CleanSecureData(url, secureKey, SecureDataLocation.UrlQuery);

            // Assert
            Assert.Equal(expectedResult, cleanedUrl);
        }

        [Fact]
        public void UrlCleaner_CleanSecureData_ClearTwoSecureKeysInUrlQuery_TwoSecureKeysInUrlQueryCleared()
        {
            // Arrange
            var url = "http://test.com?user=max&pass=123456";
            var secureKey1 = "pass";
            var secureKey2 = "user";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com?user=XXX&pass=XXXXXX";
            
            // Act
            var resultUrl = urlCleaner.CleanSecureData(url, secureKey1, SecureDataLocation.UrlQuery);
            resultUrl = urlCleaner.CleanSecureData(resultUrl, secureKey2, SecureDataLocation.UrlQuery);

            // Assert
            Assert.Equal(expectedResult, resultUrl);
        }

        [Fact]
        public void UrlCleaner_CleanSecureData_ClearOneSecureKeyInUrlRest_OneSecureKeyInUrlRestCleared()
        {
            // Arrange
            var url = "http://test.com/users/max/info";
            var secureKey = "users";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info";

            // Act
            var resultUrl = urlCleaner.CleanSecureData(url, secureKey, SecureDataLocation.UrlRest);

            // Assert
            Assert.Equal(expectedResult, resultUrl);
        }

        [Fact]
        public void UrlCleaner_CleanSecureData_ClearTwoSecureKeysInUrlRest_TwoSecureKeysInUrlRestCleared()
        {
            // Arrange
            var url = "http://test.com/users/max/info/pass/1234";
            var secureKey1 = "pass";
            var secureKey2 = "users";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info/pass/XXXX";

            // Act
            var resultUrl = urlCleaner.CleanSecureData(url, secureKey1, SecureDataLocation.UrlRest);
            resultUrl = urlCleaner.CleanSecureData(resultUrl, secureKey2, SecureDataLocation.UrlRest);

            // Assert
            Assert.Equal(expectedResult, resultUrl);
        }

        [Fact]
        public void UrlCleaner_CleanSecureData_ClearOneSecureKeyInUrlRestAndOneSecureKeyInUrlQuery_OneSecureKeyInUrlRestAndOneSecureKeyInUrlQueryCleared()
        {
            // Arrange
            var url = "http://test.com/users/max?pass=1234";
            var secureKeyInRest = "users";
            var secureKeyInQuery = "pass";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX?pass=XXXX";

            // Act
            var resultUrl = urlCleaner.CleanSecureData(url, secureKeyInRest, SecureDataLocation.UrlRest);
            resultUrl = urlCleaner.CleanSecureData(resultUrl, secureKeyInQuery, SecureDataLocation.UrlQuery);

            // Assert
            Assert.Equal(expectedResult, resultUrl);
        }

        [Fact]
        public void UrlCleaner_CleanSecureData_DoNotCleanSecureKeyIfWrongSecureDataLocationIsSpecified_SecureKeyNotCleared()
        {
            // Arrange
            var url = "http://test.com/users/max/info?pass=1234";
            var secureKeyInRest = "users";
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/max/info?pass=1234";

            // Act
            var resultUrl = urlCleaner.CleanSecureData(url, secureKeyInRest, SecureDataLocation.Json);

            // Assert
            Assert.Equal(expectedResult, resultUrl);
        }
    }
}
