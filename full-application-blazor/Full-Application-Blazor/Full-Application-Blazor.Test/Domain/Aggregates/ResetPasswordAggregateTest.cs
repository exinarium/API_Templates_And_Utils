using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Domain.Aggregates
{
    public class ResetPasswordAggregateTest
    {
        private IEmailService _emailService;
        private IResetPasswordAggregate _resetPasswordAggregate;
        private IUserManager<User> _userManager;
        private User _user;
        private IOptions<Config> _config;

        public ResetPasswordAggregateTest()
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

            var smtpWrapper = new MockSmtpWrapper();
            var httpClientWrapper = new MockSendGridHttpClientWrapper();
            var repository = new MockRepository<Email>();
            var hangfireService = new MockHangfireWrapper();
            var loggerService = new LoggerService(new MockRepository<Log>());

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
                }
            });

            _emailService = new EmailService(repository, httpClientWrapper, smtpWrapper, _config, hangfireService, loggerService);
            _resetPasswordAggregate = new ResetPasswordAggregate(_userManager, _emailService, _config);
        }

        [Fact]
        public async Task SendPasswordSuccessful()
        {
            var resetPasswordRequest = new ResetPasswordRequest
            {
                EmailAddress = "test@example.com"
            };

            await _resetPasswordAggregate.SendResetPasswordEmail(resetPasswordRequest);
            Assert.True(true);
        }

        [Fact]
        public async Task UserNullSendPassword()
        {
            var resetPasswordRequest = new ResetPasswordRequest
            {
                EmailAddress = ""
            };

            await _resetPasswordAggregate.SendResetPasswordEmail(resetPasswordRequest);
            Assert.True(true);
        }

        [Fact]
        public async Task VerifyPasswordSuccessful()
        {
            var resetPasswordRequest = new VerifyResetPasswordRequest
            {
                EmailAddress = "test@example.com",
                Token = "123456",
                CurrentPassword = "Test1234",
                NewPassword = "Test5678",
            };

            var result = await _resetPasswordAggregate.VerifyResetPasswordToken(resetPasswordRequest);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UserNullVerifyPassword()
        {
            var resetPasswordRequest = new VerifyResetPasswordRequest
            {
                EmailAddress = "",
                Token = "123456",
                CurrentPassword = "Test1234",
                NewPassword = "Test5678",
            };

            var result = await _resetPasswordAggregate.VerifyResetPasswordToken(resetPasswordRequest);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifiedFalseVerifyPassword()
        {
            var resetPasswordRequest = new VerifyResetPasswordRequest
            {
                EmailAddress = "test@example.com",
                Token = "",
                CurrentPassword = "Test1234",
                NewPassword = "Test5678",
            };

            var result = await _resetPasswordAggregate.VerifyResetPasswordToken(resetPasswordRequest);
            Assert.False(result.Success);
        }
    }
}

