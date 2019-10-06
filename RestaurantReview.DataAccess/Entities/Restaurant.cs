using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Entities
{
    public class Restaurant : IEntity
    {
        public Restaurant()
        {
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public int Rating { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<Review> Reviews { get; set;  }
    }
}
