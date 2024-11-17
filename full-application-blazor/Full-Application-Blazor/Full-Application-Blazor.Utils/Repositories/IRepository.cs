using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Repositories
{
    public interface IRepository<T>
        where T : IModel, new()
    {
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<T> DeleteAsync(string id, State setDeleted = State.DELETED);
        Task<T> GetAsync(string id, State isDeleted = State.NOT_DELETED);
        Task<List<T>> ListAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null, State isDeleted = State.NOT_DELETED);
    }
}

