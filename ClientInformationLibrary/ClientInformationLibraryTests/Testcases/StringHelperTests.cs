using ClientInformation.Library.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientInformationLibrary.Tests.Testcases
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void TestReverseReturnsReversedString()
        {
            // arrange
            string input = "ClientInformation";
            string expected = "noitamrofnItneilC";

            // act
            string actual = StringHelper.Reverse(input);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsNullOrWhiteSpaceDetectsBlankInput()
        {
            // arrange / act / assert
            Assert.IsTrue(StringHelper.IsNullOrWhiteSpace(null));
            Assert.IsTrue(StringHelper.IsNullOrWhiteSpace(string.Empty));
            Assert.IsTrue(StringHelper.IsNullOrWhiteSpace("   "));
            Assert.IsFalse(StringHelper.IsNullOrWhiteSpace("value"));
        }
    }
}
