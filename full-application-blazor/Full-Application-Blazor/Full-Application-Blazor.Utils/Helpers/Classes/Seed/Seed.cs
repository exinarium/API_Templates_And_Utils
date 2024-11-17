using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    [ExcludeFromCodeCoverage]
    public class Seed : ISeed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPlanService _planService;
        private readonly ISMSService _smsService;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager, IPlanService planService, ISMSService smsService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _planService = planService;
            _smsService = smsService;
        }

        public void SeedAll(DatabaseConfig databaseConfig, SeedConfig seedConfig)
        {
            var client = new MongoClient(databaseConfig.ConnectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);

            SeedRoles(database, seedConfig.Roles).ConfigureAwait(false).GetAwaiter().GetResult();
            SeedUsers(database, seedConfig.Users).ConfigureAwait(false).GetAwaiter().GetResult();
            SeedPlans(database, seedConfig.Plans).ConfigureAwait(false).GetAwaiter().GetResult();
            SeedSMSTemplates(database, seedConfig.SMSTemplates).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task SeedRoles(IMongoDatabase database, List<Role> roles)
        {
            if (_roleManager.Roles.Any())
            {
                return;
            }

            if (roles != null && roles.Any())
            {
                foreach(var role in roles)
                {     
                    await _roleManager.CreateAsync(new Role
                    {
                        Id = role.Id,
                        Name = role.Name
                    });
                }
            }
        }

        private async Task SeedUsers(IMongoDatabase database, List<User> users)
        {
            if (_userManager.Users.Any())
            {
                return;
            }

            if (users != null && users.Any())
            {
                foreach (var user in users)
                {
                    await _userManager.CreateAsync(new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        IsDeleted = user.IsDeleted,
                        Version = user.Version,
                        Roles = user.Roles,
                        UserName = user.UserName
                    }, user.Password);
                }
            }
        }

        private async Task SeedPlans(IMongoDatabase database, List<Plan> plans)
        {
            var existingPlans = await _planService.GetAllAsync(null, 1, 1);
            if (existingPlans.Any())
            {
                return;
            }

            if (plans != null && plans.Any())
            {
                foreach (var plan in plans)
                {
                    await _planService.CreateAsync(new Plan
                    {
                        Name = plan.Name,
                        PlanType = plan.PlanType,
                        Description = plan.Description
                    });
                }
            }
        }

        private async Task SeedSMSTemplates(IMongoDatabase database, List<SMSTemplate> templates)
        {
            var existingTemplates = await _smsService.GetAllSMSTemplatesAsync(null, 1, 1);
            if (existingTemplates.Any())
            {
                return;
            }

            if (templates != null && templates.Any())
            {
                foreach (var template in templates)
                {
                    await _smsService.CreateSMSTemplateAsync(template);
                }
            }
        }
    }
}

