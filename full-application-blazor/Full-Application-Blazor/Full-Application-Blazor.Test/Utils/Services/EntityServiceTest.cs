using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class EntityServiceTest : IDisposable
    {
        private IRepository<Entity> _repository;
        private IRepository<Entity> _repository1;
        private readonly Entity _entityModel;
        private readonly Entity _entityModel1;
        private readonly Contact _contact;
        private readonly IEntityService _entityService;
        private readonly IEntityService _entityService1;

        public EntityServiceTest()
        {
            _repository = new MockRepository<Entity>();

            _contact = new Contact
            {
                Email = "Test@test.com",
                Name = "Test",
                Phone = "0000000000"
            };

            _entityModel = new Entity
            {
                PlanId = "2",
                EntityType = EntityType.ORGANIZATION,
                OrginizationName = "Test",
                RegistrationNumber = "Test01223",
                VATNumber = "0123Test",
                PhysicalAddress = "Test St.",
                PostalAddress = "Test St.",
                Contact = _contact
            };

            _entityModel1 = new Entity
            {
                PlanId = "3",
                EntityType = EntityType.ORGANIZATION,
                OrginizationName = null,
                RegistrationNumber = null,
                VATNumber = null,
                PhysicalAddress = "Test St.",
                PostalAddress = "Test St.",
                Contact = _contact
            };

            _repository1 = new MockRepository<Entity>
            {
                Value = _entityModel1,
            };

            _repository = new MockRepository<Entity>
            {
                Value = _entityModel,
            };

            _entityService = new EntityService(_repository);
            _entityService1 = new EntityService(_repository1);
        }

        public void Dispose()
        {
            _repository = null;
        }

        [Fact]
        public async Task SaveNewEntityAsync() 
        {
            await _entityService.CreateAsync(_entityModel);
            var test = await _entityService.GetAsync(_entityModel.Id);
            Assert.True(test.PlanId == "2");
        }

        [Fact]
        public async Task SaveNewEntitySomeNullAsync()
        {
            await _entityService1.CreateAsync(_entityModel1);
            Assert.True(true);

            await _entityService.CreateAsync(_entityModel1);
            var test = await _entityService1.GetAsync(_entityModel1.Id);
            Assert.True(test.PlanId == "3");
        }

        [Fact]
        public async void UpdateEntity()
        {
            _entityModel.RegistrationNumber = "Test012ddsaf543123";

            _repository = new MockRepository<Entity>
            {
                Value = _entityModel,
            };

            await _entityService.UpdateAsync(_entityModel);
            var test = await _entityService.GetAsync(_entityModel.Id);
            Assert.True(test.RegistrationNumber == "Test012ddsaf543123");
        }

        [Fact]
        public async void ListEntity()
        {
            var list = await _entityService.GetAllAsync();
            Assert.True(list.Any());
        }

        [Fact]
        public async void GetEntity()
        {
            var value = await _entityService.GetAsync(_entityModel.Id);
            Assert.True(value.PlanId == "2");
        }

        [Fact]
        public async void DeleteEntity()
        {
            var val = await _entityService.DeleteAsync(_entityModel.Id);
            Assert.Equal(State.DELETED, val.IsDeleted);
        }

        [Theory]
        [InlineData("1234567890", "1234567890", "1234567890")]
        [InlineData(null, "1234567890", "1234567890")]
        [InlineData("1234567890", null, "1234567890")]
        [InlineData("1234567890", "1234567890", null)]
        public async void TestNullValuesonCreate(string orginizationName, string registrationNumber, string vATNumber)
        {
            _entityModel1.OrginizationName = orginizationName;
            _entityModel1.RegistrationNumber = registrationNumber;
            _entityModel1.VATNumber = vATNumber;

            _repository1 = new MockRepository<Entity>
            {
                Value = _entityModel1,
            };

            var val = await _entityService1.CreateAsync(_entityModel1);
            Assert.Equal(vATNumber, val.VATNumber);
        }
    }
}

