using System.ComponentModel.DataAnnotations;

namespace RestaurantReview.Web.Models.Request
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address must be of valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password must consist of 5 or more characters.")]
        [MaxLength(50, ErrorMessage = "Password can't consist of more than 50 characters.")]
        public string Password { get; set; }


        public string Role { get; set; }
    }
}
