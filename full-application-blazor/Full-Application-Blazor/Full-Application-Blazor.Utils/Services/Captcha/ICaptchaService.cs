using System.Net.Http;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface ICaptchaService
    {
        Task<bool> ValidateCaptcha(string token);
    }
}
