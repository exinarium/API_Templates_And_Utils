using System.Threading.Tasks;
using Twilio.Types;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
    public interface ITwilioWrapper
    {
        Task Init(string username, string password);
        Task Create(PhoneNumber from, string body, PhoneNumber to);
    }
}
