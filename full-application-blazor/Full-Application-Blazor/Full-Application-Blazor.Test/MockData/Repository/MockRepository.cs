using System;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Test.MockData.Repository
{
    public class MockRepository<T> : IRepository<T>
        where T: IModel, new()
    {
        public T Value { get; set; }

        public bool IsNullReturn { get; set; } = false;

        public MockRepository()
        {
        }

        public async Task<T> AddAsync(T item)
        {
            return item;
        }

        public async Task<T> DeleteAsync(string id, State setDeleted = State.DELETED)
        {
            var mockResult = new T();
            mockResult.Id = id;
            mockResult.IsDeleted = setDeleted;

            return mockResult;
        }

        public async Task<T> GetAsync(string id, State isDeleted = State.NOT_DELETED)
        {
            if (IsNullReturn || string.IsNullOrEmpty(id)) 
            {
                return default(T);
            }
            else if(Value != null)
            {
                return Value;
            }
            
            var mockResult = new T();
            mockResult.Id = id;

            return mockResult;
        }

        public async Task<List<T>> ListAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null, State isDeleted = State.NOT_DELETED)
        {
            if(IsNullReturn)
            {
                return new List<T>();
            }

            var mockList = new List<T>();

            if(Value != null)
            {
                mockList.Add(Value);
            }

            for(var i = 0; i < itemsPerPage; i++)
            {
                mockList.Add(new T());
            }

            return mockList;
        }

        public async Task<T> UpdateAsync(T item)
        {
            return item;
        }
    }
}

