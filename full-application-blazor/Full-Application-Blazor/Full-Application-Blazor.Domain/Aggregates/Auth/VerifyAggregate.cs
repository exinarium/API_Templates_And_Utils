using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Full_Application_Blazor.Domain.Aggregates
{
    public class VerifyAggregate : IVerifyAggregate
    {
        private readonly IUserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        private readonly Config _config;

        public VerifyAggregate(IUserManager<User> userManager, IEmailService emailService, ISMSService smsService, IOptions<Config> config)
        {
            _userManager = userManager;
            _emailService = emailService;
            _smsService = smsService;
            _config = config.Value;
        }

        public async Task SendVerificationEmail(ClaimsPrincipal user)
        {
            var dbUser = await _userManager.GetUserAsync(user);

            if (dbUser == null)
            {
                return;
            }

            var email = new Email
            {
                EmailType = EmailType.VERIFY_EMAIL,
                Subject = VerificationConstants.VERIFY_YOUR_EMAIL,
                To = new List<string> { dbUser.Email },
                HtmlBody = @$"
                    <p>Please use this code to verify your email address:</p>
                    <p>{await _userManager.GenerateUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "emailConfirmation")}</p>
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

        public async Task SendVerificationPhoneNumber(ClaimsPrincipal user)
        {
            var dbUser = await _userManager.GetUserAsync(user);

            if (dbUser == null)
            {
                return;
            }

            var templates = await _smsService.GetAllSMSTemplatesAsync(
                new Order
                {
                    PropertyName = "ModifiedDateTime",
                    SortDirection = SortDirection.Ascending
                },
                1, 1, null,
                new List<IFilter>
                {
                    new Filter<string>
                    {
                        Property = "TemplateName",
                        Operator = Operator.EQ,
                        Value = "VerifyPhoneNumber"
                    }
                });

            var keyValues = new Dictionary<string, string>
            {
                ["[CODE]"] = await _userManager.GenerateUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "phoneNumberConfirmation")
            };

            var templateId = templates.FirstOrDefault()?.Id;

            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException(SMSConstants.SMS_TEMPLATE_NOT_FOUND);
            }

            var sms = new SMS
            {
                ReceivingNumber = dbUser.PhoneNumber,
                KeyValuePairs = keyValues,
                TemplateId = templateId
            };

            await _smsService.SaveSMSAsync(sms);
        }

        public async Task<VerificationResponse> VerifyEmailAddress(VerifyEmailAddressRequest request, ClaimsPrincipal user)
        {
            var dbUser = await _userManager.GetUserAsync(user);

            if (dbUser?.Email != request.EmailAddress)
            {
                return new VerificationResponse
                {
                    Success = false
                };
            }

            var verified = await _userManager.VerifyUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "emailConfirmation", request.Token);

            if (verified)
            {
                dbUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(dbUser);
            }

            return new VerificationResponse
            {
                Success = verified
            };
        }

        public async Task<VerificationResponse> VerifyPhoneNumber(VerifyPhoneNumberRequest request, ClaimsPrincipal user)
        {
            var dbUser = await _userManager.GetUserAsync(user);

            if (dbUser?.PhoneNumber != request.PhoneNumber)
            {
                return new VerificationResponse
                {
                    Success = false
                };
            }

            var verified = await _userManager.VerifyUserTokenAsync(dbUser, TokenOptions.DefaultPhoneProvider, "phoneNumberConfirmation", request.Token);

            if (verified)
            {
                dbUser.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(dbUser);
            }

            return new VerificationResponse
            {
                Success = verified
            };
        }
    }
}

