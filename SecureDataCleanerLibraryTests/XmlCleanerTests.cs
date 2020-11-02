using SecureDataCleanerLibrary.Cleaners;
using SecureDataCleanerLibrary.Models.Enums;
using Xunit;

namespace SecureDataCleanerLibraryTests
{
    public class XmlCleanerTests
    {
        [Fact]
        public void XmlCleaner_CleanSecureData_ClearOneSecureKeyInXmlElementValue_OneSecureKeyInXmlElementValueCleared()
        {
            // Arrange
            var xml = "<auth><user>max</user><pass>123456</pass></auth>";
            var secureKey = "pass";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth>\r\n  <user>max</user>\r\n  <pass>XXXXXX</pass>\r\n</auth>";

            // Act
            var cleanedXml = xmlCleaner.CleanSecureData(xml, secureKey, SecureDataLocation.XmlElementValue);

            // Assert
            Assert.Equal(expectedResult, cleanedXml);
        }

        [Fact]
        public void XmlCleaner_CleanSecureData_ClearTwoSecureKeysInXmlElementValue_TwoSecureKeysInXmlElementValueCleared()
        {
            // Arrange
            var xml = "<auth><user>max</user><pass>123456</pass></auth>";
            var secureKey1 = "pass";
            var secureKey2 = "user";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth>\r\n  <user>XXX</user>\r\n  <pass>XXXXXX</pass>\r\n</auth>";

            // Act
            var resultXml = xmlCleaner.CleanSecureData(xml, secureKey1, SecureDataLocation.XmlElementValue);
            resultXml = xmlCleaner.CleanSecureData(resultXml, secureKey2, SecureDataLocation.XmlElementValue);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }

        [Fact]
        public void XmlCleaner_CleanSecureData_ClearOneSecureKeyInXmlAttribute_OneSecureKeyInXmlAttributeCleared()
        {
            // Arrange
            var xml = @"<auth user=""max"" pass=""123456""/>";
            var secureKey = "user";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = @"<auth user=""XXX"" pass=""123456"" />";

            // Act
            var resultXml = xmlCleaner.CleanSecureData(xml, secureKey, SecureDataLocation.XmlAttribute);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }

        [Fact]
        public void XmlCleaner_CleanSecureData_ClearTwoSecureKeysInXmlAttribute_TwoSecureKeysInXmlAttributeCleared()
        {
            // Arrange
            var xml = @"<auth user=""max"" pass=""123456""/>";
            var secureKey1 = "pass";
            var secureKey2 = "user";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = @"<auth user=""XXX"" pass=""XXXXXX"" />";

            // Act
            var resultXml = xmlCleaner.CleanSecureData(xml, secureKey1, SecureDataLocation.XmlAttribute);
            resultXml = xmlCleaner.CleanSecureData(resultXml, secureKey2, SecureDataLocation.XmlAttribute);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }

        [Fact]
        public void XmlCleaner_CleanSecureData_ClearOneSecureKeyInXmlAttributeAndOneSecureKeyInXmlElementValue_OneSecureKeyInXmlAttributeAndOneSecureKeyInXmlElementValueCleared()
        {
            // Arrange
            var xml = @"<user login=""max"" pass=""123456"">Maxim</user>";
            var secureKeyInXmlElement = "user";
            var secureKeyInXmlAttribute = "pass";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = @"<user login=""max"" pass=""XXXXXX"">XXXXX</user>";

            // Act
            var resultXml = xmlCleaner.CleanSecureData(xml, secureKeyInXmlElement, SecureDataLocation.XmlElementValue);
            resultXml = xmlCleaner.CleanSecureData(resultXml, secureKeyInXmlAttribute, SecureDataLocation.XmlAttribute);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }

        [Fact]
        public void XmlCleaner_CleanSecureData_DoNotCleanSecureKeyIfWrongSecureDataLocationIsSpecified_SecureKeyNotCleared()
        {
            // Arrange
            var xml = @"<user login=""max"" pass=""123456"">Maxim</user>";
            var secureKeyInXmlAttribute = "pass";
            var xmlCleaner = new XmlCleaner();

            var expectedResult = @"<user login=""max"" pass=""123456"">Maxim</user>";

            // Act
            var resultXml = xmlCleaner.CleanSecureData(xml, secureKeyInXmlAttribute, SecureDataLocation.JsonAttribute);

            // Assert
            Assert.Equal(expectedResult, resultXml);
        }
    }
}
