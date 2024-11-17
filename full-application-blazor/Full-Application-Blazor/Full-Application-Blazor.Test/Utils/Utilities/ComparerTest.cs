using System;
using Full_Application_Blazor.Utils.Helpers.Utilities;

namespace Full_Application_Blazor.Test.Utils.Utilities
{
    public class ComparerTest : IDisposable
    {
        public ComparerTest()
        {
        }

        public void Dispose()
        {
            
        }

        [Theory]
        [InlineData(new Byte[] { 54, 68, 69 }, new Byte[] { 54, 68, 69, 73 })]
        [InlineData(new Byte[] { 54, 68, 73, 69 }, new Byte[] { 54, 68, 69, 73 })]
        [InlineData(new Byte[] { 54, 68, 69, 73 }, new Byte[] { 54, 68, 69, 73})]
        public void TestByteArrayCompare(byte[] a1, byte[] a2)
        {
            if(a1.Length == 3)
            {
                Assert.False(Comparers.ByteArrayCompare(a1, a2));
            }
            else if (a1.Length == 4 && a1.ElementAt(3) == 69)
            {
                Assert.False(Comparers.ByteArrayCompare(a1, a2));
            }
            else
            {
                Assert.True(Comparers.ByteArrayCompare(a1, a2));
            }
        }
    }
}

