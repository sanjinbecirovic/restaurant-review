using System;

namespace RestaurantReview.Web.Models.Response
{
    public class LoginResponseModel
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
