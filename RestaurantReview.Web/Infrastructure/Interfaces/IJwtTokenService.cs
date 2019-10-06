using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.Web.Infrastructure.Interfaces
{
    public interface IJwtTokenService
    {
        Task<JwtSecurityToken> GenerateTokenAsync(User user);

        string WriteToken(JwtSecurityToken token);
    }
}
