using Full_Application_Blazor.Utils.Models;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IProfileService
    {
        Task<Profile> CreateAsync(Profile model, string userId);
        Task<Profile> GetAsync(string profileId, string userId);
        Task<Profile> UpdateAsync(Profile model, string userId);
        Task<Profile> DeleteAsync(Profile model, string userId);
    }
}
