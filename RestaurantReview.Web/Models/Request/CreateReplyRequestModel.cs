using System.ComponentModel.DataAnnotations;

namespace RestaurantReview.Web.Models.Request
{
    public class CreateReplyRequestModel
    {
        [MinLength(5, ErrorMessage = "Reply must consist of 5 or more characters.")]
        [MaxLength(2000, ErrorMessage = "Reply cannot consist of 2000 or more characters.")]
        [Required(ErrorMessage = "Reply text is required.")]
        public string Text { get; set; }
    }
}
