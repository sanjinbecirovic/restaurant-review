using System.ComponentModel.DataAnnotations;

namespace RestaurantReview.Web.Models.Request
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Password { get; set; }
    }
}
