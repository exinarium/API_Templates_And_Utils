using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
	public interface IUserManager<T> where T: MongoIdentityUser<string>
    {
		Task<T> FindByEmailAsync(string emailAddress);
		Task<bool> CheckPasswordAsync(T user, string password);
		Task<string> GenerateUserTokenAsync(T user, string tokenProvider, string purpose);
		Task<bool> VerifyUserTokenAsync(T user, string tokenProvider, string purpose, string token);
		Task<IdentityResult> CreateAsync(T user, string password);
		Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal);
		Task<IdentityResult> UpdateAsync(T user);
		Task<IdentityResult> ChangePasswordAsync(T user, string currentPassword, string newPassword);
    }
}

