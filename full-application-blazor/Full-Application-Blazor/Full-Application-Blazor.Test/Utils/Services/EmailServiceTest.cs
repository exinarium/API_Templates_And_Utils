using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class EmailServiceTest : IDisposable
    {
        private readonly IEmailService _emailService;
        private IRepository<Email> _repository;
        private readonly IHttpClientWrapper _httpClient;
        private ISmtpWrapper _smtpClient;
        private readonly Email _email;
        private readonly IOptions<Config> _config;
        private readonly IHangfireWrapper _hangfire;

        private static SendgridTemplate _sendgridTemplate
        {
            get
            {
                return new SendgridTemplate
                {
                    DynamicTemplateData = new Dictionary<string, string>
                    {
                        {"EMAIL", "Test.Test@Test.com"},
                        {"MOBILE_NUMBER", "2930492390"},
                        {"MESSAGE", "This is a Test"}
                    }
                };
            }
        }

        public EmailServiceTest()
        {
            _repository = new MockRepository<Email>();
            _httpClient = new MockSendGridHttpClientWrapper();
            _smtpClient = new MockSmtpWrapper();
            _hangfire = new MockHangfireWrapper();

            _config = Options.Create<Config>(new Config
            {
                EmailConfig = new EmailConfig
                {
                    Port = 23432,
                    Host = "localhost",
                    Username = "dsaasd",
                    Password = "password",
                    FromAddress = "test@test.com",
                    FromName = "Test",
                    EnableSSL = true
                },
                SendGridConfig = new SendGridConfig
                {
                    SendGridGetTemplateURL = "https://api.sendgrid.com/v3/templates/",
                    SendGridPostMailURL = "https://www.creativ360.com/",
                    SendGridusername = "key",
                    SendGridPublicId = "id",
                    SendGridTemplateId = "templateId",
                }
            });

            List<string> emailList = new List<string> { "fsdhfjn@hdsadas.com" };
            var testFile = $".{Path.DirectorySeparatorChar}MockData{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}MockTest.txt";

            _email = new Email
            {
                Id = "emailId",
                To = emailList,
                CC = emailList,
                BCC = emailList,
                ReplyTo = emailList,
                AttachmentFileNames = new List<string> { testFile },
                HtmlBody = "fsdhfjn@hdsadas.com",
                EmailType = EmailType.OTHER,
                FromName = _config.Value.EmailConfig.FromName,
                Subject = "fsdhfjn@hdsadas.com",
                From = _config.Value.EmailConfig.FromAddress,
                IsHtml = true,
                IsSMTPEmail = true,
            };

            _repository = new MockRepository<Email>
            {
                Value = _email,
            };

            var loggerService = new LoggerService(new MockRepository<Log>());
            _emailService = new EmailService(_repository, _httpClient, _smtpClient, _config, _hangfire, loggerService);
        }

        public void Dispose()
        {
            _smtpClient = null;
            _repository = null;
        }

        [Fact]
        public async void TestExecuteSendEmailSendProviderEmailNull()
        {
            await _emailService.ExecuteSendEmail(null);
        }

        [Fact]
        public async void TestSendEmailAsync()
        {
            await _emailService.SendEmailAsync(_email);
        }

        
        [Fact]
        public async void TestExecuteSendEmailSendSMTPEmail()
        {
            await _emailService.ExecuteSendEmail(_email.Id);
        }

        [Fact]
        public async void TestExecuteSendEmailSendProviderEmail()
        {
            _email.AttachmentFileNames = null;
            _email.KeyValuePairs = _sendgridTemplate.DynamicTemplateData;
            _email.IsSMTPEmail = false;

            _repository = new MockRepository<Email>
            {
                Value = _email,
            };

            await _emailService.ExecuteSendEmail(_email.Id);
        }

        [Fact]
        public async void TestExecuteSendEmailSendProviderEmailException()
        {
            
            _email.To = null;
            _email.ReplyTo = null;
            _email.IsSMTPEmail = false;

            _repository = new MockRepository<Email>
            {
                Value = _email,
            };

            try 
            { 
                await _emailService.ExecuteSendEmail(_email.Id); 
            } 
            catch (Exception) 
            {
                Assert.Throws<FormatException>(() => _emailService.ExecuteSendEmail(_email.Id).GetAwaiter().GetResult());
            }
        }

        [Fact]
        public async void TestExecuteSendEmailNull()
        {
            _repository = new MockRepository<Email>
            {
                Value = null,
            };

            await _emailService.ExecuteSendEmail(null);
        }

        [Fact]
        public async void TestNullLists()
        {
            _email.CC = null;
            _email.BCC = null;
            _email.ReplyTo = null;

            _repository = new MockRepository<Email>
            {
                Value = _email,
            };

            await _emailService.ExecuteSendEmail(_email.Id);
        }

        [Fact]
        public async void TestEmptyLists()
        {
            _email.CC = new List<string>();
            _email.BCC = new List<string>();
            _email.ReplyTo = new List<string>();

            _repository = new MockRepository<Email>
            {
                Value = _email,
            };

            await _emailService.ExecuteSendEmail(_email.Id);
        }
    }
}