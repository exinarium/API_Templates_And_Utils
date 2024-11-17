using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockSmtpWrapper : ISmtpWrapper
    {
        public bool EnableSsl { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public NetworkCredential Credentials { get; set; }
        public bool MailSent { get; set; }

        public MockSmtpWrapper()
        {
            
        }

        public Task SendAsync(MailMessage message, object userToken)
        {
            return Task.Run(() => Console.WriteLine("Send Async Call"));
        }

        public Task SendMailAsync(MailMessage mail)
        {
            return Task.Run(() => Console.WriteLine("Send Mail Async Call"));
        }
    }
}