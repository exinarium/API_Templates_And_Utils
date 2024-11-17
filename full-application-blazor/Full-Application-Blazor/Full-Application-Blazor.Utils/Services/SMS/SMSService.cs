using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Hangfire;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Full_Application_Blazor.Utils.Services
{
    public class SMSService : ISMSService
    {
        private readonly IOptions<Config> _SMSConfig;
        private readonly IRepository<SMSTemplate> _repoTemplate;
        private readonly IRepository<SMS> _repoSMS;
        private readonly ITwilioWrapper _wrapper;
        private readonly IHangfireWrapper _hangfire;
        private readonly ILoggerService _logger;

        public SMSService(
            IRepository<SMSTemplate> repoTemplate,
            IRepository<SMS> repoSMS,
            IOptions<Config> sMSConfig,
            ITwilioWrapper wrapper,
            IHangfireWrapper hangfire,
            ILoggerService logger)
        {
            _repoTemplate = repoTemplate;
            _repoSMS = repoSMS;
            _SMSConfig = sMSConfig;
            _wrapper = wrapper;
            _hangfire = hangfire;
            _logger = logger;
        }

        public async Task CreateSMSTemplateAsync(SMSTemplate smsTemplate)
        {
            await _repoTemplate.AddAsync(smsTemplate);
        }

        public async Task UpdateSMSTemplateAsync(SMSTemplate smsTemplate)
        {
            await _repoTemplate.UpdateAsync(smsTemplate);
        }

        public async Task DeleteSMSTemplateAsync(string smsTemplateId)
        {
            await _repoTemplate.DeleteAsync(smsTemplateId);
        }

        public async Task<SMSTemplate> GetSMSTemplateAsync(string smsTemplateId)
        {
            return await _repoTemplate.GetAsync(smsTemplateId);
        }

        public async Task<List<SMSTemplate>> GetAllSMSTemplatesAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null)
        {
            return await _repoTemplate.ListAsync(order, pageNumber, itemsPerPage, search, filters);
        }

        public async Task SaveSMSAsync(SMS sms)
        {
            var savedSMS = await _repoSMS.AddAsync(sms);
            _hangfire.BackgroundJobEnqueue<ISMSService>(x => x.SendSMS(savedSMS.Id));
        }

        [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 10, 30, 60, 120, 240 })]
        public async Task SendSMS(string smsId)
        {
            try
            {
                var smsSend = await _repoSMS.GetAsync(smsId);
                var smsTemplate = await GetSMSTemplateAsync(smsSend.TemplateId);

                if (smsTemplate == null || smsTemplate.IsApproved != ApprovalStatus.APPROVED || !smsTemplate.IsActive)
                {
                    throw new ArgumentException(SMSConstants.SMS_TEMPLATE_NOT_FOUND);
                }

                smsSend.Text = ReplaceKeyValuePairs(smsTemplate.TemplateText, smsSend.KeyValuePairs);

                if (ValidateData(smsSend))
                {
                    await Send(smsSend);
                }
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = SMSConstants.HANGFIRE_ERROR+$"{ smsId}",
                    ClassName = nameof(SMSService),
                    LogType = Helpers.Enums.LogType.ERROR,
                    LogPriority = Helpers.Enums.LogPriority.HIGH,
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        private bool ValidateData(SMS sms)
        {
            if (string.IsNullOrEmpty(sms.ReceivingNumber))
            {
                sms.IsSent = false;
                sms.ErrorCodes = SMSConstants.MOBILE_NUMBER_EMPTY;
                _repoSMS.UpdateAsync(sms);

                throw new ArgumentException(SMSConstants.MOBILE_NUMBER_EMPTY);
            }

            if (string.IsNullOrEmpty(sms.Text))
            {
                sms.IsSent = false;
                sms.ErrorCodes = SMSConstants.MESSAGE_EMPTY;
                _repoSMS.UpdateAsync(sms);

                throw new ArgumentException(SMSConstants.MESSAGE_EMPTY);
            }

            if (sms.Text.Length > 120)
            {
                sms.IsSent = false;
                sms.ErrorCodes = SMSConstants.MESSAGE_TO_LONG;
                _repoSMS.UpdateAsync(sms);

                throw new ArgumentException(SMSConstants.MESSAGE_TO_LONG);
            }

            if (sms.ReceivingNumber.Length != 12)
            {
                sms.IsSent = false;
                sms.ErrorCodes = SMSConstants.RECIEVING_NUMBER_INCORECT;
                _repoSMS.UpdateAsync(sms);

                throw new ArgumentException(SMSConstants.RECIEVING_NUMBER_INCORECT);
            }
            return true;
        }

        private async Task Send(SMS sms)
        {
            await _wrapper.Init(_SMSConfig.Value.SMSConfig.AccountSid, _SMSConfig.Value.SMSConfig.AuthToken);

            await _wrapper.Create(
                from: new Twilio.Types.PhoneNumber(_SMSConfig.Value.SMSConfig.FromNumber),
                body: sms.Text,
                to: new Twilio.Types.PhoneNumber(sms.ReceivingNumber)
            );

            sms.IsSent = true;
            sms.SendDate = DateTime.UtcNow;
            sms.ModifiedDateTime = DateTime.UtcNow;

            await _repoSMS.UpdateAsync(sms);
        }

        private string ReplaceKeyValuePairs(string text, Dictionary<string, string> keyPairs)
        {
            foreach (var item in keyPairs)
            {
                if (text.Contains(item.Key))
                {
                    text = text.Replace(item.Key, item.Value);
                }
            }
            return text;
        }
    }
}
