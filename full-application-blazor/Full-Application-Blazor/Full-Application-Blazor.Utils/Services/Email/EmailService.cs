using System;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Repositories;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Net;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Full_Application_Blazor.Utils.Configuration;
using Hangfire;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Helpers.Constants;

namespace Full_Application_Blazor.Utils.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<Models.Email> _repository;
        private readonly IHttpClientWrapper _httpClient;
        private readonly ISmtpWrapper _smtpClient;
        private readonly Config _config;
        private readonly IHangfireWrapper _hangfire;
        private readonly ILoggerService _logger;

        public EmailService(
            IRepository<Models.Email> repository,
            IHttpClientWrapper httpClient,
            ISmtpWrapper smtpClient,
            IOptions<Config> config,
            IHangfireWrapper hangfire,
            ILoggerService logger)
        {
            _repository = repository;
            _httpClient = httpClient;
            _smtpClient = smtpClient;
            _config = config.Value;
            _hangfire = hangfire;
            _logger = logger;
        }

        public async Task SendEmailAsync(Models.Email email)
        {
            var savedMail = await _repository.AddAsync(email);
            _hangfire.BackgroundJobEnqueue<IEmailService>(x => x.ExecuteSendEmail(savedMail.Id));
        }

        [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 10, 30, 60, 120, 240 })]
        public async Task ExecuteSendEmail(string emailId)
        {
            try
            {
                var email = await _repository.GetAsync(emailId);

                if (email != null)
                {
                    if (email.IsSMTPEmail)
                    {
                        await SendSMTPEmail(email);
                    }
                    else
                    {
                        await SendProviderEmail(email);
                    }
                }
            }
            catch (Exception e)
            {
                var log = new Log {
                    CustomMessage = EmailConstants.HANGFIRE_ERROR+ $"{emailId}",
                    ClassName = nameof(EmailService),
                    LogType = Helpers.Enums.LogType.ERROR,
                    LogPriority = Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        private async Task SendSMTPEmail(Models.Email email)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email.From, email.FromName);
                mail.To.Add(String.Join(";", email.To));
                mail.Subject = email.Subject;
                mail.IsBodyHtml = true;
                mail.Body = email.HtmlBody;

                if(email.CC != null && email.CC.Count > 0)
                {
                    mail.CC.Add(String.Join(";", email.CC));
                }

                if (email.BCC != null && email.BCC.Count > 0)
                {
                    mail.Bcc.Add(String.Join(";", email.BCC));
                }

                if (email.ReplyTo != null && email.ReplyTo.Count > 0)
                {
                    mail.ReplyToList.Add(String.Join(";", email.ReplyTo));
                }

                if (email.AttachmentFileNames != null)
                {
                    foreach (string attachmentFilename in email.AttachmentFileNames)
                    {
                        Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                        disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                        disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                        disposition.FileName = Path.GetFileName(attachmentFilename);
                        disposition.Size = new FileInfo(attachmentFilename).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        mail.Attachments.Add(attachment);
                    }
                }

                NetworkCredential credential = new NetworkCredential(_config.EmailConfig.Username, _config.EmailConfig.Password);
                _smtpClient.EnableSsl = _config.EmailConfig.EnableSSL;
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtpClient.Port = _config.EmailConfig.Port;
                _smtpClient.Host = _config.EmailConfig.Host;
                _smtpClient.Credentials = credential;

                await _smtpClient.SendMailAsync(mail);
            }
        }

        private async Task SendProviderEmail(Models.Email email)
        {
            using (MailMessage mail = new MailMessage())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _config.SendGridConfig.SendGridApiKey);
                _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                _httpClient.BaseAddress = new Uri(_config.SendGridConfig.SendGridGetTemplateURL + _config.SendGridConfig.SendGridTemplateId);

                var template = await _httpClient.GetAsync(_httpClient.BaseAddress.ToString());
                var contents = await template.Content.ReadAsStringAsync();

                if (template.StatusCode == HttpStatusCode.OK)
                {
                    var jsonTemplate = JsonSerializer.Serialize(contents);
                    if (email.KeyValuePairs != null)
                    {
                        foreach (var kv in email.KeyValuePairs)
                        {
                            if (jsonTemplate.Contains(kv.Key.ToString()))
                            {
                                jsonTemplate.Replace(kv.Key.ToString(), kv.Value.ToString());
                            }
                        }
                    }

                    mail.From = new MailAddress(email.From);
                    mail.To.Add(String.Join(";", email.To));
                    mail.ReplyToList.Add(String.Join(";", email.ReplyTo));
                    mail.CC.Add(String.Join(";", email.CC));
                    mail.Bcc.Add(String.Join(";", email.BCC));
                    mail.Body = jsonTemplate;
                    mail.Subject = email.Subject;
                    mail.IsBodyHtml = true;

                    NetworkCredential credential = new NetworkCredential(_config.EmailConfig.Username, _config.EmailConfig.Password);
                    _smtpClient.EnableSsl = _config.EmailConfig.EnableSSL;
                    _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    _smtpClient.Port = _config.EmailConfig.Port;
                    _smtpClient.Host = _config.EmailConfig.Host;
                    _smtpClient.Credentials = credential;

                    await _smtpClient.SendMailAsync(mail);
                }
            }
        }
    }
}