using Full_Application_Blazor.Test.MockData.Enums;
using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using MongoDB.Bson;

namespace Full_Application_Blazor.Test.Utils.Utilities
{
    public class EnumsTest : IDisposable
    {
        public EnumsTest()
        {
        }

        public void Dispose()
        {
        }

        [Fact]
        public void TestGetDescription()
        {
            var a = Enums.GetEnumDescription(DescriptiveEnum.A);
            var b = Enums.GetEnumDescription(DescriptiveEnum.B);
            var c = Enums.GetEnumDescription(DescriptiveEnum.NO_DESCRIPTION);
            var d = Enums.GetEnumDescription(null);

            Assert.Equal("One", a);
            Assert.Equal("Two", b);
            Assert.Equal("NO_DESCRIPTION", c);
            Assert.Equal(string.Empty, d);
        }

        public static IEnumerable<object[]> ObjectData =
        new List<object[]>
            {
                new object[] { null },
                new object[] { new Student
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    AuditLogId = null,
                    CreatedDateTime = DateTime.UtcNow,
                    ModifiedDateTime = DateTime.UtcNow,
                    IsDeleted = State.NOT_DELETED,
                    Name = "Foo",
                    Surname = "Bar",
                    Version = 1
                }},
        };
    }
}

