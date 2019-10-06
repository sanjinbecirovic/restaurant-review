using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess
{
    public class RestaurantReviewContext : IdentityDbContext<User>
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public RestaurantReviewContext()
        {
        }

        public RestaurantReviewContext(DbContextOptions<RestaurantReviewContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = optionsBuilder ?? throw new ArgumentNullException(nameof(optionsBuilder));
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=RestaurantReview;Integrated Security=True;MultipleActiveResultSets=True",
                    sqlOptions => sqlOptions.MigrationsHistoryTable("EFMigrationsHistory"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            // Register entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantReviewContext).Assembly);

            modelBuilder.Entity<Reply>()
                .HasOne<Review>(rep => rep.Review)
                .WithOne(rev => rev.Reply)
                .HasForeignKey<Reply>(rep => rep.ReviewId);

            modelBuilder.Entity<Reply>()
                .HasOne<User>(rep => rep.User)
                .WithMany(u => u.Replies)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
