using SecureDataCleanerLibrary.Cleaners;
using SecureDataCleanerLibrary.Models.Enums;
using Xunit;

namespace SecureDataCleanerLibraryTests
{
    public class JsonCleanerTests
    {
        [Fact]
        public void JsonCleaner_CleanSecureData_ClearOneSecureKeyInJsonElementValue_OneSecureKeyInJsonElementValueCleared()
        {
            // Arrange
            var json = "{\"user\": \"max\", \"pass\":\"123456\"}";
            var secureKey = "pass";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{\"user\":\"max\",\"pass\":\"XXXXXX\"}";

            // Act
            var cleanedJson = jsonCleaner.CleanSecureData(json, secureKey, SecureDataLocation.JsonElementValue);

            // Assert
            Assert.Equal(expectedResult, cleanedJson);
        }

        [Fact]
        public void JsonCleaner_CleanSecureData_ClearTwoSecureKeysInJsonElementValue_TwoSecureKeysInJsonElementValueCleared()
        {
            // Arrange
            var json = "{\"user\":\"max\",\"pass\":\"123456\"}";
            var secureKey1 = "pass";
            var secureKey2 = "user";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{\"user\":\"XXX\",\"pass\":\"XXXXXX\"}";

            // Act
            var resultXml = jsonCleaner.CleanSecureData(json, secureKey1, SecureDataLocation.JsonElementValue);
            resultXml = jsonCleaner.CleanSecureData(resultXml, secureKey2, SecureDataLocation.JsonElementValue);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }

        [Fact]
        public void JsonCleaner_CleanSecureData_ClearOneSecureKeyInJsonAttribute_OneSecureKeyInJsonAttributeCleared()
        {
            // Arrange
            var json = "{user: {value:\"max\"}, pass:{value:\"123456\"}}";
            var secureKey = "user";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{\"user\":{\"value\":\"XXX\"},\"pass\":{\"value\":\"123456\"}}";

            // Act
            var resultJson = jsonCleaner.CleanSecureData(json, secureKey, SecureDataLocation.JsonAttribute);

            // Assert
            Assert.Equal(expectedResult, resultJson);
        }

        [Fact]
        public void JsonCleaner_CleanSecureData_ClearTwoSecureKeysInJsonAttribute_TwoSecureKeysInJsonAttributeCleared()
        {
            // Arrange
            var json = "{user: {value:\"max\"}, pass:{value:\"123456\"}}";
            var secureKey1 = "pass";
            var secureKey2 = "user";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{\"user\":{\"value\":\"XXX\"},\"pass\":{\"value\":\"XXXXXX\"}}";

            // Act
            var resultJson = jsonCleaner.CleanSecureData(json, secureKey1, SecureDataLocation.JsonAttribute);
            resultJson = jsonCleaner.CleanSecureData(resultJson, secureKey2, SecureDataLocation.JsonAttribute);

            // Assert
            Assert.Equal(expectedResult, resultJson);
        }

        [Fact]
        public void JsonCleaner_CleanSecureData_ClearOneSecureKeyInJsonAttributeAndOneSecureKeyInJsonElementValue_OneSecureKeyInJsonAttributeAndOneSecureKeyInJsonElementValueCleared()
        {
            // Arrange
            var json = "{user: {value:\"max\"}, pass:{value:\"123456\"}, login: \"maxim\"}";
            var secureKeyInXmlElement = "login";
            var secureKeyInXmlAttribute = "user";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{\"user\":{\"value\":\"XXX\"},\"pass\":{\"value\":\"123456\"},\"login\":\"XXXXX\"}";

            // Act
            var resultJson = jsonCleaner.CleanSecureData(json, secureKeyInXmlElement, SecureDataLocation.JsonElementValue);
            resultJson = jsonCleaner.CleanSecureData(resultJson, secureKeyInXmlAttribute, SecureDataLocation.JsonAttribute);

            // Assert
            Assert.Equal(expectedResult, resultJson);
        }

        [Fact]
        public void JsonCleaner_CleanSecureData_DoNotCleanSecureKeyIfWrongSecureDataLocationIsSpecified_SecureKeyNotCleared()
        {
            // Arrange
            var json = "{user:{value:\"max\"},pass:{value:\"123456\"},login:\"maxim\"}";
            var secureKeyInXmlAttribute = "pass";
            var jsonCleaner = new JsonCleaner();

            var expectedResult = "{user:{value:\"max\"},pass:{value:\"123456\"},login:\"maxim\"}";

            // Act
            var resultXml = jsonCleaner.CleanSecureData(json, secureKeyInXmlAttribute, SecureDataLocation.XmlAttribute);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }
    }
}
