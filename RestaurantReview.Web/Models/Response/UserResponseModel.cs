using System;
using System.Collections.Generic;

namespace RestaurantReview.Web.Models.Response
{
    public class UserResponseModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public IList<string> Roles { get; set; }
    }
}
