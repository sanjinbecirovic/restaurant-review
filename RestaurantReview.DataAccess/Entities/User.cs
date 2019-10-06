using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RestaurantReview.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            Reviews = new HashSet<Review>();
            Restaurants = new HashSet<Restaurant>();
            Replies = new HashSet<Reply>();
        }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }

    }
}
