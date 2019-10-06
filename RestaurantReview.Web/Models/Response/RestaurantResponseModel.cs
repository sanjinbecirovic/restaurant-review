using System.Collections.Generic;

namespace RestaurantReview.Web.Models.Response
{
    public class RestaurantResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public string UserId { get; set; }

        public UserResponseModel User { get; set; }

        public IList<ReviewsResponseModel> Reviews { get; set; }
    }
}
