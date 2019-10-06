using System;
using System.ComponentModel.DataAnnotations;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.Web.Models.Request
{
    public class CreateReviewRequestModel
    {
        [Required(ErrorMessage = "Review rating is required.")]
        public Rating Rating { get; set; }

        [MaxLength(2000, ErrorMessage = "Review Text cannot consist of 2000 or more characters.")]
        [Required(ErrorMessage = "Review Text is required.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Restaurant visit date & time is required.")]
        public DateTime DateOfVisit { get; set; }
    }
}
