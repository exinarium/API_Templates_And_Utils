using System;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Repositories;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockPaymentService : PaymentService
    {
        public bool IsNullToken { get; set; }

        public MockPaymentService(IRepository<Invoice> repository, IOptions<Config> config, IHttpClientWrapper httpClient)
            : base(repository, config, httpClient)
        {
        }

        public override async Task<string> GetPaymentToken(string customerId)
        {
            if (IsNullToken)
            {
                return null;
            }

            return Guid.NewGuid().ToString();
        }

        public override async Task SubscriptionPaymentJob()
        {
            Console.WriteLine("SubscriptionJobRun");
        }

        public override async Task<bool> UpdatePaymentToken(string customerId, string token)
        {
            return true;
        }
    }
}

