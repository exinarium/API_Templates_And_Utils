using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Utils.Services
{
    public class EntityService : IEntityService
    {
        private readonly IRepository<Entity> _entityRepository;

        public EntityService(IRepository<Entity> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<Entity> CreateAsync(Entity model)
        {
            return await _entityRepository.AddAsync(model);
        }

        public async Task<Entity> GetAsync(string id)
        {
            return await _entityRepository.GetAsync(id);
        }

        public async Task<List<Entity>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null)
        {
            return await _entityRepository.ListAsync(order, pageNumber, itemsPerPage, search, filters);
        }

        public async Task<Entity> UpdateAsync(Entity model)
        {
             return await _entityRepository.UpdateAsync(model);
        }

        public async Task<Entity> DeleteAsync(string id)
        {
            return await _entityRepository.DeleteAsync(id);
        }
    }
}

