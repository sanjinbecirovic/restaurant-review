using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.Web.Models.Request
{
    public class UpdateRestaurantRatingModel
    {
        public Rating Rating { get; set; }
    }
}
