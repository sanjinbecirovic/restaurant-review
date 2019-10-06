using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Infrastructure.Interfaces;

namespace RestaurantReview.Web.Infrastructure
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenSettings JwtTokenSettings;
        private readonly UserManager<User> UserManager;

        public JwtTokenService(IOptions<JwtTokenSettings> jwtTokenSettingsProvider, UserManager<User> userManager)
        {
            this.JwtTokenSettings = jwtTokenSettingsProvider.Value;
            this.UserManager = userManager;
        }

        public async Task<JwtSecurityToken> GenerateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            var roles = await UserManager.GetRolesAsync(user);
            if (roles != null && roles.Count() > 0)
            {
                foreach (var r in roles)
                    claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenSettings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(JwtTokenSettings.ExpireDays);

            var token = new JwtSecurityToken(
                JwtTokenSettings.Issuer,
                JwtTokenSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return token;
        }

        public string WriteToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
