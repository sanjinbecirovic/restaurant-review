using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantReviewContext dbContext;

        public UserRepository(RestaurantReviewContext context)
        {
            this.dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await this.dbContext.Users.ToListAsync();
    }
}
