using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(RestaurantReviewContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
            => await this.dbContext.Restaurants.Select(r => new Restaurant
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                Description = r.Description,
                Rating = dbContext.Reviews.Any(rw => rw.RestaurantId == r.Id) ? dbContext.Reviews.Where(rw => rw.RestaurantId == r.Id).Sum(rw => (int)rw.Rating) / dbContext.Reviews.Count(rw => rw.RestaurantId == r.Id) : 0
            }).AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Restaurant>> GetRestaurantsWithRatingAsync(Rating rating)
            => await this.dbContext.Reviews
                .Include(r => r.Restaurant)
                .AsNoTracking()
                .Where(r => r.Rating == rating)
                .Select(r => r.Restaurant)
                .Distinct()
                .ToListAsync();

        public async Task<IEnumerable<Restaurant>> GetRestaurantsForUserAsync(string userId)
            => await this.dbContext.Restaurants
            .Where(r => r.UserId == userId)
            .Select(r => new Restaurant
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                Description = r.Description,
                Rating = dbContext.Reviews.Any(rw => rw.RestaurantId == r.Id) ? dbContext.Reviews.Where(rw => rw.RestaurantId == r.Id).Sum(rw => (int)rw.Rating) / dbContext.Reviews.Count(rw => rw.RestaurantId == r.Id) : 0
            }).AsNoTracking().ToListAsync();

        public async Task<Restaurant> GetRestaurantWithName(string name)
            => await this.dbContext.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Name == name);
    }
}
