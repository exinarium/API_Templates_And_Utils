using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Full_Application_Blazor.Domain.Aggregates
{
    public class LoginAggregate : ILoginAggregate
    {
        private readonly IUserManager<User> _userManager;
        private readonly IRoleManager<Role> _roleManager;
        private readonly Config _config;

        public LoginAggregate(IUserManager<User> userManager, IRoleManager<Role> roleManager, IOptions<Config> config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config?.Value;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailAddress);

            if (user == null || user.IsDeleted == State.DELETED)
            {
                return null;
            }

            var passwordMatch = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordMatch)
            {
                return null;
            }

            if (_config == null || _config.JWTConfig == null)
            {
                throw new ArgumentNullException(LoginConstants.CONFIGURATION_INCOMPLETE);
            }

            var roles = new List<Role>();

            foreach (var roleId in user.Roles)
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role != null)
                {
                    roles.Add(role);
                }
            }

            var token = GenerateJWTToken(user, roles);

            return new LoginResponse
            {
                Token = token,
                LastName = user.LastName,
                FirstName = user.FirstName,
                IsEmailVerified = user.EmailConfirmed,
                IsTelephoneVerified = user.PhoneNumberConfirmed
            };
        }

        private string GenerateJWTToken(User user, List<Role> roles)
        {
            var issuer = _config.JWTConfig.Issuer;
            var audience = _config.JWTConfig.Audience;
            var expiryMinutes = _config.JWTConfig.ExpiryMinutes;
            var key = Encoding.ASCII.GetBytes(_config.JWTConfig.Key);

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            if (!user.EmailConfirmed || !user.PhoneNumberConfirmed)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Unverified"));
            }
            else
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}

