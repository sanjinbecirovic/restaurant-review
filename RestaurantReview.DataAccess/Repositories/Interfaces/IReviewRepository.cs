using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IList<Review>> GetReviewsForRestaurantAsync(int restaurantId);
    }
}
