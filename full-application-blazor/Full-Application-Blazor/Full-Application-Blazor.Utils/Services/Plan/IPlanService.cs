using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IPlanService
    {
        Task<Plan> CreateAsync(Plan model);
        Task<Plan> GetAsync(string id);
        Task<List<Plan>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task<Plan> UpdateAsync(Plan model);
        Task<Plan> DeleteAsync(string id);
    }
}
