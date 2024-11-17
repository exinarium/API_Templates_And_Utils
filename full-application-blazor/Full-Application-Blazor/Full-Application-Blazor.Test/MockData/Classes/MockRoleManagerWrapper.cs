using System;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Test.MockData.Classes
{
	public class MockRoleManagerWrapper : IRoleManager<Role>
	{
        private Role _role;

		public MockRoleManagerWrapper(Role role)
		{
            _role = role;
		}

        public async Task<Role> FindByIdAsync(string roleId)
        {
            if(string.IsNullOrEmpty(roleId))
            {
                return null;
            }

            return _role;
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            return _role;
        }
    }
}

