using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using MongoDB.Bson;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class BankingDetailsServiceTest : IDisposable
    {
        private IRepository<Entity> _entityRepository;
        private IEntityService _entityService;
        private Entity _entityModel;

        private IRepository<BankingDetail> _bankingRepository;
        private readonly IBankingDetailService _bankingService;
        private readonly BankingDetail _bankingDetail;

        public BankingDetailsServiceTest() 
        {
            _entityRepository = new MockRepository<Entity>();

            _entityModel = new Entity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PlanId = "3",
                EntityType = EntityType.ORGANIZATION,
                OrginizationName = "Test",
                RegistrationNumber = "Test",
                VATNumber = "Test",
                PhysicalAddress = "Test St.",
                PostalAddress = "Test St.",
            };

            _bankingDetail = new BankingDetail
            {
                Id = "1",
                AccountHolderName = "Test Name",
                BankName = "Test Bank",
                AccountType = AccountType.CHEQUE,
                BranchNumber = "654321",
                AccountNumber = "1234567890",
                SwiftCode = "Test01223"
            };

            _entityRepository = new MockRepository<Entity>
            {
                Value = _entityModel,
            };

            _bankingRepository = new MockRepository<BankingDetail>
            {
                Value = _bankingDetail,
            };

            _entityService = new EntityService(_entityRepository);
            _bankingService = new BankingDetailService(_entityRepository);

            _bankingService.CreateAsync(_bankingDetail, _entityModel.Id).GetAwaiter().GetResult();
        }
        public void Dispose()
        {
            _entityRepository = null;
            _bankingRepository = null;
        }

        [Fact]
        public async Task CreateAsync()
        {
            
            var test = await _bankingService.GetAsync(_entityModel.Id);
            Assert.True(test.AccountNumber == "1234567890");
        }

        [Fact]
        public async Task GetAsync()
        {
            var test = await _bankingService.GetAsync(_entityModel.Id);
            Assert.True(test.AccountNumber == "1234567890");
        }

        [Fact]
        public async void UpdateAsync()
        {
            _bankingDetail.AccountNumber = "0000007634";

            _bankingRepository = new MockRepository<BankingDetail>
            {
                Value = _bankingDetail,
            };

            await _bankingService.UpdateAsync(_bankingDetail, _entityModel.Id);
            var test = await _bankingService.GetAsync(_entityModel.Id);
            Assert.True(test.AccountNumber == "0000007634");
        }

        [Fact]
        public async void DeleteAsync()
        {
            var val = await _bankingService.DeleteAsync(_entityModel.Id);
            Assert.Equal(State.DELETED, val.IsDeleted);
        }
    }
}