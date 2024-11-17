using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Test.Helpers.Services;
using MongoDB.Driver;
using DeletedState = Full_Application_Blazor.Utils.Helpers.Enums.State;

namespace Full_Application_Blazor.Test.Utils.Services;

public class RepositoryTest : MongoIntegrationTest, IDisposable
{
    public RepositoryTest()
    {
        CreateConnection();
    }

    public void Dispose()
    {
        _client.DropDatabase(_databaseName);
        _runner.Dispose();
    }

    [Fact]
    public void TestInsertDocument()
    {
        var student = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = DeletedState.NOT_DELETED,
            ModifiedDateTime = DateTime.UtcNow,
            Name = "Foo",
            Surname = "Bar"
        };

        var repository = new Repository<Student>(_client, _options);
        repository.AddAsync(student).GetAwaiter().GetResult();

        var results = _collection.FindAsync(Builders<Student>.Filter.Empty, null, CancellationToken.None).GetAwaiter().GetResult().ToList();
        Assert.True(results.Count() > 0);
    }

    [Fact]
    public void TestUpdateDocument()
    {
        var student = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = DeletedState.NOT_DELETED,
            ModifiedDateTime = DateTime.UtcNow,
            Name = "Foo",
            Surname = "Bar"
        };

        _collection.InsertOneAsync(student, null, CancellationToken.None).GetAwaiter().GetResult();

        var repository = new Repository<Student>(_client, _options);

        student.Name = "FooFoo";
        repository.UpdateAsync(student).GetAwaiter().GetResult();

        var result = _collection.FindAsync(x => x.Id == student.Id, null, CancellationToken.None).GetAwaiter().GetResult().FirstOrDefault();
        Assert.NotNull(result);
        Assert.True(result.Name == "FooFoo");
    }

    [Theory]
    [InlineData(DeletedState.DELETED, DeletedState.NOT_DELETED)]
    [InlineData(DeletedState.NOT_DELETED, DeletedState.DELETED)]
    public void TestDeleteDocument(DeletedState deleted, DeletedState initialState)
    {
        var student = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = initialState,
            ModifiedDateTime = DateTime.UtcNow,
            Name = "Foo",
            Surname = "Bar"
        };

        _collection.InsertOneAsync(student, null, CancellationToken.None).GetAwaiter().GetResult();

        var repository = new Repository<Student>(_client, _options);
        repository.DeleteAsync(student.Id, deleted).GetAwaiter().GetResult();

        var result = _collection.FindAsync(x => x.Id == student.Id, null, CancellationToken.None).GetAwaiter().GetResult().FirstOrDefault();
        Assert.NotNull(result);
        Assert.True(result.IsDeleted == deleted);
    }

    [Theory]
    [InlineData(DeletedState.DELETED)]
    [InlineData(DeletedState.NOT_DELETED)]
    public void TestGetSingleDocument(DeletedState deleted)
    {
        var student = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = deleted,
            ModifiedDateTime = DateTime.UtcNow,
            Name = "Foo",
            Surname = "Bar"
        };

        var student2 = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = deleted,
            ModifiedDateTime = DateTime.UtcNow,
            Name = "Foo",
            Surname = "Bar"
        };

        _collection.InsertOneAsync(student, null, CancellationToken.None).GetAwaiter().GetResult();
        _collection.InsertOneAsync(student2, null, CancellationToken.None).GetAwaiter().GetResult();

        var repository = new Repository<Student>(_client, _options);
        var result = repository.GetAsync(student.Id, deleted).GetAwaiter().GetResult();

        Assert.NotNull(result);
        Assert.True(result.Id == student.Id);
    }

    [Theory, MemberData(nameof(ListData))]
    public void ListMultipleDocuments(Order? order, int pageNumber, int itemsPerPage, Search? search, List<IFilter>? filters, DeletedState isDeleted)
    {
        for (var i = 0; i < 29; i++)
        {
            var student = new Student
            {
                CreatedDateTime = DateTime.UtcNow,
                IsDeleted = isDeleted,
                ModifiedDateTime = DateTime.UtcNow,
                Name = $"Foo{i}",
                Surname = $"Bar{i}"
            };

            _collection.InsertOneAsync(student, null, CancellationToken.None).GetAwaiter().GetResult();
        }

        var student2 = new Student
        {
            CreatedDateTime = DateTime.UtcNow,
            IsDeleted = isDeleted,
            ModifiedDateTime = DateTime.UtcNow,
            Name = $"Foo101",
            Surname = null
        };

        _collection.InsertOneAsync(student2, null, CancellationToken.None).GetAwaiter().GetResult();

        var repository = new Repository<Student>(_client, _options);
        var results = repository.ListAsync(order, pageNumber, itemsPerPage, search, filters, isDeleted).GetAwaiter().GetResult();

        Assert.NotNull(results);
        Assert.True(results.Count() <= itemsPerPage);

        if (order != null)
            Assert.True(order.SortDirection == SortDirection.Ascending ?
                string.CompareOrdinal(results[0].Name, results[1].Name) == -1 :
                string.CompareOrdinal(results[0].Name, results[1].Name) == 1);

        if (search != null)
        {
            if (search.Properties != null && search.Properties.Count() > 0)
                Assert.True(results[0].Surname.Contains(search.SearchString) && search.Properties[0] == "Surname");
            else if (search.Properties == null || search.Properties.Count() == 0)
                Assert.False(results[0].Surname.Contains(search.SearchString));
        }

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                if (filter != null && filter.ToString() != null)
                {
                    if (filter.Operator == Operator.EQ ||
                        filter.Operator == Operator.CONTAINS)
                    {
                        Assert.True(results[0].Surname.Contains(filter?.ToString() ?? "") && filter?.Property == "Surname");
                    }
                    else if (filter.Operator == Operator.NEQ)
                    {
                        Assert.False(results[0].Surname.Contains(filter?.ToString() ?? "") && filter?.Property == "Surname");
                    }
                    else if (
                        filter.Operator == Operator.GT ||
                        filter.Operator == Operator.GTE ||
                        filter.Operator == Operator.LT ||
                        filter.Operator == Operator.LTE
                        )
                    {
                        Assert.True(results.Count() > 0);
                    }
                    else if (filter.Operator == Operator.NOT_IN)
                    {
                        Assert.True(results.Count() > 0 && results.Where(x => x.Surname == "Bar1").FirstOrDefault() == null);
                    }
                    else if (filter.Operator == Operator.IN)
                    {
                        Assert.True(results.Count() == 3);
                    }
                }
                else if (filter != null && filter.Operator == Operator.NULL)
                {
                    Assert.True(results.Count() == 1 && results[0].Name == "Foo101");
                }
                else if (filter != null && filter.Operator == Operator.NOT_NULL)
                {
                    Assert.True(results.Count() > 0);
                }
            }
        }
    }

    public static IEnumerable<object[]> ListData =
        new List<object[]>
        {
            new object[] { null, 1, 10, null, null, DeletedState.NOT_DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Ascending
                }, 1, 10, null, null, DeletedState.DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Descending
                }, 1, 10, null, null, DeletedState.DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Ascending
                }, 2, 4, null, null, DeletedState.NOT_DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Ascending
                }, 1, 10, new Search
                {
                    Properties = new List<string> { "Surname" },
                    SearchString = "Bar2"
                }, null, DeletedState.NOT_DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Ascending
                }, 1, 10, new Search
                {
                    Properties = null,
                    SearchString = "Bar2"
                }, null, DeletedState.NOT_DELETED },
            new object[] { new Order {
                    PropertyName = "Name", SortDirection = SortDirection.Ascending
                }, 1, 10, new Search
                {
                    Properties = new List<string>(),
                    SearchString = "Bar2"
                }, null, DeletedState.NOT_DELETED },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.EQ,
                        Property = "Surname",
                        Value = "Bar1"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.NEQ,
                        Property = "Surname",
                        Value = "Bar1"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.CONTAINS,
                        Property = "Surname",
                        Value = "Bar1"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.GT,
                        Property = "Surname",
                        Value = "AAA"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.GTE,
                        Property = "Surname",
                        Value = "AAA"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.LT,
                        Property = "Surname",
                        Value = "CCC"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.LTE,
                        Property = "Surname",
                        Value = "CCC"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.NULL,
                        Property = "Surname",
                        Value = null
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.NOT_NULL,
                        Property = "Surname",
                        Value = null
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.IN,
                        Property = "Surname",
                        Value = "Bar1;Bar2;Bar3"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.NOT_IN,
                        Property = "Surname",
                        Value = "Bar1;Bar2;Bar3"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter> {
                    new Filter<string>
                    {
                        Operator = Operator.NO_OP,
                        Property = "Surname",
                        Value = "Bar1;Bar2;Bar3"
                    }
                }, DeletedState.NOT_DELETED
            },
            new object[] { null, 1, 10, null, new List<IFilter>(), DeletedState.NOT_DELETED },
    };
}
