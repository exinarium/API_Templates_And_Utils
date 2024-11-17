using Full_Application_Blazor.Utils.Configuration;
using Microsoft.Extensions.Options;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Test.MockData.Classes;


namespace Full_Application_Blazor.Test.Utils.Services
{
    public class SMSServiceTest : IDisposable
    {
        private readonly IOptions<Config> _smsConfig;
        private ISMSService _smsService;

        private readonly ITwilioWrapper _wrapper;
        private readonly IHangfireWrapper _hangfire;

        private IRepository<SMSTemplate> _templateRepository;
        private IRepository<SMSTemplate> _templateRepository2;
        private IRepository<SMS> _smsRepository;

        private readonly LoggerService _loggerService;

        private SMSTemplate _smsTemplate;
        private SMSTemplate _smsTemplate2;
        private SMSTemplate _smsTemplate3;
        private SMSTemplate _smsTemplate4;
        private SMS _SMS;

        public SMSServiceTest()
        {
            _smsRepository = new MockRepository<SMS>();
            _hangfire = new MockHangfireWrapper();

            _wrapper = new MockTwilioWrapper();

            _smsConfig = Options.Create<Config>(new Config
            {
                SMSConfig = new SMSConfig
                {
                    FromNumber = "+1712345678",
                    AccountSid = "sid",
                    AuthToken = "auth",
                }
            });

            _smsTemplate = new SMSTemplate
            {
                Id = "1",
                TemplateName = "Test",
                IsActive = true,
                TemplateText = "This is a Test CLIENT_NAME and CLIENT_NUMBER",
                IsApproved = ApprovalStatus.APPROVED,
                ApprovedUserId = "1",
            };

            _smsTemplate2 = new SMSTemplate
            {
                Id = "2",
                TemplateName = "Test",
                IsActive = false,
                TemplateText = "This is a Test CLIENT_NAME and CLIENT_NUMBER",
                IsApproved = ApprovalStatus.APPROVED,
                ApprovedUserId = "1",
            };

            _smsTemplate3 = new SMSTemplate
            {
                Id = "3",
                TemplateName = "Test",
                IsActive = false,
                TemplateText = "This is a Test CLIENT_NAME and CLIENT_NUMBER",
                IsApproved = ApprovalStatus.PENDING,
                ApprovedUserId = "1",
            };

            _smsTemplate4 = new SMSTemplate
            {
                Id = "4",
                TemplateName = "Test",
                IsActive = false,
                TemplateText = "This is a Test CLIENT_NAME and CLIENT_NUMBER",
                IsApproved = ApprovalStatus.REJECTED,
                ApprovedUserId = "1",
            };

            _SMS = new SMS
            {
                Id = "1",
                ReceivingNumber = "+27712345678",
                TemplateId = "1",
                KeyValuePairs = new Dictionary<string, string>() { { "CLIENT_NAME", "Jimmy"},{ "CLIENT_NUMBER", "Test321"} },
                IsSent = false,
            };

            _templateRepository = new MockRepository<SMSTemplate>
            {
                Value = _smsTemplate,
            };

            _templateRepository2 = new MockRepository<SMSTemplate>
            {
                Value = null
            };

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            _loggerService = new LoggerService(new MockRepository<Log>());
            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);
        }

        public void Dispose()
        {
            _templateRepository = null;
            _smsRepository = null;
        }

        [Fact]
        public async void SaveSMSTemplate()
        {
            await _smsService.CreateSMSTemplateAsync(_smsTemplate);
            Assert.True(true);
        }

        [Fact]
        public async void UpdateSMSTemplate()
        {
            await _smsService.UpdateSMSTemplateAsync(_smsTemplate);
            Assert.True(true);
        }

        [Fact]
        public async void GetSMSTemplate()
        {
            var result = await _smsService.GetSMSTemplateAsync(_smsTemplate.Id);
            Assert.True(result != null);
        }

        [Fact]
        public async void DeleteSMSTemplate()
        {
            await _smsService.DeleteSMSTemplateAsync(_smsTemplate.Id);
            Assert.True(true);
        }

        [Fact]
        public async void SaveSendSMS()
        {
            await _smsService.SaveSMSAsync(_SMS);
            Assert.True(true);
        }

        [Fact]
        public async void SendSMS()
        {
            await _smsService.SendSMS(_SMS.Id);
            Assert.True(true);
        }


        [Fact]
        public async void SendNull1()
        {
            Assert.Throws<NullReferenceException>(() => _smsService.SendSMS(null).GetAwaiter().GetResult());
        }

        [Fact]
        public async void NotActive()
        {
            _smsTemplate.IsActive = false;
            _templateRepository = new MockRepository<SMSTemplate>
            {
                Value = _smsTemplate,
            };

            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        [Fact]
        public async void RecievingNumberNull()
        {
            _SMS.ReceivingNumber = null;

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            var template = _smsTemplate;
            template.TemplateText = "This is a testThis is a testThis is a testThis is a testThis is a testThis is a testThis is a testThis is a testThis is a test";

            _templateRepository = new MockRepository<SMSTemplate>
            {
                Value = _smsTemplate,
            };

            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData("okMqWOUI89MUFJdzvyFZ4SCTozmFjitOTGcgzV47t4gwxkCAhuogB0PRk9ey8XLO5G8l36FSprxkmKk3tsWbUUPVS2WG4a7P7OFHickzT7kD2uxUsnd7Q4GO1")]
        [InlineData("")]
        public async void TestMessageToLongOrEmpty(string text)
        {
            var template = _smsTemplate;
            template.TemplateText = text;

            _SMS.TemplateId = "1";
            _SMS.ReceivingNumber = "+27712345678";

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            _templateRepository = new MockRepository<SMSTemplate>
            {
                Value = _smsTemplate,
            };

            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        

        [Fact]
        public async void RecievingNumberIncorect()
        {
            _SMS.ReceivingNumber = "+27712345678";
            _SMS.TemplateId = "1";

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        [Fact]
        public async void RecievingTemplateNotSpecified()
        {
            _SMS.ReceivingNumber = "+27712345678";

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            _smsService = new SMSService(_templateRepository, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        [Fact]
        public async void RecievingTemplateNull()
        {
            _SMS.ReceivingNumber = "+27712345678";

            _smsRepository = new MockRepository<SMS>
            {
                Value = _SMS,
            };

            _smsService = new SMSService(_templateRepository2, _smsRepository, _smsConfig, _wrapper, _hangfire, _loggerService);

            Assert.Throws<ArgumentException>(() => _smsService.SendSMS(_SMS.Id).GetAwaiter().GetResult());
        }

        [Fact]
        public async void ListSMSTemplates()
        {
            var list = await _smsService.GetAllSMSTemplatesAsync();
            Assert.True(list.Any());
        }
    }
}
