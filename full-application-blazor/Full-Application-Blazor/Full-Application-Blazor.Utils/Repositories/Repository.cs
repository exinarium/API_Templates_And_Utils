using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using Full_Application_Blazor.Utils.Configuration;

namespace Full_Application_Blazor.Utils.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : IModel, new()
    {
        private readonly IMongoDatabase _database;
        private readonly Config _config;
        private readonly IMongoCollection<T> _collection;

        public Repository(IMongoClient client, IOptions<Config> config)
        {
            var collectionName = $"{Char.ToLower(typeof(T).Name[0])}{typeof(T).Name.Substring(1)}";

            _config = config.Value;
            _database = client.GetDatabase(_config.DatabaseConfig.DatabaseName);
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task<T> AddAsync(T item)
        {
            await _collection.InsertOneAsync(item, null, CancellationToken.None);
            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            return await _collection.FindOneAndReplaceAsync(x => x.Id == item.Id, item);
        }

        public async Task<T> DeleteAsync(string id, State setDeleted = State.DELETED)
        {
            var updateDefBuilder = Builders<T>.Update;
            var updateDef = updateDefBuilder.Combine(new UpdateDefinition<T>[]
            {
                updateDefBuilder.Set(x => x.IsDeleted, setDeleted)
            });

            return await _collection.FindOneAndUpdateAsync<T>(x => x.Id == id, updateDef, new FindOneAndUpdateOptions<T, T>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        public async Task<T> GetAsync(string id, State isDeleted = State.NOT_DELETED)
        {
            return (await _collection.FindAsync(x => x.Id == id && x.IsDeleted == isDeleted, new FindOptions<T>
            {
                Limit = 1

            })).FirstOrDefault();
        }

        public async Task<List<T>> ListAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null, State isDeleted = State.NOT_DELETED)
        {
            var sortDef = Builders<T>.Sort;
            var filterDefBuilder = Builders<T>.Filter;

            var filterOrList = new List<FilterDefinition<T>>();
            var filterAndList = new List<FilterDefinition<T>>();
            var filterDef = filterDefBuilder.Where(x => x.IsDeleted == isDeleted);

            if (search != null && search.Properties != null && !string.IsNullOrEmpty(search.SearchString))
            {
                foreach (var prop in search.Properties)
                {
                    var property = typeof(T).GetProperties().Where(x => x.Name == prop).FirstOrDefault();
                    filterOrList.Add(filterDefBuilder.Regex(property.Name, $".*{search.SearchString}.*"));
                }

                if (filterOrList.Count > 0)
                    filterDef &= filterDefBuilder.Or(filterOrList);
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var type = filter.TypeCode;
                    var value = filter.ToString();
                    var expression = Convert.ChangeType(value, type);
                    var definition = GetFilterDefinition(filterDefBuilder, filter, expression);

                    if (definition != filterDefBuilder.Empty)
                    {
                        filterAndList.Add(definition);
                    }
                }

                if (filterAndList.Count > 0)
                    filterDef &= filterDefBuilder.And(filterAndList);
            }

            return (await _collection.FindAsync(filterDef, new FindOptions<T>
            {
                Sort = order == null ? sortDef.Ascending("Id") :
                    order.SortDirection == SortDirection.Ascending ?
                    sortDef.Ascending(order.PropertyName) :
                    sortDef.Descending(order.PropertyName),
                Skip = pageNumber - 1,
                Limit = itemsPerPage
            })).ToList();
        }

        private FilterDefinition<T> GetFilterDefinition(FilterDefinitionBuilder<T> filterDefBuilder, IFilter filter, object? expression)
        {
            var property = typeof(T).GetProperties().Where(x => x.Name == filter.Property).FirstOrDefault();

            switch (filter.Operator)
            {
                case Operator.EQ:
                    {
                        return filterDefBuilder.Eq(property.Name, expression);
                    }
                case Operator.NEQ:
                    {
                        return filterDefBuilder.Ne(property.Name, expression);
                    }
                case Operator.CONTAINS:
                    {
                        return filterDefBuilder.Regex(property.Name, $".*{expression}.*");
                    }
                case Operator.IN:
                    {
                        var inList = new List<StringOrRegularExpression>();
                        var splitList = filter.ToString().Split(';');

                        foreach(var split in splitList)
                        {
                            inList.Add(split);
                        }

                        return filterDefBuilder.StringIn(property.Name, inList);
                    }
                case Operator.NOT_IN:
                    {
                        var inList = new List<StringOrRegularExpression>();
                        var splitList = filter.ToString().Split(';');

                        foreach (var split in splitList)
                        {
                            inList.Add(split);
                        }

                        return filterDefBuilder.StringNin(property.Name, inList);
                    }
                case Operator.NULL:
                    {
                        return filterDefBuilder.Eq(property.Name, expression);
                    }
                case Operator.NOT_NULL:
                    {
                        return filterDefBuilder.Ne(property.Name, expression);
                    }
                case Operator.GT:
                    {
                        return filterDefBuilder.Gt(property.Name, expression);
                    }
                case Operator.GTE:
                    {
                        return filterDefBuilder.Gte(property.Name, expression);
                    }
                case Operator.LT:
                    {
                        return filterDefBuilder.Lt(property.Name, expression);
                    }
                case Operator.LTE:
                    {
                        return filterDefBuilder.Lte(property.Name, expression);
                    }
                default:
                    return filterDefBuilder.Empty;
            }
        }
    }
}

