using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;
using RestaurantReview.Web.Extensions;
using RestaurantReview.Web.Models.Request;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Controllers.V1
{
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/restaurant/{restaurantId:int}/[controller]")]
    public class ReviewsController : ApiControllerBase
    {
        private IReviewRepository reviewRepository;

        private readonly UserManager<User> userManager;

        public ReviewsController(IReviewRepository reviewRepository, UserManager<User> userManager, IMapper mapper)
            : base(mapper)
        {
            this.reviewRepository = reviewRepository;
            this.userManager = userManager;
        }

        // GET: api/restaurants/1/reviews
        [HttpGet]
        [ProducesResponseType(typeof(IList<ReviewsResponseModel>), 200)]
        public async Task<IActionResult> Get(int restaurantId, [FromQuery]int? rating, [FromQuery]int? withoutReply)
        {
            var entities = await this.reviewRepository.GetAll()
                .Where(r => r.RestaurantId == restaurantId)
                .Include(r => r.User)
                .Include(r => r.Reply)
                .ThenInclude(r => r.User)
                .OrderByDescending(r => r.Timestamp)
                .Distinct()
                .ToListAsync();

            if (rating != null)
            {
                entities = entities.Where(r => r.RestaurantId == restaurantId && r.Rating == (Rating)rating).OrderByDescending(r => r.Timestamp).ToList();
            }

            if (withoutReply != null && withoutReply == 1)
            {
                entities = entities.Where(r => r.Reply == null).ToList();
            }

            var response = Mapper.Map<IList<ReviewsResponseModel>>(entities);
            return Ok(response);

        }

        [HttpGet("top")]
        [ProducesResponseType(typeof(ReviewsResponseModel), 200)]
        public async Task<IActionResult> GetTopReview(int restaurantId)
        {
            var entities = await this.reviewRepository.GetAll()
                .Where(r => r.RestaurantId == restaurantId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Timestamp)
                .OrderByDescending(r => r.Rating)
                .FirstOrDefaultAsync();

            var response = Mapper.Map<ReviewsResponseModel>(entities);
            return Ok(response);
        }

        [HttpGet("worst")]
        [ProducesResponseType(typeof(ReviewsResponseModel), 200)]
        public async Task<IActionResult> GetWorstReview(int restaurantId)
        {
            var entities = await this.reviewRepository.GetAll()
                .Where(r => r.RestaurantId == restaurantId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Timestamp)
                .OrderBy(r => r.Rating)
                .FirstOrDefaultAsync();

            var response = Mapper.Map<ReviewsResponseModel>(entities);
            return Ok(response);

        }

        // GET: api/restaurants/1/reviews/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReviewsResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int restaurantId, int id)
        {
            var review = await this.reviewRepository.GetByIdAsync(id);

            if (review == null) return NotFound();

            var response = Mapper.Map<ReviewsResponseModel>(review);
            return Ok(response);
        }

        // POST: api/restaurants/1/reviews
        [HttpPost]
        [ProducesResponseType(typeof(ReviewsResponseModel), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(int restaurantId, [FromBody] CreateReviewRequestModel model)
        {
            var review = CreateReview(User.GetUserId(), restaurantId, model, null);
            review = await this.reviewRepository.CreateAsync(review);
            return Created("", review);
        }

        // PUT: api/restaurants/1/reviews/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int restaurantId, int id, [FromBody] CreateReviewRequestModel model)
        {
            var review = CreateReview(User.GetUserId(), restaurantId, model, id);
            await this.reviewRepository.UpdateAsync(review);
            return Ok();
        }

        // DELETE: api/restaurants/1/reviews/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(int restaurantId, int id)
        {
            await this.reviewRepository.DeleteAsync(id);
            return Ok();
        }

        private Review CreateReview(string userId, int restaurantId, CreateReviewRequestModel model, int? reviewId)
        {
            return new Review
            {
                Id = reviewId ?? 0,
                Text = model.Text,
                Rating = model.Rating,
                UserId = userId,
                RestaurantId = restaurantId,
                Timestamp = model.DateOfVisit
            };
        }
    }
}
