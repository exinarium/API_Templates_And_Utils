using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Domain.Aggregates
{
    public class LoginAggregateTest
    {
        private ILoginAggregate _loginAggregate;
        private IUserManager<User> _userManager;
        private IRoleManager<Role> _roleManager;
        private User _user;
        private Role _role;
        private IOptions<Config> _config;

        public LoginAggregateTest()
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

            _userManager = new MockUserManagerWrapper(_user);
            _roleManager = new MockRoleManagerWrapper(_role);
            _config = Options.Create<Config>(new Config
            {
                JWTConfig = new JWTConfig
                {
                    Issuer = "https://creativ360.com",
                    Audience = "https://creativ360.com",
                    Key = "2s5v8y/B?E(H+MbQeThWmZq3t6w9z$C&F)J@NcRfUjXn2r5u7x!A%D*G-KaPdSgV",
                    ExpiryMinutes = 120
                }
            });


            _loginAggregate = new LoginAggregate(_userManager, _roleManager, _config);
        }

        [Fact]
        public async Task LoginSuccessful()
        {
            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.NotNull(loginResponse.Token);
            Assert.NotEmpty(loginResponse.Token);
            Assert.Equal(loginResponse.FirstName, _user.FirstName);
            Assert.Equal(loginResponse.LastName, _user.LastName);
            Assert.False(loginResponse.IsEmailVerified);
            Assert.False(loginResponse.IsTelephoneVerified);
        }

        [Fact]
        public async Task UserNull()
        {
            var loginRequest = new LoginRequest
            {
                EmailAddress = "",
                Password = "CORRECT_PASSWORD"
            };

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.Null(loginResponse);
        }

        [Fact]
        public async Task UserDeleted()
        {
            var user = _user;
            user.IsDeleted = State.DELETED;
            var userManager = new MockUserManagerWrapper(user);

            var loginAggregate = new LoginAggregate(userManager, _roleManager, _config);

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            var loginResponse = await loginAggregate.Login(loginRequest);
            Assert.Null(loginResponse);
        }

        [Fact]
        public async Task InvalidCredentials()
        {
            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "INCORRECT_PASSWORD"
            };

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.Null(loginResponse);
        }

        [Fact]
        public async Task ConfigNull()
        {
            var loginAggregate = new LoginAggregate(_userManager, _roleManager, null);

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            Func<LoginRequest, Task<LoginResponse>> login = loginAggregate.Login;

            await Assert.ThrowsAsync<ArgumentNullException>("Configuration incomplete to generate user token", () => login(loginRequest));
        }

        [Fact]
        public async Task JwtConfigNull()
        {
            var loginAggregate = new LoginAggregate(_userManager, _roleManager, Options.Create<Config>(new Config()));

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            Func<LoginRequest, Task<LoginResponse>> login = loginAggregate.Login;

            await Assert.ThrowsAsync<ArgumentNullException>("Configuration incomplete to generate user token", () => login(loginRequest));
        }

        [Fact]
        public async Task UserWithRoles()
        {
            var user = _user;
            user.Roles = new List<string> { "63ebedb8ba81c67bca0b9baf", "1234" };
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;

            var userManager = new MockUserManagerWrapper(user);
            var loginAggregate = new LoginAggregate(_userManager, _roleManager, Options.Create<Config>(new Config()));

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            Func<LoginRequest, Task<LoginResponse>> login = loginAggregate.Login;

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.NotNull(loginResponse.Token);
            Assert.NotEmpty(loginResponse.Token);
            Assert.Equal(loginResponse.FirstName, _user.FirstName);
            Assert.Equal(loginResponse.LastName, _user.LastName);
            Assert.True(loginResponse.IsEmailVerified);
            Assert.True(loginResponse.IsTelephoneVerified);
        }

        [Fact]
        public async Task UserEmailConfirmed()
        {
            var user = _user;
            user.EmailConfirmed = true;

            var userManager = new MockUserManagerWrapper(user);
            var loginAggregate = new LoginAggregate(_userManager, _roleManager, Options.Create<Config>(new Config()));

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            Func<LoginRequest, Task<LoginResponse>> login = loginAggregate.Login;

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.NotNull(loginResponse.Token);
            Assert.NotEmpty(loginResponse.Token);
            Assert.Equal(loginResponse.FirstName, _user.FirstName);
            Assert.Equal(loginResponse.LastName, _user.LastName);
            Assert.True(loginResponse.IsEmailVerified);
            Assert.False(loginResponse.IsTelephoneVerified);
        }

        [Fact]
        public async Task UserPhoneConfirmed()
        {
            var user = _user;
            user.PhoneNumberConfirmed = true;

            var userManager = new MockUserManagerWrapper(user);
            var loginAggregate = new LoginAggregate(_userManager, _roleManager, Options.Create<Config>(new Config()));

            var loginRequest = new LoginRequest
            {
                EmailAddress = "test@example.com",
                Password = "CORRECT_PASSWORD"
            };

            Func<LoginRequest, Task<LoginResponse>> login = loginAggregate.Login;

            var loginResponse = await _loginAggregate.Login(loginRequest);
            Assert.NotNull(loginResponse.Token);
            Assert.NotEmpty(loginResponse.Token);
            Assert.Equal(loginResponse.FirstName, _user.FirstName);
            Assert.Equal(loginResponse.LastName, _user.LastName);
            Assert.False(loginResponse.IsEmailVerified);
            Assert.True(loginResponse.IsTelephoneVerified);
        }
    }
}

