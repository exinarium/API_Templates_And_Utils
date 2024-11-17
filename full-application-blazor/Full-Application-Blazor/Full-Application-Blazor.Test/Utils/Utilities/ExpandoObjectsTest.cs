using System;
using System.Dynamic;
using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Utils.Helpers.Contants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using MongoDB.Bson;

namespace Full_Application_Blazor.Test.Utils.Utilities
{
    public class ExpandoObjectsTest : IDisposable
    {
        public ExpandoObjectsTest()
        {
        }

        public void Dispose()
        {
        }

        [Theory, MemberData(nameof(ObjectData))]
        public void TestFromObject(Student value)
        {
            if (value == null)
            {
                var action = () => ExpandoObjects<Student>.FromObject(value);
                Assert.Null(value);
                Assert.Throws<ArgumentNullException>(ExpandoObjectConstants.OBJECT_VALUE_NULL_ERROR, action);
            }
            else
            {
             var expando = ExpandoObjects<Student>.FromObject(value);
                Assert.True(expando != null);

                if (expando != null)
                {
                    var ex1 = expando.Where(x => x.Key == "name")?.FirstOrDefault();
                    Assert.True(ex1 != null);
                    Assert.Equal(value.Name, ex1.Value.Value.ToString());

                    var ex2 = expando.Where(x => x.Key == "Surname")?.FirstOrDefault();
                    Assert.True(ex2 != null);
                    Assert.Equal(value.Surname, ex2.Value.Value.ToString());
                }
                else
                {
                    Assert.True(false);
                }
            }
        }

        [Theory, MemberData(nameof(ExpandoData))]
        public void TestToObject(IDictionary<string, object> values)
        {
            if (values == null)
            {
                var action = () => ExpandoObjects<Student>.ToObject(null);
                Assert.Null(values);
                Assert.Throws<ArgumentNullException>(ExpandoObjectConstants.EXPANDO_VALUE_NULL_ERROR, action);
            }
            else
            {
                var value = GetExpandoObject(values);
                var student = ExpandoObjects<Student>.ToObject(value);
                Assert.True(student != null);

                if (student != null)
                {
                    var ex1 = value.Where(x => x.Key == "name")?.FirstOrDefault();
                    Assert.True(ex1 != null);
                    Assert.Equal(ex1.Value.Value.ToString(), student.Name);

                    var ex2 = value.Where(x => x.Key == "Surname")?.FirstOrDefault();
                    Assert.True(ex2 != null);
                    Assert.Equal(ex2.Value.Value.ToString(), student.Surname);
                }
                else
                {
                    Assert.True(false);
                }
            }
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

        public static IEnumerable<object[]> ExpandoData =
        new List<object[]>
            {
                new object[] { null },
                new object[] { new Dictionary<string, object>
                {
                    { "name", "Foo" },
                    { "Surname", "Bar" },
                    { "Test", 1 }
                } },
        };

        private ExpandoObject GetExpandoObject(IDictionary<string, object> values)
        {
            var expando = new ExpandoObject();

            foreach (var value in values)
            {
                expando.TryAdd(value.Key, value.Value);
            }

            return expando;
        }
    }
}

