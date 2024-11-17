using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
    public interface ISmtpWrapper
    {
        bool EnableSsl { get; set; }
        SmtpDeliveryMethod DeliveryMethod { get; set; }
        int Port { get; set; }
        string Host { get; set; }
        NetworkCredential Credentials { get; set; }
        Task SendAsync(MailMessage message, object userToken);
        Task SendMailAsync(MailMessage mail);
    }
}
