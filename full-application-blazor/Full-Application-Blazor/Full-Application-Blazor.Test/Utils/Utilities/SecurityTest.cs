using System;
using Full_Application_Blazor.Utils.Helpers.Utilities;
namespace Full_Application_Blazor.Test.Utils.Utilities
{
    public class SecurityTest : IDisposable
    {
        public SecurityTest()
        {

        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void TestURLEncode()
        {
            var testString = "%!# $&'()*+,/:;=?@[]";
            var encodedString = Security.UrlEncode(testString);
            var finalString = "%25%21%23+%24%26%27%28%29%2A%2B%2C%2F%3A%3B%3D%3F%40%5B%5D";

            Assert.Equal(finalString, encodedString);
        }

        [Fact]
        public void TestCreateMD5Hash()
        {
            var testString = "This is a test string to be hashed";
            var hashString = Security.CreateMD5Hash(testString);
            var finalHash = "7f91d09f372f15f3e1694a183ac28a83";

            Assert.Equal(finalHash, hashString);
        }
    }
}

