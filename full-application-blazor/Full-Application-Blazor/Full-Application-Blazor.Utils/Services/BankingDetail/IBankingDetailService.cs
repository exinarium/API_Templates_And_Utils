using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IBankingDetailService
    {
        Task<BankingDetail> CreateAsync(BankingDetail model, string entityId);
        Task<BankingDetail> GetAsync(string id);
        Task<BankingDetail> UpdateAsync(BankingDetail model, string entityId);
        Task<BankingDetail> DeleteAsync(string id);
    }
}
