using System;
using System.Diagnostics.CodeAnalysis;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Classes.Wrappers;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;

namespace Full_Application_Blazor.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class AuthServices
	{
		public static void InjectAuthServices(IServiceCollection services)
		{
            services.AddScoped<IRoleManager<Role>, RoleManagerWrapper<Role>>();
            services.AddScoped<IUserManager<User>, UserManagerWrapper<User>>();
            services.AddScoped<IRepository<Entity>, Repository<Entity>>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<ILoginAggregate, LoginAggregate>();
            services.AddScoped<ISignupAggregate, SignupAggregate>();
            services.AddScoped<IVerifyAggregate, VerifyAggregate>();
            services.AddScoped<IResetPasswordAggregate, ResetPasswordAggregate>();
        }
	}
}

