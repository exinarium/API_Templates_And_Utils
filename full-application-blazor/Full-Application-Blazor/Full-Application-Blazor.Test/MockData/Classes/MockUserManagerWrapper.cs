using System;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockUserManagerWrapper : IdentityResult, IUserManager<User>
    {
        private User _user; 

        public MockUserManagerWrapper(User user)
        {
            _user = user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            if(string.IsNullOrEmpty(newPassword))
            {
                this.Succeeded = false;
                return this;
            }

            this.Succeeded = true;
            return this;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if(password == "CORRECT_PASSWORD")
            {
                return true;
            }

            return false;
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            this.Succeeded = true;
            return this;
        }

        public async Task<User> FindByEmailAsync(string emailAddress)
        {
            if(string.IsNullOrEmpty(emailAddress))
            {
                return null;
            }

            return _user;
        }

        public async Task<string> GenerateUserTokenAsync(User user, string tokenProvider, string purpose)
        {
            return "123456";
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal == null)
            {
                return null;
            }

            return _user;
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            this.Succeeded = true;
            return this;
        }

        public async Task<bool> VerifyUserTokenAsync(User user, string tokenProvider, string purpose, string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return false;
            }

            return true;
        }
    }
}

