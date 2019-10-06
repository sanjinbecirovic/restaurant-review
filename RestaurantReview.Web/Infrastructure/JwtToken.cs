using System;

namespace RestaurantReview.Web.Infrastructure
{
    public class JwtToken
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
