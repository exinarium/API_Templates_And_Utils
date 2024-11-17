using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _repository;

        public ReviewService(IRepository<Review> repository)
        {
            _repository = repository;
        }

        public async Task<Review> CreateAsync(Review model)
        {
            return await _repository.AddAsync(model);
        }

        public async Task<Review> GetAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<Review>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null)
        {
            return await _repository.ListAsync(order, pageNumber, itemsPerPage, search, filters);
        }

        public async Task<Review> UpdateAsync(Review model)
        {
            return await _repository.UpdateAsync(model);
        }

        public async Task<Review> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
