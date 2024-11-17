using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Models.Email email);
        Task ExecuteSendEmail(string emailId);
    }
}

