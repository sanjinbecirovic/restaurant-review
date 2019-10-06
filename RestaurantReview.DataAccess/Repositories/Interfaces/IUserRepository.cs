using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
