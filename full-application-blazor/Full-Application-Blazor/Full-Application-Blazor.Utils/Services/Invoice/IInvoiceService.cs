using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IInvoiceService
    {
        Task<Invoice> CreateAsync(Invoice model);
        Task<Invoice> GetAsync(string id);
        Task<List<Invoice>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task<Invoice> UpdateAsync(Invoice model);
        Task<Invoice> DeleteAsync(string id);
    }
}
