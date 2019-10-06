using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;
using RestaurantReview.Web.Infrastructure;

namespace RestaurantReview.Web
{
    public static class Seed
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                IRestaurantRepository restaurantRepository = scope.ServiceProvider.GetRequiredService<IRestaurantRepository>();
                IReviewRepository reviewRepository = scope.ServiceProvider.GetRequiredService<IReviewRepository>();
                IReplyRepository replyRepository = scope.ServiceProvider.GetRequiredService<IReplyRepository>();

                // Seed roles
                var roles = new[] { UserRoles.Admin, UserRoles.RestaurantOwner };
                foreach (var r in roles)
                {
                    if (!roleManager.RoleExistsAsync(r).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(r)).Wait();
                    }
                }

                // Seed admin user
                if (userManager.FindByNameAsync("admin").Result == null)
                {
                    var admin = new User { UserName = "admin", Email = "admin@localhost.com" };
                    var adminResult = userManager.CreateAsync(admin, "admin").Result;
                    if (!adminResult.Succeeded)
                    {
                        var strErrors = adminResult.Errors.Select(e => e.Description);
                        var error = string.Join("; ", strErrors);
                        throw new Exception(error);
                    }

                    var adminRoleResult = userManager.AddToRoleAsync(admin, UserRoles.Admin).Result;
                    if (!adminRoleResult.Succeeded)
                    {
                        var strErrors = adminRoleResult.Errors.Select(e => e.Description);
                        var error = string.Join("; ", strErrors);
                        throw new Exception(error);
                    }
                }

                User owner = userManager.FindByNameAsync("owner").Result;
                // Seed owner
                if (owner == null)
                {
                    owner = new User { UserName = "owner", Email = "owner@localhost.com" };
                    var ownerResult = userManager.CreateAsync(owner, "owner").Result;
                    if (!ownerResult.Succeeded)
                    {
                        var strErrors = ownerResult.Errors.Select(e => e.Description);
                        var error = string.Join("; ", strErrors);
                        throw new Exception(error);
                    }

                    var ownerRoleResult = userManager.AddToRoleAsync(owner, UserRoles.RestaurantOwner).Result;
                    if (!ownerRoleResult.Succeeded)
                    {
                        var strErrors = ownerRoleResult.Errors.Select(e => e.Description);
                        var error = string.Join("; ", strErrors);
                        throw new Exception(error);
                    }
                }

                User user = userManager.FindByNameAsync("user").Result;
                // Seed user
                if (user == null)
                {
                    user = new User { UserName = "user", Email = "user@localhost.com" };
                    var userResult = userManager.CreateAsync(user, "user1").Result;
                    if (!userResult.Succeeded)
                    {
                        var strErrors = userResult.Errors.Select(e => e.Description);
                        var error = string.Join("; ", strErrors);
                        throw new Exception(error);
                    }
                }

                Restaurant restaurant = null;
                // Seed restaurant
                if (owner != null && !restaurantRepository.GetRestaurantsForUserAsync(owner.Id).Result.Any())
                {
                    restaurant = restaurantRepository.CreateAsync(new Restaurant
                    {
                        UserId = owner.Id,
                        Name = "Owner restaurant",
                        Address = "Restaurant address",
                    }).Result;
                }

                Review review = null;
                // Seed review
                if(user != null && restaurant != null)
                {
                    review = reviewRepository.CreateAsync(
                        new Review
                        {
                            Rating = Rating.Five,
                            RestaurantId = restaurant.Id,
                            Text = "First review",
                            Timestamp = DateTime.Now,
                            UserId = user.Id
                        }).Result;
                }

                Reply reply = null;
                // Seed reply
                if(review != null && owner != null)
                {
                    reply = replyRepository.CreateAsync(
                        new Reply
                        {
                            ReviewId = review.Id,
                            Text = "First reply",
                            TimeStamp = DateTime.Now,
                            UserId = owner.Id
                        }
                    ).Result;
                }

            }
        }
    }
}
