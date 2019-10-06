using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { }
            );

            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { },
                new Restaurant { },
                new Restaurant {  }
            );
        }
    }
}
