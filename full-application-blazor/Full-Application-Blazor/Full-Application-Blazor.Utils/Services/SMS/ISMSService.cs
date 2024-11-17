using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Services
{
    public interface ISMSService
    {
        Task CreateSMSTemplateAsync(SMSTemplate smsTemplate);
        Task UpdateSMSTemplateAsync(SMSTemplate smsTemplate);
        Task DeleteSMSTemplateAsync(string smsTemplateId);
        Task<SMSTemplate> GetSMSTemplateAsync(string smsTemplateId);
        Task<List<SMSTemplate>> GetAllSMSTemplatesAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task SaveSMSAsync(SMS sms);
        Task SendSMS(string smsId);
    }
}
