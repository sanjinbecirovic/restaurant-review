using System.ComponentModel.DataAnnotations;

namespace RestaurantReview.Web.Models.Request
{
    public class CreateRestaurantRequestModel
    {
        [MinLength(5, ErrorMessage = "Restaurant Name cannot consist of less than 5 characters.")]
        [MaxLength(50, ErrorMessage = "Restaurant Name cannot consist of 50 or more characters.")]
        [Required(ErrorMessage = "Restaurant name is required.")]
        public string Name { get; set; }

        [MinLength(5, ErrorMessage = "Restaurant Address cannot consist of less than 5 characters.")]
        [MaxLength(50, ErrorMessage = "Restaurant Address cannot consist of 50 or more characters.")]
        [Required(ErrorMessage = "Restaurant address is required.")]
        public string Address { get; set; }

        [MinLength(5, ErrorMessage = "Restaurant Description cannot consist of less than 5 characters.")]
        [MaxLength(200, ErrorMessage = "Restaurant Description cannot consist of 50 or more characters.")]
        public string Description { get; set; }
    }
}
