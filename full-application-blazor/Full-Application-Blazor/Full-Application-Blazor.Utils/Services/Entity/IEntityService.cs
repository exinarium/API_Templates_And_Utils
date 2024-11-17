using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IEntityService
    {
        Task<Entity> CreateAsync(Entity model);
        Task<Entity> GetAsync(string id);
        Task<List<Entity>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task<Entity> UpdateAsync(Entity model);
        Task<Entity> DeleteAsync(string id);
    }
}

