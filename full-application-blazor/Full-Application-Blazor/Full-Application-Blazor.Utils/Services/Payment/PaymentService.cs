using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Contants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.Extensions.Options;
using System.Linq;
using Full_Application_Blazor.Utils.Configuration;

namespace Full_Application_Blazor.Utils.Services
{
    public abstract class PaymentService : IPaymentService
    {
        protected readonly IRepository<Invoice> _invoiceRepository;
        protected readonly Config _config;
        protected readonly IHttpClientWrapper _httpClient;

        protected PaymentService(IRepository<Invoice> repository, IOptions<Config> config, IHttpClientWrapper httpClient)
        {
            _invoiceRepository = repository;
            _config = config?.Value;
            _httpClient = httpClient;
        }

        public async Task<Invoice> CreateInvoice(Invoice invoice)
        {
            return await _invoiceRepository.AddAsync(invoice);
        }

        public async Task<Invoice> ManuallyProcessInvoice(string invoiceId, double amount, DateTime paymentDate)
        {
            var invoice = await GetInvoice(invoiceId);

            invoice.PaidAmount = amount;
            invoice.PaymentDate = paymentDate;
            invoice.Status = InvoiceStatus.PAID;
            invoice.ModifiedDateTime = DateTime.UtcNow;
            invoice.ManuallyProcessed = true;
            invoice.Error = null;
            invoice.ErrorCode = null;


            return await _invoiceRepository.UpdateAsync(invoice);
        }

        public async Task<Invoice> ProcessInvoice(string invoiceId, string processStatus, double amount, string error = null, int? errorCode = null)
        {
            var invoice = await GetInvoice(invoiceId);

            if (processStatus == PaymentConstants.PROCESS_COMPLETE)
            {
                invoice.PaidAmount = amount;
                invoice.PaymentDate = DateTime.UtcNow;
                invoice.Status = InvoiceStatus.PAID;
                invoice.ModifiedDateTime = DateTime.UtcNow;
                invoice.ManuallyProcessed = false;
                invoice.Error = null;
                invoice.ErrorCode = null;
            }
            else
            {
                invoice.PaidAmount = 0;
                invoice.PaymentDate = null;
                invoice.Status = InvoiceStatus.UNPAID;
                invoice.ModifiedDateTime = DateTime.UtcNow;
                invoice.ManuallyProcessed = false;
                invoice.Error = error;
                invoice.ErrorCode = errorCode;
            }

            return await _invoiceRepository.UpdateAsync(invoice);
        }

