using System.ComponentModel.DataAnnotations;

namespace RestaurantReview.Web.Models.Request
{
    public class LockoutRequestModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        public bool Enabled { get; set; }
    }
}
