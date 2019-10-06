using System;

namespace RestaurantReview.Web.Models.Response
{
    public class ReplyResponseModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime TimeStamp { get; set; }

        public UserResponseModel User { get; set; }
    }
}
