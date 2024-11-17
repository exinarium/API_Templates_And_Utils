using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace Full_Application_Blazor.Domain.Aggregates
{
    public class SignupAggregate : ISignupAggregate
    {
        private readonly IUserManager<User> _userManager;
        private readonly IRoleManager<Role> _roleManager;
        private readonly IEntityService _entityService;
        private readonly IPlanService _planService;

        public SignupAggregate(IUserManager<User> userManager, IRoleManager<Role> roleManager, IEntityService entityService, IPlanService planService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _entityService = entityService;
            _planService = planService;
        }
        
        public async Task<SignupResponse> Signup(SignupRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);

            if(existingUser != null)
            {
                return null;
            }

            var entityId = await CreateEntity();
            var userCreated = await CreateUser(request, entityId);

            return new SignupResponse
            {
                Success = userCreated
            };
        }

        private async Task<string> CreateEntity()
        {
            var plans = await _planService.GetAllAsync();
            var initialPlan = plans.Where(x => x.PlanType == PlanType.FREE).FirstOrDefault();

            if(initialPlan == null)
            {
                throw new ArgumentNullException(SignupConstants.PLAN_NOT_FOUND);
            }

            var entity = await _entityService.CreateAsync(new Entity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PlanId = initialPlan.Id,
                EntityType = EntityType.INDIVIDUAL
            });

            return entity.Id;
        }

        private async Task<bool> CreateUser(SignupRequest request, string entityId)
        {
            var role = await _roleManager.FindByNameAsync("Administrator");
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                UserName = request.EmailAddress,
                IsDeleted = State.NOT_DELETED,
                LockoutEnabled = true,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                Version = 1,
                Roles = new List<string> { role.Id },
                EntityId = entityId,
                Profile = new List<Profile>
                {
                    new Profile
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        ModifiedDateTime = DateTime.UtcNow,
                        Id = ObjectId.GenerateNewId().ToString(),
                        IsDeleted = State.NOT_DELETED,
                        Version = 1
                    }
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            return result.Succeeded;
        }
    }
}

