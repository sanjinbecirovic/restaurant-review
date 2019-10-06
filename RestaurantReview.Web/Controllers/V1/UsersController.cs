using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;
using RestaurantReview.Web.Extensions;
using RestaurantReview.Web.Infrastructure;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Controllers.V1
{
    public class UsersController : ApiControllerBase
    {
        private readonly UserManager<User> userManager;

        private readonly IUserRepository usersRepository;

        private readonly IMapper mapper;

        public UsersController(UserManager<User> userManager, IUserRepository usersRepository, IMapper mapper)
            : base(mapper)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<UserResponseModel>), 200)]
        public async Task<IActionResult> Get()
        {
            if (!User.IsInRole(UserRoles.Admin))
                return Unauthorized();


            var users = await this.usersRepository.GetAllUsersAsync();
            var models = mapper.Map<IList<UserResponseModel>>(users);

            if (users != null && users.Count() > 0)
            {
                foreach (var user in models)
                    user.Roles = await userManager.GetRolesAsync(new User { Id = user.Id });
            }

            return Ok(models);
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponseModel), 200)]
        public async Task<IActionResult> Me()
        {
            // Check if user exists
            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
                return NotFound();

            var model = mapper.Map<UserResponseModel>(user);
            model.Roles = await userManager.GetRolesAsync(user);

            return Ok(model);
        }
    }
}
