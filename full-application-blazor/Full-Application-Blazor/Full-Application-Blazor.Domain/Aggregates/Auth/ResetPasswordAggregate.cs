using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Domain.Aggregates
{
	public class ResetPasswordAggregate : IResetPasswordAggregate
    {
        private readonly IUserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly Config _config;

        public ResetPasswordAggregate(IUserManager<User> userManager, IEmailService emailService, IOptions<Config> config)
		{
            _userManager = userManager;
            _emailService = emailService;
            _config = config.Value;
        }

        public async Task SendResetPasswordEmail(ResetPasswordRequest request)
        {
            var dbUser = await _userManager.FindByEmailAsync(request.EmailAddress);

            if (dbUser == null)
            {
                return;
            }

            var email = new Email
            {
                EmailType = EmailType.FORGOT_PASSWORD,
                Subject = ResetPasswordConstants.EMAIL_SUBJECT,
                To = new List<string> { dbUser.Email },
                HtmlBody = @$"
                    <p>Please use this code to reset your password:</p>
                    <p>{await _userManager.GenerateUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "resetPassword")}</p>
                    <p>Kind regards</p>
                ",
                IsSMTPEmail = true,
                IsHtml = true,
                From = _config.EmailConfig.FromAddress,
                FromName = _config.EmailConfig.FromName,
                ReplyTo = new List<string> { _config.EmailConfig.FromAddress }
            };

            await _emailService.SendEmailAsync(email);
        }

        public async Task<ResetPasswordResponse> VerifyResetPasswordToken(VerifyResetPasswordRequest request)
        {
            var dbUser = await _userManager.FindByEmailAsync(request.EmailAddress);

            if (dbUser == null)
            {
                return new ResetPasswordResponse
                {
                    Success = false
                };
            }

            var verified = await _userManager.VerifyUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "resetPassword", request.Token);

            if (verified)
            {
                var result = await _userManager.ChangePasswordAsync(dbUser, request.CurrentPassword, request.NewPassword);
                return new ResetPasswordResponse
                {
                    Success = result.Succeeded
                };
            }

            return new ResetPasswordResponse
            {
                Success = false
            };
        }
    }
}

