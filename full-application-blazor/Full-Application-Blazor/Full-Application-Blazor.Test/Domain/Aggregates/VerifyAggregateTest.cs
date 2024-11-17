using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Domain.Aggregates
{
    public class VerifyAggregateTest
    {
        private IEmailService _emailService;
        private ISMSService _smsService;
        private IVerifyAggregate _verifyAggregate;
        private IUserManager<User> _userManager;
        private User _user;
        private IOptions<Config> _config;
        private MockRepository<SMS> _smsRepository;
        private ITwilioWrapper _twillioWrapper;
        private IHangfireWrapper _hangfireService;
        private ILoggerService _loggerService;

        public VerifyAggregateTest()
        {
            _user = new User
            {
                Id = "63ebedb8ba81c67bca0b9bae",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM",
                UserName = "test@example.com",
                EmailConfirmed = true,
                PhoneNumber = "+27011011001",
                PhoneNumberConfirmed = true,
                FirstName = "John",
                LastName = "Doe",
                Roles = new List<string> { "63ebedb8ba81c67bca0b9baf" }
            };

            _userManager = new MockUserManagerWrapper(_user);
            _smsRepository = new MockRepository<SMS>();

            var smtpWrapper = new MockSmtpWrapper();
            var httpClientWrapper = new MockSendGridHttpClientWrapper();
            var emailRepository = new MockRepository<Email>();

            _hangfireService = new MockHangfireWrapper();
            _loggerService = new LoggerService(new MockRepository<Log>());
            _twillioWrapper = new MockTwilioWrapper();

            var smsTemplateRepository = new MockRepository<SMSTemplate>
            {
                Value = new SMSTemplate
                {
                    Id = "63ebedb8ba81c67bca0b9baf",
                    TemplateName = "VerifyPhoneNumber"
                }
            };

            _config = Options.Create<Config>(new Config
            {
                EmailConfig = new EmailConfig
                {
                    Username = "username",
                    Password = "password",
                    Host = "smtp.sendgrid.net",
                    Port = 587,
                    FromAddress = "test@creativ360.com",
                    FromName = "Creativ360 Development",
                    EnableSSL = true
                },
                SMSConfig = new SMSConfig
                {
                    FromNumber = "+27600174018",
                    AccountSid = "sid",
                    AuthToken = "auth"
                }
            });

            _emailService = new EmailService(emailRepository, httpClientWrapper, smtpWrapper, _config, _hangfireService, _loggerService);
            _smsService = new SMSService(smsTemplateRepository, _smsRepository, _config, _twillioWrapper, _hangfireService, _loggerService);
            _verifyAggregate = new VerifyAggregate(_userManager, _emailService, _smsService, _config);
        }

        [Fact]
        public async Task SendVerificationEmailSuccessful()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            await _verifyAggregate.SendVerificationEmail(claimsPrincipal);
            Assert.True(true);
        }

        [Fact]
        public async Task SendVerificationEmailWithoutUser()
        {
            await _verifyAggregate.SendVerificationEmail(null);
            Assert.True(true);
        }

        [Fact]
        public async Task SendVerificationSmsSuccessful()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            await _verifyAggregate.SendVerificationPhoneNumber(claimsPrincipal);
            Assert.True(true);
        }

        [Fact]
        public async Task SendVerificationSmsWithoutUser()
        {
            await _verifyAggregate.SendVerificationPhoneNumber(null);
            Assert.True(true);
        }

        [Fact]
        public async Task SendVerificationSmsWithoutTemplates()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var smsTemplateRepository = new MockRepository<SMSTemplate> { IsNullReturn = true };
            var smsService = new SMSService(smsTemplateRepository, _smsRepository, _config, _twillioWrapper, _hangfireService, _loggerService);
            var verifyAggregate = new VerifyAggregate(_userManager, _emailService, smsService, _config);

            Func<ClaimsPrincipal, Task> verificationSms = verifyAggregate.SendVerificationPhoneNumber;
            await Assert.ThrowsAsync<ArgumentNullException>(SMSConstants.SMS_TEMPLATE_NOT_FOUND, () => verificationSms(claimsPrincipal));
        }

        [Fact]
        public async Task VerifiyEmailSuccessful()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyEmailRequest = new VerifyEmailAddressRequest
            {
                EmailAddress = _user.Email,
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyEmailAddress(verifyEmailRequest, claimsPrincipal);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task VerifiyEmailWithoutUser()
        {
            var verifyEmailRequest = new VerifyEmailAddressRequest
            {
                EmailAddress = _user.Email,
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyEmailAddress(verifyEmailRequest, null);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiyEmailWrongEmail()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyEmailRequest = new VerifyEmailAddressRequest
            {
                EmailAddress = "test@example2.co.za",
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyEmailAddress(verifyEmailRequest, claimsPrincipal);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiyEmailWrongToken()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyEmailRequest = new VerifyEmailAddressRequest
            {
                EmailAddress = _user.Email,
                Token = ""
            };

            var result = await _verifyAggregate.VerifyEmailAddress(verifyEmailRequest, claimsPrincipal);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiyPhoneSuccessful()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyPhoneRequest = new VerifyPhoneNumberRequest
            {
                PhoneNumber = _user.PhoneNumber,
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyPhoneNumber(verifyPhoneRequest, claimsPrincipal);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task VerifiyPhoneWithoutUser()
        {
            var verifyPhoneRequest = new VerifyPhoneNumberRequest
            {
                PhoneNumber = _user.PhoneNumber,
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyPhoneNumber(verifyPhoneRequest, null);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiyPhoneWrongNumber()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyPhoneRequest = new VerifyPhoneNumberRequest
            {
                PhoneNumber = "+27121231234",
                Token = "123456"
            };

            var result = await _verifyAggregate.VerifyPhoneNumber(verifyPhoneRequest, claimsPrincipal);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiyPhoneWrongToken()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Role, "Unverified"),
            });

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var verifyPhoneRequest = new VerifyPhoneNumberRequest
            {
                PhoneNumber = _user.PhoneNumber,
                Token = ""
            };

            var result = await _verifyAggregate.VerifyPhoneNumber(verifyPhoneRequest, claimsPrincipal);
            Assert.False(result.Success);
        }
    }
}

