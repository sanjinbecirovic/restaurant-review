using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Entities
{
    public class Review : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [MaxLength(2000)]
        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public User User { get; set; }

        public Restaurant Restaurant { get; set; }

        public Reply Reply { get; set; }
    }
}
