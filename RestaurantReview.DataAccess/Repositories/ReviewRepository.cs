using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(RestaurantReviewContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IList<Review>> GetReviewsForRestaurantAsync(int restaurantId)
            => await this.dbContext.Reviews.AsNoTracking().Where(r => r.RestaurantId == restaurantId).ToListAsync();


    }
}
