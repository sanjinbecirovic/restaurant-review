using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Entities
{
    public class Reply : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        [ForeignKey("Review")]
        public int ReviewId { get; set; }

        [MaxLength(2000)]
        [Required]
        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public Review Review { get; set; }

        public User User { get; set; }
    }
}