        public virtual async Task<bool> SubmitPayment(string invoiceId)
        {
            var invoice = await GetInvoice(invoiceId);
            var paymentRequestBody = GeneratePaymentRequest(invoice);
            var token = await GetPaymentToken(invoice.EntityID);

            if (!string.IsNullOrEmpty(token))
            {
                var response = await MakePaymentRequest(paymentRequestBody, token);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    throw new HttpRequestException(PaymentConstants.NO_RESPONSE_FROM_PROVIDER);
                }

                var responseBodyObject = JsonSerializer.Deserialize<ExpandoObject>(responseBody);

                var rootExpandoProperties = responseBodyObject.Select(kvp => kvp).ToArray();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var processedInvoice = await ProcessInvoice(invoice.Id, PaymentConstants.PROCESS_COMPLETE, invoice.TotalAmount);
                    return processedInvoice != null;
                }
                else
                {
                    var dataObject = JsonSerializer.Deserialize<ExpandoObject>((JsonElement)(rootExpandoProperties.Where(x => x.Key == "data").FirstOrDefault().Value));

                    var dataExpandoProperties = dataObject.Select(kvp => kvp).ToArray();

                    string errorMessage = dataExpandoProperties.Where(x => x.Key == "response").FirstOrDefault().Value?.ToString() ?? PaymentConstants.UNKNOWN_ERROR;
                    string errorCode = dataExpandoProperties.Where(x => x.Key == "code").FirstOrDefault().Value?.ToString() ?? "500";

                    var processedInvoice = await ProcessInvoice(paymentRequestBody.ItemName, "ERROR", 0, errorMessage, Convert.ToInt32(errorCode));
                    return false;
                }
            }
            else
            {
                var processedInvoice = await ProcessInvoice(paymentRequestBody.ItemName, "ERROR", 0, PaymentConstants.NO_CARD_DETAILS_ERROR, 404);
                return false;
            }
        }

        public virtual string GeneratePaymentSignature(SortedDictionary<string, string> data)
        {
            var pfOutput = "";
            foreach (var key in data)
            {
                pfOutput += $"{Security.UrlEncode(key.Key)}={Security.UrlEncode(key.Value)}&";
            }

            var getString = pfOutput.Substring(0, pfOutput.Length - 1);
            return Security.CreateMD5Hash(getString);
        }

        protected async Task<Invoice> GetInvoice(string invoiceId)
        {
            var invoice = await _invoiceRepository.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new ArgumentNullException(PaymentConstants.INVOICE_DOES_NOT_EXIST);
            }

            return invoice;
        }

        protected virtual PaymentRequest GeneratePaymentRequest(Invoice invoice)
        {
            var paymentRequestBody = new PaymentRequest
            {
                MerchantId = Convert.ToInt32(_config.PaymentConfig.PayfastMerchantID),
                Version = "v1",
                Timestamp = DateTime.UtcNow.ToString("s"),
                Signature = string.Empty,
                Amount = invoice.TotalAmount,
                ItemName = $"Invoice Number: #{invoice.InvoiceNumber}",
                ITN = false
            };

            var paymentDictionary = new SortedDictionary<string, string>();
            paymentDictionary.Add("amount", paymentRequestBody.Amount.ToString());
            paymentDictionary.Add("itn", paymentRequestBody.ITN.ToString().ToLower());
            paymentDictionary.Add("item_name", paymentRequestBody.ItemName.ToString());
            paymentDictionary.Add("merchant-id", paymentRequestBody.MerchantId.ToString());
            paymentDictionary.Add("passphrase", _config.PaymentConfig.PayfastPassphrase.ToString());
            paymentDictionary.Add("timestamp", paymentRequestBody.Timestamp.ToString());
            paymentDictionary.Add("version", paymentRequestBody.Version.ToString());

            paymentRequestBody.Signature = GeneratePaymentSignature(paymentDictionary);

            return paymentRequestBody;
        }

        protected virtual async Task<HttpResponseMessage> MakePaymentRequest(PaymentRequest paymentRequest, string token)
        {
            var testing = _config.PaymentConfig.TestMode ? "?testing=true" : "";

            _httpClient.BaseAddress = new Uri($"{_config.PaymentConfig.PayfastSubscriptionURL}/");
            _httpClient.DefaultRequestHeaders.Add("merchant-id", paymentRequest.MerchantId.ToString());
            _httpClient.DefaultRequestHeaders.Add("version", paymentRequest.Version.ToString());
            _httpClient.DefaultRequestHeaders.Add("timestamp", paymentRequest.Timestamp.ToString());
            _httpClient.DefaultRequestHeaders.Add("signature", paymentRequest.Signature.ToString());

            var jsonStringBuilder = new StringBuilder();
            jsonStringBuilder.Append("{");

            jsonStringBuilder.Append($"\"amount\" : \"{paymentRequest.Amount.ToString()}\",");
            jsonStringBuilder.Append($"\"item_name\" : \"{paymentRequest.ItemName.ToString()}\",");
            jsonStringBuilder.Append($"\"itn\" : \"{paymentRequest.ITN.ToString().ToLower()}\"");

            jsonStringBuilder.Append("}");

            var content = new StringContent(jsonStringBuilder.ToString(), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"subscriptions/{token}/adhoc{testing}", content);
        }

        public abstract Task<bool> UpdatePaymentToken(string customerId, string token);

        public abstract Task<string> GetPaymentToken(string customerId);

        public abstract Task SubscriptionPaymentJob();
    }
}

