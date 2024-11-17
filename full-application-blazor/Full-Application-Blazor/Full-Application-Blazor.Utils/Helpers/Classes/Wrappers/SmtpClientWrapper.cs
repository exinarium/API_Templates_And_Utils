using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    [ExcludeFromCodeCoverage]
    public class SmtpClientWrapper : ISmtpWrapper
    {
        private readonly SmtpClient _smtpClient;
        private readonly Config _config;
        private bool _enableSSL;
        private SmtpDeliveryMethod _smtpDeliveryMethod;
        private int _port;
        private string _host;
        private NetworkCredential _networkCredential;

        public SmtpClientWrapper(IOptions<Config> config)
        {
            _config = config.Value;
            _smtpClient = new SmtpClient();
            _enableSSL = _config.EmailConfig.EnableSSL;
            _smtpDeliveryMethod = SmtpDeliveryMethod.Network;
            _port = _config.EmailConfig.Port;
            _host = _config.EmailConfig.Host;
            _networkCredential = null;
        }

        public bool EnableSsl
        {
            get
            {
                return _enableSSL;
            }
            set
            {
                _enableSSL = value;
            }
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get
            {
                return _smtpDeliveryMethod;
            }
            set
            {
                _smtpDeliveryMethod = value;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        public NetworkCredential Credentials
        {
            get
            {
                return _networkCredential;
            }
            set
            {
                _networkCredential = value;
            }
        }

        public async Task SendAsync(MailMessage message, object userToken)
        {
            _smtpClient.EnableSsl = EnableSsl;
            _smtpClient.DeliveryMethod = DeliveryMethod;
            _smtpClient.Port =Port;
            _smtpClient.Host = Host;
            _smtpClient.Credentials = Credentials;
            _smtpClient.SendAsync(message, userToken);
        }

        public async Task SendMailAsync(MailMessage mail)
        {
            _smtpClient.EnableSsl = EnableSsl;
            _smtpClient.DeliveryMethod = DeliveryMethod;
            _smtpClient.Port = Port;
            _smtpClient.Host = Host;
            _smtpClient.Credentials = Credentials;
            await _smtpClient.SendMailAsync(mail);
        }
    }
}

