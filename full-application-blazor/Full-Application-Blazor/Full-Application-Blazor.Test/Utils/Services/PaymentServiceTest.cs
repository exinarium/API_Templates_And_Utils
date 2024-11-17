using System;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Contants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using static MongoDB.Driver.WriteConcern;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class PaymentServiceTest : IDisposable
    {
        private readonly IPaymentService _paymentService;
        private readonly IHttpClientWrapper _httpClient;
        private readonly IOptions<Config> _config;
        private readonly Invoice _invoice;
        private IRepository<Invoice> _repository;

        public PaymentServiceTest()
        {
            _repository = new MockRepository<Invoice>();
            _httpClient = new MockPayfastHttpClient();
            _config = Options.Create<Config>(new Config
            {
                PaymentConfig = new PaymentConfig
                {
                    PayfastCancelURL = "https://www.creativ360.com",
                    PayfastItemName = "",
                    PayfastMerchantID = "123",
                    PayfastMerchantKey = "",
                    PayfastNotifyURL = "https://www.creativ360.com",
                    PayfastPassphrase = "",
                    PayfastReturnURL = "https://www.creativ360.com",
                    PayfastSubscriptionURL = "https://www.creativ360.com",
                    PayfastURL = "https://www.creativ360.com",
                    TestMode = false
                }
            });

            _paymentService = new MockPaymentService(_repository, _config, _httpClient);
            _invoice = new Invoice
            {
                AuditLogId = null,
                BankDetailsId = ObjectId.GenerateNewId().ToString(),
                CreatedDateTime = DateTime.UtcNow,
                EntityID = ObjectId.GenerateNewId().ToString(),
                DueDate = DateTime.UtcNow.AddDays(7),
                Error = null,
                ErrorCode = null,
                Id = ObjectId.GenerateNewId().ToString(),
                InvoiceDescription = "Test Invoice",
                InvoiceNumber = "INV0001",
                InvoiceType = InvoiceType.ONCE_OFF_PAYMENT,
                IsDeleted = State.NOT_DELETED,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Amount = 10,
                        Description = "A",
                        DiscountAmount = 0,
                        DiscountPercentage = 0,
                        ItemId = ObjectId.GenerateNewId().ToString(),
                        Quantity = 1
                    },
                    new LineItem
                    {
                        Amount = 20,
                        Description = "B",
                        DiscountAmount = 0,
                        DiscountPercentage = 0,
                        ItemId = ObjectId.GenerateNewId().ToString(),
                        Quantity = 1
                    },
                    new LineItem
                    {
                        Amount = 30,
                        Description = "C",
                        DiscountAmount = 0,
                        DiscountPercentage = 0,
                        ItemId = ObjectId.GenerateNewId().ToString(),
                        Quantity = 1
                    }
                },
                ManuallyProcessed = false,
                ModifiedDateTime = DateTime.UtcNow,
                PaidAmount = 0,
                PaymentDate = null,
                Status = InvoiceStatus.PENDING,
                TotalAmount = 60,
                TotalVATAmount = 0,
                VATPercentage = 0,
                Version = 1
            };
        }

        public void Dispose()
        {
        }

        [Fact]
        public void CreateInvoice()
        {
            var result = _paymentService.CreateInvoice(_invoice).GetAwaiter().GetResult();
            Assert.NotNull(result);
        }

        [Fact]
        public void GenerateSignature()
        {
            var dictionary = new SortedDictionary<string, string>
            {
                {"merchant-id", "10000100"},
                {"merchant_key","46f0cd694581a"},
                {"amount", "100" },
                {"item_name","Pieter"},
                {"itn","https://google.com"},
                {"passphrase","TestPass"},
                {"timestamp","2022-08-19T00:19:23"},
                {"version","v1"}
        };

            var result = _paymentService.GeneratePaymentSignature(dictionary);
            Assert.Equal("9f2c943cfb0a0c49f8b4982e363a1d75", result);
        }

        [Theory]
        [InlineData("62feb8948852262d1206e030", false)]
        [InlineData(null, false)]
        [InlineData("62feb8948852262d1206e030", true)]
        public void ManuallyProcessInvoice(string invoiceId, bool fixedValue)
        {
            if (string.IsNullOrEmpty(invoiceId))
            {
                Assert.Throws<ArgumentNullException>(PaymentConstants.INVOICE_DOES_NOT_EXIST, () => _paymentService.ManuallyProcessInvoice(invoiceId, _invoice.TotalAmount, DateTime.UtcNow).GetAwaiter().GetResult());
            }
            else if (fixedValue)
            {
                var invoice = _invoice;
                invoice.VATPercentage = 15;

                var repository = new MockRepository<Invoice>
                {
                    Value = invoice
                };

                var paymentService = new MockPaymentService(repository, null, _httpClient);

                var result = paymentService.ManuallyProcessInvoice(invoiceId, _invoice.TotalAmount, DateTime.UtcNow).GetAwaiter().GetResult();

                Assert.NotNull(result);
                Assert.True(result.Status == InvoiceStatus.PAID);
                Assert.True(result.ManuallyProcessed == true);
            }
            else
            {
                var result = _paymentService.ManuallyProcessInvoice(invoiceId, _invoice.TotalAmount, DateTime.UtcNow).GetAwaiter().GetResult();

                Assert.NotNull(result);
                Assert.True(result.Status == InvoiceStatus.PAID);
                Assert.True(result.ManuallyProcessed == true);
            }
        }

        [Theory]
        [InlineData(null, null, PaymentConstants.PROCESS_COMPLETE, false)]
        [InlineData(null, null, PaymentConstants.PROCESS_COMPLETE, true)]
        [InlineData("An unknown error occurred", 500, PaymentConstants.PROCESS_ERROR, false)]
        public void ProcessInvoice(string error, int? errorCode, string processStatus, bool fixedValue)
        {
            Invoice? result;

            if(fixedValue)
            {
                var invoice = _invoice;
                invoice.VATPercentage = 15;

                var repository = new MockRepository<Invoice>
                {
                    Value = invoice
                };

                var paymentService = new MockPaymentService(repository, _config, _httpClient);
                result = paymentService.ProcessInvoice("62feb8948852262d1206e030", processStatus, 50, error, errorCode).GetAwaiter().GetResult();

                Assert.True((int)result.TotalAmount == 60);
            }
            else
            {
                result = _paymentService.ProcessInvoice("62feb8948852262d1206e030", processStatus, 50, error, errorCode).GetAwaiter().GetResult();                
            }

            Assert.NotNull(result);

            if (string.IsNullOrEmpty(error))
            {
                Assert.True(result.Error == null);
                Assert.True(result.ErrorCode == null);
                Assert.True(result.Status == InvoiceStatus.PAID);
                Assert.True(result.PaymentDate != null);
            }
            else
            {
                Assert.True(result.Error == error);
                Assert.True(result.ErrorCode == errorCode);
                Assert.True(result.Status == InvoiceStatus.UNPAID);
                Assert.True(result.PaymentDate == null);
            }
        }

        [Theory]
        [InlineData(true, false, false, false, false)]
        [InlineData(false, false, false, false, true)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, false, true, false)]
        public void SubmitPayment(bool isNullToken, bool isError, bool isNoContent, bool isUnknownError, bool isTestMode)
        {
            var httpClient = new MockPayfastHttpClient
            {
                IsError = isError,
                IsNoContent = isNoContent,
                IsUnknownError = isUnknownError
            };

            var paymentService = new MockPaymentService(_repository, isTestMode? _config : Options.Create<Config>(new Config
            {
                PaymentConfig = new PaymentConfig
                {
                    PayfastCancelURL = "https://www.creativ360.com",
                    PayfastItemName = "",
                    PayfastMerchantID = "123",
                    PayfastMerchantKey = "",
                    PayfastNotifyURL = "https://www.creativ360.com",
                    PayfastPassphrase = "",
                    PayfastReturnURL = "https://www.creativ360.com",
                    PayfastSubscriptionURL = "https://www.creativ360.com",
                    PayfastURL = "https://www.creativ360.com",
                    TestMode = true
                }
            }), httpClient)
            {
                IsNullToken = isNullToken
            };

            bool result = false;

            if (isNullToken)
            {
                result = paymentService.SubmitPayment("62feb8948852262d1206e030").GetAwaiter().GetResult();

                Assert.False(result);
            }
            else
            {
                if(isNoContent)
                {
                    Assert.ThrowsAny<HttpRequestException>(() => paymentService.SubmitPayment("62feb8948852262d1206e030").GetAwaiter().GetResult());
                }
                else if(isError)
                {
                    result = paymentService.SubmitPayment("62feb8948852262d1206e030").GetAwaiter().GetResult();

                    Assert.False(result);
                }
                else if (isUnknownError)
                {
                    result = paymentService.SubmitPayment("62feb8948852262d1206e030").GetAwaiter().GetResult();

                    Assert.False(result);
                }
                else
                {
                    result = _paymentService.SubmitPayment("62feb8948852262d1206e030").GetAwaiter().GetResult();
                    Assert.True(result);
                }
            }
        }
    }
}

