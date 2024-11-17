using System;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
	public interface IRoleManager<T> where T: MongoIdentityRole<string>
    {
		Task<T> FindByIdAsync(string roleId);
        Task<T> FindByNameAsync(string roleName);
    }
}

