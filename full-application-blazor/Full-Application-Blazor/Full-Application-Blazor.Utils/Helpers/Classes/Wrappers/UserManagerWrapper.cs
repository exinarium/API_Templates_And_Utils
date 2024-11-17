using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Microsoft.AspNetCore.Identity;

namespace Full_Application_Blazor.Utils.Helpers.Classes.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class UserManagerWrapper<T> : IUserManager<T>
        where T: MongoIdentityUser<string>
    {
        private readonly UserManager<T> _userManager;

		public UserManagerWrapper(UserManager<T> userManager)
		{
            _userManager = userManager;
		}

        public async Task<IdentityResult> ChangePasswordAsync(T user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(T user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(T user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<T> FindByEmailAsync(string emailAddress)
        {
            return await _userManager.FindByEmailAsync(emailAddress);
        }

        public async Task<string> GenerateUserTokenAsync(T user, string tokenProvider, string purpose)
        {
            return await _userManager.GenerateUserTokenAsync(user, tokenProvider, purpose);
        }

        public async Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<IdentityResult> UpdateAsync(T user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> VerifyUserTokenAsync(T user, string tokenProvider, string purpose, string token)
        {
            return await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, token);
        }
    }
}

