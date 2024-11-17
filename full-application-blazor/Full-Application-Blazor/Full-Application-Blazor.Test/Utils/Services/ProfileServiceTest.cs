using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using MongoDB.Bson;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class ProfileServiceTest
    {
        private IRepository<User> _userRepository;
        private IRepository<Entity> _entityRepository;
        private IProfileService _profileService;
        private Profile _profileModelIndividual;
        private Profile _profileModelOrganization;
        private Entity _entityModel;
        private User _userModel;

        public ProfileServiceTest()
        {
            _userRepository = new MockRepository<User>();
            _entityRepository = new MockRepository<Entity>();

            _profileModelIndividual = new Profile
            {
                Id = "1",
                PhysicalAddress = "kjdsfhalksdjf",
                PostalAddress = "jhdfasghjkdfgsa",
                ProfilePhoto = "5"
            };

            _profileModelOrganization = new Profile
            {
                Id = "2",
                VideoUrl = "hjsakld.com",
                OrganizationPhotos = new List<string> { "1", "2", "3", "4" }
            };

            _entityModel = new Entity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PlanId = "3",
                EntityType = EntityType.ORGANIZATION,
                OrginizationName = null,
                RegistrationNumber = null,
                VATNumber = null,
                PhysicalAddress = "Test St.",
                PostalAddress = "Test St.",
            };

            _userModel = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                EntityId = _entityModel.Id,
                Profile = new List<Profile> { _profileModelIndividual },
            };

            _entityRepository = new MockRepository<Entity>
            {
                Value = _entityModel,
            };

            _userRepository = new MockRepository<User>
            {
                Value = _userModel,
            };

            _profileService = new ProfileService(_userRepository, _entityRepository);
        }

        [Fact]
        public async void Create()
        {
            var result = await _profileService.CreateAsync(_profileModelOrganization, _userModel.Id);
            Assert.True(result.VideoUrl == "hjsakld.com");
        }

        [Fact]
        public async void Get()
        {
            var result = await _profileService.GetAsync("1", _userModel.Id);
            Assert.True(result.PhysicalAddress == "kjdsfhalksdjf");
        }

        [Fact]
        public async void UpdateIndividual()
        {
            _entityModel.EntityType = EntityType.INDIVIDUAL;
            _entityRepository = new MockRepository<Entity>
            {
                Value = _entityModel,
            };
            await _entityRepository.UpdateAsync(_entityModel);

            _profileService = new ProfileService(_userRepository, _entityRepository);

            _profileModelIndividual = new Profile
            {
                Id = "1",
                PhysicalAddress = "eqwrewqr",
                PostalAddress = "sgsadsafasfgdgs",
                ProfilePhoto = "7",
            };

            var result = await _profileService.UpdateAsync(_profileModelIndividual, _userModel.Id);
            Assert.True(result.PostalAddress == "sgsadsafasfgdgs");
        }

        [Fact]
        public async void UpdateOrganization()
        {
            await _profileService.CreateAsync(_profileModelOrganization, _userModel.Id);
            _profileModelOrganization = new Profile
            {
                Id = "2",
                VideoUrl = "hjsadfasdsafdasfkld.com",
                OrganizationPhotos = new List<string> { "1", "2", "3", "4" }
            };

            var result = await _profileService.UpdateAsync(_profileModelOrganization, _userModel.Id);
            Assert.True(result.VideoUrl == "hjsadfasdsafdasfkld.com");
        }

        [Fact]
        public async void Delete()
        {
            var result = await _profileService.DeleteAsync(_profileModelIndividual, _userModel.Id);
            Assert.True(result.IsDeleted == State.DELETED);
        }
    }
}
