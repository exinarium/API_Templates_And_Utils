using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;

namespace Full_Application_Blazor.Test.Domain.Aggregates
{
    public class SignupAggregateTest
    {
        private ISignupAggregate _signupAggregate;
        private IUserManager<User> _userManager;
        private IRoleManager<Role> _roleManager;
        private IPlanService _planService;
        private IEntityService _entityService;
        private User _user;
        private Role _role;

        public SignupAggregateTest()
        {
            _user = new User
            {
                Id = "63ebedb8ba81c67bca0b9bae",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM",
                UserName = "test@example.com",
                EmailConfirmed = false,
                PhoneNumber = "+27011011001",
                PhoneNumberConfirmed = false,
                FirstName = "John",
                LastName = "Doe"
            };

            _role = new Role
            {
                Id = "63ebedb8ba81c67bca0b9baf",
                Name = "Administrator"
            };

            _userManager = new MockUserManagerWrapper(null);
            _roleManager = new MockRoleManagerWrapper(_role);

            var entityRepository = new MockRepository<Entity>();
            var planRepository = new MockRepository<Plan>
            {
                Value = new Plan
                {
                    Id = "63ebedb8ba81c67bca0b9baf",
                    PlanType = PlanType.FREE,
                    Description = "A free plan",
                    Name = "Free"
                }
            };

            _planService = new PlanService(planRepository);
            _entityService = new EntityService(entityRepository);

            _signupAggregate = new SignupAggregate(_userManager, _roleManager, _entityService, _planService);
        }

        [Fact]
        public async Task SignupSuccessful()
        {
            var signupRequest = new SignupRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD",
                FirstName = "Test",
                LastName = "Example",
                PhoneNumber = "+27011101111"
            };

            var signupResponse = await _signupAggregate.Signup(signupRequest);
            Assert.True(signupResponse.Success);
        }

        [Fact]
        public async Task SignupUserExists()
        {
            var signupRequest = new SignupRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD",
                FirstName = "Test",
                LastName = "Example",
                PhoneNumber = "+27011101111"
            };

            var userManager = new MockUserManagerWrapper(_user);
            var signupAggregate = new SignupAggregate(userManager, _roleManager, _entityService, _planService);

            var signupResponse = await signupAggregate.Signup(signupRequest);
            Assert.Null(signupResponse);
        }

        [Fact]
        public async Task SignupPlanNotExist()
        {
            var signupRequest = new SignupRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD",
                FirstName = "Test",
                LastName = "Example",
                PhoneNumber = "+27011101111"
            };

            var repository = new MockRepository<Plan>
            {
                Value = new Plan
                {
                    Id = "63ebedb8ba81c67bca0b9baf",
                    PlanType = PlanType.UNLIMITED,
                    Description = "An unlimited plan",
                    Name = "Unlimited"
                }
            };

            var planService = new PlanService(repository);

            var signupAggregate = new SignupAggregate(_userManager, _roleManager, _entityService, planService);

            Func<SignupRequest, Task<SignupResponse>> signup = signupAggregate.Signup;
            await Assert.ThrowsAsync<ArgumentNullException>("Plan not found", () => signup(signupRequest));
        }
    }
}

