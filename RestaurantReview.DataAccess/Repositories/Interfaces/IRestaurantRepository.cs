using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess.Repositories.Interfaces
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();

        Task<IEnumerable<Restaurant>> GetRestaurantsWithRatingAsync(Rating rating);

        Task<IEnumerable<Restaurant>> GetRestaurantsForUserAsync(string userId);

        Task<Restaurant> GetRestaurantWithName(string name);
    }
}
;
