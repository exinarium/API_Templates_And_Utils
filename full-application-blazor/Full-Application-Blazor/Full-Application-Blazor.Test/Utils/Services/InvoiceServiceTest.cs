using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using MongoDB.Bson;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class InvoiceServiceTest : IDisposable
    {
        private IRepository<Invoice> _repository;
        private Invoice _model;
        private readonly IInvoiceService _service;

        public InvoiceServiceTest()
        {
            _repository = new MockRepository<Invoice>();

            _model = new Invoice
            {
                Id = "1234567890",
                InvoiceNumber = "Test1234567890",
                InvoiceDescription = "1234567890",
                DueDate = DateTime.UtcNow,
                InvoiceDate = DateTime.UtcNow,
                IsSentPrinted = false,
                BankDetailsId = ObjectId.GenerateNewId().ToString(),
                EntityID = "1234",
                Status = InvoiceStatus.PENDING,
                InvoiceType = InvoiceType.ONCE_OFF_PAYMENT,
                TotalAmount = 1312,
                TotalVATAmount = 1312,
                PaidAmount = 1312,
                VATPercentage = 1312,
                PaymentDate = DateTime.UtcNow,
                ManuallyProcessed = false,
                Error = null,
                ErrorCode = 0,
                LineItems = new List<LineItem>
                {
                    
                    new LineItem
                    {
                        Description = "Test.Test@Test.com",
                        Amount = 2930492,
                        Quantity = 2930492,
                        DiscountAmount = 3049,
                        DiscountPercentage = 3049,
                        ItemId = "1234567890"
                    }, 
                    new LineItem
                    {
                        Description = "TestTest",
                        Amount = 2942330492,
                        Quantity = 292,
                        DiscountAmount = 30449,
                        DiscountPercentage = 30149,
                        ItemId = "134190"
                    },
                }
            };

            _repository = new MockRepository<Invoice>
            {
                Value = _model,
            };

            _service = new InvoicesService(_repository);
        }
        public void Dispose()
        {
            _repository = null;
        }

        [Theory]
        [InlineData("Test1234567890", "Test1234567890")]
        [InlineData("Test1234567890", null)]
        [InlineData(null, "Test1234567890")]
        public async Task SaveAsync(string CustomerId, string InvoiceDescription)
        {
            _model.EntityID = CustomerId;
            _model.InvoiceDescription = InvoiceDescription;
            _repository = new MockRepository<Invoice>
            {
                Value = _model,
            };

            await _service.CreateAsync(_model);
            var test = await _service.GetAsync(_model.Id);
            Assert.True(test.EntityID == "Test1234567890" || test.EntityID == null);
        }

        [Fact]
        public async void UpdateDetails()
        {
            _model.EntityID = "25424352345";

            _repository = new MockRepository<Invoice>
            {
                Value = _model,
            };

            await _service.UpdateAsync(_model);
            var test = await _service.GetAsync(_model.Id);
            Assert.True(test.EntityID == "25424352345");
        }

        [Fact]
        public async void ListDetails()
        {
            var list = await _service.GetAllAsync();
            Assert.True(list.Any());
        }

        [Fact]
        public async void GetDetails()
        {
            var value = await _service.GetAsync(_model.Id);
            Assert.True(value.EntityID != null && value.Id == _model.Id);
        }

        [Fact]
        public async void Delete()
        {
            var val = await _service.DeleteAsync(_model.Id);
            Assert.Equal(State.DELETED, val.IsDeleted);
        }
    }
}
