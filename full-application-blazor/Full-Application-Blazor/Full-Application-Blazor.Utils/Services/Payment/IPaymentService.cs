using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IPaymentService
    {
        Task<Invoice> CreateInvoice(Invoice invoice);
        Task<Invoice> ProcessInvoice(string invoiceId, string processStatus, double amount, string error = null, int? errorCode = null);
        Task<bool> SubmitPayment(string invoiceId);
        Task<bool> UpdatePaymentToken(string entityId, string token);
        Task<string> GetPaymentToken(string entityId);
        Task<Invoice> ManuallyProcessInvoice(string invoiceId, double amount, DateTime paymentDate);
        Task SubscriptionPaymentJob();
        string GeneratePaymentSignature(SortedDictionary<string, string> data);
    }
}

