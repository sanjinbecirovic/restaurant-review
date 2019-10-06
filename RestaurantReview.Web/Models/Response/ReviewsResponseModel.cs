using System;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.Web.Models.Response
{
    public class ReviewsResponseModel
    {
        public int Id { get; set; }

        public Rating Rating { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public string Text { get; set; }

        public string UserId { get; set; }

        public UserResponseModel User { get; set; }

        public ReplyResponseModel Reply { get; set; }
    }
}
