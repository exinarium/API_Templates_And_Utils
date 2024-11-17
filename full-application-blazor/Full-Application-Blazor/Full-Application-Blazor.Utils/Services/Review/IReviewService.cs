using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IReviewService
    {
        Task<Review> CreateAsync(Review model);
        Task<Review> GetAsync(string id);
        Task<List<Review>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task<Review> UpdateAsync(Review model);
        Task<Review> DeleteAsync(string id);
    }
}
