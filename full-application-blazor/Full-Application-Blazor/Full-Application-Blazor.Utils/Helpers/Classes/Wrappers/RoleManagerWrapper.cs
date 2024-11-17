using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Full_Application_Blazor.Utils.Helpers.Classes.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class RoleManagerWrapper<T> : IRoleManager<T> where T: MongoIdentityRole<string>
    {
		private readonly RoleManager<T> _roleManager;

		public RoleManagerWrapper(RoleManager<T> roleManager)
		{
            _roleManager = roleManager;
		}

        public async Task<T> FindByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<T> FindByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
    }
}

