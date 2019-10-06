using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class RestaurantsController : ApiControllerBase
    {
        private readonly UserManager<User> userManager;

        private IRestaurantRepository restaurantRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository, UserManager<User> userManager, IMapper mapper)
            : base(mapper)
        {
            this.restaurantRepository = restaurantRepository;
            this.userManager = userManager;
        }

        // GET: api/restaurant
        [HttpGet]
        [ProducesResponseType(typeof(RestaurantResponseModel), 200)]
        public async Task<IActionResult> Get([FromQuery]int? rating)
        {
            var entities = await this.restaurantRepository.GetRestaurantsAsync();

            if (rating != null)
            {
                entities = entities.Where(e => e.Rating == rating);
            }

            var response = Mapper.Map<IList<RestaurantResponseModel>>(entities);
            return Ok(response);
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(RestaurantResponseModel), 200)]
        public async Task<IActionResult> UserRestaurants()
        {
            var entities = await this.restaurantRepository.GetRestaurantsForUserAsync(User.GetUserId());

            var response = Mapper.Map<IList<RestaurantResponseModel>>(entities);
            return Ok(response);
        }

        [HttpGet("{id:int}/pendingreplycount")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> PendingReplies(int id)
        {
            var entities = await this.restaurantRepository.GetAll()
                .Where(r => r.Id == id)
                .Include(r => r.Reviews)
                .ThenInclude(r => r.Reply)
                .ToListAsync();

            var count = entities.SelectMany(r => r.Reviews).Count(rw => rw.Reply == null);
            return Ok(count);
        }

        // GET: api/restaurant/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RestaurantResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await this.restaurantRepository.GetAll()
                .Where(r => r.Id == id)
                .Include(r => r.User)
                .Include(r => r.Reviews)
                .ThenInclude(r => r.Reply)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync();

            if (entity == null) return NotFound();

            var response = Mapper.Map<RestaurantResponseModel>(entity);

            foreach (var item in response.Reviews)
            {
                var user = await this.userManager.FindByIdAsync(item.UserId);
                item.User = Mapper.Map<UserResponseModel>(user);
            }

            response.Rating = entity.Reviews.Count > 0 ? entity.Reviews.Sum(r => (int)r.Rating) / entity.Reviews.Count : 0;

            return Ok(response);
        }

        // POST: api/restaurant
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantResponseModel), 201)]
        public async Task<IActionResult> Post([FromBody] CreateRestaurantRequestModel model)
        {
            var restaurant = await this.restaurantRepository.GetRestaurantWithName(model.Name);
            if (restaurant != null)
            {
                ModelState.AddModelError(nameof(model.Name), "Restaurant with a given name already exists.");
                return BadRequest(ModelState);
            }

            restaurant = CreateRestaurant(User.GetUserId(), model, null);
            restaurant = await this.restaurantRepository.CreateAsync(restaurant);
            return new CreatedResult("", restaurant);
        }

        // PUT: api/restaurant/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RestaurantResponseModel), 200)]
        public async Task<IActionResult> Put(int id, [FromBody] CreateRestaurantRequestModel model)
        {
            var restaurant = CreateRestaurant(User.GetUserId(), model, id);
            restaurant = await this.restaurantRepository.UpdateAsync(restaurant);
            return Ok(restaurant);
        }

        // DELETE: api/restaurant/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.restaurantRepository.DeleteAsync(id);
            return Ok();
        }

        private Restaurant CreateRestaurant(string userId, CreateRestaurantRequestModel model, int? restaurantId)
        {
            return new Restaurant
            {
                Id = restaurantId ?? 0,
                Name = model.Name,
                Address = model.Address,
                UserId = userId,
                Description = model.Description
            };
        }
    }
}
